Imports IscarDal


Public Class clsUser

    Public Enum e_UserType
        AdminALL = 0
        BranchAdmin = 1
        RegularUser = 2
    End Enum
    Public Shared Function GetUserType(BranchCode As String, loggedEmail As String, ByRef UserType As String) As DataTable
        Dim dt As DataTable = Nothing
        UserType = "2"
        Try
            Dim oSql As New SqlDal()
            Dim oParams As New SqlParams()

            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParams.Add(New SqlParam("@loggedEmail", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, loggedEmail, 50))

            dt = oSql.ExecuteSPReturnDT("USP_GetUserDetails", oParams)

            If Not dt Is Nothing Then
                If dt.Rows.Count > 0 Then
                    If dt.Rows(0).Item("CompanyName").ToString.Trim = "ALL" Then
                        UserType = "0"
                        Return dt
                    ElseIf dt.Rows(0).Item("CompanyName").ToString.Trim = BranchCode AndAlso dt.Rows(0).Item("BranchAdmin").ToString.toupper.Trim = "TRUE" Then
                        UserType = "1"
                        Return dt
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
        UserType = "2"
        Return dt
    End Function


    Public Shared Sub UpdateUserToUsersDomain(BranchCodeCurrent As String, loggedEmail As String, BranchCodeNew As String)
        Try
            Dim oSql As New SqlDal()
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@BranchCodeCurrent", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCodeCurrent, 2))
            oParams.Add(New SqlParam("@loggedEmail", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, loggedEmail, 50))
            oParams.Add(New SqlParam("@BranchCodeNew", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, BranchCodeNew, 50))

            oSql.ExecuteSP("USP_UpdateUserToUsersDomain", oParams)

        Catch ex As Exception
            Throw
        End Try
    End Sub


    Public Shared Function GetIsDomailAdmin(loggedEmail As String) As DataTable
        Try
            Dim oSql As New SqlDal()
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@loggedEmail", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, loggedEmail, 50))

            dt = oSql.ExecuteSPReturnDT("usp_IsDomailAdmin", oParams)
            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function



    Public Shared Function SetIsUserAdminAllOrBranch() As Boolean
        Try


            Dim oSql As New SqlDal()
                Dim oParams As New SqlParams()
                Dim dt As DataTable

            Dim loggedEmail As String = StateManager.GetValue(StateManager.Keys.s_loggedEmail, True)
            oParams.Add(New SqlParam("@loggedEmail", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, loggedEmail, 50))

            dt = oSql.ExecuteSPReturnDT("USP_IsAccountAdmin", oParams)
            If Not dt Is Nothing Then
                If dt.Rows.Count > 0 Then
                    If dt.Rows(0).Item("CompanyName").ToString.Trim.ToUpper <> "ALL" AndAlso dt.Rows(0).Item("branchadmin").ToString.Trim.ToUpper <> "1" Then
                        Return True
                    End If
                End If
            End If

            Return False

        Catch ex As Exception
            Return False
        End Try

    End Function


    Public Shared Function SetIsUserAdmin() As Boolean
        Try

            Dim loggedEmail As String = StateManager.GetValue(StateManager.Keys.s_loggedEmail, True)
            Dim DT_pER As DataTable = clsUser.GetIsDomailAdmin(loggedEmail)
            If Not DT_pER Is Nothing AndAlso DT_pER.Rows.Count > 0 Then
                If DT_pER.Rows(0).Item(0).ToString = "1" Then
                    StateManager.SetValue(StateManager.Keys.s_IsAdmin, "YES")
                    Return True
                End If
            End If
            StateManager.SetValue(StateManager.Keys.s_IsAdmin, "NO")
            Return False
        Catch ex As Exception
            StateManager.SetValue(StateManager.Keys.s_IsAdmin, "NO")
            Return False
        End Try



    End Function

    Public Shared Function GETLoggedUsers(ByVal BranchCode As String, ByVal loggedEmail As String, sAny As String) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 200))
            oParams.Add(New SqlParam("@loggedEmail", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, loggedEmail))
            If sAny.ToString.Trim <> "" Then
                oParams.Add(New SqlParam("@Any", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sAny.ToString.Trim))
            End If
            dt = oSql.ExecuteSPReturnDT("USP_GETLoggedUsers", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function ConnectUserToCustomer(ByVal BranchCode As String, ByVal loggedEmail As String, AS400_Cust As String) As String
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()

            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode))
            oParams.Add(New SqlParam("@loggedEmail", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, loggedEmail))
            oParams.Add(New SqlParam("@AS400_Cust", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, AS400_Cust))
            oParams.Add(New SqlParam("@NotExistUser", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInputOutput, "0"))

            oSql.ExecuteSP("USP_ConnectUserToCustomer", oParams)

            Return oParams.GetParameter("@NotExistUser").Value.ToString

        Catch ex As Exception
            Throw
        End Try
    End Function


    Public Shared Function AddUserToUsersDomain(ByVal BranchCode As String, ByVal loggedEmail As String, DisplayName As String, Surname As String, GivenName As String) As String
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()

            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode))
            oParams.Add(New SqlParam("@loggedEmail", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, loggedEmail))
            oParams.Add(New SqlParam("@DisplayName", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, DisplayName))
            oParams.Add(New SqlParam("@Surname", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, Surname))
            oParams.Add(New SqlParam("@GivenName", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, GivenName))
            oParams.Add(New SqlParam("@NotExistUser", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInputOutput, "0"))

            oSql.ExecuteSP("USP_AddUserToUsersDomain", oParams)

            Return oParams.GetParameter("@NotExistUser").Value.ToString

        Catch ex As Exception
            Throw
        End Try
    End Function

End Class
