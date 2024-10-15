Imports IscarDal

Public Class clsAdminData


    Public Shared Function Get_Constants() As DataTable
        Try

            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim Params As New SqlParams
            Dim dtConstants As DataTable

            dtConstants = oSql.ExecuteSPReturnDT("USP_GetQuotationConstant", Params)

            Return dtConstants
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function Get_ConstantsQty() As DataTable
        Try

            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim Params As New SqlParams
            Dim dtConstants As DataTable

            dtConstants = oSql.ExecuteSPReturnDT("USP_GetQuotationConstantQty", Params)

            Return dtConstants
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function Get_ModelBranchGP() As DataTable
        Try

            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim Params As New SqlParams
            Dim dtModelBranchGP As DataTable

            dtModelBranchGP = oSql.ExecuteSPReturnDT("USP_GetModelBranchGP", Params)

            Return dtModelBranchGP
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub UpdateQuotationsConstants(ModelNum As Integer, ConstValue As Decimal, ConstName As String)
        Try

            Dim oTxSql As SqlDal
            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

            Dim oParamsD As New SqlParams()
            oParamsD.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum))
            oParamsD.Add(New SqlParam("@ConstValue", SqlParam.ParamType.ptFloat, SqlParam.ParamDirection.pdInput, ConstValue))
            oParamsD.Add(New SqlParam("@ConstName", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, ConstName))

            oTxSql.ExecuteSP("USP_UpdateQuotationConstant", oParamsD)

            oParamsD = Nothing
            oTxSql = Nothing

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Public Shared Sub UpdateQuotationsConstantsQTY(ModelNum As Integer, ConstValue As Decimal, ConstName As String, MaxQTY As Integer)
        Try

            Dim oTxSql As SqlDal
            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

            Dim oParamsD As New SqlParams()
            oParamsD.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum))
            oParamsD.Add(New SqlParam("@ConstValue", SqlParam.ParamType.ptFloat, SqlParam.ParamDirection.pdInput, ConstValue))
            oParamsD.Add(New SqlParam("@ConstName", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, ConstName))
            oParamsD.Add(New SqlParam("@MaxQTY", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, MaxQTY))

            oTxSql.ExecuteSP("USP_UpdateQuotationConstantQTY", oParamsD)

            oParamsD = Nothing
            oTxSql = Nothing

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Public Shared Sub UpdateModelBranchGP(ModelNum As Integer, BranchNumber As Integer, GPType As String, CustomerType As String, GPValue As Decimal, MaxQTY As Integer)
        Try

            Dim oTxSql As SqlDal
            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

            Dim oParamsD As New SqlParams()


            oParamsD.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum))
            oParamsD.Add(New SqlParam("@BranchNumber", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, BranchNumber))
            oParamsD.Add(New SqlParam("@GPType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, GPType))
            oParamsD.Add(New SqlParam("@CustomerType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, CustomerType))

            oParamsD.Add(New SqlParam("@GPValue", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, GPValue))
            oParamsD.Add(New SqlParam("@MaxQTY", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, MaxQTY))

            oTxSql.ExecuteSP("USP_UpdateModelBranchGP", oParamsD)

            oParamsD = Nothing
            oTxSql = Nothing

        Catch ex As Exception

            Throw
        End Try
    End Sub
End Class
