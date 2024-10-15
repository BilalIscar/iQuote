Imports System.Drawing
Imports System.Reflection
Imports SemiApp_bl
Public Class ConfiguratorBuilder
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            '--------------------SELECTED FLAG/LANGUAGE--------------
            Dim selectedBC As String = "ZZ"
            If clsLanguage.CheckIfLanguageSelected(CType(Master.FindControl("hfLanguageselected"), HiddenField).Value, selectedBC) = True Then
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetCaptionForLabelsD", "SetCaptionForLabels()", True)
            End If
            '--------------------------------------------------------


            SetCaptionsLanguageConFig()

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

                Try
                    If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempConfigDefaultShow, False) Is Nothing Then
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempConfigDefaultShow, "")
                    End If
                Catch ex As Exception
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempConfigDefaultShow, "")
                End Try

                If Not IsPostBack Then

                    SetTabs()

                    fillMainData()

                    Dim ss As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempConfigDefaultShow, False)
                    If ss <> "DONE" Then
                        If ConfigurationManager.AppSettings("IsConfigurationSelection").ToString = "TRUE" Then
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempConfigDefaultShow, "DONE")
                            SelectDefault()
                        End If
                    End If

                Else


                    If SelectedMainIdIndex.Text.ToString <> "" AndAlso IsNumeric(SelectedMainIdIndex.Text) Then
                        CType(lvMainApp.Items(SelectedMainIdIndex.Text).FindControl("imgModelIcon1"), ImageButton).BackColor = Color.FromName("#B5C4DA")
                        CType(lvMainApp.Items(SelectedMainIdIndex.Text).FindControl("divModelIcon1"), HtmlGenericControl).Style.Add("background-color", "#B5C4DA")
                        If SelectedSubIdIndex.Text.ToString <> "" AndAlso IsNumeric(SelectedSubIdIndex.Text) Then
                            CType(lvMainApp_Sub.Items(SelectedSubIdIndex.Text).FindControl("imgModelIcon2"), ImageButton).BackColor = Color.FromName("#B5C4DA")
                            CType(lvMainApp_Sub.Items(SelectedSubIdIndex.Text).FindControl("divModelIcon2"), HtmlGenericControl).Style.Add("background-color", "#B5C4DA")
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.CONFIGURATORBUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)
        End Try
    End Sub
    Private Sub SetTabs()
        wucTabs.tcModel = True
        wucTabs.tcMatirial = False
        wucTabs.tcParameters = False
        wucTabs.tcGetQuotation = False
        wucTabs.SelectedItem = wucTab.E_MNUiTEMS.Model
        wucTabs.ItemsVisiblty()
    End Sub

    Private Sub SetCaptionsLanguageConFig()
        Try
            Dim dv As DataView = clsLanguage.Get_LanguageCaption("Config")
            If Not dv Is Nothing AndAlso dv.Count > 0 Then
                Dim currentEnum As String = ""
                For i As Integer = 0 To dv.Count - 1
                    currentEnum = dv.Item(i).Item("LanguageEnumCode")
                    Select Case currentEnum

                        Case clsLanguage.e_LanguageConstants.ModelIcon_Groove_Turn
                            hfModelIcon_Groove_Turn.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.ModelIcon_ISO_Turning
                            hfModelIcon_ISO_Turning.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.ModelIcon_Threading
                            hfModelIcon_Threading.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.ModelIcon_Milling
                            hfModelIcon_Milling.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.ModelIcon_Hole_Making
                            hfModelIcon_Hole_Making.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.ModelIcon_Hole_Reaming
                            hfModelIcon_Hole_Reaming.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.Indexable
                            hfModelIcon_Indexable.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.Solid
                            hfModelIcon_Solid.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.SolidEndmilsSquare
                            hfModelIcon_SolidEndmilsSquare.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.SolidEndmilsBallNose
                            hfModelIcon_SolidEndmilsBallNose.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                    End Select
                Next
            End If

        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.CONFIGURATORBUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)
        End Try
    End Sub

    Private Sub fillMainData()
        Try
            Dim dt As DataTable = eCat.Getcat_MenuApplication
            lvMainApp.DataSource = dt
            lvMainApp.DataBind()
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.CONFIGURATORBUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)
        End Try


    End Sub

    Private Sub fillSubData_1()
        Try
            Dim dt As DataTable = eCat.Get_SubData_1(SelectedMainIdFamily.Text, lvMainApp.Items.Count)
            lvMainApp_Sub.DataSource = dt
            lvMainApp_Sub.DataBind()
            lvMainApp.Style.Add("display", "none")
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.CONFIGURATORBUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)
        End Try

    End Sub

    Private Sub fillSubData_2(MainDataMANUM As String, SubApp As String)
        Try
            Dim dtMLF As DataTable = clsModel.GetModelList_ModelsFormFamily(MainDataMANUM, "C", SubApp)
            lvMainApp_SubSub.DataSource = dtMLF
            lvMainApp_SubSub.DataBind()

        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.CONFIGURATORBUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)
        End Try


    End Sub

    Protected Sub imgFamiliesList_Click(sender As Object, e As ImageClickEventArgs)
        Try

            Dim bc As String = clsBranch.ReturnActiveBranchCodeSESSTION
            SessionManager.Clear_Sessions_For_NewStartedQuotation()

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.BranchCode, bc)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationOpenType, clsQuatation.Enum_QuotationOpenType.CONFIGURATOR)

            Server.Transfer("~/Forms/Material.aspx?iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")))

        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.CONFIGURATORBUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)
        End Try


    End Sub

    Private Sub SelectDefault()
        Try
            SelectedMainIdIndex.Text = 3
            SelectedSubIdIndex.Text = 1

            SelectedMainIdFamily.Text = "ML"
            CType(lvMainApp.Items(SelectedMainIdIndex.Text).FindControl("imgModelIcon1"), ImageButton).BackColor = Color.FromName("#B5C4DA")
            CType(lvMainApp.Items(SelectedMainIdIndex.Text).FindControl("divModelIcon1"), HtmlGenericControl).Style.Add("background-color", "#B5C4DA")

            fillSubData_1()

            Dim s1 As String = "Solid"
            Dim s2 As String = SelectedMainIdFamily.Text


            fillSubData_2(s2, s1)

            CType(lvMainApp_Sub.Items(SelectedSubIdIndex.Text).FindControl("imgModelIcon2"), ImageButton).BackColor = Color.FromName("#B5C4DA")
            CType(lvMainApp_Sub.Items(SelectedSubIdIndex.Text).FindControl("divModelIcon2"), HtmlGenericControl).Style.Add("background-color", "#B5C4DA")

        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.CONFIGURATORBUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)
        End Try
    End Sub


    Private Sub lvMainApp_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles lvMainApp.ItemDataBound
        Try
            Try
                If (e.Item.ItemType = ListViewItemType.DataItem) AndAlso Not e.Item.Controls Is Nothing AndAlso e.Item.Controls.Count > 0 Then
                    CType(e.Item.Controls(1).FindControl("btnModelname1"), Button).Text = CType(e.Item.DataItem(), DataRowView)("MANAMO").ToString.Trim
                    CType(e.Item.Controls(1).FindControl("lblFamilyCode"), Label).Text = CType(e.Item.DataItem(), DataRowView)("MANUM").ToString.Trim
                    CType(e.Item.Controls(1).FindControl("imgModelIcon1"), ImageButton).CommandArgument = e.Item.DataItemIndex

                    If CType(e.Item.DataItem(), DataRowView)("Active").ToString.ToUpper = "TRUE" Then
                        CType(e.Item.Controls(1).FindControl("imgModelIcon1"), ImageButton).CssClass = "img-fluid img-thumbnail ApplicationListItemDivMainF ModelIconButton"
                        CType(e.Item.Controls(1).FindControl("btnModelname1"), Button).Enabled = True
                        CType(e.Item.Controls(1).FindControl("btnModelname1"), Button).CssClass = "ModelCaptionButton"
                        CType(e.Item.Controls(1).FindControl("btnModelname1"), Button).Attributes.Add("loading", "lazy")
                    Else
                        CType(e.Item.Controls(1).FindControl("imgModelIcon1"), ImageButton).CssClass = "img-fluid img-thumbnail imgModelIconFilterF ModelIconButton"
                        CType(e.Item.Controls(1).FindControl("btnModelname1"), Button).Enabled = False
                        CType(e.Item.Controls(1).FindControl("btnModelname1"), Button).CssClass = "ModelCaptionButtonfilter"
                        CType(e.Item.Controls(1).FindControl("btnModelname1"), Button).Attributes.Add("loading", "lazy")
                    End If
                End If
            Catch ex As Exception
                GeneralException.BuildError(Page, ex)
            End Try
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.CONFIGURATORBUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)
        End Try
    End Sub

    Private Sub lvMainApp_ItemCommand(sender As Object, e As ListViewCommandEventArgs) Handles lvMainApp.ItemCommand
        Try
            If e.Item.ItemType = ListViewItemType.DataItem Then
                hfListViewLastSelected.Value = "A"
                SelectedSubIdIndex.Text = ""

                lvMainApp_Sub.DataSource = Nothing
                lvMainApp_Sub.DataBind()
                lvMainApp_SubSub.DataSource = Nothing
                lvMainApp_SubSub.DataBind()

                For i As Int16 = 0 To lvMainApp.Items.Count - 1
                    CType(lvMainApp.Items(i).Controls(1).FindControl("imgModelIcon1"), ImageButton).BackColor = Nothing
                    CType(lvMainApp.Items(i).Controls(1).FindControl("divModelIcon1"), HtmlGenericControl).Style.Add("background-color", Nothing)
                Next

                CType(e.Item.Controls(1).FindControl("imgModelIcon1"), ImageButton).BackColor = Color.FromName("#B5C4DA")
                CType(e.Item.Controls(1).FindControl("divModelIcon1"), HtmlGenericControl).Style.Add("background-color", "#B5C4DA")
                SelectedMainIdIndex.Text = e.CommandArgument
                SelectedMainIdFamily.Text = CType(e.Item.Controls(1).FindControl("lblFamilyCode"), Label).Text
                fillSubData_1()
            End If
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.CONFIGURATORBUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)
        End Try
    End Sub

    Private Sub lvMainApp_Sub_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles lvMainApp_Sub.ItemDataBound
        Try

            If e.Item.ItemType = ListViewItemType.DataItem Then

                If CType(e.Item.DataItem(), DataRowView)("PictureName") <> "" Then
                    CType(e.Item.Controls(1).FindControl("imgModelIcon2"), ImageButton).ImageUrl = "../media/Configuration/SecondType/" & CType(e.Item.DataItem(), DataRowView)("PictureName") & ".png".ToString
                Else
                    CType(e.Item.Controls(1).FindControl("imgModelIcon2"), ImageButton).ImageUrl = "../media/Configuration/SecondType/CL.png"
                End If

                CType(e.Item.Controls(1).FindControl("btnModelname2"), Button).Text = CType(e.Item.DataItem(), DataRowView)("FamilyToolDes").ToString

                CType(e.Item.Controls(1).FindControl("imgModelIcon2"), ImageButton).CommandArgument = e.Item.DataItemIndex

                If CType(e.Item.DataItem(), DataRowView)("PictureName").ToString.ToUpper = "INDEXABLE" Or CType(e.Item.DataItem(), DataRowView)("PictureName").ToString.ToUpper = "" Then
                    CType(e.Item.Controls(1).FindControl("imgModelIcon2"), ImageButton).CssClass = "img-fluid img-thumbnail imgModelIconFilterF ModelIconButton"
                    CType(e.Item.Controls(1).FindControl("imgModelIcon2"), ImageButton).Enabled = False
                    CType(e.Item.Controls(1).FindControl("btnModelname2"), Button).Enabled = False
                    CType(e.Item.Controls(1).FindControl("btnModelname2"), Button).CssClass = "ModelCaptionButtonfilter"
                    CType(e.Item.Controls(1).FindControl("btnModelname2"), Button).Attributes.Add("loading", "lazy")
                Else
                    CType(e.Item.Controls(1).FindControl("imgModelIcon2"), ImageButton).CssClass = "img-fluid img-thumbnail ApplicationListItemDivMainF ModelIconButton"
                    CType(e.Item.Controls(1).FindControl("imgModelIcon2"), ImageButton).Enabled = True
                    CType(e.Item.Controls(1).FindControl("btnModelname2"), Button).Enabled = True
                    CType(e.Item.Controls(1).FindControl("btnModelname2"), Button).CssClass = "ModelCaptionButton"
                    CType(e.Item.Controls(1).FindControl("btnModelname2"), Button).Attributes.Add("loading", "lazy")
                End If


            End If

        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.CONFIGURATORBUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)
        End Try
    End Sub

    Private Sub lvMainApp_Sub_ItemCommand(sender As Object, e As ListViewCommandEventArgs) Handles lvMainApp_Sub.ItemCommand
        Try

            Try
                CType(lvMainApp.Items(SelectedMainIdIndex.Text).FindControl("imgModelIcon1"), ImageButton).BackColor = Color.FromName("#B5C4DA")
                CType(lvMainApp.Items(SelectedMainIdIndex.Text).FindControl("divModelIcon1"), HtmlGenericControl).Style.Add("background-color", "#B5C4DA")
            Catch ex As Exception

            End Try
            Try
                For i As Int16 = 0 To lvMainApp_Sub.Items.Count - 1
                    CType(e.Item.Controls(i).FindControl("imgModelIcon2"), ImageButton).BackColor = Color.FromName("#FFFFFF")
                    CType(e.Item.Controls(i).FindControl("divModelIcon2"), HtmlGenericControl).Style.Add("background-color", "#FFFFFF")
                Next
            Catch ex As Exception

            End Try

            hfListViewLastSelected.Value = "B"
            Dim s1 As String = CType(e.Item.Controls(1).FindControl("btnModelname2"), Button).Text
            Dim s2 As String = SelectedMainIdFamily.Text
            CType(e.Item.Controls(1).FindControl("imgModelIcon2"), ImageButton).BackColor = Color.FromName("#B5C4DA")
            CType(e.Item.Controls(1).FindControl("divModelIcon2"), HtmlGenericControl).Style.Add("background-color", "#B5C4DA")

            SelectedSubIdIndex.Text = e.CommandArgument
            fillSubData_2(s2, s1)

        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.CONFIGURATORBUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)
        End Try
    End Sub

    Private Sub lvMainApp_SubSub_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles lvMainApp_SubSub.ItemDataBound
        Try

            If e.Item.ItemType = ListViewItemType.DataItem Then
                If CType(e.Item.DataItem(), DataRowView)("ModelIcon").ToString <> "" Then
                    CType(e.Item.Controls(1).FindControl("imgModelIcon3"), ImageButton).ImageUrl = "../media/Configuration/ThirdModelImages/" & CType(e.Item.DataItem(), DataRowView)("ModelIcon") & ".gif"
                    Dim ss As String = CType(e.Item.DataItem(), DataRowView)("ModelDes").ToString
                    CType(e.Item.Controls(1).FindControl("btnModelname3"), Button).Text = ss ' ss.Substring(0, ss.IndexOf("_PROD"))

                    CType(e.Item.Controls(1).FindControl("lblFamilyNum"), Label).Text = CType(e.Item.DataItem(), DataRowView)("FamilyNum").ToString
                    CType(e.Item.Controls(1).FindControl("lblFamilyIcon"), Label).Text = CType(e.Item.DataItem(), DataRowView)("ModelIcon").ToString

                    CType(e.Item.Controls(1).FindControl("imgModelIcon3"), ImageButton).Enabled = True
                    CType(e.Item.Controls(1).FindControl("imgModelIcon3"), ImageButton).CssClass = "img-fluid img-thumbnail ApplicationListItemDivMainSS ModelIconButton"
                    CType(e.Item.Controls(1).FindControl("btnModelname3"), Button).Enabled = True
                    CType(e.Item.Controls(1).FindControl("btnModelname3"), Button).CssClass = "ModelCaptionButton"
                    CType(e.Item.Controls(1).FindControl("btnModelname3"), Button).Attributes.Add("loading", "lazy")
                Else
                    CType(e.Item.Controls(1).FindControl("imgModelIcon3"), ImageButton).ImageUrl = "../media/Configuration/ThirdModelImages/CL.png"
                    CType(e.Item.Controls(1).FindControl("btnModelname3"), Button).Text = ""
                    CType(e.Item.Controls(1).FindControl("imgModelIcon3"), ImageButton).Enabled = False
                    CType(e.Item.Controls(1).FindControl("btnModelname3"), Button).Enabled = False
                    CType(e.Item.Controls(1).FindControl("btnModelname3"), Button).Visible = False
                    CType(e.Item.Controls(1).FindControl("btnModelname3"), Button).Attributes.Add("loading", "lazy")
                End If
            End If

        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.CONFIGURATORBUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)
        End Try
    End Sub

    Private Sub writetos(sD As String)
        Try
            Dim s As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, sD, s, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, sD, ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
        End Try
    End Sub

    Private Sub lvMainApp_SubSub_ItemCommand(sender As Object, e As ListViewCommandEventArgs) Handles lvMainApp_SubSub.ItemCommand
        Try
            hfListViewLastSelected.Value = "C"

            Dim bc As String = clsBranch.ReturnActiveBranchCodeSESSTION

            SessionManager.Clear_Sessions_For_NewStartedQuotation()

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.BranchCode, bc)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationOpenType, clsQuatation.Enum_QuotationOpenType.CONFIGURATOR)

            Dim fannum As String = CType(e.Item.FindControl("lblFamilyNum"), Label).Text

            Dim dtTimeStamp As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtTempOnlyForCheck, "")

            If Not dtTimeStamp Is Nothing AndAlso dtTimeStamp.Rows.Count > 0 Then
                dtTimeStamp.Rows(0).Item("ModelID") = fannum
                dtTimeStamp.Rows(0).Item("ModelNum") = fannum
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelNumModification, fannum)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelNumConfiguration, fannum)

                dtTimeStamp.Rows(0).Item("FamilyID") = CType(e.Item.FindControl("lblFamilyIcon"), Label).Text
            End If

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtTempOnlyForCheck, dtTimeStamp)

            Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"

            Response.Redirect("~/Forms/Material.aspx" & uniqueID & "iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), False)
        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.CONFIGURATORBUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)
        End Try
    End Sub

    Private Sub ConfiguratorBuilder_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "lasthideListViewA", "lasthideListView();", True)
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "CheckDivWidth", "SetDivHeightWithScroll();", True)

            Try
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetLanControlsCaptionCon", "SetCaptionForLabelsCon()", True)
            Catch ex As Exception

            End Try


        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.CONFIGURATORBUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)
        End Try
    End Sub

    Private Sub btnBackA_Click(sender As Object, e As EventArgs) Handles btnBackCi.Click, btnBackBi.Click
        Try
            Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
            Response.Redirect("ConfiguratorBuilder.aspx" & uniqueID & "iqlang=" & utl.ReturnParamLanguage(Request("iqlang"), True) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang"), True), False)


        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.CONFIGURATORBUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)
        End Try

    End Sub
    Private Sub CheckIfLanguageSelectedConfig()
        Try

            Dim cl As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.qut_LanguageId, False)
            Dim nl As String = CType(Master.FindControl("hfLanguageselected"), HiddenField).Value

            If cl <> nl AndAlso nl <> "" Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_LanguageCaptions, CType(Nothing, DataTable))
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.qut_LanguageId, nl)
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetCaptionForLabelsD", "SetCaptionForLabels()", True)
            End If

        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.CONFIGURATORBUILDER_ERROR, MethodBase.GetCurrentMethod().Name, ex.Message, True)
        End Try
    End Sub


End Class