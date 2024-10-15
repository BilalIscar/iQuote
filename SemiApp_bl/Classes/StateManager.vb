Imports System.Configuration
Imports System.Web
Imports Newtonsoft.Json.Linq

Public Class StateManager
#Region "Enum's"
    Public Enum Keys

        's_WorkCenterID

        s_BranchCode
        s_loggedEmail
        s_DisplayName
        s_Surname
        s_GivenName
        s_Country
        s_CompanyName
        s_CustomerNumber
        s_CustomerName
        s_CustomerAddress
        s_STNCustomerCurrency
        s_FormName
        s_paymentTerms
        s_shipmethod
        s_iQutUsMail
        s_IsAdmin
        s_Culture
        s_TemporarilyBranchCode
        s_CustomerType
        s_salesperson
        s_salespersonEmail
        s_deskUser
        s_deskUserEmail
        s_technicalPerson
        s_technicalPersonEmail
        'TemporarilyQuotation_User

        's_faxNumber
        's_postalCode
        's_countryCode
        's_appUserName
        's_internetCustomerNumber
        's_supplierNumber
        's_galCustomerNumber
        's_isImc


    End Enum

    Public Enum WorkCenterID
        'Subsidiary = 0
        ISCAR_LTD = 1
        'UOP = 2
        'KIDAN = 3
        'METALDUR = 4
        'WERTEC = 5
        'INGERSOLL = 6
        'TAEGUTEC_INDIA = 7
        'TAEGUTEC_KORIA = 8
        'UNITAC = 9
        'ISCAR_GMBH = 10
        'OUTILTEC = 11
        'Zurim_Tools = 12
        'ISCAR_ELECTROSTAL = 13
        'TUNGALOY_JAPAN = 14
        'ISCAR_HUNGARY = 15
        'ISCAR_SLOVENIJA = 16
        'ISCAR_FRANE = 17
        'INGERSOLL_GMBH = 18
        'ISCAR_BRASIL = 19
        'IMC_DALIAN_CHINA = 20
        'TAEGUTEC_TURKEY = 21
        'ISCAR_METALS = 22
        'ISCAR_TURKEY = 23
        'IT_TE_DI_ITALY = 24
    End Enum
    Public Enum User_Group

        '************************'************************'************************'************************
        '************************FOR ANY CHANGES MUST CHECK USP_GetUsersList & USP_GetModels PROCEDURE'************************
        '************************'************************'************************'************************
        Administrator_Plus = 0
        Administrator = 1
        SalesMen = 2
        Manager = 3
        Employee = 4
        Read_Only = 5
        Cut_Grip = 6
        Parting = 7
        Drilling = 8
        S_C_Endmills = 9
        Milling = 10
        Turning = 11
        Katia_Dept = 12
        MTB = 13
        ISCAR_GMBH_Employee = 14
        ISCAR_GMBH_SalesMan = 15
        ISCAR_GMBH_Dealer = 16
        Administrator_ReadOnly = 17
        ISCAR_Austria_Dealer = 18  ''ISCAR OSTR    IA
        ISCAR_SWISS_SalesMan = 19
        ISCAR_FRANCE_SalesMan = 20
        ISCAR_GMBH_Administrator = 21
        ISCAR_Dealer = 22
        '************************'************************'************************'************************
        '************************FOR ANY CHANGES MUST CHECK USP_GetUsersList & USP_GetModels PROCEDURE'************************
        '************************'************************'************************'************************

    End Enum


#End Region

