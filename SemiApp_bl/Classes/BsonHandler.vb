Imports System.Web

Public Class BsonHandler
    Implements IHttpHandler

    Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim id As String = context.Request.QueryString("id")
        If String.IsNullOrEmpty(id) Then
            context.Response.StatusCode = 400 ' Bad Request
            Return
        End If

        ' Map the ID to the actual file path
        Dim filePath As String = MapIdToFilePath(id)
        If String.IsNullOrEmpty(filePath) OrElse Not System.IO.File.Exists(filePath) Then
            context.Response.StatusCode = 404 ' Not Found
            Return
        End If

        context.Response.ContentType = "application/bson"
        context.Response.TransmitFile(filePath)
    End Sub

    Public ReadOnly Property IsReusable As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Private Function MapIdToFilePath(id As String) As String
        ' Implement your logic to map the id to a file path
        ' For example, assuming the files are stored in App_Data folder
        Return HttpContext.Current.Server.MapPath($"~/App_Data/{id}.bson")
    End Function
End Class
