Imports System.Web
Imports IscarDal

Public Class SecurityManager
#Region "Enum's"
    Public Enum SecurityRoles As Integer
        Administration = 1
        QuatationCreation = 2
        QuatationDeletition = 3
        General = 4
        ViewModelInDesign = 5
        WholeBranchQoutationDeletition = 6
        QuatationUpdate = 7
        WholeBranchQuatationUpdate = 8
        ImportCustomersRulles = 9
        QuotationCreateRevision = 10
        WholeBranchQuotationCreateRevision = 11
        DeleteQuotationLog = 12
        WholeBranchQuotationList = 13
        ShowToolsMenu = 14
        UpdateToolsGlobaldata = 15
        SendQuotationByMail = 16
        UnShowSalesmenData = 17
        AllowToSeeSites = 18
        AllowToChangeValidTime = 19
        SalesmenShowsOnlyHisQutations = 20
        DisalbeChangingDiscountPrice = 21
        CalcDefalutGP = 22
        AllowToInsertNewQutFromGAL = 23
        SeeQuotationsListBranchesOnly = 24
        AllowToIncreasePrice = 25
        AllowAccessToiQuoteToolSys = 26
        AllowToBuildQuotatinReports = 27
    End Enum
#End Region

#Region "Function's"
    Public Shared Function UserInRole(ByVal SecurityRole As SecurityRoles) As Boolean
        If UserIsAuthenticated() Then
            If HttpContext.Current.User.IsInRole(SecurityRole.GetHashCode.ToString) Then
                Return True
            Else : Return False
            End If
        Else : Return False
        End If
    End Function

    Public Shared Function UserIsAuthenticated() As Boolean
        Return HttpContext.Current.User.Identity.IsAuthenticated
    End Function

    Public Shared Sub ReCreateidentity()
        'Dim Cookie As String = StateManager.GetValue(StateManager.Keys.s_EncrypedCookie)
        'Dim Ticket As FormsAuthenticationTicket
        'Dim DoLogin As Boolean = True
        'If Not IsNothing(Cookie) Then
        '    Ticket = FormsAuthentication.Decrypt(Cookie)
        '    If Ticket.Expired Then
        '        FormsAuthentication.SignOut()
        '    Else
        '        Dim principal As GenericPrincipal
        '        Dim identity As GenericIdentity
        '        Dim Roles() As String = CryptoManagerTDES.Decode(Ticket.UserData).Split(",".ToCharArray())

        '        identity = New GenericIdentity(Ticket.Name.ToString(), "Custom")
        '        principal = New GenericPrincipal(identity, Roles)
        '        HttpContext.Current.User = principal
        '        DoLogin = False
        '    End If
        'End If
    End Sub
#End Region



    Public Shared Function GetMainDataED(ByVal BranchCode As String) As DataTable
        Try
            Dim oSql As New SqlDal("MainConnectionString")
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@branchCode", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, BranchCode))
            dt = oSql.ExecuteSPReturnDT("usp_GetEncryptionDecryptionData", oParams)
            Return dt
        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Sub aaaa(ByVal aaaa As String)
        Try

            Dim oSql As New SqlDal("MainConnectionString")
            Dim oParams As New SqlParams()

            oParams.Add(New SqlParam("@aaaa", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, aaaa))
            oSql.ExecuteSP("usp_aaaa", oParams)
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Shared Function CheckIfUserNotAuthinticatedwithTheQuote(FormToCheck As String) As Boolean

        Return True
        Try

            'Dim sUserCo As String = CookiesManager.GetValue(CookiesManager.Keys.HkhtrycdFg, True).ToString.Trim
            'Dim sUserSess As String = StateManager.GetValue(StateManager.Keys.s_loggedEmail, True).ToString.Trim

            'If sUserCo <> "" AndAlso sUserSess <> "" AndAlso sUserCo.Contains("@") AndAlso sUserCo.Contains("@") Then
            '    If sUserCo <> sUserSess Then
            '        If clsUser.SetIsUserAdminAllOrBranch = False Then
            '            'aaaa("x")
            '            Return False
            '        End If
            '    End If

            'Else
            If FormToCheck = "PRICE" Then

                    If IsNothing(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)) OrElse SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False).ToString = "" Then

                        Return False
                    End If
                End If
                Return True
        Catch ex As Exception
            Return True
        End Try
    End Function

End Class

