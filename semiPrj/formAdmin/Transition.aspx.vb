Imports System.Drawing
Imports System.Security.Policy
Imports SemiApp_bl
'Imports System.Security.Principal


Public Class Transition
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            txtMailAddressNote.Visible = False

            pnlRado.Visible = False


            disenModCombo()

            If clsUser.SetIsUserAdmin = True Then
                paneltoHide2.Visible = True
                paneltoHide1.Visible = True
                paneltoHide3.Visible = True
                paneltoHide3C.Visible = True
                PanelToHide4.Visible = False

            Else
                paneltoHide2.Visible = False
                paneltoHide1.Visible = False
                paneltoHide3.Visible = False
                paneltoHide3C.Visible = False
                PanelToHide4.Visible = True

            End If

            FillData()

            If Not IsPostBack Then

                Dim le As String = ""

                Try
                    le = Request.LogonUserIdentity.Name
                Catch ex As Exception
                    le = ""
                End Try

                Dim s() = le.Split("\")

                Try
                    txtMailAddress.Text = StateManager.GetValueG(StateManager.Keys.s_iQutUsMail).ToString
                Catch ex As Exception

                End Try
                If Not txtMailAddress.Text.Contains("@") Or txtMailAddress.Text.ToString.IndexOf("@") < 2 Then
                    If Not s Is Nothing AndAlso s.Count > 0 Then
                        txtMailAddress.Text = s(1).ToString.Trim & "@iscar.co.il"
                    Else
                        If le.ToString.ToUpper.Contains("BILAL") Then
                            txtMailAddress.Text = "bilal@iscar.co.il"
                        ElseIf le.ToString.ToUpper.Contains("MICKI") Then
                            txtMailAddress.Text = "mickiv@iscar.co.il"
                        ElseIf le.ToString.ToUpper.Contains("NATALY") Then
                            txtMailAddress.Text = "natalyp@iscar.co.il"
                        ElseIf le.ToString.ToUpper.Contains("TAMAR") Then
                            txtMailAddress.Text = "tamars@iscar.co.ill"
                        Else


                            pnlRado.Visible = True
                        End If
                    End If
                End If
                
                FillddlCalcMode()

                FillFamilylist()

                Try
                    FillModelComcoMID()
                Catch ex As Exception
                End Try

                FillModCombo()
                FillGridItems()

            End If

            SessionManager.Clear_ALL_Sessions()

            SessionManager.Clear_ALL_SessionsTimeStamp()


        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub
    Private Sub FillData()
        Try

            Dim dt As DataTable = SecurityManager.GetMainDataED("")

            Dim dynamicRadioX As New RadioButton()
            dynamicRadioX.ID = "rdX"
            dynamicRadioX.AutoPostBack = False
            dynamicRadioX.Enabled = False
            dynamicRadioX.CssClass = "rdX_non"
            pnlRd.Controls.Add(dynamicRadioX)

            Dim dynamicLabelTitle1 As New TextBox
            dynamicLabelTitle1.Text = "      Branch"
            dynamicLabelTitle1.CssClass = "lbborTitle"
            pnlRd.Controls.Add(dynamicLabelTitle1)

            Dim dynamicLabelTitle2 As New TextBox
            dynamicLabelTitle2.Text = "      Branch Number"
            dynamicLabelTitle2.CssClass = "lbborTitle"
            pnlRd.Controls.Add(dynamicLabelTitle2)

            Dim dynamicLabelTitle3 As New TextBox
            dynamicLabelTitle3.Text = "      Customer Number"
            dynamicLabelTitle3.CssClass = "lbborTitle"
            pnlRd.Controls.Add(dynamicLabelTitle3)

            pnlRd.Controls.Add(New LiteralControl("<br>"))

            For Each r As DataRow In dt.Rows

                Dim dynamicImage As New System.Web.UI.WebControls.Image
                dynamicImage.ImageUrl = "../media/flags/" & r("BranchCode").ToString() & ".svg"
                dynamicImage.Width = "20"
                dynamicImage.CssClass = "verM"
                pnlRd.Controls.Add(dynamicImage)



                Dim dynamicRadio As New RadioButton()
                dynamicRadio.ID = "rd" & r("BranchCode").ToString()
                dynamicRadio.AutoPostBack = True
                dynamicRadio.GroupName = "rdGrpMain"
                dynamicRadio.Checked = False
                AddHandler dynamicRadio.CheckedChanged, AddressOf dynamicRadio_CheckedChanged
                If r("BranchCode").ToString.ToUpper.ToString = "ZZ" Then
                    dynamicRadio.Enabled = False
                End If
                If r("BranchNumber").ToString.ToUpper.ToString = "" Then
                    dynamicRadio.Enabled = False
                End If
                If r("CustomerNumber").ToString.ToUpper.ToString = "" Then
                    dynamicRadio.Enabled = False
                End If
                dynamicRadio.CssClass = "verM"

                pnlRd.Controls.Add(dynamicRadio)
                Dim dynamicLabelBC As New TextBox
                dynamicLabelBC.ID = "lblBranchCodevl" & r("BranchCode").ToString()
                dynamicLabelBC.Text = r("BranchCode").ToString() & " - " & r("BranchName").ToString
                dynamicLabelBC.CssClass = "lbbor"
                dynamicLabelBC.Enabled = False
                pnlRd.Controls.Add(dynamicLabelBC)
                Dim dynamicLabelBN As New TextBox
                dynamicLabelBN.ID = "lblBranchNumbervl" & r("BranchCode").ToString()
                If r("BranchNumber").ToString() <> "" Then
                    dynamicLabelBN.Text = r("BranchNumber").ToString()
                Else
                    dynamicLabelBN.Text = r("BranchNumber").ToString() & " "
                End If
                dynamicLabelBN.CssClass = "lbbor"
                dynamicLabelBN.Enabled = False
                pnlRd.Controls.Add(dynamicLabelBN)
                Dim dynamicLabelCN As New TextBox
                dynamicLabelCN.ID = "lblCustomerNumbervl" & r("BranchCode").ToString()
                If r("CustomerNumber").ToString() <> "" Then
                    dynamicLabelCN.Text = r("CustomerNumber").ToString()
                Else
                    dynamicLabelCN.Text = r("CustomerNumber").ToString() & " "
                End If
                dynamicLabelCN.CssClass = "lbborEn"
                pnlRd.Controls.Add(dynamicLabelCN)

                pnlRd.Controls.Add(New LiteralControl("<br>"))

            Next


        Catch ex As Exception

        End Try
    End Sub


    Private Sub dynamicRadio_CheckedChanged(sender As Object, e As EventArgs)

        Try
            FillData(sender.id.ToString.ToUpper.Replace("RD", ""))
            UpdatePanel1.Update()
        Catch ex As Exception

        End Try

    End Sub
    Private Sub dynamicButton_Click(sender As Object, e As EventArgs)

        Try

            FillData(sender)

        Catch ex As Exception

        End Try

    End Sub


    Private Sub disenModCombo()
        Try


            Dim sStartModificationKey As String = ConfigurationManager.AppSettings("IsFamilyModification").ToString.ToUpper
            If sStartModificationKey = "FALSE" Then
                If ddlModel_Items_MOD.SelectedIndex > 0 Then
                    StartModification.Enabled = True
                    StartModification.ForeColor = Color.DarkBlue
                    StartModification.BackColor = Color.FromName("#e9e9e9")
                Else
                    StartModification.Enabled = False
                    StartModification.ForeColor = Color.FromName("#e9e9e9")
                    StartModification.BackColor = Color.LightGray
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ChooseBranch()
        Try
            Dim sB As String = clsBranch.ReturnActiveBranchCodeState
            If sB <> "" Then
                If Not StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True) Is Nothing AndAlso StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True) <> "" Then
                    CType(pnlRd.FindControl("rd" & sB), RadioButton).Checked = True

                    CType(pnlRd.FindControl("lblCustomerNumbervl" & sB), TextBox).Text = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
                End If
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub FillddlCalcMode()
        Try
            Dim dt_CMOD As New DataTable
            dt_CMOD.Columns.Add("CalcM")
            dt_CMOD.Columns.Add("CalcD")
            dt_CMOD.Rows.Add()
            dt_CMOD.Rows.Add()

            dt_CMOD.Rows(0)(0) = "MKT"
            dt_CMOD.Rows(0)(1) = "MKT"
            dt_CMOD.Rows(1)(0) = "TFR"
            dt_CMOD.Rows(1)(1) = "TFR"



            ddlCalcMode.DataValueField = "CalcM"
            ddlCalcMode.DataTextField = "CalcD"

            ddlCalcMode.DataSource = dt_CMOD
            ddlCalcMode.DataBind()

            ddlCalcMode.SelectedValue = ConfigurationManager.AppSettings("DefaultCalcMethod").ToString.Trim()

            dt_CMOD = Nothing
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try


    End Sub
    Private Sub FillModelComcoMID()
        Try
            If Not ddlFamily_MOD.SelectedItem Is Nothing Then
                Dim dt_MOD As DataTable = clsModel.GetModellist("M", vers_MOD.Text, ddlFamily_MOD.SelectedItem.Text)

                ddlModel_MOD.DataValueField = "ModelNum"
                ddlModel_MOD.DataTextField = "ModelNumDes"

                ddlModel_MOD.DataSource = dt_MOD

                ddlModel_MOD.DataBind()
            End If

        Catch ex As Exception

            GeneralException.BuildError(Page, ex.Message)
        End Try


    End Sub

    Private Sub FillFamilylist()
        Try
            Dim dt_MOD As DataTable = clsModel.GetFamilylist("M")

            ddlFamily_MOD.DataValueField = "ModelName"
            ddlFamily_MOD.DataTextField = "ModelName"

            ddlFamily_MOD.DataSource = dt_MOD

            ddlFamily_MOD.DataBind()
        Catch ex As Exception

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub StartQuote(sOpenType As String, FamilyId As String)
        Try

            txtMailAddressNote.Visible = False
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceCalculateFlag, ddlCalcMode.SelectedItem.Text.ToString)
            Dim dt As DataTable
            If sOpenType = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                dt = clsQuatation.Get_CreateTimeStampUniqID(sOpenType, lang.Text, vers.Text, FamilyId, "iQuote-ADMIN", itemNumbe.Text)
            Else
                Dim itn As String = ""
                Try
                    itn = ddlModel_Items_MOD.SelectedValue
                Catch ex As Exception
                    itn = ""
                End Try
                If itn Is Nothing Then itn = ""

                dt = clsQuatation.Get_CreateTimeStampUniqID(sOpenType, lang.Text, vers.Text, FamilyId, "iQuote-ADMIN", itn)
            End If


            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Dim TimeStamp As String = dt.Rows(0).Item(0).ToString
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TimeStamp, TimeStamp)


                Dim appId As String = ConfigurationManager.AppSettings("ApplicationID")
                Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
                Response.Redirect("~/Forms/Login.aspx" & uniqueID & "iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), False)

            End If

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub


    Private Sub startConfig_Click(sender As Object, e As EventArgs) Handles startConfig.Click
        Try


            StartQuote(clsQuatation.Enum_QuotationOpenType.CONFIGURATOR, 0)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub StartModification_Click(sender As Object, e As EventArgs) Handles StartModification.Click
        Try
            If ddlFamily_MOD.SelectedIndex > -1 AndAlso ddlModel_Items_MOD.SelectedIndex > -1 Then

                StartQuote(clsQuatation.Enum_QuotationOpenType.MODIFICATION, ddlModel_MOD.SelectedItem.Value)

            End If

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub ddlFamily_MOD_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFamily_MOD.SelectedIndexChanged
        Try
            FillModCombo()
            FillGridItems()
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try

        FillModCombo()

    End Sub
    Private Sub FillModCombo()
        Try
            If Not ddlModel_MOD.SelectedItem Is Nothing Then
                Dim s As String = vers_MOD.Text
                Dim dt As DataTable = clsCatFamilyItems.GetCatFamilyItems_AS400(ddlModel_MOD.SelectedItem.Value, vers_MOD.Text)

                ddlModel_Items_MOD.DataValueField = "GICAT"
                ddlModel_Items_MOD.DataTextField = "GICAT"
                Dim dr As DataRow = dt.NewRow
                dt.Rows.InsertAt(dr, 0)
                ddlModel_Items_MOD.DataSource = dt

                ddlModel_Items_MOD.DataBind()
            End If

        Catch ex As Exception

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub ddlModel_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            FillModCombo()
            FillGridItems()
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub

    Private Sub FillGridItems()
        Try


            Dim dt As DataTable = ddlModel_Items_MOD.DataSource
            If Not dt Is Nothing Then
                dt.Rows.RemoveAt(0)
                dgvItems.DataSource = dt
                dgvItems.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub dgvItems_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvItems.RowCommand
        Try
            If e.CommandName = "GICAT" Then
                ddlModel_Items_MOD.SelectedValue = CType(dgvItems.Rows(e.CommandArgument).Cells(2).Controls(0), Button).Text.ToString.Trim
                disenModCombo()
            End If
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub QuotationListMod_Click(sender As Object, e As EventArgs) Handles QuotationListMod.Click
        Try
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempConfigDefaultShow, "")

            Dim uniqueID As String = "?"
            Response.Redirect("../Default.aspx" & uniqueID & "iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), False)

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub
    Private Sub rbIT_CheckedChanged(sender As Object, e As EventArgs) 'Handles rbIT.CheckedChanged
        'If rbIT.Checked Then
        '    FillData("rbIT")
        'End If
    End Sub
    Private Sub rbIMM_CheckedChanged(sender As Object, e As EventArgs) 'Handles rbIMM.CheckedChanged
        'If rbIMM.Checked Then
        '    ' StateManager.Clear_State()
        '    FillData("mm")

        'End If
    End Sub

    Private Sub rbINCH_CheckedChanged(sender As Object, e As EventArgs) 'Handles rbINCH.CheckedChanged
        'If rbINCH.Checked Then
        '    ' StateManager.Clear_State()
        '    FillData("i")
        'End If
    End Sub
    Private Sub FillData(BR As String)
        Try


            If BR = "" Then

                Try
                    For Each c As Control In pnlRd.Controls
                        If c.GetType = GetType(RadioButton) Then
                            If CType(c, RadioButton).Checked Then
                                BR = c.ID.Replace("rd", "")
                                Exit For
                            End If
                        End If
                    Next
                Catch ex As Exception

                End Try



                'If rbIMM.Checked Then
                '    BR = "mm"
                'End If
                'If rbINCH.Checked Then
                '    BR = "i"
                'End If
                'If rbIINCHZZ.Checked Then
                '    BR = "zz"
                'End If
                'If rbISmm.Checked Then
                '    BR = "rbISmm"
                'End If
                'If rbIT.Checked Then
                '    BR = "rbIT"
                'End If
            End If

            If BR = "" Then Exit Sub
            Dim brmi As String = ""
            If rbMMmain.Checked Then brmi = "M"
            If rbInchmain.Checked Then brmi = "I"

            'txtBranchCode.Text = BR

            UpdatePanel1.Update()
            '  txtBranchNo.Text = CType(pnlRd.FindControl("lblBranchNumbervl" & BR), TextBox).Text
            vers_MOD.Text = brmi
            vers.Text = brmi
            '  txtCustomerT.Text = CType(pnlRd.FindControl("lblCustomerNumbervl" & BR), TextBox).Text ' txt_customer1.Text ' 50400


            'If BR = "" Then

            '    If rbIMM.Checked Then
            '        BR = "mm"
            '    End If
            '    If rbINCH.Checked Then
            '        BR = "i"
            '    End If
            '    If rbIINCHZZ.Checked Then
            '        BR = "zz"
            '    End If
            '    If rbISmm.Checked Then
            '        BR = "rbISmm"
            '    End If
            '    If rbIT.Checked Then
            '        BR = "rbIT"
            '    End If
            'End If

            'If BR = "mm" Then


            'ElseIf BR = "rbIT" Then
            '    txtBranchCode.Text = "IT"
            '    txtBranchNo.Text = BranchNo_MC_IT.Text ' 311000
            '    vers_MOD.Text = brmi
            '    vers.Text = brmi
            '    txtCustomerT.Text = txt_customer1_IT.Text ' 50400
            'ElseIf BR = "i" Then
            '    txtBranchCode.Text = "JU"
            '    txtBranchNo.Text = branch_MOD11.Text ' 100475
            '    vers_MOD.Text = brmi
            '    vers.Text = brmi
            '    txtCustomerT.Text = txt_customer2.Text ' 184200
            'ElseIf BR = "zz" Then
            '    txtBranchCode.Text = "ZZ"
            '    txtBranchNo.Text = branch_MOD22.Text ' 999999
            '    vers_MOD.Text = brmi
            '    vers.Text = brmi
            '    txtCustomerT.Text = txt_customer3.Text ' 99991
            'ElseIf BR = "rbISmm" Then
            '    txtBranchCode.Text = "XZ"
            '    txtBranchNo.Text = branch_MOD33.Text
            '    vers_MOD.Text = brmi
            '    vers.Text = brmi
            '    txtCustomerT.Text = txt_customer4.Text
            'End If

            FillModelComcoMID()
            ddlModel_MOSelectedIndexChanged()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ddlModel_MOD_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlModel_MOD.SelectedIndexChanged, ddlFamily_MOD.SelectedIndexChanged
        Try
            FillModCombo()
            FillGridItems()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub ddlModel_MOSelectedIndexChanged()
        Try
            FillModCombo()
            FillGridItems()
        Catch ex As Exception

        End Try

    End Sub







    Private Function ChangeUserDataInGal() As Boolean
        Try
            Dim sMail As String = ""
            Try
                sMail = StateManager.GetValue(StateManager.Keys.s_loggedEmail, True)
            Catch ex As Exception
                sMail = ""
            End Try
            Dim md As String = txtMailAddress.Text
            If md.ToString.Contains("@") AndAlso md.ToString.Trim.Substring(0, 1) <> "@" AndAlso md.ToString.Trim.ToUpper.Contains("@ISCAR.CO.IL") Then
                If sMail = "" Then sMail = md
            End If
            Dim BranchCode As String = ""
            Dim GalCustomerNumber As String = ""

            Try
                For Each c As Control In pnlRd.Controls
                    If TypeOf c Is RadioButton Then
                        If CType(c, RadioButton).Checked Then
                            BranchCode = c.ID.Substring(2, 2)
                            Exit For
                        End If
                    End If
                Next
                For Each c As Control In pnlRd.Controls
                    If TypeOf c Is TextBox Then
                        If Not CType(c, TextBox).ID Is Nothing AndAlso CType(c, TextBox).ID.ToString = "lblCustomerNumbervl" & BranchCode Then
                            GalCustomerNumber = CType(c, TextBox).Text
                            Exit For
                        End If
                    End If
                Next
            Catch ex As Exception

            End Try

            If BranchCode <> "" AndAlso BranchCode <> "" Then
                Try
                    Return GAL.ChangeUserBranchCodeAndNumberInGAL(sMail, BranchCode, GalCustomerNumber)
                Catch ex As Exception

                End Try

            End If

            Return False
        Catch ex As Exception
            'GeneralException.WriteEventErrors(ex.Message, GeneralException.e_LogTitle.GENERAL.ToString)

        End Try
    End Function

    Private Sub rbEmailMickiv_CheckedChanged(sender As Object, e As EventArgs) Handles rbEmailMickiv.CheckedChanged
        txtMailAddress.Text = "mickiv@iscar.co.il"
    End Sub

    Private Sub rbEmailNataly_CheckedChanged(sender As Object, e As EventArgs) Handles rbEmailNataly.CheckedChanged
        txtMailAddress.Text = "natalyp@iscar.co.il"
    End Sub

    Private Sub rbEmailTamar_CheckedChanged(sender As Object, e As EventArgs) Handles rbEmailTamar.CheckedChanged
        txtMailAddress.Text = "tamars@iscar.co.ill"

    End Sub

    Private Sub rbEmailBilal_CheckedChanged(sender As Object, e As EventArgs) Handles rbEmailBilal.CheckedChanged
        txtMailAddress.Text = "bilal@iscar.co.il"
    End Sub

    Private Sub Transition_Init(sender As Object, e As EventArgs) Handles Me.Init

    End Sub

    Private Sub Log_In()

        Try

            Dim QuotaionNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber).ToString
            If QuotaionNo = "" Then
                QuotaionNo = "0"
            End If
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FormName, HttpContext.Current.Request.UrlReferrer.AbsoluteUri.ToString)

            Dim siteType As String = ConfigurationManager.AppSettings("siteType")
            Dim appId As String = ConfigurationManager.AppSettings("ApplicationID")
            Dim Lang As String = ConfigurationManager.AppSettings("Lang")

            Dim mPage As String = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.ToString.Replace("http://iquote.ssl.imc-companies.com/", "https://iquote.ssl.imc-companies.com/")

            If ConfigurationManager.AppSettings("NewIMCDLogin") = "YES" Then
                Dim t As String = IMCID.GetIMCIDToken()
                Dim tt As String = ""
                Dim d As DataTable = LocalizationManager.Convert_JsonToDataTable(t)
                If Not d Is Nothing AndAlso d.Rows.Count > 0 Then
                    tt = d.Rows(0).Item("token").ToString
                End If
                Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
                HttpContext.Current.Response.Redirect(ConfigurationManager.AppSettings("LoginURLNEW") & uniqueID & "token=" & tt & "&sitetype=" & siteType & "&lang=" & Lang, False)
            Else
                Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
                HttpContext.Current.Response.Redirect(ConfigurationManager.AppSettings("LoginURL") & uniqueID & "sId=" & appId & "&sitetype=" & siteType & "&lang=" & Lang, False)


            End If
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub

    Private Sub Log_Out()
        Try

            Try
                StateManager.Clear_State(False)

                SessionManager.SetSessionDetails_Temporarily("TRUE")

                Dim siteType As String = ConfigurationManager.AppSettings("siteType")
                Dim appId As String = ConfigurationManager.AppSettings("ApplicationID")
                Dim Lang As String = ConfigurationManager.AppSettings("Lang")

                Dim mPage As String = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.ToString.Replace("http://iquote.ssl.imc-companies.com/", "https://iquote.ssl.imc-companies.com/")


                If ConfigurationManager.AppSettings("NewIMCDLogin").ToUpper = "YES" Then
                    Dim t As String = IMCID.GetIMCIDToken()
                    Dim tt As String = ""
                    Dim d As DataTable = LocalizationManager.Convert_JsonToDataTable(t)
                    If Not d Is Nothing AndAlso d.Rows.Count > 0 Then
                        tt = d.Rows(0).Item("token").ToString
                    End If
                    Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
                    HttpContext.Current.Response.Redirect(ConfigurationManager.AppSettings("LoginSignOutURLReqNEW") & uniqueID & "token=" & tt & "&sitetype=" & siteType & "&lang=" & Lang, False)
                Else
                    Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
                    HttpContext.Current.Response.Redirect(ConfigurationManager.AppSettings("LoginSignOutURLReq") & uniqueID & "sId=" & appId & "&sitetype=" & siteType & "&lang=" & Lang, False)
                End If

            Catch ex As Exception
                'GeneralException.WriteEventErrors("Error log in MONA LogOut - " & ex.Message, GeneralException.e_LogTitle.GENERAL.ToString)
            End Try

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub btnConnectToGal_Click(sender As Object, e As EventArgs) Handles btnConnectToGal.Click
        Try
            Dim BranchCodeCurrent As String = clsBranch.ReturnActiveBranchCodeState
            Dim loggedEmail As String = txtMailAddress.Text
            Dim BranchCodeNew As String = "" ' txtBranchCode.Text
            For Each c As Control In pnlRd.Controls
                If TypeOf c Is RadioButton Then
                    If CType(c, RadioButton).Checked Then
                        BranchCodeNew = c.ID.Substring(2, 2)
                        Exit For
                    End If
                End If
            Next
            ', loggedEmail As String, BranchCodeNew As String
            If BranchCodeNew <> "" Then
                clsUser.UpdateUserToUsersDomain(BranchCodeCurrent, loggedEmail, BranchCodeNew)
                StateManager.SetValueG(StateManager.Keys.s_iQutUsMail, txtMailAddress.Text)
                If ChangeUserDataInGal() = False Then
                    txtMailAddressNote.Visible = True
                Else
                    Log_Out()
                    Log_In()
                End If
            End If
        Catch ex As Exception

        End Try


    End Sub

    Private Sub Transition_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender


        If Not IsPostBack Then
            ChooseBranch()
            FillData(clsBranch.ReturnActiveBranchCodeState)
        End If
        Try
            imgLine.ImageUrl = "../media/ModelImages/SolidEndmills/" & ddlModel_MOD.SelectedValue & ".gif"
        Catch ex As Exception

        End Try
        Try
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "ScriptTransRR", "ShowFpanel();", True)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub rb_Hmm_CheckedChanged(sender As Object, e As EventArgs) Handles rb_Hmm.CheckedChanged
        Try


            vers_MOD.Text = "M"
            vers.Text = "M"
            FillModelComcoMID()
            FillModCombo()
            FillGridItems()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub rb_HInch_CheckedChanged(sender As Object, e As EventArgs) Handles rb_HInch.CheckedChanged
        Try


            vers_MOD.Text = "I"
            vers.Text = "I"
            FillModelComcoMID()
            FillModCombo()
            FillGridItems()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub rbMMmain_CheckedChanged(sender As Object, e As EventArgs) Handles rbMMmain.CheckedChanged, rbInchmain.CheckedChanged
        Try
            FillData("")
        Catch ex As Exception

        End Try

    End Sub

End Class

