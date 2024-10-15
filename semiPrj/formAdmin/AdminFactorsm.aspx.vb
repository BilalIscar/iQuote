
Imports SemiApp_bl
Public Class AdminFactorsm

    Inherits System.Web.UI.Page

    Private dt_PriceCopy As New DataTable
    Private dt_ConstForTable As New DataTable

    Private Enum e_PricesGrid As Integer
        priceid = 0
        btnprice = 1
        QTY = 2
        NETPRICE = 3
        TOTAL = 4
        COSTPRICE = 5
        GP = 6
        DELIVERYWEEKS = 7
        TFRPrice = 8
        btnAddToCart = 9
        btnDELETE = 10
        OrderQuantity = 11
        QTYFct = 12



    End Enum

    Private Sub UnvisibleItems()
        gvFactorsV.Visible = False
        gvModelParametersCode.Visible = False
        gvCon.Visible = False
        gvQtyfactors.Visible = False
        gvFactors.Visible = False
        gvGen.Visible = False
        gvPrices.Visible = False
        gvGen1.Visible = False
        gvParams.Visible = False
        gvModelParametersCode.Visible = False

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        UnvisibleItems()

        If clsUser.SetIsUserAdmin = True Then

            gvFactorsV.Visible = True
            gvModelParametersCode.Visible = True
            gvCon.Visible = True
            gvQtyfactors.Visible = True
            gvFactors.Visible = True
            gvGen.Visible = True
            gvPrices.Visible = True
            gvGen1.Visible = True
            gvParams.Visible = True
            gvModelParametersCode.Visible = True
            Try

                Try
                    Try
                        ServerData.Text = "REMOTE_ADDR-" & Request.ServerVariables("REMOTE_ADDR") & " "
                        ServerData.Text &= "LOCAL_ADDR" & Request.ServerVariables("LOCAL_ADDR") & " "
                        ServerData.Text &= "SERVER_NAME" & Request.ServerVariables("SERVER_NAME") & " "
                        'ServerData.Text &= "REMOTE_ADDR:" & Request.ServerVariables("REMOTE_ADDR")
                        If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber, False) Is Nothing Then
                            lblItemNoCat.Text = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber, False).ToString

                        End If

                        If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.familyNo, False) Is Nothing Then
                            lblItemNoCat.Text &= " : " & SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.familyNo, False).ToString

                        End If
                    Catch ex As Exception

                    End Try
                Catch ex As Exception

                End Try
                If Not IsPostBack Then
                    Try
                        If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "") Is Nothing Then
                            dt_PriceCopy = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "").Copy

                        End If
                    Catch ex As Exception

                    End Try

                End If

                'Dim _PriceFormula As String = "" ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormula, False)

                Dim _PriceFormulaMKT As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormula_MKT, False)
                Dim _PriceFormulaMKT_N As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormulaMKT_Value, False)

                Dim _PriceFormulaTFR As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormulaTFR, False)
                Dim _PriceFormulaTFR_N As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormulaTFR_Value, False)

                Dim _PriceFormulaMKTBranch As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BranchPriceFormulaMKT, False)
                Dim _PriceFormulaMKTBranch_N As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BranchPriceFormulaMKT_Value, False)

                Dim _PriceFormulaTFRBranch As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BranchPriceFormulaTFR, False)
                Dim _PriceFormulaTFRBranch_N As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BranchPriceFormulaTFR_Value, False)


                Dim _CostFormula As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._CostFormula, False)
                Dim _GPFormula As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._GPFormula, False)
                Dim _ConstFormula As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ConstFormula, False)
                Dim P_FormulaNames As String = "" ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormulaWithParamsNames, False)

                lblFormulaMKT.Text = _PriceFormulaMKT_N
                lblFormulaMKTV.Text = _PriceFormulaMKT_N
                lblFormulaMKTV.Text = _PriceFormulaMKT_N
                lblFormulaMKT_B.Text = _PriceFormulaMKT
                lblFormulaMKT_BV.Text = _PriceFormulaMKT

                lblFormulaTFR.Text = _PriceFormulaTFR_N
                lblFormulaTFRV.Text = _PriceFormulaTFR_N
                lblFormulaTFR_B.Text = _PriceFormulaTFR
                lblFormulaTFR_BV.Text = _PriceFormulaTFR

                'lblFormulaPriceNet.Text = "<b>Calculate TFR = </b>" & _PriceFormulaTFR_N
                'lblFormulaPriceNetVal.Text = "<b>Calculate Price Net =</b><br> " & _PriceFormulaMKT_N

                lblFormulaMKTBRANCH.Text = _PriceFormulaMKTBranch_N
                lblFormulaMKTBRANCH_B.Text = _PriceFormulaMKTBranch
                lblFormulaTFRBRANCH.Text = _PriceFormulaTFRBranch_N
                lblFormulaTFRBRANCH_B.Text = _PriceFormulaTFRBranch
                lblFormulaTFRBRANCHV.Text = _PriceFormulaTFRBranch_N
                lblFormulaTFRBRANCH_BV.Text = _PriceFormulaTFRBranch
                lblFormulaMKTBRANCHV.Text = _PriceFormulaMKTBranch_N
                lblFormulaMKTBRANCH_BV.Text = _PriceFormulaMKTBranch

                lblCostFormula.Text = _CostFormula
                lblGP.Text = _GPFormula
                lblTFRMKT.Text = _ConstFormula

                'lblFormulaCon.Text = _PriceFormulaMKT



                Dim ss As String = _ConstFormula
                Dim dtC As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Consts, "")
                Dim s As String = ""
                If Not dtC Is Nothing AndAlso dtC.Rows.Count > 0 Then
                    dt_ConstForTable.Columns.Add("Name")
                    dt_ConstForTable.Columns.Add("Val")
                    dt_ConstForTable.Columns.Add("Description")
                    dt_ConstForTable.Rows.Add()
                    dt_ConstForTable.Rows(dt_ConstForTable.Rows.Count - 1).Item("Name") = ""
                    dt_ConstForTable.Rows(dt_ConstForTable.Rows.Count - 1).Item("Val") = ""
                    For Each r As DataRow In dtC.Rows
                        ss = ss.Replace("CONST(" & r.Item("ConstName").ToString & ")", r.Item("ConstValue").ToString)
                        If r.Item("ConstName").ToString = "SP_SETUP" Then
                            ss = ss.Replace("CONST(SP_SETUP)", r.Item("ConstValue").ToString)
                            ss = ss.Replace("CONST(SPSETUP)", r.Item("ConstValue").ToString)
                        End If
                        If r.Item("ConstName").ToString = "STN_SETUP" Then
                            ss = ss.Replace("CONST(STN_SETUP)", r.Item("ConstValue").ToString)
                            ss = ss.Replace("CONST(STNSETUP)", r.Item("ConstValue").ToString)
                        End If


                        s &= "[<b>" & r.Item("ConstName").ToString & "</b> : " & r.Item("ConstValue").ToString & "]----"
                        dt_ConstForTable.Rows.Add()
                        dt_ConstForTable.Rows(dt_ConstForTable.Rows.Count - 1).Item("Name") = r.Item("ConstName").ToString
                        dt_ConstForTable.Rows(dt_ConstForTable.Rows.Count - 1).Item("Val") = r.Item("ConstValue").ToString()
                    Next
                End If
                'lblFormulaConval.Text = "<b>QTYFct = </b>" & ss
                'lblConsFormula_B.Text = s

                Dim ModelNo As String = ""
                Dim sStart As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)
                If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                    ModelNo = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumConfiguration)
                ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                    ModelNo = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumModification)
                End If
                Dim dt_FamilyProperties As DataTable = Nothing
                If IsNumeric(ModelNo) Then
                    dt_FamilyProperties = clsPrice.GetFamilyProperties(CInt(ModelNo))

                End If
                If Not dt_FamilyProperties Is Nothing AndAlso dt_FamilyProperties.Rows.Count > 0 Then
                    lblDesc.Text = dt_FamilyProperties.Rows(0).Item("DescriptionFormula").ToString.Trim

                End If

                lblTFRMKT.Text = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceCalculateFlag, False)

                'lblRate.Text = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.RateTFR_USD, False)
                'lblItemCAT.Text = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber, False)
                'lblSTN.Text = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.STN, False)

                If lblTFRMKT.Text = "TFR" Then
                    lblFormulaMKTBRANCHV.Visible = False
                    lblFormulaMKTBRANCH_BV.Visible = False
                    lblFormulaMKTV.Visible = False
                    lblFormulaMKTV.Visible = False
                    lblFormulaMKT_BV.Visible = False
                    'Label1V.Visible = False
                    'Label2V.Visible = False
                End If
                If lblTFRMKT.Text = "MKT" Then
                    lblFormulaTFRV.Visible = False
                    lblFormulaTFR_BV.Visible = False
                    lblFormulaTFRBRANCHV.Visible = False
                    lblFormulaTFRBRANCH_BV.Visible = False
                    'lblfV.Visible = False
                    'Label1.Visible = False

                End If


                'Set_ParamsFactors(ModelNo)
                Dim _dtFactorsWithValues As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._TempdtFactorsWithValues, "")

                SetTempTabels(ModelNo, _dtFactorsWithValues)

                'Dim dt_PriceCopy As New DataTable
                'dt_PriceCopy = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "").Copy



                Dim dtQutFactors As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FactorsQty, "")

                'If dt_PriceCopy.Columns("QTYFct") Is Nothing Then
                '    dt_PriceCopy.Columns.Add("QTYFct")
                'End If
                If Not dt_PriceCopy Is Nothing AndAlso dt_PriceCopy.Rows.Count > 0 Then
                    'Try
                    '    For Each r1 As DataRow In dtQutFactors.Rows
                    '        For Each r2 As DataRow In dt_PriceCopy.Rows
                    '            If r1.Item("Qty").ToString = r2.Item("Qty").ToString Then
                    '                r2.Item("QTYFct") = r1.Item("FactorValue")
                    '                Exit For
                    '            End If
                    '        Next
                    '    Next
                    'Catch ex As Exception
                    'End Try


                    'dt_PriceCopy.Columns.Add(e_PricesGrid.QTYFct.ToString)
                    'For Each r As DataRow In dt_PriceCopy.Rows
                    '    Try
                    '        Dim res As String = ""
                    '        Dim oFormula As New FormulaResult(ss, CType(Nothing, DataTable), 0, Nothing)
                    '        oFormula.Quantity = r.Item(e_PricesGrid.QTY)
                    '        res = oFormula.ParseAndCalculate
                    '        r.Item(e_PricesGrid.QTYFct) = res
                    '    Catch ex As Exception
                    '        r.Item(e_PricesGrid.QTYFct) = ss
                    '    End Try


                    'Next
                    Dim bDpnCheck As Boolean = False
                    For Each r As DataRow In dt_PriceCopy.Rows
                        If bDpnCheck = False Then
                            Try
                                If (r.Item(e_PricesGrid.QTYFct).ToString = "" OrElse r.Item(e_PricesGrid.QTYFct).ToString = "0") AndAlso r.Item(e_PricesGrid.QTY).ToString <> "" Then
                                    Dim res As String = ""
                                    Dim oFormula As New FormulaResult(ss, CType(Nothing, DataTable), 0, Nothing)
                                    oFormula.Quantity = r.Item(e_PricesGrid.QTY)
                                    res = oFormula.ParseAndCalculate
                                    r.Item(e_PricesGrid.QTYFct) = res
                                Else
                                    bDpnCheck = True
                                End If
                            Catch ex As Exception
                                r.Item(e_PricesGrid.QTYFct) = ss
                        End Try
                        End If

                Next


                    gvPrices.DataSource = dt_PriceCopy
                    gvPrices.DataBind()


                End If
                Dim dtx As DataTable = clsQuatation.GetActiveQuotation_DTparams
                gvParams.DataSource = dtx
                gvParams.DataBind()




                'gvxxx.DataSource = clsQuatation.GetActiveQuotation_DTparams
                'gvxxx.DataBind()
                'gvFactors.DataSource = _dtFactorsWithValues
                'gvFactors.DataBind()

                fillGenGrid()

                'dt_PriceCopy = Nothing
            Catch ex As Exception
                UnvisibleItems()

            End Try
        Else

        End If


    End Sub
    Private Sub fillGenGrid()

        Dim dtG As New DataTable
        dtG.Columns.Add("Name")
        dtG.Columns.Add("Val")
        dtG.Columns.Add("Description")

        Try
            dtG.Rows.Add()
            dtG.Rows(dtG.Rows.Count - 1).Item("Name") = "Branch Code"
            dtG.Rows(dtG.Rows.Count - 1).Item("Val") = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)
            dtG.Rows.Add()
            dtG.Rows(dtG.Rows.Count - 1).Item("Name") = "Quotation Number"
            dtG.Rows(dtG.Rows.Count - 1).Item("Val") = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
            dtG.Rows.Add()
            dtG.Rows(dtG.Rows.Count - 1).Item("Name") = "Quotation AS400 Number"
            dtG.Rows(dtG.Rows.Count - 1).Item("Val") = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number, False)
            dtG.Rows.Add()
            dtG.Rows(dtG.Rows.Count - 1).Item("Name") = "Quotation AS 400 Line"
            dtG.Rows(dtG.Rows.Count - 1).Item("Val") = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400RowNumber, False)
            dtG.Rows.Add()
            dtG.Rows(dtG.Rows.Count - 1).Item("Name") = "Customer Name"
            'dtG.Rows(dtG.Rows.Count - 1).Item("Val") = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.CustomerName, False)
            dtG.Rows(dtG.Rows.Count - 1).Item("Val") = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.CustomerNumber, False)  'StateManager.GetValue(StateManager.Keys.s_CustomerName, True)
            dtG.Rows.Add()
            dtG.Rows(dtG.Rows.Count - 1).Item("Name") = "Customer Number"
            dtG.Rows(dtG.Rows.Count - 1).Item("Val") = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.CustomerName, False)  ' StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
            dtG.Rows.Add()
            dtG.Rows(dtG.Rows.Count - 1).Item("Name") = "Item No."
            dtG.Rows(dtG.Rows.Count - 1).Item("Val") = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber, False)
            dtG.Rows.Add()
            dtG.Rows(dtG.Rows.Count - 1).Item("Name") = "STN Customer Currency"
            dtG.Rows(dtG.Rows.Count - 1).Item("Val") = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerCurrency, False) ' StateManager.GetValue(StateManager.Keys.s_STNCustomerCurrency, True) 
            dtG.Rows.Add()
            dtG.Rows(dtG.Rows.Count - 1).Item("Name") = "STN Customer NET Price"
            dtG.Rows(dtG.Rows.Count - 1).Item("Val") = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerPrice, False)
            dtG.Rows(dtG.Rows.Count - 1).Item("Description") = "From Branch Customer Net Price (412)"
            dtG.Rows.Add()
            dtG.Rows(dtG.Rows.Count - 1).Item("Name") = "STN Branch TFR"
            dtG.Rows(dtG.Rows.Count - 1).Item("Val") = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TFR_STNBranchCurrency, False)
            dtG.Rows.Add()
            dtG.Rows(dtG.Rows.Count - 1).Item("Name") = "STN Branch TFR Price"
            dtG.Rows(dtG.Rows.Count - 1).Item("Val") = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TFR_STNBranchPrice, False)
            dtG.Rows(dtG.Rows.Count - 1).Item("Description") = "From Vendor (LTD) Transfer Price To Branch (312)"

            dtG.Rows.Add()
            dtG.Rows(dtG.Rows.Count - 1).Item("Name") = "Rate TFR-USD"
            dtG.Rows(dtG.Rows.Count - 1).Item("Val") = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.RateTFR_USD, False)
            dtG.Rows.Add()
            dtG.Rows(dtG.Rows.Count - 1).Item("Name") = "Rate MKT-USD"
            dtG.Rows(dtG.Rows.Count - 1).Item("Val") = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.RateMKT_USD, False)

            dtG.Rows.Add()
            dtG.Rows(dtG.Rows.Count - 1).Item("Name") = "DC"
            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._DC, False) = "" Then
                dtG.Rows(dtG.Rows.Count - 1).Item("Val") = clsPrice.Get_DC(clsQuatation.GetActiveQuotation_DTparams)
            Else
                dtG.Rows(dtG.Rows.Count - 1).Item("Val") = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._DC, False)
            End If
            dtG.Rows.Add()
            dtG.Rows.Add()
            dtG.Rows(dtG.Rows.Count - 1).Item("Name") = "Family GP"
            dtG.Rows(dtG.Rows.Count - 1).Item("Val") = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FAMILYGP, False)
            dtG.Rows(dtG.Rows.Count - 1).Item("Description") = "multiple 100 - Precentage"
            dtG.Rows.Add()
            dtG.Rows(dtG.Rows.Count - 1).Item("Name") = "SUBGP"
            dtG.Rows(dtG.Rows.Count - 1).Item("Val") = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.SUBGP, False)
            dtG.Rows(dtG.Rows.Count - 1).Item("Description") = "multiple 100 - Precentage"
            'dt_ConstForTable.Rows.Add()

            For Each rrr As DataRow In dt_ConstForTable.Rows
                dtG.Rows.Add()
                dtG.Rows(dtG.Rows.Count - 1).Item("name") = rrr("name")
                dtG.Rows(dtG.Rows.Count - 1).Item("val") = rrr("val")
                If rrr("name").ToString.ToUpper.Trim = "SP_SETUP" Or rrr("name").ToString.ToUpper.Trim = "STN_SETUP" Then
                    dtG.Rows(dtG.Rows.Count - 1).Item("Description") = "Minutes"
                ElseIf rrr("name").ToString.ToUpper.Trim = "BATCH" Then
                    dtG.Rows(dtG.Rows.Count - 1).Item("Description") = "PCS"
                ElseIf rrr("name").ToString.ToUpper.Trim = "GP" Then
                    dtG.Rows(dtG.Rows.Count - 1).Item("Description") = "Precentage"
                ElseIf rrr("name").ToString.ToUpper.Trim = "LBR" Then
                    dtG.Rows(dtG.Rows.Count - 1).Item("Description") = "USD"
                End If
            Next

            gvGen.DataSource = dtG
            gvGen.DataBind()
            gvGen1.DataSource = dtG
            gvGen1.DataBind()

            dt_ConstForTable = Nothing
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub
    Private Function SetSTNTEMPLabel() As String

    End Function





    Private Sub SetTempTabels(ModelNo As String, _dtFactorsWithVal As DataTable)
        Try

            Try
                Dim dtQutFactors As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FactorsQty, "")

                gvQtyfactors.DataSource = dtQutFactors
                gvQtyfactors.DataBind()

                Dim dtPFactors As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParametersFactors, "")

                gvFactors.DataSource = dtPFactors
                gvFactors.DataBind()

                Dim dtPFactorsPC As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtTQuotatioListModelParametersCode, "")

                gvModelParametersCode.DataSource = dtPFactorsPC
                gvModelParametersCode.DataBind()


                Dim gvFactorsVT As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._TempdtFactorsWithValues, "")


                Dim dtt As New DataTable
                If Not gvFactorsVT Is Nothing Then
                    dtt = gvFactorsVT.Copy
                    dtt.Rows.Clear()


                    Dim dv As DataView = gvFactorsVT.DefaultView
                    dv.Sort = "dc_FactorName, dc_FactorValue DESC"
                    For i As Integer = 0 To dv.Count - 1
                        If i > 0 Then
                            If dv(i).Item(0).ToString <> dv(i - 1).Item(0) Then
                                dtt.Rows.Add()
                                dtt.Rows(dtt.Rows.Count - 1).Item(0) = dv(i).Item(0).ToString
                                dtt.Rows(dtt.Rows.Count - 1).Item(1) = dv(i).Item(1).ToString
                                dtt.Rows(dtt.Rows.Count - 1).Item(2) = dv(i).Item(2).ToString
                            End If
                        Else
                            dtt.Rows.Add()
                            dtt.Rows(dtt.Rows.Count - 1).Item(0) = dv(i).Item(0).ToString
                            dtt.Rows(dtt.Rows.Count - 1).Item(1) = dv(i).Item(1).ToString
                            dtt.Rows(dtt.Rows.Count - 1).Item(2) = dv(i).Item(2).ToString
                        End If
                    Next

                    dtt.DefaultView.Sort = "dc_FactorOrderSeq "

                    gvFactorsV.DataSource = dtt
                    gvFactorsV.DataBind()
                End If


            Catch ex As Exception

            End Try





            Dim dtFac As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParametersFactors, "")
            Dim dtParamListT As DataTable = Nothing


            Dim dtEffectTD As DataTable = Nothing
            Dim sStart As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)
            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                dtEffectTD = clsPrices.GetEffectsParamQtyFactor(ModelNo)
                dtParamListT = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListConfiguration, "")
            ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                dtEffectTD = clsPrices.GetEffectsParamQtyFactor(ModelNo)
                dtParamListT = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListModification, "")
            End If

            If Not dtParamListT Is Nothing AndAlso dtParamListT.Rows.Count > 0 Then

                Dim dtF As New DataTable
                dtF.Columns.Add("paramcode")
                dtF.Columns.Add("pVal")

                If Not dtEffectTD Is Nothing AndAlso dtEffectTD.Rows.Count > 0 Then

                    Dim ParamValue1 As String = ""

                    For Each rr As DataRow In dtEffectTD.Rows
                        For Each rrr As DataRow In dtParamListT.Rows
                            If rrr.Item("Measure").ToString.Trim <> "" Then
                                If rrr.Item("CostName").ToString.Trim = rr.Item("paramcode").ToString.Trim.Replace("{", "").Replace("}", "") Then
                                    ParamValue1 = rrr.Item("Measure").ToString.Trim
                                    dtF.Rows.Add()
                                    dtF.Rows(dtF.Rows.Count - 1).Item("paramcode") = rrr.Item("CostName").ToString.Trim
                                    dtF.Rows(dtF.Rows.Count - 1).Item("pVal") = ParamValue1
                                End If

                            End If
                        Next
                    Next

                End If

                gvCon.DataSource = dtF
                gvCon.DataBind()
                dtF = Nothing

            End If

            Exit Sub

            If Not _dtFactorsWithVal Is Nothing AndAlso _dtFactorsWithVal.Rows.Count > 0 Then
                Dim dt_F As New DataTable
                dt_F = _dtFactorsWithVal.Copy
                dt_F.Columns.Add("STN")
                dt_F.Columns.Add("Selected")
                dt_F.Columns.Add("FactorCondition")
                dt_F.Columns.Add("Manipulation")

                If Not dt_F Is Nothing AndAlso dt_F.Rows.Count > 0 Then

                    Dim dtTemp As New DataTable




                    dtTemp = Get_ParamsGridVal()
                    If Not dtTemp Is Nothing AndAlso dtTemp.Rows.Count > 0 Then
                        For Each r As DataRow In dt_F.Rows
                            For Each cc As DataColumn In dtTemp.Columns
                                If r.Item("dc_FactorName").ToString.Trim = cc.ColumnName Then
                                    r.Item("STN") = dtTemp.Rows(0).Item(cc.ColumnName).ToString.Trim
                                    Exit For
                                End If
                            Next
                        Next
                        dtTemp = Nothing
                    End If



                    For Each r As DataRow In dt_F.Rows
                        For Each rrr As DataRow In dtFac.Rows
                            If r.Item("dc_FactorName").ToString.Trim = rrr.Item("FactorParam").ToString.Trim Then
                                r.Item("FactorCondition") = rrr.Item("Condition1").ToString.Trim
                                r.Item("Manipulation") = rrr.Item("Manipulation").ToString.Trim
                                Exit For
                            End If
                        Next
                    Next

                    For Each r As DataRow In dt_F.Rows
                        For Each rrr As DataRow In dtParamListT.Rows
                            If r.Item("dc_FactorName").ToString.Trim = rrr.Item("CostName").ToString.Trim Then
                                r.Item("Selected") = rrr.Item("Measure").ToString.Trim
                                Exit For
                            End If
                        Next
                    Next


                    gvFactors.DataSource = dt_F
                    gvFactors.DataBind()

                End If
            End If


        Catch ex As Exception

        End Try
    End Sub


    Private Function Get_ParamsGridVal() As DataTable
        Try
            Dim dtP As New DataTable
            Dim dtParamList As DataTable = Nothing

            Dim dt As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamsFromCatalog, "")

            Dim r_A As DataRow = dtP.NewRow
            dtP.Rows.Add(r_A)
            For Each r As DataRow In dt.Rows
                dtP.Columns.Add(r.Item("GIPRGP_ISO").ToString.Trim.Replace(" ", ""))
                dtP.Rows(0).Item(dtP.Columns.Count - 1) = r.Item("Val").ToString
            Next

            Return dtP

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)

        End Try
    End Function






    Private Sub gvPrices_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvPrices.RowDataBound
        Try
            Try
                If e.Row.RowType = DataControlRowType.DataRow Then

                    'Dim dt As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "")
                    If Not dt_PriceCopy Is Nothing AndAlso dt_PriceCopy.Rows.Count > 0 Then

                        If dt_PriceCopy.Rows(e.Row.RowIndex)(e_PricesGrid.QTY).ToString <> "" Then

                            CType(e.Row.FindControl("txtQuantity"), TextBox).Text = dt_PriceCopy.Rows(e.Row.RowIndex)(e_PricesGrid.QTY).ToString
                            CType(e.Row.FindControl("txtTotal"), TextBox).Text = dt_PriceCopy.Rows(e.Row.RowIndex)(e_PricesGrid.TOTAL).ToString
                            CType(e.Row.FindControl("txtDeliveryWeeks"), TextBox).Text = dt_PriceCopy.Rows(e.Row.RowIndex)(e_PricesGrid.DELIVERYWEEKS).ToString
                            If IsNumeric(dt_PriceCopy.Rows(e.Row.RowIndex)(e_PricesGrid.NETPRICE).ToString) Then
                                CType(e.Row.FindControl("txtNetPrice"), TextBox).Text = Format(CDbl(dt_PriceCopy.Rows(e.Row.RowIndex)(e_PricesGrid.NETPRICE)), "#.##")
                            Else
                                CType(e.Row.FindControl("txtNetPrice"), TextBox).Text = dt_PriceCopy.Rows(e.Row.RowIndex)(e_PricesGrid.NETPRICE).ToString
                            End If

                            If IsNumeric(dt_PriceCopy.Rows(e.Row.RowIndex)(e_PricesGrid.TFRPrice).ToString) Then
                                CType(e.Row.FindControl("txtTFRPrice"), TextBox).Text = Format(CDbl(dt_PriceCopy.Rows(e.Row.RowIndex)(e_PricesGrid.TFRPrice)), "#.##")
                            Else
                                CType(e.Row.FindControl("txtTFRPrice"), TextBox).Text = dt_PriceCopy.Rows(e.Row.RowIndex)(e_PricesGrid.TFRPrice).ToString
                            End If
                            If IsNumeric(dt_PriceCopy.Rows(e.Row.RowIndex)("QTYFct").ToString) Then
                                CType(e.Row.FindControl("txtQTYFct"), TextBox).Text = Format(CDbl(dt_PriceCopy.Rows(e.Row.RowIndex)("QTYFct")), "#.##")
                            Else
                                CType(e.Row.FindControl("txtQTYFct"), TextBox).Text = dt_PriceCopy.Rows(e.Row.RowIndex)("QTYFct").ToString
                            End If
                            If IsNumeric(dt_PriceCopy.Rows(e.Row.RowIndex)(e_PricesGrid.COSTPRICE).ToString) Then
                                CType(e.Row.FindControl("txtCostPrice"), TextBox).Text = Format(CDbl(dt_PriceCopy.Rows(e.Row.RowIndex)(e_PricesGrid.COSTPRICE)), "#.##")
                            Else
                                CType(e.Row.FindControl("txtCostPrice"), TextBox).Text = dt_PriceCopy.Rows(e.Row.RowIndex)(e_PricesGrid.COSTPRICE).ToString
                            End If


                            If IsNumeric(dt_PriceCopy.Rows(e.Row.RowIndex)("GP").ToString) Then
                                CType(e.Row.FindControl("txtGP"), TextBox).Text = Format(CDbl(dt_PriceCopy.Rows(e.Row.RowIndex)("GP")), "#.##")
                            Else
                                CType(e.Row.FindControl("txtGP"), TextBox).Text = dt_PriceCopy.Rows(e.Row.RowIndex)("GP").ToString
                            End If

                            If IsNumeric(dt_PriceCopy.Rows(e.Row.RowIndex)(e_PricesGrid.TOTAL)) Then
                                If IsNumeric(dt_PriceCopy.Rows(e.Row.RowIndex)(e_PricesGrid.NETPRICE).ToString) Then
                                    CType(e.Row.FindControl("txtTotal"), TextBox).Text = Format(CDbl(dt_PriceCopy.Rows(e.Row.RowIndex)(e_PricesGrid.TOTAL)), "#.##")
                                Else
                                    CType(e.Row.FindControl("txtTotal"), TextBox).Text = dt_PriceCopy.Rows(e.Row.RowIndex)(e_PricesGrid.TOTAL).ToString
                                End If
                            End If
                        End If

                    End If

                End If


            Catch ex As Exception
                GeneralException.BuildError(Page, ex.Message)
            End Try
        Catch ex As Exception

        End Try
    End Sub

    Private Sub gvCon_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvCon.RowDataBound, gvFactors.RowDataBound, gvGen.RowDataBound, gvGen1.RowDataBound, gvModelParametersCode.RowDataBound, gvParams.RowDataBound, gvPrices.RowDataBound, gvQtyfactors.RowDataBound
        Try


            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" & " this.style.backgroundColor='#00ffe8';")
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;")


            End If
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub AdminFactors_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        'Try
        '    CookiesManager.SetValue(CookiesManager.Keys.etoedfrku, "", True)
        '    Dim sLogEmail As String = clsQuatation.ACTIVE_UseLoggedEmail

        '    CookiesManager.SetValue(CookiesManager.Keys.etoedfrku, sLogEmail, True)
        'Catch ex As Exception

        'End Try
    End Sub

    Private Sub AdminFactorsm_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerCurrency, False) Is Nothing AndAlso Not gvPrices.HeaderRow Is Nothing Then
                gvPrices.HeaderRow.Cells(e_PricesGrid.NETPRICE).Text = "Net Price" & " (" & SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerCurrency, False) & ")"
                gvPrices.HeaderRow.Cells(e_PricesGrid.TFRPrice).Text = "TFR Price" & " (" & SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TFR_STNBranchCurrency) & ")"
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub form1_Load(sender As Object, e As EventArgs) Handles form1.Load
        Try
            Dim loggedEmail As String = StateManager.GetValue(StateManager.Keys.s_loggedEmail, True)
            Dim BranchCode As String = StateManager.GetValue(StateManager.Keys.s_BranchCode)
            Dim USerType As String = ""
            Dim sdtUserType As DataTable = clsUser.GetUserType(BranchCode, loggedEmail, USerType)

            If Not sdtUserType Is Nothing AndAlso sdtUserType.Rows.Count > 0 Then
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
        Catch ex As Exception
            Response.Redirect("~/Default.aspx", False)
        End Try

    End Sub
End Class