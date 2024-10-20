Imports System.ComponentModel.DataAnnotations
Imports System.IO
Imports System.Net
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography
Imports System.ServiceModel.Security
Imports System.Web.Configuration
Imports System.Web.Script.Serialization
Imports System.Windows.Forms
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports SemiApp_bl
Imports SemiApp_bl.CatiaDrawing

Public Class LogIn
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim erIMCID As String = ""
        Dim _result As String = ""

        Dim _ssBil As String = ""

        'Try
        '    Dim CountCooRequest As String = "0"

        '    If CookiesManager.GetValue(CookiesManager.Keys.RequestCount, False) Is Nothing Then
        '        CountCooRequest = "0"
        '    Else
        '        CountCooRequest = CookiesManager.GetValue(CookiesManager.Keys.RequestCount, False).ToString
        '    End If
        '    CookiesManager.SetValue(CookiesManager.Keys.RequestCount, CountCooRequest, False)

        '    If IsNumeric(CountCooRequest.ToString) Then
        '        CountCooRequest = CInt(CountCooRequest) + 1
        '        CookiesManager.SetValue(CookiesManager.Keys.RequestCount, CountCooRequest, False)
        '    End If
        '    If CInt(CountCooRequest) > 4 Then
        '        _result = "DONTCHECK"
        '    End If
        'Catch ex As Exception

        ''End Try
        'If Not Request.QueryString("result") Is Nothing Then
        '    Try
        '        GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.LOGIN_ERROR.ToString, "Request", Request.QueryString("result").ToString, False)
        '    Catch ex As Exception
        '        GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.LOGIN_ERROR.ToString, "Request ex", ex.Message, False)
        '    End Try
        'Else
        '    GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.LOGIN_ERROR.ToString, "Request ex", "NOTHING", False)
        'End If
        'GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.LOGIN_ERROR.ToString, "Request ex", Request.Url.AbsoluteUri, False)
        Try

            Try
                Dim formStartFB As String = ""

                formStartFB = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FormName, False)
                If formStartFB.ToString.ToUpper.Contains("STARTFB.ASPX") Then
                    Response.Redirect(formStartFB, True)
                End If

            Catch ex As Exception

            End Try


            If ConfigurationManager.AppSettings("DoCheckLogIn") = "FALSE" Then
                _result = "DONTCHECK"
                _ssBil = ""
            Else
                If Request.QueryString("result") IsNot Nothing AndAlso Request.QueryString("result").ToString <> "" Then
                    _result = Request.QueryString("result").ToString.Trim
                End If

                'If Request.QueryString("ssBil") IsNot Nothing AndAlso Request.QueryString("ssBil").ToString.Trim <> "" Then
                '    _ssBil = Request.QueryString("ssBil").ToString.Trim
                '    CookiesManager.Set_Cookie(CookiesManager.Keys.ssBil.ToString, _ssBil, ConfigurationManager.AppSettings("siteType").ToString)
                '    GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.LOGIN, "_ssBil0", Request.QueryString("ssBil"), False)
                'ElseIf _result = "" Then
                '    If Not CookiesManager.Get_Cookie(CookiesManager.Keys.ssBil.ToString) Is Nothing AndAlso CookiesManager.Get_Cookie(CookiesManager.Keys.ssBil.ToString).ToString.Trim <> "" Then
                '        GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.LOGIN, "_ssBil1", CookiesManager.Get_Cookie(CookiesManager.Keys.ssBil.ToString).ToString, False)
                '        _ssBil = CookiesManager.Get_Cookie(CookiesManager.Keys.ssBil.ToString).ToString.Trim
                '        CookiesManager.Set_Cookie(CookiesManager.Keys.ssBil.ToString, _ssBil, ConfigurationManager.AppSettings("siteType").ToString)
                '    Else
                '        GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.LOGIN, "_ssBil1", "NOTHING", False)
                '    End If
                'End If
            End If
            'GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.LOGIN.ToString, "LogIn Page_Load", "CheckCookie - From IMCID To iQuote = result = " & _result, "", "", "", "")


            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_LanguageCaptions, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._tempBackFromImcLOGIN, "DONE")
            Dim sOpenIquote As String = ""
            Try
                sOpenIquote = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ErrorInWSGAL, False)
            Catch ex As Exception
                sOpenIquote = ""
            End Try
            If sOpenIquote <> "TRUE" Then
                SessionManager.Clear_Sessions_OpenIquote("FALSE")
            End If

            SessionManager.SetSessionDetails_Temporarily(clsQuatation.IsTemporary_Quotatiom().ToString)

            Dim BrCbefore As String = ""

            If _result <> "DONTCHECK" Then
                Dim resultisempty As Boolean = True
                Dim dtResFromSesstion As DataTable = Nothing
                If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_UserResult, "") Is Nothing Then
                    dtResFromSesstion = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_UserResult, "")
                    If Not dtResFromSesstion Is Nothing AndAlso dtResFromSesstion.Rows.Count > 0 Then
                        resultisempty = False
                    End If
                End If

                If resultisempty = False Then
                    If dtResFromSesstion.Columns.Count = 1 AndAlso dtResFromSesstion.Rows(0).Item(0).ToString = "0" Then
                        _result = "0"
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ShowQutPrice, "")
                    Else
                        SetDatatWhneTableIsNotEmpty(dtResFromSesstion)
                    End If

                Else
                    If _result <> "" AndAlso _result <> "0" AndAlso _result <> "DONTCHECK" AndAlso _result.ToString.Length > 8 Then
                        'CookiesManager.Set_Cookie(CookiesManager.Keys.ssBil.ToString, GetRequestssBill(), ConfigurationManager.AppSettings("siteType").ToString)

                        SetDatatWhneResultIsNotEmpty(erIMCID, _result, BrCbefore)
                    Else
                        If _result <> "0" Then
                            If _result = "" Then
                                If _ssBil <> "" Then
                                    _result = GetNewResultFromIP(_ssBil)
                                    If Not _result Is Nothing AndAlso _result <> "" AndAlso _result <> "0" AndAlso _result.Length > 8 Then
                                        'Dim _ResultN As DataTable = ConvertJsonToDataTable(_result)
                                        Dim serializer As New JavaScriptSerializer()
                                        Dim data As Dictionary(Of String, String) = serializer.Deserialize(Of Dictionary(Of String, String))(_result)
                                        If data("isValid").ToString.ToUpper = "TRUE" Then
                                            SetDatatWhneResultIsNotEmpty(erIMCID, data("token"), BrCbefore)
                                            'CookiesManager.Set_Cookie(CookiesManager.Keys.ssBil.ToString, data("ssBil"), ConfigurationManager.AppSettings("siteType").ToString)
                                        Else
                                            _result = "0"
                                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ShowQutPrice, "")
                                        End If
                                    Else
                                        _result = "0"
                                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ShowQutPrice, "")
                                    End If

                                Else : CheckCookie()
                                End If
                            Else : CheckCookie()
                            End If
                        Else
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ShowQutPrice, "")
                            Dim dtRes As New DataTable
                            Dim drres As DataRow = dtRes.NewRow
                            dtRes.Rows.Add(drres)
                            dtRes.Columns.Add()
                            dtRes.Rows(0).Item(0) = "0"
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_UserResult, dtRes)
                            dtRes = Nothing
                        End If

                    End If
                End If


            End If

            Dim sFormN As String = ""
            Try
                sFormN = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FormName, False)
            Catch ex As Exception
            End Try

            If sFormN.ToString.ToUpper.Contains("PRIC") Then
                If (_result = "" Or _result = "0") AndAlso _result <> "00" Then
                    sFormN = ""
                End If
            End If

            If sFormN <> "" Then
                Dim dfS As String = sFormN.ToString.ToUpper
                Dim qnn As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
                If dfS.Contains("PRICES") AndAlso BrCbefore = "ZZ" AndAlso _result <> "00" Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempDoConnect, "DOCONNECT_FROMLOGIN")
                ElseIf dfS.Contains("QBUILDER") AndAlso BrCbefore = "ZZ" AndAlso _result <> "00" AndAlso qnn.ToString.Trim <> "" AndAlso IsNumeric(qnn.ToString.Trim) Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempDoConnect, "DOCONNECT_FROMLOGIN")
                End If
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FormName, "")

                Dim msgl As String = ""
                Try
                    Dim ssDispN As String = StateManager.GetValue(StateManager.Keys.s_DisplayName, False).ToString
                    If ssDispN.ToString.Trim = "" Then
                        msgl &= "Display Name : " & StateManager.GetValue(StateManager.Keys.s_Surname, False).ToString & " " & StateManager.GetValue(StateManager.Keys.s_GivenName, False).ToString & " / " & sFormN
                    Else
                        msgl &= "Display Name : " & ssDispN & " / " & sFormN
                    End If
                    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.Entrance.ToString, "Entrance", msgl, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_iQuoteQuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

                Catch ex As Exception
                End Try

                Response.Redirect(sFormN, False)


            Else

                Dim sTimeStamp As String = ""
                Try
                    sTimeStamp = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TimeStamp, False)
                Catch ex As Exception
                    sTimeStamp = ""
                End Try
                If sTimeStamp Is Nothing Then sTimeStamp = ""

                Dim sOpenType As String = ""
                Dim sfamilyID As String = "0"
                Dim itemNo As String = "0"
                Dim svers As String = "M"
                Dim sLanguage As String = "en"
                Dim ModelNoConf As String = ""
                Dim ModelNoModf As String = ""
                Dim sReqApp As String = ""
                Dim RequestApplication As String = ""
                Dim dtTimeStamp As DataTable = Nothing

                If sTimeStamp <> "" Then
                    dtTimeStamp = clsQuatation.GetQuotationDetailsByTimeStamp(sTimeStamp)
                    If Not dtTimeStamp Is Nothing AndAlso dtTimeStamp.Rows.Count > 0 Then

                        itemNo = dtTimeStamp.Rows(0).Item("ItemNumber").ToString
                        If itemNo.ToString.Trim = "" Then
                            itemNo = "0"
                        End If
                        sfamilyID = dtTimeStamp.Rows(0).Item("FamilyID").ToString
                        svers = dtTimeStamp.Rows(0).Item("vesion").ToString
                        sLanguage = dtTimeStamp.Rows(0).Item("Language").ToString
                        ModelNoConf = dtTimeStamp.Rows(0).Item("ModelNum").ToString
                        ModelNoModf = dtTimeStamp.Rows(0).Item("FamilyID").ToString
                        sReqApp = dtTimeStamp.Rows(0).Item("RequestApplication").ToString
                        sOpenType = dtTimeStamp.Rows(0).Item("OpenType").ToString
                        RequestApplication = dtTimeStamp.Rows(0).Item("RequestApplication").ToString
                    Else
                        Response.Redirect("..\forms\Notification.aspx", False)
                    End If

                Else
                    svers = "M"
                    sLanguage = "en"
                    svers = "M"
                    sOpenType = 2
                    sfamilyID = "0"
                End If

                Dim BranchCode As String = StateManager.GetValue(StateManager.Keys.s_BranchCode, True)
                Dim CustomerNo As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
                Dim LoggedEmail As String = StateManager.GetValue(StateManager.Keys.s_loggedEmail, True)


                If sOpenType = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                    start.StartForm("", sOpenType, itemNo, sLanguage, svers, BranchCode, "", 0, CustomerNo, sReqApp, "", False, False)

                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempConfigDefaultShow, "")

                    Dim msgl As String = ""
                    Try
                        Dim ssDispN As String = StateManager.GetValue(StateManager.Keys.s_DisplayName, False).ToString
                        If ssDispN.ToString.Trim = "" Then
                            msgl &= "Display Name : " & StateManager.GetValue(StateManager.Keys.s_Surname, False).ToString & " " & StateManager.GetValue(StateManager.Keys.s_GivenName, False).ToString & " / Configuration"
                        Else
                            msgl &= "Display Name : " & ssDispN & " / Configuration"
                        End If
                        GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.Entrance.ToString, "Entrance", msgl, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_iQuoteQuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

                    Catch ex As Exception
                    End Try
                    Dim uniqueID As String = HttpContext.Current.Request.QueryString("rErepTr")
                    'CookiesManager.SetValue(CookiesManager.Keys.RequestCount, "0", False)
                    '  Response.Redirect("ConfiguratorBuilder.aspx?rErepTr=" & uniqueID & "&iqlang=" & utl.ReturnParamLanguage(Request.QueryString("iqlang"), True) & "&repLang=" & utl.ReturnReportLanguage(Request.QueryString("repLang"), True), False)
                    Server.Transfer("ConfiguratorBuilder.aspx?rErepTr=" & uniqueID & "&iqlang=" & utl.ReturnParamLanguage(Request.QueryString("iqlang"), True) & "&repLang=" & utl.ReturnReportLanguage(Request.QueryString("repLang"), True), True)
                ElseIf sOpenType = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                    Dim msgl As String = ""
                    Try
                        Dim ssDispN As String = StateManager.GetValue(StateManager.Keys.s_DisplayName, False).ToString
                        If ssDispN.ToString.Trim = "" Then
                            msgl &= "Display Name : " & StateManager.GetValue(StateManager.Keys.s_Surname, False).ToString & " " & StateManager.GetValue(StateManager.Keys.s_GivenName, False).ToString & " / Modification"
                        Else
                            msgl &= "Display Name : " & ssDispN & " / Modification"
                        End If
                        GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.Entrance.ToString, "Entrance", msgl, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_iQuoteQuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

                    Catch ex As Exception
                    End Try
                    SessionManager.SetSessionDetails_SEMI_NewQuoatation(sOpenType, ModelNoModf, CustomerNo, ModelNoModf, ModelNoConf, BranchCode, 0, sLanguage, svers, itemNo, LoggedEmail, "", CType(Nothing, DataTable), CType(Nothing, DataTable), sfamilyID, CType(Nothing, DataTable), itemNo)
                    Dim uniqueID As String = HttpContext.Current.Request.QueryString("rErepTr")
                    '  CookiesManager.SetValue(CookiesManager.Keys.RequestCount, "0", False)
                    Response.Redirect("QBuilder.aspx?rErepTr=" & uniqueID & "&iqlang=" & utl.ReturnParamLanguage(Request.QueryString("iqlang"), True) & "&repLang=" & utl.ReturnReportLanguage(Request.QueryString("repLang"), True), False)

                End If

            End If

        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.LOGIN_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message & " - " & erIMCID, True)


            If Not ex.Message.ToString.ToUpper.Contains("THREAD WAS BEING ABORTED") Then
                Dim uniqueID As String = HttpContext.Current.Request("rErepTr")
                '  CookiesManager.SetValue(CookiesManager.Keys.RequestCount, "0", False)
                Server.Transfer("ConfiguratorBuilder.aspx?rErepTr=" & uniqueID & "&", True)
            End If

        End Try



    End Sub

    Private Function GetRequestssBill() As String
        Try
            If Not Request.QueryString("ssBil") Is Nothing AndAlso Request.QueryString("ssBil").ToString <> "" Then
                Return Request.QueryString("ssBil").ToString
            End If
            Return ""
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.LOGIN_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, False)
        End Try
    End Function

    Private Sub SetDatatWhneResultIsNotEmpty(ByRef erIMCID As String, ByRef _result As String, ByRef BrCbefore As String)
        Try
            Dim table As DataTable = CType(Nothing, DataTable)
            Try
                erIMCID &= " ---- _result = GetJasonFromAPI BEGIN  ---- " & _result
                _result = GetJasonFromAPI(_result)
                erIMCID &= " ---- _result = GetJasonFromAPI END "
                table = ConvertJsonToDataTable(_result)
                erIMCID &= " table = DeserializeObject"
            Catch ex As Exception
                Dim er As String = "<b>Hello iQuote Support (IT / Marketing),</b></br></br>"
                Dim leeE As String = clsQuatation.ACTIVE_UseLoggedEmail
                er &= "IMCID Error Occurred,<br>Quotation Information:<br>Branch Code: <br>Logged Email: " & leeE.ToString & "<br>Customer / Error Message:<br>"
                clsMail.Send_iQuoteError("iQuote-Error in IMCID login", er & " ----- " & erIMCID, BrCbefore.ToString, "IMCID Error Occurred")
            End Try

            If Not table Is Nothing AndAlso table.Rows.Count > 0 Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_UserResult, table)
                Try
                    For erd As Integer = 0 To table.Columns.Count - 1
                        erIMCID &= table.Columns(erd).ColumnName.ToString & ":" & table.Rows(0).Item(erd).ToString & " / "
                    Next
                Catch ex As Exception
                End Try

                Dim sGc As String = table.Rows(0).Item("branchcode").ToString.Trim
                Dim sGcForConnectLanguage As String = sGc
                If Not IsNumeric(table.Rows(0).Item("galcustomernumber").ToString.Trim) AndAlso table.Rows(0).Item("galcustomernumber").ToString.Trim <> "0" Then
                    Dim sCn As String = ""
                    Dim sLe As String = table.Rows(0).Item("LoggedEmail").ToString.Trim

                    Dim sGalDet As DataTable = Nothing
                    If sGc <> "" AndAlso sLe <> "" Then
                        sGalDet = GAL.GetGalData("CUSTOMEDETAILSRBYMAIL", sGc, ConfigurationManager.AppSettings("AS400APPpathForGetData"), "", "", "", "", "", "", sLe, "", True)
                    End If
                    Dim bFoundUser As Boolean = False
                    If Not sGalDet Is Nothing AndAlso sGalDet.Rows.Count > 0 Then
                        sCn = sGalDet.Rows(0).Item("customernumber").ToString.Trim
                        If sCn <> "" AndAlso IsNumeric(sCn) AndAlso sCn <> "0" Then
                            bFoundUser = True
                        End If
                    End If

                    If bFoundUser = False Then
                        _result = "00"
                        Dim givenName As String = table.Rows(0).Item("givenName").ToString.Trim.Replace("[]", "")
                        Dim surname As String = table.Rows(0).Item("surname").ToString.Trim.Replace("[]", "")
                        Dim businessPhones As String = table.Rows(0).Item("businessPhones").ToString.Trim.Replace("[]", "")
                        Dim companyName As String = table.Rows(0).Item("companyName").ToString.Trim.Replace("[]", "")
                        ' Dim semail As String = table.Rows(0).Item("email").ToString.Trim
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempShowCustomerAlertLogIN, sLe & "#" & sGc & "#" & givenName & "#" & surname & "#" & businessPhones & "#" & companyName)
                        sGc = "ZZ"
                    Else
                        Try

                            Dim b_b As Boolean = GAL.ChangeUserBranchCodeAndNumberInGAL(sLe, sGc, sCn)
                            'GAL.ChangeUserDataIn_Gal(sLe, sGc, sCn)
                        Catch ex As Exception

                        End Try

                    End If

                End If
                BrCbefore = StateManager.GetValue(StateManager.Keys.s_BranchCode, False).ToString.Trim
                StateManager.SetNewQuotationState(table, sGc, sGcForConnectLanguage)
            End If



        Catch ex As Exception

            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.LOGIN_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, False)

        End Try
    End Sub
    Private Sub SetDatatWhneTableIsNotEmpty(table As DataTable)
        Try

            If Not table Is Nothing AndAlso table.Rows.Count > 0 Then

                Dim sGc As String = table.Rows(0).Item("branchcode").ToString.Trim
                Dim sGcForConnectLanguage As String = sGc
                If Not IsNumeric(table.Rows(0).Item("galcustomernumber").ToString.Trim) AndAlso table.Rows(0).Item("galcustomernumber").ToString.Trim <> "0" Then
                    Dim sCn As String = ""
                    Dim sLe As String = table.Rows(0).Item("LoggedEmail").ToString.Trim

                    Dim sGalDet As DataTable = Nothing
                    If sGc <> "" AndAlso sLe <> "" Then
                        sGalDet = GAL.GetGalData("CUSTOMEDETAILSRBYMAIL", sGc, ConfigurationManager.AppSettings("AS400APPpathForGetData"), "", "", "", "", "", "", sLe, "", True)
                    End If
                    Dim bFoundUser As Boolean = False
                    If Not sGalDet Is Nothing AndAlso sGalDet.Rows.Count > 0 Then
                        sCn = sGalDet.Rows(0).Item("customernumber").ToString.Trim
                        If sCn <> "" AndAlso IsNumeric(sCn) AndAlso sCn <> "0" Then
                            bFoundUser = True
                        End If
                    End If

                    If bFoundUser = False Then
                        Dim givenName As String = table.Rows(0).Item("givenName").ToString.Trim.Replace("[]", "")
                        Dim surname As String = table.Rows(0).Item("surname").ToString.Trim.Replace("[]", "")
                        Dim businessPhones As String = table.Rows(0).Item("businessPhones").ToString.Trim.Replace("[]", "")
                        Dim companyName As String = table.Rows(0).Item("companyName").ToString.Trim.Replace("[]", "")

                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempShowCustomerAlertLogIN, sLe & "#" & sGc & "#" & givenName & "#" & surname & "#" & businessPhones & "#" & companyName)
                        sGc = "ZZ"
                    Else
                        Try
                            Dim b_b As Boolean = GAL.ChangeUserBranchCodeAndNumberInGAL(sLe, sGc, sCn)
                        Catch ex As Exception

                        End Try

                    End If

                End If
                StateManager.SetNewQuotationState(table, sGc, sGcForConnectLanguage)
            End If

        Catch ex As Exception

            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.LOGIN_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, False)

        End Try
    End Sub
    Private Sub CheckCookie()

        If ConfigurationManager.AppSettings("DoCheckLogIn") <> "FALSE" Then

            Try
                Dim mPage As String = ""

                If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FormName, False) Is Nothing AndAlso SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FormName, False).ToString <> "" Then
                    mPage = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FormName, False)
                End If

                Dim siteType As String = ConfigurationManager.AppSettings("siteType")
                Dim appId As String = ConfigurationManager.AppSettings("ApplicationID")
                Dim Lang As String = ConfigurationManager.AppSettings("Lang")
                Dim uniqueID As String = HttpContext.Current.Request("rErepTr")

                Dim t As String = IMCID.GetIMCIDToken()
                Dim tt As String = ""
                Dim d As DataTable = LocalizationManager.Convert_JsonToDataTable(t)
                If Not d Is Nothing AndAlso d.Rows.Count > 0 Then
                    tt = d.Rows(0).Item("token").ToString
                End If
                HttpContext.Current.Response.Redirect(ConfigurationManager.AppSettings("LoginCheckURLNEW") & "?rErepTr=" & uniqueID & "&token=" & tt & "&sitetype=" & siteType & "&lang=" & Lang, True)
            Catch ex As Exception
                GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.LOGIN_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, False)
            End Try
        End If

    End Sub

    Public Sub LogIn(mPage As String)
        If ConfigurationManager.AppSettings("DoCheckLogIn") <> "FALSE" Then
            Try

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_UserResult, CType(Nothing, DataTable))


                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FormName, mPage)


                Dim siteType As String = ConfigurationManager.AppSettings("siteType")
                Dim appId As String = ConfigurationManager.AppSettings("ApplicationID")
                Dim Lang As String = ConfigurationManager.AppSettings("Lang")
                Dim uniqueID As String = HttpContext.Current.Request("rErepTr")

                Dim s As String = ""
                If ConfigurationManager.AppSettings("NewIMCDLogin") = "YES" Then
                    Dim t As String = IMCID.GetIMCIDToken()
                    Dim tt As String = ""
                    Dim d As DataTable = LocalizationManager.Convert_JsonToDataTable(t)
                    If Not d Is Nothing AndAlso d.Rows.Count > 0 Then
                        tt = d.Rows(0).Item("token").ToString
                    End If
                    ' s = ConfigurationManager.AppSettings("LoginURLNEW") & "?rErepTr=" & uniqueID & "&token=" & tt & "&sitetype=" & siteType & "&lang=" & Lang
                    s = ConfigurationManager.AppSettings("LoginURLNEW") & "?rErepTr=" & uniqueID & "&token=" & tt & "&sitetype=" & siteType
                Else
                    s = ConfigurationManager.AppSettings("LoginURL") & "?rErepTr=" & uniqueID & "&Result=0&sId=" & appId & "&sitetype=" & siteType & "&lang=" & Lang

                End If


                HttpContext.Current.Response.Redirect(s, False)

            Catch ex As Exception

                GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.LOGIN_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, False)

            End Try
        End If

    End Sub

    Public Shared Function GetJasonFromAPI(Token As String) As String
        Try
            Dim appILogIPlinkd As String = ""
            If ConfigurationManager.AppSettings("NewIMCDLogin") = "YES" Then
                appILogIPlinkd = ConfigurationManager.AppSettings("LogIPlinkNEW")
            Else
                appILogIPlinkd = ConfigurationManager.AppSettings("LogIPlink")
            End If

            Dim query As NameValueCollection = HttpUtility.ParseQueryString(String.Empty)

            Dim url As UriBuilder = New UriBuilder(appILogIPlinkd)

            url.Query = query.ToString()

            Dim sIP As WebClient = New WebClient()
            sIP.Headers.Add("Accept", "application/json")
            sIP.Headers.Add("Content-Type", "application/json; charset=UTF-8")
            'sIP.Headers.Add("Content-Type", "application/json")
            sIP.Headers.Add("token", "Bearer " & Token)
            ' sIP.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) EastUSiQuote Chrome/118.0.0.0 Safari/537.36")

            Dim response As String = sIP.UploadString(url.ToString, "POST")

            If ConfigurationManager.AppSettings("NewIMCDLogin") = "YES" Then
                If response <> "" AndAlso response.ToString.ToUpper.Contains("LOGGEDEMAIL") Then
                    Return response.ToString.Trim
                Else
                    Return "0"
                End If
            Else
                If response <> "" AndAlso response.ToString.ToUpper.Contains("LOGGEDEMAIL") AndAlso Not response.ToString.ToUpper.Contains("TABLE") AndAlso response.StartsWith("[") Then
                    Return response.ToString.Trim
                Else
                    Return "0"
                End If

            End If

        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.LOGIN_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, False)
            Return "0"
        End Try
    End Function

    Public Shared Function ConvertJsonToDataTable(jsonData As String) As DataTable

        Dim dataTable As New DataTable()
        Try
            If jsonData.IndexOf("[") = 0 Then
                jsonData = jsonData
            Else
                jsonData = "[" & jsonData & "]"
            End If


            ' Parse JSON array
            Dim jsonArray As JArray = JArray.Parse(jsonData)

            ' Extract column names from the first item
            Dim columns As List(Of String) = jsonArray.First().ToObject(Of JObject)().Properties().Select(Function(p) p.Name).ToList()

            ' Add columns to DataTable
            For Each columnName As String In columns
                dataTable.Columns.Add(columnName, GetType(String))
            Next

            ' Populate DataTable with data
            For Each item As JObject In jsonArray
                Dim values As Object() = item.Properties().Select(Function(p) p.Value.ToString()).ToArray()
                dataTable.Rows.Add(values)
            Next

            Return dataTable
        Catch ex As Exception
            dataTable = Nothing
            Return dataTable
        End Try


    End Function
    Public Shared Sub LogINcL()
        If ConfigurationManager.AppSettings("DoCheckLogIn") <> "FALSE" Then
            Try
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._tempBackFromImcLOGIN, "START")

                Dim l As New LogIn
                l.LogIn(HttpContext.Current.Request.UrlReferrer.AbsoluteUri.ToString)
                l.Dispose()
                l = Nothing

            Catch ex As Exception
                GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.LOGIN_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, False)
            End Try
        End If

    End Sub


    Public Shared Function LogOut()
        Try

            Try
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_UserResult, CType(Nothing, DataTable))

                'CookiesManager.ClearValueCookie(CookiesManager.Keys.ssBil.ToString)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationNumber, "")
                SessionManager.Clear_Sessions_OpenIquote("FALSE")
                StateManager.Clear_State(False)

                'If ConfigurationManager.AppSettings("DoCheckCookie") = "LOCAL" Then
                '    'Check cookie user info 1
                '    CookiesManager.CleareCookie_userinfo()
                'End If

                SessionManager.SetSessionDetails_Temporarily("TRUE")

                Dim siteType As String = ConfigurationManager.AppSettings("siteType")
                Dim appId As String = ConfigurationManager.AppSettings("ApplicationID")
                Dim Lang As String = ConfigurationManager.AppSettings("Lang")

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FormName, HttpContext.Current.Request.UrlReferrer.AbsoluteUri.ToString)

                Dim mPage As String = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.ToString.Replace("http://iquote.ssl.imc-companies.com/", "https://iquote.ssl.imc-companies.com/")



                Dim uniqueID As String = HttpContext.Current.Request("rErepTr")

                If ConfigurationManager.AppSettings("NewIMCDLogin") = "YES" Then
                    Dim t As String = IMCID.GetIMCIDToken()
                    Dim tt As String = ""
                    Dim d As DataTable = LocalizationManager.Convert_JsonToDataTable(t)
                    If Not d Is Nothing AndAlso d.Rows.Count > 0 Then
                        tt = d.Rows(0).Item("token").ToString
                    End If
                    HttpContext.Current.Response.Redirect(ConfigurationManager.AppSettings("LoginSignOutURLReqNEW") & "?rErepTr=" & uniqueID & "&token=" & tt & "&sitetype=" & siteType & "&lang=" & Lang, True)
                Else
                    HttpContext.Current.Response.Redirect(ConfigurationManager.AppSettings("LoginSignOutURLReq") & "?rErepTr=" & uniqueID & "&sId=" & appId & "&sitetype=" & siteType & "&lang=" & Lang, True)

                End If
            Catch ex As Exception

                GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.LOGIN_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            End Try

        Catch ex As Exception

        End Try
    End Function

    Private Function GetNewResultFromIP(cookieBill As String)
        Try

            If Not cookieBill Is Nothing AndAlso cookieBill <> "" Then
                Try
                    Dim appILogIPlinkd As String = ConfigurationManager.AppSettings("LogGetCookieBil")

                    Dim query As NameValueCollection = HttpUtility.ParseQueryString(String.Empty)

                    Dim url As UriBuilder = New UriBuilder(appILogIPlinkd)

                    Dim token = IMCID.GetIMCIDToken
                    Dim serializer As New JavaScriptSerializer()
                    Dim data As Dictionary(Of String, String) = serializer.Deserialize(Of Dictionary(Of String, String))(token)
                    Dim tokenValue As String = data("token")

                    url.Query = query.ToString()

                    Dim sIP As WebClient = New WebClient()
                    sIP.Headers.Add("Accept", "application/json")
                    sIP.Headers.Add("Content-Type", "application/json; charset=UTF-8")
                    sIP.Headers.Add("token", tokenValue)
                    sIP.Headers.Add("ssBil", cookieBill)
                    Dim response As String = sIP.UploadString(url.ToString, "POST")

                    If response <> "" Then
                        Return response.ToString.Trim
                    End If

                Catch ex As Exception
                    GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.LOGIN_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, False)
                    Return ""
                End Try
            End If
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.LOGIN_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, False)
        End Try
        Return ""
    End Function

End Class