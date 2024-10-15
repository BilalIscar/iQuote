Public Class ShowPDFReportBSON
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Try

        '    Dim LocalFolder As String = ConfigurationManager.AppSettings("DrawingReports")
        '    Dim _fileName As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._DrawingName, False)

        '    If (File.Exists(LocalFolder & _fileName)) Then

        '        Dim b() As Byte 
        '        b = System.IO.File.ReadAllBytes(LocalFolder & _fileName)

        '        Dim _bc As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)

        '        If Not b Is Nothing Then
        '            Response.ContentType = "application/pdf"
        '            Response.AddHeader("Content-disposition", "inline")
        '            Response.BinaryWrite(b.ToArray())

        '        End If
        '    Else
        '        'Dim bytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/sp2013_apps-overview.pdf"))
        '        Dim b() As Byte = Nothing
        '        Dim _fileFolderPath As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Temp_fileFolderPath, False)

        '        Dim _bc As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)

        '        If _fileFolderPath <> "" AndAlso _fileName <> "" Then

        '            Documents.getDocument(_fileFolderPath, _fileName, clsBranch.ReturnActiveBranchCodeForDocuments, b, clsBranch.ReturnActiveStorageFolderForDouuments)

        '            If Not b Is Nothing Then
        '                Response.ContentType = "application/pdf"
        '                Response.AddHeader("Content-disposition", "inline")
        '                Response.BinaryWrite(b.ToArray())
        '            End If

        '        End If
        '    End If



        'Catch ex As Exception
        '    GeneralException.WriteEventErrors(ex.Message)

        '    'GeneralException.BuildError(Page, ex.Message)
        'End Try


    End Sub


End Class


'Namespace ForumTest
'    Partial Public Class Test
'        Inherits System.Web.UI.Page

'        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)

'        End Sub
'    End Class
'End Namespace
