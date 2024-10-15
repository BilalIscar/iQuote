Public Class ClsDate


    Enum Enum_DateFormatTypes
        yyyymmdd = 1
        yyyymmdd_MMSS = 2
        yyyymmdd_HHMMSS = 3
        _Date = 4
        _ddmmyy = 5
        _Hour = 6
        _Day = 7
        _Year = 8
        _Month = 9
        _ddmmyyyy = 10
        yyyymmddHHMMSS = 11

    End Enum

    Public Shared Function GetDateTimeReturnStringFormat(DateToConvert As Date, Date_Format As Integer) As String
        'DateToConvert.ToString("dd-MM-yyy HH:mm:ss")
        Try
            Dim sD As String = ""
            Select Case Date_Format
                Case 1
                    sD = String.Format("{0:00}", Convert.ToDateTime(DateToConvert).Year)
                    sD &= String.Format("{0:00}", Convert.ToDateTime(DateToConvert).Month)
                    sD &= String.Format("{0:00}", Convert.ToDateTime(DateToConvert).Day)
                Case 2
                    sD = String.Format("{0:00}", Convert.ToDateTime(DateToConvert).Year)
                    sD &= String.Format("{0:00}", Convert.ToDateTime(DateToConvert).Month)
                    sD &= String.Format("{0:00}", Convert.ToDateTime(DateToConvert).Day)
                    sD &= " "
                    sD &= String.Format("{0:00}", Convert.ToDateTime(Now).Hour)
                    sD &= ":"
                    sD &= String.Format("{0:00}", Convert.ToDateTime(Now).Minute)
                Case 3
                    sD = String.Format("{0:00}", Convert.ToDateTime(DateToConvert).Year)
                    sD &= String.Format("{0:00}", Convert.ToDateTime(DateToConvert).Month)
                    sD &= String.Format("{0:00}", Convert.ToDateTime(DateToConvert).Day)
                    sD &= " "
                    sD &= String.Format("{0:00}", Convert.ToDateTime(Now).Hour)
                    sD &= ":"
                    sD &= String.Format("{0:00}", Convert.ToDateTime(Now).Minute)
                    sD &= ":"
                    sD &= String.Format("{0:00}", Convert.ToDateTime(Now).Second)
                Case 4
                    sD = Convert.ToDateTime(DateToConvert).Date
                Case 5
                    sD = Format(Convert.ToDateTime(DateToConvert).Date.Day, "00") & "/" & Format(Convert.ToDateTime(DateToConvert).Date.Month, "00") & "/" & Right(Convert.ToDateTime(DateToConvert).Date.Year, 2)
                Case 6
                    sD = Convert.ToDateTime(DateToConvert).Hour
                Case 7
                    sD = Convert.ToDateTime(DateToConvert).Day
                Case 8
                    sD = Convert.ToDateTime(DateToConvert).Year
                Case 9
                    sD = Convert.ToDateTime(DateToConvert).Month
                Case 10
                    sD = Format(Convert.ToDateTime(DateToConvert).Date.Day, "00") & "/" & Format(Convert.ToDateTime(DateToConvert).Date.Month, "00") & "/" & Right(Convert.ToDateTime(DateToConvert).Date.Year, 4)

                Case 11
                    sD = String.Format("{0:00}", Convert.ToDateTime(DateToConvert).Year)
                    sD &= String.Format("{0:00}", Convert.ToDateTime(DateToConvert).Month)
                    sD &= String.Format("{0:00}", Convert.ToDateTime(DateToConvert).Day)
                    sD &= " "
                    sD &= String.Format("{0:00}", Convert.ToDateTime(Now).Hour)
                    sD &= ":"
                    sD &= String.Format("{0:00}", Convert.ToDateTime(Now).Minute)
                    sD &= ":"
                    sD &= String.Format("{0:00}", Convert.ToDateTime(Now).Second)
                    sD = Replace(sD, ":", "")
                    sD = Replace(sD, " ", "")
            End Select
            Return sD
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
