Imports System.Drawing
Imports System.EnterpriseServices
Imports System.IO
Imports System.Net.NetworkInformation
Imports System.Net.WebRequestMethods
Imports SemiApp_bl


Public Class QuotationsList
    Inherits System.Web.UI.Page

    Private Enum E_QuotationListGrid
        sSelect = 0
        QuotationNum = 1
        AS400QuotationNum = 2
        OpenTypeDesp = 3
        SemiToolDescription = 4
        QuotationDat = 5
        ExpiredDate = 6
        PricesChaneged = 7
        imgLastUpdate = 8
        LastUpdate = 9
        ReportBranchCode = 10
        Expired = 11
        Ordered = 12
        Status = 13
        ModelNum = 14
        BranchCode = 15
        OfferBy_ID = 16
        OpenType = 17
        'ImageOrdered = 18
        itemNumber = 18
        Duplivcate = 19
        Renew = 20
        loggedEmail = 21
        'Delete = 17
        TemporarilyQuotation = 22
        CustomerNumber = 23
        CustomerName = 24
        FolderPath = 25
        Submitted = 26
    End Enum

    Private _Submitted As String = "Submitted"
    Private _Created As String = "Created"
    Private _Ordered As String = "Ordered"
    'Private sortExp As String
    'Private sortDir As String
    Private selectCustomer As String = "Select Customer"
    Private _doCheckLanguage As Boolean = False
    Private Property SortDirection As String
        Get
            Return IIf(ViewState("SortDirection") IsNot Nothing, Convert.ToString(ViewState("SortDirection")), "ASC")
        End Get
        Set(value As String)
            ViewState("SortDirection") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            '--------------------SELECTED FLAG/LANGUAGE--------------
            Dim selectedBC As String = "ZZ"
            If clsLanguage.CheckIfLanguageSelected(CType(Master.FindControl("hfLanguageselected"), HiddenField).Value, selectedBC) = True Then
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetCaptionForLabelsD", "SetCaptionForLabels()", True)
                _doCheckLanguage = True
            End If
            '--------------------------------------------------------

        Catch ex As Exception

        End Try

        Try

            If Idinty.CheckSesstionTimeOutQuotList = True Then
                If ConfigurationManager.AppSettings("IsDebugMode").ToString.ToUpper = "TRUE" Then
                    Response.Redirect("http://localhost:60377/Default.aspx?STARTFB=STARTFB_N", True)
                ElseIf ConfigurationManager.AppSettings("IsDebugMode").ToString.ToUpper = "TEST" Then
                    Response.Redirect("http://dmstest/iQuote/Default.aspx?STARTFB=STARTFB_N", True)
                Else
                    Response.Redirect("https://iquote.ssl.imc-companies.com/iQuote/Default.aspx?STARTFB=STARTFB_N", True)
                End If

            Else

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.vers_OnlyForNew, "")

                Dim mybtnUnit As Button = CType(Master.FindControl("btnUnit"), Button)
                Dim mybtnUnit1 As Button = CType(Master.FindControl("btnUnitC"), Button)
                'Dim myMasterPanel As Panel = CType(Master.FindControl("pnlSwichUpper"), Panel)
                mybtnUnit.Enabled = False
                mybtnUnit1.Enabled = False
                'myMasterPanel.Visible = False

                If clsQuatation.IsTemporaryCustomerShowType Then
                    txtConnect.Enabled = False
                    txtConnect.Visible = False
                    'btnConnect.Enabled = False
                    'btnConnect.Visible = False
                    AddtoMyQuotations.Enabled = False
                    AddtoMyQuotations.Visible = False
                End If


                ddlBranch.Visible = False
                ddlBranch.Enabled = False
                divBS.Visible = False
                divBC.Visible = False
                ddlCustomer.Visible = False
                ddlCustomer.Enabled = False

                Dim sLogId As String = ""
                Try
                    sLogId = StateManager.GetValue(StateManager.Keys.s_loggedEmail, True)
                Catch ex As Exception

                End Try



                If Not IsPostBack Then
                    SessionManager.Clear_ALL_Sessions()
                    SessionManager.Clear_ALL_SessionsTimeStamp()

                    SetTabs()


                    Try

                        If sLogId Is Nothing Then

                            Try
                                Response.Redirect("~/Default.aspx?iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), False)
                            Catch ex As Exception
                                ' Response.Redirect("~/Default.aspx", False)
                            End Try

                        End If
                        If sLogId.ToString = "" Then

                            Try
                                Response.Redirect("~/Default.aspx?iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), False)
                            Catch ex As Exception
                                '  Response.Redirect("~/Default.aspx", False)
                            End Try
                        End If
                    Catch ex As Exception
                        '  Response.Redirect("~/Default.aspx", False)
                    End Try

                End If




                If sLogId <> "" Then
                    txtSearchAll.DataBind()

                    If txtSearchAll.Text <> "" Then
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempSearchAny, txtSearchAll.Text)
                    End If

                    Try
                        If ddlBranch.SelectedValue.ToString <> "" Then
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempSearchBranch, ddlBranch.SelectedValue)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If ddlCustomer.SelectedValue.ToString <> "" Then
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempSearchCustomer, ddlCustomer.SelectedValue)
                        End If
                    Catch ex As Exception

                    End Try

                    If Not IsPostBack OrElse _doCheckLanguage = True Then

                        'Try
                        '    SetCaptionsLanguage()
                        'Catch ex As Exception

                        'End Try
                        FillGrid()

                    End If
                End If

                'If Not IsPostBack Then

                '    If clsUser.SetIsUserAdmin = True Then
                '        ddlBranch.Visible = True
                '        dgvQuotations.Columns(E_QuotationListGrid.BranchCode).ItemStyle.CssClass = "css_GridItemiMportantSmall"
                '        dgvQuotations.Columns(E_QuotationListGrid.BranchCode).HeaderStyle.CssClass = "css_GridHeaderiMportantSmall"
                '        divBS.Visible = True
                '    End If
                '    fillDDlBranches()

                'End If
            End If
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try

        'Try
        '    SetCaptionsLanguage()
        'Catch ex As Exception

        'End Try

        'changed when added flag to the price form
        'Try
        '    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ManualydialogHideX", "ManualydialogHide();", True)
        'Catch ex As Exception

        'End Try
    End Sub

    Private Sub fillDDlBranches()
        Try

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempQuotationListCustomer, "")
            If Not IsPostBack Then
                Dim dtd As DataTable = clsBranch.GetBranches()
                If Not dtd Is Nothing Then
                    Dim dr As DataRow = dtd.NewRow
                    dtd.Rows.InsertAt(dr, 0)
                    ddlBranch.DataSource = dtd
                    ddlBranch.DataValueField = "BranchCode"
                    ddlBranch.DataTextField = "BR"
                    dr("BR") = "Select Branch"

                    ddlBranch.DataBind()
                    'Try
                    '    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempSearchBranch, ddlBranch.SelectedValue)
                    'Catch ex As Exception

                    'End Try

                End If
            End If

        Catch ex As Exception

        End Try

        'Throw New NotImplementedException()
    End Sub
    Private Sub fillDDlCustomers()

        'Try
        '    SetCaptionsLanguage()
        'Catch ex As Exception

        'End Try
        Try
            Dim br As String = ddlBranch.SelectedValue
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempQuotationListCustomer, "")

            If br = "" Then
                br = clsBranch.ReturnActiveBranchCodeState
            End If

            Dim dtd As DataTable = customer.Get_CustomerNameNoList(br)
            If Not dtd Is Nothing Then
                Dim dr As DataRow = dtd.NewRow
                dtd.Rows.InsertAt(dr, 0)
                ddlCustomer.DataSource = dtd
                ddlCustomer.DataValueField = "CustomerNumber"
                ddlCustomer.DataTextField = "CustomerName"
                dr("CustomerName") = selectCustomer ' "Select Customer"

                ddlCustomer.DataBind()
                Try
                    If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchCustomer, False) <> "" Then
                        ddlCustomer.SelectedValue = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchCustomer, False)
                    End If
                Catch ex As Exception

                End Try
                Try
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempSearchCustomer, ddlCustomer.SelectedValue)
                Catch ex As Exception

                End Try
            End If

        Catch ex As Exception

        End Try

        'Throw New NotImplementedException()
    End Sub
    Private Sub SetTabs()

        wucTabs.tcModel = True
        wucTabs.tcMatirial = False
        wucTabs.tcParameters = False
        wucTabs.tcGetQuotation = False
        wucTabs.SelectedItem = wucTab.E_MNUiTEMS.MyQuotations
        'divUnit.Visible = False

        wucTabs.ItemsVisiblty()
    End Sub
    Private Sub FillGrid() 'Optional ByVal sortExpression As String = Nothing
        Try
            ' SetCaptionsLanguage()
            Try
                '--------------------SELECTED FLAG/LANGUAGE--------------
                Dim selectedBC As String = "ZZ"
                If clsLanguage.CheckIfLanguageSelected(CType(Master.FindControl("hfLanguageselected"), HiddenField).Value, selectedBC) = True Then
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetCaptionForLabelsD", "SetCaptionForLabels()", True)
                    ' _doCheckLanguage = True
                End If
                '--------------------------------------------------------

                SetCaptionsLanguage()
            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try
        'Exit Sub
        Try

            'Try
            '    sortExpression = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.tempListSortCol, False)
            'Catch ex As Exception
            '    sortExpression = ""
            'End Try



            Dim sAny As String = txtSearchAll.Text.ToString.Trim

            If sAny = "" Then
                sAny = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchAny, False)
            End If

            Dim dt As DataTable
            Dim BC As String = ""
            Try
                BC = clsBranch.ReturnActiveBranchCodeState

            Catch ex As Exception
                BC = ""
            End Try
            Dim loggedEmail As String = clsQuatation.ACTIVE_UseLoggedEmail

            Dim ssc As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchCustomer, False)

            Dim USerType As String = ""
            Dim sdtUserType As DataTable = clsUser.GetUserType(BC, loggedEmail, USerType)
            If Not sdtUserType Is Nothing AndAlso sdtUserType.Rows.Count > 0 Then


                fillDDlBranches()

                fillDDlCustomers()
            End If
            Dim QuotationBranchCode As String = ddlBranch.SelectedValue
            Dim QuotationCus As String = ddlCustomer.SelectedValue



            'Dim CN As String = ""
            'If ddlCustomer.SelectedIndex > 0 Then
            '    CN = ddlCustomer.SelectedValue
            'End If

            'Dim searchSesstion As String = chkSearchOrderd.Checked.ToString & ";"
            'searchSesstion &= chkSearchValid.Checked.ToString & ";"
            'searchSesstion &= chkSearchExpired.Checked.ToString & ";"
            'searchSesstion &= loggedEmail.ToString & ";"
            'searchSesstion &= sAny.ToString & ";"
            'searchSesstion &= QuotationBranchCode.ToString & ";"
            'searchSesstion &= ssc.ToString & ";"

            Dim sOR As Boolean = False
            Dim sOE As Boolean = False
            Dim sOS As Boolean = False
            'Dim sOB As Boolean = False
            Try
                If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchOrdered, False) Is Nothing Then
                    If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchOrdered, False).ToString.ToUpper = "FALSE" Or SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchOrdered, False).ToString.ToUpper = "TRUE" Then
                        sOR = CBool(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchOrdered, False))
                    End If
                End If
            Catch ex As Exception
            End Try
            Try
                If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchExpired, False) Is Nothing Then
                    If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchExpired, False).ToString.ToUpper = "FALSE" Or SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchExpired, False).ToString.ToUpper = "TRUE" Then
                        sOE = CBool(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchExpired, False))
                    End If
                End If
            Catch ex As Exception
            End Try
            Try
                If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchSubmitted, False) Is Nothing Then
                    If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchSubmitted, False).ToString.ToUpper = "FALSE" Or SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchSubmitted, False).ToString.ToUpper = "TRUE" Then
                        sOS = CBool(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchSubmitted, False))
                    End If
                End If
            Catch ex As Exception
            End Try


            Try
                If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchBranch, False) Is Nothing Then
                    QuotationBranchCode = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchBranch, False)
                End If
            Catch ex As Exception
            End Try
            Try
                If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchCustomer, False) Is Nothing Then
                    QuotationCus = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchCustomer, False)
                End If
            Catch ex As Exception
            End Try

            'dt = clsQuatation.getQuotationList(BC, "", "", sOR, chkSearchValid.Checked, sOE, sOS, loggedEmail, sAny, QuotationBranchCode, QuotationCus)
            dt = clsQuatation.getQuotationList(BC, "", "", sOR, False, sOE, sOS, loggedEmail, sAny, QuotationBranchCode, QuotationCus)
            If Not dt Is Nothing Then
                dt.Columns.Add("PricesChaneged")
                For Each drr As DataRow In dt.Rows
                    drr("customername") = drr("customername").ToString.Replace("      ", " ").Replace("     ", " ").Replace("    ", " ").Replace("   ", " ").Replace("  ", " ").Replace(" ", "_")
                Next
            End If


            Dim ColSort As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.tempListSortCol, False)
            Dim ColSortOrder As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.tempListSortOrder, False)


            If (Not (ColSort) Is Nothing) AndAlso ColSort <> "" AndAlso ColSortOrder <> "" Then 'Not (sortExpression) Is Nothing
                Dim dv As DataView = dt.AsDataView
                'Me.SortDirection = IIf(Me.SortDirection = "ASC", "DESC", "ASC")
                'sortDir = Me.SortDirection

                dv.Sort = ColSort & " " & ColSortOrder
                'sortExp = sortExpression

                'dv.Sort = sortExpression & " " & Me.SortDirection
                'sortExp = sortExpression
                'dgvQuotations.DataSource = dv
                lvQuotationListA.DataSource = dv
            Else
                'dgvQuotations.DataSource = dt
                lvQuotationListA.DataSource = dt

            End If

            'dgvQuotations.DataBind()
            'lvQuotationList.DataBind()
            lvQuotationListA.DataBind()


            divBS.Visible = False
            divBC.Visible = False
            ddlBranch.Visible = False
            ddlCustomer.Visible = False


            If Not lvQuotationListA Is Nothing AndAlso lvQuotationListA.Items.Count > 0 Then

                CType(lvQuotationListA.FindControl("imgbtnlvSORTAA"), ImageButton).CssClass = " display_non"
                CType(lvQuotationListA.FindControl("imgbtnlvSORTK"), ImageButton).CssClass = " display_non"

            End If

            For i As Integer = 0 To lvQuotationListA.Items.Count - 1
                If lvQuotationListA.Items(i).ItemType = ListViewItemType.DataItem Then
                    CType(lvQuotationListA.Items(i).FindControl("divQN"), HtmlGenericControl).Style.Add("display", "none")
                    CType(lvQuotationListA.Items(i).FindControl("divCUSno"), HtmlGenericControl).Style.Add("display", "none")
                    CType(lvQuotationListA.Items(i).FindControl("divBCA"), HtmlGenericControl).Style.Add("display", "none")
                    CType(lvQuotationListA.Items(i).FindControl("divBCA"), HtmlGenericControl).Style.Add("display", "none")

                    CType(lvQuotationListA.Items(i).FindControl("divCUSna"), HtmlGenericControl).Style.Add("display", "none")
                    CType(lvQuotationListA.Items(i).FindControl("lblvlQuotationNum"), Label).CssClass = " display_non"
                    CType(lvQuotationListA.Items(i).FindControl("lblvlloggedEmail"), Label).CssClass = " display_non"
                    CType(lvQuotationListA.Items(i).FindControl("lblvlBranchCode"), Label).CssClass = " display_non"
                    CType(lvQuotationListA.Items(i).FindControl("lblvlCustomerNumber"), Label).CssClass = " display_non"
                    CType(lvQuotationListA.Items(i).FindControl("lblvlCustomerName"), Label).CssClass = " display_non"
                    CType(lvQuotationListA.Items(i).FindControl("lblvlOpenTypeDesp"), Label).CssClass = " display_non"
                End If
            Next
            If Not lvQuotationListA Is Nothing AndAlso lvQuotationListA.Items.Count > 0 Then
                CType(lvQuotationListA.FindControl("pnliQuoteNO"), HtmlTableCell).Style.Add("display", "none")
                CType(lvQuotationListA.FindControl("pnliCusNO"), HtmlTableCell).Style.Add("display", "none")
                CType(lvQuotationListA.FindControl("pnliCusNA"), HtmlTableCell).Style.Add("display", "none")
                CType(lvQuotationListA.FindControl("divLoggedEmail"), HtmlTableCell).Style.Add("display", "none")
                CType(lvQuotationListA.FindControl("pnlBC"), HtmlTableCell).Style.Add("display", "none")
                CType(lvQuotationListA.FindControl("pnlMC"), HtmlTableCell).Style.Add("display", "none")
            End If


            If Not sdtUserType Is Nothing AndAlso sdtUserType.Rows.Count > 0 Then

                'fillDDlBranches()

                'fillDDlCustomers()

                Select Case USerType
                    Case clsUser.e_UserType.AdminALL
                        divBS.Visible = True
                        divBC.Visible = True
                        ddlBranch.Visible = True
                        ddlBranch.Enabled = True
                        ddlCustomer.Visible = True
                        ddlCustomer.Enabled = True
                        If Not lvQuotationListA Is Nothing AndAlso lvQuotationListA.Items.Count > 0 Then
                            CType(lvQuotationListA.FindControl("pnliQuoteNO"), HtmlTableCell).Style.Add("display", "block")
                            CType(lvQuotationListA.FindControl("pnliCusNO"), HtmlTableCell).Style.Add("display", "block !important")
                            CType(lvQuotationListA.FindControl("pnlBC"), HtmlTableCell).Style.Add("display", "block !important")
                            CType(lvQuotationListA.FindControl("pnlMC"), HtmlTableCell).Style.Add("display", "block !important")
                            CType(lvQuotationListA.FindControl("pnliCusNA"), HtmlTableCell).Style.Add("display", "block")
                            CType(lvQuotationListA.FindControl("pnliQuoteNOAs400"), HtmlTableCell).Style.Add("max-width", "100px")
                            CType(lvQuotationListA.FindControl("pnliQuoteNOAs400"), HtmlTableCell).Style.Add("min-width", "100px")
                            CType(lvQuotationListA.FindControl("divLoggedEmail"), HtmlTableCell).Style.Add("display", "block")

                            CType(lvQuotationListA.FindControl("pnlqutDated"), HtmlTableCell).Style.Add("max-width", "100px")
                            CType(lvQuotationListA.FindControl("pnlqutDated"), HtmlTableCell).Style.Add("min-width", "100px")
                            CType(lvQuotationListA.FindControl("pnlqutDatee"), HtmlTableCell).Style.Add("max-width", "100px")
                            CType(lvQuotationListA.FindControl("pnlqutDatee"), HtmlTableCell).Style.Add("min-width", "100px")
                        End If



                        'CType(lvQuotationListA.FindControl("lblHeadQutNo"), Label).Width = 80 '.Style.Add("max-width", "100px")
                        'CType(lvQuotationListA.FindControl("lblHeadQutNo"), Label)Type(lvQuotationListA.FindControl("lblHeadQutNo"), Label).Style.Add("min-width", "100px")

                        For i As Integer = 0 To lvQuotationListA.Items.Count - 1
                            If lvQuotationListA.Items(i).ItemType = ListViewItemType.DataItem Then
                                CType(lvQuotationListA.Items(i).FindControl("divTemplateQD"), HtmlGenericControl).Style.Add("max-width", "100px")
                                CType(lvQuotationListA.Items(i).FindControl("divTemplateQD"), HtmlGenericControl).Style.Add("min-width", "100px")
                                CType(lvQuotationListA.Items(i).FindControl("divTemplateQDE"), HtmlGenericControl).Style.Add("max-width", "100px")
                                CType(lvQuotationListA.Items(i).FindControl("divTemplateQDE"), HtmlGenericControl).Style.Add("min-width", "100px")

                                CType(lvQuotationListA.Items(i).FindControl("divQN"), HtmlGenericControl).Style.Add("display", "block")
                                CType(lvQuotationListA.Items(i).FindControl("divCUSno"), HtmlGenericControl).Style.Add("display", "block")
                                CType(lvQuotationListA.Items(i).FindControl("divBCA"), HtmlGenericControl).Style.Add("display", "block")
                                CType(lvQuotationListA.Items(i).FindControl("lblvlMC"), HtmlGenericControl).Style.Add("display", "block")
                                CType(lvQuotationListA.Items(i).FindControl("divCUSna"), HtmlGenericControl).Style.Add("display", "block")
                                CType(lvQuotationListA.Items(i).FindControl("lblvlQuotationNum"), Label).CssClass = " FontFamilyRoboto FontSizeRoboto13 AlignCenter"
                                CType(lvQuotationListA.FindControl("imgbtnlvSORTAA"), ImageButton).CssClass = " FontFamilyRoboto FontSizeRoboto13 AlignCenter"
                                CType(lvQuotationListA.FindControl("imgbtnlvSORTK"), ImageButton).CssClass = " FontFamilyRoboto FontSizeRoboto13 AlignCenter"
                                CType(lvQuotationListA.Items(i).FindControl("lblvlloggedEmail"), Label).CssClass = " css_GridItemiMportant FontFamilyRoboto FontSizeRoboto13 AlignCenter"
                                CType(lvQuotationListA.Items(i).FindControl("lblvlBranchCode"), Label).CssClass = " css_GridItemiMportant FontFamilyRoboto FontSizeRoboto13 AlignCenter"
                                CType(lvQuotationListA.Items(i).FindControl("lblvlCustomerNumber"), Label).CssClass = " css_GridItemiMportant FontFamilyRoboto FontSizeRoboto13 AlignCenter"
                                CType(lvQuotationListA.Items(i).FindControl("lblvlOpenTypeDesp"), Label).CssClass = " css_GridItemiMportant FontFamilyRoboto FontSizeRoboto13 AlignCenter"
                                CType(lvQuotationListA.Items(i).FindControl("lblvlCustomerName"), Label).CssClass = " css_GridItemiMportantportant FontFamilyRoboto FontSizeRoboto13 AlignCenter"
                                CType(lvQuotationListA.Items(i).FindControl("lblvlOpenTypeDesp"), Label).CssClass = " css_GridItemiMportantportant FontFamilyRoboto FontSizeRoboto13 AlignCenter"
                                CType(lvQuotationListA.Items(i).FindControl("lblvlAS400Number"), Label).Width = 100
                                CType(lvQuotationListA.Items(i).FindControl("divQNA400"), HtmlGenericControl).Style.Add("max-width", "100px")
                                CType(lvQuotationListA.Items(i).FindControl("divQNA400"), HtmlGenericControl).Style.Add("min-width", "100px")
                                'CType(lvQuotationListA.Items(i).FindControl("lblvlAS400Number"), Label).Style.Add("min-width", "100px")
                            End If
                        Next



                        If QuotationBranchCode = "" Then
                            'If sdtUserType.Rows(0)(0).ToString = "0" Then
                            'ddlBranch.SelectedValue = BC
                            fillDDlCustomers()
                            'End If
                        Else
                            'ddlBranch.SelectedValue = QuotationBranchCode
                            fillDDlCustomers()
                        End If
                    Case clsUser.e_UserType.BranchAdmin
                        CType(lvQuotationListA.FindControl("pnliCusNA"), HtmlTableCell).Style.Add("display", "block")
                        CType(lvQuotationListA.FindControl("divLoggedEmail"), HtmlTableCell).Style.Add("display", "block")

                        For i As Integer = 0 To lvQuotationListA.Items.Count - 1
                            If lvQuotationListA.Items(i).ItemType = ListViewItemType.DataItem Then
                                CType(lvQuotationListA.Items(i).FindControl("divCUSna"), HtmlGenericControl).Style.Add("display", "block")
                                CType(lvQuotationListA.Items(i).FindControl("divQNA400"), HtmlGenericControl).Style.Add("max-width", "216px")
                                CType(lvQuotationListA.Items(i).FindControl("divQNA400"), HtmlGenericControl).Style.Add("min-width", "216px")
                                CType(lvQuotationListA.Items(i).FindControl("lblvlAS400Number"), Label).Width = 216
                                CType(lvQuotationListA.Items(i).FindControl("lblvlloggedEmail"), Label).CssClass = " css_GridItemiMportant FontFamilyRoboto FontSizeRoboto13 AlignCenter"
                                CType(lvQuotationListA.Items(i).FindControl("lblvlBranchCode"), Label).CssClass = " css_GridItemiMportant FontFamilyRoboto FontSizeRoboto13 AlignCenter"
                                CType(lvQuotationListA.Items(i).FindControl("lblvlOpenTypeDesp"), Label).CssClass = " css_GridItemiMportant FontFamilyRoboto FontSizeRoboto13 AlignCenter"
                                CType(lvQuotationListA.Items(i).FindControl("lblvlCustomerName"), Label).CssClass = " css_GridItemiMportantportant FontFamilyRoboto FontSizeRoboto13 AlignCenter"
                            End If
                        Next
                        ddlCustomer.Visible = True
                        ddlCustomer.Enabled = True
                        divBC.Visible = True
                        ' Case clsUser.e_UserType.RegularUser

                    Case Else
                        For i As Integer = 0 To lvQuotationListA.Items.Count - 1
                            If lvQuotationListA.Items(i).ItemType = ListViewItemType.DataItem Then
                                CType(lvQuotationListA.Items(i).FindControl("divQNA400"), HtmlGenericControl).Style.Add("max-width", "216px")
                                CType(lvQuotationListA.Items(i).FindControl("divQNA400"), HtmlGenericControl).Style.Add("min-width", "216px")
                                CType(lvQuotationListA.Items(i).FindControl("lblvlAS400Number"), Label).Width = 216
                            End If
                        Next
                End Select
            End If



            'FindChengepPricesQuotyations()
            FindChengepPricesQuotyationsLV()

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub



    Private Sub btnSearchItemNo_Click(sender As Object, e As ImageClickEventArgs) Handles btnSearchAll.Click
        FillGrid()
    End Sub

    Private Sub QuotationsList_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try


            txtSearchAll.Text = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchAny, False)
            Try
                If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchOrdered, False) Is Nothing Then
                    Dim sO As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchOrdered, False).ToString
                    If sO.ToString.ToUpper = "FALSE" Or sO.ToString.ToUpper = "TRUE" Then
                        chkSearchOrderd.Checked = CBool(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchOrdered, False))
                    End If
                End If
                If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchExpired, False) Is Nothing Then
                    Dim sE As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchExpired, False).ToString
                    If sE.ToString.ToUpper = "FALSE" Or sE.ToString.ToUpper = "TRUE" Then
                        chkSearchExpired.Checked = CBool(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchExpired, False))
                    End If
                End If
                If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchSubmitted, False) Is Nothing Then
                    Dim sS As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchSubmitted, False).ToString
                    If sS.ToString.ToUpper = "FALSE" Or sS.ToString.ToUpper = "TRUE" Then
                        chkSearchSubmit.Checked = CBool(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchSubmitted, False))
                    End If
                End If
                Try
                    If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchBranch, False) Is Nothing Then
                        ddlBranch.SelectedValue = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchBranch, False)
                    End If
                Catch ex As Exception

                End Try
                Try
                    If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchCustomer, False) Is Nothing Then
                        ddlCustomer.SelectedValue = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempSearchCustomer, False)
                    End If
                Catch ex As Exception

                End Try






                Try
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "CheckDivWidthList", "SetDivHeightWithScrollQutLIst();", True)
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub




    Private Sub chkSearchExpired_CheckedChanged(sender As Object, e As EventArgs) Handles chkSearchExpired.CheckedChanged, chkSearchOrderd.CheckedChanged, chkSearchSubmit.CheckedChanged
        Try
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempSearchOrdered, chkSearchOrderd.Checked)
        Catch ex As Exception
        End Try
        Try
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempSearchSubmitted, chkSearchSubmit.Checked)
        Catch ex As Exception
        End Try
        Try
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempSearchExpired, chkSearchExpired.Checked)
        Catch ex As Exception
        End Try

        FillGrid()
    End Sub

    Private Sub btnDuplicateQuotation_Click(sender As Object, e As EventArgs) Handles btnDuplicateQuotation.Click
        Try

            SessionManager.SetSessionDetails_SEMI_Duplicate(lblToDuplicateBC.Text, lblToDuplicateQutNo.Text)
            Try
                Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
                Response.Redirect("QBuilder.aspx" & uniqueID & "iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), False)
            Catch ex As Exception
                ' Response.Redirect("QBuilder.aspx", False)
            End Try

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub


    'Private Sub SaveQuotationLocal(bc As String, QuotationNo As String)

    '    Try

    '        Dim errorMSG As String = ""

    '        Dim ssB As Boolean = False
    '        'If bc.ToUpper = "XZ" Then
    '        ssB = clsUpdateData.UpdateQuotationGAL(QuotationNo, QuotationNo, bc, "Quotation List SaveQuotationLocal")
    '        'End If

    '    Catch ex As Exception
    '        'GeneralException.WriteEventErrors(ex.Message, GeneralException.e_LogTitle.GENERAL.ToString)
    '        GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "QUOTATION LIST Get_ParamsGridVal", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

    '    End Try

    'End Sub



    Private Sub btnRenewQuotation_Click(sender As Object, e As EventArgs) Handles btnRenewQuotation.Click
        'Try
        '    clsUpdateData.RenewQuotation(lblToRenewQutNo.Text, lblToRenewBC.Text)
        '    FillGrid()
        'Catch ex As Exception
        '    GeneralException.BuildError(Page, ex.Message)
        'End Try
    End Sub

    'Private Sub btnDoDeleteQuotation_Click(sender As Object, e As EventArgs) Handles btnDoDeleteQuotation.Click
    '    Try
    '        Dim sQut As String = lblDeleteQuotationNo.Text
    '        Dim sBranchCode As String = lblDeleteBranchCode.Text
    '        Dim FP As String = lblFolderPath.Text
    '        If IsNumeric(sQut) AndAlso sBranchCode <> "" Then
    '            clsUpdateData.DeleteQuotation(sQut, sBranchCode, FP, True)
    '            FillGrid()
    '        End If
    '    Catch ex As Exception
    '        GeneralException.WriteEventErrors("Build2Dbson", ex.Message)
    '    End Try
    'End Sub

    Private Sub AddtoMyQuotations_Click(sender As Object, e As EventArgs) Handles AddtoMyQuotations.Click
        Dim s As String = txtConnect.Text
        If s <> "" Then
            FindQuotation(s)
        End If

    End Sub

    Private Sub FindQuotation(SerialNo As String)
        Try
            'Dim sB As String = "ZZ" ' StateManager.GetValue(StateManager.Keys.s_BranchCode, False)
            Dim dt As DataTable = clsQuatation.GetQuotationBySerialNo(SerialNo)
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Dim qut As String = dt.Rows(0).Item("QuotationNum").ToString
                Dim modcon As String = dt.Rows(0).Item("OpenType").ToString
                Dim TemporarilyQuotation As String = dt.Rows(0).Item("TemporarilyQuotation").ToString
                If TemporarilyQuotation = "True" Then

                    SessionManager.Clear_ALL_SessionsTimeStamp()
                    If start.StartForm(clsQuatation.e_QuotationStatus.Exist_QutOpenedFromQuotationList, modcon, "", "", "", "ZZ", "", "", "", "", qut, True, True) = True Then
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempDontGetPrice, "FALSE")
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempDoConnect, "DOCONNECT")

                        '-------------------                    
                        Dim displayTpe As String = "Prices.aspx"

                        displayTpe = "Prices.aspx"

                        Try
                            Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
                            Response.Redirect(displayTpe & uniqueID & "iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), False)
                        Catch ex As Exception
                            '  Response.Redirect(displayTpe, False)
                        End Try

                        '-------------------

                    End If
                End If
            Else
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertSNnE", "SerialNumberNotExistAlert();", True)
            End If

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub txtSearchAll_TextChanged(sender As Object, e As EventArgs) Handles txtSearchAll.TextChanged
        If txtSearchAll.Text = "" Then
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempSearchAny, "")
        End If
    End Sub

    Private Sub ddlBranch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlBranch.SelectedIndexChanged
        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempSearchCustomer, "")
        Try
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempSearchBranch, ddlBranch.SelectedValue)
        Catch ex As Exception

        End Try

        FillGrid()
    End Sub

    Private Sub txtSearchAll_DataBinding(sender As Object, e As EventArgs) Handles txtSearchAll.DataBinding
        Try

            txtSearchAll.Attributes.Add("onkeydown", "return EnterValueForSearch('" & txtSearchAll.ClientID & "', '" & btnSearchAll.ClientID & "');")
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub ddlCustomer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCustomer.SelectedIndexChanged
        Try
            If ddlCustomer.SelectedIndex > 0 Then
                'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempQuotationListCustomer, ddlCustomer.SelectedIndex)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempSearchCustomer, ddlCustomer.SelectedValue)
            Else
                'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempQuotationListCustomer, "")
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempSearchCustomer, "")
            End If
            FillGrid()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub btnRedirectPrices_Click(sender As Object, e As EventArgs) Handles btnRedirectPrices.Click
        Response.Redirect(selecteddisplayType.Text, False)
    End Sub

    Private Sub lvQuotationList_ItemDataBound(sender As Object, e As ListViewItemEventArgs) 'Handles lvQuotationList.ItemDataBound
        Try

            Try

                If (e.Item.ItemType = ListViewItemType.DataItem) AndAlso Not e.Item.Controls Is Nothing AndAlso e.Item.Controls.Count > 0 Then

                    CType(e.Item.Controls(1).FindControl("lbllvQuotationNumber"), Label).Text = CType(e.Item.DataItem(), DataRowView)("QuotationNum").ToString.Trim
                    CType(e.Item.Controls(1).FindControl("lbllvQuotationDesc"), Label).Text = CType(e.Item.DataItem(), DataRowView)("SemiToolDescription").ToString.Trim


                    'CType(e.Item.Controls(1).FindControl("btnModelname1"), Button).Text = CType(e.Item.DataItem(), DataRowView)("MANAMO").ToString.Trim
                    'CType(e.Item.Controls(1).FindControl("lblFamilyCode"), Label).Text = CType(e.Item.DataItem(), DataRowView)("MANUM").ToString.Trim
                    'CType(e.Item.Controls(1).FindControl("imgModelIcon1"), ImageButton).CommandArgument = e.Item.DataItemIndex

                    'If CType(e.Item.DataItem(), DataRowView)("Active").ToString.ToUpper = "TRUE" Then
                    '    CType(e.Item.Controls(1).FindControl("imgModelIcon1"), ImageButton).CssClass = "img-fluid img-thumbnail ApplicationListItemDivMainF ModelIconButton"
                    '    CType(e.Item.Controls(1).FindControl("btnModelname1"), Button).Enabled = True
                    '    CType(e.Item.Controls(1).FindControl("btnModelname1"), Button).CssClass = "ModelCaptionButton"
                    'Else
                    '    CType(e.Item.Controls(1).FindControl("imgModelIcon1"), ImageButton).CssClass = "img-fluid img-thumbnail imgModelIconFilterF ModelIconButton"
                    '    CType(e.Item.Controls(1).FindControl("btnModelname1"), Button).Enabled = False
                    '    CType(e.Item.Controls(1).FindControl("btnModelname1"), Button).CssClass = "ModelCaptionButtonfilter"
                    'End If

                End If

            Catch ex As Exception
                GeneralException.BuildError(Page, ex)
            End Try

        Catch ex As Exception
            GeneralException.BuildError(Page, ex)
        End Try
    End Sub

    Protected Sub OnPagePropertiesChanging(sender As Object, e As PagePropertiesChangingEventArgs)
        TryCast(lvQuotationListA.FindControl("DataPager1"), DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        FillGrid()

    End Sub

    Private Sub FindChengepPricesQuotyationsLV()
        Try

            'If ConfigurationManager.AppSettings("AS400UpdatePricesTransaction") = "YES" Then



            Dim YesPricesRevisionExist As Boolean = False
                For i As Integer = 0 To lvQuotationListA.Items.Count - 1
                    SessionManager.Clear_Sessions_OpenIquote("FALSE")
                    If lvQuotationListA.Items(i).ItemType = ListViewItemType.DataItem Then
                        YesPricesRevisionExist = False
                        Dim BranchCode As String = CType(lvQuotationListA.Items(i).FindControl("lblvlBranchCode"), Label).Text
                        Dim QutNumQ As String = CType(lvQuotationListA.Items(i).FindControl("lblvlQuotationNum"), Label).Text
                        Dim QutNum As String = CType(lvQuotationListA.Items(i).FindControl("lblvlAS400Number"), Label).Text
                        Dim CustomerNo As String = CType(lvQuotationListA.Items(i).FindControl("lblvlCustomerNumber"), Label).Text
                        Dim Qexp As String = CType(lvQuotationListA.Items(i).FindControl("lbllvExpired"), Label).Text.ToString.ToUpper

                        If Qexp = "FALSE" AndAlso QutNumQ <> QutNum AndAlso IsNumeric(QutNum) AndAlso CInt(QutNum) > 0 Then

                        Dim dtS As DataTable
                        'If ConfigurationManager.AppSettings("AS400UpdatePricesTransaction") = "YES" Then
                        dtS = GAL.GetGalData("PRICES", BranchCode, ConfigurationManager.AppSettings("AS400APPpathForGetData"), CustomerNo, "", "", "", "", "", "", QutNum, False)
                        'Else
                        '    dtS = Nothing
                        'End If


                        Dim dtQuotePrices As DataTable = clsQuatation.GeQuotationPrices(BranchCode, QutNumQ)
                            Dim modcon As String = CType(lvQuotationListA.Items(i).FindControl("lblvlOpenType"), Label).Text ' r.Cells(E_QuotationListGrid.OpenType).Text.ToString.ToUpper
                            If QutNumQ = "" Or QutNumQ = "&nbsp;" Then Exit Sub
                            If Not IsNumeric(QutNumQ) Then Exit Sub
                            Dim didTrans As Boolean = False
                            Dim lu As String = ""
                            Dim doContinueCheck As Boolean = True
                            Dim dDaynew As String = ""
                            Dim dMonthnew As String = ""
                            Dim dYearnew As String = ""
                            Dim quantityChenged As String = ""

                            Try
                                If Not dtS Is Nothing AndAlso dtS.Rows.Count > 0 AndAlso Not dtQuotePrices Is Nothing AndAlso dtQuotePrices.Rows.Count > 0 Then
                                    For Each rGal As DataRow In dtS.Rows
                                        If Not CDbl(rGal("unitprice")) > 0 Then
                                            doContinueCheck = False
                                            Exit For
                                        End If
                                    Next
                                Else
                                    doContinueCheck = False
                                End If
                            Catch ex As Exception
                                doContinueCheck = False
                            End Try

                            If doContinueCheck = True Then
                                For Each rGal As DataRow In dtS.Rows
                                    didTrans = False
                                    lu = ""
                                    For Each rQuote As DataRow In dtQuotePrices.Rows
                                        If rQuote("PriceRevision").ToString.Trim <> "0" Then
                                            YesPricesRevisionExist = True
                                        End If
                                        If rGal("quantityFrom").ToString = rQuote("QTY").ToString Then
                                            dYearnew = Left(rGal("lastTransaction"), 4).ToString
                                            dMonthnew = rGal("lastTransaction").ToString.Substring(4, 2)
                                            dDaynew = Right(rGal("lastTransaction"), 2).ToString
                                            If CDbl(rGal("unitprice")) <> CDbl(rQuote("NetPrice")) Then
                                                quantityChenged &= rGal("quantityFrom") & ","
                                                'r.Cells(E_QuotationListGrid.PricesChaneged).Text = quantityChenged
                                                CType(lvQuotationListA.Items(i).FindControl("lblvlPricesChaneged"), Label).Text = quantityChenged
                                                didTrans = True
                                                Try
                                                    lu = rGal("lasttransaction").ToString.Substring(6, 2) & "/" & rGal("lasttransaction").ToString.Substring(4, 2) & "/" & rGal("lasttransaction").ToString.Substring(0, 4)
                                                Catch ex As Exception
                                                    lu = ""
                                                End Try

                                                Exit For
                                            End If
                                        End If
                                    Next
                                    If didTrans = True Then
                                        Exit For
                                    End If
                                Next
                            End If

                            If didTrans = True Then
                                CType(lvQuotationListA.Items(i).FindControl("ImgPVA"), ImageButton).ImageUrl = "../media/Images/bluebell.png"
                                If lu <> "" AndAlso IsDate(lu) Then
                                    'r.Cells(E_QuotationListGrid.LastUpdate).Text = lu
                                    CType(lvQuotationListA.Items(i).FindControl("lbllvLastUpdate"), Label).Text = lu
                                End If
                            Else
                                If YesPricesRevisionExist = True Then
                                    CType(lvQuotationListA.Items(i).FindControl("ImgPVA"), ImageButton).ImageUrl = "../media/Images/greybell.png"
                                Else
                                    CType(lvQuotationListA.Items(i).FindControl("ImgPVA"), ImageButton).ImageUrl = "../media/Images/DEFAULT.png"
                                End If
                            End If
                        Else
                            CType(lvQuotationListA.Items(i).FindControl("ImgPVA"), ImageButton).ImageUrl = "../media/Images/DEFAULT.png"
                        End If
                    End If


                Next
            'End If

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub


    Private Sub CleareSortImage()
        Try
            Dim sOrderIMG As String = "../media/icons/none.svg"

            CType(lvQuotationListA.FindControl("imgbtnlvSORTA"), ImageButton).ImageUrl = sOrderIMG

            CType(lvQuotationListA.FindControl("imgbtnlvSORTB"), ImageButton).ImageUrl = sOrderIMG
            CType(lvQuotationListA.FindControl("imgbtnlvSORTC"), ImageButton).ImageUrl = sOrderIMG
            CType(lvQuotationListA.FindControl("imgbtnlvSORTD"), ImageButton).ImageUrl = sOrderIMG
            CType(lvQuotationListA.FindControl("imgbtnlvSORTE"), ImageButton).ImageUrl = sOrderIMG
            CType(lvQuotationListA.FindControl("imgbtnlvSORTF"), ImageButton).ImageUrl = sOrderIMG
            CType(lvQuotationListA.FindControl("imgbtnlvSORTAA"), ImageButton).ImageUrl = sOrderIMG
            CType(lvQuotationListA.FindControl("imgbtnlvSORTK"), ImageButton).ImageUrl = sOrderIMG
            CType(lvQuotationListA.FindControl("imgbtnlvSORTAcustNo"), ImageButton).ImageUrl = sOrderIMG
            CType(lvQuotationListA.FindControl("imgbtnlvSORTAcustNa"), ImageButton).ImageUrl = sOrderIMG
        Catch ex As Exception

        End Try
    End Sub

    Private Sub lvQuotationListA_ItemCommand(sender As Object, e As ListViewCommandEventArgs) Handles lvQuotationListA.ItemCommand
        Try

            Dim eArgListV As String = "" 'e.CommandArgument - CType(lvQuotationListA.FindControl("DataPager1"), DataPager).StartRowIndex


            Dim sOrderCol As String = ""
            Dim sOrderOrder As String = ""

            Try
                If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.tempListSortCol, False) Is Nothing Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortCol, "")
                End If
                If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.tempListSortOrder, False) Is Nothing Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortOrder, "")
                End If

                sOrderCol = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.tempListSortCol, False)
                sOrderOrder = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.tempListSortOrder, False)
            Catch ex As Exception

            End Try

            CleareSortImage()

            Dim sOrderIMG As String = "../media/icons/none.svg"
            If sOrderOrder = "ASC" Then sOrderIMG = "../media/icons/asc.svg" Else sOrderIMG = "../media/icons/desc.svg"


            Select Case e.CommandName.ToUpper
                Case "SELECTA"
                    eArgListV = e.CommandArgument - CType(lvQuotationListA.FindControl("DataPager1"), DataPager).StartRowIndex
                    Dim BranchCode As String = CType(lvQuotationListA.Items(eArgListV).FindControl("lblvlBranchCode"), Label).Text.ToString   ' dgvQuotations.Rows(e.CommandArgument).Cells(E_QuotationListGrid.BranchCode).Text.ToString
                    Dim CustomerNo As String = CType(lvQuotationListA.Items(eArgListV).FindControl("lblvlCustomerNumber"), Label).Text.ToString   ' dgvQuotations.Rows(e.CommandArgument).Cells(E_QuotationListGrid.CustomerNumber).Text.ToString
                    Dim QutNum As String = CType(lvQuotationListA.Items(eArgListV).FindControl("lblvlAS400Number"), Label).Text.ToString   'dgvQuotations.Rows(e.CommandArgument).Cells(E_QuotationListGrid.AS400QuotationNum).Text
                    Dim QutNumQ As String = CType(lvQuotationListA.Items(eArgListV).FindControl("lblvlQuotationNum"), Label).Text.ToString   ' dgvQuotations.Rows(e.CommandArgument).Cells(E_QuotationListGrid.QuotationNum).Text
                    Dim Qexp As String = CType(lvQuotationListA.Items(eArgListV).FindControl("lbllvExpired"), Label).Text.ToString.ToUpper   ' dgvQuotations.Rows(e.CommandArgument).Cells(E_QuotationListGrid.Expired).Text.ToString.ToUpper
                    Dim dtS As DataTable
                    If QutNumQ <> QutNum AndAlso IsNumeric(QutNum) AndAlso CInt(QutNum) > 0 Then
                        dtS = GAL.GetGalData("PRICES", BranchCode, ConfigurationManager.AppSettings("AS400APPpathForGetData"), CustomerNo, "", "", "", "", "", "", QutNum, True)
                    Else
                        'dtS = GAL.GetGalData("PRICES", BranchCode, ConfigurationManager.AppSettings("AS400APPpathForGetData"), CustomerNo, "", "", "", "", "", "", QutNum, False)
                    End If

                    Dim dtQuotePrices As DataTable = clsQuatation.GeQuotationPrices(BranchCode, QutNumQ)

                    'If dgvQuotations.Rows(e.CommandArgument).Cells(E_QuotationListGrid.QuotationNum).Text.Trim = "" Or dgvQuotations.Rows(e.CommandArgument).Cells(E_QuotationListGrid.QuotationNum).Text.Trim = "&nbsp;" Then Exit Sub
                    If CType(lvQuotationListA.Items(eArgListV).FindControl("lblvlQuotationNum"), Label).Text.ToString.Trim = "" Or CType(lvQuotationListA.Items(eArgListV).FindControl("lblvlQuotationNum"), Label).Text.ToString.Trim = "&nbsp;" Then Exit Sub

                    Dim modcon As String = CType(lvQuotationListA.Items(eArgListV).FindControl("lblvlOpenType"), Label).Text.ToString.Trim
                    Dim b As Boolean = start.StartForm(clsQuatation.e_QuotationStatus.Exist_QutOpenedFromQuotationList, modcon, "", "", "", BranchCode, "", "", "", "", QutNumQ, True, False)
                    If BranchCode = "ZZ" Then
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempDontGetPrice, "True")
                    End If
                    Dim displayTpe As String = "Prices.aspx"
                    ' Try
                    'If Request.Browser.IsMobileDevice Then
                    displayTpe = "Prices.aspx"
                    'End If
                    'Catch ex As Exception
                    'End Try
                    Dim didTrans As Boolean = False
                    Dim doContinueCheck As Boolean = True
                    Dim dDaynew As String = ""
                    Dim dMonthnew As String = ""
                    Dim dYearnew As String = ""
                    Dim quantityChenged As String = ""
                    If ConfigurationManager.AppSettings("AS400UpdatePricesTransaction") = "YES" AndAlso Qexp.ToString.ToUpper = "FALSE" AndAlso IsNumeric(QutNum) AndAlso QutNum > 0 Then
                        Try
                            If Not dtS Is Nothing AndAlso dtS.Rows.Count > 0 AndAlso Not dtQuotePrices Is Nothing AndAlso dtQuotePrices.Rows.Count > 0 Then
                                For Each rGal As DataRow In dtS.Rows
                                    If Not CDbl(rGal("unitprice")) > 0 Then
                                        doContinueCheck = False
                                        Exit For
                                    End If
                                Next
                            Else
                                doContinueCheck = False
                            End If
                        Catch ex As Exception
                            doContinueCheck = False
                        End Try
                        Dim PriceRevision As Integer = 0
                        If doContinueCheck = True Then
                            selecteddisplayType.Text = Request.UrlReferrer.AbsoluteUri.Replace("QuotationsList.aspx", displayTpe)
                            For Each rGal As DataRow In dtS.Rows
                                For Each rQuote As DataRow In dtQuotePrices.Rows
                                    If rGal("quantityFrom").ToString = rQuote("QTY").ToString Then
                                        dYearnew = Left(rGal("lastTransaction"), 4).ToString
                                        dMonthnew = rGal("lastTransaction").ToString.Substring(4, 2)
                                        dDaynew = Right(rGal("lastTransaction"), 2).ToString
                                        'If rGal("lastTransaction").ToString <> ClsDate.GetDateTimeReturnStringFormat(rQuote("LastUpdate"), ClsDate.Enum_DateFormatTypes.yyyymmdd) Then

                                        If IsNumeric(rGal("unitprice").ToString) AndAlso IsNumeric(rQuote("NetPrice")) Then
                                            Dim cdbU As String = rGal("unitprice").ToString.Replace(",", ".")
                                            Dim cdbQ As String = rQuote("NetPrice").ToString.Replace(",", ".")
                                            'If CDbl(cdbU) <> CDbl(cdbQ) Then
                                            If Format(CDbl(cdbU), "#.##") <> Format(CDbl(cdbQ), "#.##") Then

                                                'If Format(CDate(dDaynew & "/" & dMonthnew & "/" & dYearnew), "dd/MM/yy") > Format(CDate(rQuote("LastUpdate")), "dd/MM/yy") Then
                                                quantityChenged &= rGal("quantityFrom") & ","
                                                clsUpdateData.UpdateQuotationPricesTransaction(rGal("lastTransaction"), cdbU, rGal("quantityFrom"), PriceRevision)
                                                'Dim dtp As DataTable = clsQuatation.GetQuatationPrices(BranchCode, QutNumQ)
                                                'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Price, dtp)
                                                SessionManager.SetExistPrices(BranchCode, QutNumQ, CType(Nothing, DataTable))
                                                didTrans = True
                                                Exit For
                                                'End If

                                            End If

                                        End If
                                    End If
                                Next
                            Next
                        End If
                    End If

                    'quantityChenged = "2,3,"
                    'didTrans = TrueTransactionExist
                    If didTrans = True Then
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyReportBuild, "NO")
                        If quantityChenged.Length > 0 Then
                            If quantityChenged.Contains(",") Then
                                quantityChenged = quantityChenged.Substring(0, quantityChenged.Length - 1)
                            End If
                            'End If
                        End If
                        QutNum = QutNum ' & " (Quantity : " & quantityChenged.ToString & ")"
                        Dim sD As String = dDaynew.ToString & "/" & dMonthnew.ToString & "/" & dYearnew.ToString
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "ScriptTrans", "TransactionExist('" & QutNum & "' , '" & sD & "');", True)
                    Else

                        Try
                            Dim reLan As String = ""
                            Try
                                reLan = CType(lvQuotationListA.Items(eArgListV).FindControl("lblvlBranchCode"), Label).Text
                                Dim bBc As String = Path.GetFileName(CType(lvQuotationListA.Items(eArgListV).FindControl("ImageRepFlag"), ImageButton).ImageUrl).ToString.Substring(0, 2).ToString
                                reLan = CryptoManagerTDES.Encode(bBc)
                            Catch ex As Exception
                                reLan = CryptoManagerTDES.Encode(reLan)
                            End Try

                            Response.Redirect(displayTpe & "?rErepTr=" & Request("rErepTr") & "&iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & reLan, False)
                        Catch ex As Exception
                            '   Response.Redirect(displayTpe, False)
                        End Try
                    End If


                Case "CMDDUPLICATE"
                    eArgListV = e.CommandArgument - CType(lvQuotationListA.FindControl("DataPager1"), DataPager).StartRowIndex
                    lblToDuplicateQutNo.Text = CType(lvQuotationListA.Items(eArgListV).FindControl("lblvlQuotationNum"), Label).Text.ToString ' CType(lvQuotationListA.Items(i).FindControl("lblvlQuotationNum"), Label).Text ' dgvQuotations.Rows(e.CommandArgument).Cells(E_QuotationListGrid.QuotationNum).Text.ToString()
                    lblToDuplicateBC.Text = CType(lvQuotationListA.Items(eArgListV).FindControl("lblvlBranchCode"), Label).Text.ToString '  dgvQuotations.Rows(e.CommandArgument).Cells(E_QuotationListGrid.BranchCode).Text.ToString()

                    'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Script", "DuplicateConfirm('" & dgvQuotations.Rows(e.CommandArgument).Cells(E_QuotationListGrid.QuotationNum).Text.ToString() & "');", True)
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Script", "DuplicateConfirm();", True)

                Case "RENEWxxxxxxxxxxxx"
                    'eArgListV = e.CommandArgument - CType(lvQuotationListA.FindControl("DataPager1"), DataPager).StartRowIndexr
                    'lblToRenewQutNo.Text = dgvQuotations.Rows(e.CommandArgument).Cells(E_QuotationListGrid.QuotationNum).Text.ToString()
                    'lblToRenewBC.Text = dgvQuotations.Rows(e.CommandArgument).Cells(E_QuotationListGrid.BranchCode).Text.ToString()
                    'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Script", "RenewConfirm();", True)

                    'Case "DELETEQ"
                    '    lblDeleteQuotationNo.Text = dgvQuotations.Rows(e.CommandArgument).Cells(E_QuotationListGrid.QuotationNum).Text.ToString()
                    '    lblDeleteBranchCode.Text = dgvQuotations.Rows(e.CommandArgument).Cells(E_QuotationListGrid.BranchCode).Text.ToString()
                    '    lblFolderPath.Text = dgvQuotations.Rows(e.CommandArgument).Cells(E_QuotationListGrid.FolderPath).Text.ToString()
                    '    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Script", "DeleteQuotation1();", True)

                Case "SORTA"
                    If sOrderOrder = "ASC" Or sOrderOrder = "" Then sOrderOrder = "DESC" Else sOrderOrder = "ASC"
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortCol, "AS400Number")
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortOrder, sOrderOrder)
                    FillGrid()
                    CType(lvQuotationListA.FindControl("imgbtnlvSORTA"), ImageButton).ImageUrl = sOrderIMG
                Case "SORTB"
                    If sOrderOrder = "ASC" Or sOrderOrder = "" Then sOrderOrder = "DESC" Else sOrderOrder = "ASC"
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortCol, "SemiToolDescription")
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortOrder, sOrderOrder)
                    FillGrid()
                    CType(lvQuotationListA.FindControl("imgbtnlvSORTB"), ImageButton).ImageUrl = sOrderIMG

                Case "SORTC"
                    If sOrderOrder = "ASC" Or sOrderOrder = "" Then sOrderOrder = "DESC" Else sOrderOrder = "ASC"
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortCol, "quotationdate")
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortOrder, sOrderOrder)
                    FillGrid()

                    CType(lvQuotationListA.FindControl("imgbtnlvSORTC"), ImageButton).ImageUrl = sOrderIMG

                Case "SORTD"
                    If sOrderOrder = "ASC" Or sOrderOrder = "" Then sOrderOrder = "DESC" Else sOrderOrder = "ASC"
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortCol, "ExpiredDate")
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortOrder, sOrderOrder)
                    FillGrid()

                    CType(lvQuotationListA.FindControl("imgbtnlvSORTD"), ImageButton).ImageUrl = sOrderIMG

                Case "SORTE"
                    If sOrderOrder = "ASC" Or sOrderOrder = "" Then sOrderOrder = "DESC" Else sOrderOrder = "ASC"
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortCol, "LastUpdate")
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortOrder, sOrderOrder)
                    FillGrid()

                    CType(lvQuotationListA.FindControl("imgbtnlvSORTE"), ImageButton).ImageUrl = sOrderIMG

                Case "SORTF"
                    If sOrderOrder = "ASC" Or sOrderOrder = "" Then sOrderOrder = "DESC" Else sOrderOrder = "ASC"
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortCol, "status")
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortOrder, sOrderOrder)
                    FillGrid()

                    CType(lvQuotationListA.FindControl("imgbtnlvSORTF"), ImageButton).ImageUrl = sOrderIMG



                    'Case "SORTI"
                    '    If sOrderOrder = "ASC" Then sOrderOrder = "DESC" Else sOrderOrder = "ASC"
                    '    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortCol, "itemnumber")
                    '    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortOrder, sOrderOrder)
                    '    FillGrid()
                    'imgbtnlvSORTG

                Case "SORTAA"
                    If sOrderOrder = "ASC" Or sOrderOrder = "" Then sOrderOrder = "DESC" Else sOrderOrder = "ASC"
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortCol, "quotationnum")
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortOrder, sOrderOrder)
                    FillGrid()

                    CType(lvQuotationListA.FindControl("imgbtnlvSORTAA"), ImageButton).ImageUrl = sOrderIMG

                Case "SORTK"
                    If sOrderOrder = "ASC" Or sOrderOrder = "" Then sOrderOrder = "DESC" Else sOrderOrder = "ASC"
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortCol, "loggedemail")
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortOrder, sOrderOrder)
                    FillGrid()


                    CType(lvQuotationListA.FindControl("imgbtnlvSORTK"), ImageButton).ImageUrl = sOrderIMG

                Case "SORTACNA"
                    If sOrderOrder = "ASC" Or sOrderOrder = "" Then sOrderOrder = "DESC" Else sOrderOrder = "ASC"
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortCol, "customername")
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortOrder, sOrderOrder)
                    FillGrid()

                    CType(lvQuotationListA.FindControl("imgbtnlvSORTAcustNa"), ImageButton).ImageUrl = sOrderIMG
                Case "SORTACNO"
                    If sOrderOrder = "ASC" Or sOrderOrder = "" Then sOrderOrder = "DESC" Else sOrderOrder = "ASC"
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortCol, "customernumber")
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortOrder, sOrderOrder)
                    FillGrid()

                    CType(lvQuotationListA.FindControl("imgbtnlvSORTAcustNo"), ImageButton).ImageUrl = sOrderIMG

            End Select
        Catch ex As AuthenticationException
            Throw
        Catch ex As GeneralException
            GeneralException.BuildError(Page, ex)
        Catch ex As Exception
            GeneralException.BuildError(Page, ex)
        End Try
    End Sub

    Private Sub lvQuotationListA_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles lvQuotationListA.ItemDataBound
        Try

            'If e.Row.RowType = DataControlRowType.Header Then

            '    For i As Integer = 0 To e.Row.Cells.Count - 1
            '        Try
            '            If e.Row.Cells(i).Controls.Count > 0 AndAlso dgvQuotations.Columns(i).SortExpression.ToString <> "" Then
            '                Dim Link As LinkButton = CType(e.Row.Cells(i).Controls(0), LinkButton)
            '                Link.Attributes.Add("style", "text-decoration:none;")
            '            End If
            '        Catch ex As Exception

            '        End Try

            '    Next
            'End If

            If e.Item.ItemType = ListViewItemType.DataItem Then

                If IsDate(CType(e.Item.FindControl("lblvlExpiredDate"), Label).Text) Then
                    Dim d As DateTime = CType(e.Item.FindControl("lblvlExpiredDate"), Label).Text

                    CType(e.Item.FindControl("imgRenew"), ImageButton).Enabled = False

                    If d < Now.Date Then
                        CType(e.Item.FindControl("imgRenew"), ImageButton).Visible = True
                    Else
                        CType(e.Item.FindControl("imgRenew"), ImageButton).Visible = False
                    End If

                    'If CType(e.Item.FindControl("lblvlExpiredDate"), Label).Text.ToString.ToUpper = "TRUE" Then

                    'End If

                    If CType(e.Item.FindControl("lbllvOrdered"), Label).Text = "True" Then
                        CType(e.Item.FindControl("lvimgStatus"), ImageButton).ImageUrl = "../media/Icons/Ordered.png"
                        CType(e.Item.FindControl("lbllvStatus"), Label).Text = _Ordered ' " Ordered"
                    ElseIf CType(e.Item.FindControl("lbllvSubmitted"), Label).Text = "1" Then
                        CType(e.Item.FindControl("lvimgStatus"), ImageButton).ImageUrl = "../media/Icons/submitted.png"
                        CType(e.Item.FindControl("lbllvStatus"), Label).Text = _Submitted ' " Submitted"
                    Else
                        CType(e.Item.FindControl("lvimgStatus"), ImageButton).ImageUrl = "../media/Icons/Created.png"
                        CType(e.Item.FindControl("lbllvStatus"), Label).Text = _Created ' " Created"

                    End If
                End If

            End If


        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub SetCaptionsLanguage()
        Try
            Dim dv As DataView = clsLanguage.Get_LanguageCaption("Quotations")
            If Not dv Is Nothing AndAlso dv.Count > 0 Then
                Dim currentEnum As String = ""
                For i As Integer = 0 To dv.Count - 1
                    currentEnum = dv.Item(i).Item("LanguageEnumCode")
                    Select Case currentEnum
                        Case clsLanguage.e_LanguageConstants.txtSearchAll
                            'txtSearchAll.placeholder = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            Dim sTdf As String = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            txtSearchAll.Attributes.Add("placeholder", sTdf)
                        Case clsLanguage.e_LanguageConstants.chkSearchOrderd
                            chkSearchOrderd.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.chkSearchExpired
                            chkSearchExpired.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.chkSearchSubmit
                            chkSearchSubmit.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblTmyQut
                            Dim asdf As String = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            lblmql.Text = asdf
                            'Dim sDtre As String = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            'sDtre = CType(lvQuotationListA.FindControl("pnliQuoteNOAs400"), HtmlTableCell).InnerText.Replace("Quotation Number", sDtre)
                            hfGridQutNo.Value = asdf
                        Case clsLanguage.e_LanguageConstants.txtConnect
                            Dim sTdfDF As String = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            txtConnect.Attributes.Add("placeholder", sTdfDF)
                        Case clsLanguage.e_LanguageConstants.AddtoMyQuotations
                            AddtoMyQuotations.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblID
                            hfGridItemDesc.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.QuotationDate
                            hfQuotationDate.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblExpiryDate
                            hfExpiryDate.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblLastUpdate
                            hfLastUpdate.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.Status
                            hfStatus.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)

                        Case clsLanguage.e_LanguageConstants.ReportLanguage
                            hfReportLanguage.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            'Case clsLanguage.e_LanguageConstants.ReportLanguage
                            '    hfSatatus.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            'Case clsLanguage.e_LanguageConstants.
                            hfItemNumber.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.ItemNumber
                            hfItemNumber.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.CustomerName
                            hfCustomerName.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)

                        Case clsLanguage.e_LanguageConstants.SelectCustomer
                            If Not ddlCustomer.SelectedItem Is Nothing Then
                                selectCustomer = ddlCustomer.SelectedItem.Text.Replace("Select Customer", IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString))
                                ' selectCustomer = (CType(ddlCustomer.DataSource, DataTable)).Rows(0).Item("CustomerNAme").ToString.Replace("Select Customer", "Select Customer1")
                                'Case clsLanguage.e_LanguageConstants.lblTmyQut
                                fillDDlCustomers()
                            End If

                        Case clsLanguage.e_LanguageConstants._Ordered
                            _Ordered = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants._Submitted
                            _Submitted = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants._Created
                            _Created = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants._PriceChangeAlert1
                            lblAlertPriceCh1.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants._PriceChangeAlert2
                            lblAlertPriceCh2.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            lblAlertPriceCh2.Text = lblAlertPriceCh2.Text.Replace("dateRed", "<span style=color:red;>redDate</span>")
                            lblAlertPriceCh2.Text = lblAlertPriceCh2.Text.Replace("qutBlue", "<span style=color:blue;>qutBlue</span>")
                        Case clsLanguage.e_LanguageConstants.ViewQuotatiomn
                            lblVVqut.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)

                        Case clsLanguage.e_LanguageConstants.DuplicateQuotation
                            hfduplicateTitle.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.buttonCancel
                            hfduplicateButtonCancel.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.Duplicate
                            hfduplicateButton.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.DuplicateConfirm
                            hfduplicateMessage.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.First
                            hfFirst.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.Nextt
                            hfNext.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.Previous
                            hfPrevious.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)

                    End Select
                Next
            End If

        Catch ex As Exception

        End Try
    End Sub
End Class