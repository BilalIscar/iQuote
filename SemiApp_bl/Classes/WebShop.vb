Imports IscarDal
Public Class WebShop

    Public Shared Function Get_imcLoginGetFCSByBranch(ByVal sID As String, BranchCode As String) As DataTable
        Try

            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@imcCompany", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sID))
            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, BranchCode))

            dt = oSql.ExecuteSPReturnDT("catalog.dbo.imcLoginGetFCSByBranch", oParams)
            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
