Imports System.Drawing
Imports SemiApp_bl

Public Class Material
    Inherits System.Web.UI.Page

    Private _Start As String

    Private SetMaterialCation As String = "Set"
    Private _languageID As Int16 = 1
    Private _DoFill As Boolean = False
    Private Enum e_GridMAterial As Integer
        SelectComande = 0
        Group = 1
        id = 2
        Description = 3
        condition = 4
        Hardness = 5
        TabDescription = 6
    End Enum
    Private Function returnCurentOpenType() As String
        Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)

    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Try

        '    Dim loggedEmail As String = CookiesManager.GetStringValue(CryptoManagerTDES.Encode("UserEmailForCurentQut"))
        '    Dim BranchCode As String = CookiesManager.GetStringValue(CryptoManagerTDES.Encode("BranchCodeForCurentQut"))

        '    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "loggedEmail", loggedEmail, clsBranch.ReturnActiveBranchCodeState, "", "", clsQuatation.ACTIVE_UseLoggedEmail.ToString)

        'Catch ex As Exception

        'End Try
        '--------------------SELECTED FLAG/LANGUAGE--------------
        Dim selectedBC As String = "ZZ"
        If clsLanguage.CheckIfLanguageSelected(CType(Master.FindControl("hfLanguageselected"), HiddenField).Value, selectedBC) = True Then
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetCaptionForLabelsD", "SetCaptionForLabels()", True)
            _DoFill = True
            SelectedColor.Text = "blue"
            SelectedForColor.Text = "Color.White"
        End If
        '--------------------------------------------------------

        Try
            Try
                _languageID = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.qut_LanguageId, False)

            Catch ex As Exception

            End Try
            If Not IsPostBack OrElse _DoFill = True Then
                SessionManager.Clear_Sessions_ForBeginSendMessages()
                SetTabs()
            End If

            Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"

            If Idinty.CheckSesstionTimeOut = True Then
                If ConfigurationManager.AppSettings("IsDebugMode").ToString.ToUpper = "TRUE" Then
                    Response.Redirect("http://localhost:60377/Default.aspx?STARTFB=STARTFB_N", True)
                ElseIf ConfigurationManager.AppSettings("IsDebugMode").ToString.ToUpper = "TEST" Then
                    Response.Redirect("http://dmstest/iQuote/Default.aspx?STARTFB=STARTFB_N", True)
                Else
                    Response.Redirect("https://iquote.ssl.imc-companies.com/iQuote/Default.aspx?STARTFB=STARTFB_N", True)
                End If


            Else
                _Start = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)
                FillMainMaterial()
            End If

            Try
                _languageID = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.qut_LanguageId, False)
            Catch ex As Exception

            End Try


        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "Material", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

        End Try


    End Sub


    Private Sub SetTabs()

        wucTabs.tcModel = False
        wucTabs.tcMatirial = True
        wucTabs.tcParameters = False
        wucTabs.tcGetQuotation = False
        wucTabs.SelectedItem = wucTab.E_MNUiTEMS.Matirial


        wucTabs.ItemsVisiblty()
    End Sub


    Private Sub FillMainMaterial()

        Try
            If Not IsPostBack OrElse _DoFill = True Then

                dlMaterialMain.DataSource = clsMaterial.GetMainMaterial(_languageID)
                dlMaterialMain.DataBind()
                dlMaterialMain1.DataSource = clsMaterial.GetMainMaterial(_languageID)
                dlMaterialMain1.DataBind()

                fillGVMaterials("")

            End If
        Catch ex As Exception
            GeneralException.BuildError(Page, ex)
        End Try
    End Sub
    Private Sub btmOenQbuilder_Click(sender As Object, e As EventArgs) Handles btmOenQbuilder.Click

        Try
            Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"

            Server.Transfer("QBuilder.aspx" & uniqueID & "iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), True)
        Catch ex As Exception
            Response.Redirect("QBuilder.aspx" & UniqueID, False)
        End Try


    End Sub

    Private Sub dlMaterialMain_ItemDataBound(sender As Object, e As DataListItemEventArgs) Handles dlMaterialMain.ItemDataBound
        Try

            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

                Dim sColor As String = CType(e.Item.DataItem(), DataRowView)("CategoryColor").ToString
                Dim sForColor As String = CType(e.Item.DataItem(), DataRowView)("ForColor").ToString

                CType(e.Item.FindControl("btnModelIcon"), Button).BackColor = System.Drawing.ColorTranslator.FromHtml(sColor)
                CType(e.Item.FindControl("btnModelIcon"), Button).ForeColor = Color.FromName(sForColor)
                CType(e.Item.FindControl("btnModelIcon"), Button).Text = CType(e.Item.DataItem(), DataRowView)("Category").ToString


                CType(e.Item.FindControl("lblMaterialName"), Label).Text = CType(e.Item.DataItem(), DataRowView)("CategoryName").ToString.Trim
                CType(e.Item.FindControl("lblGroupName"), Label).Text = CType(e.Item.DataItem(), DataRowView)("GroupName").ToString.Trim
                CType(e.Item.FindControl("btnModelIconBackColor"), Label).Text = sColor

                CType(e.Item.FindControl("btnModelIcon"), Button).Attributes.Add("onblur", "this.style.borderWidth='0'")
                CType(e.Item.FindControl("btnModelIcon"), Button).Attributes.Add("onmouseover", "this.style.cursor='hand'")

                CType(e.Item.FindControl("pnlSelected"), Panel).Attributes.Add("onclick", "return ModelSelect('" & CType(e.Item.FindControl("btnModelIcon"), Button).ClientID & "');")

            End If
        Catch ex As Exception
            GeneralException.BuildError(Page, ex)
        End Try
    End Sub

    Private Sub dlMaterialMain_ItemCommand(source As Object, e As DataListCommandEventArgs) Handles dlMaterialMain.ItemCommand
        Try

            SelectedColor.Text = CType(e.Item.FindControl("btnModelIconBackColor"), Label).Text '  CType(e.Item.FindControl("btnModelIcon"), Button).BackColor.Name ' "blue" ' CType(e.Item.Controls(2).Controls(1), Button).BackColor.Name
            SelectedForColor.Text = CType(e.Item.FindControl("btnModelIcon"), Button).ForeColor.Name ' "blue" ' CType(e.Item.Controls(2).Controls(1), Button).BackColor.Name

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMainMaterial, CType(e.Item.FindControl("lblMaterialName"), Label).Text)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMainMaterialCategory, CType(e.Item.FindControl("btnModelIcon"), Button).Text)

            e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#b5c4da") 'Color.FromName("#b5c4da")


            ' fillGVMaterials(CType(e.Item.FindControl("lblMaterialName"), Label).Text)
            fillGVMaterials(CType(e.Item.FindControl("lblGroupName"), Label).Text)

        Catch ex As Exception
            ' GeneralException.WriteEventErrors(ex.Message, GeneralException.e_LogTitle.MATERIAL.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "dlMaterialMain_ItemCommand", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex)
        End Try
    End Sub
    Private Sub fillGVMaterials(lblGroupName As String)
        Try
            '  GeneralException.WriteEvent(0)
            If lblGroupName.ToString <> "" Then
                '  GeneralException.WriteEvent(1)

                gvMaterials.DataSource = clsMaterial.GetMaterial(lblGroupName, _languageID)
                gvMaterials.DataBind()

            Else
                '  GeneralException.WriteEvent(2)

                Dim s1 As String = CType(dlMaterialMain.DataSource, DataTable).Rows(0).Item("categoryname") ' "Steel" 'CType(dlMaterialMain.Items(0).Controls(2).Controls(3), Label).Text
                Dim s2 As String = CType(dlMaterialMain.DataSource, DataTable).Rows(0).Item("category") ' "P" 'CType(dlMaterialMain.Items(0).FindControl("btnModelIcon"), Button).Text
                Dim s3 As String = CType(dlMaterialMain.DataSource, DataTable).Rows(0).Item("GroupName") ' "P" 'CType(dlMaterialMain.Items(0).FindControl("btnModelIcon"), Button).Text

                'GeneralException.WriteEvent("s1=" & s1)
                'GeneralException.WriteEvent("s2=" & s2)
                gvMaterials.DataSource = clsMaterial.GetMaterial(s3, _languageID)
                gvMaterials.DataBind()

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMainMaterial, s1)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMainMaterialCategory, s2)

                'dlMaterialMain.Items(0).BackColor = Color.FromName("#b5c4da")
                dlMaterialMain.Items(0).BackColor = System.Drawing.ColorTranslator.FromHtml("#b5c4da")
                dlMaterialMain1.Items(0).BackColor = System.Drawing.ColorTranslator.FromHtml("#b5c4da")
            End If


            '  GeneralException.WriteEvent(gvMaterials.Rows.Count)
        Catch ex As Exception
            'GeneralException.WriteEventErrors(ex.Message, GeneralException.e_LogTitle.MATERIAL.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "MATERIAL Get_ParamsGridVal", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex)
        End Try
        Try
            _languageID = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.qut_LanguageId, False)
            SetCaptionsLanguage()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub gvMaterials_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvMaterials.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim sColor As String = SelectedColor.Text
                Dim sForColor As String = SelectedForColor.Text
                CType(e.Row.FindControl("pnlSquareColor"), Panel).BackColor = System.Drawing.ColorTranslator.FromHtml(sColor)
                CType(e.Row.FindControl("pnlSquareColor"), Panel).ForeColor = Color.FromName(sForColor)
                CType(e.Row.FindControl("lblGroup"), Label).Text = e.Row.Cells(e_GridMAterial.id).Text
                e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" & " this.style.backgroundColor='#ebedf2';")
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;")

                e.Row.Attributes.Add("onclick", "return SubModelSelect('" & CType(e.Row.FindControl("ButtonSelect"), Button).ClientID & "');")

            End If
        Catch ex As Exception
            GeneralException.BuildError(Page, ex)
        End Try
    End Sub

    Private Sub gvMaterials_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvMaterials.RowCommand

        Try


            If e.CommandName = "SelectMaterial" Then

                SessionManager.ClearSessionAfterChangewMaterial()

                Dim s As String = gvMaterials.Rows(e.CommandArgument).Cells(e_GridMAterial.Description).Text
                Dim sT As String = gvMaterials.Rows(e.CommandArgument).Cells(e_GridMAterial.TabDescription).Text
                Dim sTiD As String = gvMaterials.Rows(e.CommandArgument).Cells(e_GridMAterial.id).Text

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMaterial, s)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMaterialTabDescription, sT)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.MaterialsID, sTiD)

                Dim sStart As String = returnCurentOpenType()
                If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                    ReFillDtParams(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempMainMaterial, False), s)

                End If
                Dim _Quotation_Status As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationStatus, False)
                If _Quotation_Status = clsQuatation.e_QuotationStatus.Exist_QutOpenedFromQuotationList Then 'Or  _Quotation_Status = clsQuatation.e_QuotationStatus.Exist_Qut_OpenedFromParameters clsQuatation.e_QuotationStatus.EXIST_QUOTATION_MODIF Or _QuotationStatus = clsQuatation.e_QuotationStatus.EXIST_QUOTATION_CONFOG Then

                    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CurrentParameterIndex, 1)
                Else
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CurrentParameterIndex, 1)

                End If

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyBulidFormPostBack, "False")

                Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"


                Try

                    Server.Transfer("QBuilder.aspx" & uniqueID & "iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), True)
                Catch ex As Exception
                    Response.Redirect("QBuilder.aspx" & uniqueID, False)
                End Try
            End If
        Catch ex As Exception
            GeneralException.BuildError(Page, ex)
        End Try


    End Sub

    Private Sub ReFillDtParams(sTempMaterial As String, sTempMaterialTabDescription As String)
        Try

            'Dim TempdtParamsChangable As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._TempdtParamsChangable, "")
            Dim TempdtParamsChangable As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListConfiguration, "")

            If Not TempdtParamsChangable Is Nothing AndAlso TempdtParamsChangable.Rows.Count > 0 Then
                For Each r As DataRow In TempdtParamsChangable.Rows
                    If r.Item("Label").ToString.ToUpper = "WORKPIECE MATERIAL" Then
                        r.Item("Measure") = sTempMaterial

                    End If
                    If r.Item("Label").ToString.ToUpper = "SELECT MATERIAL" Then
                        r.Item("Measure") = sTempMaterialTabDescription
                    End If
                Next
                'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempdtParamsChangable, TempdtParamsChangable)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListConfiguration, TempdtParamsChangable)

            End If
        Catch ex As Exception
            GeneralException.BuildError(Page, ex)
        End Try
    End Sub

    Private Sub dlMaterialMain1_ItemCommand(source As Object, e As DataListCommandEventArgs) Handles dlMaterialMain1.ItemCommand
        Try

            SelectedColor.Text = CType(e.Item.FindControl("btnModelIconBackColor1"), Label).Text
            SelectedForColor.Text = CType(e.Item.FindControl("btnModelIcon1"), Button).ForeColor.Name

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMainMaterial, CType(e.Item.FindControl("lblMaterialName1"), Label).Text)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMainMaterialCategory, CType(e.Item.FindControl("btnModelIcon1"), Button).Text)

            e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#b5c4da")

            ' fillGVMaterials(CType(e.Item.FindControl("lblMaterialName1"), Label).Text)
            fillGVMaterials(CType(e.Item.FindControl("lblGroupName1"), Label).Text)

        Catch ex As Exception
            'GeneralException.WriteEventErrors(ex.Message, GeneralException.e_LogTitle.MATERIAL.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "dlMaterialMain1_ItemCommand", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex)
        End Try
    End Sub

    Private Sub dlMaterialMain1_ItemDataBound(sender As Object, e As DataListItemEventArgs) Handles dlMaterialMain1.ItemDataBound
        Try

            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

                Dim sColor As String = CType(e.Item.DataItem(), DataRowView)("CategoryColor").ToString
                Dim sForColor As String = CType(e.Item.DataItem(), DataRowView)("ForColor").ToString

                CType(e.Item.FindControl("btnModelIcon1"), Button).BackColor = System.Drawing.ColorTranslator.FromHtml(sColor)
                CType(e.Item.FindControl("btnModelIcon1"), Button).ForeColor = Color.FromName(sForColor)
                CType(e.Item.FindControl("btnModelIcon1"), Button).Text = CType(e.Item.DataItem(), DataRowView)("Category").ToString


                CType(e.Item.FindControl("lblMaterialName1"), Label).Text = CType(e.Item.DataItem(), DataRowView)("CategoryName").ToString.Trim
                CType(e.Item.FindControl("lblGroupName1"), Label).Text = CType(e.Item.DataItem(), DataRowView)("GroupName").ToString.Trim
                CType(e.Item.FindControl("btnModelIconBackColor1"), Label).Text = sColor

                CType(e.Item.FindControl("btnModelIcon1"), Button).Attributes.Add("onblur", "this.style.borderWidth='0'")
                CType(e.Item.FindControl("btnModelIcon1"), Button).Attributes.Add("onmouseover", "this.style.cursor='hand'")

                CType(e.Item.FindControl("pnlSelected1"), Panel).Attributes.Add("onclick", "return ModelSelect('" & CType(e.Item.FindControl("btnModelIcon1"), Button).ClientID & "');")
                CType(e.Item.FindControl("pnlSelected1"), Panel).Attributes.Add("onclick", "return ModelSelect('" & CType(e.Item.FindControl("btnModelIcon1"), Button).ClientID & "');")

                'Console.Write(CType(e.Item.FindControl("btnModelIcon"), Button).ClientID)

            End If
        Catch ex As Exception
            GeneralException.BuildError(Page, ex)
        End Try
    End Sub

    Private Sub SetCaptionsLanguage()

        Try
            Dim dv As DataView = clsLanguage.Get_LanguageCaption("Material")
            If Not dv Is Nothing AndAlso dv.Count > 0 Then
                Dim currentEnum As String = ""
                For i As Integer = 0 To dv.Count - 1
                    currentEnum = dv.Item(i).Item("LanguageEnumCode")
                    Select Case currentEnum
                        Case clsLanguage.e_LanguageConstants.setMaterialCaption
                            SetMaterialCation = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            lblSetMat.Text = SetMaterialCation ' lblSetMat.Text.Replace("Set ", SetMaterialCation & " ")
                        Case clsLanguage.e_LanguageConstants.Group
                            gvMaterials.HeaderRow.Cells(e_GridMAterial.Group).Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.Description
                            gvMaterials.HeaderRow.Cells(e_GridMAterial.Description).Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.Condition
                            gvMaterials.HeaderRow.Cells(e_GridMAterial.condition).Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.Hardness
                            gvMaterials.HeaderRow.Cells(e_GridMAterial.Hardness).Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)

                    End Select
                Next
            End If

        Catch ex As Exception

        End Try
    End Sub
End Class