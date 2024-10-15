Imports System.Configuration
Imports System.Globalization
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports IscarDal

Public Class GeneralException
    Inherits Exception

    Public Enum e_LogTitle
        AS400 = 0
        PARAMETERS = 1
        PRICES = 2
        REPORT = 3
        GENERAL = 4
        LOGIN = 5
        TIMEOUT = 6
        [ERROR] = 7
        PDFBSON_3D2D = 8
        BSON_3D = 9
        PDF_2D = 10
        DOCMNG = 11
        EMAIL = 12
        PARSER = 13
        UPDATEDATA = 14
        MATERIAL = 15
        MASTER = 16
        REPORT_ERROR = 17
        REPORT_PRINTOUT = 18
        REPORT_PRINTOUT_ERROR = 19
        LOGIN_ERROR = 20
        CONNECT_USER = 21
        DOCMNG_ERROR = 22
        AS400_ERROR = 23
        CountTimeer = 24
        Entrance = 25
        Submit = 26
        UPLOADFILE = 27
        MAIL_ERROR = 28
        CONFIGURATORBUILDER_ERROR = 29
        BUILDER_ERROR = 30
        DEFAULT_ERROR = 31
    End Enum
    Public Enum ErrorMessages
        AuthenticationException_ErrMsg

        General_ErrMsg

        User_UserAlreadyExist_ErrMsg
        User_UserIsLocked_ErrMsg
        User_UserIsLockedAfter60Days_ErrMsg
        User_UserIsBlocked_ErrMsg
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal Message As String)
        MyBase.New(Message)

        'WriteEvent(Message, EventLogEntryType.Information)
    End Sub

    Public Sub New(ByVal MessageEnum As GeneralException.ErrorMessages)
        MyBase.New(MessageEnum.ToString())

        'WriteEvent(MessageEnum.ToString(), EventLogEntryType.Information)
    End Sub

    Public Sub New(ByVal Message As String, ByVal innerException As Exception)
        MyBase.New(Message, innerException)

        WriteEvent(innerException, Message)
    End Sub

    Public Sub New(ByVal Message As String, ByVal InnerException As Exception, ByVal LogMessage As String)
        MyBase.New(Message, InnerException)

        WriteEvent(InnerException, LogMessage)
    End Sub

    Public Shared Sub BuildError(ByVal Page As Page, ByVal Ex As Exception, Optional ByVal Message As String = "")


        'If Not CBool(ConfigurationManager.AppSettings("IsDebugMode")) Then
        '    If Message = String.Empty Then
        '        Message = My.Resources.ErrorMessages.ResourceManager.GetString(ErrorMessages.General_ErrMsg.ToString)
        '    End If
        '    BuildError(Page, Message)
        '    WriteEvent(Ex)
        'Else
        Dim s As String
        s = Message & "<BR>" & "Exception:" & Ex.GetType.ToString & "<BR>" & "Exception Message:" & Ex.Message & "<BR>" & "Stack Trace:" & Ex.StackTrace & "<BR><HR>"
        If Not (Ex.InnerException) Is Nothing Then
            s &= "Inner Exception:" & Ex.InnerException.GetType.ToString & "<BR>" & "InnerException Message:" & Ex.InnerException.Message & "<BR>" & "InnerException Stack Trace:" & Ex.InnerException.StackTrace & "<BR><HR>"
        End If
        'BuildError(Page, s)
        BuildError(Page, Ex.Message)
        WriteEvent(Ex, s)
        'End If
        'End If

    End Sub

    Public Shared Sub BuildError(ByVal Page As Page, ByVal Ex As GeneralException)

        Dim s As String
        s = "Exception:" & Ex.GetType.ToString & "<BR>" & "Exception Message:" & Ex.Message & "<BR>" & "Stack Trace:" & Ex.StackTrace & "<BR><HR>"
        If Not (Ex.InnerException) Is Nothing Then
            s &= "Inner Exception:" & Ex.InnerException.GetType.ToString & "<BR>" & "InnerException Message:" & Ex.InnerException.Message & "<BR>" & "InnerException Stack Trace:" & Ex.InnerException.StackTrace & "<BR><HR>"
        End If
        'BuildError(Page, s)
        BuildError(Page, Ex.Message)
        WriteEvent(Ex, s)
        'End If
        'End If

    End Sub

    Public Shared Sub BuildError(ByVal Page As Page, ByVal Message As String)
        Try



            Dim val As New CustomValidator

            val.ErrorMessage = Message
            val.IsValid = False
            If Not Page.Validators Is Nothing Then ' AndAlso Page.Validators.Count < 1 Then
                Dim bValid As Boolean = False
                If Page.Validators.Count > 0 AndAlso Page.Validators(0).ErrorMessage.ToString.ToUpper.Trim.Contains("ERROR GETING ITEMS FROM CATALOG") Then
                    bValid = True
                End If
                If bValid = False Then
                    Page.Validators.Add(val)
                End If
            End If
            'End If
        Catch ex As Exception

        End Try
    End Sub

    Public Shared Sub WriteEvent(ByVal ex As Exception)
        WriteEvent(ex, String.Empty)
    End Sub

    Public Shared Sub WriteEvent(ByVal ex As Exception, ByVal LogMessage As String)

        Dim Msg As String = ""

        Try
            'Msg = "BRANCH:" & StateManager.GetValue(StateManager.Keys.BranchCode) & vbCrLf
        Catch
            Msg = "Branch: UNKNOWN" & vbCrLf
        End Try

        'Msg &= "IP:" & HttpContext.Current.Request.UserHostAddress & vbCrLf

        Try
            'Msg &= "USER:" & StateManager.GetValue(StateManager.Keys.UserFullName) & vbCrLf
        Catch
            Msg &= "USER: UNKNOWN"
        End Try

        'Msg &= "URL:" & HttpContext.Current.Request.Url.AbsoluteUri & vbCrLf & vbCrLf
        Msg &= "MESSAGE:" & LogMessage & vbCrLf
        Msg &= "EXCEPTION MESSAGE:" & ex.Message & vbCrLf & vbCrLf
        Msg &= "TRACE: "

        If Not ex.StackTrace Is Nothing Then Msg &= ex.StackTrace.ToString

        ' WriteEvent(Msg)
        'End If
    End Sub


    Public Shared Sub LogWriteEventSql(ByVal LOG_TYPE As String, LOG_TITLE As String, ByVal LogMessage As String, BRANCH_CODE As String, QUOTATION_NUMBER As String, GALQUOTATION_NUMBER As String, LOGGED_EMAIL As String)
        ' Exit Sub
        'Dim BRANCH_CODE As String = "" ' = clsBranch.ReturnActiveBranchCodeState
        'Dim QUOTATION_NUMBER As String = "" '= clsQuatation.ACTIVE_iQuoteQuotationNumber
        'Dim GALQUOTATION_NUMBER As String = "" ' = clsQuatation.ACTIVE_GALQuotationNumber
        'Dim LOGGED_EMAIL As String = "" ' "clsQuatation.ACTIVE_UseLoggedEmail

        If Not LogMessage.ToString.ToUpper.Contains("THREAD WAS BEING ABORTED") Then

            Dim country As String = ""
            Try
                country = RegionInfo.CurrentRegion.DisplayName
            Catch ex As Exception

            End Try

            Try
                If ConfigurationManager.AppSettings("writeTo_LOGSql") = "YES" Then
                    Try

                        'AddquotationLogNew(LogMessage, LOG_TYPE, LOG_TITLE, BRANCH_CODE, QUOTATION_NUMBER, GALQUOTATION_NUMBER, LOGGED_EMAIL)

                        Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
                        Dim Params As New SqlParams

                        Params.Add(New SqlParam("@LOG_TYPE", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Left(LOG_TYPE, 19), 20))
                        Params.Add(New SqlParam("@BRANCH_CODE", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Left(BRANCH_CODE, 4), 5))
                        Params.Add(New SqlParam("@QUOTATION_NUMBER", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Left(QUOTATION_NUMBER, 99), 100))
                        Params.Add(New SqlParam("@GAL_NUMBER", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Left(GALQUOTATION_NUMBER, 99), 100))
                        Params.Add(New SqlParam("@LOGGED_EMAIL", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Left(LOGGED_EMAIL, 99), 100))
                        Params.Add(New SqlParam("@LOG_DES", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, LogMessage))
                        Params.Add(New SqlParam("@LOG_TITLE", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Left(LOG_TITLE, 99), 100))
                        Params.Add(New SqlParam("@country", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Left(country, 99), 100))

                        oSql.ExecuteSP("usp_CreateLog", Params)

                        'Dim LogMessage1 As String = "Procedure : " & FunctionName & vbCrLf & "Log :" & LogMessage2.ToString
                        'Dim LogFileName As String = String.Format(ConfigurationManager.AppSettings("LogPathReport"), Now)
                        'Dim fp As New StreamWriter(LogFileName, True)
                        'fp.WriteLine(Now.ToLongTimeString & ":" & vbCrLf & sn & vbCrLf & LogMessage1 & vbCrLf)
                        'fp.Flush()
                        'fp.Close()
                    Catch ex As Exception
                        'no action
                    End Try
                End If
            Catch ex As Exception

            End Try

            Try
                If ConfigurationManager.AppSettings("writeTo_LOGFile") = "YES" Then
                    LogWriteEventiFile(LOG_TYPE, LOG_TITLE, LogMessage, BRANCH_CODE, QUOTATION_NUMBER, GALQUOTATION_NUMBER, LOGGED_EMAIL)
                End If
            Catch ex As Exception
                'no action
            End Try

        End If
    End Sub


    Public Shared Sub LogWriteEventiFile(ByVal LOG_TYPE As String, LOG_TITLE As String, ByVal LogMessage As String, BRANCH_CODE As String, QUOTATION_NUMBER As String, GALQUOTATION_NUMBER As String, LOGGED_EMAIL As String)

        'Dim BRANCH_CODE As String = "" ' = clsBranch.ReturnActiveBranchCodeState
        'Dim QUOTATION_NUMBER As String = "" '= clsQuatation.ACTIVE_iQuoteQuotationNumber
        'Dim GALQUOTATION_NUMBER As String = "" ' = clsQuatation.ACTIVE_GALQuotationNumber
        'Dim LOGGED_EMAIL As String = "" ' "clsQuatation.ACTIVE_UseLoggedEmail

        If ConfigurationManager.AppSettings("writeTo_LOGFile") = "YES" Then
            Try

                Dim LogMsg As String = ""
                LogMsg &= "Type : " & LOG_TYPE & " -- "
                LogMsg &= "Title : " & LOG_TITLE & " -- "
                LogMsg &= "BRANCH_CODE : " & BRANCH_CODE & " -- "
                LogMsg &= "QUOTATION_NUMBER : " & QUOTATION_NUMBER & " -- "
                LogMsg &= "GALQUOTATION_NUMBER : " & GALQUOTATION_NUMBER & " -- "
                LogMsg &= "LOGGED_EMAIL : " & LOGGED_EMAIL & vbCrLf
                LogMsg &= "Message : " & LogMessage & vbCrLf & "------------------------------------------------------------------------"

                Dim LogFileName As String = ""
                If LOG_TYPE.ToString.ToUpper.Contains("ERROR") Then
                    LogFileName = String.Format(ConfigurationManager.AppSettings("LogPathError"), Now)
                ElseIf LOG_TYPE.ToString.ToUpper.Contains("LOGIN") Then
                    LogFileName = String.Format(ConfigurationManager.AppSettings("LogPathLogIn"), Now)
                ElseIf LOG_TYPE.ToString.ToUpper.Contains("REPORT") Then
                    LogFileName = String.Format(ConfigurationManager.AppSettings("LogPathReport"), Now)
                ElseIf LOG_TYPE.ToString.ToUpper.Contains("COUNT") Then
                    LogFileName = String.Format(ConfigurationManager.AppSettings("LogPathCounter"), Now)
                Else
                    LogFileName = String.Format(ConfigurationManager.AppSettings("LogPath"), Now)
                End If


                'Dim LogFileName As String = String.Format(ConfigurationManager.AppSettings("LogPathReport"), Now)
                Dim fp As New StreamWriter(LogFileName, True)
                fp.WriteLine(Now.ToLongTimeString & ":" & LogMsg)
                fp.Flush()
                fp.Close()
            Catch ex As Exception
                'no action
            End Try
        End If

    End Sub



    'Public Shared Sub WriteEventLogReport(ByVal FunctionName As String, ByVal LogMessage2 As String, ByVal LogTitle As String, Optional sn As String = "")


    '    If ConfigurationManager.AppSettings("writeTo_LOG") = "YES" Then
    '        Try

    '            AddquotationLog("FunctionName:" & FunctionName & "Log: " & LogMessage2 & " - " & sn, e_LogType.GENERAL.ToString, LogTitle)

    '            ''Dim sn As String = ""
    '            'Try
    '            '    If sn = "" Then
    '            '        sn = "(Quotation Number : " & SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False) & ")"
    '            '        sn &= " -- (Quotation Date : " & SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationDate, False) & ")"
    '            '        sn &= " -- (BranchCode : " & SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False) & ")"
    '            '        sn &= " -- (Email Log : " & StateManager.GetValue(StateManager.Keys.s_loggedEmail).ToString & ")"
    '            '    End If
    '            'Catch ex As Exception

    '            'End Try
    '            Dim LogMessage1 As String = "Procedure : " & FunctionName & vbCrLf & "Log :" & LogMessage2.ToString
    '            Dim LogFileName As String = String.Format(ConfigurationManager.AppSettings("LogPathReport"), Now)
    '            Dim fp As New StreamWriter(LogFileName, True)
    '            fp.WriteLine(Now.ToLongTimeString & ":" & vbCrLf & sn & vbCrLf & LogMessage1 & vbCrLf)
    '            fp.Flush()
    '            fp.Close()
    '        Catch ex As Exception
    '            'no action
    '        End Try
    '    End If

    Public Shared Sub whriteSqlExiption(ErrorType As String, LogTitle As String, erMessage As String, GetQuotationData As Boolean)
        Try
            If Not erMessage.ToString.ToUpper.Contains("THREAD WAS BEING ABORTED") Then
                If GetQuotationData = False Then
                    GeneralException.LogWriteEventSql(ErrorType, LogTitle, erMessage, "", "", "", "")
                Else
                    GeneralException.LogWriteEventSql(ErrorType, LogTitle, erMessage, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_iQuoteQuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

End Class
