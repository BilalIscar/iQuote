'Relation({12}>70.8,100,({12}+29.19))
Public Class FormulaResult
    Public Tree As Node
    Public Formula As String
    Public CurrentParameterIndex As Integer
    Public Parameters As DataTable
    Public ResultSetRules As DataTable
    Public ResOrder As String

    Public Quantity As Integer = -1
    Public StandardPrice As Decimal = -1
    'Public BranchNumber As Integer = -1
    Public GALSystem As String = ""
    Public CustomerNumber As Integer = -1
    Public ModelNum As String = "0"
    Public SPSETUP As String = "0"
    Public STNSETUP As String = "0"


    Public Enum ConstantsStates As Integer
        Compute = 1
        ComputeAndStore = 2
        RetrieveOnly = 3
    End Enum
    Public Constants As DataTable
    Public ConstantsState As ConstantsStates = ConstantsStates.Compute

    Private ComplexOperators As String(,) = { _
                                                {">=", "$"}, _
                                                {"=>", "$"}, _
                                                {"<=", "@"}, _
                                                {"=<", "@"}, _
                                                {"<>", "~"}, _
                                                {"><", "~"}, _
                                                {"&", "`"}, _
                                                {"AND", "?"}, _
                                                {"--", "+"}, _
                                                {"XOR", Chr(174)}, _
                                                {"OR", "|"},
                                                {"MOD", Chr(169)} _
                                            }

    '                                                {"EQV", Chr(235)}, _
    '                                               {"IMP", Chr(236)} _

    Public Shared Operators As String() = { _
                                                "^", _
                                                "/", _
                                                "\", _
                                                "*", _
                                                "-", _
                                                "+", _
                                                ">", _
                                                "<", _
                                                "=", _
                                                "$", _
                                                "@", _
                                                "~", _
                                                "`", _
                                                "?", _
                                                Chr(174), _
                                                "|", _
                                                Chr(169) _
                                                }

    Private Shared SpecialLabels As String(,) = { _
                                            {"#X#", "-"}, _
                                            {"""""", ""}, _
                                            {"#####/#", ""} _
                                         }

    'Private Shared SpecialLabels As String(,) = { _
    '                                    {"#X#", "-"}, _
    '                                    {"#####/#", ""} _
    '}

    Private Shared SpecialLabelsDisplay As String(,) = { _
                                        {"""""", ""} _
                                     }


