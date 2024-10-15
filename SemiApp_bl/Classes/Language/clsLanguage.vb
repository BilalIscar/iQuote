
Imports IscarDal

Public Class clsLanguage
    'Inherits System.Web.UI.Page

    Public Enum e_Form

        CustomerService = 1
        QuotationInformation = 2
        Order = 3
        Order2 = 4

    End Enum

    Public Enum e_Languages
        english = 1
        german = 2
        italian = 4

    End Enum
    Public Enum e_LanguageReportConstants

        TitleLeft1 = 1
        TitleLeftOfferNumber = 2
        TitleRightDate = 3
        TitleItemDescription = 4
        TitleTechnicalOfferStatus = 5
        TitleExpirdDate = 6
        TitleCustomerDetails = 7
        TitleQUOTATION = 8
        TitleQuantity = 9
        TitleNETPrice = 10
        TitleTotalPrice = 11
        TitleTechSpecification = 12
        TitleDeliveryTime = 13
        TitlePaymentTerms = 14
        Title3DModel = 15
        TitleApprovalDrawing = 16
        TitleContactDetails = 17
        TitleComments = 18
        TitleLeftQuotationNumber = 19
        TitleHeader = 20
        TitleItemType = 21
        TitleCustomerNumber = 22
        TitleCustomeAdress = 23
        TitlePage = 24
        TitleSectionShipment = 25
        TitlePayment = 26
        TitleShipping = 27
        TitleContactBranch = 28
        TitleContactBranchAddress = 29
        TitleOfferBy = 30
        TitleSectionApproval = 31
        TitleCompanyName = 32
        TitleAuthorizedPerson = 33
        TitleSectionApprovalDate = 34
        TitleSectionApprovalSignature = 35
        TitleSectionApprovalCustomer = 36
        TitleSectionApprovalSubsidiary = 37
        TitleCustomerName = 38
        TitleWeeks = 39
        TitleDays = 40
        RemarkLine1 = 41
        RemarkLine2 = 42
        RemarkLine3 = 43
        RemarkLine4 = 44
        RemarkLine5 = 45
        RemarkLine6 = 46
        RemarkLine7 = 47
        TitleLeft1Technical = 48
        RemarkLine8 = 49
        FooterLeftBranchName = 50
        FooterLeftBranchAddress1 = 51
        FooterLeftBranchAddress2 = 52
        FooterMiddleTel = 53
        FooterMiddleFax = 54
        FooterMiddleMail = 55
        FooterRightAccounts = 56
        FooterRightAccount1 = 57
        FooterRightAccount2 = 58
        FooterUnderImageLocation1 = 59
        FooterUnderImageLocation2 = 60
        TitleContactSalesPerson = 61
        TitleContactDeskUser = 62
        TitleContactTechnicalPerson = 63
    End Enum

    Public Enum e_LanguageConstants

        btn_ContactUs = 1
        btn_Feedback = 2
        btnloginUserName = 3
        btnOpenQuotations = 4
        btnUserG1 = 5
        ModelIcon_Groove_Turn = 6
        ModelIcon_ISO_Turning = 7
        ModelIcon_Threading = 8
        ModelIcon_Milling = 9
        ModelIcon_Hole_Making
        ModelIcon_Hole_Reaming = 11
        Tab1 = 12
        Tab2 = 13
        Tab3 = 14
        Tab4 = 15
        MyAccountbtn = 16
        NewQuotation = 17
        btnLogoutInput = 18
        titleMyAccountDetails = 19
        CustomerNumber = 20
        CustomerName = 21
        CustomerAddress = 22
        SubSidiary = 23
        SubsidaryAddress = 24
        ContactDetails = 25
        PaymentTerms = 26
        ShippingMethod = 27
        SalesPerson = 28
        Email = 29
        DeskUser = 30
        TechnicalPerson = 31
        ClearAll = 32
        ParametersOverview = 33
        SetCation = 34
        RelatedImages = 35
        setMaterialCaption = 36
        Group = 37
        Description = 38
        Condition = 39
        Hardness = 40
        Country = 41
        Company_Name = 42
        Message = 43
        Contact_Us = 44
        toImproveTitle1 = 45
        toImproveTitle2 = 46
        toImproveTitle3 = 47
        toImproveTitle4 = 48
        buttonSubmit = 49
        buttonSend = 50
        lblTitle1 = 51
        lblTitle2 = 52
        lblTitle3 = 53
        lblLGI1 = 54
        lblLGI2 = 55
        lblLGI3 = 56
        lblLGI4 = 57
        lblLGI5 = 58
        lblLGI6 = 59
        SaveQuotation_1 = 60
        SaveQuotation_2 = 61
        btnLogOnNORegistration = 62
        SaveQuotation_0 = 63
        lblTmyQut = 64
        lblTpleaseLogin = 65
        lblTinsertoffer = 66
        btnFind = 67
        iQuoteMessage = 68
        lblQD = 69
        SelectReportLanguage = 70
        ReportLanguage = 71
        lblQut = 72
        lblID = 73
        lblDelv = 74
        lblcurrency = 75
        lblCreatedDate = 76
        lblLastUpdate = 78
        lblExpiryDate = 79
        lblPriceTempAlert = 80
        lblQuotationDr = 81
        txtSearchAll = 82
        chkSearchOrderd = 83
        chkSearchExpired = 84
        chkSearchSubmit = 85
        txtConnect = 86
        AddtoMyQuotations = 87
        QuotationDate = 88
        ItemNumber = 89
        SelectCustomer = 90
        hfQuotationSaved = 91
        hfMyQuotations = 92
        hf3DlineFirst = 93
        hf3DlineSecond = 94
        hfSubmetted = 95
        hfSubmetted1 = 96
        hfSubmitAlertLine1 = 97
        hfSubmitAlertLine2 = 98
        hfSubmitAlertLine3 = 99
        hfSubmitAlertLine4 = 100
        hfSubmitAlertLine5 = 101
        hfSubmitAlertLine6 = 102
        buttonCancel = 103
        titleEmailQuotation = 104
        AlertEmailQut = 105
        ConfirmbuttonRefresh = 106
        SelectQuantity = 107
        SelectQuantityTitle = 108
        AttachSignedDrawing = 109
        AttachFile = 110
        MyContactDetails = 111
        PhoneNumber = 112
        PlaceOrder = 113
        PlaceOrderTitle = 114
        btnUpdateQuotation = 115
        DeleteQuotation = 116
        FileType = 117
        FileName = 118
        FileDate = 119
        FileSize = 120
        btn_adddocument = 121
        lbl_View = 122
        D3labelView = 123
        D2labelView = 124
        hfFooter1 = 125
        hfFooter2 = 126
        hfFooter3 = 127
        hfFooter4 = 128
        DiscarChenges = 129
        DeleteQuotationAreYouSure = 130
        backtoediting = 131
        AlertDeleteFirsLine = 132
        lblMaxFileSize = 133
        GridColQuantity = 134
        GridColNetPrice = 135
        GridColTotalPrice = 136
        lblDeleveryWeeks = 137
        lblQuotationDetails = 138
        lblQuotationDetailsTemp = 139
        lblQuotationDetailsValid = 140
        lblPingCaption = 141
        hfClose = 142
        Indexable = 143
        Solid = 144
        SolidEndmilsSquare = 145
        SolidEndmilsBallNose = 146
        _Created = 147
        _Ordered = 148
        _Submitted = 149
        SaveQuotation_3 = 150
        _PriceChangeAlert1 = 151
        _PriceChangeAlert2 = 152
        QuotationHistory = 153
        ViewQuotatiomn = 154
        RevisionDate = 155
        NewNetPrice = 156
        PrevNetPrice = 157
        CustomerOrderNumber = 158
        CustomerItemNumber = 159
        CustomerReqNumber = 160
        SerialNo = 161
        Duplicate = 162
        DuplicateConfirm = 163
        DuplicateQuotation = 164
        PriceOr = 165
        DateOr = 166
        QuotationOrders = 167
        QDate = 168
        QPrice = 169
        LegalNotice = 170
        TermsOfUuse = 171
        TermsOfSale = 172
        PrivacyPolicy = 173
        Status = 174
        Previous = 175
        Nextt = 176
        First = 177
    End Enum
    Public Shared Function Get_LanguageCaption(ControlLocation As String) As DataView
        Try
            'Dim dtModel As DataTable
            '  dtModel = CacheManager.GetDataTable(CacheManager.Keys.LanguageConstants, "glb_TToolGeometricParameter", " ModelNum=" & ModelNum & " ", "TabIndex", "*")


            Dim dt As DataTable = Nothing
            Dim langdt As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_LanguageCaptions, "")
            If langdt Is Nothing OrElse langdt.Rows.Count = 0 Then
                Dim lid As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.qut_LanguageId, False)
                If lid Is Nothing OrElse lid.ToString = "" OrElse lid.ToString = "0" Then
                    lid = "1"
                End If
                langdt = Get_LanguageCaptionFromDB(lid)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_LanguageCaptions, langdt)
            End If
            'Return langdt

            If Not langdt Is Nothing AndAlso langdt.Rows.Count > 0 Then
                Dim dv As DataView = langdt.DefaultView
                If ControlLocation <> "" Then
                    dv.RowFilter = "ControlLocation = '" & ControlLocation & "' OR ControlLocation = 'ALL'"
                End If
                Return dv
            End If

        Catch ex As Exception
            Return CType(Nothing, DataView)
        End Try

    End Function
    Public Shared Function Get_LanguageCaptionInEnglish(ControlLocation As String) As DataView
        Try


            Dim langdt As DataTable = Get_LanguageCaptionFromDB(1)


            If Not langdt Is Nothing AndAlso langdt.Rows.Count > 0 Then
                Dim dv As DataView = langdt.DefaultView
                If ControlLocation <> "" Then
                    dv.RowFilter = "ControlLocation = '" & ControlLocation & "' OR ControlLocation = 'ALL'"
                End If
                Return dv
            End If

        Catch ex As Exception
            Return CType(Nothing, DataView)
        End Try

    End Function


    Public Shared Function Get_LanguageCaptionFromDB(ByVal LanguageId As Integer) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@LanguageId", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, LanguageId))

            dt = oSql.ExecuteSPReturnDT("glb_GetLanguageLabelsCaption", oParams)
            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function Get_LanguageMessagesCaption(iFormID As Integer) As DataTable
        Try
            Dim LanguageId As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.qut_LanguageId, False)
            If LanguageId = "0" Then LanguageId = "1"
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@iFormID", SqlParam.ParamType.ptSmallInt, SqlParam.ParamDirection.pdInput, iFormID))
            oParams.Add(New SqlParam("@LANGUAGE_ID", SqlParam.ParamType.ptSmallInt, SqlParam.ParamDirection.pdInput, LanguageId))

            dt = oSql.ExecuteSPReturnDT("glb_GetLanguageMessagesCaption", oParams)
            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function
    'Public Shared Function Get_LanguageMessagesCaption(ByVal LanguageId As Integer, sFormID As Integer, s As String) As DataTable
    '    Try
    '        Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
    '        Dim oParams As New SqlParams()
    '        Dim dt As DataTable

    '        oParams.Add(New SqlParam("@sForm", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, sFormID))
    '        oParams.Add(New SqlParam("@LANGUAGE_ID", SqlParam.ParamType.ptSmallInt, SqlParam.ParamDirection.pdInput, LanguageId))

    '        dt = oSql.ExecuteSPReturnDT("glb_GetLanguageMessagesCaption", oParams)
    '        Return dt
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function

    Public Shared Function CheckIfLanguageSelected(Languageselected As String, ByRef selectedBC As String) As Boolean
        Try

            Dim cl As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.qut_LanguageId, False)

            If cl <> Languageselected AndAlso Languageselected <> "" Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_LanguageCaptions, CType(Nothing, DataTable))

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.qut_LanguageId, Languageselected)
                cl = Languageselected
                selectedBC = GetBranchFlagCode(cl, clsBranch.ReturnActiveBranchCodeState)
                Return True
            End If

            selectedBC = GetBranchFlagCode(cl, clsBranch.ReturnActiveBranchCodeState)
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Shared Function GetBranchFlagCode(languageId As String, BranchCode As String) As String

        Dim bc As String = "ZZ"
        'If BranchCode = "ZZ" Then
        '    Return "ZZ"
        'End If
        Try
            If BranchCode.ToString <> "" AndAlso BranchCode.ToString.Length = 2 AndAlso BranchCode <> "ZZ" Then
                bc = BranchCode
            End If
        Catch ex As Exception
            bc = "ZZ"
        End Try

        Try
            Select Case languageId.ToUpper
                Case "0" : Return "ZZ"
                Case "1" : Return "IS"
                Case "2" : Return "IG"
                Case "3" : Return "FR"
                Case "4" : Return "IT"
                Case "6" : Return "ID"
                Case "11" : Return "HP"
                Case "13" : Return "IK"
                Case "25" : Return "IU"
                Case "24" : Return "IS"
                Case "53" : Return "JU"
                Case "25" : Return "IU"
                Case "34" : Return "IP"
                Case "27" : Return "IA"
                Case "50" : Return "LJ"
                Case "52" : Return "FL"
                Case Else
                    Return bc


            End Select
        Catch ex As Exception

        End Try

    End Function

    Public Shared Function GetLanguageLabels(LabelEN) As String
        Try
            Dim lid As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.qut_LanguageId, False)
            Dim dt As DataTable = Nothing
            'dt = CacheManager.GetDataTable(CacheManager.Keys.Rules, "glb_TLanguage_Labels", " LANGUAGE_ID=" & lid & " ", "LANGUAGE_ID", "*")
            dt = CacheManager.GetDataTable(CacheManager.Keys.LanguageLabels, "glb_TLanguage_Labels", " LANGUAGE_ID=" & lid & " AND (LABEL_EN_CAPTION = '" & LabelEN & "' ) ", "LANGUAGE_ID", "*")
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 AndAlso dt.Rows(0).Item("LABER_TRANSLATED_CAPTION").ToString.Trim <> "" Then
                Return dt.Rows(0).Item("LABER_TRANSLATED_CAPTION").ToString.Trim
            End If
            If LabelEN.ToString.Contains("(") Then
                dt = CacheManager.GetDataTable(CacheManager.Keys.LanguageLabels, "glb_TLanguage_Labels", " LANGUAGE_ID=" & lid & " AND (LABEL_EN_CAPTION = '" & LabelEN.ToString.Substring(0, LabelEN.ToString.IndexOf("(")) & "' )".Trim, "LANGUAGE_ID", "*")
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 AndAlso dt.Rows(0).Item("LABER_TRANSLATED_CAPTION").ToString.Trim <> "" Then
                    Dim sDs As String = LabelEN.ToString.Substring(LabelEN.ToString.IndexOf("("), LabelEN.ToString.Length - LabelEN.ToString.IndexOf("("))
                    Return dt.Rows(0).Item("LABER_TRANSLATED_CAPTION").ToString.Trim & " " & sDs
                End If
            End If

            Return LabelEN
        Catch ex As Exception
            Return LabelEN
        End Try
    End Function


End Class
