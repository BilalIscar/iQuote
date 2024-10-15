Imports System.Drawing

Public Class Images
    Public Shared Function GetImageReturnByteImageStreem(ImageS As Image) As Byte()
        Try

            If Not ImageS Is Nothing Then
                'lImage = InitializeStreamBitmap()
                Dim bitmapBytes As Byte() = Nothing
                Using stream As New System.IO.MemoryStream
                    ImageS.Save(stream, Imaging.ImageFormat.Gif)
                    bitmapBytes = stream.ToArray
                End Using
                ImageS.Dispose()
                ImageS = Nothing
                Return bitmapBytes
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function


End Class
