Imports IscarDal

Public Class clsStandard

    Public Shared Function GetStandardCatalog(ByVal FamAbs As String, ByVal _Params As DataTable) As String

        Try

            Dim sFamAbs As String = FamAbs
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@FamAbs", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, FamAbs, 2000))
            dt = oSql.ExecuteSPReturnDT("uspCreateCrossTab", oParams)

            Dim dr() As DataRow
            Dim Filter As String = ""
            Dim preFilter As String

            dr = _Params.Select("Priority>=0", "Priority")

            Dim IsNumericColumn_l As Boolean
            Dim dt_Temp As New DataTable
            For Each col As DataColumn In dt.Columns
                dt_Temp.Columns.Add(col.ColumnName)
            Next

            For Each col As DataColumn In dt.Columns
                IsNumericColumn_l = False
                For Each row As DataRow In dt.Rows
                    If Not row.Item(col.ColumnName) Is Nothing AndAlso IsNumeric(row.Item(col.ColumnName)) Then
                        If Format(CDec(row.Item(col.ColumnName)), "#.###############").ToString = "" Then
                            row.Item(col.ColumnName) = 0
                        Else
                            row.Item(col.ColumnName) = Format(CDec(row.Item(col.ColumnName)), "#.###############")
                        End If
                    Else
                        IsNumericColumn_l = True
                    End If
                Next
                If IsNumericColumn_l = False AndAlso col.ColumnName <> "itm_cat" Then
                    dt_Temp.Columns(col.ColumnName).DataType = Type.GetType("System.Decimal")
                End If
            Next

            For Each row As DataRow In dt.Rows
                dt_Temp.Rows.Add()
            Next
            Dim i As Integer = 0
            For Each col As DataColumn In dt.Columns
                For Each row As DataRow In dt.Rows
                    dt_Temp.Rows(i).Item(col.ColumnName) = row.Item(col.ColumnName).ToString
                    i += 1
                Next
                i = 0
            Next

            For Each dtc As DataColumn In dt_Temp.Columns
                If IsNumeric(dtc.ColumnName) Then
                    dtc.ColumnName = "c" & dtc.ColumnName
                End If
            Next

            Dim dvStandard As DataView = dt_Temp.DefaultView
            Dim sOprator As String
            Dim sColNameLabel As String = ""
            Dim sColNameLabelForSort As String = ""

            Dim sValue As String

            Filter = "itm_cat <> '' "
            dvStandard.RowFilter = Filter
            For Each dvr As DataRow In dr
                If dvr("Priority") > 0 Then '0 = Family

                    If Not dvr("FindOperator") Is DBNull.Value Then sOprator = dvr("FindOperator") Else sOprator = "="
                    If IsNumeric(dvr("Measure")) = True And dvr("Measure").ToString.IndexOf(".") = -1 And sOprator <> "=" Then
                        sValue = dvr("Measure") & ".0"
                    Else
                        sValue = dvr("Measure")
                    End If
                    sValue = sValue.Trim

                    Dim LabelNumExsit As Boolean = False
                    Dim LabelOption As Boolean = False


                    sColNameLabel = "c" & dvr("LabelNum")

                    '---Check if the column exists in the table with LabelNum value
                    For Each dtc As DataColumn In dt_Temp.Columns
                        If dtc.ColumnName = "c" & dvr("LabelNum") Then
                            LabelNumExsit = True : Exit For
                        End If
                    Next

                    If LabelNumExsit = False AndAlso Not dvr("OptionalLabelNum") Is Nothing AndAlso IsNumeric(dvr("OptionalLabelNum")) Then
                        sColNameLabel = "c" & dvr("OptionalLabelNum")
                        '---Check if the column exists in the table with OptionalLabelNum value
                        For Each dtc As DataColumn In dt_Temp.Columns
                            If dtc.ColumnName = "c" & dvr("OptionalLabelNum") Then
                                LabelOption = True : Exit For
                            End If
                        Next

                    End If


                    '****Check if the column exists in the table with LabelNum or OptionalLabelNum value****
                    If LabelNumExsit = True Or LabelOption = True Then
                        If dvr("LabelNum").ToString <> "" AndAlso dvr("LabelNum").ToString <> "0" Then
                            sColNameLabelForSort = sColNameLabel
                        End If

                        preFilter = dvStandard.RowFilter
                        Select Case sOprator
                            Case "="
                                Filter &= " and " & sColNameLabel & " " & sOprator & " '" & sValue & "'"
                                'If sColNameOptional <> "" AndAlso sColNameOptional.Length > 1 AndAlso IsNumeric(Mid(sColNameOptional, 2, sColNameOptional.Length)) Then
                                '    Filter &= " and (" & sColNameOptional & " " & sOprator & " '" & sValue & "' OR " & sColNameLabel & " " & sOprator & " '" & sValue & "') "
                                'Else
                                '    Filter &= " and " & sColNameLabel & " " & sOprator & " '" & sValue & "'"
                                'End If

                                dvStandard.RowFilter = Filter
                                If dvStandard.Count = 0 Then
                                    dvStandard.RowFilter = preFilter
                                    Filter = preFilter
                                End If
                            Case "<>"
                                Dim famValue1 As String = ""
                                Dim famValue2 As String = ""

                                'dvStandard.Table.Columns("c153").Expression = "convert(c35,'System.Decimal')"
                                If IsNumeric(sValue) = True Then
                                    dvStandard.RowFilter &= " and " & sColNameLabel & " >= " & (sValue)
                                Else
                                    Dim nRowf As String = dvStandard.RowFilter.ToString
                                    Try
                                        dvStandard.RowFilter &= " and " & sColNameLabel & " = '" & sValue & "'"
                                        If dvStandard.Count = 0 Then
                                            dvStandard.RowFilter = nRowf
                                        End If
                                    Catch ex As Exception
                                        dvStandard.RowFilter = preFilter
                                        'no action, it happen when the datacolumntype=decimal and the sValue = string
                                    End Try
                                End If

                                'dvStandard.Sort = sColNameLabel
                                If dvStandard.Sort <> "" Then
                                    dvStandard.Sort &= "," & sColNameLabel
                                Else
                                    dvStandard.Sort = sColNameLabel
                                End If
                                If dvStandard.Count > 0 Then famValue1 = dvStandard.Item(0)(sColNameLabel)

                                dvStandard.RowFilter = preFilter
                                If IsNumeric(sValue) = True Then
                                    dvStandard.RowFilter &= " and " & sColNameLabel & " <= " & sValue
                                End If

                                If dvStandard.Sort <> "" Then
                                    dvStandard.Sort &= "," & sColNameLabel & " DESC"
                                    dvStandard.Sort = Replace(dvStandard.Sort, sColNameLabel & "," & sColNameLabel & " DESC", sColNameLabel & " DESC")
                                Else
                                    dvStandard.Sort = sColNameLabel & " DESC"
                                End If

                                If dvStandard.Count > 0 Then famValue2 = dvStandard.Item(0)(sColNameLabel)

                                dvStandard.RowFilter = preFilter

                                If famValue1 <> "" AndAlso IsNumeric(famValue1) = True And
                                famValue2 <> "" AndAlso IsNumeric(famValue2) = True AndAlso
                                    IsNumeric(sValue) = True Then
                                    If IsNumeric(famValue1) = True And IsNumeric(famValue2) = True Then
                                        If famValue1 - sValue <= sValue - famValue2 Then
                                            Filter &= " and " & sColNameLabel & " = '" & famValue1 & "'"
                                        Else
                                            Filter &= " and " & sColNameLabel & " = '" & famValue2 & "'"
                                        End If
                                    End If
                                ElseIf famValue1 <> "" AndAlso IsNumeric(famValue1) = True Then
                                    sValue = famValue1
                                    Filter &= " and " & sColNameLabel & " = '" & sValue & "'"
                                ElseIf famValue2 <> "" AndAlso IsNumeric(famValue2) = True Then
                                    sValue = famValue2
                                    Filter &= " and " & sColNameLabel & " = '" & sValue & "'"
                                End If

                                dvStandard.RowFilter = Filter

                                If dvStandard.Count = 0 Then
                                    dvStandard.RowFilter = preFilter
                                    Filter = preFilter
                                End If
                            Case "<="
                                Filter &= " and " & sColNameLabel & " " & sOprator & " " & sValue
                                dvStandard.RowFilter = Filter


                                If dvStandard.Sort <> "" Then
                                    dvStandard.Sort &= "," & sColNameLabel & " DESC"
                                    dvStandard.Sort = Replace(dvStandard.Sort, sColNameLabel & "," & sColNameLabel & " DESC", sColNameLabel & " DESC")
                                Else
                                    dvStandard.Sort = sColNameLabel & " DESC"
                                End If

                                If dvStandard.Count = 0 Then
                                    dvStandard.RowFilter = preFilter
                                    Filter = preFilter
                                End If
                            Case ">="
                                Filter &= " and " & sColNameLabel & " " & sOprator & " " & sValue
                                dvStandard.RowFilter = Filter
                                If dvStandard.Sort <> "" Then
                                    dvStandard.Sort &= "," & sColNameLabel
                                Else
                                    dvStandard.Sort = sColNameLabel
                                End If
                                If dvStandard.Count = 0 Then
                                    dvStandard.RowFilter = preFilter
                                    Filter = preFilter
                                End If
                        End Select
                    End If
                End If
            Next

            sColNameLabel = "itm_cat"


            '----------------------
            'Sorting the lat column in order to get the the first item

            If dvStandard.Table.Rows.Count > 0 AndAlso sColNameLabelForSort <> "" Then
                Dim isNum As Boolean = True
                For Each row As DataRow In dvStandard.Table.Rows
                    If row.Item(sColNameLabelForSort).ToString = "" OrElse IsNumeric(row.Item(sColNameLabelForSort)) = False Then
                        isNum = False : Exit For
                    End If
                Next
                If isNum = True Then
                    dvStandard.Table.Columns.Add("SortCol", Type.GetType("System.Decimal"))
                    dvStandard.Table.Columns("SortCol").AllowDBNull = True
                    For Each row As DataRow In dvStandard.Table.Rows
                        row.Item("SortCol") = row.Item(sColNameLabelForSort)
                    Next

                    If dvStandard.Count > 1 Then
                        If dvStandard.Sort <> "" Then
                            Dim SortType As String = ""
                            Try
                                'c182 DESC,c174 DESC
                                If InStr(dvStandard.Sort, sColNameLabelForSort & " DESC") > 0 Then SortType = " DESC"
                                dvStandard.Sort = Replace(dvStandard.Sort, "," & sColNameLabelForSort & " DESC", "")
                                dvStandard.Sort = Replace(dvStandard.Sort, "," & sColNameLabelForSort, "")
                                If dvStandard.Sort <> "" Then dvStandard.Sort &= ",SortCol" & SortType Else dvStandard.Sort = "SortCol" & SortType

                            Catch ex As Exception
                            End Try

                        Else
                            dvStandard.Sort = "SortCol"
                        End If
                    End If
                End If
            End If

            Return dvStandard.Item(0)("itm_cat").ToString.Trim


        Catch ex As Exception
            Throw
        End Try
    End Function





End Class
