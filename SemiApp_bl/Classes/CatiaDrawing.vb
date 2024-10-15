Imports System.Configuration
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Text
Imports System.Text.Json
Imports IscarDal

Public Class CatiaDrawing
    Public Class OkResult
        Public Property bsonPath As String
        Public Property pdfPath As String
    End Class

    Public Class Item
        Public Property Name As String
        Public Property Value As String
    End Class

    Public Shared Function GetCatiaDrawingName(QuotationNum As String) As String

        Try
            If IsNumeric(QuotationNum) Then
                QuotationNum = clUtils.Digits7Number(QuotationNum)
                Dim FileName As String = "DRW_" & QuotationNum & ".pdf"
                'If FileName <> "" Then
                '    Dim s() As String = FileName.Split("\")
                '    Return "drw_" & s(1) & ".pdf"
                'End If
                Return FileName
            End If


        Catch ex As Exception
            Return QuotationNum
            'GeneralException.WriteEvent("BuildandGetDocumentName", ex.Message)
        End Try


    End Function

    Public Shared Function GetCatiaDrawingBSONName(QuotationNum As String) As String

        Try
            If IsNumeric(QuotationNum) Then
                QuotationNum = clUtils.Digits7Number(QuotationNum)
                Dim FileName As String = "DRW_BSON_" & QuotationNum & ".pdf"
                Return FileName
            End If


        Catch ex As Exception
            Return QuotationNum
        End Try


    End Function

    Private Shared Function GetDrawingParamsARRThread(th_sver As String, th_DTparams As DataTable, th_sQutNOAs400 As String, th_BranchCode As String, th_SemiToolDescription As String, th_s_CustomerNumber As String, ByRef sLogWrite As String) As List(Of Item)


        Try

            If Not th_DTparams Is Nothing AndAlso th_DTparams.Rows.Count > 0 Then
                Dim array As New List(Of Item)
                Dim dvDrawing As New DataView
                dvDrawing = th_DTparams.DefaultView
                dvDrawing.RowFilter = "DrawingField<>''"
                dvDrawing.Sort = "DrawingField"
                If Not dvDrawing Is Nothing AndAlso dvDrawing.Count > 0 Then
                    For i As Integer = 0 To dvDrawing.Count - 1
                        Dim sV As String = dvDrawing(i).Item("Measure").ToString
                        Dim DC As Boolean = False
                        Try
                            DC = dvDrawing(i).Item("DrawingConversion")
                        Catch ex As Exception
                            DC = False
                        End Try

                        If Not sV.ToString.ToUpper.Contains("VISIBLE") Then

                            Dim sN As String = dvDrawing(i).Item("DrawingField").ToString
                            sN = sN.Substring(sN.IndexOf("#") + 1, sN.Length - (sN.IndexOf("#") + 1))
                            array.Add(New Item)
                            array.Item(array.Count - 1).Name = sN

                            If IsNumeric(sV) AndAlso th_sver.ToUpper = "I" AndAlso DC = True Then
                                sV = sV * 25.4
                            End If

                            array.Item(array.Count - 1).Value = sV
                            sLogWrite &= "<Param Name=" & sN & ">" & "<Param Value=" & sV & ">" & Environment.NewLine

                        End If
                    Next

                    '-------ManualParam------
                    array.Add(New Item)
                    array.Item(array.Count - 1).Name = "CustomerName"
                    array.Item(array.Count - 1).Value = th_s_CustomerNumber
                    sLogWrite &= "<Param Name=" & array.Item(array.Count - 1).Name & ">" & "<Param Value=" & array.Item(array.Count - 1).Value & ">" & Environment.NewLine

                    array.Add(New Item)
                    array.Item(array.Count - 1).Name = "BRANCH"
                    array.Item(array.Count - 1).Value = "ISCAR"
                    sLogWrite &= "<Param Name=" & array.Item(array.Count - 1).Name & ">" & "<Param Value=" & array.Item(array.Count - 1).Value & ">" & Environment.NewLine

                    array.Add(New Item)
                    array.Item(array.Count - 1).Name = "OfferdBy"
                    array.Item(array.Count - 1).Value = "iQuote"
                    sLogWrite &= "<Param Name=" & array.Item(array.Count - 1).Name & ">" & "<Param Value=" & array.Item(array.Count - 1).Value & ">" & Environment.NewLine

                    array.Add(New Item)
                    array.Item(array.Count - 1).Name = "Design"
                    array.Item(array.Count - 1).Value = "iQuote"
                    sLogWrite &= "<Param Name=" & array.Item(array.Count - 1).Name & ">" & "<Param Value=" & array.Item(array.Count - 1).Value & ">" & Environment.NewLine

                    If IsNumeric(th_sQutNOAs400) Then
                        th_sQutNOAs400 = Format(CInt(th_sQutNOAs400), "0000000")
                    End If

                    Dim drid As String = Get_DrawingNumber(th_BranchCode)

                    array.Add(New Item)
                    array.Item(array.Count - 1).Name = "DrawingNum"
                    array.Item(array.Count - 1).Value = drid
                    sLogWrite &= "<Param Name=" & array.Item(array.Count - 1).Name & ">" & "<Param Value=" & array.Item(array.Count - 1).Value & ">" & Environment.NewLine

                    array.Add(New Item)
                    array.Item(array.Count - 1).Name = "RFQ"
                    array.Item(array.Count - 1).Value = th_sQutNOAs400
                    sLogWrite &= "<Param Name=" & array.Item(array.Count - 1).Name & ">" & "<Param Value=" & array.Item(array.Count - 1).Value & ">" & Environment.NewLine

                    array.Add(New Item)
                    array.Item(array.Count - 1).Name = "DrawingRevision"
                    array.Item(array.Count - 1).Value = 0
                    sLogWrite &= "<Param Name=" & array.Item(array.Count - 1).Name & ">" & "<Param Value=" & array.Item(array.Count - 1).Value & ">" & Environment.NewLine

                    array.Add(New Item)
                    array.Item(array.Count - 1).Name = "ToolDescription"
                    array.Item(array.Count - 1).Value = th_SemiToolDescription
                    sLogWrite &= "<Param Name=" & array.Item(array.Count - 1).Name & ">" & "<Param Value=" & array.Item(array.Count - 1).Value & ">" & Environment.NewLine

                    'Dim iii As Int16 = "435345464574573653567655476334753475"
                    'GeneralException.WriteEventLogReport("GetDrawingParamsARR", sLogWrite)

                    Return array

                End If
            End If


        Catch ex As Exception
            'GeneralException.WriteEventLogReport("GetDrawingParamsARR", ex.Message, GeneralException.e_LogTitle.PDFBSON_3D2D.ToString)

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "BSON_2D Catia drawing", "function : " & ex.Message, th_BranchCode, th_sQutNOAs400, th_sQutNOAs400, "")
            clsMail.SendEmailErrorCatiaDrawing("iQuote Build Drawing Error", "GetDrawingParamsARR<br>" & ex.Message & "<br>", sLogWrite, th_BranchCode, "")
        End Try

    End Function

    Public Shared Function Get_DrawingNumber(ByVal BranchCode As String) As String
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode))

            dt = oSql.ExecuteSPReturnDT("USP_GETDrawingNumber", oParams)
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Return dt.Rows(0)("DRW_NUMBER")
            End If
            Return ""
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Shared Sub CreateCatiaDrawing(th_sver_Rep As String, th_dt_DTparams_Rep As DataTable, th_FolderPath_Rep As String, th_DrawingReports_Rep As String, th_ReturnActiveStorageFolderForDouuments_Rep As String,
                                                    th_ReturnActiveBranchCodeForDocuments_Rep As String, th_sQutNOAs400_Rep As String, th_BranchCode_Rep As String,
                                                    th_SemiToolDescription_Rep As String, th_s_CustomerNumbe_Rep As String, th_newFileName_Rep As String, ByRef result_Rep As String,
                                                ByRef sLogCatiaReportThred As String, th_LoogedEmail_Rep As String, urlServReport As String, ByRef sSuccess As Boolean, UpdateType As String)
        'Dim logStatus_Rep As String = ""
        Dim restService_Rep As String = ""

        Try
            clUtils.SetNewErrorDescription(sLogCatiaReportThred, "Start - CreateCatiaDrawingThread")

            Dim client_Rep As New HttpClient
            client_Rep = HttpClientFactory.Create()
            client_Rep.DefaultRequestHeaders.UserAgent.Clear()
            client_Rep.DefaultRequestHeaders.UserAgent.ParseAdd("Chrome/22.0.1229.94")
            client_Rep.DefaultRequestHeaders.ConnectionClose = False
            client_Rep.Timeout = New TimeSpan(0, 5, 0)

            If Not th_dt_DTparams_Rep Is Nothing AndAlso th_dt_DTparams_Rep.Rows.Count > 0 Then

                Dim dvDrawing_Rep As New DataView
                dvDrawing_Rep = th_dt_DTparams_Rep.DefaultView
                dvDrawing_Rep.RowFilter = "DrawingField<>''"
                dvDrawing_Rep.Sort = "DrawingField"
                If Not dvDrawing_Rep Is Nothing AndAlso dvDrawing_Rep.Count > 0 Then

                    Dim array_rep As New List(Of Item)
                    array_rep = GetDrawingParamsARRThread(th_sver_Rep, th_dt_DTparams_Rep, th_sQutNOAs400_Rep, th_BranchCode_Rep, th_SemiToolDescription_Rep, th_s_CustomerNumbe_Rep, sLogCatiaReportThred)
                    'clUtils.SetNewErrorDescription(sLogCatiaReportThred, "Parameters : " & sLogCatiaReportThred)

                    Dim content_Rep = New StringContent(JsonSerializer.Serialize(array_rep), Encoding.UTF8, "application/json")
                    'Dim myContent_Rep As String = content_Rep.ReadAsStringAsync().Result
                    restService_Rep = urlServReport ' ConfigurationManager.AppSettings("createpdf_API")


                    Try
                        'GeneralException.WriteEventLogReport("urlServReport", restService_Rep, GeneralException.e_LogTitle.PDFBSON_3D2D.ToString)
                        GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT.ToString, "CreateCatiaDrawing - urlServReport", restService_Rep, th_BranchCode_Rep, th_sQutNOAs400_Rep, th_sQutNOAs400_Rep, th_LoogedEmail_Rep)

                        Dim response_Rep = client_Rep.PostAsync(restService_Rep, content_Rep).Result()

                        clUtils.SetNewErrorDescription(response_Rep.StatusCode.ToString & " " & sLogCatiaReportThred, "StatusCode : " & response_Rep.StatusCode.ToString)
                        clUtils.SetNewErrorDescription(sLogCatiaReportThred, "ReasonPhrase : " & response_Rep.ReasonPhrase.ToString)
                        clUtils.SetNewErrorDescription(sLogCatiaReportThred, "th_BranchCode_Rep : " & th_BranchCode_Rep.ToString)
                        clUtils.SetNewErrorDescription(sLogCatiaReportThred, "IsSuccessStatusCode : " & response_Rep.IsSuccessStatusCode.ToString)

                        If response_Rep.IsSuccessStatusCode Then
                            sSuccess = True
                            'GeneralException.WriteEventLogReport("IsSuccessStatusCode", sSuccess, GeneralException.e_LogTitle.PDFBSON_3D2D.ToString)
                            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT.ToString, "CreateCatiaDrawing ", "response result - IsSuccessStatusCode", th_BranchCode_Rep, th_sQutNOAs400_Rep, th_sQutNOAs400_Rep, th_LoogedEmail_Rep)
                            Using responseStreamC = response_Rep.Content.ReadAsStreamAsync().Result

                                result_Rep = JsonSerializer.DeserializeAsync(Of String)(responseStreamC).Result
                                'GeneralException.WriteEventLogReport("result_Rep", result_Rep, GeneralException.e_LogTitle.PDFBSON_3D2D.ToString)
                                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT.ToString, "CreateCatiaDrawing - result_Rep", "responseStreamC = " & result_Rep, th_BranchCode_Rep, th_sQutNOAs400_Rep, th_sQutNOAs400_Rep, th_LoogedEmail_Rep)

                                Dim FolderName_rep As String = System.IO.Path.GetFileName(result_Rep)
                                'GeneralException.WriteEventLogReport("FolderName_rep", FolderName_rep, GeneralException.e_LogTitle.PDFBSON_3D2D.ToString)
                                'GeneralException.LogWriteEventiNew(GeneralException.e_LogTitle.REPORT.ToString, "CreateCatiaDrawing - result_Rep", "responseStreamC = " & result_Rep, th_BranchCode_Rep, th_sQutNOAs400_Rep, "", th_LoogedEmail_Rep)


                                Dim StorePath_rep As String = ConfigurationManager.AppSettings("DrawingReports") & th_BranchCode_Rep & "\"
                                'GeneralException.WriteEventLogReport("StorePath_rep", StorePath_rep, GeneralException.e_LogTitle.PDFBSON_3D2D.ToString)

                                'Dim LocalfolderPath_rep As String = ConfigurationManager.AppSettings("DrawingReports").ToString.Trim & th_BranchCode_Rep & "\"
                                Dim sPathForLog As String = ""

                                Try
                                    sPathForLog = "result_Rep=" & result_Rep & vbNewLine
                                    sPathForLog &= "StorePath_rep & th_newFileName_Rep=" & StorePath_rep & th_newFileName_Rep & vbNewLine
                                    sPathForLog &= "FolderName_rep" & FolderName_rep & vbNewLine
                                    sPathForLog &= "th_ReturnActiveBranchCodeForDocuments_Rep" & th_ReturnActiveBranchCodeForDocuments_Rep & vbNewLine
                                    sPathForLog &= "th_ReturnActiveStorageFolderForDouuments_Rep" & th_ReturnActiveStorageFolderForDouuments_Rep & vbNewLine
                                    sPathForLog &= "th_FolderPath_Rep" & th_FolderPath_Rep & vbNewLine
                                    'GeneralException.WriteEventLogReport("sPathForLog", sPathForLog, GeneralException.e_LogTitle.PDFBSON_3D2D.ToString)


                                Catch ex As Exception

                                End Try
                                'GeneralException.WriteEventLogReport("sPathForLog", sPathForLog, GeneralException.e_LogTitle.PDFBSON_3D2D.ToString)
                                'GeneralException.LogWriteEventiNew(GeneralException.e_LogTitle.REPORT.ToString, sPathForLog, th_BranchCode_Rep, th_sQutNOAs400_Rep, "", th_LoogedEmail_Rep)


                                GetAndStoreFile_CaiaReport(result_Rep, StorePath_rep & th_newFileName_Rep, FolderName_rep, "pdf", th_ReturnActiveBranchCodeForDocuments_Rep, th_newFileName_Rep, th_ReturnActiveStorageFolderForDouuments_Rep, th_FolderPath_Rep, True, UpdateType, th_sQutNOAs400_Rep)

                                result_Rep = th_newFileName_Rep


                                'Try
                                '    Dim sRe As String = ""
                                '    sRe = "result path :" & result_Rep
                                '    sRe &= " - StorePath path :" & StorePath_rep
                                '    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT.ToString, "CreateCatiaDrawing", sRe, th_BranchCode_Rep, th_sQutNOAs400_Rep, th_sQutNOAs400_Rep, "")
                                'Catch ex As Exception
                                'End Try

                                Try

                                Catch ex As Exception

                                End Try
                                clUtils.SetNewErrorDescription(sLogCatiaReportThred, "FolderName : " & FolderName_rep.ToString)
                                clUtils.SetNewErrorDescription(result_Rep, "FolderName : " & result_Rep.ToString)
                                clUtils.SetNewErrorDescription(FolderName_rep, "FolderName : " & FolderName_rep.ToString)
                                clUtils.SetNewErrorDescription(StorePath_rep, "FolderName : " & StorePath_rep.ToString)

                            End Using
                        Else
                            'GeneralException.WriteEventLogReport("IsSuccessStatusCode Fail ", sSuccess, GeneralException.e_LogTitle.PDFBSON_3D2D.ToString)

                            clUtils.SetNewErrorDescription(sLogCatiaReportThred, "IsSuccessStatusCode : " & "Not succedded : (" & response_Rep.IsSuccessStatusCode & ")")
                        End If
                    Catch ex As Exception
                        clUtils.SetNewErrorDescription(sLogCatiaReportThred, "failed to create drawing,<BR>Update parameters : " & ex.Message & "<BR>")
                        clsMail.SendEmailErrorCatiaDrawing("failed to create drawing,<BR>Update parameters", "CreateCatiaDrawingThread<br>" & ex.Message & "<br>", sLogCatiaReportThred, th_BranchCode_Rep, th_LoogedEmail_Rep)
                    End Try

                    array_rep = Nothing
                    content_Rep = Nothing

                End If

                dvDrawing_Rep.RowFilter = ""
                dvDrawing_Rep = Nothing

            End If
            client_Rep = Nothing
        Catch ex As Exception
            sSuccess = False
            'GeneralException.WriteEventLogReport("urlServReport", restService_Rep, GeneralException.e_LogTitle.PDFBSON_3D2D.ToString)
            'GeneralException.WriteEventLogReport("CreateCatiaDrawingThread", sLogCatiaReportThred, GeneralException.e_LogTitle.PDFBSON_3D2D.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "sSuccess = False", "ex = " & ex.Message, th_BranchCode_Rep, th_sQutNOAs400_Rep, th_sQutNOAs400_Rep, th_LoogedEmail_Rep)

            clsMail.SendEmailErrorCatiaDrawing("iQuote Build Drawing Error", "CreateCatiaDrawingThread<br>" & ex.Message & "<br>", sLogCatiaReportThred, th_BranchCode_Rep, th_LoogedEmail_Rep)
            'GeneralException.WriteEventLogReport("CreateCatiaDrawing", ex.Message, GeneralException.e_LogTitle.PDFBSON_3D2D.ToString)
        End Try

    End Sub
    Public Shared Function CreateCatiaBSON(th_sver_BSON As String, th_dt_DTparams_BSON As DataTable, th_FolderPath_BSON As String, th_DrawingReports_BSON As String, th_ReturnActiveStorageFolderForDouuments_BSON As String,
                                                    th_ReturnActiveBranchCodeForDocuments_BSON As String, th_sQutNOAs400_BSON As String, th_BranchCode_BSON As String,
                                                    th_SemiToolDescription_BSON As String, th_s_CustomerNumbe_BSON As String, th_newFileName_BSON As String, ByRef result_BSON As String, ByRef log_RepBson As String, urlServBSON As String, ByRef sSuccessBSON As Boolean) As String
        Try
            clUtils.SetNewErrorDescription(log_RepBson, "Start - CreateCatiaBSONThread")
            clUtils.SetNewErrorDescription(log_RepBson, "Start HttpClient settings")

            Dim clientN As New HttpClient
            clientN = HttpClientFactory.Create()
            clientN.DefaultRequestHeaders.UserAgent.Clear()
            clientN.DefaultRequestHeaders.UserAgent.ParseAdd("Chrome/22.0.1229.94")
            clientN.DefaultRequestHeaders.ConnectionClose = False
            clientN.Timeout = New TimeSpan(0, 5, 0) 'TimeSpan.FromMinutes(10)

            If Not th_dt_DTparams_BSON Is Nothing AndAlso th_dt_DTparams_BSON.Rows.Count > 0 Then

                Dim dvDrawingN As New DataView
                dvDrawingN = th_dt_DTparams_BSON.DefaultView
                dvDrawingN.RowFilter = "DrawingField<>''"
                dvDrawingN.Sort = "DrawingField"

                If Not dvDrawingN Is Nothing AndAlso dvDrawingN.Count > 0 Then

                    Dim arrayN As New List(Of Item)
                    arrayN = GetDrawingParamsARRThread(th_sver_BSON, th_dt_DTparams_BSON, th_sQutNOAs400_BSON, th_BranchCode_BSON, th_SemiToolDescription_BSON, th_s_CustomerNumbe_BSON, log_RepBson)
                    Dim contentDN = New StringContent(JsonSerializer.Serialize(arrayN), Encoding.UTF8, "application/json")
                    Dim restServiceN As String = urlServBSON ' ConfigurationManager.AppSettings("createbson_API")
                    Try
                        'GeneralException.WriteEventLogReport("BSON urlServBSON FileLocation", urlServBSON, GeneralException.e_LogTitle.PDFBSON_3D2D.ToString)
                        GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT.ToString, "BSON urlServBSON FileLocation", urlServBSON, th_BranchCode_BSON, th_sQutNOAs400_BSON, th_sQutNOAs400_BSON, "")


                        Dim responseN = clientN.PostAsync(restServiceN, contentDN).Result
                        clUtils.SetNewErrorDescription(log_RepBson, "restServiceN : " & restServiceN.ToString)
                        clUtils.SetNewErrorDescription(log_RepBson, "StatusCode : " & responseN.StatusCode.ToString)
                        clUtils.SetNewErrorDescription(log_RepBson, "ReasonPhrase : " & responseN.ReasonPhrase.ToString)
                        clUtils.SetNewErrorDescription(log_RepBson, "th_BranchCode_Rep : " & th_BranchCode_BSON.ToString)
                        clUtils.SetNewErrorDescription(log_RepBson, "IsSuccessStatusCode : " & responseN.IsSuccessStatusCode.ToString)

                        Dim BSONFolderQuotationFiles As String = ConfigurationManager.AppSettings("BSONReports")
                        clUtils.SetNewErrorDescription(log_RepBson, "BSONFolder : " & BSONFolderQuotationFiles.ToString)

                        If responseN.IsSuccessStatusCode Then
                            sSuccessBSON = True
                            Try

                                Using responseStream = responseN.Content.ReadAsStreamAsync().Result
                                    Dim resultN As String = JsonSerializer.DeserializeAsync(Of String)(responseStream).Result.ToString
                                    'Dim resultNS As String = JsonSerializer.DeserializeAsync(Of String)(responseStream).Result.ToString
                                    'Dim resultNS As String = JsonSerializer.DeserializeAsync(Of String)(responseStream).Result.ToString
                                    'resultNS = resultNS.Replace(bsonFolderNameN, "")

                                    clUtils.SetNewErrorDescription(log_RepBson, "resultN : " & resultN.ToString)
                                    Dim bsonFolderNameN As String = System.IO.Path.GetFileName(resultN)
                                    clUtils.SetNewErrorDescription(log_RepBson, "bsonFolderNameN : " & bsonFolderNameN.ToString)
                                    Dim StorePathN As String = BSONFolderQuotationFiles & bsonFolderNameN & "\"
                                    clUtils.SetNewErrorDescription(log_RepBson, "StorePathN : " & StorePathN.ToString)
                                    Dim StorePathSubN As String = BSONFolderQuotationFiles & bsonFolderNameN & "\r\0\"
                                    clUtils.SetNewErrorDescription(log_RepBson, "StorePathSubN : " & StorePathSubN.ToString)

                                    'Dim wc As WebClient = New WebClient()
                                    'urlServBSON.Substring(0, urlServBSON.IndexOf("ps07/") + 5)
                                    'Dim surlServBSON As String = ""

                                    Try
                                        ' surlServBSON = "urlServBSON 1 =" & urlServBSON.Substring(0, urlServBSON.IndexOf(".com/") + 5) & ConfigurationManager.AppSettings("CatiaDrawingAPIpath") & bsonFolderNameN & "/CoordinateSystems.json"
                                        'urlServBSON &= "urlServBSON 2 = " & StorePathN & "CoordinateSystems.json"
                                    Catch ex As Exception

                                    End Try
                                    ' GeneralException.WriteEventLogReport("surlServBSON", surlServBSON)
                                    'GeneralException.WriteEventLogReport("resultN - CoordinateSystems", resultN & "/CoordinateSystems.json", GeneralException.e_LogTitle.PDFBSON_3D2D.ToString)
                                    'GeneralException.WriteEventLogReport("resultN - model", resultN & "/model.bson", GeneralException.e_LogTitle.PDFBSON_3D2D.ToString)

                                    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT.ToString, "resultN - CoordinateSystems", resultN & "/CoordinateSystems.json", th_BranchCode_BSON, th_sQutNOAs400_BSON, th_sQutNOAs400_BSON, "")
                                    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT.ToString, "resultN - model", resultN & "/model.bson", th_BranchCode_BSON, th_sQutNOAs400_BSON, th_sQutNOAs400_BSON, "")

                                    'urlServBSON
                                    ''GetAndStoreFile(resultN.Substring(0, resultN.IndexOf(".com/") + 5) & ConfigurationManager.AppSettings("CatiaDrawingAPIpath") & bsonFolderNameN & "/CoordinateSystems.json", StorePathN & "CoordinateSystems.json", StorePathN, "BSON", "", "", "", "", False)
                                    ''GetAndStoreFile(resultN.Substring(0, resultN.IndexOf(".com/") + 5) & ConfigurationManager.AppSettings("CatiaDrawingAPIpath") & bsonFolderNameN & "/model.bson", StorePathN & "model.bson", StorePathN, "BSON", "", "", "", "", False)
                                    ''GetAndStoreFile(resultN.Substring(0, resultN.IndexOf(".com/") + 5) & ConfigurationManager.AppSettings("CatiaDrawingAPIpath") & bsonFolderNameN & "/r/0/lod_0.bin", StorePathN & "r\0\lod_0.bin", StorePathSubN, "BSON", "", "", "", "", False)


                                    GetAndStoreFile(resultN & "/CoordinateSystems.json", StorePathN & "CoordinateSystems.json", StorePathN, "BSON", th_BranchCode_BSON, "", "", "", False, "BSON", th_sQutNOAs400_BSON)
                                    GetAndStoreFile(resultN & "/model.bson", StorePathN & "model.bson", StorePathN, "BSON", th_BranchCode_BSON, "", "", "", False, "BSON", th_sQutNOAs400_BSON)
                                    GetAndStoreFile(resultN & "/r/0/lod_0.bin", StorePathN & "r\0\lod_0.bin", StorePathSubN, "BSON", th_BranchCode_BSON, "", "", "", False, "BSON", th_sQutNOAs400_BSON)
                                    'Try
                                    '    GetAndStoreFileIndexViewer(resultN & "viewer_index.html", BSONFolderQuotationFiles & bsonFolderNameN)
                                    'Catch ex As Exception
                                    'End Try

                                    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT.ToString & "d", "viewer_index.html", BSONFolderQuotationFiles & "viewer_index.html", th_BranchCode_BSON, th_sQutNOAs400_BSON, th_sQutNOAs400_BSON, "")
                                    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT.ToString & "d", "StorePathN", StorePathN, th_BranchCode_BSON, th_sQutNOAs400_BSON, th_sQutNOAs400_BSON, "")



                                    '''Dim BSONFolderQuotationFiles As String = ConfigurationManager.AppSettings("BSONReports")
                                    '''Dim StorePathN As String = BSONFolderQuotationFiles & "c74fb319-dbe6-4314-8a8e-1a4e761043a9.bson" & "\"

                                    ''If File.Exists(ConfigurationManager.AppSettings("BSONReports").ToString & "viewer_indexC.html") Then
                                    ''    Try
                                    ''        'File.Delete(ConfigurationManager.AppSettings("BSONReports").ToString & "viewer_index.html")
                                    ''        GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT.ToString & "d", "viewer_indexChtml", BSONFolderQuotationFiles & "viewer_index.html", th_BranchCode_BSON, th_sQutNOAs400_BSON, th_sQutNOAs400_BSON, "")

                                    ''        CatiaDrawing.GetAndStoreFileIndexViewer(BSONFolderQuotationFiles & "viewer_indexC.html", StorePathN, bsonFolderNameN)

                                    ''        'GetAndStoreFileIndexViewer(BSONFolderQuotationFiles & "viewer_index.html", StorePathN)


                                    ''    Catch ex As Exception
                                    ''        GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT.ToString & "d", "viewer_indexC.html er", ex.Message, th_BranchCode_BSON, th_sQutNOAs400_BSON, th_sQutNOAs400_BSON, "")

                                    ''    End Try
                                    ''End If

                                    result_BSON = resultN
                                    clUtils.SetNewErrorDescription(log_RepBson, "result_BSON : " & result_BSON.ToString)

                                    If th_sQutNOAs400_BSON <> "" AndAlso IsNumeric(th_sQutNOAs400_BSON) Then
                                        clsUpdateData.UpdateQuotationBSONThread(th_sQutNOAs400_BSON, th_BranchCode_BSON, bsonFolderNameN)
                                    End If

                                    'Try

                                    '    Dim sRe As String = ""
                                    '    sRe = "result path :" & resultN & "/CoordinateSystems.json"
                                    '    sRe &= " - StorePath path :" & StorePathN & "CoordinateSystems.json"
                                    '    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT.ToString, "CreateCatiaBSON", sRe, th_BranchCode_BSON, th_sQutNOAs400_BSON, th_sQutNOAs400_BSON, "")
                                    'Catch ex As Exception
                                    'End Try
                                End Using
                            Catch ex As Exception
                                'GeneralException.WriteEventLogReport("CreateCatiaBSON error", ex.Message.ToString, GeneralException.e_LogTitle.PDFBSON_3D2D.ToString)

                                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "resultN - model", "CreateCatiaBSON error", th_BranchCode_BSON, th_sQutNOAs400_BSON, th_sQutNOAs400_BSON, "")
                                clUtils.SetNewErrorDescription(log_RepBson, "ReadAsStreamAsync Error : " & ex.Message)
                            End Try
                        Else
                            clUtils.SetNewErrorDescription(log_RepBson, "responseN.IsSuccessStatusCode : " & responseN.IsSuccessStatusCode.ToString)
                            clsUpdateData.UpdateQuotationBSONThread(th_sQutNOAs400_BSON, th_BranchCode_BSON, "ERROR")
                            'GeneralException.WriteEventLogReport("CreateCatiaDrawingThread", "iQuote Build BSON Error", GeneralException.e_LogTitle.PDFBSON_3D2D.ToString)
                            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "CreateCatiaDrawingThread", "iQuote Build BSON Error", th_BranchCode_BSON, th_sQutNOAs400_BSON, th_sQutNOAs400_BSON, "")
                            clsMail.SendEmailErrorCatiaDrawing("iQuote Build Drawing BSON Error", "CreateCatiaBSONThread BSON<br><br>", th_sQutNOAs400_BSON, th_BranchCode_BSON, "")
                        End If
                    Catch ex As Exception
                        'GeneralException.WriteEventLogReport("CreateCatiaDrawingThread BSON", log_RepBson, GeneralException.e_LogTitle.PDFBSON_3D2D.ToString)
                        GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "iQuote Build Drawing BSON Error", log_RepBson & " ----ex = " & ex.Message, th_BranchCode_BSON, th_sQutNOAs400_BSON, th_sQutNOAs400_BSON, "")
                        clsMail.SendEmailErrorCatiaDrawing("iQuote Build Drawing BSON Error", "CreateCatiaBSONThread BSON<br>" & ex.Message & "<br>", th_sQutNOAs400_BSON, th_BranchCode_BSON, "")
                        'GeneralException.WriteEventLogReport("CreateCatiaDrawing", ex.Message, GeneralException.e_LogTitle.PDFBSON_3D2D.ToString)
                    End Try
                Else
                    clsUpdateData.UpdateQuotationBSONThread(th_sQutNOAs400_BSON, th_BranchCode_BSON, "ERROR")
                    log_RepBson = log_RepBson & "Buil_BSON :  " & "Empty Drawind Fields"
                    'GeneralException.WriteEventLogBSONThread("iQuote Build BSON Error", "CreateCatiaBSONThread", log_RepBson, th_sQutNOAs400_BSON, Now.ToLongDateString, th_BranchCode_BSON, "", "")
                    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "iQuote Build BSON Error", log_RepBson, th_BranchCode_BSON, th_sQutNOAs400_BSON, th_sQutNOAs400_BSON, "")
                End If

                dvDrawingN.RowFilter = ""
                dvDrawingN = Nothing
                clientN = Nothing
            End If
        Catch ex As Exception
            sSuccessBSON = False
            clsUpdateData.UpdateQuotationBSONThread(th_sQutNOAs400_BSON, th_BranchCode_BSON, "ERROR")
            'GeneralException.WriteEventLogBSONThread("iQuote Build BSON Error", "CreateCatiaBSONThread", log_RepBson, th_sQutNOAs400_BSON, Now.ToLongDateString, th_BranchCode_BSON, "", "")

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "iQuote Build Drawing BSON Error", log_RepBson & " ----ex = " & ex.Message, th_BranchCode_BSON, th_sQutNOAs400_BSON, th_sQutNOAs400_BSON, "")

            clsMail.SendEmailErrorCatiaDrawing("iQuote Build BSON Error", "CreateCatiaBSONThread<br>" & ex.Message & "<br>", log_RepBson, th_BranchCode_BSON, "")
        End Try

    End Function

    Public Shared Sub GetAndStoreActiveReportFile(ByVal bFile As Byte(), ByVal sLocalFile As String, sFilaName As String)
        Try

            Dim fileStream As FileStream = New FileStream(sLocalFile & sFilaName, FileMode.Create, FileAccess.ReadWrite)
            fileStream.Write(bFile, 0, bFile.Length)
            fileStream.Close()


        Catch ex As Exception
            'GeneralException.WriteEventErrors(ex.Message, GeneralException.e_LogTitle.PDFBSON_3D2D)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "GetAndStoreActiveReportFile", " ex = " & ex.Message, "", "", "", "")

        End Try
    End Sub
    Public Shared Sub GetAndStoreFileIndexViewer(ByVal sUrl As String, FolderPath As String, bsonFolderNameN As String)
        Try
            If FolderPath <> "" Then
                ' Ensure FolderPath ends with a directory separator
                If Not FolderPath.EndsWith(Path.DirectorySeparatorChar.ToString()) Then
                    FolderPath &= Path.DirectorySeparatorChar
                End If

                ' Define the source and target file names
                Dim sourceFileName As String = "viewer_indexC.html"
                Dim targetFileName As String = "viewer_index.html"

                ' Combine FolderPath and file names to create the full file paths
                Dim sourceFilePath As String = Path.Combine(FolderPath, sourceFileName)
                Dim targetFilePath As String = Path.Combine(FolderPath, targetFileName)

                ' Create the directory if it does not exist
                If Not Directory.Exists(FolderPath) Then
                    Directory.CreateDirectory(FolderPath)
                End If

                ' Check if the target file is locked by another process
                If File.Exists(targetFilePath) AndAlso IsFileLocked(New FileInfo(targetFilePath)) Then
                    Throw New IOException($"The file '{targetFilePath}' is being used by another process.")
                End If

                ' Create a WebRequest to get the file
                Dim request As WebRequest = WebRequest.Create(sUrl)
                request.Timeout = 6000000

                Using resp As WebResponse = request.GetResponse()
                    Using dataStream As Stream = resp.GetResponseStream()
                        Using localStream As Stream = File.Create(sourceFilePath)
                            Dim buffer(1023) As Byte
                            Dim bytesRead As Integer

                            While (InlineAssignHelper(bytesRead, dataStream.Read(buffer, 0, buffer.Length))) > 0
                                localStream.Write(buffer, 0, bytesRead)
                            End While

                            localStream.Flush()
                        End Using
                    End Using
                End Using

                ' Modify the specific line in the file
                Dim fileContent As String = File.ReadAllText(sourceFilePath)
                fileContent = fileContent.Replace("let path = params.get('path');", $"let path = '{bsonFolderNameN}';")
                File.WriteAllText(sourceFilePath, fileContent)

                ' Rename the file from sourceFilePath to targetFilePath
                If File.Exists(targetFilePath) Then
                    File.Delete(targetFilePath) ' Delete existing file if it exists
                End If
                File.Move(sourceFilePath, targetFilePath) ' Rename the file
            End If
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString(), "GetAndStoreFileIndexViewer", " ex = " & ex.Message, "", "", "", "")
        End Try
    End Sub

    'Public Shared Sub GetAndStoreFileIndexViewer(ByVal sUrl As String, FolderPath As String)
    '    Try
    '        If FolderPath <> "" Then
    '            ' Ensure FolderPath ends with a directory separator
    '            If Not FolderPath.EndsWith(Path.DirectorySeparatorChar.ToString()) Then
    '                FolderPath &= Path.DirectorySeparatorChar
    '            End If

    '            ' Define the source and target file names
    '            Dim sourceFileName As String = "viewer_indexC.html"
    '            Dim targetFileName As String = "viewer_index.html"

    '            ' Combine FolderPath and file names to create the full file paths
    '            Dim sourceFilePath As String = Path.Combine(FolderPath, sourceFileName)
    '            Dim targetFilePath As String = Path.Combine(FolderPath, targetFileName)

    '            ' Create the directory if it does not exist
    '            If Not Directory.Exists(FolderPath) Then
    '                Directory.CreateDirectory(FolderPath)
    '            End If

    '            ' Check if the target file is locked by another process
    '            If File.Exists(targetFilePath) AndAlso IsFileLocked(New FileInfo(targetFilePath)) Then
    '                Throw New IOException($"The file '{targetFilePath}' is being used by another process.")
    '            End If

    '            ' Create a WebRequest to get the file
    '            Dim request As WebRequest = WebRequest.Create(sUrl)
    '            request.Timeout = 6000000

    '            Using resp As WebResponse = request.GetResponse()
    '                Using dataStream As Stream = resp.GetResponseStream()
    '                    Using localStream As Stream = File.Create(sourceFilePath)
    '                        Dim buffer(1023) As Byte
    '                        Dim bytesRead As Integer

    '                        While (InlineAssignHelper(bytesRead, dataStream.Read(buffer, 0, buffer.Length))) > 0
    '                            localStream.Write(buffer, 0, bytesRead)
    '                        End While

    '                        localStream.Flush()
    '                    End Using
    '                End Using
    '            End Using

    '            ' Rename the file from sourceFilePath to targetFilePath
    '            If File.Exists(targetFilePath) Then
    '                File.Delete(targetFilePath) ' Delete existing file if it exists
    '            End If
    '            File.Move(sourceFilePath, targetFilePath) ' Rename the file
    '        End If
    '    Catch ex As Exception
    '        GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString(), "GetAndStoreFileIndexViewer", " ex = " & ex.Message, "", "", "", "")
    '    End Try
    'End Sub

    'Public Shared Sub GetAndStoreFileIndexViewer(ByVal sUrl As String, FolderPath As String, sLocalFile As String)
    '    Try
    '        If FolderPath <> "" Then
    '            ' Ensure FolderPath ends with a directory separator
    '            If Not FolderPath.EndsWith(Path.DirectorySeparatorChar.ToString()) Then
    '                FolderPath &= Path.DirectorySeparatorChar
    '            End If

    '            ' Combine FolderPath and sLocalFile to create the full file path
    '            Dim fullFilePath As String = Path.Combine(FolderPath, sLocalFile)

    '            ' Create the directory if it does not exist
    '            If Not Directory.Exists(FolderPath) Then
    '                Directory.CreateDirectory(FolderPath)
    '            End If

    '            ' Check if the file is locked by another process
    '            If File.Exists(fullFilePath) AndAlso IsFileLocked(New FileInfo(fullFilePath)) Then
    '                Throw New IOException($"The file '{fullFilePath}' is being used by another process.")
    '            End If

    '            ' Create a WebRequest to get the file
    '            Dim request As WebRequest = WebRequest.Create(sUrl)
    '            request.Timeout = 6000000

    '            Using resp As WebResponse = request.GetResponse()
    '                Using dataStream As Stream = resp.GetResponseStream()
    '                    Using localStream As Stream = File.Create(fullFilePath)
    '                        Dim buffer(1023) As Byte
    '                        Dim bytesRead As Integer

    '                        While (InlineAssignHelper(bytesRead, dataStream.Read(buffer, 0, buffer.Length))) > 0
    '                            localStream.Write(buffer, 0, bytesRead)
    '                        End While

    '                        localStream.Flush()
    '                    End Using
    '                End Using
    '            End Using
    '        End If
    '    Catch ex As Exception
    '        GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString(), "GetAndStoreFileIndexViewer", " ex = " & ex.Message, "", "", "", "")
    '    End Try
    'End Sub

    Private Shared Function IsFileLocked(file As FileInfo) As Boolean
        Try
            Using stream As FileStream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None)
                stream.Close()
            End Using
        Catch ex As IOException
            Return True
        End Try
        Return False
    End Function

    ' Helper function for inline assignment in the While loop
    Public Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
        target = value
        Return value
    End Function
    Private Shared Sub GetAndStoreFile(ByVal sUrl As String, ByVal sLocalFile As String, FolderPath As String, FileType As String, branchcode As String, fileName As String, StorageFolder As String, DocMngFolderPath As String, UploadToDocuments As Boolean, UpdateType As String, QuotationNo As String)
        Try

            Dim request As WebRequest = WebRequest.Create(sUrl)
            request.Timeout = 6000000
            Dim resp As WebResponse = request.GetResponse()
            Dim dataStream As Stream = resp.GetResponseStream()

            Dim localStream As Stream
            If File.Exists(sLocalFile) Then
                File.Delete(sLocalFile)
            End If

            If FileType = "BSON" Then
                If (Not System.IO.Directory.Exists(FolderPath)) Then
                    Try
                        System.IO.Directory.CreateDirectory(FolderPath)
                    Catch ex As Exception
                    End Try
                End If
            End If

            localStream = File.Create(sLocalFile)
            Dim buffer(1023) As Byte
            Dim bytesRead As Integer
            Dim bytesProcessed As Integer = 0

            While True
                '// Read data (up to 1k) from the stream
                bytesRead = dataStream.Read(buffer, 0, buffer.Length)

                '// Write the data to the local file
                localStream.Write(buffer, 0, bytesRead)

                '// Increment total bytes processed
                bytesProcessed += bytesRead
                If bytesRead = 0 Then
                    Exit While
                End If
            End While

            localStream.Flush()
            localStream.Close()

            dataStream.Close()
            resp.Close()

            Dim Filed() As Byte
            Try
                Dim fs As New FileStream(sLocalFile, FileMode.Open, FileAccess.Read)
                ReDim Filed(fs.Length - 1)
                fs.Read(Filed, 0, fs.Length)
                fs.Close()
                fs = Nothing
                clsUpdateData.UpdateQuotationSuccess(UpdateType, True, False, False, QuotationNo, branchcode)

            Catch ex As Exception
                Throw New Exception("Temp Local File load failure, " & ex.Message)
            End Try

            If UploadToDocuments = True Then
                Dim Success As String = ""
                Dim UserAS400 As String = "DBO"
                If ConfigurationManager.AppSettings("DocMngService").ToString.Trim = "CLOUD" Then
                    Success = Documents.DocmngServiceAuthentication("UPLOAD", "", "", branchcode, fileName, "", StorageFolder, "", "", "", "", "", "", DocMngFolderPath, "", UserAS400, Filed, Nothing, Nothing)
                Else
                    Success = Documents.DocmngServiceAuthentication("UPLOAD", "", "", branchcode, fileName, "", StorageFolder, "", "", "", "", "", "", DocMngFolderPath, "", UserAS400, Filed, Nothing, Nothing)
                End If

            End If

        Catch ex As Exception

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "GetAndStoreFile", " ex = " & ex.Message, "", "", "", "")

        End Try
    End Sub

    Private Shared Sub GetAndStoreFile_CaiaReport(ByVal sUrl_rep As String, ByVal sLocalFile_rep As String, FolderPath_rep As String, FileType_rep As String, branchcode_rep As String, fileName_rep As String, StorageFolder_rep As String, DocMngFolderPath_rep As String, UploadToDocuments_rep As Boolean, UpdateType As String, QuotationNo As String)
        Try

            Dim request_rep As WebRequest = WebRequest.Create(sUrl_rep)
            request_rep.Timeout = 6000000
            Dim resp_rep As WebResponse = request_rep.GetResponse()
            Dim dataStream_rep As Stream = resp_rep.GetResponseStream()

            Dim localStream_rep As Stream
            If File.Exists(sLocalFile_rep) Then
                File.Delete(sLocalFile_rep)
            End If

            If FileType_rep = "BSON" Then
                If (Not System.IO.Directory.Exists(FolderPath_rep)) Then
                    Try
                        System.IO.Directory.CreateDirectory(FolderPath_rep)
                    Catch ex As Exception
                    End Try
                End If
            End If

            localStream_rep = File.Create(sLocalFile_rep)
            Dim buffer_rep(1023) As Byte
            Dim bytesRead_rep As Integer
            Dim bytesProcessed_rep As Integer = 0

            While True
                '// Read data (up to 1k) from the stream
                bytesRead_rep = dataStream_rep.Read(buffer_rep, 0, buffer_rep.Length)

                '// Write the data to the local file
                localStream_rep.Write(buffer_rep, 0, bytesRead_rep)

                '// Increment total bytes processed
                bytesProcessed_rep += bytesRead_rep
                If bytesRead_rep = 0 Then
                    Exit While
                End If
            End While

            localStream_rep.Flush()
            localStream_rep.Close()

            'reader.Close()
            dataStream_rep.Close()
            resp_rep.Close()

            Dim Filed_rep() As Byte
            Try
                Dim fs_rep As New FileStream(sLocalFile_rep, FileMode.Open, FileAccess.Read)
                ReDim Filed_rep(fs_rep.Length - 1)
                fs_rep.Read(Filed_rep, 0, fs_rep.Length)
                fs_rep.Close()
                fs_rep = Nothing
            Catch ex As Exception
                Throw New Exception("Temp Local File load failure, " & ex.Message)
            End Try

            If UploadToDocuments_rep = True Then
                Dim Success_rep As String = ""
                Dim UserAS400_rep As String = "DBO"
                If ConfigurationManager.AppSettings("DocMngService").ToString.Trim = "CLOUD" Then
                    Success_rep = Documents.DocmngServiceAuthentication("UPLOAD", "", "", branchcode_rep, fileName_rep, "", StorageFolder_rep, "", "", "", "", "", "", DocMngFolderPath_rep, "", UserAS400_rep, Filed_rep, Nothing, Nothing)
                    If UpdateType = "2D" Then
                        clsUpdateData.UpdateQuotationSuccess(UpdateType, False, True, False, QuotationNo, branchcode_rep)
                    ElseIf UpdateType = "3DPDF" Then
                        clsUpdateData.UpdateQuotationSuccess(UpdateType, False, False, True, QuotationNo, branchcode_rep)
                    End If

                Else
                    Success_rep = Documents.DocmngServiceAuthentication("UPLOAD", "", "", branchcode_rep, fileName_rep, "", StorageFolder_rep, "", "", "", "", "", "", DocMngFolderPath_rep, "", UserAS400_rep, Filed_rep, Nothing, Nothing)
                    If UpdateType = "2D" Then
                        clsUpdateData.UpdateQuotationSuccess(UpdateType, False, True, False, QuotationNo, branchcode_rep)
                    ElseIf UpdateType = "3DPDF" Then
                        clsUpdateData.UpdateQuotationSuccess(UpdateType, False, False, True, QuotationNo, branchcode_rep)
                    End If
                End If

            End If

        Catch ex As Exception
            'GeneralException.WriteEventErrors(ex.Message, GeneralException.e_LogTitle.PDFBSON_3D2D)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "GetAndStoreFile_CaiaReport", " ex = " & ex.Message, "", "", "", "")

        End Try
    End Sub

    Public Shared Function GetFilePath_CatiaDrawing(Qutno As String, BranchCode As String, ByRef sFileName As String, ByRef sFilePath As String) As Boolean
        Try

            'Dim DN As String = ""

            Dim folderPath As String = ConfigurationManager.AppSettings("DrawingReports").ToString.Trim & clsBranch.ReturnActiveBranchCodeForDocuments & "\"
            Dim sDirName As String = ClsDate.GetDateTimeReturnStringFormat(Now, ClsDate.Enum_DateFormatTypes.yyyymmddHHMMSS)
            folderPath &= BranchCode & "_" & Qutno & "_" & sDirName
            Documents.CreateLocalDirectory(folderPath)

            sFilePath = folderPath
            sFileName = BranchCode & "_" & Qutno & "_" & sDirName

            Return True
        Catch ex As Exception
            'GeneralException.WriteEventErrors(ex.Message, GeneralException.e_LogTitle.PDFBSON_3D2D)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "GetFilePath_CatiaDrawing", " ex = " & ex.Message, "", "", "", "")

            sFileName = ""
            sFilePath = ""
            Return False
            'Throw
        End Try
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class