#Region "Sub's"
    Public Shared Function GetValue(ByVal Key As Keys, Optional ByVal Encrypted As Boolean = True) As String
        Try


            'If Encrypted Then
            '    Return CryptoManager.Decode(HttpContext.Current.Request.Cookies(Key.ToString).Value)
            'Else
            '    Return HttpContext.Current.Request.Cookies(Key.ToString).Value
            'End If

             Dim CheckKey As String = GetKeyFor(Key.ToString)
            If Key = StateManager.Keys.s_BranchCode AndAlso HttpContext.Current.Session("State_" & CheckKey.ToString) = "" Then
                If HttpContext.Current.Session("State_" & CheckKey.ToString) = "" Then
                    Return "ZZ"
                End If
            Else
                If Not HttpContext.Current Is Nothing Then
                    If HttpContext.Current.Session("State_" & CheckKey.ToString) Is Nothing Then
                        Return ""
                    Else
                        'If Key = StateManager.Keys.s_BranchCode AndAlso HttpContext.Current.Session("State_" & Key.ToString) = "" Then
                        '    If HttpContext.Current.Session("State_" & Key.ToString) = "" Then
                        '        Return "ZZ"
                        '    End If
                        'End If

                        If Key = StateManager.Keys.s_CustomerNumber AndAlso HttpContext.Current.Session("State_" & CheckKey.ToString) = "" Then
                            If HttpContext.Current.Session("State_" & CheckKey.ToString) = "" Then
                                Return "99991"
                            End If
                        End If

                        If HttpContext.Current.Session("State_" & CheckKey.ToString) Is Nothing Then
                            Return ""
                        Else
                            'Return HttpContext.Current.Session("State_" & Key.ToString)
                            Return CryptoManagerTDES.Decode(HttpContext.Current.Session("State_" & CheckKey.ToString))
                        End If
                    End If
                Else
                    Return ""
                End If
            End If

            Return ""
        Catch ex As Exception

        End Try

    End Function

    Private Shared Function GetKeyFor(Key As String) As String
        Try
            If Key <> "" Then
                Dim uniqid As String = HttpContext.Current.Request("rErepTr")
                If uniqid <> "" Then
                    Key = Key & uniqid
                    Return Key
                Else
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationOpenType, "")
                    Return Key
                End If
            Else
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationOpenType, "")
                Return Key
            End If
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationOpenType, "")
            Return Key

        Catch ex As Exception
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationOpenType, "")
            Return Key
        End Try
        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationOpenType, "")
        Return Key

    End Function

    Public Shared Sub SetValue(ByVal Key As Keys, ByVal Value As String, Optional ByVal Encrypted As Boolean = True)
        'Dim uID As String = HttpContext.Current.Request("uID")

        'If Encrypted Then
        '    HttpContext.Current.Response.Cookies.Add(New HttpCookie(Key.ToString, CryptoManager.Encode(Value)))
        'Else
        '    HttpContext.Current.Response.Cookies.Add(New HttpCookie(Key.ToString, Value))
        'End If
        HttpContext.Current.Session("State_" & GetKeyFor(Key.ToString).ToString) = CryptoManagerTDES.Encode(Value) ' Value
        'HttpContext.Current.Session(uID & "State_" & Key.ToString) = Value
    End Sub
    Public Shared Sub SetValueG(ByVal Key As Keys, ByVal Value As String)

        'HttpContext.Current.Response.Cookies.Add(New HttpCookie(Key.ToString, Value))
        HttpContext.Current.Response.Cookies.Add(New HttpCookie(Key.ToString, CryptoManagerTDES.Encode(Value)))

    End Sub

    Public Shared Function GetValueG(ByVal Key As Keys) As String
        Try


            If HttpContext.Current.Request.Cookies(GetKeyFor(Key.ToString).ToString) Is Nothing Then
                Return ""
            Else
                'Return HttpContext.Current.Request.Cookies(Key.ToString).Value
                Return CryptoManagerTDES.Decode(HttpContext.Current.Request.Cookies(GetKeyFor(Key.ToString).ToString).Value)

            End If
        Catch ex As Exception

        End Try
    End Function
    'Public Sub Clearcookie()
    '    'setcookie ("NSC_TMAA", "", time() - 3600, "/", ".iscar.com", 1);
    '    'HttpContext.Current.Response.Cookies.Se()
    '    'For Each strKey In Request.QueryString
    '    '    Response.Cookies(strKey) = Request.QueryString(strKey)
    '    'Next
    'End Sub


    'Public Shared Function GetValue(ByVal Key As Keys, Optional ByVal Encrypted As Boolean = False) As String
    '    If Encrypted Then
    '        Return CryptoManager.Decode(HttpContext.Current.Session(Key))
    '    Else : Return HttpContext.Current.Session(Key)
    '    End If
    'End Function

    'Public Shared Sub SetValue(ByVal Key As Keys, ByVal Value As String, Optional ByVal Encrypted As Boolean = False)
    '    If Encrypted Then
    '        HttpContext.Current.Session(Key) = CryptoManager.Encode(Value)
    '    Else : HttpContext.Current.Session(Key) = Value
    '    End If
    'End Sub



