Imports System.IO
Imports IscarDal
Imports Microsoft.Reporting.WebForms
Imports SemiApp_bl




Public Class MainQuotationDataReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            'ddlBracnhCodes.Items.Add(New ListItem(BranchCode, BranchCode))
            'ddlBracnhCodes.SelectedValue = BranchCode


            txtQuotationNumber.Text = CryptoManager.Decode(Request("QuotationNum"))
            txtBranchCode.Text = CryptoManager.Decode(Request("BranchCode"))
            txtLanguageId.Text = CryptoManager.Decode(Request("LanguageId"))

            txtAS400_Line.Text = CryptoManager.Decode(Request("AS400_Line"))
            txtBranchNum.Text = CryptoManager.Decode(Request("BranchNum"))
            txtAS400Year.Text = CryptoManager.Decode(Request("AS400Year"))
            txtStorageFolder.Text = CryptoManager.Decode(Request("StorageFolder"))
            txtReportType.Text = CryptoManager.Decode(Request("ReportType"))
            txtQuotationDate.Text = CryptoManager.Decode(Request("QuotationDate"))
            txtDateCultureStartWith.Text = Request("DCultStartW")
            txtselectedLanguage.Text = CryptoManagerTDES.Decode(Request("selectedLanguage"))
            txtNumberSuparator.Text = Request("NSup")



            btnShow_Click(Nothing, Nothing)
        Catch ex As Exception
            ' GeneralException.WriteEventErrors("Page_Load: " & ex.Message, GeneralException.e_LogTitle.REPORT.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "MainQuotationDataReport Page_Load", ex.Message, txtBranchCode.Text, txtQuotationNumber.Text, "", "")

            Throw New GeneralException(ex.Message)
        End Try
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        Try
            Dim Params As New List(Of ReportParameter)

            'Dim ServerName As String
            'ServerName = Request.Url.Scheme & Uri.SchemeDelimiter & Request.Url.Host
            'If Request.Url.Port <> 80 Then
            '    ServerName &= ":" & Request.Url.Port
            'End If

            'ServerName &= "/"

            Params.Add(New ReportParameter("BranchCode", txtBranchCode.Text)) 'ddlBracnhCodes.SelectedValue
            Params.Add(New ReportParameter("QuotationNum", txtQuotationNumber.Text)) 'txtQuotationNumber.Text
            Params.Add(New ReportParameter("LanguageId", txtLanguageId.Text)) 'txtQuotationNumber.Text
            Params.Add(New ReportParameter("ReportType", txtReportType.Text))
            Params.Add(New ReportParameter("DateCultureStartWith", txtDateCultureStartWith.Text))
            Params.Add(New ReportParameter("NumberSuparator", txtNumberSuparator.Text))
            Params.Add(New ReportParameter("selectedLanguage", txtselectedLanguage.Text))

            Dim oSql As New IscarDal.SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()

            oParams.Add(New SqlParam("@LanguageID", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, Integer.Parse(txtLanguageId.Text)))
            'If txtselectedLanguage.Text.ToString = "" Then

            If txtBranchCode.Text = "ZZ" Then
                'If txtselectedLanguage.Text.ToString = "" Then
                '    oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, txtselectedLanguage.Text.ToString))
                'Else
                oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, txtBranchCode.Text))
                'End If
            Else
                oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, txtBranchCode.Text))
            End If

            'Else
            '    oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, txtselectedLanguage.Text.ToString))
            'End If

            Dim dt As DataTable = oSql.ExecuteSPReturnDT("USP_ReportMainData", oParams)

            Params.Add(New ReportParameter("TitleFontName", "Roboto"))
            Params.Add(New ReportParameter("FieldFontName", "Roboto Medium"))

            Params.Add(New ReportParameter("TitleFontSize", "18pt"))
            Params.Add(New ReportParameter("TitleFontSizeS", "16pt"))
            Params.Add(New ReportParameter("FieldTitleFontSize", "13pt"))
            Params.Add(New ReportParameter("FieldFontSize", "11pt"))
            Params.Add(New ReportParameter("TitleFontColor", "#1d5095")) ' "#1d5095"
            Params.Add(New ReportParameter("TitleBackColor", "#e5eaee")) '1d5095
            Params.Add(New ReportParameter("TitleFontBold", "Bold"))
            Params.Add(New ReportParameter("LineColor", "#fede15"))
            Params.Add(New ReportParameter("FieldsBackGroundColor", "#E5EAEF"))

            'Params.Add(New ReportParameter("TitleLeft1", dt.Rows(0)(clsLanguage.LanguageReportConstants.TitleLeft1.ToString).ToString))

            If txtBranchCode.Text = "ZZ" Then
                Params.Add(New ReportParameter("TitleLeft1", getParamVal(clsLanguage.e_LanguageReportConstants.TitleLeft1Technical.ToString, dt).ToString & " "))
            Else
                Params.Add(New ReportParameter("TitleLeft1", getParamVal(clsLanguage.e_LanguageReportConstants.TitleLeft1.ToString, dt).ToString & " "))
            End If


            Params.Add(New ReportParameter("TitleLeftOfferNumber", getParamVal(clsLanguage.e_LanguageReportConstants.TitleLeftOfferNumber.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleRightDate", getParamVal(clsLanguage.e_LanguageReportConstants.TitleRightDate.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleItemDescription", getParamVal(clsLanguage.e_LanguageReportConstants.TitleItemDescription.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleTechnicalOfferStatus", getParamVal(clsLanguage.e_LanguageReportConstants.TitleTechnicalOfferStatus.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleCustomerDetails", getParamVal(clsLanguage.e_LanguageReportConstants.TitleCustomerDetails.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleExpirdDate", getParamVal(clsLanguage.e_LanguageReportConstants.TitleExpirdDate.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleQUOTATION", getParamVal(clsLanguage.e_LanguageReportConstants.TitleQUOTATION.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleQuantity", getParamVal(clsLanguage.e_LanguageReportConstants.TitleQuantity.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleNETPrice", getParamVal(clsLanguage.e_LanguageReportConstants.TitleNETPrice.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleTotalPrice", getParamVal(clsLanguage.e_LanguageReportConstants.TitleTotalPrice.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleTechSpecification", getParamVal(clsLanguage.e_LanguageReportConstants.TitleTechSpecification.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleDeliveryTime", getParamVal(clsLanguage.e_LanguageReportConstants.TitleDeliveryTime.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitlePaymentTerms", getParamVal(clsLanguage.e_LanguageReportConstants.TitlePaymentTerms.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("Title3DModel", getParamVal(clsLanguage.e_LanguageReportConstants.Title3DModel.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleApprovalDrawing", getParamVal(clsLanguage.e_LanguageReportConstants.TitleApprovalDrawing.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleContactDetails", getParamVal(clsLanguage.e_LanguageReportConstants.TitleContactDetails.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleComments", getParamVal(clsLanguage.e_LanguageReportConstants.TitleComments.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleLeftQuotationNumber", getParamVal(clsLanguage.e_LanguageReportConstants.TitleLeftQuotationNumber.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleHeader", getParamVal(clsLanguage.e_LanguageReportConstants.TitleHeader.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleItemType", getParamVal(clsLanguage.e_LanguageReportConstants.TitleItemType.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleCustomerNumber", getParamVal(clsLanguage.e_LanguageReportConstants.TitleCustomerNumber.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleCustomeAdress", getParamVal(clsLanguage.e_LanguageReportConstants.TitleCustomeAdress.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitlePage", getParamVal(clsLanguage.e_LanguageReportConstants.TitlePage.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleSectionShipment", getParamVal(clsLanguage.e_LanguageReportConstants.TitleSectionShipment.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitlePayment", getParamVal(clsLanguage.e_LanguageReportConstants.TitlePayment.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleShipping", getParamVal(clsLanguage.e_LanguageReportConstants.TitleShipping.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleContactBranch", getParamVal(clsLanguage.e_LanguageReportConstants.TitleContactBranch.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleContactBranchAddress", getParamVal(clsLanguage.e_LanguageReportConstants.TitleContactBranchAddress.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleOfferBy", getParamVal(clsLanguage.e_LanguageReportConstants.TitleOfferBy.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleSectionApproval", getParamVal(clsLanguage.e_LanguageReportConstants.TitleSectionApproval.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleCompanyName", getParamVal(clsLanguage.e_LanguageReportConstants.TitleCompanyName.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleAuthorizedPerson", getParamVal(clsLanguage.e_LanguageReportConstants.TitleAuthorizedPerson.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleSectionApprovalDate", getParamVal(clsLanguage.e_LanguageReportConstants.TitleSectionApprovalDate.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleSectionApprovalSignature", getParamVal(clsLanguage.e_LanguageReportConstants.TitleSectionApprovalSignature.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleSectionApprovalCustomer", getParamVal(clsLanguage.e_LanguageReportConstants.TitleSectionApprovalCustomer.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleSectionApprovalSubsidiary", getParamVal(clsLanguage.e_LanguageReportConstants.TitleSectionApprovalSubsidiary.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleCustomerName", getParamVal(clsLanguage.e_LanguageReportConstants.TitleCustomerName.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleWeeks", getParamVal(clsLanguage.e_LanguageReportConstants.TitleWeeks.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleDays", getParamVal(clsLanguage.e_LanguageReportConstants.TitleDays.ToString, dt).ToString & " "))


            Params.Add(New ReportParameter("RemarkLine1", getParamVal(clsLanguage.e_LanguageReportConstants.RemarkLine1.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("RemarkLine2", getParamVal(clsLanguage.e_LanguageReportConstants.RemarkLine2.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("RemarkLine3", getParamVal(clsLanguage.e_LanguageReportConstants.RemarkLine3.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("RemarkLine4", getParamVal(clsLanguage.e_LanguageReportConstants.RemarkLine4.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("RemarkLine5", getParamVal(clsLanguage.e_LanguageReportConstants.RemarkLine5.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("RemarkLine6", getParamVal(clsLanguage.e_LanguageReportConstants.RemarkLine6.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("RemarkLine7", getParamVal(clsLanguage.e_LanguageReportConstants.RemarkLine7.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("RemarkLine8", getParamVal(clsLanguage.e_LanguageReportConstants.RemarkLine8.ToString, dt).ToString & " "))

            Params.Add(New ReportParameter("FooterLeftBranchName", getParamVal(clsLanguage.e_LanguageReportConstants.FooterLeftBranchName.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("FooterLeftBranchAddress1", getParamVal(clsLanguage.e_LanguageReportConstants.FooterLeftBranchAddress1.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("FooterLeftBranchAddress2", getParamVal(clsLanguage.e_LanguageReportConstants.FooterLeftBranchAddress2.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("FooterMiddleTel", getParamVal(clsLanguage.e_LanguageReportConstants.FooterMiddleTel.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("FooterMiddleFax", getParamVal(clsLanguage.e_LanguageReportConstants.FooterMiddleFax.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("FooterMiddleMail", getParamVal(clsLanguage.e_LanguageReportConstants.FooterMiddleMail.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("FooterRightAccounts", getParamVal(clsLanguage.e_LanguageReportConstants.FooterRightAccounts.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("FooterRightAccount1", getParamVal(clsLanguage.e_LanguageReportConstants.FooterRightAccount1.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("FooterRightAccount2", getParamVal(clsLanguage.e_LanguageReportConstants.FooterRightAccount2.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("FooterUnderImageLocation1", getParamVal(clsLanguage.e_LanguageReportConstants.FooterUnderImageLocation1.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("FooterUnderImageLocation2", getParamVal(clsLanguage.e_LanguageReportConstants.FooterUnderImageLocation2.ToString, dt).ToString & " "))

            Params.Add(New ReportParameter("TitleContactSalesPerson", getParamVal(clsLanguage.e_LanguageReportConstants.TitleContactSalesPerson.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleContactDeskUser", getParamVal(clsLanguage.e_LanguageReportConstants.TitleContactDeskUser.ToString, dt).ToString & " "))
            Params.Add(New ReportParameter("TitleContactTechnicalPerson", getParamVal(clsLanguage.e_LanguageReportConstants.TitleContactTechnicalPerson.ToString, dt).ToString & " "))
            'Params.Add(New ReportParameter("ServerName", ServerName))
            'Params.Add(New ReportParameter("LanguageId", txtLanguageId.Text))
            'Params.Add(New ReportParameter("ModelNum", txtModelNum.Text))


            ''Params.Add(New ReportParameter("Address2Caption", dt.Rows(0)(clsLanguage.LanguageConstants.Box_tefen).ToString))
            ''Params.Add(New ReportParameter("AddressZipCaption", dt.Rows(0)(clsLanguage.LanguageConstants.Israel).ToString))
            ''Params.Add(New ReportParameter("Address1Caption", dt.Rows(0)(clsLanguage.LanguageConstants.Marketing_Dept).ToString))
            ''Params.Add(New ReportParameter("QuotationNumCaption", dt.Rows(0)(clsLanguage.LanguageConstants.ISCAR_QUOTATION_NO).ToString))
            ''Params.Add(New ReportParameter("QuotationDateCaption", dt.Rows(0)(clsLanguage.LanguageConstants.Quotation_date).ToString))
            ''Params.Add(New ReportParameter("DescriptionCaption", dt.Rows(0)(clsLanguage.LanguageConstants.Tool_Description).ToString))

            ''Dim oParams1 As New SqlParams()
            ''oParams1.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, ddlBracnhCodes.SelectedValue, 2))
            ''oParams1.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, Integer.Parse(txtQuotationNumber.Text)))
            ''Dim dt1 As DataTable = oSql.ExecuteSPReturnDT("USP_ReportQuotationList", oParams1)
            ''Params.Add(New ReportParameter("QuotationDate", LocalizationManager.DateFormater(dt1.Rows(0)("QuotationDate"), "g")))

            ''Dim HiddenSkech As Boolean = False
            ''Dim HiddenParam As Boolean = False
            ''Dim HiddenPrice As Boolean = False

            ''If Trim(dt1.Rows(0)("DrawingNum").ToString) <> "" Then
            ''    HiddenSkech = True
            ''End If


            ''If txtReportType.Text = "PARAM" Then
            ''    HiddenPrice = True
            ''End If

            ''AS400Number = dt1.Rows(0)("AS400Number").ToString
            ''AS400RowNumber = dt1.Rows(0)("AS400RowNumber").ToString
            ''AS400Year = dt1.Rows(0)("AS400Year").ToString

            ''Params.Add(New ReportParameter("DrawingNum", HiddenSkech))
            ''Params.Add(New ReportParameter("AS400RowNumber", dt1.Rows(0)("AS400RowNumber").ToString))
            ''Params.Add(New ReportParameter("AS400Number", dt1.Rows(0)("AS400Number").ToString))
            ''Params.Add(New ReportParameter("Description", dt1.Rows(0)("ToolDescription").ToString))
            ''Params.Add(New ReportParameter("ReportType", HiddenParam))
            ''Params.Add(New ReportParameter("ReportTypeParam", HiddenPrice))
            ''Params.Add(New ReportParameter("CultureName", CryptoManager.Decode(Request("Culture"))))
            ''Params.Add(New ReportParameter("Approval", dt.Rows(0)(clsLanguage.LanguageConstants.Approval).ToString))
            ''Params.Add(New ReportParameter("Customer", dt.Rows(0)(clsLanguage.LanguageConstants.Customer2).ToString))
            ''Params.Add(New ReportParameter("Subsidiary", dt.Rows(0)(clsLanguage.LanguageConstants.Subsidiary).ToString))
            ''Params.Add(New ReportParameter("CompanyName", dt.Rows(0)(clsLanguage.LanguageConstants.Company_Name).ToString))
            ''Params.Add(New ReportParameter("AutorizedPersonName", dt.Rows(0)(clsLanguage.LanguageConstants.Autorized_person_name).ToString))
            ''Params.Add(New ReportParameter("DateWord", dt.Rows(0)(clsLanguage.LanguageConstants.Date4).ToString))
            ''Params.Add(New ReportParameter("Signature", dt.Rows(0)(clsLanguage.LanguageConstants.Signature1).ToString))
            ''Params.Add(New ReportParameter("OfWord", dt.Rows(0)(clsLanguage.LanguageConstants.[Of]).ToString))

            ''Dim Price_TYP As String = CryptoManager.Decode(Request("FileName"))
            ''If InStr(Price_TYP, "S_") > 0 Then
            ''    Price_TYP = "S" '' Purchase Price
            ''End If
            ''Params.Add(New ReportParameter("Price_TYP", Price_TYP))


            ReportViewer1.LocalReport.SetParameters(Params)
            ReportViewer1.LocalReport.Refresh()

            ReportViewer1.Visible = True

        Catch ex As Exception
            ' GeneralException.WriteEventErrors("btnShow_Click: " & ex.Message, GeneralException.e_LogTitle.REPORT.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "MainQuotationDataReport btnShow_Click", ex.Message, txtBranchCode.Text, txtQuotationNumber.Text, "", "")

            Throw New GeneralException(ex.Message)
        End Try
    End Sub
    Private Function getParamVal(ParamName As String, dt As DataTable) As String
        Try
            If Not dt.Columns(ParamName) Is Nothing Then
                Return dt.Rows(0)(ParamName).ToString.Trim
            Else
                Return "EMPTY"
            End If


        Catch ex As Exception
            'Return "EMPTY"
        End Try
        Return "EMPTY"
    End Function
    Private Sub MainQuotationDataReport_SaveStateComplete(sender As Object, e As EventArgs) Handles Me.SaveStateComplete
        Try

            Dim bytes As Byte()
            Dim mimeType As String = Nothing 'application/pdfF
            Dim encoding As String = Nothing + 9
            Dim extension As String = Nothing
            Dim warnings As Warning() = Nothing
            Dim streamids As String() = Nothing

            bytes = ReportViewer1.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)

            Dim FolderLocation As String = ConfigurationManager.AppSettings("ReportTempFolder") & CryptoManager.Decode(Request("FileName"))

            Dim fs As New FileStream(FolderLocation, FileMode.Create)
            fs.Write(bytes, 0, bytes.Length)
            fs.Close()

            Dim QuotationNumber As String = txtQuotationNumber.Text 'SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
            Dim BranchCode As String = txtBranchCode.Text 'SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)
            Dim AS400_Line As String = txtAS400_Line.Text ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400RowNumber, False)
            Dim qd As String = txtQuotationDate.Text
            'Dim AS400Year As String = txtAS400Year.Text ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Year, False)
            Dim StorageFolder As String = txtStorageFolder.Text

            Dim FolderPath As String = CryptoManager.Decode(Request("FolderPath")) ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FolderPath, False)

            Documents.UploadFileToDocuments(BranchCode, FolderPath, CryptoManager.Decode(Request("FileName")), ConfigurationManager.AppSettings("ReportTempFolder"), "Catia Drawing-MainQuotationDataReport", BranchCode & ConfigurationManager.AppSettings("DOCMNG_ExtentionFolder"))

        Catch ex As GeneralException
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "MainQuotationDataReport_SaveStateComplete", ex.Message, txtBranchCode.Text, txtQuotationNumber.Text, "", "")

            Throw
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.REPORT_ERROR.ToString, "MainQuotationDataReport_SaveStateComplete", ex.InnerException.Message.ToString, txtBranchCode.Text, txtQuotationNumber.Text, "", "")
            Throw New GeneralException(GeneralException.ErrorMessages.General_ErrMsg.ToString, ex, "GetActivities failed")
        End Try
    End Sub


End Class