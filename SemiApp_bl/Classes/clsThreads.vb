Imports System.Configuration
Imports System.Threading
Imports IscarDal
Public Class clsThreads


    Private th_sver_Rep As String = ""
    Private th_dt_DTparams_Rep As DataTable = Nothing
    Private th_FolderPath_Rep As String = ""
    Private th_DrawingReports_Rep As String = ""
    Private th_ReturnActiveStorageFolderForDouuments_Rep As String = ""
    Private th_ReturnActiveBranchCodeForDocuments_Rep As String = ""
    Private th_sQutNOAs400_Rep As String = ""
    Private th_BranchCode_Rep As String
    Private th_SemiToolDescription_Rep As String
    Private th_s_CustomerNumber_Rep As String
    Private th_newFileName_Rep As String
    Private th_newFileNameBSON_Rep As String
    Private th_LoogedEmail_Rep As String

    Public Sub DoThreadReport(bDelete As Boolean)
        Try
            If clsQuatation.checkIfCanDoDrawingForActiveModel() = False Then
                Exit Sub
            End If
        Catch ex As Exception

        End Try
        If ConfigurationManager.AppSettings("CATIA_ACTIVE") = "YES" Then

            Try

                If bDelete = True Then
                    Documents.DeleteCatiaReportsLocaly()
                    Documents.DeleteCatiaReportsBSONLocaly()
                    'Documents.DeleteCatiaReportsInDocuments()
                    Dim sFolderPath_Rep As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FolderPath, False)
                    Documents.DeleteFile(clsBranch.ReturnActiveStorageFolderForDouuments, clsBranch.ReturnActiveStorageFolderForDouuments, sFolderPath_Rep, False)
                End If
                'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempAllReadyTriedtoBuildDrawingAutomaticly, "YESf")

                th_sver_Rep = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.vers, False)
                th_dt_DTparams_Rep = clsQuatation.GetActiveQuotation_DTparams
                th_FolderPath_Rep = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FolderPath, False)
                th_DrawingReports_Rep = ConfigurationManager.AppSettings("DrawingReports").ToString.Trim & clsBranch.ReturnActiveBranchCodeForDocuments & "\"
                th_ReturnActiveStorageFolderForDouuments_Rep = clsBranch.ReturnActiveStorageFolderForDouuments()
                th_ReturnActiveBranchCodeForDocuments_Rep = clsBranch.ReturnActiveBranchCodeForDocuments()
                'th_sQutNOAs400_Rep = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number, False)
                th_LoogedEmail_Rep = clsQuatation.ACTIVE_UseLoggedEmail
                th_sQutNOAs400_Rep = clsQuatation.ACTIVE_QuotationNumber
                th_BranchCode_Rep = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)
                th_SemiToolDescription_Rep = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.SemiToolDescription, False)
                th_s_CustomerNumber_Rep = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
                th_newFileName_Rep = Documents.BuildandGetDocumentName()
                th_newFileNameBSON_Rep = Documents.BuildandGetDocumentNamePDFBSON()

                Dim thread1 As Thread = New Thread(AddressOf BuildCatiaReportThred)
                Try
                    thread1.IsBackground = True
                    thread1.Start()
                Catch ex As Threading.ThreadAbortException
                    thread1.Abort()
                Catch ex As Exception
                    thread1.Abort()
                End Try

                Try
                    ' thread1.Join()
                    Threading.Thread.Sleep(2000)
                    If ConfigurationManager.AppSettings("RunPDF2DWiththread") = "YES" Then
                        Dim sclsth_RepBSON As New clsThreadPDFBSON

                        sclsth_RepBSON.DoThreadReportBSON(th_sver_Rep, th_dt_DTparams_Rep, th_FolderPath_Rep, th_DrawingReports_Rep, th_ReturnActiveStorageFolderForDouuments_Rep,
                            th_ReturnActiveBranchCodeForDocuments_Rep, th_LoogedEmail_Rep, th_sQutNOAs400_Rep, th_BranchCode_Rep, th_SemiToolDescription_Rep, th_s_CustomerNumber_Rep, th_newFileNameBSON_Rep)
                        sclsth_RepBSON = Nothing
                    End If
                Catch ex As Exception
                End Try


            Catch ex As Threading.ThreadAbortException
                Dim sMsg As String = "Error : create catia report , Function name DoThreadReport<br>"
                sMsg = ex.Message & "<br>"
                sMsg = sMsg & "Company - " & th_BranchCode_Rep
                sMsg = sMsg & "Date - " & Now.ToLongDateString
                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "PDF_2D", ex.Message, th_BranchCode_Rep, th_sQutNOAs400_Rep, th_sQutNOAs400_Rep, th_LoogedEmail_Rep)

                clsMail.SendEmailWithoutAttachment(ConfigurationManager.AppSettings("MailFrom").ToString.Trim, "SemiStandatrd Message", sMsg, True, False, "", False, Nothing, "", "", "")
            Catch ex As Exception
                Dim sMsg As String = "Error : create catia report , Function name DoThreadReport<br>"
                sMsg = ex.Message & "<br>"
                sMsg = sMsg & "Company - " & th_BranchCode_Rep
                sMsg = sMsg & "Date - " & Now.ToLongDateString
                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "PDF_2D", ex.Message, th_BranchCode_Rep, th_sQutNOAs400_Rep, th_sQutNOAs400_Rep, th_LoogedEmail_Rep)
                clsMail.SendEmailWithoutAttachment(ConfigurationManager.AppSettings("MailFrom").ToString.Trim, "SemiStandatrd Message", sMsg, True, False, "", False, Nothing, "", "", "")
            End Try

        End If

    End Sub

    Private Sub BuildCatiaReportThred()

        Dim sLogCatiaReportThred_2D As String = ""
        Dim sStatus_2D As String = ""


        Try

            clUtils.SetNewErrorDescription(sLogCatiaReportThred_2D, "Set 2D Request Service")

            Dim RequestService_2D As Object = Nothing 'As requestqueueWsProd.Service1SoapClient = New requestqueueWsProd.Service1SoapClient()
            Dim EitRequest_2D As Object = Nothing 'As requestqueueWsProd.EITQRRequest = New requestqueueWsProd.EITQRRequest()
            Dim eitResult_2D As Object = Nothing 'As requestqueueWsProd.EITResult = Nothing

            Dim WSLocaly_2D As String = ConfigurationManager.AppSettings("UserCATIA_WSLocaly").ToString.Trim
            If WSLocaly_2D = "LOCAL" Then
                RequestService_2D = New requestqueueWs.Service1SoapClient()
                EitRequest_2D = New requestqueueWs.EITQRRequest()
                eitResult_2D = Nothing
            ElseIf WSLocaly_2D = "PROD" Then
                RequestService_2D = New requestqueueWsProd.Service1SoapClient()
                EitRequest_2D = New requestqueueWsProd.EITQRRequest()
                eitResult_2D = Nothing
            ElseIf WSLocaly_2D = "BALANCER" Then
                RequestService_2D = New requestqueueWsBalancer.Service1SoapClient()
                EitRequest_2D = New requestqueueWsBalancer.EITQRRequest()
                eitResult_2D = Nothing
            End If

            Try
                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT.ToString, "BSON BuildCatiaReportThredBSON", DirectCast(RequestService_2D, System.ServiceModel.ClientBase(Of SemiApp_bl.requestqueueWs.Service1Soap)).Endpoint.Address.Uri.AbsoluteUri, th_BranchCode_Rep, th_sQutNOAs400_Rep, th_sQutNOAs400_Rep, "")
            Catch ex As Exception
            End Try

            EitRequest_2D.AppName = ConfigurationManager.AppSettings("2DRequestAppName")
            EitRequest_2D.Rt = "00:00:59"
            EitRequest_2D.TotalSizeOfFiles = 0.4
            EitRequest_2D.UserName = IIf(th_LoogedEmail_Rep.ToString <> "", th_LoogedEmail_Rep.ToString, "iQuote2D - unidentified")

            eitResult_2D = RequestService_2D.CreateRequest(EitRequest_2D)
            If eitResult_2D Is Nothing OrElse eitResult_2D.QR_ID.ToString.Trim = "" Then clUtils.SetNewErrorDescription(sLogCatiaReportThred_2D, "eitResult is nothing or empty, eitResult.Status=" & eitResult_2D.Status.ToString)

            sStatus_2D = eitResult_2D.Status

            Dim sSc_2D As String = ConfigurationManager.AppSettings("TimeToWaitForBuildCATIA")

            Dim urlSerReport_2D As String = ""
            Dim result_Rep_2D As String = ""
            Dim Success_2D As Boolean = False

            EitRequest_2D.Id = eitResult_2D.QR_ID

            For TimerThreadSleepBSON_2D As Integer = 0 To CInt(sSc_2D)
                sStatus_2D = eitResult_2D.Status
                If sStatus_2D.ToLower = "w" Then
                    Threading.Thread.Sleep(1000)
                    EitRequest_2D.Status = "w"
                    eitResult_2D = RequestService_2D.UpdateRequest(EitRequest_2D)
                ElseIf sStatus_2D.ToLower = "e" Then
                    clsMail.SendEmailErrorCatiaDrawing("Create 2D Failed!", "Create 2D Failed", sLogCatiaReportThred_2D, th_ReturnActiveBranchCodeForDocuments_Rep, th_LoogedEmail_Rep)
                    Exit For
                ElseIf sStatus_2D.ToLower = "p" Then
                    Exit For
                End If
            Next


            If sStatus_2D.ToLower = "w" Then
                EitRequest_2D.Status = "e"
                EitRequest_2D.ErrorDescription = "Wait for queue for more then " + (sSc_2D - 1) + " secounds"
                RequestService_2D.UpdateRequest(EitRequest_2D)
                clsMail.SendEmailErrorCatiaDrawing("Create 2D Failed!", "Create 2D Failed", sLogCatiaReportThred_2D, th_ReturnActiveBranchCodeForDocuments_Rep, th_LoogedEmail_Rep)
            ElseIf sStatus_2D.ToLower = "p" Then
                urlSerReport_2D = eitResult_2D.ApplicationUrl
                Try
                    CatiaDrawing.CreateCatiaDrawing(th_sver_Rep, th_dt_DTparams_Rep, th_FolderPath_Rep, th_DrawingReports_Rep,
                                                                                                             th_ReturnActiveStorageFolderForDouuments_Rep, th_ReturnActiveBranchCodeForDocuments_Rep, th_sQutNOAs400_Rep, th_BranchCode_Rep,
                                                                                                          th_SemiToolDescription_Rep, th_s_CustomerNumber_Rep, th_newFileName_Rep, result_Rep_2D, sLogCatiaReportThred_2D, th_LoogedEmail_Rep, urlSerReport_2D, Success_2D, "2D")
                    clUtils.SetNewErrorDescription(sLogCatiaReportThred_2D, "Success - " & Success_2D)
                    If Success_2D = True Then
                        EitRequest_2D.Id = eitResult_2D.QR_ID
                        EitRequest_2D.Status = "c"
                        RequestService_2D.UpdateRequest(EitRequest_2D)

                        UpdateQuotationDrawing(th_sQutNOAs400_Rep, th_BranchCode_Rep)


                    End If

                Catch ex As Exception
                    Success_2D = False
                    'clsMail.SendEmailErrorCatiaDrawing("CreateCatiaDrawing Failed!", "Exp(1)" & "<br>" & ex.Message, sLogCatiaReportThred_2D, th_ReturnActiveBranchCodeForDocuments_Rep, th_LoogedEmail_Rep)
                End Try

            End If

            If Success_2D = False Then
                EitRequest_2D.Id = eitResult_2D.QR_ID
                EitRequest_2D.Status = "e"
                EitRequest_2D.ErrorDescription = Left(th_LoogedEmail_Rep & " : " & sLogCatiaReportThred_2D, 300).Replace("'", "")
                RequestService_2D.UpdateRequest(EitRequest_2D)
                clsMail.SendEmailErrorCatiaDrawing("Create 2D Failed!", "Create 2D Failed", sLogCatiaReportThred_2D, th_ReturnActiveBranchCodeForDocuments_Rep, th_LoogedEmail_Rep)
                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "Create 2D Failed!", sLogCatiaReportThred_2D, th_BranchCode_Rep, th_sQutNOAs400_Rep, "", th_LoogedEmail_Rep)
            End If

            RequestService_2D = Nothing
            EitRequest_2D = Nothing
            eitResult_2D = Nothing


        Catch ex As Exception

        End Try
    End Sub


    Public Shared Sub UpdateQuotationDrawing(th_sQutNOAs4 As String, th_BranchCod As String)
        Try

            Dim qnam As String = "DRW_" & Format(CInt(th_sQutNOAs4), "0000000") & ".pdf"

            Dim oTxSql As SqlDal
            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

            Dim oParamsD As New SqlParams()
            oParamsD.Add(New SqlParam("@AS4no", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, th_sQutNOAs4))
            oParamsD.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, th_BranchCod, 2))
            oParamsD.Add(New SqlParam("@DrawingName", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, qnam, 200))

            oTxSql.ExecuteSP("USP_UpdateQuotation_Drawing", oParamsD)

            oParamsD = Nothing
            oTxSql = Nothing

        Catch ex As Exception
            'GeneralException.WriteEventErrors("UpdateQuotationDrawing-" & ex.Message, GeneralException.e_LogTitle.UPDATEDATA.ToString)
            'GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateQuotationDrawing", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        End Try
    End Sub




End Class
