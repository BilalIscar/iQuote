Imports System.Configuration
Imports System.IO
Imports System.Threading

Public Class clsThreadPDFBSON

    Private thB_sver_Rep As String = ""
    Private thB_dt_DTparams_Rep As DataTable = Nothing
    Private thB_FolderPath_Rep As String = ""
    Private thB_DrawingReports_Rep As String = ""
    Private thB_ReturnActiveStorageFolderForDouuments_Rep As String = ""
    Private thB_ReturnActiveBranchCodeForDocuments_Rep As String = ""
    Private thB_sQutNOAs400_Rep As String = ""
    Private thB_BranchCode_Rep As String
    Private thB_SemiToolDescription_Rep As String
    Private thB_s_CustomerNumber_Rep As String
    Private thB_newFileName_Rep As String
    Private thB_LoogedEmail_Rep As String

    Public Sub DoThreadReportBSON(thB_sver_Re As String, thB_dt_DTparams_Re As DataTable, thB_FolderPath_Re As String, thB_DrawingReports_Re As String, thB_ReturnActiveStorageFolderForDouuments_Re As String,
                                  thB_ReturnActiveBranchCodeForDocuments_Re As String, thB_LoogedEmail_Re As String,
                                  thB_sQutNOAs400_Re As String, thB_BranchCode_Re As String, thB_SemiToolDescription_Re As String, thB_s_CustomerNumber_Re As String, thB_newFileName_Re As String)
        Try
            If clsQuatation.checkIfCanDoDrawingForActiveModel() = False Then
                Exit Sub
            End If
        Catch ex As Exception

        End Try
        If ConfigurationManager.AppSettings("CATIA_ACTIVE") = "YES" Then

            Try

                thB_sver_Rep = thB_sver_Re
                thB_dt_DTparams_Rep = thB_dt_DTparams_Re
                thB_FolderPath_Rep = thB_FolderPath_Re
                thB_DrawingReports_Rep = thB_DrawingReports_Re
                thB_ReturnActiveStorageFolderForDouuments_Rep = thB_ReturnActiveStorageFolderForDouuments_Re
                thB_ReturnActiveBranchCodeForDocuments_Rep = thB_ReturnActiveBranchCodeForDocuments_Re
                thB_LoogedEmail_Rep = thB_LoogedEmail_Re
                thB_sQutNOAs400_Rep = thB_sQutNOAs400_Re
                thB_BranchCode_Rep = thB_BranchCode_Re
                thB_SemiToolDescription_Rep = thB_SemiToolDescription_Re
                thB_s_CustomerNumber_Rep = thB_s_CustomerNumber_Re
                thB_newFileName_Rep = thB_newFileName_Re

                Dim thread3 As Thread = New Thread(AddressOf BuildCatiaReportThredBSON)
                Try
                    thread3.IsBackground = True
                    thread3.Start()
                Catch ex As Threading.ThreadAbortException
                    thread3.Abort()
                Catch ex As Exception
                    thread3.Abort()
                End Try

            Catch ex As Threading.ThreadAbortException
                Dim sMsg As String = "Error : create catia report , Function name DoThreadReport<br>"
                sMsg = ex.Message & "<br>"
                sMsg = sMsg & "Company - " & thB_BranchCode_Rep
                sMsg = sMsg & "Date - " & Now.ToLongDateString

                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "2D_3D", ex.Message, thB_BranchCode_Rep, thB_sQutNOAs400_Rep, thB_sQutNOAs400_Rep, thB_LoogedEmail_Re)
                clsMail.SendEmailWithoutAttachment(ConfigurationManager.AppSettings("MailFrom").ToString.Trim, "SemiStandatrd Message", sMsg, True, False, "", False, Nothing, "", "", "")
            Catch ex As Exception
                Dim sMsg As String = "Error : create catia report , Function name DoThreadReport<br>"
                sMsg = ex.Message & "<br>"
                sMsg = sMsg & "Company - " & thB_BranchCode_Rep
                sMsg = sMsg & "Date - " & Now.ToLongDateString

                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "2D_3D", ex.Message, thB_BranchCode_Rep, thB_sQutNOAs400_Rep, thB_sQutNOAs400_Rep, thB_LoogedEmail_Re)
                clsMail.SendEmailWithoutAttachment(ConfigurationManager.AppSettings("MailFrom").ToString.Trim, "SemiStandatrd Message", sMsg, True, False, "", False, Nothing, "", "", "")
            End Try

        End If

    End Sub



    Private Sub BuildCatiaReportThredBSON()

        Dim sLogCatiaReportThred_2D3D As String = ""
        Dim sStatus_2D3D As String = ""


        Try

            clUtils.SetNewErrorDescription(sLogCatiaReportThred_2D3D, "Set 2D3D Request Service")

            '--------DEFINE-------------
            Dim RequestService_2D3D As Object = Nothing
            Dim EitRequest_2D3D As Object = Nothing
            Dim eitResult_2D3D As Object = Nothing

            Dim WSLocaly_2D3D As String = ConfigurationManager.AppSettings("UserCATIA_WSLocaly").ToString.Trim
            If WSLocaly_2D3D = "LOCAL" Then
                RequestService_2D3D = New requestqueueWs.Service1SoapClient()
                EitRequest_2D3D = New requestqueueWs.EITQRRequest()
                eitResult_2D3D = Nothing
            ElseIf WSLocaly_2D3D = "PROD" Then
                RequestService_2D3D = New requestqueueWsProd.Service1SoapClient()
                EitRequest_2D3D = New requestqueueWsProd.EITQRRequest()
                eitResult_2D3D = Nothing
            ElseIf WSLocaly_2D3D = "BALANCER" Then
                RequestService_2D3D = New requestqueueWsBalancer.Service1SoapClient
                EitRequest_2D3D = New requestqueueWsBalancer.EITQRRequest()
                eitResult_2D3D = Nothing
            End If
            '----------------------------


            EitRequest_2D3D.AppName = ConfigurationManager.AppSettings("3DPDFRequestAppName")
            EitRequest_2D3D.Rt = "00:00:59"
            EitRequest_2D3D.TotalSizeOfFiles = 0.4
            EitRequest_2D3D.UserName = IIf(thB_LoogedEmail_Rep.ToString <> "", thB_LoogedEmail_Rep.ToString, "iQuote2D3D - unidentified")
            Try
                eitResult_2D3D = RequestService_2D3D.CreateRequest(EitRequest_2D3D)
                If eitResult_2D3D Is Nothing OrElse eitResult_2D3D.QR_ID.ToString.Trim = "" Then
                    clUtils.SetNewErrorDescription(sLogCatiaReportThred_2D3D, "eitResult is nothing or empty, eitResult.Status=" & eitResult_2D3D.Status.ToString)
                End If
            Catch ex As Exception
                clUtils.SetNewErrorDescription(sLogCatiaReportThred_2D3D, "eitResult Exception: " & ex.Message)
                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "2D_3D eitResult Exception!", sLogCatiaReportThred_2D3D, thB_BranchCode_Rep, thB_sQutNOAs400_Rep, thB_sQutNOAs400_Rep, thB_LoogedEmail_Rep)
            End Try

            Try
                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT.ToString, "BSON BuildCatiaReportThredBSON", DirectCast(RequestService_2D3D, System.ServiceModel.ClientBase(Of SemiApp_bl.requestqueueWs.Service1Soap)).Endpoint.Address.Uri.AbsoluteUri, thB_BranchCode_Rep, thB_sQutNOAs400_Rep, thB_sQutNOAs400_Rep, "")
            Catch ex As Exception
            End Try
            Try
                If Not eitResult_2D3D Is Nothing Then
                    sStatus_2D3D = eitResult_2D3D.Status

                End If
            Catch ex As Exception

            End Try

            Dim sSc_2D3D As String = ConfigurationManager.AppSettings("TimeToWaitForBuildCATIA")

            Dim urlSerReport_2D3D As String = ""
            Dim result_Rep_2D3D As String = ""
            Dim Success_2D3D As Boolean = False

            If Not eitResult_2D3D Is Nothing Then
                EitRequest_2D3D.Id = eitResult_2D3D.QR_ID
            End If

            For TimerThreadSleepBSON_2D3D As Integer = 0 To CInt(sSc_2D3D)
                If Not eitResult_2D3D Is Nothing Then
                    sStatus_2D3D = eitResult_2D3D.Status
                    If sStatus_2D3D.ToLower = "w" Then
                        Threading.Thread.Sleep(1000) : EitRequest_2D3D.Status = "w" : eitResult_2D3D = RequestService_2D3D.UpdateRequest(EitRequest_2D3D)
                    ElseIf sStatus_2D3D.ToLower = "e" Then
                        clsMail.SendEmailErrorCatiaDrawing("Create 2D3D Failed!", "Create 2D3D Failed", sLogCatiaReportThred_2D3D, thB_ReturnActiveBranchCodeForDocuments_Rep, thB_LoogedEmail_Rep)
                        Exit For
                    ElseIf sStatus_2D3D.ToLower = "p" Then
                        Exit For
                    End If
                Else
                    Exit For
                End If

            Next


            If sStatus_2D3D.ToLower = "w" Then
                EitRequest_2D3D.Status = "e"
                EitRequest_2D3D.ErrorDescription = "Wait for queue for more then " + (sSc_2D3D - 1) + " secounds"
                RequestService_2D3D.UpdateRequest(EitRequest_2D3D)
                clsMail.SendEmailErrorCatiaDrawing("Create 2D3D Failed!", "Create 2D3D Failed", sLogCatiaReportThred_2D3D, thB_ReturnActiveBranchCodeForDocuments_Rep, thB_LoogedEmail_Rep)

            ElseIf sStatus_2D3D.ToLower = "p" Then
                urlSerReport_2D3D = eitResult_2D3D.ApplicationUrl
                Try
                    CatiaDrawing.CreateCatiaDrawing(thB_sver_Rep, thB_dt_DTparams_Rep, thB_FolderPath_Rep, thB_DrawingReports_Rep,
                                                                                                         thB_ReturnActiveStorageFolderForDouuments_Rep, thB_ReturnActiveBranchCodeForDocuments_Rep, thB_sQutNOAs400_Rep, thB_BranchCode_Rep,
                                                                                                      thB_SemiToolDescription_Rep, thB_s_CustomerNumber_Rep, thB_newFileName_Rep, result_Rep_2D3D, sLogCatiaReportThred_2D3D, thB_LoogedEmail_Rep, urlSerReport_2D3D, Success_2D3D, "3DPDF")
                    If Success_2D3D = True Then
                        EitRequest_2D3D.Id = eitResult_2D3D.QR_ID
                        EitRequest_2D3D.Status = "c"
                        RequestService_2D3D.UpdateRequest(EitRequest_2D3D)
                        File.Copy(ConfigurationManager.AppSettings("TempFilesfOLDEERlOCATION") & "BSON.txt", ConfigurationManager.AppSettings("TempFilesfOLDEERlOCATION") & thB_sQutNOAs400_Rep & "_BSON_X.txt")
                        clsMail.SendEmailErrorCatiaDrawing("Create 2D3D Success.", "Create 2D3D Success.", sLogCatiaReportThred_2D3D, thB_ReturnActiveBranchCodeForDocuments_Rep, thB_LoogedEmail_Rep)
                    End If
                Catch ex As Exception
                    Success_2D3D = False
                    'clsMail.SendEmailErrorCatiaDrawing("CreateCatiaDrawing Failed!", "Exp(1)" & "<br>" & ex.Message, sLogCatiaReportThred_2D3D, thB_ReturnActiveBranchCodeForDocuments_Rep, thB_LoogedEmail_Rep)
                End Try

                clUtils.SetNewErrorDescription(sLogCatiaReportThred_2D3D, "Success - " & Success_2D3D)

                If Success_2D3D = False Then
                    EitRequest_2D3D.Id = eitResult_2D3D.QR_ID
                    EitRequest_2D3D.Status = "e"
                    EitRequest_2D3D.ErrorDescription = Left(thB_LoogedEmail_Rep & " : " & sLogCatiaReportThred_2D3D, 300).Replace("'", "")
                    RequestService_2D3D.UpdateRequest(EitRequest_2D3D)
                    clsMail.SendEmailErrorCatiaDrawing("Create 2D3D Failed!", "Create 2D3D Failed", sLogCatiaReportThred_2D3D, thB_ReturnActiveBranchCodeForDocuments_Rep, thB_LoogedEmail_Rep)
                End If
            Else
                If Not eitResult_2D3D Is Nothing Then
                    EitRequest_2D3D.Id = eitResult_2D3D.QR_ID
                    EitRequest_2D3D.Status = "e"
                    RequestService_2D3D.UpdateRequest(EitRequest_2D3D)
                    clsMail.SendEmailErrorCatiaDrawing("Create 2D3D Failed!", "Create 2D3D Failed", sLogCatiaReportThred_2D3D, thB_ReturnActiveBranchCodeForDocuments_Rep, thB_LoogedEmail_Rep)
                Else
                    clsMail.SendEmailErrorCatiaDrawing("Create 2D3D Failed!", "Create 2D3D Failed", "eitResult_2D3D = Nothing", thB_ReturnActiveBranchCodeForDocuments_Rep, thB_LoogedEmail_Rep)
                End If

            End If

            RequestService_2D3D = Nothing
            EitRequest_2D3D = Nothing
            eitResult_2D3D = Nothing


        Catch ex As Exception

        End Try

    End Sub


End Class
