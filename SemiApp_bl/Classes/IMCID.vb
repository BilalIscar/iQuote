Imports System.Collections.Specialized
Imports System.Configuration
Imports System.Net
Imports System.Net.Http
Imports System.Web

Public Class IMCID


    Public Shared Function GetIMCIDToken() As String
        Try

            Try

                Dim appILogIPlinkd As String = ConfigurationManager.AppSettings("LogIPlinkNEW")
                Dim query As NameValueCollection = HttpUtility.ParseQueryString(String.Empty)

                Dim url As UriBuilder = New UriBuilder(appILogIPlinkd)

                url.Query = query.ToString()

                Dim sIP As WebClient = New WebClient()
                sIP.Headers.Add("Accept", "application/json")
                sIP.Headers.Add("Content-Type", "application/json; charset=UTF-8")
                'sIP.Headers.Add("token", ")
                Dim apid As String = ConfigurationManager.AppSettings("ApplicationID")
                Dim response As String = sIP.UploadString(ConfigurationManager.AppSettings("LoginGetTokenNEW") & apid, "POST")
                Return response

                'If response <> "" AndAlso response.ToString.ToUpper.Contains("LOGGEDEMAIL") AndAlso Not response.ToString.ToUpper.Contains("TABLE") AndAlso response.StartsWith("[") Then
                '    Return response.ToString.Trim
                'Else
                '    Return "0"
                'End If
            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try
    End Function

End Class
