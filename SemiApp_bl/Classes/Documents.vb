Imports System.Configuration
Imports System.IO
Imports System.Web
Public Class Documents

    Public Shared Sub CreateLocalDirectory(folderPath As String)
        Try

            If Not Directory.Exists(folderPath) Then
                'If Directory (Folder) does not exists. Create it.
                Directory.CreateDirectory(folderPath)
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub


    Public Shared Sub UploadFileToDocuments(BranchCode As String, DocMngFolderPath As String, fileName As String, LocalFilePathAndName As String, FunctionNameForErrors As String, StorageFolder As String)

        Dim UserAS400 As String = "DBO"


        Try

            If ConfigurationManager.AppSettings("DOCMNG_ACTIVE") = "YES" Then
                Dim File() As Byte
                Try
                    Dim fs As New FileStream(LocalFilePathAndName & fileName, FileMode.Open, FileAccess.Read)
                    ReDim File(fs.Length - 1)
                    fs.Read(File, 0, fs.Length)
                    fs.Close()
                    fs = Nothing
                Catch ex As Exception
                    Throw New Exception("Temp Local File load failure, " & ex.Message)
                End Try

                Dim Success As String = ""
                If ConfigurationManager.AppSettings("DocMngService").ToString.Trim = "CLOUD" Then
                    Success = DocmngServiceAuthentication("UPLOAD", "", "", BranchCode, fileName, "", StorageFolder, "", "", "", "", "", "", DocMngFolderPath, "", UserAS400, File, Nothing, Nothing)
                Else
                    Success = DocmngServiceAuthentication("UPLOAD", "", "", BranchCode, fileName, "", StorageFolder, "", "", "", "", "", "", DocMngFolderPath, "", UserAS400, File, Nothing, Nothing)
                End If
                If Success.ToString.ToUpper.Contains("FALSE") Then
                    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.DOCMNG_ERROR.ToString, "Function Name - " & FunctionNameForErrors, "End upload " & FunctionNameForErrors & " to document - " & Success, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail.ToString)
                Else

                End If

            End If

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.DOCMNG_ERROR.ToString, "UploadFileToDocuments", DocMngFolderPath.ToString & "-" & fileName.ToString & "-" & FunctionNameForErrors & "-" & ex.Message, BranchCode, "", "", UserAS400)
            Throw
        End Try
    End Sub

    Public Shared Sub UploadLocalFileToDocuments(ByVal LocalFilePath As String, ByVal FolderPath As String, ByVal User As String, BranchCode As String, StorageFolder As String, Optional sFileName As String = "")
        Try
            If ConfigurationManager.AppSettings("DOCMNG_ACTIVE") = "YES" Then
                Dim HostServer As String = ""

                Dim FileName As String = ""
                If sFileName <> "" Then
                    FileName = sFileName
                Else
                    FileName = System.IO.Path.GetFileName(LocalFilePath)
                End If

                Dim File() As Byte
                Try

                    Dim fs As New FileStream(LocalFilePath, FileMode.Open, FileAccess.Read)
                    ReDim File(fs.Length - 1)
                    fs.Read(File, 0, fs.Length)
                    fs.Close()
                    fs = Nothing
                Catch ex As Exception
                    Throw New Exception("Temp Local File load failure, " & ex.Message)
                End Try

                If ConfigurationManager.AppSettings("DocMngService").ToString.Trim = "CLOUD" Then
                    Dim Success As String = DocmngServiceAuthentication("UPLOAD", "", "", BranchCode, FileName, "", StorageFolder, "", "", "", "", "", "", FolderPath, "", User, File, Nothing, Nothing)
                Else
                    Dim Success As String = DocmngServiceAuthentication("UPLOAD", "", "", BranchCode, FileName, "", StorageFolder, "", "", "", "", "", "", FolderPath, "", User, File, Nothing, Nothing)
                End If

            End If
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.DOCMNG_ERROR.ToString, "UploadLocalFileToDocuments", "ex=" & ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        End Try
    End Sub

    Public Shared Function DeleteFileNotBelongToQutReports(ByVal BranchCode As String, ByVal Subject As String, ByVal FolderPath As String, FileNAme As String, FileId As String, as400usr As String, DeleteReports As Boolean)
        Try
            If (ConfigurationManager.AppSettings("DOCMNG_ACTIVE") = "YES" AndAlso Not FileNAme.ToString.ToUpper.Contains("REPORT") AndAlso Not FileNAme.ToString.ToUpper.Contains("DRW")) Or DeleteReports = True Then

                If ConfigurationManager.AppSettings("DocMngService").ToString.Trim = "CLOUD" Then
                    Dim Success As String = DocmngServiceAuthentication("DELETE", "", "", BranchCode, FileNAme, FolderPath, "", Subject, "", "", "", "", "", "", FileId.ToString, as400usr, Nothing, Nothing, Nothing)
                Else
                    Dim Success As String = DocmngServiceAuthentication("DELETE", "", "", BranchCode, FileNAme, FolderPath, "", Subject, "", "", "", "", "", "", FileId.ToString, as400usr, Nothing, Nothing, Nothing)
                End If

            End If

        Catch ex As Exception

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.DOCMNG_ERROR.ToString, "DeleteFileNotBelongToQutReports", "ex=" & ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

        End Try
    End Function
    Public Shared Sub DeleteFile(ByVal BranchCode As String, ByVal Subject As String, ByVal FolderPath As String, AllFiles As Boolean)
        Try
            If ConfigurationManager.AppSettings("DOCMNG_ACTIVE") = "YES" Then
                AllFiles = False

                Dim dt As DataTable = Documents.GetDocumentsList(BranchCode, FolderPath, Subject)
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    Dim fId As String
                    Dim as400usr As String
                    For Each r As DataRow In dt.Rows
                        fId = r("FileId").ToString
                        as400usr = r("as400usr")
                        Dim rName As String = r("FName").ToString.ToUpper
                        Dim rNmaeL As String = rName
                        Try
                            rNmaeL = "DRW_" & Format(CInt(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)), "0000000") & ".pdf"
                        Catch ex As Exception
                            rNmaeL = rName
                        End Try

                        Dim bDoDeleteBoth As Boolean = False
                        If rName <> rNmaeL.ToUpper Then
                            bDoDeleteBoth = True
                        End If

                        If AllFiles = True Then
                            Try
                                If ConfigurationManager.AppSettings("DocMngService").ToString.Trim = "CLOUD" Then
                                    Dim Success As String = DocmngServiceAuthentication("DELETE", "", "", BranchCode, rName, FolderPath, "", Subject, "", "", "", "", "", "", fId.ToString, as400usr, Nothing, Nothing, Nothing)
                                    If bDoDeleteBoth Then Success = DocmngServiceAuthentication("DELETE", "", "", BranchCode, rNmaeL, FolderPath, "", Subject, "", "", "", "", "", "", fId.ToString, as400usr, Nothing, Nothing, Nothing)
                                Else
                                    Dim Success As String = DocmngServiceAuthentication("DELETE", "", "", BranchCode, rName, FolderPath, "", Subject, "", "", "", "", "", "", fId.ToString, as400usr, Nothing, Nothing, Nothing)
                                    If bDoDeleteBoth Then Success = DocmngServiceAuthentication("DELETE", "", "", BranchCode, rNmaeL, FolderPath, "", Subject, "", "", "", "", "", "", fId.ToString, as400usr, Nothing, Nothing, Nothing)

                                End If

                            Catch ex As Exception
                            End Try
                        ElseIf rName.Contains("REPORT_") Or rName.Contains("DRW") Then

                            Try
                                If ConfigurationManager.AppSettings("DocMngService").ToString.Trim = "CLOUD" Then
                                    Dim Success As String = DocmngServiceAuthentication("DELETE", "", "", BranchCode, rName, FolderPath, "", Subject, "", "", "", "", "", "", fId.ToString, as400usr, Nothing, Nothing, Nothing)
                                    If bDoDeleteBoth Then Success = DocmngServiceAuthentication("DELETE", "", "", BranchCode, rNmaeL, FolderPath, "", Subject, "", "", "", "", "", "", fId.ToString, as400usr, Nothing, Nothing, Nothing)

                                Else
                                    Dim Success As String = DocmngServiceAuthentication("DELETE", "", "", BranchCode, rName, FolderPath, "", Subject, "", "", "", "", "", "", fId.ToString, as400usr, Nothing, Nothing, Nothing)
                                    If bDoDeleteBoth Then Success = DocmngServiceAuthentication("DELETE", "", "", BranchCode, rNmaeL, FolderPath, "", Subject, "", "", "", "", "", "", fId.ToString, as400usr, Nothing, Nothing, Nothing)

                                End If
                            Catch ex As Exception
                            End Try
                        End If

                    Next

                End If

            End If
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.DOCMNG_ERROR.ToString, "DeleteFile", "ex=" & ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

        End Try
    End Sub

    Public Shared Function ConnectFiles(Subject As String, QuotationNo As String, TargetSubject As String, Owner As String, TargetBranchCode As String, AdditionalData As String, FolderArr As String) As Boolean
        Try


            If IsNumeric(QuotationNo) Then
                QuotationNo = Format(CInt(QuotationNo), "0000000")
            End If

            If ConfigurationManager.AppSettings("DocMngService").ToString.Trim = "CLOUD" Then
                Dim Success As String = DocmngServiceAuthentication("MOVEFILES", FolderArr, "", "", "", "", "", Subject, QuotationNo, TargetSubject, Owner, TargetBranchCode, AdditionalData, "", "", "", Nothing, Nothing, Nothing)
                Return CBool(Success)
            Else
                Dim Success As String = DocmngServiceAuthentication("MOVEFILES", FolderArr, "", "", "", "", "", Subject, QuotationNo, TargetSubject, Owner, TargetBranchCode, AdditionalData, "", "", "", Nothing, Nothing, Nothing)
                Return CBool(Success)
            End If

        Catch ex As Exception

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.DOCMNG_ERROR.ToString, "ConnectFiles", "ex=" & ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
            Return False
        End Try
    End Function

    Public Shared Function GetDocumentsList(BranchCode As String, FolderPath As String, StorageFolder As String) As DataTable
        Try
            If ConfigurationManager.AppSettings("DOCMNG_ACTIVE") = "YES" Then
                Dim dt As DataTable = Nothing
                Dim ds As New DataSet

                If ConfigurationManager.AppSettings("DocMngService").ToString.Trim = "CLOUD" Then
                    Dim Success As String = DocmngServiceAuthentication("LISTOBJECT", "", "", BranchCode, "", FolderPath, StorageFolder, "", "", "", "", "", "", "", "", "", Nothing, Nothing, ds)
                Else
                    Dim Success As String = DocmngServiceAuthentication("LISTOBJECT", "", "", BranchCode, "", FolderPath, StorageFolder, "", "", "", "", "", "", "", "", "", Nothing, Nothing, ds)
                End If

                If Not ds Is Nothing AndAlso ds.Tables.Count > 0 Then
                    dt = ds.Tables(0)
                End If

                ds = Nothing

                Return dt

            End If
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.DOCMNG_ERROR.ToString, "GetDocumentsList", "ex=" & ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
        End Try

    End Function

    Public Shared Function GetFile(ByVal BranchCode As String, ByVal Subject As String, ByVal FilePath As String, ByVal FileName As String) As Byte()
        Try
            If ConfigurationManager.AppSettings("DOCMNG_ACTIVE") = "YES" Then

                Dim b() As Byte = Nothing
                Dim Success As String = ""
                If ConfigurationManager.AppSettings("DocMngService").ToString.Trim = "CLOUD" Then
                    Success = DocmngServiceAuthentication("DOWNLOAD", FilePath, Subject, BranchCode, FileName, "", "", "", "", "", "", "", "", "", "", "", Nothing, b, Nothing)
                Else
                    Success = DocmngServiceAuthentication("DOWNLOAD", FilePath, Subject, BranchCode, FileName, "", "", "", "", "", "", "", "", "", "", "", Nothing, b, Nothing)
                End If
                If Success <> "FALSE" AndAlso Not b Is Nothing Then
                    Return b
                Else
                    Return Nothing
                End If
            End If
        Catch ex As Exception

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.DOCMNG_ERROR.ToString, "GetFile", "ex=" & ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

        End Try
    End Function

    Public Shared Function IsFileExist(ByVal FolderPath As String, ByVal FileName As String, ByVal BranchCode As String, Subject As String) As Integer
        Try
            If ConfigurationManager.AppSettings("DOCMNG_ACTIVE") = "YES" Then

                If ConfigurationManager.AppSettings("DocMngService").ToString.Trim = "CLOUD" Then
                    Dim Success As String = DocmngServiceAuthentication("EXIST", "", "", BranchCode, FileName, FolderPath, "", Subject, "", "", "", "", "", "", "", "", Nothing, Nothing, Nothing)
                Else
                    Dim Success As String = DocmngServiceAuthentication("EXIST", "", "", BranchCode, FileName, FolderPath, "", Subject, "", "", "", "", "", "", "", "", Nothing, Nothing, Nothing)
                End If
            End If

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.DOCMNG_ERROR.ToString, "IsFileExist", "ex=" & ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

        End Try
    End Function

    Public Shared Function IsAllowedMimeType(mimeType As String) As Boolean
        ' Define the allowed MIME types (adjust as needed)
        Dim allowedMimeTypes As String() = {"application/vnd.openxmlformats-officedocument.wordprocessingml.doc--ument", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "image/jpeg", "image/png", "application/pdf", "application/vnd.ms-excel", "application/msword", "text/plain", "application/mspowerpoint", "image/gif", "image/bmp", "image/vnd.dxf", "application/step", "application/stp."}
        ' Dim allowedMimeTypes As String() = {"application/vnd.openxmlformats-officedocument.wordprocessingml.doc--ument", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "image/jpeg", "image/png", "application/pdf", "application/vnd.ms-excel", "application/msword", "text/plain", "application/mspowerpoint", "image/gif", "image/bmp", "image/tiff", "application/acad", "application/octet-stream"}

        ' Check if the provided MIME type is in the allowed list
        Return allowedMimeTypes.Contains(mimeType)

    End Function
    Public Shared Function IsAllowedMimeTypeS(mimeType As String) As Boolean
        ' Define the allowed MIME types (adjust as needed)
        Dim allowedMimeTypes As String() = {"image/jpeg", "image/png", "application/pdf"}

        ' Check if the provided MIME type is in the allowed list
        Return allowedMimeTypes.Contains(mimeType)


    End Function

    Public Shared Function GetMimeType(ByVal FileName As String) As String
        Try

            Dim ext As String = System.IO.Path.GetExtension(FileName)
            ext = UCase(Right(ext, Len(ext) - 1))
            Dim Mime As String

            Select Case ext
                Case "XLS", "XLSX"
                    Mime = "application/vnd.ms-excel"
                Case "DOC", "DOCX"
                    Mime = "application/msword"
                Case "PDF"
                    Mime = "application/pdf"
                Case "TXT"
                    Mime = "text/plain"
                Case "PPT", "PPS"
                    Mime = "application/mspowerpoint"
                Case "GIF"
                    Mime = "image/gif"
                Case "JPG", "JPEG"
                    Mime = "image/jpeg"
                Case "BMP"
                    Mime = "image/bmp"
                Case "PNG"
                    Mime = "image/png"
                Case "TIF", "TIFF"
                    Mime = "image/tiff"
                Case "DWG"
                    Mime = "application/acad"
                Case Else
                    Mime = "application/octet-stream"





                    '    case ".3dm": mime="x-world/x-3dmf"
                    'Case ".3dmf" : Mime = "x-world/x-3dmf"
                    'Case ".a" : Mime = "application/octet-stream"
                    'Case ".aab" : Mime = "application/x-authorware-bin"
                    'Case ".aam" : Mime = "application/x-authorware-map"
                    'Case ".aas" : Mime = "application/x-authorware-seg"
                    'Case ".abc" : Mime = "text/vnd.abc"
                    'Case ".acgi" : Mime = "text/html"
                    'Case ".afl" : Mime = "video/animaflex"
                    'Case ".ai" : Mime = "application/postscript"
                    'Case ".aif" : Mime = "audio/aiff"
                    'Case ".aif" : Mime = "audio/x-aiff"
                    'Case ".aifc" : Mime = "audio/aiff"
                    'Case ".aifc" : Mime = "audio/x-aiff"
                    'Case ".aiff" : Mime = "audio/aiff"
                    'Case ".aiff" : Mime = "audio/x-aiff"
                    'Case ".aim" : Mime = "application/x-aim"
                    'Case ".aip" : Mime = "text/x-audiosoft-intra"
                    'Case ".ani" : Mime = "application/x-navi-animation"
                    'Case ".aos" : Mime = "application/x-nokia-9000-communicator-add-on-software"
                    'Case ".aps" : Mime = "application/mime"
                    'Case ".arc" : Mime = "application/octet-stream"
                    'Case ".arj" : Mime = "application/arj"
                    'Case ".arj" : Mime = "application/octet-stream"
                    'Case ".art" : Mime = "image/x-jg"
                    'Case ".asf" : Mime = "video/x-ms-asf"
                    'Case ".asm" : Mime = "text/x-asm"
                    'Case ".asp" : Mime = "text/asp"
                    'Case ".asx" : Mime = "application/x-mplayer2"
                    'Case ".asx" : Mime = "video/x-ms-asf"
                    'Case ".asx" : Mime = "video/x-ms-asf-plugin"
                    'Case ".au" : Mime = "audio/basic"
                    'Case ".au" : Mime = "audio/x-au"
                    'Case ".avi" : Mime = "application/x-troff-msvideo"
                    'Case ".avi" : Mime = "video/avi"
                    'Case ".avi" : Mime = "video/msvideo"
                    'Case ".avi" : Mime = "video/x-msvideo"
                    'Case ".avs" : Mime = "video/avs-video"
                    'Case ".bcpio" : Mime = "application/x-bcpio"
                    'Case ".bin" : Mime = "application/mac-binary"
                    'Case ".bin" : Mime = "application/macbinary"
                    'Case ".bin" : Mime = "application/octet-stream"
                    'Case ".bin" : Mime = "application/x-binary"
                    'Case ".bin" : Mime = "application/x-macbinary"
                    'Case ".bm" : Mime = "image/bmp"
                    'Case ".bmp" : Mime = "image/bmp"
                    'Case ".bmp" : Mime = "image/x-windows-bmp"
                    'Case ".boo" : Mime = "application/book"
                    'Case ".book" : Mime = "application/book"
                    'Case ".boz" : Mime = "application/x-bzip2"
                    'Case ".bsh" : Mime = "application/x-bsh"
                    'Case ".bz" : Mime = "application/x-bzip"
                    'Case ".bz2" : Mime = "application/x-bzip2"
                    'Case ".c" : Mime = "text/plain"
                    'Case ".c" : Mime = "text/x-c"
                    'Case ".c++" : Mime = "text/plain"
                    'Case ".cat" : Mime = "application/vnd.ms-pki.seccat"
                    'Case ".cc" : Mime = "text/plain"
                    'Case ".cc" : Mime = "text/x-c"
                    'Case ".ccad" : Mime = "application/clariscad"
                    'Case ".cco" : Mime = "application/x-cocoa"
                    'Case ".cdf" : Mime = "application/cdf"
                    'Case ".cdf" : Mime = "application/x-cdf"
                    'Case ".cdf" : Mime = "application/x-netcdf"
                    'Case ".cer" : Mime = "application/pkix-cert"
                    'Case ".cer" : Mime = "application/x-x509-ca-cert"
                    'Case ".cha" : Mime = "application/x-chat"
                    'Case ".chat" : Mime = "application/x-chat"
                    'Case ".class" : Mime = "application/java"
                    'Case ".class" : Mime = "application/java-byte-code"
                    'Case ".class" : Mime = "application/x-java-class"
                    'Case ".com" : Mime = "application/octet-stream"
                    'Case ".com" : Mime = "text/plain"
                    'Case ".conf" : Mime = "text/plain"
                    'Case ".cpio" : Mime = "application/x-cpio"
                    'Case ".cpp" : Mime = "text/x-c"
                    'Case ".cpt" : Mime = "application/mac-compactpro"
                    'Case ".cpt" : Mime = "application/x-compactpro"
                    'Case ".cpt" : Mime = "application/x-cpt"
                    'Case ".crl" : Mime = "application/pkcs-crl"
                    'Case ".crl" : Mime = "application/pkix-crl"
                    'Case ".crt" : Mime = "application/pkix-cert"
                    'Case ".crt" : Mime = "application/x-x509-ca-cert"
                    'Case ".crt" : Mime = "application/x-x509-user-cert"
                    'Case ".csh" : Mime = "application/x-csh"
                    'Case ".csh" : Mime = "text/x-script.csh"
                    'Case ".css" : Mime = "application/x-pointplus"
                    'Case ".css" : Mime = "text/css"
                    'Case ".cxx" : Mime = "text/plain"
                    'Case ".dcr" : Mime = "application/x-director"
                    'Case ".deepv" : Mime = "application/x-deepv"
                    'Case ".def" : Mime = "text/plain"
                    'Case ".der" : Mime = "application/x-x509-ca-cert"
                    'Case ".dif" : Mime = "video/x-dv"
                    'Case ".dir" : Mime = "application/x-director"
                    'Case ".dl" : Mime = "video/dl"
                    'Case ".dl" : Mime = "video/x-dl"
                    'Case ".doc" : Mime = "application/msword"
                    'Case ".dot" : Mime = "application/msword"
                    'Case ".dp" : Mime = "application/commonground"
                    'Case ".drw" : Mime = "application/drafting"
                    'Case ".dump" : Mime = "application/octet-stream"
                    'Case ".dv" : Mime = "video/x-dv"
                    'Case ".dvi" : Mime = "application/x-dvi"
                    'Case ".dwf" : Mime = "drawing/x-dwf (old)"
                    'Case ".dwf" : Mime = "model/vnd.dwf"
                    'Case ".dwg" : Mime = "application/acad"
                    'Case ".dwg" : Mime = "image/vnd.dwg"
                    'Case ".dwg" : Mime = "image/x-dwg"
                    'Case ".dxf" : Mime = "application/dxf"
                    'Case ".dxf" : Mime = "image/vnd.dwg"
                    'Case ".dxf" : Mime = "image/x-dwg"
                    'Case ".dxr" : Mime = "application/x-director"
                    'Case ".el" : Mime = "text/x-script.elisp"
                    'Case ".elc" : Mime = "application/x-bytecode.elisp (compiled elisp)"
                    'Case ".elc" : Mime = "application/x-elc"
                    'Case ".env" : Mime = "application/x-envoy"
                    'Case ".eps" : Mime = "application/postscript"
                    'Case ".es" : Mime = "application/x-esrehber"
                    'Case ".etx" : Mime = "text/x-setext"
                    'Case ".evy" : Mime = "application/envoy"
                    'Case ".evy" : Mime = "application/x-envoy"
                    'Case ".exe" : Mime = "application/octet-stream"
                    'Case ".f" : Mime = "text/plain"
                    'Case ".f" : Mime = "text/x-fortran"
                    'Case ".f77" : Mime = "text/x-fortran"
                    'Case ".f90" : Mime = "text/plain"
                    'Case ".f90" : Mime = "text/x-fortran"
                    'Case ".fdf" : Mime = "application/vnd.fdf"
                    'Case ".fif" : Mime = "application/fractals"
                    'Case ".fif" : Mime = "image/fif"
                    'Case ".fli" : Mime = "video/fli"
                    'Case ".fli" : Mime = "video/x-fli"
                    'Case ".flo" : Mime = "image/florian"
                    'Case ".flx" : Mime = "text/vnd.fmi.flexstor"
                    'Case ".fmf" : Mime = "video/x-atomic3d-feature"
                    'Case ".for" : Mime = "text/plain"
                    'Case ".for" : Mime = "text/x-fortran"
                    'Case ".fpx" : Mime = "image/vnd.fpx"
                    'Case ".fpx" : Mime = "image/vnd.net-fpx"
                    'Case ".frl" : Mime = "application/freeloader"
                    'Case ".funk" : Mime = "audio/make"
                    'Case ".g" : Mime = "text/plain"
                    'Case ".g3" : Mime = "image/g3fax"
                    'Case ".gif" : Mime = "image/gif"
                    'Case ".gl" : Mime = "video/gl"
                    'Case ".gl" : Mime = "video/x-gl"
                    'Case ".gsd" : Mime = "audio/x-gsm"
                    'Case ".gsm" : Mime = "audio/x-gsm"
                    'Case ".gsp" : Mime = "application/x-gsp"
                    'Case ".gss" : Mime = "application/x-gss"
                    'Case ".gtar" : Mime = "application/x-gtar"
                    'Case ".gz" : Mime = "application/x-compressed"
                    'Case ".gz" : Mime = "application/x-gzip"
                    'Case ".gzip" : Mime = "application/x-gzip"
                    'Case ".gzip" : Mime = "multipart/x-gzip"
                    'Case ".h" : Mime = "text/plain"
                    'Case ".h" : Mime = "text/x-h"
                    'Case ".hdf" : Mime = "application/x-hdf"
                    'Case ".help" : Mime = "application/x-helpfile"
                    'Case ".hgl" : Mime = "application/vnd.hp-hpgl"
                    'Case ".hh" : Mime = "text/plain"
                    'Case ".hh" : Mime = "text/x-h"
                    'Case ".hlb" : Mime = "text/x-script"
                    'Case ".hlp" : Mime = "application/hlp"
                    'Case ".hlp" : Mime = "application/x-helpfile"
                    'Case ".hlp" : Mime = "application/x-winhelp"
                    'Case ".hpg" : Mime = "application/vnd.hp-hpgl"
                    'Case ".hpgl" : Mime = "application/vnd.hp-hpgl"
                    'Case ".hqx" : Mime = "application/binhex"
                    'Case ".hqx" : Mime = "application/binhex4"
                    'Case ".hqx" : Mime = "application/mac-binhex"
                    'Case ".hqx" : Mime = "application/mac-binhex40"
                    'Case ".hqx" : Mime = "application/x-binhex40"
                    'Case ".hqx" : Mime = "application/x-mac-binhex40"
                    'Case ".hta" : Mime = "application/hta"
                    'Case ".htc" : Mime = "text/x-component"
                    'Case ".htm" : Mime = "text/html"
                    'Case ".html" : Mime = "text/html"
                    'Case ".htmls" : Mime = "text/html"
                    'Case ".htt" : Mime = "text/webviewhtml"
                    'Case ".htx" : Mime = "text/html"
                    'Case ".ice" : Mime = "x-conference/x-cooltalk"
                    'Case ".ico" : Mime = "image/x-icon"
                    'Case ".idc" : Mime = "text/plain"
                    'Case ".ief" : Mime = "image/ief"
                    'Case ".iefs" : Mime = "image/ief"
                    'Case ".iges" : Mime = "application/iges"
                    'Case ".iges" : Mime = "model/iges"
                    'Case ".igs" : Mime = "application/iges"
                    'Case ".igs" : Mime = "model/iges"
                    'Case ".ima" : Mime = "application/x-ima"
                    'Case ".imap" : Mime = "application/x-httpd-imap"
                    'Case ".inf" : Mime = "application/inf"
                    'Case ".ins" : Mime = "application/x-internett-signup"
                    'Case ".ip" : Mime = "application/x-ip2"
                    'Case ".isu" : Mime = "video/x-isvideo"
                    'Case ".it" : Mime = "audio/it"
                    'Case ".iv" : Mime = "application/x-inventor"
                    'Case ".ivr" : Mime = "i-world/i-vrml"
                    'Case ".ivy" : Mime = "application/x-livescreen"
                    'Case ".jam" : Mime = "audio/x-jam"
                    'Case ".jav" : Mime = "text/plain"
                    'Case ".jav" : Mime = "text/x-java-source"
                    'Case ".java" : Mime = "text/plain"
                    'Case ".java" : Mime = "text/x-java-source"
                    'Case ".jcm" : Mime = "application/x-java-commerce"
                    'Case ".jfif" : Mime = "image/jpeg"
                    'Case ".jfif" : Mime = "image/pjpeg"
                    'Case ".jfif-tbnl" : Mime = "image/jpeg"
                    'Case ".jpe" : Mime = "image/jpeg"
                    'Case ".jpe" : Mime = "image/pjpeg"
                    'Case ".jpeg" : Mime = "image/jpeg"
                    'Case ".jpeg" : Mime = "image/pjpeg"
                    'Case ".jpg" : Mime = "image/jpeg"
                    'Case ".jpg" : Mime = "image/pjpeg"
                    'Case ".jps" : Mime = "image/x-jps"
                    'Case ".js" : Mime = "application/x-javascript"
                    'Case ".js" : Mime = "application/javascript"
                    'Case ".js" : Mime = "application/ecmascript"
                    'Case ".js" : Mime = "text/javascript"
                    'Case ".js" : Mime = "text/ecmascript"
                    'Case ".jut" : Mime = "image/jutvision"
                    'Case ".kar" : Mime = "audio/midi"
                    'Case ".kar" : Mime = "music/x-karaoke"
                    'Case ".ksh" : Mime = "application/x-ksh"
                    'Case ".ksh" : Mime = "text/x-script.ksh"
                    'Case ".la" : Mime = "audio/nspaudio"
                    'Case ".la" : Mime = "audio/x-nspaudio"
                    'Case ".lam" : Mime = "audio/x-liveaudio"
                    'Case ".latex" : Mime = "application/x-latex"
                    'Case ".lha" : Mime = "application/lha"
                    'Case ".lha" : Mime = "application/octet-stream"
                    'Case ".lha" : Mime = "application/x-lha"
                    'Case ".lhx" : Mime = "application/octet-stream"
                    'Case ".list" : Mime = "text/plain"
                    'Case ".lma" : Mime = "audio/nspaudio"
                    'Case ".lma" : Mime = "audio/x-nspaudio"
                    'Case ".log" : Mime = "text/plain"
                    'Case ".lsp" : Mime = "application/x-lisp"
                    'Case ".lsp" : Mime = "text/x-script.lisp"
                    'Case ".lst" : Mime = "text/plain"
                    'Case ".lsx" : Mime = "text/x-la-asf"
                    'Case ".ltx" : Mime = "application/x-latex"
                    'Case ".lzh" : Mime = "application/octet-stream"
                    'Case ".lzh" : Mime = "application/x-lzh"
                    'Case ".lzx" : Mime = "application/lzx"
                    'Case ".lzx" : Mime = "application/octet-stream"
                    'Case ".lzx" : Mime = "application/x-lzx"
                    'Case ".m" : Mime = "text/plain"
                    'Case ".m" : Mime = "text/x-m"
                    'Case ".m1v" : Mime = "video/mpeg"
                    'Case ".m2a" : Mime = "audio/mpeg"
                    'Case ".m2v" : Mime = "video/mpeg"
                    'Case ".m3u" : Mime = "audio/x-mpequrl"
                    'Case ".man" : Mime = "application/x-troff-man"
                    'Case ".map" : Mime = "application/x-navimap"
                    'Case ".mar" : Mime = "text/plain"
                    'Case ".mbd" : Mime = "application/mbedlet"
                    'Case ".mc$" : Mime = "application/x-magic-cap-package-1.0"
                    'Case ".mcd" : Mime = "application/mcad"
                    'Case ".mcd" : Mime = "application/x-mathcad"
                    'Case ".mcf" : Mime = "image/vasa"
                    'Case ".mcf" : Mime = "text/mcf"
                    'Case ".mcp" : Mime = "application/netmc"
                    'Case ".me" : Mime = "application/x-troff-me"
                    'Case ".mht" : Mime = "message/rfc822"
                    'Case ".mhtml" : Mime = "message/rfc822"
                    'Case ".mid" : Mime = "application/x-midi"
                    'Case ".mid" : Mime = "audio/midi"
                    'Case ".mid" : Mime = "audio/x-mid"
                    'Case ".mid" : Mime = "audio/x-midi"
                    'Case ".mid" : Mime = "music/crescendo"
                    'Case ".mid" : Mime = "x-music/x-midi"
                    'Case ".midi" : Mime = "application/x-midi"
                    'Case ".midi" : Mime = "audio/midi"
                    'Case ".midi" : Mime = "audio/x-mid"
                    'Case ".midi" : Mime = "audio/x-midi"
                    'Case ".midi" : Mime = "music/crescendo"
                    'Case ".midi" : Mime = "x-music/x-midi"
                    'Case ".mif" : Mime = "application/x-frame"
                    'Case ".mif" : Mime = "application/x-mif"
                    'Case ".mime" : Mime = "message/rfc822"
                    'Case ".mime" : Mime = "www/mime"
                    'Case ".mjf" : Mime = "audio/x-vnd.audioexplosion.mjuicemediafile"
                    'Case ".mjpg" : Mime = "video/x-motion-jpeg"
                    'Case ".mm" : Mime = "application/base64"
                    'Case ".mm" : Mime = "application/x-meme"
                    'Case ".mme" : Mime = "application/base64"
                    'Case ".mod" : Mime = "audio/mod"
                    'Case ".mod" : Mime = "audio/x-mod"
                    'Case ".moov" : Mime = "video/quicktime"
                    'Case ".mov" : Mime = "video/quicktime"
                    'Case ".movie" : Mime = "video/x-sgi-movie"
                    'Case ".mp2" : Mime = "audio/mpeg"
                    'Case ".mp2" : Mime = "audio/x-mpeg"
                    'Case ".mp2" : Mime = "video/mpeg"
                    'Case ".mp2" : Mime = "video/x-mpeg"
                    'Case ".mp2" : Mime = "video/x-mpeq2a"
                    'Case ".mp3" : Mime = "audio/mpeg3"
                    'Case ".mp3" : Mime = "audio/x-mpeg-3"
                    'Case ".mp3" : Mime = "video/mpeg"
                    'Case ".mp3" : Mime = "video/x-mpeg"
                    'Case ".mpa" : Mime = "audio/mpeg"
                    'Case ".mpa" : Mime = "video/mpeg"
                    'Case ".mpc" : Mime = "application/x-project"
                    'Case ".mpe" : Mime = "video/mpeg"
                    'Case ".mpeg" : Mime = "video/mpeg"
                    'Case ".mpg" : Mime = "audio/mpeg"
                    'Case ".mpg" : Mime = "video/mpeg"
                    'Case ".mpga" : Mime = "audio/mpeg"
                    'Case ".mpp" : Mime = "application/vnd.ms-project"
                    'Case ".mpt" : Mime = "application/x-project"
                    'Case ".mpv" : Mime = "application/x-project"
                    'Case ".mpx" : Mime = "application/x-project"
                    'Case ".mrc" : Mime = "application/marc"
                    'Case ".ms" : Mime = "application/x-troff-ms"
                    'Case ".mv" : Mime = "video/x-sgi-movie"
                    'Case ".my" : Mime = "audio/make"
                    'Case ".mzz" : Mime = "application/x-vnd.audioexplosion.mzz"
                    'Case ".nap" : Mime = "image/naplps"
                    'Case ".naplps" : Mime = "image/naplps"
                    'Case ".nc" : Mime = "application/x-netcdf"
                    'Case ".ncm" : Mime = "application/vnd.nokia.configuration-message"
                    'Case ".nif" : Mime = "image/x-niff"
                    'Case ".niff" : Mime = "image/x-niff"
                    'Case ".nix" : Mime = "application/x-mix-transfer"
                    'Case ".nsc" : Mime = "application/x-conference"
                    'Case ".nvd" : Mime = "application/x-navidoc"
                    'Case ".o" : Mime = "application/octet-stream"
                    'Case ".oda" : Mime = "application/oda"
                    'Case ".omc" : Mime = "application/x-omc"
                    'Case ".omcd" : Mime = "application/x-omcdatamaker"
                    'Case ".omcr" : Mime = "application/x-omcregerator"
                    'Case ".p" : Mime = "text/x-pascal"
                    'Case ".p10" : Mime = "application/pkcs10"
                    'Case ".p10" : Mime = "application/x-pkcs10"
                    'Case ".p12" : Mime = "application/pkcs-12"
                    'Case ".p12" : Mime = "application/x-pkcs12"
                    'Case ".p7a" : Mime = "application/x-pkcs7-signature"
                    'Case ".p7c" : Mime = "application/pkcs7-mime"
                    'Case ".p7c" : Mime = "application/x-pkcs7-mime"
                    'Case ".p7m" : Mime = "application/pkcs7-mime"
                    'Case ".p7m" : Mime = "application/x-pkcs7-mime"
                    'Case ".p7r" : Mime = "application/x-pkcs7-certreqresp"
                    'Case ".p7s" : Mime = "application/pkcs7-signature"
                    'Case ".part" : Mime = "application/pro_eng"
                    'Case ".pas" : Mime = "text/pascal"
                    'Case ".pbm" : Mime = "image/x-portable-bitmap"
                    'Case ".pcl" : Mime = "application/vnd.hp-pcl"
                    'Case ".pcl" : Mime = "application/x-pcl"
                    'Case ".pct" : Mime = "image/x-pict"
                    'Case ".pcx" : Mime = "image/x-pcx"
                    'Case ".pdb" : Mime = "chemical/x-pdb"
                    'Case ".pdf" : Mime = "application/pdf"
                    'Case ".pfunk" : Mime = "audio/make"
                    'Case ".pfunk" : Mime = "audio/make.my.funk"
                    'Case ".pgm" : Mime = "image/x-portable-graymap"
                    'Case ".pgm" : Mime = "image/x-portable-greymap"
                    'Case ".pic" : Mime = "image/pict"
                    'Case ".pict" : Mime = "image/pict"
                    'Case ".pkg" : Mime = "application/x-newton-compatible-pkg"
                    'Case ".pko" : Mime = "application/vnd.ms-pki.pko"
                    'Case ".pl" : Mime = "text/plain"
                    'Case ".pl" : Mime = "text/x-script.perl"
                    'Case ".plx" : Mime = "application/x-pixclscript"
                    'Case ".pm" : Mime = "image/x-xpixmap"
                    'Case ".pm" : Mime = "text/x-script.perl-module"
                    'Case ".pm4" : Mime = "application/x-pagemaker"
                    'Case ".pm5" : Mime = "application/x-pagemaker"
                    'Case ".png" : Mime = "image/png"
                    'Case ".pnm" : Mime = "application/x-portable-anymap"
                    'Case ".pnm" : Mime = "image/x-portable-anymap"
                    'Case ".pot" : Mime = "application/mspowerpoint"
                    'Case ".pot" : Mime = "application/vnd.ms-powerpoint"
                    'Case ".pov" : Mime = "model/x-pov"
                    'Case ".ppa" : Mime = "application/vnd.ms-powerpoint"
                    'Case ".ppm" : Mime = "image/x-portable-pixmap"
                    'Case ".pps" : Mime = "application/mspowerpoint"
                    'Case ".pps" : Mime = "application/vnd.ms-powerpoint"
                    'Case ".ppt" : Mime = "application/mspowerpoint"
                    'Case ".ppt" : Mime = "application/powerpoint"
                    'Case ".ppt" : Mime = "application/vnd.ms-powerpoint"
                    'Case ".ppt" : Mime = "application/x-mspowerpoint"
                    'Case ".ppz" : Mime = "application/mspowerpoint"
                    'Case ".pre" : Mime = "application/x-freelance"
                    'Case ".prt" : Mime = "application/pro_eng"
                    'Case ".ps" : Mime = "application/postscript"
                    'Case ".psd" : Mime = "application/octet-stream"
                    'Case ".pvu" : Mime = "paleovu/x-pv"
                    'Case ".pwz" : Mime = "application/vnd.ms-powerpoint"
                    'Case ".py" : Mime = "text/x-script.phyton"
                    'Case ".pyc" : Mime = "applicaiton/x-bytecode.python"
                    'Case ".qcp" : Mime = "audio/vnd.qcelp"
                    'Case ".qd3" : Mime = "x-world/x-3dmf"
                    'Case ".qd3d" : Mime = "x-world/x-3dmf"
                    'Case ".qif" : Mime = "image/x-quicktime"
                    'Case ".qt" : Mime = "video/quicktime"
                    'Case ".qtc" : Mime = "video/x-qtc"
                    'Case ".qti" : Mime = "image/x-quicktime"
                    'Case ".qtif" : Mime = "image/x-quicktime"
                    'Case ".ra" : Mime = "audio/x-pn-realaudio"
                    'Case ".ra" : Mime = "audio/x-pn-realaudio-plugin"
                    'Case ".ra" : Mime = "audio/x-realaudio"
                    'Case ".ram" : Mime = "audio/x-pn-realaudio"
                    'Case ".ras" : Mime = "application/x-cmu-raster"
                    'Case ".ras" : Mime = "image/cmu-raster"
                    'Case ".ras" : Mime = "image/x-cmu-raster"
                    'Case ".rast" : Mime = "image/cmu-raster"
                    'Case ".rexx" : Mime = "text/x-script.rexx"
                    'Case ".rf" : Mime = "image/vnd.rn-realflash"
                    'Case ".rgb" : Mime = "image/x-rgb"
                    'Case ".rm" : Mime = "application/vnd.rn-realmedia"
                    'Case ".rm" : Mime = "audio/x-pn-realaudio"
                    'Case ".rmi" : Mime = "audio/mid"
                    'Case ".rmm" : Mime = "audio/x-pn-realaudio"
                    'Case ".rmp" : Mime = "audio/x-pn-realaudio"
                    'Case ".rmp" : Mime = "audio/x-pn-realaudio-plugin"
                    'Case ".rng" : Mime = "application/ringing-tones"
                    'Case ".rng" : Mime = "application/vnd.nokia.ringing-tone"
                    'Case ".rnx" : Mime = "application/vnd.rn-realplayer"
                    'Case ".roff" : Mime = "application/x-troff"
                    'Case ".rp" : Mime = "image/vnd.rn-realpix"
                    'Case ".rpm" : Mime = "audio/x-pn-realaudio-plugin"
                    'Case ".rt" : Mime = "text/richtext"
                    'Case ".rt" : Mime = "text/vnd.rn-realtext"
                    'Case ".rtf" : Mime = "application/rtf"
                    'Case ".rtf" : Mime = "application/x-rtf"
                    'Case ".rtf" : Mime = "text/richtext"
                    'Case ".rtx" : Mime = "application/rtf"
                    'Case ".rtx" : Mime = "text/richtext"
                    'Case ".rv" : Mime = "video/vnd.rn-realvideo"
                    'Case ".s" : Mime = "text/x-asm"
                    'Case ".s3m" : Mime = "audio/s3m"
                    'Case ".saveme" : Mime = "application/octet-stream"
                    'Case ".sbk" : Mime = "application/x-tbook"
                    'Case ".scm" : Mime = "application/x-lotusscreencam"
                    'Case ".scm" : Mime = "text/x-script.guile"
                    'Case ".scm" : Mime = "text/x-script.scheme"
                    'Case ".scm" : Mime = "video/x-scm"
                    'Case ".sdml" : Mime = "text/plain"
                    'Case ".sdp" : Mime = "application/sdp"
                    'Case ".sdp" : Mime = "application/x-sdp"
                    'Case ".sdr" : Mime = "application/sounder"
                    'Case ".sea" : Mime = "application/sea"
                    'Case ".sea" : Mime = "application/x-sea"
                    'Case ".set" : Mime = "application/set"
                    'Case ".sgm" : Mime = "text/sgml"
                    'Case ".sgm" : Mime = "text/x-sgml"
                    'Case ".sgml" : Mime = "text/sgml"
                    'Case ".sgml" : Mime = "text/x-sgml"
                    'Case ".sh" : Mime = "application/x-bsh"
                    'Case ".sh" : Mime = "application/x-sh"
                    'Case ".sh" : Mime = "application/x-shar"
                    'Case ".sh" : Mime = "text/x-script.sh"
                    'Case ".shar" : Mime = "application/x-bsh"
                    'Case ".shar" : Mime = "application/x-shar"
                    'Case ".shtml" : Mime = "text/html"
                    'Case ".shtml" : Mime = "text/x-server-parsed-html"
                    'Case ".sid" : Mime = "audio/x-psid"
                    'Case ".sit" : Mime = "application/x-sit"
                    'Case ".sit" : Mime = "application/x-stuffit"
                    'Case ".skd" : Mime = "application/x-koan"
                    'Case ".skm" : Mime = "application/x-koan"
                    'Case ".skp" : Mime = "application/x-koan"
                    'Case ".skt" : Mime = "application/x-koan"
                    'Case ".sl" : Mime = "application/x-seelogo"
                    'Case ".smi" : Mime = "application/smil"
                    'Case ".smil" : Mime = "application/smil"
                    'Case ".snd" : Mime = "audio/basic"
                    'Case ".snd" : Mime = "audio/x-adpcm"
                    'Case ".sol" : Mime = "application/solids"
                    'Case ".spc" : Mime = "application/x-pkcs7-certificates"
                    'Case ".spc" : Mime = "text/x-speech"
                    'Case ".spl" : Mime = "application/futuresplash"
                    'Case ".spr" : Mime = "application/x-sprite"
                    'Case ".sprite" : Mime = "application/x-sprite"
                    'Case ".src" : Mime = "application/x-wais-source"
                    'Case ".ssi" : Mime = "text/x-server-parsed-html"
                    'Case ".ssm" : Mime = "application/streamingmedia"
                    'Case ".sst" : Mime = "application/vnd.ms-pki.certstore"
                    'Case ".step" : Mime = "application/step"
                    'Case ".stl" : Mime = "application/sla"
                    'Case ".stl" : Mime = "application/vnd.ms-pki.stl"
                    'Case ".stl" : Mime = "application/x-navistyle"
                    'Case ".stp" : Mime = "application/step"
                    'Case ".sv4cpio" : Mime = "application/x-sv4cpio"
                    'Case ".sv4crc" : Mime = "application/x-sv4crc"
                    'Case ".svf" : Mime = "image/vnd.dwg"
                    'Case ".svf" : Mime = "image/x-dwg"
                    'Case ".svr" : Mime = "application/x-world"
                    'Case ".svr" : Mime = "x-world/x-svr"
                    'Case ".swf" : Mime = "application/x-shockwave-flash"
                    'Case ".t" : Mime = "application/x-troff"
                    'Case ".talk" : Mime = "text/x-speech"
                    'Case ".tar" : Mime = "application/x-tar"
                    'Case ".tbk" : Mime = "application/toolbook"
                    'Case ".tbk" : Mime = "application/x-tbook"
                    'Case ".tcl" : Mime = "application/x-tcl"
                    'Case ".tcl" : Mime = "text/x-script.tcl"
                    'Case ".tcsh" : Mime = "text/x-script.tcsh"
                    'Case ".tex" : Mime = "application/x-tex"
                    'Case ".texi" : Mime = "application/x-texinfo"
                    'Case ".texinfo" : Mime = "application/x-texinfo"
                    'Case ".text" : Mime = "application/plain"
                    'Case ".text" : Mime = "text/plain"
                    'Case ".tgz" : Mime = "application/gnutar"
                    'Case ".tgz" : Mime = "application/x-compressed"
                    'Case ".tif" : Mime = "image/tiff"
                    'Case ".tif" : Mime = "image/x-tiff"
                    'Case ".tiff" : Mime = "image/tiff"
                    'Case ".tiff" : Mime = "image/x-tiff"
                    'Case ".tr" : Mime = "application/x-troff"
                    'Case ".tsi" : Mime = "audio/tsp-audio"
                    'Case ".tsp" : Mime = "application/dsptype"
                    'Case ".tsp" : Mime = "audio/tsplayer"
                    'Case ".tsv" : Mime = "text/tab-separated-values"
                    'Case ".turbot" : Mime = "image/florian"
                    'Case ".txt" : Mime = "text/plain"
                    'Case ".uil" : Mime = "text/x-uil"
                    'Case ".uni" : Mime = "text/uri-list"
                    'Case ".unis" : Mime = "text/uri-list"
                    'Case ".unv" : Mime = "application/i-deas"
                    'Case ".uri" : Mime = "text/uri-list"
                    'Case ".uris" : Mime = "text/uri-list"
                    'Case ".ustar" : Mime = "application/x-ustar"
                    'Case ".ustar" : Mime = "multipart/x-ustar"
                    'Case ".uu" : Mime = "application/octet-stream"
                    'Case ".uu" : Mime = "text/x-uuencode"
                    'Case ".uue" : Mime = "text/x-uuencode"
                    'Case ".vcd" : Mime = "application/x-cdlink"
                    'Case ".vcs" : Mime = "text/x-vcalendar"
                    'Case ".vda" : Mime = "application/vda"
                    'Case ".vdo" : Mime = "video/vdo"
                    'Case ".vew" : Mime = "application/groupwise"
                    'Case ".viv" : Mime = "video/vivo"
                    'Case ".viv" : Mime = "video/vnd.vivo"
                    'Case ".vivo" : Mime = "video/vivo"
                    'Case ".vivo" : Mime = "video/vnd.vivo"
                    'Case ".vmd" : Mime = "application/vocaltec-media-desc"
                    'Case ".vmf" : Mime = "application/vocaltec-media-file"
                    'Case ".voc" : Mime = "audio/voc"
                    'Case ".voc" : Mime = "audio/x-voc"
                    'Case ".vos" : Mime = "video/vosaic"
                    'Case ".vox" : Mime = "audio/voxware"
                    'Case ".vqe" : Mime = "audio/x-twinvq-plugin"
                    'Case ".vqf" : Mime = "audio/x-twinvq"
                    'Case ".vql" : Mime = "audio/x-twinvq-plugin"
                    'Case ".vrml" : Mime = "application/x-vrml"
                    'Case ".vrml" : Mime = "model/vrml"
                    'Case ".vrml" : Mime = "x-world/x-vrml"
                    'Case ".vrt" : Mime = "x-world/x-vrt"
                    'Case ".vsd" : Mime = "application/x-visio"
                    'Case ".vst" : Mime = "application/x-visio"
                    'Case ".vsw" : Mime = "application/x-visio"
                    'Case ".w60" : Mime = "application/wordperfect6.0"
                    'Case ".w61" : Mime = "application/wordperfect6.1"
                    'Case ".w6w" : Mime = "application/msword"
                    'Case ".wav" : Mime = "audio/wav"
                    'Case ".wav" : Mime = "audio/x-wav"
                    'Case ".wb1" : Mime = "application/x-qpro"
                    'Case ".wbmp" : Mime = "image/vnd.wap.wbmp"
                    'Case ".web" : Mime = "application/vnd.xara"
                    'Case ".wiz" : Mime = "application/msword"
                    'Case ".wk1" : Mime = "application/x-123"
                    'Case ".wmf" : Mime = "windows/metafile"
                    'Case ".wml" : Mime = "text/vnd.wap.wml"
                    'Case ".wmlc" : Mime = "application/vnd.wap.wmlc"
                    'Case ".wmls" : Mime = "text/vnd.wap.wmlscript"
                    'Case ".wmlsc" : Mime = "application/vnd.wap.wmlscriptc"
                    'Case ".word" : Mime = "application/msword"
                    'Case ".wp" : Mime = "application/wordperfect"
                    'Case ".wp5" : Mime = "application/wordperfect"
                    'Case ".wp5" : Mime = "application/wordperfect6.0"
                    'Case ".wp6" : Mime = "application/wordperfect"
                    'Case ".wpd" : Mime = "application/wordperfect"
                    'Case ".wpd" : Mime = "application/x-wpwin"
                    'Case ".wq1" : Mime = "application/x-lotus"
                    'Case ".wri" : Mime = "application/mswrite"
                    'Case ".wri" : Mime = "application/x-wri"
                    'Case ".wrl" : Mime = "application/x-world"
                    'Case ".wrl" : Mime = "model/vrml"
                    'Case ".wrl" : Mime = "x-world/x-vrml"
                    'Case ".wrz" : Mime = "model/vrml"
                    'Case ".wrz" : Mime = "x-world/x-vrml"
                    'Case ".wsc" : Mime = "text/scriplet"
                    'Case ".wsrc" : Mime = "application/x-wais-source"
                    'Case ".wtk" : Mime = "application/x-wintalk"
                    'Case ".xbm" : Mime = "image/x-xbitmap"
                    'Case ".xbm" : Mime = "image/x-xbm"
                    'Case ".xbm" : Mime = "image/xbm"
                    'Case ".xdr" : Mime = "video/x-amt-demorun"
                    'Case ".xgz" : Mime = "xgl/drawing"
                    'Case ".xif" : Mime = "image/vnd.xiff"
                    'Case ".xl" : Mime = "application/excel"
                    'Case ".xla" : Mime = "application/excel"
                    'Case ".xla" : Mime = "application/x-excel"
                    'Case ".xla" : Mime = "application/x-msexcel"
                    'Case ".xlb" : Mime = "application/excel"
                    'Case ".xlb" : Mime = "application/vnd.ms-excel"
                    'Case ".xlb" : Mime = "application/x-excel"
                    'Case ".xlc" : Mime = "application/excel"
                    'Case ".xlc" : Mime = "application/vnd.ms-excel"
                    'Case ".xlc" : Mime = "application/x-excel"
                    'Case ".xld" : Mime = "application/excel"
                    'Case ".xld" : Mime = "application/x-excel"
                    'Case ".xlk" : Mime = "application/excel"
                    'Case ".xlk" : Mime = "application/x-excel"
                    'Case ".xll" : Mime = "application/excel"
                    'Case ".xll" : Mime = "application/vnd.ms-excel"
                    'Case ".xll" : Mime = "application/x-excel"
                    'Case ".xlm" : Mime = "application/excel"
                    'Case ".xlm" : Mime = "application/vnd.ms-excel"
                    'Case ".xlm" : Mime = "application/x-excel"
                    'Case ".xls" : Mime = "application/excel"
                    'Case ".xls" : Mime = "application/vnd.ms-excel"
                    'Case ".xls" : Mime = "application/x-excel"
                    'Case ".xls" : Mime = "application/x-msexcel"
                    'Case ".xlt" : Mime = "application/excel"
                    'Case ".xlt" : Mime = "application/x-excel"
                    'Case ".xlv" : Mime = "application/excel"
                    'Case ".xlv" : Mime = "application/x-excel"
                    'Case ".xlw" : Mime = "application/excel"
                    'Case ".xlw" : Mime = "application/vnd.ms-excel"
                    'Case ".xlw" : Mime = "application/x-excel"
                    'Case ".xlw" : Mime = "application/x-msexcel"
                    'Case ".xm" : Mime = "audio/xm"
                    'Case ".xml" : Mime = "application/xml"
                    'Case ".xml" : Mime = "text/xml"
                    'Case ".xmz" : Mime = "xgl/movie"
                    'Case ".xpix" : Mime = "application/x-vnd.ls-xpix"
                    'Case ".xpm" : Mime = "image/x-xpixmap"
                    'Case ".xpm" : Mime = "image/xpm"
                    'Case ".x-png" : Mime = "image/png"
                    'Case ".xsr" : Mime = "video/x-amt-showrun"
                    'Case ".xwd" : Mime = "image/x-xwd"
                    'Case ".xwd" : Mime = "image/x-xwindowdump"
                    'Case ".xyz" : Mime = "chemical/x-pdb"
                    'Case ".z" : Mime = "application/x-compress"
                    'Case ".z" : Mime = "application/x-compressed"
                    'Case ".zip" : Mime = "application/x-compressed"
                    'Case ".zip" : Mime = "application/x-zip-compressed"
                    'Case ".zip" : Mime = "application/zip"
                    'Case ".zip" : Mime = "multipart/x-zip"
                    'Case ".zoo" : Mime = "application/octet-stream"
                    'Case ".zsh" : Mime = "text/x-script.zsh"

            End Select
            Return Mime
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.DOCMNG_ERROR.ToString, "GetMimeType", "ex=" & ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
        End Try


        Return ""

    End Function


    Public Shared Sub OpenDocument(FileFoldePath As String, FileName As String, BranchCode As String, ByRef bToReturn() As Byte, StorageFolder As String)
        Try
            If ConfigurationManager.AppSettings("DOCMNG_ACTIVE") = "YES" Then


                Dim b() As Byte

                b = Documents.GetFile(BranchCode, StorageFolder, FileFoldePath, FileName)
                HttpContext.Current.Response.Clear()
                HttpContext.Current.Response.ContentType = Documents.GetMimeType(FileName)
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" & FileName)
                HttpContext.Current.Response.BinaryWrite(b)

                HttpContext.Current.Response.Flush()
                HttpContext.Current.Response.SuppressContent = True
                HttpContext.Current.ApplicationInstance.CompleteRequest()

            End If
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.DOCMNG_ERROR.ToString, "OpenDocument", "ex=" & ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
        End Try

    End Sub


    Public Shared Sub getDocument(FileFoldePath As String, FileName As String, BranchCode As String, ByRef bToReturn() As Byte, StorageFolder As String)
        Try
            If ConfigurationManager.AppSettings("DOCMNG_ACTIVE") = "YES" Then

                Dim b() As Byte = Documents.GetFile(BranchCode, StorageFolder, FileFoldePath, FileName)
                bToReturn = b

            End If
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.DOCMNG_ERROR.ToString, "getDocument",
          "ex=" & ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
        End Try

    End Sub

    Public Shared Function GetCatiaDrawingName(qutNo As String) As String

        Try

            Dim FileName As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FolderPath, False)
            If FileName <> "" Then
                Dim s() As String = FileName.Split("\")
                Return "DRW_" & s(1) & ".pdf"
            End If
            Return FileName
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.DOCMNG_ERROR.ToString, "GetCatiaDrawingName",
          "ex=" & ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
            Return qutNo

        End Try


    End Function

    Public Shared Function BuildandGetDocumentName() As String

        Dim QuotationNum As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number)

        Try

            Dim FileName As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FolderPath, False)
            If FileName <> "" Then
                Dim s() As String = FileName.Split("\")
                Return "DRW_" & s(1) & ".pdf"
            End If
            Return QuotationNum
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.DOCMNG_ERROR.ToString, "BuildandGetDocumentName",
          "ex=" & ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
            Return QuotationNum

        End Try


    End Function
    Public Shared Function BuildandGetDocumentNamePDFBSON() As String

        Dim QuotationNum As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number)

        Try

            Dim FileName As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FolderPath, False)
            If FileName <> "" Then
                Dim s() As String = FileName.Split("\")
                Return "DRW_BSON_" & s(1) & ".pdf"
            End If
            Return QuotationNum
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.DOCMNG_ERROR.ToString, "BuildandGetDocumentNamePDFBSON",
          "ex=" & ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
            Return QuotationNum

        End Try


    End Function

    Public Shared Sub DeleteCatiaReportsLocaly()
        Try
            Dim sQutNumberDrawing As String = CatiaDrawing.GetCatiaDrawingName(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False))
            Dim sQutNumberAs400Drawing As String = CatiaDrawing.GetCatiaDrawingName(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number, False))
            Dim LocalfolderPath As String = ConfigurationManager.AppSettings("DrawingReports").ToString.Trim & clsBranch.ReturnActiveBranchCodeForDocuments & "\"

            If File.Exists(LocalfolderPath & sQutNumberDrawing) Then
                File.Delete(LocalfolderPath & sQutNumberDrawing)
            End If
            If sQutNumberDrawing <> sQutNumberAs400Drawing Then
                If Not sQutNumberAs400Drawing.Contains("0000000") Then
                    If File.Exists(LocalfolderPath & sQutNumberAs400Drawing) Then
                        File.Delete(LocalfolderPath & sQutNumberAs400Drawing)
                    End If
                End If
            End If



            If clsBranch.ReturnActiveBranchCodeForDocuments <> "ZZ" Then

                LocalfolderPath = ConfigurationManager.AppSettings("DrawingReports").ToString.Trim & "ZZ" & "\"

                If File.Exists(LocalfolderPath & sQutNumberDrawing) Then
                    File.Delete(LocalfolderPath & sQutNumberDrawing)
                End If
                If sQutNumberDrawing <> sQutNumberAs400Drawing Then
                    If Not sQutNumberAs400Drawing.Contains("0000000") Then
                        If File.Exists(LocalfolderPath & sQutNumberAs400Drawing) Then
                            File.Delete(LocalfolderPath & sQutNumberAs400Drawing)
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.DOCMNG_ERROR.ToString, "DeleteCatiaReportsLocaly",
                      "ex=" & ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
        End Try
    End Sub

    Public Shared Sub DeleteCatiaReportsBSONLocaly()
        Try
            Dim sQutNumberDrawingBSON As String = CatiaDrawing.GetCatiaDrawingBSONName(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False))
            Dim sQutNumberAs400DrawingBSON As String = CatiaDrawing.GetCatiaDrawingBSONName(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number, False))
            Dim LocalfolderPathBSON As String = ConfigurationManager.AppSettings("DrawingReports").ToString.Trim & clsBranch.ReturnActiveBranchCodeForDocuments & "\"

            If File.Exists(LocalfolderPathBSON & sQutNumberDrawingBSON) Then
                File.Delete(LocalfolderPathBSON & sQutNumberDrawingBSON)
            End If
            If sQutNumberDrawingBSON <> sQutNumberAs400DrawingBSON Then
                If Not sQutNumberAs400DrawingBSON.Contains("0000000") Then
                    If File.Exists(LocalfolderPathBSON & sQutNumberAs400DrawingBSON) Then
                        File.Delete(LocalfolderPathBSON & sQutNumberAs400DrawingBSON)
                    End If
                End If
            End If

            If clsBranch.ReturnActiveBranchCodeForDocuments <> "ZZ" Then

                LocalfolderPathBSON = ConfigurationManager.AppSettings("DrawingReports").ToString.Trim & "ZZ" & "\"

                If File.Exists(LocalfolderPathBSON & sQutNumberDrawingBSON) Then
                    File.Delete(LocalfolderPathBSON & sQutNumberDrawingBSON)
                End If
                If sQutNumberDrawingBSON <> sQutNumberAs400DrawingBSON Then
                    If Not sQutNumberAs400DrawingBSON.Contains("0000000") Then
                        If File.Exists(LocalfolderPathBSON & sQutNumberAs400DrawingBSON) Then
                            File.Delete(LocalfolderPathBSON & sQutNumberAs400DrawingBSON)
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.DOCMNG_ERROR.ToString, "DeleteCatiaReportsBSONLocaly",
                                  "ex=" & ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
        End Try
    End Sub

    Public Shared Function CHECKandReaname_ReportName(squt As String, squtAs400 As String) As Boolean
        Try
            If squt = squtAs400 Then
                Return True
            Else
                Dim StorePath As String = ConfigurationManager.AppSettings("DrawingReports") & clsBranch.ReturnActiveBranchCodeForDocuments & "\"
                Dim QutFileName As String = StorePath & "DRW_" & squt & ".pdf"
                Dim As400QutFileName As String = StorePath & "DRW_" & squtAs400 & ".pdf"
                Dim sFolderPath As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FolderPath, False)
                If Not (File.Exists(As400QutFileName)) Then
                    Return False
                Else
                    Return True
                End If
                Return False
            End If

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.DOCMNG_ERROR.ToString, "CHECKandReaname_ReportName",
                                              "ex=" & ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
            Return False
        End Try

    End Function

    Public Shared Function DocmngServiceAuthentication(serviseType As String, ByVal FolderArr As String, ByVal sSubject As String, ByVal branchCode As String, ByVal FileName As String, FolderPath As String, StorageFolder As String,
                                                 Subject As String, QuotationNo As String, TargetSubject As String, Owner As String, TargetBranchCode As String, AdditionalData As String, DocMngFolderPath As String,
                                                 FileId As String, UserAS400 As String, File() As Byte,
                                                 ByRef bFileDounloaded() As Byte, ByRef dsListObject As DataSet) As String

        Try

            If ConfigurationManager.AppSettings("DocMngService").ToString.Trim = "CLOUD" Then

                Try

                    Using wsL As New wsDocMngServicesCloud.wsDocMngServices
                        Try
                            Dim GetToken As String = ConfigurationManager.AppSettings("DEV_TOKEN")
                            Dim GetTokenKey As String = ConfigurationManager.AppSettings("DEV_TOKEN_KEY")
                            Dim GetUniqeIDToken As String = Format(Now, "dd/MM/yy H:mm:ss zzz")
                            wsL.Timeout = 6000000

                            Dim header As wsDocMngServicesCloud.ValidationSoapHeader = New wsDocMngServicesCloud.ValidationSoapHeader()
                            header.DevToken = CryptoManagerTDES.Encode(GetToken)
                            header.DevTokenKey = CryptoManagerTDES.Encode(GetTokenKey)
                            header.UniqeIDToken = CryptoManagerTDES.Encode(GetUniqeIDToken)
                            'header.uni
                            wsL.ValidationSoapHeaderValue = header

                        Catch ex As Exception
                            Return "CONNECT_ERR"
                        End Try


                        Select Case serviseType
                            Case "DOWNLOAD" '--------------DOWNLOAD--------------
                                Try
                                    bFileDounloaded = wsL.Download_Document(FolderArr, sSubject, branchCode, FileName)
                                    If bFileDounloaded Is Nothing Then
                                        Return "FALSE"
                                    End If
                                Catch ex As Exception
                                    ' wsL = Nothing
                                    bFileDounloaded = Nothing
                                    Return "FALSE"
                                End Try

                            Case "LISTOBJECT" '--------------LISTOBJECT--------------
                                Try
                                    dsListObject = wsL.GetListObject(FolderPath, StorageFolder, branchCode)
                                    If dsListObject Is Nothing Then
                                        Return "FALSE"
                                    End If
                                Catch ex As Exception
                                    'wsL = Nothing
                                    dsListObject = Nothing
                                    Return "FALSE"
                                End Try

                            Case "MOVEFILES" '--------------MOVE--------------
                                Try
                                    Dim s As String = wsL.MoveFLD_OBJECT(Subject, FolderArr.Substring(0, FolderArr.IndexOf("\")) & "\" & QuotationNo & "\001", TargetSubject, FolderArr, Owner, TargetBranchCode, AdditionalData)
                                    If s.ToString.ToUpper.Contains("ERROR") Then
                                        Return "False"
                                    Else
                                        Return "True"
                                    End If
                                Catch ex As Exception
                                    'wsL = Nothing
                                    Return "False"
                                End Try
                            Case "UPLOAD" '-------------UPLOAD - -------------
                                Try
                                    Dim s As String = wsL.Upload_Document(branchCode, StorageFolder, DocMngFolderPath, FileName, UserAS400, File, True)
                                    If s.ToString.ToUpper.Contains("ERROR") Then
                                        Return "FALSEERROR"
                                    End If
                                    Return s
                                Catch ex As Exception
                                    'wsL = Nothing
                                    Return "FALSE"
                                End Try
                                Return "TRUE"
                            Case "DELETE" '--------------DELETE--------------
                                Try
                                    If Not IsNumeric(FileId) Then FileId = "0"
                                    Dim s As String = wsL.DeleteFile(Subject, branchCode, FolderPath, FileName, CInt(FileId), UserAS400)
                                    If s.ToString.ToUpper.Contains("ERROR") Then
                                        Return "FALSEERROR"
                                    Else
                                        Return "TRUE"
                                    End If
                                Catch ex As Exception
                                    ' wsL = Nothing
                                    Return "FALSE"
                                End Try
                            Case "EXIST"
                                Try
                                    Dim s As String = wsL.IsObjectExist(FolderPath, Subject, FileName, branchCode)
                                    If s.ToString.ToUpper.Contains("ERROR") Then
                                        Return "FALSEERROR"
                                    End If
                                Catch ex As Exception
                                    'wsL = Nothing
                                    Return "FALSE"
                                End Try
                        End Select
                    End Using

                    ' Dim wsL As New wsDocMngServicesCloud.wsDocMngServices


                Catch ex As Exception
                    Return "FALSE"
                End Try
                Return "TRUE"
            ElseIf ConfigurationManager.AppSettings("DocMngService").ToString.Trim = "LOCAL" Then
                Try
                    'Dim wsL As New wsDocMngServices.wsDocMngServices
                    Using wsL As New wsDocMngServices.wsDocMngServices
                        Try
                            Dim GetToken As String = ConfigurationManager.AppSettings("DEV_TOKEN")
                            Dim GetTokenKey As String = ConfigurationManager.AppSettings("DEV_TOKEN_KEY")
                            Dim GetUniqeIDToken As String = Format(Now, "dd/MM/yy H:mm:ss zzz")
                            wsL.Timeout = 6000000
                            Dim header As wsDocMngServices.ValidationSoapHeader = New wsDocMngServices.ValidationSoapHeader()
                            header.DevToken = CryptoManagerTDES.Encode(GetToken)
                            header.DevTokenKey = CryptoManagerTDES.Encode(GetTokenKey)
                            header.UniqeIDToken = CryptoManagerTDES.Encode(GetUniqeIDToken)
                            wsL.ValidationSoapHeaderValue = header

                        Catch ex As Exception
                            Return "CONNECT_ERR"
                        End Try


                        Select Case serviseType
                            Case "DOWNLOAD" '--------------DOWNLOAD--------------
                                Try
                                    bFileDounloaded = wsL.Download_Document(FolderArr, sSubject, branchCode, FileName)
                                    If bFileDounloaded Is Nothing Then
                                        Return "FALSE"
                                    End If
                                Catch ex As Exception
                                    '  wsL = Nothing
                                    bFileDounloaded = Nothing
                                    Return "FALSE"
                                End Try

                            Case "LISTOBJECT" '--------------LISTOBJECT--------------
                                Try
                                    dsListObject = wsL.GetListObject(FolderPath, StorageFolder, branchCode)
                                    If dsListObject Is Nothing Then
                                        Return "FALSE"
                                    End If
                                Catch ex As Exception
                                    'wsL = Nothing
                                    dsListObject = Nothing
                                    Return "FALSE"
                                End Try

                            Case "MOVEFILES" '--------------MOVE--------------
                                Try
                                    Dim s As String = wsL.MoveFLD_OBJECT(Subject, FolderArr.Substring(0, FolderArr.IndexOf("\")) & "\" & QuotationNo & "\001", TargetSubject, FolderArr, Owner, TargetBranchCode, AdditionalData)
                                    If s.ToString.ToUpper.Contains("ERROR") Then
                                        Return "False"
                                    Else
                                        Return "True"
                                    End If
                                Catch ex As Exception
                                    'wsL = Nothing
                                    Return "False"
                                End Try
                            Case "UPLOAD" '-------------UPLOAD - -------------
                                Try
                                    Dim s As String = wsL.Upload_Document(branchCode, StorageFolder, DocMngFolderPath, FileName, UserAS400, File, True)
                                    If s.ToString.ToUpper.Contains("ERROR") Or s.ToString.ToUpper.Contains("FAIL") Then
                                        GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_PRINTOUT_ERROR.ToString, "Upload_Document", s, branchCode, QuotationNo, "", UserAS400.ToString)

                                        Return "FALSEERROR"
                                    End If
                                    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_PRINTOUT_ERROR.ToString, "Upload_Document", s, branchCode, QuotationNo, "", UserAS400.ToString)
                                    Return s
                                Catch ex As Exception
                                    ' wsL = Nothing
                                    Return "FALSE"
                                End Try
                                Return "TRUE"
                            Case "DELETE" '--------------DELETE--------------
                                Try
                                    If Not IsNumeric(FileId) Then FileId = "0"
                                    Dim s As String = wsL.DeleteFile(Subject, branchCode, FolderPath, FileName, CInt(FileId), UserAS400)
                                    If s.ToString.ToUpper.Contains("ERROR") Then
                                        Return "FALSEERROR"
                                    Else
                                        Return "TRUE"
                                    End If
                                Catch ex As Exception
                                    'wsL = Nothing
                                    Return "FALSE"
                                End Try
                            Case "EXIST"
                                Try
                                    Dim s As String = wsL.IsObjectExist(FolderPath, Subject, FileName, branchCode)
                                    If s.ToString.ToUpper.Contains("ERROR") Then
                                        Return "FALSEERROR"
                                    End If
                                Catch ex As Exception
                                    ' wsL = Nothing
                                    Return "FALSE"
                                End Try
                        End Select
                    End Using


                Catch ex As Exception
                    Return "FALSE"
                End Try
                Return "TRUE"
            End If

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.DOCMNG_ERROR.ToString, "DocmngServiceAuthentication",
                                              "ex=" & ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
        End Try
    End Function






End Class
