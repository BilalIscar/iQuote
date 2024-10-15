Imports System.Drawing
Imports System.IO
Imports System.Reflection
Imports SemiApp_bl


Public Class QBuilder
    Inherits System.Web.UI.Page

    'Private displayTpe As String = "Prices.aspx"

    Private _lblTitle1Cap1 As String = "Your account request is being processed"
    Private _lblTitle1Cap2 As String = "You are almost there!"
    Private _lblTitle1Cap3 As String = "Please Note"

    Private _lblLGI_1 As String = "In the meantime, you can receive a temporary offer"
    Private _lblLGI_2 As String = "Please login to view your quotation"
    Private _lblLGI_3 As String = "<b>Modify to set your required<br>geometrical parameters.</b>"
    Private _lblLGI_4 As String = "<b><font color='red'>Please note:</font></b> clicking 'Get New Price'<br>"
    Private _lblLGI_5 As String = "View your quotation"
    Private _lblLGI_6 As String = "We are currently unable to show prices.<br>In the meantime, you can view your temporary technical offer.<br>You can use your <b>temporary technical quotation number to access iQuote and use the 'My Quotation' tab to log-in and get prices.</b>"
    Private _SaveQuotation_1 As String = "Get New Price"
    Private _SaveQuotation_2 As String = "Get Price"
    Private _SaveQuotation_3 As String = "Login & Get Price"



    Const _SPECIAL_FUNCTION_RELATION As String = "RELATION("
    Const _SPECIAL_FUNCTION_CROSLISTCHECK As String = "CROSLISTCHECK("
    Const _SPECIAL_FUNCTION_CROSLIST As String = "CROSLIST("
    Const _SPECIAL_FUNCTION_DisplayFromList As String = "DISPLAYFROMLIST("
    Const _SPECIAL_FUNCTION_CROS As String = "CROS("
    Const _SPECIAL_FUNCTION_QB As String = "QBFORMULA"
    'Const _SPECIAL_FUNCTION_SETREMARKS As String = "SETREMARKS("

    Private _ModelNumModification As Integer
    Private _ModelNumConfiguration As Integer

    Private _ModelModification As DataTable
    Private _ModelConfiguration As DataTable

    Private _dtParamListModification As DataTable
    Private _dtParamListConfiguration As DataTable

    Private dtParametersListView As DataTable

    Private _familyNo As String = ""
    Private _Quotation_Status As String = ""
    Private _BranchCode As String = ""
    Private _BranchNumber As String = ""

    Private _DC As String = ""
    Private _Customer As String = ""
    Private _QutNo As String = ""
    Private _Lang As String = ""
    Private _Vers As String = ""
    Private _ItemNumber As String = ""
    Private _MainPicL As String = ""
    Private _MainPicR As String = ""
    Private _loggedEmail As String = ""
    Private AllRedyBulidFormPostBack As Boolean = False
    Private SetRangeDefailtValue As Boolean = False

    Private CurrentParameterIndexEND As Boolean = False

    Private colwidth As Integer = 0

    Private bTemporarilyQuotation As Boolean

    Private _ShowMsg As Boolean = True
    Private ListVieItemCounter As Integer = 0
    Private SetCation As String = "Set"
    Private _doCheckLanguage As Boolean = False

    Private Property CurrentParameterIndex() As Integer
        Get
            Return lblCurrentParameterIndex.Text
        End Get
        Set(ByVal value As Integer)
            lblCurrentParameterIndex.Text = value
        End Set
    End Property

    Private Property isEditMode() As Boolean
        Get
            Return lblisEditMode.Text
        End Get
        Set(ByVal value As Boolean)
            lblisEditMode.Text = value
        End Set
    End Property

    Private Enum GridValuesIndex As Integer
        Order = 0

        OperationH = 1
        HeightLimitH = 2
        'Remarks = 3
        MinBtn = 3
        lblMakf = 4
        DetailsBtn = 5
        'Details = 7
        LowLimitH = 6
        Remarks = 7
        lblRemarks = 8
        RullNotation = 9
        Operation = 10
        LowLimit = 11
        HeightLimit = 12
        PictSelect = 13
        PictHelp = 14
        StringValue = 15
        RullNotationImage = 16
        'DescValue = 16
        'ImgPictHelp = 15
    End Enum

    Private Enum GridParamList As Integer

        SelectParameterIcon = 0
        TabIndex = 1
        Label = 2
        CostName = 3
        Measure = 4
        Order = 5
        Formula = 6
        Visible = 7
        PrevParam = 8
        MainParameters = 9
        'ParamType = 9
        'VisibleTable = 9
    End Enum


    Private Sub SetStartedVariables(ClearallATART As Boolean)
        Try

            Try
                AllRedyBulidFormPostBack = CType(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._AllRedyBulidFormPostBack, False), Boolean)
            Catch ex As Exception
                AllRedyBulidFormPostBack = False
            End Try

            bTemporarilyQuotation = clsQuatation.IsTemporary_Quotatiom()
            _QutNo = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)

            _Quotation_Status = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationStatus, False)

            _BranchCode = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)
            _familyNo = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.familyNo, False)
            _Customer = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True) ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.CustomerNumber, False)
            _ItemNumber = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber, False)
            _Lang = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.language, False)
            _Vers = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.vers, False)
            _MainPicL = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.mainPicL, False)
            _MainPicR = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.mainPicR, False)
            _loggedEmail = StateManager.GetValue(StateManager.Keys.s_loggedEmail, False) ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.loggedEmail, False)
            Dim sSuccess As Boolean = True


            If AllRedyBulidFormPostBack = False Then

                _DC = ""

                _ModelNumModification = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumModification, False)
                _ModelNumConfiguration = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumConfiguration, False)

                _ModelConfiguration = clsModel.GetModelDetails(Integer.Parse(_ModelNumConfiguration))
                _ModelModification = clsModel.GetModelDetails(Integer.Parse(_ModelNumModification))

                build_dt_Params()

                If _QutNo = "" Or (ClearallATART = True And _QutNo = "") Then

                    Dim _Start As String = clsQuatation.ACTIVE_OpenType.ToString 'returnCurentOpenType()
                    Dim dtParametersLabels As DataTable = clsModel.GetParamsArrayByVal(clsQuatation.ACTIVE_ModelNumber, _Start, 0, "") 'returnCurentModelNumber
                    dtParametersListView = dtParametersLabels
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParametersListView, dtParametersListView)
                    SessionManager.SetSessionDetails_SEMI_NewQuoatation(_Start, _MainPicL, _Customer, _ModelNumModification, _ModelNumConfiguration, _BranchCode, 0, _Lang, _Vers, _ItemNumber, _loggedEmail, "", _ModelConfiguration, _ModelModification, _familyNo, dtParametersListView, _ItemNumber)
                    If clsQuatation.ACTIVE_OpenType.ToString = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then 'returnCurentOpenType()

                        Get_ParamsGridVal(False, sSuccess)


                    End If

                    Set_ControlsStyle(False)
                    txtValue.DataBind()

                    SetCurrentParamIndex("", 1)

                    SetParameter(True)

                    CONTINUE_CONFIGURATOR()

                Else

                    Set_ControlsStyle(False)
                    txtValue.DataBind()
                    SetCurrentParamIndex("", CInt(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.CurrentParameterIndex)))

                    SetParameter(True)

                    CONTINUE_CONFIGURATOR()

                End If

                AllRedyBulidFormPostBack = True

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyBulidFormPostBack, True)
                FillImages()
            Else

                SetCurrentParamIndex("", CInt(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.CurrentParameterIndex)))

                _DC = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._DC, False)

                _ModelNumConfiguration = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumConfiguration, False)
                _ModelConfiguration = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelConfiguration, "")
                _ModelNumModification = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumModification, False)
                _ModelModification = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelModification, "")

                dtParametersListView = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParametersListView, "")

                build_dt_Params()

                If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._DC, False) Is Nothing Then
                    If _DC.ToString = "" Then
                        _DC = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._DC, False)
                    End If
                End If

                AllRedyBulidFormPostBack = True

                If Not IsPostBack OrElse _doCheckLanguage = True Then

                    txtValue.DataBind()
                    SetParameter(True)

                End If

                FillImages()

                txtValue.DataBind()
            End If


            If sSuccess = False Then
                lvParams.Visible = False
                dgvRulles.Visible = False
                divslider.Visible = False
                lblCurrnetParameterName.Visible = False
                Label1.Visible = False
                ImgMainImageL.Visible = False
                ImgMainImageR.Visible = False
                lblParamsDes.Visible = False
                ImageLeftBoderID.Visible = False
                ImgMainImageRID.Visible = False
                Exit Sub
            End If


        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)
            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub


    Private Sub build_dt_Params()

        Try

            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListConfiguration, "") Is Nothing Then

                Dim dtParametersConfiguration As DataTable = clsModel.GetModelParameters(_ModelNumConfiguration, True)
                _dtParamListConfiguration = dtParametersConfiguration.Clone()
                _dtParamListConfiguration.Columns.Add(New DataColumn("Measure"))
                _dtParamListConfiguration.Columns.Add(New DataColumn("Order"))
                _dtParamListConfiguration.Columns.Add(New DataColumn("PictSelect"))
                _dtParamListConfiguration.Columns.Add(New DataColumn("PictHelp"))
                _dtParamListConfiguration.Columns.Add(New DataColumn("PrevParam"))
                '_dtParamListConfiguration.Columns.Add(New DataColumn("StringValue"))
                '_dtParamListConfiguration.Columns.Add(New DataColumn("DescValue"))
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListConfiguration, _dtParamListConfiguration)
            Else
                _dtParamListConfiguration = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListConfiguration, "")
            End If




            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListModification, "") Is Nothing Then

                Dim dtParametersModification As DataTable = clsModel.GetModelParameters(_ModelNumModification, True)
                _dtParamListModification = dtParametersModification.Clone()
                _dtParamListModification.Columns.Add(New DataColumn("Measure"))
                _dtParamListModification.Columns.Add(New DataColumn("Order"))
                _dtParamListModification.Columns.Add(New DataColumn("PictSelect"))
                _dtParamListModification.Columns.Add(New DataColumn("PictHelp"))
                _dtParamListModification.Columns.Add(New DataColumn("PrevParam"))
                ' _dtParamListModification.Columns.Add(New DataColumn("StringValue"))
                '_dtParamListConfiguration.Columns.Add(New DataColumn("DescValue"))
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListModification, _dtParamListModification)
            Else
                _dtParamListModification = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListModification, "")
            End If


        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub

    Private Sub hideLogInButonIFDisabeldCheckLog()
        Try


            If ConfigurationManager.AppSettings("DoCheckLogIn") = "FALSE" Then
                btnSaveQuotation.Enabled = False
                btnSaveQuotation.Visible = False
                lblLGI.Visible = False
            End If
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

        End Try
    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            '--------------------SELECTED FLAG/LANGUAGE--------------
            Dim selectedBC As String = "ZZ"
            If clsLanguage.CheckIfLanguageSelected(CType(Master.FindControl("hfLanguageselected"), HiddenField).Value, selectedBC) = True Then
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetCaptionForLabelsD", "SetCaptionForLabels()", True)
                _doCheckLanguage = True
            End If
            '--------------------------------------------------------

            SetCaptionsLanguage()
        Catch ex As Exception

        End Try

        Try



            Try

                'If Request.Browser.IsMobileDevice Then
                '   displayTpe = "Prices.aspx"
                'Else
                'End If

            Catch ex As Exception

            End Try

            If Idinty.CheckSesstionTimeOut = True Then
                If ConfigurationManager.AppSettings("IsDebugMode").ToString.ToUpper = "TRUE" Then
                    Response.Redirect("http://localhost:60377/Default.aspx?STARTFB=STARTFB_N", True)
                ElseIf ConfigurationManager.AppSettings("IsDebugMode").ToString.ToUpper = "TEST" Then
                    Response.Redirect("http://dmstest/iQuote/Default.aspx?STARTFB=STARTFB_N", True)
                Else
                    Response.Redirect("https://iquote.ssl.imc-companies.com/iQuote/Default.aspx?STARTFB=STARTFB_N", True)
                End If


            Else
                If RullsCount.Text <> "END" Then
                    RullsCount.Text = "OPEN"
                End If


                Try
                    Dim s As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)


                    If Not IsNumeric(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumModification, False)) Or
                        Not IsNumeric(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumConfiguration, False)) Then
                        Try
                            Response.Redirect("../Default.aspx?iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), True)
                        Catch ex As Exception
                            ' Response.Redirect("../Default.aspx", True)
                        End Try
                    End If

                Catch ex As Exception
                    ' Response.Redirect("../Default.aspx", True)

                End Try


                If Not Request("GETPRICE") Is Nothing AndAlso Request("GETPRICE").ToString = "True" Then

                    StartAuthenticationProcess()
                    RedirectPrices()

                Else

                    SetStartedVariables(False)

                    Try

                        If ConfigurationManager.AppSettings("DoCheckLogIn") = "True" Then
                            btnSaveQuotation.Attributes.Add("onclick", "HideLastDiv();")
                        End If

                        btnLogOnNORegistration.Attributes.Add("onclick", "HideLastDiv();")

                    Catch ex As Exception
                        GeneralException.BuildError(Page, ex.Message)
                    End Try

                    Try
                        Dim ssV As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.vers, False)
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Set_vR", "setVerr('" & ssV & "');", True)
                    Catch ex As Exception

                    End Try

                End If
            End If




        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)


        End Try

        'changed when added flag to the price form
        'Try
        '    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ManualydialogHideX", "ManualydialogHide();", True)
        'Catch ex As Exception

        'End Try

    End Sub

    Private Sub SetTabs()
        Try


            wucTabs.tcModel = False
            wucTabs.tcMatirial = False
            wucTabs.tcParameters = True
            wucTabs.tcGetQuotation = False
            wucTabs.SelectedItem = wucTab.E_MNUiTEMS.Parameters

            wucTabs.ItemsVisiblty()
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

        End Try
    End Sub

    Private Sub DivFinalQuoteFormVisibilty(divA As Boolean, divB As Boolean, LastParameter As Boolean)
        Try


            divmyDivRightLast.Visible = divA    '   You are almost there! -- The offer is valid for the next 48 Houre
            ' If divB = True Then Stop
            If divB = True Then
                Try
                    Response.Redirect("~/Default.aspx?iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), True)
                Catch ex As Exception
                    'Response.Redirect("~/Default.aspx", False)
                End Try
            Else
                divmyDivRightLastF.Visible = divB   '   There is no reference found!<br>Please contact IMC suppoer.
                If LastParameter = True Then
                    Dim ssA As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._TempShowCustomerAlertLogIN, False).ToString.ToUpper

                    If ssA <> "EMPTY" Or ssA = "NO" Then
                        lblT1.Text = _lblTitle1Cap1 '"Your account request is being processed"
                        lblLGI.Text = _lblLGI_1 ' "In the meantime, you can receive a temporary offer"
                    Else
                        lblT1.Text = _lblTitle1Cap2 '"You are almost there!"
                        lblLGI.Text = _lblLGI_2 ' "Please login to view your quotation"
                    End If



                    Dim ot As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)
                    If ot = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                        Dim sDf As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagModificationCatalogDeferent, False)
                        Dim sbc As String = StateManager.GetValue(StateManager.Keys.s_BranchCode, False)
                        'lblT1.Text = "You are almost there!"
                        If sDf <> "TRUE" Then
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagModificationCatalogDeferent, "LASTPARAMDID")

                            lblLGI.Text = _lblLGI_3 ' "<b>Modify to set your required<br>geometrical parameters.</b>"
                            btnSaveQuotation.Visible = False
                            btnDiscardChanges.Visible = False
                            btnLogOnNORegistration.Visible = False
                        Else
                            'lblLGI.Text = "Please login to view your quotation"
                            btnSaveQuotation.Visible = True
                            btnDiscardChanges.Visible = False
                            'btnSaveQuotation.Width = 250
                            DivPriceAskLabelVisibility()
                        End If
                    Else

                        btnSaveQuotation.Visible = True
                        btnDiscardChanges.Visible = False
                        DivPriceAskLabelVisibility()
                    End If

                    hideLogInButonIFDisabeldCheckLog()
                End If
            End If



        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub
    Private Sub DivPriceAskLabelVisibility()
        Try
            btnDiscardChanges.Visible = False
            imgGalNote.ImageUrl = "../media/Icons/AR_l.svg"
            imgGalNote.Width = "30"
            imgGalNote.Height = "60"
            'pnlDife.Visible = True
            If Not clsQuatation.IsTemporary_Quotatiom AndAlso Not clsQuatation.IsTemporaryCustomer Then
                Dim sQutNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
                Dim sFlagParametersChanged As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagParametersChanged, False)
                If sQutNo <> "" And sFlagParametersChanged.ToString.ToUpper = "TRUE" Then
                    lblLGI.Text = _lblLGI_4 ' "<b><font color='red'>Please note:</font></b> clicking 'Get New Price'<br>"
                    lblLGI.Text &= "will replace the current data with the<br>"
                    lblLGI.Text &= "new set parameters data<br>"
                    Try
                        If Not clsQuatation.IsTemporaryLoggedEmail Then
                            Dim dtpr As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "")
                            If Not dtpr Is Nothing AndAlso dtpr.Rows.Count > 0 Then
                                If IsNumeric(dtpr.Rows(0).Item("Qty").ToString) Then
                                    btnDiscardChanges.Visible = True
                                End If
                            End If
                        Else
                            btnDiscardChanges.Visible = True
                        End If
                    Catch ex As Exception

                    End Try

                    btnDiscardChanges.Width = 150
                    btnSaveQuotation.Width = 150
                    btnSaveQuotation.Text = _SaveQuotation_1 ' "Get New Price"
                Else
                    lblLGI.Text = _lblLGI_5 ' "View your quotation"
                    btnSaveQuotation.Text = _SaveQuotation_2 ' "Get Price"
                End If

                btnLogOnNORegistration.Visible = False

            Else
                btnSaveQuotation.Text = _SaveQuotation_3
                btnLogOnNORegistration.Visible = True
                btnSaveQuotation.Visible = clsQuatation.IsTemporaryLoggedEmail
                hideLogInButonIFDisabeldCheckLog()


                Dim sErrorWSGAL As String = "FALSE"
                Try
                    sErrorWSGAL = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ErrorInWSGAL, False)
                Catch ex As Exception
                End Try

                If sErrorWSGAL = "TRUE" Then

                    Dim ot As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)
                    If ot = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                        Dim sDf As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagModificationCatalogDeferent, False)
                        If sDf = "TRUE" Then
                            lblT1.CssClass = "FontFamily FontSizeRoboto25 FForCbachColor"
                            lblT1.Text = _lblTitle1Cap3 '"Please Note"
                            lblLGI.Text = _lblLGI_6 ' "We are currently unable to show prices."
                            'lblLGI.Text &= "<br>In the meantime, you can view your temporary technical offer."
                            'lblLGI.Text &= "<br>You can use your <b>temporary technical quotation number to access iQuote and use the 'My Quotation' tab to log-in and get prices.</b>"


                            'System.Web.UI.ScriptManager.RegisterClientScri(Page, GetType(Page), "SetWorningImageGAL", "SetWorningImage()", True)
                            imgGalNote.ImageUrl = "../media/Icons/warningblue.png"
                            imgGalNote.Width = "80"
                            imgGalNote.Height = "72"
                            pnlDife.Visible = False
                        End If
                    Else
                        lblT1.CssClass = "FontFamily FontSizeRoboto25 FForCbachColor"
                        lblT1.Text = _lblTitle1Cap3 ' "Please Note"
                        lblLGI.Text = _lblLGI_6 ' "We are currently unable to show prices."
                        'lblLGI.Text &= "<br>In the meantime, you can view your temporary technical offer."
                        'lblLGI.Text &= "<br>You can use your <b>temporary technical quotation number to access iQuote and use the 'My Quotation' tab to log-in and get prices.</b>"
                        'System.Web.UI.ScriptManager.RegisterClientScri(Page, GetType(Page), "SetWorningImageGAL", "SetWorningImage()", True)
                        imgGalNote.ImageUrl = "../media/Icons/warningblue.png"
                        imgGalNote.Width = "80"
                        imgGalNote.Height = "72"
                        pnlDife.Visible = False
                    End If


                    'lblT1.CssClass = "FontFamily FontSizeRoboto25"
                    '    lblT1.Text = "Please Note"
                    '    lblLGI.Text = "We are currently unable to show prices."
                    '    lblLGI.Text &= "<br>In the meantime, you can view your temporary technical offer."
                    '    lblLGI.Text &= "<br>You can use your <b>temporary technical quotation number to access iQuote and use the 'My Quotation' tab to log-in and get prices.</b>"
                    '    'System.Web.UI.ScriptManager.RegisterClientScri(Page, GetType(Page), "SetWorningImageGAL", "SetWorningImage()", True)
                    '    imgGalNote.ImageUrl = "../media/Icons/warningblue.png"
                    '    imgGalNote.Width = "80"
                    imgGalNote.Height = "80"


                End If
            End If
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub
    Private Sub Set_ControlsStyle(LastParameter As Boolean)
        Try

            If _Quotation_Status = clsQuatation.e_QuotationStatus.Exist_QutOpenedFromQuotationList Or _Quotation_Status = clsQuatation.e_QuotationStatus.Exist_Qut_OpenedFromParameters Then ' clsQuatation.e_QuotationStatus.EXIST_QUOTATION_MODIF Or _QuotationStatus = clsQuatation.e_QuotationStatus.EXIST_QUOTATION_CONFOG Then
                dgvParamList.Columns.Item(GridParamList.SelectParameterIcon).Visible = False

            End If

            lblValidation.Text = ""
            If LastParameter = False Then
                pnlRulles.CssClass = "pnlRullescss"
                pnlSetParam.CssClass = "DisplayBlock"
                pnlNote.CssClass = "DisplayBlock"
                divmyDivRight.Visible = True
                DivFinalQuoteFormVisibilty(False, False, LastParameter)

            Else
                pnlRulles.CssClass = "DisplayNone"
                pnlSetParam.CssClass = "DisplayNone"
                pnlNote.CssClass = "DisplayNone"
                divmyDivRight.Visible = False
                lblValidation.Text = GetErrors()
                DivPriceAskLabelVisibility()

                Dim sIt As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber, False)
                If sIt <> "" AndAlso IsNumeric(sIt) Then
                    DivFinalQuoteFormVisibilty(True, False, True)
                Else
                    DivFinalQuoteFormVisibilty(False, True, True)
                End If
            End If

        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub
    Protected Function GetErrors() As String

        Try

            Dim Errors As String = ""
            Dim isValidTest As Boolean = False
            Validate("myValidationGroup")
            isValidTest = IsValid

            If Not isValidTest Then

                For Each ctrl As BaseValidator In Me.Validators

                    Errors += ctrl.ErrorMessage & vbLf

                Next
            End If

            Return Errors.Trim()
        Catch ex As Exception
            Error ""
        End Try

    End Function


    Private Sub Fill_gvParamsList(d As DataView)
        Try


            dgvParamList.DataSource = d
            dgvParamList.DataBind()
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

        End Try
    End Sub




    Private Function Get_ParamsGridVal(NewStart As Boolean, ByRef success As Boolean) As DataTable

        Try
            success = True
            Dim dtP As New DataTable
            Dim dtParamList As DataTable = Nothing
            Dim sitemNumber As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber).ToString
            Dim sLang As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.language).ToString
            Dim svers As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.vers).ToString

            If sitemNumber <> "" Then

                Dim dt As DataTable = Nothing
                Dim dv As DataView = SemiApp_bl.CatalogIscarData.GetItemParametersMobileISO(sitemNumber, svers, sLang, NewStart)
                dt = dv.ToTable
                Dim dt1 As DataTable = dv.Table

                If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Temp_ClearAllForModification, False) <> "TRUE" Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamsFromCatalog, dt)
                End If

                Dim r_A As DataRow = dtP.NewRow
                dtP.Rows.Add(r_A)
                For Each r As DataRow In dt1.Rows
                    dtP.Columns.Add(r.Item("GIPRGP_ISO").ToString.Trim.Replace(" ", ""))
                    dtP.Rows(0).Item(dtP.Columns.Count - 1) = r.Item("Val").ToString.Trim
                Next
            End If
            Return dtP

        Catch ex As Exception
            success = False
            'GeneralException.WriteEventErrors(ex.Message, GeneralException.e_LogTitle.PARAMETERS.ToString)
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, "<b>Error Geting Items from Catalog!</b><br>Please start a new quotation.")

        End Try

    End Function

    Private Function GetCurrentValFromCatalog(rVal As DataRow) As String
        Try
            Dim dt As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtModParamsFinalSelected, "")
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then

                For Each er As DataRow In dt.Rows
                    If er.Item("GPNUM_ISO").ToString.Trim = rVal.Item("GPNUM_ISO").ToString.Trim Then
                        If er.Item("Measure").ToString.Trim <> "" Then
                            Return er.Item("Measure").ToString.Trim
                        End If
                        Exit For
                    End If
                Next
            End If
            Return rVal.Item("val").ToString
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            Return rVal.Item("val").ToString

        End Try
    End Function
    Private Function GetCurrentValFromParams(sLabel As String) As String
        Try
            Dim ssLabel As String = ""
            If sLabel.Contains("(") Then
                sLabel = sLabel.Substring(0, InStr(sLabel, "(") - 1).ToString.Trim
            End If
            Dim dt As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtModParamsFinalSelected, "")
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                For Each er As DataRow In dt.Rows
                    If er.Item("Label").ToString.Trim = sLabel.ToString.Trim Or er.Item("Label").ToString.Trim = sLabel Then
                        If er.Item("Measure").ToString.Trim <> "" Then
                            Return er.Item("Measure").ToString.Trim
                        End If
                        Exit For
                    End If
                Next
            End If
            Return ""
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            Return ""

        End Try
    End Function


    Private Sub RuleSelectValue(RunAllParams As Boolean, ByVal RowIndex As Integer, Optional ByVal DefaultMeasure As String = "", Optional ByVal DefaultOrder As String = "")

        Try

            Dim Measure As String
            Dim Order As String = ""
            Dim PictSelect As String = ""
            Dim PictHelp As String = ""
            Dim StringValue As String = ""

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagParametersChanged, True)
            Dim sC As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagModificationCatalogDeferent, False)
            If sC = "LASTPARAMDID" Then
                Try
                    If _dtParamListModification.Rows.Count > CurrentParameterIndex - 1 Then
                        If IsNumeric(DefaultMeasure) AndAlso IsNumeric(_dtParamListModification.Rows(CurrentParameterIndex - 1).Item("Measure")) Then
                            If CDbl(_dtParamListModification.Rows(CurrentParameterIndex - 1).Item("Measure")) <> CDbl(DefaultMeasure) Then
                                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagModificationCatalogDeferent, "TRUE")
                            End If
                        Else
                            If _dtParamListModification.Rows(CurrentParameterIndex - 1).Item("Measure").ToString <> DefaultMeasure.ToString Then
                                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagModificationCatalogDeferent, "TRUE")
                            End If
                        End If
                    End If
                Catch ex As Exception
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagModificationCatalogDeferent, "TRUE")
                End Try

            End If

            '-----CHECK FOR CHANGE REFERENCE WHEN PARAM CHANGE - DTM 192483------------
            Dim sCHD As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagModificationCatalogDeferent, False)
            Dim sCFinish As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagModificationRecursiveFinish, False)

            If sCHD.ToUpper = "TRUE" AndAlso sCFinish.ToUpper <> "TRUE" Then
                Try
                    If _dtParamListModification.Rows.Count > CurrentParameterIndex - 1 Then
                        If IsNumeric(DefaultMeasure) AndAlso IsNumeric(_dtParamListModification.Rows(CurrentParameterIndex - 1).Item("Measure")) Then
                            If CDbl(_dtParamListModification.Rows(CurrentParameterIndex - 1).Item("Measure")) <> CDbl(DefaultMeasure) Then
                                If GetParamCodeToCheckReference(_dtParamListModification.Rows(CurrentParameterIndex - 1).Item("costname").ToString) = True Then
                                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagModificationRecursiveFinish, "True")
                                    'CurrentParameterIndex = 1
                                    ChangeReferenceInDtParams()
                                End If
                            End If
                        Else
                            If _dtParamListModification.Rows(CurrentParameterIndex - 1).Item("Measure").ToString <> DefaultMeasure.ToString Then
                                If GetParamCodeToCheckReference(_dtParamListModification.Rows(CurrentParameterIndex - 1).Item("costname").ToString) = True Then
                                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagModificationRecursiveFinish, "True")
                                    ChangeReferenceInDtParams()
                                End If
                            End If
                        End If
                    End If
                Catch ex As Exception
                    'If _dtParamListModification.Rows(CurrentParameterIndex - 1).Item("label").ToString.ToLower.Contains("head diameter") Then
                    '    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagModificationRecursiveFinish, "True")
                    'End If
                End Try

            End If
            '-----------------

            Dim sStart As String = clsQuatation.ACTIVE_OpenType.ToString 'returnCurentOpenType() 'SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)

            If isEditMode Then
                If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then

                    For i As Integer = _dtParamListConfiguration.Rows.Count - 1 To CurrentParameterIndex - 1 Step -1
                        _dtParamListConfiguration.Rows(i).Delete()
                    Next
                    _dtParamListConfiguration.AcceptChanges()            'pay attention!
                    isEditMode = False
                    btnCancel.Visible = False
                ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then

                    For i As Integer = _dtParamListModification.Rows.Count - 1 To CurrentParameterIndex - 1 Step -1
                        _dtParamListModification.Rows(i).Delete()
                    Next
                    _dtParamListModification.AcceptChanges()            'pay attention!
                    isEditMode = False
                    btnCancel.Visible = False

                End If
            End If

            If dgvRulles.Rows.Count > 0 Then
                StringValue = RemoveQuetes(dgvRulles.Rows(RowIndex).Cells(GridValuesIndex.StringValue).Text.ToString)
            End If

            If Server.HtmlDecode(DefaultMeasure).ToString.Trim <> "" Then
                Measure = Server.HtmlDecode(DefaultMeasure)
            Else
                Measure = Server.HtmlDecode(dgvRulles.Rows(RowIndex).Cells(GridValuesIndex.HeightLimitH).Text)
            End If

            If Server.HtmlDecode(DefaultOrder).ToString.Trim <> "" Then
                Order = DefaultOrder
                PictSelect = dgvRulles.Rows(RowIndex).Cells(GridValuesIndex.PictSelect).Text.Trim
                PictHelp = dgvRulles.Rows(RowIndex).Cells(GridValuesIndex.PictHelp).Text.Trim
            Else
                If dgvRulles.Rows.Count > 0 Then
                    Order = dgvRulles.Rows(RowIndex).Cells(GridValuesIndex.Order).Text
                    PictSelect = dgvRulles.Rows(RowIndex).Cells(GridValuesIndex.PictSelect).Text.Trim
                    PictHelp = dgvRulles.Rows(RowIndex).Cells(GridValuesIndex.PictHelp).Text.Trim
                End If
            End If

            'txtValue.Text = Measure
            Set_Txt_Slider_Value(Measure, "")

            Dim sMeasure As String = RemoveQuetes(Measure)


            Dim dtParameters As DataTable = Nothing
            Dim dr As DataRow = Nothing

            Dim formatf As String = ""
            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                dtParameters = clsModel.GetModelParameters(_ModelNumConfiguration, True)
                dr = _dtParamListConfiguration.NewRow
                For i As Integer = 0 To dtParameters.Columns.Count - 1
                    dr(dtParameters.Columns(i).ColumnName) = dtParameters.Rows(CurrentParameterIndex - 1)(i)
                Next
                formatf = dr("formatformula").ToString
                Dim oFormulaFormat As New FormulaResult(formatf, _dtParamListConfiguration, 0, Nothing)
                formatf = oFormulaFormat.ParseAndCalculate()
                'dr("Label") = SET_PARAMETER_LabelName(dr("GPNUM_ISO").ToString.Trim, _dtParamListConfiguration, dr)
                oFormulaFormat = Nothing
            ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                dtParameters = clsModel.GetModelParameters(_ModelNumModification, True)
                dr = _dtParamListModification.NewRow
                For i As Integer = 0 To dtParameters.Columns.Count - 1
                    dr(dtParameters.Columns(i).ColumnName) = dtParameters.Rows(CurrentParameterIndex - 1)(i)
                Next
                formatf = dr("formatformula").ToString
                Dim oFormulaFormat As New FormulaResult(formatf, _dtParamListModification, 0, Nothing)
                formatf = oFormulaFormat.ParseAndCalculate()
                'dr("Label") = SET_PARAMETER_LabelName(dr("GPNUM_ISO").ToString.Trim, _dtParamListModification, dr)
                oFormulaFormat = Nothing
            End If

            dr("Measure") = FormatFormula(sMeasure, formatf)

            dr("Order") = Order
            dr("PictSelect") = PictSelect
            dr("PictHelp") = PictHelp
            dr("StringValue") = StringValue
            'dr("DescValue") = DescValue

            Dim sEnableParam As String = dr("MainParameters")
            If sEnableParam = "1" Or (sStart <> "" AndAlso sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR) Then
                Dim PrevParam As Integer = 0
                Try
                    PrevParam = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.PrevParameterSelected)
                Catch ex As Exception

                End Try
                dr("PrevParam") = PrevParam - 1
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PrevParameterSelected, CInt(CurrentParameterIndex))
            End If
            dr("Label") = dr("Label").ToString.Replace("(" & dr("CostName") & ")", "").Trim()



            '-----SET PARAMETER LABEL (NAME)------
            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                If Not _dtParamListConfiguration Is Nothing Then
                    If Not IsDuplicate(_dtParamListConfiguration, dr) Then
                        _dtParamListConfiguration.Rows.Add(dr) ' same like => check if rows.count<=CInt(lblCurrentParameterIndex.Text)
                    End If

                    Dim dv_Params As DataView = _dtParamListConfiguration.Copy.DefaultView
                    dv_Params.RowFilter = "visiblegrid= True And Measure <>'' AND Measure<>'UNVISIBLE'"
                    Fill_gvParamsList(dv_Params)
                End If

            Else
                If Not _dtParamListModification Is Nothing Then
                    If Not IsDuplicate(_dtParamListModification, dr) Then
                        _dtParamListModification.Rows.Add(dr) ' same like => check if rows.count<=CInt(lblCurrentParameterIndex.Text)
                    End If

                    Dim dv_Params As DataView = _dtParamListModification.Copy.DefaultView
                    dv_Params.RowFilter = "visiblegrid=true AND Measure<>'' AND Measure<>'UNVISIBLE'"
                    Fill_gvParamsList(dv_Params)
                End If
            End If


            SetCurrentParamIndex("_", 1)
            SetParameter(RunAllParams)
            FillImages() 'False

            If False Then

                Dim dv As DataView = CType(dgvRulles.DataSource, DataView)
                Dim vDummy As DataTable = Nothing
                For i As Integer = 0 To dv.Count - 1
                    Select Case dv(i)("Operation").ToString
                        Case "<>"
                            Dim lowVal As String = dv(i)("LowLimitH").ToString
                            Dim hiVal As String = dv(i)("HeightLimitH").ToString

                            If lowVal = "" Then
                                lowVal = lowVal
                            Else
                                If IsNumeric(lowVal) Then
                                    lowVal = lowVal
                                Else
                                    If Left(lowVal, 1) = "{" AndAlso Right(lowVal, 1) = "}" AndAlso IsNumeric(Mid(lowVal, 2, Len(lowVal) - 2)) Then
                                        If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                            lowVal = _dtParamListConfiguration.Rows(CInt(Mid(lowVal, 2, Len(lowVal) - 2)) - 1)("Measure").ToString()
                                        Else
                                            lowVal = _dtParamListModification.Rows(CInt(Mid(lowVal, 2, Len(lowVal) - 2)) - 1)("Measure").ToString()
                                        End If
                                    Else
                                        Dim v As DataTable = Nothing
                                        Dim dummyResOrder As String = ""
                                        If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                            lowVal = ComputeFormula(lowVal, _dtParamListConfiguration, CurrentParameterIndex, v, dummyResOrder)
                                        Else
                                            lowVal = ComputeFormula(lowVal, _dtParamListModification, CurrentParameterIndex, v, dummyResOrder)
                                        End If

                                    End If
                                End If
                            End If

                            If hiVal = "" Then
                                hiVal = hiVal
                            Else
                                If IsNumeric(hiVal) Then
                                    hiVal = hiVal
                                Else
                                    If Left(hiVal, 1) = "{" AndAlso Right(hiVal, 1) = "}" AndAlso IsNumeric(Mid(hiVal, 2, Len(hiVal) - 2)) Then
                                        If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                            hiVal = _dtParamListConfiguration.Rows(CInt(Mid(hiVal, 2, Len(hiVal) - 2)) - 1)("Measure").ToString()
                                        Else
                                            hiVal = _dtParamListModification.Rows(CInt(Mid(hiVal, 2, Len(hiVal) - 2)) - 1)("Measure").ToString()
                                        End If

                                    Else
                                        Dim dummyResOrder As String = ""
                                        If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                            hiVal = ComputeFormula(hiVal, _dtParamListConfiguration, CurrentParameterIndex, vDummy, dummyResOrder)
                                        Else
                                            hiVal = ComputeFormula(hiVal, _dtParamListModification, CurrentParameterIndex, vDummy, dummyResOrder)
                                        End If

                                    End If
                                End If
                            End If

                            If dv(i)("LowLimitH").ToString = dv(i)("LowLimit").ToString Then
                                dv(i)("LowLimitH") = lowVal
                                dv(i)("LowLimit") = lowVal
                            Else
                                dv(i)("LowLimitH") = lowVal
                            End If

                            If dv(i)("HeightLimitH").ToString = dv(i)("HeightLimit").ToString Then
                                dv(i)("HeightLimitH") = hiVal
                                dv(i)("HeightLimit") = hiVal
                            Else
                                dv(i)("HeightLimitH") = hiVal
                            End If
                    End Select
                Next

                'txtValue.Text = ""
                Set_Txt_Slider_Value("", "")
            End If
            'CType(Master.FindControl("SM_Mastr"), ScriptManager).SetFocus(txtValue)
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "LostFocusTxt", "LostFocusTxtVal()", True)
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, False)

            If ex.Message.Contains("There is no row at position") Then
                Response.Redirect("~/Default.aspx", False)
            End If
            GeneralException.BuildError(Page, ex.Message)
        End Try
        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-RuleSelectValue-" & i_counter)

    End Sub
    Private Sub ChangeReferenceInDtParams()
        Try
            If Not _dtParamListModification Is Nothing AndAlso _dtParamListModification.Rows.Count > 0 Then
                Dim i As Integer = CurrentParameterIndex
                Dim dv As DataView = _dtParamListModification.DefaultView
                dv.RowFilter = "Label='Reference'"
                If Not dv Is Nothing AndAlso dv.Count > 0 AndAlso CInt(dv(0).Item("Tabindex")) < CurrentParameterIndex Then

                    Dim sR As String = GetNewRefernceDependsOnParam(clsQuatation.ACTIVE_ModelNumber, "{DC}", _DC)
                    If sR.ToString <> "" Then
                        dv.RowFilter = "CostName='DC'"
                        If Not dv Is Nothing AndAlso dv.Count > 0 Then
                            _dtParamListModification.Rows(dv(0).Item("TabIndex")).Item("Measure") = sR
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.itemNumber, sR)
                        End If
                    End If
                End If
                'For Each r As DataRow In _dtParamListModification.Rows
                '    If r.Item("") Then
                'Next
            End If
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

        End Try

    End Sub
    Private Function GetParamCodeToCheckReference(CurentParamLabel As String) As Boolean

        Return True

        'Try
        '    Dim dt As DataTable = clsReference.FindStandardItemParamCodeToCheck(clsQuatation.ACTIVE_ModelNumber)
        '    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
        '        If dt.Rows(0).Item("ParamCode").ToString.ToLower <> "" Then
        '            'sStr += dt.Rows(0).Item("ParamCoode").ToString.ToLower & ";"
        '            If CurentParamLabel.ToString.Trim.ToLower.Contains(dt.Rows(0).Item("ParamCode").ToString.Trim.ToLower.Replace("{", "").Replace("}", "")) Then
        '                Return True
        '            End If
        '        End If
        '    End If
        '    Return False
        'Catch ex As Exception
        '    Return False
        'End Try
    End Function
    Private Function ReaplaceHtmCharsWithShowChars(V As String) As String
        Try
            If V.Contains("°") Then
                V = Replace(V, "°", "&#176;")
            End If
            Return V
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Function

    Private Function ReplaceHTMLtags(sHtmlStr As String) As String
        Try

            sHtmlStr = sHtmlStr.Replace("&LT;", "<").Replace("&GT;", ">")
            Return sHtmlStr

        Catch ex As Exception

        End Try
    End Function

    Private Sub CheckIllegalParameterValue(ByRef Value As String, ByRef Res As String, ByRef ResOrder As String, ByRef iRuleIndex As Integer, CheckIllegalFormat As Boolean)
        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-CheckIllegalParameterValue-" & i_counter)

        Try
            Value = ReaplaceHtmCharsWithShowChars(Value)

            'Dim Res As String = ""
            'Dim ResOrder As String = ""
            'Dim Value As String = txtValue.Text
            'Dim iRuleIndex As Integer = 0

            If Server.HtmlDecode(Value).ToString.Trim <> "" Then
                If dgvRulles.Rows.Count = 0 Then
                    If Not IsNumeric(Value) Then
                        'GeneralException.BuildError(Page, "Value must be numeric!")
                        Exit Sub
                    Else
                        Res = Value
                        ResOrder = "" 'gr.Cells(GridValuesIndex.Order).Text.ToUpper
                        Exit Sub
                    End If
                Else
                    Dim sMsg As String
                    sMsg = "The value you requested is out of the iQuote range."
                    sMsg &= "<BR>"
                    If InStr(1, StateManager.GetValue(StateManager.Keys.s_CompanyName), "TAEGUTEC") > 0 Then
                        sMsg &= "Please refer to TaeguTec LTD. to receive a quotation for a special tool."
                    Else
                        sMsg &= "Please refer to ISCAR LTD. to receive a quotation for a special tool."
                    End If
                    For iRuleIndex = 0 To dgvRulles.Rows.Count - 1
                        Dim gr As GridViewRow = dgvRulles.Rows(iRuleIndex)

                        Dim sHeightLimitH As String = gr.Cells(GridValuesIndex.HeightLimitH).Text.ToUpper
                        Dim sHeightLimit As String = gr.Cells(GridValuesIndex.HeightLimit).Text.ToUpper
                        Dim sHeightLimitHReplace As String = ReplaceHTMLtags(gr.Cells(GridValuesIndex.HeightLimitH).Text.ToUpper)
                        Dim sHeightLimitReplace As String = ReplaceHTMLtags(gr.Cells(GridValuesIndex.HeightLimit).Text.ToUpper)

                        Select Case Server.HtmlDecode(gr.Cells(GridValuesIndex.Operation).Text)
                            Case "="
                                If Value.ToUpper = sHeightLimitH Or Value.ToUpper = sHeightLimit Or Value.ToUpper = sHeightLimitHReplace Or Value.ToUpper = sHeightLimitReplace Then
                                    Res = Value
                                    ResOrder = gr.Cells(GridValuesIndex.Order).Text.ToUpper
                                    Exit For
                                End If
                            Case "<>"
                                If Not IsNumeric(Value) Then
                                    'GeneralException.BuildError(Page, sMsg)
                                    Exit Sub
                                End If
                                For Each oOperator As String In FormulaResult.Operators
                                    If oOperator <> "-" AndAlso InStr(Value, oOperator) > 0 Then
                                        'GeneralException.BuildError(Page, sMsg)
                                        Exit Sub
                                    End If
                                Next

                                Dim dicimalOk As Boolean = True
                                If CheckIllegalFormat Then
                                    Try
                                        If CDbl(Value) <> CDbl(CDec(gr.Cells(GridValuesIndex.LowLimit).Text)) Then
                                            Dim sCount_v As String = dicimalsCount(Value)
                                            Dim sCount_D As String = dicimalsCount(CDec(gr.Cells(GridValuesIndex.LowLimit).Text))
                                            If sCount_v > sCount_D Then
                                                dicimalOk = False
                                                Res = "ERRORMSG"
                                                For tt As Int16 = 0 To sCount_D - 1
                                                    Res &= "#"
                                                Next
                                                Exit For
                                            End If
                                        End If
                                    Catch ex As Exception

                                    End Try
                                End If








                                If dicimalOk = True AndAlso (CDec(Value) >= CDec(gr.Cells(GridValuesIndex.LowLimit).Text) And CDec(Value) <= CDec(gr.Cells(GridValuesIndex.HeightLimit).Text)) Then
                                    Res = Value
                                    ResOrder = gr.Cells(GridValuesIndex.Order).Text.ToUpper
                                    Exit For
                                End If
                            Case ">"
                                'If Not IsNumeric(Value) And Value.ToString <> gr.Cells(GridValuesIndex.HeightLimitH).Text Then
                                '    GeneralException.BuildError(Page, sMsg)
                                '    Exit Sub
                                'ElseIf Not IsNumeric(Value) And Value.ToString = gr.Cells(GridValuesIndex.HeightLimitH).Text Then
                                '    Res = Value
                                '    ResOrder = gr.Cells(GridValuesIndex.Order).Text.ToUpper
                                '    Exit For
                                'ElseIf IsNumeric(Value) Then
                                '    If CDec(Value) > CDec(gr.Cells(GridValuesIndex.LowLimit).Text) Then
                                '        Res = Value
                                '        ResOrder = gr.Cells(GridValuesIndex.Order).Text.ToUpper
                                '        Exit For
                                '    End If
                                'Else
                                '    GeneralException.BuildError(Page, sMsg)
                                '    Exit Sub
                                'End If

                                If IsNumeric(Value) Then
                                    If CDec(Value) > CDec(gr.Cells(GridValuesIndex.LowLimit).Text) Then
                                        Res = Value
                                        ResOrder = gr.Cells(GridValuesIndex.Order).Text.ToUpper
                                        Exit For
                                    End If
                                Else
                                    'GeneralException.BuildError(Page, sMsg)
                                    Exit Sub
                                End If
                            Case "<"
                                If Not IsNumeric(Value) Then
                                    'GeneralException.BuildError(Page, sMsg)
                                    Exit Sub
                                End If
                                If CDec(Value) < CDec(gr.Cells(GridValuesIndex.HeightLimit).Text) Then
                                    Res = Value
                                    ResOrder = gr.Cells(GridValuesIndex.Order).Text.ToUpper
                                    Exit For
                                End If
                            Case "<="
                                If Not IsNumeric(Value) Then
                                    'GeneralException.BuildError(Page, sMsg)
                                    Exit Sub
                                End If
                                If CDec(Value) <= CDec(gr.Cells(GridValuesIndex.HeightLimit).Text) Then
                                    Res = Value
                                    ResOrder = gr.Cells(GridValuesIndex.Order).Text.ToUpper
                                    Exit For
                                End If
                            Case ">="
                                If Not IsNumeric(Value) Then
                                    'GeneralException.BuildError(Page, sMsg)
                                    Exit Sub
                                End If
                                If CDec(Value) >= CDec(gr.Cells(GridValuesIndex.LowLimit).Text) Then
                                    Res = Value
                                    ResOrder = gr.Cells(GridValuesIndex.Order).Text.ToUpper
                                    Exit For
                                End If
                            Case "All"
                                Res = Value
                                ResOrder = gr.Cells(GridValuesIndex.Order).Text.ToUpper
                                Exit For
                            Case Else
                                'TODO: !InputLimit=Measure
                        End Select
                    Next

                End If

                'If Res = "" Then
                '    GeneralException.BuildError(Page, "Illegal parameter value")
                'Else
                '    RuleSelectValue(iRuleIndex, Res, ResOrder)
                'End If
            End If
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try

        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-CheckIllegalParameterValue-" & i_counter)

    End Sub

    Private Function dicimalsCount(sVal As String) As Integer
        Try
            If sVal.Contains(".") Then
                Dim ssV() As String = sVal.Split(".")
                If ssV.Count = 2 Then
                    Return ssV(1).Count
                End If
            End If
            If sVal.Contains(",") Then
                Dim ssV() As String = sVal.Split(",")
                If ssV.Count = 2 Then
                    Return ssV(1).Count
                End If
            End If
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            Return sVal
        End Try

    End Function
    Protected Sub btnSelectparam_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectParam.Click
        Try
            'Set_Param(False)
            If RunSetParam(False, True, True) = True Then
                'txtValue0.Text = txtValue.Text
                CONTINUE_CONFIGURATOR()
            End If

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "DoHumborgerAA", "DoHumborger();", True)
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Function RunSetParam(RunAllParams As Boolean, ShowErrorMessage As Boolean, CheckIllegalFormat As Boolean) As Boolean
        Try
            Dim s As String = ""
            Dim b As Boolean = Set_Param(RunAllParams, s, ShowErrorMessage, CheckIllegalFormat)
            If b = True Then
                Return True
            Else
                'GeneralException.BuildError(Page, s)
                Return False
            End If
        Catch ex As Exception
            'GeneralException.BuildError(Page, ex.Message)
        End Try
    End Function


    Private Sub Set_Txt_Slider_Value(txtVal As String, txtslider As String)
        Try

            Dim s As String = ""
            s = RemoveApostrophes(txtVal)

            txtValue.Text = LocalizationManager.CulturingNumber(s, False)
            txtValue0.Text = LocalizationManager.UnCulturingNumber(s)
            txtslider = LocalizationManager.UnCulturingNumber(txtslider)
            If IsNumeric(txtslider) Then
                'Slider1.Text = txtslider
                If txtslider <> txtSliderMinVal.Text Then
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertNN", "sliders(" & txtslider & "," & txtSliderMinVal.Text & "," & txtSliderMaxVal.Text & "," & txtSliderStepVal.Text & ");", True)
                End If
            End If

        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub

    Public Function RemoveApostrophes(mainFormula As String) As String
        Try
            If mainFormula.Contains("""") Then
                mainFormula = mainFormula.Replace("""", "")
                Return mainFormula
            Else
                Return mainFormula
            End If

        Catch ex As Exception
            Throw
        End Try
        Return ""
    End Function

    Private Function Set_Param(RunAllParams As Boolean, ByRef sM As String, ShowErrorMessage As Boolean, CheckIllegalFormat As Boolean) As Boolean
        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-Set_Param-" & i_counter)

        Try
            Dim Res As String = ""
            Dim ResOrder As String = ""
            Dim iRuleIndex As Integer = 0

            If txtValue0.Text = "" Then
                txtValue0.Text = LocalizationManager.UnCulturingNumber(txtValue.Text)
            Else
                If IsNumeric(txtValue0.Text) AndAlso txtValue0.Text.ToString.Contains(",") Then
                    txtValue0.Text = LocalizationManager.UnCulturingNumber(txtValue.Text)

                End If
            End If

            Dim Value As String = txtValue0.Text

            CheckIllegalParameterValue(Value, Res, ResOrder, iRuleIndex, CheckIllegalFormat)



            Dim sMsg As String = ""
            If ShowErrorMessage = True Then
                If Res = "" Then
                    sMsg = "&nbsp;&nbsp;<b><font color=red>Out of range!</font><font color=black>&nbsp;Can't customize your tool?</font></b>&nbsp;<font color=black>Contact </font><input type=button id='btnC' onclick='GetSupport()' style='cursor:pointer;' class='FontFamilyRoboto FontSizeRoboto18 BorderNone ButtonBackGroundTransperant ButtonUnderline' value='iQuote team support' />"
                ElseIf Res.Contains("ERRORMSG") Then
                    Dim dSol As String = Res.Replace("ERRORMSG", "")
                    Res = ""
                    sMsg = "&nbsp;&nbsp;<b><font color=red>Kindly Note!</font><font color=black>&nbsp;The value entered is not according to iQuote parameters format (#." & dSol & ")</font></b>"
                End If
                'sMsg = "&nbsp;&nbsp;* Out of range! Can't customize your tool?&nbsp;&nbsp;please contact: <input type=button id='btnC' onclick='GetSupport()' style='cursor:pointer;' class='FontFamily validSumcss FontFamilyRoboto FontSizeRoboto18 BorderNone ButtonBackGroundTransperant ButtonUnderline' value='iquote@iscar.com' />"

            End If
            If Res.Contains("ERRORMSG") Then
                Res = ""
            End If
            'If RunAllParams Then
            '    sMsg = ""
            'End If

            '<BR>Please refer to ISCAR LTD. to receive a quotation for a special tool.
            If Res = "" Then
                'Dim TempdtParamsChangable As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._TempdtParamsChangable, "")
                'If TempdtParamsChangable Is Nothing Then 'AndAlso TempdtParamsChangable.Rows.Count > 0 AndAlso CurrentParameterIndexEND = False Then
                '    sM = sMsg
                '    GeneralException.BuildError(Page, sMsg)
                'ElseIf TempdtParamsChangable.Rows.Count <= CurrentParameterIndex Then
                '    sM = sMsg
                '    GeneralException.BuildError(Page, sMsg)
                'End If

                sM = sMsg
                If _ShowMsg = True Then
                    'Set_Txt_Slider_Value(LocalizationManager.UnCulturingNumber(txtValue.Text), -100)
                    Set_Txt_Slider_Value(txtValue.Text, -100)
                    GeneralException.BuildError(Page, sMsg)
                End If

                Return False
            Else
                RuleSelectValue(RunAllParams, iRuleIndex, Res, ResOrder)
                Return True
            End If

            'Dim sMsg As String
            'sMsg = "The tool you requested is out of the iQuote range."
            'sMsg &= "<BR>"
            'If InStr(1, StateManager.GetValue(StateManager.Keys.BranchName), "TAEGUTEC") > 0 Then
            '    sMsg &= "Please refer to TaeguTec LTD. to receive a quotation for a special tool."
            'Else
            '    sMsg &= "Please refer to ISCAR LTD. to receive a quotation for a special tool."
            'End If





        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            Return False
            'GeneralException.BuildError(Page, ex.Message)
        Finally
            'If txtValue0.Text = "" Then
            'txtValue0.Text = txtValue.Text
            'End If
        End Try
        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-Set_Param-" & i_counter)

    End Function


    Private Sub Set_TempdtParamsChangableParamsConfig(sEnd As Boolean)
        Try

            Dim d As New DataTable
            d = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListConfiguration, "").Copy
            Dim dTemp As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._TempdtParamsChangable, "")
            If dTemp Is Nothing Or sEnd = True Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempdtParamsChangable, d)
            Else
                If dTemp.Rows.Count < d.Rows.Count Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempdtParamsChangable, d)
                End If
            End If
            d = Nothing
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub Set_TempdtParamsChangableParamsModif(sEnd As Boolean)
        Try
            If sEnd = False Then

                If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._TempdtParamsChangable, "") Is Nothing Then
                    Dim d As New DataTable
                    d = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListModification, "").Copy
                    Dim dTemp As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._TempdtParamsChangable, "")
                    If dTemp Is Nothing Or sEnd = True Then
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempdtParamsChangable, d)
                    Else
                        If dTemp.Rows.Count < d.Rows.Count Then
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempdtParamsChangable, d)
                        End If
                    End If

                    d = Nothing
                End If


            End If
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub


    Private Sub SetParameter(RunAllParams As Boolean)

        Try
            Dim sMOd As String = "0"

            sMOd = clsQuatation.ACTIVE_ModelNumber 'returnCurentModelNumber()

            Dim sStart As String = clsQuatation.ACTIVE_OpenType.ToString 'returnCurentOpenType() ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)

            Dim dtParameters As DataTable = clsModel.GetModelParameters(CInt(sMOd), True)   'All Model parameters
            Dim dtRules As DataTable
            Dim dtRulesFilter As String
            Dim drRules() As DataRow
            Dim IsFirstItteration As Boolean = True                                 ' If is first loop of while

            While True
                Trace.Write("Parameter Start: " & CurrentParameterIndex)
                If dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString = "END" Then

                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtModParamsFinalSelected, _dtParamListModification)

                    'UnvisibleLVItems = True
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Temp_ClearAllForModification, "FALSE")
                    RullsCount.Text = "END"
                    divRulls.Style.Add("display", "none")
                    'pnlContainer.Height = "660"
                    Get_DC()
                    GetItemCat()

                    CurrentParameterIndexEND = True

                    'end of parameters list
                    Dim QutNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber).ToString
                    'Dim CurrStatus As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationStatus)

                    dtRulesFilter = "LabelNum=" & CurrentParameterIndex
                    'dtRules = clsModel.GetModelRules(ModelNum)
                    dtRules = clsModel.GetModelRules(sMOd)
                    drRules = dtRules.Select(dtRulesFilter)

                    If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                        Set_TempdtParamsChangableParamsConfig(True)
                        Dim dtT As DataTable = _dtParamListConfiguration.Copy
                        ShowRules(CurrentParameterIndex, drRules, dtParameters, dtT)
                    Else
                        Set_TempdtParamsChangableParamsModif(True)
                        ShowRules(CurrentParameterIndex, drRules, dtParameters, _dtParamListModification)
                    End If

                    Set_ControlsStyle(True)

                    Dim _dt As DataTable = _dtParamListConfiguration.Copy
                    If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempdtParamsSelected, _dt)
                    Else
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempdtParamsSelected, _dt)

                    End If

                    Exit While
                Else
                    RullsCount.Text = ""
                    If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                        Set_TempdtParamsChangableParamsConfig(False)
                    Else
                        Set_TempdtParamsChangableParamsModif(False)
                    End If

                    divRulls.Style.Add("display", "solid")
                    Set_ControlsStyle(False)

                End If

                If Not CBool(dtParameters.Rows(CurrentParameterIndex - 1)("Active")) Then

                    If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                        SetResult(0, "", CurrentParameterIndex, dtParameters, _dtParamListConfiguration)
                    Else
                        SetResult(0, "", CurrentParameterIndex, dtParameters, _dtParamListModification)
                    End If

                    SetCurrentParamIndex("_", 1)

                    Continue While
                End If


                If Mid(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString.ToUpper, 1, _SPECIAL_FUNCTION_QB.Length) = _SPECIAL_FUNCTION_QB Then

                    Dim res As String
                    Dim dvDummy As DataTable = Nothing
                    Dim resOrder As String = ""

                    'HL_GETSCALE:MILLIMETER;HL_GETWORKPIECEMATERIAL:STEEL:HL_GETMATERIAL;Steel dfsdfsdfds:HL_CATEGORY:Category

                    dtRules = clsModel.GetModelRules(sMOd)

                    dtRulesFilter = "LabelNum=" & CurrentParameterIndex
                    drRules = dtRules.Select(dtRulesFilter)
                    If drRules.Length > 0 Then

                        If drRules(0).Item("HeightLimitH").ToString <> "" Then
                            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                res = ComputeFormula(drRules(0).Item("HeightLimitH").ToString, _dtParamListConfiguration, CurrentParameterIndex, dvDummy, resOrder)
                                'SetResult_SpecialQB(sStrSplit(ID).Split(":")(0).ToString, sStrSplit(ID).Split(":")(1).ToString, resOrder, CurrentParameterIndex, dtParameters, _dtParamListConfiguration)
                            Else
                                res = ComputeFormula(drRules(0).Item("HeightLimitH").ToString, _dtParamListModification, CurrentParameterIndex, dvDummy, resOrder)

                            End If
                            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                SetResult(res, "R1", CurrentParameterIndex, dtParameters, _dtParamListConfiguration)
                            Else
                                SetResult(res, "R1", CurrentParameterIndex, dtParameters, _dtParamListModification)
                            End If
                        ElseIf drRules(0).Item("HeightLimit").ToString <> "" Then
                            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                res = ComputeFormula(drRules(0).Item("HeightLimit").ToString, _dtParamListConfiguration, CurrentParameterIndex, dvDummy, resOrder)
                            Else
                                res = ComputeFormula(drRules(0).Item("HeightLimit").ToString, _dtParamListModification, CurrentParameterIndex, dvDummy, resOrder)
                            End If
                            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                SetResult(res, "R1", CurrentParameterIndex, dtParameters, _dtParamListConfiguration)
                            Else
                                SetResult(res, "R1", CurrentParameterIndex, dtParameters, _dtParamListModification)
                            End If

                        End If




                        'sP = "StringValue"
                        If drRules(0).Item("StringValue").ToString <> "" Then
                            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                res = ComputeFormula(drRules(0).Item("StringValue").ToString, _dtParamListConfiguration, CurrentParameterIndex, dvDummy, resOrder)
                            Else
                                res = ComputeFormula(drRules(0).Item("StringValue").ToString, _dtParamListModification, CurrentParameterIndex, dvDummy, resOrder)
                            End If
                            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                SetResult(res, "R1", CurrentParameterIndex, dtParameters, _dtParamListConfiguration,, "STRINGVALUE")
                            Else
                                SetResult(res, "R1", CurrentParameterIndex, dtParameters, _dtParamListModification,, "STRINGVALUE")
                            End If
                        End If

                        'sP = "DescValue"
                        If drRules(0).Item("DescValue").ToString <> "" Then
                            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                res = ComputeFormula(drRules(0).Item("DescValue").ToString, _dtParamListConfiguration, CurrentParameterIndex, dvDummy, resOrder)
                            Else
                                res = ComputeFormula(drRules(0).Item("DescValue").ToString, _dtParamListModification, CurrentParameterIndex, dvDummy, resOrder)
                            End If
                            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                SetResult(res, "R1", CurrentParameterIndex, dtParameters, _dtParamListConfiguration,, "DESCVALUE")
                            Else
                                SetResult(res, "R1", CurrentParameterIndex, dtParameters, _dtParamListModification,, "DESCVALUE")
                            End If
                        End If

                    End If

                ElseIf Mid(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString.ToUpper, 1, _SPECIAL_FUNCTION_RELATION.Length) = _SPECIAL_FUNCTION_RELATION Then

                    Dim res As String = ""
                    Dim resOrder As String = ""
                    Dim dvDummy As DataTable = Nothing

                    If RunAllParams And sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                        If checkIfResunvisible(dtParameters, dvDummy, resOrder).ToString.ToUpper <> "UNVISIBLE" Then
                            If dtParameters.Rows(CurrentParameterIndex - 1)("GPNUM_ISO").ToString <> "" Then
                                res = GetParameterStaticResult(dtParameters.Rows(CurrentParameterIndex - 1)("GPNUM_ISO").ToString, dtParameters, CurrentParameterIndex, CType(Nothing, DataTable), resOrder)
                            Else
                                res = GetParameterStaticResult(dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString, dtParameters, CurrentParameterIndex, CType(Nothing, DataTable), resOrder)
                            End If
                        End If
                        res = ComputeFormula(res, clsQuatation.GetActiveQuotation_DTparams, CurrentParameterIndex, dvDummy, resOrder)
                    Else

                        res = ComputeFormula(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString, clsQuatation.GetActiveQuotation_DTparams, CurrentParameterIndex, dvDummy, resOrder) 'ReturnCurentDTparams

                    End If

                    If res.ToUpper <> "VISIBLE" Then
                        If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                            SetResult(res, resOrder, CurrentParameterIndex, dtParameters, _dtParamListConfiguration)
                        Else
                            SetResult(res, resOrder, CurrentParameterIndex, dtParameters, _dtParamListModification,,, True)
                        End If

                        SetCurrentParamIndex("_", 1)
                        Continue While
                    End If

                ElseIf Mid(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString.ToUpper, 1, _SPECIAL_FUNCTION_CROSLISTCHECK.Length) = _SPECIAL_FUNCTION_CROSLISTCHECK Then
                    Dim res As String
                    Dim resOrder As String = ""
                    Dim dvDummy As DataTable = Nothing

                    'res = ComputeFormula(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString, _dtParamList, CurrentParameterIndex, dvDummy, resOrder)
                    If RunAllParams And sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                        If checkIfResunvisible(dtParameters, dvDummy, resOrder).ToString.ToUpper <> "UNVISIBLE" Then
                            'res = GetParameterStaticResult(dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString)
                            If dtParameters.Rows(CurrentParameterIndex - 1)("GPNUM_ISO").ToString <> "" Then
                                res = GetParameterStaticResult(dtParameters.Rows(CurrentParameterIndex - 1)("GPNUM_ISO").ToString, dtParameters, CurrentParameterIndex, CType(Nothing, DataTable), resOrder)
                            Else
                                res = GetParameterStaticResult(dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString, dtParameters, CurrentParameterIndex, CType(Nothing, DataTable), resOrder)
                            End If
                        End If

                    Else
                        If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                            res = ComputeFormula(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString, _dtParamListConfiguration, CurrentParameterIndex, dvDummy, resOrder)
                        Else
                            res = ComputeFormula(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString, _dtParamListModification, CurrentParameterIndex, dvDummy, resOrder)
                        End If

                    End If

                    If res.ToUpper = "UNVISIBLE" Then

                        If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                            SetResult(res, resOrder, CurrentParameterIndex, dtParameters, _dtParamListConfiguration, "False")
                        Else
                            SetResult(res, resOrder, CurrentParameterIndex, dtParameters, _dtParamListModification, "False")
                        End If
                        SetCurrentParamIndex("_", 1)
                        Continue While
                    Else
                        dtRules = dvDummy
                        drRules = dtRules.Select()
                        If res Is Nothing Or res = "0" Then

                            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                SetResult(res, resOrder, CurrentParameterIndex, dtParameters, _dtParamListConfiguration, "False")
                            Else
                                SetResult(res, resOrder, CurrentParameterIndex, dtParameters, _dtParamListModification, "False")
                            End If

                            SetCurrentParamIndex("_", 1)
                            Continue While
                        Else

                            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                Dim dtT As DataTable = _dtParamListConfiguration.Copy
                                ShowRules(CurrentParameterIndex, drRules, dtParameters, dtT)
                            Else
                                ShowRules(CurrentParameterIndex, drRules, dtParameters, _dtParamListModification)
                            End If

                            'If Boolean.Parse(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagEndParameterArrivedFromExistQuotation)) Then
                            '    pnlSetParam.Visible = False : pnlNote.Visible = False : pnlRulles.Visible = False
                            '    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagEndParameterArrivedFromExistQuotation, False)
                            'End If

                            Exit While
                        End If

                    End If

                End If

                'dtRules = clsModel.GetModelRules(ModelNum)
                dtRules = clsModel.GetModelRules(sMOd)

                dtRulesFilter = "LabelNum=" & CurrentParameterIndex
                drRules = dtRules.Select(dtRulesFilter)
                If Mid(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString.ToUpper, 1, _SPECIAL_FUNCTION_QB.Length) <> _SPECIAL_FUNCTION_QB Then
                    If drRules.Length > 0 Then

                        Dim dtRelationsTmp As DataTable = clsModel.GetModelRelations(sMOd, CurrentParameterIndex)
                        Dim drRelationsTmp() As DataRow

                        drRelationsTmp = dtRelationsTmp.Select()

                        If drRelationsTmp.Length > 0 Then
                            'In case of more than one relation parameter, the rules filter check is done in special functions
                            If Mid(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString.ToUpper, 1, _SPECIAL_FUNCTION_CROSLIST.Length) <> _SPECIAL_FUNCTION_CROSLIST And
                            Mid(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString.ToUpper, 1, _SPECIAL_FUNCTION_CROS.Length) <> _SPECIAL_FUNCTION_CROS And
                             Mid(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString.ToUpper, 1, _SPECIAL_FUNCTION_DisplayFromList.Length) <> _SPECIAL_FUNCTION_DisplayFromList And
                            Mid(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString.ToUpper, 1, _SPECIAL_FUNCTION_CROSLISTCHECK.Length) <> _SPECIAL_FUNCTION_CROSLISTCHECK Then
                                If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                    drRelationsTmp = dtRelationsTmp.Select("RelationRuleNum=" & CurrentParameterIndex & " AND Order='" & _dtParamListConfiguration.Rows(drRelationsTmp(0)("LabelNum") - 1)("Order") & "'")
                                Else
                                    drRelationsTmp = dtRelationsTmp.Select("RelationRuleNum=" & CurrentParameterIndex & " AND Order='" & _dtParamListModification.Rows(drRelationsTmp(0)("LabelNum") - 1)("Order") & "'")
                                End If


                                If drRelationsTmp.Length > 0 Then
                                    Dim OrderFilter As String = ""
                                    Dim orString As String = ""
                                    For i As Integer = 0 To drRelationsTmp.Length - 1
                                        OrderFilter &= " " & orString & " Order='" & drRelationsTmp(i)("RelationRules").ToString & "' "
                                        orString = " OR "
                                    Next
                                    OrderFilter = " ( " & OrderFilter & " ) "
                                    dtRulesFilter &= " AND " & OrderFilter
                                    drRules = dtRules.Select(dtRulesFilter)

                                End If
                            End If
                        End If
                        '40
                        '20.01.09 bilal & micki
                        'In case of more than one relation parameter, the rules filter UNVISIBLE check is done in special functions
                        If Mid(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString.ToUpper, 1, _SPECIAL_FUNCTION_CROSLIST.Length) <> _SPECIAL_FUNCTION_CROSLIST And
                        Mid(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString.ToUpper, 1, _SPECIAL_FUNCTION_CROS.Length) <> _SPECIAL_FUNCTION_CROS And
                        Mid(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString.ToUpper, 1, _SPECIAL_FUNCTION_DisplayFromList.Length) <> _SPECIAL_FUNCTION_DisplayFromList And
                        Mid(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString.ToUpper, 1, _SPECIAL_FUNCTION_CROSLISTCHECK.Length) <> _SPECIAL_FUNCTION_CROSLISTCHECK Then

                            For Each dr As DataRow In drRules
                                If dr("Operation").ToString.ToUpper = "UNVISIBLE" Then
                                    'SetResult("UNVISIBLE", dr("Order"), CurrentParameterIndex, dtParameters, _dtParamList)


                                    If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                        SetResult("", dr("Order"), CurrentParameterIndex, dtParameters, _dtParamListConfiguration)
                                    Else
                                        SetResult("", dr("Order"), CurrentParameterIndex, dtParameters, _dtParamListModification)
                                    End If

                                    SetCurrentParamIndex("_", 1)
                                    Continue While
                                End If
                            Next
                        End If
                        If dtParameters.Rows(CurrentParameterIndex - 1)("Visible") Then
                            If Mid(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString.ToUpper, 1, _SPECIAL_FUNCTION_CROSLIST.Length) = _SPECIAL_FUNCTION_CROSLIST Or
                               Mid(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString.ToUpper, 1, _SPECIAL_FUNCTION_DisplayFromList.Length) = _SPECIAL_FUNCTION_DisplayFromList Then
                                Dim dtDummy As DataTable = Nothing
                                Dim res As String
                                Dim resOrder As String = ""
                                'res = ComputeFormula(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString, _dtParamList, CurrentParameterIndex, dtDummy, resOrder)
                                If RunAllParams And sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                                    'res = GetParameterStaticResult(dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString)
                                    If dtParameters.Rows(CurrentParameterIndex - 1)("GPNUM_ISO").ToString <> "" Then
                                        res = GetParameterStaticResult(dtParameters.Rows(CurrentParameterIndex - 1)("GPNUM_ISO").ToString, dtParameters, CurrentParameterIndex, CType(Nothing, DataTable), resOrder)
                                    Else
                                        res = GetParameterStaticResult(dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString, dtParameters, CurrentParameterIndex, CType(Nothing, DataTable), resOrder)
                                    End If
                                Else

                                    If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                        res = ComputeFormula(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString, _dtParamListConfiguration, CurrentParameterIndex, dtDummy, resOrder)
                                    Else
                                        res = ComputeFormula(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString, _dtParamListModification, CurrentParameterIndex, dtDummy, resOrder)
                                    End If

                                End If

                                dtRules = dtDummy
                                drRules = dtRules.Select()
                                '---------------
                                If res Is Nothing Or res = "0" Then

                                    If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                        SetResult(res, resOrder, CurrentParameterIndex, dtParameters, _dtParamListConfiguration, "False")
                                    Else
                                        SetResult(res, resOrder, CurrentParameterIndex, dtParameters, _dtParamListModification, "False")
                                    End If



                                    SetCurrentParamIndex("_", 1)
                                    Continue While
                                Else
                                    If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                        Dim dtT As DataTable = _dtParamListConfiguration.Copy
                                        ShowRules(CurrentParameterIndex, drRules, dtParameters, dtT)
                                    Else
                                        ShowRules(CurrentParameterIndex, drRules, dtParameters, _dtParamListModification)
                                    End If


                                    'If Boolean.Parse(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagEndParameterArrivedFromExistQuotation)) Then
                                    '    pnlSetParam.Visible = False : pnlNote.Visible = False : pnlRulles.Visible = False
                                    '    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagEndParameterArrivedFromExistQuotation, False)
                                    'End If


                                    Exit While
                                End If
                            Else
                                If Not dtRules Is Nothing Then
                                    dtRulesFilter &= " AND VisibleHelp = true " 'AND InputLimit = 'unvisible' 
                                    drRules = dtRules.Select(dtRulesFilter)
                                End If
                                If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                    Dim dtT As DataTable = _dtParamListConfiguration.Copy

                                    ShowRules(CurrentParameterIndex, drRules, dtParameters, dtT)
                                Else
                                    ShowRules(CurrentParameterIndex, drRules, dtParameters, _dtParamListModification)
                                End If

                                'If Boolean.Parse(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagEndParameterArrivedFromExistQuotation)) Then
                                '    If _dtParamList.Rows.Count = 0 Then
                                '        pnlSetParam.Visible = True
                                '        pnlRulles.Visible = True
                                '        pnlNote.Visible = True
                                '    Else
                                '        pnlSetParam.Visible = False
                                '        pnlRulles.Visible = False
                                '        pnlNote.Visible = False
                                '    End If
                                '    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagEndParameterArrivedFromExistQuotation, False)
                                'End If
                                Exit While
                            End If
                        Else
                            'check InputLimit, Parameter is not for present
                            Dim res As String
                            Dim resOrder As String = ""
                            Dim dvDummy As DataTable
                            dvDummy = Nothing
                            'res = ComputeFormula(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString, _dtParamList, CurrentParameterIndex, dvDummy, resOrder)



                            If dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString.ToUpper = "REFERENCE" Then

                                If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then

                                    'Dim dv As DataView = _dtParamListConfiguration.DefaultView
                                    'dv.RowFilter = "Label=DC"
                                    'Dim dcVal As String = dv.Item(0)(1)
                                    res = ""
                                Else

                                    Dim dv As New DataView
                                    dv = _dtParamListModification.DefaultView
                                    dv.RowFilter = "CostName='DC'"
                                    Dim dcVal As String = dv.Item(0)("measure")
                                    res = GetNewRefernceDependsOnParam(clsQuatation.ACTIVE_ModelNumber, "{DC}", dcVal) 'returnCurentModelNumber
                                    dv.RowFilter = ""
                                    dv = Nothing
                                End If

                                'res = ComputeFormula(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString, _dtParamListModification, CurrentParameterIndex, dvDummy, resOrder)
                                If CheckIfReferenceForModificationChanged(dtParameters, CurrentParameterIndex - 1, res) = True Then
                                    'start New quotaion after get data from catalog by New reference
                                    'ContinueEmptyValues = True
                                    'If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Temp_ClearAllForModification, False) <> "TRUE" Then
                                    Dim sSuccess As Boolean = True
                                    Get_ParamsGridVal(True, sSuccess)

                                    If sSuccess = False Then
                                        'Response.Redirect("..\forms\Notification.aspx", False)
                                    End If

                                End If
                            Else
                                If RunAllParams And sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                                    'res = GetParameterStaticResult(dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString)
                                    If checkIfResunvisible(dtParameters, dvDummy, resOrder).ToString.ToUpper <> "UNVISIBLE" Then
                                        If dtParameters.Rows(CurrentParameterIndex - 1)("GPNUM_ISO").ToString <> "" Then
                                            res = GetParameterStaticResult(dtParameters.Rows(CurrentParameterIndex - 1)("GPNUM_ISO").ToString, dtParameters, CurrentParameterIndex, dvDummy, resOrder)
                                        Else
                                            res = GetParameterStaticResult(dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString, dtParameters, CurrentParameterIndex, CType(Nothing, DataTable), resOrder)
                                        End If
                                    End If

                                Else
                                    If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                        res = ComputeFormula(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString, _dtParamListConfiguration, CurrentParameterIndex, dvDummy, resOrder)
                                    Else
                                        res = ComputeFormula(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString, _dtParamListModification, CurrentParameterIndex, dvDummy, resOrder)
                                    End If

                                End If
                            End If

                            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                SetResult(res, resOrder, CurrentParameterIndex, dtParameters, _dtParamListConfiguration)
                            Else
                                SetResult(res, resOrder, CurrentParameterIndex, dtParameters, _dtParamListModification)
                            End If
                            SetCurrentParamIndex("_", 1)
                            Continue While


                        End If
                        dtRulesFilter &= " AND VisibleForHelp=true"
                        drRules = dtRules.Select(dtRulesFilter)

                    Else
                        Dim res As String
                        Dim resOrder As String = ""
                        Dim dvDummy As DataTable = Nothing

                        'res = ComputeFormula(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString, _dtParamList, CurrentParameterIndex, dvDummy, resOrder)

                        Dim sFlagParamChanged As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagParametersChanged, False)

                        Dim sFp As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagModificationRecursiveFinish, False)






                        If dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString.ToUpper = "REFERENCE" AndAlso sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then

                            Get_DC()
                            GetItemCat()

                            res = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber).ToString
                        ElseIf dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString.ToUpper = "REFERENCE" AndAlso sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION AndAlso sFp.ToUpper = "TRUE" Then

                            Dim dv As New DataView
                            dv = _dtParamListModification.DefaultView
                            dv.RowFilter = "CostName='DC'"
                            'Dim dcVal As String = LocalizationManager.UnCulturingNumber(dv.Item(0)("measure"))
                            Dim dcVal As String = dv.Item(0)("measure")


                            res = GetNewRefernceDependsOnParam(clsQuatation.ACTIVE_ModelNumber, "{DC}", dcVal) 'returnCurentModelNumber
                            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber).ToString = "0" Then
                                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.itemNumber, res.ToString.Trim)
                                GetCatalogForNewReference(res.ToString.Trim)
                            ElseIf IsNumeric(res.ToString) AndAlso SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber).ToString <> res Then
                                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.itemNumber, res.ToString.Trim)
                                GetCatalogForNewReference(res.ToString.Trim)

                            End If
                            dv.RowFilter = ""
                            dv = Nothing

                        ElseIf dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString.ToUpper = "CATALOGITEM" Then
                            res = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.CATALOGITEMNUMBER, False)

                        Else
                            'RunAllParams And
                            'If RunAllParams = False Then Stop
                            If sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                                'res = GetParameterStaticResult(dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString)
                                If checkIfResunvisible(dtParameters, dvDummy, resOrder).ToString.ToUpper <> "UNVISIBLE" Then
                                    If dtParameters.Rows(CurrentParameterIndex - 1)("GPNUM_ISO").ToString <> "" Then
                                        res = GetParameterStaticResult(dtParameters.Rows(CurrentParameterIndex - 1)("GPNUM_ISO").ToString, dtParameters, CurrentParameterIndex, CType(Nothing, DataTable), resOrder)
                                    Else
                                        res = GetParameterStaticResult(dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString, dtParameters, CurrentParameterIndex, CType(Nothing, DataTable), resOrder)
                                    End If
                                End If

                            Else

                                If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                    res = ComputeFormula(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString, _dtParamListConfiguration, CurrentParameterIndex, dvDummy, resOrder)
                                Else
                                    res = ComputeFormula(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString, _dtParamListModification, CurrentParameterIndex, dvDummy, resOrder)
                                End If

                            End If

                        End If


                        If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                            SetResult(res, resOrder, CurrentParameterIndex, dtParameters, _dtParamListConfiguration)
                        Else
                            SetResult(res, resOrder, CurrentParameterIndex, dtParameters, _dtParamListModification)
                        End If



                        SetCurrentParamIndex("_", 1)
                        Continue While
                    End If
                End If
                SetCurrentParamIndex("_", 1)
                Continue While

            End While
        Catch ex As Threading.ThreadAbortException
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            'no action
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, False)

            If ex.Message.Contains("There is no row at position") Then
                Response.Redirect("~/Default.aspx", False)
            End If
            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub

    Private Sub SetLiveParams(setValues As Boolean)
        Try
            Dim sStart As String = clsQuatation.ACTIVE_OpenType.ToString
            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListConfigurationLive, "") Is Nothing Or setValues = True Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListConfigurationLive, _dtParamListConfiguration)
                ElseIf SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListConfigurationLive, "").Rows.Count < 4 Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListConfigurationLive, _dtParamListConfiguration)
                End If
            Else
                If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListModificationLive, "") Is Nothing Or setValues = True Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListModificationLive, _dtParamListModification)
                ElseIf SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListModificationLive, "").Rows.Count < 4 Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListModificationLive, _dtParamListModification)
                End If
            End If
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub
    Private Sub SetParamsFromLive()
        Try
            Dim sStart As String = clsQuatation.ACTIVE_OpenType.ToString
            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListConfigurationLive, "") Is Nothing AndAlso SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListConfigurationLive, "").Rows.Count > 0 Then
                    _dtParamListConfiguration = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListConfigurationLive, "")
                End If
            Else
                If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListModificationLive, "") Is Nothing AndAlso SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListModificationLive, "").Rows.Count > 0 Then
                    _dtParamListModification = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListModificationLive, "")
                End If
            End If
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub
    Public Function checkIfResunvisible(dtParameters As DataTable, dvDummy As DataTable, resOrder As String) As String
        Try

            Dim res As String = ComputeFormula(dtParameters.Rows(CurrentParameterIndex - 1)("Formula").ToString, clsQuatation.GetActiveQuotation_DTparams, CurrentParameterIndex, dvDummy, resOrder)
            Return res
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Function
    Private Sub SetCurrentParamIndex(manul As String, val As Integer)
        Try
            'Try
            '    If CurrentParameterIndex = 43 Then Stop
            'Catch ex As Exception

            'End Try

            If manul = "" Then
                CurrentParameterIndex = val
            Else
                CurrentParameterIndex += 1
            End If

        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub
    Private Sub setParamValueInDT_Modi_Conf(ParamFormula As String, ParamValue As String)
        Try

            For Each r As DataRow In clsQuatation.GetActiveQuotation_DTparams.Rows ' ReturnCurentDTparams.Rows
                If r.Item("Formula").ToString = ParamFormula Then
                    r.Item("Measure") = ParamValue
                End If
            Next

        Catch ex As Exception

        End Try
    End Sub


    Private Function GetNewRefernceDependsOnParam(FamilyNum As String, ParamReference As String, ParamValue As String) As String
        Try
            Dim dt As DataTable = Nothing
            If FamilyNum <> "" AndAlso ParamReference <> "" AndAlso ParamValue <> "" AndAlso IsNumeric(FamilyNum) AndAlso IsNumeric(ParamValue) Then

                dt = clsReference.FindStandardItemModification(CInt(FamilyNum), ParamReference, CDec(ParamValue))
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    Return dt.Rows(0).Item("ItemRefrence").ToString.Trim
                End If

            End If
            Return ""
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            Return ""
        End Try
    End Function
    Private Function CheckIfReferenceForModificationChanged(dtP As DataTable, ParamIndexToCheck As Integer, res As String) As Boolean
        Try

            If dtP.Rows(ParamIndexToCheck).Item("Label").ToString.ToUpper.Trim = "REFERENCE" Then
                Dim s As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber, False)
                If s <> res Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.itemNumber, res)
                    GetCatalogForNewReference(res)
                    'Start From begining
                    Return True
                End If
            End If
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            Return False
        End Try
    End Function
    Private Sub GetCatalogForNewReference(itmno As String)

        Try
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtModParamsFinalSelected, CType(Nothing, DataTable))

            Dim sLang As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.language).ToString
            Dim svers As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.vers).ToString
            Dim dt As DataTable = Nothing
            Dim dv As DataView = SemiApp_bl.CatalogIscarData.GetItemParametersMobileISO(itmno, svers, sLang, True)
            dt = dv.ToTable
            Dim dt1 As DataTable = dv.Table

            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Temp_ClearAllForModification, False) <> "TRUE" Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamsFromCatalog, dt)
            End If
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub

    Private Sub ShowRules(ByVal CurrentParameterIndex As Integer, ByVal drs() As DataRow, ByVal dtParameters As DataTable, ByVal _dtParamList As DataTable)

        Try

            Dim paramLabel As String = dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString

            If paramLabel <> "" Then
                Dim SetL As String = SetCation '"Set"
                Dim costnameLabel As String = dtParameters.Rows(CurrentParameterIndex - 1)("CostName").ToString
                Dim cn As String = ""
                If costnameLabel <> "" And costnameLabel <> paramLabel Then
                    'cn = " (" & costnameLabel & ")"
                    cn = " (" & clsLanguage.GetLanguageLabels(costnameLabel) & ")"
                End If
                If paramLabel.Contains(")") Then
                    cn = ""
                End If
                'Dim st As String = SetL & " " & paramLabel & cn
                Dim st As String = SetL & " " & clsLanguage.GetLanguageLabels(paramLabel) & cn
                If st <> "" Then
                    lblCurrnetParameterName.Text = st
                Else
                    lblCurrnetParameterName.Text = paramLabel
                End If
                If costnameLabel <> "" Then
                    Dim bPro As Boolean = False
                    If Not dtParameters.Rows(CurrentParameterIndex - 1)("PropertyImageRequirement") Is Nothing Then
                        If dtParameters.Rows(CurrentParameterIndex - 1)("PropertyImageRequirement").ToString = "True" Then
                            bPro = True
                        End If
                    End If
                    Dim s As String = dtParameters.Rows(CurrentParameterIndex - 1)("PropertyImage").ToString
                    Dim sl As String = dtParameters.Rows(CurrentParameterIndex - 1)("SelectModelImage").ToString
                End If
            Else
                pnlSetParam.CssClass = "DisplayNone"
            End If

            Dim dt As DataTable = Nothing
            If drs.Length > 0 Then
                dt = drs(0).Table.Clone

                For Each dr As DataRow In drs
                    Dim newDr As DataRow = dt.NewRow
                    For i As Integer = 0 To dt.Columns.Count - 1
                        newDr(i) = dr(i)
                    Next
                    'If Not IsDuplicate(_dtParamList, newDr) Then
                    dt.Rows.Add(newDr)
                    'End If
                Next

                ComputeRulesVaues(dt.Select, dtParameters, _dtParamList)

                'show rules     
                'se1.BoundControlID = ""
                dgvRulles.DataSource = dt '"inputlimit <> 'unvisible'"
                dgvRulles.DataBind()


                If dt.Rows.Count > 0 Then
                    divslider.Style.Add("display", "none")
                    divGridRulls.Style.Add("display", "none")

                    For Each row As GridViewRow In dgvRulles.Rows

                        If Server.HtmlDecode(row.Cells(GridValuesIndex.Operation).Text.ToString) = "<>" Then
                            divslider.Style.Add("display", "sold")
                        End If
                        If Server.HtmlDecode(row.Cells(GridValuesIndex.Operation).Text.ToString) = "=" Then
                            divGridRulls.Style.Add("display", "sold")
                        End If


                        If Server.HtmlDecode(row.Cells(GridValuesIndex.Operation).Text.ToString) = "<>" AndAlso (Server.HtmlDecode(row.Cells(GridValuesIndex.LowLimitH).Text).ToString.Trim = "") Then
                            row.Cells(GridValuesIndex.MinBtn).CssClass = "cssBoundFieldDisplayNone"
                            row.Cells(GridValuesIndex.lblMakf).CssClass = "cssBoundFieldDisplayNone"
                        ElseIf Server.HtmlDecode(row.Cells(GridValuesIndex.Operation).Text.ToString) = "<>" AndAlso (Server.HtmlDecode(row.Cells(GridValuesIndex.HeightLimitH).Text).ToString.Trim = "") Then
                            row.Cells(GridValuesIndex.DetailsBtn).CssClass = "cssBoundFieldDisplayNone"
                            row.Cells(GridValuesIndex.lblMakf).CssClass = "cssBoundFieldDisplayNone"
                        End If

                        If Server.HtmlDecode(row.Cells(GridValuesIndex.Operation).Text.ToString) <> "<>" AndAlso Server.HtmlDecode(row.Cells(GridValuesIndex.LowLimitH).Text).ToString.Trim = "" Then
                            row.Cells(GridValuesIndex.MinBtn).CssClass = "cssBoundFieldDisplayNone"
                            row.Cells(GridValuesIndex.lblMakf).CssClass = "cssBoundFieldDisplayNone"

                        End If

                        If Server.HtmlDecode(row.Cells(GridValuesIndex.Operation).Text.ToString) <> "<>" AndAlso Server.HtmlDecode(row.Cells(GridValuesIndex.HeightLimitH).Text).ToString.Trim = "" Then
                            row.Cells(GridValuesIndex.DetailsBtn).CssClass = "cssBoundFieldDisplayNone"
                            row.Cells(GridValuesIndex.lblMakf).CssClass = "cssBoundFieldDisplayNone"
                        End If

                        If Server.HtmlDecode(row.Cells(GridValuesIndex.Operation).Text.ToString) = "<>" Then
                            row.CssClass = "DisplayNone"
                            'divGridRulls.CssClass = "DisplayNone"

                        End If
                    Next
                End If


                If RullsCount.Text <> "END" Then
                    Try
                        If dgvRulles.Rows.Count > 4 Then
                            RullsCount.Text = "CLOSE"
                        Else
                            RullsCount.Text = "OPEN"
                        End If
                    Catch ex As Exception
                        RullsCount.Text = "CLOSE"

                    End Try
                End If

                Dim dvDummy As DataTable = Nothing
                Dim OrderDummy As String = ""
                Dim sRem As String = dtParameters.Rows(CurrentParameterIndex - 1)("Remark").ToString.Trim
                'pnlNote.Visible = False
                pnlNote.Style.Add("display", "none")
                If Not String.IsNullOrEmpty(sRem) Then
                    txtRemark.Text = ComputeFormula(sRem, _dtParamList, CurrentParameterIndex - 1, dvDummy, OrderDummy)
                    If Trim(txtRemark.Text) <> "" AndAlso Trim(txtRemark.Text) <> """""" Then
                        'pnlNote.Visible = True
                        pnlNote.Style.Add("display", "block")
                    End If
                End If

                'dgvRulles.Enabled = True
                dgvRulles.CssClass = "cssGridRulles"
                'pnlRulles.Visible = True
                pnlRulles.CssClass = "pnlRullescss"

            Else
                Dim dvDummy As DataTable = Nothing
                Dim OrderDummy As String = ""
                Dim sRem As String = dtParameters.Rows(CurrentParameterIndex - 1)("Remark").ToString.Trim
                'pnlNote.Visible = False
                pnlNote.Style.Add("display", "none")
                If Not String.IsNullOrEmpty(sRem) Then
                    txtRemark.Text = ComputeFormula(sRem, _dtParamList, CurrentParameterIndex - 1, dvDummy, OrderDummy)
                    If Trim(txtRemark.Text) <> "" AndAlso Trim(txtRemark.Text) <> """""" Then
                        'pnlNote.Visible = True
                        pnlNote.Style.Add("display", "block")
                    End If
                End If
                'se1.BoundControlID = ""
                dgvRulles.DataSource = dt
                dgvRulles.DataBind()
            End If

            If Not _dtParamList Is Nothing Then
                Dim dv_Params As DataView = _dtParamList.Copy.DefaultView
                dv_Params.RowFilter = "visiblegrid=true AND Measure<>'' AND Measure<>'UNVISIBLE'"
                Fill_gvParamsList(dv_Params)
            End If


        Catch ex As Threading.ThreadAbortException
            'no action
        Catch ex As GeneralException
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex)
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex)
            Dim _ex As New GeneralException(String.Empty, ex)
        Finally
            'If txtValue0.Text = "" Then
            'txtValue0.Text = txtValue.Text
            'End If
        End Try

        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-ShowRules-" & i_counter)

    End Sub

    Private Sub SetSliderStyle(sControl As Control)
        Try
            For Each CurrentControl As Control In sControl.Controls
                If Not CurrentControl.ID Is Nothing Then
                    'Response.Write(CurrentControl.ID.ToString)
                    lblSliderId.Text &= CurrentControl.ID.ToString
                End If
                SetSliderStyle(CurrentControl)

            Next
        Catch ex As Threading.ThreadAbortException
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            'no action
        Catch ex As GeneralException
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex)
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex)
            Dim _ex As New GeneralException(String.Empty, ex)
        End Try
    End Sub

    Private Sub ComputeRulesVaues(ByVal drs() As DataRow, ByVal dtParameters As DataTable, ByVal _dtParamList As DataTable)

        Try
            Dim LowLimit As String
            Dim HeighLimit As String
            Dim LowLimitH As String
            Dim HeighLimitH As String
            Dim Remarks As String
            Dim inputLimit As String

            Dim ResLowLimit As String
            Dim ResHeighLimit As String
            Dim ResLowLimitH As String
            Dim ResHeighLimitH As String


            Dim dvDummy As DataTable = Nothing
            Dim OrderDummy As String = ""

            For Each dr As DataRow In drs
                '--InputLimit--
                inputLimit = dr("inputLimit").ToString

                Try
                    Dim sTin As Integer = inputLimit.IndexOf("{")
                    If sTin > -1 Then
                        If Not IsNumeric(inputLimit.Substring(sTin + 1, 1)) Then
                            inputLimit = FormulaResult.Conver_InputLimit_Formula(inputLimit, _dtParamList)
                        End If
                    End If
                Catch ex As Exception

                End Try




                inputLimit = ComputeFormula(inputLimit, _dtParamList, CurrentParameterIndex, dvDummy, OrderDummy)
                If inputLimit.ToUpper = "UNVISIBLE" Then
                    dr.Delete()
                Else
                    LowLimit = dr("LowLimit").ToString
                    HeighLimit = dr("HeightLimit").ToString
                    LowLimitH = dr("LowLimitH").ToString
                    HeighLimitH = dr("HeightLimitH").ToString
                    Remarks = dr("Remarks").ToString


                    ResHeighLimit = ComputeFormula(HeighLimit, _dtParamList, CurrentParameterIndex, dvDummy, OrderDummy)
                    If HeighLimit = HeighLimitH Then
                        ResHeighLimitH = ResHeighLimit
                    Else
                        ResHeighLimitH = ComputeFormula(HeighLimitH, _dtParamList, CurrentParameterIndex, dvDummy, OrderDummy)
                    End If

                    ResLowLimit = ComputeFormula(LowLimit, _dtParamList, CurrentParameterIndex, dvDummy, OrderDummy)
                    If LowLimit = LowLimitH Then
                        ResLowLimitH = ResLowLimit
                    Else
                        ResLowLimitH = ComputeFormula(LowLimitH, _dtParamList, CurrentParameterIndex, dvDummy, OrderDummy)
                    End If

                    '--Remarks--
                    Remarks = ComputeFormula(Remarks, _dtParamList, CurrentParameterIndex, dvDummy, OrderDummy)


                    '------------------
                    Try
                        If dr("RullNotation").ToString <> "" Then
                            Dim sRullNotation As String = dr("RullNotation").ToString
                            Dim sRullNotationF As New FormulaResult(sRullNotation, _dtParamList, 0, Nothing)
                            dr("RullNotation") = sRullNotationF.ParseAndCalculate()
                            sRullNotationF = Nothing
                        End If
                    Catch ex As Exception

                    End Try

                    '------------------

                    dr("LowLimit") = ResLowLimit
                    dr("HeightLimit") = ResHeighLimit
                    dr("LowLimitH") = ResLowLimitH
                    dr("HeightLimitH") = ResHeighLimitH
                    dr("Remarks") = Remarks
                    dr("inputLimit") = inputLimit
                End If



            Next
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub

    Private Sub SetResult(ByVal res As String, ByVal resOrder As String, ByVal CurrentParameterIndex As Integer, ByVal dtParameters As DataTable, ByVal _dtParamList As DataTable, Optional ByVal VisibleGrid As String = "", Optional SPECIAL_FUNCTION_QB As String = "", Optional setPrevParam As Boolean = False)

        Try
            If _dtParamList.Rows.Count <= CurrentParameterIndex - 1 Then
                Dim dr As DataRow = _dtParamList.NewRow
                For i As Integer = 0 To dtParameters.Columns.Count - 1
                    dr(dtParameters.Columns(i).ColumnName) = dtParameters.Rows(CurrentParameterIndex - 1)(i)
                Next
                dr("Measure") = ""
                dr("Order") = ""
                dr("PictSelect") = ""
                dr("PictHelp") = ""
                '-------------------------
                If setPrevParam = True Then
                    Dim sStart As String = clsQuatation.ACTIVE_OpenType.ToString

                    Dim sEnableParam As String = dr("MainParameters")
                    If sEnableParam = "1" Or (sStart <> "" AndAlso sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION) Then

                        Dim PrevParam As Integer = 0
                        Try
                            PrevParam = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.PrevParameterSelected)
                        Catch ex As Exception

                        End Try
                        dr("PrevParam") = PrevParam - 1

                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PrevParameterSelected, CInt(CurrentParameterIndex))

                    End If
                    dr("Label") = dr("Label").ToString.Replace("(" & dr("CostName") & ")", "").Trim()
                End If

                '-------------------------
                'dr("StringValue") = ""
                'dr("DescValue") = ""
                If Not IsDuplicate(_dtParamList, dr) Then
                    'dr("Label") = SET_PARAMETER_LabelNAME(dr("GPNUM_ISO").ToString.Trim, _dtParamList, dr)
                    _dtParamList.Rows.Add(dr)
                End If
            Else
                If SPECIAL_FUNCTION_QB <> "DESCVALUE" And SPECIAL_FUNCTION_QB <> "STRINGVALUE" Then

                    For i As Integer = 0 To dtParameters.Columns.Count - 1
                        _dtParamList.Rows(CurrentParameterIndex - 1)(dtParameters.Columns(i).ColumnName) = dtParameters.Rows(CurrentParameterIndex - 1)(i)
                    Next
                End If
            End If
            Dim PicName As String = _dtParamList.Rows(CurrentParameterIndex - 1)("PictSelect").ToString.Trim

            Dim formatf As String = _dtParamList.Rows(CurrentParameterIndex - 1)("formatformula").ToString.Trim
            Dim oFormulaFormat As New FormulaResult(formatf, _dtParamList, 0, Nothing)
            formatf = oFormulaFormat.ParseAndCalculate()
            oFormulaFormat = Nothing
            'If formatf <> "" Then
            '    Dim gg As String = "df"
            'End If


            If SPECIAL_FUNCTION_QB = "STRINGVALUE" Then
                Dim sStrVal As String = RemoveQuetes(res)
                '07.07.22 change UnCulturingNumber
                'sStrVal = FormatFormula(LocalizationManager.UnCulturingNumber(sStrVal), formatf)
                sStrVal = FormatFormula(sStrVal, formatf)

                'If VisibleGrid = False Then
                '    _dtParamList.Rows(CurrentParameterIndex - 1)("VisibleGrid") = "False"
                'End If

                '07.07.22 change UnCulturingNumber
                '_dtParamList.Rows(CurrentParameterIndex - 1)("StringValue") = LocalizationManager.UnCulturingNumber(sStrVal)
                _dtParamList.Rows(CurrentParameterIndex - 1)("StringValue") = sStrVal
                _dtParamList.Rows(CurrentParameterIndex - 1)("Order") = resOrder
                res = _dtParamList.Rows(CurrentParameterIndex - 1)("Measure")
            End If

            If SPECIAL_FUNCTION_QB = "DESCVALUE" Then
                Dim sStrVal As String = RemoveQuetes(res)
                '07.07.22 change UnCulturingNumber
                'sStrVal = FormatFormula(LocalizationManager.UnCulturingNumber(sStrVal), formatf)
                sStrVal = FormatFormula(sStrVal, formatf)

                'If VisibleGrid = False Then
                '    _dtParamList.Rows(CurrentParameterIndex - 1)("VisibleGrid") = "False"
                'End If

                '07.07.22 change UnCulturingNumber
                ' _dtParamList.Rows(CurrentParameterIndex - 1)("DescValue") = LocalizationManager.UnCulturingNumber(sStrVal)
                _dtParamList.Rows(CurrentParameterIndex - 1)("DescValue") = sStrVal
                _dtParamList.Rows(CurrentParameterIndex - 1)("Order") = resOrder
                res = _dtParamList.Rows(CurrentParameterIndex - 1)("Measure")
            End If


            Dim sMeasure As String = RemoveQuetes(res)

            sMeasure = FormatFormula(sMeasure, formatf)

            If VisibleGrid = "False" Then
                _dtParamList.Rows(CurrentParameterIndex - 1)("VisibleGrid") = "False"
            End If

            _dtParamList.Rows(CurrentParameterIndex - 1)("Measure") = sMeasure
            _dtParamList.Rows(CurrentParameterIndex - 1)("Order") = resOrder

            If PicName = "" And resOrder <> "" And SPECIAL_FUNCTION_QB = "" Then
                Dim dtRulesFilter As String
                dtRulesFilter = "LabelNum=" & CurrentParameterIndex & " And Order= '" & resOrder & "'"
                Dim sStart As String = clsQuatation.ACTIVE_OpenType.ToString 'returnCurentOpenType() ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)
                Dim dtRules As DataTable
                If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                    dtRules = clsModel.GetModelRules(_ModelNumConfiguration)
                Else
                    dtRules = clsModel.GetModelRules(_ModelNumModification)
                End If

                Dim drRules As DataRow()
                drRules = dtRules.Select(dtRulesFilter)
                If drRules.Length = 1 Then
                    _dtParamList.Rows(CurrentParameterIndex - 1)("PictSelect") = drRules(0)("PictSelect")
                End If
            End If

            'SetParameterPictureOLD(PicName)
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub

    Private Function PreComputeFormula(ByRef ParamFormula As String, ByVal dtParamList As DataTable) As Boolean
        'return true when need to continue to ComputeFormula
        Try
            'if is empty
            If ParamFormula = "" Then
                Return False
            End If

            'if is numeric
            If IsNumeric(ParamFormula) Then
                Return False
            End If

            'if is free text    -->         "..."               'TODO: check it
            Dim res As String
            Dim QoutesRemoved As Boolean
            res = RemoveQuetes(ParamFormula, QoutesRemoved)
            If QoutesRemoved Then
                ParamFormula = res
                Return False
            End If

            'if is single parameter -->     {x}
            If Len(ParamFormula) > 2 Then
                If Left(ParamFormula, 1) = "{" AndAlso Right(ParamFormula, 1) = "}" AndAlso IsNumeric(Mid(ParamFormula, 2, Len(ParamFormula) - 2)) Then
                    ParamFormula = dtParamList.Rows((Mid(ParamFormula, 2, Len(ParamFormula) - 2)) - 1)("Measure").ToString
                    Return False
                End If
            End If

            Return True
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Function

    Private Function ComputeFormula(ByVal ParamFormula As String, ByVal dtParamList As DataTable, ByVal CurrentParameterIndex As Integer, ByRef ResultSetRules As DataTable, ByRef ResOrder As String) As String

        ComputeFormula = Nothing
        Try

            Try
                Dim sTin As Integer = ParamFormula.IndexOf("{")
                If sTin > -1 Then
                    If Not IsNumeric(ParamFormula.Substring(sTin + 1, 1)) Then
                        ParamFormula = FormulaResult.Conver_InputLimit_Formula(ParamFormula, dtParamList)
                    End If
                End If
            Catch ex As Exception

            End Try


            Trace.Write("ComputeFormula, Start")
            If Not PreComputeFormula(ParamFormula, dtParamList) Then
                Trace.Write("ComputeFormula, PreComputeFormula")

                Return ParamFormula
            End If
            Trace.Write("ComputeFormula, PreComputeFormula After")

            Dim value As String

            If IsNumeric(ParamFormula) Then

                value = ParamFormula
                ResOrder = Nothing
            Else
                Dim oFormula As New FormulaResult(ParamFormula, dtParamList, CurrentParameterIndex, ResultSetRules)
                value = oFormula.ParseAndCalculate

                ResultSetRules = oFormula.ResultSetRules

                ResOrder = oFormula.ResOrder
                oFormula = Nothing
            End If

            Return value
        Catch ex As Exception
            'GeneralException.WriteEventErrors("ComputeFormula-" & ex.Message, GeneralException.e_LogTitle.PARAMETERS.ToString)
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message & " - Param Index= " & CurrentParameterIndex)
        End Try

    End Function


    Protected Sub dgvParamList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgvParamList.RowCommand
        Try

            If e.CommandName = "SelectParam" Then

                SelectParamFromList(dgvParamList.Rows(e.CommandArgument).Cells(GridParamList.PrevParam).Text)

                dgvParamList.Rows(e.CommandArgument).BackColor = Drawing.Color.FromArgb(206, 220, 234) 'FFFBD6
                For i As Integer = e.CommandArgument + 1 To dgvParamList.Rows.Count - 1
                    dgvParamList.Rows(i).BackColor = Drawing.Color.FromArgb(255, 255, 255)
                    dgvParamList.Rows(i).ForeColor = Drawing.Color.FromArgb(211, 211, 211)
                Next

            End If
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Protected Sub dgvParamList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgvParamList.RowDataBound


        Try
            'hide first empty line
            'TODO: check perfomrance affected !!!!!!!!
            If e.Row.RowType = DataControlRowType.DataRow Then
                If IsNumeric(e.Row.Cells(GridParamList.PrevParam).Text) Then
                    CType(e.Row.Cells(GridParamList.SelectParameterIcon).Controls(0), ImageButton).Visible = True
                Else
                    CType(e.Row.Cells(GridParamList.SelectParameterIcon).Controls(0), ImageButton).Visible = False
                End If

                e.Row.Cells(GridParamList.Measure).Text = e.Row.Cells(GridParamList.Measure).Text

                If e.Row.Cells(GridParamList.Label).Text.ToString.Trim.Contains(e.Row.Cells(GridParamList.CostName).Text.ToString.Trim) Then
                    '22.07.22
                    ' If LocalizationManager.CulturingNumber(e.Row.Cells(GridParamList.Label).Text.ToString.Trim).Contains(e.Row.Cells(GridParamList.CostName).Text.ToString.Trim) Then
                    e.Row.Cells(GridParamList.CostName).Text = ""
                End If


            End If
            '
            ''13.08.20

            If e.Row.RowType = DataControlRowType.DataRow Then
                ListVieItemCounter = 1
                If (e.Row.RowState = DataControlRowState.Alternate) Then

                    e.Row.Attributes.Add("onmouseover", "lblTitle.BackColor='BLUE'")

                Else

                End If
            End If

        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try


    End Sub

    Private Sub VisiblityImages()
        Try
            If clsQuatation.ACTIVE_OpenType = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                ImageLeftBoderID.Style.Add("display", "none")
                'ImgMainImageRID.Style.Add("height", "100%")
                'ImgMainImageR.Style.Add("height", "")
                'ImgMainImageR.CssClass = ""
            End If

        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        Try

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagModificationRecursiveFinish, True)

            If Not Request("GetPrice") Is Nothing AndAlso Request("GetPrice").ToString = "true" Then


            Else

                Try
                    If isEditMode Then
                        SessionManager.ClearSessionForEditMode()
                    Else
                        btnSelectParam.Attributes.Remove("onclick")
                    End If

                    Try
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CurrentParameterIndex, CurrentParameterIndex)
                        Dim sStart As String = clsQuatation.ACTIVE_OpenType.ToString

                        If sStart <> "" Then

                            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListConfiguration, _dtParamListConfiguration)

                            ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                                btnClearAll.Enabled = False
                                btnClearAll.Visible = False
                                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListModification, _dtParamListModification)
                                If (_Quotation_Status <> clsQuatation.e_QuotationStatus.Exist_QutOpenedFromQuotationList AndAlso _Quotation_Status <> clsQuatation.e_QuotationStatus.Exist_Qut_OpenedFromParameters) AndAlso Not dgvParamList Is Nothing AndAlso dgvParamList.Rows.Count <= 0 Then
                                    Dim dtParameters As DataTable = clsModel.GetModelParameters(_ModelNumModification, True)
                                    ContinueRecursiveMODIF(dtParameters)
                                End If
                            End If
                        Else
                            btnClearAll.Enabled = False
                            btnClearAll.Visible = False
                        End If

                        SetDefaultValue()

                        Dim v As DataView = dgvParamList.DataSource
                        If Not v Is Nothing Then
                            BindListView(v.ToTable)

                        End If

                        If Not IsPostBack Then
                            SetTabs()
                            SessionManager.Clear_Sessions_ForBeginSendMessages()
                        End If

                    Catch ex As Exception

                    End Try


                Catch ex As Exception
                    GeneralException.BuildError(Page, ex.Message)
                Finally
                    VisiblityImages()
                End Try

            End If


            Try
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "CheckDivWidthBuilder", "SetDivHeightWithScrollBuilder();", True)
            Catch ex As Exception

            End Try


        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try


        Try
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetLanControlsCaption", "SetCaptionForLabels()", True)
            'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "disaleNewbtnN", "disaleNewbtn()", True)
            'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideAccountBTNN", "HideAccountBTN()", True)


        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillImages() 'LastSaveImages As Boolean

        Try

            Dim YL As String = ""
            Try
                YL = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempShoeLogInFrame, False)
            Catch ex As Exception
            End Try

            If YL <> "" Then
                If YL = "L" Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempShoeLogInFrame, "")
                End If


                Dim Il As String = ""
                Dim Ir As String = ""
                Try
                    Il = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempLeftImage, False)
                    Ir = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempRightImage, False)
                    If Il <> "" Then
                        ImgMainImageL.ImageUrl = Il
                    End If
                    If Ir <> "" Then
                        ImgMainImageR.ImageUrl = Ir
                    End If

                    'SharedFunction.UserAutinticationNotSucceded("FillImages: ImgMainImageL: " & Il)
                    'SharedFunction.UserAutinticationNotSucceded("FillImages: ImgMainImageR: " & Ir)

                Catch ex As Exception
                    'SharedFunction.UserAutinticationNotSucceded("FillImages: ERROR1 L: " & Il)
                    'SharedFunction.UserAutinticationNotSucceded("FillImages: ERROR1 R: " & Ir)
                End Try


            Else

                If _Quotation_Status = clsQuatation.e_QuotationStatus.New_Qut Or _Quotation_Status = clsQuatation.e_QuotationStatus.Exist_Qut_OpenedFromParameters Then 'And LastSaveImages = False Then
                    Dim dtModel As DataTable = clsQuatation.ACTIVE_ModelDT

                    If Not dtModel Is Nothing AndAlso dtModel.Rows.Count > 0 Then
                        'SharedFunction.UserAutinticationNotSucceded("FillImages: dtModel: " & dtModel.Rows.Count)

                        Dim Left_Picture_Path As String = ""
                        'Dim Left_Picture_Name As String = ""
                        Dim Right_Picture_Path As String = ""
                        Dim Right_Picture_Name As String = ""

                        Dim pp As String = dtModel.Rows(0).Item("PicturePath").ToString.Trim
                        If pp.Contains(";") Then
                            Left_Picture_Path = pp.Split(";")(0)
                            Right_Picture_Path = pp.Split(";")(1)
                        Else
                            Left_Picture_Path = pp
                            Right_Picture_Path = pp
                        End If

                        Try
                            If Not dtParametersListView Is Nothing AndAlso dtParametersListView.Rows.Count > 0 Then
                                'SharedFunction.UserAutinticationNotSucceded("FillImages: dtParametersListView: " & dtParametersListView.Rows.Count)

                                Dim TabIndex As Integer = -1
                                For i As Integer = 0 To dtParametersListView.Rows.Count - 1
                                    If dtParametersListView.Rows(i).Item("TabIndex").ToString = CurrentParameterIndex Then
                                        TabIndex = i
                                        Exit For
                                    End If
                                Next

                                '-----------Left Image-----------
                                'SharedFunction.UserAutinticationNotSucceded("FillImages: ACTIVE_OpenType: " & clsQuatation.ACTIVE_OpenType.ToString)
                                'SharedFunction.UserAutinticationNotSucceded("FillImages: TabIndex: " & TabIndex.ToString)

                                If clsQuatation.ACTIVE_OpenType.ToString = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                    If TabIndex = -1 Then
                                        Exit Sub
                                    End If
                                End If
                                FindImageL(TabIndex, Left_Picture_Path)


                                '-----------Right Image-----------
                                If TabIndex = -1 Then
                                    Exit Sub
                                End If

                                Right_Picture_Name = dtParametersListView.Rows(TabIndex).Item("PropertyImage").ToString.Trim

                                If Right_Picture_Name <> "" Then

                                    FindImageR(Right_Picture_Name, Right_Picture_Path)
                                Else
                                    If ImgMainImageR.ImageUrl = "" Then
                                        For ii As Integer = TabIndex To 0 Step -1
                                            If dtParametersListView.Rows(ii).Item("PropertyImage").ToString.Trim <> "" Then
                                                FindImageR(dtParametersListView.Rows(ii).Item("PropertyImage").ToString.Trim, Right_Picture_Path)
                                                Exit For
                                            End If
                                        Next
                                    End If

                                End If

                                'End If
                            End If
                        Catch ex As Exception

                        End Try

                    End If

                ElseIf _Quotation_Status = clsQuatation.e_QuotationStatus.Exist_QutOpenedFromQuotationList Then ' Or LastSaveImages = True clsQuatation.e_QuotationStatus.EXIST_QUOTATION_MODIF Or _QuotationStatus = clsQuatation.e_QuotationStatus.EXIST_QUOTATION_CONFOG Then
                    Dim dt As DataTable = clsQuatation.GetQuotationListImages(_BranchCode, _QutNo)
                    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                        Try

                            Dim imagem As Byte() = CType(dt.Rows(0).Item("mainPicL"), Byte())
                            Dim PROFILE_PIC As String = Convert.ToBase64String(imagem)
                            ImgMainImageL.ImageUrl = String.Format("data:image/jpg;base64,{0}", PROFILE_PIC)


                            Dim imagemR As Byte() = CType(dt.Rows(0).Item("mainPicR"), Byte())
                            Dim PROFILE_PICR As String = Convert.ToBase64String(imagemR)
                            ImgMainImageR.ImageUrl = String.Format("data:image/jpg;base64,{0}", PROFILE_PICR)

                        Catch ex As Exception

                        End Try
                    End If

                End If


            End If

        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        Finally
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempLeftImage, ImgMainImageL.ImageUrl)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempRightImage, ImgMainImageR.ImageUrl)
        End Try

    End Sub
    Private Sub FindImageL(TabIndex As Integer, Left_Picture_Path As String)

        Try
            Dim Left_Picture_Name As String = ""

            If clsQuatation.ACTIVE_OpenType.ToString = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then

                Left_Picture_Name = dtParametersListView.Rows(TabIndex).Item("SelectModelImage").ToString.Trim

                If Left_Picture_Name = "" Then
                    For ii As Integer = TabIndex To 0 Step -1
                        If dtParametersListView.Rows(ii).Item("SelectModelImage").ToString.Trim <> "" Then
                            Left_Picture_Name = dtParametersListView.Rows(ii).Item("SelectModelImage").ToString.Trim
                            Exit For
                        End If
                    Next
                End If

                Dim sL As String = Left_Picture_Name

                Dim pPath, jpgPic, gifPic, pngPic As String

                If sL <> "" Then

                    Dim ConvertedFormulaL As String = FormulaResult.Conver_Image_Formula(sL, clsQuatation.GetActiveQuotation_DTparams) 'ReturnCurentDTparams

                    Dim oFormulaFormatL As New FormulaResult(ConvertedFormulaL, clsQuatation.GetActiveQuotation_DTparams, 0, Nothing) 'ReturnCurentDTparams

                    sL = oFormulaFormatL.ParseAndCalculate()

                    oFormulaFormatL = Nothing

                    pPath = "~\media\ModelImages\" & Left_Picture_Path & "\"
                    jpgPic = pPath & Replace(sL, """", "") & ".jpg"
                    gifPic = pPath & Replace(sL, """", "") & ".gif"
                    pngPic = pPath & Replace(sL, """", "") & ".png"
                    '  GeneralException.WriteEvent("FillImages: jpgPic: " & jpgPic)

                    If (File.Exists(Server.MapPath(jpgPic))) Then : ImgMainImageL.ImageUrl = jpgPic
                    ElseIf (File.Exists(Server.MapPath(gifPic))) Then : ImgMainImageL.ImageUrl = gifPic
                    ElseIf (File.Exists(Server.MapPath(pngPic))) Then : ImgMainImageL.ImageUrl = pngPic
                    Else
                        If ImgMainImageL.ImageUrl = "" AndAlso TabIndex > 0 Then
                            TabIndex = TabIndex - 1
                            FindImageL(TabIndex - 1, Left_Picture_Path)
                        End If

                    End If
                End If
            ElseIf clsQuatation.ACTIVE_OpenType.ToString = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then ' returnCurentOpenType()
                Dim m As DataTable = _ModelModification
                If Not m Is Nothing AndAlso m.Rows.Count > 0 Then
                    Dim sp As String = m.Rows(0).Item("StartPicture").ToString & ".gif"
                    sp = "~\media\ModelImages\" & Left_Picture_Path & "\" & sp
                    ImgMainImageL.ImageUrl = sp
                End If
            End If
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

        End Try


    End Sub
    Private Sub FindImageR(Right_Picture_Name As String, Right_Picture_Path As String)

        Try
            Dim ConvertedFormulaR As String = FormulaResult.Conver_Image_Formula(Right_Picture_Name, clsQuatation.GetActiveQuotation_DTparams) 'ReturnCurentDTparams
            Dim oFormulaFormatR As New FormulaResult(ConvertedFormulaR, clsQuatation.GetActiveQuotation_DTparams, 0, Nothing) 'ReturnCurentDTparams
            Right_Picture_Name = oFormulaFormatR.ParseAndCalculate()
            oFormulaFormatR = Nothing

            Dim pPath, jpgPic, gifPic, pngPic As String

            pPath = "~\media\ParamImages\" & Right_Picture_Path & "\"
            jpgPic = pPath & Replace(Right_Picture_Name, """", "") & ".jpg"
            gifPic = pPath & Replace(Right_Picture_Name, """", "") & ".gif"
            pngPic = pPath & Replace(Right_Picture_Name, """", "") & ".png"

            If (File.Exists(Server.MapPath(jpgPic))) Then : ImgMainImageR.ImageUrl = jpgPic
            ElseIf (File.Exists(Server.MapPath(gifPic))) Then : ImgMainImageR.ImageUrl = gifPic
            ElseIf (File.Exists(Server.MapPath(pngPic))) Then : ImgMainImageR.ImageUrl = pngPic
            End If
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

        End Try

    End Sub
    Private Function RemoveQuetes(ByVal s As String) As String
        Try


            Dim bDummy As Boolean = False
            Return RemoveQuetes(s, bDummy)

        Catch ex As Exception

        End Try
    End Function

    Private Function FormatFormula(ByVal Mesure As String, ByVal FormatValue As String) As String

        FormatFormula = Nothing
        Try
            FormatValue = Replace(FormatValue, """", "")


            Dim newMesure As String = ""
            Dim iVBbug As Integer
            Dim dcMesure As Decimal
            'If IsNumeric(LocalizationManager.CulturingNumber(Mesure)) AndAlso Not String.IsNullOrEmpty(FormatValue) Then
            If IsNumeric(Mesure) AndAlso Not String.IsNullOrEmpty(FormatValue) Then
                dcMesure = Decimal.Parse(Mesure)
                'dcMesure = Decimal.Parse(LocalizationManager.CulturingNumber(Mesure))
                Select Case FormatValue
                    Case "RU"
                        newMesure = Format(dcMesure + 0.49999, "#0")
                    Case "RD" : newMesure = Math.Round(dcMesure - 0.499999, 0)
                    Case "#0" : newMesure = Format(dcMesure / 10, "#0") * 10
                    Case "FIX000"
                        newMesure = New String("0", 3 - Len(CStr(Fix(dcMesure)))) & CStr(Fix(dcMesure))
                    Case "FIX" : newMesure = Fix(dcMesure)
                    Case "Round" : newMesure = Fix(dcMesure + 0.5)
                    Case "RoundTol" : newMesure = Math.Round(CType(dcMesure, Double))

                    Case "05"
                        If Math.Abs(CInt(dcMesure * 10) Mod 5) < 3 Then
                            newMesure = ((CInt(dcMesure * 10) - (CInt(dcMesure * 10) Mod 5))) / 10
                        Else
                            newMesure = ((CInt(dcMesure * 10) + (IIf((CInt(dcMesure * 10) Mod 5) < 0, Math.Abs((CInt(dcMesure * 10) Mod 5)) - 5, 5 - (CInt(dcMesure * 10) Mod 5))))) / 10
                        End If
                    Case "%4"
                        iVBbug = Format(CDbl(dcMesure) * 100, "0")
                        If dcMesure > 0 Then
                            If Math.Abs(iVBbug Mod 4) < 2 Then
                                newMesure = ((iVBbug - (iVBbug Mod 4))) / 100
                            Else
                                newMesure = ((iVBbug + (4 - (iVBbug Mod 4)))) / 100
                            End If
                        Else
                            If Math.Abs(iVBbug Mod 4) < 2 Then
                                newMesure = ((iVBbug + Math.Abs(iVBbug Mod 4))) / 100
                            Else
                                newMesure = ((iVBbug - (4 - Math.Abs(iVBbug Mod 4)))) / 100
                            End If
                        End If
                    Case "0&"
                        If IsNumeric(dcMesure) Then
                            If CInt(dcMesure / 10) = 1 Then
                                newMesure = "0" & Format(dcMesure, "0")
                            Else
                                newMesure = Format(dcMesure, "0")
                            End If
                        Else
                            newMesure = Format(dcMesure, "0")
                        End If
                    Case "#.#0"
                        newMesure = Format(dcMesure, "#.0")
                        If newMesure = ".0" Or newMesure = ",0" Then newMesure = 0
                    Case "%%%"
                        If dcMesure.ToString.Length > 2 Then
                            newMesure = dcMesure
                        ElseIf dcMesure.ToString.Length = 0 Then
                            newMesure = "000"
                        ElseIf dcMesure.ToString.Length = 1 Then
                            newMesure = "0" & dcMesure & "0"
                        ElseIf dcMesure.ToString.Length = 2 Then
                            newMesure = "0" & dcMesure
                        End If
                    Case "%%"
                        If dcMesure.ToString.Length > 1 Then
                            newMesure = dcMesure
                        ElseIf dcMesure.ToString.Length = 0 Then
                            newMesure = "00"
                        ElseIf dcMesure.ToString.Length = 1 Then
                            newMesure = "0" & dcMesure
                        End If
                    Case "%%%|"
                        If dcMesure.ToString.Length > 3 Then
                            newMesure = Left(Replace(dcMesure, ".", ""), 3)
                        ElseIf dcMesure.ToString.Length > 2 Then
                            dcMesure = Replace(dcMesure, ".", "")
                            newMesure = dcMesure
                            If dcMesure.ToString.Length = 2 Then
                                newMesure = "0" & dcMesure
                            Else
                                newMesure = dcMesure
                            End If
                        ElseIf dcMesure.ToString.Length = 0 Then
                            newMesure = "000"
                        ElseIf dcMesure.ToString.Length = 1 Then
                            newMesure = "0" & dcMesure & "0"
                        ElseIf dcMesure.ToString.Length = 2 Then
                            newMesure = dcMesure & "0"
                        End If
                    Case "%%%%"
                        'dcMesure = dcMesure * 1000
                        dcMesure = Fix(dcMesure)
                        newMesure = New String("0", 4 - Len(Left(dcMesure, 4))) & Left(dcMesure, 4)
                    Case "%%%%%"
                        dcMesure = dcMesure * 1000
                        dcMesure = Fix(dcMesure)
                        newMesure = New String("0", 5 - Len(Left(dcMesure, 5))) & Left(dcMesure, 5)
                    Case "ZogiSGSF"
                        newMesure = Fix(dcMesure) + IIf(Fix(dcMesure + 1.5) Mod 2, 1, 0)
                        newMesure = CInt(newMesure) + IIf(CInt(newMesure + 1.50001) Mod 2, 1, 0)
                        newMesure = IIf(newMesure Mod 2, newMesure - 1, newMesure)
                        '-----------bilal----21.12.5------
                    Case "###|"
                        newMesure = CInt((dcMesure * 10) - 0.499)
                        If dcMesure < 10 Then newMesure = "0" & newMesure
                    Case "###||" : newMesure = Format((Format(CDbl(dcMesure), "0.00") * 100), "000")
                    Case "FSlotForZ" 'TODO --check
                        Dim sStart As String = clsQuatation.ACTIVE_OpenType.ToString 'returnCurentOpenType() ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)

                        If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                            If _dtParamListConfiguration.Rows.Count > 0 Then
                                Dim FirsParamVal As String = _dtParamListConfiguration.Rows(0)("Measure").ToString
                                If Mid(FirsParamVal, FirsParamVal.ToString.Length, 1) = "N" Then
                                    newMesure = Format(dcMesure, "##")
                                    '14.03.12'If CInt(dcMesure) Mod 2 <> 0 Then newMesure = CInt(dcMesure) - 1
                                    If CInt(newMesure) Mod 2 <> 0 Then newMesure = CInt(newMesure) - 1
                                Else
                                    newMesure = Format((CDbl(dcMesure) - 0.3), "##")
                                End If
                            End If
                        ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                            If _dtParamListModification.Rows.Count > 0 Then
                                Dim FirsParamVal As String = _dtParamListModification.Rows(0)("Measure").ToString
                                If Mid(FirsParamVal, FirsParamVal.ToString.Length, 1) = "N" Then
                                    newMesure = Format(dcMesure, "##")
                                    '14.03.12'If CInt(dcMesure) Mod 2 <> 0 Then newMesure = CInt(dcMesure) - 1
                                    If CInt(newMesure) Mod 2 <> 0 Then newMesure = CInt(newMesure) - 1
                                Else
                                    newMesure = Format((CDbl(dcMesure) - 0.3), "##")
                                End If
                            End If
                        End If

                        '    Format(22.5, "##") = 23
                        '    Format(22.3, "##") = 22



                        '----
                        '------------------------------------

                        'Case "0.0#" : newMesure = Format(dcMesure, "0.0#")
                        'Case "0.000" : newMesure = Format(dcMesure, "0.000")
                        'Case "0.0000" : newMesure = Format(dcMesure, "0.0000")
                        'Case "0.000#" : newMesure = Format(dcMesure, "0.000#")
                        'Case "0.0000#" : newMesure = Format(dcMesure, "0.0000#")
                        'Case "0.000##" : newMesure = Format(dcMesure, "0.000##")
                        'Case "000.00" : newMesure = Format(dcMesure, "000.00")
                        'Case "0.00" : newMesure = Format(dcMesure, "0.00")
                        'Case "#.00" : newMesure = Format(dcMesure, "#.00")
                        'Case "#.000" : newMesure = Format(dcMesure, "#.000")
                        'Case "#.0" : newMesure = Format(dcMesure, "#.0")
                        'Case "0.0" : newMesure = Format(dcMesure, "0.0")
                        'Case "##" : newMesure = Format(dcMesure, "##")
                        'Case "##.0#" : newMesure = Format(dcMesure, "##.0#")

                    Case Else
                        newMesure = Format(dcMesure, FormatValue)

                End Select
            End If

            If newMesure = "" Then
                Return Mesure
            Else
                Return newMesure
            End If

        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try
        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-FormatFormula-" & i_counter)

    End Function


    Private Function RemoveQuetes(ByVal s As String, ByRef QutesRemoved As Boolean) As String
        RemoveQuetes = Nothing
        Try
            Dim quot As String = Server.HtmlEncode("""")
            Dim rs As String
            If Len(s) > 2 Then
                If (Left(s, 1) = """" And Right(s, 1) = """") Then
                    rs = Mid(s, 2, Len(s) - 2)
                    QutesRemoved = True
                ElseIf (Left(s, Len(quot)) = quot And Right(s, Len(quot)) = quot) Then
                    rs = Mid(s, Len(quot) + 1, Len(s) - (Len(quot) * 2))
                    QutesRemoved = True
                Else
                    rs = s
                    QutesRemoved = False
                End If
            Else
                rs = s
                QutesRemoved = False
            End If
            Return rs
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.BUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Function


    Protected Sub dgvRulles_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgvRulles.RowCommand

        Try
            If e.CommandName = "DetailsBtn" Or e.CommandName = "MinBtn" Then ' "Details" Then
                Dim HeightLowLimitValue As String
                Dim HeightLowLimit As String
                Dim HeightLowLimittOsET As String

                If e.CommandName.ToUpper = "MINBTN" Then
                    HeightLowLimit = dgvRulles.Rows(e.CommandArgument).Cells(GridValuesIndex.LowLimitH).Text
                    HeightLowLimitValue = dgvRulles.Rows(e.CommandArgument).Cells(GridValuesIndex.LowLimit).Text
                Else
                    HeightLowLimit = dgvRulles.Rows(e.CommandArgument).Cells(GridValuesIndex.HeightLimitH).Text
                    HeightLowLimitValue = dgvRulles.Rows(e.CommandArgument).Cells(GridValuesIndex.HeightLimit).Text
                End If

                '*************************************
                If Server.HtmlDecode(dgvRulles.Rows(e.CommandArgument).Cells(GridValuesIndex.Operation).Text) = "<>" Then
                    If Server.HtmlDecode(dgvRulles.Rows(e.CommandArgument).Cells(GridValuesIndex.LowLimitH).Text).Trim = "" Then
                        HeightLowLimit = dgvRulles.Rows(e.CommandArgument).Cells(GridValuesIndex.HeightLimit).Text
                        HeightLowLimitValue = dgvRulles.Rows(e.CommandArgument).Cells(GridValuesIndex.HeightLimit).Text
                    ElseIf Server.HtmlDecode(dgvRulles.Rows(e.CommandArgument).Cells(GridValuesIndex.HeightLimitH).Text).Trim = "" Then
                        HeightLowLimit = dgvRulles.Rows(e.CommandArgument).Cells(GridValuesIndex.LowLimit).Text
                        HeightLowLimitValue = dgvRulles.Rows(e.CommandArgument).Cells(GridValuesIndex.LowLimit).Text
                    End If
                End If
                '*************************************

                Dim Res As String = ""
                Dim ResOrder As String = ""
                Dim iRuleIndex As Integer = 0
                Dim arOperation As String = Server.HtmlDecode(dgvRulles.Rows(e.CommandArgument).Cells(GridValuesIndex.Operation).Text)

                If arOperation = "=" AndAlso HeightLowLimitValue <> HeightLowLimit Then
                    CheckIllegalParameterValue(HeightLowLimit, Res, ResOrder, iRuleIndex, False)
                    HeightLowLimittOsET = HeightLowLimit
                Else
                    If (arOperation = "=" Or arOperation = "<=" Or arOperation = ">=") AndAlso HeightLowLimitValue <> "" Then
                        CheckIllegalParameterValue(HeightLowLimitValue, Res, ResOrder, iRuleIndex, False)
                        HeightLowLimittOsET = HeightLowLimitValue

                    Else
                        CheckIllegalParameterValue(HeightLowLimit, Res, ResOrder, iRuleIndex, False)
                        HeightLowLimittOsET = HeightLowLimit

                    End If
                End If



                Dim sMsg As String
                sMsg = "The value you requested is out of the iQuote range."


                If Res = "" Or Res = "ERRORMSG" Then
                    Res = ""
                    GeneralException.BuildError(Page, sMsg)
                Else
                    'pnlSetParam.Visible = True

                    pnlSetParam.CssClass = "DisplayBlock"
                    pnlNote.CssClass = "DisplayBlock"
                    'pnlNote.Visible = True
                    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagEndParameterArrivedFromRevisionButton, False)
                    'RuleSelectValue(CInt(e.CommandArgument), HeightLowLimit)
                    'RuleSelectValue(False, CInt(e.CommandArgument), HeightLowLimitValue)
                    RuleSelectValue(True, CInt(e.CommandArgument), HeightLowLimittOsET)
                    CONTINUE_CONFIGURATOR()
                End If
            End If

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "DoHumborgerAB", "DoHumborger();", True)
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-dgvRulles_RowCommand-" & i_counter)

    End Sub

    Protected Sub dgvRulles_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgvRulles.RowDataBound
        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-dgvRulles_RowDataBound-" & i_counter)

        Try


            If e.Row.RowType = DataControlRowType.DataRow Then

                If e.Row.Cells(GridValuesIndex.RullNotation).Text.Trim <> "" Then
                    CType(e.Row.Cells(GridValuesIndex.RullNotationImage).FindControl("imgRullNotation"), HtmlImage).Src = "~\media\Images\Default\" & e.Row.Cells(GridValuesIndex.RullNotation).Text & ".svg"
                Else
                    CType(e.Row.Cells(GridValuesIndex.RullNotationImage).FindControl("imgRullNotation"), HtmlImage).Src = ""

                End If

                CType(e.Row.Cells(GridValuesIndex.lblRemarks).Controls(1), Label).Text = Server.HtmlDecode(e.Row.Cells(GridValuesIndex.Remarks).Text).Trim

                Dim txtToDisplay As String = e.Row.Cells(GridValuesIndex.OperationH).Text.Trim
                If txtToDisplay <> "=" And txtToDisplay <> "-" Then
                    CType(e.Row.Cells(GridValuesIndex.DetailsBtn).Controls(1), Button).Text = Server.HtmlDecode(txtToDisplay) & " " & Server.HtmlDecode(e.Row.Cells(GridValuesIndex.HeightLimitH).Text)
                    'CType(e.Row.Cells(GridValuesIndex.DetailsBtn).Controls(1), Button).Text = Server.HtmlDecode(txtToDisplay) & " " & clsLanguage.GetLanguageLabels(Server.HtmlDecode(e.Row.Cells(GridValuesIndex.HeightLimitH).Text).ToString.Trim)
                Else
                    CType(e.Row.Cells(GridValuesIndex.DetailsBtn).Controls(1), Button).Text = Server.HtmlDecode(e.Row.Cells(GridValuesIndex.HeightLimitH).Text)
                    'CType(e.Row.Cells(GridValuesIndex.DetailsBtn).Controls(1), Button).Text = clsLanguage.GetLanguageLabels(Server.HtmlDecode(e.Row.Cells(GridValuesIndex.HeightLimitH).Text).ToString.Trim)
                End If

                CType(e.Row.Cells(GridValuesIndex.DetailsBtn).Controls(1), Button).Text &= " " & Server.HtmlDecode(e.Row.Cells(GridValuesIndex.Remarks).Text).Trim
                'CType(e.Row.Cells(GridValuesIndex.DetailsBtn).Controls(1), Button).Text &= " " & clsLanguage.GetLanguageLabels(Server.HtmlDecode(e.Row.Cells(GridValuesIndex.Remarks).Text).ToString.Trim)

                CType(e.Row.Cells(GridValuesIndex.DetailsBtn).Controls(1), Button).Text = clsLanguage.GetLanguageLabels(CType(e.Row.Cells(GridValuesIndex.DetailsBtn).Controls(1), Button).Text.ToString.Trim)


                CType(e.Row.Cells(GridValuesIndex.MinBtn).Controls(1), Button).Text = Server.HtmlDecode(e.Row.Cells(GridValuesIndex.LowLimitH).Text)

                e.Row.Cells(GridValuesIndex.HeightLimitH).Text = e.Row.Cells(GridValuesIndex.HeightLimitH).Text

                If e.Row.Cells(GridValuesIndex.HeightLimitH).Text.Length > 40 Or colwidth = 100 Then
                    colwidth = 100
                    CType(e.Row.Cells(GridValuesIndex.DetailsBtn).Controls(1), Button).Width = "400"
                End If

                Try

                    If e.Row.Cells(GridValuesIndex.LowLimitH).Text.Trim.ToUpper.Replace("&NBSP;", "") <> "" AndAlso e.Row.Cells(GridValuesIndex.HeightLimitH).Text.Trim.ToUpper.Replace("&NBSP;", "") <> "" Then

                        Dim H As String = e.Row.Cells(GridValuesIndex.HeightLimit).Text '+ 0.000000000001
                        Dim L As String = e.Row.Cells(GridValuesIndex.LowLimit).Text

                        '--------------SLIDER'--------------
                        Dim DecimalFormat As String = "0"
                        Dim DecimalFormatS() As String
                        Try
                            Dim dtParameters As DataTable = clsModel.GetModelParameters(clsQuatation.ACTIVE_ModelNumber, True)
                            Dim sD As String = dtParameters.Rows(CurrentParameterIndex - 1).Item("FormatFormula").ToString
                            If Not IsNumeric(sD) Then
                                Dim oFormula As New FormulaResult(sD, clsQuatation.GetActiveQuotation_DTparams, 0, Nothing)
                                If oFormula.ToString.Trim <> "" Then
                                    oFormula.Quantity = Nothing
                                    sD = oFormula.ParseAndCalculate
                                End If
                            End If

                            If sD <> "" AndAlso IsNumeric(sD) Then
                                If sD.Contains(",") Then
                                    DecimalFormatS = sD.Split(",")
                                Else
                                    DecimalFormatS = sD.Split(".")
                                End If

                                If DecimalFormatS.Length > 0 Then
                                    DecimalFormat = DecimalFormatS(1).Replace("""", "").Length
                                End If
                            End If

                        Catch ex As Exception
                            DecimalFormat = "0"
                        End Try
                        If DecimalFormat = "" Then
                            DecimalFormat = "0"
                        End If


                        '--------------SLIDER NEW'--------------
                        Dim nD As Decimal = 1
                        'Dim nD As Decimal = ((H - L) * (10 ^ se1.Decimals) + 1)

                        Try
                            If Not IsNumeric(DecimalFormat) Then
                                nD = DecimalFormat.Length
                            End If
                        Catch ex As Exception
                            nD = 0
                        End Try

                        Select Case DecimalFormat
                            Case "1" : nD = "0.1"
                            Case "2" : nD = "0.01"
                            Case "3" : nD = "0.001"
                            Case "4" : nD = "0.0001"
                            Case "5" : nD = "0.0001"
                            Case "6" : nD = "0.0001"
                        End Select

                        'se1.BoundControlID = "txtValue"


                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "sliders(" & L & "," & L & "," & H & "," & nD & ");", True)
                        txtSliderStepVal.Text = nD
                        txtSliderMaxVal.Text = H '- 0.000000000001
                        txtSliderMinVal.Text = L

                        '--------------SLIDER NEW'--------------

                        SetRangeDefailtValue = True

                        Try
                            Set_Txt_Slider_Value(L, L)
                        Catch ex As Exception
                        End Try
                        btnMinValueRange.Text = LocalizationManager.CulturingNumber(e.Row.Cells(GridValuesIndex.LowLimit).Text, False)
                        btnMaxValueRange.Text = LocalizationManager.CulturingNumber(e.Row.Cells(GridValuesIndex.HeightLimit).Text, False)
                        dgvRulles.CssClass = "DisplayNone"
                    End If


                Catch ex As Exception

                End Try
            End If

        Catch ex As Threading.ThreadAbortException
            'no action
        Catch ex As GeneralException
            GeneralException.BuildError(Page, ex)
        Catch ex As Exception
            GeneralException.BuildError(Page, ex)
            Dim _ex As New GeneralException(String.Empty, ex)
        End Try

    End Sub

    Private Function IsDuplicate(ByVal dt As DataTable, ByVal dr As DataRow) As Boolean
        Try
            If dt.Rows.Count > 0 Then
                If CInt(dt.Rows(dt.Rows.Count - 1)("TabIndex")) >= CInt(dr("TabIndex")) Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Function


    Private Function GetParameterStaticResult(ParamISO As String, dtP As DataTable, CurrentParameterI As String, dvDummy As DataTable, resOrder As String) As String

        Try

            Dim dtCatParams As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamsFromCatalog, "")

            If ParamISO <> "" Then
                'Dim dtCatParamsAllData As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamsFromCatalogAllParametersData, "")
                Dim dtCatParamsAllData As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamsFromCatalog, "")

                For Each r As DataRow In dtCatParamsAllData.Rows
                    If r("GPNUM_ISO").ToString.Trim = ParamISO.ToString.Trim Then
                        If r.Item("Val").ToString.Trim <> "" Then
                            Return r.Item("Val").ToString.Trim
                        End If
                    End If
                Next
            End If


            Dim dtRulesFilter As String = "LabelNum=" & CurrentParameterIndex

            Dim sStart As String = clsQuatation.ACTIVE_OpenType.ToString
            Dim dtRules As DataTable
            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                dtRules = clsModel.GetModelRules(_ModelNumConfiguration)
            Else
                dtRules = clsModel.GetModelRules(_ModelNumModification)
            End If


            Dim drRules() As DataRow = dtRules.Select(dtRulesFilter)

            If drRules.Length > 0 Then
                For Each dr As DataRow In drRules
                    'For Each c As DataColumn In dtCatParams.Columns
                    For Each c As DataRow In dtCatParams.Rows
                        Dim ss As String = dr.Item("HeightLimit").ToString
                        If ss = c.Item("GIPRGP_ISO").ToString.Trim Then ' c.ColumnName Then
                            Return ss
                        End If
                    Next
                Next
            End If

            Dim itemNumber As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber)

            Dim family_No As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.familyNo, False)

            Dim s As String = GetExceptionDataItemValue(ParamISO, itemNumber, family_No)

            If s <> "" Then
                Return s
            End If

            Dim d As DataTable
            If clsQuatation.ACTIVE_OpenType.ToString = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then ' returnCurentOpenType() 
                d = _dtParamListModification
            Else
                d = _dtParamListConfiguration

            End If
            If dtP.Rows(CurrentParameterIndex - 1)("Formula").ToString <> "" AndAlso Not dtP.Rows(CurrentParameterIndex - 1)("Formula").ToString.Contains("N/A") Then
                Return ComputeFormula(dtP.Rows(CurrentParameterIndex - 1)("Formula").ToString, _dtParamListModification, CurrentParameterI, dvDummy, resOrder).ToString
            End If
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)

        End Try
        Return ""

    End Function


    Private Function GetExceptionDataItemValue(ParamIso As String, itemNumber As String, ModelId As Integer) As String

        Try

            If ParamIso.ToString.ToUpper = "GRADE" Then
                Dim sGetDataItem As String = ""

                Dim bFileUploadDocMngPath As String = ConfigurationManager.AppSettings("UserCATALOG_WSLocaly").ToString.Trim
                If bFileUploadDocMngPath = "True" Then

                    Dim ws As New wsCATIscarDataLocal.IscarData
                    ws.Timeout = 180000
                    sGetDataItem = ws.GetItemGrade(itemNumber)
                    Return sGetDataItem
                    ws = Nothing
                Else

                    Dim ws As New wsCATIscarData.IscarData
                    ws.Timeout = 180000
                    sGetDataItem = ws.GetItemGrade(itemNumber)
                    Return sGetDataItem
                    ws = Nothing
                End If


            ElseIf ParamIso.ToString.ToUpper.Contains("REFERENCE") Then
                Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber, False)

            ElseIf ParamIso.ToString.ToUpper.Contains("REF.") Then

                Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber, False)


            ElseIf ParamIso.ToString.ToUpper.Contains("WORKPIECE MATERIAL") Then

                Dim dt As DataTable = clsQuatation.GETFamilylCuttingCondiotionValue(ModelId)
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    Return dt.Rows(0).Item("Workpiece Material").ToString.Trim
                End If
            ElseIf ParamIso.ToString.ToUpper.Contains("APPLICATION TYPE") Then

                Dim dt As DataTable = clsQuatation.GETFamilylCuttingCondiotionValue(ModelId)
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    Return dt.Rows(0).Item("Application Type").ToString.Trim
                End If
            End If

            Return ""

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try

        Return ""

    End Function


    Private Sub txtValue_DataBinding(sender As Object, e As EventArgs) Handles txtValue.DataBinding
        Try

            txtValue.Attributes.Add("onkeydown", "EnterParamValue('" & txtValue.ClientID & "', '" & btnSelectParam.ClientID & "');")
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub CONTINUE_CONFIGURATOR()
        Try
            If clsQuatation.ACTIVE_OpenType.ToString = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then 'returnCurentOpenType() 
                Dim dtParameters As DataTable = clsModel.GetModelParameters(_ModelNumModification, True)
                ContinueRecursiveMODIF(dtParameters) ', dtCatParams
            ElseIf clsQuatation.ACTIVE_OpenType.ToString = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then 'returnCurentOpenType()
                Dim TempdtParamsChangable As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._TempdtParamsChangable, "")
                If Not TempdtParamsChangable Is Nothing AndAlso TempdtParamsChangable.Rows.Count > 0 AndAlso CurrentParameterIndexEND = False Then
                    'Dim dtParametersLabels As DataTable = clsModel.GetParamsArrayByVal(_ModelNumConfiguration)
                    'Dim dtparamsC As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamsFomCatalog, "")
                    ContinueRecursiveCONFIG(TempdtParamsChangable, TempdtParamsChangable)
                End If


            End If


        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub
    Private Sub btnMinValueRange_Click(sender As Object, e As EventArgs) Handles btnMinValueRange.Click
        Try
            'txtValue.Text = btnMinValueRange.Text
            txtValue0.Text = LocalizationManager.UnCulturingNumber(btnMinValueRange.Text)
            'Set_Txt_Slider_Value(btnMinValueRange.Text, "")
            'Set_Param(False)
            'CONTINUE_CONFIGURATOR()

            If RunSetParam(False, True, False) = True Then
                CONTINUE_CONFIGURATOR()
            End If

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "DoHumborgeAC", "DoHumborger();", True)
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub btnMaxValueRange_Click(sender As Object, e As EventArgs) Handles btnMaxValueRange.Click
        Try
            'txtValue.Text = btnMaxValueRange.Text
            txtValue0.Text = LocalizationManager.UnCulturingNumber(btnMaxValueRange.Text)

            'Set_Txt_Slider_Value(btnMaxValueRange.Text, "")
            'Set_Param(False)
            'CONTINUE_CONFIGURATOR()

            If RunSetParam(False, True, False) = True Then
                CONTINUE_CONFIGURATOR()
            End If

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "DoHumborgerAD", "DoHumborger();", True)
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub ContinueRecursiveMODIF(dtParameters As DataTable)
        Try

            Dim strStartEnd As String = "START"

            Do Until strStartEnd = "END"

                If clsQuatation.ACTIVE_OpenType.ToString = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                    If SearchParameterAndSetValueIfExistMODIF(strStartEnd, dtParameters, clsQuatation.Enum_QuotationOpenType.MODIFICATION) = False Then
                        Exit Do
                    End If
                End If
            Loop

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub

    Private Sub ContinueRecursiveCONFIG(dtParameters As DataTable, dtCatParams As DataTable)

        Try
            Dim strStartEnd As String = "START"

            Do Until strStartEnd = "END"

                If SearchParameterAndSetValueIfExistCONFIG(strStartEnd, dtParameters, dtCatParams, dtCatParams, clsQuatation.Enum_QuotationOpenType.CONFIGURATOR) = False Then
                    Exit Do
                End If
            Loop

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub

    Private Function SearchParameterAndSetValueIfExistMODIF(ByRef strStartEnd As String, dtParameters As DataTable, QuotatinOpenType As String) As Boolean

        Try
            Dim dtCatParams As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamsFromCatalog, "")
            Dim dtCatParamsAllData As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamsFromCatalog, "")

            Dim FoundParam As Boolean = False
            Dim ParamName As String = dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString
            Dim ParamISO As String = dtParameters.Rows(CurrentParameterIndex - 1)("GPNUM_ISO").ToString


            If ParamName <> "REFERENCE" Then
                Dim ss As String = GetCurrentValFromParams(ParamName)
                If ss <> "" Then
                    Set_Txt_Slider_Value(ss, "")
                    If RunSetParam(True, False, False) = True Then
                        strStartEnd = dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString
                        Return True
                    Else
                        Return False
                    End If
                End If

            End If



            If ParamISO <> "" Then
                If Not dtCatParamsAllData Is Nothing Then
                    For Each r As DataRow In dtCatParamsAllData.Rows
                        If (r("GPNUM_ISO").ToString = ParamISO.ToString) AndAlso r("Val").ToString.Trim <> "-" Then
                            Set_Txt_Slider_Value(r.Item("Val").ToString.Trim, "")
                            ' Set_Txt_Slider_Value(GetCurrentValFromCatalog(r).ToString.Trim, "")
                            If RunSetParam(True, False, False) = True Then
                                strStartEnd = dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString
                                Return True
                            Else
                                Return False
                            End If
                        End If
                    Next
                End If

            End If

            If FoundParam = False Then
                Dim dtRulesFilter As String = "LabelNum=" & CurrentParameterIndex
                Dim dtRules As DataTable
                If QuotatinOpenType = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                    dtRules = clsModel.GetModelRules(_ModelNumModification)
                Else
                    dtRules = clsModel.GetModelRules(_ModelNumConfiguration)
                End If

                Dim drRules() As DataRow = dtRules.Select(dtRulesFilter)

                If drRules.Length > 0 Then
                    For Each dr As DataRow In drRules
                        Dim ssH As String = dr.Item("HeightLimit").ToString
                        Dim ssHH As String = dr.Item("HeightLimitH").ToString
                        If QuotatinOpenType = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                            ssH = ComputeFormula(ssH, _dtParamListModification, CurrentParameterIndex, Nothing, "")
                        Else
                            ssH = ComputeFormula(ssH, _dtParamListConfiguration, CurrentParameterIndex, Nothing, "")
                        End If
                        If Not dtCatParams Is Nothing Then
                            For Each c As DataRow In dtCatParams.Rows
                                If ssH = c.Item("GIPRGP_ISO").ToString.Trim Or ssH = c.Item("GPDSC_ISO").ToString.Trim Then ' c.ColumnName Then
                                    Set_Txt_Slider_Value(ssHH, "")
                                    If RunSetParam(True, False, False) = True Then
                                        strStartEnd = dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString
                                        Return True
                                    Else
                                        Return False
                                    End If
                                End If

                                If c.Item("GPDSC_REG").ToString.ToUpper.Trim = dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString.ToUpper.Trim Then ' c.ColumnName Then
                                    Set_Txt_Slider_Value(ssHH, "")
                                    If RunSetParam(True, False, False) = True Then
                                        strStartEnd = dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString
                                        Return True
                                    Else
                                        Return False
                                    End If
                                End If
                            Next
                        End If

                    Next
                End If

            End If

            Dim itemNumber As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber)

            If ParamName.ToUpper.Trim = "REFERENCE" Then

                Dim dd As Integer = 0
                'ToString do .... check
            ElseIf ParamName.ToString.ToUpper = "GRADE" Then

                Dim sGetDataItem As String = ""

                'txtValue.Text = ws.GetItemGrade(itemNumber)

                Dim bFileUploadDocMngPath As String = ConfigurationManager.AppSettings("UserCATALOG_WSLocaly").ToString.Trim
                If bFileUploadDocMngPath = "TRUE" Then
                    Dim ws As New wsCATIscarDataLocal.IscarData
                    ws.Timeout = 180000
                    Set_Txt_Slider_Value(ws.GetItemGrade(itemNumber), "")
                    ws = Nothing
                Else
                    Dim ws As New wsCATIscarData.IscarData
                    ws.Timeout = 180000
                    Set_Txt_Slider_Value(ws.GetItemGrade(itemNumber), "")
                    ws = Nothing
                End If

                If RunSetParam(True, False, False) = True Then
                    strStartEnd = dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString
                    Return True
                Else
                    Return False
                End If

            ElseIf ParamName.ToString.ToUpper.Contains("REF.") Then
                Set_Txt_Slider_Value(itemNumber, "")
                If RunSetParam(True, False, False) = True Then
                    strStartEnd = dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString
                    Return True
                Else
                    Return False
                End If
            End If

            If ParamName.ToString.ToUpper.Contains("WORKPIECE MATERIAL") Then
                Dim family_No As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.familyNo, False)
                Set_Txt_Slider_Value(GetExceptionDataItemValue(ParamName.ToString.ToUpper, itemNumber, family_No), "")
                If RunSetParam(True, False, False) = True Then
                    Set_Param(True, "", False, False)
                    strStartEnd = dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString
                    Return True
                Else
                    Return False
                End If
            End If

            If ParamName.ToString.ToUpper.Contains("APPLICATION TYPE") Then
                Dim family_No As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.familyNo, False)
                Set_Txt_Slider_Value(GetExceptionDataItemValue(ParamName.ToString.ToUpper, itemNumber, family_No), "")
                If RunSetParam(True, False, False) = True Then
                    strStartEnd = dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString
                    Return True
                Else
                    Return False
                End If
            End If

            '--------------------if no valur found serch default value in rulles and if exist choose it and continue------------------------------
            Dim dtRulesFilterR As String = "LabelNum=" & CurrentParameterIndex
            Dim dtRulesR As DataTable
            If QuotatinOpenType = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                dtRulesR = clsModel.GetModelRules(_ModelNumModification)
            Else
                dtRulesR = clsModel.GetModelRules(_ModelNumConfiguration)
            End If
            Dim drRulesR() As DataRow = dtRulesR.Select(dtRulesFilterR)
            If drRulesR.Length > 0 Then
                For Each dr As DataRow In drRulesR
                    Dim ss As String = dr.Item("RullNotation").ToString
                    If ss.Contains("DEFAULT") Then
                        Set_Txt_Slider_Value(dr.Item("HeightLimit").ToString, "")
                        If RunSetParam(True, False, False) = True Then
                            strStartEnd = dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString
                            Return True
                        Else
                            Return False
                        End If
                    End If
                Next
            End If



            '--------------------------------------------------

            If FoundParam = False Then
                Set_Txt_Slider_Value("", "")
            End If

            strStartEnd = dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString ' _dtParamList.Rows(_dtParamList.Rows.Count - 1).Item("Label").ToString.ToUpper()

            Return False
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Function


    Private Function SearchParameterAndSetValueIfExistCONFIG(ByRef strStartEnd As String, dtParameters As DataTable, dtCatParamsAllData As DataTable, dtCatParams As DataTable, QuotatinOpenType As String) As Boolean

        Try
            Dim FoundParam As Boolean = False
            If CurrentParameterIndex <= dtParameters.Rows.Count Then
                _ShowMsg = False
                Dim ParamName As String = dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString
                Dim ParamISO As String = dtParameters.Rows(CurrentParameterIndex - 1)("GPNUM_ISO").ToString


                For Each r As DataRow In dtCatParamsAllData.Rows
                    If ParamName = r("Label").ToString Then 'Or (r("GPNUM_ISO").ToString = ParamISO.ToString AndAlso r("Measure").ToString.Trim <> "-")
                        Set_Txt_Slider_Value(r.Item("Measure").ToString, "")
                        If RunSetParam(True, False, False) = True Then
                            If CurrentParameterIndex <= dtParameters.Rows.Count Then
                                strStartEnd = dtParameters.Rows(CurrentParameterIndex - 1)("Label").ToString
                                Return True
                            Else
                                Return False
                            End If

                        Else
                            Return False
                        End If
                    End If
                Next

            End If


            Return False
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Function


    Private Sub btnQuotationList_Click(sender As Object, e As EventArgs) 'Handles btnQuotationList.Click
        Try

            Dim sBc As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)

            Try

                Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
                Response.Redirect("QuotationsList.aspx" & uniqueID & "iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("iqlang")), False)
            Catch ex As Exception
                ' Response.Redirect("QuotationsList.aspx" & UniqueID, False)
            End Try

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub



    Private Function PrepareBitmap(PicturePath As String) As Byte()
        Try
            Dim PictureName As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.mainPicL)

            Dim oQuotation As New clsQuatation
            Dim bitmapBytes As Byte()
            Try

                Dim bmp As Bitmap = New Bitmap(New Bitmap(PicturePath))
                Using stream As New System.IO.MemoryStream
                    'bmp.Save(stream, bmp.RawFormat)
                    bmp.Save(stream, Imaging.ImageFormat.Gif)
                    bitmapBytes = stream.ToArray
                    Return bitmapBytes
                End Using
                bmp.Dispose()
                bmp = Nothing
                oQuotation = Nothing
            Catch ex As Exception
                'Dim bmp As Bitmap = oQuotation.PrepareBitmapForNewQuotation(PicturePath, HttpContext.Current.Server.MapPath("~"), "Start.jpg", CurrentParameterIndex)
                'Using stream As New System.IO.MemoryStream
                '    'bmp.Save(stream, bmp.RawFormat)
                '    bmp.Save(stream, Imaging.ImageFormat.Jpeg)
                '    bitmapBytes = stream.ToArray
                '    Return bitmapBytes

                'End Using
                'bmp.Dispose()
                'bmp = Nothing
            End Try
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Function



    Private Sub StartAuthenticationProcess()

        Try

            Dim QuotaionNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber).ToString

            Dim bc As String = clsBranch.ReturnActiveBranchCodeForDocuments

            Dim clsQuotation As New clsUpdateData
            Dim bmpL As Bitmap = Nothing
            Dim bmpR As Bitmap = Nothing

            Try
                bmpL = New Bitmap(HttpContext.Current.Server.MapPath("~") & ImgMainImageL.ImageUrl.Substring(1))
                'bmpL = LoadImage(ImgMainImageL.ImageUrl)
            Catch ex As Exception
            End Try
            Try
                bmpR = New Bitmap(HttpContext.Current.Server.MapPath("~") & ImgMainImageR.ImageUrl.Substring(1))
            Catch ex As Exception
            End Try

            Dim qn As String
            Dim sMsgN As String = ""
            If QuotaionNo = "" Or QuotaionNo = "0" Then
                QuotaionNo = clsQuotation.InsertNewQuotation(clsQuatation.ACTIVE_OpenType.ToString, ConfigurationManager.AppSettings("ImageNonItemNoL"), 0, bTemporarilyQuotation, bmpL, bmpR).ToString

                Dim NewQuatationNum As String = ""

                If Not clsQuatation.IsTemporary_Quotatiom Then
                    Dim ssq As Boolean
                    Dim CanUpdateGal As Boolean = True

                    qn = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
                    Dim qnAS As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number, False)

                    ssq = clsUpdateData.UpdateQuotationGAL(qn, qnAS, clsBranch.ReturnActiveBranchCodeForDocuments, " StartAuthenticationProcess ", sMsgN, NewQuatationNum, CanUpdateGal)
                    If CanUpdateGal = True Then
                        If Not ssq Then
                            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.AS400_ERROR.ToString, "StartAuthenticationProcess", "First Fail Update quotation AS400 e1 Error msg : "" ## " & sMsgN, clsBranch.ReturnActiveBranchCodeForDocuments, qn, NewQuatationNum, clsQuatation.ACTIVE_UseLoggedEmail.ToString)
                            ssq = clsUpdateData.UpdateQuotationGAL(qn, qnAS, clsBranch.ReturnActiveBranchCodeForDocuments, " StartAuthenticationProcess ", sMsgN, NewQuatationNum, CanUpdateGal)
                        End If
                    End If
                    If CanUpdateGal = True Then
                        If ssq = False Then
                            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.AS400_ERROR.ToString, "StartAuthenticationProcess", "Second Fail Update quotation AS400 e1 Error msg : "" ## " & sMsgN, clsBranch.ReturnActiveBranchCodeForDocuments, qn, NewQuatationNum, clsQuatation.ACTIVE_UseLoggedEmail.ToString)
                        End If
                    End If


                End If

                'If ssq = False Then
                '    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.AS400_ERROR.ToString, "StartAuthenticationProcess", "Second Fail Update quotation AS400 e1 Error msg : "" ## " & sMsgN, clsBranch.ReturnActiveBranchCodeForDocuments, qn, NewQuatationNum, clsQuatation.ACTIVE_UseLoggedEmail.ToString)
                'Else
                Try
                    clsUpdateData.UpdateQuotationUploadFilesServiceResult(bc, QuotaionNo, "")
                Catch ex As Exception
                End Try

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempAllReadyTriedtoBuildDrawingAutomaticly, "YES1")

                If ConfigurationManager.AppSettings("RunBSONWiththread") = "YES" Then
                    Dim cclsThreadsBSON As New clsThreadsBSON
                    cclsThreadsBSON.DoThreadBSON()
                    cclsThreadsBSON = Nothing
                End If

                If ConfigurationManager.AppSettings("RunPDF2DWiththread") = "YES" Then
                    clsUpdateData.UpdateQuotationSuccess("ALL", False, False, False, clsQuatation.ACTIVE_QuotationNumber, clsBranch.ReturnActiveBranchCodeForDocuments)
                    Dim sclsth_Rep As New clsThreads
                    sclsth_Rep.DoThreadReport(False)
                    sclsth_Rep = Nothing
                End If
                'End If



            Else

                Dim sFlagParametersChanged As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagParametersChanged, False)
                If sFlagParametersChanged.ToString.ToUpper = "TRUE" Then
                    clsQuotation.UpdateQuotation(clsQuatation.ACTIVE_OpenType.ToString, CInt(QuotaionNo), ConfigurationManager.AppSettings("ImageNonItemNoL"), 0, bmpL, bmpR)
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagPricesChanged, False)
                    Try
                        clsUpdateData.UpdateQuotationUploadFilesServiceResult(bc, QuotaionNo, "")
                    Catch ex As Exception

                    End Try

                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempAllReadyTriedtoBuildDrawingAutomaticly, "YES1")

                    If ConfigurationManager.AppSettings("RunBSONWiththread") = "YES" Then
                        Dim cclsThreadsBSON As New clsThreadsBSON
                        cclsThreadsBSON.DoThreadBSON()
                        cclsThreadsBSON = Nothing
                    End If
                    If ConfigurationManager.AppSettings("RunPDF2DWiththread") = "YES" Then
                        clsUpdateData.UpdateQuotationSuccess("ALL", False, False, False, clsQuatation.ACTIVE_QuotationNumber, clsBranch.ReturnActiveBranchCodeForDocuments)
                        Dim sclsth_Rep As New clsThreads
                        sclsth_Rep.DoThreadReport(True)
                        sclsth_Rep = Nothing
                    End If

                End If

            End If

            clsQuotation = Nothing

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub


    Public Function LoadImage(ur As Byte) As Image
        Try


            Dim image As Image

            Using ms As MemoryStream = New MemoryStream(ur)
                image = Image.FromStream(ms)
            End Using

            Return image
        Catch ex As Exception

        End Try
    End Function

#Region "PRICES"

    Private Sub GetItemCat()

        Try


            Dim sStart As String = clsQuatation.ACTIVE_OpenType.ToString 'returnCurentOpenType()

            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then

                Dim CatNo As String = ""
                Dim FamilyNum As String = ""


                Dim ModelNo As Integer = 0

                ModelNo = clsQuatation.ACTIVE_ModelNumber ' returnCurentModelNumber()

                Dim branchcode As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode).ToString
                Dim DC_P As String = "{DC}"
                Dim DC_V As String = lblDC.Text.ToString.Trim

                Dim sStr As String = clsQuatation.SetSTNTEMPLabel()
                Dim sStrGetFamily As String = clsQuatation.GetFamily(sStr)

                If sStrGetFamily <> "" Then
                    Dim dtc As DataTable = clsReference.GetCatalogNumber(branchcode, 0, ModelNo, DC_P, DC_V, sStr)
                    If Not dtc Is Nothing AndAlso dtc.Rows.Count > 0 Then
                        CatNo = dtc.Rows(0).Item("ItemRefrence")
                        FamilyNum = dtc.Rows(0).Item("FamilyNum")

                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.itemNumber, CatNo.ToString.Trim)
                        setParamValueInDT_Modi_Conf("GetFamily", FamilyNum)
                    End If
                Else
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.itemNumber, "")
                End If

            ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                setParamValueInDT_Modi_Conf("GetFamily", SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumModification, False))
            End If


        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub



    Private Sub Get_DC()

        Try
            _DC = lblDC.Text.ToString.Trim

            If _DC = "" Then

                lblDC.Text = clsPrice.Get_DC(clsQuatation.GetActiveQuotation_DTparams) 'ReturnCurentDTparams

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DC, lblDC.Text)

            End If
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-Get_DC-" & i_counter)

    End Sub


    'Private Sub SetDefaultValue()
    '    Try

    '        If SetRangeDefailtValue = True Then
    '            Try
    '                Dim dt As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._TempdtParamsSelected, "")

    '                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
    '                    If CurrentParameterIndex - 1 < dt.Rows.Count Then

    '                        If IsNumeric(dt.Rows(CurrentParameterIndex - 1).Item("Measure")) Then

    '                            Set_Txt_Slider_Value(dt.Rows(CurrentParameterIndex - 1).Item("Measure"), dt.Rows(CurrentParameterIndex - 1).Item("Measure"))

    '                        End If
    '                    Else
    '                        Set_Txt_Slider_Value("", "")
    '                        SetRangeDefailtValue = False
    '                    End If

    '                End If
    '            Catch ex As Exception
    '                'txtValue.Text = ""
    '                Set_Txt_Slider_Value("", "")
    '                SetRangeDefailtValue = False
    '            End Try

    '        End If


    '        SetRangeDefailtValue = False

    '    Catch ex As Exception
    '        SetRangeDefailtValue = False

    '    End Try
    '    SetRangeDefailtValue = False
    'End Sub
    Private Sub SetDefaultValue()
        Try

            If SetRangeDefailtValue = True Then
                Try
                    Dim dt As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._TempdtParamsSelected, "")

                    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                        If IsNumeric(dt.Rows(CurrentParameterIndex - 1).Item("Measure")) Then

                            Set_Txt_Slider_Value(dt.Rows(CurrentParameterIndex - 1).Item("Measure"), dt.Rows(CurrentParameterIndex - 1).Item("Measure"))

                        End If
                    End If
                Catch ex As Exception
                    'txtValue.Text = ""
                    Set_Txt_Slider_Value("", "")
                    SetRangeDefailtValue = False
                End Try

            End If


            SetRangeDefailtValue = False

        Catch ex As Exception
            SetRangeDefailtValue = False

        End Try
        SetRangeDefailtValue = False
    End Sub

#End Region



#Region "ListViewParams"


    Private Property Persons() As DataTable
        Get
            Return If(ViewState("Persons") IsNot Nothing, DirectCast(ViewState("Persons"), DataTable), Nothing)
        End Get
        Set(value As DataTable)
            ViewState("Persons") = value
        End Set
    End Property

    Private Sub BindListView(d As DataTable)
        Try


            Dim dP As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParametersListView, "")


            For Each listvieR As DataRow In dP.Rows
                listvieR.Item("Measure") = ""
                listvieR.Item("PrevParam") = ""
                For Each ParamsR As DataRow In d.Rows
                    If listvieR.Item("TabIndex").ToString = ParamsR.Item("TabIndex").ToString Then
                        listvieR.Item("Measure") = ParamsR.Item("Measure").ToString ' "- " &
                        listvieR.Item("PrevParam") = ParamsR.Item("PrevParam").ToString
                    End If
                Next
            Next



            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParametersListView, dP)
            lvParams.DataSource = dP
            lvParams.DataBind()

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub



    Private Sub SelectParamFromList(Param As String)
        Try
            Dim sSelectTedParamOLD As String = ""

            isEditMode = True

            lblDC.Text = ""

            SetCurrentParamIndex("", CInt(Param)) ' Param + 2
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PrevParameterSelected, CInt(CurrentParameterIndex) - 1)

            SetParameter(False)

            FillImages()

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub


    Private Sub StartOver()
        Try
            SetCurrentParamIndex("", 1)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CurrentParameterIndex, 1)
            isEditMode = True

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.itemNumber, "", False)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListConfiguration, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListModification, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListConfigurationLive, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListModificationLive, CType(Nothing, DataTable))

            _dtParamListConfiguration = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListConfiguration, "")
            _dtParamListModification = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListModification, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempdtParamsChangable, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyBulidFormPostBack, False)
            AllRedyBulidFormPostBack = False
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempdtParamsSelected, CType(Nothing, DataTable))

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempdtParamsSelected, _dt)
            If clsQuatation.ACTIVE_OpenType.ToString = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then 'returnCurentOpenType()

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Temp_ClearAllForModification, "TRUE")
            Else
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamsFromCatalog, CType(Nothing, DataTable))
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtModParamsFinalSelected, CType(Nothing, DataTable))


            End If
            SetStartedVariables(True)

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub


    Private Sub SaveLogin(Authentication As Boolean)
        Try


            If Authentication = False Then
                StartAuthenticationProcess()
                SetLiveParams(True)
                RedirectPrices()
            Else

                Dim l As New LogIn
                l.LogIn(HttpContext.Current.Request.UrlReferrer.AbsoluteUri.ToString)
                l.Dispose()
                l = Nothing
            End If



        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub
    Private Sub RedirectPrices()
        Try
            Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
            Try
                Response.Redirect("Prices.aspx" & uniqueID & "iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), False)
            Catch ex As Exception
                Response.Redirect("Prices.aspx" & "QuotationsList.aspx" & uniqueID, False)
            End Try


        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub DiscardProces()
        Try

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagPricesChanged, False)
            RedirectPrices()

        Catch ex As Exception

        End Try
    End Sub



#End Region


    Private Sub btnClearAll_Click(sender As Object, e As EventArgs) Handles btnClearAll.Click
        Try


            StartOver()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnSaveQuotation_Click(sender As Object, e As EventArgs) Handles btnSaveQuotation.Click
        Try

            'If btnSaveQuotation.Text = "Get New Price" Or btnSaveQuotation.Text = "Get Price" Then
            If btnSaveQuotation.Text = _SaveQuotation_1 Or btnSaveQuotation.Text = _SaveQuotation_2 Or btnSaveQuotation.Text = "Get New Price" Or btnSaveQuotation.Text = "Get Price" Then
                If btnSaveQuotation.Text = _SaveQuotation_1 Or btnSaveQuotation.Text = "Get New Price" Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempDontGetPrice, "FALSE")
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Price, CType(Nothing, DataTable))
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyReportBuild, "NO")

                End If

                SessionManager.SetSessionDetails_Temporarily("FALSE")

                bTemporarilyQuotation = False
                SaveLogin(False)
            Else
                SaveLogin(True)
            End If


        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub btnLogOnNORegistration_Click(sender As Object, e As EventArgs) Handles btnLogOnNORegistration.Click
        Try
            SaveLogin(False)
        Catch ex As Exception

        End Try
    End Sub


    Protected Sub txtValue_TextChanged(sender As Object, e As EventArgs)
        Try


            txtValue0.Text = LocalizationManager.UnCulturingNumber(txtValue.Text)
        Catch ex As Exception

        End Try
    End Sub



    Private Sub BtnSubmitMessage_Click(sender As Object, e As EventArgs) Handles btnSubmitMessage.Click
        Try

            'IsNullOrWhiteSpace

            If txtMailEmail.Text <> "" Or txtMailName.Text <> "" Then
                Dim sMsg As String = ""
                sMsg &= "Name:  " & txtMailName.Text & "<br>"
                sMsg &= "Email Address: " & txtMailEmail.Text & "<br>"
                sMsg &= "Country: " & txtMailCountry.Text & "<br>"
                sMsg &= "Company Name: " & txtMailCompanyName.Text & "<br>"
                sMsg &= "Message:" & vbNewLine & txtMailMessage.Text & "<br>"

                clsMail.SendEmailWithoutAttachment(ConfigurationManager.AppSettings("MailFrom").ToString.Trim, "SemiStandatrd Message", sMsg, True, True, Server.MapPath("~"), False, Nothing, "", "", "")

            End If

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub



    Private Sub lvParams_ItemCommand(sender As Object, e As ListViewCommandEventArgs) Handles lvParams.ItemCommand
        Try
            If e.CommandArgument = "btnParamArray" Then

                Try

                    If IsNumeric(CType(lvParams.Items(e.Item.DisplayIndex).FindControl("lblPrevParam"), Label).Text) Then
                        Dim iP As String = CType(lvParams.Items(e.Item.DisplayIndex).FindControl("lblTabIndex"), Label).Text
                        lblForCheck.Text = iP
                        SelectParamFromList(iP)

                        Set_Txt_Slider_Value(CType(e.Item.FindControl("lblmeasure"), Label).Text, CType(e.Item.FindControl("lblmeasure"), Label).Text)
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "DoHumborgerAZ", "DoHumborger();", True)
                    End If



                Catch ex As Exception

                End Try

            End If
        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub
    Private Sub lvParams_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles lvParams.ItemDataBound


        Try
            If (e.Item.ItemType = ListViewItemType.DataItem) Then
                Dim PrevParam As String = "0"
                Dim TabIndex As String = "0"
                Try
                    PrevParam = CType(e.Item.FindControl("lblPrevParam"), Label).Text
                    TabIndex = CType(e.Item.FindControl("lbltabindex"), Label).Text
                Catch ex As Exception
                End Try
                Try
                    CType(e.Item.FindControl("btnParamArray"), Button).Text = clsLanguage.GetLanguageLabels(CType(e.Item.FindControl("btnParamArray"), Button).Text.Trim)
                Catch ex As Exception

                End Try

                If IsNumeric(TabIndex) AndAlso TabIndex = CurrentParameterIndex Then 'Selected Param

                    If Not IsNumeric(CType(e.Item.FindControl("lblPrevParam"), Label).Text) Then
                        CType(e.Item.FindControl("btnParamArray"), Button).Enabled = False
                    End If

                    If IsNumeric(CType(e.Item.FindControl("lblMeasure"), Label).Text) Then
                        CType(e.Item.FindControl("lblMeasure"), Label).Text = LocalizationManager.CulturingNumber((CType(e.Item.FindControl("lblMeasure"), Label).Text), False)
                        CType(e.Item.FindControl("lblMeasureCaption"), Label).Text = clsLanguage.GetLanguageLabels(LocalizationManager.CulturingNumber((CType(e.Item.FindControl("lblMeasure"), Label).Text), False))
                    End If

                    CType(e.Item.FindControl("btnParamArray"), Button).CssClass = "ListViewLinkButtonN FontFamilyRoboto FontSizeRoboto13"
                    CType(e.Item.FindControl("lblMeasure"), Label).CssClass = "ListViewMeasureN FontFamilyRoboto FontSizeRoboto13"
                    CType(e.Item.FindControl("lblMeasureCaption"), Label).CssClass = "ListViewMeasureN FontFamilyRoboto FontSizeRoboto13"

                    CType(e.Item.FindControl("btnParamArray"), Button).Enabled = False

                    CType(e.Item.FindControl("pnlX"), Panel).BackColor = Color.FromName("#b5c4da")

                ElseIf IsNumeric(TabIndex) AndAlso TabIndex > CurrentParameterIndex Then
                    CType(e.Item.FindControl("btnParamArray"), Button).Enabled = False

                    CType(e.Item.FindControl("btnParamArray"), Button).CssClass = "ListViewLinkButtonNDis FontFamilyRoboto FontSizeRoboto13"
                    If IsNumeric(CType(e.Item.FindControl("lblMeasure"), Label).Text) Then
                        CType(e.Item.FindControl("lblMeasure"), Label).Text = LocalizationManager.CulturingNumber((CType(e.Item.FindControl("lblMeasure"), Label).Text), False)
                        CType(e.Item.FindControl("lblMeasureCaption"), Label).Text = clsLanguage.GetLanguageLabels(LocalizationManager.CulturingNumber((CType(e.Item.FindControl("lblMeasure"), Label).Text), False))
                    End If
                    CType(e.Item.FindControl("lblMeasure"), Label).CssClass = "ListViewMeasureNDis FontFamilyRoboto FontSizeRoboto13"
                    CType(e.Item.FindControl("lblMeasureCaption"), Label).CssClass = "ListViewMeasureNDis FontFamilyRoboto FontSizeRoboto13"

                    CType(e.Item.FindControl("btnParamArray"), Button).ForeColor = System.Drawing.ColorTranslator.FromHtml("#8f8f8f")

                Else

                    If IsNumeric(CType(e.Item.FindControl("lblMeasure"), Label).Text) Then
                        CType(e.Item.FindControl("lblMeasure"), Label).Text = LocalizationManager.CulturingNumber((CType(e.Item.FindControl("lblMeasure"), Label).Text), False)
                        CType(e.Item.FindControl("lblMeasureCaption"), Label).Text = clsLanguage.GetLanguageLabels(LocalizationManager.CulturingNumber((CType(e.Item.FindControl("lblMeasure"), Label).Text), False))
                    End If

                    If IsNumeric(CType(e.Item.FindControl("lblPrevParam"), Label).Text) Then
                        CType(e.Item.FindControl("btnParamArray"), Button).CssClass = "ListViewLinkButtonN FontFamilyRoboto FontSizeRoboto13"
                        CType(e.Item.FindControl("lblMeasure"), Label).CssClass = "ListViewMeasureN FontFamilyRoboto FontSizeRoboto13"
                        CType(e.Item.FindControl("lblMeasureCaption"), Label).CssClass = "ListViewMeasureN FontFamilyRoboto FontSizeRoboto13"
                    Else
                        CType(e.Item.FindControl("btnParamArray"), Button).CssClass = "ListViewLinkButtonNDis FontFamilyRoboto FontSizeRoboto13"
                        CType(e.Item.FindControl("lblMeasure"), Label).CssClass = "ListViewMeasureNDis FontFamilyRoboto FontSizeRoboto13"
                        CType(e.Item.FindControl("lblMeasureCaption"), Label).CssClass = "ListViewMeasureNDis FontFamilyRoboto FontSizeRoboto13"
                        CType(e.Item.FindControl("btnParamArray"), Button).ForeColor = System.Drawing.ColorTranslator.FromHtml("#8f8f8f")
                        CType(e.Item.FindControl("btnParamArray"), Button).Enabled = False

                        If CType(e.Item.FindControl("lblMeasure"), Label).Text = "" Then
                            e.Item.Visible = False
                        End If

                    End If
                End If



                Try
                    CType(e.Item.FindControl("lblMeasureCaption"), Label).Text = clsLanguage.GetLanguageLabels(CType(e.Item.FindControl("lblMeasure"), Label).Text)
                    CType(e.Item.FindControl("lblMeasureCaption"), Label).CssClass = CType(e.Item.FindControl("lblMeasure"), Label).CssClass
                    CType(e.Item.FindControl("lblMeasure"), Label).CssClass = "DisplayNone"
                Catch ex As Exception

                End Try

                If CType(e.Item.FindControl("lblVisibleTable"), Label).Text.ToString.ToUpper = "TRUE" Then
                    Dim sAd As String = StateManager.GetValue(StateManager.Keys.s_IsAdmin, True)
                    If sAd = "YES" Then
                        CType(e.Item.FindControl("btnParamArray"), Button).Style.Add("color", "red")
                    Else
                        e.Item.Visible = False
                    End If
                End If

            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub btnSubmitMessagePricesBuild_Click(sender As Object, e As EventArgs) Handles btnSubmitMessagePricesBuild.Click
        Try

            Dim sMsg As String = "There is no option to show prices. Please Contact iQuote Support."

            Dim sQutNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
            Dim sCn As String = StateManager.GetValue(StateManager.Keys.s_CustomerName, True)
            Dim sCno As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
            Dim sCompN As String = StateManager.GetValue(StateManager.Keys.s_CompanyName, True)
            Dim sBc As String = StateManager.GetValue(StateManager.Keys.s_BranchCode, True)
            Dim sLe As String = StateManager.GetValue(StateManager.Keys.s_loggedEmail, True)

            sMsg = sMsg & "<br><br>"
            sMsg = sMsg & "QuotationNumber - " & sQutNo & " - <br>"
            sMsg = sMsg & "BranchCode - " & sBc & " - <br>"
            sMsg = sMsg & "CompanyName - " & sCompN & " - <br>"
            sMsg = sMsg & "CustomerNumber - " & sCno & " - <br>"
            sMsg = sMsg & "CustomerName - " & sCn & " - <br>"
            sMsg = sMsg & "Logged Email - " & sLe & " - <br>"


            clsMail.SendEmailWithoutAttachment(ConfigurationManager.AppSettings("MailFrom").ToString.Trim, "iQuote Oops Message", sMsg, True, True, Server.MapPath("~"), False, Nothing, "", "", "")

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertNbre", "SendMailRef();", True)

        Catch ex As Exception
            'GeneralException.WriteEventErrors(ex.Message, GeneralException.e_LogTitle.PARAMETERS.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "QBUILDER btnSubmitMessagePricesBuild_Click", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub btnDiscardChanges_Click(sender As Object, e As EventArgs) Handles btnDiscardChanges.Click

        Try
            Dim qut As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
            If IsNumeric(qut) Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagPricesChanged, False)
                Dim BranchCode As String = clsBranch.ReturnActiveBranchCodeForDocuments
                SessionManager.SetSessionDetails_SEMI_ExistQuoatation(BranchCode, qut, clsQuatation.e_QuotationStatus.Exist_Qut_OpenedFromParameters, False)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ToEnableParamsTab, "TRUE")
                SetParamsFromLive()
                RedirectPrices()
            End If
        Catch ex As Exception
        End Try

    End Sub

    Private Sub QBuilder_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            Try
                If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._tempBackFromImcLOGIN, False) = "START" Then

                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._tempBackFromImcLOGIN, "DONE")

                    Response.Redirect(Request.RawUrl)

                End If
            Catch ex As Exception
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._tempBackFromImcLOGIN, "DONE")
            End Try

            If SecurityManager.CheckIfUserNotAuthinticatedwithTheQuote("") = False Then
                Response.Redirect("~/Default.aspx", False)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub SetCaptionsLanguage()
        'Try
        '    Dim dt As DataTable = clsLanguage.Get_LanguageCaption(clsBranch.ReturnActiveBranchCodeState)
        '    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
        '        Dim dv As DataView = dt.DefaultView
        '        dv.RowFilter = "ControlLocation = 'Build'"
        '        Dim currentEnum As String = ""
        '        For i As Integer = 0 To dv.Count - 1
        '            currentEnum = dv.Item(i).Item("LanguageEnumCode")
        '            Select Case currentEnum
        '                Case clsLanguage.e_LanguageConstants.ClearAll : btnClearAll.Text = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
        '            End Select
        '        Next
        '    End If

        'Catch ex As Exception

        'End Try

        Try
            Dim dv As DataView = clsLanguage.Get_LanguageCaption("Build")
            If Not dv Is Nothing AndAlso dv.Count > 0 Then
                Dim currentEnum As String = ""
                For i As Integer = 0 To dv.Count - 1
                    currentEnum = dv.Item(i).Item("LanguageEnumCode")
                    Select Case currentEnum
                        Case clsLanguage.e_LanguageConstants.ClearAll
                            btnClearAll.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.ParametersOverview
                            lblParamsDes.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.SetCation
                            SetCation = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            lblCurrnetParameterName.Text = lblCurrnetParameterName.Text.Replace("Set ", SetCation & " ")
                        Case clsLanguage.e_LanguageConstants.RelatedImages
                            Label1.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblTitle1
                            _lblTitle1Cap1 = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblTitle2
                            _lblTitle1Cap2 = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblTitle3
                            _lblTitle1Cap3 = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblLGI1
                            _lblLGI_1 = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblLGI2
                            _lblLGI_2 = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblLGI3
                            _lblLGI_3 = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblLGI4
                            _lblLGI_4 = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblLGI5
                            _lblLGI_5 = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblLGI6
                            _lblLGI_6 = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.SaveQuotation_1
                            _SaveQuotation_1 = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.SaveQuotation_2
                            _SaveQuotation_2 = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.SaveQuotation_3
                            _SaveQuotation_3 = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.btnLogOnNORegistration
                            btnLogOnNORegistration.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.DiscarChenges
                            btnDiscardChanges.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            'Case clsLanguage.e_LanguageConstants.SaveQuotation_0
                            '    btnSaveQuotation.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)




                    End Select
                Next
            End If

        Catch ex As Exception

        End Try
    End Sub
End Class