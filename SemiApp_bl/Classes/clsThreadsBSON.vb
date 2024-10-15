Imports System.Configuration
Imports System.IO
Imports System.Threading
Imports System.Web

Public Class clsThreadsBSON


    Private th_sver_BSON As String = ""
    Private th_dt_DTparams_BSON As DataTable = Nothing
    Private th_FolderPath_BSON As String = ""
    Private th_DrawingReports_BSON As String = ""
    Private th_ReturnActiveStorageFolderForDouuments_BSON As String = ""
    Private th_ReturnActiveBranchCodeForDocuments_BSON As String = ""
    Private th_sQutNOAs400_BSON As String = ""
    Private th_BranchCode_BSON As String
    Private th_SemiToolDescription_BSON As String
    Private th_s_CustomerNumber_BSON As String
    Private th_newFileName_BSON As String
    Private th_LoogedEmail_BSON As String
    'Private th_httpAurl As String


    Public Sub DoThreadBSON()
        Try
            If clsQuatation.checkIfCanDoDrawingForActiveModel() = False Then
                Exit Sub
            End If
        Catch ex As Exception

        End Try


        Try
            If ConfigurationManager.AppSettings("BSON_ACTIVE") = "YES" Then
                'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempAllReadyTriedtoBuildBSONAutomaticly, "YESf")
                th_sver_BSON = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.vers, False)
                th_dt_DTparams_BSON = clsQuatation.GetActiveQuotation_DTparams
                th_LoogedEmail_BSON = clsQuatation.ACTIVE_UseLoggedEmail
                th_sQutNOAs400_BSON = clsQuatation.ACTIVE_QuotationNumber
                th_BranchCode_BSON = clsBranch.ReturnActiveBranchCodeForDocuments
                th_SemiToolDescription_BSON = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.SemiToolDescription, False)
                th_s_CustomerNumber_BSON = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
                'th_httpAurl = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Replace("http://iquote.ssl.imc-companies.com/", "https://iquote.ssl.imc-companies.com/")

                Dim file_name As String = th_sQutNOAs400_BSON & "_BSON.txt"
                Dim file_name2 As String = th_sQutNOAs400_BSON & "_BSON_X.txt"

                'Dim s As String = th_httpAurl
                's = s.Substring(0, s.IndexOf("Forms")) & "TempReportsAndBSON/"

                If File.Exists(ConfigurationManager.AppSettings("TempFilesfOLDEERlOCATION").ToString & file_name) Then
                    Try
                        File.Delete(ConfigurationManager.AppSettings("TempFilesfOLDEERlOCATION").ToString & file_name)
                    Catch ex As Exception
                    End Try

                End If

                If File.Exists(ConfigurationManager.AppSettings("TempFilesfOLDEERlOCATION").ToString & file_name2) Then
                    Try
                        File.Delete(ConfigurationManager.AppSettings("TempFilesfOLDEERlOCATION").ToString & file_name2)
                    Catch ex As Exception
                    End Try

                End If



                Dim thread_BSON As Thread = New Thread(AddressOf BuildBSONReportThread)
                Try
                    thread_BSON.IsBackground = True
                    thread_BSON.Start()

                Catch ex As Threading.ThreadAbortException
                    thread_BSON.Abort()
                Catch ex As Exception
                    thread_BSON.Abort()
                End Try


            End If

        Catch ex As Threading.ThreadAbortException
            Dim sMsg As String = "Error : create bson report , Function name DoThreadReport<br>"
            sMsg = ex.Message & "<br>"
            sMsg = sMsg & "Company - " & th_BranchCode_BSON
            sMsg = sMsg & "Date - " & Now.ToLongDateString
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "BSON_3D", ex.Message, th_BranchCode_BSON, th_sQutNOAs400_BSON, th_sQutNOAs400_BSON, th_LoogedEmail_BSON)
            clsMail.SendEmailWithoutAttachment(ConfigurationManager.AppSettings("MailFrom").ToString.Trim, "SemiStandatrd Message", sMsg, True, False, "", False, Nothing, "", "", "")
        Catch ex As Exception
            Dim sMsg As String = "Error : create bson report , Function name DoThreadReport<br>"
            sMsg = ex.Message & "<br>"
            sMsg = sMsg & "Company - " & th_BranchCode_BSON
            sMsg = sMsg & "Date - " & Now.ToLongDateString
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "BSON_3D", ex.Message, th_BranchCode_BSON, th_sQutNOAs400_BSON, th_sQutNOAs400_BSON, th_LoogedEmail_BSON)
            clsMail.SendEmailWithoutAttachment(ConfigurationManager.AppSettings("MailFrom").ToString.Trim, "SemiStandatrd Message", sMsg, True, False, "", False, Nothing, "", "", "")
        End Try

    End Sub

    'Private Sub BuildBSONReportThread40Sec()
    '    Try
    '        Dim sSc As String = ConfigurationManager.AppSettings("TimeToWaitForBuildCATIA")
    '        For TimerThreadSleepBSON As Integer = 0 To CInt(sSc)

    '            Threading.Thread.Sleep(1000)
    '        Next

    '        File.Copy(ConfigurationManager.AppSettings("TempFilesfOLDEERlOCATION") & "BSON.txt", ConfigurationManager.AppSettings("TempFilesfOLDEERlOCATION") & th_sQutNOAs400_BSON & "_BSON_X.txt")

    '    Catch ex As Exception

    '    End Try

    'End Sub





    Private Sub BuildBSONReportThread()
        'GeneralException.WriteEventErrors("BSON 1- " & th_sQutNOAs400_BSON & "-")
        'GeneralException.WriteEventLogReport("BuildBSONReportThread", "BuildBSONReportThread")

        Dim sLogCatiaReportThredBSON As String = ""
        Dim statusBSON As String = ""
        Try

            clUtils.SetNewErrorDescription(sLogCatiaReportThredBSON, "Set BSON Request Service")

            Dim RequestServiceBSON 'As requestqueueWsProd.Service1SoapClient = New requestqueueWsProd.Service1SoapClient()
            Dim EitRequestBSON 'As requestqueueWsProd.EITQRRequest = New requestqueueWsProd.EITQRRequest()
            Dim eitResultBSON 'As requestqueueWsProd.EITResult

            Dim bUserServisCATIA As String = ConfigurationManager.AppSettings("UserCATIA_WSLocaly").ToString.Trim

            If bUserServisCATIA = "LOCAL" Then

                RequestServiceBSON = New requestqueueWs.Service1SoapClient()
                EitRequestBSON = New requestqueueWs.EITQRRequest()
                eitResultBSON = Nothing 'As requestqueueWs.EITResult

            ElseIf bUserServisCATIA = "PROD" Then

                RequestServiceBSON = New requestqueueWsProd.Service1SoapClient()
                EitRequestBSON = New requestqueueWsProd.EITQRRequest()
                eitResultBSON = Nothing 'As requestqueueWsProd.EITResult

            ElseIf bUserServisCATIA = "BALANCER" Then
                Try
                    RequestServiceBSON = New requestqueueWsBalancer.Service1SoapClient()
                    EitRequestBSON = New requestqueueWsBalancer.EITQRRequest()
                    eitResultBSON = Nothing
                Catch ex As Exception
                    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "BSON_3D", ex.Message, th_BranchCode_BSON, th_sQutNOAs400_BSON, th_sQutNOAs400_BSON, th_LoogedEmail_BSON)
                End Try

            End If

            EitRequestBSON.AppName = ConfigurationManager.AppSettings("3DRequestAppName")
            EitRequestBSON.Rt = "00:00:59"
            EitRequestBSON.TotalSizeOfFiles = 0.4
            EitRequestBSON.UserName = IIf(th_LoogedEmail_BSON.ToString <> "", th_LoogedEmail_BSON.ToString, "iQuote3D - unidentified")
            clUtils.SetNewErrorDescription(sLogCatiaReportThredBSON, "AppName : " & ConfigurationManager.AppSettings("3DRequestAppName"))

            Dim sRepSql As String = ""
            'sRepSql = "RequestServiceBSON = " & RequestServiceBSON.ToString
            'sRepSql &= "EitRequestBSON = " & EitRequestBSON.
            'sRepSql &= "eitResultBSON = " & eitResultBSON

            Try
                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT.ToString, "BSON BuildBSONReportThread", DirectCast(RequestServiceBSON, System.ServiceModel.ClientBase(Of SemiApp_bl.requestqueueWs.Service1Soap)).Endpoint.Address.Uri.AbsoluteUri, th_BranchCode_BSON, th_sQutNOAs400_BSON, th_sQutNOAs400_BSON, "")
            Catch ex As Exception
            End Try

            eitResultBSON = RequestServiceBSON.CreateRequest(EitRequestBSON)
                If eitResultBSON Is Nothing Then clUtils.SetNewErrorDescription(sLogCatiaReportThredBSON, "eitResultBSON is nothing, eitResultBSON.Status=" & eitResultBSON.Status.ToString)

            statusBSON = eitResultBSON.Status
            Dim sSc_3D As String = ConfigurationManager.AppSettings("TimeToWaitForBuildCATIA")

            Dim urlServBSON As String
            Dim result_RepBSON As String = ""
            Dim sSuccessBson As Boolean = False

            EitRequestBSON.Id = eitResultBSON.QR_ID

            For TimerThreadSleepBSON As Integer = 0 To CInt(sSc_3D)
                statusBSON = eitResultBSON.Status
                If statusBSON.ToLower = "w" Then
                    Threading.Thread.Sleep(1000)
                    EitRequestBSON.Status = "w"
                    eitResultBSON = RequestServiceBSON.UpdateRequest(EitRequestBSON)
                ElseIf statusBSON.ToLower = "e" Then
                    clsMail.SendEmailErrorCatiaDrawing("Create 2D Failed!", "Create 2D Failed", sLogCatiaReportThredBSON, th_ReturnActiveBranchCodeForDocuments_BSON, th_LoogedEmail_BSON)

                ElseIf statusBSON.ToLower = "p" Then

                    Exit For
                End If
            Next

            If statusBSON.ToLower = "w" Then
                EitRequestBSON.Status = "e"
                EitRequestBSON.ErrorDescription = "Wait for queue for more then " + (sSc_3D - 1) + " secounds"
                RequestServiceBSON.UpdateRequest(EitRequestBSON)
                clsMail.SendEmailErrorCatiaDrawing("Create 2D Failed!", "Create 2D Failed", sLogCatiaReportThredBSON, th_ReturnActiveBranchCodeForDocuments_BSON, th_LoogedEmail_BSON)

            ElseIf statusBSON.ToLower = "p" Then

                urlServBSON = eitResultBSON.ApplicationUrl
                If Not urlServBSON.ToUpper.Contains("ISCAR.COM") Then
                    urlServBSON = urlServBSON.Replace("cps07", "cps07.iscar.com")
                End If

                sSuccessBson = False
                Try
                    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.BSON_3D.ToString, "BSON Creat location", urlServBSON, th_BranchCode_BSON, th_sQutNOAs400_BSON, "", th_LoogedEmail_BSON)
                    CatiaDrawing.CreateCatiaBSON(th_sver_BSON, th_dt_DTparams_BSON, th_FolderPath_BSON, th_DrawingReports_BSON,
                                                                                                         th_ReturnActiveStorageFolderForDouuments_BSON, th_ReturnActiveBranchCodeForDocuments_BSON, th_sQutNOAs400_BSON,
                                                                                                         th_BranchCode_BSON, th_SemiToolDescription_BSON, th_s_CustomerNumber_BSON, th_newFileName_BSON, result_RepBSON, sLogCatiaReportThredBSON, urlServBSON, sSuccessBson)

                    File.Copy(ConfigurationManager.AppSettings("TempFilesfOLDEERlOCATION") & "BSON.txt", ConfigurationManager.AppSettings("TempFilesfOLDEERlOCATION") & th_sQutNOAs400_BSON & "_BSON.txt")

                    clUtils.SetNewErrorDescription(sLogCatiaReportThredBSON, "SuccessBSON - " & sSuccessBson)

                Catch ex As Exception
                    sSuccessBson = False
                End Try


                If sSuccessBson = False Then
                    EitRequestBSON.Status = "e"
                    EitRequestBSON.ErrorDescription = Left(th_LoogedEmail_BSON & " : " & sLogCatiaReportThredBSON, 300).Replace("'", "")
                    RequestServiceBSON.UpdateRequest(EitRequestBSON)
                    clsMail.SendEmailErrorCatiaDrawing("Create 2D Failed!", "Create 2D Failed", sLogCatiaReportThredBSON, th_ReturnActiveBranchCodeForDocuments_BSON, th_LoogedEmail_BSON)
                Else
                    EitRequestBSON.Status = "c"
                    RequestServiceBSON.UpdateRequest(EitRequestBSON)
                End If
            Else
                EitRequestBSON.Status = "e"
                RequestServiceBSON.UpdateRequest(EitRequestBSON)
                clsMail.SendEmailErrorCatiaDrawing("Create 2D Failed!", "Create 2D Failed", sLogCatiaReportThredBSON, th_ReturnActiveBranchCodeForDocuments_BSON, th_LoogedEmail_BSON)

            End If

            RequestServiceBSON = Nothing
            EitRequestBSON = Nothing
            eitResultBSON = Nothing

        Catch ex As Exception
            clUtils.SetNewErrorDescription(sLogCatiaReportThredBSON, "ex 4 Exception" & ex.Message)
        End Try

    End Sub

End Class
