Imports System.IO
Imports System.Reflection
Imports SemiApp_bl
Public Class _Default
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim uniqueID As String = ""
        Dim langC As String = ""
        'Dim doClearCo As Boolean = True

        'Try

        '    If Request.QueryString("DontClearCoo") IsNot Nothing Then
        '        If Request.QueryString("DontClearCoo").ToString.Trim.ToUpper = "NO" Then
        '            doClearCo = False
        '        End If
        '    End If
        'Catch ex As Exception

        'End Try
        'Try
        '    If doClearCo = True Then
        '        CookiesManager.ClearValueCookie(CookiesManager.Keys.ssBil)
        '    End If
        'Catch ex As Exception

        'End Try
        Try

            If Not IsPostBack Then

                uniqueID = Request.QueryString("rErepTr")
                langC = Request.QueryString("langC")
                If String.IsNullOrEmpty(uniqueID) Then
                    uniqueID = "t" & DateTime.Now.Ticks.ToString() & "-" & New Random().Next(10000).ToString()

                    Try
                        uniqueID = CryptoManager.Encode(uniqueID)
                        Dim resultLD As Boolean = IsOnlyLetters(uniqueID)
                        If resultLD = False Then
                            uniqueID = "t" & DateTime.Now.Ticks.ToString() & "-" & New Random().Next(10000).ToString()
                        End If
                    Catch ex As Exception
                        uniqueID = "t" & DateTime.Now.Ticks.ToString() & "-" & New Random().Next(10000).ToString()
                    End Try

                    Dim TimeStampN As String = ""
                    Try
                        If Not Request("TimeStamp") Is Nothing Then
                            TimeStampN = Request("TimeStamp").ToString.Trim
                        End If

                    Catch ex As Exception
                        TimeStampN = ""
                    End Try

                    Dim sc As String = ""
                    Try
                        If Not IsNothing(Request("check")) Then
                            sc = Request("check")
                        End If
                    Catch ex As Exception
                        sc = ""
                    End Try

                    ' uniqueID = CryptoManager.Encode(uniqueID)

                    Dim ssBilDef As String = ""

                    'If Request("ssBilDef") IsNot Nothing AndAlso Request("ssBilDef").ToString <> "" Then
                    '    ssBilDef = Request("ssBilDef").ToString
                    '    CookiesManager.Set_Cookie(CookiesManager.Keys.ssBil.ToString, ssBilDef.ToString, ConfigurationManager.AppSettings("siteType").ToString)
                    'End If

                    If TimeStampN <> "" Then
                        If Request.Url.ToString.Contains("STARTFB=") Then
                            'STARTFB=STARTFB_N
                            Response.Redirect(Request.Url.AbsolutePath & "?rErepTr=" & uniqueID & "&STARTFB=" & Request("STARTFB").ToString & "&TimeStamp=" & TimeStampN & "&check=" & sc & "&ssBilDef=" & ssBilDef & "&langC=" & langC, True)
                        Else
                            Response.Redirect(Request.Url.AbsolutePath & "?rErepTr=" & uniqueID & "&TimeStamp=" & TimeStampN & "&check=" & sc & "&ssBilDef=" & ssBilDef & "&langC=" & langC, True)
                        End If
                    Else
                        If Request.Url.ToString.Contains("STARTFB=") Then
                            'STARTFB=STARTFB_N
                            Response.Redirect(Request.Url.AbsolutePath & "?rErepTr=" & uniqueID & "&STARTFB=" & Request("STARTFB").ToString & "&check=" & sc & "&ssBilDef=" & ssBilDef & "&langC=" & langC, True)
                        Else
                            Response.Redirect(Request.Url.AbsolutePath & "?rErepTr=" & uniqueID & "&check=" & sc & "&ssBilDef=" & ssBilDef & "&langC=" & langC, True)
                        End If
                    End If


                Else
                    ' Store the unique ID in the hidden field for further use
                    'TabUniqueID.Value = uniqueID
                End If
            Else
                ' Retrieve the unique ID from the hidden field on postback
                'uniqueID = TabUniqueID.Value
            End If

            'If Request("ssBilDef") IsNot Nothing AndAlso Request("ssBilDef").ToString <> "" Then
            '    CookiesManager.Set_Cookie(CookiesManager.Keys.ssBil.ToString, Request("ssBilDef").ToString, ConfigurationManager.AppSettings("siteType").ToString)
            'End If

            Dim sf As String = Path.GetFileName(Request.PhysicalPath)
            'Dim fn As String = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempShowCustomerAlertLogIN, "EMPTY")

            Dim sOpenIquote As String = ""

            Try
                sOpenIquote = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ErrorInWSGAL, False)
            Catch ex As Exception
                sOpenIquote = ""
            End Try
            If sOpenIquote <> "TRUE" Then
                SessionManager.Clear_Sessions_OpenIquote("FALSE")
            End If

            If StateManager.GetValue(StateManager.Keys.s_loggedEmail, True) Is Nothing Then
                StateManager.Clear_State(False)
            Else
                StateManager.Clear_State(True)
            End If

            SessionManager.Clear_ALL_Sessions()

            SessionManager.Clear_ALL_SessionsTimeStamp()

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FormName, "")
            Dim sTmjh As String = ""
            Try

                If Not Request("STARTFB") Is Nothing Then
                    sTmjh = Request("STARTFB")
                End If

            Catch ex As Exception
                sTmjh = ""
            End Try

            Dim TimeStamp As String = ""
            Try
                If Not Request("TimeStamp") Is Nothing Then
                    TimeStamp = Request("TimeStamp").ToString.Trim
                    'Try
                    '    CookiesManager.ClearValueCookie(CookiesManager.Keys.ssBil.ToString)
                    'Catch ex As Exception

                    'End Try
                End If

            Catch ex As Exception
                TimeStamp = ""
            End Try

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TimeStamp, TimeStamp)

            If sTmjh.ToString.ToUpper.Contains("STARTFB_Y") Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FormName, "StartFB.aspx?Login=Yes")
            ElseIf sTmjh.ToString.ToUpper.Contains("STARTFB_N") Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FormName, "StartFB.aspx?Login=No")
            End If

            Dim appId As String = ConfigurationManager.AppSettings("ApplicationID")



            Dim resSignOut As String = ""
            Try
                If Not IsNothing(Request("check")) AndAlso Request("check") = "DONTCHECK" Then
                    resSignOut = "DONTCHECK"
                End If
            Catch ex As Exception
                resSignOut = ""
            End Try

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_LanguageCaptions, CType(Nothing, DataTable))
            If langC IsNot Nothing AndAlso langC <> "" AndAlso langC <> "ZZ" AndAlso langC <> "IS" AndAlso langC <> "XZ" Then
                Dim dtLangParam As DataTable = clsBranch.GetBranchLanguages("", "", langC)
                If Not dtLangParam Is Nothing AndAlso dtLangParam.Rows.Count > 0 Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.qut_LanguageId, dtLangParam.Rows(0).Item("LanguageCode").ToString)
                End If
            End If



            If resSignOut = "DONTCHECK" Then
                Response.Redirect("Forms/Login.aspx?rErepTr=" & uniqueID & "&Result=0", False)
            Else
                Response.Redirect("Forms/Login.aspx?rErepTr=" & uniqueID, False)
            End If

            'Response.Redirect("Forms/Login.aspx?rErepTr=" & uniqueID, True)

        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.DEFAULT_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, False)

        End Try
    End Sub
    Function IsOnlyLetters(input As String) As Boolean
        Try
            Return input.All(Function(c) Char.IsLetterOrDigit(c))

        Catch ex As Exception
            Return False
        End Try
    End Function
    'Private Sub _Default_Init(sender As Object, e As EventArgs) Handles Me.Init
    '    Try
    '        If Not Request("TimeStamp") Is Nothing Then
    '            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.LOGIN.ToString, "Login Load ", "TimeStamp = " & Request("TimeStamp").ToString.Trim, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_iQuoteQuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
    '        Else
    '            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.LOGIN.ToString, "Login Load ", "TimeStamp ---- XX", clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_iQuoteQuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
    '        End If

    '    Catch ex As Exception

    '    End Try
    'End Sub
End Class