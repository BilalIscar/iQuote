Imports IscarDal

Public Class clUtils
    Public Shared Function GetList(ByVal Dal As IDal, ByVal TableName As String, Optional ByVal Library As String = "", Optional ByVal Where As String = "", Optional ByVal OrderBy As String = "", Optional ByVal Fields As String = "*") As DataTable
        Dim SQL As String

        Try
            SQL = "SELECT " & Fields & vbCrLf & " " &
                  "FROM " & Library & TableName & " " & vbCrLf

            If Not Where = String.Empty Then SQL &= " WHERE " & Where & " " & vbCrLf
            If Not OrderBy = String.Empty Then SQL &= " ORDER BY " & OrderBy & " "

            Return Dal.GetDataTable(SQL)
        Catch ex As GeneralException
            Throw
        Catch ex As Exception
            Throw New GeneralException(GeneralException.ErrorMessages.General_ErrMsg.ToString, ex, "GetList failed")
        End Try
    End Function

    Public Shared Function Digits7Number(iNumber As String) As String
        Return Format(CInt(iNumber), "0000000")
    End Function




    Public Shared Sub SetNewErrorDescription(ByRef CurerentError As String, NewAddError As String)
        Try
            CurerentError = CurerentError & Now & " : " & NewAddError & vbCrLf
        Catch ex As Exception

        End Try

    End Sub
End Class
