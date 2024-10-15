
Imports System.Configuration
Imports System.IO
Imports System.Security
Imports System.Security.Cryptography
Imports System.Text

Public Class CryptoManagerTDES

    Private Const EncodedStringSize As Integer = 256
    Private Const Delimiter As String = "%"
    Private Shared rijnKey() As Byte '= {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15}
    Private Shared rijnIV() As Byte '= {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15}


    Private Shared Function GetByteArrayFromString(ByVal Data As String) As Byte()
        Dim b() As Byte = {1, 2, 3}
        Dim DataByte(Data.Length - 1) As Byte
        Dim i As Integer
        For i = 0 To Data.Length - 1
            DataByte(i) = CByte(Asc(Data.Substring(i, 1)))
        Next
        Return DataByte
    End Function
    Private Shared Function GetStringFromByteArray(ByVal DataByte As Byte()) As String
        Dim s As String = ""
        'Dim n As Long = 0
        Dim i As Integer
        Dim FirstNonZero As Boolean = False
        For i = DataByte.Length - 1 To 0 Step -1
            If DataByte(i) = 0 And Not FirstNonZero Then
            Else
                FirstNonZero = True
                s = Chr(DataByte(i)) & s
            End If
        Next
        's = n.ToString
        Return s
    End Function

    Shared Sub New()
        Dim a As String
        a = ""
        SetKey(rijnKey)
        SetIV(rijnIV)

    End Sub

    'Private Shared KeyStr As String = "ISCAR SmiSTD ENCRYPT VAL"
    'Private Shared IVStr As String = "4802943532039404"
    Private Shared KeyStr As String = ConfigurationManager.AppSettings("DEV_KeyStr") '"KEY-ISCAR WS-DMS ENCRYPT"
    Private Shared IVStr As String = ConfigurationManager.AppSettings("DEV_IVStr") '


    Private Shared Sub SetKey(ByRef ByteArr As Byte(), Optional ByVal OldKey As Boolean = False)
        Dim i As Integer

        If OldKey Then
            'ReDim ByteArr(KeyStrOLD.Length - 1)
            'For i = 0 To KeyStrOLD.Length - 1
            '    ByteArr(i) = CByte(Asc(Mid(KeyStrOLD, i + 1, 1)))
            'Next
        Else
            ReDim ByteArr(KeyStr.Length - 1)
            For i = 0 To KeyStr.Length - 1
                ByteArr(i) = CByte(Asc(Mid(KeyStr, i + 1, 1)))
            Next
        End If

    End Sub

    Private Shared Sub SetIV(ByRef ByteArr As Byte(), Optional ByVal OldKey As Boolean = False)
        Dim i As Integer

        If OldKey Then
            'ReDim ByteArr(IVStrOLD.Length - 1)
            'For i = 0 To IVStrOLD.Length - 1
            '    ByteArr(i) = CByte(Asc(Mid(IVStrOLD, i + 1, 1)))
            'Next
        Else
            ReDim ByteArr(IVStr.Length - 1)
            For i = 0 To IVStr.Length - 1
                ByteArr(i) = CByte(Asc(Mid(IVStr, i + 1, 1)))
            Next
        End If

    End Sub

    Private Shared Function ByteArray2StringPresentation(ByVal arr As Byte()) As String
        Dim i As Integer
        Dim str As String = ""
        Dim FirstNonZero As Boolean = False

        For i = arr.Length - 1 To 0 Step -1
            If arr(i) = 0 And Not FirstNonZero Then
            Else
                str = Delimiter & CStr(arr(i)) & str
                FirstNonZero = True
            End If
        Next
        str = Trim(str)
        If str <> "" Then
            str = Mid(str, 2)
        End If
        Return str
    End Function

    Private Shared Function StringPresentation2ByteArray(ByVal str As String) As Byte()
        'Dim arr() As String
        'arr = Split(str, Delimiter)
        'Dim byteArr(arr.Length - 1) As Byte
        'Dim i As Integer
        'For i = 0 To arr.Length - 1
        '    byteArr(i) = CByte(CInt(arr(i)))
        'Next
        'Return byteArr
        Dim b64() As Byte
        b64 = Convert.FromBase64String(str)
        Return b64
    End Function

    Private Shared Function ByteArray2StringPresentation(ByVal strm As MemoryStream) As String
        'Dim i As Integer
        'Dim str As String = ""
        Dim arr() As Byte = strm.ToArray
        Dim str64 As String

        str64 = Convert.ToBase64String(arr)
        'For i = arr.Length - 1 To 0 Step -1
        '    str = Delimiter & CStr(arr(i)) & str
        'Next
        'Str = Trim(Str)
        'If str <> "" Then
        '    str = Mid(str, 2)
        'End If
        'Return str
        Return str64
    End Function

    Private Shared Function GetStringFromByteArray(ByVal strm As MemoryStream) As String
        Dim s As String = ""
        Dim i As Integer
        Dim arr() As Byte = strm.ToArray
        For i = arr.Length - 1 To 0 Step -1
            s = Chr(arr(i)) & s
        Next
        Return s
    End Function


    Public Shared Function Encode(ByVal data As String) As String
        Dim b() As Byte = GetByteArrayFromString(data)
        'Create the file streams to handle the input and output files.
        'Dim fin As New FileStream(inName, FileMode.Open, FileAccess.Read)
        Dim fin As New MemoryStream(b)

        '        Dim fout As New FileStream(OutName, FileMode.OpenOrCreate, FileAccess.Write)
        '       fout.SetLength(0)
        'Dim bout(EncodedStringSize) As Byte    !!!
        'Dim fout As New MemoryStream(bout, 0, EncodedStringSize)   !!!
        Dim fout As MemoryStream = New MemoryStream(EncodedStringSize)

        'Create variables to help with read and write.
        Dim bin(EncodedStringSize) As Byte  'This is intermediate storage for the encryption.
        Dim rdlen As Long = 0 'This is the total number of bytes written.
        Dim totlen As Long = fin.Length 'Total length of the input file.
        Dim len As Integer 'This is the number of bytes to be written at a time.
        'Creates the default implementation, which is RijndaelManaged.
        'Dim rijn As SymmetricAlgorithm = SymmetricAlgorithm.Create()    '!!!
        Dim rijn As Cryptography.TripleDES = TripleDES.Create   '!!!


        Dim encStream As New CryptoStream(fout,
           rijn.CreateEncryptor(rijnKey, rijnIV), CryptoStreamMode.Write)
        'rijn.GenerateKey()
        'rijn.GenerateIV()

        'Dim encStream As New CryptoStream(fout, _
        '   rijn.CreateEncryptor(), CryptoStreamMode.Write)


        'Read from the input file, then encrypt and write to the output file.
        While rdlen < totlen
            len = fin.Read(bin, 0, EncodedStringSize)
            encStream.Write(bin, 0, len)
            rdlen = Convert.ToInt32(rdlen + len)
            'Console.WriteLine("{0} bytes processed", rdlen)
        End While

        encStream.Close()
        fout.Close()
        fin.Close()
        ' #### CHANGE TO new FUNCTION THAT CONVERT TO STRIN PRESENTING
        '      {12,15,13,89} ==> "12,15,13,89"
        'Return GetStringFromByteArray(bout)



        'Return ByteArray2StringPresentation(bout)   '!!!
        Return ByteArray2StringPresentation(fout)
    End Function

    Public Shared Function Decode(ByVal data As String) As String
        Try

            If Not data Is Nothing AndAlso data <> "" Then

                'Const EncodedStringSize As Integer = 128


                'Dim b() As Byte = GetByteArrayFromString(Data)
                Dim b() As Byte = StringPresentation2ByteArray(data)
                'Create the file streams to handle the input and output files.
                'Dim fin As New FileStream(inName, FileMode.Open, FileAccess.Read)
                Dim fin As New MemoryStream(b)

                '        Dim fout As New FileStream(OutName, FileMode.OpenOrCreate, FileAccess.Write)
                '       fout.SetLength(0)
                'Dim bout(EncodedStringSize) As Byte    !!!
                'Dim fout As New MemoryStream(bout, 0, EncodedStringSize)   '!!!
                Dim fout As MemoryStream = New MemoryStream(EncodedStringSize) '!!!

                'Create variables to help with read and write.
                Dim bin(EncodedStringSize) As Byte  'This is intermediate storage for the encryption.
                Dim rdlen As Long = 0 'This is the total number of bytes written.
                Dim totlen As Long = fin.Length 'Total length of the input file.
                Dim len As Integer 'This is the number of bytes to be written at a time.
                'Creates the default implementation, which is RijndaelManaged.
                'Dim rijn As SymmetricAlgorithm = SymmetricAlgorithm.Create()    '!!!
                Dim rijn As Cryptography.TripleDES = TripleDES.Create



                Dim encStream As New CryptoStream(fout,
                   rijn.CreateDecryptor(rijnKey, rijnIV), CryptoStreamMode.Write)
                'rijn.GenerateKey()
                'rijn.GenerateIV()

                'Dim encStream As New CryptoStream(fout, _
                '   rijn.CreateEncryptor(), CryptoStreamMode.Write)


                'Read from the input file, then encrypt and write to the output file.
                While rdlen < totlen
                    len = fin.Read(bin, 0, EncodedStringSize)
                    encStream.Write(bin, 0, len)
                    rdlen = Convert.ToInt32(rdlen + len)
                    'Console.WriteLine("{0} bytes processed", rdlen)
                End While

                encStream.Close()
                fout.Close()
                fin.Close()



                'Return GetStringFromByteArray(bout)    '!!!
                Return GetStringFromByteArray(fout)
            Else
                Return ""
            End If

        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Shared Function GetSignature(ByVal clientId As String, ByVal clientSecret As String, ByVal dateString As String) As String
        Dim stringToSign As String = String.Format("{0}:{1}", clientId, dateString, clientSecret)
        Dim hmac As HMACSHA1 = New HMACSHA1(Encoding.UTF8.GetBytes(clientSecret))
        Return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)))
    End Function
End Class

