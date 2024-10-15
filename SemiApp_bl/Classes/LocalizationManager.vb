Imports System.Globalization
Imports System.Threading
Imports Newtonsoft.Json.Linq

Public Class LocalizationManager

    Public Function CurencyFormater(ByVal Value As Decimal) As String
        Dim s As String

        Try

            s = String.Format("{0:c}", Value)

            Return s
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function NumberCulterSpecificFormater(ByVal Value As Decimal, Optional ByVal FormatString As String = "") As String
        Dim s As String
        Dim _Culture As CultureInfo = Thread.CurrentThread.CurrentCulture

        Try
            'Thread.CurrentThread.CurrentCulture = Culture

            If FormatString = "" Then
                s = Value.ToString()
            Else
                s = String.Format(FormatString, Value)
            End If

            'Thread.CurrentThread.CurrentCulture = _Culture

            Return s
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function CurrencySymbol() As String
        Dim s As String
        Dim _Culture As CultureInfo = Thread.CurrentThread.CurrentCulture

        Try
            'Thread.CurrentThread.CurrentCulture = Culture

            s = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol

            'Thread.CurrentThread.CurrentCulture = _Culture

            Return s
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function DateFormater(ByVal Value As DateTime, ByVal FormatString As String) As String
        Try
            Return String.Format("{0:" & FormatString & "}", Value)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetDateFormat(d_text As String) As String
        Dim dateString As String = d_text
        Dim formats As String = "MM/dd/yyyy"
        Dim dateValue As DateTime

        If DateTime.TryParseExact(dateString, formats, New CultureInfo("en-US"), DateTimeStyles.None, dateValue) Then
            Return "MM/dd/yyyy"
        End If

        'Dim dt As Date
        'If Date.TryParseExact(lblDate.Text.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture, Globalization.DateTimeStyles.None, dt) Then

        'End If
    End Function


    Public Shared Function NumberFormater(ByVal Value As Decimal, ByVal FormatString As String) As String
        Try
            Return CStr(IIf(String.Format("{0:" & FormatString & "}", Value) = String.Empty, 0, String.Format("{0:" & FormatString & "}", Value)))
        Catch ex As Exception
            Throw
        End Try
    End Function

    'Public Shared Function RoundForGraph(ByVal value As Integer) As Integer

    '    Dim s As String
    '    Dim n As Integer
    '    Dim m As Integer

    '    n = value.ToString.Length
    '    m = CType(Left(value.ToString, 2), Integer)

    '    If m = 0 Then
    '        m = 0
    '    ElseIf (m = 10 Or m = 11) Then
    '        m = 12
    '    ElseIf (m >= 12 And m <= 14) Then
    '        m = 15
    '    ElseIf (m >= 15 And m <= 19) Then
    '        m = 20
    '    ElseIf (m >= 20 And m <= 24) Then
    '        m = 25
    '    ElseIf (m >= 25 And m <= 29) Then
    '        m = 30
    '    ElseIf (m >= 30 And m <= 34) Then
    '        m = 35
    '    ElseIf (m >= 35 And m <= 39) Then
    '        m = 40
    '    ElseIf (m >= 40 And m <= 49) Then
    '        m = 50
    '    ElseIf (m >= 50 And m <= 59) Then
    '        m = 60
    '    ElseIf (m >= 60 And m <= 69) Then
    '        m = 70
    '    ElseIf (m >= 70 And m <= 79) Then
    '        m = 80
    '    Else : m = 100
    '    End If

    '    If m = 0 Then
    '        n = 0
    '        n = 0
    '    Else
    '        n = n - 2
    '        s = Replace(Space(n), " ", "0")
    '    End If

    '    s = m.ToString & s

    '    Return CInt(s)
    'End Function


    'Public Sub New()
    '    Culture = New CultureInfo(StateManager.GetValue(StateManager.Keys.BranchCurency))
    '    DefaultCulture = New CultureInfo(General.GetSetting("DefaultCultureName"))
    'End Sub

    'Public Sub New(ByVal CultureString As String)
    '    Culture = New CultureInfo(CultureString)
    'End Sub

    Public Shared Function GetCurentSaparetaorNumberCulture()

        Dim Separator As String = "P"
        Dim curui As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
        Dim curui1 As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentUICulture
        Try

            If Not StateManager.GetValue(StateManager.Keys.s_Culture, False) Is Nothing Then
                If StateManager.GetValue(StateManager.Keys.s_Culture, False) <> "" Then
                    Dim cc As String = StateManager.GetValue(StateManager.Keys.s_Culture, False)
                    System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(cc)
                    System.Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(cc)
                End If
            End If

            Separator = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator

            System.Threading.Thread.CurrentThread.CurrentCulture = curui
            System.Threading.Thread.CurrentThread.CurrentUICulture = curui1

            If Separator = "," Then
                Return "C"
            End If
            Return "P"
        Catch ex As Exception
            System.Threading.Thread.CurrentThread.CurrentCulture = curui
            System.Threading.Thread.CurrentThread.CurrentUICulture = curui1
            Return "P"
        End Try
    End Function

    Public Shared Function GetCurentStartWithDateCulture()
        Dim DateStartWith As String = "D"
        Dim curui As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
        Dim curui1 As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentUICulture
        Try

            If Not StateManager.GetValue(StateManager.Keys.s_Culture, False) Is Nothing Then
                If StateManager.GetValue(StateManager.Keys.s_Culture, False) <> "" Then
                    Dim cc As String = StateManager.GetValue(StateManager.Keys.s_Culture, False)
                    System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(cc)
                    System.Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(cc)
                End If
            End If

            If DateTimeFormatInfo.CurrentInfo.ShortDatePattern.ToString.Length > 0 AndAlso DateTimeFormatInfo.CurrentInfo.ShortDatePattern.StartsWith("m") Then
                DateStartWith = "M"
            ElseIf DateTimeFormatInfo.CurrentInfo.ShortDatePattern.ToString.Length > 0 AndAlso DateTimeFormatInfo.CurrentInfo.ShortDatePattern.StartsWith("M") Then
                DateStartWith = "M"
            End If

            System.Threading.Thread.CurrentThread.CurrentCulture = curui
            System.Threading.Thread.CurrentThread.CurrentUICulture = curui1

            Return DateStartWith
        Catch ex As Exception
            System.Threading.Thread.CurrentThread.CurrentCulture = curui
            System.Threading.Thread.CurrentThread.CurrentUICulture = curui1

            Return DateStartWith
        End Try
    End Function

    Public Shared Function CulturingNumber(ByVal value As String, totalPrice As Boolean) As String
        Dim curui As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
        Dim curui1 As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentUICulture
        Try


            If IsNumeric(value) Then
                Dim morT As Boolean = False

                If value > 1000 Then
                    morT = True
                End If

                If Not StateManager.GetValue(StateManager.Keys.s_Culture, False) Is Nothing Then
                    If StateManager.GetValue(StateManager.Keys.s_Culture, False) <> "" Then
                        Dim cc As String = StateManager.GetValue(StateManager.Keys.s_Culture, False)
                        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(cc)
                        System.Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(cc)
                    End If
                End If

                Dim Separator As String = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator

                If Separator = "," Then
                    value = Replace(value.ToString, ".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator)
                    If totalPrice = True AndAlso morT = True Then
                        Dim i As Integer = 0
                        Dim sval As String = Format(CDbl(value), "#,###.##")
                        Return sval
                    End If
                End If
            End If
            System.Threading.Thread.CurrentThread.CurrentCulture = curui
            System.Threading.Thread.CurrentThread.CurrentUICulture = curui1

            Return value
        Catch ex As Exception
            System.Threading.Thread.CurrentThread.CurrentCulture = curui
            System.Threading.Thread.CurrentThread.CurrentUICulture = curui1

            Return value
        End Try


    End Function
    Public Shared Function CulturingDate(ByVal value As String) As String
        Dim curui As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
        Dim curui1 As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentUICulture
        Try


            If IsDate(value) Then

                If Not StateManager.GetValue(StateManager.Keys.s_Culture, False) Is Nothing Then
                    If StateManager.GetValue(StateManager.Keys.s_Culture, False) <> "" Then
                        Dim cc As String = StateManager.GetValue(StateManager.Keys.s_Culture, False)
                        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(cc)
                        System.Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(cc)
                    End If
                End If

                Dim StartL As String = DateTimeFormatInfo.CurrentInfo.ShortDatePattern
                If StartL.ToString.Length > 0 AndAlso StartL.StartsWith("m") Then
                    Dim sDate() As String = value.Split("/")
                    Dim sNewDate As String = sDate(1) & "/" & sDate(0) & "/" & sDate(2)
                    value = sNewDate
                ElseIf StartL.ToString.Length > 0 AndAlso StartL.StartsWith("M") Then
                    Dim sDate() As String = value.Split("/")
                    Dim sNewDate As String = sDate(1) & "/" & sDate(0) & "/" & sDate(2)
                    value = sNewDate
                End If

            End If
            System.Threading.Thread.CurrentThread.CurrentCulture = curui
            System.Threading.Thread.CurrentThread.CurrentUICulture = curui1

            Return value
        Catch ex As Exception
            System.Threading.Thread.CurrentThread.CurrentCulture = curui
            System.Threading.Thread.CurrentThread.CurrentUICulture = curui1

            Return value
        End Try

    End Function
    Public Shared Function CulturingNumberForShowInValueTxt(ByVal value As String) As String
        Try
            If StateManager.GetValue(StateManager.Keys.s_BranchCode) = "IDxx" Then
                If IsNumeric(value) Then
                    value = Replace(value, ".", ",")
                End If
            End If
            Return value
        Catch ex As Exception
            Return value
        End Try
    End Function



    Public Shared Function UnCulturingNumber(ByVal value As String) As String
        Dim curui As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
        Dim curui1 As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentUICulture

        Try
            If IsNumeric(value) Then

                If Not StateManager.GetValue(StateManager.Keys.s_Culture, False) Is Nothing Then
                    If StateManager.GetValue(StateManager.Keys.s_Culture, False) <> "" Then
                        Dim cc As String = StateManager.GetValue(StateManager.Keys.s_Culture, False)
                        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(cc)
                        System.Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(cc)
                    End If
                End If

                Dim Separator As String = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator

                If Separator = "," Then
                    value = Replace(value.ToString, ",", ".")
                End If
            End If
            System.Threading.Thread.CurrentThread.CurrentCulture = curui
            System.Threading.Thread.CurrentThread.CurrentUICulture = curui1

            Return value
        Catch ex As Exception
            System.Threading.Thread.CurrentThread.CurrentCulture = curui
            System.Threading.Thread.CurrentThread.CurrentUICulture = curui1

            'System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US")
            'System.Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo("en-US")

            Return value
        End Try


    End Function



    'Public Shared Function ConvertJsonToDataTable(jsonData As String) As DataTable

    '    Dim dataTable As New DataTable()
    '    Try
    '        If jsonData.IndexOf("[") = 0 Then
    '            jsonData = jsonData
    '        Else
    '            jsonData = "[" & jsonData & "]"
    '        End If


    '        ' Parse JSON array
    '        Dim jsonArray As JArray = JArray.Parse(jsonData)

    '        ' Extract column names from the first item
    '        Dim columns As List(Of String) = jsonArray.First().ToObject(Of JObject)().Properties().Select(Function(p) p.Name).ToList()

    '        ' Add columns to DataTable
    '        For Each columnName As String In columns
    '            dataTable.Columns.Add(columnName, GetType(String))
    '        Next

    '        ' Populate DataTable with data
    '        For Each item As JObject In jsonArray
    '            Dim values As Object() = item.Properties().Select(Function(p) p.Value.ToString()).ToArray()
    '            dataTable.Rows.Add(values)
    '        Next

    '        Return dataTable
    '    Catch ex As Exception
    '        dataTable = Nothing
    '        Return dataTable
    '    End Try


    'End Function

    Public Shared Function Convert_JsonToDataTable(jsonData As String) As DataTable

        Dim dataTable As New DataTable()
        Try
            If Not jsonData Is Nothing Then
                If jsonData.IndexOf("[") = 0 Then
                    jsonData = jsonData
                Else
                    jsonData = "[" & jsonData & "]"
                End If


                ' Parse JSON array
                Dim jsonArray As JArray = JArray.Parse(jsonData)

                ' Extract column names from the first item
                Dim columns As List(Of String) = jsonArray.First().ToObject(Of JObject)().Properties().Select(Function(p) p.Name).ToList()

                ' Add columns to DataTable
                For Each columnName As String In columns
                    dataTable.Columns.Add(columnName, GetType(String))
                Next

                ' Populate DataTable with data
                For Each item As JObject In jsonArray
                    Dim values As Object() = item.Properties().Select(Function(p) p.Value.ToString()).ToArray()
                    dataTable.Rows.Add(values)
                Next

            End If

            Return dataTable
        Catch ex As Exception
            dataTable = Nothing
            Return dataTable
        End Try


    End Function
End Class
