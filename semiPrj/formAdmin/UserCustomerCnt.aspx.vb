Imports SemiApp_bl
Imports System.IO
Imports System.Net
Imports IscarDal
Imports Newtonsoft.Json
Imports Microsoft.ReportingServices.Rendering.ExcelOpenXmlRenderer.Parser.drawingml.x2006

Public Class UserCustomerCnt
    Inherits System.Web.UI.Page

    Private _s_Branch_Code = ""
    Private _s_Branch_Code_State = ""

    Private _s_logged_Email = ""
    Private _s_logged_Email_State = ""

    Private Enum e_EnumGridCols
        sSelect = 0
        BranchCode = 1
        DisplayName = 2
        loggedEmail = 3
        AS400_Cust = 4
    End Enum

    Dim sbc As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Try
        '    If unf.Value = "" Then
        '        If Not Request("rErepTr") Is Nothing Then
        '            If Request("rErepTr").ToString <> "" Then
        '                unf.Value = CryptoManager.Decode(HttpContext.Current.Request("rErepTr"))
        '            End If
        '        End If
        '    End If

        'Catch ex As Exception

        'End Try

        ' GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "Page_Load", "Page_Load", clsBranch.ReturnActiveBranchCodeState, "", "", clsQuatation.ACTIVE_UseLoggedEmail.ToString)


        '_s_logged_Email = CookiesManager.GetStringValue(CryptoManagerTDES.Encode("UserEmailForCurentQut"))
        '_s_Branch_Code = CookiesManager.GetStringValue(CryptoManagerTDES.Encode("BranchCodeForCurentQut"))

        ' checkifusercanaccess()


        '_s_logged_Email = StateManager.GetValue(StateManager.Keys.s_loggedEmail, True)
        '_s_Branch_Code = StateManager.GetValue(StateManager.Keys.s_BranchCode)

        checkifusercanaccess()

        Try
            lblError.Text = ""
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertHD", "HideDiv();", True)
            If _s_Branch_Code <> "" Then

                txtBranchCode.Text = _s_Branch_Code

                txtSearchAll.DataBind()

                Dim dtb As DataTable = clsBranch.GetBranchDetails(_s_Branch_Code)
                If Not dtb Is Nothing AndAlso dtb.Rows.Count > 0 Then
                    sbc = dtb.Rows(0).Item("BranchName").ToString '& "-" & dt.Rows(0).Item("BranchNumber").ToString & " ; "
                    lblTitle.Text = "IQUOTE - " & sbc & " ADMINISTRATION"
                End If

                If Not IsPostBack Then



                    fillDDlBranches()


                    FillGrid()
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub updateData()

        Dim ex1 As String = ""
        Try
            Dim BC As String = ddlBranch.SelectedValue
            Dim LogMainEmail As String = _s_logged_Email
            Dim loggedEmail As String = lblEmailtoConnect.Text.Trim
            Dim sCustomerNumber As String = txtCustomerNumber.Text.Trim
            Dim wrongV As Boolean = False
            If BC.Length <> "2" Or Not loggedEmail.Contains("@") Or Not IsNumeric(sCustomerNumber) Then
                wrongV = True
            End If

            If wrongV Then
                'lblErrorConect.Text = "Missing data!"
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertHDSM", "ShowAlert(Missing data!);", True)
            Else
                If sCustomerNumber <> "" AndAlso BC <> "" AndAlso loggedEmail <> "" AndAlso IsNumeric(sCustomerNumber) AndAlso CInt(sCustomerNumber) > 0 Then

                    writetoconnect("Start Update Sql Data", "logged Email", loggedEmail, "Customer Number", sCustomerNumber, "Branch Code", BC)
                    Dim s As String = clsUser.ConnectUserToCustomer(BC, loggedEmail, sCustomerNumber).ToString.Trim
                    If s = "1" Then
                        'If ChangeUserDataInGal(BC, loggedEmail, sCustomerNumber, ex1) = False Then
                        'Dim md As String = loggedEmail
                        If loggedEmail.ToString.Contains("@") AndAlso loggedEmail.ToString.Trim.Substring(0, 1) <> "@" AndAlso loggedEmail.ToString.Trim.ToUpper.Contains("@ISCAR.CO.IL") Then
                            If GAL.ChangeUserBranchCodeAndNumberInGAL(loggedEmail, BC, sCustomerNumber) = False Then
                                writetoconnect("Change " & loggedEmail & " User Data In Gal Failed" & " " & ex1)
                                lblError.Text = ex1
                                'lblErrorConect.Text = "Faild To conect user To branch " & ex1
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertHDSB", "ShowAlert(""" & "Faild To conect user To branch " & ex1 & "!"");", True)
                            Else
                                writetoconnect("Change " & loggedEmail & " User Data In Gal Succeeded")
                                ' clsUser.ConnectUserToCustomer(BC, loggedEmail, dis)
                                'lblErrorConect.Text = "Connection succeeded"

                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertHDS", "ShowAlertSuccess(""Update Succeded."");", True)




                            End If
                        End If
                    Else
                        writetoconnect("User Not exist In lcl_TUsersDomain")
                        lblError.Text = "User Not exist In lcl_TUsersDomain"
                        ' lblErrorConect.Text = "User Not exist In IMCID!"
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertHDSU", "ShowAlert(""User Not exist In IMCID!"");", True)
                    End If

                Else
                    'lblErrorConect.Text = "Missing data! Fill all required entry fields"
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertHDSUng", "ShowAlert(""Missing data! Fill all required entry fields!"");", True)
                End If
            End If
            FillGrid()
        Catch ex As Exception
            'lblErrorConect.Text = "Faild To conect user To branch " & ex1
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertHDSUng", "ShowAlert(""Faild To conect USER!"");", True)
        End Try
    End Sub


    Private Sub FillGrid(Optional ByVal sortExpression As String = Nothing)
        Try
            Dim sAny As String = txtSearchAll.Text.ToString.Trim
            Dim dt As DataTable
            Dim BC As String = _s_Branch_Code
            Dim loggedEmail As String = _s_logged_Email
            Dim sB As String = ""
            If Not ddlBranch.SelectedValue Is Nothing AndAlso (ddlBranch.SelectedValue.ToString.Length = "2" Or ddlBranch.SelectedIndex > 0) Then
                sbc = ddlBranch.SelectedValue
                txtBranchCode.Text = ddlBranch.SelectedValue
            End If
            If sbc <> "" AndAlso sbc.Length = 2 Then
                dt = clsUser.GETLoggedUsers(sbc, loggedEmail, sAny)
            Else
                dt = clsUser.GETLoggedUsers(BC, loggedEmail, sAny)
            End If


            If Not dt Is Nothing Then
                gvConnect.DataSource = dt
                gvConnect.DataBind()
            End If


        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    'Private Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click
    '    updateData()
    'End Sub

    Private Sub txtSearchAll_DataBinding(sender As Object, e As EventArgs) Handles txtSearchAll.DataBinding
        Try

            txtSearchAll.Attributes.Add("onkeydown", "Return EnterValueForSearch('" & txtSearchAll.ClientID & "', '" & btnSearchAll.ClientID & "');")
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub btnSearchAll_Click(sender As Object, e As ImageClickEventArgs) Handles btnSearchAll.Click
        FillGrid()
    End Sub

    Private Sub gvConnect_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvConnect.RowCommand
        Try
            Select Case e.CommandName.ToUpper
                Case "SELECT"
                    txtCustomerNumber.Text = gvConnect.Rows(e.CommandArgument).Cells(e_EnumGridCols.AS400_Cust).Text.ToString
                    lblEmailtoConnect.Text = gvConnect.Rows(e.CommandArgument).Cells(e_EnumGridCols.loggedEmail).Text.ToString
                    'lblDisplayName.Text = gvConnect.Rows(e.CommandArgument).Cells(e_EnumGridCols.DisplayName).Text.ToString
            End Select
        Catch ex As Exception
            Throw
        End Try
    End Sub


    Private Sub fillDDlBranches()
        Try

            Dim loggedEmail As String = _s_logged_Email
            Dim BranchCode As String = _s_Branch_Code
            Dim USerType As String = ""
            Dim sdtUserType As DataTable = clsUser.GetUserType(BranchCode, loggedEmail, USerType)
            If Not sdtUserType Is Nothing AndAlso sdtUserType.Rows.Count > 0 Then
                If sdtUserType.Rows(0).Item("BranchAdmin").ToString.ToUpper = "TRUE" Then
                    Dim bUserAll As Boolean = False
                    If USerType = clsUser.e_UserType.AdminALL Then
                        bUserAll = True
                    End If

                    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempQuotationListCustomer, "")

                    Dim dtd As DataTable = clsBranch.GetBranches()
                    If Not dtd Is Nothing Then
                        If bUserAll = True Then
                            Dim dr As DataRow = dtd.NewRow
                            dtd.Rows.InsertAt(dr, 0)
                            ddlBranch.DataSource = dtd
                            ddlBranch.DataValueField = "BranchCode"
                            ddlBranch.DataTextField = "BR"
                            dr("BR") = "Select Branch"

                            ddlBranch.DataBind()
                            ddlBranch.SelectedValue = BranchCode
                            txtBranchCode.Text = BranchCode
                        Else
                            ddlBranch.Enabled = False
                            Dim dv As DataView = dtd.DefaultView
                            dv.RowFilter = "BranchCode='" & BranchCode & "'"
                            ddlBranch.DataSource = dv
                            ddlBranch.DataValueField = "BranchCode"
                            ddlBranch.DataTextField = "BR"

                            ddlBranch.DataBind()
                        End If

                    End If
                Else
                    Response.Redirect("~/Default.aspx", False)
                End If

            Else
                Response.Redirect("~/Default.aspx", False)
            End If

        Catch ex As Exception
            Response.Redirect("~/Default.aspx", False)
        End Try

        'Throw New NotImplementedException()
    End Sub
    Private Sub checkifusercanaccess()
        Try
            'Exit Sub
            If ConfigurationManager.AppSettings("DOCHECKCOOKIE").ToString = "YES" Then
                Try
                    _s_logged_Email = CookiesManager.GetStringValue(CryptoManagerTDES.Encode("UserEmailForCurentQut"))
                    _s_Branch_Code = CookiesManager.GetStringValue(CryptoManagerTDES.Encode("BranchCodeForCurentQut"))
                    If _s_logged_Email.ToString = "" Then
                        _s_logged_Email = Request(CryptoManagerTDES.Encode("UserEmailForCurentQut"))
                        _s_logged_Email = CryptoManagerTDES.Decode(_s_logged_Email)

                    End If
                    If _s_logged_Email.ToString = "" Then
                        _s_Branch_Code = Request(CryptoManagerTDES.Encode("BranchCodeForCurentQut"))
                        _s_Branch_Code = CryptoManagerTDES.Decode(_s_Branch_Code)

                    End If

                    _s_logged_Email_State = StateManager.GetValue(StateManager.Keys.s_loggedEmail, True)
                    _s_Branch_Code_State = StateManager.GetValue(StateManager.Keys.s_BranchCode)
                Catch ex As Exception

                End Try
                'GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "_s_logged_Email", _s_logged_Email, clsBranch.ReturnActiveBranchCodeState, "", "", clsQuatation.ACTIVE_UseLoggedEmail.ToString)
                'GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "_s_Branch_Code", _s_Branch_Code, clsBranch.ReturnActiveBranchCodeState, "", "", clsQuatation.ACTIVE_UseLoggedEmail.ToString)

                Try
                    If _s_Branch_Code = "" AndAlso _s_logged_Email = "" Then
                        _s_logged_Email = CryptoManagerTDES.Decode(Request("rEreptochr"))
                        _s_Branch_Code = CryptoManagerTDES.Decode(Request("rEreptochrbc"))
                    End If
                Catch ex As Exception

                End Try
                'GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "_s_logged_Email", _s_logged_Email, clsBranch.ReturnActiveBranchCodeState, "", "", clsQuatation.ACTIVE_UseLoggedEmail.ToString)
                'GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "_s_Branch_Code", _s_Branch_Code, clsBranch.ReturnActiveBranchCodeState, "", "", clsQuatation.ACTIVE_UseLoggedEmail.ToString)




                'Dim loggedEmail As String = _s_logged_Email
                'Dim BranchCode As String = _s_Branch_Code

                'GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "loggedEmail", loggedEmail, clsBranch.ReturnActiveBranchCodeState, "", "", clsQuatation.ACTIVE_UseLoggedEmail.ToString)


                If _s_logged_Email = _s_logged_Email_State AndAlso _s_Branch_Code = _s_Branch_Code_State AndAlso _s_logged_Email <> "" AndAlso _s_logged_Email.ToString.Contains("@") AndAlso _s_Branch_Code.ToString.Length = "2" AndAlso _s_Branch_Code <> "ZZ" Then
                Else
                    Response.Redirect("~/Default.aspx", True)
                End If

                Dim USerType As String = ""
                Dim sdtUserType As DataTable = clsUser.GetUserType(_s_Branch_Code_State, _s_logged_Email_State, USerType)
                If Not sdtUserType Is Nothing AndAlso sdtUserType.Rows.Count > 0 Then
                    If sdtUserType.Rows(0).Item("BranchAdmin").ToString.ToUpper = "TRUE" Then

                    Else
                        Response.Redirect("~/Default.aspx", True)
                    End If

                Else
                    Response.Redirect("~/Default.aspx", True)
                End If
            End If
        Catch ex As Exception
            Response.Redirect("~/Default.aspx", True)
        End Try

        'Throw New NotImplementedException()

    End Sub
    'Private Function ChangeUserDataInGal(BC As String, sMail As String, Customer As String, ByRef exE As String) As Boolean
    '    Dim exE1 As String = ""
    '    Try

    '        Dim md As String = sMail
    '        If md.ToString.Contains("@") AndAlso md.ToString.Trim.Substring(0, 1) <> "@" AndAlso md.ToString.Trim.ToUpper.Contains("@ISCAR.CO.IL") Then
    '            If sMail = "" Then sMail = md
    '        End If
    '        'If Not sMail Is Nothing AndAlso sMail <> "" Then
    '        Dim BranchCode As String = BC
    '        Dim GalCustomerNumber As String = Customer

    '        Dim ApUrl As String = ConfigurationManager.AppSettings("LoginURLupdateUserFixed")
    '        Dim siteType As String = ConfigurationManager.AppSettings("siteType")
    '        Dim appId As String = ConfigurationManager.AppSettings("ApplicationID")

    '        writetoconnect("ChangeUserBranchCodeAndNumberInGAL", "siteType", siteType, "appId", appId, "ApUrl", ApUrl, "GalCustomerNumber", GalCustomerNumber, "ApUrl", ApUrl)

    '        If ChangeUserBranchCodeAndNumberInGAL(siteType, appId, ApUrl, sMail, BranchCode, GalCustomerNumber, exE1) = True Then

    '            Return True
    '        End If
    '        exE = exE1
    '        Return False
    '    Catch ex As Exception
    '        exE = ex.Message & exE1
    '        Return False
    '    End Try

    'End Function


    'Private Function ChangeUserBranchCodeAndNumberInGAL(siteType As String, appId As String, ApUrl As String, sMail As String, BranchCode As String, GalCustomerNumber As String, ByRef exE As String) As Boolean
    '    Dim exE2 As String = ""
    '    Try


    '        'Dim userDetails As String = "email=" & sMail & "&branchCode=" & BranchCode & "&galCustomerNumber=" & GalCustomerNumber
    '        Dim userDetails As String = sMail & "/" & BranchCode & "/" & GalCustomerNumber
    '        Dim webAPI As String = ApUrl & appId & "/" & siteType & "/" & userDetails

    '        writetoconnect("sendAndGet", "webAPI", webAPI)
    '        Dim Status As String = sendAndGet(webAPI, exE2)
    '        writetoconnect("ChangeUserBranchCodeAndNumberInGAL", "Status", Status)
    '        If Status.Contains("200\") Then
    '            Return True
    '        Else
    '            exE = Status
    '            Return False
    '        End If
    '        Try

    '        Catch ex As Exception
    '            exE2 = ex.Message
    '        End Try



    '    Catch ex As WebException
    '        exE = exE2 & ex.Message
    '        Throw
    '    End Try
    'End Function

    Public Shared Function sendAndGet(url As String, ByRef exE As String) As String
        Try
            Dim response As HttpWebResponse
            Dim readStream As StreamReader
            Dim res As String

            Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) EastUSiQuote Chrome/118.0.0.0 Safari/537.36"
            request.Credentials = CredentialCache.DefaultCredentials
            response = CType(request.GetResponse(), HttpWebResponse)
            Dim receiveStream As Stream = response.GetResponseStream()
            readStream = New StreamReader(receiveStream, Encoding.UTF8)
            res = readStream.ReadToEnd()
            response.Close()
            readStream.Close()

            Return res

        Catch ex As WebException
            exE = ex.Message
            Throw

        End Try
    End Function

    Private Sub ddlBranch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlBranch.SelectedIndexChanged
        FillGrid()
    End Sub

    Private Sub btnActiveSave_Click(sender As Object, e As EventArgs) Handles btnActiveSave.Click
        Try
            Dim txtbc As String = txtBranchCode.Text.Trim
            Dim txtdn As String = txtDisplayName.Text.Trim
            Dim txtle As String = txtLoggedEmail.Text.Trim
            Dim txtsn As String = txtSurname.Text.Trim
            Dim txtgn As String = txtGivenName.Text.Trim
            Dim se As String = ""
            If txtbc <> "" And txtdn <> "" And txtle <> "" And txtsn <> "" And txtgn <> "" Then
                writetoconnect("Add user to Sql Data lcl_TUsersDomain", "Branch Code", txtbc, "Display Name", txtdn, "Logged Email", txtle, "Sur name", txtsn, "Given Name", txtgn)

                se = clsUser.AddUserToUsersDomain(txtbc, txtle, txtdn, txtsn, txtgn)
                If se = "1" Then
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertHDSEx", "ShowAlert(""user already exists!"");", True)
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertHDDs", "ShowDivT();", True)

                End If
            Else
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertHDSExRF", "ShowAlert(""Please fill all required fields!"");", True)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub writetoconnect(sSubject As String, Optional s2 As String = "", Optional b2 As String = "", Optional s3 As String = "", Optional b3 As String = "", Optional s4 As String = "", Optional b4 As String = "", Optional s5 As String = "", Optional b5 As String = "", Optional s6 As String = "", Optional b6 As String = "")
        Try

            Dim sstr As String = sSubject
            If s2 <> "" Then sstr &= s2 & " : " & b2 & " ; "
            If s3 <> "" Then sstr &= s3 & " : " & b3 & " ; "
            If s4 <> "" Then sstr &= s4 & " : " & b4 & " ; "
            If s5 <> "" Then sstr &= s5 & " : " & b5 & " ; "
            If s6 <> "" Then sstr &= s6 & " : " & b6 & " ; "

            'GeneralException.WriteEventConnectUser(sSubject & " - " & sstr)

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.CONNECT_USER.ToString, "Connect user to customer", sstr, clsBranch.ReturnActiveBranchCodeState, "", "", clsQuatation.ACTIVE_UseLoggedEmail.ToString)



        Catch ex As Exception

        End Try

    End Sub

    Private Sub btnActiveConnect_Click(sender As Object, e As EventArgs) Handles btnActiveConnect.Click
        updateData()
    End Sub

    Private Sub gvConnect_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvConnect.RowDataBound
        Try


            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" & " this.style.backgroundColor='#f7f7f7';")
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;")

            End If
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub btnGoHome_Click(sender As Object, e As EventArgs) Handles btnGoHome.Click

        If lblEmailtoConnect.Text.ToString.Trim.ToLower = clsQuatation.ACTIVE_UseLoggedEmail.ToString.Trim.ToLower Then
            'LogIn.LogOut()
            If ConfigurationManager.AppSettings("IsDebugMode").ToString.ToUpper = "TRUE" Then
                Response.Redirect("http://localhost:60377/Default.aspx", False)
            ElseIf ConfigurationManager.AppSettings("IsDebugMode").ToString.ToUpper = "TEST" Then
                Response.Redirect("http://dmstest/iQuote/Default.aspx", False)
            Else
                Response.Redirect("https://iquote.ssl.imc-companies.com/iQuote/Default.aspx", False)
            End If
        End If


    End Sub

    'Private Sub UserCustomerCnt_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
    '    Try
    '        _s_logged_Email = CookiesManager.GetStringValue(CryptoManagerTDES.Encode("UserEmailForCurentQut"))
    '        _s_Branch_Code = CookiesManager.GetStringValue(CryptoManagerTDES.Encode("BranchCodeForCurentQut"))

    '        'checkifusercanaccess()
    '        CookiesManager.SetValue(CookiesManager.Keys.etoedfrku, "", True)
    '        Dim sLogEmail As String = clsQuatation.ACTIVE_UseLoggedEmail

    '        CookiesManager.SetValue(CookiesManager.Keys.etoedfrku, sLogEmail, True)
    '    Catch ex As Exception

    '    End Try
    'End Sub
End Class