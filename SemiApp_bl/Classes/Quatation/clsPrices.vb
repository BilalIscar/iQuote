Imports IscarDal

Public Class clsPrices


    Public Shared Function GetEffectsParamQtyFactor(ModelNum As Integer) As DataTable
        Try


            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim Params As New SqlParams
            Dim dtFactors As DataTable

            Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 4))
            Params.Add(New SqlParam("@ParamManipulation", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, "FindQtyFactor"))

            Dim sStart As String = clsQuatation.ACTIVE_OpenType
            Dim sop As String = ""
            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                sop = "Configuration"
            ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                sop = "Modification"
            End If
            Params.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sop))

            'dtFactors = oSql.ExecuteSPReturnDT("[USP_GetParamsForQtyFactorAdmin]", Params)
            dtFactors = oSql.ExecuteSPReturnDT("[USP_GetParametersForFactors]", Params)

            Return dtFactors
        Catch ex As Exception
            Throw
        End Try
    End Function


    Public Shared Function GetPricesQty(FamilyNum As Integer) As DataTable

        Try
            Dim dt_PriceFormula As DataTable = Nothing
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim Params As New SqlParams

            'Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum))
            Params.Add(New SqlParam("ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, FamilyNum))
            dt_PriceFormula = oSql.ExecuteSPReturnDT("USP_GETModelQTY", Params)

            Return dt_PriceFormula

        Catch ex As Exception
            Throw
        End Try



    End Function


    Public Shared Function GetParametersForFactors(ModelNum As Integer, ParamManipulation As String) As String
        Try
            Dim dt_Factors As DataTable = Nothing
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim Params As New SqlParams

            Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 4))
            Params.Add(New SqlParam("@ParamManipulation", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, ParamManipulation, 50))

            Dim sStart As String = clsQuatation.ACTIVE_OpenType
            Dim sop As String = ""
            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                sop = "Configuration"
            ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                sop = "Modification"
            End If
            Params.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sop))

            dt_Factors = oSql.ExecuteSPReturnDT("USP_GetParametersForFactors", Params)
            If Not dt_Factors Is Nothing AndAlso dt_Factors.Rows.Count > 0 Then
                Return dt_Factors.Rows(0).Item(0).ToString.Trim
            End If
            Return ""

        Catch ex As Exception
            Throw
        End Try



    End Function
    Public Shared Function GetParametersForFactors(ModelNum As Integer, ParamManipulation As String, n As DataTable) As DataTable
        Try
            Dim dt_Factors As DataTable = Nothing
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim Params As New SqlParams

            Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 4))
            Params.Add(New SqlParam("@ParamManipulation", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, ParamManipulation, 50))
            Dim sStart As String = clsQuatation.ACTIVE_OpenType
            Dim sop As String = ""
            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                sop = "Configuration"
            ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                sop = "Modification"
            End If
            Params.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sop))
            dt_Factors = oSql.ExecuteSPReturnDT("USP_GetParametersForFactors", Params)
            If Not dt_Factors Is Nothing AndAlso dt_Factors.Rows.Count > 0 Then
                Return dt_Factors
            End If
            Return Nothing

        Catch ex As Exception
            Throw
        End Try



    End Function
End Class
