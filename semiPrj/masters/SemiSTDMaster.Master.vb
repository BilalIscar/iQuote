Imports System.IO
Imports System.Net
Imports SemiApp_bl
Imports SemiApp_bl.clsUser

Public Class SemiSTDMaster
    Inherits System.Web.UI.MasterPage

    Public Property MyQuotetionClick As Boolean = False

    Private Sub ShowAdminItems()
        Dim USerType As String = ""
        Dim unicid As String = ""
        Dim BranchCode As String = ""
        Try
            'If lblUserInformation.Text = "" Then
            lblUserInformation.Visible = False
            hlPostBachAdmin.Visible = False
            hlTransition.Visible = False
            hlUserCustomerConnect.Visible = False

            unicid = Request("rErepTr")
            BranchCode = StateManager.GetValue(StateManager.Keys.s_BranchCode)
            Dim btnsUserAdmin As String = ConfigurationManager.AppSettings("ShowUserAdminButton")

            Dim loggedEmail As String = StateManager.GetValue(StateManager.Keys.s_loggedEmail, True)

            lblBranchCode.Text = BranchCode

            Dim sdtUserType As DataTable = clsUser.GetUserType(BranchCode, loggedEmail, USerType)

            Select Case USerType
                Case clsUser.e_UserType.AdminALL
                    If Not sdtUserType Is Nothing AndAlso sdtUserType.Rows.Count > 0 AndAlso sdtUserType.Rows(0).Item("BranchAdmin").ToString.ToUpper = "TRUE" Then
                        'Dim Ss_loggedEmail As String = CryptoManager.Encode(StateManager.GetValue(StateManager.Keys.s_loggedEmail).ToString)
                        Dim Ss_loggedEmail As String = StateManager.GetValue(StateManager.Keys.s_loggedEmail).ToString
                        'Dim Ss_BC As String = CryptoManager.Encode(StateManager.GetValue(StateManager.Keys.s_BranchCode).ToString)
                        Dim Ss_BC As String = StateManager.GetValue(StateManager.Keys.s_BranchCode).ToString
                        'CookiesManager.SetStringValue("UserEmailForCurentQut", Ss_loggedEmail, ConfigurationManager.AppSettings("siteType").ToString.ToUpper)
                        'CookiesManager.SetStringValue("BranchCodeForCurentQut", Ss_BC, ConfigurationManager.AppSettings("siteType").ToString.ToUpper)
                        If btnsUserAdmin = "YES" Then
                            SetConnectHLenabled(unicid)
                        End If
                    End If
                    lblUserInformation.Visible = True

                    hlPostBachAdmin.NavigateUrl = "..\formAdmin\AdminFactors.aspx?rErepTr=" & unicid
                    hlPostBachAdmin.Visible = True
                    Dim dt As DataTable = clsBranch.GetBranchDetails(StateManager.GetValue(StateManager.Keys.s_BranchCode))
                    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                        lblUserInformation.Text = "Connected As:  " & dt.Rows(0).Item("BranchName").ToString & "-" & dt.Rows(0).Item("BranchNumber").ToString & " ; "
                    End If
                    lblUserInformation.Text &= "Customer:  " & StateManager.GetValue(StateManager.Keys.s_CustomerNumber) & " " & StateManager.GetValue(StateManager.Keys.s_CustomerName)
                Case clsUser.e_UserType.BranchAdmin
                    If Not sdtUserType Is Nothing AndAlso sdtUserType.Rows.Count > 0 AndAlso sdtUserType.Rows(0).Item("BranchAdmin").ToString.ToUpper = "TRUE" Then
                        Dim Ss_loggedEmail As String = StateManager.GetValue(StateManager.Keys.s_loggedEmail).ToString
                        'Dim Ss_loggedEmail As String = CryptoManager.Encode(StateManager.GetValue(StateManager.Keys.s_loggedEmail).ToString)
                        Dim Ss_BC As String = StateManager.GetValue(StateManager.Keys.s_BranchCode).ToString
                        'Dim Ss_BC As String = CryptoManager.Encode(StateManager.GetValue(StateManager.Keys.s_BranchCode).ToString)
                        'CookiesManager.SetStringValue("UserEmailForCurentQut", Ss_loggedEmail, ConfigurationManager.AppSettings("siteType").ToString.ToUpper)
                        'CookiesManager.SetStringValue("BranchCodeForCurentQut", Ss_BC, ConfigurationManager.AppSettings("siteType").ToString.ToUpper)
                        If btnsUserAdmin = "YES" Then
                            SetConnectHLenabled(unicid)
                        End If
                    End If
            End Select
        Catch ex As Exception

        End Try

        Try
            Dim btns As String = ConfigurationManager.AppSettings("ShowSimulationButton")

            If btns.ToUpper = "ALWAYS" Then
                hlTransition.NavigateUrl = "..\formAdmin\Transition.aspx?rErepTr=" & unicid
                hlTransition.Visible = True
            ElseIf btns.ToUpper = "NO" Then
                hlTransition.Visible = False
            ElseIf btns.ToUpper = "YES" Then
                Dim dtsm As DataTable = clsBranch.GetBranchDetails(BranchCode)
                If Not dtsm Is Nothing AndAlso dtsm.Rows.Count > 0 Then
                    '"IG"
                    If dtsm.Rows(0).Item("Simulation").ToString = "YES" Then
                        hlTransition.NavigateUrl = "..\formAdmin\Transition.aspx?rErepTr=" & unicid
                        hlTransition.Visible = True
                    ElseIf dtsm.Rows(0).Item("Simulation").ToString = "NO" Then
                        Select Case USerType
                            Case clsUser.e_UserType.AdminALL
                                hlTransition.NavigateUrl = "..\formAdmin\Transition.aspx?rErepTr=" & unicid
                                hlTransition.Visible = True
                                'Case clsUser.e_UserType.BranchAdmin
                                '    hlTransition.NavigateUrl = "..\formAdmin\Transition.aspx?rErepTr=" & unicid
                                '    hlTransition.Visible = True
                        End Select
                    End If
                End If





            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub SetConnectHLenabled(unicid As String)
        Try
            Dim Ss_loggedEmail As String = CryptoManagerTDES.Encode(StateManager.GetValue(StateManager.Keys.s_loggedEmail).ToString)
            Dim Ss_BC As String = CryptoManagerTDES.Encode(StateManager.GetValue(StateManager.Keys.s_BranchCode).ToString)
            ' Dim ce As String = CookiesManager.GetStringValue(CryptoManagerTDES.Encode("UserEmailForCurentQut"))

            If Ss_BC.ToString <> "" AndAlso Ss_loggedEmail.ToString <> "" Then
                'CookiesManager.SetStringValue("UserEmailForCurentQut", Ss_loggedEmail, ConfigurationManager.AppSettings("siteType").ToString.ToUpper)
                'CookiesManager.SetStringValue("BranchCodeForCurentQut", Ss_BC, ConfigurationManager.AppSettings("siteType").ToString.ToUpper)

                'Response.Write("Raw Cookie Value: " & CookiesManager.GetStringValue(CryptoManagerTDES.Encode("UserEmailForCurentQut")))


                hlUserCustomerConnect.NavigateUrl = "~\formAdmin\UserCustomerCnt.aspx?rErepTr=" & unicid & "&rEreptochr=" & Ss_loggedEmail & "&rEreptochrbc=" & Ss_BC
                'hlUserCustomerConnect.NavigateUrl = "~\formAdmin\UserCustomerCnt.aspx?rErepTr=" & unicid & "&rEreptochr=" & CookiesManager.GetStringValue(CryptoManagerTDES.Encode("UserEmailForCurentQut"))
                hlUserCustomerConnect.Visible = True
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub SemiSTDMaster_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        'Try
        '    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideshlogbuttonPricesCallx", "hideshlogbuttonSelected();", True)
        '    ScriptManager.RegisterStartupScript(Page, Page.GetType(), " ManualDialogSelectedLanguagex", "ManualDialogSelectedLanguage();", True)
        'Catch ex As Exception

        'End Try
        Try
            'CookiesManager.SetValue(CookiesManager.Keys.etoedfrku, "", True)
            Dim sLogEmail As String = clsQuatation.ACTIVE_UseLoggedEmail

            'CookiesManager.SetValue(CookiesManager.Keys.etoedfrku, sLogEmail, True)
        Catch ex As Exception

        End Try

        Try
            btnloginUserNameVF.Visible = False
            Dim sgn As String = ""
            sgn = StateManager.GetValue(StateManager.Keys.s_DisplayName, False)

            If sgn Is Nothing Then sgn = ""

            Try
                If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TemporarilyQuotation, False) Is Nothing Then

                    If Not clsQuatation.IsTemporary_Quotatiom Then
                        btnDivLogIn.Enabled = False
                    End If
                End If
            Catch ex As Exception
                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "SemiSTDMaster_PreRender", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            End Try



            sgn.Replace("0", "").Replace(" ", "")
            Try
                'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetLanControlsCaption", "SetCaptionForLabels()", True)
                'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "disaleNewbtnN", "disaleNewbtn()", True)
                'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideAccountBTNN", "HideAccountBTN()", True)


            Catch ex As Exception

            End Try
            If sgn <> "" Then
                txthideshlogbutton.Text = sgn
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideShowLogOutButtonA", "<script>hideshlogbutton(1)</script>", False)
                btnloginUserName.Visible = False
            Else
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideShowLogOutButtonB", "<script>hideshlogbutton(1)</script>", False)

                btnloginUserName.Visible = True
            End If


            If ConfigurationManager.AppSettings("DoCheckLogIn") = "False" Then
                btnloginUserName.Enabled = False
                btnloginUserName.Visible = False
                btnloginUserNameVF.Visible = True
            End If
            If btnUnit.Text = "" Then

                Dim s As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.vers, False)
                If s = "" Then s = "M"
                btnUnit.Text = s
            End If

            If clsQuatation.ACTIVE_OpenType.ToString = clsQuatation.Enum_QuotationOpenType.MODIFICATION.ToString Then
                btnUnit.Enabled = False
                btnUnitC.Enabled = False
            Else
                btnUnit.Enabled = True
                btnUnitC.Enabled = True
            End If

            Dim SHowLogF As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempShoeLogInFrame, False).ToString

            If SHowLogF = "Y" AndAlso clsQuatation.IsTemporary_Quotatiom Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempShoeLogInFrame, "")
                divMyQut2.Visible = True
            Else
                divMyQut2.Visible = False
            End If

            txttxtdivMyQut2.Text = "True"

            heBC.Text = clsBranch.ReturnActiveBranchCodeState
            reBC.Text = CryptoManagerTDES.Decode(Request("repLang"))
            If hfselectedBC.Value = "" Then
                hfselectedBC.Value = "ZZ"

            End If
            If hfselectedBC.Value = "IG" Or heBC.Text = "IG" Then
                pnlinfGud1.Visible = True
                pnlinfGud2.Visible = True
                pnlinfGud3.Visible = True
                pnlinfGud4.Visible = True
            Else
                pnlinfGud1.Visible = False
                pnlinfGud2.Visible = False
                pnlinfGud3.Visible = False
                pnlinfGud4.Visible = False
            End If
            reBCDcod.Text = Request("repLang")

            ttrErepTr.Text = Request("rErepTr")

            If ttrErepTr.Text.ToString.Trim = "" Then
                ttrErepTr.Text = ""
            End If


        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "SemiSTDMaster_PreRender", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        Finally
            Try
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), " CloseLangselectSd", "CloseLangselect();", True)
            Catch ex As Exception

            End Try
        End Try

    End Sub

    Private Sub FindQuotation(SerialNo As String)

        Try
            Dim dt As DataTable = clsQuatation.GetQuotationBySerialNo(SerialNo)
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Dim qut As String = dt.Rows(0).Item("QuotationNum").ToString
                Dim modcon As String = dt.Rows(0).Item("OpenType").ToString
                Dim TemporarilyQuotation As String = dt.Rows(0).Item("TemporarilyQuotation").ToString
                If TemporarilyQuotation = "True" Then
                    SessionManager.Clear_ALL_SessionsTimeStamp()

                    If start.StartForm(clsQuatation.e_QuotationStatus.Exist_QutOpenedFromQuotationList, modcon, "", "", "", "ZZ", "", "", "", "", qut, True, False) = True Then
                        Dim displayTpe As String = "Prices.aspx"

                        Try
                            Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"

                            Response.Redirect(displayTpe & uniqueID & "iqlang= " & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), False)
                        Catch ex As Exception
                            ' Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
                            '  Response.Redirect(displayTpe & uniqueID, False)
                        End Try

                    End If

                End If
            Else
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "SerialNumberNotExistAlert();", True)
            End If

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub btnOpenQuotations_Click(sender As Object, e As EventArgs) Handles btnOpenQuotations.Click
        Try
            MyQuotetionClick = True
            If Not clsQuatation.IsTemporary_User(True, False) Then

                SessionManager.Clear_ALL_Sessions()
                SessionManager.Clear_ALL_SessionsTimeStamp()

                Try
                    Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"

                    Response.Redirect("~/Forms/QuotationsList.aspx" & uniqueID & "iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("iqlang")), True)
                Catch ex As Exception
                    'Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
                    'Response.Redirect("~/Forms/QuotationsList.aspx" & uniqueID, False)
                End Try
            Else
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempShoeLogInFrame, "Y")

            End If


        Catch ex As Exception
            'GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub SemiSTDMaster_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try

            '--------------------SELECTED FLAG/LANGUAGE--------------
            Dim selectedBC As String = "ZZ"
            If clsLanguage.CheckIfLanguageSelected(hfLanguageselected.Value.ToString, selectedBC) = True Then
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetCaptionForLabelsD", "SetCaptionForLabels()", True)
            End If
            hfselectedBC.Value = selectedBC
            '--------------------------------------------------------


            If Not Request("repLang") Is Nothing AndAlso Request("repLang") <> "" Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.selectedReportLanguage, Request("repLang"))
            Else
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.selectedReportLanguage, "pnh0tFBOF/I=") '"ZZ"
            End If

            lblMasterReporCountry.Text = clsBranch.GetCountryName(CryptoManagerTDES.Decode(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.selectedReportLanguage, False)))
        Catch ex As Exception
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.selectedReportLanguage, "pnh0tFBOF/I=")
        End Try

        Try
            If Not Request("iqlang") Is Nothing AndAlso Request("iqlang") <> "" Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.selectedLanguage, Request("iqlang"))
            Else
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.selectedLanguage, "pnh0tFBOF/I=") '"ZZ"
            End If
        Catch ex As Exception
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.selectedLanguage, "pnh0tFBOF/I=")
        End Try

        Try
            SetCaptionsLanguage()

            If Not IsPostBack Then
                FillLanguagesSelected()
                ShowAdminItems()



                If clsQuatation.IsTemporary_User(False, True) Then
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideAccountBTN", "HideAccountBTN();", True)
                    pnlGuideTemp.Visible = True
                Else
                    pnlGuide.Visible = True

                    Try

                        lblCustomerNo.Text = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
                        lblCustoName.Text = StateManager.GetValue(StateManager.Keys.s_CustomerName, True).ToString.Trim
                        lblCustomerAddress.Text = StateManager.GetValue(StateManager.Keys.s_CustomerAddress, True).ToString.Trim
                        lblPaymenttTerms.Text = StateManager.GetValue(StateManager.Keys.s_paymentTerms, True).ToString.Trim
                        lblShippingMethod.Text = StateManager.GetValue(StateManager.Keys.s_shipmethod, True).ToString.Trim
                        lblsalesperson.Text = StateManager.GetValue(StateManager.Keys.s_salesperson, True).ToString.Trim
                        lblsalespersonEmail.Text = StateManager.GetValue(StateManager.Keys.s_salespersonEmail, True).ToString.Trim
                        lbldeskUser.Text = StateManager.GetValue(StateManager.Keys.s_deskUser, True).ToString.Trim
                        lbldeskUserEmail.Text = StateManager.GetValue(StateManager.Keys.s_deskUserEmail, True).ToString.Trim
                        lbltechnicalPerson.Text = StateManager.GetValue(StateManager.Keys.s_technicalPerson, True).ToString.Trim
                        lbltechnicalPersonEmail.Text = StateManager.GetValue(StateManager.Keys.s_technicalPersonEmail, True).ToString.Trim

                        Dim dt As DataTable = clsBranch.GetBranchDetails(clsBranch.ReturnActiveBranchCodeState) 'SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)
                        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                            lblISCARSubsidiary.Text = dt.Rows(0).Item("BranchName").ToString
                            lblSubsidiaryAddress.Text = dt.Rows(0).Item("Address1").ToString
                        End If

                    Catch ex As Exception
                    End Try

                End If

            End If

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try

        Try

            Try
                If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.vers_OnlyForNew, False) Is Nothing Then
                    If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.vers_OnlyForNew, False) = "I" Then
                        If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.vers, False) = "" Then
                            btnUnitC_ClickOnlyForVer()

                        End If
                    End If
                End If
            Catch ex As Exception

            End Try

            Dim ssA As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._TempShowCustomerAlertLogIN, False).ToString.ToUpper

            If ssA <> "" Then

                Dim s() As String = ssA.Split("#")

                If s.Length > 1 Then
                    If s(1).ToString <> "" AndAlso s(1).Length = 2 Then
                        If CheckIfAllRedySentMailToCustomer(s(0).ToString, s(1).ToString) = False Then
                            'Dim langID As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.qut_LanguageId, False)
                            Dim dtMessage As DataTable = clsLanguage.Get_LanguageMessagesCaption(clsLanguage.e_Form.CustomerService)

                            Dim txt1 As String = dtMessage.Rows(0).Item("MESSAGE").ToString
                            Dim txt2 As String = dtMessage.Rows(1).Item("MESSAGE").ToString
                            Dim txt3 As String = dtMessage.Rows(2).Item("MESSAGE").ToString
                            Dim txt4 As String = dtMessage.Rows(3).Item("MESSAGE").ToString
                            Dim txt5 As String = dtMessage.Rows(4).Item("MESSAGE").ToString
                            Dim txt6 As String = dtMessage.Rows(5).Item("MESSAGE").ToString
                            Dim txt7 As String = dtMessage.Rows(6).Item("MESSAGE").ToString
                            Dim txt8 As String = dtMessage.Rows(7).Item("MESSAGE").ToString
                            Dim txt9 As String = dtMessage.Rows(8).Item("MESSAGE").ToString
                            Dim txt10 As String = dtMessage.Rows(9).Item("MESSAGE").ToString
                            Dim txt11 As String = dtMessage.Rows(10).Item("MESSAGE").ToString

                            Dim dt As DataTable = clsBranch.GetBranchDetails(s(1))
                            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                                Dim Sb As String = dt.Rows(0)("BranchName").ToString.Trim
                                Dim ba As String = dt.Rows(0)("CustomerServiceMail").ToString.Trim
                                Dim givenName As String = s(2).ToString
                                Dim surname As String = s(3).ToString
                                Dim businessPhones As String = s(4).ToString
                                Dim companyName As String = s(5).ToString

                                Dim sStr As String = ""
                                sStr &= "<div style='font-family: Oswald,Calibri,Arial,Script;font-size: 16px'>" & txt2 & "</div>"
                                sStr &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txt4 & "</div>"
                                sStr &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txt5 & "</div>"
                                sStr &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txt6 & "</div>"

                                clsMail.SendEmailWithoutAttachment(s(0).ToString, txt1, sStr, False, True, Server.MapPath("~"), False, Nothing, "", "", "")

                                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempShowCustomerAlertLogIN, "NO")

                                Dim bcTos As String = s(1).ToString
                                If bcTos <> "IG" Then
                                    bcTos = ""
                                End If
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "CusomerAlertF", "CusomerAlertForLoginFailedTimeOut('" + Sb + "','" + bcTos + "');", True)

                                Dim sStr1 As String = ""
                                sStr1 &= "<div style='font-family: Oswald,Calibri,Arial,Script;font-size: 16px'>" & txt3 & "</div>"
                                sStr1 &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txt7 & "</div>"
                                sStr1 &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txt8 & " : " & s(0).ToString & ",</div>"
                                sStr1 &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txt9 & " : " & givenName & " " & surname & ",</div>"
                                sStr1 &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txt10 & " : " & businessPhones & ",</div>"
                                sStr1 &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txt11 & " : " & companyName & ",</div>"

                                clsMail.SendEmailWithoutAttachment(ba, txt1, sStr1, False, True, Server.MapPath("~"), False, Nothing, "", "", "")

                            End If
                        End If

                        End If
                End If

            End If

            Try
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideshlogbuttonPricesCallx", "hideshlogbuttonSelected();", True)
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), " ManualDialogSelectedLanguagex", "ManualDialogSelectedLanguage();", True)
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), " disaleNewbtnxx", "disaleNewbtn();", True)
            Catch ex As Exception

            End Try

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub

    Protected Sub FillLanguagesSelected()

        Try

            Dim sS As String = "pnh0tFBOF/I="
            Dim dt As DataTable = clsBranch.GetBranchLanguages(clsBranch.ReturnActiveBranchCodeState, sS, "")

            Dim d1 As New DataColumn : d1.ColumnName = "LanguageCode"
            Dim d2 As New DataColumn : d2.ColumnName = "LanguageImg"
            Dim d3 As New DataColumn : d3.ColumnName = "LanguageName"
            Dim d4 As New DataColumn : d4.ColumnName = "LanguageId"
            Dim d5 As New DataColumn : d5.ColumnName = "ISOCode"
            Dim d6 As New DataColumn : d6.ColumnName = "CountryFlagName"

            Dim dtLangauges As New DataTable
            dtLangauges.Columns.Add(d1)
            dtLangauges.Columns.Add(d2)
            dtLangauges.Columns.Add(d3)
            dtLangauges.Columns.Add(d4)
            dtLangauges.Columns.Add(d5)
            dtLangauges.Columns.Add(d6)
            For Each r As DataRow In dt.Rows

                dtLangauges.Rows.Add()
                dtLangauges.Rows(dtLangauges.Rows.Count - 1).Item("LanguageCode") = CryptoManagerTDES.Encode(r.Item("BranchCode"))
                dtLangauges.Rows(dtLangauges.Rows.Count - 1).Item("LanguageImg") = r.Item("CountryFlagName")
                dtLangauges.Rows(dtLangauges.Rows.Count - 1).Item("LanguageName") = r.Item("Language")
                If r.Item("BranchCode") = "ZZ" Then
                    dtLangauges.Rows(dtLangauges.Rows.Count - 1).Item("LanguageId") = 0
                Else
                    dtLangauges.Rows(dtLangauges.Rows.Count - 1).Item("LanguageId") = r.Item("LanguageId")
                End If
                dtLangauges.Rows(dtLangauges.Rows.Count - 1).Item("ISOCode") = r.Item("ISOCode")
                dtLangauges.Rows(dtLangauges.Rows.Count - 1).Item("CountryFlagName") = r.Item("CountryFlagName")
            Next

            rptLanguagesListCaptions.DataSource = dtLangauges
            rptLanguagesListCaptions.DataBind()

            d1 = Nothing
            d2 = Nothing
            d3 = Nothing
            d4 = Nothing
            d5 = Nothing
            d6 = Nothing

        Catch ex As Exception

        End Try

    End Sub
    Private Function CheckIfAllRedySentMailToCustomer(LoggedEm As String, BC As String) As Boolean
        Try

            Dim dt As DataTable = clsQuatation.Check_IfAllRedySentMailToCustomer(BC, LoggedEm)
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                If IsNumeric(dt.Rows(0).Item("Count").ToString) AndAlso CInt(dt.Rows(0).Item("Count")) > 0 Then
                    Return True

                End If
            End If
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub btnloginUserNAme_Click(sender As Object, e As EventArgs) Handles btnloginUserName.Click
        Try
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempShoeLogInFrame, "L")

            Try
                If Request.Url.AbsoluteUri.ToString.ToUpper.Trim.Contains("MATERIAL.ASPX") Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempShoeLogInFrame, "")
                End If
            Catch ex As Exception

            End Try





            LogIn.LogINcL()

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Try

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_LanguageCaptions, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.qut_LanguageId, 0)
            LogIn.LogOut()


        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Protected Sub btnUnitC_ClickOnlyForVer()
        Try
            SessionManager.Clear_ALL_SessionsTimeStamp()
            SessionManager.Clear_Sessions_For_NewStartedQuotation()
            Dim s As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.vers, False)

            If s = "M" Or s = "" Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.vers, "I")
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.vers_OnlyForNew, "I")

                btnUnit.Text = "I"
            Else
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.vers, "M")
                btnUnit.Text = "M"
            End If
            Dim sY As String = ""
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempConfigDefaultShow, "")

            Try
                'Dim ssBilDef As String = CookiesManager.Get_Cookie(CookiesManager.Keys.ssBil.ToString)

                Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
                Response.Redirect("ConfiguratorBuilder.aspx" & uniqueID & "iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), False)
            Catch ex As Exception
                'Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
                'Response.Redirect("ConfiguratorBuilder.aspx" & uniqueID, False)
            End Try

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub
    Protected Sub btnUnitC_Click(sender As Object, e As EventArgs)
        Try
            SessionManager.Clear_ALL_SessionsTimeStamp()

            SessionManager.Clear_Sessions_For_NewStartedQuotation()

            Dim s As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.vers, False)

            If s = "M" Or s = "" Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.vers, "I")
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.vers_OnlyForNew, "I")
                btnUnit.Text = "I"
            Else
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.vers, "M")
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.vers_OnlyForNew, "")
                btnUnit.Text = "M"
            End If
            Dim sY As String = ""
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempConfigDefaultShow, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.DoCheck_LogIN, "NO")



            Try
                'Dim ssBilDef As String = CookiesManager.Get_Cookie(CookiesManager.Keys.ssBil.ToString)

                Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
                'Response.Redirect("../Default.aspx" & uniqueID & "&iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")) & "&DontClearCoo=NO", False)

                Response.Redirect("../Default.aspx" & uniqueID & "&iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")) & "&DontClearCoo=NO", False)
            Catch ex As Exception
                ' Response.Redirect("../Default.aspx", False)
            End Try

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub btnCloseMyQutDiv_Click(sender As Object, e As EventArgs) Handles btnCloseMyQutDiv.Click
        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempShoeLogInFrame, "")
        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempLeftImage, "")
        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempRightImage, "")
    End Sub

    Private Sub btnDivLogIn_Click(sender As Object, e As EventArgs) Handles btnDivLogIn.Click
        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempShoeLogInFrame, "Y")

        LogIn.LogINcL()

    End Sub

    Private Sub btnFind_Click(sender As Object, e As EventArgs) Handles btnFind.Click
        If txtFindSN.Text.Trim <> "" Then

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempShoeLogInFrame, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempLeftImage, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempRightImage, "")

            FindQuotation(txtFindSN.Text.Trim)

        End If
    End Sub

    Private Sub btnReportSubmitID_Click(sender As Object, e As EventArgs) Handles btnReportSubmitID.Click
        Try
            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.tempMessageAllredySent) = "NO" Then
                Dim sMsg As String = ""
                Dim sName As String = txtMessagetxtMailName.Text
                Dim sEmail As String = txtMessagetxtMailEmail.Text
                Dim sCompany As String = txtMessagetxtMailCountry.Text
                Dim sCompanyName As String = txtMessagetxtMailCompanyName.Text
                Dim sMessage As String = txtMessageContactU.Text
                sMessage = sMessage.Replace("" & vbCrLf & "", "<br>")
                Dim bc As String = clsBranch.ReturnActiveBranchCodeState
                sMsg = sMsg & "<br>"
                sMsg = sMsg & "Name - " & sName
                sMsg = sMsg & "</br>" & hfEmail.Value & " - " & sEmail
                sMsg = sMsg & "</br>" & hftCountry.Value & " - " & sCompany
                sMsg = sMsg & "</br>" & hftCompanyName.Value & " - " & sCompanyName
                sMsg = sMsg & "</br>" & hftMessage.Value & ":" & sMessage

                clsMail.SendEmailWithoutAttachment(ConfigurationManager.AppSettings("MailFrom").ToString.Trim, "iQuote " & hftContactUs.Value, sMsg, True, True, Server.MapPath("~"), False, Nothing, bc, "", "CONTACTUS")

                txtMessagetxtMailName.Text = ""
                txtMessagetxtMailEmail.Text = ""
                txtMessagetxtMailCountry.Text = ""
                txtMessagetxtMailCompanyName.Text = ""
                txtMessageContactU.Text = ""

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempMessageAllredySent, "YES")

            End If

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "btnReportSubmitID_Click", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub btnSendFeedback_Click(sender As Object, e As EventArgs) Handles btnSendFeedback.Click
        Try
            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.tempMessageSendFeedBack) = "NO" Then

                Dim sMsg As String = ""

                Dim feedBackA As String = txtFeed_Apic.Text
                Dim feedBackB As String = txtFeed_Bpic.Text

                Dim sMessage As String = txtMessageFeedBack.Text
                sMessage = sMessage.Replace("" & vbCrLf & "", "<br>")
                Dim siMogiA As String = ""
                Select Case feedBackA
                    Case "AA" : siMogiA = ":)"
                    Case "AB" : siMogiA = ":|"
                    Case "AC" : siMogiA = ":("
                End Select
                Dim siMogiB As String = ""
                Select Case feedBackB
                    Case "BA" : siMogiB = ":)"
                    Case "BB" : siMogiB = ":|"
                    Case "BC" : siMogiB = ":("
                End Select

                sMsg &= "<br>"
                sMsg &= "<br>"
                sMsg &= "<font color=blue>Did you receive relevant result?</font><br> " & siMogiA
                sMsg &= "<br>"
                sMsg &= "<br>"
                sMsg &= "<font color=blue>Are you satisfied with the new iQuote Advisor?</font><br> " & siMogiB
                sMsg &= "<br>"
                sMsg &= "<br>"
                sMsg &= "<font color=blue>What would you improve?</font><br> " & sMessage

                Dim bc As String = clsBranch.ReturnActiveBranchCodeState
                clsMail.SendEmailWithoutAttachment(ConfigurationManager.AppSettings("MailFrom").ToString.Trim, "iQuote Feedback", sMsg, True, True, Server.MapPath("~"), False, Nothing, bc, "", "FEEDBACK")

                txtFeed_Apic.Text = ""
                txtFeed_Bpic.Text = ""
                txtMessageFeedBack.Text = ""

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempMessageSendFeedBack, "YES")
            End If

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "btnSendFeedback_Click", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub btnStartNewQ_Click(sender As Object, e As EventArgs) Handles btnStartNewQ.Click ', btnStartNewQmenue.Click
        MyQuotetionClick = True
        SessionManager.Clear_Sessions_OpenIquote("FALSE")
        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.DoCheck_LogIN, "NO")

        Dim galCustomerNumber As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, False)
        Dim bctoGetDetails As String = clsBranch.ReturnActiveBranchCodeState


        '------------------------
        Dim sLg As String = ""
        Try
            sLg = StateManager.GetValue(StateManager.Keys.s_loggedEmail, True)
        Catch ex As Exception
            sLg = ""
        End Try
        If sLg Is Nothing Then sLg = ""


        If sLg <> "" AndAlso sLg.Contains("@") AndAlso SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TemporarilyQuotation, False).ToString.ToUpper = "TRUE" Then
            Dim dtb As DataTable = clsBranch.GetBranchDetails(bctoGetDetails)
            If Not dtb Is Nothing AndAlso dtb.Rows.Count > 0 AndAlso bctoGetDetails <> "ZZ" Then
                SessionManager.SetSessionDetails_Temporarily("FALSE")
            Else
                SessionManager.SetSessionDetails_Temporarily("TRUE")
            End If

            Try
                clsQuatation.GetCustomerAccountDetails(galCustomerNumber, bctoGetDetails)
            Catch ex As Exception
                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "SetNewQuotationState", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
            End Try
        End If

        Try
            Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
            Response.Redirect("../Default.aspx" & uniqueID & "&iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")) & "&DontClearCoo=NO", False)
            'Dim ssBilDef As String = CookiesManager.Get_Cookie(CookiesManager.Keys.ssBil.ToString)
            'Response.Redirect("../Default.aspx?iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")) & "&DontClearCoo=NO", False)
        Catch ex As Exception
            '  Response.Redirect("../Default.aspx", False)
        End Try
    End Sub

    Private Sub btnOpenHome_Click(sender As Object, e As ImageClickEventArgs) Handles btnOpenHome.Click
        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.DoCheck_LogIN, "NO")


        Try
            Response.Redirect("~/Default.aspx?iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), False)
        Catch ex As Exception
            ' Response.Redirect("~/Default.aspx", False)
        End Try
    End Sub


    Protected Sub FillLanguages()

        Try

            Dim sS As String = "pnh0tFBOF/I="
            Dim dt As DataTable = clsBranch.GetBranchLanguages(clsBranch.ReturnActiveBranchCodeState, sS, "")

            Dim d1 As New DataColumn : d1.ColumnName = "LanguageCode"
            Dim d2 As New DataColumn : d2.ColumnName = "LanguageImg"
            Dim d3 As New DataColumn : d3.ColumnName = "LanguageName"
            Dim d4 As New DataColumn : d4.ColumnName = "LanguageId"
            Dim d5 As New DataColumn : d5.ColumnName = "ISOCode"
            Dim d6 As New DataColumn : d6.ColumnName = "CountryFlagName"

            Dim dtLangauges As New DataTable
            dtLangauges.Columns.Add(d1)
            dtLangauges.Columns.Add(d2)
            dtLangauges.Columns.Add(d3)
            dtLangauges.Columns.Add(d4)
            dtLangauges.Columns.Add(d5)
            dtLangauges.Columns.Add(d6)
            For Each r As DataRow In dt.Rows

                dtLangauges.Rows.Add()
                dtLangauges.Rows(dtLangauges.Rows.Count - 1).Item("LanguageCode") = CryptoManagerTDES.Encode(r.Item("BranchCode"))
                dtLangauges.Rows(dtLangauges.Rows.Count - 1).Item("LanguageImg") = r.Item("CountryFlagName")
                dtLangauges.Rows(dtLangauges.Rows.Count - 1).Item("LanguageName") = r.Item("Language")
                dtLangauges.Rows(dtLangauges.Rows.Count - 1).Item("LanguageId") = r.Item("LanguageId")
                dtLangauges.Rows(dtLangauges.Rows.Count - 1).Item("ISOCode") = r.Item("ISOCode")
                dtLangauges.Rows(dtLangauges.Rows.Count - 1).Item("CountryFlagName") = r.Item("CountryFlagName")
            Next

        Catch ex As Exception

        End Try
    End Sub
    'Private Sub SetCaptionsLanguage()
    '    Try
    '        'Dim selectedLanguage As String = "EN"
    '        'Dim resourceFileName As String = $"Resources.{selectedLanguage}.resx"
    '        'Dim caption As String = GetGlobalResourceObject(resourceFileName, "rsCustomerName").ToString()

    '        Dim dt As DataTable = clsLanguage.Get_LanguageCaption(clsBranch.ReturnActiveBranchCodeState)
    '        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
    '            Dim dv As DataView = dt.DefaultView
    '            'Dim rf As Integer = clsLanguage.e_LanguageConstants.Tab1.GetHashCode
    '            'dv.RowFilter = "LanguageEnumCode = " & clsLanguage.e_LanguageConstants.Tab1.GetHashCode
    '            dt.Select("LanguageEnumCode = " & clsLanguage.e_LanguageConstants.Tab1.GetHashCode).ToString()
    '            Dim currentEnum As String = ""
    '            For i = 0 To dt.Rows.Count - 1
    '                currentEnum = dt.Rows(i).Item("LanguageEnumCode")
    '                Select Case currentEnum

    '                    Case clsLanguage.e_LanguageConstants.MyAccountbtn
    '                        hfMyAccountbtn.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.btnOpenQuotations
    '                        hfMyQuotations.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.btn_Feedback
    '                        hfbtn_Feedback.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.btn_ContactUs
    '                        hfbtn_ContactUs.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.btnUserG1
    '                        hfbtnUserG2.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.NewQuotation
    '                        hffontNewQ.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.btnloginUserName
    '                        hfLogIn.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.btnLogoutInput
    '                        hfLogOut.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.titleMyAccountDetails
    '                        hfMyAccountDetails.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.CustomerNumber
    '                        hfCustomerNumber.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.CustomerName
    '                        hfCustomerName.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.CustomerAddress
    '                        hfCustomerAddress.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.SubSidiary
    '                        hfSubSidiary.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.SubsidaryAddress
    '                        hfSubsidaryAddress.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.ContactDetails
    '                        hfContactDetails.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.PaymentTerms
    '                        hfPaymentTerms.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.ShippingMethod
    '                        hfShippingMethod.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.SalesPerson
    '                        hfSalesPerson.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.Email
    '                        hfEmail.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.DeskUser
    '                        HfDeskUser.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.TechnicalPerson
    '                        HfTechnicalPerson.Value = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)



    '                        'Case clsLanguage.e_LanguageConstants.btnloginUserName : btnloginUserNameVF.Text = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)
    '                        '  Case clsLanguage.e_LanguageConstants.Tab3 : lblParameters.Text = IIf(dt.Rows(i).Item("ControlCaptionTitleBranch").ToString <> "", dt.Rows(i).Item("ControlCaptionTitleBranch").ToString, dt.Rows(i).Item("ControlCaptionTitleEn").ToString)


    '                End Select
    '            Next

    '        End If


    '    Catch ex As Exception

    '    End Try
    'End Sub
    Private Sub SetCaptionsLanguage()
        Try
            'Dim selectedLanguage As String = "EN"
            'Dim resourceFileName As String = $"Resources.{selectedLanguage}.resx"
            'Dim caption As String = GetGlobalResourceObject(resourceFileName, "rsCustomerName").ToString()

            Dim dv As DataView = clsLanguage.Get_LanguageCaption("Main")
            If Not dv Is Nothing AndAlso dv.Count > 0 Then
                Dim currentEnum As String = ""
                For i = 0 To dv.Count - 1
                    currentEnum = dv.Item(i).Item("LanguageEnumCode")
                    Select Case currentEnum

                        Case clsLanguage.e_LanguageConstants.MyAccountbtn
                            hfMyAccountbtn.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.btnOpenQuotations
                            hfMyQuotations.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.btn_Feedback
                            hfbtn_Feedback.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.btn_ContactUs
                            hfbtn_ContactUs.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.btnUserG1
                            hfbtnUserG2.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.NewQuotation
                            hffontNewQ.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.btnloginUserName
                            hfLogIn.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.btnLogoutInput
                            hfLogOut.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.titleMyAccountDetails
                            hfMyAccountDetails.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.CustomerNumber
                            hfCustomerNumber.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.CustomerName
                            hfCustomerName.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.CustomerAddress
                            hfCustomerAddress.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.SubSidiary
                            hfSubSidiary.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.SubsidaryAddress
                            hfSubsidaryAddress.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.ContactDetails
                            hfContactDetails.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.PaymentTerms
                            hfPaymentTerms.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.ShippingMethod
                            hfShippingMethod.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.SalesPerson
                            hfSalesPerson.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.Email
                            hfEmail.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.DeskUser
                            HfDeskUser.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.TechnicalPerson
                            HfTechnicalPerson.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.Country
                            hftCountry.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.Company_Name
                            hftCompanyName.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.Message
                            hftMessage.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.Contact_Us
                            hftContactUs.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.toImproveTitle1
                            hftImprove.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.toImproveTitle2
                            hftrelevantRes.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.toImproveTitle3
                            hftiQuoteAdvisor.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.toImproveTitle4
                            hftiwouldyouimprove.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.buttonSubmit
                            hfbSubmit.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.buttonSend
                            hfbSend.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblTmyQut
                            lblTmyQut.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblTpleaseLogin
                            lblTpleaseLogin.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblTinsertoffer
                            lblTinsertoffer.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.btnFind
                            btnFind.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.buttonCancel
                            hfbCancel.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.hfFooter1
                            'hfFooter1.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            Footer1Ens.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.hfFooter2
                            Footer2Ens.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.hfFooter3
                            Footer3Ens.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.hfFooter4
                            Footer4Ens.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblPingCaption
                            hfPingCaption.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            'Case clsLanguage.e_LanguageConstants.btnloginUserName : btnloginUserNameVF.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            '  Case clsLanguage.e_LanguageConstants.Tab3 : lblParameters.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.hfClose
                            hfCloseMaster.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.LegalNotice
                            btninfoGud1.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.TermsOfUuse
                            btninfoGud2.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.TermsOfSale
                            btninfoGud3.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.PrivacyPolicy
                            btninfoGud4.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                    End Select
                Next

            End If


        Catch ex As Exception

        End Try
    End Sub

    'Private Sub rptLanguagesListCaptions_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rptLanguagesListCaptions.ItemCommand
    '    Try
    '        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_LanguageCaptions, CType(Nothing, DataTable))

    '        ' SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.qut_LanguageId, newLanguage)

    '        '  clsLanguage.Get_LanguageCaption()

    '    Catch ex As Exception

    '    End Try
    'End Sub


End Class