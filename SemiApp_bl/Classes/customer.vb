Imports IscarDal

Public Class customer



    Public Shared Function Get_CustomerNameNoList(ByVal BranchCode As String) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode))

            dt = oSql.ExecuteSPReturnDT("USP_GetCustomerNameNoList", oParams)
            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function Get_CustomerQuotationOptions(ByVal CustType As String) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@CustType", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, CustType))

            dt = oSql.ExecuteSPReturnDT("USP_GetCustomerQuotationOptions", oParams)
            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
