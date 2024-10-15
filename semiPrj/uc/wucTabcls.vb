Imports AjaxControlToolkit.HTMLEditor.ToolbarButton
Imports SemiApp_bl
Partial Class wucTab
    Inherits System.Web.UI.UserControl

    'Private Enum e_TabsItems As Integer
    '    ProductionType = 0
    '    Material = 1
    '    Parameters = 2
    '    GetQuotation = 3
    '    MyQuotations = 4
    'End Enum

    Private _tcModel As Boolean = False
    Private _tcMatirial As Boolean = False
    Private _tcParameters As Boolean = False
    Private _tcGetQuotation As Boolean = False

    Private _SelectedItem As Integer


    Public Enum E_MNUiTEMS
        Model = 0
        Matirial = 1
        Parameters = 2
        GetQuotation = 3
        MyQuotations = 4
    End Enum

    Public Property tcModel As Boolean
        Get
            Return _tcModel
        End Get
        Set(value As Boolean)
            _tcModel = value
        End Set
    End Property

    Public Property tcMatirial As Boolean
        Get
            Return _tcMatirial
        End Get
        Set(value As Boolean)
            _tcMatirial = value
        End Set
    End Property

    Public Property tcParameters As Boolean
        Get
            Return _tcParameters
        End Get
        Set(value As Boolean)
            _tcParameters = value
        End Set
    End Property

    Public Property tcGetQuotation As Boolean
        Get
            Return _tcGetQuotation
        End Get
        Set(value As Boolean)
            _tcGetQuotation = value
        End Set
    End Property

    Public Property SelectedItem As Integer
        Get
            Return _SelectedItem
        End Get
        Set(value As Integer)
            _SelectedItem = value
        End Set
    End Property

    Private Sub SetCaptionsLanguage()
        Try
            Dim dv As DataView = clsLanguage.Get_LanguageCaption("Tab")
            If Not dv Is Nothing AndAlso dv.Count > 0 Then
                Dim currentEnum As String = ""
                For i = 0 To dv.Count - 1
                    currentEnum = dv.Item(i).Item("LanguageEnumCode")
                    Select Case currentEnum

                        Case clsLanguage.e_LanguageConstants.Tab1 : lblProductType.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.Tab2 : lblMaterial.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.Tab3 : lblParameters.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.Tab4 : lblGetQuotation.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)

                    End Select
                Next

            End If


        Catch ex As Exception

        End Try
    End Sub


    Private Function returnCurentModel() As DataTable

        Try
            Dim sCon As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)
            Dim sMod As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)

            If sCon <> "" AndAlso IsNumeric(sCon) AndAlso CInt(sCon) = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR.GetHashCode Then
                Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelConfiguration, "")
            ElseIf sMod <> "" AndAlso IsNumeric(sMod) AndAlso CInt(sMod) = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelModification, "")
            End If

            Return CType(Nothing, DataTable)

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Function


    Private Sub SetLabelColor(Type As String, sLabelItem As Control, subLabelItem As Control)

        Dim disColor As String = "#545755"
        Dim enColor As String = "#1d5095"

        If Type = "ACTIVE" Then

            CType(sLabelItem, Label).Style.Add("color", enColor)
            CType(subLabelItem, Label).Style.Add("color", enColor)

        Else

            CType(sLabelItem, Label).Style.Add("color", disColor)
            CType(subLabelItem, Label).Style.Add("color", disColor)

        End If

    End Sub
    Public Sub ItemsVisiblty()
        Try

            Dim _Quotation_Status As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationStatus, False)

            Dim sOpenType As String = clsQuatation.ACTIVE_OpenType
            '--------------------------------------

            btnProductType.Style.Add("background-color", "#e9eaec")
            btnMaterial.Style.Add("background-color", "#e9eaec")
            btnParameters.Style.Add("background-color", "#e9eaec")
            btnGetQuotation.Style.Add("background-color", "#e9eaec")

            SetLabelColor("", lblProductType, lblProductTypeTEXT)
            SetLabelColor("", lblMaterial, lblMaterialTEXT)
            SetLabelColor("", lblParameters, lblParametersTEXT)
            SetLabelColor("", lblGetQuotation, lblGetQuotationTEXT)

            divModel.Style.Add("background-image", "../media/TabImage/icon_ProductType_dis.svg")
            divMaterial.Style.Add("background-image", "../media/TabImage/icon_Material_dis.svg")
            divParameters.Style.Add("background-image", "../media/TabImage/icon_parameters_dis.svg")
            divQuotations.Style.Add("background-image", "../media/TabImage/icon_Resaults_dis.svg")

            btnProductType.Disabled = True
            btnMaterial.Disabled = True
            btnParameters.Disabled = True

            Dim bSubmmitd As Boolean = True
            Dim sSubmitted As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Submitted, False)
            If sSubmitted = "0" Then
                bSubmmitd = False
            End If
            Try
                Dim sAllp As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ToEnableParamsTab, False)
                If sAllp = "TRUE" Then
                    btnParameters.Disabled = bSubmmitd ' False
                End If
            Catch ex As Exception

            End Try


            btnGetQuotation.Disabled = True

            'lblProductType.Text = "Product Type"
            'lblMaterial.Text = "Material"
            'lblParameters.Text = "Parameters"
            'lblGetQuotation.Text = "Quotation"
            '--------------------------------------

            Select Case SelectedItem
                Case E_MNUiTEMS.Model
                    'btnProductType.Style.Add("background-image", "../media/TabImage/Model_ON.png")
                    btnProductType.Style.Add("background-color", "white")
                    btnProductType.Style.Add("border-top", "4px solid #f1e32a")

                    SetLabelColor("ACTIVE", lblProductType, lblProductTypeTEXT)

                    divModel.Style.Add("background-image", "../media/TabImage/icon_ProductType_active.svg")


                Case E_MNUiTEMS.Matirial
                    'btnMaterial.Style.Add("background-image", "../media/TabImage/Material_ON.png")
                    btnMaterial.Style.Add("background-color", "white")
                    btnMaterial.Style.Add("border-top", "4px solid #f1e32a")

                    SetLabelColor("ACTIVE", lblProductType, lblProductTypeTEXT)
                    SetLabelColor("ACTIVE", lblMaterial, lblMaterialTEXT)

                    divModel.Style.Add("background-image", "../media/TabImage/icon_ProductType_active.svg")
                    divMaterial.Style.Add("background-image", "../media/TabImage/icon_Material_active.svg")

                    'btnProductType.Disabled = False

                    'btnProductType.Style.Add("cursor", "pointer")

                Case E_MNUiTEMS.Parameters

                    btnParameters.Style.Add("background-color", "white")
                    btnParameters.Style.Add("border-top", "4px solid #f1e32a")



                    If sOpenType = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                        SetLabelColor("ACTIVE", lblProductType, lblProductTypeTEXT)
                        SetLabelColor("ACTIVE", lblMaterial, lblMaterialTEXT)
                        divModel.Style.Add("background-image", "../media/TabImage/icon_ProductType_active.svg")
                        divMaterial.Style.Add("background-image", "../media/TabImage/icon_Material_active.svg")
                    End If


                    SetLabelColor("ACTIVE", lblParameters, lblParametersTEXT)


                    divParameters.Style.Add("background-image", "../media/TabImage/icon_parameters_active.svg")

                    'btnProductType.Disabled = False
                    btnMaterial.Disabled = False

                    'btnProductType.Style.Add("cursor", "pointer")
                    btnMaterial.Style.Add("cursor", "pointer")
                    If _Quotation_Status = clsQuatation.e_QuotationStatus.New_Qut Or _Quotation_Status = clsQuatation.e_QuotationStatus.Exist_Qut_OpenedFromParameters Then ' clsQuatation.e_QuotationStatus.NEW_QUOTATION_CONFOG Or clsQuatation.e_QuotationStatus.NEW_QUOTATION_MODIF Then
                        Try
                            Dim sQut As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
                            If sQut <> "" Then
                                btnGetQuotation.Disabled = False
                                btnGetQuotation.Style.Add("cursor", "pointer")
                            End If

                        Catch ex As Exception

                        End Try

                    End If
                Case E_MNUiTEMS.GetQuotation

                    btnGetQuotation.Style.Add("background-color", "white")
                    btnGetQuotation.Style.Add("border-top", "4px solid #f1e32a")
                    divQuotations.Style.Add("background-image", "../media/TabImage/icon_Resaults_active.svg")
                    SetLabelColor("ACTIVE", lblGetQuotation, lblGetQuotationTEXT)




                    btnMaterial.Disabled = True
                    btnMaterial.Style.Add("cursor", "default")
                    If _Quotation_Status = clsQuatation.e_QuotationStatus.New_Qut Or _Quotation_Status = clsQuatation.e_QuotationStatus.Exist_Qut_OpenedFromParameters Then ' clsQuatation.e_QuotationStatus.NEW_QUOTATION_CONFOG Or clsQuatation.e_QuotationStatus.NEW_QUOTATION_MODIF Then

                        If sOpenType = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                            divModel.Style.Add("background-image", "../media/TabImage/icon_ProductType_active.svg")
                            divMaterial.Style.Add("background-image", "../media/TabImage/icon_Material_active.svg")

                            SetLabelColor("ACTIVE", lblProductType, lblProductTypeTEXT)
                            SetLabelColor("ACTIVE", lblMaterial, lblMaterialTEXT)
                        End If






                        Dim gp As String = ""
                        Try
                            gp = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempDontGetPrice, False)
                        Catch ex As Exception
                            gp = "FALSE"
                        End Try

                        If gp.ToString.ToUpper <> "TRUE" Then
                            btnParameters.Disabled = bSubmmitd ' False
                            If bSubmmitd = False Then btnParameters.Style.Add("cursor", "pointer")
                            If bSubmmitd = False Then
                                SetLabelColor("ACTIVE", lblParameters, lblParametersTEXT)
                                divParameters.Style.Add("background-image", "../media/TabImage/icon_parameters_active.svg")


                            End If
                        End If

                    End If

                Case E_MNUiTEMS.MyQuotations

                    ''btnProductType.Style.Add("cursor", "pointer")

                    'btnProductType.Style.Add("background-image", "../media/TabImage/Model_ON.png")

                    'SetLabelColor("ACTIVE", lblProductType, lblProductTypeTEXT)

                    'divModel.Style.Add("background-image", "../media/TabImage/icon_ProductType_active.svg")

            End Select

            If _Quotation_Status = clsQuatation.e_QuotationStatus.Exist_QutOpenedFromQuotationList Then ' clsQuatation.e_QuotationStatus.EXIST_QUOTATION_MODIF Or _QuotationStatus = clsQuatation.e_QuotationStatus.EXIST_QUOTATION_MODIF Then

                btnParameters.Disabled = True
                btnGetQuotation.Disabled = True
                btnParameters.Style.Add("cursor", "default")
                btnGetQuotation.Style.Add("cursor", "default")
                SetLabelColor("", lblParameters, lblParametersTEXT)

            ElseIf _Quotation_Status = clsQuatation.e_QuotationStatus.Exist_Qut_OpenedFromParameters Then




                Dim gp As String = ""
                Try
                    gp = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempDontGetPrice, False)
                Catch ex As Exception
                    gp = "FALSE"
                End Try

                If gp.ToString.ToUpper <> "TRUE" Then
                    btnMaterial.Disabled = False
                End If








                btnMaterial.Style.Add("cursor", "pointer")

                'btnProductType.Disabled = True
                'btnMaterial.Style.Add("cursor", "default")
                'btnProductType.Style.Add("cursor", "default")
            End If

            If sOpenType = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                btnProductType.Disabled = False
                btnProductType.Style.Add("cursor", "pointer")
            Else
                btnProductType.Disabled = True
                btnProductType.Style.Add("cursor", "default")
                btnMaterial.Disabled = True
                btnMaterial.Style.Add("cursor", "default")
            End If



            Dim dt As DataTable = returnCurentModel()
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Dim sDec As String = dt.Rows(0).Item("ModelDes").ToString
                lblProductTypeTEXT.Text = sDec
            Else
                Dim _ModelNumConfiguration As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumConfiguration, False)
                If _ModelNumConfiguration <> "" Then
                    dt = clsModel.GetModelDetails(Integer.Parse(_ModelNumConfiguration))
                    Dim sDec As String = dt.Rows(0).Item("ModelDes").ToString
                    lblProductTypeTEXT.Text = sDec
                Else
                    lblProductTypeTEXT.Text = ""
                End If

            End If
            ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempWorkPieceMaterialValue) & "-" &
            lblMaterialTEXT.Text = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempMaterialTabDescription)
            'GeneralException.WriteEventLogInCounter("105")

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub


    Private Sub btnRunPT_Click(sender As Object, e As EventArgs) Handles btnRunPT.Click

        'GeneralException.WriteEventLogInCounter("106")

        SessionManager.Clear_ALL_SessionsTimeStamp()
        SessionManager.Clear_Sessions_For_NewStartedQuotation()

        Dim sY As String = ""

        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempConfigDefaultShow, "")


        Try
            Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
            Response.Redirect("ConfiguratorBuilder.aspx" & uniqueID & "iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), False)
        Catch ex As Exception
            'Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
            'Response.Redirect("ConfiguratorBuilder.aspx" & uniqueID, False)
        End Try
    End Sub

    Private Sub btnRunMT_Click(sender As Object, e As EventArgs) Handles btnRunMT.Click


        Try
            Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
            Response.Redirect("Material.aspx" & uniqueID & "iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), False)
        Catch ex As Exception
            'Response.Redirect("Material.aspx" & UniqueID, False)
        End Try
    End Sub

    Private Sub btnRunBL_Click(sender As Object, e As EventArgs) Handles btnRunBL.Click

        Try
            Dim sStart As String = clsQuatation.ACTIVE_OpenType.ToString


            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                Dim dt As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListConfigurationLive, "")
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListConfiguration, SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListConfigurationLive, ""))
                End If
            Else
                Dim dt As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListModification, "")
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListModification, SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListModificationLive, ""))
                End If
            End If

            Try
                Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"

                Response.Redirect("QBuilder.aspx" & uniqueID & "iqlang=" & utl.ReturnParamLanguage(Request("iqlang"), True) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), False)
            Catch ex As Exception
                'Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
                'Response.Redirect("QBuilder.aspx" & UniqueID, False)
            End Try

        Catch ex As Exception
            Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
            Response.Redirect("QBuilder.aspx" & UniqueID, False)
        End Try
    End Sub

    Private Sub btnGetQuotation_ServerClick(sender As Object, e As EventArgs) Handles btnGetQuotation.ServerClick
        Try
            'GeneralException.WriteEventLogInCounter("109")

            Dim qut As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
            If IsNumeric(qut) Then
                Dim BranchCode As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)
                Dim sFlagParametersChanged As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagParametersChanged, False)
                If sFlagParametersChanged.ToString.ToUpper = "TRUE" Then
                    SessionManager.SetSessionDetails_SEMI_ExistQuoatation(BranchCode, qut, clsQuatation.e_QuotationStatus.Exist_Qut_OpenedFromParameters, False)
                End If

                Dim displayTpe As String = "Prices.aspx"

                Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"


                Try

                    Response.Redirect(displayTpe & uniqueID & "iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), False)
                Catch ex As Exception
                    Response.Redirect(displayTpe & uniqueID, False)
                End Try

            End If
            'GeneralException.WriteEventLogInCounter("110")

        Catch ex As Exception

        End Try
    End Sub

    Private Sub wucTab_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            'If Not IsPostBack Then
            SetCaptionsLanguage()
            'End If
        Catch ex As Exception

        End Try
    End Sub
End Class