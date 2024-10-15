Imports IscarDal

Public Class clsPrice

    Public Shared Function ConvertFromTRF2SelectedCurrency(ByVal price As Decimal, ByVal Rate As Decimal) As Decimal
        Try
            'TODO: check get rate from AS400 ( * or /)
            'Return Format(price * Rate, "#.00") 'Fixed from / to *, at 12/05/2010. see also USP_GetRates
            Return price * Rate 'Fixed from / to *, at 12/05/2010. see also USP_GetRates
        Catch ex As Exception
            Throw
        End Try
    End Function


    Public Shared Function GetModel_Factors(ModelNum As Integer) As DataTable

        Try

            Dim sFN As String = clsPrices.GetParametersForFactors(ModelNum, "FindFactor")
            Dim sF As String = clsPrices.GetParametersForFactors(ModelNum, "FindFactor")
            If sF <> "" Then
                sF = FormulaResult.Conver_InputLimit_Formula(sF, clsQuatation.GetActiveQuotation_DTparams)
                Dim oFormula As New FormulaResult(sF, clsQuatation.GetActiveQuotation_DTparams, 0, Nothing)
                If oFormula.ToString.Trim <> "" Then
                    Dim cust As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
                    oFormula.CustomerNumber = cust
                    oFormula.Quantity = Nothing
                    Dim res1 As String = oFormula.ParseAndCalculate

                    If res1 <> "" AndAlso IsNumeric(res1) Then
                        Dim Scale As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.vers, False)

                        Dim dt_Factors As DataTable = Nothing
                        Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
                        Dim Params As New SqlParams

                        Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 4))
                        Params.Add(New SqlParam("@FactorParamCode", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sFN, 25))
                        Params.Add(New SqlParam("@SearchValue", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, res1))
                        Params.Add(New SqlParam("@Scale", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Scale))
                        Dim sStart As String = clsQuatation.ACTIVE_OpenType
                        Dim sop As String = ""
                        If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                            sop = "Configuration"
                        ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                            sop = "Modification"
                        End If
                        Params.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sop))

                        dt_Factors = oSql.ExecuteSPReturnDT("USP_GetParametersFactors", Params)

                        Return dt_Factors

                    End If
                End If
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try

    End Function


    Public Shared Function GetFamilyProperties(FamilyNum As Integer) As DataTable

        Try
            Dim dt_PriceFormula As DataTable = Nothing
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim Params As New SqlParams

            Params.Add(New SqlParam("@FamilyNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, FamilyNum))
            dt_PriceFormula = oSql.ExecuteSPReturnDT("USP_GetFamilyProperties", Params)

            Return dt_PriceFormula

        Catch ex As Exception
            Throw
        End Try



    End Function


    Public Shared Function Get_DC(dtParams As DataTable) As String
        Dim _DC As String
        Try
            If Not dtParams Is Nothing Then
                Dim dr As DataRow() = dtParams.Select("CostName='DC'")
                _DC = dr(0)("Measure")


                Return _DC
            End If

        Catch ex As Exception
            Return ""
        End Try
    End Function



End Class