#End Region

    Public Shared Sub Clear_State(ClearOnlyIfSesstionIsNothing As Boolean)
        Try

            'SessionManager.Clear_ALLSessions_OpenIquote()


            If ClearOnlyIfSesstionIsNothing = False Then
                SessionManager.Clear_Sessions_ShowCustomerType()

                StateManager.SetValue(StateManager.Keys.s_BranchCode, "")
                StateManager.SetValue(StateManager.Keys.s_loggedEmail, "")
                'CookiesManager.SetValue(CookiesManager.Keys.HkhtrycdFg, "", True)
                StateManager.SetValue(StateManager.Keys.s_DisplayName, "")
                StateManager.SetValue(StateManager.Keys.s_Surname, "")
                StateManager.SetValue(StateManager.Keys.s_GivenName, "")
                StateManager.SetValue(StateManager.Keys.s_Country, "")
                StateManager.SetValue(StateManager.Keys.s_CompanyName, "")
                StateManager.SetValue(StateManager.Keys.s_CustomerName, "")
                StateManager.SetValue(StateManager.Keys.s_CustomerNumber, "")
                StateManager.SetValue(StateManager.Keys.s_CustomerAddress, "")
                StateManager.SetValue(StateManager.Keys.s_shipmethod, "")
                StateManager.SetValue(StateManager.Keys.s_CustomerType, "")
                StateManager.SetValue(StateManager.Keys.s_paymentTerms, "")
                StateManager.SetValue(StateManager.Keys.s_STNCustomerCurrency, "ZZZ")
                StateManager.SetValue(StateManager.Keys.s_IsAdmin, "")
                StateManager.SetValue(StateManager.Keys.s_Culture, "")
                StateManager.SetValue(StateManager.Keys.s_TemporarilyBranchCode, "")

                StateManager.SetValue(StateManager.Keys.s_salesperson, "")
                StateManager.SetValue(StateManager.Keys.s_salespersonEmail, "")
                StateManager.SetValue(StateManager.Keys.s_deskUser, "")
                StateManager.SetValue(StateManager.Keys.s_deskUserEmail, "")
                StateManager.SetValue(StateManager.Keys.s_technicalPerson, "")
                StateManager.SetValue(StateManager.Keys.s_technicalPersonEmail, "")
                'StateManager.SetValue(StateManager.Keys.TemporarilyQuotation_User, "")


                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.selectedReportLanguage, "")
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.selectedLanguage, "")
                '    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.qut_LanguageId, "0")

                clsUser.SetIsUserAdmin()

            Else
                If StateManager.GetValue(StateManager.Keys.s_BranchCode, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_BranchCode, "")
                If StateManager.GetValue(StateManager.Keys.s_loggedEmail, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_loggedEmail, "")
                'If StateManager.GetValue(StateManager.Keys.TemporarilyQuotation_User, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.TemporarilyQuotation_User, "")
                If StateManager.GetValue(StateManager.Keys.s_DisplayName, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_DisplayName, "")
                If StateManager.GetValue(StateManager.Keys.s_GivenName, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_Surname, "")
                If StateManager.GetValue(StateManager.Keys.s_BranchCode, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_GivenName, "")
                If StateManager.GetValue(StateManager.Keys.s_Country, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_Country, "")
                If StateManager.GetValue(StateManager.Keys.s_CompanyName, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_CompanyName, "")
                If StateManager.GetValue(StateManager.Keys.s_CustomerName, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_CustomerName, "")
                If StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_CustomerNumber, "")
                If StateManager.GetValue(StateManager.Keys.s_CustomerAddress, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_CustomerAddress, "")
                If StateManager.GetValue(StateManager.Keys.s_paymentTerms, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_paymentTerms, "")
                If StateManager.GetValue(StateManager.Keys.s_shipmethod, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_shipmethod, "")

                If StateManager.GetValue(StateManager.Keys.s_salesperson, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_salesperson, "")
                If StateManager.GetValue(StateManager.Keys.s_salespersonEmail, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_salespersonEmail, "")
                If StateManager.GetValue(StateManager.Keys.s_deskUser, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_deskUser, "")
                If StateManager.GetValue(StateManager.Keys.s_deskUserEmail, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_deskUserEmail, "")
                If StateManager.GetValue(StateManager.Keys.s_technicalPerson, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_technicalPerson, "")
                If StateManager.GetValue(StateManager.Keys.s_technicalPersonEmail, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_technicalPersonEmail, "")

                If StateManager.GetValue(StateManager.Keys.s_CustomerType, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_CustomerType, "")
                If StateManager.GetValue(StateManager.Keys.s_STNCustomerCurrency, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_STNCustomerCurrency, "ZZZ")
                If StateManager.GetValue(StateManager.Keys.s_IsAdmin, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_IsAdmin, "")
                StateManager.SetValue(StateManager.Keys.s_IsAdmin, "")
                If StateManager.GetValue(StateManager.Keys.s_Culture, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_Culture, "")


                If StateManager.GetValue(StateManager.Keys.s_TemporarilyBranchCode, True) Is Nothing Then StateManager.SetValue(StateManager.Keys.s_TemporarilyBranchCode, "")

                clsUser.SetIsUserAdmin()
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub



    Public Shared Sub SetNewQuotationStateTemporarily(table As DataTable, bctoGetDetails As String)

    End Sub
    Public Shared Sub SetNewQuotationState(table As DataTable, bctoGetDetails As String, sGcForConnectLanguage As String)

        Try
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.BranchCode, bctoGetDetails)
            StateManager.SetValue(StateManager.Keys.s_BranchCode, bctoGetDetails)
            StateManager.SetValue(StateManager.Keys.s_TemporarilyBranchCode, table.Rows(0).Item("branchcode").ToString.Trim)
            StateManager.SetValue(StateManager.Keys.s_loggedEmail, table.Rows(0).Item("loggedEmail").ToString.Trim)
            'CookiesManager.SetValue(CookiesManager.Keys.HkhtrycdFg, table.Rows(0).Item("loggedEmail").ToString.Trim, True)
            StateManager.SetValue(StateManager.Keys.s_Surname, table.Rows(0).Item("surname").ToString.Trim)
            StateManager.SetValue(StateManager.Keys.s_DisplayName, table.Rows(0).Item("displayname").ToString.Trim)

            Dim dtb As DataTable = clsBranch.GetBranchDetails(bctoGetDetails)
            If Not dtb Is Nothing AndAlso dtb.Rows.Count > 0 AndAlso bctoGetDetails <> "ZZ" Then
                SessionManager.SetSessionDetails_Temporarily("FALSE")
                StateManager.SetValue(StateManager.Keys.s_GivenName, table.Rows(0).Item("givenName").ToString.Trim)
                StateManager.SetValue(StateManager.Keys.s_Country, table.Rows(0).Item("country").ToString.Trim)
                StateManager.SetValue(StateManager.Keys.s_CompanyName, table.Rows(0).Item("companyName").ToString.Trim)
                StateManager.SetValue(StateManager.Keys.s_CustomerNumber, table.Rows(0).Item("galCustomerNumber").ToString.Trim)
                StateManager.SetValue(StateManager.Keys.s_CustomerName, table.Rows(0).Item("companyName").ToString.Trim)
                StateManager.SetValue(StateManager.Keys.s_Culture, dtb.Rows(0).Item("culture").ToString.Trim)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.qut_LanguageId, dtb.Rows(0).Item("languageid").ToString.Trim)
            Else
                SessionManager.SetSessionDetails_Temporarily("TRUE")
                StateManager.SetValue(StateManager.Keys.s_GivenName, "")
                StateManager.SetValue(StateManager.Keys.s_Country, "")
                StateManager.SetValue(StateManager.Keys.s_CompanyName, "")
                StateManager.SetValue(StateManager.Keys.s_CustomerNumber, 0)
                StateManager.SetValue(StateManager.Keys.s_CustomerName, "")
                StateManager.SetValue(StateManager.Keys.s_Culture, "")
                Dim l As String = "0"
                Try
                    If sGcForConnectLanguage <> "" AndAlso sGcForConnectLanguage <> "ZZ" AndAlso bctoGetDetails <> sGcForConnectLanguage Then
                        Dim dtbForConnectLang As DataTable = clsBranch.GetBranchDetails(sGcForConnectLanguage)
                        If Not dtbForConnectLang Is Nothing AndAlso dtbForConnectLang.Rows.Count > 0 Then
                            l = dtbForConnectLang.Rows(0).Item("languageid").ToString.Trim
                        End If
                    End If
                Catch ex As Exception
                    l = "0"
                End Try


                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.qut_LanguageId, l)
            End If


            StateManager.SetValue(StateManager.Keys.s_IsAdmin, "")
            clsUser.SetIsUserAdmin()



            Try
                clsQuatation.GetCustomerAccountDetails(table.Rows(0).Item("galCustomerNumber").ToString, table.Rows(0).Item("branchCode").ToString)
            Catch ex As Exception
                'GeneralException.WriteEventErrors("BOB ERROR:" & ex.Message, GeneralException.e_LogTitle.GENERAL.ToString)
                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "SetNewQuotationState", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            End Try


            ''Check cookie user info 2
            'If ConfigurationManager.AppSettings("DoCheckCookie") = "LOCAL" Then

            '    CookiesManager.SetValue(CookiesManager.Keys._C_Id, table.Rows(0).Item("Id").ToString, False)
            '    CookiesManager.SetValue(CookiesManager.Keys._C_BranchCode, table.Rows(0).Item("branchCode").ToString, False)
            '    CookiesManager.SetValue(CookiesManager.Keys._C_loggedEmail, table.Rows(0).Item("loggedEmail").ToString, False)
            '    CookiesManager.SetValue(CookiesManager.Keys.HkhtrycdFg, table.Rows(0).Item("loggedEmail").ToString, False)
            '    CookiesManager.SetValue(CookiesManager.Keys._C_DisplayName, table.Rows(0).Item("displayname").ToString, False)
            '    CookiesManager.SetValue(CookiesManager.Keys._C_Surname, table.Rows(0).Item("surname").ToString, False)
            '    CookiesManager.SetValue(CookiesManager.Keys._C_GivenName, table.Rows(0).Item("givenName").ToString, False)
            '    CookiesManager.SetValue(CookiesManager.Keys._C_Country, table.Rows(0).Item("country").ToString, False)
            '    CookiesManager.SetValue(CookiesManager.Keys._C_CompanyName, table.Rows(0).Item("companyName").ToString, False)
            '    CookiesManager.SetValue(CookiesManager.Keys._C_CustomerNumber, table.Rows(0).Item("galCustomerNumber").ToString, False)
            '    CookiesManager.SetValue(CookiesManager.Keys._C_CustomerName, table.Rows(0).Item("galCustomerNumber").ToString, False)
            '    CookiesManager.SetValue(CookiesManager.Keys._C_Culture, table.Rows(0).Item("companyName").ToString, False)
            'End If

        Catch ex As Exception
            Throw
        End Try

    End Sub
End Class