#Region "SemiFactorsPrices"

    Public Sub New(ByVal _formula As String, ByVal _Parameters As DataTable, ByVal _CurrentParameterIndex As Integer, ByVal _ResultSetRules As DataTable)
        Formula = _formula
        '-----------
        Dim s1 As Integer = 0
        Dim s2 As Integer = 0
        Dim s3 As Integer = 0
        Dim s4 As Integer = 0
        If Not Formula Is Nothing Then
            For DD = 0 To Formula.Length - 1
                If Formula(DD) = "(" Then s1 += 1
                If Formula(DD) = ")" Then s2 += 1
                If Formula(DD) = "{" Then s3 += 1
                If Formula(DD) = "}" Then s4 += 1
            Next
        End If

        If s1 <> s2 And s3 <> s4 Then
            'GeneralException.WriteEventErrors("Parse - Number Of brackets Is different in Formula " & Formula, GeneralException.e_LogTitle.PARSER.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "Parse - Number Of brackets Is different in Formula", "Formula - " & Formula, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Return
        Else
            Parameters = _Parameters
            CurrentParameterIndex = _CurrentParameterIndex
            ResultSetRules = _ResultSetRules

            If False Then
                Dim dt As DataTable = Parameters.Copy
                dt.TableName = "_dtParamList"
                dt.WriteXml("c:\parameters.xml")
            End If
        End If
        '-----------

    End Sub


    'Private Shared Function ReplaceSTNparam(Formula As String) As String

    '    Try


    '        Dim FormulaReplace As String = Formula

    '        For i = 0 To 10
    '            If Formula.IndexOf("{STN ") = 0 Then
    '                Exit For
    '            Else
    '                Dim sStnStart As Integer = Formula.IndexOf("{STN ") + 4
    '                Dim sStnEnd As Integer = Formula.IndexOf("}", Formula.IndexOf("{STN "))
    '                Dim sStr As String = Formula.Substring(sStnStart, sStnEnd - sStnStart)
    '                Formula.Replace("{STN " & sStr, "STN" & sStr.Substring(0, sStr - 1))
    '            End If
    '        Next
    '        Return FormulaReplace
    '    Catch ex As Exception
    '        Throw
    '    End Try
    '    Return ""



    '    Throw New NotImplementedException()
    'End Function


    Public Shared Function Conver_Price_Formula(FormulaToConvert As String, dtParam As DataTable) As String

        Dim ConvertedFormula As String

        ConvertedFormula = ReplaceParamNameWithNumbers(FormulaToConvert, dtParam)
        ConvertedFormula = ReplaceEmptyParamsL(ConvertedFormula)
        ConvertedFormula = ReplaceEmptyParamsR(ConvertedFormula)
        ConvertedFormula = ReplaceEmptySpacing(ConvertedFormula, True)
        'ConvertedFormula = AddFactorsToFormula(ConvertedFormula)

        Return ConvertedFormula

    End Function

    Public Shared Function Conver_InputLimit_Formula(FormulaToConvert As String, dtParam As DataTable) As String

        Dim ConvertedFormula As String

        ConvertedFormula = ReplaceParamNameWithNumbers(FormulaToConvert, dtParam)
        'ConvertedFormula = ReplaceEmptyParamsL(ConvertedFormula)
        'ConvertedFormula = ReplaceEmptyParamsR(ConvertedFormula)
        'ConvertedFormula = ReplaceEmptySpacing(ConvertedFormula, True)
        'ConvertedFormula = AddFactorsToFormula(ConvertedFormula)

        Return ConvertedFormula

    End Function

    Public Shared Function Conver_Image_Formula(FormulaToConvert As String, dtParam As DataTable) As String

        Dim ConvertedFormula As String

        ConvertedFormula = ReplaceParamNameWithNumbers(FormulaToConvert, dtParam)
        'ConvertedFormula = ReplaceEmptyParamsL(ConvertedFormula)
        'ConvertedFormula = ReplaceEmptyParamsR(ConvertedFormula)
        'ConvertedFormula = ReplaceEmptySpacing(ConvertedFormula, True)
        'ConvertedFormula = AddFactorsToFormula(ConvertedFormula)

        Return ConvertedFormula


    End Function
    Public Shared Function Conver_Description_Formula(FormulaToConvert As String, dtParam As DataTable) As String

        Dim ConvertedFormula As String

        ConvertedFormula = ReplaceParamNameWithNumbers(FormulaToConvert, dtParam)
        'ConvertedFormula = ReplaceEmptyParamsL(ConvertedFormula)
        'ConvertedFormula = ReplaceEmptyParamsR(ConvertedFormula)
        'ConvertedFormula = ReplaceEmptySpacing(ConvertedFormula, True)
        'ConvertedFormula = ReplaceParamNameWithValues(ConvertedFormula, dtParam)
        'ConvertedFormula = AddFactorsToFormula(ConvertedFormula)

        Return ConvertedFormula

    End Function
    Public Shared Function Conver_STR_Formula(FormulaToConvert As String, _dtParamList As DataTable) As String
        Try
            If _dtParamList Is Nothing Then
                Dim OpenType As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType)

                If OpenType = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                    _dtParamList = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListModification, "")

                ElseIf OpenType = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                    _dtParamList = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListConfiguration, "")
                End If

            End If

            FormulaToConvert = FormulaResult.ReplaceFunctions(FormulaToConvert, 1)
            FormulaToConvert = FormulaResult.ReplacebracketForRelation(FormulaToConvert)
            FormulaToConvert = FormulaResult.ReplaceParamNameWithNumbers(FormulaToConvert, _dtParamList)
            FormulaToConvert = FormulaResult.ReplaceEmptySpacing(FormulaToConvert, False)
            FormulaToConvert = FormulaResult.ReplaceEmptyParamsL(FormulaToConvert)
            FormulaToConvert = FormulaResult.ReplaceEmptyParamsR(FormulaToConvert)
            FormulaToConvert = FormulaResult.AddApostrophes(FormulaToConvert)
            'ConvertedFormula = FormulaResult.ReplaceEmptySpacing(ConvertedFormula, True)
            FormulaToConvert = FormulaResult.ReplaceLadder(FormulaToConvert)

            Return FormulaToConvert

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Shared Function ReplaceParamNameWithNumbers(Formula As String, dt_Params As DataTable) As String
        Try
            Dim FormulaReplace As String = Formula
            Dim iFactorStart As Integer
            Dim iFactorEnd As Integer
            Dim sStrToReplace As String = ""
            'If Formula.Contains("{STN APMX}") Then Stop
            Dim sStns As String = "{STN "
            Dim mx As Integer = 5
            If Not Formula.Contains(sStns) Then
                sStns = ""
                mx = Formula.Length
            End If
            If Formula.Length > 4 Then
                mx = 5
            End If

            For i As Integer = 0 To Formula.Length - 1
                If Formula.Substring(i, 1) = "{" AndAlso Formula.Substring(i, mx) <> sStns Then
                    iFactorStart = i
                    iFactorEnd = 0
                    For j As Integer = i To Formula.Length - 1
                        If Formula.Substring(j, 1) = "}" AndAlso iFactorEnd > 0 Then
                            sStrToReplace = Formula.Substring(iFactorStart + 1, iFactorEnd - 1)

                            If FoundParamNumber(sStrToReplace, dt_Params).ToString <> "" Then
                                FormulaReplace = FormulaReplace.Replace("{" & sStrToReplace & "}", "{" & FoundParamNumber(sStrToReplace, dt_Params) & "}")
                            Else
                                If IsNumeric(sStrToReplace.ToString) Then
                                    FormulaReplace = FormulaReplace.Replace("{" & sStrToReplace & "}", "{" & sStrToReplace & "}")
                                Else
                                    FormulaReplace = FormulaReplace.Replace("{" & sStrToReplace & "}", 0)
                                End If

                            End If

                            Exit For
                        End If
                        iFactorEnd += 1
                    Next

                End If
            Next
            If FormulaReplace = "{}" Then
                FormulaReplace = 0
            End If
            Return FormulaReplace
        Catch ex As Exception
            Throw
        End Try
        Return ""
    End Function

    Public Shared Function ReplaceParamNameWithNumbersForConstants(Formula As String, dt_Params As DataTable) As String
        'Parameters not exist will replaced with {0}
        Try
            Dim FormulaReplace As String = Formula
            Dim iFactorStart As Integer
            Dim iFactorEnd As Integer
            Dim sStrToReplace As String = ""

            Dim sStns As String = "{STN "
            Dim mx As Integer = 5
            If Not Formula.Contains(sStns) Then
                sStns = ""
                mx = Formula.Length
            End If
            If Formula.Length > 4 Then
                mx = 5
            End If

            For i As Integer = 0 To Formula.Length - 1
                If Formula.Substring(i, 1) = "{" AndAlso Formula.Substring(i, mx) <> sStns Then
                    iFactorStart = i
                    iFactorEnd = 0
                    For j As Integer = i To Formula.Length - 1
                        If Formula.Substring(j, 1) = "}" AndAlso iFactorEnd > 0 Then
                            sStrToReplace = Formula.Substring(iFactorStart + 1, iFactorEnd - 1)

                            If FoundParamNumber(sStrToReplace, dt_Params).ToString <> "" Then
                                FormulaReplace = FormulaReplace.Replace("{" & sStrToReplace & "}", "CONST(" & FoundParamNumber(sStrToReplace, dt_Params) & ")")
                            Else
                                FormulaReplace = FormulaReplace.Replace("{" & sStrToReplace & "}", "(0)")
                            End If

                            Exit For
                        End If
                        iFactorEnd += 1
                    Next

                End If
            Next
            If FormulaReplace = "{}" Then
                FormulaReplace = 0
            End If
            Return FormulaReplace
        Catch ex As Exception
            Throw
        End Try
        Return ""
    End Function
    Public Shared Function ReplaceParamNameWithValues(Formula As String, dt_Params As DataTable) As String
        Try
            Dim sCondition As String = ""
            Dim sFormat As String = ""
            Dim sValue As String = ""
            For Each r As DataRow In dt_Params.Rows
                sCondition = r.Item("DescConditions").ToString.Trim
                sFormat = r.Item("DescFormat").ToString.Trim
                sValue = r.Item("DescValue").ToString.Trim
                If sCondition <> "" Then

                End If

                Formula = FormulaResult.ReplaceValWithDescriptionVal(Formula, "{" & r.Item("TabIndex").ToString & "}", r.Item("Measure").ToString)
            Next

            'For Each r As DataRow In dt_Params.Rows
            '    Formula = FormulaResult.ReplaceValWithVal(Formula, "{" & r.Item("TabIndex").ToString & "}", r.Item("Measure").ToString)
            'Next

            Return Formula
        Catch ex As Exception
            Throw
        End Try
        Return ""
    End Function
    Private Shared Function FoundParamNumber(sStrToReplace As String, dt_Params As DataTable) As String
        Try

            Dim sStr As String = ""
            For Each r As DataRow In dt_Params.Rows
                'If r.Item("CostName").ToString.Trim.ToUpper = sStrToReplace.ToString.Trim.ToUpper Or r.Item("Label").ToString.Trim.ToUpper.Contains(sStrToReplace.ToString.Trim.ToUpper) Then
                If r.Item("CostName").ToString.Trim.ToUpper = sStrToReplace.ToString.Trim.ToUpper Or r.Item("Label").ToString.Trim.ToUpper = sStrToReplace.ToString.Trim.ToUpper Then
                    sStr = r.Item("TabIndex").ToString.Trim
                    Exit For
                End If
            Next
            Return sStr
            Throw New NotImplementedException()
        Catch ex As Exception
            Throw
        End Try
    End Function
    'Private Shared Function FoundParamNumber(sStrToReplace As String, dt_Params As DataTable) As String
    '    Try

    '        Dim sStr As String = ""
    '        For Each r As DataRow In dt_Params.Rows
    '            'If r.Item("CostName").ToString.Trim.ToUpper = sStrToReplace.ToString.Trim.ToUpper Or r.Item("Label").ToString.Trim.ToUpper.Contains(sStrToReplace.ToString.Trim.ToUpper) Then
    '            Dim sLabel As String = r.Item("Label").ToString.Trim.ToUpper()

    '            If sLabel.Length > 0 Then
    '                If sLabel.Substring(r.Item("Label").ToString.Trim.Length - 1, 1) = ")" Then
    '                    Dim i = sLabel.Length
    '                    Do Until sLabel.Substring(r.Item("Label").ToString.Trim.Length - 1, 1) = "("
    '                    Loop
    '                End If
    '            End If
    '            If r.Item("CostName").ToString.Trim.ToUpper = sStrToReplace.ToString.Trim.ToUpper Or r.Item("Label").ToString.Trim.ToUpper = sStrToReplace.ToString.Trim.ToUpper Then
    '                sStr = r.Item("TabIndex").ToString.Trim
    '                Exit For
    '            End If
    '        Next
    '        Return sStr
    '        Throw New NotImplementedException()
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function

    Public Shared Function ReturnConvertedFormula(MainFormula As String, dt_Params As DataTable, NewRefernce As String) As String
        Try

            Dim ReturnedFormula As String = ""

            'Reaplase parameters name with parameter number

            'ReturnedFormula = ReplaceSTNparam(MainFormula)
            'ReturnedFormula = ReplaceParamNameWithNumbers(MainFormula, dt_Params)


            'Reaplase STN parameters name with parameter number

        Catch ex As Exception
            Throw
        End Try
        Throw New NotImplementedException()
    End Function

    Public Shared Function ReplaceEmptyParamsL(mainFormula As String) As String
        mainFormula = mainFormula.Replace("+{}", "")
        mainFormula = mainFormula.Replace("*{}", "")
        mainFormula = mainFormula.Replace("-{}", "")
        Return mainFormula
        Throw New NotImplementedException()
    End Function

    Public Shared Function ReplaceEmptyParamsR(mainFormula As String) As String
        mainFormula = mainFormula.Replace("{}+", "")
        mainFormula = mainFormula.Replace("{}*", "")
        mainFormula = mainFormula.Replace("{}-", "")
        Return mainFormula
        Throw New NotImplementedException()
    End Function

    Public Shared Function ReplaceLadder(mainFormula As String) As String
        mainFormula = mainFormula.Replace("#", "")
        Return mainFormula
        Throw New NotImplementedException()
    End Function

    Public Shared Function ReplaceValWithVal(Expretion As String, ValToFind As String, ValToReplace As String) As String

        Dim S As String = ValToFind
        S = Expretion.Replace(ValToFind, ValToReplace)
        s = s.Replace(ValToFind.ToUpper, ValToReplace)
        Return s 'Expretion.Replace(ValToFind, ValToReplace)

        Throw New NotImplementedException()
    End Function


    Public Shared Function ReplaceValWithDescriptionVal(Expretion As String, ValToFind As String, ValToReplace As String) As String
        Return Expretion.Replace(ValToFind, ValToReplace)

        Throw New NotImplementedException()
    End Function


    Public Shared Function ReplaceEmptySpacing(mainFormula As String, ReplaceAllSpaces As Boolean) As String

        'IF {SHANK}= Safe Lock AND {STN SHANK}=C OR {STN SHANK}=W
        'If {SHANK} Then= Safe Lock And {STN SHANK}=C Or {STN SHANK}=W



        'IF {SHANK}= "Safe Lock" AND {STN SHANK}="C" OR {STN SHANK}="W")


        If Not ReplaceAllSpaces Then
            mainFormula = mainFormula.Replace("     ", " ")
            mainFormula = mainFormula.Replace("    ", " ")
            mainFormula = mainFormula.Replace("   ", " ")
            mainFormula = mainFormula.Replace("  ", " ")
            mainFormula = mainFormula.Replace(" {", "{")
            mainFormula = mainFormula.Replace("{ ", "{")
            mainFormula = mainFormula.Replace("} ", "}")
            mainFormula = mainFormula.Replace(" }", "}")
            mainFormula = mainFormula.Replace(" (", "(")
            mainFormula = mainFormula.Replace("( ", "(")
            mainFormula = mainFormula.Replace(") ", ")")
            mainFormula = mainFormula.Replace(" )", ")")
            mainFormula = mainFormula.Replace("= ", "=")
            mainFormula = mainFormula.Replace(" =", "=")
            mainFormula = mainFormula.Replace(" IF", "IF")
            mainFormula = mainFormula.Replace("IF ", "IF")
            mainFormula = mainFormula.Replace(" Relation", "Relation")
            mainFormula = mainFormula.Replace("Relation ", "Relation")
            mainFormula = mainFormula.Replace("* ", "*")
            mainFormula = mainFormula.Replace(" *", "*")
            mainFormula = mainFormula.Replace("+ ", "+")
            mainFormula = mainFormula.Replace(" +", "+")
            mainFormula = mainFormula.Replace("> ", ">")
            mainFormula = mainFormula.Replace(" <", "<")
            mainFormula = mainFormula.Replace("= ", "=")
            mainFormula = mainFormula.Replace(" =", "=")
            mainFormula = mainFormula.Replace("- ", "-")
            mainFormula = mainFormula.Replace(" -", "-")

            mainFormula = mainFormula.Replace("{     }", "{}")
            mainFormula = mainFormula.Replace("{    }", "{}")
            mainFormula = mainFormula.Replace("{   }", "{}")
            mainFormula = mainFormula.Replace("{  }", "{}")
            mainFormula = mainFormula.Replace("{ }", "{}")
        Else
            mainFormula = mainFormula.Replace("     ", " ")
            mainFormula = mainFormula.Replace("    ", " ")
            mainFormula = mainFormula.Replace("   ", " ")
            mainFormula = mainFormula.Replace("  ", " ")
            mainFormula = mainFormula.Replace(" ", "")
        End If




        'mainFormula = mainFormula.Replace(" ", "")
        'mainFormula = mainFormula.Replace("{STN", "{STN ")
        ''mainFormula = mainFormula.Replace("AND", "AND")

        Return mainFormula

        Throw New NotImplementedException()
    End Function

    Public Shared Function ReplacebracketForRelation(mainFormula As String) As String
        Try
            If mainFormula.Contains("Relation") Then
                'mainFormula = mainFormula.Replace("Relation", "Relation(")
                'mainFormula = mainFormula.Replace("AND", ")AND(")

                Dim _mainFormula_L As String
                Dim _mainFormula_R As String

                _mainFormula_L = mainFormula.Substring(mainFormula.IndexOf("(") + 1, mainFormula.IndexOf(",") - (mainFormula.IndexOf("(") + 1))
                _mainFormula_R = mainFormula.Substring(mainFormula.IndexOf(","), mainFormula.Length - mainFormula.IndexOf(","))


                _mainFormula_L = _mainFormula_L.Replace("AND", ")AND(")
                _mainFormula_L = _mainFormula_L.Replace("OR", ")OR(")
                _mainFormula_L = "(" & _mainFormula_L & ")"

                Return ("Relation(" & _mainFormula_L & _mainFormula_R)
            Else
                Return mainFormula

            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function AddApostrophes(mainFormula As String) As String
        Dim ij As Integer = 1
        Dim j As Integer = 1
        Dim bInserted As Boolean = False

        Dim m As String = mainFormula

        For i As Integer = 0 To m.Length - 1
            Dim c As Char = m(i)
            If (c = "=" Or c = ">") AndAlso Not IsNumeric(m(i + 1)) AndAlso m(i + 1) <> "{" Then
                For ij = i To m.Length - 1
                    bInserted = False
                    If Char.IsLetter(m.Chars(ij)) Then
                        m = m.Insert(ij, """")
                        For j = ij To m.Length - 1
                            bInserted = False
                            If m.Chars(j) = " " Then
                                m = m.Insert(j + 1, "#")
                                'm = m.Substring(0, j) & "#" & m.Substring(j, m.Length - j - 1)
                            ElseIf m.Chars(j) = ")" Then
                                m = m.Insert(j, """")
                                bInserted = True
                                Exit For
                            End If

                        Next
                    End If
                    If bInserted = True Then
                        Exit For
                    End If
                Next


                'Do Until Not Char.IsLetter(mainFormula.Chars(j)) And mainFormula.Chars(j) <> " "
                '    mainFormula = mainFormula.Insert(j, """")
                '    j += 1
                '    i = j
                '    Exit Do
                'Loop
                'If bInserted = True Then

                '    Do Until mainFormula.Chars(j) = ")" ' (Not Char.IsLetter(mainFormula.Chars(j)) And mainFormula.Chars(j) <> " ")
                '        j += 1
                '        mainFormula.Insert(j - 1, """")
                '        i = j
                '        Exit Do
                '    Loop

                'End If


            End If
        Next
        Return m

        Throw New NotImplementedException()
    End Function

    Public Shared Function ReplaceFunctions(mainFormula As String, ValueParamToReturnIfTrue As Decimal) As String
        If mainFormula.Contains("IF") Then
            mainFormula = mainFormula.Replace("IF", "Relation(")
            mainFormula = mainFormula & "," & ValueParamToReturnIfTrue & ",0)"
        End If


        Return mainFormula

        Throw New NotImplementedException()
    End Function

    'Public Shared Function AddFactorsToFormula(mainFormula As String) As String


    '    mainFormula = mainFormula.Replace("{", "FAC{")

    '    Return mainFormula

    '    Throw New NotImplementedException()
    'End Function






#End Region






    Private Function PrepareOperators(ByVal s As String) As String
        Dim ReturnFormula As String = ""
        If s <> "" Then
            ReturnFormula = "(" & s & ")"
        Else
            Dim iiii As Integer = 0
        End If
        Dim UpperCaseFormula As String

        For i As Integer = 0 To UBound(ComplexOperators, 1)
            Dim place As Integer = 0
            Dim finished As Boolean = False
            While Not finished
                UpperCaseFormula = ReturnFormula.ToUpper
                place = UpperCaseFormula.IndexOf(ComplexOperators(i, 0), place)
                If place <> -1 Then
                    If (Split(UpperCaseFormula.Substring(0, place), """").Length Mod 2) = 0 Then
                        'within ""
                        'no replace
                    Else
                        'not within ""
                        'ReturnFormula = ReturnFormula.Substring(0, place - 1) & Replace(UpperCaseFormula, ComplexOperators(i, 0), ComplexOperators(i, 1), place, 1)
                        ReturnFormula = ReturnFormula.Substring(0, place) & _
                                        UpperCaseFormula.Replace(ComplexOperators(i, 0), ComplexOperators(i, 1)).Substring(place, ComplexOperators(i, 1).Length) & _
                                        ReturnFormula.Substring(place + ComplexOperators(i, 0).Length)
                    End If

                    place += 1
                Else
                    finished = True
                End If
            End While
        Next
        Return ReturnFormula
    End Function
    Private Function IsOperator(ByVal ch As String) As Boolean
        For i As Integer = 0 To UBound(ComplexOperators, 1)
            If ch = ComplexOperators(i, 1) Then
                Return True
            End If
        Next
        For i As Integer = 0 To UBound(Operators)
            If ch = Operators(i) Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Shared Function ReplaceSpecialLabels(ByVal _formula As String, Optional ByVal QutNo As String = "") As String


        For i As Integer = 0 To UBound(SpecialLabels, 1)
            If SpecialLabels(i, 1) = "" AndAlso InStr(_formula, "Relation") > 0 Then
                Dim j As Integer
                j = 100
            Else
                If SpecialLabels(i, 0).ToString = "#####/#" Then
                    If QutNo <> "" Then
                        _formula = _formula.Replace(SpecialLabels(i, 0), QutNo & "/" & 0)
                    End If
                Else
                    _formula = _formula.Replace(SpecialLabels(i, 0), SpecialLabels(i, 1))
                End If
            End If
        Next

        '--05.09.11--
        'For i As Integer = 0 To UBound(SpecialLabels, 1)
        '    If SpecialLabels(i, 0).ToString = "#####/#" Then
        '        If QutNo <> "" Then
        '            _formula = _formula.Replace(SpecialLabels(i, 0), QutNo & "/" & QutVersion)
        '        End If
        '    Else
        '        _formula = _formula.Replace(SpecialLabels(i, 0), SpecialLabels(i, 1))
        '    End If
        'Next
        '------------

        Return _formula


        'For i As Integer = 0 To UBound(SpecialLabels, 1)
        '    If SpecialLabels(i, 0).ToString = "#####/#" Then
        '        If QutNo <> "" Then
        '            _formula = _formula.Replace(SpecialLabels(i, 0), QutNo & "/" & QutVersion)
        '        End If
        '    Else
        '        If SpecialLabels(i, 1) = "" AndAlso InStr(_formula, "Relation") > 0 Then
        '            Dim _formulaTemp As String = Mid(_formula, InStr(_formula, ","), _formula.Length)
        '            _formulaTemp = _formulaTemp.Replace(SpecialLabels(i, 0), SpecialLabels(i, 1))
        '            _formula = Left(_formula, InStr(_formula, ",") - 1) & _formulaTemp
        '        Else
        '            _formula = _formula.Replace(SpecialLabels(i, 0), SpecialLabels(i, 1))
        '        End If
        '    End If
        'Next
        'Return _formula


        'For i As Integer = 0 To UBound(SpecialLabels, 1)
        '    If SpecialLabels(i, 0).ToString = "#####/#" Then
        '        If QutNo <> "" Then
        '            _formula = _formula.Replace(SpecialLabels(i, 0), QutNo & "/" & QutVersion)
        '        End If
        '    Else
        '        If _formula.IndexOf("Relation") = 0 Then
        '            _formula = _formula.Replace(SpecialLabels(i, 0), SpecialLabels(i, 1))
        '        Else
        '            If SpecialLabels(i, 1) <> "" Then
        '                Dim _formulaTemp As String = Mid(_formula, _formula.IndexOf("Relation"), _formula.Length)

        '                _formula = _formula.Replace(SpecialLabels(i, 0), SpecialLabels(i, 1))
        '            Else

        '            End If

        '        End If
        '    End If
        'Next
        'Return _formula

    End Function
    Public Shared Function ReplaceSpecialLabelsDisplay(ByVal _formula As String) As String
        Try
            For i As Integer = 0 To UBound(SpecialLabelsDisplay, 1)
                _formula = _formula.Replace(SpecialLabelsDisplay(i, 0), SpecialLabelsDisplay(i, 1))
            Next

            If _formula.Length > 2 Then
                If Left(_formula.ToString, 1) = """" AndAlso Right(_formula.ToString, 1) = """" Then
                    _formula = Mid(_formula, 2, _formula.Length - 2)
                End If
            End If
            If _formula = "," Then
                _formula = ""
            End If
            Return _formula
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Function Parse(Optional ByVal WithCompute As Boolean = False) As Item
        Dim s As String = ""
        Try

            ' 1. Prepare sinle item list
            Dim isQuatedStringStarted As Boolean = False
            Dim finished As Boolean = False
            Dim i As Integer = -1
            Dim tmp As String = ""
            Dim LastChar As String = ""
            Dim CurrentChar As String = ""
            'Dim CurrentNode As Node = New Node("", Nothing)

            Dim RootItem As Item
            Dim CurrentItem As Item

            RootItem = New Item("", Nothing)
            CurrentItem = RootItem

            s = Formula

            Dim s1 As Integer = 0
            Dim s2 As Integer = 0
            Dim s3 As Integer = 0
            Dim s4 As Integer = 0
            If Not Formula Is Nothing Then
                For DD = 0 To Formula.Length - 1
                    If s(DD) = "(" Then s1 += 1
                    If s(DD) = ")" Then s2 += 1
                    If s(DD) = "{" Then s3 += 1
                    If s(DD) = "}" Then s4 += 1
                Next
            End If



            If s1 <> s2 Or s3 <> s4 Then
                'GeneralException.WriteEventErrors("Parse - Number Of brackets Is different in Formula " & Formula, GeneralException.e_LogTitle.PARSER.ToString)
                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "Parse", "Parse - Number Of brackets Is different in Formula " & Formula, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

                Throw New Exception
                'Return RootItem
            Else

                s = PrepareOperators(s)

                While Not finished
                    i += 1
                    If i > s.Length - 1 Then
                        finished = True
                        Continue While
                    End If
                    LastChar = CurrentChar
                    CurrentChar = s.Substring(i, 1) '.ToUpper

                    If isQuatedStringStarted Then
                        If CurrentChar = """" Then

                        Else
                            tmp &= CurrentChar
                            Continue While
                        End If
                    End If
                    '"
                    If CurrentChar = """" Then
                        If Not isQuatedStringStarted Then
                            'first time - אי-זוגי
                            If tmp <> "" Then
                                CurrentItem = New Item(tmp, CurrentItem)
                                tmp = ""
                            End If
                            tmp = ""
                            'CurrentItem = New Item("""", CurrentItem)
                            isQuatedStringStarted = True
                            Continue While
                        Else
                            'second time - זוגי
                            CurrentItem = New Item("""" & tmp & """", CurrentItem)
                            'CurrentItem = New Item(tmp, CurrentItem)
                            isQuatedStringStarted = False
                            tmp = ""
                            Continue While
                        End If
                    End If

                    'A-Z, 0-9, <space>
                    If (CurrentChar >= "A" And CurrentChar <= "Z") OrElse (CurrentChar >= "a" And CurrentChar <= "z") OrElse CurrentChar = " " OrElse CurrentChar = "." OrElse (CurrentChar >= "0" And CurrentChar <= "9") Then
                        tmp &= CurrentChar
                        Continue While
                    End If

                    '(,),{,}                        TODO:??? ,[,]
                    If CurrentChar = "(" OrElse CurrentChar = ")" OrElse CurrentChar = "{" OrElse CurrentChar = "}" Then '  TODO: ??? OrElse CurrentChar = "[" OrElse CurrentChar = "]" Then
                        If tmp <> "" Then
                            CurrentItem = New Item(tmp, CurrentItem)
                            tmp = ""
                        End If
                        CurrentItem = New Item(CurrentChar, CurrentItem)
                        Continue While
                    End If

                    'all arithmetics operators (single characters)
                    If IsOperator(CurrentChar) Then
                        If tmp <> "" Then
                            CurrentItem = New Item(tmp, CurrentItem)
                            tmp = ""
                        End If
                        CurrentItem = New Item(CurrentChar, CurrentItem)
                        Continue While
                    End If

                    If CurrentChar = "," Then
                        If tmp <> "" Then
                            CurrentItem = New Item(tmp, CurrentItem)
                            tmp = ""
                        End If
                        CurrentItem = New Item(CurrentChar, CurrentItem)
                        Continue While
                    End If
                    i += 1
                End While

                If tmp <> "" Then
                    CurrentItem = New Item(tmp, CurrentItem)
                    tmp = ""
                End If


                '2. טיפול בסוגריים
                finished = False
                CurrentItem = RootItem
                Dim LastLeftParenthes As Item
                LastLeftParenthes = Nothing
                Dim Pointers As New List(Of Item)
                Dim tmpCur As Item = Nothing
                Dim ifSkip As Boolean = False

                While Not finished
                    If Not ifSkip Then
                        CurrentItem = CurrentItem.NextItem
                    End If
                    ifSkip = False

                    If CurrentItem Is Nothing Then
                        If Pointers.Count = 0 Then
                            Exit While
                        Else
                            CurrentItem = Pointers(Pointers.Count - 1)
                            Pointers.RemoveAt(Pointers.Count - 1)
                            Continue While
                        End If
                    End If

                    If CurrentItem.Expression = "{" Then
                        CurrentItem.Expression = "{" & CurrentItem.NextItem.Expression & "}"
                        Dim DeleteItem As Item = CurrentItem.NextItem
                        CurrentItem.NextItem = CurrentItem.NextItem.NextItem.NextItem
                        If Not DeleteItem.NextItem.NextItem Is Nothing Then
                            DeleteItem.NextItem.NextItem.PrevItem = CurrentItem
                        End If
                        DeleteItem.NextItem.NextItem = Nothing
                        DeleteItem.NextItem.PrevItem = Nothing
                        DeleteItem.NextItem = Nothing
                        DeleteItem.PrevItem = Nothing
                        Continue While
                    End If
                    If CurrentItem.Expression = "(" Then
                        LastLeftParenthes = CurrentItem
                        Pointers.Add(CurrentItem.PrevItem)
                        Continue While
                    End If
                    If CurrentItem.Expression = ")" Then
                        'LastLeftParenthes.PrevItem.SubList = LastLeftParenthes
                        Pointers(Pointers.Count - 1).SubList = LastLeftParenthes

                        LastLeftParenthes.PrevItem = Nothing

                        LastLeftParenthes.SubParent = Pointers(Pointers.Count - 1)

                        'LastLeftParenthes.PrevItem.NextItem = CurrentItem.NextItem
                        Pointers(Pointers.Count - 1).NextItem = CurrentItem.NextItem

                        If Not CurrentItem.NextItem Is Nothing Then
                            CurrentItem.NextItem.PrevItem = Pointers(Pointers.Count - 1) 'LastLeftParenthes.PrevItem
                        End If

                        If Pointers(Pointers.Count - 1).Expression <> "(" Then
                            Dim ix As Integer = Pointers.Count - 1
                            Dim flag As Boolean = Pointers(ix).Expression <> "("
                            While (ix > 0) And (flag)
                                ix -= 1
                                flag = Pointers(ix).Expression <> "("
                                If flag Then
                                    flag = Pointers(ix).NextItem.Expression <> "("
                                    If flag Then

                                    Else
                                        LastLeftParenthes = Pointers(ix).NextItem
                                        Exit While
                                    End If
                                Else
                                    LastLeftParenthes = Pointers(ix).NextItem   'TODO: ?????????
                                    Exit While
                                End If
                            End While
                            If ix = 0 Then
                                LastLeftParenthes = Pointers(0).NextItem
                            End If
                        Else
                            LastLeftParenthes = Pointers(Pointers.Count - 1)
                        End If

                        Pointers.RemoveAt(Pointers.Count - 1)


                        'If Pointers.Count > 0 Then
                        tmpCur = CurrentItem.NextItem
                        CurrentItem.NextItem = Nothing      'todo: if next is ( - do not nothing
                        'Pointers.Add(CurrentItem)
                        CurrentItem = tmpCur
                        'End If
                        ifSkip = True
                        Continue While
                    End If
                End While

            End If

            ' '' ''3. tree build with math Priority
            '' ''CurrentItem = RootItem
            '' ''PreBuildStructure(CurrentItem)

            ' '' ''Tree = CurrentNode
            ' '' ''CurrentNode.Nodes.Add(GetNode(CurrentItem, CurrentNode))




            '3.
            CurrentItem = RootItem

            If WithCompute Then
                Compute(CurrentItem)
            End If






            Return RootItem

        Catch ex As Exception
            'GeneralException.WriteEventErrors("Parse-" & ex.Message & " - formula : " & s, GeneralException.e_LogTitle.PARSER.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "Parse", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
            Throw
        End Try


    End Function
    Public Function ParseAndCalculate() As String
        'Dim curui As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
        'Dim curui1 As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentUICulture

        'System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US")
        'System.Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo("en-US")
        Dim itm As Item

        Try

            itm = Parse(True)

            While Not itm.SubList Is Nothing
                itm = itm.SubList
            End While
        Catch ex As Exception
            'GeneralException.WriteEventErrors("ParseAndCalculate-" & ex.Message, GeneralException.e_LogTitle.PARSER.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "ParseAndCalculate", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw New Exception(ex.Message)
            'Finally
            '    System.Threading.Thread.CurrentThread.CurrentCulture = curui
            '    System.Threading.Thread.CurrentThread.CurrentUICulture = curui1
        End Try

        Return itm.Expression




    End Function

    Private Sub Compute(ByVal itm As Item)


        Dim sSsubListCurrent As String = ""

        Try

            Dim CurrentConstname As String

            Dim subListCurrent As Item = itm

            Try
                sSsubListCurrent = subListCurrent.Expression
            Catch ex As Exception

            End Try

            While Not subListCurrent Is Nothing
                If Not subListCurrent.SubList Is Nothing Then
                    Compute(subListCurrent.SubList)
                End If
                subListCurrent = subListCurrent.NextItem
            End While

            '1. replace all parameters {}, compute all functions
            '2. execute all operators
            '3. delete ()

            Dim lastCurrentItem As Item = itm
            subListCurrent = itm
            '1.
            Dim STNParam As String = "NOTstn"
            While Not subListCurrent Is Nothing
                If Not IsNumeric(subListCurrent.Expression) AndAlso subListCurrent.Expression <> "(" AndAlso subListCurrent.Expression <> ")" Then

                    'parameter
                    If Left(subListCurrent.Expression, 1) = "{" And Right(subListCurrent.Expression, 1) = "}" Then
                        If Left(subListCurrent.Expression, 5) = "{STN " Then
                            STNParam = subListCurrent.Expression.ToUpper
                        Else
                            subListCurrent.ParameterIndex = CInt(Mid(subListCurrent.Expression, 2, Len(subListCurrent.Expression) - 2))
                            subListCurrent.Expression = Parameters.Rows(Mid(subListCurrent.Expression, 2, Len(subListCurrent.Expression) - 2) - 1)("Measure").ToString
                            lastCurrentItem = lastCurrentItem.NextItem
                            subListCurrent = lastCurrentItem
                            Continue While
                        End If




                    End If

                    Try
                        sSsubListCurrent = subListCurrent.Expression
                    Catch ex As Exception

                    End Try
                    'compute function
                    Dim Params As List(Of Item)
                    Select Case UCase(subListCurrent.Expression)


                        Case "GETSCALE"
                            'subListCurrent.Expression = "GETSCALE:" & Calc_SCALE(subListCurrent.SubList) & ";"
                            subListCurrent.Expression = Calc_GetSCALE(subListCurrent.SubList)
                            'subListCurrent.SubList.SubParent = Nothing
                            'subListCurrent.SubList = Nothing

                        Case "GETMAINMATERIAL"
                            'subListCurrent.Expression = "GETWORKPIECEMATERIAL:" & Calc_WORKPIECEMATERIAL(subListCurrent.SubList) & ";"
                            subListCurrent.Expression = CALC_GETMAINMATERIAL(subListCurrent.SubList)
                            'subListCurrent.SubList.SubParent = Nothing
                            'subListCurrent.SubList = Nothing


                        Case "GETMATERIAL"
                            'subListCurrent.Expression = "GETMATERIAL:" & Calc_MATERIAL(subListCurrent.SubList) & ";"
                            subListCurrent.Expression = Calc_GetMATERIAL(subListCurrent.SubList)
                            'subListCurrent.SubList.SubParent = Nothing
                            'subListCurrent.SubList = Nothing

                        Case "GETMAINMATERIALCAT"
                            'subListCurrent.Expression = "GETWORKPIECEMATERIALCATEGORY:" & Calc_MATERIALCATEGORY(subListCurrent.SubList) & ";"
                            subListCurrent.Expression = CALC_GETMAINMATERIALCAT(subListCurrent.SubList)
                            'subListCurrent.SubList.SubParent = Nothing
                            'subListCurrent.SubList = Nothing


                            '----------------------------------------------------------------------------


                        Case "GETFAMILY"
                            'GetFamily(ItemRefernce)
                            subListCurrent.Expression = Calc_GetFamily()

                        ''Case "GETFAMILYMODIFICATION"
                        ''    subListCurrent.Expression = Calc_GetFamilyModification()


                        Case "STRINGVALUE"
                            subListCurrent.Expression = Calc_StringValue(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing

                        Case "STRINGDESC"
                            subListCurrent.Expression = Calc_StringDesc(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing

                        Case "QTYFCT"
                            subListCurrent.Expression = Calc_QTY_Fct()
                            'subListCurrent.Expression = Calc_QTYFct()
                        Case "QBVALUEBYQTY"
                            subListCurrent.Expression = Calc_QBValueByQty(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "INPUTMEASURE"
                            itm.Expression = Calc_InputMeasure()

                        Case "RELATION"
                            Params = PrapareParameters(subListCurrent.SubList, 3)
                            subListCurrent.Expression = ReplaceSpecialLabelsDisplay(Calc_Relation(Params(0), Params(1), Params(2)))
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "CROSLIST"
                            Params = PrapareParameters(subListCurrent.SubList)
                            subListCurrent.Expression = Calc_CrosList(Params)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "CROSLISTCHECK"
                            Params = PrapareParameters(subListCurrent.SubList)
                            subListCurrent.Expression = Calc_CrosListCheck(Params)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing


                        Case "DISPLAYFROMLIST"
                            Params = PrapareParameters(subListCurrent.SubList)
                            subListCurrent.Expression = Calc_DisplayFromList(subListCurrent.SubList, Params(0), Params(1))
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing


                        Case "CROS"
                            Params = PrapareParameters(subListCurrent.SubList)
                            subListCurrent.Expression = Calc_Cros(Params)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "RANGERULE"
                            subListCurrent.Expression = Calc_RangeRule(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing

                        Case "CHECKINLIST"
                            subListCurrent.Expression = Calc_CheckinList(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "GETCATNUM"
                            subListCurrent.Expression = Calc_GetCatNum()
                        Case "GETPRICE"
                            Params = PrapareParameters(subListCurrent.SubList, 2)
                            subListCurrent.Expression = Calc_GetPrice(Params(0), Params(1))
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "GETCURRENTBRANCH"
                            subListCurrent.Expression = Calc_GetCurrentBranch()
                        Case "GETCURRENTBRANCHCODE"
                            subListCurrent.Expression = Calc_GetCurrentBranchCode()
                        Case "SETREMARKS"
                            Params = PrapareParameters(subListCurrent.SubList, 2)
                            'subListCurrent.Expression = Calc_SetRemarks(Params(0), Params(1))
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "SHOWMESSAGE"
                            Params = PrapareParameters(subListCurrent.SubList, 2)
                            subListCurrent.Expression = Calc_ShowMessage(Params(0), Params(1))
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "GETLENGTH"
                            subListCurrent.Expression = Calc_GetLength(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "GETMAX"
                            Params = PrapareParameters(subListCurrent.SubList, 2)
                            subListCurrent.Expression = Calc_GetMax(Params(0), Params(1))
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "GETRIGHT"
                            Params = PrapareParameters(subListCurrent.SubList, 2)
                            subListCurrent.Expression = Calc_GetRight(Params(0), Params(1))
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing

                        Case "GETREPLACE"
                            Params = PrapareParameters(subListCurrent.SubList, 3)
                            subListCurrent.Expression = Calc_GetReplace(Params(0), Params(1), Params(2))
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing

                        Case "GETNUMERICVALUES"
                            subListCurrent.Expression = Calc_GetNumericValues(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "ITEMRANGEGROUP"
                            Params = PrapareParameters(subListCurrent.SubList, 4)
                            subListCurrent.Expression = Calc_ItemRangeGroup(Params(0), Params(1), Params(2), Params(3))
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing

                        Case "VALUEBYRANGE"
                            Params = PrapareParameters(subListCurrent.SubList, 4)
                            'subListCurrent.Expression = Calc_DisplayFromList(subListCurrent.SubList, Params(0), Params(1))
                            subListCurrent.Expression = Calc_ValueByRange(Params(0), Params(1), Params(3).Expression)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing


                        Case "GETWCENTER"
                            'subListCurrent.Expression = Calc_GetWCenter()
                        Case "GETCUSTOMERNUM"
                            subListCurrent.Expression = Calc_GetCustomerNum()
                        Case "GETWORKCENTER"
                            'subListCurrent.Expression = Calc_GetWCenter()
                        Case "GETCURRENCIESRATIO"
                            Params = PrapareParameters(subListCurrent.SubList, 2)
                            subListCurrent.Expression = Calc_GetCurrenciesRatio(Params(0), Params(1))
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing

                        Case Calc_GetCurrency()
                            subListCurrent.Expression = Calc_GetCurrency()
                        Case "GETREFERENCEDATA"
                            Params = PrapareParameters(subListCurrent.SubList, 2)
                            subListCurrent.Expression = Calc_GetReferenceData(Params(0), Params(1))
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "DRAWINGNUM"
                            'subListCurrent.Expression = Calc_GetDrawingNum()
                        Case "INSTRING"
                            Params = PrapareParameters(subListCurrent.SubList, 2)
                            subListCurrent.Expression = Calc_InString(Params(0), Params(1))
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                            '=================================================================================================
                            'Computed Constansts
                            'Case "MOD"
                            '    Params = PrapareParameters(subListCurrent.SubList, 2)
                            '    subListCurrent.Expression = Calc_Math_MOD(Params(0), Params(1))
                            '    subListCurrent.SubList.SubParent = Nothing
                            '    subListCurrent.SubList = Nothing


                        Case "USERGROUP"
                            Params = PrapareParameters(subListCurrent.SubList, 2)
                            subListCurrent.Expression = Calc_USER_GROUP()
                            'subListCurrent.SubList.SubParent = Nothing
                            'subListCurrent.SubList = Nothing

                        Case "FAMILYGP"
                            subListCurrent.Expression = Calc_FAMILYGP()
                        Case "SUBGP"
                            subListCurrent.Expression = Calc_SUBGP()
                        Case "CONST"
                            CurrentConstname = subListCurrent.SubList.Expression
                            Select Case ConstantsState
                                Case ConstantsStates.Compute
                                    subListCurrent.Expression = Calc_Const(subListCurrent.SubList, SPSETUP, STNSETUP, Quantity)
                                Case ConstantsStates.ComputeAndStore
                                    subListCurrent.Expression = Calc_Const(subListCurrent.SubList, SPSETUP, STNSETUP, Quantity)
                                    SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                                Case ConstantsStates.RetrieveOnly
                                    subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            End Select
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "GP1"
                            subListCurrent.Expression = Calc_Gp(1)
                            'CurrentConstname = "GP1"
                            'Select Case ConstantsState
                            '    Case ConstantsStates.Compute
                            '        subListCurrent.Expression = Calc_Gp(1)
                            '    Case ConstantsStates.ComputeAndStore
                            '        subListCurrent.Expression = Calc_Gp(1)
                            '        SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                            '    Case ConstantsStates.RetrieveOnly
                            '        subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            'End Select
                        Case "GP2"
                            subListCurrent.Expression = Calc_Gp(2)
                            'CurrentConstname = "GP2"
                            'Select Case ConstantsState
                            '    Case ConstantsStates.Compute
                            '        subListCurrent.Expression = Calc_Gp(2)
                            '    Case ConstantsStates.ComputeAndStore
                            '        subListCurrent.Expression = Calc_Gp(2)
                            '        SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                            '    Case ConstantsStates.RetrieveOnly
                            '        subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            'End Select
                        Case "GP3"
                            subListCurrent.Expression = Calc_Gp(3)
                            'CurrentConstname = "GP3"
                            'Select Case ConstantsState
                            '    Case ConstantsStates.Compute
                            '        subListCurrent.Expression = Calc_Gp(3)
                            '    Case ConstantsStates.ComputeAndStore
                            '        subListCurrent.Expression = Calc_Gp(3)
                            '        SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                            '    Case ConstantsStates.RetrieveOnly
                            '        subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            'End Select
                        Case "STNNET"
                            'subListCurrent.Expression = Calc_Stn()
                            CurrentConstname = "STNNET"
                            Select Case ConstantsState
                                Case ConstantsStates.Compute
                                    subListCurrent.Expression = Calc_NET_STNCustomerPrice()
                                Case ConstantsStates.ComputeAndStore
                                    subListCurrent.Expression = Calc_NET_STNCustomerPrice()
                                    SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                                Case ConstantsStates.RetrieveOnly
                                    subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            End Select
                        Case "STNTFR"
                            'subListCurrent.Expression = Calc_Stn()
                            CurrentConstname = "STNTFR"
                            Select Case ConstantsState
                                Case ConstantsStates.Compute
                                    subListCurrent.Expression = Calc_TFR_STNCustomerPrice()
                                Case ConstantsStates.ComputeAndStore
                                    subListCurrent.Expression = Calc_TFR_STNCustomerPrice()
                                    SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                                Case ConstantsStates.RetrieveOnly
                                    subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            End Select
                        Case STNParam ' STN WITH PARAM ADN REFERENCE
                            'subListCurrent.Expression = Calc_Stn()
                            CurrentConstname = "StnWithRefernce"
                            Select Case ConstantsState
                                Case ConstantsStates.Compute
                                    subListCurrent.Expression = Calc_StnWithRefernce(STNParam)
                                Case ConstantsStates.ComputeAndStore
                                    subListCurrent.Expression = Calc_StnWithRefernce(STNParam)
                                    SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                                Case ConstantsStates.RetrieveOnly
                                    subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            End Select
                        'Case "TC"
                        '    'subListCurrent.Expression = Calc_Stn()
                        '    '-------09.01.10 Bilal LTD_GP----------
                        '    CurrentConstname = "TC"

                        '    'subListCurrent.Expression = Calc_GetFactorQuantity()
                        '    'SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                        '    'ChangeConstantRowValue(CurrentConstname, subListCurrent.Expression)


                        '    Select Case ConstantsState
                        '        Case ConstantsStates.Compute
                        '            subListCurrent.Expression = Calc_TC()
                        '        Case ConstantsStates.ComputeAndStore
                        '            subListCurrent.Expression = Calc_TC()
                        '            SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                        '        Case ConstantsStates.RetrieveOnly
                        '            subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                        '    End Select
                        '    '--------------------------------------
                        Case "RATE"
                            'todo
                            'subListCurrent.Expression = Calc_Rate()'99.99
                            CurrentConstname = "RATE"
                            Select Case ConstantsState
                                Case ConstantsStates.Compute
                                    subListCurrent.Expression = Calc_Rate("RATE")
                                Case ConstantsStates.ComputeAndStore
                                    subListCurrent.Expression = Calc_Rate("RATE")
                                    SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                                Case ConstantsStates.RetrieveOnly
                                    subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            End Select
                        Case "EURTOUSD"
                            CurrentConstname = "EURTOUSD"
                            Select Case ConstantsState
                                Case ConstantsStates.Compute
                                    subListCurrent.Expression = Calc_Rate("EURTOUSD")
                                Case ConstantsStates.ComputeAndStore
                                    subListCurrent.Expression = Calc_Rate("EURTOUSD")
                                    SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                                Case ConstantsStates.RetrieveOnly
                                    subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            End Select
                        Case "WCTOUSD"
                            CurrentConstname = "WCTOUSD"
                            Select Case ConstantsState
                                Case ConstantsStates.Compute
                                    subListCurrent.Expression = Calc_Rate("WCTOUSD")
                                Case ConstantsStates.ComputeAndStore
                                    subListCurrent.Expression = Calc_Rate("WCTOUSD")
                                    SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                                Case ConstantsStates.RetrieveOnly
                                    subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            End Select
                        Case "RATEMKTUSD"
                            CurrentConstname = "RATEMKTUSD"
                            Select Case ConstantsState
                                Case ConstantsStates.Compute
                                    subListCurrent.Expression = Calc_Rate("RATEMKTUSD")
                                Case ConstantsStates.ComputeAndStore
                                    subListCurrent.Expression = Calc_Rate("RATEMKTUSD")
                                    SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                                Case ConstantsStates.RetrieveOnly
                                    subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            End Select
                        Case "RATETFRUSD"
                            CurrentConstname = "RATETFRUSD"
                            Select Case ConstantsState
                                Case ConstantsStates.Compute
                                    subListCurrent.Expression = Calc_Rate("RATETFRUSD")
                                Case ConstantsStates.ComputeAndStore
                                    subListCurrent.Expression = Calc_Rate("RATETFRUSD")
                                    SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                                Case ConstantsStates.RetrieveOnly
                                    subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            End Select
                        Case "TFRTOEUR"
                            CurrentConstname = "TFRTOEUR"
                            Select Case ConstantsState
                                Case ConstantsStates.Compute
                                    subListCurrent.Expression = Calc_Rate("TFRTOEUR")
                                Case ConstantsStates.ComputeAndStore
                                    subListCurrent.Expression = Calc_Rate("TFRTOEUR")
                                    SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                                Case ConstantsStates.RetrieveOnly
                                    subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            End Select
                        Case "WCGP"
                            'subListCurrent.Expression = 1
                            'subListCurrent.Expression = Calc_CGp(1)
                            CurrentConstname = "WCGP"
                            Select Case ConstantsState
                                Case ConstantsStates.Compute
                                    subListCurrent.Expression = Calc_WCGP()
                                Case ConstantsStates.ComputeAndStore
                                    subListCurrent.Expression = Calc_WCGP()
                                    SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                                Case ConstantsStates.RetrieveOnly
                                    subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            End Select

                        'Case "CGP1"
                        '    'subListCurrent.Expression = Calc_CGp(1)
                        '    CurrentConstname = "CGP1"
                        '    Select Case ConstantsState
                        '        Case ConstantsStates.Compute
                        '            subListCurrent.Expression = Calc_CGp(1)
                        '        Case ConstantsStates.ComputeAndStore
                        '            subListCurrent.Expression = Calc_CGp(1)
                        '            SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                        '        Case ConstantsStates.RetrieveOnly
                        '            subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                        '    End Select

                        Case "DESCRIPTION"
                            subListCurrent.Expression = Calc_Description()
                        'Case "DISREF"
                        '    subListCurrent.Expression = Calc_DISREF()

                        'Case "CGP2"
                        '    'subListCurrent.Expression = Calc_CGp(2)
                        '    CurrentConstname = "CGP2"
                        '    Select Case ConstantsState
                        '        Case ConstantsStates.Compute
                        '            subListCurrent.Expression = Calc_CGp(2)
                        '        Case ConstantsStates.ComputeAndStore
                        '            subListCurrent.Expression = Calc_CGp(2)
                        '            SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                        '        Case ConstantsStates.RetrieveOnly
                        '            subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                        '    End Select
                        Case "TFRCURRENCY"
                            CurrentConstname = "TFRCURRENCY"
                            Select Case ConstantsState
                                Case ConstantsStates.Compute
                                    subListCurrent.Expression = Calc_TFR_STNBranchCurrency()
                                Case ConstantsStates.ComputeAndStore
                                    subListCurrent.Expression = Calc_TFR_STNBranchCurrency()
                                    SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                                Case ConstantsStates.RetrieveOnly
                                    subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            End Select
                        Case "NETCURRENCY"
                            CurrentConstname = "NETCURRENCY"
                            Select Case ConstantsState
                                Case ConstantsStates.Compute
                                    subListCurrent.Expression = Calc_NET_STNCustomerCurrency()
                                Case ConstantsStates.ComputeAndStore
                                    subListCurrent.Expression = Calc_NET_STNCustomerCurrency()
                                    SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                                Case ConstantsStates.RetrieveOnly
                                    subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            End Select
                        'Case "CGP3"
                        '    'subListCurrent.Expression = Calc_CGp(3)
                        '    CurrentConstname = "CGP3"
                        '    Select Case ConstantsState
                        '        Case ConstantsStates.Compute
                        '            subListCurrent.Expression = Calc_CGp(3)
                        '        Case ConstantsStates.ComputeAndStore
                        '            subListCurrent.Expression = Calc_CGp(3)
                        '            SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                        '        Case ConstantsStates.RetrieveOnly
                        '            subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                        '    End Select

                        Case "WCFACT"
                            'subListCurrent.Expression = CALC_WCFACT()

                            CurrentConstname = "WCFACT"
                            Select Case ConstantsState
                                Case ConstantsStates.Compute
                                    subListCurrent.Expression = CALC_WCFACT()
                                Case ConstantsStates.ComputeAndStore
                                    subListCurrent.Expression = CALC_WCFACT()
                                    SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                                Case ConstantsStates.RetrieveOnly
                                    'subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                                    Try
                                        subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                                    Catch ex As Exception
                                        If ex.Message.ToUpper = "CONSTANT NOT STORED" Then
                                            subListCurrent.Expression = CALC_WCFACT()
                                        Else
                                            Throw
                                        End If
                                    End Try
                            End Select

                            'Case "RATEFACT"
                            '    CurrentConstname = "RATEFACT"
                            '    Select Case ConstantsState
                            '        Case ConstantsStates.Compute
                            '            subListCurrent.Expression = Calc_RATEFACT()
                            '        Case ConstantsStates.ComputeAndStore
                            '            subListCurrent.Expression = Calc_RATEFACT()
                            '            SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                            '        Case ConstantsStates.RetrieveOnly
                            '            subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            '    End Select
                            '=================================================================================================
                            'Quantity

                        Case "QTY"
                            subListCurrent.Expression = Calc_Qty()

                        Case "FQTY"
                            '--This factor depends on the quantities so we have to calculate it all time
                            CurrentConstname = "FQTY"
                            subListCurrent.Expression = Calc_GetFactorQuantity()
                            SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                            ChangeConstantRowValue(CurrentConstname, subListCurrent.Expression)

                            '----04.12.11----
                            'CurrentConstname = "FQTY"
                            'Select Case ConstantsState
                            '    Case ConstantsStates.Compute
                            '        subListCurrent.Expression = Calc_GetFactorQuantity()
                            '    Case ConstantsStates.ComputeAndStore
                            '        subListCurrent.Expression = Calc_GetFactorQuantity()
                            '        SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                            '    Case ConstantsStates.RetrieveOnly
                            '        Try
                            '            subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            '        Catch ex As Exception
                            '            If ex.Message.ToUpper = "CONSTANT NOT STORED" Then
                            '                subListCurrent.Expression = Calc_GetFactorQuantity()
                            '            Else
                            '                Throw
                            '            End If
                            '        End Try
                            'End Select

                        Case "SPCFACT"
                            subListCurrent.Expression = Calc_GetSpecialFactor()
                            'CurrentConstname = "SPCFACT"
                            'Select Case ConstantsState
                            '    Case ConstantsStates.Compute
                            '        subListCurrent.Expression = Calc_GetSpecialFactor()
                            '    Case ConstantsStates.ComputeAndStore
                            '        subListCurrent.Expression = Calc_GetSpecialFactor()
                            '        SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                            '    Case ConstantsStates.RetrieveOnly
                            '        Try
                            '            subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            '        Catch ex As Exception
                            '            If ex.Message.ToUpper = "CONSTANT NOT STORED" Then
                            '                subListCurrent.Expression = Calc_GetSpecialFactor()
                            '            Else
                            '                Throw
                            '            End If
                            '        End Try
                            'End Select







                        Case "LOWQTYFACT"
                            '--This factor depends on the quantities so we have to calculate it all time
                            CurrentConstname = "LOWQTYFACT"
                            subListCurrent.Expression = Calc_GetLowQuantityFactor()
                            SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                            ChangeConstantRowValue(CurrentConstname, subListCurrent.Expression)

                            '----04.12.11----
                            'CurrentConstname = "LOWQTYFACT"
                            'Select Case ConstantsState
                            '    Case ConstantsStates.Compute
                            '        subListCurrent.Expression = Calc_GetLowQuantityFactor()
                            '    Case ConstantsStates.ComputeAndStore
                            '        subListCurrent.Expression = Calc_GetLowQuantityFactor()
                            '        SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                            '    Case ConstantsStates.RetrieveOnly
                            '        Try
                            '            subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            '        Catch ex As Exception
                            '            If ex.Message.ToUpper = "CONSTANT NOT STORED" Then
                            '                subListCurrent.Expression = Calc_GetLowQuantityFactor()
                            '            Else
                            '                Throw
                            '            End If
                            '        End Try
                            'End Select

                        Case "GUNDIV"
                            CurrentConstname = "GUNDIV"
                            Select Case ConstantsState
                                Case ConstantsStates.Compute
                                    subListCurrent.Expression = Calc_GUNDIV()
                                Case ConstantsStates.ComputeAndStore
                                    subListCurrent.Expression = Calc_GUNDIV()
                                    SelectAndCreateConstantRow(CurrentConstname, subListCurrent.Expression)
                                Case ConstantsStates.RetrieveOnly
                                    subListCurrent.Expression = SelectConstantRow(CurrentConstname)
                            End Select

                            '=================================================================================================
                            'trigo
                        Case "COS"
                            subListCurrent.Expression = Calc_Trigo_Cos(subListCurrent.SubList)
                            subListCurrent.IllegalCalculation = subListCurrent.SubList.IllegalCalculation
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "SIN"
                            subListCurrent.Expression = Calc_Trigo_Sin(subListCurrent.SubList)
                            subListCurrent.IllegalCalculation = subListCurrent.SubList.IllegalCalculation
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "ACOS"
                            subListCurrent.Expression = Calc_Trigo_ACos(subListCurrent.SubList)
                            subListCurrent.IllegalCalculation = subListCurrent.SubList.IllegalCalculation
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "ASIN"
                            subListCurrent.Expression = Calc_Trigo_Asin(subListCurrent.SubList)
                            subListCurrent.IllegalCalculation = subListCurrent.SubList.IllegalCalculation
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "TAN"
                            subListCurrent.Expression = Calc_Trigo_Tan(subListCurrent.SubList)
                            subListCurrent.IllegalCalculation = subListCurrent.SubList.IllegalCalculation
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "ATN"
                            subListCurrent.Expression = Calc_Trigo_Atn(subListCurrent.SubList)
                            subListCurrent.IllegalCalculation = subListCurrent.SubList.IllegalCalculation
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing

                            '=================================================================================================
                            'Math
                        Case "FIX10"
                            subListCurrent.Expression = Calc_Math_FIX0(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "FIX100"
                            subListCurrent.Expression = Calc_Math_FIX00(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "FIX1000"
                            subListCurrent.Expression = Calc_Math_FIX1000(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing

                        Case "ABS00"
                            subListCurrent.Expression = Calc_Math_ABS00(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing

                        Case "RECHW1000"
                            subListCurrent.Expression = Calc_RECHW1000(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing



                        Case "ABSDSC00"
                            subListCurrent.Expression = Calc_Math_ABS00DSC(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "ABSDSC000"
                            subListCurrent.Expression = Calc_Math_ABS000DSC(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "ABSDESC1000"
                            subListCurrent.Expression = Calc_Math_ABS1000DSC(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "ABSDSC"
                            subListCurrent.Expression = Calc_Math_AbsDSC(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing

                        Case "ABS"
                            subListCurrent.Expression = Calc_Math_Abs(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "EXP"
                            subListCurrent.Expression = Calc_Math_Exp(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "LOG"
                            subListCurrent.Expression = Calc_Math_Log(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "FIX"
                            subListCurrent.Expression = Calc_Math_Fix(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "SQR"
                            subListCurrent.Expression = Calc_Math_Sqr(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "INT"
                            subListCurrent.Expression = Calc_Math_Int(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing

                        Case "ROUNDUP"
                            subListCurrent.Expression = Calc_Math_RoundUP(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing

                        Case "ROUNDDOWN"
                            subListCurrent.Expression = Calc_Math_RoundDown(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing

                        Case "ROUND"
                            subListCurrent.Expression = Calc_Math_Round(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "ROUND0"
                            subListCurrent.Expression = Calc_Math_Round0(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "ROUND00"
                            subListCurrent.Expression = Calc_Math_Round00(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "ROUND000"
                            subListCurrent.Expression = Calc_Math_Round000(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing

                        Case "ROUNDDSC0"
                            subListCurrent.Expression = Calc_Math_Round0DSC(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "ROUNDDSC00"
                            subListCurrent.Expression = Calc_Math_Round00DSC(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing
                        Case "ROUNDDSC000"
                            subListCurrent.Expression = Calc_Math_Round000DSC(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing

                        Case "SGN"
                            subListCurrent.Expression = Calc_Math_Sgn(subListCurrent.SubList)
                            subListCurrent.SubList.SubParent = Nothing
                            subListCurrent.SubList = Nothing


                    End Select
                    lastCurrentItem = lastCurrentItem.NextItem
                Else
                    If subListCurrent.Expression = "(" Then
                        If Not subListCurrent.SubList Is Nothing Then
                            subListCurrent.Expression = subListCurrent.SubList.Expression
                            subListCurrent.SubList = Nothing
                            lastCurrentItem = lastCurrentItem.NextItem
                        Else
                            subListCurrent.SubParent.SubList = subListCurrent.NextItem
                            subListCurrent.NextItem.SubParent = subListCurrent.SubParent
                            subListCurrent.NextItem.PrevItem = Nothing
                            'subListCurrent.NextItem.NextItem = subListCurrent.NextItem.NextItem
                            lastCurrentItem = subListCurrent.NextItem
                            itm = subListCurrent.NextItem

                            subListCurrent = Nothing
                        End If
                    ElseIf subListCurrent.Expression = ")" Then
                        If subListCurrent.NextItem Is Nothing Then
                            subListCurrent.PrevItem.NextItem = Nothing
                            lastCurrentItem = Nothing
                        Else
                            subListCurrent.PrevItem.NextItem = subListCurrent.NextItem
                            subListCurrent.NextItem.PrevItem = subListCurrent.PrevItem
                            lastCurrentItem = subListCurrent.NextItem
                            subListCurrent = Nothing
                        End If
                    Else
                        lastCurrentItem = lastCurrentItem.NextItem
                    End If
                End If

                'lastCurrentItem = lastCurrentItem.NextItem
                subListCurrent = lastCurrentItem
            End While


            '2.
            subListCurrent = lastCurrentItem

            subListCurrent = itm
            For Each op As String In Operators
                lastCurrentItem = subListCurrent
                While Not subListCurrent Is Nothing
                    If subListCurrent.Expression = op Then
                        Dim leftOp As Item
                        Dim rightOp As Item
                        rightOp = Nothing


                        If Not subListCurrent.PrevItem Is Nothing AndAlso subListCurrent.PrevItem.Expression <> "(" AndAlso subListCurrent.PrevItem.Expression <> "," Then
                            leftOp = subListCurrent.PrevItem

                        Else
                            '??? only (-x)
                            leftOp = Nothing
                        End If
                        If Not subListCurrent.SubList Is Nothing Then
                            rightOp = subListCurrent.SubList
                        Else
                            If Not subListCurrent.NextItem Is Nothing Then
                                rightOp = subListCurrent.NextItem
                            Else
                                '???? ein kaze davar!
                            End If
                        End If

                        If leftOp Is Nothing And op = "-" Then
                            'TODO:  (-x)
                            'Dim virtualItem as new Item("0",subListCurrent.PrevItem)
                            CalculateItem(subListCurrent, leftOp, rightOp)
                        Else
                            CalculateItem(subListCurrent, leftOp, rightOp)
                        End If

                    End If
                    subListCurrent = subListCurrent.NextItem
                End While
                subListCurrent = lastCurrentItem
            Next

        Catch ex As Exception

            'GeneralException.WriteEventErrors("Compute-" & ex.Message & "- subListCurrent.Expression : " & sSsubListCurrent, GeneralException.e_LogTitle.PARSER.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "Compute", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        End Try
    End Sub

    Private Sub CalculateItem(ByVal itm As Item, ByVal lft As Item, ByVal rght As Item)
        Dim isSpecialMinus As Boolean = False
        Dim isOpLeft As Boolean = False
        Dim originalLeft As Item = lft
        Dim isSpecialThreadString As Boolean = False
        Try

            'If CurrentParameterIndex = 227 Then
            '    Dim ff As Integer = CurrentParameterIndex
            'End If

            If Not lft Is Nothing AndAlso IsOperator(lft.Expression) Then
                isOpLeft = True
                originalLeft = lft
                lft = lft.SubList
            End If

            Dim iPrevExp As String = ""
            Dim iNextExp As String = ""
            If Not itm.PrevItem Is Nothing Then
                iPrevExp = itm.PrevItem.Expression.ToString()
            End If
            If Not itm.NextItem Is Nothing Then
                iNextExp = itm.NextItem.Expression.ToString()
            End If

            If (iPrevExp = "`" AndAlso iNextExp = "`") Or (iPrevExp = "`" AndAlso iNextExp = "") Or (iPrevExp = "" AndAlso iNextExp = "`") Then
                itm.Expression = itm.Expression
                itm.IllegalCalculation = False
                isSpecialThreadString = True
            Else
                Select Case UCase(itm.Expression)
                    'Operators
                    Case "^"
                        itm.Expression = Format(CDec(lft.Expression) ^ CDec(rght.Expression), "0.##########")
                        'If lft.IllegalCalculation Or rght.IllegalCalculation Then
                        '    itm.IllegalCalculation = True
                        'End If
                        itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
                    Case "*"
                        itm.Expression = Format(CDec(lft.Expression) * CDec(rght.Expression), "0.##########")
                        itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
                    Case "\"
                        itm.Expression = Format(CDec(lft.Expression) / CDec(rght.Expression), "0.##########")
                        itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
                    Case "/"
                        itm.Expression = Format(CDec(lft.Expression) / CDec(rght.Expression), "0.##########")
                        itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
                    Case "+"
                        itm.Expression = Format(CDec(lft.Expression) + CDec(rght.Expression), "0.##########")
                        itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
                    Case "-"
                        If lft Is Nothing Then
                            itm.Expression = CDec(rght.Expression) * (-1)
                            itm.IllegalCalculation = rght.IllegalCalculation
                            isSpecialMinus = True
                        Else
                            'TODO check empty string if change to 0
                            '18.01.10 micki bilal
                            If lft.Expression.ToString <> "" AndAlso rght.Expression.ToString <> "" Then
                                itm.Expression = Format(CDec(lft.Expression) - CDec(rght.Expression), "0.##########")
                                itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
                            Else
                                itm.Expression = 0
                                'TODO: check IllegalCalculation condition
                                'itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
                            End If
                        End If
                    Case ">"
                        If IsNumeric(lft.Expression) AndAlso IsNumeric(rght.Expression) Then
                            If CDbl(lft.Expression) > CDbl(rght.Expression) Then
                                itm.Expression = "True"
                            Else
                                itm.Expression = "False"
                            End If
                            itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
                        Else
                            itm.Expression = "False"
                            'TODO: check IllegalCalculation condition
                            'itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
                        End If
                    Case "<"

                        If IsNumeric(lft.Expression) AndAlso IsNumeric(rght.Expression) Then
                            If CDbl(lft.Expression) < CDbl(rght.Expression) Then
                                itm.Expression = "True"
                            Else
                                itm.Expression = "False"
                            End If
                            itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
                        Else
                            itm.Expression = "False"
                            'TODO: check IllegalCalculation condition
                            'itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
                        End If
                    Case "="
                        Dim isn As Boolean = False
                        Try
                            If IsNumeric(lft.Expression) AndAlso IsNumeric(rght.Expression) Then
                                If CDbl(lft.Expression) = CDbl(rght.Expression) Then
                                    isn = True
                                End If
                            End If
                        Catch ex As Exception

                        End Try



                        If isn = True Or UCase(lft.Expression) = UCase(rght.Expression) Or (lft.Expression = "" And rght.Expression = """""") Or (lft.Expression = """""" And rght.Expression = "") Then
                            itm.Expression = "True"
                        Else
                            If lft.Expression.Length >= 2 AndAlso Left(lft.Expression, 1) = """" AndAlso Right(lft.Expression, 1) = """" AndAlso
                                        rght.Expression.Length >= 1 AndAlso Left(rght.Expression, 1) <> """" AndAlso Right(rght.Expression, 1) <> """" AndAlso
                                        UCase(Right(Left(lft.Expression, lft.Expression.Length - 1), lft.Expression.Length - 2)) = UCase(rght.Expression) Then
                                itm.Expression = "True"
                            Else
                                If rght.Expression.Length >= 2 AndAlso Left(rght.Expression, 1) = """" AndAlso Right(rght.Expression, 1) = """" AndAlso
                                            lft.Expression.Length >= 1 AndAlso Left(lft.Expression, 1) <> """" AndAlso Right(lft.Expression, 1) <> """" AndAlso
                                            UCase(Right(Left(rght.Expression, rght.Expression.Length - 1), rght.Expression.Length - 2)) = UCase(lft.Expression) Then
                                    itm.Expression = "True"
                                Else
                                    itm.Expression = "False"
                                End If
                            End If
                            'itm.Expression = "False"
                        End If
                        itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation

                        'Complex Operators

                    Case "$"
                        If IsNumeric(lft.Expression) AndAlso IsNumeric(rght.Expression) Then
                            If CDbl(lft.Expression) >= CDbl(rght.Expression) Then
                                itm.Expression = "True"
                            Else
                                itm.Expression = "False"
                            End If
                            itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
                        Else
                            itm.Expression = "False"
                            'TODO: check IllegalCalculation condition
                            'itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
                        End If
                    Case "@"
                        If IsNumeric(lft.Expression) AndAlso IsNumeric(rght.Expression) Then
                            If CDbl(lft.Expression) <= CDbl(rght.Expression) Then
                                itm.Expression = "True"
                            Else
                                itm.Expression = "False"
                            End If
                            itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
                        Else
                            itm.Expression = "False"
                            'TODO: check IllegalCalculation condition
                            'itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
                        End If
                    Case "~"    '<>
                        Dim isn As Boolean = True
                        Try
                            If IsNumeric(lft.Expression) AndAlso IsNumeric(rght.Expression) Then
                                If CDbl(lft.Expression) <> CDbl(rght.Expression) Then
                                    isn = False
                                End If
                            End If
                        Catch ex As Exception

                        End Try

                        If isn = False Then
                            itm.Expression = "True"
                        Else
                            If UCase(lft.Expression) = UCase(rght.Expression) Then
                                itm.Expression = "False"
                            Else
                                If lft.Expression.Length >= 2 AndAlso Left(lft.Expression, 1) = """" AndAlso Right(lft.Expression, 1) = """" AndAlso
                                            rght.Expression.Length >= 1 AndAlso Left(rght.Expression, 1) <> """" AndAlso Right(rght.Expression, 1) <> """" AndAlso
                                            UCase(Right(Left(lft.Expression, lft.Expression.Length - 1), lft.Expression.Length - 2)) = UCase(rght.Expression) Then
                                    itm.Expression = "False"
                                Else
                                    If rght.Expression.Length >= 2 AndAlso Left(rght.Expression, 1) = """" AndAlso Right(rght.Expression, 1) = """" AndAlso
                                                lft.Expression.Length >= 1 AndAlso Left(lft.Expression, 1) <> """" AndAlso Right(lft.Expression, 1) <> """" AndAlso
                                                UCase(Right(Left(rght.Expression, rght.Expression.Length - 1), rght.Expression.Length - 2)) = UCase(lft.Expression) Then
                                        itm.Expression = "False"
                                    Else
                                        itm.Expression = "True"
                                    End If
                                End If
                                'itm.Expression = "False"
                            End If
                        End If




                        itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
                    Case "`"
                        Dim lExpr As String = lft.Expression
                        Dim rExpr As String = rght.Expression

                        If Len(lExpr) > 2 Then
                            If Left(lExpr, 1) = """" And Right(lExpr, 1) = """" Then
                                lExpr = lExpr.Substring(1, Len(lExpr) - 2)
                            End If
                        End If
                        If Len(rExpr) > 2 Then
                            If Left(rExpr, 1) = """" And Right(rExpr, 1) = """" Then
                                rExpr = rExpr.Substring(1, Len(rExpr) - 2)
                            End If
                        End If

                        itm.Expression = lExpr & rExpr
                        itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation

                    Case "?"
                        itm.Expression = CBool(lft.Expression) And CBool(rght.Expression)
                        itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
                    Case Chr(174)
                        itm.Expression = CBool(lft.Expression) Xor CBool(rght.Expression)
                        itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
                    Case "|"
                        itm.Expression = CBool(lft.Expression) Or CBool(rght.Expression)
                        itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
                    Case Chr(169)
                        itm.Expression = lft.Expression Mod rght.Expression
                        itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation

                End Select
            End If
        Catch ex As Exception
            itm.Expression = ""
            itm.IllegalCalculation = True
            Dim s As String = "IllegalCalculation: Item: " & itm.Expression & "; Left:"
            If Not lft Is Nothing Then
                s &= lft.Expression
            Else
                s &= "[null]"
            End If
            s &= "; Right:"
            If Not rght Is Nothing Then
                s &= rght.Expression
            Else
                s &= "[null]"
            End If
            'TODO check if need
            Throw New Exception(s)
            '----
        End Try

        'delete right
        If Not isSpecialThreadString Then
            If Not itm.SubList Is Nothing Then
                itm.SubList = Nothing
            Else
                If rght.NextItem Is Nothing Then
                    itm.NextItem = Nothing
                    rght = Nothing
                Else
                    itm.NextItem = rght.NextItem
                    itm.NextItem.PrevItem = itm
                    rght = Nothing
                End If
            End If
        End If
        'delete left

        If isSpecialMinus Or isSpecialThreadString Then
            'na action.
            'Dim s As String = ""
        Else
            If isOpLeft Then
                originalLeft.SubList.SubParent = Nothing
                originalLeft.SubList = Nothing
            Else
                If lft.PrevItem Is Nothing Then
                    lft.SubParent.SubList = itm
                    itm.SubParent = lft.SubParent
                    itm.PrevItem = Nothing
                    lft = Nothing
                Else
                    lft.PrevItem.NextItem = itm
                    itm.PrevItem = lft.PrevItem
                    lft = Nothing
                End If
            End If
        End If





        'Dim isSpecialMinus As Boolean = False
        'Dim isOpLeft As Boolean = False
        'Dim originalLeft As Item = lft

        'Try

        '    'If CurrentParameterIndex = 227 Then
        '    '    Dim ff As Integer = CurrentParameterIndex
        '    'End If

        '    If Not lft Is Nothing AndAlso IsOperator(lft.Expression) Then
        '        isOpLeft = True
        '        originalLeft = lft
        '        lft = lft.SubList
        '    End If

        '    Select Case UCase(itm.Expression)
        '        'Operators
        '        Case "^"
        '            itm.Expression = Format(CDec(lft.Expression) ^ CDec(rght.Expression), "0.##########")
        '            'If lft.IllegalCalculation Or rght.IllegalCalculation Then
        '            '    itm.IllegalCalculation = True
        '            'End If
        '            itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
        '        Case "*"
        '            itm.Expression = Format(CDec(lft.Expression) * CDec(rght.Expression), "0.##########")
        '            itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
        '        Case "\"
        '            itm.Expression = Format(CDec(lft.Expression) / CDec(rght.Expression), "0.##########")
        '            itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
        '        Case "/"
        '            itm.Expression = Format(CDec(lft.Expression) / CDec(rght.Expression), "0.##########")
        '            itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
        '        Case "+"
        '            itm.Expression = Format(CDec(lft.Expression) + CDec(rght.Expression), "0.##########")
        '            itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
        '        Case "-"
        '            If lft Is Nothing Then
        '                itm.Expression = CDec(rght.Expression) * (-1)
        '                itm.IllegalCalculation = rght.IllegalCalculation
        '                isSpecialMinus = True
        '            Else
        '                'TODO check empty string if change to 0
        '                '18.01.10 micki bilal
        '                If lft.Expression.ToString <> "" AndAlso rght.Expression.ToString <> "" Then
        '                    itm.Expression = Format(CDec(lft.Expression) - CDec(rght.Expression), "0.##########")
        '                    itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
        '                Else
        '                    itm.Expression = 0
        '                    'TODO: check IllegalCalculation condition
        '                    'itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
        '                End If
        '            End If
        '        Case ">"
        '            If IsNumeric(lft.Expression) AndAlso IsNumeric(rght.Expression) Then
        '                If CDbl(lft.Expression) > CDbl(rght.Expression) Then
        '                    itm.Expression = "True"
        '                Else
        '                    itm.Expression = "False"
        '                End If
        '                itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
        '            Else
        '                itm.Expression = "False"
        '                'TODO: check IllegalCalculation condition
        '                'itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
        '            End If
        '        Case "<"
        '            If IsNumeric(lft.Expression) AndAlso IsNumeric(rght.Expression) Then
        '                If CDbl(lft.Expression) < CDbl(rght.Expression) Then
        '                    itm.Expression = "True"
        '                Else
        '                    itm.Expression = "False"
        '                End If
        '                itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
        '            Else
        '                itm.Expression = "False"
        '                'TODO: check IllegalCalculation condition
        '                'itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
        '            End If
        '        Case "="
        '            If UCase(lft.Expression) = UCase(rght.Expression) Or (lft.Expression = "" And rght.Expression = """""") Or (lft.Expression = """""" And rght.Expression = "") Then
        '                itm.Expression = "True"
        '            Else
        '                If lft.Expression.Length >= 2 AndAlso Left(lft.Expression, 1) = """" AndAlso Right(lft.Expression, 1) = """" AndAlso _
        '                            rght.Expression.Length >= 1 AndAlso Left(rght.Expression, 1) <> """" AndAlso Right(rght.Expression, 1) <> """" AndAlso _
        '                            UCase(Right(Left(lft.Expression, lft.Expression.Length - 1), lft.Expression.Length - 2)) = UCase(rght.Expression) Then
        '                    itm.Expression = "True"
        '                Else
        '                    If rght.Expression.Length >= 2 AndAlso Left(rght.Expression, 1) = """" AndAlso Right(rght.Expression, 1) = """" AndAlso _
        '                                lft.Expression.Length >= 1 AndAlso Left(lft.Expression, 1) <> """" AndAlso Right(lft.Expression, 1) <> """" AndAlso _
        '                                UCase(Right(Left(rght.Expression, rght.Expression.Length - 1), rght.Expression.Length - 2)) = UCase(lft.Expression) Then
        '                        itm.Expression = "True"
        '                    Else
        '                        itm.Expression = "False"
        '                    End If
        '                End If
        '                'itm.Expression = "False"
        '            End If
        '            itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation

        '            'Complex Operators

        '        Case "$"
        '            If IsNumeric(lft.Expression) AndAlso IsNumeric(rght.Expression) Then
        '                If CDbl(lft.Expression) >= CDbl(rght.Expression) Then
        '                    itm.Expression = "True"
        '                Else
        '                    itm.Expression = "False"
        '                End If
        '                itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
        '            Else
        '                itm.Expression = "False"
        '                'TODO: check IllegalCalculation condition
        '                'itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
        '            End If
        '        Case "@"
        '            If IsNumeric(lft.Expression) AndAlso IsNumeric(rght.Expression) Then
        '                If CDbl(lft.Expression) <= CDbl(rght.Expression) Then
        '                    itm.Expression = "True"
        '                Else
        '                    itm.Expression = "False"
        '                End If
        '                itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
        '            Else
        '                itm.Expression = "False"
        '                'TODO: check IllegalCalculation condition
        '                'itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
        '            End If
        '        Case "~"    '<>
        '            If UCase(lft.Expression) = UCase(rght.Expression) Then
        '                itm.Expression = "False"
        '            Else
        '                If lft.Expression.Length >= 2 AndAlso Left(lft.Expression, 1) = """" AndAlso Right(lft.Expression, 1) = """" AndAlso _
        '                            rght.Expression.Length >= 1 AndAlso Left(rght.Expression, 1) <> """" AndAlso Right(rght.Expression, 1) <> """" AndAlso _
        '                            UCase(Right(Left(lft.Expression, lft.Expression.Length - 1), lft.Expression.Length - 2)) = UCase(rght.Expression) Then
        '                    itm.Expression = "False"
        '                Else
        '                    If rght.Expression.Length >= 2 AndAlso Left(rght.Expression, 1) = """" AndAlso Right(rght.Expression, 1) = """" AndAlso _
        '                                lft.Expression.Length >= 1 AndAlso Left(lft.Expression, 1) <> """" AndAlso Right(lft.Expression, 1) <> """" AndAlso _
        '                                UCase(Right(Left(rght.Expression, rght.Expression.Length - 1), rght.Expression.Length - 2)) = UCase(lft.Expression) Then
        '                        itm.Expression = "False"
        '                    Else
        '                        itm.Expression = "True"
        '                    End If
        '                End If
        '                'itm.Expression = "False"
        '            End If
        '            itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
        '        Case "`"
        '            Dim lExpr As String = lft.Expression
        '            Dim rExpr As String = rght.Expression

        '            If Len(lExpr) > 2 Then
        '                If Left(lExpr, 1) = """" And Right(lExpr, 1) = """" Then
        '                    lExpr = lExpr.Substring(1, Len(lExpr) - 2)
        '                End If
        '            End If
        '            If Len(rExpr) > 2 Then
        '                If Left(rExpr, 1) = """" And Right(rExpr, 1) = """" Then
        '                    rExpr = rExpr.Substring(1, Len(rExpr) - 2)
        '                End If
        '            End If

        '            itm.Expression = lExpr & rExpr
        '            itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation

        '        Case "?"
        '            itm.Expression = CBool(lft.Expression) And CBool(rght.Expression)
        '            itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
        '        Case Chr(233)
        '            itm.Expression = CBool(lft.Expression) Xor CBool(rght.Expression)
        '            itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
        '        Case "|"
        '            itm.Expression = CBool(lft.Expression) Or CBool(rght.Expression)
        '            itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation
        '        Case Chr(234)
        '            itm.Expression = lft.Expression Mod rght.Expression
        '            itm.IllegalCalculation = lft.IllegalCalculation Or rght.IllegalCalculation

        '    End Select
        'Catch ex As Exception
        '    itm.Expression = ""
        '    itm.IllegalCalculation = True
        '    Dim s As String = "IllegalCalculation: Item: " & itm.Expression & "; Left:"
        '    If Not lft Is Nothing Then
        '        s &= lft.Expression
        '    Else
        '        s &= "[null]"
        '    End If
        '    s &= "; Right:"
        '    If Not rght Is Nothing Then
        '        s &= rght.Expression
        '    Else
        '        s &= "[null]"
        '    End If
        '    'TODO check if need
        '    'Throw New Exception(s)
        '    '----
        'End Try

        ''delete right
        'If Not itm.SubList Is Nothing Then
        '    itm.SubList = Nothing
        'Else
        '    If rght.NextItem Is Nothing Then
        '        itm.NextItem = Nothing
        '        rght = Nothing
        '    Else
        '        itm.NextItem = rght.NextItem
        '        itm.NextItem.PrevItem = itm
        '        rght = Nothing
        '    End If
        'End If

        ''delete left

        'If isSpecialMinus Then
        '    'na action.
        '    'Dim s As String = ""
        'Else
        '    If isOpLeft Then
        '        originalLeft.SubList.SubParent = Nothing
        '        originalLeft.SubList = Nothing
        '    Else
        '        If lft.PrevItem Is Nothing Then
        '            lft.SubParent.SubList = itm
        '            itm.SubParent = lft.SubParent
        '            itm.PrevItem = Nothing
        '            lft = Nothing
        '        Else
        '            lft.PrevItem.NextItem = itm
        '            itm.PrevItem = lft.PrevItem
        '            lft = Nothing
        '        End If
        '    End If
        'End If

    End Sub
    Private Function PrapareParameters(ByVal itm As Item, ByVal RequiredCount As Integer) As List(Of Item)
        Dim l As New List(Of Item)
        Dim curr As Item = itm
        Dim CommaCount As Integer = 0
        l.Add(itm)
        While Not curr Is Nothing And CommaCount < RequiredCount - 1
            If curr.Expression = "," Then
                If curr.SubList Is Nothing Then
                    l.Add(curr.NextItem)
                Else
                    l.Add(curr.SubList)
                End If
                CommaCount += 1
            End If
            curr = curr.NextItem
        End While

        Return l
    End Function
    Private Function PrapareParameters(ByVal itm As Item) As List(Of Item)
        Dim l As New List(Of Item)
        Dim curr As Item = itm
        Dim CommaCount As Integer = 0
        l.Add(itm)
        While Not curr Is Nothing
            If curr.Expression = "," Then
                If curr.SubList Is Nothing Then
                    l.Add(curr.NextItem)
                Else
                    l.Add(curr.SubList)
                End If
                CommaCount += 1
            End If
            curr = curr.NextItem
        End While

        Return l
    End Function

    'Private Sub PreBuildStructure(ByVal itm As Item)
    '    'If itm.SubList Is Nothing Then
    '    'Exit Sub
    '    'End If
    '    ' בדיקה האם זהו ליסט סופי ואין לו תתי ליסטים
    '    'Dim subList As Item = itm.SubList
    '    Dim subListCurrent As Item = itm
    '    'Dim isLast As Boolean = True
    '    While Not subListCurrent Is Nothing
    '        If Not subListCurrent.SubList Is Nothing Then
    '            PreBuildStructure(subListCurrent.SubList)
    '        End If
    '        subListCurrent = subListCurrent.NextItem
    '    End While

    '    ' חישוב של ליסט נוכחי
    '    ' חישוב אריתמטי בלבד
    '    Dim lastCurrentItem As Item
    '    'If itm.SubList Is Nothing Then
    '    subListCurrent = itm
    '    'Else
    '    'subListCurrent = itm.SubList
    '    'End If



    '    For Each op As String In Operators
    '        lastCurrentItem = subListCurrent
    '        While Not subListCurrent Is Nothing
    '            If subListCurrent.Expression = op Then
    '                PreBuildItem(subListCurrent)
    '            End If
    '            subListCurrent = subListCurrent.NextItem
    '        End While
    '        subListCurrent = lastCurrentItem
    '    Next

    'End Sub

    'Private Sub PreBuildItem(ByVal itm As Item)
    '    Dim leftOperand As Item
    '    Dim rigthOperand As Item

    '    If Not itm.PrevItem Is Nothing Then
    '        leftOperand = itm.PrevItem
    '        If itm.SubList Is Nothing Then
    '            itm.SubList = leftOperand
    '            leftOperand.SubParent = itm
    '        Else
    '            leftOperand.NextItem = itm.SubList
    '            itm.SubList.PrevItem = leftOperand
    '            itm.SubList = leftOperand
    '            leftOperand.SubParent = itm
    '        End If

    '        If leftOperand.PrevItem Is Nothing Then
    '            leftOperand.SubParent.SubList = itm
    '            itm.SubParent = leftOperand.SubParent
    '            itm.PrevItem = Nothing
    '        Else
    '            leftOperand.PrevItem.NextItem = itm
    '            itm.PrevItem = leftOperand.PrevItem
    '        End If


    '        leftOperand.PrevItem = Nothing
    '        leftOperand.NextItem = Nothing

    '    Else
    '        '??????
    '        Throw New Exception("left")
    '    End If
    '    If Not itm.NextItem Is Nothing Then
    '        rigthOperand = itm.NextItem
    '        itm.NextItem = rigthOperand.NextItem
    '        If Not itm.NextItem Is Nothing Then
    '            itm.NextItem.PrevItem = itm
    '        End If

    '        'rigthOperand = itm.NextItem
    '        leftOperand.NextItem = rigthOperand
    '        rigthOperand.PrevItem = leftOperand
    '        rigthOperand.NextItem = Nothing
    '    Else
    '        If Not itm.SubList Is Nothing Then
    '            rigthOperand = itm.SubList
    '            leftOperand.PrevItem.NextItem = itm
    '            itm.PrevItem = leftOperand.PrevItem

    '            itm.SubList = leftOperand
    '            leftOperand.SubParent = itm
    '            leftOperand.NextItem = rigthOperand
    '            leftOperand.PrevItem = Nothing
    '            rigthOperand.PrevItem = leftOperand

    '        Else
    '            Throw New Exception("right")
    '        End If
    '    End If




    'End Sub

    'Private Function Calc_Operator_Mult(ByVal Operand1 As Item, ByVal Operand2 As Item) As String
    '    Return CalculateNode(Operand1) * CalculateNode(Operand2)
    'End Function



    'Private Function GetNode(ByVal item As Item, ByVal CurrentNode As Node) As Node
    '    Dim cur As Item = item
    '    Dim leftItem As Item
    '    Dim rightItem As Item
    '    Dim leftNode As Node
    '    Dim rightNode As Node
    '    Dim curOperatorNode As Node
    '    For Each opr As String In Operators
    '        While Not cur.NextItem Is Nothing
    '            If cur.Expression = opr Then
    '                curOperatorNode = New Node(cur.Expression, CurrentNode)
    '                If Not cur.PrevItem.SubList Is Nothing Then
    '                    leftNode = GetNode(cur.PrevItem.SubList, curOperatorNode)
    '                Else
    '                    leftNode = New Node(cur.PrevItem.Expression, curOperatorNode)
    '                End If
    '                'TODO: right
    '            End If
    '        End While
    '    Next


    'End Function

    'Public Function Calculatate() As String
    '    Dim CurrentNode As Node = Tree
    '    Dim Result As String

    '    Result = CalculateNode(CurrentNode)

    '    'txtResult.Text = Result
    '    Return Result


    'End Function

    'Private Function CalculateNode(ByVal CurrentNode As Node) As String

    '    Return ""
    'End Function
    Private Function SelectAndCreateConstantRow(ByVal ConstName As String, ByVal ConstValue As String) As DataRow
        Dim dr As DataRow
        If Constants Is Nothing Then
            Constants = New DataTable
            Constants.Columns.Add(New DataColumn("ConstName"))
            Constants.Columns.Add(New DataColumn("ConstValue"))
        End If
        Dim drN() As DataRow = Constants.Select("ConstName='" & ConstName & "'")
        If drN.Length = 0 Then
            dr = Constants.NewRow
            dr("ConstName") = ConstName
            dr("ConstValue") = ConstValue
            Constants.Rows.Add(dr)
        Else
            dr = drN(0)
        End If
        Return dr
    End Function

    Private Sub ChangeConstantRowValue(ByVal ConstName As String, ByVal ConstValue As String) 'As DataRow
        '----04.12.11----
        Dim dt As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Consts, "")
        If Not IsNothing(dt) Then
            For Each row As DataRow In dt.Rows
                If row("ConstName") = ConstName Then
                    row("ConstValue") = ConstValue
                End If
            Next
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Consts, dt)
        End If
    End Sub

    Private Function SelectConstantRow(ByVal ConstName As String) As String

        If Constants Is Nothing Then
            Throw New Exception("Constant not stored")
        End If

        Dim drN() As DataRow = Constants.Select("ConstName='" & ConstName & "'")
        If drN.Length = 0 Then
            Throw New Exception("Constant not stored")
        Else
            Return drN(0)("ConstValue")

            'Dim sSTN As String = drN(0)("ConstValue")
            'Dim DiscountFromSTN As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.DiscountFromSTN)
            'If DiscountFromSTN <> "" AndAlso IsNumeric(DiscountFromSTN) = True AndAlso DiscountFromSTN <> "0" Then
            '    sSTN = CDbl(sSTN) * (1 - DiscountFromSTN / 100)
            'End If
            'Return sSTN
        End If

    End Function

    Public Function FormulaSimplify(ByVal sFormula As String) As String

        Dim res As String = sFormula
        For i As Integer = 0 To Parameters.Rows.Count - 1
            res = res.Replace("{" & (i + 1).ToString() & "}", Parameters.Rows(i)("Measure").ToString)
        Next

        res = Replace(res, Chr(34), "")

        'Dim dt_m As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Model, "")
        'Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString
        'Dim sVar As String
        'Do Until InStr(res, "CONST") = 0
        '    sVar = res.Substring(InStr(res, "CONST") - 1, InStr(InStr(res, "CONST"), res, ")") - InStr(res, "CONST") + 1)
        '    Dim dt As DataTable = CacheManager.GetDataTable(CacheManager.Keys.Constants, "glb_tConstant", "ModelNum=" & ModelNum, "", "*")
        '    Dim dr() As DataRow = dt.Select("ConstName='" & sVar & "'")
        '    If dr.Length > 0 AndAlso dr(0)("ConstValue").ToString <> "" Then
        '        res.Replace(sVar, dr(0)("ConstValue").ToString)
        '    End If
        'Loop

        'Calc_Const(subListCurrent.SubList)

        '            Case "GP1"
        'Calc_Gp(1)

        '            Case "GP2"
        'Calc_Gp(2)
        '                 Case "GP3"
        'subListCurrent.Expression = Calc_Gp(3)

        '            Case "STN"
        'Calc_Stn()
        '               Case "RATE"
        'Calc_Rate()

        '            Case "WCGP"
        'Calc_WCGP()

        '            Case "CGP1"
        'Calc_CGp(1)

        '            Case "CGP2"
        'CurrentConstname = "CGP2"

        '            Case "CGP3"
        'Calc_CGp(3)
        'Calc_Qty()

        Return res
    End Function

    'Public Shared Function CorrelatedWC_Prices(ByVal WorkCenterID As Integer, ByVal ITM_CAT As Integer, ByVal BRANCH_TFR_Price As String, ByVal BRANCH_MKT_Price As String, CustomerNum As String) As String

    '    'Bool[TFRNET_CURWC*1.2<MARWC*(1-LIMIT_DISC)]*MARWC*(1-LIMIT_DISC)+Bool[TFRNET_CURWC*1.2>MARWC*(1-LIMIT_DISC)-0.001]*Bool[TFRNET_CURWC*(1-0.25)>((TFRWC+TFRNET_CURWC)/2)]*TFRNET_CURWC*(1-0.25)+Bool[TFRNET_CURWC*1.2>MARWC*(1-LIMIT_DISC)-0.001]*Bool[TFRNET_CURWC*(1-0.25)<((TFRWC+TFRNET_CURWC)/2)+0.001]*((TFRWC+TFRNET_CURWC)/2)

    '    Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
    '    Dim Params As New SqlParams
    '    Dim dtWC_Prices As DataTable

    '    Dim curRate As Decimal
    '    Dim dt_m As DataTable
    '    dt_m = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Model, "")
    '    Dim BranchCur As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TFRCurrency)
    '    Dim WCCur As String = dt_m.Rows(0)("Calc_CUR").ToString
    '    Dim BranchCode As String = StateManager.GetValue(StateManager.Keys.BranchCode)
    '    If WCCur <> BranchCur Then

    '        Dim oParams As New SqlParams()
    '        oParams = New SqlParams
    '        oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
    '        oParams.Add(New SqlParam("@FromCur", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCur, 3))
    '        oParams.Add(New SqlParam("@ToCur", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, WCCur, 3))
    '        oParams.Add(New SqlParam("@RateValue", SqlParam.ParamType.ptFloat, SqlParam.ParamDirection.pdOutput, Nothing))
    '        oSql.ExecuteSP("USP_GetRates", oParams)
    '        If Not oParams.GetParameter("@RateValue").Value Is Nothing AndAlso oParams.GetParameter("@RateValue").Value.ToString <> "" Then
    '            curRate = 1 / CDbl(oParams.GetParameter("@RateValue").Value)
    '        End If

    '    Else
    '        curRate = 1
    '    End If

    '    Params.Add(New SqlParam("@branchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
    '    Params.Add(New SqlParam("@ITM_CAT", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, CInt(ITM_CAT), 4))
    '    Params.Add(New SqlParam("@WorkCenterID", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, CInt(WorkCenterID), 4))
    '    Params.Add(New SqlParam("@BRANCH_TFR_Price", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, CDec(BRANCH_TFR_Price)))
    '    Params.Add(New SqlParam("@BRANCH_MKT_Price", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, CDec(BRANCH_MKT_Price)))
    '    Params.Add(New SqlParam("@TFR_RAte", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, CDec(curRate)))
    '    Params.Add(New SqlParam("@CustomerNum", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, CustomerNum))

    '    dtWC_Prices = oSql.ExecuteSPReturnDT("USP_CorrelatedWC_Prices", Params)

    '    Dim res As String = dtWC_Prices.Rows(0).Item("STN_Col")

    '    Dim dummy_ResultSetRules As DataTable = Nothing
    '    Dim dt_p As DataTable = Nothing
    '    Dim oFormula As New FormulaResult(res, dt_p, 0, dummy_ResultSetRules)
    '    Dim value As String = oFormula.ParseAndCalculate
    '    value = oFormula.ParseAndCalculate * CDec(curRate)
    '    Return value
    'End Function

    'Public Shared Function CorrelatedWC_PricesUOP(ByVal WorkCenterID As Integer, ByVal ITM_CAT As Integer) As String


    '    Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
    '    Dim Params As New SqlParams
    '    Dim dtWC_Prices As DataTable


    '    Params.Add(New SqlParam("@ITM_CAT", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, CInt(ITM_CAT), 4))
    '    Params.Add(New SqlParam("@WorkCenterID", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, CInt(WorkCenterID), 4))

    '    dtWC_Prices = oSql.ExecuteSPReturnDT("USP_CorrelatedWC_Prices_UOP", Params)

    '    Dim res As String = dtWC_Prices.Rows(0).Item("STN_Col")

    '    Dim dummy_ResultSetRules As DataTable = Nothing
    '    Dim dt_p As DataTable = Nothing
    '    Dim oFormula As New FormulaResult(res, dt_p, 0, dummy_ResultSetRules)
    '    Dim value As String = oFormula.ParseAndCalculate
    '    value = oFormula.ParseAndCalculate * CDec(curRate)
    '    Return value
    'End Function

End Class
