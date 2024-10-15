Imports IscarDal

Public Class clsReference

    Public Shared Function GetCatalogNumber(BranchCode As String, QuotationNumber As Integer, ModelNum As Integer, ParamRefrence As String, ParamValue As Decimal, FindString As String) As DataTable

        Try

            Dim dt As DataTable
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNumber))
            oParams.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum))
            oParams.Add(New SqlParam("@ParamRefrence", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, ParamRefrence, 50))
            oParams.Add(New SqlParam("@ParamValue", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, ParamValue))
            oParams.Add(New SqlParam("@FindString", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, FindString, 50))
            dt = oSql.ExecuteSPReturnDT("USP_FindStandardRefrenceItem", oParams)
            Return dt

        Catch ex As Exception
            Throw
        End Try
    End Function


    Public Shared Function FindStandardItemModification(ByVal FamilyNum As Integer, ByVal ParamReference As String, ParamValue As Decimal) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@FamilyNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, FamilyNum))
            oParams.Add(New SqlParam("@ParamReference", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, ParamReference))
            oParams.Add(New SqlParam("@ParamValue", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, ParamValue))

            dt = oSql.ExecuteSPReturnDT("USP_FindStandardItemModification", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function

End Class
