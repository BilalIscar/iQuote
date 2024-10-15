Imports System.Collections.Specialized

Imports System.Configuration

Imports System.IO
Imports System.Net
Imports System.Net.Http

Imports System.Text
Imports System.Web
Imports Newtonsoft.Json

Public Class GAL

    Public Shared Function CheckIfCanUpdateGAL() As Boolean
        Try

            Dim sErrorWSGAL = "FALSE"
            Try
                sErrorWSGAL = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ErrorInWSGAL, False)
            Catch ex As Exception
                sErrorWSGAL = "FALSE"
            End Try

            If sErrorWSGAL <> "TRUE" Then
                Dim bc As String = clsBranch.ReturnActiveBranchCodeState ' StateManager.GetValue(StateManager.Keys.s_BranchCode, False)
                If bc <> "ZZ" Then
                    Dim dtUpdate As DataTable = SecurityManager.GetMainDataED(bc)
                    If Not dtUpdate Is Nothing AndAlso dtUpdate.Rows.Count > 0 Then
                        Dim sAS400Update As String = dtUpdate.Rows(0).Item("EnableAS400Update").ToString
                        If sAS400Update = "YES" Then
                            Return True
                        ElseIf sAS400Update = "TEST" Then
                            Dim cn As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, False)
                            Dim sTestCustomer As String = dtUpdate.Rows(0).Item("AS400UpdateCustomer").ToString
                            If sTestCustomer = cn Then
                                Return True
                            End If
                        End If
                    End If
                End If
            End If



            Return False
        Catch ex As Exception
            Return False
        End Try


    End Function

    Public Shared Function ChangeUserBranchCodeAndNumberInGAL(sMail As String, BranchCode As String, GalCustomerNumber As String) As Boolean
        Dim logyesno As String = ConfigurationManager.AppSettings("NewIMCDLogin")

        Dim ApUrl As String = ""
        If ConfigurationManager.AppSettings("NewIMCDLogin").ToUpper = "YES" Then
            ApUrl = ConfigurationManager.AppSettings("LoginURLupdateUserFixedNEW")
        Else
            ApUrl = ConfigurationManager.AppSettings("LoginURLupdateUserFixed")
        End If

        Dim siteType As String = ConfigurationManager.AppSettings("siteType")
        Dim appId As String = ConfigurationManager.AppSettings("ApplicationID")


        If logyesno.ToString.ToUpper = "YES" Then
            Dim SwebAPI As String = ""
            Try

                'Dim Status As String = sendAndGetNew(ApUrl, BranchCode, sMail, GalCustomerNumber, appId, siteType)
                Dim Status As String = SetGalData(ApUrl, "CUSTOMEDETAILSRBYMAIL", BranchCode, GalCustomerNumber, ApUrl, sMail)

                If Status.Contains("200") Then
                    Return True
                Else
                    Return False
                End If

            Catch ex As WebException
                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "ChangeUserBranchCodeAndNumberInGAL", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "ChangeUserBranchCodeAndNumberInGAL webAPI", SwebAPI, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            End Try
        Else
            Dim SwebAPI As String = ""
            Try

                Dim userDetails As String = sMail & "/" & BranchCode & "/" & GalCustomerNumber
                Dim webAPI As String = ApUrl & appId & "/" & siteType & "/" & userDetails
                SwebAPI = webAPI
                Dim Status As String = sendAndGet(webAPI)
                If Status.Contains("200\") Then
                    Return True
                Else
                    Return False
                End If

            Catch ex As WebException
                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "ChangeUserBranchCodeAndNumberInGAL", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "ChangeUserBranchCodeAndNumberInGAL webAPI", SwebAPI, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            End Try
        End If


    End Function

    Public Shared Function sendAndGet(url As String) As String
        Try
            Dim response As HttpWebResponse
            Dim readStream As StreamReader
            Dim res As String

            Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)

            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) EastUSiQuote Chrome/118.0.0.0 Safari/537.36"

            request.Timeout = 180000
            request.Credentials = CredentialCache.DefaultCredentials
            response = CType(request.GetResponse(), HttpWebResponse)
            Dim receiveStream As Stream = response.GetResponseStream()
            readStream = New StreamReader(receiveStream, Encoding.UTF8)
            res = readStream.ReadToEnd()
            response.Close()
            readStream.Close()
            '  GeneralException.WriteEvent(res.ToString)
            Return res

        Catch ex As WebException
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "sendAndGet", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "sendAndGet url", url, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

        End Try
    End Function



    Public Shared Function SetGalData(ApUrl As String, DataType As String, BranchCode As String, CustomerNo As String, StringToConnect As String, loggedMail As String) As String

        Dim signature As String = ""
        Dim tTime As String = ""
        Dim response As String = ""

        Try

            If ConfigurationManager.AppSettings("AS400GetData") = "YES" Then


                Dim sErrorWSGAL = "FALSE"
                Try
                    sErrorWSGAL = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ErrorInWSGAL, False)
                Catch ex As Exception
                    sErrorWSGAL = "FALSE"
                End Try

                If sErrorWSGAL <> "TRUE" Then

                    ServicePointManager.SecurityProtocol = CType(768, SecurityProtocolType) Or CType(3072, SecurityProtocolType)
                    Dim query As NameValueCollection = HttpUtility.ParseQueryString(String.Empty)

                    If DataType <> "" Then

                        If DataType = "CUSTOMEDETAILSRBYMAIL" Then
                            Dim client_Rep As New HttpClient()
                            client_Rep = HttpClientFactory.Create()



                            'client_Rep.DefaultRequestHeaders.Add("Content-Type", "application/json")
                            client_Rep.DefaultRequestHeaders.Add("Accept", "application/json")
                            client_Rep.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*")
                            client_Rep.DefaultRequestHeaders.Add("Access-Control-Allow-Headers", "Content-Type")
                            client_Rep.DefaultRequestHeaders.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS, DELETE, PUT")

                            'client_Rep.DefaultRequestHeaders.UserAgent.Clear()
                            'client_Rep.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) ReportViewr-EUR Chrome/118.0.0.0 Safari/537.36")
                            'client_Rep.DefaultRequestHeaders.ConnectionClose = False
                            ''client_Rep.Timeout = New TimeSpan(0, 0, ConfigurationManager.AppSettings("ApiGalTimeOut"))
                            ''tTime = GetTimeZone(BranchCode)
                            ''client_Rep.DefaultRequestHeaders.Add("x-imc-date", tTime)

                            'client_Rep.DefaultRequestHeaders.Add("Access-Control-Allow-Headers", "Content-Type")


                            'client_Rep.DefaultRequestHeaders.Add("Connection", "keep-alive")

                            Dim t As String = IMCID.GetIMCIDToken()
                            Dim tt As String = ""
                            Dim d As DataTable = LocalizationManager.Convert_JsonToDataTable(t)
                            If Not d Is Nothing AndAlso d.Rows.Count > 0 Then
                                tt = d.Rows(0).Item("token").ToString
                            End If
                            client_Rep.DefaultRequestHeaders.Add("Authorization", tt)

                            Dim jsonData As String = JsonConvert.SerializeObject(New With {.email = loggedMail, .galCustomerNumber = CustomerNo, .branchCode = BranchCode})

                            Dim stringContent As New StringContent(jsonData, Encoding.UTF8, "application/json")

                            response = " start GetAsync "
                            Dim response_Rep As Object = Nothing
                            Try
                                response_Rep = client_Rep.PostAsync(ApUrl, stringContent).Result
                                If response_Rep.statuscode = HttpStatusCode.OK Then
                                    Return "200"
                                Else
                                    Return response_Rep.ToString
                                End If

                            Catch exff As Exception
                                response += " response_Rep " & response_Rep.ToString & " Err msg : " & exff.Message
                            End Try
                            Return response
                        End If

                    End If
                End If
            Else
                Return CType(Nothing, String)
            End If
        Catch ex As Exception

            WritoToSql("FCD-API fail ", ApUrl, signature, "", tTime, response.ToString, DataType, "", CustomerNo, "", "", "", "", StringToConnect, " // " & ex.Message & "// HEADERS : ", True)

        End Try

    End Function
    Public Shared Function GetGalData(DataType As String, BranchCode As String, DevV2 As String, CustomerNo As String, sitem As String, svalue As String, CurrensyFrom As String, CurrensyTo As String, StringToConnect As String, loggedMail As String, QuotationNo As String, doCheckError As Boolean) As DataTable

        Dim sGET As String = ""
        Dim signature As String = ""
        Dim authorization As String = ""
        Dim tTime As String = ""
        Dim response As String = ""

        Try
            If BranchCode.ToString Is Nothing OrElse BranchCode.ToString = "" OrElse BranchCode.ToString = "ZZ" Then
                Return CType(Nothing, DataTable)
            End If
            If ConfigurationManager.AppSettings("AS400GetData") = "YES" Then


                Dim sErrorWSGAL = "FALSE"
                Try
                    sErrorWSGAL = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ErrorInWSGAL, False)
                Catch ex As Exception
                    sErrorWSGAL = "FALSE"
                End Try

                If sErrorWSGAL <> "TRUE" Then



                    Dim sUrl As String = GetAPIurl(BranchCode, "", "")

                    ServicePointManager.SecurityProtocol = CType(768, SecurityProtocolType) Or CType(3072, SecurityProtocolType)
                    Dim query As NameValueCollection = HttpUtility.ParseQueryString(String.Empty)

                    If DataType <> "" Then
                        If DataType = "CUSTOMER" Then
                            sGET = sUrl & "customer/" & CustomerNo
                        ElseIf DataType = "PRICE" Then
                            sGET = sUrl & "fcditems/" & sitem & "/customer/" & CustomerNo
                        ElseIf DataType = "CUSTOMERSUB" Then
                            sGET = sUrl & "customer/" & CustomerNo & "/address"
                        ElseIf DataType = "CUSTOMEDETAILSRBYMAIL" Then
                            sGET = sUrl & "contact"

                            Dim param1 As String = loggedMail

                            Dim requestUri As String = $"{sGET}?email={param1}"

                            sGET = requestUri
                            'sGET = sUrl & "contact?email=" & loggedMail
                            'Dim myQueryStringCollection As NameValueCollection = New NameValueCollection()
                            'myQueryStringCollection.Add("email", loggedMail)
                            ''wc.QueryString = myQueryStringCollection
                        ElseIf DataType = "IQUOTE" Then
                            sGET = sUrl & StringToConnect
                        ElseIf DataType = "CURRENCY" Then
                            If IsNumeric(svalue) Then
                                sGET = sUrl & "currency/" & CurrensyFrom & "/rate/" & CurrensyTo & "?value=" & svalue
                            Else
                                sGET = sUrl & "currency/" & CurrensyFrom & "/rate/" & CurrensyTo & "?value=" & 1
                            End If
                        ElseIf DataType = "PRICES" Then
                            sGET = sUrl & "customer/" & CustomerNo & "/quote/" & QuotationNo & "/line/1?_expand=prices"
                        End If
                    End If


                    Dim client_Rep As New HttpClient
                    client_Rep = HttpClientFactory.Create()
                    client_Rep.DefaultRequestHeaders.UserAgent.Clear()
                    ' client_Rep.DefaultRequestHeaders.UserAgent.ParseAdd("Chrome/22.0.1229.94")
                    client_Rep.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) ReportViewr-EUR Chrome/118.0.0.0 Safari/537.36")
                    client_Rep.DefaultRequestHeaders.ConnectionClose = False
                    client_Rep.Timeout = New TimeSpan(0, 0, ConfigurationManager.AppSettings("ApiGalTimeOut"))
                    authorization = "Bearer " & GetToken(BranchCode)
                    client_Rep.DefaultRequestHeaders.Add("Authorization", authorization)
                    tTime = GetTimeZone(BranchCode)
                    client_Rep.DefaultRequestHeaders.Add("x-imc-date", tTime)
                    client_Rep.DefaultRequestHeaders.Add("Accept", "application/json")
                    client_Rep.DefaultRequestHeaders.Add("Connection", "keep-alive")
                    response = " start GetAsync "
                    Dim response_Rep As Object = Nothing
                    Try
                        response_Rep = client_Rep.GetAsync(sGET).Result
                    Catch exff As Exception
                        response += " response_Rep " & response_Rep.ToString & " Err msg : " & exff.Message
                    End Try

                    response += " finish GetAsync "
                    If response_Rep.IsSuccessStatusCode Then
                        response = response_Rep.Content.ReadAsStringAsync().Result
                        'SessionManager.Clear_Sessions_OpenIquote("FALSE")
                        Return GalDataAsTable(response.ToString, DataType)
                    Else
                        WritoToSql("First FCD-API fail befor run one more time", sGET, signature, authorization, tTime, response.ToString, DataType, DevV2, CustomerNo, sitem, svalue, CurrensyFrom, CurrensyTo, StringToConnect, response_Rep.ToString, False)
                        Dim response_RepE = client_Rep.GetAsync(sGET).Result
                        If response_Rep.IsSuccessStatusCode Then
                            response = response_RepE.Content.ReadAsStringAsync().Result
                            'SessionManager.Clear_Sessions_OpenIquote("FALSE")
                            Return GalDataAsTable(response.ToString, DataType)
                        Else
                            WritoToSql("Second FCD-API fail befor run one more time", sGET, signature, authorization, tTime, response_Rep.ToString, DataType, DevV2, CustomerNo, sitem, svalue, CurrensyFrom, CurrensyTo, StringToConnect, response_Rep.ToString, True)
                        End If
                    End If

                    Return CType(Nothing, DataTable)

                End If

                '  End If

            Else
                Return CType(Nothing, DataTable)
            End If
        Catch ex As Exception

            WritoToSql("FCD-API fail ", sGET, signature, authorization, tTime, response.ToString, DataType, DevV2, CustomerNo, sitem, svalue, CurrensyFrom, CurrensyTo, StringToConnect, " // " & ex.Message & "// HEADERS : ", True)

        End Try

    End Function

    Public Shared Function GalDataAsTable(sJSON As String, sType As String) As DataTable
        Try
            'svalue As String
            'json = response

            'If response.Substring(0, 1) <> "[" Then
            '    Json = "[" & Json & "]"
            'End If
            'SessionManager.Clear_Sessions_OpenIquote("FALSE")

            Dim dtJ As DataTable = LocalizationManager.Convert_JsonToDataTable(sJSON)
            If sType.ToString.ToUpper = "PRICES" AndAlso Not dtJ Is Nothing AndAlso dtJ.Rows.Count > 0 Then
                Dim dtJPrices As DataTable = Nothing
                If dtJ.Rows(0).Item("Prices").ToString.Length > 4 Then
                    dtJPrices = LocalizationManager.Convert_JsonToDataTable(dtJ.Rows(0).Item("Prices"))

                End If
                Return dtJPrices
            Else
                Return dtJ

            End If


            'Try
            '    If Not dtJ Is Nothing AndAlso dtJ.Rows.Count = 1 Then
            '        If sJSON.Contains("exchangeRate") AndAlso svalue <> "" AndAlso IsNumeric(svalue) Then
            '            dtJ.Columns.Add("inputValue") : dtJ.Columns.Add("outputValue")
            '            dtJ.Rows(0).Item("inputValue") = svalue : dtJ.Rows(0).Item("outputValue") = svalue * dtJ.Rows(0).Item("exchangeRate")
            '        End If
            '    End If
            'Catch ex As Exception
            'End Try


            ' Return GetDatatableFromJson(Json, svalue, DataType)
        Catch ex As Exception

        End Try
    End Function
    Public Shared Sub WritoToSql(sSe As String, sGET As String, Signature As String, Authorization As String, tTime As String, Json As String, DataType As String,
                                 DevV2 As String, CustomerNo As String, sitem As String, svalue As String, CurrensyFrom As String, CurrensyTo As String, StringToConnect As String, exX As String, sendMAil As Boolean)
        Dim er As String = ""
        Dim erHeaders As String = ""

        Try
            SessionManager.Clear_Sessions_OpenIquote(sSe)

            Try
                erHeaders = sSe & " //// " & sGET
                erHeaders &= " // signature : " & Signature
                erHeaders &= " // authorization : " & Authorization
                erHeaders &= " // Time : " & tTime
                erHeaders &= " // json : " & Json
            Catch exh As Exception

            End Try

            Dim le As String = ""

            Try
                le = clsQuatation.ACTIVE_UseLoggedEmail
            Catch ex As Exception
            End Try
            Dim leeE As String = ""
            Try
                leeE = clsQuatation.ACTIVE_UseLoggedEmail
            Catch exlR As Exception
            End Try
            er &= "AS400 API Error Occurred , therefore logon user received Technical offer.<br>"
            er &= "Quotation Information:<br>"
            er &= "Branch Code: IG<br>"
            er &= "Logged Email: " & leeE.ToString & "<br>"
            er &= "Customer / Error Message:<br>"
            er &= "DataType: " & DataType & " / Dev-V2: " & DevV2 & " / CustomerNo: " & CustomerNo & " / item: " & sitem & " / value: " & svalue & " / CurrensyFrom: " & CurrensyFrom & " / CurrensyTo: " & CurrensyTo & " / StringToConnect: " & StringToConnect & " / Error msg: " & exX

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.AS400_ERROR.ToString, "GetGalData ", erHeaders & " -- " & "Error msg : " & exX, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            If sendMAil = True Then
                clsMail.Send_iQuoteError("iQuote-Error in GetGalData", "<b>Hello iQuote Support (IT / Marketing),</b></br></br>" & er, "", "FCD Fail")
            End If

            'clsMail.SendEmailError("iQuote-Error in AS400 API", er & " / error msg: " & ex.Message)
        Catch exhx As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.AS400_ERROR.ToString, "GetGalData ", "Error msg : " & exhx.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

        End Try
    End Sub
    'Private Shared Function GetDatatableFromJson(sJson As String, sVal As String, DataType As String) As DataTable
    '    Try
    '        Dim json As String = sJson
    '        If sJson.Substring(0, 1) <> "[" Then
    '            json = "[" & json & "]"
    '        End If
    '        If DataType = "PRICES" AndAlso sJson.ToString.ToUpper.Contains("PRICES") Then
    '            json = sJson.Substring(sJson.ToString.ToUpper.IndexOf("PRICES") + 8, json.Length - sJson.ToString.ToUpper.IndexOf("PRICES") - 10)
    '        End If
    '        'Dim json As String = "[" & txtData.Text & "]"
    '        Dim dt As DataTable = JsonConvert.DeserializeObject(Of DataTable)(json)
    '        Try
    '            If Not dt Is Nothing AndAlso dt.Rows.Count = 1 Then
    '                If json.Contains("exchangeRate") AndAlso sVal <> "" AndAlso IsNumeric(sVal) Then
    '                    dt.Columns.Add("inputValue")
    '                    dt.Columns.Add("outputValue")

    '                    dt.Rows(0).Item("inputValue") = sVal
    '                    dt.Rows(0).Item("outputValue") = sVal * dt.Rows(0).Item("exchangeRate")
    '                End If
    '            End If

    '        Catch ex As Exception

    '        End Try

    '        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
    '            Return dt
    '        Else
    '            Return CType(Nothing, DataTable)
    '        End If

    '    Catch ex As Exception
    '        GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.AS400_ERROR.ToString, "GetDatatableFromJson", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

    '        Return CType(Nothing, DataTable)
    '    End Try
    'End Function



    Public Shared Function GalPostData(BranchCode As String, sJSONData As String, CustomerNo As String, QuoteNumber As String, SubmitAction As Boolean) As String

        Dim sRep = ""
        Dim RequestCount As Int16 = 0
        Try

            If SubmitAction Then
                If Not IsNumeric(QuoteNumber.ToString) Then Return "FALSE"
                If CInt(QuoteNumber) < 100 Then Return "FALSE"
            End If

            Dim client_Rep As New HttpClient
            client_Rep = HttpClientFactory.Create()
            client_Rep.DefaultRequestHeaders.UserAgent.Clear()
            client_Rep.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) ReportViewr-EUR Chrome/118.0.0.0 Safari/537.36")
            client_Rep.DefaultRequestHeaders.ConnectionClose = False
            client_Rep.Timeout = New TimeSpan(0, 0, ConfigurationManager.AppSettings("ApiGalTimeOut"))
            client_Rep.DefaultRequestHeaders.Add("Authorization", "Bearer " & GetToken(BranchCode))
            client_Rep.DefaultRequestHeaders.Add("x-imc-date", GetTimeZone(BranchCode))
            client_Rep.DefaultRequestHeaders.Add("Accept", "application/json")
            client_Rep.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) ReportViewr-EUR Chrome/118.0.0.0 Safari/537.36")

            Dim httpContent As New StringContent(sJSONData, Encoding.UTF8, "application/json")

            Dim APIurl As String = GetAPIurl(BranchCode, QuoteNumber, CustomerNo)
            Dim response_Rep = Nothing

            If SubmitAction Then
                ' Dim wc As WebClient = New WebClient()
            Else
                response_Rep = client_Rep.PostAsync(APIurl, httpContent).Result()
            End If

            sRep = response_Rep.ToString

            If response_Rep.IsSuccessStatusCode Then
                Return response_Rep.Content.ReadAsStringAsync().Result
            Else

                Try
                    WritoToSql("First FCD-API fail to GalPostData", "First FCD-API fail to ostDataForHH_D_HP " & APIurl, "", GetToken(BranchCode), "", "", "", "", CustomerNo, "", "", "", "", "", sRep.ToString & " - JSON : " & sJSONData.ToString, False)
                    Dim httpContentE As New StringContent(sJSONData, Encoding.UTF8, "application/json")
                    Dim response_RepE = client_Rep.PostAsync(APIurl, httpContentE).Result()
                    If response_RepE.IsSuccessStatusCode Then
                        Return response_RepE.Content.ReadAsStringAsync().Result
                    Else
                        WritoToSql("Second FCD-API fail to GalPostData", "Second FCD-API fail to GalPostData " & APIurl, "", GetToken(BranchCode), "", "", "", "", CustomerNo, "", "", "", "", "", sRep.ToString & " - JSON : " & sJSONData.ToString, False)
                        Return "FALSE"
                    End If
                Catch ex As Exception
                    WritoToSql("FCD-API fail to GalPostData", "FCD-API fail to GalPostData " & APIurl, "", GetToken(BranchCode), "", "", "", "", CustomerNo, "", "", "", "", "", sRep.ToString & " - JSON : " & sJSONData.ToString, False)
                    Return "FALSE"
                End Try
            End If


        Catch exE As Exception
            WritoToSql("FCD-API fail to GalPostData", "FCD-API fail to GalPostData ", "", GetToken(BranchCode), "", "", "", "", CustomerNo, "", "", "", "", "", sRep.ToString, False)
            Return "FALSE"
        End Try
    End Function








    'Public Shared Function fcdSubmitWebClient(BranchCode As String, CustomerNo As String, AS400Number As String) As String
    '    Dim s As String = ""
    '    Try

    '        If Not AS400Number = "0" AndAlso Not AS400Number Is Nothing AndAlso IsNumeric(AS400Number.ToString) AndAlso IsNumeric(CustomerNo.ToString) Then
    '            Dim qutNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False).ToString
    '            Dim qutNoAs400 As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number, False).ToString
    '            If qutNo <> qutNoAs400 Then
    '                Dim sUrl As String = GetAPIurl(BranchCode, "", "") & "customer/" & CustomerNo & "/quote/" & AS400Number & "?x-csrf-token=fetch"
    '                Dim sToken As String = GetToken(BranchCode)
    '                Dim sTimeZone As String = GetTimeZone(BranchCode)
    '                ServicePointManager.SecurityProtocol = CType(768, SecurityProtocolType) Or CType(3072, SecurityProtocolType)
    '                Dim wc As WebClient = New WebClient()
    '                wc.Headers.Add("Authorization", sToken)
    '                wc.Headers.Add("x-imc-date", sTimeZone)
    '                wc.Headers.Add("Accept", "application/json")
    '                wc.Headers.Add("Content-Type", "application/json; charset=UTF-8")

    '                s = "{action:submit}"
    '                s = s.Replace("action", """action""")
    '                s = s.Replace("submit", """submit""")

    '                Try
    '                    wc.UploadString(sUrl, "PATCH", s)
    '                    Return "DONE"
    '                Catch ex As Exception
    '                    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.AS400_ERROR.ToString, "fcdSubmit", "First fcdSubmit FCD-API fail befor run one more time ## " & "Error msg : " & ex.Message & " ## " & s, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
    '                    Try
    '                        wc.UploadString(sUrl, "PATCH", s)
    '                        Return "DONE"
    '                    Catch exd As Exception
    '                        GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.AS400_ERROR.ToString, "fcdSubmit", "Second fcdSubmit FCD-API fail befor run one more time ## " & "Error msg : " & exd.Message & " ## " & s, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
    '                    End Try
    '                End Try
    '                Return ""
    '            End If
    '        End If


    '    Catch ex As Exception
    '        GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.AS400_ERROR.ToString, "fcdSubmit", "wsClient : " & s & " Error message : " & ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

    '        Return ex.Message
    '    End Try
    'End Function

    Public Shared Function fcdSubmitOrderGAL(BranchCode As String, CustomerNo As String, AS400Number As String, SubmitOrder As String, Quantity As String) As String
        Dim s As String = ""
        Dim responseRetuned As String = ""
        Try
            If Not AS400Number = "0" AndAlso Not AS400Number Is Nothing AndAlso IsNumeric(AS400Number.ToString) AndAlso IsNumeric(CustomerNo.ToString) Then
                Dim qutNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False).ToString
                Dim qutNoAs400 As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number, False).ToString
                If qutNo <> qutNoAs400 Then
                    Dim sUrl As String = GetAPIurl(BranchCode, "", "") & "customer/" & CustomerNo & "/quote/" & AS400Number & "?x-csrf-token=fetch"
                    Dim sToken As String = GetToken(BranchCode)
                    Dim sTimeZone As String = GetTimeZone(BranchCode)
                    ServicePointManager.SecurityProtocol = CType(768, SecurityProtocolType) Or CType(3072, SecurityProtocolType)

                    'Dim jsonData As String = "{ ""action"": ""action"", ""submit"": ""submit"" }"


                    Dim jsonData As String = "{ ""action"": ""submit"" }"
                    If SubmitOrder = "SUBMIT" Then
                        jsonData = "{ ""action"": ""submit"" }"
                    Else
                        jsonData = "{ ""action"": ""order"",""orderqty"": """ & Quantity & """}"
                    End If

                    Dim timeout As TimeSpan = TimeSpan.FromSeconds(ConfigurationManager.AppSettings("ApiGalTimeOut"))

                    Using httpClient As New HttpClient() With {
                        .Timeout = timeout
                            }
                        Dim request As New HttpRequestMessage(New HttpMethod("PATCH"), sUrl)

                        'httpClient = HttpClientFactory.Create()
                        request.Headers.UserAgent.Clear()
                        request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) ReportViewr-EUR Chrome/118.0.0.0 Safari/537.36")
                        request.Headers.ConnectionClose = False
                        'request.Timeout = New TimeSpan(0, 0, 30)
                        request.Headers.Add("Authorization", "Bearer " & sToken)
                        request.Headers.Add("x-imc-date", sTimeZone)
                        request.Headers.Add("Accept", "application/json")
                        request.Headers.Add("Connection", "keep-alive")

                        request.Content = New StringContent(jsonData, Encoding.UTF8, "application/json")
                        Dim response As HttpResponseMessage = httpClient.SendAsync(request).Result
                        If Not response Is Nothing Then
                            responseRetuned = response.ToString
                        End If

                        If response.IsSuccessStatusCode Then
                            Return "DONE"
                        Else
                            Try
                                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.AS400_ERROR.ToString, "fcdSubmit", "First fcdSubmit FCD-API fail befor run one more time ## " & response.ToString & " ## " & s, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
                                Dim responseX As HttpResponseMessage = httpClient.SendAsync(request).Result
                                If responseX.IsSuccessStatusCode Then
                                    Return "DONE"
                                Else
                                    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.AS400_ERROR.ToString, "fcdSubmit", "Second fcdSubmit FCD-API fail befor run one more time ## " & responseX.ToString & " ## " & s, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
                                    Return "Second"
                                End If
                            Catch exX As Exception
                                ' GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.AS400_ERROR.ToString, "fcdSubmit", "fcdSubmit FCD-API fail befor run one more time ## " & "Error msg : " & exX.Message & " ## " & s, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
                                Return exX.Message
                            End Try
                        End If
                    End Using

                    Return responseRetuned
                End If
            End If
        Catch exH As Exception
            ' GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.AS400_ERROR.ToString, "fcdSubmit", "wsClient : " & s & " Error message : " & exH.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
            Return responseRetuned.ToString & " //// Error msg : " & exH.Message
        End Try
    End Function












    Public Shared Function GetBranchTimeZone(branchcode As String) As String
        Try
            Dim dt As DataTable = clsBranch.GetBranchDetails(branchcode)
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Return dt.Rows(0).Item("TimeZone").ToString

            End If

        Catch ex As Exception

        End Try
    End Function
    Public Shared Function GetAPIBranchUrl(branchcode As String, DevV2 As String) As String
        Try
            Dim dt As DataTable = clsBranch.GetBranchDetails(branchcode)
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Select Case dt.Rows(0).Item("BranchUrl")
                    Case "APIBranchUrl_eur"
                        Return "https://fcd-eur.ssl.imc-companies.com/api/XX/".Replace("XX", DevV2) & branchcode & "/"

                    Case "APIBranchUrl_jpn"
                        Return "https://fcd-jpn.ssl.imc-companies.com/api/XX/".Replace("XX", DevV2) & branchcode & "/"

                    Case "APIBranchUrl_kor"
                        Return "https://fcd-kor.ssl.imc-companies.com/api/XX/".Replace("XX", DevV2) & branchcode & "/"

                    Case "APIBranchUrl_ltd"
                        Return "https://fcd-ltd.ssl.imc-companies.com/api/XX/".Replace("XX", DevV2) & branchcode & "/"

                    Case "APIBranchUrl_usa"
                        Return "https://fcd-usa.ssl.imc-companies.com/api/XX/".Replace("XX", DevV2) & branchcode & "/"
                    Case Else
                        Return ""
                End Select

            End If

        Catch ex As Exception
            Return ""
        End Try
    End Function


    Private Shared Function GetAPIurl(BranchCode As String, QuoteNumber As String, CustomerNo As String) As String
        Try
            'ServicePointManager.SecurityProtocol = CType(768, SecurityProtocolType) Or CType(3072, SecurityProtocolType)

            Dim query As NameValueCollection = HttpUtility.ParseQueryString(String.Empty)
            Dim sPost As String = ""

            If QuoteNumber = "" AndAlso CustomerNo = "" Then
                sPost = GetAPIBranchUrl(BranchCode, ConfigurationManager.AppSettings("AS400APPpathForGetData"))
            Else
                If QuoteNumber = "0" Then
                    sPost = GetAPIBranchUrl(BranchCode, ConfigurationManager.AppSettings("AS400APPpathForGetData")) & "customer/" & CustomerNo & "/quote"
                Else
                    sPost = GetAPIBranchUrl(BranchCode, ConfigurationManager.AppSettings("AS400APPpathForGetData")) & "customer/" & CustomerNo & "/quote/" & QuoteNumber & "/line/1"
                End If
            End If

            'sPost &= "?x-csrf-token=fetch"
            'Dim url As UriBuilder = New UriBuilder(sPost)
            'url.Query = query.ToString()
            Return sPost ' url.Uri.AbsoluteUri
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Shared Function GetTimeZone(BranchCode As String) As String
        Try
            Dim sTimeZone As String = GetBranchTimeZone(BranchCode)
            Dim CurrentZone As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(sTimeZone)
            Dim nowZone As DateTime = TimeZoneInfo.ConvertTime(DateTime.Now, CurrentZone)
            Dim tTime As String = nowZone.ToString("r").ToString.Replace(" GMT", "")
            Return tTime
        Catch ex As Exception
            Return ""
        End Try
    End Function



    Private Shared Function GetToken(BranchCode As String) As String
        Try


            ''wc.Headers.Add("Authorization", authorization)
            ''wc.Headers.Add("x-imc-date", tTime)
            ''wc.Headers.Add("Accept", "application/json")
            '''wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded")
            ''wc.Headers.Add("Content-Type", "application/json; charset=UTF-8")
            '''wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8; charset=UTF-8")



            Dim sClientId As String = ""
            Dim sSecret As String = ""
            If BranchCode = "XZ" Then
                sClientId = ConfigurationManager.AppSettings("ClientId_XZ")
                sSecret = ConfigurationManager.AppSettings("SecretId_XZ")
            Else
                sClientId = ConfigurationManager.AppSettings("ClientId_ALL")
                sSecret = ConfigurationManager.AppSettings("SecretId_ALL")
            End If

            Dim signature As String = CryptoManagerTDES.GetSignature(sClientId, sSecret, GetTimeZone(BranchCode))
            Dim authorization As String = String.Format("IMC {0}:{1}", sClientId, signature)
            Return authorization
        Catch ex As Exception
            Return ""
        End Try
    End Function



End Class
