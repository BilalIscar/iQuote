Imports System.IO
Public Class CryptoManager
    Public Shared Function Encode(ByVal data As String) As String
        Dim ms As New MemoryStream()
        Dim wr As StreamWriter

        wr = New StreamWriter(ms)

        wr.AutoFlush = True

        wr.Write(data)

        Return (Convert.ToBase64String(ms.ToArray))
    End Function

    Public Shared Function Decode(ByVal data As String) As String
        Return System.Text.Encoding.Default.GetString(Convert.FromBase64String(data))
    End Function
End Class

