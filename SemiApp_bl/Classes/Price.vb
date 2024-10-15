Imports System.Configuration

Public Class Price

    Public Enum e_PriceFlag
        TFR = 1
        MKT = 2
    End Enum


    Public Shared Sub Get_TFR_NET_Price(BranchCodeAPITefen As String)
        Try
            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerPrice, False) Is Nothing Or SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerPrice, False) = "" Or
                SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TFR_STNBranchPrice, False) Is Nothing Or SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TFR_STNBranchPrice, False) = "" Then

                Dim ItemNumber As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber).ToString
                Dim cust As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)

                Dim _BranchCode As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)

                Dim bn As String = clsBranch.GetBranchNumberCode(_BranchCode)
                If _BranchCode = "IS" Or _BranchCode = "XZ" Then
                    bn = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
                End If

                Dim dtC As DataTable = GAL.GetGalData("PRICE", "IS", ConfigurationManager.AppSettings("AS400APPpathForGetData"), bn, ItemNumber, "", "", "", "", "", "", True)
                If Not dtC Is Nothing AndAlso dtC.Rows.Count > 0 Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchPrice, dtC.Rows(0).Item("netPrice"))
                End If

                Dim dt As DataTable = GAL.GetGalData("PRICE", _BranchCode, ConfigurationManager.AppSettings("AS400APPpathForGetData"), cust, ItemNumber, "", "", "", "", "", "", True)
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerPrice, dt.Rows(0).Item("netPrice"))
                End If



            End If

        Catch ex As Exception
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchPrice, 0)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerPrice, 0)
        End Try
    End Sub

    Public Shared Sub Set_ParamsFactors()
        Try
            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._TempdtFactorsWithValues, "") Is Nothing Then
                Dim ssFamily As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.familyNo, False)

                Dim dtFactorsWithValu As New DataTable
                Dim dtFac As DataTable = Nothing
                Dim dt_Param As DataTable = Nothing
                If dtFac Is Nothing Then
                    Dim sStart As String = clsQuatation.ACTIVE_OpenType.ToString



                    If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                        Dim _ModelNumConfiguration As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumConfiguration, False)
                        dtFac = clsPrice.GetModel_Factors(Integer.Parse(_ModelNumConfiguration))
                        Dim _dtParamListConfiguration As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListConfiguration, "")
                        dt_Param = _dtParamListConfiguration
                        dtFac.Columns.Add("MetCondition")
                    ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                        Dim _ModelNumModification As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumModification, False)
                        dtFac = clsPrice.GetModel_Factors(Integer.Parse(_ModelNumModification))
                        Dim _dtParamListModification As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListModification, "")
                        dt_Param = _dtParamListModification
                        dtFac.Columns.Add("MetCondition")
                    End If

                    Dim dv As DataView = dtFac.DefaultView

                    For ii As Integer = dv.Count - 1 To 0 Step -1
                        Dim ssAll As String = dv(ii).Item("FactorParam").ToString & dv(ii).Item("Condition1").ToString & dv(ii).Item("Condition2").ToString & dv(ii).Item("Condition3").ToString
                        If dv(ii).Item("FactorParam").ToString.Trim.ToUpper = ("CORNER DESIGN") And Not ssAll.Contains(ssFamily) Then
                            dv.Delete(ii)
                        End If

                    Next
                    dtFac = dv.ToTable
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParametersFactors, dtFac)

                    If Not dtFac Is Nothing AndAlso dtFac.Rows.Count > 0 Then
                        If Not dt_Param Is Nothing AndAlso dt_Param.Rows.Count > 0 Then

                            Dim dc_FactorName As New DataColumn
                            Dim dc_FactorValue As New DataColumn
                            Dim dc_FactorOrderSeq As New DataColumn("dc_FactorOrderSeq", GetType(System.Int32))
                            dc_FactorName.ColumnName = "dc_FactorName"
                            dc_FactorValue.ColumnName = "dc_FactorValue"
                            dc_FactorOrderSeq.ColumnName = "dc_FactorOrderSeq"
                            dtFactorsWithValu.Columns.Add(dc_FactorName)
                            dtFactorsWithValu.Columns.Add(dc_FactorValue)
                            dtFactorsWithValu.Columns.Add(dc_FactorOrderSeq)
                            Dim f As String = ""
                            For Each rF As DataRow In dtFac.Rows
                                Try

                                    f = 0

                                    Dim nFormula As String = ""
                                    If rF.Item("Condition1").ToString.Trim() = "" Then
                                        nFormula = 1
                                    Else
                                        nFormula = (FormulaResult.Conver_STR_Formula(rF.Item("Condition1").ToString.Trim() & " " & rF.Item("Condition2").ToString.Trim() & " " & rF.Item("Condition3").ToString.Trim(), dt_Param))
                                    End If

                                    Dim cust As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
                                    Dim oFormula As New FormulaResult(nFormula, dt_Param, 0, Nothing)
                                    If oFormula.ToString.Trim <> "" Then

                                        Dim drWithVal As DataRow = dtFactorsWithValu.NewRow

                                        oFormula.CustomerNumber = cust
                                        oFormula.Quantity = Nothing
                                        Dim res1 As String = oFormula.ParseAndCalculate
                                        drWithVal.Item("dc_FactorName") = rF.Item("FactorParam").ToString.Trim
                                        drWithVal.Item("dc_FactorOrderSeq") = rF.Item("OrderSeq").ToString.Trim

                                        If res1 = 1 Then
                                            rF.Item("MetCondition") = True

                                            Select Case rF.Item("FactorType").ToString.Trim
                                                Case "%"
                                                    If IsNumeric(rF.Item("FactorValue").ToString) Then
                                                        f = rF.Item("FactorValue") / 100
                                                    Else
                                                        f = rF.Item("FactorValue").ToString
                                                    End If
                                                Case Else
                                                    f = rF.Item("FactorValue")
                                            End Select

                                            If rF.Item("Manipulation").ToString.Trim() <> "" Then
                                                Dim m As String = rF.Item("Manipulation").ToString.Trim()

                                                m = (FormulaResult.Conver_STR_Formula(m, dt_Param))


                                                Dim oFormulaN As New FormulaResult(m, dt_Param, 0, Nothing)
                                                If oFormulaN.ToString.Trim <> "" Then
                                                    Dim res2 As String = oFormulaN.ParseAndCalculate
                                                    If IsNumeric(res2) Then
                                                        f = CDbl(f) * CDbl(res2)
                                                    End If
                                                End If
                                            End If

                                        End If
                                        If rF.Item("FactorValue").ToString = "" Then
                                            f = "0"
                                        End If

                                        drWithVal.Item("dc_FactorValue") = f

                                        dtFactorsWithValu.Rows.Add(drWithVal)

                                    End If
                                    oFormula = Nothing

                                Catch ex As Exception

                                    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "Set_ParamsFactors", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

                                    Throw
                                End Try
                            Next
                            dtFactorsWithValu.DefaultView.Sort = "dc_FactorValue DESC"
                            dtFactorsWithValu = dtFactorsWithValu.DefaultView.ToTable

                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempdtFactorsWithValues, dtFactorsWithValu)

                            dc_FactorName = Nothing
                            dc_FactorValue = Nothing
                        End If
                    End If

                End If

                dtFactorsWithValu = Nothing

            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Shared Function RemoveApostrophesQ(mainFormula As String) As String
        Try
            If mainFormula.Contains("""") Then
                mainFormula = mainFormula.Replace("""", "")
                Return mainFormula
            Else
                Return mainFormula
            End If

        Catch ex As Exception
            Throw
        End Try
        Return ""
    End Function

    Public Shared Sub SetSessionFormula(dt As DataTable)
        Try

            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then

                Dim sD As String = dt.Rows(0).Item("DescriptionFormula").ToString
                Dim sC As String = dt.Rows(0).Item("CostFormula").ToString
                Dim sG As String = dt.Rows(0).Item("GPFormula").ToString

                Dim s As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceCalculateFlag, False)
                If s = "" Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceCalculateFlag, dt.Rows(0).Item("PriceCalculateFlag").ToString)
                End If


                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormula_MKT, dt.Rows(0).Item("PriceFormulaMKT").ToString)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormulaTFR, dt.Rows(0).Item("PriceFormulaTFR").ToString)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BranchPriceFormulaMKT, dt.Rows(0).Item("BranchPriceFormulaMKT").ToString)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BranchPriceFormulaTFR, dt.Rows(0).Item("BranchPriceFormulaTFR").ToString)


                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DescriptionFormulaWithParamsNames, sD)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._CostFormula, sC)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._GPFormula, sG)

            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub Get_PriceFormula(FamilyProperties As DataTable)

        Dim _PriceFormulaMKT As String = ""
        Dim _PriceFormulaTFR As String = ""
        Dim _BranchPriceFormulaMKT As String = ""
        Dim _BranchPriceFormulaTFR As String = ""
        Try

            _PriceFormulaMKT = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormula_MKT, False)
            _PriceFormulaTFR = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormulaTFR, False)
            _BranchPriceFormulaMKT = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BranchPriceFormulaMKT, False)
            _BranchPriceFormulaTFR = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BranchPriceFormulaTFR, False)

            If Not FamilyProperties Is Nothing AndAlso FamilyProperties.Rows.Count > 0 Then

                If _PriceFormulaMKT = "" Then
                    _PriceFormulaMKT = FamilyProperties.Rows(0).Item("PriceFormulaMKT").ToString.Trim
                End If
                If _PriceFormulaTFR = "" Then
                    _PriceFormulaTFR = FamilyProperties.Rows(0).Item("PriceFormulaTFR").ToString.Trim
                End If
                If _BranchPriceFormulaMKT = "" Then
                    _BranchPriceFormulaMKT = FamilyProperties.Rows(0).Item("BranchPriceFormulaMKT").ToString.Trim
                End If
                If _BranchPriceFormulaTFR = "" Then
                    _BranchPriceFormulaTFR = FamilyProperties.Rows(0).Item("BranchPriceFormulaTFR").ToString.Trim
                End If

            End If

        Catch ex As Exception

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "Get_PriceFormula_PriceFormulaMKT", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "Get_PriceFormula_PriceFormulaTFR", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "Get_PriceFormula_BranchPriceFormulaMKT", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "Get_PriceFormula_BranchPriceFormulaTFR", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        End Try
    End Sub


    Public Shared Sub GetDelivery_ValidTime(dt As DataTable)
        Try
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Delivery_Weeks, dt.Rows(0).Item("Delivery_Weeks").ToString)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.DeliveryRad_Weeks, dt.Rows(0).Item("DeliveryRad_Weeks").ToString)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ValidTime_Weeks, dt.Rows(0).Item("ValidTime_Weeks").ToString)
            Else
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Delivery_Weeks, "")
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.DeliveryRad_Weeks, "")
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ValidTime_Weeks, "")
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub GetPriceRates()
        Try
            Dim bc As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)
            Dim ToCur As String = ""



            'Dim FromCur As String = "USD"

            'ToCur = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TFR_STNBranchCurrency)
            'clsRates_Currency.Get_Rates(bc, "USD", ToCur, "RATETFRUSD")

            'ToCur = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerCurrency)
            ToCur = StateManager.GetValue(StateManager.Keys.s_STNCustomerCurrency, True)
            clsRates_Currency.Get_Rates(bc, "USD", ToCur, "RATEMKTUSD")

            If bc.ToUpper = "IS" Or bc.ToUpper = "XZ" Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.RateTFR_USD, SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.RateMKT_USD, False))
            Else
                ToCur = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TFR_STNBranchCurrency)
                clsRates_Currency.Get_Rates(bc, "USD", ToCur, "RATETFRUSD")
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

End Class
