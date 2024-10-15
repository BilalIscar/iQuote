Imports System.IO
Imports SemiApp_bl


Public Class CatiaReportHandel
    Implements System.Web.IHttpHandler

    'Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

    '    context.Response.ContentType = "text/plain"
    '    context.Response.Write("Hello World!")

    'End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            'GeneralException.WriteEventLogReport("0 _ IsReusable & False : ")
            Return False
        End Get
    End Property


    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        ' Write your handler implementation here.
        Try

            Dim LocalFolder As String = ConfigurationManager.AppSettings("DrawingReports") & context.Request.QueryString("ActiveBranch") & "\"
            Dim _fileName As String = context.Request.QueryString("_fileName") ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._DrawingName, False)
            'GeneralException.WriteEventLogReport("0 _ LocalFolder & _fileName : " & LocalFolder & _fileName)

            'GeneralException.WriteEventLogReport("_fileName : " & _fileName & "<br>")
            'GeneralException.WriteEventLogReport("LocalFolder : " & LocalFolder & "<br>")
            If context.Request.QueryString("ShowBuild") = "SHOW" AndAlso File.Exists(LocalFolder & _fileName) Then
                'GeneralException.WriteEventLogReport("ProcessRequest", "01 _ LocalFolder & _fileName : " & LocalFolder & _fileName, GeneralException.e_LogTitle.REPORT)
                Try
                    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_PRINTOUT.ToString, "PrintOut report-1", "ProcessRequest - 01 _ LocalFolder & _fileName : " & LocalFolder & _fileName, "", "", "", "")
                Catch ex As Exception

                End Try

                'GeneralException.WriteEventLogReport("ShowBuild : " & "<br>")
                Dim b() As Byte
                b = System.IO.File.ReadAllBytes(LocalFolder & _fileName)

                'Dim _bc As String = context.Request.QueryString("_bc")

                If Not b Is Nothing Then
                    'GeneralException.WriteEventLogReport("ShowBuild : " & " b")
                    context.Response.ContentType = "application/pdf"
                    context.Response.AddHeader("Content-disposition", "inline")
                    context.Response.BinaryWrite(b.ToArray())
                End If
            Else
                'GeneralException.WriteEventLogReport("02 _ LocalFolder & _fileName : " & LocalFolder & _fileName)
                If (File.Exists(LocalFolder & _fileName)) Then
                    'GeneralException.WriteEventLogReport("ProcessRequest", "03 _ LocalFolder & _fileName : " & LocalFolder & _fileName, GeneralException.e_LogTitle.REPORT)
                    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_PRINTOUT.ToString, "PrintOut report-2", "ProcessRequest - 01 _ LocalFolder & _fileName : " & LocalFolder & _fileName, "", "", "", "")

                    Dim b() As Byte
                    b = System.IO.File.ReadAllBytes(LocalFolder & _fileName)
                    ' GeneralException.WriteEventLogReport("ProcessRequest", "1 _ LocalFolder & _fileName : " & LocalFolder & _fileName)
                    'Dim _bc As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)
                    'Dim _bc As String = context.Request.QueryString("_bc")

                    If Not b Is Nothing Then
                        context.Response.ContentType = "application/pdf"
                        context.Response.AddHeader("Content-disposition", "inline")
                        context.Response.BinaryWrite(b.ToArray())
                        'GeneralException.WriteEventLogReport("2 _ LocalFolder & _fileName : " & LocalFolder & _fileName)
                    End If
                Else
                    'GeneralException.WriteEventLogReport("04 _ LocalFolder & _fileName : " & LocalFolder & _fileName)
                    'Dim bytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/sp2013_apps-overview.pdf"))
                    Dim b() As Byte = Nothing
                    'Dim _fileFolderPath As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Temp_fileFolderPath, False)
                    'GeneralException.WriteEventLogReport("3 _ LocalFolder & _fileName : " & LocalFolder & _fileName)
                    Dim _fileFolderPath As String = context.Request.QueryString("_fileFolderPath")

                    'Dim _bc As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)
                    Dim _bc As String = context.Request.QueryString("_bc")

                    If _fileFolderPath <> "" AndAlso _fileName <> "" Then

                        Documents.getDocument(_fileFolderPath, _fileName, context.Request.QueryString("ActiveBranch"), b, context.Request.QueryString("ActiveBranchD"))
                        'GeneralException.WriteEventLogReport("_fileFolderPath : " & _fileFolderPath & "<br>")
                        'GeneralException.WriteEventLogReport("_fileName : " & _fileName & "<br>")
                        'GeneralException.WriteEventLogReport("ActiveBranch : " & context.Request.QueryString("ActiveBranch") & "<br>")
                        'GeneralException.WriteEventLogReport("ActiveBranchD : " & context.Request.QueryString("ActiveBranchD") & "<br>")
                        If Not b Is Nothing Then
                            context.Response.ContentType = "application/pdf"
                            context.Response.AddHeader("Content-disposition", "inline")
                            context.Response.BinaryWrite(b.ToArray())
                            'GeneralException.WriteEventLogReport("4 _ LocalFolder & _fileName : " & LocalFolder & _fileName)

                        End If

                    End If
                End If

            End If


        Catch ex As Exception
            'GeneralException.WriteEventErrors("ProcessRequest : " & ex.Message, GeneralException.e_LogTitle.REPORT)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_PRINTOUT_ERROR.ToString, "PrintOut report error", "ProcessRequest : " & ex.Message, GeneralException.e_LogTitle.REPORT.ToString, "", "", "")

        End Try
    End Sub
End Class