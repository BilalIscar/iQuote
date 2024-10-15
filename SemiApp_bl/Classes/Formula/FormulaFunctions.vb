Imports System.Configuration
Imports System.Web
Imports IscarDal

Partial Public Class FormulaResult
    'InputMeasure


    Private Function returnCurentModel() As DataTable
        If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False) = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
            Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelConfiguration, "")
        ElseIf SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False) = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
            Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelModification, "")
        End If
    End Function






    Private Function Calc_InputMeasure() As String
        Try
            HttpContext.Current.Trace.Write("Calc_InputMeasure, Start")
            Dim res As String
            Dim value As String
            'Dim ModelNum As Integer = CryptoManagerTDES.Decode(HttpContext.Current.Request("ModelNum"))
            'Dim ModelNum As Integer = CryptoManagerTDES.Decode(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationModelNumber))
            'Dim ModelNum As Integer = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ModelNumber)
            Dim dt_m As DataTable = returnCurentModel() 'SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Model, "")

            Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString

            Dim LabelNum As Integer
            Dim Order As String
            Dim OrderRelationRule As String

            HttpContext.Current.Trace.Write("Calc_InputMeasure, 1")

            'Dim dvRelations As DataView = clsModel.GetModelRelations(ModelNum).Copy.DefaultView
            'dvRelations.RowFilter = "RelationRuleNum = " & CurrentParameterIndex

            Dim dtRelationsTmp As DataTable = clsModel.GetModelRelations(ModelNum, CurrentParameterIndex)
            HttpContext.Current.Trace.Write("Calc_InputMeasure, 1.5")

            Dim drRelationsTmp() As DataRow = dtRelationsTmp.Select() '("RelationRuleNum = " & CurrentParameterIndex)
            'Dim drRelationsTmp() As DataRow = dtRelationsTmp.Select("RelationRuleNum = " & CurrentParameterIndex)

            HttpContext.Current.Trace.Write("Calc_InputMeasure, 2")

            Try
                LabelNum = CInt(drRelationsTmp(0)("LabelNum"))
            Catch ex As Exception
                Throw New Exception("InputMeasure Exception - GetLabelNum failure. ParameterIndex=" & CurrentParameterIndex)
            End Try
            HttpContext.Current.Trace.Write("Calc_InputMeasure, 3")

            Order = Parameters.Rows(LabelNum - 1)("Order").ToString
            'dvRelations.RowFilter = "RelationRuleNum = " & CurrentParameterIndex & " AND Order = '" & Order & "'"

            drRelationsTmp = dtRelationsTmp.Select("RelationRuleNum = " & CurrentParameterIndex & " AND Order = '" & Order & "'")
            HttpContext.Current.Trace.Write("Calc_InputMeasure, 4")

            If drRelationsTmp.Length = 0 Then
                Return "0" 'TODO:   LabelNum=68 (Drill)
            ElseIf drRelationsTmp.Length <> 1 Then
                Throw New Exception("InputMeasure Exception - Wrong Relations rows number. ParameterIndex=" & CurrentParameterIndex)
            End If
            HttpContext.Current.Trace.Write("Calc_InputMeasure, 5")

            OrderRelationRule = drRelationsTmp(0)("RelationRules").ToString

            'Dim dvRules As DataView = clsModel.GetModelRules(ModelNum).Copy.DefaultView
            'dvRules.RowFilter = "LabelNum=" & CurrentParameterIndex & " AND Order = '" & OrderRelationRule & "'"

            HttpContext.Current.Trace.Write("Calc_InputMeasure, 6")

            Dim dtRulesTmp As DataTable = clsModel.GetModelRules(ModelNum)
            HttpContext.Current.Trace.Write("Calc_InputMeasure, 6.5")

            Dim drRulesTmp() As DataRow = dtRulesTmp.Select("LabelNum=" & CurrentParameterIndex & " AND Order = '" & OrderRelationRule & "'")
            HttpContext.Current.Trace.Write("Calc_InputMeasure, 7")

            If drRulesTmp.Length <> 1 Then
                Throw New Exception("InputMeasure Exception - Wrong Rules rows number. ParameterIndex=" & CurrentParameterIndex)
            End If
            HttpContext.Current.Trace.Write("Calc_InputMeasure, 8")

            res = drRulesTmp(0)("InputLimit").ToString
            ResOrder = drRulesTmp(0)("Order").ToString
            HttpContext.Current.Trace.Write("Calc_InputMeasure, 9")

            Dim dummy_ResultSetRules As DataTable = Nothing
            Dim oFormula As New FormulaResult(res, Parameters, CurrentParameterIndex, dummy_ResultSetRules)
            HttpContext.Current.Trace.Write("Calc_InputMeasure, 10")

            value = oFormula.ParseAndCalculate
            HttpContext.Current.Trace.Write("Calc_InputMeasure, 11")

            oFormula = Nothing
            Return value
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function Calc_StringDesc(ByVal itm As Item) As String
        Dim DescVal As String = ""
        Try
            Dim dt_m As DataTable = returnCurentModel()
            Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString
            Dim dtRulesTmp As DataTable = clsModel.GetModelRules(ModelNum)

            Dim LabelNum As Integer = CInt(itm.ParameterIndex)
            Dim Order As String = Parameters.Rows(LabelNum - 1)("Order").ToString

            Dim drRulesTmp() As DataRow = dtRulesTmp.Select("LabelNum= " & LabelNum & " AND Order = '" & Order & "'", "OrderForHelp")
            DescVal = drRulesTmp(0)("DescValue").ToString

            Dim oFormulaX As New FormulaResult(DescVal, Parameters, 0, Nothing)
            DescVal = oFormulaX.ParseAndCalculate
            Return DescVal


            ResOrder = ""
            Return "0"
        Catch ex As Exception
            'GeneralException.WriteEventErrors("Calc_StringDesc-" & ex.Message & "- DescValue : " & DescVal, GeneralException.e_LogTitle.PARSER.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "Calc_StringDesc", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        End Try
    End Function

    Private Function Calc_StringValue(ByVal itm As Item) As String
        Try
            Dim dt_m As DataTable = returnCurentModel()
            Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString
            Dim dtRulesTmp As DataTable = clsModel.GetModelRules(ModelNum)

            Dim LabelNum As Integer = CInt(itm.ParameterIndex)

            'LabelNum = CInt(drRelationsTmp(0)("LabelNum"))
            Dim Order As String = Parameters.Rows(LabelNum - 1)("Order").ToString

            Dim StringVal As String = ""
            Dim drRulesTmp() As DataRow = dtRulesTmp.Select("LabelNum= " & LabelNum & " AND Order = '" & Order & "'", "OrderForHelp")
            If drRulesTmp.Length > 0 Then 'StringValue From QB
                StringVal = drRulesTmp(0)("StringValue").ToString
            Else
                'StringValue From QA
                Dim dtParam As DataTable = clsQuatation.GetActiveQuotation_DTparams()
                Dim drRulesTmpQA() As DataRow = dtParam.Select("TabIndex= " & LabelNum)
                If drRulesTmpQA.Length > 0 Then
                    StringVal = drRulesTmpQA(0)("StringValue").ToString
                End If

            End If

            ' If LabelNum = 2 Then Stop
            Dim oFormulaX As New FormulaResult(StringVal, Parameters, 0, Nothing)
            StringVal = oFormulaX.ParseAndCalculate

            If LabelNum = 2 AndAlso StringVal = "" Then
                Dim hh As Int16
                hh = 44
            End If

            Return StringVal


            ResOrder = ""
            Return "0"
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function Calc_Relation(ByVal Condition As Item, ByVal Result1 As Item, ByVal Result2 As Item) As String
        Try
            If CBool(Condition.Expression) Then
                If Not IsNothing(Result1) Then
                    If Result1.IllegalCalculation Then
                        Throw New Exception("Illegal Compute Expression")
                    End If
                    'Result2.IllegalCalculation = False
                    Return Result1.Expression
                Else
                    'Result2.IllegalCalculation = False
                    Return String.Empty
                End If

            Else
                If Not IsNothing(Result2) Then
                    If Result2.IllegalCalculation Then
                        Throw New Exception("Illegal Compute Expression")
                    End If
                    'Result1.IllegalCalculation = False
                    Return Result2.Expression
                Else
                    'Result1.IllegalCalculation = False
                    Return String.Empty
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function Calc_CrosList(ByVal items As List(Of Item)) As String
        Try
            'Dim ModelNum As Integer = CryptoManagerTDES.Decode(HttpContext.Current.Request("ModelNum"))
            'Dim ModelNum As Integer = CryptoManagerTDES.Decode(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationModelNumber))
            'Dim ModelNum As Integer = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ModelNumber)
            Dim dt_m As DataTable = returnCurentModel() ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Model, "")
            Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString

            'SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationModelNumber)
            Dim LabelNum As Integer
            Dim Order As String
            'Dim dvRelations() As DataView
            Dim colRelations As New List(Of DataRow())
            Dim colRelationsDT As New List(Of DataTable)
            Dim colRelationFilter As New List(Of String)
            'Dim AllParameters As DataTable
            Dim dtResultRules As DataTable


            If items.Count = 0 Then
                Throw New Exception("CrosList Exception - no parameters")
            End If

            'ReDim dvRelations(Nodes.Count - 1)

            For i As Integer = 0 To items.Count - 1
                Dim n As Item = items(i)
                'If i = 0 Then
                '    LabelNum = CInt(n.Nodes(0).Nodes(0).NodeValue)
                'Else
                '    LabelNum = CInt(n.Nodes(0).NodeValue)
                'End If
                LabelNum = CInt(n.ParameterIndex)
                Order = Parameters.Rows(LabelNum - 1)("Order").ToString
                'dvRelations(i) = clsModel.GetModelRelations(ModelNum).Copy.DefaultView
                Dim dt As DataTable = clsModel.GetModelRelations(ModelNum, CurrentParameterIndex)
                Dim filter As String = "LabelNum = '" & LabelNum & "' AND Order = '" & Order & "' "
                'Dim filter As String = "LabelNum = '" & LabelNum & "' AND Order = '" & Order & "'" & " And RelationRuleNum = " & CurrentParameterIndex & " "
                Dim dr() As DataRow = dt.Select(filter)
                colRelations.Add(dr)
                colRelationsDT.Add(dt)
                colRelationFilter.Add(filter)
                'dvRelations(i).RowFilter = "RelationRuleNum = " & CurrentParameterIndex & " AND LabelNum = '" & LabelNum & "' AND Order = '" & Order & "'"
                'dvRelations(i).RowFilter = "LabelNum = '" & LabelNum & "' AND Order = '" & Order & "'"
                'dvRelations(i).RowFilter = dvRelations(i).RowFilter & " And RelationRuleNum = " & CurrentParameterIndex & " "
            Next

            Dim dvAllRules As DataView = clsModel.GetModelRules(ModelNum).Copy.DefaultView
            dvAllRules.RowFilter = "(LabelNum=" & CurrentParameterIndex & " AND VisibleHelp=True)"
            dvAllRules.RowFilter &= "OR (LabelNum=" & CurrentParameterIndex & " AND (operation='visible' OR operation='unvisible'))"

            dtResultRules = dvAllRules.Table.Clone
            Dim IsPresent As Boolean = True
            For i As Integer = 0 To dvAllRules.Count - 1
                IsPresent = True
                Dim CurrentOrder As String = dvAllRules(i)("Order").ToString

                Dim iDr As Integer = -1
                For Each dr As DataRow() In colRelations
                    iDr += 1
                    'Dim r() As DataRow
                    Dim tmpFilter As String
                    'r = dv.Table.Select(" RelationRules = '" & CurrentOrder & "'")
                    'dv.RowFilter = " RelationRules = '" & CurrentOrder & "'"\
                    'Dim dvT As DataTable
                    'dvT.DataSet = dv.DataViewManage
                    'dvT.DefaultView.Table = dv.Table

                    tmpFilter = colRelationFilter(iDr) ' dv.RowFilter
                    Dim dr1 As DataRow() = colRelationsDT(iDr).Select(tmpFilter & " and RelationRules = '" & CurrentOrder & "'")
                    'dv.RowFilter = dv.RowFilter & " and RelationRules = '" & CurrentOrder & "'"
                    'dv.RowFilter = " RelationRules = '" & CurrentOrder & "'"
                    'RelationRuleNum = 17 AND LabelNum = '5' AND Order = 'R1

                    If dr1.Length = 0 Then
                        IsPresent = False
                        'dv.RowFilter = tmpFilter
                        Continue For
                    End If
                    'dv.RowFilter = tmpFilter

                Next
                If IsPresent Then
                    Dim dr As DataRow = dtResultRules.NewRow
                    For j As Integer = 0 To dtResultRules.Columns.Count - 1
                        dr(j) = dvAllRules(i)(j)
                    Next
                    dtResultRules.Rows.Add(dr)
                End If
            Next

            ResultSetRules = dtResultRules '.DefaultView


            For Each dr As DataRow In ResultSetRules.Rows
                If dr("Operation").ToString.ToUpper = "UNVISIBLE" Then
                    ResOrder = dr("Order").ToString()
                    Return 0
                End If
            Next

            'Return "CrosListUnvisible"
            If dtResultRules.Rows.Count = 0 Then
                Return 0
            Else
                Return ""
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    'CrosListCheck
    Private Function Calc_CrosListCheck(ByVal items As List(Of Item)) As String
        Try
            'Dim res As String
            'Dim value As String
            'Dim ModelNum As Integer = CryptoManagerTDES.Decode(HttpContext.Current.Request("ModelNum"))
            'Dim ModelNum As Integer = CryptoManagerTDES.Decode(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationModelNumber))
            'Dim ModelNum As Integer = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ModelNumber)
            Dim dt_m As DataTable = returnCurentModel() ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Model, "")
            Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString

            'Dim LabelNum As Integer
            'Dim Order As String
            'Dim OrderRelationRule As String
            'Dim dvRelations() As DataView
            'Dim AllParameters As DataTable
            'Dim dtResultRules As DataTable


            If items.Count = 0 Then
                Throw New Exception("CrosList Exception - no parameters")
            End If

            If Parameters.Rows(CInt(items(items.Count - 1).ParameterIndex) - 1)("Measure").ToString = "1" Then
                Dim dtRules As DataTable = clsModel.GetModelRules(ModelNum).Copy
                Dim drs() As DataRow = dtRules.Select("LabelNum=" & CurrentParameterIndex & " AND operation='unvisible' ")
                'TODO: check it in old system
                If drs.Length > 0 Then
                    ResOrder = drs(0)("Order").ToString
                    Return drs(0)("Operation").ToString
                Else
                    Return ""
                End If
            Else
                items.RemoveAt(items.Count - 1)
                Return Calc_CrosList(items)
            End If



            'For Each dr As DataRow In ResultSetRules.Rows
            '    If dr("Operation").ToString.ToUpper = "UNVISIBLE" Then
            '        ResOrder = dr("Order").ToString()
            '        Return 0
            '    End If
            'Next
            'If dtResultRules.Rows.Count = 0 Then
            '    Return 0
            'Else
            '    Return ""
            'End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    'Cros
    Private Function Calc_Cros(ByVal items As List(Of Item)) As String
        Try
            Dim res As String
            Dim value As String
            'Dim ModelNum As Integer = CryptoManagerTDES.Decode(HttpContext.Current.Request("ModelNum"))
            'Dim ModelNum As Integer = CryptoManagerTDES.Decode(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationModelNumber))
            'Dim ModelNum As Integer = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ModelNumber)
            Dim dt_m As DataTable = returnCurentModel() ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Model, "")
            Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString

            Dim LabelNum As Integer
            Dim Order As String = ""
            'Dim OrderRelationRule As String
            'Dim dvRelations As DataView
            Dim dtRelationsTmp As DataTable
            Dim drRelationsTmp() As DataRow
            'Dim dtResultRules As DataTable
            Dim orS As String = ""


            If items.Count = 0 Then
                Throw New Exception("CrosList Exception - no parameters")
            End If

            'Dim s As String = "relationrulenum=" & CurrentParameterIndex & " and ("
            Dim ParamsCount As Integer = 0
            Dim s As String = " ("
            For i As Integer = 0 To items.Count - 1
                Dim n As Item = items(i)
                'If i = 0 Then
                '    LabelNum = CInt(n.Nodes(0).Nodes(0).NodeValue)
                'Else
                '    LabelNum = CInt(n.Nodes(0).NodeValue)
                'End If
                LabelNum = CInt(n.ParameterIndex)
                Order = Parameters.Rows(LabelNum - 1)("Order").ToString

                s &= orS & "(labelnum=" & LabelNum & " and order='" & Order & "') "
                orS = " or "
                ParamsCount += 1
            Next
            s &= ")"

            dtRelationsTmp = clsModel.GetModelRelations(ModelNum, CurrentParameterIndex)
            drRelationsTmp = dtRelationsTmp.Select(s, "relationrules")
            'dvRelations.Sort = "relationrules"

            Dim found As Boolean = False


            If ParamsCount <> 2 Then
                Dim ff As Integer = 0
            End If
            If ParamsCount = 5 Then '--Cros({},{},{},{},{})
                For i As Integer = 4 To drRelationsTmp.Length - 1
                    If drRelationsTmp(i)("relationrules").ToString = drRelationsTmp(i - 1)("relationrules").ToString AndAlso
                       drRelationsTmp(i)("relationrules").ToString = drRelationsTmp(i - 2)("relationrules").ToString AndAlso
                       drRelationsTmp(i)("relationrules").ToString = drRelationsTmp(i - 3)("relationrules").ToString AndAlso
                       drRelationsTmp(i)("relationrules").ToString = drRelationsTmp(i - 4)("relationrules").ToString Then
                        Order = drRelationsTmp(i)("relationrules").ToString
                        found = True
                        Exit For
                    End If
                Next
            End If
            If ParamsCount = 4 Then '--Cros({},{},{},{})
                For i As Integer = 3 To drRelationsTmp.Length - 1
                    If drRelationsTmp(i)("relationrules").ToString = drRelationsTmp(i - 1)("relationrules").ToString AndAlso
                       drRelationsTmp(i)("relationrules").ToString = drRelationsTmp(i - 2)("relationrules").ToString AndAlso
                       drRelationsTmp(i)("relationrules").ToString = drRelationsTmp(i - 3)("relationrules").ToString Then
                        Order = drRelationsTmp(i)("relationrules").ToString
                        found = True
                        Exit For
                    End If
                Next
            End If
            If ParamsCount = 3 Then '--Cros({},{},{})
                For i As Integer = 2 To drRelationsTmp.Length - 1
                    If drRelationsTmp(i)("relationrules").ToString = drRelationsTmp(i - 1)("relationrules").ToString AndAlso
                       drRelationsTmp(i)("relationrules").ToString = drRelationsTmp(i - 2)("relationrules").ToString Then
                        Order = drRelationsTmp(i)("relationrules").ToString
                        found = True
                        Exit For
                    End If
                Next
            End If

            If found = False Then '--Cros({},{})
                For i As Integer = 1 To drRelationsTmp.Length - 1
                    If drRelationsTmp(i)("relationrules").ToString = drRelationsTmp(i - 1)("relationrules").ToString Then
                        Order = drRelationsTmp(i)("relationrules").ToString
                        found = True
                        Exit For
                    End If
                Next
            End If


            'Dim found As Boolean = False
            'For i As Integer = 1 To drRelationsTmp.Length - 1
            '    If drRelationsTmp(i)("relationrules").ToString = drRelationsTmp(i - 1)("relationrules").ToString Then
            '        Order = drRelationsTmp(i)("relationrules").ToString
            '        found = True
            '        Exit For
            '    End If
            'Next

            If found Then
                Dim dtAllRules As DataTable = clsModel.GetModelRules(ModelNum)
                Dim drAllRules() As DataRow = dtAllRules.Select("LabelNum=" & CurrentParameterIndex & " and Order='" & Order & "'")
                'dvAllRules.RowFilter = "LabelNum=" & CurrentParameterIndex & " and Order='" & Order & "'"

                res = drAllRules(0)("InputLimit").ToString
                ResOrder = drAllRules(0)("Order").ToString

                '#fix by Micki, 19.01.2010, check if need in other functions
                If res = "" Then
                    If drAllRules(0)("HeightLimit").ToString <> "" Then
                        res = drAllRules(0)("HeightLimit").ToString
                    Else
                        res = ""
                    End If

                End If

                Dim dummy_ResultSetRules As DataTable = Nothing
                Dim oFormula As New FormulaResult(res, Parameters, CurrentParameterIndex, dummy_ResultSetRules)
                value = oFormula.ParseAndCalculate
                oFormula = Nothing
                Return value
            Else
                Return "0" 'TODO: Exception???
            End If


        Catch ex As Exception
            Throw
        End Try






        '' ''Dim res As String
        '' ''Dim value As String
        ' '' ''Dim ModelNum As Integer = CryptoManagerTDES.Decode(HttpContext.Current.Request("ModelNum"))
        ' '' ''Dim ModelNum As Integer = CryptoManagerTDES.Decode(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationModelNumber))
        ' '' ''Dim ModelNum As Integer = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ModelNumber)
        '' ''Dim dt_m As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Model, "")
        '' ''Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString

        '' ''Dim LabelNum As Integer
        '' ''Dim Order As String = ""
        ' '' ''Dim OrderRelationRule As String
        ' '' ''Dim dvRelations As DataView
        '' ''Dim dtRelationsTmp As DataTable
        '' ''Dim drRelationsTmp() As DataRow
        ' '' ''Dim dtResultRules As DataTable
        '' ''Dim orS As String = ""


        '' ''If items.Count = 0 Then
        '' ''    Throw New Exception("CrosList Exception - no parameters")
        '' ''End If

        ' '' ''Dim s As String = "relationrulenum=" & CurrentParameterIndex & " and ("

        '' ''Dim s As String = " ("
        '' ''For i As Integer = 0 To items.Count - 1
        '' ''    Dim n As Item = items(i)
        '' ''    'If i = 0 Then
        '' ''    '    LabelNum = CInt(n.Nodes(0).Nodes(0).NodeValue)
        '' ''    'Else
        '' ''    '    LabelNum = CInt(n.Nodes(0).NodeValue)
        '' ''    'End If
        '' ''    LabelNum = CInt(n.ParameterIndex)
        '' ''    Order = Parameters.Rows(LabelNum - 1)("Order").ToString

        '' ''    s &= orS & "(labelnum=" & LabelNum & " and order='" & Order & "') "
        '' ''    orS = " or "

        '' ''Next
        '' ''s &= ")"

        '' ''dtRelationsTmp = clsModel.GetModelRelations(ModelNum, CurrentParameterIndex)
        '' ''drRelationsTmp = dtRelationsTmp.Select(s, "relationrules")
        ' '' ''dvRelations.Sort = "relationrules"

        '' ''Dim found As Boolean = False
        '' ''For i As Integer = 1 To drRelationsTmp.Length - 1
        '' ''    If drRelationsTmp(i)("relationrules").ToString = drRelationsTmp(i - 1)("relationrules").ToString Then
        '' ''        Order = drRelationsTmp(i)("relationrules").ToString
        '' ''        found = True
        '' ''        Exit For
        '' ''    End If
        '' ''Next

        '' ''If found Then
        '' ''    Dim dtAllRules As DataTable = clsModel.GetModelRules(ModelNum)
        '' ''    Dim drAllRules() As DataRow = dtAllRules.Select("LabelNum=" & CurrentParameterIndex & " and Order='" & Order & "'")
        '' ''    'dvAllRules.RowFilter = "LabelNum=" & CurrentParameterIndex & " and Order='" & Order & "'"

        '' ''    res = drAllRules(0)("InputLimit").ToString
        '' ''    ResOrder = drAllRules(0)("Order").ToString

        '' ''    '#fix by Micki, 19.01.2010, check if need in other functions
        '' ''    If res = "" Then
        '' ''        If drAllRules(0)("HeightLimit").ToString <> "" Then
        '' ''            res = drAllRules(0)("HeightLimit").ToString
        '' ''        Else
        '' ''            res = "0"
        '' ''        End If

        '' ''    End If

        '' ''    Dim dummy_ResultSetRules As DataTable = Nothing
        '' ''    Dim oFormula As New FormulaResult(res, Parameters, CurrentParameterIndex, dummy_ResultSetRules)
        '' ''    value = oFormula.ParseAndCalculate
        '' ''    oFormula = Nothing
        '' ''    Return value
        '' ''Else
        '' ''    Return "0" 'TODO: Exception???
        '' ''End If


    End Function


    Private Function Calc_QBValueByQty(ByVal itm As Item) As String
        Dim value As String = "-1"
        Try
            'AND Convert(LowLimit, 'System.Decimal') <= " & Quantity & " AND Convert(HeightLimit, 'System.Decimal') > " & Quantity

            Dim dummy_ResultSetRules As DataTable = Nothing
            Dim LabelNum As String = ""
            Dim tParameters As DataRow() = Parameters.Select("Label='CheckByQty'")
            Dim TabIndex As String = tParameters(0).Item("TabIndex")
            Dim dt_m As DataTable = returnCurentModel() 'SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Model, "")

            Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString
            Dim dtRules As DataTable = clsModel.GetModelRules(ModelNum)
            'Dim s As String = "Label='" & TabIndex & "'"
            's &= "LowLimit >= '" & Quantity & "' HeightLimit < '" & Quantity & "'"

            Dim s As String = "LabelNum = '" & TabIndex & "'"



            Dim dr() As DataRow = dtRules.Select(s)

            For i As Integer = 0 To dr.Length - 1
                If Quantity >= CDec(dr(i)("LowLimit")) AndAlso Quantity < CDec(dr(i)("HeightLimit")) Then
                    value = dr(i)("InputLimit").ToString
                    Dim oFormula As New FormulaResult(value, Parameters, CurrentParameterIndex, dummy_ResultSetRules)
                    value = oFormula.ParseAndCalculate
                    ResOrder = ""
                    Return value
                End If
            Next




        Catch ex As Exception
            Return value
        End Try
    End Function




    Private Function Calc_RangeRule(ByVal itm As Item) As String
        Try
            Dim value As String
            Dim minVal As String
            Dim maxVal As String
            'Dim ModelNum As Integer = CryptoManagerTDES.Decode(HttpContext.Current.Request("ModelNum"))
            'Dim ModelNum As Integer = CryptoManagerTDES.Decode(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationModelNumber))
            'Dim ModelNum As Integer = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ModelNumber)
            Dim dt_m As DataTable = returnCurentModel() 'SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Model, "")
            Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString

            Dim dummy_ResultSetRules As DataTable = Nothing
            Dim LabelNum As String


            'Dim dvRules As DataView = clsModel.GetModelRules(ModelNum).Copy.DefaultView
            Dim dtRulesTmp As DataTable = clsModel.GetModelRules(ModelNum)
            Dim drRulesTmp() As DataRow = dtRulesTmp.Select("LabelNum= " & CurrentParameterIndex, "OrderForHelp")
            'dvRules.RowFilter = "LabelNum=" & CurrentParameterIndex
            'dvRules.Sort = "OrderForHelp"
            LabelNum = Parameters.Rows(itm.ParameterIndex - 1)("Measure").ToString

            For Each dr As DataRow In drRulesTmp
                minVal = dr("LowLimit").ToString
                maxVal = dr("HeightLimit").ToString

                If Not IsNumeric(maxVal) Then
                    Dim oFormula As New FormulaResult(maxVal, Parameters, CurrentParameterIndex, dummy_ResultSetRules)
                    maxVal = oFormula.ParseAndCalculate
                    oFormula = Nothing
                End If

                If CDec(LabelNum) < CDec(maxVal) Then
                    If Not IsNumeric(minVal) Then
                        Dim oFormula As New FormulaResult(minVal, Parameters, CurrentParameterIndex, dummy_ResultSetRules)
                        minVal = oFormula.ParseAndCalculate
                        oFormula = Nothing
                    End If
                    If CDec(LabelNum) < CDec(maxVal) And CDec(LabelNum) >= CDec(minVal) Then
                        value = dr("InputLimit").ToString
                        '18.01.10 micki bilal
                        Dim oFormula As New FormulaResult(value, Parameters, CurrentParameterIndex, dummy_ResultSetRules)
                        value = oFormula.ParseAndCalculate
                        ResOrder = dr("Order").ToString
                        Return value
                    End If

                End If
            Next

            ResOrder = ""
            Return "0"
        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function Calc_CheckinList(ByVal itm As Item) As String
        Try
            Dim value As String
            Dim res As String
            'Dim ModelNum As Integer = CryptoManagerTDES.Decode(HttpContext.Current.Request("ModelNum"))
            'Dim ModelNum As Integer = CryptoManagerTDES.Decode(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationModelNumber))
            'Dim ModelNum As Integer = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ModelNumber)
            Dim dt_m As DataTable = returnCurentModel() ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Model, "")
            Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString

            Dim dummy_ResultSetRules As DataTable = Nothing


            value = itm.Expression

            If Left(value, 1) = """" AndAlso Right(value, 1) = """" Then
                value = value.Substring(1, value.Length - 2)
            End If

            'Dim dvRules As DataView = clsModel.GetModelRules(ModelNum).Copy.DefaultView
            Dim dtRulesTmp As DataTable = clsModel.GetModelRules(ModelNum)
            'Dim drRulesTmp() As DataRow = dtRulesTmp.Select("LabelNum=" & CurrentParameterIndex & " And HeightLimit='" & value & "'", "OrderForHelp")
            Dim drRulesTmp() As DataRow = dtRulesTmp.Select("LabelNum=" & CurrentParameterIndex & " and (HeightLimit='" & value & "' OR HeightLimit='""" & value & """')", "OrderForHelp")
            'Dim drRulesTmp() As DataRow = dtRulesTmp.Select("LabelNum=" & CurrentParameterIndex & " and (HeightLimit='" & value & "' OR HeightLimit='""" & value & """')", "OrderForHelp")

            'dvRules.RowFilter = "LabelNum=" & CurrentParameterIndex & " and HeightLimit='" & value & "'"
            'dvRules.Sort = "OrderForHelp"
            'LabelNum = Parameters.Rows(CInt(Nodes(0).Nodes(0).Nodes(0).NodeValue) - 1)("Measure").ToString

            If drRulesTmp.Length > 0 Then
                'if not drRulesTmp(0)("InputLimit") is dbnull
                Dim oFormula As New FormulaResult(drRulesTmp(0)("InputLimit").ToString, Parameters, CurrentParameterIndex, dummy_ResultSetRules)
                res = oFormula.ParseAndCalculate
                ResOrder = drRulesTmp(0)("Order")
                oFormula = Nothing
                Return res
            Else
                ResOrder = ""
                Return "0"
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function Calc_CheckinQtyList(ByVal Result1 As Item, ByVal Result2 As Item) As String
        'Try

        '    Dim dt_m As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Model, "")
        '    Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString
        '    Dim dtRulesTmp As DataTable = clsModel.GetModelRules(ModelNum)
        '    Dim drRulesTmp() As DataRow = dtRulesTmp.Select("LabelNum=" & CurrentParameterIndex & " and (LowLimit >= '" & value & "' AND HeightLimit < '" & value & "')", "OrderForHelp")

        '    Dim res As String
        '    Dim dummy_ResultSetRules As DataTable = Nothing

        '    If drRulesTmp.Length > 0 Then
        '        Dim oFormula As New FormulaResult(drRulesTmp(0)("InputLimit").ToString, Parameters, CurrentParameterIndex, dummy_ResultSetRules)
        '        res = oFormula.ParseAndCalculate
        '        ResOrder = drRulesTmp(0)("Order")
        '        oFormula = Nothing
        '        Return res
        '    Else
        '        ResOrder = ""
        '        Return "0"
        '    End If

        '    Dim res1 As String
        '    Dim res2 As String
        '    res1 = Result1.Expression.ToString
        '    res2 = Result2.Expression.ToString


        '    If InStr(res1, res2) > 0 Then
        '        Return "1"
        '    Else
        '        Return "0"
        '    End If

        'Catch ex As Exception
        '    Throw
        'End Try
    End Function

    Private Function Calc_GetCatNum() As String
        Try
            Dim catno As String
            Dim dr() As DataRow
            dr = Parameters.Select("Priority>=0", "Priority")
            catno = clsStandard.GetStandardCatalog(dr(0)("Measure").ToString, Parameters)
            Return catno
        Catch ex As Exception
            Throw
        End Try
    End Function
    'GetPrice
    Private Function Calc_GetPrice(ByVal n1 As Item, ByVal n2 As Item) As String
        'Try
        '    If StateManager.GetValue(StateManager.Keys.GalSystem) = StateManager.GalSystem.GAL2.ToString Then
        '        Return clsStandard.FindGal2StandardPriceForCatalog(n1.Expression, n2.Expression)
        '    ElseIf StateManager.GetValue(StateManager.Keys.GalSystem) = StateManager.GalSystem.GAL6.ToString Then

        '        If StateManager.GetValue(StateManager.Keys.GetGALDataFromTefen) = "True" Then
        '            Dim dt As DataTable = clsBranch.GetBranchDetails(StateManager.GetValue(StateManager.Keys.BranchCode))
        '            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 AndAlso IsNumeric(dt.Rows(0).Item("BranchNumber")) Then
        '                Dim BranchCode As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode)
        '                If BranchCode = "FM" Then
        '                    ''30.06.19 - משימה 131288
        '                    'Return clsStandard.FindGal6StandardPriceForCatalog(n1.Expression, dt.Rows(0).Item("BranchNumber"), n2.Expression, "FM", True)
        '                    Return clsStandard.FindGal6StandardPrice_New(n1.Expression, dt.Rows(0).Item("BranchNumber"), "FM", True, n2.Expression)
        '                Else
        '                    ''30.06.19 - משימה 131288
        '                    'Return clsStandard.FindGal6StandardPriceForCatalog(n1.Expression, dt.Rows(0).Item("BranchNumber"), n2.Expression, "", True)
        '                    Return clsStandard.FindGal6StandardPrice_New(n1.Expression, dt.Rows(0).Item("BranchNumber"), "", True, n2.Expression)
        '                End If
        '            End If
        '        End If
        '        Dim CustomerNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.CustomerNumber)
        '        Dim CustomerNumTemp As Integer = CustomerNo
        '        Dim WC_ID As String = StateManager.GetValue(StateManager.Keys.WorkCenterID)
        '        If CInt(WC_ID) > CInt(StateManager.WorkCenterID.ISCAR_LTD) Then
        '            CustomerNo = clsCustomer.GetGlobalCustomerNumber(Integer.Parse(WC_ID), CustomerNo)
        '            If CustomerNo = 0 Then
        '                Dim bn As String = StateManager.GetValue(StateManager.Keys.BranchNumber)


        '                'If StateManager.GetValue(StateManager.Keys.WorkCenterID) = StateManager.WorkCenterID.TUNGALOY_JAPAN Then
        '                '    ''30.06.19 - משימה 131288
        '                '    'Return clsStandard.FindGal6StandardPriceForCatalog(n1.Expression, 730514, n2.Expression, "KP")
        '                '    Return clsStandard.FindGal6StandardPrice_New(n1.Expression, 730514, "KP", True, n2.Expression)


        '                If StateManager.GetValue(StateManager.Keys.WorkCenterID) = StateManager.WorkCenterID.INGERSOLL Then
        '                    ''30.06.19 - משימה 131288
        '                    'Return clsStandard.FindGal6StandardPriceForCatalog(n1.Expression, 100486, n2.Expression, "IS")
        '                    Return clsStandard.FindGal6StandardPrice_New(n1.Expression, 100486, "IS", True, n2.Expression)
        '                Else
        '                    ''30.06.19 - משימה 131288
        '                    'Return clsStandard.FindGal6StandardPriceForCatalog(n1.Expression, CustomerNumTemp, n2.Expression,  "")
        '                    Return clsStandard.FindGal6StandardPrice_New(n1.Expression, CustomerNumTemp, "", False, n2.Expression)
        '                End If
        '            Else
        '                ''30.06.19 - משימה 131288
        '                'Return clsStandard.FindGal6StandardPriceForCatalog(n1.Expression, CustomerNo, n2.Expression, "IS")
        '                Return clsStandard.FindGal6StandardPrice_New(n1.Expression, CustomerNo, "IS", False, n2.Expression)
        '            End If
        '        Else ''Regular Branches
        '            ''30.06.19 - משימה 131288
        '            'Return clsStandard.FindGal6StandardPriceForCatalog(n1.Expression, CustomerNo, n2.Expression, "")
        '            Return clsStandard.FindGal6StandardPrice_New(n1.Expression, CustomerNo, "", True, n2.Expression)
        '        End If

        '        'Dim CustomerNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.CustomerNumber)

        '        'If StateManager.GetValue(StateManager.Keys.GetGALDataFromTefen) = "True" Then
        '        '    Dim dt As DataTable = clsBranch.GetBranchDetails(StateManager.GetValue(StateManager.Keys.BranchCode))
        '        '    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 AndAlso IsNumeric(dt.Rows(0).Item("BranchNumber")) Then
        '        '        Return clsStandard.FindGal6StandardPriceForCatalog(n1.Expression, dt.Rows(0).Item("BranchNumber"), "", Quantity, True)
        '        '    End If
        '        'End If

        '        'Return clsStandard.FindGal6StandardPriceForCatalog(n1.Expression, CustomerNo, n2.Expression, "")
        '    End If
        'Catch ex As Exception
        '    Throw
        'End Try
    End Function

    Private Function Calc_InString(ByVal Result1 As Item, ByVal Result2 As Item) As String
        Try
            Dim res1 As String
            Dim res2 As String
            res1 = Result1.Expression.ToString
            res2 = Result2.Expression.ToString
            If InStr(res1, res2) > 0 Then
                Return "1"
            Else
                Return "0"
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function Calc_USER_GROUP() As String
        Try
            'Dim USER_GROUP_ID As String = StateManager.GetValue(StateManager.Keys.USER_GROUP_ID)

            'Select Case USER_GROUP_ID
            '    Case StateManager.User_Group.SalesMen : Return "S"
            '    Case StateManager.User_Group.Administrator : Return "A"
            '    Case StateManager.User_Group.Administrator_Plus : Return "A"
            '    Case StateManager.User_Group.Employee : Return "E"
            '    Case StateManager.User_Group.ISCAR_GMBH_SalesMan : Return "S"
            '    Case StateManager.User_Group.ISCAR_SWISS_SalesMan : Return "S"
            '    Case StateManager.User_Group.Manager : Return "M"
            '    Case StateManager.User_Group.ISCAR_GMBH_Dealer : Return "D"
            '    Case StateManager.User_Group.ISCAR_Austria_Dealer : Return "D"
            '    Case StateManager.User_Group.ISCAR_FRANCE_SalesMan : Return "S"
            '    Case StateManager.User_Group.ISCAR_GMBH_Administrator : Return "A"
            '    Case StateManager.User_Group.ISCAR_GMBH_Employee : Return "E"

            '    Case StateManager.User_Group.Cut_Grip : Return "P"
            '    Case StateManager.User_Group.Drilling : Return "P"
            '    Case StateManager.User_Group.Katia_Dept : Return "P"
            '    Case StateManager.User_Group.Milling : Return "P"
            '    Case StateManager.User_Group.MTB : Return "P"
            '    Case StateManager.User_Group.Parting : Return "P"
            '    Case StateManager.User_Group.S_C_Endmills : Return "P"
            '    Case StateManager.User_Group.Turning : Return "P"

            'End Select
            Return ""

        Catch ex As Exception
            Throw
        End Try
    End Function


    'GetCurrentBranch
    Private Function Calc_GetCurrentBranch() As String
        Try
            Return clsBranch.GetBranchNumberCode(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)) ' StateManager.GetValue(StateManager.Keys.s_BranchNumber) ' BranchNumber
        Catch ex As Exception
            Throw
        End Try
    End Function
    'GetCurrentBranchCode
    Private Function Calc_GetCurrentBranchCode() As String
        Try
            Return StateManager.GetValue(StateManager.Keys.s_BranchCode) ' BranchNumber
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function Calc_ShowMessage(ByVal n1 As Item, Optional ByVal n2 As Item = Nothing)
        Try
            Calc_ShowMessage = Nothing
            Dim sMessage1 As String = n1.Expression
            If Not n2 Is Nothing Then
                Dim sMessageShow As String = n2.Expression
                If sMessageShow = "1" Then
                    'GeneralException.WriteEvent(n1.Expression)

                    'Dim strMessage = "Please go to File --> Page Setup, and select 'Landscape' orientation before printing"
                    'Dim strScript As String = "<script language=JavaScript>alert('" & strMessage & "');</script>"
                    'If (Not System.Web.UI.Page.IsStartupScriptRegistered("clientScript")) Then
                    '    System.Web.UI.Page.RegisterStartupScript("clientScript", strScript)
                    '    UI.Page.CreateHtmlTextWriterFromType()
                    'End If
                End If
            Else
                'GeneralException.WriteEvent(n1.Expression)
            End If
            'confirm(
        Catch ex As Exception
            Throw
        End Try
    End Function



    'GetLength
    Private Function Calc_GetLength(ByVal Result1 As Item) As String
        Try
            Dim resStr As String = Result1.Expression

            For i As Integer = 0 To UBound(SpecialLabels, 1)
                resStr = Replace(resStr, SpecialLabels(i, 0), SpecialLabels(i, 1))
            Next

            'Dim res1 As Integer = Integer.Parse(Result1.Expression.Length)
            Dim res1 As Integer = Integer.Parse(resStr.Length)

            'Dim value As Integer

            'value = Parameters.Rows(CInt(Nodes(0).Nodes(0).Nodes(0).NodeValue) - 1)("Measure").ToString.Length
            ResOrder = ""
            Return res1
        Catch ex As Exception
            Throw
        End Try
    End Function
    'Calc_GetNumericValues
    Private Function Calc_GetNumericValues(ByVal Itm As Item) As String
        Try
            Dim s As String = Itm.Expression
            Dim res1 As String = ""
            For i As Integer = 0 To s.Length
                If IsNumeric(Mid(s, i + 1, 1)) Then
                    res1 &= Mid(s, i + 1, 1)
                End If
            Next
            ResOrder = ""
            Return res1
        Catch ex As Exception
            Throw
        End Try

    End Function
    'GetMax
    Private Function Calc_GetMax(ByVal Result1 As Item, ByVal Result2 As Item) As String
        Try
            Dim res1 As Decimal
            Dim res2 As Decimal
            res1 = CDec(Result1.Expression)
            res2 = CDec(Result2.Expression)
            If res1 > res2 Then
                Return Result1.Expression
            Else
                Return Result2.Expression
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function Calc_ValueByRange(ByVal Result1 As Item, ByVal Result2 As Item, ByVal Result3 As String) As String

        Try

            Dim dResult1 As String = Result1.Expression

            Dim dt_m As DataTable = returnCurentModel()
            'Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString
            'Dim dtRulesTmp As DataTable = clsModel.GetModelRules(ModelNum)

            Dim dtParam As DataTable = clsQuatation.GetActiveQuotation_DTparams()
            Dim ParanTabIndex As String = Result1.ParameterIndex + 1
            'Dim s As String = SearchField.Expression.Replace("""", "")

            Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString


            Dim dtRulesTmp As DataTable
            dtRulesTmp = clsModel.GetModelRules(ModelNum)
            Dim dr() As DataRow = dtRulesTmp.Select("LabelNum = " & ParanTabIndex & " AND HeightLimitHFormula = '" & Result1.Expression & "'")
            If Not dr Is Nothing AndAlso dr.Count > 0 Then
                For i As Integer = 0 To dr.Count - 1
                    If IsNumeric(dr(0)("LowLimit")) AndAlso IsNumeric(dr(0)("HeightLimit")) AndAlso IsNumeric(Result2.Expression) Then
                        If CDbl(dr(0)("LowLimit") <= CDbl(Result2.Expression)) And CDbl(dr(0)("HeightLimit") > CDbl(Result2.Expression)) Then
                            Return dr(0)(Replace(Result3, """", "")).ToString

                        End If
                    End If
                Next
            End If


            Return ""

            'Dim dv As DataView = dtRulesTmp.Copy.DefaultView


            'dv.RowFilter = "LabelNum = " & ParanTabIndex '& " And " & s & "= '""" & value & """'"

            'Dim sFilter As String
            'sFilter = "LabelNum = " & ParanTabIndex
            'sFilter &= " AND HeightLimitHFormula = '" & Result1.Expression & "'"
            'sFilter &= " AND LowLimit <= " & Result2.Expression
            'sFilter &= " AND HeightLimit > " & Result2.Expression

            'dv.RowFilter = sFilter

            'Dim dvRange As DataView = dv


            'If dv.Count > 0 Then
            '    Return dv(0)(Replace(Result3, """", ""))
            'Else
            '    Return ""
            'End If
            'ResultSetRules = dv.ToTable

        Catch ex As Exception
            Throw
        End Try
    End Function

    'ItemRangeGroup
    Private Function Calc_ItemRangeGroup(ByVal Result1 As Item, ByVal Result2 As Item, ByVal Condition As Item, ByVal Result3 As Item) As String
        Try
            Calc_ItemRangeGroup = Nothing
            'Select Case CurrentParameterIndex
            '    Case 153
            '        Return 47
            '    Case 162
            '        Return 37
            'End Select
            'Exit Function

            '------

            'Dim dtRelationsTmp As DataTable = clsModel.GetModelRelations(ModelNum)
            'Dim drRelationsTmp() As DataRow = dtRelationsTmp.Select("RelationRuleNum = " & CurrentParameterIndex)

            Dim dResult1 As Decimal = Result1.Expression
            Dim dResult2 As Decimal = Result2.Expression
            Dim dResult3 As Decimal = Result3.Expression
            Dim dResult3_100 As Decimal = dResult3 + 100

            Dim Filter As String
            Filter = "LabelNum=" & CurrentParameterIndex
            'If InStr(dResult3.ToString, ".") = 0 Then Filter &= " AND LowLimit >= " & dResult3 & ".0" Else Filter &= " AND LowLimit >= " & dResult3
            'If InStr(dResult3_100.ToString, ".") = 0 Then Filter &= " AND LowLimit < " & dResult3_100 & ".0" Else Filter &= " AND LowLimit < " & dResult3_100
            'If InStr(dResult2.ToString, ".") = 0 Then Filter &= " AND LowLimitH >= " & dResult1 & ".0" Else Filter &= " AND LowLimitH >= " & dResult1
            Filter &= " AND LowLimit >= " & Format(dResult3, "0.0#")
            Filter &= " AND LowLimit < " & Format(dResult3_100, "0.0#")
            Filter &= " AND LowLimitH >= " & Format(dResult1, "0.0#")

            'Dim ModelNum As Integer = CryptoManagerTDES.Decode(HttpContext.Current.Request("ModelNum"))
            'Dim ModelNum As Integer = CryptoManagerTDES.Decode(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationModelNumber))
            'Dim ModelNum As Integer = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ModelNumber)
            Dim dt_m As DataTable = returnCurentModel() ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Model, "")
            Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString

            Dim dvRules As DataView = clsModel.GetModelRules(ModelNum).Copy.DefaultView
            dvRules.RowFilter = Filter

            ''Filter="LabelNum=153 AND LowLimit >= 0 AND LowLimit < 100 AND LowLimitH >= 20"
            ''select * from glb_TToolGeometricRules where modelnum=444 and labelnum=153
            ''AND cast(LowLimit as decimal(18,3))>=0 AND cast(LowLimit as decimal(18,3))<100
            ''AND cast(LowLimitH as decimal(18,3)) >20

            'dvRules.Sort = "LowLimitH"

            Dim sResult3RealVal As Double
            sResult3RealVal = dvRules.Item(0)("LowLimitH")

            Filter = "LabelNum=" & CurrentParameterIndex
            'If InStr(dResult3.ToString, ".") = 0 Then Filter &= " AND LowLimit >= " & dResult3 & ".0" Else Filter &= " AND LowLimit >= " & dResult3
            'If InStr(dResult3_100.ToString, ".") = 0 Then Filter &= " AND LowLimit < " & dResult3_100 & ".0" Else Filter &= " AND LowLimit < " & dResult3_100
            'If InStr(sResult3RealVal.ToString, ".") = 0 Then Filter &= " AND LowLimitH = " & sResult3RealVal & ".0" Else Filter &= " AND LowLimitH = " & sResult3RealVal

            Filter &= " AND LowLimit >= " & Format(dResult3, "0.0#")
            Filter &= " AND LowLimit < " & Format(dResult3_100, "0.0#")
            Filter &= " AND LowLimitH = " & Format(sResult3RealVal, "0.0#")
            dvRules.RowFilter = Filter
            Dim isSent As Boolean = True

            Do Until isSent = False Or dvRules.Count = 0
                For Each dvr As DataRowView In dvRules
                    If CDbl(dResult2) >= CDbl(dvr("LowLimit")) AndAlso CDbl(dResult2) < CDbl(dvr("HeightLimit")) Then
                        ResOrder = dvr("Order").ToString
                        Return dvr("InputLimit")
                        isSent = False
                        Exit For
                    End If
                Next
                sResult3RealVal = sResult3RealVal + 1
                Filter = "LabelNum=" & CurrentParameterIndex
                Filter &= " AND LowLimit >= " & Format(dResult3, "0.0#")
                Filter &= " AND LowLimit < " & Format(dResult3_100, "0.0#")
                Filter &= " AND LowLimitH = " & Format(sResult3RealVal, "0.0#")
                dvRules.RowFilter = Filter
            Loop

            If isSent = True Then
                ResOrder = ""
                Return "0"
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function Calc_GetFamily() As String
        Try
            Dim FindString As String = clsQuatation.SetSTNTEMPLabel()
            Dim s As String = clsQuatation.GetFamily(FindString)

            'Dim ItemRefrence As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber)
            'Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            'Dim Params As New SqlParams
            'Dim dtFam As DataTable

            'Params.Add(New SqlParam("@ItemRefrence", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, ItemRefrence, 10))

            'dtFam = oSql.ExecuteSPReturnDT("USP_GetFamilyNo", Params)

            'If Not dtFam Is Nothing AndAlso dtFam.Rows.Count > 0 Then
            '    Return dtFam.Rows.Item(0).ToString
            'End If
            Return s
        Catch ex As Exception
            Throw
        End Try
    End Function



    'Private Function Calc_GetFamily() As String

    '    'Return CustomerNumber
    '    Dim ws As New GetAS400Data.Service1
    '    Dim s As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber)

    '    ' Dim temp As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.)

    '    '5622420

    '    Dim refno As String = ""
    '    refno = ws.GetFamilyNo(s)
    '    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.familyNo, refno)
    '    Return refno


    'End Function

    'Private Function Calc_GetFamilyModification() As String
    '    'Return CustomerNumber
    '    Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.familyNo)
    'End Function

    'GetCustomerNum
    Private Function Calc_GetCustomerNum() As String
        'Return CustomerNumber
        Return StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
    End Function

    'GetCurrenciesRatio
    Private Function Calc_GetCurrenciesRatio(ByVal Result1 As Item, ByVal Result2 As Item) As String
        'TODO: not implemented
        Return "0"
    End Function

    'GetCurrency
    Private Function Calc_GetCurrency() As String
        'TODO: not implemented
        Return "0"
    End Function

    'GetReferenceData
    Private Function Calc_GetReferenceData(ByVal CatNumParam As Item, ByVal EntryNumParam As Item) As String
        Try
            Calc_GetReferenceData = Nothing
            Dim CatNum As String = CatNumParam.Expression
            Dim EntryNum As Integer = EntryNumParam.Expression
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim dt As DataTable
            Dim sSql As String = "SELECT ltrim(rtrim(dvalue)) as dvalue, Inch_MM From glb_TCatalogData WHERE itm_cat = '" & CatNum & "' and dsg_entry=" & EntryNum
            dt = oSql.GetDataTable(sSql)

            If dt.Rows.Count > 0 Then
                If dt.Rows.Count = 1 Then
                    If IsNumeric(dt.Rows(0)("dvalue")) Then
                        dt.Rows(0)("dvalue") = Format(CDec(dt.Rows(0)("dvalue")), "#.#")
                    End If
                    Return dt.Rows(0)("dvalue").ToString
                Else
                    Dim dtParamTmp As DataTable = Parameters
                    Dim drParamTmp() As DataRow = dtParamTmp.Select("LabelNum=180")
                    If drParamTmp.Length > 0 Then
                        For i As Integer = 0 To dt.Rows.Count - 1
                            Dim sInch_MM As String
                            sInch_MM = Replace(drParamTmp(0)("Measure").ToString, "0", "I")
                            sInch_MM = Replace(drParamTmp(0)("Measure").ToString, "1", "M")

                            '16.05.10 Endmills MM
                            'Check if not mack problems in other lines (String.Trim)

                            If dt.Rows(i)("Inch_MM").ToString = sInch_MM Then
                                If IsNumeric(dt.Rows(i)("dvalue")) Then
                                    dt.Rows(i)("dvalue") = Format(CDec(dt.Rows(i)("dvalue")), "#.#")
                                End If
                                Return dt.Rows(i)("dvalue").ToString
                            Else
                                If IsNumeric(dt.Rows(i + 1)("dvalue")) Then
                                    dt.Rows(i + 1)("dvalue") = Format(CDec(dt.Rows(i + 1)("dvalue")), "#.#")
                                End If
                                Return dt.Rows(i + 1)("dvalue").ToString
                            End If

                            ''If dt.Rows(i)("Inch_MM").ToString = sInch_MM Then
                            ''    Return dt.Rows(i)("dvalue").ToString.Trim
                            ''Else
                            ''    Return dt.Rows(i + 1)("dvalue").ToString.Trim
                            ''End If
                        Next i
                    Else
                        Return 0
                    End If
                End If
            Else
                Return 0
            End If


        Catch ex As Exception
            Throw
        End Try

    End Function

    'Const
    Private Function Calc_Const(ByVal Result1 As Item, Optional SP_SETUP As String = "", Optional STN_SETUP As String = "", Optional QTY As String = "") As String
        Dim qn As String = ""
        Dim bc As String = ""
        Try
            qn = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False).ToString
            bc = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False).ToString
        Catch ex As Exception
        End Try

        Dim res1 As String = Result1.Expression

        Try



            If res1 = "SP_SETUP" Or res1 = "SPSETUP" Then
                AddUpdateConstant("SP_SETUP", SP_SETUP)
                Return SP_SETUP

            ElseIf res1 = "STN_SETUP" Or res1 = "STNSETUP" Then
                AddUpdateConstant("STN_SETUP", STN_SETUP)
                Return STN_SETUP

            Else
                Dim WorkCenter As Integer = clsBranch.GetWorkCenterID(bc) 'StateManager.GetValue(StateManager.Keys.s_WorkCenterID)


                If Len(res1) > 2 Then
                    If Left(res1, 1) = """" And Right(res1, 1) = """" Then
                        res1 = res1.Substring(1, res1.Length - 2)
                    End If
                End If

                Dim dt_m As DataTable = returnCurentModel() 'SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Model, "")
                Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString

                '--WorkCenterID=" & WorkCenter & " --> For the TC Fprmula
                '--WorkCenterID=0 --> For Price Formula

                Dim ModType As String = "Configuration"
                If clsQuatation.ACTIVE_OpenType = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                    ModType = "Modification"
                End If
                If Not IsNumeric(ModelNum) Then ModelNum = 0
                Dim WorkCenterID As String = clsBranch.GetWorkCenterID(bc) ' StateManager.GetValue(StateManager.Keys.s_WorkCenterID)
                If Not IsNumeric(WorkCenterID) Then WorkCenterID = "0"

                If IsNothing(QTY) Then
                    QTY = "0"
                End If
                If QTY = "" Then QTY = "0"


                Dim dt As DataTable = clsQuatation.GetQuotationConstants(ModelNum, ModType, WorkCenter, CInt(QTY))


                'Dim dt As DataTable = CacheManager.GetDataTable(CacheManager.Keys.Constants, "glb_tConstant", "ModelNum=" & ModelNum & " AND (WorkCenterID=" & WorkCenter & " OR WorkCenterID=0)", "", "*")
                'If dt.Rows.Count = 0 Then
                '    dt = CacheManager.GetDataTable(CacheManager.Keys.Constants, "glb_tConstant", "ModelNum=" & ModelNum, "", "*")
                'End If

                Dim dr() As DataRow = dt.Select("ConstName='" & res1 & "' AND (WorkCenterId= " & WorkCenterID & " OR WorkCenterId =0)", "WorkCenterID DESC")

                If dr.Length = 0 Then

                    '  GeneralException.WriteEvent("Bad Constant Result : " & res1 & " : QutNo : " & qn & " BranchCode : " & bc)

                    Throw New Exception("Bad Constant Result-ConstName='" & res1)

                End If

                ResOrder = ""
                AddUpdateConstant(res1, dr(0)("ConstValue").ToString)
                Return dr(0)("ConstValue").ToString
            End If

            'Dim res1 As String = Result1.Expression

            'If Len(res1) > 2 Then
            '    If Left(res1, 1) = """" And Right(res1, 1) = """" Then
            '        res1 = res1.Substring(1, res1.Length - 2)
            '    End If
            'End If

            'Dim dt_m As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Model, "")
            'Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString

            ''Dim dt As DataTable = CacheManager.GetDataTable(CacheManager.Keys.Constants, "glb_tConstant", "ModelNum=" & SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ModelNumber), "", "*")
            'Dim dt As DataTable = CacheManager.GetDataTable(CacheManager.Keys.Constants, "glb_tConstant", "ModelNum=" & ModelNum, "", "*")

            'Dim dr() As DataRow = dt.Select("ConstName='" & res1 & "'")
            'If dr.Length <> 1 Then
            '    Throw New Exception("Bad Constant Result")
            'End If

            'ResOrder = ""
            'Return dr(0)("ConstValue").ToString
        Catch ex As Exception
            ' GeneralException.WriteEventErrors("Bad Constant Result : QutNo : " & qn & " BranchCode : " & bc, GeneralException.e_LogTitle.PARSER.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "Calc_Const", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw New Exception("Bad Constant Result-ConstName='" & res1)

            Throw
        End Try
    End Function
    Private Sub AddUpdateConstant(Constname As String, ConstValue As String)
        Try
            Dim bc As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)
            Dim qn As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)

            Dim dt As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Consts, "")
            If Not IsNothing(dt) Then
                For Each row As DataRow In dt.Rows
                    If row("ConstName") = Constname Then
                        row("ConstValue") = ConstValue
                        row("QuotationNum") = qn
                        row("BranchCode") = bc
                        row("ConstValue") = ConstValue
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Consts, dt)
                        Exit Sub
                    End If
                Next
                dt.Rows.Add()
                dt.Rows(dt.Rows.Count - 1).Item("ConstName") = Constname
                dt.Rows(dt.Rows.Count - 1).Item("ConstValue") = ConstValue
                dt.Rows(dt.Rows.Count - 1).Item("QuotationNum") = qn
                dt.Rows(dt.Rows.Count - 1).Item("BranchCode") = bc
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Consts, dt)
            Else
                dt.Rows.Add()
                dt.Rows(dt.Rows.Count - 1).Item("ConstName") = Constname
                dt.Rows(dt.Rows.Count - 1).Item("ConstValue") = ConstValue
                dt.Rows(dt.Rows.Count - 1).Item("QuotationNum") = qn
                dt.Rows(dt.Rows.Count - 1).Item("BranchCode") = bc
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Consts, dt)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    'GP1, GP2, GP3
    Private Function Calc_Gp(ByVal GPNumber As Integer) As String
        'Try
        '    '============================================
        '    'bDinamicGP => From TModel
        '    'For new quotation
        '    'bDinamicGP = False => Get FIRST GP in glb_TGP without filtering by Quantity and parameters
        '    'bDinamicGP = True => Get FIRST GP in glb_TGP with filtering by Quantity and parameters

        '    'For existing quotation
        '    'bDinamicGP = False => Get GP From TQuotationPrices
        '    'bDinamicGP = True => Get FIRST GP in glb_TGP with filtering by Quantity and parameters

        '    'Quantity - Must Be Initialized
        '    '============================================

        '    If Quantity <= 0 Then
        '        Throw New Exception("Wrong Quantity Parameter")
        '    End If

        '    Dim ColumnName As String = "GP" & GPNumber.ToString
        '    Dim ModelNum As String = CType(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Model, ""), DataTable).Rows(0)("ModelNum").ToString
        '    Dim dt_Price As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "")
        '    Dim iRPriceCount As Int16 = dt_Price.Rows.Count
        '    If iRPriceCount = 1 Then 'one empty row
        '        If dt_Price.Rows(0)("QTY").ToString.Trim = "" Then
        '            iRPriceCount = 0
        '        End If
        '    End If

        '    Dim dt_GP As DataTable = CacheManager.GetDataTable(CacheManager.Keys.GP, "glb_tGp", "ModelNum=" & ModelNum & " and " & ColumnName & " < 9999", "", "*")

        '    ''--GP_Demanic=True or (dt_price rows = 0 and quotation=exist)

        '    Dim bDenamicGP As Boolean = CType(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Model, ""), DataTable).Rows(0)("bDinamicGP")


        '    If bDenamicGP = True Or
        '        (
        '            iRPriceCount = 0 And
        '            (
        '                SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationStatus) = SessionManager.QuotationStatus.ExistQutStartFromList Or
        '                SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationStatus) = SessionManager.QuotationStatus.ExistQutStartFromRev Or
        '                SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationStatus) = SessionManager.QuotationStatus.ExistQutStartFromAs400
        '                )
        '        ) Then


        '        If dt_GP.Rows.Count = 1 Then
        '            Return dt_GP.Rows(0)(ColumnName).ToString
        '        End If

        '        Dim filter As String
        '        Dim pVal As String
        '        Dim arrpVal() As String
        '        Dim ParamCountValue As String
        '        filter = "(QtyL <= " & Quantity & " and " & "QtyH > " & Quantity & " and Operation='<>')"
        '        filter &= " or (" & Quantity & "> QtyH and Operation='>')"
        '        filter &= " or (" & Quantity & "< QtyL and Operation='<')"
        '        filter &= " or (" & Quantity & "= QtyH and Operation='=')"
        '        Dim dr() As DataRow = dt_GP.Select(filter)
        '        If dr.Length = 0 Then
        '            ResOrder = "" : SetGpSesstion(ColumnName, "0") : Return "0"
        '        End If

        '        Dim dt_Params As DataTable
        '        dt_Params = clsModel.GetModelGPParam(ModelNum)
        '        If Not dt_Params Is Nothing AndAlso dt_Params.Rows.Count <> 1 Then
        '            ResOrder = "" : SetGpSesstion(ColumnName, "0") : Return "0"
        '        End If

        '        Dim ParamNameGp As String = dt_Params.Rows(0)("Label").ToString
        '        Dim dt_Par As DataTable
        '        dt_Par = Parameters.Copy
        '        Dim dr_Par() As DataRow = dt_Par.Select("Label='" & ParamNameGp & "'")
        '        If dr_Par.Length.ToString <> "1" Then
        '            ResOrder = "" : SetGpSesstion(ColumnName, "0") : Return "0"
        '        End If

        '        Dim ParamNum As Integer = CInt(dr_Par(0)("TabIndex")) - 1
        '        For Each d As DataRow In dr
        '            'ParamNum = d("ParamNum") - 1
        '            pVal = d("ParamVal").ToString
        '            ParamCountValue = Parameters.Rows(ParamNum)("Measure").ToString
        '            arrpVal = Split(pVal, "@")
        '            For Each s As String In arrpVal
        '                If UCase(ParamCountValue) = UCase(s) Then
        '                    ResOrder = ""
        '                    SetGpSesstion(ColumnName, d(ColumnName).ToString)
        '                    Return d(ColumnName).ToString
        '                End If
        '            Next
        '        Next



        '    Else
        '        ResOrder = ""
        '        If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationStatus) = SessionManager.QuotationStatus.ExistQutStartFromList Or _
        '             SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationStatus) = SessionManager.QuotationStatus.ExistQutStartFromRev Or _
        '             SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationStatus) = SessionManager.QuotationStatus.ExistQutStartFromAs400 Then
        '            'Exist quotation and dt_Price.Rows.Count > 0
        '            If dt_Price.Rows(0)(ColumnName).ToString <> "" Then
        '                SetGpSesstion(ColumnName, dt_Price.Rows(0)(ColumnName).ToString)
        '                Return dt_Price.Rows(0)(ColumnName).ToString
        '            Else
        '                SetGpSesstion(ColumnName, "0")
        '                Return "0"
        '            End If
        '        Else 'New quotation
        '            If dt_GP.Rows.Count > 0 Then
        '                SetGpSesstion(ColumnName, dt_GP.Rows(0)(ColumnName).ToString)
        '                Return dt_GP.Rows(0)(ColumnName).ToString

        '            Else
        '                SetGpSesstion(ColumnName, "0")
        '                Return "0"
        '            End If
        '        End If
        '    End If

        '    ResOrder = ""
        '    SetGpSesstion(ColumnName, "0")
        '    Return "0"

        'Catch ex As Exception
        '    Throw
        'End Try


    End Function
    Public Function Calc_TFR_STNCustomerPrice() As String
        Try
            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TFR_STNBranchPrice, False) Is Nothing Or SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TFR_STNBranchPrice, False) = "" Then
                Dim bc As String = "IS" ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False).ToString
                Dim ItemNumber As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber, False).ToString

                Dim cust As String = clsBranch.GetBranchNumberCode(bc) ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchNumber, False)

                If bc = "IS" Or bc = "XZ" Then
                    cust = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
                End If

                Dim dt As DataTable = GAL.GetGalData("PRICE", bc, ConfigurationManager.AppSettings("AS400APPpathForGetData"), cust, "", "", "", "", "", "", "", True)

                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerrice, dt.Rows(0).Item("listPrice"))
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchPrice, dt.Rows(0).Item("netPrice"))
                    Return dt.Rows(0).Item("netPrice").ToString
                End If
                Return ""

                'Dim brenchNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchNumber, False).ToString

                ''Dim sP As String = clsStandard.Run_ODC3_inWebSwrvice(bc, 311000, ItemNumber, 1, "0")
                'Dim sP As String = clsStandard.FindGal6Prices("TFR", brenchNo, "IS", ItemNumber, CustomerNumber, bc, 1, 0)
                'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerPrice, sP)
                'Return sP
            Else
                Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TFR_STNBranchPrice, False).ToString
                'Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerPrice, False).ToString
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Function Calc_SUBGP() As String
        Try

            'SUBGP = [1 – STNTRF / STNNET] 
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.SUBGP, "0")

            Dim sNET_STNCustomerPrice As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerPrice, False)
            Dim sTFR_STNBranchPrice As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TFR_STNBranchPrice, False)

            If sNET_STNCustomerPrice Is Nothing Then
                Return 0
            End If
            If sTFR_STNBranchPrice Is Nothing Then
                Return 0
            End If
            '-----------------------
            If Not IsNumeric(sNET_STNCustomerPrice) Then
                Return 0
            End If
            If Not IsNumeric(sTFR_STNBranchPrice) Then
                Return 0
            End If
            '-----------------------
            If sNET_STNCustomerPrice = "0" Then
                Return 0
            End If
            '-----------------------
            Dim sv As Decimal = 1 - (CDec(sTFR_STNBranchPrice) / CDec(sNET_STNCustomerPrice))
            If IsNumeric(sv) Then
                sv = Format(sv, "0.##")
            End If

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.SUBGP, sv)

            Try
                If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FAMILYGP_MaxQTY, False).ToString <> "" AndAlso IsNumeric(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FAMILYGP_MaxQTY, False).ToString) Then
                    Dim MXQ As Integer = CInt(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FAMILYGP_MaxQTY, False).ToString)
                    If Quantity <= MXQ Then
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.SUBGP, SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FAMILYGP, False))
                        sv = CDec(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FAMILYGP, False))
                    End If
                End If
            Catch ex As Exception

            End Try


            Return sv

        Catch ex As Exception
            Return 0
        End Try
    End Function
    Public Function Calc_FAMILYGP() As String
        Try
            Dim fg As String = ""
            Try
                fg = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FAMILYGP, False)
            Catch ex As Exception

            End Try

            If fg = "" Or fg = "0" Then

                Dim ModelNum As String = clsQuatation.ACTIVE_ModelNumber
                Dim BranchCode As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)
                Dim CustomerType As String = StateManager.GetValue(StateManager.Keys.s_CustomerType)
                Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
                Dim Params As New SqlParams
                Dim dt As DataTable
                Dim ModelType As String = clsQuatation.ACTIVE_OpenTypeName

                Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 4))
                Params.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, BranchCode))
                'Params.Add(New SqlParam("@Quantity", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, Quantity, 4))
                Params.Add(New SqlParam("@CustomerType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, CustomerType))
                Params.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, ModelType))


                dt = oSql.ExecuteSPReturnDT("[USP_GetModelBranchCusromerGP]", Params)

                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FAMILYGP, dt.Rows(0).Item("GPValue").ToString)
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FAMILYGP_MaxQTY, dt.Rows(0).Item("MaxQTY").ToString)


                    Return dt.Rows(0).Item("GPValue").ToString
                Else
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FAMILYGP, 0)
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FAMILYGP_MaxQTY, 0)

                    Return "0"
                End If
            Else
                Return fg
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function Calc_NET_STNCustomerPrice() As String

        Dim s As String = "0"
        Try
            s = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerPrice, False).ToString
        Catch ex As Exception

        End Try

        'Or (IsNumeric(s) = True AndAlso Not CDbl(s) > 0)
        Try
            If s Is Nothing Or s = "" Then
                Dim bc As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False).ToString
                Dim ItemNumber As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber, False).ToString
                Dim cust As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)

                Dim StringToGet As String = "fcditems/" & ItemNumber & "/customer/" & cust


                Dim dt As DataTable = GAL.GetGalData("PRICE", bc, ConfigurationManager.AppSettings("AS400APPpathForGetData"), cust, ItemNumber, "", "", "", "", "", "", True)

                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerPrice, dt.Rows(0).Item("netPrice"))
                    Return dt.Rows(0).Item("netPrice")
                End If
                Return ""

                'Dim brenchNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchNumber, False).ToString

                ''Dim sP As String = clsStandard.Run_ODC3_inWebSwrvice(bc, 311000, ItemNumber, 1, "0")
                'Dim sP As String = clsStandard.FindGal6Prices("NET", brenchNo, "IS", ItemNumber, CustomerNumber, bc, 1, 0)
                'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerPrice, sP)
                'Return sP
            Else
                Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerPrice, False).ToString
            End If


            '    Dim StandartTransferPrice As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.StandartTransferPrice)

            '    If StandartTransferPrice < 0 Then
            '        Throw New Exception("Wrong StandardPrice Parameter")
            '    End If

            '    Dim dt_m As DataTable

            '    Dim sStart As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)
            '    If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
            '        dt_m = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_ModelConfiguration, "")
            '    Else
            '        dt_m = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_ModelModification, "")
            '    End If

            '    Dim StandartMarketPrice As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.StandartMarketPrice)
            '    Dim CatalogNumber As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.CatalogNumber)

            '    Dim BranchCode As String

            '    BranchCode = StateManager.GetValue(StateManager.Keys.BranchCode)

            '    If dt_m.Rows(0)("WorkCenter").ToString <> StateManager.WorkCenterID.ISCAR_LTD AndAlso dt_m.Rows(0)("WorkCenter").ToString <> StateManager.WorkCenterID.TAEGUTEC_KORIA AndAlso dt_m.Rows(0)("WorkCenter").ToString <> StateManager.WorkCenterID.TUNGALOY_JAPAN Then
            '        '    StandardPrice = FormulaResult.CorrelatedWC_Prices(dt_m.Rows(0)("WorkCenter").ToString, CatalogNumber, StandartTransferPrice, StandartMarketPrice)
            '        Dim BranchCur As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TFRCurrency)
            '        Dim WCCur As String = dt_m.Rows(0)("Calc_CUR").ToString
            '        If WCCur <> BranchCur Then
            '            'Dim BranchCode As String = StateManager.GetValue(StateManager.Keys.BranchCode)

            '            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            '            Dim oParams As New SqlParams()
            '            oParams = New SqlParams
            '            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            '            oParams.Add(New SqlParam("@FromCur", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCur, 3))
            '            oParams.Add(New SqlParam("@ToCur", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, WCCur, 3))
            '            oParams.Add(New SqlParam("@RateValue", SqlParam.ParamType.ptFloat, SqlParam.ParamDirection.pdOutput, Nothing))
            '            oSql.ExecuteSP("USP_GetRates", oParams)
            '            If Not oParams.GetParameter("@RateValue").Value Is Nothing AndAlso oParams.GetParameter("@RateValue").Value.ToString <> "" Then
            '                Dim curRate As Double
            '                curRate = oParams.GetParameter("@RateValue").Value
            '                StandartTransferPrice = StandartTransferPrice * curRate
            '            End If

            '        End If
            '    End If

            '    Dim WorkCenterId_Model As String = dt_m.Rows(0)("WorkCenter").ToString

            '    Return StandartTransferPrice
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function SetNewGPNUM_ISONumbetr(dtModel As DataTable, ParamName As String) As String
        Try
            Dim newmae As String = ""
            Dim dt As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamsFromCatalog, "")
            For Each r As DataRow In dtModel.Rows
                'If r.Item("GPNUM_ISO_Alternate") = "0" Then
                '    Return ""
                'End If
                If r.Item("GPNUM_ISO_Alternate") <> "0" AndAlso ParamName.Replace("{", "").Replace("}", "").Replace("STN", "").Replace(" ", "") = r.Item("CostName") Then
                    For Each rr As DataRow In dt.Rows
                        If r.Item("GPNUM_ISO_Alternate") <> "0" Then
                            If r.Item("GPNUM_ISO_Alternate") = rr.Item("GPNUM_ISO") Then

                                newmae = rr.Item("GIPRGP_ISO").ToString.Trim
                                Return newmae
                            End If
                        End If
                    Next
                End If
            Next
            Return newmae

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Function Calc_StnWithRefernce(ParamName As String) As String
        Try

            Dim dtModif As DataTable = clsQuatation.GetActiveQuotation_DTparams
            Dim siso As String = SetNewGPNUM_ISONumbetr(dtModif, ParamName)
            If siso <> "" Then
                ParamName = "{STN " & siso & "}"

            End If


            Dim pn As String = ParamName.Substring(5, ParamName.Length - 6)
            Dim sitemNumber As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber).ToString
            Dim sLang As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.language).ToString
            Dim svers As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.vers).ToString

            Dim s As String = "-;_;/;:; ;"

            If sitemNumber <> "" Then
                Dim dt As DataTable = Nothing

                dt = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamsFromCatalog, "")

                Dim dv As DataView
                '= SemiApp_bl.CatalogIscarData.GetItemParametersMobileISO(sitemNumber, svers, sLang)
                If dt Is Nothing Then
                    dv = SemiApp_bl.CatalogIscarData.GetItemParametersMobileISO(sitemNumber, svers, sLang, False)
                    dt = dv.Table
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamsFromCatalog, dt)
                End If
                'dt = dv.ToTable
                'Dim dt1 As DataTable = dv.Table



                For Each r As DataRow In dt.Rows
                    If ParamName.ToUpper.Contains("STN GRADE") Then

                        Dim bFileUploadDocMngPath As String = ConfigurationManager.AppSettings("UserCATALOG_WSLocaly").ToString.Trim
                        Dim sGetDataItem As String = ""
                        If bFileUploadDocMngPath = "TRUE" Then
                            Dim ws As New wsCATIscarDataLocal.IscarData
                            ws.Timeout = 180000
                            sGetDataItem = ws.GetItemGrade(sitemNumber)
                            ws = Nothing
                        Else
                            Dim ws As New wsCATIscarData.IscarData
                            ws.Timeout = 180000
                            sGetDataItem = ws.GetItemGrade(sitemNumber)
                            ws = Nothing
                        End If


                        If Not sGetDataItem Is Nothing Then
                            Return sGetDataItem
                        Else
                            Return 0
                        End If
                    Else
                        If "{STN " & r.Item("GIPRGP_ISO").ToString.Trim.ToUpper & "}" = ParamName.ToUpper Then
                            If Not s.Contains(r.Item("Val").ToString.Trim & ";") Then
                                Return r.Item("Val")
                            End If
                        End If
                    End If

                Next


            End If

            Return "-1"

        Catch ex As Exception

            Throw
        End Try
    End Function
    Public Function Calc_Rate(ByVal ConstName As String) As String
        Try
            Calc_Rate = Nothing
            '!!!!!!!!!!!!!!!!!
            'Session Rate - Must Be Initialized
            '!!!!!!!!!!!!!!!!!
            Select Case ConstName
                Case "RATE"
                    If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.RateTFR_USD) < 0 Then
                        Throw New Exception("Wrong Rate Parameter")
                    End If
                    ResOrder = ""
                    Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.RateTFR_USD)
                Case "EURTOUSD"
                    ResOrder = ""
                    'Dim ETU As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.RateEUR_USD)
                    'If IsNumeric(ETU) Then
                    '    'Return Format(CDbl(ETU), "0.####")
                    '    Return LocalizationManager.UnCulturingNumber(Format(CDbl(LocalizationManager.CulturingNumber(ETU)), "0.####"))
                    'End If
                Case "TFRTOEUR"
                    ResOrder = ""
                    'Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.RateTFR_EUR)
                Case "WCTOUSD"
                    ResOrder = ""
                    'Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.RateWC_USD)
                Case "RATEMKTUSD"  'TFRRate - MKT Currency from tCustomers
                    ResOrder = ""
                    Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.RateMKT_USD)
                Case "RATETFRUSD" 'TFRRate - TFR Currency from branchdetails
                    ResOrder = ""
                    Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.RateTFR_USD)
            End Select
        Catch ex As Exception
            Throw
        End Try
    End Function

    'Public Shared Function Calc_TC() As Decimal
    '    Try
    '        Dim bc As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode)
    '        Dim itemNumber As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber)
    '        If itemNumber <> "" Then
    '            Dim dTC_LTD As Decimal
    '            Dim CustomerNum As Integer = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.CustomerNumber)
    '            clsStandard.Get_VAVST_GAL6(itemNumber, dTC_LTD, 0, 0, 0, True, bc)
    '            Return dTC_LTD
    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function

    'Public Shared Function Calc_RATEFACT() As Decimal
    '    Try
    '        Try
    '            Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.RATE_FACTOR)
    '        Catch ex As Exception
    '            Throw
    '        End Try
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function

    Public Shared Function Calc_NET_STNCustomerCurrency() As String
        Try
            Try
                'Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerCurrency)
                Return StateManager.GetValue(StateManager.Keys.s_STNCustomerCurrency, True)
            Catch ex As Exception
                Throw
            End Try
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function Calc_TFR_STNBranchCurrency() As String
        Try
            Try
                Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TFR_STNBranchCurrency)
            Catch ex As Exception
                Throw
            End Try
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function Calc_GetRight(ByVal itm As Item, ByVal ConditionLen As Item) As String
        Try
            Dim res1 As String = Right(itm.Expression, Integer.Parse(ConditionLen.Expression))

            ResOrder = ""
            Return res1
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function Calc_GetReplace(ByVal itm As Item, ByVal Fined As Item, ByVal Replacement As Item) As String
        Try
            Dim res1 As String = Replace(itm.Expression, Fined.Expression, Replacement.Expression)

            For i As Integer = 0 To UBound(SpecialLabels, 1)
                If SpecialLabels(i, 1) = Fined.Expression Or """" & SpecialLabels(i, 1) & """" = Fined.Expression Then
                    res1 = Replace(res1, SpecialLabels(i, 0), Replacement.Expression)
                    res1 = Replace(res1, """", "")
                End If
            Next

            ResOrder = ""
            Return res1
        Catch ex As Exception
            Throw
        End Try
    End Function


    Public Function Calc_Qty() As String
        Try
            '!!!!!!!!!!!!!!!!!
            'Quantity - Must Be Initialized
            '!!!!!!!!!!!!!!!!!
            If Quantity < 0 Then
                Throw New Exception("Wrong Quantity Parameter")
            End If

            ResOrder = ""
            Return Quantity
        Catch ex As Exception
            Throw
        End Try
    End Function


    Public Function Calc_QTY_Fct_AllRows() As String

        If Quantity <= 0 Then
            Throw New Exception("Wrong Quantity Parameter")
        End If
        Try

            '"{LU}=0.3;{....}=Rougxx"

            Dim dt As DataTable = clsPrices.GetParametersForFactors(ModelNum, "FindQtyFactor", Nothing)
            Dim strLoop As String = ""
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Dim ParamNameString As String = ""
                Dim ParamNameNumber As String = ""
                Dim ParamValue As String = ""
                For Each r As DataRow In dt.Rows
                    ParamNameString = r.Item("paramcode").ToString
                    ParamNameNumber = FormulaResult.Conver_InputLimit_Formula(ParamNameString, clsQuatation.GetActiveQuotation_DTparams)

                    Dim oFormula As New FormulaResult(ParamNameNumber, clsQuatation.GetActiveQuotation_DTparams, 0, Nothing)
                    If oFormula.ToString.Trim <> "" Then
                        Dim cust As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
                        oFormula.CustomerNumber = cust
                        oFormula.Quantity = Nothing
                        ParamValue = oFormula.ParseAndCalculate
                        If ParamValue.ToString.ToUpper = "UNVISIBLE" Then
                            ParamValue = "0"
                        End If
                    End If

                    If IsNumeric(r.Item("SerachValue").ToString) Then
                        strLoop &= ParamNameString & "=" & ParamValue & ";"
                    Else
                        strLoop &= ParamNameString & "=""" & ParamValue & """;"
                    End If
                Next
                If strLoop.Length > 0 Then
                    strLoop = strLoop.Substring(0, strLoop.Length - 1)
                End If

                Dim sStart As String = clsQuatation.ACTIVE_OpenType
                Dim sop As String = ""
                If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                    sop = "Configuration"
                ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                    sop = "Modification"
                End If


                If strLoop <> "" Then

                    Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
                    Dim Params As New SqlParams
                    Dim dtFactors As DataTable

                    Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 4))
                    Params.Add(New SqlParam("@ParamArrValues", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, strLoop, 250))
                    Params.Add(New SqlParam("@Quantity", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, Quantity, 4))
                    Params.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sop))

                    dtFactors = oSql.ExecuteSPReturnDT("[USP_GetQTYFactorsParameters]", Params)

                    If dtFactors Is Nothing OrElse dtFactors.Rows.Count = 0 Then
                        Params = New SqlParams
                        Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 4))
                        Params.Add(New SqlParam("@ParamArrValues", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, strLoop, 250))
                        Params.Add(New SqlParam("@Quantity", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, 0, 4))
                        Params.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sop))
                        dtFactors = oSql.ExecuteSPReturnDT("[USP_GetQTYFactorsParameters]", Params)
                    End If

                    Dim dtFactorsX As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FactorsQty, "")
                    'If dtFactorsX Is Nothing Or dtFactorsX.Rows.Count = 0 Then
                    '    dtFactorsX = dtFactors
                    'End If


                    If Not dtFactors Is Nothing AndAlso dtFactors.Rows.Count > 0 Then

                        'Params = New SqlParams

                        'Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 4))
                        'Params.Add(New SqlParam("@ParamArrValues", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, strLoop, 250))
                        'Params.Add(New SqlParam("@Quantity", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, 0, 4))
                        'Params.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sop))

                        'dtFactorsX = oSql.ExecuteSPReturnDT("[USP_GetQTYFactorsParameters]", Params)
                        'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FactorsQty, dtFactorsX)
                        If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FactorsQty, "") Is Nothing Then
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FactorsQty, dtFactors)
                            dtFactorsX = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FactorsQty, "")
                        Else
                            If dtFactorsX.Rows(dtFactorsX.Rows.Count - 1).Item("QTY").ToString <> Quantity Then
                                dtFactorsX.Rows.Add()
                            End If
                        End If



                        If dtFactors.Rows(0).Item("FormulaFactorValue").ToString.Trim = "" AndAlso IsNumeric(dtFactors.Rows(0).Item("FactorValue").ToString) Then
                            For ii As Integer = 0 To dtFactors.Columns.Count - 1
                                dtFactorsX.Rows(dtFactorsX.Rows.Count - 1).Item(ii) = dtFactors.Rows(0).Item(ii)
                            Next
                            dtFactorsX.Rows(dtFactorsX.Rows.Count - 1).Item("FormulaFactorValue") = dtFactors.Rows(0).Item(dtFactors.Rows(0).Item("FactorValue").ToString)
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FactorsQty, dtFactorsX)
                            Return CDbl(dtFactors.Rows(0).Item("FactorValue"))
                        Else
                            'Return Get_FactorValue(CDbl(dtFactors.Rows(0).Item("FactorValue")))

                            '----
                            Dim res As String = ""
                            Dim value As String = ""
                            Dim oFormula As New FormulaResult(dtFactors.Rows(0).Item("FormulaFactorValue").ToString.Replace("SP_SETUP", "SPSETUP").Replace("STN_SETUP", "STNSETUP"), Parameters, 0, Nothing)

                            oFormula.Quantity = Quantity
                            oFormula.SPSETUP = dtFactors.Rows(0).Item("SP_SETUP").ToString
                            oFormula.STNSETUP = dtFactors.Rows(0).Item("STN_SETUP").ToString
                            res = oFormula.ParseAndCalculate

                            Try
                                Dim s As String = ""
                                s = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ConstFormula, False).ToString
                                If s = "" Then
                                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ConstFormula, oFormula.Formula.ToString)
                                End If
                            Catch ex As Exception

                            End Try

                            For ii As Integer = 0 To dtFactors.Columns.Count - 1
                                dtFactorsX.Rows(dtFactorsX.Rows.Count - 1).Item(ii) = dtFactors.Rows(0).Item(ii)
                            Next
                            dtFactorsX.Rows(dtFactorsX.Rows.Count - 1).Item("Qty") = Quantity
                            dtFactorsX.Rows(dtFactorsX.Rows.Count - 1).Item("FactorValue") = res
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FactorsQty, dtFactorsX)

                            Return res
                            '----
                        End If

                    End If


                    Return "0"

                End If
            End If

        Catch ex As Exception
            Throw
        End Try

        Return "0"
    End Function
    Public Function Calc_QTY_Fct() As String

        If Quantity <= 0 Then
            Throw New Exception("Wrong Quantity Parameter")
        End If
        Try

            '"{LU}=0.3;{....}=Rougxx"

            Dim dt As DataTable = clsPrices.GetParametersForFactors(ModelNum, "FindQtyFactor", Nothing)
            Dim strLoop As String = ""
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Dim ParamNameString As String = ""
                Dim ParamNameNumber As String = ""
                Dim ParamValue As String = ""
                For Each r As DataRow In dt.Rows
                    ParamNameString = r.Item("paramcode").ToString
                    ParamNameNumber = FormulaResult.Conver_InputLimit_Formula(ParamNameString, clsQuatation.GetActiveQuotation_DTparams)

                    Dim oFormula As New FormulaResult(ParamNameNumber, clsQuatation.GetActiveQuotation_DTparams, 0, Nothing)
                    If oFormula.ToString.Trim <> "" Then
                        Dim cust As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
                        oFormula.CustomerNumber = cust
                        oFormula.Quantity = Nothing
                        ParamValue = oFormula.ParseAndCalculate
                        If ParamValue.ToString.ToUpper = "UNVISIBLE" Then
                            ParamValue = "0"
                        End If
                    End If

                    If IsNumeric(r.Item("SerachValue").ToString) Then
                        strLoop &= ParamNameString & "=" & ParamValue & ";"
                    Else
                        strLoop &= ParamNameString & "=""" & ParamValue & """;"
                    End If
                Next
                If strLoop.Length > 0 Then
                    strLoop = strLoop.Substring(0, strLoop.Length - 1)
                End If

                Dim sStart As String = clsQuatation.ACTIVE_OpenType
                Dim sop As String = ""
                If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                    sop = "Configuration"
                ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                    sop = "Modification"
                End If
                If strLoop <> "" Then

                    Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
                    Dim Params As New SqlParams
                    Dim dtFactors As DataTable

                    Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 4))
                    Params.Add(New SqlParam("@ParamArrValues", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, strLoop, 250))
                    Params.Add(New SqlParam("@Quantity", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, Quantity, 4))
                    Params.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sop))

                    dtFactors = oSql.ExecuteSPReturnDT("[USP_GetQTYFactorsParameters]", Params)

                    If dtFactors Is Nothing Then
                        Params = New SqlParams
                        Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 4))
                        Params.Add(New SqlParam("@ParamArrValues", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, strLoop, 250))
                        Params.Add(New SqlParam("@Quantity", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, 0, 4))
                        Params.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sop))
                        dtFactors = oSql.ExecuteSPReturnDT("[USP_GetQTYFactorsParameters]", Params)
                    ElseIf dtFactors.Rows.Count = 0 Then
                        Params = New SqlParams
                        Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 4))
                        Params.Add(New SqlParam("@ParamArrValues", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, strLoop, 250))
                        Params.Add(New SqlParam("@Quantity", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, 0, 4))
                        Params.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sop))
                        dtFactors = oSql.ExecuteSPReturnDT("[USP_GetQTYFactorsParameters]", Params)
                    End If
                    Dim bStrValANDNotContainQuantyty As Boolean = False
                    If Not dtFactors Is Nothing AndAlso dtFactors.Rows.Count = 1 AndAlso dtFactors.Rows(0).Item("Qty").ToString = "0" AndAlso dtFactors.Rows(0).Item("QtyString").ToString <> "" AndAlso dtFactors.Rows(0).Item("QtyString").ToString <> "0" Then
                        Dim s As String = dtFactors.Rows(0).Item("QtyString").ToString
                        If s <> "" AndAlso s.Contains(";") AndAlso Not s.EndsWith(";") Then
                            s &= ";"
                        End If
                        If Not s.Contains(Quantity & ";") AndAlso Not s.Contains(";" & Quantity) Then
                            bStrValANDNotContainQuantyty = True
                        End If
                    End If

                    If Not dtFactors Is Nothing AndAlso dtFactors.Rows.Count > 0 AndAlso bStrValANDNotContainQuantyty = False Then
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempQTYFct, "")
                        Params = New SqlParams
                        Dim dtFactorsX As DataTable
                        Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 4))
                        Params.Add(New SqlParam("@ParamArrValues", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, strLoop, 250))
                        Params.Add(New SqlParam("@Quantity", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, 0, 4))
                        Params.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sop))

                        dtFactorsX = oSql.ExecuteSPReturnDT("[USP_GetQTYFactorsParameters]", Params)
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FactorsQty, dtFactorsX)

                        If dtFactors.Rows(0).Item("FormulaFactorValue").ToString.Trim = "" AndAlso IsNumeric(dtFactors.Rows(0).Item("FactorValue").ToString) Then

                            Return CDbl(dtFactors.Rows(0).Item("FactorValue"))
                        Else
                            'Return Get_FactorValue(CDbl(dtFactors.Rows(0).Item("FactorValue")))

                            '----
                            Dim res As String = ""
                            Dim value As String = ""
                            Dim oFormula As New FormulaResult(dtFactors.Rows(0).Item("FormulaFactorValue").ToString.Replace("SP_SETUP", "SPSETUP").Replace("STN_SETUP", "STNSETUP"), Parameters, 0, Nothing)

                            oFormula.Quantity = Quantity
                            oFormula.SPSETUP = dtFactors.Rows(0).Item("SP_SETUP").ToString
                            oFormula.STNSETUP = dtFactors.Rows(0).Item("STN_SETUP").ToString
                            res = oFormula.ParseAndCalculate
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempQTYFct, res)
                            'SetQTYFct(res)
                            Try
                                Dim s As String = ""
                                s = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ConstFormula, False).ToString
                                If s = "" Then
                                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ConstFormula, oFormula.Formula.ToString)
                                End If
                            Catch ex As Exception

                            End Try

                            Return res
                            '----
                        End If

                    End If


                    Return "0"

                End If
            End If

        Catch ex As Exception
            Throw
        End Try

        Return "0"
    End Function

    Private Sub SetQTYFct(QTYFctRES As Decimal)
        Try
            Dim dt As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "")
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                For idtp As Integer = 0 To dt.Rows.Count - 1
                    If dt.Rows(idtp).Item("QTY").ToString = "" Then
                        dt.Rows(idtp).Item("QTYFct") = QTYFctRES
                        Exit For
                    End If
                Next
            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Function Calc_QTY_Fct061123() As String

        If Quantity <= 0 Then
            Throw New Exception("Wrong Quantity Parameter")
        End If
        Try

            '"{LU}=0.3;{....}=Rougxx"

            Dim dt As DataTable = clsPrices.GetParametersForFactors(ModelNum, "FindQtyFactor", Nothing)
            Dim strLoop As String = ""
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Dim ParamNameString As String = ""
                Dim ParamNameNumber As String = ""
                Dim ParamValue As String = ""
                For Each r As DataRow In dt.Rows
                    ParamNameString = r.Item("paramcode").ToString
                    ParamNameNumber = FormulaResult.Conver_InputLimit_Formula(ParamNameString, clsQuatation.GetActiveQuotation_DTparams)

                    Dim oFormula As New FormulaResult(ParamNameNumber, clsQuatation.GetActiveQuotation_DTparams, 0, Nothing)
                    If oFormula.ToString.Trim <> "" Then
                        Dim cust As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
                        oFormula.CustomerNumber = cust
                        oFormula.Quantity = Nothing
                        ParamValue = oFormula.ParseAndCalculate
                        If ParamValue.ToString.ToUpper = "UNVISIBLE" Then
                            ParamValue = "0"
                        End If
                    End If

                    If IsNumeric(r.Item("SerachValue").ToString) Then
                        strLoop &= ParamNameString & "=" & ParamValue & ";"
                    Else
                        strLoop &= ParamNameString & "=""" & ParamValue & """;"
                    End If
                Next
                If strLoop.Length > 0 Then
                    strLoop = strLoop.Substring(0, strLoop.Length - 1)
                End If

                Dim sStart As String = clsQuatation.ACTIVE_OpenType
                Dim sop As String = ""
                If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                    sop = "Configuration"
                ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                    sop = "Modification"
                End If
                If strLoop <> "" Then

                    Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
                    Dim Params As New SqlParams
                    Dim dtFactors As DataTable

                    Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 4))
                    Params.Add(New SqlParam("@ParamArrValues", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, strLoop, 250))
                    Params.Add(New SqlParam("@Quantity", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, Quantity, 4))
                    Params.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sop))

                    dtFactors = oSql.ExecuteSPReturnDT("[USP_GetQTYFactorsParameters]", Params)

                    If dtFactors Is Nothing Then
                        Params = New SqlParams
                        Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 4))
                        Params.Add(New SqlParam("@ParamArrValues", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, strLoop, 250))
                        Params.Add(New SqlParam("@Quantity", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, 0, 4))
                        Params.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sop))
                        dtFactors = oSql.ExecuteSPReturnDT("[USP_GetQTYFactorsParameters]", Params)
                    ElseIf dtFactors.Rows.Count = 0 Then
                        Params = New SqlParams
                        Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 4))
                        Params.Add(New SqlParam("@ParamArrValues", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, strLoop, 250))
                        Params.Add(New SqlParam("@Quantity", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, 0, 4))
                        Params.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sop))
                        dtFactors = oSql.ExecuteSPReturnDT("[USP_GetQTYFactorsParameters]", Params)
                    End If
                    Dim bStrValANDNotContainQuantyty As Boolean = False
                    If Not dtFactors Is Nothing AndAlso dtFactors.Rows.Count = 1 AndAlso dtFactors.Rows(0).Item("Qty").ToString = "0" AndAlso dtFactors.Rows(0).Item("QtyString").ToString <> "" AndAlso dtFactors.Rows(0).Item("QtyString").ToString <> "0" Then
                        Dim s As String = dtFactors.Rows(0).Item("QtyString").ToString
                        If s <> "" AndAlso s.Contains(";") AndAlso Not s.EndsWith(";") Then
                            s &= ";"
                        End If
                        If Not s.Contains(Quantity & ";") AndAlso Not s.Contains(";" & Quantity) Then
                            bStrValANDNotContainQuantyty = True
                        End If
                    End If

                    If Not dtFactors Is Nothing AndAlso dtFactors.Rows.Count > 0 AndAlso bStrValANDNotContainQuantyty = False Then

                        Params = New SqlParams
                        Dim dtFactorsX As DataTable
                        Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 4))
                        Params.Add(New SqlParam("@ParamArrValues", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, strLoop, 250))
                        Params.Add(New SqlParam("@Quantity", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, 0, 4))
                        Params.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sop))

                        dtFactorsX = oSql.ExecuteSPReturnDT("[USP_GetQTYFactorsParameters]", Params)
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FactorsQty, dtFactorsX)

                        If dtFactors.Rows(0).Item("FormulaFactorValue").ToString.Trim = "" AndAlso IsNumeric(dtFactors.Rows(0).Item("FactorValue").ToString) Then

                            Return CDbl(dtFactors.Rows(0).Item("FactorValue"))
                        Else
                            'Return Get_FactorValue(CDbl(dtFactors.Rows(0).Item("FactorValue")))

                            '----
                            Dim res As String = ""
                            Dim value As String = ""
                            Dim oFormula As New FormulaResult(dtFactors.Rows(0).Item("FormulaFactorValue").ToString.Replace("SP_SETUP", "SPSETUP").Replace("STN_SETUP", "STNSETUP"), Parameters, 0, Nothing)

                            oFormula.Quantity = Quantity
                            oFormula.SPSETUP = dtFactors.Rows(0).Item("SP_SETUP").ToString
                            oFormula.STNSETUP = dtFactors.Rows(0).Item("STN_SETUP").ToString
                            res = oFormula.ParseAndCalculate

                            Try
                                Dim s As String = ""
                                s = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ConstFormula, False).ToString
                                If s = "" Then
                                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ConstFormula, oFormula.Formula.ToString)
                                End If
                            Catch ex As Exception

                            End Try

                            Return res
                            '----
                        End If

                    End If


                    Return "0"

                End If
            End If

        Catch ex As Exception
            Throw
        End Try

        Return "0"
    End Function

    Public Function Calc_QTY_Fct211021() As String

        If Quantity <= 0 Then
            Throw New Exception("Wrong Quantity Parameter")
        End If
        Try

            '"{LU}=0.3;{....}=Rougxx"

            Dim dt As DataTable = clsPrices.GetParametersForFactors(ModelNum, "FindQtyFactor", Nothing)
            Dim strLoop As String = ""
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Dim ParamNameString As String = ""
                Dim ParamNameNumber As String = ""
                Dim ParamValue As String = ""
                For Each r As DataRow In dt.Rows
                    ParamNameString = r.Item("paramcode").ToString
                    ParamNameNumber = FormulaResult.Conver_InputLimit_Formula(ParamNameString, clsQuatation.GetActiveQuotation_DTparams)

                    Dim oFormula As New FormulaResult(ParamNameNumber, clsQuatation.GetActiveQuotation_DTparams, 0, Nothing)
                    If oFormula.ToString.Trim <> "" Then
                        Dim cust As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
                        oFormula.CustomerNumber = cust
                        oFormula.Quantity = Nothing
                        ParamValue = oFormula.ParseAndCalculate
                        If ParamValue.ToString.ToUpper = "UNVISIBLE" Then
                            ParamValue = "0"
                        End If
                    End If

                    If IsNumeric(r.Item("SerachValue").ToString) Then
                        strLoop &= ParamNameString & "=" & ParamValue & ";"
                    Else
                        strLoop &= ParamNameString & "=""" & ParamValue & """;"
                    End If
                Next
                If strLoop.Length > 0 Then
                    strLoop = strLoop.Substring(0, strLoop.Length - 1)
                End If

                Dim sStart As String = clsQuatation.ACTIVE_OpenType
                Dim sop As String = ""
                If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                    sop = "Configuration"
                ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                    sop = "Modification"
                End If
                If strLoop <> "" Then

                    Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
                    Dim Params As New SqlParams
                    Dim dtFactors As DataTable

                    Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 4))
                    Params.Add(New SqlParam("@ParamArrValues", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, strLoop, 250))
                    Params.Add(New SqlParam("@Quantity", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, Quantity, 4))
                    Params.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sop))


                    dtFactors = oSql.ExecuteSPReturnDT("[USP_GetQTYFactorsParameters]", Params)

                    If Not dtFactors Is Nothing AndAlso dtFactors.Rows.Count > 0 Then

                        Params = New SqlParams
                        Dim dtFactorsX As DataTable
                        Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 4))
                        Params.Add(New SqlParam("@ParamArrValues", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, strLoop, 250))
                        Params.Add(New SqlParam("@Quantity", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, 0, 4))
                        Params.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sop))

                        dtFactorsX = oSql.ExecuteSPReturnDT("[USP_GetQTYFactorsParameters]", Params)
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FactorsQty, dtFactorsX)

                        If dtFactors.Rows(0).Item("FormulaFactorValue").ToString.Trim = "" AndAlso IsNumeric(dtFactors.Rows(0).Item("FactorValue").ToString) Then

                            Return CDbl(dtFactors.Rows(0).Item("FactorValue"))
                        Else
                            'Return Get_FactorValue(CDbl(dtFactors.Rows(0).Item("FactorValue")))

                            '----
                            Dim res As String = ""
                            Dim value As String = ""
                            Dim oFormula As New FormulaResult(dtFactors.Rows(0).Item("FormulaFactorValue").ToString.Replace("SP_SETUP", "SPSETUP").Replace("STN_SETUP", "STNSETUP"), Parameters, 0, Nothing)

                            oFormula.Quantity = Quantity
                            oFormula.SPSETUP = dtFactors.Rows(0).Item("SP_SETUP").ToString
                            oFormula.STNSETUP = dtFactors.Rows(0).Item("STN_SETUP").ToString
                            res = oFormula.ParseAndCalculate

                            Try
                                Dim s As String = ""
                                s = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ConstFormula, False).ToString
                                If s = "" Then
                                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ConstFormula, oFormula.Formula.ToString)
                                End If
                            Catch ex As Exception

                            End Try

                            Return res
                            '----
                        End If

                    End If


                    Return "0"

                End If
            End If

        Catch ex As Exception
            Throw
        End Try

        Return "0"
    End Function




    Public Function Calc_GetSCALE(itm As Item) As String
        Try
            'Dim dt_m As DataTable = returnCurentModel()
            'Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString
            'Dim dtRulesTmp As DataTable = clsModel.GetModelRules(ModelNum)

            'Dim LabelNum As Integer = CInt(itm.ParameterIndex)
            'Dim Order As String = Parameters.Rows(LabelNum - 1)("Order").ToString

            'Dim drRulesTmp() As DataRow = dtRulesTmp.Select("LabelNum= " & LabelNum & " AND Order = '" & Order & "'", "OrderForHelp")

            Dim sS As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.vers)
            'Dim oFormula As New FormulaResult(sS, Parameters, 0, Nothing)
            'sS = oFormula.ParseAndCalculate
            Select Case sS
                Case "M"
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.vers, "M")
                    Return "Metric"
                Case "I"
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.vers, "I")
                    Return "Inch"
                Case Else
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.vers, "M")
                    Return "Metric"
            End Select

            'oFormula = Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Function CALC_GETMAINMATERIAL(itm As Item) As String
        Try
            'Dim dt_m As DataTable = returnCurentModel()
            'Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString
            'Dim dtRulesTmp As DataTable = clsModel.GetModelRules(ModelNum)

            'Dim LabelNum As Integer = CInt(itm.ParameterIndex)
            'Dim Order As String = Parameters.Rows(LabelNum - 1)("Order").ToString

            'Dim drRulesTmp() As DataRow = dtRulesTmp.Select("LabelNum= " & LabelNum & " AND Order = '" & Order & "'", "OrderForHelp")

            Dim sS As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempMainMaterial, False)
            Return sS

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function Calc_GetMATERIAL(itm As Item) As String
        Try
            'Dim dt_m As DataTable = returnCurentModel()
            'Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString
            'Dim dtRulesTmp As DataTable = clsModel.GetModelRules(ModelNum)

            'Dim LabelNum As Integer = CInt(itm.ParameterIndex)
            'Dim Order As String = Parameters.Rows(LabelNum - 1)("Order").ToString

            'Dim drRulesTmp() As DataRow = dtRulesTmp.Select("LabelNum= " & LabelNum & " AND Order = '" & Order & "'", "OrderForHelp")

            Dim sS As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempMaterial, False)
            Return sS

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function CALC_GETMAINMATERIALCAT(itm As Item) As String
        Try
            'Dim dt_m As DataTable = returnCurentModel()
            'Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString
            'Dim dtRulesTmp As DataTable = clsModel.GetModelRules(ModelNum)

            'Dim LabelNum As Integer = CInt(itm.ParameterIndex)
            'Dim Order As String = Parameters.Rows(LabelNum - 1)("Order").ToString

            'Dim drRulesTmp() As DataRow = dtRulesTmp.Select("LabelNum= " & LabelNum & " AND Order = '" & Order & "'", "OrderForHelp")

            Dim sS As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempMainMaterialCategory, False)
            Return sS

        Catch ex As Exception
            Throw
        End Try
    End Function

    'Public Function Calc_SCALE(itm As Item) As String
    '    Try
    '        Dim sTr As String = ""
    '        'Dim dt_m As DataTable = _dtParamListModification

    '        Dim dt_m As DataTable = returnCurentModel()
    '        Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString
    '        Dim dtRulesTmp As DataTable = clsModel.GetModelRules(ModelNum)

    '        Dim LabelNum As Integer = CInt(itm.ParameterIndex)
    '        Dim Order As String = Parameters.Rows(LabelNum - 1)("Order").ToString

    '        Dim drRulesTmp() As DataRow = dtRulesTmp.Select("LabelNum= " & LabelNum & " AND Order = '" & Order & "'", "OrderForHelp")

    '        'HL_="HeightLimit"

    '        Select Case drRulesTmp(0)("HeightLimit").ToString.ToUpper
    '            Case "GETSCALE"
    '                Dim sS As String = "HL_MILLIMETER " 'SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.SCALE)
    '                Dim oFormula As New FormulaResult(sS, Parameters, 0, Nothing)
    '                sS = oFormula.ParseAndCalculate
    '                sTr = "GETSCALE:" & sS & ";"
    '                oFormula = Nothing
    '            Case "GETWORKPIECEMATERIAL"
    '                Dim sS As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempWorkPieceMaterialValue, False)
    '                sTr &= "HL_GETWORKPIECEMATERIAL:" & sS & ";"
    '            Case "GETMATERIAL"
    '                Dim sS As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempModelValue, False)
    '                sTr &= "HL_GETMATERIAL:" & sS & ";"
    '            Case "GETWORKPIECEMATERIALCATEGORY"
    '                Dim sS As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempWorkPieceMaterialCategory, False)
    '                sTr &= "HL_CATEGORY:" & sS & ";"
    '        End Select

    '        Return sTr

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function


    Public Function Calc_Description() As String
        Try
            Dim dt_FamilyProperties As DataTable = clsPrice.GetFamilyProperties(clsQuatation.ACTIVE_ModelNumber)
            GetDescriptionFormula(dt_FamilyProperties)
            Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.SemiToolDescription, False)

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Sub GetDescriptionFormula(dt As DataTable)
        Dim sErrorFormula As String = ""
        Dim DescriptionFormula As String = ""
        Dim iCuIndfex As Integer = 0
        Dim ModelNo As String = clsQuatation.ACTIVE_ModelNumber
        Dim sP As String = ""
        Dim sV As String = ""
        Dim sN As String = ""
        Dim sss As String
        Try

            If Not dt Is Nothing Then
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    DescriptionFormula = dt.Rows(0).Item("DescriptionFormula").ToString.Trim
                    Dim dtParams As DataTable = Nothing
                    Dim sStart As String = clsQuatation.ACTIVE_OpenType.ToString
                    dtParams = clsQuatation.GetActiveQuotation_DTparams

                    Dim dtParamTemp As DataTable = dtParams.Copy


                    Dim list As New Dictionary(Of String, String)
                    list.Add("unvisble", "")

                    For i As Integer = 0 To dtParams.Rows.Count - 1

                        iCuIndfex = i + 1
                        Dim sFormat As String = dtParams.Rows(i).Item("DescFormat").ToString.Trim
                        sV = dtParams.Rows(i).Item("DescValue").ToString.Trim.Replace("Measure{", "{")

                        If sV.Length > 7 AndAlso sV.Substring(0, 7) = "String(" Then
                            Select Case sV.Substring(8, sV.Length - 10)
                                Case "/"
                                    list.Add("TEMPa", sV.Substring(8, sV.Length - 10))
                                    sV = "TEMPa"
                            End Select
                        Else
                            sV = FormulaResult.Conver_Description_Formula(sV, dtParams)
                        End If


                        If sFormat.ToString.ToUpper = "DESCVALUE" Then
                            Dim oFormula2 As New FormulaResult(sV, dtParams, 0, Nothing)
                            sErrorFormula = oFormula2.Formula.ToString
                            Dim res1 As String = oFormula2.ParseAndCalculate
                            dtParamTemp.Rows(i).Item("Measure") = res1
                        Else

                            sP = dtParams.Rows(i).Item("DescConditions").ToString.Trim
                            sP = FormulaResult.Conver_Description_Formula(sP, dtParams)

                            If sP = "" Then
                                sN = sV
                            Else
                                sN = sP.Replace("unvisible", """""")
                                sN = sN.Replace("visible", sV)
                            End If
                            Dim oFormula1 As New FormulaResult(sN, dtParams, 0, Nothing)

                            If oFormula1.ToString.Trim <> "" Then
                                'oFormula1.CurrentParameterIndex = CurrentParameterIndex
                                sErrorFormula = oFormula1.Formula.ToString
                                Dim res1 As String = oFormula1.ParseAndCalculate
                                'res1 = CastFormulaFormat(res1, sFormat)


                                'Dim o As New FormulaResult(sFormat, dtParams, 0, Nothing)
                                'Dim res2 As String = o.ParseAndCalculate


                                dtParamTemp.Rows(i).Item("Measure") = res1
                            End If


                        End If
                    Next

                    Dim ConvertedFormula As String = FormulaResult.Conver_Description_Formula(DescriptionFormula, dtParamTemp)
                    Dim oFormula As New FormulaResult(ConvertedFormula, dtParamTemp, 0, Nothing)

                    If oFormula.ToString.Trim <> "" Then
                        sErrorFormula = oFormula.Formula.ToString
                        Dim res1 As String = oFormula.ParseAndCalculate
                        For Each Item In list
                            res1 = res1.Replace(Item.Key, Item.Value)
                        Next
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.SemiToolDescription, res1)

                    End If

                End If
            End If

        Catch ex As Exception
            'sss = "Model Number - " & ModelNo & "<br>"
            sss = "Model Number - " & ModelNo & "<br>"
            sss &= "Parameter Index - " & iCuIndfex & "<br>"
            sss &= "DescConditions - " & sP & "<br>"
            sss &= "DescValue - " & sV & "<br>"
            sss &= "<br>Error - " & ex.Message

            clsMail.SendEmailError("Error in cal description formula", sss, False, "", "")


            'GeneralException.WriteEventErrors(sss, GeneralException.e_LogTitle.PARSER.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "GetDescriptionFormula", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw New Exception("Object error in Description formula")
            'Throw
        End Try
    End Sub
    Public Shared Function CastFormulaFormat(VaueToFormat As String, FormatType As String) As String
        Try
            Select Case FormatType
                Case "000"
                    If IsNumeric(VaueToFormat) Then
                        VaueToFormat = CDbl(VaueToFormat)
                        VaueToFormat = VaueToFormat.ToString.Replace(".", "")

                        If VaueToFormat.ToString.Length = 1 Then
                            VaueToFormat = "0" & VaueToFormat & "0"
                        End If
                        If VaueToFormat.ToString.Length = 2 Then
                            VaueToFormat = "0" & VaueToFormat
                        End If

                        'VaueToFormat = Format(Math.Round(CType(VaueToFormat, Double)), "000")
                    End If
                Case "00"
                    If IsNumeric(VaueToFormat) Then
                        VaueToFormat = Format(Math.Round(CType(VaueToFormat, Double)), "00")
                    End If
                Case "INT"
                    If IsNumeric(VaueToFormat) Then
                        VaueToFormat = CInt(VaueToFormat)
                    End If
                Case "ROUND"
                    If IsNumeric(VaueToFormat) Then
                        VaueToFormat = Math.Round(CType(VaueToFormat, Double))
                    End If
            End Select
            Return VaueToFormat
        Catch ex As Exception
            Return ""
            Throw

        End Try
        Return VaueToFormat

    End Function

    Public Function Calc_GUNDIV() As String
        Try
            Dim bFDiv3 As Boolean = False

            If Quantity >= 20 Then
                Return 2
            Else
                Dim SemiToolDescription As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.SemiToolDescription)
                Dim dt_m As DataTable = returnCurentModel() ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Model, "")
                Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString
                'If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationStatus) = SessionManager.QuotationStatus.NewQuotation Then
                'If ModelNum = clsModel.e_Model.Gun_Drill Or ModelNum = clsModel.e_Model.Gun_Drill_TUNGALOY Then
                '    If Left(Trim(SemiToolDescription), 4) = "STGD" Then
                '        bFDiv3 = True
                '        'Else
                '        'If FName = "frmQuotOrder" Then
                '        'If NumOfModel = 60 And Left(Trim(rstQ(2)), 4) = "STGD" Then bFDiv3 = True
                '        'Else
                '        'If NumOfModel = 60 And Left(Trim(ToolDesc), 4) = "STGD" Then bFDiv3 = True
                '        'If Left(Trim(ToolDesc), 4) = "STGD" Then bFDiv3 = True
                '        'End If
                '    End If
                'End If
                ''End If
            End If

            If bFDiv3 = True Then
                Return 2
            Else
                Return 1
            End If
            Return 0
        Catch ex As Exception
            Throw
        End Try
    End Function

    'CGP1, CGP2, CGP3
    'Private Function Calc_CGp(ByVal GPNumber As Integer) As String
    '    Try
    '        Calc_CGp = Nothing
    '        Dim ColumnName As String = "Customer_GP" & GPNumber.ToString
    '        Select Case GPNumber
    '            Case "1"
    '                Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Customer_GP1).ToString
    '            Case "2"
    '                Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Customer_GP2).ToString
    '            Case "3"
    '                Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Customer_GP3).ToString
    '        End Select
    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Function

    'Private Function Calc_DISREF() As String
    '    Try
    '        Calc_DISREF = Nothing

    '        Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.DiscountFromSTN).ToString

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function

    Private Function Calc_WCGP() As String
        Try
            Return "" 'SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.WC_FACTOR)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function Calc_GetFactorQuantity() As String
        If Quantity <= 0 Then
            Throw New Exception("Wrong Quantity Parameter")
        End If
        Try

            Dim dt_m As DataTable = returnCurentModel() ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Model, "")
            Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim Params As New SqlParams
            Dim dtFactors As DataTable

            Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 4))
            dtFactors = oSql.ExecuteSPReturnDT("USP_GetFactorsQuantities", Params)

            If dtFactors.Rows.Count = 0 Then
                Return "1"
            End If

            Dim Operation As String
            Dim Factor As Decimal

            For Each dr As DataRow In dtFactors.Rows
                Operation = dr("Operation").ToString
                Select Case Operation
                    Case "<>"
                        If (Quantity > dr("LowQTY")) And (Quantity < dr("HeightQTY")) Then
                            Factor = dr("Factor")
                            Return Factor
                        End If
                    Case "="
                        If Quantity = dr("HeightQTY") Then
                            Factor = dr("Factor")
                            Return Factor
                        End If
                    Case "<="
                        If Quantity <= dr("HeightQTY") Then
                            Factor = dr("Factor")
                            Return Factor
                        End If
                    Case ">="
                        If Quantity >= dr("HeightQTY") Then
                            Factor = dr("Factor")
                            Return Factor
                        End If
                End Select
            Next
            Return "1"

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Function Calc_GetSpecialFactor() As String
        Try

            If Quantity <= 0 Then
                Throw New Exception("Wrong Quantity Parameter")
            End If

            Dim dt_m As DataTable = returnCurentModel() ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Model, "")
            Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim Params As New SqlParams
            Dim dtSpecialFactors As DataTable

            Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 4))
            dtSpecialFactors = oSql.ExecuteSPReturnDT("USP_GetSpecialFactorsQty", Params)

            If dtSpecialFactors.Rows.Count = 0 Then
                Return "0"
            End If

            Dim drs() As DataRow
            drs = dtSpecialFactors.Select("DependentParameter='QTY'")

            Dim Operation As String
            Dim Factor As Decimal = 0
            Dim GroupNum As Integer = 0
            Dim ParamVal As Decimal = 0

            For Each dr As DataRow In drs
                Operation = dr("Operator").ToString
                Select Case Operation
                    Case "<>" : If (Quantity > CDec(dr("LowLimit"))) And (Quantity < dr("HighLimit")) Then GroupNum = dr("GroupNum")
                    Case "=" : If Quantity = CDec(dr("HighLimit")) Then GroupNum = dr("GroupNum")
                    Case "<=" : If Quantity <= CDec(dr("HighLimit")) Then GroupNum = dr("GroupNum")
                    Case ">=" : If Quantity >= CDec(dr("HighLimit")) Then GroupNum = dr("GroupNum")
                End Select
            Next

            If GroupNum.ToString <> "0" AndAlso IsNumeric(GroupNum) Then
                Dim drsF() As DataRow
                drsF = dtSpecialFactors.Select("DependentParameter<>'QTY' and groupnum=" & GroupNum)
                If drsF.Length > 0 Then
                    Dim drFz As DataRow = drsF(0)
                    ParamVal = Parameters.Rows(drFz("DependentParameter") - 1)("Measure").ToString
                    For Each drF As DataRow In drsF
                        Operation = drF("Operator").ToString
                        Select Case Operation
                            Case "<>" : If (ParamVal > CDec(drF("LowLimit"))) And (Quantity < drF("HighLimit")) Then Factor = drF("Factor")
                            Case "=" : If ParamVal = CDec(drF("HighLimit")) Then Factor = drF("Factor")
                            Case "<=" : If ParamVal <= CDec(drF("HighLimit")) Then Factor = drF("Factor")
                            Case ">=" : If ParamVal >= CDec(drF("HighLimit")) Then Factor = drF("Factor")
                        End Select
                    Next
                End If
            End If

            Return Factor
        Catch ex As Exception
            Throw
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' Branch Type (State) 
    ''' S=Branch
    ''' W=WorkCenter
    ''' A=Agent
    ''' L=LTD
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 

    Public Shared Function CALC_WCFACT() As String
        'Bra
        Try
            Dim wcFactor As String
            'If StateManager.GetValue(StateManager.Keys.BranchType) = "S" Or StateManager.GetValue(StateManager.Keys.BranchType) = "A" Then
            '    wcFactor = "1"
            'Else
            wcFactor = "0"
            'End If
            'FormulaPrice = Replace(FormulaPrice, "WCFactor", IIf(WC_Branch = 0 And BranchState <> LTD, "1", "0"))
            Return wcFactor
        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Function Calc_GetLowQuantityFactor() As String
        Try
            If Calc_GetFactorQuantity() = 0 Then
                Return 1
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw
        End Try

    End Function

#Region "Trigo and Math"
    Private Function Calc_Trigo_Cos(ByVal Operand As Item)
        Try
            Return Math.Cos(Operand.Expression)
        Catch ex As Exception
            Operand.IllegalCalculation = True
            Return 0
        End Try
    End Function


    Private Function Calc_Trigo_Sin(ByVal Operand As Item)
        Try
            Return Math.Sin(Operand.Expression)
        Catch ex As Exception
            Operand.IllegalCalculation = True
            Return 0
        End Try
    End Function


    Private Function Calc_Trigo_ACos(ByVal Operand As Item)
        Try
            Return Math.Acos(Operand.Expression)
        Catch ex As Exception
            Operand.IllegalCalculation = True
            Return 0
        End Try
    End Function


    Private Function Calc_Trigo_Asin(ByVal Operand As Item)
        Try
            Return Math.Asin(Operand.Expression)
        Catch ex As Exception
            Operand.IllegalCalculation = True
            Return 0
        End Try
    End Function


    Private Function Calc_Trigo_Tan(ByVal Operand As Item)
        Try
            Return Math.Tan(Operand.Expression)
        Catch ex As Exception
            Operand.IllegalCalculation = True
            Return 0
        End Try
    End Function


    Private Function Calc_Trigo_Atn(ByVal Operand As Item)
        Try
            Return Math.Atan(Operand.Expression)
        Catch ex As Exception
            Operand.IllegalCalculation = True
            Return 0
        End Try
    End Function


    Private Function Calc_Math_AbsDSC(ByVal Operand As Item)
        Return Math.Abs(CDec(Operand.Expression))
    End Function
    Private Function Calc_Math_Abs(ByVal Operand As Item)
        Return Math.Abs(CDec(Operand.Expression))
    End Function
    Private Function Calc_Math_FIX0(ByVal Operand As Item)
        Select Case CDbl(Operand.Expression)
            Case >= 1
                Return Format(CDbl(Operand.Expression), "0.0")
            Case Else
                Return CInt(Operand.Expression * 10)
        End Select


    End Function
    Private Function Calc_Math_FIX00(ByVal Operand As Item)
        Select Case CDbl(Operand.Expression)
            Case >= 1
                Return Format(CDbl(Operand.Expression), "0.0")
            Case Else
                Return CInt(Operand.Expression * 100)
        End Select
        'Return CInt(Operand.Expression * 100)
    End Function
    Private Function Calc_Math_FIX1000(ByVal Operand As Item)
        If IsNumeric(Operand.Expression) Then
            Select Case CDbl(Operand.Expression)
                Case >= 1
                    '  Return Format(CDbl(Operand.Expression), "0.0")
                    'Return Format(CDbl(CDbl(Operand.Expression) - 0.049), "0.0")
                    Return Format(CDbl(CDbl(Operand.Expression) - 0.0499), "0.0")
                Case Else
                    Return CInt(Operand.Expression * 1000)
            End Select
            'Return CInt(Operand.Expression * 1000)
        Else

            Return 0
        End If

    End Function
    Private Function Calc_Math_Exp(ByVal Operand As Item)
        Return Math.Exp(Operand.Expression)
    End Function

    Private Function Calc_Math_Log(ByVal Operand As Item)
        Return Math.Log(Operand.Expression)
    End Function


    Private Function Calc_Math_Fix(ByVal Operand As Item)
        Return Math.Floor(CDec(Operand.Expression))     'TODO: ??? Fix = Floor ???
    End Function


    Private Function Calc_Math_Sqr(ByVal Operand As Item)
        Return Math.Sqrt(Operand.Expression)
    End Function

    Private Function Calc_Math_Int(ByVal Operand As Item)
        Return CInt(Operand.Expression)
    End Function
    Private Function Calc_Math_Sgn(ByVal Operand As Item)
        Return Math.Sign(CDec(Operand.Expression))      'TODO:???
    End Function

    Private Function Calc_Math_RoundUP(ByVal Operand As Item)
        If IsNumeric(Operand.Expression) Then
            Return Math.Round(CDec(Math.Round(CDec(Operand.Expression) + 0.49D, 2)))
        Else
            Return 0
        End If
        'Return Math.Round(CDec(Math.Round(CDec(Operand.Expression) + 0.49D, 2)))
    End Function

    Private Function Calc_Math_RoundDown(ByVal Operand As Item)
        If IsNumeric(Operand.Expression) Then
            Return Math.Round(CDec(Math.Round(CDec(Operand.Expression) - 0.49D, 2)))
        Else

            Return 0
        End If

    End Function
    Private Function Calc_Math_Round(ByVal Operand As Item)
        If IsNumeric(Operand.Expression) Then
            Return Math.Round(CDec(Operand.Expression))
        Else
            Return 0
        End If
    End Function
    Private Function Calc_Math_Round0(ByVal Operand As Item)

        If IsNumeric(Operand.Expression) Then
            Return Format(CDec(Operand.Expression), "0.0")
        Else
            Return 0
        End If
    End Function
    Private Function Calc_Math_Round00(ByVal Operand As Item)
        If IsNumeric(Operand.Expression) Then
            Return Format(CDec(Operand.Expression), "0.00")
        Else
            Return 0
        End If

    End Function
    Private Function Calc_Math_Round000(ByVal Operand As Item)

        If IsNumeric(Operand.Expression) Then
            Return Format(CDec(Operand.Expression), "0.000")
        Else
            Return 0
        End If
    End Function

    Private Function Calc_Math_Round0DSC(ByVal Operand As Item)

        If IsNumeric(Operand.Expression) Then
            Return Format(CDec(Operand.Expression), ".0")
        Else
            Return 0
        End If
    End Function
    Private Function Calc_Math_Round00DSC(ByVal Operand As Item)

        If IsNumeric(Operand.Expression) Then
            Return Format(CDec(Operand.Expression), ".00")
        Else
            Return 0
        End If
    End Function
    Private Function Calc_Math_Round000DSC(ByVal Operand As Item)


        If IsNumeric(Operand.Expression) Then
            Return Format(CDec(Operand.Expression), ".000")
        Else
            Return 0
        End If
    End Function
    Private Function Calc_Math_ABS000DSC(ByVal Operand As Item)

        Dim VaueToFormatDef As String = Operand.Expression
        Dim VaueToFormat As String = Operand.Expression

        If IsNumeric(VaueToFormat) Then
            If VaueToFormat < 10 Then
                VaueToFormat = "0" & CInt(VaueToFormat * 100).ToString
            Else
                VaueToFormat = CInt(VaueToFormat * 100)
            End If
        End If
        Return VaueToFormat

        'If IsNumeric(VaueToFormat) Then
        '    VaueToFormat = CDbl(VaueToFormat)
        '    VaueToFormat = VaueToFormat.ToString.Replace(".", "")

        '    If VaueToFormat.ToString.Length = 1 Then
        '        VaueToFormat = "0" & VaueToFormat & "0"
        '    End If
        '    If VaueToFormat.ToString.Length = 2 Then
        '        VaueToFormat = "0" & VaueToFormat
        '    End If

        '    If VaueToFormatDef.Length = 4 Then
        '        If VaueToFormatDef.Substring(VaueToFormatDef.Length - 1, VaueToFormatDef.Length - 3) = "0" AndAlso VaueToFormatDef.Contains(".") Then
        '            VaueToFormat = CInt(VaueToFormatDef)
        '        End If
        '    End If


        '    Return VaueToFormat
        '    'VaueToFormat = Format(Math.Round(CType(VaueToFormat, Double)), "000")
        'End If
        'Return "000"


    End Function

    Private Function Calc_RECHW1000(ByVal Operand As Item)

        Dim VaueToFormat As String = Operand.Expression

        If IsNumeric(VaueToFormat) Then
            If (CDbl(VaueToFormat) * 1000) < 10 Then
                VaueToFormat = "00" & (VaueToFormat * 1000).ToString
            Else
                If (CDbl(VaueToFormat) * 1000) < 100 Then
                    VaueToFormat = "0" & (VaueToFormat * 1000).ToString
                Else
                    VaueToFormat = (VaueToFormat * 1000).ToString
                End If
            End If

        End If
        Return VaueToFormat

        'If IsNumeric(VaueToFormat) Then
        '    VaueToFormat = CDbl(VaueToFormat)
        '    VaueToFormat = VaueToFormat.ToString.Replace(".", "")

        '    If VaueToFormat.ToString.Length = 1 Then
        '        VaueToFormat = "0" & VaueToFormat & "0"
        '    End If
        '    If VaueToFormat.ToString.Length = 2 Then
        '        VaueToFormat = "0" & VaueToFormat
        '    End If

        '    If VaueToFormatDef.Length = 4 Then
        '        If VaueToFormatDef.Substring(VaueToFormatDef.Length - 1, VaueToFormatDef.Length - 3) = "0" AndAlso VaueToFormatDef.Contains(".") Then
        '            VaueToFormat = CInt(VaueToFormatDef)
        '        End If
        '    End If


        '    Return VaueToFormat
        '    'VaueToFormat = Format(Math.Round(CType(VaueToFormat, Double)), "000")
        'End If
        'Return "000"


    End Function

    Private Function Calc_Math_ABS1000DSC(ByVal Operand As Item)

        Dim VaueToFormat As String = Operand.Expression

        If IsNumeric(VaueToFormat) Then
            If VaueToFormat < 0.01 Then
                VaueToFormat = "00" & (VaueToFormat * 1000).ToString
            Else
                VaueToFormat = "0" & (VaueToFormat * 1000).ToString
            End If
        End If
        Return VaueToFormat

        'If IsNumeric(VaueToFormat) Then
        '    VaueToFormat = CDbl(VaueToFormat)
        '    VaueToFormat = VaueToFormat.ToString.Replace(".", "")

        '    If VaueToFormat.ToString.Length = 1 Then
        '        VaueToFormat = "0" & VaueToFormat & "0"
        '    End If
        '    If VaueToFormat.ToString.Length = 2 Then
        '        VaueToFormat = "0" & VaueToFormat
        '    End If

        '    If VaueToFormatDef.Length = 4 Then
        '        If VaueToFormatDef.Substring(VaueToFormatDef.Length - 1, VaueToFormatDef.Length - 3) = "0" AndAlso VaueToFormatDef.Contains(".") Then
        '            VaueToFormat = CInt(VaueToFormatDef)
        '        End If
        '    End If


        '    Return VaueToFormat
        '    'VaueToFormat = Format(Math.Round(CType(VaueToFormat, Double)), "000")
        'End If
        'Return "000"


    End Function



    'Private Function Calc_Math_ABS000DSC(ByVal Operand As Item)

    '    Dim VaueToFormatDef As String = Operand.Expression
    '    Dim VaueToFormat As String = Operand.Expression

    '    If IsNumeric(VaueToFormat) Then
    '        VaueToFormat = CDbl(VaueToFormat)
    '        VaueToFormat = VaueToFormat.ToString.Replace(".", "")

    '        If VaueToFormat.ToString.Length = 1 Then
    '            VaueToFormat = "0" & VaueToFormat & "0"
    '        End If
    '        If VaueToFormat.ToString.Length = 2 Then
    '            VaueToFormat = "0" & VaueToFormat
    '        End If

    '        If VaueToFormatDef.Length = 4 Then
    '            If VaueToFormatDef.Substring(VaueToFormatDef.Length - 1, VaueToFormatDef.Length - 3) = "0" AndAlso VaueToFormatDef.Contains(".") Then
    '                VaueToFormat = CInt(VaueToFormatDef)
    '            End If
    '        End If


    '        Return VaueToFormat
    '        'VaueToFormat = Format(Math.Round(CType(VaueToFormat, Double)), "000")
    '    End If
    '    Return "000"
    'End Function

    Private Function Calc_Math_ABS000(ByVal Operand As Item)

        Dim VaueToFormat As String = Operand.Expression
        If IsNumeric(VaueToFormat) Then
            VaueToFormat = CDbl(VaueToFormat)
            VaueToFormat = VaueToFormat.ToString.Replace(".", "")

            If VaueToFormat.ToString.Length = 1 Then
                VaueToFormat = "0" & VaueToFormat & "0"
            End If
            If VaueToFormat.ToString.Length = 2 Then
                VaueToFormat = "0" & VaueToFormat
            End If



            Return VaueToFormat
            'VaueToFormat = Format(Math.Round(CType(VaueToFormat, Double)), "000")
        End If
        Return "000"
    End Function
    Private Function Calc_Math_ABS00(ByVal Operand As Item)

        Dim VaueToFormat As String = Operand.Expression
        If IsNumeric(VaueToFormat) Then
            VaueToFormat = Math.Truncate(CDbl(VaueToFormat))

            If VaueToFormat.ToString.Length = 1 Then
                VaueToFormat = "0" & VaueToFormat
                Return VaueToFormat
            End If
            If VaueToFormat.ToString.Length = 2 Then
                Return VaueToFormat
            End If
            If VaueToFormat.ToString.Length > 2 Then
                VaueToFormat = VaueToFormat.ToString.Substring(VaueToFormat.Length - 2, 2)
                Return VaueToFormat
            End If

            'VaueToFormat = Format(Math.Round(CType(VaueToFormat, Double)), "000")
        End If
        Return "00"
    End Function

    Private Function Calc_Math_ABS00DSC(ByVal Operand As Item)

        Dim VaueToFormat As String = Operand.Expression
        If IsNumeric(VaueToFormat) Then
            VaueToFormat = Math.Truncate(CDbl(VaueToFormat))

            If VaueToFormat.ToString.Length = 1 Then
                VaueToFormat = "0" & VaueToFormat
                Return VaueToFormat
            End If
            If VaueToFormat.ToString.Length = 2 Then
                Return VaueToFormat
            End If
            If VaueToFormat.ToString.Length > 2 Then
                VaueToFormat = VaueToFormat.ToString.Substring(VaueToFormat.Length - 2, 2)
                Return VaueToFormat
            End If

            'VaueToFormat = Format(Math.Round(CType(VaueToFormat, Double)), "000")
        End If
        Return "00"
    End Function




    'Private Function Calc_Math_MOD(ByVal Result1 As Item, ByVal Result2 As Item) As String
    '    Try
    '        Dim res1 As Double = 0
    '        Dim res2 As Double = 0
    '        If Result1.Expression.ToString <> "" Then res1 = Result1.Expression
    '        If Result2.Expression.ToString <> "" Then res2 = Result2.Expression
    '        Return res1 Mod res2

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function


#End Region



#Region "SemiFactorsPrices"



    Public Function CalcFormulaFactoes(dtFactors As DataTable, Param As String, ParamValue As String) As String

        Try
            Dim dv As DataView = dtFactors.DefaultView
            dv.RowFilter = "FactorParam='" & Param & "'"
            For i As Integer = 0 To dv.Count - 1
                'If GetFactorValueWithCondition() = True Then


                'End If
            Next
        Catch ex As Exception
            Throw
        End Try

    End Function

    'Private Function GetFactorValueWithCondition(Condition As String, dtFactor As DataTable) As Boolean


    '    Throw New NotImplementedException()
    'End Function





    Private Function Calc_DisplayFromList(ByVal itm As Item, ParamName As Item, SearchField As Item) As String
        Try

            ''Select Case ModelNum, LabelNum, [Order], OrderForHelp, HeightLimit, HeightLimitH,HeightLimitFormula, InputLimit, RullNotation, StringValue
            ''From dbo.glb_TToolGeometricRules
            ''Where (ModelNum = 30020) And Labelnum = xx - tabindex And HeightLimitFormula ='"XX-measre"'

            Dim value As String
            Dim res As String

            Dim dt_m As DataTable = returnCurentModel()
            Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString

            Dim dummy_ResultSetRules As DataTable = Nothing

            value = itm.Expression

            If Left(value, 1) = """" AndAlso Right(value, 1) = """" Then
                value = value.Substring(1, value.Length - 2)
            End If

            Dim dtRulesTmp As DataTable = clsModel.GetModelRules(ModelNum)

            Dim dtParam As DataTable = clsQuatation.GetActiveQuotation_DTparams()

            Dim ParanTabIndex As String = itm.ParameterIndex + 1

            'Dim drRulesTmp() As DataRow = dtRulesTmp.Select("LabelNum=" & ParanTabIndex & " and (HeightLimit='" & value & "' OR HeightLimit='""" & value & """')", "OrderForHelp")

            Dim s As String = SearchField.Expression.Replace("""", "")


            Dim dv As DataView = dtRulesTmp.DefaultView

            dv.RowFilter = "LabelNum = " & ParanTabIndex & " AND " & s & "= '""" & value & """'"

            ResultSetRules = dv.ToTable

            'Return "VISIBLE"


            If ResultSetRules.Rows.Count = 0 Then
                Return 0
            Else
                Return ""

            End If

            '  Dim drRulesTmp() As DataRow = dtRulesTmp.Select("LabelNum = " & ParanTabIndex & " AND " & s & "= '""" & value & """'")

            'Dim sVal As String = drRulesTmp(CInt(ParanTabIndex))
            'Dim drRulesTmp() As DataRow = dtRulesTmp.Select("LabelNum=" & ParanTabIndex & " and (" & SearchField.Expression & "='" & value & "' OR " & SearchField.Expression & "='""" & value & """')", "OrderForHelp")

            ' If drRulesTmp.Length > 0 Then
            '    'if not drRulesTmp(0)("InputLimit") is dbnull
            '    Dim oFormula As New FormulaResult(drRulesTmp(0)("InputLimit").ToString, Parameters, CurrentParameterIndex, dummy_ResultSetRules)
            '    res = oFormula.ParseAndCalculate
            '    ResOrder = drRulesTmp(0)("Order")
            '    oFormula = Nothing
            'Return res
            'Else
            '    ResOrder = ""
            '    Return "0"

            'End If

            'For Each dr As DataRow In ResultSetRules.Rows
            '    If dr("Operation").ToString.ToUpper = "UNVISIBLE" Then
            '        ResOrder = dr("Order").ToString()
            '        Return 0
            '    End If
            'Next

            'ResultSetRules = dtResultRules '.DefaultView


            ''Return "CrosListUnvisible"
            'If dtResultRules.Rows.Count = 0 Then
            '    Return 0
            'Else
            '    Return ""
            'End If

        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region

End Class
