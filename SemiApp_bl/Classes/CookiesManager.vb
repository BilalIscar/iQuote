Imports System.Net
Imports System.Reflection
Imports System.Web
Imports Newtonsoft.Json.Linq

Public Class CookiesManager

    'Public Enum Keys
    'etoedfrku = 11
    'HkhtrycdFg = 12
    'ssBil = 13
    'End Enum

    'Public Shared Function GetValue(ByVal Key As Keys, Optional ByVal Encrypted As Boolean = False) As String
    '    Try

    '        If Encrypted Then
    '            If Not HttpContext.Current.Request.Cookies(Key.ToString) Is Nothing Then
    '                Return CryptoManager.Decode(HttpContext.Current.Request.Cookies(Key.ToString).Value)
    '            Else
    '                Return ""
    '            End If

    '        Else
    '            If Not HttpContext.Current.Request.Cookies(Key.ToString) Is Nothing Then
    '                Return HttpContext.Current.Request.Cookies(Key.ToString).Value
    '            Else
    '                Return ""
    '            End If

    '        End If
    '    Catch ex As Exception
    '        GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, False)
    '    End Try

    'End Function

    'Public Shared Sub SetValue(ByVal Key As Keys, ByVal Value As String, Optional ByVal Encrypted As Boolean = False)

    '    Try
    '        If Encrypted Then
    '            'Dim cookie As New HttpCookie(Key.ToString, CryptoManager.Encode(Value))
    '            'cookie.Expires = DateTime.Now.AddMinutes(60)
    '            'HttpContext.Current.Response.Cookies.Add(cookie)

    '            HttpContext.Current.Response.Cookies.Add(New HttpCookie(Key.ToString, CryptoManager.Encode(Value)))
    '        Else
    '            HttpContext.Current.Response.Cookies.Add(New HttpCookie(Key.ToString, Value))
    '        End If

    '    Catch ex As Exception
    '        GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, False)
    '    End Try
    'End Sub
    'Public Shared Sub SetStringValue(ByVal Key As String, ByVal Value As String, siteType As String)
    '    Try
    '        Dim myCookie As New HttpCookie(CryptoManagerTDES.Encode(Key.ToString))
    '        myCookie.Expires = DateTime.Now.AddMinutes(30)
    '        myCookie.Value = CryptoManagerTDES.Encode(Value)
    '        If siteType = "GLOBAL" Then
    '            myCookie.Domain = "iquote.ssl.imc-companies.com"
    '        End If

    '        myCookie.Path = "/"
    '        myCookie.SameSite = SameSiteMode.Strict
    '        myCookie.Secure = HttpContext.Current.Request.IsSecureConnection ' True '; // Only If your site Is Using HTTPS
    '        myCookie.HttpOnly = True '; // Only If your site Is Using HTTPS

    '        HttpContext.Current.Response.Cookies.Add(myCookie)

    '    Catch ex As Exception
    '        GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, False)
    '    End Try
    'End Sub

    Public Shared Function GetStringValue(ByVal Key As String) As String
        Try
            Dim cookie As HttpCookie = HttpContext.Current.Request.Cookies(Key)
            If cookie IsNot Nothing Then
                Return CryptoManagerTDES.Decode(cookie.Value)
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function

    'Public Shared Sub ClearValueCookie(ByVal Key As String)
    '    Try
    '        HttpContext.Current.Response.Cookies.Add(New HttpCookie(Key.ToString, ""))
    '        HttpContext.Current.Response.Cookies(Key.ToString).Expires = DateTime.Now.AddDays(30)
    '    Catch ex As Exception
    '        GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, False)
    '    End Try
    'End Sub

    'Public Shared Sub Set_CookieX(ByVal Key As String, ByVal Value As String, siteType As String)
    '    Try
    '        Dim myCookie As New HttpCookie(Key)
    '        myCookie.Value = Value
    '        myCookie.Expires = DateTime.Now.AddHours(1)
    '        myCookie.Path = "/"  ' Ensure the path is correct
    '        If siteType = "GLOBAL" Then
    '            myCookie.Domain = ".iquote.ssl.imc-companies.com"  ' Cookie valid across all subdomains of yourdomain.com
    '        End If

    '        HttpContext.Current.Response.Cookies.Add(myCookie)

    '    Catch ex As Exception
    '        GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, False)
    '    End Try
    'End Sub

    'Public Shared Function Get_CookieX(ByVal Key As String) As String
    '    Try
    '        Dim cookie As HttpCookie = HttpContext.Current.Request.Cookies(Key.ToString)
    '        If cookie IsNot Nothing Then
    '            Return cookie.Value
    '        Else
    '            Return ""
    '        End If


    '    Catch ex As Exception

    '        Return ""
    '    End Try
    'End Function

End Class