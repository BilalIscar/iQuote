Imports System.Drawing
Imports System.IO
Imports System.Net
Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography
Imports System.Threading
Imports SemiApp_bl




Public Class Prices


    Inherits System.Web.UI.Page

    Private ReadOnly allowedExtensions As String() = {".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tif", ".tiff", ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".dwg", ".dxf"}


    Private _ModelNumModification As Integer
    Private _ModelNumConfiguration As Integer

    Private _ModelModification As DataTable
    Private _ModelConfiguration As DataTable

    Private _dtParamListModification As DataTable
    Private _dtParamListConfiguration As DataTable

    Private _BranchCode As String = ""
    Private _BranchNumber As String = ""
    Private _PriceFormula_MKT As String = ""
    Private _PriceFormulaTFR As String = ""
    Private _BranchPriceFormulaMKT As String = ""
    Private _BranchPriceFormulaTFR As String = ""
    Private _PriceCalculateFlag As String
    Private _Start As String = ""
    Private _QutNo As String = ""

    Private _fileFolderPath As String = ""
    Private _fileName As String = ""

    Private TemporarilyQuotation As Boolean
    Private ReportTabToShow As String

    Dim _sFileName As String

    Private FillDocumentDone As Boolean = False

    Private DeletedBuildreport As Boolean = False

    Private FillSelectedQtyDone As Boolean = False
    Private _SubmitEmail_1 As String = "Submit Quotation"
    Private _SubmitEmail_2 As String = "Email Quotation Details"
    Private _DocumentGridCo1FileType As String = "File Type"
    Private _DocumentGridCo1FileName As String = "File Name"
    Private _DocumentGridCo1FileDate As String = "File Date"
    Private _DocumentGridCo1FileSize As String = "File Size (KB)"

    Private _PricesGridCo1Quantity As String = "Quantity"
    Private _PricesGridCo1NetPrice As String = "Net Price"
    Private _PricesGridCo1TotalPrice As String = "ToTal Price"
    Private _Rweek As String = "Weeks"
    Private _QuatationDetails As String = "Quotation Details"
    Private _QuatationDetailsTemp As String = "Temporary Technical Offer"
    Private _QuatationDetailsValidTemp As String = "Valid for One Week"
    Private _doCheckLanguage As Boolean = False

    Private langRevisionDate As String = ""
    Private langQuantity As String = ""
    Private langNewNetPrice As String = ""
    Private langPrevNetPrice As String = ""

    Private landQuotationOrders
    Private landQuotationNumber
    Private landDate
    Private landPrice


    Private Enum E_PricesGrid As Integer
        Price_ID = 0
        btnPrice = 1
        'SELECTP = 2
        QTY = 2
        NETPRICE = 3
        TOTAL = 4
        COSTPRICE = 5
        GP = 6
        DELIVERYWEEKS = 7
        TFRPrice = 8
        btnAddToCart = 9
        btnDELETE = 10
        OrderedQuantity = 11
        QTYFct = 12

    End Enum

    Private Enum E_GridParamList As Integer

        SelectParameterIcon = 0
        TabIndex = 1
        Label = 2
        CostName = 3
        Measure = 4
        Order = 5
        Formula = 6
        Visible = 7
        PrevParam = 8

    End Enum

    Private Enum E_DocumentsGrid As Integer
        FileId = 0
        NewFile = 1
        FileType = 2
        FName = 3
        FilePath = 4
        FolderArr = 5
        Subject = 6
        FileDate = 7
        [Object] = 8
        SubjectId = 9
        FileSize = 10
        OpenFile = 11
        DeleteFile = 12
        'BranchCode = 13
    End Enum




    Private Function ReturnCurentDTparams() As DataTable
        Dim OpenType As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType)

        Dim _dtParamList As DataTable = Nothing
        If OpenType = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
            _dtParamList = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListModification, "")

        ElseIf OpenType = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
            _dtParamList = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListConfiguration, "")
        End If
        Return _dtParamList
    End Function

    Private Sub GetAccountDetails()
        Try
            Dim iiii As Int16 = 0

            lblCustomerNo.Text = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
            lblCustoName.Text = StateManager.GetValue(StateManager.Keys.s_CustomerName, True).ToString.Trim
            lblCustomerAddress.Text = StateManager.GetValue(StateManager.Keys.s_CustomerAddress, True).ToString.Trim
            lblPaymenttTerms.Text = StateManager.GetValue(StateManager.Keys.s_paymentTerms, True).ToString.Trim
            lblShippingMethod.Text = StateManager.GetValue(StateManager.Keys.s_shipmethod, True).ToString.Trim

            Dim dt As DataTable = clsBranch.GetBranchDetails(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False))
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                lblISCARSubsidiary.Text = dt.Rows(0).Item("BranchName").ToString
                lblSubsidiaryAddress.Text = dt.Rows(0).Item("Address1").ToString
            End If
            Dim sOtherDetails As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400quoteReturnedJSON, False)
            If Not sOtherDetails Is Nothing Then

            End If

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "GetAccountDetails", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub




    Private Sub GetBranchTFRCurrency()

        Try
            Dim cust As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True) ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.CustomerNumber, False)
            Dim branchCode As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)
            Dim dt As DataTable = clsBranch.GetBranchDetails(branchCode)

            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                If branchCode = "IS" Or branchCode = "XZ" Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchCurrency, dt.Rows(0).Item("MKTCurrency").ToString)
                    If Not StateManager.GetValue(StateManager.Keys.s_STNCustomerCurrency, True) Is Nothing AndAlso StateManager.GetValue(StateManager.Keys.s_STNCustomerCurrency, True) <> "" Then
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchCurrency, StateManager.GetValue(StateManager.Keys.s_STNCustomerCurrency, True))
                    End If
                Else
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchCurrency, dt.Rows(0).Item("TFRCurrency").ToString)

                End If
            End If

        Catch ex As Exception
            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub

    Private Sub StartrPriceProcessing()
        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-StartrPriceProcessing-" & i_counter)

        Try

            'GetCustomerMKTCurrency()
            GetBranchTFRCurrency()

            Price.Get_TFR_NET_Price(ConfigurationManager.AppSettings("BranchCodeAPITefen"))

            Price.Set_ParamsFactors()

            Dim dt_FamilyProperties As DataTable = clsPrice.GetFamilyProperties(CInt(clsQuatation.ACTIVE_ModelNumber))

            Price.SetSessionFormula(dt_FamilyProperties)
            Price.Get_PriceFormula(dt_FamilyProperties)
            '_PriceFormula = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormula, False)

            _PriceFormula_MKT = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormula_MKT, False)
            _PriceFormulaTFR = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormulaTFR, False)
            _BranchPriceFormulaMKT = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BranchPriceFormulaMKT, False)
            _BranchPriceFormulaTFR = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BranchPriceFormulaTFR, False)
            _PriceCalculateFlag = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceCalculateFlag, False)

            'Get_CostFormula(dt_FamilyProperties)
            'Get_GPFormula(dt_FamilyProperties)

            FormulaResult.GetDescriptionFormula(dt_FamilyProperties)

            'Price.GetDelivery_ValidTime(dt_FamilyProperties)
            'SetDeleviryValidTime(dt_FamilyProperties)

            Price.GetPriceRates()


            If CheckIfCanDoPriceByRate() <> True Then
                'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert5", "continueAlert();", True)
                ShowOooPsAlert()
            End If

        Catch ex As Exception
            'GeneralException.WriteEventErrors(ex.Message, GeneralException.e_LogTitle.PRICES.ToString)



            If ex.Message.ToString.ToUpper.Contains("CATALOG ERROR") Then
                GeneralException.BuildError(Page, "<b>Error Geting Items from Catalog!</b><br>Please start a new quotation.")
            Else
                GeneralException.BuildError(Page, ex.Message)
            End If
        End Try
        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-StartrPriceProcessing-" & i_counter)

    End Sub





    Private Sub SetStartedVariables()
        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-SetStartedVariables-" & i_counter)

        Try

            _QutNo = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False).ToString

            Dim sSc_3D As String = ConfigurationManager.AppSettings("TimeToWaitForBuildCATIA")
            hfSecondsAllCount.Value = sSc_3D

            Dim sQutStatus As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationStatus, False)


            If sQutStatus <> clsQuatation.e_QuotationStatus.New_Qut AndAlso sQutStatus <> clsQuatation.e_QuotationStatus.Exist_Qut_OpenedFromParameters Then
                'DisableForm()
                hfSecondsStartCount.Text = "0"
            Else
                Dim sTts As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationDateWithTime, False)
                If sTts = "" Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationDateWithTime, Now)
                    hfSecondsStartCount.Text = sSc_3D
                Else
                    Dim SecondsStartCount As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationDateWithTime, False)
                    Dim NowDatw As String = Now
                    If IsDate(SecondsStartCount) AndAlso (hfSecondsStartCount.Text.ToString = "" Or hfSecondsStartCount.Text.ToString = "0") Then
                        SecondsStartCount = CDate(SecondsStartCount).AddSeconds(sSc_3D)
                        Dim iSec As Integer = 0
                        Do Until CDate(NowDatw).AddSeconds(iSec) > CDate(SecondsStartCount) Or iSec > CInt(sSc_3D)
                            iSec += 1
                        Loop
                        hfSecondsStartCount.Text = iSec
                    End If
                End If
            End If


            'Dim sXd As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ExpiredDate, False)
            'If Not sXd Is Nothing AndAlso sXd <> "" AndAlso IsDate(sXd) Then
            '    If CDate(sXd) > Now.Date Then
            '        EnableForm()
            '    End If
            'End If

            TemporarilyQuotation = clsQuatation.IsTemporary_Quotatiom
            hfTemporaryQuotation.Value = TemporarilyQuotation

            'If TemporarilyQuotation = True Then
            'Else
            '    btnDefaultQuantityHided.Enabled = True
            '    btnDefaultQuantityHided.Visible = True
            '    btnDefaultQuantity.CssClass = "FontFamilyRoboto FontSizeRoboto13 QuotationDetailsButton"
            'End If

            _dtParamListModification = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListModification, "")
            _dtParamListConfiguration = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListConfiguration, "")
            '_PriceFormula = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormula, False)

            _PriceFormula_MKT = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormula_MKT, False)
            _PriceFormulaTFR = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormulaTFR, False)
            _BranchPriceFormulaMKT = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BranchPriceFormulaMKT, False)
            _BranchPriceFormulaTFR = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BranchPriceFormulaTFR, False)
            _PriceCalculateFlag = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceCalculateFlag, False)

            _BranchCode = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)

            If _Start = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then

                If IsNumeric(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumConfiguration, False)) Then
                    _ModelNumConfiguration = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumConfiguration, False)
                Else
                    Try

                        Response.Redirect("../Default.aspx?iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), False)
                    Catch ex As Exception
                        Response.Redirect("../Default.aspx", False)
                    End Try

                End If

                _ModelConfiguration = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelConfiguration, "")
            Else
                If IsNumeric(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumModification, False)) Then
                    _ModelNumModification = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumModification, False)
                    _ModelModification = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelModification, "")
                Else
                    Try
                        Response.Redirect("../Default.aspx?iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), False)
                    Catch ex As Exception
                        Response.Redirect("../Default.aspx", False)
                    End Try
                End If

                '_ModelNumModification = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumModification, False)
                '_ModelModification = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelModification, "")
            End If

            Dim dtParams As DataTable = Nothing
            If _Start = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                dtParams = _dtParamListConfiguration
            ElseIf _Start = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                dtParams = _dtParamListModification
            End If

            'Build2Dbson(False)

            If Not IsPostBack OrElse _doCheckLanguage = True Then
                SessionManager.Clear_Sessions_ForBeginSendMessages()
                FillListView()
                fill_ParamsGrid()
                'If gvDocuments.DataSource = Nothing Then
                'FillDocuments()

                'End If
            End If

            Dim TemporarilyS As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TemporarilyQuotationID, False)

            If Not IsPostBack OrElse _doCheckLanguage = True Then


                Dim sCustomer As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, False).ToString.Trim

                Dim gp As String = ""
                Try
                    gp = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempDontGetPrice, False)
                Catch ex As Exception
                    gp = "FALSE"
                End Try

                If gp = "TRUE" Then
                    Dim dt_Price As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "")
                    If Not dt_Price Is Nothing And (Not IsPostBack OrElse _doCheckLanguage = True) Then
                        FillPriceGrid()
                        'FillDocuments()
                    End If

                Else
                    'If _QutNo <> "" AndAlso TemporarilyL = True AndAlso lu <> "" AndAlso TemporarilyS <> "" AndAlso lu.Contains("@") AndAlso lu.Length > 3 AndAlso Not clsQuatation.IsTemporaryCustomer Then
                    'Dim sS4 As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number, False)
                    Dim sSq As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)

                    'If sS4 <> "0" AndAlso IsNumeric(sS4) AndAlso CInt(sS4) > 1 AndAlso sSq = sS4 AndAlso _QutNo <> "" AndAlso Not clsQuatation.IsTemporaryLoggedEmail AndAlso Not clsQuatation.IsTemporary_Quotatiom AndAlso Not clsQuatation.IsTemporaryCustomer AndAlso Not clsQuatation.IsTemporaryCustomerShowType Then
                    Dim sDoCon As String = ""
                    Try
                        sDoCon = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._TempDoConnect, False)

                    Catch ex As Exception

                    End Try
                    If sDoCon = "DOCONNECT_FROMLOGIN" Or (sDoCon = "DOCONNECT" AndAlso clsQuatation.IsTemporary_Quotatiom(False) = True AndAlso clsQuatation.IsTemporary_User(False, True) = False) Then
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempDoConnect, "")

                        Dim s As String = clsUpdateData.ConnectQuotation(TemporarilyS)


                        If s.ToString <> "" AndAlso s.ToString <> "0" Then


                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.selectedReportLanguage, CryptoManagerTDES.Encode(clsBranch.ReturnActiveBranchCodeState).ToString)

                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ShowQutPrice, "1")
                            Dim sOldQutNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerPrice, "")

                            Dim ShowGridPrice As String = ""
                            Try
                                If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ShowQutPrice, False).ToString Is Nothing Then
                                    ShowGridPrice = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ShowQutPrice, False).ToString
                                End If
                            Catch ex As Exception
                            End Try
                            SessionManager.SetSessionDetails_SEMI_ExistQuoatation(StateManager.GetValue(StateManager.Keys.s_BranchCode, True), s, clsQuatation.e_QuotationStatus.Exist_Qut_OpenedFromParameters, True)

                            '----------------
                            'hfSecondsAllCount.Value = ConfigurationManager.AppSettings("TimeToWaitForBuildCATIA")
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationDateWithTime, Now)
                            hfSecondsStartCount.Text = sSc_3D
                            '----------------

                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ShowQutPrice, ShowGridPrice)

                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempDontGetPrice, "FALSE")
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchPrice, "")

                            'EnableForm()

                            '-------------------
                            Dim sitemNumber As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber).ToString
                            Dim sLang As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.language).ToString
                            Dim svers As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.vers).ToString
                            Dim dt As DataTable = Nothing
                            Dim dv As DataView = SemiApp_bl.CatalogIscarData.GetItemParametersMobileISO(sitemNumber, svers, sLang, False)
                            dt = dv.ToTable
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamsFromCatalog, dt)
                            '-------------------

                            TemporarilyQuotation = False
                            _QutNo = s

                            'If clsQuatation.checkIfCanDoCalcPriceForActiveModel() = True Then
                            CalculateAndGetPrice()

                            clsUpdateData.UpdateQuotationDataAfter_Connect(TemporarilyQuotation)

                            '-------
                            Try
                                Dim sDOCMNG_ExtentionFolder = ConfigurationManager.AppSettings("DOCMNG_ExtentionFolder")
                                Dim abc As String = StateManager.GetValue(StateManager.Keys.s_BranchCode, True)
                                'Dim sAs400 As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number, False)
                                Dim FolderPAth As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FolderPath, False)


                                Try
                                    If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.REPORT_ID, False).ToString <> "" Then

                                        Dim fIdf As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.REPORT_ID, False).ToString

                                        Dim oldfp As String = FolderPAth.Substring(0, InStr(FolderPAth, "\")) & Format(CInt(sOldQutNo), "0000000") & "\001"
                                        Dim sO As String = clsBranch.ReturnActiveStorageFolderForDouuments
                                        sO = "ZZ" & sO.Substring(2)
                                        Dim Success As String = Documents.DocmngServiceAuthentication("DELETE", "", "", "ZZ", "REPORT_" & Format(CInt(sOldQutNo), "0000000") & ".pdf", oldfp, "", sO, "", "", "", "", "", "", fIdf, "DBO", Nothing, Nothing, Nothing)
                                    End If
                                Catch ex As Exception

                                End Try
                                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyReportBuild, "NO")
                                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.REPORT_ID, "")
                                Dim cSucc As Boolean = Documents.ConnectFiles("ZZ" & sDOCMNG_ExtentionFolder, sOldQutNo, abc & sDOCMNG_ExtentionFolder, "DBO", abc, "", FolderPAth)

                                '--------------------------
                                Try
                                    clsUpdateData.UpdateQuotationUploadFilesServiceResult(abc, _QutNo, "")
                                Catch ex As Exception

                                End Try
                                'BuildReportAuto(True)
                                'BuildBSONReportAuto()

                                'If ConfigurationManager.AppSettings("RunPDF2DWiththread") = "YES" Then




                                If ConfigurationManager.AppSettings("RunPDF2DWiththread") = "YES" Then
                                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempAllReadyTriedtoBuildDrawingAutomaticly, "YES4")
                                    txtAllReadyTriedtoBuildDrawing.Text = "YES4"

                                    Dim sclsth As New clsThreads
                                    sclsth.DoThreadReport(True)
                                    sclsth = Nothing
                                End If


                                If cSucc = False Then
                                    'GeneralException.WriteEventErrors("Could not connect technical quotation - " & _QutNo & " to new quotation - " & s, GeneralException.e_LogTitle.PRICES.ToString)
                                    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "SetStartedVariables", "Could not connect technical quotation - " & _QutNo & " to new quotation - " & s, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

                                    clsMail.SendEmailError("iQuote connect quotation error", "Could not connect technical quotation - " & _QutNo & " to new quotation - " & s, False, "Documents.ConnectFiles not success", clsBranch.ReturnActiveBranchCodeState)
                                End If

                                FillDocumentDone = False

                                If sDoCon = "DOCONNECT" Then
                                    SessionManager.SetSessionDetails_SEMI_ExistQuoatation(StateManager.GetValue(StateManager.Keys.s_BranchCode, True), s, clsQuatation.e_QuotationStatus.Exist_QutOpenedFromQuotationList, True)
                                End If

                            Catch ex As Exception

                                If sDoCon = "DOCONNECT" Then
                                    SessionManager.SetSessionDetails_SEMI_ExistQuoatation(StateManager.GetValue(StateManager.Keys.s_BranchCode, True), s, clsQuatation.e_QuotationStatus.Exist_QutOpenedFromQuotationList, True)
                                End If
                            End Try

                            '------
                        Else
                            GeneralException.BuildError(Page, "Error connect quotation")
                        End If
                    Else
                        Dim dt_Price As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "")
                        If dt_Price Is Nothing Then
                            CalculateAndGetPrice()
                        Else
                            If Not IsPostBack OrElse _doCheckLanguage = True Then
                                FillPriceGrid()
                            End If
                        End If
                    End If
                End If
            Else
                FillPriceGridTewp()
            End If

            _QutNo = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False).ToString

            If Not clsQuatation.IsTemporary_Quotatiom Then
                lblQutNumber.Text = clsQuatation.ACTIVE_QuotationNumber ' ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number, False)
            Else
                lblQutNumber.Text = TemporarilyS
            End If

            txtCurrency.Text = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TFR_STNBranchCurrency, False)
            lblItemDescription.Text = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.SemiToolDescription, False)
            lblCustomerCurrency.Text = StateManager.GetValue(StateManager.Keys.s_STNCustomerCurrency, True) ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerCurrency, False)
            LblCreateDate.Text = LocalizationManager.CulturingDate(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationDate, False).ToString)
            lblExpDate.Text = LocalizationManager.CulturingDate(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ExpiredDate, False).ToString)
            lblLastUpdateDate.Text = LocalizationManager.CulturingDate(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.LastUpdateDate, False).ToString)

            Dim devl As Integer = 0
            Dim sD1 As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Delivery_Weeks, False)
            Dim sD2 As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.DeliveryRad_Weeks, False)

            If IsNumeric(sD1.ToString) AndAlso IsNumeric(sD2.ToString) Then
                devl = CInt(sD1) + CInt(sD2)
                lblDelivery.Text = (devl.ToString & " Weeks").Replace("Weeks", _Rweek)
            ElseIf IsNumeric(sD1.ToString) Then
                devl = CInt(sD1)
                lblDelivery.Text = (devl.ToString & " Weeks").Replace("Weeks", _Rweek)
            ElseIf IsNumeric(sD2.ToString) Then
                devl = CInt(sD2)
                lblDelivery.Text = (devl.ToString & " Weeks").Replace("Weeks", _Rweek)
            End If


        Catch ex As Exception

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "SetStartedVariables", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        End Try
        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-SetStartedVariables-" & i_counter)

    End Sub

    Private Function GetCurrentPriceFormula() As String
        Try
            Dim sFlag As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceCalculateFlag, False)
            If sFlag = Price.e_PriceFlag.TFR.ToString Then
                Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormulaTFR, False)
            ElseIf sFlag = Price.e_PriceFlag.MKT.ToString Then
                Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormula_MKT, False)
            End If
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "GetCurrentPriceFormula", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Function
    Private Function GetCurrentPriceFormulaBranch() As String
        Try
            Dim sFlag As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceCalculateFlag, False)
            If sFlag = Price.e_PriceFlag.TFR.ToString Then
                Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BranchPriceFormulaTFR, False)
            ElseIf sFlag = Price.e_PriceFlag.MKT.ToString Then
                Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BranchPriceFormulaMKT, False)
            End If
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "GetCurrentPriceFormulaBranch", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Function
    Private Sub CalculateAndGetPrice()
        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-CalculateAndGetPrice-" & i_counter)

        Try
            If Not TemporarilyQuotation Then
                ' If clsBranch.ReturnActiveBranchCodeForDocuments <> "ZZ" Then
                If gvPrices.DataSource = Nothing Then
                    GetAccountDetails()
                    If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationStatus, False) <> clsQuatation.e_QuotationStatus.Exist_QutOpenedFromQuotationList Then

                        Dim gp As String = ""
                        Try
                            gp = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempDontGetPrice, False)
                        Catch ex As Exception
                            gp = "FALSE"
                        End Try

                        If gp.ToString.ToUpper <> "TRUE" Then
                            StartrPriceProcessing()
                            GetAutoStartPrices(clsQuatation.ACTIVE_ModelNumber)
                            SaveQuotation(False)
                        End If

                    Else
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationStatus, clsQuatation.e_QuotationStatus.Exist_Qut_OpenedFromParameters)
                    End If

                    FillPriceGrid()

                    If gvDocuments.DataSource Is Nothing Then
                        'FillDocuments()
                    End If
                Else
                    If Not clsQuatation.IsTemporary_User(True, True) Then
                        ShowOooPsAlert()
                        'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert6", "continueAlert();", True)
                    End If
                End If


            Else

                SaveQuotation(False)
                'FillDocuments()
                FillPriceGrid()
            End If
        Catch ex As Exception
            'GeneralException.WriteEventErrors(ex.Message, GeneralException.e_LogTitle.PRICES.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "CalculateAndGetPrice", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        End Try
        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-CalculateAndGetPrice-" & i_counter)

    End Sub

    Private Sub ShowOooPsAlert()
        Try
            Dim dOppsa As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._AllRedyOOppssAlertShows, False)
            If dOppsa = "NO" Then
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert5", "continueAlert();", True)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyOOppssAlertShows, "YES")
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub FillDocuments()

        If FillDocumentDone = True Then Exit Sub

        Try

            Dim dt As DataTable
            Dim FolderPAth As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FolderPath, False)
            If FolderPAth <> "" Then
                dt = Documents.GetDocumentsList(clsBranch.ReturnActiveBranchCodeForDocuments, FolderPAth, clsBranch.ReturnActiveStorageFolderForDouuments)

                If Not dt Is Nothing Then
                    Try

                        If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.REPORT_ID, False) = "" Then


                            For Each sdF As DataRow In dt.Rows
                                If sdF.Item("FileId").ToString <> "" AndAlso sdF.Item("FName").ToString.ToUpper.Contains("REPORT_") Then
                                    If _BranchCode = "ZZ" Then
                                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.REPORT_ID, sdF.Item("FileId").ToString)
                                    End If
                                    Exit For
                                End If
                            Next
                        End If
                    Catch ex As Exception

                    End Try

                    Dim dd1 As DataRow() = dt.Select("FName like 'DRW%' or FName like 'REPORT_%'", "CreateDate")


                    Dim dty As New DataTable
                    dty = dt.Clone()

                    For ii As Integer = 0 To dd1.Length - 1
                        dty.ImportRow(dd1(ii))
                    Next

                    If Not dty Is Nothing Then
                        dty.Columns.Add("Desc")
                    End If
                    '---------
                    Dim filesFound1 As Boolean = False
                    Dim filesFound2 As Boolean = False
                    For icG As Int16 = 0 To dty.Rows.Count - 1
                        If dty.Rows(icG).Item("FName").ToString.Trim.Contains("DRW_BSON") Then
                            filesFound2 = True
                        ElseIf dty.Rows(icG).Item("FName").ToString.Trim.Contains("DRW_") Then
                            filesFound1 = True
                        End If
                    Next


                    Dim qutStat As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationStatus, False)
                    If qutStat = clsQuatation.e_QuotationStatus.Exist_Qut_OpenedFromParameters AndAlso filesFound1 = False Then
                        Dim d1t As DataRow = dty.NewRow
                        d1t.Item("Desc") = "Technical Drawing"
                        d1t.Item("FileId") = "0"
                        dty.Rows.Add(d1t)
                    End If
                    If qutStat = clsQuatation.e_QuotationStatus.Exist_Qut_OpenedFromParameters AndAlso filesFound2 = False Then
                        Dim d2t As DataRow = dty.NewRow
                        d2t.Item("FileId") = "0"
                        d2t.Item("Desc") = "3D Model"
                        dty.Rows.Add(d2t)
                    End If
                    '---------

                    Dim dd12 As DataRow() = dt.Select("FName not like 'DRW%' and FName not like 'REPORT_%'", "CreateDate")

                    For ii As Integer = 0 To dd12.Length - 1
                        dty.ImportRow(dd12(ii))
                    Next
                    'Dim shfRepoprtsNames As String = ""
                    hfRepoprtsNames.Value = ""
                    hfFilesCoun.Value = 0

                    For Each erdf As DataRow In dty.Rows
                        If erdf.Item("FName").ToString.Contains("REPORT_") Then
                            erdf.Item("Desc") = "Quotation Report"
                            Try
                                hfRepoprtsNames.Value &= "_" & erdf.Item("Fname").ToString.Split("_")(2).Substring(0, 2) ' CryptoManagerTDES.Encode(erdf.Item("Subject").ToString.Substring(0, 2))
                            Catch ex As Exception

                            End Try

                        ElseIf erdf.Item("FName").ToString.Contains("DRW_BSON_") Then
                            erdf.Item("Desc") = "3D Model"
                        ElseIf erdf.Item("FName").ToString.Contains("DRW_") Then
                            erdf.Item("Desc") = "Technical Drawing"
                        ElseIf erdf.Item("FileId").ToString = "0" Then
                            erdf.Item("Desc") = erdf.Item("Desc").ToString
                        Else
                            erdf.Item("Desc") = "Personal"
                            Try
                                If hfFilesCoun.Value.ToString = "" OrElse Not IsNumeric(hfFilesCoun.Value.ToString) Then
                                    hfFilesCoun.Value = 1
                                Else
                                    hfFilesCoun.Value += 1
                                End If
                            Catch ex As Exception
                                hfFilesCoun.Value = 1
                            End Try

                        End If
                    Next




                    gvDocuments.DataSource = dty
                    gvDocuments.DataBind()

                    FillDocumentDone = True

                Else
                    'GeneralException.WriteEventLogReport("GetDocumentsList : END")
                End If

            End If

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES FillDocuments", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        End Try


        'Try
        '    SetCaptionsLanguageDocuments()
        'Catch ex As Exception

        'End Try
    End Sub


    Private Sub FillListView()
        Try

            Dim dP As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParametersListView, "")
            lvParams.DataSource = dP
            lvParams.DataBind()

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "FillListView", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub

    Protected Sub ExportToImage(sender As Object, e As EventArgs)
        'Dim base64 As String = Request.Form(hfImageData.UniqueID).Split(",")(1)
        'Dim bytes As Byte() = Convert.FromBase64String(base64)
        'Response.Clear()
        'Response.ContentType = "image/png"
        'Response.AddHeader("Content-Disposition", "attachment; filename=HTML.png")
        'Response.Buffer = True
        'Response.Cache.SetCacheability(HttpCacheability.NoCache)
        'Response.BinaryWrite(bytes)
        'Response.End()
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
        'Try
        '    If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BSONfolder, False) Is Nothing AndAlso SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BSONfolder, False).ToString <> "" Then
        '        BsonTextID.Text = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BSONfolder, False)
        '    End If
        'Catch ex As Exception

        'End Try
        Dim uplodedfiledone As Boolean = False
        'lblApprovalDocs.Text = ""
        Try

            Try
                _Start = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)
                hfSubmetted.Value = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Submitted, False)
                'If hfSubmetted.Value = "1" Then
                If hfSubmetted.Value = "1" OrElse clsBranch.ReturnActiveBranchCodeState.ToString.ToUpper = "ZZ" Then
                        hfSubmetted.Value = _SubmitEmail_2 ' "Email Quotation Details"
                    Else
                        hfSubmetted.Value = _SubmitEmail_1 ' "Submit Quotation"
                End If
            Catch ex As Exception
                _Start = ""
            End Try
            Try
                Dim atta As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.tempAttachOrderFile, False)
                hfSelectFile.Value = atta
            Catch ex As Exception

            End Try

            If Not IsPostBack OrElse _doCheckLanguage = True Then
                FillLanguagesPrice()
            End If

            If Idinty.CheckSesstionTimeOut = True Then
                If ConfigurationManager.AppSettings("IsDebugMode").ToString.ToUpper = "TRUE" Then
                    Response.Redirect("http://localhost:60377/Default.aspx?STARTFB=STARTFB_Y", True)
                ElseIf ConfigurationManager.AppSettings("IsDebugMode").ToString.ToUpper = "TEST" Then
                    Response.Redirect("http://dmstest/iQuote/Default.aspx?STARTFB=STARTFB_Y", True)
                Else
                    Response.Redirect("https://iquote.ssl.imc-companies.com/iQuote/Default.aspx?STARTFB=STARTFB_Y", True)
                End If

            Else

                DisableForm(True)

                gvPrices_Temp.DataSource = Nothing
                gvPrices_Temp.DataBind()

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagModificationCatalogDeferent, "NOTENDYET")

                Dim sPdfAlr As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._TempAllReadyTriedtoBuildDrawingAutomaticly, False)
                If sPdfAlr = "YES1" Or sPdfAlr = "YES4" Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempAllReadyTriedtoBuildDrawingAutomaticly, "YES2")
                    txtAllReadyTriedtoBuildDrawing.Text = "YES2"
                Else
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempAllReadyTriedtoBuildDrawingAutomaticly, "YES3")
                    txtAllReadyTriedtoBuildDrawing.Text = "YES3"
                End If

                txtDoDraw.Text = ConfigurationManager.AppSettings("BSON_ACTIVE")

                Try
                    If Not IsNumeric(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumModification, False)) Or
                        Not IsNumeric(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumConfiguration, False)) Then
                        Try
                            Response.Redirect("../Default.aspx?iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), False)
                        Catch ex As Exception
                            Response.Redirect("../Default.aspx", False)
                        End Try
                    End If

                Catch ex As Exception
                    Response.Redirect("../Default.aspx", True)
                End Try

                Try

                    'If Not IsPostBack OrElse _doCheckLanguage = True Then
                    '    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert7", "frametoshow('PDF');", True)
                    'End If

                    SetStartedVariables()

                    Dim seSt As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._AllRedyReportBuild, False)

                    If seSt = "NO" Then
                        FillDocumentDone = False
                        CreateRep(False, False)
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyReportBuild, "InPROCESS")

                    Else
                        If Not IsPostBack OrElse _doCheckLanguage = True Then
                            Dim QuotaionNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber).ToString
                            Dim bc As String = clsBranch.ReturnActiveBranchCodeForDocuments
                            clsUpdateData.UpdateQuotationUploadFilesServiceResult(bc, QuotaionNo, CryptoManagerTDES.Decode(utl.ReturnReportLanguage(Request("repLang"))))
                            FillDocuments()
                            tryBuidReport()


                        End If
                    End If

                    If Not IsPostBack OrElse _doCheckLanguage = True Then
                        Try
                            TabBSON.Enabled = False
                            Tab2.Enabled = False
                            Dim dt As DataTable = clsQuatation.ACTIVE_ModelDT
                            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                                If dt.Rows(0).Item("bDrawingEnable").ToString = "True" Then
                                    Tab2.Enabled = True
                                Else
                                    Tab2.Style.Add("cursor", "Default")
                                    Tab2.Style.Add("color", "lightgray")
                                End If
                                If dt.Rows(0).Item("bBsonEnable").ToString = "True" Then
                                    TabBSON.Enabled = True
                                Else
                                    TabBSON.Style.Add("cursor", "Default")
                                    TabBSON.Style.Add("cursor", "lightgray")
                                End If
                            End If
                        Catch ex As Exception

                        End Try

                    End If

                Catch ex As Exception

                End Try


                If IsPostBack Then

                    If Not IsPostBack OrElse _doCheckLanguage = True Then

                        Try
                            If SecurityManager.CheckIfUserNotAuthinticatedwithTheQuote("PRICE") = False Then
                                Response.Redirect("~/Default.aspx", True)
                            End If
                        Catch ex As Exception

                        End Try
                    End If

                    'If fuFile.HasFiles Then
                    Dim SLoad_log As String = ""
                    If Not fuFile.PostedFile Is Nothing AndAlso fuFile.FileName <> "" Then
                        SLoad_log = "Start Upload "
                        Dim doAllowToAddFiles As Boolean = True
                        Dim mftu As String = ConfigurationManager.AppSettings("MaximumFilesToUpload").ToString
                        Dim mfstu As String = ConfigurationManager.AppSettings("MaximumFilesSizeToUpload").ToString
                        If Not IsNumeric(mftu) Then
                            mftu = 0
                        End If
                        Dim iEachrCount As Integer = 0
                        If Not gvDocuments Is Nothing AndAlso gvDocuments.Rows.Count > 0 Then
                            For iEachr As Integer = 0 To gvDocuments.Rows.Count - 1
                                If gvDocuments.Rows(iEachr).Cells(E_DocumentsGrid.FileType).Text = "Personal" Then
                                    iEachrCount += 1
                                End If
                            Next
                        End If
                        If iEachrCount > mftu Then
                            doAllowToAddFiles = False

                        End If

                        SLoad_log &= " ////Start Upload doAllowToAddFiles = " & doAllowToAddFiles.ToString
                        If doAllowToAddFiles = True Then
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert30", "DisableblockUpdatProg();", True)
                            If Not String.IsNullOrEmpty(fuFile.PostedFile.FileName) Then
                                SLoad_log &= " ////fuFile.PostedFile.FileName = " & fuFile.PostedFile.FileName.ToString

                                '---------------------------------

                                ' Validate the file extension
                                If Not ValidateFileExtension(fuFile.PostedFile.FileName.ToString) Then
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertMax1", "UploadInvalidMIMEtype('This file cannot be added!');", True)
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertsssed", "DisableHideUpdatProg();", True)

                                Else
                                    Dim sanitizedFileName As String = SanitizeFileName(fuFile.PostedFile.FileName.ToString)

                                    ' Ensure a unique file name
                                    'Dim uniqueFileName As String = GetUniqueFileName(uploadDirectory, sanitizedFileName)


                                    Dim contentType As String = fuFile.PostedFile.ContentType
                                    SLoad_log &= " ////contentType= " & contentType.ToString

                                    ' Check if the MIME type is allowed
                                    If Documents.IsAllowedMimeType(contentType) Then
                                        SLoad_log &= " ////IsAllowedMimeType= True"
                                        '' Process the file upload
                                        'Dim fileName As String = Path.GetFileName(fuFile.FileName)
                                        'FileUploadControl.SaveAs(Server.MapPath("~/Uploads/") & fileName)
                                        'StatusLabel.Text = "Upload status: File uploaded successfully!"
                                        If fuFile.PostedFile.ContentLength > mfstu Then
                                            SLoad_log &= " //// 1 ContentLength= " & fuFile.PostedFile.ContentLength
                                            Try
                                                fuFile.Dispose()
                                                fuFile.PostedFile.InputStream.Dispose()
                                                fuFile = Nothing
                                            Catch ex As Exception

                                            End Try
                                            ' ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertMax", "ShowFileToLarge();", True)
                                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertMax1", "UploadInvalidMIMEtype('This file cannot be added!');", True)
                                            Try
                                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertsssd", "DisableHideUpdatProg();", True)
                                            Catch ex As Exception

                                            End Try



                                            'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "alertMax", "ShowFileToLarge()", True)
                                        Else
                                            SLoad_log &= " //// 2 ContentLength= " & fuFile.PostedFile.ContentLength
                                            'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert30", "DisableblockUpdatProg();", True)

                                            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.UPLOADFILE.ToString, "Upload File In Prices", SLoad_log, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)


                                            Try
                                                UploadFile(fuFile, "")
                                            Catch ex As Exception
                                                SLoad_log &= " //// 2 ContentLength= ex Message " & ex.Message.ToString
                                                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "Upload File In Prices", SLoad_log, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
                                            End Try

                                            SLoad_log &= " //// Page.Request.RawUrl " & Page.Request.RawUrl

                                            ' GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "Upload File In Prices", SLoad_log, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)


                                            Response.Redirect(Page.Request.RawUrl, False)
                                            Try
                                                fuFile.Dispose()
                                                fuFile.PostedFile.InputStream.Dispose()
                                                fuFile = Nothing
                                            Catch ex As Exception

                                            End Try
                                        End If

                                    Else
                                        SLoad_log &= " ////IsAllowedMimeType= True"
                                        'StatusLabel.Text = "Upload status: Invalid MIME type!"
                                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert20", "UploadInvalidMIMEtype('This file cannot be added!');", True)
                                    End If
                                    ''''''''''''''''''
                                    'SessionManager.Clear_Sessions_ForBeginSendMessages()
                                End If

                                '---------------------------------


                                ''''''''''''''''''

                            End If
                        Else
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert21", "UploadInvalidMIMEtype('Only 4 files can be ulpaoded!');", True)
                        End If
                    End If
                    'End If
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertdd", "DisableHideUpdatProg();", True)

                    Dim sD As String = "False" ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Ordered, False)
                    Dim rft As Boolean = False
                    If sD.ToString.ToUpper <> "TRUE" AndAlso sD.ToString.ToUpper <> "1" AndAlso UploadOrderFile(rft) = True Then
                        'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ManualydialogHidePricesOnlyCall", "ManualydialogHidePricesOnly();", True)
                        uplodedfiledone = True
                    End If
                    If rft = True Then
                        uplodedfiledone = True
                    End If

                End If

                Try
                    If BsonTextID.Text = "" Then
                        SetBSON_SRC_NAME()
                    End If
                Catch ex As Exception

                End Try


                ''''--------------14.09.23-----------------
                ''Try
                ''    Dim srcValue As String = ifif.Attributes("src")

                ''    If Not String.IsNullOrEmpty(srcValue) Then
                ''        Dim request As HttpWebRequest = CType(WebRequest.Create(srcValue), HttpWebRequest)
                ''        request.Method = WebRequestMethods.Http.Head

                ''        Try
                ''            Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                ''                If response.StatusCode = HttpStatusCode.OK Then
                ''                Else
                ''                End If
                ''            End Using
                ''        Catch ex As WebException

                ''        End Try
                ''    Else
                ''    End If
                ''Catch ex As Exception

                ''End Try
                ''''-------------------------------

                txtFlagPricesChanged.Text = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagPricesChanged, False).ToString.ToUpper

            End If




        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES LOAD", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
        End Try

        Try

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideshlogbuttonPricesCall", "hideshlogbuttonPrices();", True)

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ManualydialogHidePrices", "ManualydialogHidePrices();", True)
            If uplodedfiledone Then
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ManualydialogHidePricesOnlyCall", "ManualydialogHidePricesOnly();", True)
            End If
        Catch ex As Exception

        End Try
    End Sub





    Private Function ValidateFileExtension(fileName As String) As Boolean
        Dim fileExtension As String = Path.GetExtension(fileName).ToLower()
        Return allowedExtensions.Contains(fileExtension)
    End Function
    Private Function SanitizeFileName(fileName As String) As String
        ' Remove invalid characters
        Dim invalidChars As String = New String(Path.GetInvalidFileNameChars()) & New String(Path.GetInvalidPathChars())
        Dim regexSearch As String = New String(invalidChars)
        Dim r As New Regex($"[{Regex.Escape(regexSearch)}]")
        Dim sanitizedFileName As String = r.Replace(fileName, "_")

        ' Remove directory traversal sequences
        sanitizedFileName = sanitizedFileName.Replace("..", "").Replace("/", "").Replace("\", "")

        Return sanitizedFileName
    End Function

    Private Function GetUniqueFileName(directory As String, fileName As String) As String
        Dim uniqueFileName As String = fileName
        Dim filePath As String = Path.Combine(directory, uniqueFileName)

        ' Ensure the file name is unique
        Dim count As Integer = 1
        While File.Exists(filePath)
            Dim fileNameWithoutExtension As String = Path.GetFileNameWithoutExtension(fileName)
            Dim extension As String = Path.GetExtension(fileName)
            uniqueFileName = $"{fileNameWithoutExtension}_{count}{extension}"
            filePath = Path.Combine(directory, uniqueFileName)
            count += 1
        End While

        Return uniqueFileName
    End Function











    Private Sub tryBuidReport()

        Try
            Dim sfn As String = ""
            Dim fDf As String = ""
            If Not gvDocuments.Rows Is Nothing Then
                For Each r As GridViewRow In gvDocuments.Rows
                    Dim filesNAme As String = r.Cells(E_DocumentsGrid.FName).Text.Trim

                    If filesNAme.Contains("REPORT_") AndAlso filesNAme.Length > 7 Then
                        fDf &= filesNAme.ToUpper.Substring(filesNAme.Length - 7, 7)
                    End If
                Next

                If fDf <> "" Then

                    If Not Request("repLang") Is Nothing AndAlso Request("repLang") <> "" Then
                        Dim sISOnameReport As String = Request("repLang")
                        Dim dtRAB As DataTable = clsBranch.GetBranchLanguages(CryptoManagerTDES.Decode(sISOnameReport), sISOnameReport, "")
                        Dim isoRap As String = ""
                        Dim sSpn As String = "pnh0tFBOF/I="

                        If Not dtRAB Is Nothing AndAlso dtRAB.Rows.Count > 0 Then
                            If sISOnameReport = sSpn Then
                                For Each r1 As DataRow In dtRAB.Rows
                                    If r1.Item("BranchCode").ToString.Trim = "ZZ" Then
                                        isoRap &= r1.Item("ISOCode").ToString.Trim
                                        Exit For
                                    End If
                                Next
                            Else
                                isoRap &= dtRAB.Rows(0).Item("ISOCode").ToString
                            End If
                        End If
                        '------
                        isoRap = "_" & isoRap
                        If isoRap <> "" AndAlso isoRap.Contains("_") AndAlso isoRap.Length > 2 Then

                            If InStr(fDf, isoRap) = 0 Then

                                FillDocumentDone = False
                                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.selectedReportLanguage, Request("repLang"))
                                CreateRep(False, False)
                            End If

                        End If
                    End If


                End If


            End If


        Catch ex As Exception

        End Try
    End Sub

    Private Sub fill_ParamsGrid()
        Try
            Dim dtParamList As DataTable = Nothing

            If _Start = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                dtParamList = _dtParamListModification
            ElseIf _Start = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                dtParamList = _dtParamListConfiguration
            End If

            If Not dtParamList Is Nothing Then
                dgvParamList.DataSource = dtParamList
                dgvParamList.DataBind()
            End If

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES Get_ParamsGridVal", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Protected Sub dgvParamList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgvParamList.RowCommand
        Try

            If e.CommandName = "SelectParam" Then

                'dgvParamList.Rows(e.CommandArgument).Cells(GridParamList.PrevParam).Text

            End If
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES dgvParamList_RowCommand", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Protected Sub dgvParamList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgvParamList.RowDataBound

        Try
            'hide first empty line

            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(E_GridParamList.Measure).Text = LocalizationManager.CulturingNumber(e.Row.Cells(E_GridParamList.Measure).Text, False)

                If LocalizationManager.CulturingNumber(e.Row.Cells(E_GridParamList.Label).Text.ToString.Trim, False).Contains(e.Row.Cells(E_GridParamList.CostName).Text.ToString.Trim) Then
                    e.Row.Cells(E_GridParamList.CostName).Text = ""
                End If

            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                If (e.Row.RowState = DataControlRowState.Alternate) Then

                    e.Row.Attributes.Add("onmouseover", "lblTitle.BackColor='BLUE'")

                End If
            End If


        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES dgvParamList_RowDataBound", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        End Try
        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-dgvParamList_RowDataBound-" & i_counter)

    End Sub


    Private Sub SaveQuotation(ManualSave As Boolean)

        Try
            Dim DiSave As String = "True"

            Dim clsQuotation As New clsUpdateData

            Dim sStart As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)
            If sStart.ToString = "" Then sStart = "0"

            Dim QuotationNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)

            Dim clsUP As New clsUpdateData

            If DiSave = "True" Then
                clsUP.UpdateQuotation(CInt(sStart), QuotationNo, "", TemporarilyQuotation, CType(Nothing, Drawing.Image), CType(Nothing, Drawing.Image))
            End If

            clsUP = Nothing

            clsUpdateData.UpdateQuotatioListTemporary(TemporarilyQuotation)
            If DiSave = "True" Then
                clsUpdateData.UpdateQuotationPrices()
                clsUpdateData.UpdateQuotationFormula()
                clsUpdateData.UpdateQuotationFactorsQty()

                clsUpdateData.UpdateQuotationFactorsQuotationFactors()

                clsUpdateData.UpdateQuotatioListModelParametersCode()

                clsUpdateData.UpdateConstants()
            End If

            Dim bc As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)



            If Not clsQuatation.IsTemporary_Quotatiom Then
                Dim ssq As Boolean = False
                Dim qn As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
                Dim qnAS As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number, False)
                Dim sMsgN As String = ""

                Dim NewQuatationNum As String = ""
                Dim CanUpdateGal As Boolean = True
                ssq = clsUpdateData.UpdateQuotationGAL(qn, qnAS, bc, " SaveQuotation ", sMsgN, NewQuatationNum, CanUpdateGal)

                'If Not ssq Then
                '    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.AS400_ERROR.ToString, "SaveQuotation", "First Fail Update quotation AS400 e in SaveQuotation ## Error msg : "" ## " & sMsgN, clsBranch.ReturnActiveBranchCodeForDocuments, qn, NewQuatationNum, clsQuatation.ACTIVE_UseLoggedEmail.ToString)
                '    ssq = clsUpdateData.UpdateQuotationGAL(qn, qnAS, bc, " SaveQuotation ", sMsgN, NewQuatationNum)
                'End If
                'If Not ssq Then
                '    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.AS400_ERROR.ToString, "SaveQuotation", "Second Fail Update quotation AS400 e in SaveQuotation ## Error msg : "" ## " & sMsgN, clsBranch.ReturnActiveBranchCodeForDocuments, qn, NewQuatationNum, clsQuatation.ACTIVE_UseLoggedEmail.ToString)
                'End If

                If ssq = False Then
                    If IsNumeric(qn) AndAlso qn <> "0" Then
                    Else
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.AS400Number, QuotationNo)
                    End If
                End If
            End If



            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempAllReadyShowsTemporaryMessage, "DONE")

            Dim sss As String = ""
            Try
                sss = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempAllReadyShowsSaveMessage, False)
            Catch ex As Exception
            End Try
            If sss = "" AndAlso Not TemporarilyQuotation Then
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert10", "SavedToMyQuotation();", True)
            End If
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempAllReadyShowsSaveMessage, "DONE")

            clsQuotation = Nothing


        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES SaveQuotation", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        Finally
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagPricesChanged, False)
        End Try

    End Sub

    Private Sub SaveQuotationLocal()

        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempAllReadyShowsSaveMessage, "DONE")

        Try

            Try

                Dim QuotationNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)

                clsUpdateData.UpdateQuotationPrices()

                Dim bc As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)

                Dim qn As String = ""
                Dim ssq As Boolean = False
                Dim CanUpdateGal As Boolean = True

                If Not clsQuatation.IsTemporary_Quotatiom Then
                    Dim sMsgN As String = ""
                    Dim NewQuatationNum As String = ""
                    qn = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
                    Dim qnAS As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number, False)
                    ssq = clsUpdateData.UpdateQuotationGAL(qn, qnAS, bc, " SaveQuotationLocal ", sMsgN, NewQuatationNum, CanUpdateGal)

                    'If Not ssq Then
                    '    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.AS400_ERROR.ToString, "SaveQuotationLocal", "First Fail Update quotation AS400 e in SaveQuotationLocal ## Error msg : "" ## " & sMsgN, clsBranch.ReturnActiveBranchCodeForDocuments, qn, NewQuatationNum, clsQuatation.ACTIVE_UseLoggedEmail.ToString)
                    '    ssq = clsUpdateData.UpdateQuotationGAL(qn, qnAS, bc, " SaveQuotationLocal ", sMsgN, NewQuatationNum)
                    'End If
                    'If Not ssq Then
                    '    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.AS400_ERROR.ToString, "SaveQuotationLocal", "Second Fail Update quotation AS400 e in SaveQuotationLocal ## Error msg : "" ## " & sMsgN, clsBranch.ReturnActiveBranchCodeForDocuments, qn, NewQuatationNum, clsQuatation.ACTIVE_UseLoggedEmail.ToString)
                    'End If
                End If

                If ssq = False Then
                    If IsNumeric(qn) AndAlso qn <> "0" Then
                    Else
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.AS400Number, QuotationNo)
                    End If

                End If

                Dim sss As String = ""
                Try
                    sss = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempAllReadyShowsSaveMessage, False)
                Catch ex As Exception
                End Try
                If sss = "" Then
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert11", "SavedToMyQuotation();", True)
                End If
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempAllReadyShowsSaveMessage, "DONE")

            Catch ex As Exception
                GeneralException.BuildError(Page, ex.Message)
            End Try
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "SaveQuotationLocal", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub
    Private Sub btnQuotationList_Click(sender As Object, e As EventArgs) 'Handles btnQuotationList.Click
        Try
            Dim sBc As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)

            Try

                Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"
                Response.Redirect("QuotationsList.aspx" & uniqueID & "iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("iqlang")), False)
            Catch ex As Exception
                Response.Redirect("QuotationsList.aspx", False)
            End Try

        Catch ex As Exception
            'GeneralException.WriteEventErrors(ex.Message, GeneralException.e_LogTitle.PRICES.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "btnQuotationList_Click", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub



#Region "PRICES"

    Private Sub FillRevGrid(rev As Integer)
        Try

            If hfPricesVers.Value = "" Then
                Dim CountVerCurrent As Integer = 0
                Dim CountVerNext As Integer = 1
                Dim sD As String = ""
                hfPricesSelectedVers.Value = 1
                Dim dttrans As DataTable = clsQuatation.GeQuotationPricesTransaction(clsBranch.ReturnActiveBranchCodeForDocuments, clsQuatation.ACTIVE_iQuoteQuotationNumber, CInt(hfPricesSelectedVers.Value))
                hfPricesVers.Value = "#div style=' max-height: 300px; overflow: auto; text-align: center;padding-left: 9%;padding-right: 10px;padding-top: 30px;'$#table class='divVer'$"

                Dim doclosediv As Boolean = False
                Dim MaxId As String = 0
                If Not dttrans Is Nothing AndAlso dttrans.Rows.Count > 0 Then

                    MaxId = dttrans.Rows(0).Item("PriceRevisionS").ToString
                End If


                If Not dttrans Is Nothing AndAlso dttrans.Rows.Count > 0 AndAlso MaxId <> "" AndAlso IsNumeric(MaxId) Then
                    For Each sDpv As DataRow In dttrans.Rows
                        CountVerNext = sDpv.Item("PriceRevisionS").ToString
                        If CountVerCurrent <> CountVerNext Then
                            sD = ""
                            For Each sDpv1 As DataRow In dttrans.Rows
                                If sDpv1.Item("NetPriceRevision").ToString.Trim <> "" AndAlso sDpv1.Item("PriceRevisionS").ToString = CountVerNext Then
                                    If (sD = "") Then
                                        sD = ClsDate.GetDateTimeReturnStringFormat(sDpv1.Item("LastUpdate"), ClsDate.Enum_DateFormatTypes._ddmmyy)
                                    ElseIf IsDate(sD) AndAlso (ClsDate.GetDateTimeReturnStringFormat(sDpv1.Item("LastUpdate"), ClsDate.Enum_DateFormatTypes._ddmmyy) > CDate(sD)) Then
                                        sD = ClsDate.GetDateTimeReturnStringFormat(sDpv1.Item("LastUpdate"), ClsDate.Enum_DateFormatTypes._ddmmyy)
                                    End If
                                End If
                            Next
                            If doclosediv = True Then
                                hfPricesVers.Value &= "#/div$"
                                doclosediv = False
                            End If

                            If CountVerNext = MaxId Then '1 Then
                                hfPricesVers.Value &= "#div style='border: solid; border-width: thin; border-color: *e9eaec !important; padding-left: 1% !important; text-align: left; color: *1d5095;padding-top: 4px; width:340px ' class='FontFamilyRoboto FontSizeRoboto15 '$#b$ " & langRevisionDate & ": " & sD & "#/b$#image style='cursor: pointer;float: right; width: 20px; padding-top: 6px;margin-right: 4px;' id='imgOldPriceRev" & CountVerNext & "' src='../media/Icons/arrowdown.png' onclick=SetDivHistory(" & CountVerNext & "," & MaxId & ") $#/image $ #/div$"
                            Else
                                hfPricesVers.Value &= "#div style='border: solid; border-width: thin; border-color: *e9eaec !important; padding-left: 1% !important; text-align: left; color: *1d5095;padding-top: 4px; width:340px ' class='FontFamilyRoboto FontSizeRoboto15 '$#b$ " & langRevisionDate & ": " & sD & "#/b$#image style='cursor: pointer;float: right; width: 20px; padding-top: 6px;margin-right: 4px;' id='imgOldPriceRev" & CountVerNext & "' src='../media/Icons/arrowup.png' onclick=SetDivHistory(" & CountVerNext & "," & MaxId & ") $#/image $ #/div$"
                            End If

                            Dim divOldPriceRev As String = "divOldPriceRev" & CountVerNext
                            If CountVerNext = MaxId Then '1 Then
                                hfPricesVers.Value &= "#div id=" & divOldPriceRev & " style=' width:340px; '$"
                            Else
                                hfPricesVers.Value &= "#div id=" & divOldPriceRev & " style=' width:340px;display: none '$"
                            End If

                            hfPricesVers.Value &= "#div style=' width:340px; background-color: *e9eaec; !important;display: inline-flex;' class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$"
                            hfPricesVers.Value &= "#div style=' width:68px;'  class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$#b$ " & langQuantity & "#/b$#/div$"
                            hfPricesVers.Value &= "#div style=' width:32px;'  class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$#/div$"
                            hfPricesVers.Value &= "#div style=' width:101px;'  class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$#b$ " & langNewNetPrice & "#/b$#/div$"
                            hfPricesVers.Value &= "#div style=' width:32px;'  class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$#/div$"
                            hfPricesVers.Value &= "#div style=' width:107px;color:gray'  class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$ " & langPrevNetPrice & "#/div$"
                            hfPricesVers.Value &= "#/div$"
                            CountVerCurrent = CountVerNext
                            doclosediv = True
                        End If
                        hfPricesVers.Value &= "#div style=' width:340px; background-color: *e9eaec; !important;display: inline-flex;' class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$"

                        If sDpv.Item("NetPriceRevision").ToString <> "" Then
                            hfPricesVers.Value &= "#div style=' width:68px; background-color: *e9eaec; !important;' class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$" & sDpv.Item("Qty").ToString & "#/div$"
                            hfPricesVers.Value &= "#div style=' width:32px;'  class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$#/div$"
                            hfPricesVers.Value &= "#div style=' width:101px; background-color: *e9eaec; !important;' class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$" & sDpv.Item("NetPrice").ToString & "#/div$"
                            hfPricesVers.Value &= "#div style=' width:32px;'  class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$#/div$"
                            hfPricesVers.Value &= "#div style=' width:107px;color:gray; background-color: *e9eaec; !important;' class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$&nbsp;" & sDpv.Item("NetPriceRevision").ToString & "#/div$"
                        Else
                            hfPricesVers.Value &= "#div style=' width:68px; background-color: *e9eaec; !important;' class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$" & sDpv.Item("Qty").ToString & "#/div$"
                            hfPricesVers.Value &= "#div style=' width:32px;'  class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$#/div$"
                            hfPricesVers.Value &= "#div style=' width:101px; background-color: *e9eaec; !important;' class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$#/div$"
                            hfPricesVers.Value &= "#div style=' width:32px;'  class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$#/div$"
                            hfPricesVers.Value &= "#div style=' width:107px;color:gray; background-color: *e9eaec; !important;' class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$&nbsp;" & sDpv.Item("NetPrice").ToString & "#/div$"
                        End If
                        hfPricesVers.Value &= "#/div$"

                    Next

                    If doclosediv = True Then
                        hfPricesVers.Value &= "#/div$"
                        doclosediv = False

                    End If
                    'hfPricesVers.Value &= "#/table*#/div%"

                Else
                    pnlViewHistory.Visible = False
                End If

            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub FillOrderedGrid()
        Try

            Dim Ordered As Boolean = True
            Dim sOrdered As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Ordered, False)
            If sOrdered <> "1" AndAlso sOrdered.ToUpper <> "TRUE" Then
                Ordered = False
            End If

            If Ordered = True Then


                If hfPriceOrdered.Value = "" Then

                    Dim dttOrdered As DataTable = clsQuatation.GeQuotationPricesOrdered(clsBranch.ReturnActiveBranchCodeForDocuments, clsQuatation.ACTIVE_iQuoteQuotationNumber)



                    hfPriceOrdered.Value = "#div style=' max-height: 300px; overflow: auto; text-align: center;padding-left: 2%;padding-right: 10px;padding-top: 30px;'$#table class='divVer'$"

                    Dim qn As String = clsQuatation.ACTIVE_GALQuotationNumber
                    If Not dttOrdered Is Nothing AndAlso dttOrdered.Rows.Count > 0 Then
                        hfPriceOrdered.Value &= "#div style=' width:340px; display: inline-flex;' class=''$"
                        hfPriceOrdered.Value &= "#div style='border-style:solid' width:100%;'  class='FontSizeRoboto20 MainSubTitle FontFamily BorderNone '$" & landQuotationOrders & "#/div$"
                        hfPriceOrdered.Value &= "#/div$"
                        hfPriceOrdered.Value &= "#div style=' width:340px; display: inline-flex;' class=''$"
                        hfPriceOrdered.Value &= "#div style='border-style:solid' width:100%;'  class='MainSubTitle FontFamilyRoboto BorderNone FontSizeRoboto14 '$" & landQuotationNumber & "" & qn & "#/div$"
                        hfPriceOrdered.Value &= "#/div$"
                        hfPriceOrdered.Value &= "#div style=' width:340px ;display: inline-flex;' class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$"
                        hfPriceOrdered.Value &= "#div style=' width:200px;text-align: left;'  class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$#b$" & langQuantity & "#/b$#/div$"
                        hfPriceOrdered.Value &= "#div style=' width:32px;'  class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$#/div$"
                        hfPriceOrdered.Value &= "#div style=' width:200px;text-align: left;'  class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$#b$" & landDate & "#/b$#/div$"
                        hfPriceOrdered.Value &= "#div style=' width:32px;'  class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$#/div$"
                        hfPriceOrdered.Value &= "#div style=' width:200px;text-align: left;'  class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$#b$" & landPrice & "#/b$#/div$"
                        hfPriceOrdered.Value &= "#/div$"

                        hfPricesVers.Value &= "#div style=' width:340px; display: inline-flex;' class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$"
                        For Each sDpv As DataRow In dttOrdered.Rows

                            'hfPriceOrdered.Value &= "#div style=' width:100%; background-color: *e9eaec; !important;display: inline-flex;' class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$"
                            hfPriceOrdered.Value &= "#div style=' width:340px;display: inline-flex;padding-top: 4px' class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$"
                            hfPriceOrdered.Value &= "#div style=' width:200px;text-align: left;padding-top: 4px' class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$" & sDpv.Item("OrderedQTY").ToString & "#/div$"
                            hfPriceOrdered.Value &= "#div style=' width:32px;padding-top: 4px'  class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$#/div$"
                            hfPriceOrdered.Value &= "#div style=' width:200px;text-align: left;padding-top: 4px' class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$" & ClsDate.GetDateTimeReturnStringFormat(sDpv.Item("OrderedDate").ToString, ClsDate.Enum_DateFormatTypes._ddmmyyyy).Replace("/", ".").Replace("\", ".") & "#/div$"
                            hfPriceOrdered.Value &= "#div style=' width:32px;padding-top: 4px'  class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$#/div$"
                            hfPriceOrdered.Value &= "#div style=' width:200px;text-align: left;padding-top: 4px' class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$" & sDpv.Item("NetPrice").ToString & "#/div$"
                            'hfPriceOrdered.Value &= "#div style=' width:32px;'  class='FontFamilyRoboto FontSizeRoboto14 BorderNone '$#/div$"

                            hfPriceOrdered.Value &= "#/div$"

                        Next

                        hfPriceOrdered.Value &= "#/div$"

                    Else
                        pnlViewOrdered.Visible = False
                    End If

                End If
            End If




        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillPriceGrid()
        Try

            Dim dt_Price As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "")


            Dim ShowGridPrice As String = "0"
            Try
                If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ShowQutPrice, False).ToString Is Nothing Then
                    ShowGridPrice = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ShowQutPrice, False).ToString
                End If
            Catch ex As Exception

            End Try


            If Not dt_Price Is Nothing AndAlso dt_Price.Rows.Count > 0 AndAlso ShowGridPrice = "1" Then
                If dt_Price.Rows(dt_Price.Rows.Count - 1).Item(E_PricesGrid.QTY).ToString.Trim <> "" AndAlso IsNumeric(dt_Price.Rows(dt_Price.Rows.Count - 1).Item(E_PricesGrid.QTY).ToString.Trim) Then
                    dt_Price.Rows.Add()
                End If
                gvPrices.DataSource = dt_Price
                gvPrices.DataBind()

            Else

                Dim dt_Price_Temp As New DataTable
                dt_Price_Temp.Columns.Add("txtQuantityTemp")
                dt_Price_Temp.Columns.Add("txtNetPriceTemp")
                dt_Price_Temp.Columns.Add("txtTotalTemp")
                dt_Price_Temp.Rows.Add()
                dt_Price_Temp.Rows.Add()
                dt_Price_Temp.Rows.Add()
                dt_Price_Temp.Rows.Add()
                dt_Price_Temp.Rows.Add()
                dt_Price_Temp.Rows.Add()
                dt_Price_Temp.Rows.Add()
                dt_Price_Temp.Rows.Add()
                dt_Price_Temp.Rows.Add()

                gvPrices_Temp.DataSource = dt_Price_Temp
                gvPrices_Temp.DataBind()
                lblPriceTempAlert.Visible = True
            End If

            FillSelectQuantity()

        Catch ex As Exception
            FillSelectQuantity()
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES FillPriceGrid", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub

    Private Sub FillPriceGridTewp()
        Try

            Dim dt_Price As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "")


            If dt_Price Is Nothing Then
                Dim dt_Price_Temp As New DataTable
                dt_Price_Temp.Columns.Add("txtQuantityTemp")
                dt_Price_Temp.Columns.Add("txtNetPriceTemp")
                dt_Price_Temp.Columns.Add("txtTotalTemp")
                dt_Price_Temp.Rows.Add()
                dt_Price_Temp.Rows.Add()
                dt_Price_Temp.Rows.Add()
                dt_Price_Temp.Rows.Add()
                dt_Price_Temp.Rows.Add()
                dt_Price_Temp.Rows.Add()
                dt_Price_Temp.Rows.Add()
                dt_Price_Temp.Rows.Add()
                dt_Price_Temp.Rows.Add()
                gvPrices_Temp.DataSource = dt_Price_Temp
                gvPrices_Temp.DataBind()
                lblPriceTempAlert.Visible = True
            End If


        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES FillPriceGridTewp", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub SetQuantityPrices(iRow As Integer, AutoQty As String)
        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-SetQuantityPrices-" & i_counter)

        Dim dtPrice As New DataTable
        Dim Formula_Error As String = ""
        Dim FormulaBranch_Error As String = ""
        Try

            If clsQuatation.checkIfCanDoCalcPriceForActiveModel() = False Then
                Exit Sub
            End If
            If AutoQty <> "" Then
                GetDefaultPrices(dtPrice)


                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Price, dtPrice)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_PriceTempForCalcDefaultPrices, dtPrice)

            Else
                Dim dtP As New DataTable
                If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_PriceTempForCalcDefaultPrices, "") Is Nothing Then

                    GetDefaultPrices(dtP)
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_PriceTempForCalcDefaultPrices, dtP)
                ElseIf SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_PriceTempForCalcDefaultPrices, "").Rows.Count = 0 Then

                    GetDefaultPrices(dtP)
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_PriceTempForCalcDefaultPrices, dtP)
                Else
                    dtP = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_PriceTempForCalcDefaultPrices, "")
                End If


                If iRow = -1 Then
                    If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "") Is Nothing Then
                        dtPrice.Columns.Add(E_PricesGrid.btnPrice.ToString)
                        dtPrice.Columns.Add(E_PricesGrid.Price_ID.ToString, GetType(System.Int32))
                        'dtPrice.Columns.Add(e_PricesGrid.SELECTP.ToString)
                        dtPrice.Columns.Add(E_PricesGrid.QTY.ToString, GetType(System.Int16))
                        dtPrice.Columns.Add(E_PricesGrid.NETPRICE.ToString, GetType(System.Decimal))
                        dtPrice.Columns.Add(E_PricesGrid.TOTAL.ToString, GetType(System.Decimal))
                        dtPrice.Columns.Add(E_PricesGrid.COSTPRICE.ToString, GetType(System.Decimal))
                        dtPrice.Columns.Add(E_PricesGrid.GP.ToString, GetType(System.Decimal))
                        dtPrice.Columns.Add(E_PricesGrid.DELIVERYWEEKS.ToString, GetType(System.Int16))
                        dtPrice.Columns.Add(E_PricesGrid.TFRPrice.ToString, GetType(System.Decimal))
                        dtPrice.Columns.Add(E_PricesGrid.btnAddToCart.ToString)
                        dtPrice.Columns.Add(E_PricesGrid.btnDELETE.ToString)
                        dtPrice.Columns.Add(E_PricesGrid.OrderedQuantity.ToString)
                        dtPrice.Columns.Add(E_PricesGrid.QTYFct.ToString, GetType(System.Decimal))

                        dtPrice.Rows.Add()

                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Price, dtPrice)


                    End If
                    Exit Sub
                End If

                dtPrice = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "")

                Try

                    Dim sQty As String = ""

                    Dim dtAQ As DataTable = clsPrices.GetPricesQty(clsQuatation.ACTIVE_ModelNumber)
                    If Not dtAQ Is Nothing AndAlso dtAQ.Rows.Count > 0 Then
                        sQty = dtAQ.Rows(0).Item("ModelQty").ToString
                    End If

                    If CType(gvPrices.Rows(iRow).FindControl("txtQuantity"), TextBox).Text <> "" AndAlso IsNumeric(CType(gvPrices.Rows(iRow).FindControl("txtQuantity"), TextBox).Text) Then

                        Dim myQty As Integer = CType(gvPrices.Rows(iRow).FindControl("txtQuantity"), TextBox).Text
                        Dim minQutN As Integer = 1000000

                        Dim sAllR As String = ""
                        For Each rr As DataRow In dtPrice.Rows
                            sAllR &= ";" & rr.Item(E_PricesGrid.QTY.ToString)
                        Next
                        If sAllR.Contains(";" & myQty & ";") Then
                            'GeneralException.BuildError(Page, "Quantity already selected")
                            ValidationSummaryLabel.Visible = True
                            ValidationSummaryLabel.Text = "Quantity already selected"

                            Exit Sub
                        End If



                        If Not hfMinQuantity.Value Is Nothing AndAlso hfMinQuantity.Value <> "" Then
                            Dim ss() As String = hfMinQuantity.Value.ToString.Split(";")
                            For ii As Integer = 0 To ss.Length - 1
                                If ss(ii) <> "" Then
                                    If ss(ii) < minQutN Then
                                        minQutN = ss(ii)
                                    End If
                                End If

                            Next
                        Else
                            Dim dtPq As DataTable = clsPrices.GetPricesQty(clsQuatation.ACTIVE_ModelNumber)
                            If Not dtPq Is Nothing AndAlso dtPq.Rows.Count > 0 Then
                                hfMinQuantity.Value = dtPq.Rows(0).Item("ModelQty").ToString
                            End If
                            Dim ss() As String = hfMinQuantity.Value.ToString.Split(";")
                            For ii As Integer = 0 To ss.Length - 1
                                If ss(ii) <> "" Then
                                    If ss(ii) < minQutN Then
                                        minQutN = ss(ii)
                                    End If
                                End If

                            Next
                        End If


                        If myQty < minQutN Then
                            GeneralException.BuildError(Page, "Please choose a higher quantity")
                            Exit Sub
                        End If


                        If sQty.Contains(CType(gvPrices.Rows(iRow).FindControl("txtQuantity"), TextBox).Text & ";") Then
                            Dim m As String = ""
                            Dim sStart As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)

                            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                m = _ModelNumConfiguration
                            ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                                m = _ModelNumModification
                            End If
                            Dim dt As DataTable = ReturnCurentDTparams()
                            Dim ooFormula As New FormulaResult(GetCurrentPriceFormula, dt, 0, Nothing)
                            Dim ooFormulaB As New FormulaResult(GetCurrentPriceFormulaBranch, dt, 0, Nothing)
                            Formula_Error = ooFormula.Formula.ToString
                            ooFormula.ModelNum = m
                            ooFormula.Quantity = CType(gvPrices.Rows(iRow).FindControl("txtQuantity"), TextBox).Text
                            Dim s As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
                            ooFormula.CustomerNumber = s
                            Dim res As String = ""
                            Dim resB As String = ""
                            res = ooFormula.ParseAndCalculate

                            If res = "" Then res = 0
                            If IsNumeric(res) Then
                                res = Format(CDbl(res), "#.##")
                            End If

                            dtPrice.Rows(iRow).Item(E_PricesGrid.QTY) = CType(gvPrices.Rows(iRow).FindControl("txtQuantity"), TextBox).Text
                            dtPrice.Rows(iRow).Item(E_PricesGrid.NETPRICE) = res
                            dtPrice.Rows(iRow).Item(E_PricesGrid.COSTPRICE) = SetCostPrice(CType(gvPrices.Rows(iRow).FindControl("txtQuantity"), TextBox).Text, res)
                            If IsNumeric(res) AndAlso IsNumeric(dtPrice.Rows(iRow).Item(E_PricesGrid.QTY)) Then
                                Dim sa As String = Format(CDbl(res) * CDbl(dtPrice.Rows(iRow).Item(E_PricesGrid.QTY)), "#.##")
                                If IsNumeric(sa) Then
                                    dtPrice.Rows(iRow).Item(E_PricesGrid.TOTAL) = sa
                                End If
                            End If

                            FormulaBranch_Error = ooFormulaB.Formula
                            ooFormulaB.ModelNum = m
                            ooFormulaB.Quantity = CType(gvPrices.Rows(iRow).FindControl("txtQuantity"), TextBox).Text
                            ooFormulaB.CustomerNumber = s
                            resB = ooFormulaB.ParseAndCalculate
                            If resB = "" Then resB = 0

                            dtPrice.Rows(iRow).Item(E_PricesGrid.TFRPrice) = resB
                            dtPrice.Rows(iRow).Item(E_PricesGrid.GP) = SetGP(res, dtPrice.Rows(iRow).Item(E_PricesGrid.TFRPrice))
                            'dtPrice.Rows.Add()

                        Else


                            Dim currentQty As Integer = 0
                            Dim NexttQty As Integer = 0
                            'Dim LasttQty As Integer = dtP.Rows(dtP.Rows.Count - 2).Item(E_PricesGrid.QTY)
                            Dim LasttQty As Integer = dtP.Rows(7).Item(E_PricesGrid.QTY)
                            Dim FirstQty As Integer = dtP.Rows(0).Item(E_PricesGrid.QTY)
                            If myQty > 0 Then
                                If myQty >= LasttQty Then
                                    dtPrice.Rows(iRow).Item(E_PricesGrid.QTY) = myQty
                                    'dtPrice.Rows(iRow).Item(E_PricesGrid.NETPRICE) = dtP.Rows(gvPrices.Rows.Count - 2).Item(E_PricesGrid.NETPRICE)
                                    dtPrice.Rows(iRow).Item(E_PricesGrid.NETPRICE) = dtP.Rows(7).Item(E_PricesGrid.NETPRICE)
                                    'dtPrice.Rows(iRow).Item(E_PricesGrid.COSTPRICE) = dtP.Rows(gvPrices.Rows.Count - 2).Item(E_PricesGrid.COSTPRICE) ' SetCostPrice(myQty, dtPrice.Rows(iRow).Item(e_PricesGrid.NETPRICE))
                                    dtPrice.Rows(iRow).Item(E_PricesGrid.COSTPRICE) = dtP.Rows(7).Item(E_PricesGrid.COSTPRICE) ' SetCostPrice(myQty, dtPrice.Rows(iRow).Item(e_PricesGrid.NETPRICE))
                                    'dtPrice.Rows(iRow).Item(E_PricesGrid.GP) = dtP.Rows(gvPrices.Rows.Count - 2).Item(E_PricesGrid.GP) ' SetCostPrice(dtPrice.Rows(iRow).Item(e_PricesGrid.NETPRICE), dtPrice.Rows(iRow).Item(e_PricesGrid.COSTPRICE))
                                    dtPrice.Rows(iRow).Item(E_PricesGrid.GP) = dtP.Rows(7).Item(E_PricesGrid.GP) ' SetCostPrice(dtPrice.Rows(iRow).Item(e_PricesGrid.NETPRICE), dtPrice.Rows(iRow).Item(e_PricesGrid.COSTPRICE))
                                    'dtPrice.Rows(iRow).Item(E_PricesGrid.TOTAL) = CalcQutTOTAL(dtP.Rows(gvPrices.Rows.Count - 2).Item(E_PricesGrid.NETPRICE), myQty) ' dtPrice.Rows(gvPrices.Rows.Count - 2).Item(e_PricesGrid.TOTAL)
                                    dtPrice.Rows(iRow).Item(E_PricesGrid.TOTAL) = CalcQutTOTAL(dtP.Rows(7).Item(E_PricesGrid.NETPRICE), myQty) ' dtPrice.Rows(gvPrices.Rows.Count - 2).Item(e_PricesGrid.TOTAL)
                                    ' dtPrice.Rows(iRow).Item(E_PricesGrid.TFRPrice) = dtP.Rows(gvPrices.Rows.Count - 2).Item(E_PricesGrid.TFRPrice)
                                    dtPrice.Rows(iRow).Item(E_PricesGrid.TFRPrice) = dtP.Rows(7).Item(E_PricesGrid.TFRPrice)
                                    dtPrice.Rows(iRow).Item(E_PricesGrid.QTYFct) = dtP.Rows(7).Item(E_PricesGrid.QTYFct)

                                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Price, dtPrice)
                                    Exit Sub
                                ElseIf myQty <= FirstQty Then
                                    dtPrice.Rows(iRow).Item(E_PricesGrid.QTY) = myQty
                                    dtPrice.Rows(iRow).Item(E_PricesGrid.NETPRICE) = dtP.Rows(0).Item(E_PricesGrid.NETPRICE)
                                    dtPrice.Rows(iRow).Item(E_PricesGrid.COSTPRICE) = dtP.Rows(0).Item(E_PricesGrid.COSTPRICE) ' SetCostPrice(myQty, dtPrice.Rows(iRow).Item(e_PricesGrid.NETPRICE))
                                    dtPrice.Rows(iRow).Item(E_PricesGrid.GP) = dtP.Rows(0).Item(E_PricesGrid.GP) ' SetCostPrice(dtPrice.Rows(iRow).Item(e_PricesGrid.NETPRICE), dtPrice.Rows(iRow).Item(e_PricesGrid.COSTPRICE))
                                    dtPrice.Rows(iRow).Item(E_PricesGrid.TOTAL) = CalcQutTOTAL(dtP.Rows(0).Item(E_PricesGrid.NETPRICE), myQty) ' dtPrice.Rows(0).Item(e_PricesGrid.TOTAL)
                                    dtPrice.Rows(iRow).Item(E_PricesGrid.TFRPrice) = dtP.Rows(0).Item(E_PricesGrid.TFRPrice)
                                    dtPrice.Rows(iRow).Item(E_PricesGrid.QTYFct) = dtP.Rows(0).Item(E_PricesGrid.QTYFct)

                                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Price, dtPrice)
                                    Exit Sub
                                Else
                                    If dtPrice.Rows.Count > 1 Then
                                        For i = 0 To dtP.Rows.Count - 2
                                            If dtP.Rows(i).Item(E_PricesGrid.QTY).ToString <> "" Then
                                                If myQty = dtP.Rows(i).Item(E_PricesGrid.QTY) Then
                                                    dtPrice.Rows(iRow).Item(E_PricesGrid.QTY) = myQty
                                                    dtPrice.Rows(iRow).Item(E_PricesGrid.NETPRICE) = dtP.Rows(i).Item(E_PricesGrid.NETPRICE)
                                                    dtPrice.Rows(iRow).Item(E_PricesGrid.COSTPRICE) = dtP.Rows(i).Item(E_PricesGrid.COSTPRICE) ' SetCostPrice(myQty, dtPrice.Rows(iRow).Item(e_PricesGrid.NETPRICE))
                                                    dtPrice.Rows(iRow).Item(E_PricesGrid.GP) = dtP.Rows(i).Item(E_PricesGrid.GP) 'SetCostPrice(dtPrice.Rows(iRow).Item(e_PricesGrid.NETPRICE), dtPrice.Rows(iRow).Item(e_PricesGrid.COSTPRICE))
                                                    dtPrice.Rows(iRow).Item(E_PricesGrid.TOTAL) = CalcQutTOTAL(dtP.Rows(i).Item(E_PricesGrid.NETPRICE), myQty) ' dtPrice.Rows(i).Item(e_PricesGrid.TOTAL)
                                                    dtPrice.Rows(iRow).Item(E_PricesGrid.TFRPrice) = dtP.Rows(i).Item(E_PricesGrid.TFRPrice)
                                                    dtPrice.Rows(iRow).Item(E_PricesGrid.QTYFct) = dtP.Rows(i).Item(E_PricesGrid.QTYFct)

                                                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Price, dtPrice)
                                                    Exit Sub
                                                Else
                                                    If dtP.Rows(i + 1).Item(E_PricesGrid.QTY).ToString <> "" Then
                                                        currentQty = dtP.Rows(i).Item(E_PricesGrid.QTY)
                                                        NexttQty = dtP.Rows(i + 1).Item(E_PricesGrid.QTY)

                                                        If myQty > currentQty And myQty < NexttQty Then
                                                            dtPrice.Rows(iRow).Item(E_PricesGrid.QTY) = myQty
                                                            dtPrice.Rows(iRow).Item(E_PricesGrid.NETPRICE) = dtP.Rows(i).Item(E_PricesGrid.NETPRICE)
                                                            dtPrice.Rows(iRow).Item(E_PricesGrid.COSTPRICE) = dtP.Rows(i).Item(E_PricesGrid.COSTPRICE) ' SetCostPrice(myQty, dtPrice.Rows(iRow).Item(e_PricesGrid.NETPRICE))
                                                            dtPrice.Rows(iRow).Item(E_PricesGrid.GP) = dtP.Rows(i).Item(E_PricesGrid.GP) ' SetCostPrice(dtPrice.Rows(iRow).Item(e_PricesGrid.NETPRICE), dtPrice.Rows(iRow).Item(e_PricesGrid.COSTPRICE))
                                                            dtPrice.Rows(iRow).Item(E_PricesGrid.TOTAL) = CalcQutTOTAL(dtP.Rows(i).Item(E_PricesGrid.NETPRICE), myQty) ' dtPrice.Rows(i).Item(e_PricesGrid.TOTAL)
                                                            dtPrice.Rows(iRow).Item(E_PricesGrid.TFRPrice) = dtP.Rows(i).Item(E_PricesGrid.TFRPrice)
                                                            dtPrice.Rows(iRow).Item(E_PricesGrid.QTYFct) = dtP.Rows(i).Item(E_PricesGrid.QTYFct)

                                                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Price, dtPrice)

                                                            Exit Sub
                                                        End If
                                                    End If

                                                End If
                                            End If
                                        Next
                                    End If
                                End If
                            Else
                                Exit Sub
                            End If

                            Dim res As String = ""
                            Dim resB As String = ""
                            Dim dt As DataTable = ReturnCurentDTparams()
                            Dim ooFormula As New FormulaResult(GetCurrentPriceFormula, dt, 0, Nothing)
                            Dim ooFormulaB As New FormulaResult(GetCurrentPriceFormulaBranch, dt, 0, Nothing)

                            'Dim ooFormula As New FormulaResult(_PriceFormula, _dtParamListConfiguration, 0, Nothing)

                            If ooFormula.ToString.Trim <> "" Then

                                Dim sStart As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)
                                Dim m As String = ""
                                If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                                    m = _ModelNumConfiguration
                                ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                                    m = _ModelNumModification
                                End If

                                ooFormula.ModelNum = m

                                ooFormula.Quantity = CType(gvPrices.Rows(iRow).FindControl("txtQuantity"), TextBox).Text
                                '  Dim s As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
                                Dim s As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
                                ooFormula.CustomerNumber = s
                                res = ooFormula.ParseAndCalculate
                                If res = "" Then res = 0
                                Dim NewQuantity As Integer = 0


                                dtPrice.Rows(iRow).Item(E_PricesGrid.QTY) = CType(gvPrices.Rows(iRow).FindControl("txtQuantity"), TextBox).Text
                                dtPrice.Rows(iRow).Item(E_PricesGrid.NETPRICE) = res
                                dtPrice.Rows(iRow).Item(E_PricesGrid.COSTPRICE) = SetCostPrice(CType(gvPrices.Rows(iRow).FindControl("txtQuantity"), TextBox).Text, res)
                                dtPrice.Rows(iRow).Item(E_PricesGrid.GP) = SetCostPrice(dtPrice.Rows(iRow).Item(E_PricesGrid.NETPRICE), dtPrice.Rows(iRow).Item(E_PricesGrid.COSTPRICE))
                                If IsNumeric(res) AndAlso IsNumeric(dtPrice.Rows(iRow).Item(E_PricesGrid.QTY)) Then
                                    dtPrice.Rows(iRow).Item(E_PricesGrid.TOTAL) = Format(CDbl(res) * CDbl(dtPrice.Rows(iRow).Item(E_PricesGrid.QTY)), "#.##")
                                End If


                                If ooFormulaB.ToString.Trim <> "" Then
                                    dtPrice.Rows(iRow).Item(E_PricesGrid.TFRPrice) = resB
                                    ooFormulaB.ModelNum = m
                                    ooFormulaB.Quantity = CType(gvPrices.Rows(iRow).FindControl("txtQuantity"), TextBox).Text
                                    ooFormulaB.CustomerNumber = s
                                    resB = ooFormula.ParseAndCalculate
                                    If resB = "" Then resB = 0
                                    If IsNumeric(resB) AndAlso IsNumeric(dtPrice.Rows(iRow).Item(E_PricesGrid.QTY)) Then
                                        dtPrice.Rows(iRow).Item(E_PricesGrid.TFRPrice) = Format(CDbl(resB) * CDbl(dtPrice.Rows(iRow).Item(E_PricesGrid.QTY)), "#.##")
                                    End If
                                End If

                                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Price, dtPrice)

                                ooFormula = Nothing
                                ooFormulaB = Nothing


                            End If
                        End If




                    End If
                Catch ex As Exception

                End Try

                dtP = Nothing
            End If




        Catch ex As Exception

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES SetQuantityPrices", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            clsMail.SendEmailError("SetQuantityPrices - ERROR" & Formula_Error & " - " & FormulaBranch_Error, ex.Message, False, "Set quantity prices", clsBranch.ReturnActiveBranchCodeState)

        Finally
            dtPrice = Nothing
        End Try

    End Sub


    Private Function CalcQutTOTAL(Res As String, QTY As String) As Decimal
        Try

            If IsNumeric(Res) AndAlso IsNumeric(QTY) Then
                Dim Total As String = Format(CDbl(Res) * CDbl(QTY), "#.##")
                If IsNumeric(Total) Then
                    Return Total
                End If

            End If
            Return 0

        Catch ex As Exception
            Return 0
        End Try
    End Function
    Private Function SetCostPrice(Qty As Integer, QTY_NET_PRICE As Decimal) As Decimal
        Try

            Dim stnStr As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._CostFormula)
            'QTY_NET_PRICE/(STNNET/STNTFR)


            Dim STNNET As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerPrice, False)
            Dim STNTFR As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TFR_STNBranchPrice, False)
            Dim rp As Decimal = 0

            If IsNumeric(QTY_NET_PRICE) AndAlso IsNumeric(STNTFR) AndAlso QTY_NET_PRICE > 0 AndAlso STNTFR > 0 Then
                rp = QTY_NET_PRICE / (STNNET / STNTFR) ' stnStr.Replace("QTY_NET_PRICE", STNNET).Replace("STNNET" 'QTY_NET_PRICE / (STNNET / STNTFR)
            Else
                rp = QTY_NET_PRICE
            End If

            Return rp
        Catch ex As Exception

            Return 0

        End Try
    End Function

    Private Function SetGP(netPrice As Decimal, tfrPrice As Decimal) As Decimal
        Try
            Dim GPStr As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._GPFormula)
            'QTY_NET_PRICE / (STNNET / STNTFR)
            '(QTY_NET_PRICE - CostFormula/QTY_NET_PRICE

            'GPStr = GPStr.Replace("QTY_NET_PRICE", netPrice)
            'GPStr = GPStr.Replace("CostFormula", costPrice)

            ' Dim rp As Decimal = GPStr.Replace("QTY_NET_PRICE", CDec(netPrice)) - (GPStr.Replace("CostFormula", CDec(costPrice)) / GPStr.Replace("QTY_NET_PRICE", CDec(netPrice)))

            Dim rp As Decimal = (netPrice - tfrPrice) / netPrice

            Return rp
        Catch ex As Exception

            Return 0

        End Try
    End Function



    Private Sub gvPrices_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvPrices.RowDataBound
        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-gvPrices_RowDataBound-" & i_counter)

        Try


            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim sSubmitted As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Submitted, False)
                Dim dt As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "")
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    Dim qtyT As String = ""
                    Dim netP As String = ""
                    Dim priceId As String = ""
                    Dim totalP As String = ""
                    Dim orderQ As String = ""
                    Dim costP As String = ""
                    Dim gpP As String = ""
                    Dim tfrP As String = ""
                    Dim delwP As String = ""
                    qtyT = dt.Rows(e.Row.RowIndex)(E_PricesGrid.QTY).ToString
                    netP = dt.Rows(e.Row.RowIndex)(E_PricesGrid.NETPRICE).ToString
                    priceId = dt.Rows(e.Row.RowIndex)(E_PricesGrid.Price_ID).ToString
                    totalP = dt.Rows(e.Row.RowIndex)(E_PricesGrid.TOTAL).ToString
                    orderQ = dt.Rows(e.Row.RowIndex)("OrderedQuantity").ToString
                    costP = dt.Rows(e.Row.RowIndex)(E_PricesGrid.COSTPRICE).ToString
                    gpP = dt.Rows(e.Row.RowIndex)(E_PricesGrid.GP).ToString
                    tfrP = dt.Rows(e.Row.RowIndex)(E_PricesGrid.TFRPrice).ToString
                    delwP = dt.Rows(e.Row.RowIndex)(E_PricesGrid.DELIVERYWEEKS).ToString


                    '-----------------ENABEL DISABLE ORDERED-------------------
                    Try
                        Dim sFPC As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagPricesChanged, False).ToString.ToUpper
                        If qtyT <> "" AndAlso IsNumeric(qtyT) Then
                            If sFPC = "FALSE" Then
                                Dim Submitted As Boolean = True
                                If sSubmitted = "0" Or sSubmitted.ToUpper = "FALSE" Then Submitted = False
                                If (Not TemporarilyQuotation AndAlso Submitted) Then

                                    If orderQ.ToString.ToUpper = "TRUE" Then
                                        'CType(e.Row.FindControl("txtQuantity"), TextBox).Style.Add("background-color", "#bdcbe0")
                                        CType(e.Row.Cells(E_PricesGrid.btnAddToCart).FindControl("ImgPqo"), ImageButton).Enabled = True
                                        CType(e.Row.Cells(E_PricesGrid.btnAddToCart).FindControl("ImgPqo"), ImageButton).ImageUrl = "~/media/Icons/enabledcartordered.png"
                                        CType(e.Row.Cells(E_PricesGrid.btnAddToCart).FindControl("ImgPqo"), ImageButton).CssClass = "PaddingCont"
                                    Else
                                        CType(e.Row.Cells(E_PricesGrid.btnAddToCart).FindControl("ImgPqo"), ImageButton).Enabled = True
                                        CType(e.Row.Cells(E_PricesGrid.btnAddToCart).FindControl("ImgPqo"), ImageButton).ImageUrl = "~/media/Icons/enabledcart.png"
                                        CType(e.Row.Cells(E_PricesGrid.btnAddToCart).FindControl("ImgPqo"), ImageButton).CssClass = "PaddingCont"
                                    End If

                                Else
                                    CType(e.Row.Cells(E_PricesGrid.btnAddToCart).FindControl("ImgPqo"), ImageButton).Enabled = False
                                    CType(e.Row.Cells(E_PricesGrid.btnAddToCart).FindControl("ImgPqo"), ImageButton).ImageUrl = "~/media/Icons/disabledcart.png"
                                    CType(e.Row.Cells(E_PricesGrid.btnAddToCart).FindControl("ImgPqo"), ImageButton).CssClass = "PaddingContDis"

                                End If
                            Else


                                If orderQ.ToString.ToUpper = "TRUE" Then
                                    'CType(e.Row.FindControl("txtQuantity"), TextBox).Style.Add("background-color", "#bdcbe0")
                                    CType(e.Row.Cells(E_PricesGrid.btnAddToCart).FindControl("ImgPqo"), ImageButton).Enabled = False
                                    CType(e.Row.Cells(E_PricesGrid.btnAddToCart).FindControl("ImgPqo"), ImageButton).ImageUrl = "~/media/Icons/disabledcartordered.png"
                                    CType(e.Row.Cells(E_PricesGrid.btnAddToCart).FindControl("ImgPqo"), ImageButton).CssClass = "PaddingContDis"
                                Else
                                    CType(e.Row.Cells(E_PricesGrid.btnAddToCart).FindControl("ImgPqo"), ImageButton).Enabled = False
                                    CType(e.Row.Cells(E_PricesGrid.btnAddToCart).FindControl("ImgPqo"), ImageButton).ImageUrl = "~/media/Icons/disabledcart.png"
                                    CType(e.Row.Cells(E_PricesGrid.btnAddToCart).FindControl("ImgPqo"), ImageButton).CssClass = "PaddingContDis"
                                End If
                            End If



                        Else
                            CType(e.Row.Cells(E_PricesGrid.btnAddToCart).FindControl("ImgPqo"), ImageButton).Enabled = False
                            CType(e.Row.Cells(E_PricesGrid.btnAddToCart).FindControl("ImgPqo"), ImageButton).Visible = False
                        End If

                    Catch ex As Exception
                        CType(e.Row.Cells(E_PricesGrid.btnAddToCart).FindControl("ImgPqo"), ImageButton).Enabled = False
                        CType(e.Row.Cells(E_PricesGrid.btnAddToCart).FindControl("ImgPqo"), ImageButton).Visible = False
                    End Try

                    '----------------------------------------------------------


                    CType(e.Row.FindControl("txtQuantity"), TextBox).Text = qtyT
                    CType(e.Row.FindControl("lblPriceID"), Label).Text = priceId

                    CType(e.Row.FindControl("txtTotal"), TextBox).Text = totalP
                    'CType(e.Row.FindControl("txtDeliveryWeeks"), TextBox).Text = dt.Rows(e.Row.RowIndex)(e_PricesGrid.DELIVERYWEEKS).ToString

                    If IsNumeric(netP) Then
                        CType(e.Row.FindControl("txtNetPrice"), TextBox).Text = Format(CDbl(netP), "#.##")
                        If IsNumeric(CType(e.Row.FindControl("txtNetPrice"), TextBox).Text) Then
                            CType(e.Row.FindControl("txtNetPrice"), TextBox).Text = LocalizationManager.CulturingNumber(CType(e.Row.FindControl("txtNetPrice"), TextBox).Text, False)
                        End If
                    Else
                        CType(e.Row.FindControl("txtNetPrice"), TextBox).Text = netP
                        If IsNumeric(CType(e.Row.FindControl("txtNetPrice"), TextBox).Text) Then
                            CType(e.Row.FindControl("txtNetPrice"), TextBox).Text = LocalizationManager.CulturingNumber(CType(e.Row.FindControl("txtNetPrice"), TextBox).Text, False)
                        End If
                    End If






                    If IsNumeric(costP) Then
                        CType(e.Row.FindControl("txtCostPrice"), TextBox).Text = Format(CDbl(costP), "#.##")
                    Else
                        CType(e.Row.FindControl("txtCostPrice"), TextBox).Text = costP
                    End If

                    If IsNumeric(gpP) Then
                        CType(e.Row.FindControl("txtGP"), TextBox).Text = Format(CDbl(gpP), "#.00")
                    Else
                        CType(e.Row.FindControl("txtGP"), TextBox).Text = gpP
                    End If

                    If IsNumeric(tfrP) Then
                        CType(e.Row.FindControl("txtTFRPrice"), TextBox).Text = Format(CDbl(tfrP), "#.##")
                    Else
                        CType(e.Row.FindControl("txtTFRPrice"), TextBox).Text = tfrP
                    End If


                    If IsNumeric(totalP) Then
                        If IsNumeric(netP) Then
                            CType(e.Row.FindControl("txtTotal"), TextBox).Text = Format(CDbl(totalP), "#.##")
                            If IsNumeric(CType(e.Row.FindControl("txtTotal"), TextBox).Text) Then
                                CType(e.Row.FindControl("txtTotal"), TextBox).Text = LocalizationManager.CulturingNumber(CType(e.Row.FindControl("txtTotal"), TextBox).Text, True)
                            End If
                        Else
                            CType(e.Row.FindControl("txtTotal"), TextBox).Text = totalP
                            If IsNumeric(CType(e.Row.FindControl("txtTotal"), TextBox).Text) Then
                                CType(e.Row.FindControl("txtTotal"), TextBox).Text = LocalizationManager.CulturingNumber(CType(e.Row.FindControl("txtTotal"), TextBox).Text, True)
                            End If
                        End If
                    End If

                    CType(e.Row.FindControl("txtDeliveryWeeks"), TextBox).Text = delwP

                    If e.Row.RowIndex = dt.Rows.Count - 1 Or e.Row.RowIndex <> dt.Rows.Count - 1 Then


                        CType(e.Row.Cells(E_PricesGrid.btnDELETE).Controls(0), ImageButton).Enabled = True
                        'CType(e.Row.Cells(e_PricesGrid.btnDELETE).Controls(0), ImageButton).Visible = True

                        CType(e.Row.Cells(E_PricesGrid.btnPrice).Controls(0), ImageButton).Enabled = True
                        'CType(e.Row.Cells(E_PricesGrid.btnPrice).Controls(0), ImageButton).Visible = True
                        CType(e.Row.Cells(E_PricesGrid.btnPrice).Controls(0), ImageButton).CssClass = "DisplayNone"

                        CType(e.Row.FindControl("txtQuantity"), TextBox).Enabled = True
                        'CType(e.Row.FindControl("txtQuantity"), TextBox).Style.Add("border", "Solid")
                        CType(e.Row.FindControl("txtQuantity"), TextBox).CssClass = "QtyEnableCssControl FontFamilyRoboto FontSizeRoboto14" ' Style.Add("border", "Solid")
                        CType(e.Row.FindControl("txtNetPrice"), TextBox).CssClass = "PriceEnableCssControl FontFamilyRoboto FontSizeRoboto14"
                        CType(e.Row.FindControl("txtTFRPrice"), TextBox).CssClass = "PriceEnableCssControl FontFamilyRoboto FontSizeRoboto14"
                        CType(e.Row.FindControl("txtTotal"), TextBox).CssClass = "PriceEnableCssControl FontFamilyRoboto FontSizeRoboto14"
                    Else

                        CType(e.Row.Cells(E_PricesGrid.btnDELETE).Controls(0), ImageButton).Enabled = False

                        CType(e.Row.Cells(E_PricesGrid.btnPrice).Controls(0), ImageButton).Enabled = False
                        CType(e.Row.Cells(E_PricesGrid.btnPrice).Controls(0), ImageButton).Visible = False

                        CType(e.Row.FindControl("txtQuantity"), TextBox).Enabled = False


                    End If



                    CType(e.Row.FindControl("txtQuantity"), TextBox).Attributes.Add("onchange", "return QuantityEnterChange('" & CType(e.Row.Cells(E_PricesGrid.btnPrice).Controls(0), ImageButton).ClientID & "');")
                    CType(e.Row.FindControl("txtQuantity"), TextBox).Attributes.Add("onkeypress", "QuantityEnterPressed('" & CType(e.Row.FindControl("txtQuantity"), TextBox).ClientID & "');")

                End If
            ElseIf e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(E_PricesGrid.QTY).Text = _PricesGridCo1Quantity
                e.Row.Cells(E_PricesGrid.NETPRICE).Text = _PricesGridCo1NetPrice
                e.Row.Cells(E_PricesGrid.TOTAL).Text = _PricesGridCo1TotalPrice
            End If
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES gvPrices_RowDataBound", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        End Try

        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-gvPrices_RowDataBound-" & i_counter)

    End Sub

    Private Function CheckIfCanDoPriceByRate() As Boolean
        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-CheckIfCanDoPrice-" & i_counter)

        Try
            Dim b As Boolean = True
            Dim TRF_MKT As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceCalculateFlag, False)
            If TRF_MKT = "TFR" Then
                Dim ch1 As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.RateTFR_USD, False)
                If Not ch1 Is Nothing AndAlso ch1 <> "" AndAlso IsNumeric(ch1) AndAlso CDbl(ch1) > 0 Then
                Else
                    b = False
                End If
            End If
            If TRF_MKT = "MKT" And b = True Then
                Dim ch2 As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.RateMKT_USD, False)
                If Not ch2 Is Nothing AndAlso ch2 <> "" AndAlso IsNumeric(ch2) AndAlso CDbl(ch2) > 0 Then
                Else
                    b = False
                End If
            End If

            Return b
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES CheckIfCanDoPriceByRate", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Return False
        End Try
        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-CheckIfCanDoPrice-" & i_counter)

    End Function

    Private Function CheckIfCanDoPriceByPrice() As Boolean

        Try


            Dim b As Boolean = True
            Dim TRF_MKT As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceCalculateFlag, False)
            If TRF_MKT = "TFR" Then
                Dim ch1 As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TFR_STNBranchPrice, False)
                If Not ch1 Is Nothing AndAlso ch1 <> "" AndAlso IsNumeric(ch1) AndAlso CDbl(ch1) > 0 Then
                Else
                    b = False
                End If
            End If
            If TRF_MKT = "MKT" And b = True Then
                Dim ch2 As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerPrice, False)
                If Not ch2 Is Nothing AndAlso ch2 <> "" AndAlso IsNumeric(ch2) AndAlso CDbl(ch2) > 0 Then
                Else
                    b = False
                End If
            End If

            Return b
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES CheckIfCanDoPriceByPrice", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Return False
        End Try
        'i_counter += 1 : GeneralException.WriteEventLogInCounter("QBuilder-CheckIfCanDoPrice-" & i_counter)

    End Function
    Private Sub gvPrices_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvPrices.RowCommand
        Try

            Dim dtPq As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "")

            If e.CommandArgument.ToString.ToUpper <> "IMGPQO" Then
                If Not e.CommandArgument Is Nothing AndAlso IsNumeric(e.CommandArgument.ToString) Then
                    If CType(gvPrices.Rows(e.CommandArgument).FindControl("txtQuantity"), TextBox).Text <> dtPq.Rows(e.CommandArgument).Item("QTY").ToString Then
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagPricesChanged, True)
                        txtFlagPricesChanged.Text = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagPricesChanged, False).ToString.ToUpper

                        Select Case e.CommandName.ToString.ToUpper
                            Case E_PricesGrid.btnPrice.ToString.ToUpper
                                Dim canceled As Boolean = False
                                If CheckIfCanDoPriceByPrice() = True Then
                                    SetQuantityPrices(e.CommandArgument, "")
                                    Dim dtp As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "")
                                    If Not dtp Is Nothing AndAlso dtp.Rows.Count > 0 Then
                                        Try
                                            Dim odt As DataRow = dtp.NewRow
                                            For isd As Integer = 0 To dtp.Columns.Count - 1
                                                odt(isd) = dtp.Rows(dtp.Rows.Count - 1)(isd)
                                            Next
                                            dtp.Rows(dtp.Rows.Count - 1).Delete()
                                            dtp.DefaultView.Sort = "NetPrice DESC,Qty,Total ASC"
                                            dtp.Rows.Add(odt)
                                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Price, dtp)
                                        Catch ex As Exception

                                        End Try


                                    End If
                                Else
                                    ShowOooPsAlert()
                                End If


                            Case E_PricesGrid.btnDELETE.ToString.ToUpper

                            Case E_PricesGrid.btnAddToCart.ToString.ToUpper

                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "alertSS", "ShowClickShopAlert()", True)



                        End Select

                    End If
                End If

            End If


        Catch ex As GeneralException
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES gvPrices_RowCommand", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex)
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES gvPrices_RowCommand", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)


            GeneralException.BuildError(Page, ex)
            Dim _ex As New GeneralException(String.Empty, ex)
        End Try
    End Sub
    Private Function convertToByteArray(sString As String) As Byte()
        Dim lb As Byte() = System.Text.Encoding.Default.GetBytes(sString)
        Return lb
    End Function
    Private Function GetShopLink(sMail As String, sCustomerNo As String) As String
        Dim data As String
        Dim string_data As String
        Dim consumer_secret As String

        data = "{""user_email"":""" & sMail & """,""customer_no"":""" & sCustomerNo & """}"

        string_data = Convert.ToBase64String(convertToByteArray(data))

        consumer_secret = "3665030099039446053"

        Dim t As New HMACSHA256(convertToByteArray(consumer_secret))

        Dim string_sign = Convert.ToBase64String(t.ComputeHash(convertToByteArray(string_data)))

        Dim the_message As String = string_sign & "." & string_data
        Return the_message
    End Function



    Private Sub GetAutoStartPrices(ModelNo As String)

        Try
            Dim _dtFactorsWithValues As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._TempdtFactorsWithValues, "")

            '-----------------
            Dim ConvFormulaMKT As String = _PriceFormula_MKT
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormulaMKT_Value, ConvFormulaMKT)
            If Not _dtFactorsWithValues Is Nothing Then
                For Each r As DataRow In _dtFactorsWithValues.Rows
                    ConvFormulaMKT = FormulaResult.ReplaceValWithVal(ConvFormulaMKT, "{" & r.Item("dc_FactorName").ToString & "}", r.Item("dc_FactorValue").ToString)
                Next
            End If
            _PriceFormula_MKT = ConvFormulaMKT
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormula_MKT, _PriceFormula_MKT)

            '-----------------

            Dim ConvFormulaTFR As String = _PriceFormulaTFR
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormulaTFR_Value, ConvFormulaTFR)
            If Not _dtFactorsWithValues Is Nothing Then
                For Each r As DataRow In _dtFactorsWithValues.Rows
                    ConvFormulaTFR = FormulaResult.ReplaceValWithVal(ConvFormulaTFR, "{" & r.Item("dc_FactorName").ToString & "}", r.Item("dc_FactorValue").ToString)
                Next
            End If
            _PriceFormulaTFR = ConvFormulaTFR
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormulaTFR, _PriceFormulaTFR)

            '-----------------

            Dim ConvFormulaMKTBranch As String = _BranchPriceFormulaMKT
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BranchPriceFormulaMKT_Value, ConvFormulaMKTBranch)
            If Not _dtFactorsWithValues Is Nothing Then
                For Each r As DataRow In _dtFactorsWithValues.Rows
                    ConvFormulaMKTBranch = FormulaResult.ReplaceValWithVal(ConvFormulaMKTBranch, "{" & r.Item("dc_FactorName").ToString & "}", r.Item("dc_FactorValue").ToString)
                Next
            End If
            _BranchPriceFormulaMKT = ConvFormulaMKTBranch
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BranchPriceFormulaMKT, _BranchPriceFormulaMKT)

            '-----------------

            Dim ConvFormulaTFRBranch As String = _BranchPriceFormulaTFR
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BranchPriceFormulaTFR_Value, ConvFormulaTFRBranch)
            If Not _dtFactorsWithValues Is Nothing Then
                For Each r As DataRow In _dtFactorsWithValues.Rows
                    ConvFormulaTFRBranch = FormulaResult.ReplaceValWithVal(ConvFormulaTFRBranch, "{" & r.Item("dc_FactorName").ToString & "}", r.Item("dc_FactorValue").ToString)
                Next
            End If
            _BranchPriceFormulaTFR = ConvFormulaTFRBranch
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BranchPriceFormulaTFR, _BranchPriceFormulaTFR)

            Dim sQty As String = "" '"10;20;30;40;50;60;70"

            Dim dtPq As DataTable = clsPrices.GetPricesQty(CInt(ModelNo))
            If Not dtPq Is Nothing AndAlso dtPq.Rows.Count > 0 Then
                sQty = dtPq.Rows(0).Item("ModelQty").ToString
                hfMinQuantity.Value = sQty
            Else
                GeneralException.BuildError(Page, "Missing Quantities!")

            End If

            If CheckIfCanDoPriceByPrice() = True Then
                SetQuantityPrices(-1, sQty)
            Else
                ShowOooPsAlert()
            End If

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES GetAutoStartPrices", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub

    Private Sub GetDefaultPrices(ByRef dttPrice As DataTable)
        Try
            Dim sQty As String = "" '"10;20;30;40;50;60;70"

            Dim dtPq As DataTable = clsPrices.GetPricesQty(CInt(clsQuatation.ACTIVE_ModelNumber))
            If Not dtPq Is Nothing AndAlso dtPq.Rows.Count > 0 Then
                sQty = dtPq.Rows(0).Item("ModelQty").ToString
                hfMinQuantity.Value = sQty
            Else
                GeneralException.BuildError(Page, "Missing Quantities!")
            End If

            If CheckIfCanDoPriceByPrice() = True Then
                'SetQuantityPrices(-1, sQty)

                dttPrice.Columns.Add(E_PricesGrid.btnPrice.ToString)
                dttPrice.Columns.Add(E_PricesGrid.Price_ID.ToString, GetType(System.Int32))
                'dtPrice.Columns.Add(e_PricesGrid.SELECTP.ToString)
                dttPrice.Columns.Add(E_PricesGrid.QTY.ToString, GetType(System.Int16))
                dttPrice.Columns.Add(E_PricesGrid.NETPRICE.ToString, GetType(System.Decimal))
                dttPrice.Columns.Add(E_PricesGrid.TOTAL.ToString, GetType(System.Decimal))
                dttPrice.Columns.Add(E_PricesGrid.COSTPRICE.ToString, GetType(System.Decimal))
                dttPrice.Columns.Add(E_PricesGrid.GP.ToString, GetType(System.Decimal))
                dttPrice.Columns.Add(E_PricesGrid.DELIVERYWEEKS.ToString, GetType(System.Int16))
                dttPrice.Columns.Add(E_PricesGrid.TFRPrice.ToString, GetType(System.Decimal))

                dttPrice.Columns.Add(E_PricesGrid.btnAddToCart.ToString)
                dttPrice.Columns.Add(E_PricesGrid.btnDELETE.ToString)
                dttPrice.Columns.Add(E_PricesGrid.OrderedQuantity.ToString)
                dttPrice.Columns.Add(E_PricesGrid.QTYFct.ToString, GetType(System.Decimal))

                dttPrice.Rows.Add()
                Dim res As String = ""
                Dim resB As String = ""
                Dim dt As DataTable = ReturnCurentDTparams()

                '****************************************************
                Dim ooFormula As New FormulaResult(GetCurrentPriceFormula, dt, 0, Nothing)
                Dim ooFormulaB As New FormulaResult(GetCurrentPriceFormulaBranch, dt, 0, Nothing)

                If ooFormula.ToString.Trim <> "" Then

                    Dim sStart As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)
                    Dim m As String = ""
                    If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                        m = _ModelNumConfiguration
                    ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                        m = _ModelNumModification
                    End If

                    ooFormula.ModelNum = m

                    Dim AutoQtyARR() As String = sQty.Split(";")


                    For i As Integer = 0 To AutoQtyARR.Count - 1
                        If IsNumeric(AutoQtyARR(i).ToString) Then
                            'Formula_Error = ooFormula.Formula.ToString

                            ooFormula.Quantity = AutoQtyARR(i)
                            Dim s As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
                            ooFormula.CustomerNumber = s
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempQTYFct, "")
                            res = ooFormula.ParseAndCalculate
                            If res = "" Then res = 0
                            If IsNumeric(res) Then
                                res = Format(CDbl(res), "#.##")
                            End If
                            dttPrice.Rows(i).Item(E_PricesGrid.QTY) = AutoQtyARR(i)
                            dttPrice.Rows(i).Item(E_PricesGrid.NETPRICE) = res
                            dttPrice.Rows(i).Item(E_PricesGrid.COSTPRICE) = SetCostPrice(AutoQtyARR(i), res)
                            If IsNumeric(res) AndAlso IsNumeric(dttPrice.Rows(i).Item(E_PricesGrid.QTY)) Then
                                Dim sa As String = Format(CDbl(res) * CDbl(dttPrice.Rows(i).Item(E_PricesGrid.QTY)), "#.##")
                                If IsNumeric(sa) Then
                                    dttPrice.Rows(i).Item(E_PricesGrid.TOTAL) = sa
                                End If
                            End If
                            Try
                                dttPrice.Rows(i).Item(E_PricesGrid.QTYFct) = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._TempQTYFct, False)
                            Catch ex As Exception

                            End Try

                            'FormulaBranch_Error = ooFormulaB.Formula
                            ooFormulaB.ModelNum = m
                            ooFormulaB.Quantity = AutoQtyARR(i)
                            ooFormulaB.CustomerNumber = s
                            resB = ooFormulaB.ParseAndCalculate
                            If resB = "" Then resB = 0

                            dttPrice.Rows(i).Item(E_PricesGrid.TFRPrice) = resB
                            dttPrice.Rows(i).Item(E_PricesGrid.GP) = SetGP(res, dttPrice.Rows(i).Item(E_PricesGrid.TFRPrice))
                            dttPrice.Rows.Add()

                            'CType(e.Row.FindControl("txtDeliveryWeeks"), TextBox).Text = dt.Rows(e.Row.RowIndex)(e_PricesGrid.DELIVERYWEEKS).ToString

                        End If
                    Next

                    ooFormula = Nothing


                End If

            Else
                ShowOooPsAlert()
            End If
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES GetDefaultPrices", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Dim uniqueID As String = "?rErepTr=" & Request.QueryString("rErepTr") & "&"

        Try
            Response.Redirect("QBuilder.aspx" & uniqueID & "iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), False)
        Catch ex As Exception
            Response.Redirect("QBuilder.aspx" & uniqueID, False)
        End Try
    End Sub

#End Region

    Private Sub gvDocuments_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvDocuments.RowDataBound
        Try
            '  SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DocumentsFIllDone, "YES")
            If e.Row.RowType = DataControlRowType.DataRow Then
                Try
                    If IsDate(e.Row.Cells(E_DocumentsGrid.FileDate).Text.ToString) AndAlso CDate(e.Row.Cells(E_DocumentsGrid.FileDate).Text).AddMinutes(1) < Now Then
                        CType(e.Row.FindControl("NewImage"), ImageButton).ImageUrl = "../media/Images/DEFAULT.png"
                    End If
                Catch ex As Exception

                End Try
                Try
                    If e.Row.Cells(E_DocumentsGrid.FName).Text.ToString.Contains("Approval Development") Then
                        lblApprovalDocs.Text = e.Row.Cells(E_DocumentsGrid.FName).Text.ToString
                    End If
                Catch ex As Exception

                End Try
                If e.Row.Cells(E_DocumentsGrid.FName).Text.ToString.ToUpper.Contains("DRW") Or e.Row.Cells(E_DocumentsGrid.FName).Text.ToString().ToString.ToUpper.Contains("REPORT") Or e.Row.Cells(E_DocumentsGrid.FileId).Text = "0" Then
                    CType(e.Row.FindControl("DeleteButton"), ImageButton).ImageUrl = "../media/Images/DEFAULT.png"
                    CType(e.Row.FindControl("DeleteButton"), ImageButton).Enabled = False
                End If
                If e.Row.Cells(E_DocumentsGrid.FileId).Text = "0" Then
                    CType(e.Row.FindControl("OpenButton"), ImageButton).ImageUrl = "../media/Images/DEFAULT.png"
                    CType(e.Row.FindControl("OpenButton"), ImageButton).Enabled = False
                End If
                If e.Row.Cells(E_DocumentsGrid.FName).Text.ToString.ToUpper.Contains("DRW") Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Temp_fileFolderPath, e.Row.Cells(E_DocumentsGrid.FolderArr).Text.ToString, False)
                ElseIf e.Row.Cells(E_DocumentsGrid.FileId).Text.ToString.ToUpper = "0" Then

                    Dim sSc_3D As String = ConfigurationManager.AppSettings("TimeToWaitForBuildCATIA")
                    Dim qud As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationDateWithTime, False).ToString
                    If CDate(qud).AddSeconds(CInt(sSc_3D) + 10) > Now Then
                        CType(e.Row.FindControl("NewImage"), ImageButton).ImageUrl = "../media/Images/DEFAULTRun.gif"
                    Else
                        CType(e.Row.FindControl("NewImage"), ImageButton).ImageUrl = "../media/Images/DEFAULT.png"
                    End If

                End If
            ElseIf e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(E_DocumentsGrid.FileType).Text = _DocumentGridCo1FileType
                e.Row.Cells(E_DocumentsGrid.FileSize).Text = _DocumentGridCo1FileSize
                e.Row.Cells(E_DocumentsGrid.FileDate).Text = _DocumentGridCo1FileDate
                e.Row.Cells(E_DocumentsGrid.FName).Text = _DocumentGridCo1FileName

            End If

        Catch ex As Exception
            'GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub gvDocuments_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDocuments.RowCommand
        Try
            Select Case e.CommandName.ToUpper
                Case "BTNOPEN"
                    _fileFolderPath = gvDocuments.Rows(e.CommandArgument).Cells(E_DocumentsGrid.FolderArr).Text
                    _fileName = gvDocuments.Rows(e.CommandArgument).Cells(E_DocumentsGrid.FName).Text
                    Dim b() As Byte = Nothing
                    Documents.OpenDocument(_fileFolderPath, _fileName, clsBranch.ReturnActiveBranchCodeForDocuments, b, clsBranch.ReturnActiveStorageFolderForDouuments)
                Case "DELETEFILE"
                    If gvDocuments.Rows(e.CommandArgument).Cells(E_DocumentsGrid.FName).Text.ToString().ToString.ToUpper.Contains("DRW") Or gvDocuments.Rows(e.CommandArgument).Cells(E_DocumentsGrid.FName).Text.ToString().ToString.ToUpper.Contains("REPORT") Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "alert1", "CantDeleteThisFileAlert();", True)
                        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Script", "DuplicateConfirm();", True)
                    Else

                        lblDOCSFileId.Text = gvDocuments.Rows(e.CommandArgument).Cells(E_DocumentsGrid.FileId).Text.ToString()
                        lblDOCSFileName.Text = gvDocuments.Rows(e.CommandArgument).Cells(E_DocumentsGrid.FName).Text.ToString()
                        lblDOCSFolderPath.Text = gvDocuments.Rows(e.CommandArgument).Cells(E_DocumentsGrid.FolderArr).Text.ToString()

                        'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Script", "DeleteQuotationDocument();", True)
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert2", "DeleteQuotationDocument(); ", True)
                    End If

            End Select
        Catch ex As GeneralException
            GeneralException.BuildError(Page, ex)
        Catch ex As Exception
            GeneralException.BuildError(Page, ex)
        End Try


    End Sub

    Private Sub OpenPDF_BSON()
        Try
            For Each r As GridViewRow In gvDocuments.Rows
                If r.Cells(E_DocumentsGrid.FName).Text.ToString.ToUpper.Contains("DRW_BSON_") Then
                    _fileFolderPath = r.Cells(E_DocumentsGrid.FolderArr).Text
                    _fileName = r.Cells(E_DocumentsGrid.FName).Text
                    Dim b() As Byte = Nothing
                    'Documents.OpenDocument(_fileFolderPath, _fileName, clsBranch.ReturnActiveBranchCodeForDocuments, b, clsBranch.ReturnActiveStorageFolderForDouuments)
                End If
            Next

        Catch ex As GeneralException
            GeneralException.BuildError(Page, ex)
        Catch ex As Exception
            GeneralException.BuildError(Page, ex)
        End Try
    End Sub

    Private Sub SetTabs()

        wucTabs.tcModel = False
        wucTabs.tcMatirial = False
        wucTabs.tcParameters = False
        wucTabs.tcGetQuotation = True
        wucTabs.SelectedItem = wucTab.E_MNUiTEMS.GetQuotation

        wucTabs.ItemsVisiblty()

    End Sub
    Private Sub Prices_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try



            Try
                Dim sfn As String = ""
                Dim fDf As String = ""


                For Each r As GridViewRow In gvDocuments.Rows

                    Dim filesNAme As String = r.Cells(E_DocumentsGrid.FName).Text.Trim

                    If filesNAme.Contains("REPORT_") AndAlso filesNAme.Length > 7 Then
                        fDf &= filesNAme.ToUpper.Substring(filesNAme.Length - 7, 7)
                    End If
                Next

            Catch ex As Exception

            End Try

            Try
                If Not gvPrices.HeaderRow Is Nothing Then
                    gvPrices.HeaderRow.Cells(E_PricesGrid.NETPRICE).Text &= " (" & SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerCurrency, False) & ")"
                    gvPrices.HeaderRow.Cells(E_PricesGrid.TOTAL).Text &= " (" & SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerCurrency, False) & ")"
                End If
            Catch ex As Exception
            End Try

            For Each r As GridViewRow In gvDocuments.Rows
                If r.Cells(E_DocumentsGrid.FName).Text.ToUpper.Contains("DRW_") Then 'ext.ToUpper.Contains("SHEET_")
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Temp_fileFolderPath, r.Cells(E_DocumentsGrid.FolderArr).Text, False)
                    Exit For
                End If
            Next

            If Not IsPostBack OrElse _doCheckLanguage = True Then
                SetTabs()
            End If

            _sFileName = ""

            Try

                ' WichDivDrawingToShow()
                GetAccountDetails()

            Catch ex As Exception
                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES Prices_PreRender", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

                GeneralException.BuildError(Page, ex.Message)
            End Try


            Try
                Dim sYal As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._AllRedy3DAlertShows, False)
                Dim seSt As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationStatus, False)
                If seSt <> clsQuatation.e_QuotationStatus.Exist_QutOpenedFromQuotationList AndAlso sYal = "NO" Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedy3DAlertShows, "YES1")
                    AllRedy3DAlertShows.Text = "YES1"
                ElseIf sYal = "YES1" Or sYal = "YES" Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedy3DAlertShows, "YES")
                    AllRedy3DAlertShows.Text = "YES"

                End If


                If seSt = clsQuatation.e_QuotationStatus.Exist_QutOpenedFromQuotationList Then
                    hfExistQ.Value = "YES"
                Else
                    hfExistQ.Value = "NO"
                End If
            Catch ex As Exception

            End Try

            If DeletedBuildreport = True Then
                Dim sssD As String = "Quotation report is being prepared according"
                Dim sssD2 As String = "to the selected language."
                If TemporarilyQuotation Then
                    sssD = "The Technical Offer report is being prepared according"
                End If
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ReportDeleteAlert", "ReportDeleteAlert('" & sssD & "','" & sssD2 & "');", True)
            End If




            Try
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "CheckDivWidthPrice", "SetDivHeightWithScrollPrice();", True)
            Catch ex As Exception

            End Try
            If ConfigurationManager.AppSettings("AS400GetData") = "NO" AndAlso StateManager.GetValue(StateManager.Keys.s_loggedEmail, False) <> "" AndAlso StateManager.GetValue(StateManager.Keys.s_loggedEmail, False).Contains("@") Then
                ShowOooPsAlert()
            End If

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES Prices_PreRender", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)

        End Try

        DeletedBuildreport = False



        Try
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetLanControlsCaptionCon", "SetCaptionForLabelsCon()", True)
        Catch ex As Exception

        End Try

    End Sub


    Private Sub checkFilesExist()
        Try

            hfBSONfolder.Value = ConfigurationManager.AppSettings("DrawingReportsLink").ToString
            'http://iquote-prod/iQuote/BSON/0001d29d-947a-4a26-a1cf-1a9fa70aa69a.bson/

            Dim isdebugmode As String = ConfigurationManager.AppSettings("IsDebugMode")
            Dim sBranchCode As String = clsBranch.ReturnActiveBranchCodeForDocuments

            If isdebugmode.ToString.ToUpper = "TRUE" Then
                txtPdfViewsrc4.Text = ConfigurationManager.AppSettings("DrawingReports") & sBranchCode & "\DRW_" & Format(CInt(clsQuatation.ACTIVE_QuotationNumber), "0000000") & ".pdf"
            Else
                txtPdfViewsrc4.Text = ConfigurationManager.AppSettings("DrawingReportsLink") & "CATIAREPORTS/" & sBranchCode & "/DRW_" & Format(CInt(clsQuatation.ACTIVE_QuotationNumber), "0000000") & ".pdf"
            End If
            PdfView.Attributes("Src") = txtPdfViewsrc4.Text
            Dim seSt As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationStatus, False)
            If seSt = clsQuatation.e_QuotationStatus.Exist_QutOpenedFromQuotationList Then
                BsonTextQutNo.Text = "EXIST"
                BsonTextQutNoX.Text = "EXIST"
            Else
                If isdebugmode.ToString.ToUpper = "True" Then
                    BsonTextQutNo.Text = ConfigurationManager.AppSettings("TempFilesfOLDEERlOCATION") & clsQuatation.ACTIVE_QuotationNumber & "_BSON.txt".ToString
                    BsonTextQutNoX.Text = ConfigurationManager.AppSettings("TempFilesfOLDEERlOCATION") & clsQuatation.ACTIVE_QuotationNumber & "_BSON_X.txt".ToString
                Else
                    BsonTextQutNo.Text = ConfigurationManager.AppSettings("DrawingReportsLink") & "TempReportsAndBSON/" & clsQuatation.ACTIVE_QuotationNumber & "_BSON.txt".ToString
                    BsonTextQutNoX.Text = ConfigurationManager.AppSettings("DrawingReportsLink") & "TempReportsAndBSON/" & clsQuatation.ACTIVE_QuotationNumber & "_BSON_X.txt".ToString
                End If

            End If

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES checkFilesExist", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

        End Try



    End Sub

    Private Sub btnSendMail_Click(sender As Object, e As EventArgs) Handles btnSendMail.Click, btnSendMailHT.Click
        'Try

        '    FillDocuments()
        '    'gvDocuments.Rows.Count > 0 AndAlso
        '    If txtEmailAdd.Text.ToString.Trim.Contains("@") AndAlso txtEmailAdd.Text.ToString.Trim.Count > 5 Then
        '        Dim ListOfFiles As New List(Of String)
        '        'Dim StorePath As String = ConfigurationManager.AppSettings("DrawingReports")
        '        'StorePath = ConfigurationManager.AppSettings("ReportTempFolder")
        '        ' DrawingReports="D:\Documents\Visual Studio 2019\SemiStandard\DrawingReports\Catia\"
        '        ' ReportTempFolder="D:\Documents\Visual Studio 2019\SemiStandard\SemiPDF\"
        '        If gvDocuments.Rows.Count > 0 Then
        '            For r As Integer = 0 To gvDocuments.Rows.Count - 1
        '                'ListOfFiles.Add(StorePath & gvDocuments.Rows(r).Cells(E_DocumentsGrid.FName).Text)
        '                'ListOfFiles.Add(StorePath & gvDocuments.Rows(r).Cells(E_DocumentsGrid.FName).Text)
        '                ListOfFiles.Add(gvDocuments.Rows(r).Cells(E_DocumentsGrid.FolderArr).Text & "\" & gvDocuments.Rows(r).Cells(E_DocumentsGrid.FName).Text)
        '            Next
        '        End If
        '        'clsMail.SendDrawing(_QutNo, _BranchCode, StorePath & FileName,)
        '        clsMail.SendDrawing(txtEmailAdd.Text, _QutNo, _BranchCode, "", ListOfFiles)
        '    ElseIf Not txtEmailAdd.Text.ToString.Contains("@") Then
        '        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertSendM", "EncorrectMAilMessage('1');", True)
        '    ElseIf gvDocuments.Rows.Count < 1 Then

        '    End If

        'Catch ex As Exception

        '    GeneralException.WriteEventErrors(ex.Message)
        '    If ex.Message.ToUpper.Contains("MAILBOX UNAVAILABLE") Then
        '        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertSendM2", "EncorrectMAilMessage('3');", True)
        '    End If
        '    'GeneralException.WriteEvent("Build2Dbson", ex.Message)
        'End Try
    End Sub

    Private Sub SendQutMailInfo(MailType As String, phoneNo As String, semal As String, OrderQty As String)

        If MailType = "SUBMITTED" Then
            Try

                FillDocuments()
                If lblSendTemporaryIdx.Value.ToString <> "" Then

                    If lblSendTemporaryIdx.Value.ToString <> "" AndAlso lblSendTemporaryIdx.Value.ToString.Trim.Contains("@") Then
                        Dim ListOfFiles As New List(Of String)
                        If gvDocuments.Rows.Count > 0 Then
                            For r As Integer = 0 To gvDocuments.Rows.Count - 1
                                ListOfFiles.Add(gvDocuments.Rows(r).Cells(E_DocumentsGrid.FolderArr).Text & "\" & gvDocuments.Rows(r).Cells(E_DocumentsGrid.FName).Text)
                            Next
                        End If
                        clsMail.SendDrawing("", lblSendTemporaryIdx.Value, _QutNo, _BranchCode, "", ListOfFiles, Server.MapPath("~"))

                    End If

                End If
            Catch ex As Exception
                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES SendQutMailInfo", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

                If ex.Message.ToUpper.Contains("MAILBOX UNAVAILABLE") Then
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertSendM2", "EncorrectMAilMessage('3');", True)
                End If
            End Try
        ElseIf MailType = "ORDERED" Then
            Try

                FillDocuments()
                If semal <> "" AndAlso semal.ToString.Trim.Contains("@") Then
                    Dim ListOfFiles As New List(Of String)
                    If gvDocuments.Rows.Count > 0 Then
                        For r As Integer = 0 To gvDocuments.Rows.Count - 1

                            ListOfFiles.Add(gvDocuments.Rows(r).Cells(E_DocumentsGrid.FolderArr).Text & "\" & gvDocuments.Rows(r).Cells(E_DocumentsGrid.FName).Text)

                        Next
                    End If
                    Dim sNetPrice As String = ""
                    Try
                        Dim dtPri As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "")

                        For Each ret As DataRow In dtPri.Rows
                            If ret.Item("QTY").ToString = OrderQty Then
                                sNetPrice = Format(CDbl(ret.Item("NETPRICE").ToString), "#.##")
                            End If
                        Next

                    Catch ex As Exception

                    End Try

                    clsMail.SendOrder(semal, _QutNo, _BranchCode, "", ListOfFiles, Server.MapPath("~"), phoneNo, OrderQty, sNetPrice)
                End If
            Catch ex As Exception
                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES SendQutMailInfo", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
                If ex.Message.ToUpper.Contains("MAILBOX UNAVAILABLE") Then
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertSendM2", "EncorrectMAilMessage('3');", True)
                End If
            End Try
        End If

    End Sub


    Private Sub SendQutMailInfoToTichnical()
        Try

            FillDocuments()
            Dim sEmailAddress As String = ConfigurationManager.AppSettings("MailFrom")

            Dim ListOfFiles As New List(Of String)
            If gvDocuments.Rows.Count > 0 Then
                For r As Integer = 0 To gvDocuments.Rows.Count - 1
                    ListOfFiles.Add(gvDocuments.Rows(r).Cells(E_DocumentsGrid.FolderArr).Text & "\" & gvDocuments.Rows(r).Cells(E_DocumentsGrid.FName).Text)
                Next
            End If
            clsMail.SendDrawing("For Support, error build drawing", sEmailAddress, _QutNo, _BranchCode, "", ListOfFiles, Server.MapPath("~"))

        Catch ex As Exception

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES SendQutMailInfoToTichnical", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            If ex.Message.ToUpper.Contains("MAILBOX UNAVAILABLE") Then
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertSendM2", "EncorrectMAilMessage('3');", True)
            End If
        End Try
    End Sub

    Private Sub VisiblityItems()
        Try
            lblQDtitleSM.Text = ""

            If TemporarilyQuotation Then

                ' Dim s48 As String = "<span style=color:red;>- Valid for " & ConfigurationManager.AppSettings("TemporaryExpiryDateText") & "</span>"
                Dim s48 As String = "<span style=color:red;>- " & _QuatationDetailsValidTemp
                '_QuatationDetailsValidTemp
                s48 = _QuatationDetailsTemp & " " & s48 '"Temporary Technical Offer " & s48
                lblQDtitle.Text = s48
                'upPnlAll_Documentsdiv.Style.Add("height", "130px")
            Else
                'divDocs.Visible = True
                lblQDtitle.Text = _QuatationDetails ' "Quotation Details"
                Try
                    If CType(Master.FindControl("lblUserInformation"), Label).Text <> "" AndAlso CType(Master.FindControl("lblUserInformation"), Label).Visible = True Then
                        lblQDtitleSM.Text = " - CS. no: " & SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.CustomerNumber, False)
                        lblQDtitleSM.Text &= " - CS. name: " & SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.CustomerName, False)
                    End If
                Catch ex As Exception

                End Try
            End If

            gvPrices.Columns(E_PricesGrid.GP).Visible = False
            gvPrices.Columns(E_PricesGrid.COSTPRICE).Visible = False

            If clsQuatation.IsTemporary_Quotatiom Then

                lblcurrency.Style.Add("display", "none")
                lblCustomerCurrency.Style.Add("display", "none")
                lblDelivery.Style.Add("display", "none")
                lblDelv.Style.Add("display", "none")

            End If

            'SaveButton_Style()

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES VisiblityItems", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
        End Try

    End Sub


    Protected Sub UploadFile(fileType As FileUpload, sFileName As String)
        Try

            Dim folderPath As String = ConfigurationManager.AppSettings("FileUploadDocMngPath").ToString.Trim
            Dim QutNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
            Dim BC As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)


            Dim sf As String = folderPath & Path.GetFileName(fileType.PostedFile.FileName)
            fileType.SaveAs(sf)

            Dim sFolderPath As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FolderPath, False)

            'Dim s As String = "#LocalFile=" & sf
            's &= "#sFolderPath=" & sFolderPath
            's &= "#User=DBO"
            's &= "#BranchCode=" & clsBranch.ReturnActiveBranchCodeForDocuments
            's &= "#StorageFolder=" & clsBranch.ReturnActiveStorageFolderForDouuments

            Try
                clsUpdateData.UpdateQuotationUploadFilesServiceResult(BC, QutNo, "")
            Catch ex As Exception

            End Try

            Documents.UploadLocalFileToDocuments(sf, sFolderPath, "DBO", clsBranch.ReturnActiveBranchCodeForDocuments, clsBranch.ReturnActiveStorageFolderForDouuments, sFileName)

        Catch ex As Exception
            GeneralException.BuildError(Page, "UploadFile-" & ex.Message)
        End Try

    End Sub

    Private Sub btnSaveQuotation_Click(sender As Object, e As EventArgs) Handles btnSaveQuotation.Click
        Try
            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagPricesChanged, False).ToString.ToUpper = "TRUE" Then

                SavedButtonClickecd()

            End If
            FillPriceGrid()
        Catch ex As Exception
            FillPriceGrid()
        End Try

    End Sub
    Private Sub SavedButtonClickecd()
        Try
            EnableForm()
            If btnSaveQuotation.Enabled = True Then
                FillDocumentDone = False
                SaveQuotationLocal()
                FillPriceGrid()
                CreateRep(True, False)
                FillPriceGrid()
                txtFlagPricesChanged.Text = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagPricesChanged, False).ToString.ToUpper
            End If
        Catch ex As Exception
            FillPriceGrid()
        End Try

    End Sub
    Private Sub btnDoDeleteQuotation_Click(sender As Object, e As EventArgs) Handles btnDoDeleteQuotation.Click
        Try
            EnableForm()
            If btnDelQut.Enabled = True Then
                Dim sQut As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
                Dim sBranchCode As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)
                Dim FolderPath As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FolderPath, False)
                If IsNumeric(sQut) AndAlso sBranchCode <> "" Then
                    clsUpdateData.DeleteQuotation(sQut, sBranchCode, FolderPath, True)
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.DoCheck_LogIN, "NO")

                    Try
                        Response.Redirect("../Default.aspx?iqlang=" & utl.ReturnParamLanguage(Request("iqlang")) & "&repLang=" & utl.ReturnReportLanguage(Request("repLang")), False)
                    Catch ex As Exception
                        Response.Redirect("../Default.aspx", False)
                    End Try
                End If
            End If


        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES btnDoDeleteQuotation_Click", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

        End Try

    End Sub



    Private Sub BuildReport(repLang As String, rErepTr As String)
        Dim BranchCode As String = ""
        Dim AS400Number As String = ""
        Dim QuotaionNo As String = ""
        Try


            If ConfigurationManager.AppSettings("REPORT_ACTIVE") = "YES" Then

                Try
                    Dim sLanguageId As String = 1

                    Try
                        QuotaionNo = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber).ToString
                        Dim bc As String = clsBranch.ReturnActiveBranchCodeForDocuments
                        clsUpdateData.UpdateQuotationUploadFilesServiceResult(bc, QuotaionNo, CryptoManagerTDES.Decode(repLang))
                    Catch ex As Exception

                    End Try

                    Dim dtB As DataTable = clsBranch.GetBranchDetails(CryptoManagerTDES.Decode(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.selectedReportLanguage, False)))

                    If Not dtB Is Nothing AndAlso dtB.Rows.Count > 0 Then
                        sLanguageId = dtB.Rows(0).Item("LanguageID").ToString
                    End If

                    Dim selectedLanguage As String = "pnh0tFBOF/I=" '"ZZ"
                    Try

                        If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.selectedReportLanguage) Is Nothing Then
                            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.selectedReportLanguage).ToString <> "" AndAlso SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.selectedReportLanguage).ToString.Length > 0 Then
                                selectedLanguage = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.selectedReportLanguage).ToString
                            End If
                        End If
                    Catch ex As Exception

                    End Try

                    Dim AS400_Line As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400RowNumber, False)
                    AS400Number = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number, False)
                    If AS400Number = "0" Or clsQuatation.IsTemporary_Quotatiom Then
                        AS400Number = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
                    End If
                    If IsNumeric(AS400Number) Then
                        AS400Number = Format(CInt(AS400Number), "0000000")
                    End If
                    Dim AS400Year As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationDate, False)
                    Dim ReportType As String = clsQuatation.IsTemporary_Quotatiom.ToString
                    Dim QuotationDate As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationDate, False)


                    Dim sLink As String = "/Reports/MainQuotationDataReport.aspx?"
                    Dim sFileName As String = "QT_"
                    Dim TmpQut As String = clsQuatation.IsTemporary_Quotatiom
                    If TmpQut = "False" Then
                        sFileName = "REPORT_"
                    End If

                    BranchCode = clsBranch.ReturnActiveBranchCodeForDocuments ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode).ToString
                    Dim FolderPath As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FolderPath).ToString
                    Dim sStorageFolder As String = clsBranch.ReturnActiveStorageFolderForDouuments ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode).ToString

                    sFileName = GetReportName(FolderPath, repLang, sFileName, AS400Number, False)

                    Dim sBN As String = clsBranch.GetBranchNumberCode(BranchCode)

                    Dim s As String = ""
                    s = s & "FileName=" & Server.UrlEncode(CryptoManager.Encode(sFileName))
                    s &= "&BranchCode=" & Server.UrlEncode(CryptoManager.Encode(BranchCode))
                    s &= "&QuotationNum=" & Server.UrlEncode(CryptoManager.Encode(AS400Number))
                    s &= "&Temporaray=" & Server.UrlEncode(CryptoManager.Encode(TmpQut))
                    s &= "&LanguageId=" & Server.UrlEncode(CryptoManager.Encode(sLanguageId))
                    s &= "&AS400_Line=" & Server.UrlEncode(CryptoManager.Encode(AS400_Line))
                    s &= "&BranchNum=" & Server.UrlEncode(CryptoManager.Encode(sBN))
                    s &= "&AS400Year=" & Server.UrlEncode(CryptoManager.Encode(AS400Year))
                    s &= "&StorageFolder=" & Server.UrlEncode(CryptoManager.Encode(sStorageFolder))
                    s &= "&ReportType=" & Server.UrlEncode(CryptoManager.Encode(ReportType))
                    s &= "&DrawingName=" & Documents.BuildandGetDocumentName 'CatiaDrawing.getFileName
                    s &= "&QuotationDate=" & Server.UrlEncode(CryptoManager.Encode(QuotationDate))
                    s &= "&FolderPath=" & Server.UrlEncode(CryptoManager.Encode(FolderPath))
                    s &= "&selectedLanguage=" & selectedLanguage 'Server.UrlEncode(CryptoManagerTDES.Encode(selectedLanguage))
                    s &= "&DCultStartW=" & LocalizationManager.GetCurentStartWithDateCulture 'D=Day    'M=Month
                    s &= "&NSup=" & LocalizationManager.GetCurentSaparetaorNumberCulture 'P="."    C=","
                    s &= "&rErepTr=" & rErepTr

                    Dim ServerName As String
                    ServerName = Page.Request.Url.Scheme & Uri.SchemeDelimiter & ConfigurationManager.AppSettings("PrintReportServerHost")

                    If Page.Request.Url.Port <> 80 And Page.Request.Url.Port <> 443 Then
                        ServerName &= ":" & Page.Request.Url.Port
                    End If

                    Dim request As WebRequest = WebRequest.Create(ServerName & sLink & s)

                    request.Credentials = CredentialCache.DefaultCredentials
                    request.Timeout = 180000

                    Dim Response As HttpWebResponse
                    Dim dataStream As Stream
                    Dim responseFromServer As String
                    Dim reader As StreamReader

                    Response = CType(request.GetResponse(), HttpWebResponse)
                    dataStream = Response.GetResponseStream()
                    reader = New StreamReader(dataStream)
                    responseFromServer = reader.ReadToEnd()
                    reader.Close()
                    dataStream.Close()
                    Response.Close()

                    _sFileName = sFileName

                Catch ex As GeneralException
                    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES BuildReport", ex.Message, BranchCode, AS400Number, AS400Number, "")
                    GeneralException.BuildError(Page, "Could not build the report during system problem")
                Catch ex As Exception
                    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES BuildReport", ex.Message, BranchCode, AS400Number, AS400Number, "")
                    GeneralException.BuildError(Page, "Could not build the report during system problem")
                End Try

            End If


        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES BuildReport", ex.Message, clsBranch.ReturnActiveBranchCodeState, QuotaionNo, AS400Number, "")

            GeneralException.BuildError(Page, "Could not build the report during system problem")
        End Try
    End Sub

    Private Function GetReportName(FolderPath As String, repLang As String, sFileName As String, AS400Number As String, checkiffileexist As Boolean) As String
        Try


            Dim ss() As String
            Dim sISOnameReport As String = repLang

            Dim sSpn As String = "pnh0tFBOF/I="


            Dim dtRAB As DataTable = clsBranch.GetBranchLanguages(CryptoManagerTDES.Decode(sISOnameReport), sISOnameReport, "")
            Dim isoRap As String = "_"

            If Not dtRAB Is Nothing AndAlso dtRAB.Rows.Count > 0 Then

                If sISOnameReport = sSpn Then
                    For Each r1 As DataRow In dtRAB.Rows
                        If r1.Item("BranchCode").ToString.Trim = "ZZ" Then
                            isoRap &= r1.Item("ISOCode").ToString.Trim
                            Exit For
                        End If
                    Next
                Else
                    isoRap &= dtRAB.Rows(0).Item("ISOCode").ToString
                End If
            End If

            If checkiffileexist Then
                Try
                    For Each r As GridViewRow In gvDocuments.Rows
                        If r.Cells(E_DocumentsGrid.FName.ToString).Text.Contains("isoRap") Then
                            Return "YES"
                        Else
                            Return "NO"
                        End If
                    Next
                Catch ex As Exception

                End Try
            Else
                Try

                    ss = FolderPath.Split("\")
                    sFileName = "REPORT_" & ss(1) & isoRap & ".pdf"
                Catch ex As Exception
                    sFileName = "REPORT_" & AS400Number & isoRap & ".pdf"
                End Try
            End If



            Return sFileName

        Catch ex As Exception
            Return sFileName.ToString
        End Try
    End Function

    Private Sub btnSendTemporaryId_Click(sender As Object, e As EventArgs) Handles btnSendTemporaryId.Click
        Dim fcdSubmit As String = ""
        Try

            If GAL.CheckIfCanUpdateGAL() = True Then
                Dim ssb As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Submitted, False)

                clsUpdateData.UpdateQuotationSubmitted("1", lblSendTemporaryIdx.Value.ToString)
                If lblSendTemporaryIdx.Value.ToString <> "" Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.SubmittedEmail, lblSendTemporaryIdx.Value.ToString)
                End If
                If ssb <> "1" Then
                    If Not TemporarilyQuotation Then
                        Dim AS400Number As String = clsQuatation.ACTIVE_GALQuotationNumber ' clsQuatation.ACTIVE_QuotationNumber
                        Dim custno As String = clsQuatation.ACTIVE_UseCustomerNo
                        fcdSubmit = GAL.fcdSubmitOrderGAL(clsBranch.ReturnActiveBranchCodeState, custno, AS400Number, "SUBMIT", "")
                        'Dim fcdSubmitz As String = GAL.fcdSubmit(clsBranch.ReturnActiveBranchCodeState, "v2", custno, AS400Number)
                        If fcdSubmit <> "DONE" Then
                            If fcdSubmit.ToString.ToUpper <> "SECOND" Then
                                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.AS400_ERROR.ToString, "Submit vendor error", fcdSubmit, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
                            End If
                            clsUpdateData.UpdateQuotationSubmitted("0", lblSendTemporaryIdx.Value.ToString)
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Submitted, "0")
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertSubmitEr2", "SubmitErrorMsg('Currently AS400 update is not available!');", True)
                        Else
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Submitted, "1")

                        End If
                    End If

                End If


                Try
                    hfSubmetted.Value = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Submitted, False)
                    If hfSubmetted.Value = "1" Then
                        hfSubmetted.Value = _SubmitEmail_2 ' "Email Quotation Details"
                    Else
                        hfSubmetted.Value = _SubmitEmail_1 ' "Submit Quotation"
                    End If
                Catch ex As Exception
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Submitted, "1")
                    hfSubmetted.Value = _SubmitEmail_2 ' "Email Quotation Details"
                End Try
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationStatus, clsQuatation.e_QuotationStatus.Exist_QutOpenedFromQuotationList)
                EnableForm()

            Else
                If clsBranch.ReturnActiveBranchCodeState <> "ZZ" Then
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertSubmitEr", "SubmitErrorMsg('Currently AS400 update is not available!');", True)

                End If
            End If

            SendQutMailInfo("SUBMITTED", "", "", "")

            FillPriceGrid()
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES btnSubmitMessagePrices_Click", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            If fcdSubmit <> "DONE" Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Submitted, "0")
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertSubmitEr1", "SubmitErrorMsg('Currently AS400 update is not available!.');", True)
            End If
        End Try
    End Sub

    Private Sub btnSubmitMessagePrices_Click(sender As Object, e As EventArgs) Handles btnSubmitMessagePrices.Click
        Try

            Dim sMsg As String = "There is no option to show prices. Please Contact iQuote Support."

            Dim sQutNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
            Dim sCn As String = StateManager.GetValue(StateManager.Keys.s_CustomerName, True)
            Dim sCno As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
            Dim sCompN As String = StateManager.GetValue(StateManager.Keys.s_CompanyName, True)
            Dim sBc As String = StateManager.GetValue(StateManager.Keys.s_BranchCode, True)
            Dim sLe As String = StateManager.GetValue(StateManager.Keys.s_loggedEmail, True)

            sMsg = sMsg & "<p style='font-family:Oswald; font-size:15px; '><br>"
            sMsg = sMsg & "<br>"
            sMsg = sMsg & "QuotationNumber - " & sQutNo & " - <br>"
            sMsg = sMsg & "BranchCode - " & sBc & " - <br>"
            sMsg = sMsg & "CompanyName - " & sCompN & " - <br>"
            sMsg = sMsg & "CustomerNumber - " & sCno & " - <br>"
            sMsg = sMsg & "CustomerName - " & sCn & " - <br>"
            sMsg = sMsg & "Logged Email - " & sLe & " - <br></p>"


            clsMail.SendEmailWithoutAttachment(ConfigurationManager.AppSettings("MailFrom").ToString.Trim, "iQuote Oops Message", sMsg, True, True, Server.MapPath("~"), False, Nothing, "", "", "")

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES btnSubmitMessagePrices_Click", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
            GeneralException.BuildError(Page, ex.Message)
        End Try
    End Sub

    Private Sub CreateRep(UpdateReports As Boolean, fromPlaceOrder As Boolean)
        Try
            If UpdateReports Then
                Dim repname As String = ""
                Dim didrep As Boolean = False
                For Each r As GridViewRow In gvDocuments.Rows

                    didrep = True
                    repname = r.Cells(E_DocumentsGrid.FName).Text
                    If repname.Contains("REPORT_") AndAlso repname.Length > 19 Then
                        repname = repname.ToUpper.Substring(repname.Length - 7, 7)
                        If repname.Contains("_") AndAlso repname.Contains(".PDF") Then
                            Dim bc As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)
                            Documents.DeleteFileNotBelongToQutReports(bc, clsBranch.ReturnActiveStorageFolderForDouuments, r.Cells(E_DocumentsGrid.FolderArr).Text, r.Cells(E_DocumentsGrid.FName).Text, r.Cells(E_DocumentsGrid.FileId).Text, "DBO", True)
                            If Not fromPlaceOrder Then
                                DeletedBuildreport = True
                            End If
                        End If
                    End If
                Next

            End If

            BuildReport(utl.ReturnReportLanguage(Request("repLang")), Request.QueryString("rErepTr").ToString)
        Catch ex As Exception

        End Try
        FillDocuments()
        FillPriceGrid()
    End Sub
    Private Sub btnDoDeleteDocument_Click(sender As Object, e As EventArgs) Handles btnDoDeleteDocument.Click
        Try

            Dim bc As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)

            Documents.DeleteFileNotBelongToQutReports(bc, clsBranch.ReturnActiveStorageFolderForDouuments, lblDOCSFolderPath.Text, lblDOCSFileName.Text, lblDOCSFileId.Text, "DBO", False)

            FillDocuments()
            FillPriceGrid()
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES btnDoDeleteDocument_Click", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
        End Try
    End Sub

    Private Sub btnFillGPHided_Click(sender As Object, e As EventArgs) Handles btnFillGPHided.Click
        Try
            FillPriceGrid()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub lvParams_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles lvParams.ItemDataBound

        Try

            If (e.Item.ItemType = ListViewItemType.DataItem) Then

                If CType(e.Item.FindControl("lblVisibleTable"), Label).Text.ToString.ToUpper = "TRUE" Then
                    Dim sAd As String = StateManager.GetValue(StateManager.Keys.s_IsAdmin, True)
                    If sAd = "YES" Then
                        CType(e.Item.FindControl("lblParamArray"), Label).Style.Add("color", "red")
                    Else
                        e.Item.Visible = False
                    End If
                ElseIf CType(e.Item.FindControl("lblMeasure"), Label).Text.ToString.ToUpper = "" Then
                    e.Item.Visible = False
                ElseIf IsNumeric(CType(e.Item.FindControl("lblMeasure"), Label).Text.ToString) Then
                    CType(e.Item.FindControl("lblMeasure"), Label).Text = LocalizationManager.CulturingNumber(CType(e.Item.FindControl("lblMeasure"), Label).Text, False)
                End If

            End If

            CType(e.Item.FindControl("lblParamArray"), Label).Text = clsLanguage.GetLanguageLabels(CType(e.Item.FindControl("lblParamArray"), Label).Text)
            Try
                CType(e.Item.FindControl("lblMeasureCaption"), Label).Text = clsLanguage.GetLanguageLabels(CType(e.Item.FindControl("lblMeasure"), Label).Text)
                CType(e.Item.FindControl("lblMeasure"), Label).CssClass = "DisplayNone"
            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Tab2_Click(sender As Object, e As EventArgs) Handles Tab2.Click
        t2cLICK()

    End Sub

    Private Sub t2cLICK()
        'Exit Sub
        Try

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempAllReadyTriedtoBuildDrawingAutomaticly, "YES3")
            txtAllReadyTriedtoBuildDrawing.Text = "YES3"

            Dim bFileFound As Boolean = False
            Dim sQutNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
            Dim sQutNoAs400 As String = clsQuatation.ACTIVE_QuotationNumber

            If Documents.CHECKandReaname_ReportName(Format(CInt(sQutNo), "0000000"), Format(CInt(sQutNoAs400), "0000000")) = True Then
                'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempRefreshPriceForm, "YES")
                ' SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DrawingName, "DRW_" & Format(CInt(sQutNoAs400), "0000000") & ".pdf")
                'clsUpdateData.UpdateQuotationDrawing()
                '   SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempReportTabToShow, "PDF")
                PdfView.Attributes("Src") = txtPdfViewsrc4.Text
                bFileFound = True
            End If
            FillDocuments()
            If bFileFound = False Then
                Dim sMsg As String = "Error : create catia report , Function name BuildCatiaReportThred<br>"
                sMsg &= "Faild to create report!<br>"
                sMsg &= "Company - " & SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False) & "<br>"
                sMsg &= "Date - " & Now.ToLongTimeString & "<br>"

                GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.PRICES.ToString, "Tab2_Click", sMsg, "", sQutNo, sQutNoAs400, "")

            End If

            FillPriceGrid()

            Dim Drawingexist As Boolean = False

        Catch ex As Exception
            Dim sMsg As String = "Error : create catia report , Function name BuildCatiaReportThred<br>"
            sMsg &= "Faild to create report!<br>"
            sMsg &= "Company - " & SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False) & "<br>"
            sMsg &= "Date - " & Now.ToLongTimeString & "<br>"
            Dim sQutNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
            Dim sbc As String = clsBranch.ReturnActiveBranchCodeState
            clsMail.SendEmailWithoutAttachment(ConfigurationManager.AppSettings("MailFrom").ToString.Trim, "SemiStandatrd Message", sMsg, True, False, "", False, Nothing, "", "", "")
        End Try
    End Sub

    Private Sub SetBSON_SRC_NAME()
        Try
            If ConfigurationManager.AppSettings("BSON_ACTIVE") = "YES" Then

                ' Dim sBF As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BSONfolder, False)

                ' If sBF = "" Then
                If BsonTextID.Text = "" Then

                    Dim QuotaionNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
                    Dim bc As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)
                    Dim dt As DataTable = clsQuatation.GetQuatationDetails(bc, QuotaionNo)

                    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                        Dim sBson As String = dt.Rows(0).Item("BSONfolder").ToString.Trim

                        If sBson <> "" Then
                            ' SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BSONfolder, sBson)
                            BsonTextID.Text = sBson
                        End If
                    End If
                    'Else
                    '    BsonTextID.Text = sBF
                End If
            End If

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES SetBSON_SRC_NAME", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
        End Try

    End Sub
    Private Sub TabBSON_Click(sender As Object, e As EventArgs) Handles TabBSON.Click
        Try
            FillPriceGrid()
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES TabBSON_Click", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
        End Try

    End Sub


    'Private Sub WichDivDrawingToShow()


    '    Try

    '        Dim ss As String = txtTabToShow.Text
    '        If ss = Nothing Then ss = "2"
    '        If ss = "" Then ss = "2"

    '        If ss = "2" Then
    '            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert3", "frametoshow('BSON');", True)
    '        Else
    '            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert4", "frametoshow('PDF');", True)
    '        End If


    '    Catch ex As Exception

    '        GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES WichDivDrawingToShow", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

    '    End Try
    'End Sub

    Private Sub gvPrices_Temp_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvPrices_Temp.RowDataBound

        Try

            If e.Row.RowType = DataControlRowType.DataRow Then
                Select Case e.Row.RowIndex
                    Case "0"
                        CType(e.Row.FindControl("txtQuantityTemp"), TextBox).Text = 2
                    Case "1"
                        CType(e.Row.FindControl("txtQuantityTemp"), TextBox).Text = 3
                    Case "2"
                        CType(e.Row.FindControl("txtQuantityTemp"), TextBox).Text = 5
                    Case "3"
                        CType(e.Row.FindControl("txtQuantityTemp"), TextBox).Text = 10
                    Case "4"
                        CType(e.Row.FindControl("txtQuantityTemp"), TextBox).Text = 20
                    Case "5"
                        CType(e.Row.FindControl("txtQuantityTemp"), TextBox).Text = 30
                    Case "6"
                        CType(e.Row.FindControl("txtQuantityTemp"), TextBox).Text = 50
                    Case "7"
                        CType(e.Row.FindControl("txtQuantityTemp"), TextBox).Text = 100
                End Select

                CType(e.Row.FindControl("txtQuantityTemp"), TextBox).CssClass = "QtyEnableCssControl FontFamilyRoboto FontSizeRoboto14 margL" ' Style.Add("border", "Solid")
                CType(e.Row.FindControl("txtNetPriceTemp"), TextBox).CssClass = "PriceEnableCssControl FontFamilyRoboto FontSizeRoboto14"
                CType(e.Row.FindControl("txtTotalTemp"), TextBox).CssClass = "PriceEnableCssControl FontFamilyRoboto FontSizeRoboto14"

            ElseIf e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(0).Text = _PricesGridCo1Quantity
                e.Row.Cells(1).Text = _PricesGridCo1NetPrice
                e.Row.Cells(2).Text = _PricesGridCo1TotalPrice
            End If
        Catch ex As Exception

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES gvPrices_Temp_RowDataBound", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            GeneralException.BuildError(Page, ex.Message)
        End Try

    End Sub

    Private Sub btnCreateSendMail2_Click(sender As Object, e As EventArgs) Handles btnCreateSendMail2.Click
        Try


            SavedButtonClickecd()

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "RunPrcdSendMail", "SendTechnicalOffer();", True)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnRefreshN_Click(sender As Object, e As EventArgs) Handles btnRefreshN.Click
        Try
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyReportBuild, "YES")
            FillDocuments()
            FillPriceGrid()
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempAllReadyTriedtoBuildDrawingAutomaticly, "YES3")
            txtAllReadyTriedtoBuildDrawing.Text = "YES3"
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnDoDeleteDocumentCancel_Click(sender As Object, e As EventArgs) Handles btnDoDeleteDocumentCancel.Click
        Try

            FillPriceGrid()

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "PRICES btnDoDeleteDocumentCancel_Click", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

        End Try
    End Sub

    Private Sub btnSendTemporaryIdTech_Click(sender As Object, e As EventArgs) Handles btnSendTemporaryIdTech.Click
        Try


            SendQutMailInfoToTichnical()
            FillPriceGrid()
        Catch ex As Exception

        End Try
    End Sub


    Private Sub DisableForm(clearValidLabel As Boolean)
        Try


            SetTabs()
            If clearValidLabel Then
                ValidationSummaryLabel.Visible = False
                ValidationSummaryLabel.Text = "&nbsp;"
            End If
            gvPrices.Enabled = False
            gvPrices_Temp.Enabled = False
            gvPrices.Visible = False
            gvPrices_Temp.Visible = False

            lvParams.Enabled = False

            btnSaveQuotation.Enabled = False
            btnDelQut.Enabled = False
            btnDelQut.Visible = False
            btnDelQut.Style.Add("cursor", "Default")
            btnDelQut.BorderColor = Color.Gray

            btnSaveQuotation.CssClass = "FontFamilyRoboto FontSizeRoboto13 QuotationDetailsButtonDis"
            btnDelQut.BackColor = Color.Gray
            btnDelQut.ForeColor = Color.White
            pnlClient.Visible = False
            pnlServer.Visible = False
        Catch ex As Exception

        End Try

    End Sub
    Private Sub EnableForm()
        DisableForm(False)
        Try
            Dim sXd As String = ""
            Dim Submitted As Boolean = True
            Dim Ordered As Boolean = True
            Dim sSubmitted As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Submitted, False)
            If sSubmitted = "0" Or sSubmitted.ToUpper = "FALSE" Then Submitted = False
            Dim sOrdered As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Ordered, False)
            If sOrdered <> "1" AndAlso sOrdered.ToUpper <> "TRUE" Then
                Ordered = False
            End If
            Dim Expierd As Boolean = True
            sXd = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ExpiredDate, False)
            If Not sXd Is Nothing AndAlso sXd <> "" AndAlso IsDate(sXd) Then
                If CDate(sXd) > Now.Date Then
                    Expierd = False
                End If
            End If

            If Not TemporarilyQuotation AndAlso ConfigurationManager.AppSettings("AS400GetData") = "YES" Then
                gvPrices.Visible = True
                sXd = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ExpiredDate, False)

                If Not Expierd Then
                    gvPrices.Enabled = True
                    For Each rr As GridViewRow In gvPrices.Rows

                        If Submitted Or Submitted Then
                            CType(rr.Cells(E_PricesGrid.QTY).FindControl("txtQuantity"), TextBox).Enabled = False

                            If rr.RowIndex = gvPrices.Rows.Count - 1 Then
                                CType(rr.Cells(E_PricesGrid.QTY).FindControl("txtQuantity"), TextBox).Enabled = True
                                Exit For
                            End If

                        Else
                            CType(rr.Cells(E_PricesGrid.QTY).FindControl("txtQuantity"), TextBox).Enabled = True
                        End If
                    Next
                    If ValidationSummaryLabel.Text <> "" Then
                        ValidationSummaryLabel.Visible = True
                    End If

                End If

            Else
                gvPrices_Temp.Visible = True
            End If
            '------------------Delete Button--------------------

            If Not Expierd AndAlso Not Submitted Then
                btnDelQut.Visible = True
                btnDelQut.Enabled = True
                btnDelQut.BackColor = Color.White
                btnDelQut.ForeColor = Color.FromName("#12498a")
                btnDelQut.Style.Add("cursor", "pointer")
            End If


            '----------btnSaveQuotation-----------
            If Not gvPrices Is Nothing AndAlso gvPrices.Rows.Count > 0 Then
                If Not TemporarilyQuotation Then
                    Dim sFlagPricesChanged As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagPricesChanged, False).ToString.ToUpper
                    If sFlagPricesChanged = "TRUE" Then

                        If Not Expierd Then
                            btnSaveQuotation.Enabled = True
                            btnSaveQuotation.CssClass = "FontFamilyRoboto FontSizeRoboto13 QuotationDetailsButton"
                        End If

                    End If

                End If
            End If


            If Ordered Then
                pnlViewOrdered.Visible = True
                pnlViewOrdered.Visible = True
            Else
                pnlViewOrdered.Visible = False
                pnlViewOrdered.Visible = False
            End If

            '----------btnSubmitEmail-----------
            'If Not Expierd AndAlso Not Submitted Then
            Dim strS As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._AllRedyReportBuild, False)
            'strS = "InPROCESS"
            If strS = "YES" Then
                pnlClient.Visible = False
                pnlServer.Visible = True
            Else
                If clsBranch.ReturnActiveBranchCodeState.ToString.ToUpper = "ZZ" AndAlso strS.ToString.ToUpper <> "INPROCESS" Then
                    pnlClient.Visible = False
                    pnlServer.Visible = True
                Else
                    pnlClient.Visible = True
                    pnlServer.Visible = False
                End If

            End If
            'End If


        Catch ex As Exception
            pnlClient.Visible = True
            pnlServer.Visible = False
            btnDelQut.Visible = False
            btnDelQut.Enabled = False
            btnDelQut.BackColor = Color.Gray
            btnDelQut.ForeColor = Color.White
            btnDelQut.Style.Add("cursor", "Default")
            btnDelQut.BorderColor = Color.Gray
            btnSaveQuotation.Enabled = False
            btnSaveQuotation.CssClass = "FontFamilyRoboto FontSizeRoboto13 QuotationDetailsButton"
            gvPrices.Enabled = True
        End Try

    End Sub

    Private Sub Prices_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Try


            ' Dim bvalueFromMaster As Boolean = False
            'Try
            '    Dim master As SemiSTDMaster = TryCast(Me.Master, MasterPage)
            '    If master IsNot Nothing Then
            '        bvalueFromMaster = master.MyQuotetionClick

            '    End If
            'Catch ex As Exception
            '    bvalueFromMaster = False
            'End Try

            'bvalueFromMaster = False

            'If bvalueFromMaster = False Then

            'ValidationSummaryLabel.Text = "Quantity already selected1"

            'Dim dFrl As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._DocumentsFIllDone, False).ToString
            'If dFrl = "YES" Then
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DocumentsFIllDone, "NO")
            FillDocuments()
            'End If '
            'Try
            '        gvDocuments.DataBind()
            '    Catch ex As Exception

            '    End Try

            checkFilesExist()
                VisiblityItems()
                FillPriceGrid()
                FillRevGrid(1)
                FillOrderedGrid()
                EnableForm()
            'End If

        Catch ex As Exception

        End Try

    End Sub




    Protected Sub FillSelectQuantity()

        Try
            If Not TemporarilyQuotation Then
                If Not FillSelectedQtyDone Then
                    FillSelectedQtyDone = True
                    Dim dt As DataTable
                    If gvPrices.DataSource Is Nothing Then
                        dt = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "")
                    Else
                        dt = gvPrices.DataSource
                    End If

                    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                        Dim dtPtemp As New DataTable
                        dtPtemp = dt.Copy

                        'For ir As Integer = 0 To dtPtemp.Rows.Count - 1
                        '    If dtPtemp.Rows(ir).Item("QTY").ToString = "" Then
                        '    Else
                        '        dtPtemp.Rows(ir).Item("NETPRICE") = Format(CDbl(dtPtemp.Rows(ir).Item("NETPRICE")), "#.##")
                        '    End If
                        'Next

                        lstvSelectedPrice.DataSource = dtPtemp

                        lstvSelectedPrice.DataBind()


                        dtPtemp = Nothing
                    End If
                    '--Fill OrderEmail & phone Number--
                    Dim oe As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.OrderedEmail, False)
                    Dim se As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.SubmittedEmail, False)
                    Dim op As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.OrderedPhoneNo, False)

                    Dim cO As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.PricesCustomerOrderNumber, False)
                    Dim cI As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.PricesCustomerItemNumber, False)
                    Dim cQ As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.PricesCustomerAdditionalReq, False)

                    If oe.ToString.Trim <> "" Then
                        txtSelectMail.Text = oe
                        hfSelectedEmail.Value = oe
                    Else
                        txtSelectMail.Text = se
                        hfSelectedEmail.Value = se
                    End If
                    txtSelectPhoneNo.Text = op
                    hfSelectedPhoneNo.Value = op

                    txtCustomerOrderN1.Text = cO
                    txtCustomerItemN2.Text = cI
                    txtCustomerAdditionalReq.Text = cQ
                    hfCustomerOrderN.Value = cO
                    hfCustomerItemN.Value = cI
                    hfCustomerReqN.Value = cQ

                    'hfSelectedEmail.Value = oe
                    '----
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub


    Private Function UploadOrderFile(ByRef wrongfiletype As Boolean) As Boolean
        Try
            wrongfiletype = False
            If Not fuOrder.PostedFile Is Nothing AndAlso fuOrder.FileName <> "" Then
                If Not String.IsNullOrEmpty(fuOrder.PostedFile.FileName) Then
                    Dim ft As String = fuOrder.FileName.ToString.ToUpper

                    Dim contentTypeA As String = fuOrder.PostedFile.ContentType

                    If Not Documents.IsAllowedMimeTypeS(contentTypeA) Then
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "fileUploadAlert", "fileUploadAlert();", True)
                        wrongfiletype = True
                        Return False
                    Else
                        If Not ft.Contains(".PDF") AndAlso Not ft.Contains(".PNG") AndAlso Not ft.Contains(".JPEG") Then
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "fileUploadAlert", "fileUploadAlert();", True)
                            wrongfiletype = True
                            Return False
                        Else

                            If fuOrder.PostedFile.ContentLength > 9999999 Then
                                Try
                                    fuOrder.Dispose()
                                    fuOrder.PostedFile.InputStream.Dispose()
                                    fuOrder = Nothing
                                Catch ex As Exception
                                End Try
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertMax", "ShowFileToLarge();", True)
                            Else
                                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempAttachOrderFile, "YES")
                                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.OrderedPhoneNo, hfSelectedPhoneNo.Value) 'txtSelectPhoneNo.Text
                                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.OrderedEmail, hfSelectedEmail.Value) 'txtSelectMail.Text
                                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerOrderNumber, hfCustomerOrderN.Value) 'txtSelectPhoneNo.Text
                                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerItemNumber, hfCustomerItemN.Value) 'txtSelectPhoneNo.Text
                                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerAdditionalReq, hfCustomerReqN.Value) 'txtSelectPhoneNo.Text

                                hfSelectFile.Value = "YES"
                                lblApprovalDocs.Text = "Approval Development." & fuOrder.FileName.Substring(fuOrder.FileName.LastIndexOf(".") + 1)
                                UploadFile(fuOrder, "Approval Development." & fuOrder.FileName.Substring(fuOrder.FileName.LastIndexOf(".") + 1))

                                Try
                                    fuOrder.Dispose()
                                    fuOrder.PostedFile.InputStream.Dispose()
                                    fuOrder = Nothing
                                Catch ex As Exception

                                End Try
                            End If

                            Return True
                        End If
                    End If



                End If
            End If

        Catch ex As Exception

        End Try
        Return False
    End Function

    Private Function CheckIfAllowOrder() As Boolean
        Try

        Catch ex As Exception
            Return False
        End Try
    End Function

    Protected Sub btnPlaceOrder_Click(sender As Object, e As EventArgs)
        Try

            'Dim sCustomerOrderNumber As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.PricesCustomerOrderNumber)
            'Dim sCustomerItemNumber As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.PricesCustomerItemNumber)
            'Dim sCustomerAdditionalReq As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.PricesCustomerAdditionalReq)

            Dim sCustomerOrderNumber As String = hfCustomerOrderN.Value.ToString.Trim
            Dim sCustomerItemNumber As String = hfCustomerItemN.Value.ToString.Trim
            Dim sCustomerAdditionalReq As String = hfCustomerReqN.Value.ToString.Trim
            Dim stxtSelectMail As String = hfSelectedEmail.Value
            Dim stxtSelectQTY As String = hfSelectedQuantity.Value
            Dim ssb As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Submitted, False)

            'If ssb = "1" AndAlso sCustomerItemNumber <> "" AndAlso sCustomerOrderNumber <> "" AndAlso sCustomerAdditionalReq <> "" AndAlso stxtSelectMail <> "" AndAlso hfSelectedQuantity.Value <> "" Then
            '29.07.24
            If ssb = "1" AndAlso stxtSelectMail <> "" AndAlso hfSelectedQuantity.Value <> "" AndAlso sCustomerOrderNumber <> "" Then ' AndAlso sCustomerItemNumber <> "" AndAlso sCustomerAdditionalReq <> "" Then

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerOrderNumber, sCustomerOrderNumber)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerItemNumber, sCustomerItemNumber)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerAdditionalReq, sCustomerAdditionalReq)

                If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagPricesChanged, False).ToString.ToUpper = "TRUE" Then
                    'If btnUpdateQuotationHided.Enabled = True Then
                    FillDocumentDone = False
                    SaveQuotationLocal()
                    CreateRep(True, True)
                    txtFlagPricesChanged.Text = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagPricesChanged, False).ToString.ToUpper
                    'End If
                End If

                Dim OrderedPhonN As String = hfSelectedPhoneNo.Value ' txtSelectPhoneNo.Text

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.OrderedPhoneNo, OrderedPhonN)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.OrderedEmail, stxtSelectMail)
                'AndAlso OrderedPhonN <> ""

                hfPriceOrdered.Value = ""

                Dim fcdSubmit As String = ""
                Dim AS400Number As String = clsQuatation.ACTIVE_GALQuotationNumber ' clsQuatation.ACTIVE_QuotationNumber
                Dim custno As String = clsQuatation.ACTIVE_UseCustomerNo
                fcdSubmit = GAL.fcdSubmitOrderGAL(clsBranch.ReturnActiveBranchCodeState, custno, AS400Number, "ORDER", hfSelectedQuantity.Value.ToString)


                'Dim fcdSubmitz As String = GAL.fcdSubmit(clsBranch.ReturnActiveBranchCodeState, "v2", custno, AS400Number)
                If fcdSubmit <> "DONE" Then
                    If fcdSubmit.ToString.ToUpper <> "SECOND" Then
                        GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.AS400_ERROR.ToString, "Order vendor error", fcdSubmit, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
                    End If
                    'clsUpdateData.UpdateQuotationSubmitted("0", lblSendTemporaryIdx.Value.ToString)
                    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Submitted, "0")
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertSubmitEr2", "OrderErrorMsg('Currently AS400 update is not available!');", True)
                Else
                    clsUpdateData.UpdateQuotationOrdered(hfSelectedQuantity.Value, stxtSelectMail, OrderedPhonN, sCustomerOrderNumber, sCustomerItemNumber, sCustomerAdditionalReq)
                    Dim semal As String = hfSelectedEmail.Value
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Ordered, "True")
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.OrderedPhoneNo, "")
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.OrderedEmail, "")
                    hfSelectedPhoneNo.Value = ""
                    hfSelectedEmail.Value = ""
                    hfSelectFile.Value = ""
                    'lblApprovalDocs.Visible = False
                    lblApprovalDocs.Text = ""
                    Try
                        If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "") Is Nothing AndAlso SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "").Rows.Count > 0 Then
                            For iss As Integer = 0 To SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "").Rows.Count - 1
                                If CType(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, ""), DataTable).Rows(iss).Item("QTY").ToString = hfSelectedQuantity.Value.ToString AndAlso CType(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, ""), DataTable).Rows(iss).Item("QTY").ToString.Trim <> "" Then
                                    CType(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, ""), DataTable).Rows(iss).Item("OrderedQuantity") = "True"
                                End If
                            Next
                        End If
                    Catch ex As Exception

                    End Try
                    Try
                        If semal <> "" Then
                            SendQutMailInfo("ORDERED", hfSelectedPhoneNo.Value.ToString, semal, hfSelectedQuantity.Value.ToString)
                        End If
                    Catch ex As Exception

                    End Try

                    hfSelectedQuantity.Value = ""
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempAttachOrderFile, "")

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ClosePlaceOrderDialogCall", "ClosePlaceOrderDialog();", True)
                    'PnlPlaceOrder.Enabled = False
                    'PnlPlaceOrder.Visible = False
                End If
            End If

        Catch ex As Exception
            hfSelectedQuantity.Value = ""
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempAttachOrderFile, "")
        End Try
        hfSelectedQuantity.Value = ""
        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempAttachOrderFile, "")

    End Sub

    Protected Sub FillLanguagesPrice()

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

            rptLanguagesListPrice.DataSource = dtLangauges
            rptLanguagesListPrice.DataBind()

            d1 = Nothing
            d2 = Nothing
            d3 = Nothing
            d4 = Nothing
            d5 = Nothing
            d6 = Nothing

        Catch ex As Exception

        End Try

    End Sub

    Private Sub Prices_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try


            If Not IsPostBack OrElse _doCheckLanguage = True Then

                Try
                    If SecurityManager.CheckIfUserNotAuthinticatedwithTheQuote("PRICE") = False Then
                        Response.Redirect("~/Default.aspx", True)
                    End If
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub




    'Protected Sub UploadButton_Click(sender As Object, e As EventArgs) Handles UploadButton.Click
    '    If FileUploadControl.HasFile Then
    '        Dim contentType As String = FileUploadControl.PostedFile.ContentType

    '        ' Check if the MIME type is allowed
    '        If IsAllowedMimeType(contentType) Then
    '            ' Process the file upload
    '            Dim fileName As String = Path.GetFileName(FileUploadControl.FileName)
    '            FileUploadControl.SaveAs(Server.MapPath("~/Uploads/") & fileName)
    '            StatusLabel.Text = "Upload status: File uploaded successfully!"
    '        Else
    '            StatusLabel.Text = "Upload status: Invalid MIME type!"
    '        End If
    '    Else
    '        StatusLabel.Text = "Upload status: No file selected!"
    '    End If
    'End Sub


    Private Sub SetCaptionsLanguage()
        Try
            Dim dv As DataView = clsLanguage.Get_LanguageCaption("Prices")
            If Not dv Is Nothing AndAlso dv.Count > 0 Then
                Dim currentEnum As String = ""
                For i As Integer = 0 To dv.Count - 1
                    currentEnum = dv.Item(i).Item("LanguageEnumCode")
                    Select Case currentEnum
                        Case clsLanguage.e_LanguageConstants.iQuoteMessage
                            hfcapiQuoteMessage.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.ParametersOverview
                            lblParamsDes.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblQD
                            lblQD.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.SelectReportLanguage
                            lblSelectRepLang.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblQut
                            lblQut.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            landQuotationNumber = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)

                        Case clsLanguage.e_LanguageConstants.lblID
                            lblID.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblDelv
                            lblDelv.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblCreatedDate
                            lblCreatedDate.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblLastUpdate
                            lblLastUpdate.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblExpiryDate
                            lblExpiryDate.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblPriceTempAlert
                            lblPriceTempAlert.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblQuotationDr
                            lblQuotationDr.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.hfQuotationSaved
                            hfQuotationSaved.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.hfMyQuotations
                            hfMyQuotations.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)

                        Case clsLanguage.e_LanguageConstants.hf3DlineFirst
                            hf3DlineFirst.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.hf3DlineSecond
                            hf3DlineSecond.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.hfSubmetted
                            _SubmitEmail_1 = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.hfSubmetted1
                            _SubmitEmail_2 = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.hfSubmitAlertLine1
                            hfSubmitAlertLine1.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.hfSubmitAlertLine2
                            hfSubmitAlertLine2.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.hfSubmitAlertLine3
                            hfSubmitAlertLine3.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.hfSubmitAlertLine4
                            hfSubmitAlertLine4.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.hfSubmitAlertLine5
                            hfSubmitAlertLine5.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.hfSubmitAlertLine6
                            '  txtFindSNOffer.Attributes.Add("placeholder", IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString))
                            hfInsertEmailAdd.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.titleEmailQuotation
                            lblTitleEmailQut.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.AlertEmailQut
                            lblAlertEmailQut.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.PlaceOrderTitle
                            hfplaceanOrderTitle.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.SelectQuantity
                            lblSelectQutstitle.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.SelectQuantityTitle
                            lblSelectQutstitleS.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.AttachSignedDrawing
                            lblSelectQutstitle2.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.AttachFile
                            lblPlaceanOrderAttachFile.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.MyContactDetails
                            lblSelect.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.PhoneNumber
                            lbleadP.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            lbleadP.Text = "&nbsp&nbsp;" & lbleadP.Text
                        Case clsLanguage.e_LanguageConstants.PlaceOrder
                            hfplaceanOrder.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.btnUpdateQuotation
                            btnSaveQuotation.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.DeleteQuotation
                            btnDelQut.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            'lblAlertDeleteQut.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString) & "!"
                            'lblAlertDeleteQutButton.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            'lblAlertDeleteQutButton1.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            hfDeleteQ.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.DeleteQuotationAreYouSure
                            lblAlertDeleteQut1.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)

                        Case clsLanguage.e_LanguageConstants.FileType
                            _DocumentGridCo1FileType = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.FileSize
                            _DocumentGridCo1FileSize = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.FileDate
                            _DocumentGridCo1FileDate = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.FileName
                            _DocumentGridCo1FileName = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)

                        Case clsLanguage.e_LanguageConstants.btn_adddocument
                            hfadddocument.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lbl_View
                            hfViwe.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.D3labelView
                            hf3DlabelView.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.D2labelView
                            hf2DlabelView.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.backtoediting
                            'lblBacktoEditing.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            'lblBacktoEditing1.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            hfBackToEdit.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.AlertDeleteFirsLine
                            hfDeleteQContent.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.GridColQuantity
                            _PricesGridCo1Quantity = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                            langQuantity = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)

                        Case clsLanguage.e_LanguageConstants.GridColNetPrice
                            _PricesGridCo1NetPrice = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.GridColTotalPrice
                            _PricesGridCo1TotalPrice = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblMaxFileSize
                            lblMaxFileSize.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblDeleveryWeeks
                            _Rweek = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblQuotationDetails
                            _QuatationDetails = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblQuotationDetailsTemp
                            _QuatationDetailsTemp = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.lblQuotationDetailsValid
                            _QuatationDetailsValidTemp = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.hfClose
                            hfClose.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.QuotationHistory
                            lblViHis.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.RevisionDate
                            langRevisionDate = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.NewNetPrice
                            langNewNetPrice = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.PrevNetPrice
                            langPrevNetPrice = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)

                        Case clsLanguage.e_LanguageConstants.CustomerOrderNumber
                            lblCustomerOrderN1.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)

                        Case clsLanguage.e_LanguageConstants.CustomerItemNumber
                            lblCustomerItemN2.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)

                        Case clsLanguage.e_LanguageConstants.CustomerReqNumber
                            lblCustomerAdditionalReq.Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.SerialNo
                            If Not IsNumeric(lblQutNumber.Text.ToString.Trim) Then
                                hfTheSerialNo.Value = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                                hfTheSerialNo.Value &= " " & SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TemporarilyQuotationID, False)
                            Else
                                hfTheSerialNo.Value = ""
                            End If
                        Case clsLanguage.e_LanguageConstants.QuotationOrders
                            landQuotationOrders = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.QDate
                            landDate = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
                        Case clsLanguage.e_LanguageConstants.QPrice
                            landPrice = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)

                    End Select
                Next
            End If

        Catch ex As Exception

        End Try
    End Sub
    'Private Sub SetCaptionsLanguageDocuments()
    '    Try
    '        Dim dv As DataView = clsLanguage.Get_LanguageCaption(clsBranch.ReturnActiveBranchCodeState, "Prices")
    '        If Not dv Is Nothing AndAlso dv.Count > 0 Then
    '            Dim currentEnum As String = ""
    '            For i As Integer = 0 To dv.Count - 1
    '                currentEnum = dv.Item(i).Item("LanguageEnumCode")
    '                Select Case currentEnum
    '                    Case clsLanguage.e_LanguageConstants.FileType
    '                        gvDocuments.HeaderRow.Cells(E_DocumentsGrid.FileType).Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.FileSize
    '                        gvDocuments.HeaderRow.Cells(E_DocumentsGrid.FileSize).Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.FileDate
    '                        gvDocuments.HeaderRow.Cells(E_DocumentsGrid.FileDate).Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
    '                    Case clsLanguage.e_LanguageConstants.FileName
    '                        gvDocuments.HeaderRow.Cells(E_DocumentsGrid.FName).Text = IIf(dv.Item(i).Item("ControlCaptionTitleBranch").ToString <> "", dv.Item(i).Item("ControlCaptionTitleBranch").ToString, dv.Item(i).Item("ControlCaptionTitleEn").ToString)
    '                End Select
    '            Next
    '        End If

    '    Catch ex As Exception

    '    End Try
    'End Sub
End Class


