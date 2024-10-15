Imports System.Web


Public Class SessionManager

    Public Enum ActiveQuotation
        ' ssBIl_Result
        '----------Controls Language----------------------
        dt_UserResult
        dt_LanguageCaptions
        qut_LanguageId
        '-------------------------------------------------
        DoCheck_LogIN
        TimeStamp
        '----------Formula Price----------------------
        '_DocumentsFIllDone
        _PriceFormula_MKT
        _PriceFormulaMKT_Value
        _PriceFormulaTFR
        _PriceFormulaTFR_Value
        _BranchPriceFormulaMKT
        _BranchPriceFormulaMKT_Value
        _BranchPriceFormulaTFR
        _BranchPriceFormulaTFR_Value
        _PriceCalculateFlag

        _DescriptionFormulaWithParamsNames
        SemiToolDescription ' From semi

        ShowQutPrice

        selectedReportLanguage
        selectedLanguage
        '--------------SEMI STANDARD------------------

        _AllRedyBulidFormPostBack
        _AllRedy3DAlertShows
        _AllRedyReportBuild
        '_AllRedyReportBuildFilesNames
        _AllRedyOOppssAlertShows

        '_AllRedyBulidFormPostBackPrice

        _dtParamListModification
        _dtParamListConfiguration

        _dtParamListModificationLive
        _dtParamListConfigurationLive

        _ModelConfiguration
        _ModelModification
        _dtParamsFromCatalog
        _dtModParamsFinalSelected

        _TempdtFactorsWithValues
        '_TempQuotationListCustomer
        _TempShowCustomerAlertLogIN
        _TempDoConnect
        'QuotationOpenType_MODF_CONFIG
        QuotationOpenType
        language
        vers
        vers_OnlyForNew
        itemNumber
        CATALOGITEMNUMBER
        familyNo

        REPORT_ID
        'Customer
        CustomerNumber
        CustomerName
        _ToEnableParamsTab

        mainPicL
        mainPicR
        ImgLogo
        _ModelNumModification
        _ModelNumConfiguration
        BranchCode
        'TemporarilyBranchCode
        'BranchNumber

        'STN
        'TFRPrice
        'TFRCurrency
        _ConstFormula
        '_PriceFormula
        '_PriceFormulaWithParamsNames
        '_CostFormulaWithParamsNames


        _CostFormula
        _CostFormulaValue
        _GPFormula

        _DC

        ' _BSONfolder
        '_DrawingName

        '_TempdtFactors
        _dtParametersFactors

        ErrorInWSGAL
        '--------------------TEMP-------------------------
        TempConfigDefaultShow
        '_TempOpenTransition
        '_TempQTYfactor
        'TempQTYfactor
        'TempCurrentView
        FormName
        _TempdtParamsSelected
        '_TempTemporarilyQuotationID
        _TempdtParamsChangable
        _dtParametersListView
        _dtTempOnlyForCheck

        'TempSearchQutNo
        'TempSearchItemNo
        TempSearchAny
        TempSearchOrdered
        TempSearchExpired
        TempSearchSubmitted
        TempSearchBranch
        TempSearchCustomer

        TempMainMaterial
        TempMainMaterialCategory
        TempMaterial
        TempMaterialTabDescription
        TempDontGetPrice

        'ModifiType
        'TempPDF
        Temp_fileFolderPath
        'Temp_fileName
        Temp_ClearAllForModification

        'Temp_HideSaveDivLast

        FactorsQty

        _dtTQuotatioListModelParametersCode

        TempLeftImage
        TempRightImage
        TempShoeLogInFrame
        TempAllReadyShowsSaveMessage
        TempAllReadyShowsTemporaryMessage
        'TempReportTabToShow


        'tEMPIFIFlEFT
        'tEMPIFIFrIGHT
        tempMessageAllredySent
        tempMessageSendFeedBack

        'TempRefreshPriceForm

        _TempAllReadyTriedtoBuildDrawingAutomaticly
        '_TempAllReadyTriedtoBuildBSONAutomaticly
        '_TempAllReadyTriedtoBuildDrawingBSONAutomaticly
        '_TempAllReadyShowedtoBuildDrawingBSONAutomaticly


        '---------------------------------------------


        NET_STNCustomerPrice
        NET_STNCustomerCurrency 'BOB invoics currency
        TFR_STNBranchPrice
        TFR_STNBranchCurrency ' TBranchDetails


        'Site

        dt_Price
        _TempQTYFct
        dt_PriceTempForCalcDefaultPrices

        'dt_Currencies
        dt_Consts
        'dt_ParamListRevision
        'dt_LanguageConstants

        'QuotationStart

        QuotationStatus

        QuotationNumber

        QuotationDate
        QuotationDateWithTime
        ExpiredDate
        LastUpdateDate

        'ModelPictureName
        'ModelCurrentPictureName

        'CustomerAddress1

        'ToolDescription ' AS400-start from gal, semi-start from semi
        'StandartToolDescription ' From new master

        'ItemDesc
        'StandardDesc

        'LocalCustomer
        'CatalogNumber
        'AS400Year

        PrevParameterSelected
        CurrentParameterIndex

        'GlobalBranchNo ' the branchno from tbranchdetails In case of global customer (Localcustomer=False)
        'StandartTransferPrice 'From AS400

        'OfferBy_ID

        'loggedEmail

        FolderPath
        Delivery_Weeks
        DeliveryRad_Weeks
        ValidTime_Weeks
        MaterialsID

        'DeliveryRAD
        'Delivery_DeliveryRAD 'only for update AS400 QC
        'DeliveryVIA

        RateMKT_USD  'TFRRate - MKT Currency from tCustomers
        RateTFR_USD 'TFRRate - TFR Currency from branchdetails
        FAMILYGP
        FAMILYGP_MaxQTY
        SUBGP
        'RateTFR_MKT
        'RateTFR_EUR
        'RateEUR_USD
        'RateTFR_ModelCur
        'RateFormat
        'RateTFR_WC
        'RateWC_USD
        'RateTFR_VENDOR
        'RateCUST_BASEDON


        'LTDPriceGP
        'WC_FACTOR
        'RATE_FACTOR
        'Archive

        'QuotationCurrency
        'CustomerRequest
        'ContactPerson
        'ContactPersonMail
        'ContactPersonPhone

        AS400quoteReturnedJSON

        AS400Number
        AS400RowNumber
        'DiscountFromSTN

        TemporarilyQuotation
        TemporarilyQuotationID


        Valid_Time

        '-- Save Flags
        FlagModificationRecursiveFinish
        FlagParametersChanged
        'FlagParametersChangedAfterFirstParamsSelected
        FlagPricesChanged

        FlagModificationCatalogDeferent

        Culture

        Submitted  '0=Not Submitted yest, 1=Submitted(send & emil quotation)
        Ordered
        SubmittedEmail
        OrderedEmail
        OrderedPhoneNo
        tempAttachOrderFile

        tempListSortCol
        tempListSortOrder

        _tempBackFromImcLOGIN

        PricesCustomerOrderNumber
        PricesCustomerItemNumber
        PricesCustomerAdditionalReq
    End Enum

    Public Enum LocalCustomer
        LOCAL = 1
        BRANCH = 2
    End Enum

    Private Shared Sub SetFormula(ClearFlag As Boolean, Optional Exist As Boolean = False, Optional BranchCode As String = "", Optional QuotationNumber As String = "")
        Try
            'GeneralException.WriteEventLogIn("SetFormula")

            If Exist = False Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormula_MKT, "")
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormulaMKT_Value, "")
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormulaTFR, "")
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormulaTFR_Value, "")
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BranchPriceFormulaMKT, "")
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BranchPriceFormulaMKT_Value, "")
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BranchPriceFormulaTFR, "")
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BranchPriceFormulaTFR_Value, "")
                If ClearFlag Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceCalculateFlag, "")

                End If

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._CostFormula, "")
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._GPFormula, "")
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._CostFormulaValue, "")
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DescriptionFormulaWithParamsNames, "")
                'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._CostFormulaWithParamsNames, "")
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.SemiToolDescription, "")
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ConstFormula, "")
            Else

                Dim dtQuoteSpecialPrices As DataTable = clsQuatation.GeQuotationFormula(BranchCode, QuotationNumber)
                If Not dtQuoteSpecialPrices Is Nothing AndAlso dtQuoteSpecialPrices.Rows.Count > 0 Then
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormula_MKT, dtQuoteSpecialPrices.Rows(0)("PriceFormulaMKT").ToString)
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormulaMKT_Value, dtQuoteSpecialPrices.Rows(0)("PriceFormulaMKT_Value").ToString)
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormulaTFR, dtQuoteSpecialPrices.Rows(0)("PriceFormulaTFR").ToString)
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormulaTFR_Value, dtQuoteSpecialPrices.Rows(0)("PriceFormulaTFR_Value").ToString)
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BranchPriceFormulaMKT, dtQuoteSpecialPrices.Rows(0)("BranchPriceFormulaMKT").ToString)
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BranchPriceFormulaMKT_Value, dtQuoteSpecialPrices.Rows(0)("BranchPriceFormulaMKT_Value").ToString)
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BranchPriceFormulaTFR, dtQuoteSpecialPrices.Rows(0)("BranchPriceFormulaTFR").ToString)
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BranchPriceFormulaTFR_Value, dtQuoteSpecialPrices.Rows(0)("BranchPriceFormulaTFR_Value").ToString)
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceCalculateFlag, dtQuoteSpecialPrices.Rows(0)("PriceCalculateFlag").ToString)

                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._CostFormula, dtQuoteSpecialPrices.Rows(0)("CostFormula").ToString)
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._GPFormula, dtQuoteSpecialPrices.Rows(0)("GPFormula").ToString)
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._CostFormulaValue, dtQuoteSpecialPrices.Rows(0)("CostFormulaValue").ToString)
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DescriptionFormulaWithParamsNames, dtQuoteSpecialPrices.Rows(0)("DesctriptionFormulaNames").ToString)
                    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._CostFormulaWithParamsNames, dtQuoteSpecialPrices.Rows(0)("_CostFormulaWithParamsNames").ToString)
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.SemiToolDescription, dtQuoteSpecialPrices.Rows(0)("DesctriptionFormulaNumbers").ToString)
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ConstFormula, dtQuoteSpecialPrices.Rows(0)("ConstFormula").ToString)
                End If

            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Public Shared Function GetQuotationValues(ByVal Key As ActiveQuotation, Optional ByVal Encrypted As Boolean = False) As String
        Try

            Encrypted = True
            'GeneralException.WriteEvent("000000")

            'Dim uID As String = HttpContext.Current.Request("uID")
            'GeneralException.WriteEvent("GetQuotationValues ID=" & uID)

            Dim res As String = ""
            If Encrypted Then
                'GeneralException.WriteEvent("RES1 - GetQuotationValues ID=" & uID)

                'res = CryptoManager.Decode(HttpContext.Current.Session(uID & Key.ToString)).ToString
                Try
                    If Not HttpContext.Current Is Nothing Then
                        If Not HttpContext.Current.Session(GetKeyFor(Key.ToString).ToString) Is Nothing Then
                            res = CryptoManager.Decode(HttpContext.Current.Session(GetKeyFor(Key.ToString).ToString)).ToString
                        End If
                    End If


                Catch ex As Exception
                    Return res
                End Try

            Else
                'GeneralException.WriteEvent("RES2 - GetQuotationValues ID=" & uID)
                'GeneralException.WriteEvent(Key.ToString)

                'GeneralException.WriteEvent(uID)

                'GeneralException.WriteEvent("LOOP")


                'Dim i As Integer
                'GeneralException.WriteEvent(HttpContext.Current.Session.Count)
                'For i = 0 To HttpContext.Current.Session.Count - 1
                '    GeneralException.WriteEvent(HttpContext.Current.Session.Keys(i) & ": " & HttpContext.Current.Session.Item(i) & vbCrLf)
                'Next
                'GeneralException.WriteEvent("END LOOP")


                'res = HttpContext.Current.Session(uID.ToString & Key.ToString).ToString
                Try
                    If Not HttpContext.Current Is Nothing Then
                        If Not HttpContext.Current.Session(GetKeyFor(Key.ToString).ToString) Is Nothing Then
                            res = HttpContext.Current.Session(GetKeyFor(Key.ToString).ToString).ToString
                        End If
                    End If


                Catch ex As Exception
                    Return res
                End Try


            End If
            'If res = Nothing Then
            '    'GeneralException.WriteEvent("RES2 - res=NOTHING")

            'End If
            'GeneralException.WriteEvent("RES2 - res=" & res)

            Return res
        Catch ex As Exception
            'If ex.Message.Contains("Object reference not set to an instance of an object") Then
            '    Response.Redirect("~/Forms/Notification.aspx", False)

            'End If
            Throw
        End Try
    End Function

    Public Shared Function GetQuotationValues(ByVal Key As ActiveQuotation, ByVal Dummy As String) As DataTable
        Dim res As DataTable = Nothing
        Try

            'res = HttpContext.Current.Session(HttpContext.Current.Request("uID") & Key.ToString)
            res = HttpContext.Current.Session(GetKeyFor(Key.ToString).ToString)
            Return res
        Catch ex As Exception
            Return res
        End Try
    End Function

    Public Shared Sub SetQuotationValue(ByVal Key As ActiveQuotation, ByVal Value As String, Optional ByVal Encrypted As Boolean = False)

        Encrypted = True
        'Dim uID As String = HttpContext.Current.Request.QueryString("uID")
        Try
            If Encrypted Then
                'HttpContext.Current.Session.Add(uID & Key.ToString, CryptoManager.Encode(Value))
                'CryptoManager.
                HttpContext.Current.Session.Add(GetKeyFor(Key.ToString).ToString, CryptoManager.Encode(Value))
            Else
                'HttpContext.Current.Session.Add(uID & Key.ToString, Value)
                HttpContext.Current.Session.Add(GetKeyFor(Key.ToString).ToString, Value)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Shared Sub SetQuotationValue(ByVal Key As ActiveQuotation, ByVal Value As DataTable)
        'Dim uID As String = HttpContext.Current.Request("uID")
        'HttpContext.Current.Session.Add(uID & Key.ToString, Value)
        HttpContext.Current.Session.Add(GetKeyFor(Key.ToString).ToString, Value)
    End Sub

    Public Shared Sub Clear_ALL_SessionsTimeStamp()
        Try
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TimeStamp, "")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub Clear_Sessions_ShowCustomerType()
        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ShowQutPrice, "")

    End Sub
    Public Shared Sub Clear_Sessions_ForBeginSendMessages()
        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempMessageSendFeedBack, "NO")
        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempMessageAllredySent, "NO")

    End Sub
    Public Shared Sub Clear_ALL_Sessions()
        Try

            ' SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_LanguageCaptions, "")
            '  SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_LanguageCaptions, CType(Nothing, DataTable))

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.selectedReportLanguage, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.selectedLanguage, "")
            Try
                Dim ss As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.qut_LanguageId, False)
                If ss Is Nothing OrElse ss = "0" OrElse ss = "" Then


                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.qut_LanguageId, "0")

                End If
            Catch ex As Exception
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.qut_LanguageId, "0")

            End Try




            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Submitted, "0") '0=Not Submitted yest, 1=Submitted(send & emil quotation)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Ordered, "0") '0=Not Submitted yest, 1=Submitted(send & emil quotation)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.OrderedPhoneNo, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.OrderedEmail, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.SubmittedEmail, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempAttachOrderFile, "")

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DocumentsFIllDone, "NO")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.REPORT_ID, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyReportBuild, "NO")
            '   SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyReportBuildFilesNames, "")




            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CustomerName, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CustomerNumber, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ToEnableParamsTab, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_PriceTempForCalcDefaultPrices, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedy3DAlertShows, "NO")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyOOppssAlertShows, "NO")

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempQuotationListCustomer, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempAllReadyShowedtoBuildDrawingBSONAutomaticly, "NO")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempAllReadyTriedtoBuildDrawingAutomaticly, "YES1")
            ' SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempRefreshPriceForm, "NO")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagModificationCatalogDeferent, "NOTENDYET")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempDontGetPrice, "FALSE")

            Clear_Sessions_ForBeginSendMessages()


            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempdtParamsChangable, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TemporarilyQuotationID, "TRUE")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationOpenType, clsQuatation.Enum_QuotationOpenType.NON)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.familyNo, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelNumModification, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelNumConfiguration, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PrevParameterSelected, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.language, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.vers, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.itemNumber, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CATALOGITEMNUMBER, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerPrice, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchPrice, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchCurrency, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerCurrency, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._GPFormula, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._CostFormulaValue, "")
            SetFormula(True)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DC, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationNumber, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.mainPicL, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.mainPicR, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ImgLogo, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyBulidFormPostBack, "False")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.RateTFR_USD, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.RateMKT_USD, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FAMILYGP, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FAMILYGP_MaxQTY, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.SUBGP, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerPrice, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchPrice, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.AS400quoteReturnedJSON, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.AS400Number, "0")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.AS400RowNumber, "001")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Valid_Time, "0")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationDate, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationDateWithTime, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ExpiredDate, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.LastUpdateDate, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CurrentParameterIndex, "")

            ' SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BSONfolder, "")
            '   SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DrawingName, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListModification, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListConfiguration, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListModificationLive, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListConfigurationLive, CType(Nothing, DataTable))

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelModification, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelConfiguration, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamsFromCatalog, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtModParamsFinalSelected, CType(Nothing, DataTable))

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempdtFactorsWithValues, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParametersListView, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtTempOnlyForCheck, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Consts, clsQuatation.GetQuatationConsts("", 0))

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Price, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtTQuotatioListModelParametersCode, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempdtParamsSelected, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.BranchCode, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Delivery_Weeks, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FolderPath, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerOrderNumber, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerItemNumber, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerAdditionalReq, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.DeliveryRad_Weeks, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ValidTime_Weeks, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.MaterialsID, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagParametersChanged, False)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagModificationRecursiveFinish, False)


            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagPricesChanged, False)

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempSearchQutNo, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempSearchItemNo, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempSearchAny, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempShoeLogInFrame, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempLeftImage, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempRightImage, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempAllReadyShowsTemporaryMessage, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempAllReadyShowsSaveMessage, "")

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempReportTabToShow, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationStatus, clsQuatation.e_QuotationStatus.New_Qut)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMainMaterial, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMainMaterialCategory, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMaterial, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMaterialTabDescription, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Temp_fileFolderPath, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Temp_ClearAllForModification, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FactorsQty, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParametersFactors, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempDoConnect, "")


        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Shared Sub Clear_Sessions_For_NewStartedQuotation()
        Try

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Submitted, "0")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Ordered, "0")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.OrderedPhoneNo, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.OrderedEmail, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.SubmittedEmail, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempAttachOrderFile, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DocumentsFIllDone, "NO")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.REPORT_ID, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyReportBuild, "NO")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ToEnableParamsTab, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_PriceTempForCalcDefaultPrices, CType(Nothing, DataTable))

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempDoConnect, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempAllReadyTriedtoBuildDrawingAutomaticly, "YES1")

            ' SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempRefreshPriceForm, "NO")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagModificationCatalogDeferent, "NOTENDYET")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempDontGetPrice, "FALSE")

            'GeneralException.WriteEvent("Clear_Sessions_For_NewStartedQuotation")


            Clear_Sessions_ForBeginSendMessages()


            ''''   SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerCurrency, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TemporarilyQuotationID, "")

            SetFormula(True)


            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ConstFormula, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DC, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyBulidFormPostBack, "False")


            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.RateTFR_USD, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FAMILYGP, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FAMILYGP_MaxQTY, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.RateMKT_USD, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.SUBGP, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.AS400quoteReturnedJSON, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.AS400Number, "0")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.AS400RowNumber, "001")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Valid_Time, "0")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CustomerName, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CustomerNumber, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationDate, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationDateWithTime, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ExpiredDate, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.LastUpdateDate, "")
            '  SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BSONfolder, "")
            ' SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DrawingName, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListModification, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListConfiguration, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListModificationLive, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListConfigurationLive, CType(Nothing, DataTable))

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamsFromCatalog, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtModParamsFinalSelected, CType(Nothing, DataTable))

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempdtFactorsWithValues, CType(Nothing, DataTable))
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_LanguageConstants, CType(Nothing, DataTable))

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Consts, CType(Nothing, DataTable))

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Price, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtTQuotatioListModelParametersCode, CType(Nothing, DataTable))

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParametersFactors, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempdtParamsSelected, CType(Nothing, DataTable))

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Consts, clsQuatation.GetQuatationConsts("", 0))

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagParametersChanged, False)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagModificationRecursiveFinish, False)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagPricesChanged, False)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempSearchAny, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempSearchOrdered, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempSearchSubmitted, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortCol, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempListSortOrder, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempSearchExpired, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempShoeLogInFrame, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempAllReadyShowsTemporaryMessage, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempAllReadyShowsSaveMessage, "")

            '  SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempReportTabToShow, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempLeftImage, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempRightImage, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationStatus, clsQuatation.e_QuotationStatus.New_Qut)


            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMainMaterial, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMainMaterialCategory, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMaterial, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMaterialTabDescription, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Temp_fileFolderPath, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Temp_fileName, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.MaterialsID, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Temp_ClearAllForModification, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FactorsQty, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParametersFactors, CType(Nothing, DataTable))


            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Temp_HideSaveDivLast, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerOrderNumber, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerItemNumber, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerAdditionalReq, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Delivery_Weeks, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FolderPath, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.paymentTerms, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.shipmethod, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.DeliveryRad_Weeks, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ValidTime_Weeks, "")


        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Shared Sub ClearSessionAfterChangewMaterial()
        Try

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempQuotationListCustomer, "")


            ' SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempRefreshPriceForm, "NO")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerPrice, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchPrice, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Price, CType(Nothing, DataTable))

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Delivery_Weeks, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.DeliveryRad_Weeks, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ValidTime_Weeks, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Valid_Time, "0")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ERROR_MSG, "")


            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagPricesChanged, True)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Consts, clsQuatation.GetQuatationConsts("", 0))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempdtFactorsWithValues, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FactorsQty, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParametersFactors, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamsFromCatalog, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtModParamsFinalSelected, CType(Nothing, DataTable))

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BSONfolder, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DrawingName, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempAllReadyShowsSaveMessage, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtTQuotatioListModelParametersCode, CType(Nothing, DataTable))

            Clear_Sessions_ForBeginSendMessages()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub ClearSessionForEditMode()
        Try


            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationDateWithTime, "")

            '  SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DocumentsFIllDone, "NO")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.REPORT_ID, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyReportBuild, "NO")
            '  SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyReportBuildFilesNames, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ToEnableParamsTab, "TRUE")

            ' SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempQuotationListCustomer, "")

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempAllReadyShowedtoBuildDrawingBSONAutomaticly, "NO")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempAllReadyTriedtoBuildDrawingAutomaticly, "YES1")


            ' SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempRefreshPriceForm, "NO")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TimeStamp, "")

            '  GeneralException.WriteEvent("ClearSessionForEditMode")

            Clear_Sessions_ForBeginSendMessages()



            '  GeneralException.WriteEvent("ClearSessionForEditMode")

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.MKTPriceNumber, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFRPriceNumber, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.stnGP_TC_Remark, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.STN, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFRPrice, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFRCurrency, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerPrice, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerCurrency, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchPrice, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchCurrency, "")


            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ConstFormula, "")

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormula, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormulaWithParamsNames, "")

            SetFormula(False)

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._CostFormula, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._GPFormula, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._CostFormulaValue, "")

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.DeliveryVIA, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerOrderNumber, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerItemNumber, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerAdditionalReq, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Delivery_Weeks, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.DeliveryRad_Weeks, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ValidTime_Weeks, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.DeliveryRAD, 0)
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Delivery_DeliveryRAD, 0)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Valid_Time, "0")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagEndParameterArrivedFromRevisionButton, False)
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagEndParameterArrivedFromExistQuotation, False)

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.SelectedCurCode, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Price, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtTQuotatioListModelParametersCode, CType(Nothing, DataTable))

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.RateToConvert, "")

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QtyPrcFormulaForReport, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagPricesChanged, True)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Consts, clsQuatation.GetQuatationConsts("", 0))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempdtFactorsWithValues, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FactorsQty, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParametersFactors, CType(Nothing, DataTable))

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ERROR_MSG, "")

            '  SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BSONfolder, "")
            '  SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DrawingName, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempAllReadyShowsSaveMessage, "")

            If clsQuatation.ACTIVE_OpenType = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamsFromCatalog, CType(Nothing, DataTable))
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtModParamsFinalSelected, CType(Nothing, DataTable))

            End If
            '

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedy3DAlertShows, "NO")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyOOppssAlertShows, "NO")


            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FAMILYGP, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FAMILYGP_MaxQTY, "")

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.DrawingParameters, "")
        Catch ex As Exception
            Throw
        End Try
    End Sub


    Public Shared Sub SetSessionDetails_SEMI_NewQuoatation(QuotationOpenType_MODF_CONFIG As String, MainPic As String, Customer As String, ModelNumModification As String, ModelNumConfiguration As String,
                                                           BranchCode As String, PrevParameterSelected As String, lang As String, vers As String, itemNumber As String, loggedEmail As String,
                                                           QuotationNumber As String, dt_ModelDetailsCON As DataTable, dt_ModelDetailsMOD As DataTable, familyNo As String, dtParametersListView As DataTable, CATALOGITEMNUMBER As String) 'STN As String,
        Try

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Submitted, "0")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Ordered, "0")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.OrderedPhoneNo, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.OrderedEmail, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.SubmittedEmail, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.tempAttachOrderFile, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DocumentsFIllDone, "NO")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.REPORT_ID, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyReportBuild, "NO")
            '    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyReportBuildFilesNames, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ToEnableParamsTab, "")

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempShowCustomerAlertLOgIN, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempDoConnect, "")

            ''''      SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerCurrency, "")

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempQuotationListCustomer, "")

            Clear_Sessions_ForBeginSendMessages()
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempAllReadyShowedtoBuildDrawingBSONAutomaticly, "NO")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempAllReadyTriedtoBuildDrawingAutomaticly, "YES1")

            ' SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempRefreshPriceForm, "NO")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagModificationCatalogDeferent, "NOTENDYET")


            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempDontGetPrice, "FALSE")

            '  GeneralException.WriteEvent("SetSessionDetails_SEMI_NewQuoatation")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TimeStamp, "")

            '  GeneralException.WriteEvent("SetSessionDetails_SEMI_NewQuoatation")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationStatus, clsQuatation.e_QuotationStatus.New_Qut)

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.LocalCustomer, SessionManager.LocalCustomer.LOCAL)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CurrentParameterIndex, "10")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationOpenType, QuotationOpenType_MODF_CONFIG)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.familyNo, familyNo)
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CustomerNumber, Customer)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelNumModification, ModelNumModification)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelNumConfiguration, ModelNumConfiguration)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.BranchCode, BranchCode)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PrevParameterSelected, PrevParameterSelected)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.language, lang)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.vers, vers)
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.STN, STN)
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempOpenTransition, "")

            If Not IsNumeric(QuotationOpenType_MODF_CONFIG) Then
                QuotationOpenType_MODF_CONFIG = "0"
            End If

            If Not itemNumber Is Nothing Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.itemNumber, itemNumber)
            End If
            If Not CATALOGITEMNUMBER Is Nothing Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CATALOGITEMNUMBER, CATALOGITEMNUMBER)
            End If
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.OfferBy_ID, OfferBy_ID)

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.loggedEmail, loggedEmail)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationNumber, QuotationNumber)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelModification, dt_ModelDetailsMOD)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelConfiguration, dt_ModelDetailsCON)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParametersListView, dtParametersListView)

            If Not MainPic Is Nothing Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.mainPicL, MainPic)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.mainPicR, MainPic)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ImgLogo, MainPic)
            End If

            ''Dim sHttpCatalogIllust As String = "http://www.iscar.com/SM/getCatalogIllust.aspx?Cat="
            ''Dim sHttpCatalogImage As String = "http://www.iscar.com/SM/getCatalogImage.aspx?Cat="

            ''SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.mainPicL, sHttpCatalogIllust & itemNumber & "&Comp=" & BranchCode)
            ''SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.mainPicR, sHttpCatalogImage & itemNumber & "&Comp=" & BranchCode)

            ''picL = "http://www.iscar.com/SM/getCatalogIllust.aspx?Cat=" & sitemNumber & "&Comp=" & BranchCode
            ''picR = "http://www.iscar.com/SM/getCatalogImage.aspx?Cat=" & sitemNumber & "&Comp=" & BranchCode

            ''picL = ConfigurationManager.AppSettings("ImageNonItemNoL") & ImageNonItemNoL & ".gif"

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempWorkPieceMaterialValue, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempModelValue, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempAllReadyShowsTemporaryMessage, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempAllReadyShowsSaveMessage, "")

            '  SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempReportTabToShow, "")


            'Try
            '    clsQuatation.GetCustomerAccountDetails(Customer, BranchCode)
            'Catch ex As Exception
            '    GeneralException.WriteEvent("BOB ERROR:" & ex.Message)
            'End Try


        Catch ex As Exception
            Throw
        End Try

    End Sub


    Public Shared Function SetSessionDetails_SEMI_ExistQuoatation(BranchCode As String, QuotationNumber As String, statusQut As String, ConnectFromQutList As Boolean) As Boolean
        Try

            Clear_Sessions_OpenIquote("FALSE")

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.selectedReportLanguage, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.selectedLanguage, "")
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.qut_LanguageId, "0")

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DocumentsFIllDone, "NO")

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempShowCustomerAlertLOgIN, "")


            '  SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyReportBuildFilesNames, "YES")

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempQuotationListCustomer, "")

            Clear_Sessions_ForBeginSendMessages()
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempAllReadyShowedtoBuildDrawingBSONAutomaticly, "YES")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempAllReadyTriedtoBuildDrawingAutomaticly, "YES3")
            ' SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempRefreshPriceForm, "NO")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagModificationCatalogDeferent, "NOTENDYET")


            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempDontGetPrice, "TRUE")
            'GeneralException.WriteEventLogIn("SetSessionDetails_SEMI_ExistQuoatation")

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationStatus, clsQuatation.e_QuotationStatus.Exist_QutOpenedFromQuotationList)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TimeStamp, "")


            'GeneralException.WriteEvent("SetSessionDetails_SEMI_ExistQuoatation")
            If statusQut = "" Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationStatus, clsQuatation.e_QuotationStatus.New_Qut)
            Else
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationStatus, statusQut)
            End If


            Dim dtQuoteDetails As DataTable = clsQuatation.GetQuatationDetails(BranchCode, QuotationNumber)
            If dtQuoteDetails Is Nothing Then
                Return False
            ElseIf dtQuoteDetails.Rows.Count < 1 Then
                Return False
            End If

            Dim dtModelDetails As DataTable = clsModel.GetModelDetails(Integer.Parse(dtQuoteDetails.Rows(0).Item("ModelNum")))

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyBulidFormPostBack, "True")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationNumber, QuotationNumber)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TemporarilyQuotationID, dtQuoteDetails.Rows(0)("TemporarilyQuotationID").ToString)

            SetSessionDetails_Temporarily(dtQuoteDetails.Rows(0)("TemporarilyQuotation").ToString)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.BranchCode, BranchCode)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationOpenType, dtQuoteDetails.Rows(0)("OpenType").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.familyNo, dtQuoteDetails.Rows(0)("FamilyNo").ToString)

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.STN, dtQuoteDetails.Rows(0)("StandartTransferPrice").ToString)
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFRPrice, dtQuoteDetails.Rows(0)("StandartTransferPrice").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerPrice, dtQuoteDetails.Rows(0)("NET_STNCustomerPrice").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerCurrency, dtQuoteDetails.Rows(0)("NET_STNCustomerCurrency").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchPrice, dtQuoteDetails.Rows(0)("TFR_STNBranchPrice").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchCurrency, dtQuoteDetails.Rows(0)("TFR_STNBranchCurrency").ToString)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Submitted, dtQuoteDetails.Rows(0)("Submitted").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Ordered, dtQuoteDetails.Rows(0)("Ordered").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.OrderedPhoneNo, dtQuoteDetails.Rows(0)("OrderedPhoneNo").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.OrderedEmail, dtQuoteDetails.Rows(0)("OrderedEmail").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.SubmittedEmail, dtQuoteDetails.Rows(0)("SubmittedEmail").ToString)

            Dim sla As String = dtQuoteDetails.Rows(0)("LastUpdate").ToString
            If IsDate(sla) Then
                sla = ClsDate.GetDateTimeReturnStringFormat(sla, ClsDate.Enum_DateFormatTypes._ddmmyyyy)
            End If
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.LastUpdateDate, sla)

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationDate, dtQuoteDetails.Rows(0)("QuotationDate").ToString)
            Dim od As String = dtQuoteDetails.Rows(0)("QuotationDate").ToString
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationDateWithTime, od)

            If IsDate(od) Then
                od = ClsDate.GetDateTimeReturnStringFormat(od, ClsDate.Enum_DateFormatTypes._ddmmyyyy)
            End If
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationDate, od)


            Dim sd As String = dtQuoteDetails.Rows(0)("ExpiredDate").ToString
            If IsDate(sd) Then
                'sd = ClsDate.GetDateTimeReturnStringFormat(sd, ClsDate.Enum_DateFormatTypes._ddmmyyyy)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ExpiredDate, ClsDate.GetDateTimeReturnStringFormat(sd, ClsDate.Enum_DateFormatTypes._ddmmyyyy))
            End If
            ' SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ExpiredDate, sd)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagModificationRecursiveFinish, True)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempAllReadyShowsTemporaryMessage, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempAllReadyShowsSaveMessage, "")

            '  SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempReportTabToShow, "PDF")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ShowQutPrice, dtQuoteDetails.Rows(0)("ShowQutPrice").ToString)


            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.AS400quoteReturnedJSON, dtQuoteDetails.Rows(0)("AS400quoteReturnedJSON").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.AS400Number, dtQuoteDetails.Rows(0)("AS400Number").ToString)
            ' SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.AS400Year, dtQuoteDetails.Rows(0)("AS400Year").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.AS400RowNumber, dtQuoteDetails.Rows(0)("AS400RowNumber").ToString)

            ' SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BSONfolder, dtQuoteDetails.Rows(0)("BSONfolder").ToString)
            '  SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DrawingName, dtQuoteDetails.Rows(0)("DrawingName").ToString)


            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.RateTFR_USD, dtQuoteDetails.Rows(0)("RateTFR_USD").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.RateMKT_USD, dtQuoteDetails.Rows(0)("RateMKT_USD").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FAMILYGP, dtQuoteDetails.Rows(0)("FamilyGP").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.SUBGP, dtQuoteDetails.Rows(0)("SUBGP").ToString)

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TemporarilyBranchCode, dtQuoteDetails.Rows(0)("TemporarilyBranchCode").ToString)

            Dim cntPrevIndex As Integer = 1
            'Dim drsPrevIndex() As DataRow = _dtParamList.Select("PrevParam > 0")
            'If drsPrevIndex.Length > 0 Then
            '    cntPrevIndex = drsPrevIndex(drsPrevIndex.Length - 1)("TabIndex")
            'End If
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PrevParameterSelected, cntPrevIndex)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.language, dtQuoteDetails.Rows(0)("lang").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.vers, dtQuoteDetails.Rows(0)("vers").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.itemNumber, dtQuoteDetails.Rows(0)("itemNumber").ToString)
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CatalogNumber, dtQuoteDetails.Rows(0)("CatalogNum").ToString)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CustomerNumber, dtQuoteDetails.Rows(0)("CustomerNumber").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CustomerName, dtQuoteDetails.Rows(0)("CustomerName").ToString)
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFRCurrency, dtQuoteDetails.Rows(0)("TFRCurrency").ToString)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerPrice, dtQuoteDetails.Rows(0)("NET_STNCustomerPrice").ToString)
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerCurrency, dtQuoteDetails.Rows(0)("NET_STNCustomerCurrency").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchPrice, dtQuoteDetails.Rows(0)("TFR_STNBranchPrice").ToString)
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchCurrency, dtQuoteDetails.Rows(0)("TFR_STNBranchCurrency").ToString)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerPrice, dtQuoteDetails.Rows(0)("NET_STNCustomerPrice").ToString)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchPrice, dtQuoteDetails.Rows(0)("TFR_STNBranchPrice").ToString)
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchCurrency, dtQuoteDetails.Rows(0)("TFR_STNBranchCurrency").ToString)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMainMaterialCategory, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMainMaterial, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMaterial, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.MaterialsID, dtQuoteDetails.Rows(0)("MaterialsID").ToString)
            Dim dtm As DataTable = clsMaterial.GetSelectedMaterial(dtQuoteDetails.Rows(0)("MaterialsID").ToString)
            If Not dtm Is Nothing AndAlso dtm.Rows.Count > 0 Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMaterialTabDescription, dtm.Rows(0).Item("TabDescription").ToString)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMainMaterialCategory, dtm.Rows(0).Item("Category").ToString)
            Else
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMaterialTabDescription, "")

            End If


            Dim dtParametersLabels As DataTable = clsModel.GetParamsArrayByVal(dtQuoteDetails.Rows(0)("ModelNum").ToString, dtQuoteDetails.Rows(0)("OpenType").ToString, CInt(QuotationNumber), BranchCode)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParametersListView, dtParametersLabels)



            Dim itemNumber As String = dtQuoteDetails.Rows(0)("itemNumber").ToString
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Delivery_Weeks, dtQuoteDetails.Rows(0)("Delivery_Weeks").ToString)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerOrderNumber, dtQuoteDetails.Rows(0)("CustomerOrderNumber").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerItemNumber, dtQuoteDetails.Rows(0)("CustomerItemNumber").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerAdditionalReq, dtQuoteDetails.Rows(0)("CustomerAdditionalReq").ToString)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FolderPath, dtQuoteDetails.Rows(0)("FolderPath").ToString)
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.paymentTerms, dtQuoteDetails.Rows(0)("paymentTerm").ToString)
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.shipmethod, dtQuoteDetails.Rows(0)("shipmethod").ToString)


            StateManager.SetValue(StateManager.Keys.s_CustomerType, dtQuoteDetails.Rows(0)("CustomerType").ToString)

            If ConnectFromQutList = False Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyReportBuild, "YES")
                StateManager.SetValue(StateManager.Keys.s_shipmethod, dtQuoteDetails.Rows(0)("shipmethod").ToString)
                StateManager.SetValue(StateManager.Keys.s_paymentTerms, dtQuoteDetails.Rows(0)("paymentTerms").ToString)
                StateManager.SetValue(StateManager.Keys.s_salesperson, dtQuoteDetails.Rows(0)("salesperson").ToString)
                StateManager.SetValue(StateManager.Keys.s_salespersonEmail, dtQuoteDetails.Rows(0)("salespersonEmail").ToString)
                StateManager.SetValue(StateManager.Keys.s_deskUser, dtQuoteDetails.Rows(0)("deskUser").ToString)
                StateManager.SetValue(StateManager.Keys.s_deskUserEmail, dtQuoteDetails.Rows(0)("deskUserEmail").ToString)
                StateManager.SetValue(StateManager.Keys.s_technicalPerson, dtQuoteDetails.Rows(0)("technicalPerson").ToString)
                StateManager.SetValue(StateManager.Keys.s_technicalPersonEmail, dtQuoteDetails.Rows(0)("technicalPersonEmail").ToString)
            Else
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyReportBuild, "NO")
            End If


            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.DeliveryRad_Weeks, dtQuoteDetails.Rows(0)("DeliveryRad_Weeks").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ValidTime_Weeks, dtQuoteDetails.Rows(0)("ValidTime_Weeks").ToString)
            '  SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.MaterialsID, dtQuoteDetails.Rows(0)("MaterialsID").ToString)
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.OfferBy_ID, dtQuoteDetails.Rows(0)("OfferBy_ID"))
            Dim mc As String = dtQuoteDetails.Rows(0)("OpenType")

            Dim _dtParamList As DataTable = clsQuatation.GetQuatationParams(BranchCode, QuotationNumber)
            If mc = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelConfiguration, dtModelDetails)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListConfiguration, _dtParamList)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelNumConfiguration, dtQuoteDetails.Rows(0)("ModelNum").ToString)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelNumModification, 0)
                Dim cntCurrentIndex As Integer = _dtParamList.Rows.Count + 1
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CurrentParameterIndex, cntCurrentIndex)
            ElseIf mc = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                'Dim _dtParamList As DataTable = clsQuatation.GetQuatationParams(BranchCode, QuotationNumber)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelModification, dtModelDetails)

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListModification, _dtParamList)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelNumModification, dtQuoteDetails.Rows(0)("ModelNum").ToString)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelNumConfiguration, dtQuoteDetails.Rows(0)("ModelNum").ToString)
                Dim cntCurrentIndex As Integer = _dtParamList.Rows.Count + 1
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CurrentParameterIndex, cntCurrentIndex)
            End If

            'If Not MainPic Is Nothing Then
            '    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.mainPicL, dtQuoteDetails.Rows(0)("glbIMG_L"))
            '    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.mainPicR, dtQuoteDetails.Rows(0)("glbIMG_R"))
            'Else
            '    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.mainPicL, "")
            '    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.mainPicR, "")
            'End If

            '---------------------------------------------
            '---------------------------------------------
            '---------------------------------------------
            '---------------------------------------------
            '---------------------------------------------
            ''Dim dtQuotePrices_E As DataTable = clsQuatation.GeQuotationPrices(BranchCode, QuotationNumber)

            ''SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Consts, clsQuatation.GetQuatationConsts(BranchCode, QuotationNumber))

            ''Dim dtQuotePrices As New DataTable
            ''dtQuotePrices.Columns.Add("Price_ID")
            ''dtQuotePrices.Columns.Add("btnPrice")
            ''dtQuotePrices.Columns.Add("QTY")
            '''dtQuotePrices.Columns.Add("QuantityFactor", GetType(System.Decimal))
            ''dtQuotePrices.Columns.Add("NetPrice", GetType(System.Decimal))
            ''dtQuotePrices.Columns.Add("Total", GetType(System.Decimal))
            ''dtQuotePrices.Columns.Add("CostPrice", GetType(System.Decimal))
            ''dtQuotePrices.Columns.Add("GP", GetType(System.Decimal))

            ''dtQuotePrices.Columns.Add("DeliveryWeeks")
            ''dtQuotePrices.Columns.Add("TFRPrice", GetType(System.Decimal))

            ''dtQuotePrices.Columns.Add("btnDelete")

            ''If Not dtQuotePrices_E Is Nothing Then
            ''    For Each row As DataRow In dtQuotePrices_E.Rows
            ''        Dim dr As DataRow = dtQuotePrices.NewRow
            ''        dr.Item("QTY") = row.Item("QTY")
            ''        'dr.Item("QuantityFactor") = Format(row.Item("QuantityFactor"), "0.##").ToString
            ''        dr.Item("NetPrice") = Format(row.Item("NetPrice"), "0.##").ToString
            ''        dr.Item("Total") = Format(row.Item("Total"), "0.##").ToString
            ''        dr.Item("Costprice") = Format(row.Item("CostPrice"), "0.##").ToString
            ''        dr.Item("GP") = Format(row.Item("GP"), "0.##").ToString
            ''        dr.Item("DELIVERYWEEKS") = row.Item("DELIVERYWEEKS")
            ''        dr.Item("TFRPrice") = Format(row.Item("TFRPrice"), "0.##").ToString

            ''        dtQuotePrices.Rows.Add(dr)
            ''    Next
            ''End If


            ''Dim sXd As String = dtQuoteDetails.Rows(0)("ExpiredDate").ToString
            ''If Not sXd Is Nothing AndAlso sXd <> "" AndAlso IsDate(sXd) Then
            ''    If CDate(sXd) > Now.Date Then
            ''        If Not dtQuotePrices Is Nothing AndAlso dtQuotePrices.Rows.Count > 0 AndAlso dtQuotePrices.Rows.Count < 9 Then
            ''            dtQuotePrices.Rows.Add()
            ''        End If
            ''    End If
            ''End If



            ''SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Price, dtQuotePrices)
            ''dtQuotePrices = Nothing


            SetExistPrices(BranchCode, QuotationNumber, dtQuoteDetails)


            '---------------------------------------------
            '---------------------------------------------
            '---------------------------------------------
            '---------------------------------------------
            '---------------------------------------------
            '-------------------------------
            'FactorsQty
            Dim FactorsQty As New DataTable
            FactorsQty = clsQuatation.GetQTYFactorsParameters(BranchCode, QuotationNumber)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FactorsQty, FactorsQty)
            FactorsQty = Nothing
            '-------------------------------
            '-------------------------------
            '_dtFactorsWithValues
            Dim _dtParameters_Factors As New DataTable
            _dtParameters_Factors = clsQuatation.GetQuotationParametersFactors(BranchCode, QuotationNumber)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParametersFactors, _dtParameters_Factors)
            _dtParameters_Factors = Nothing
            '-------------------------------
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DC, clsPrice.Get_DC(_dtParamList))

            '--------------lcl_TQuotatioListModelParametersCode-----------------
            Dim dtTQuotatioListModelParametersCode As New DataTable
            dtTQuotatioListModelParametersCode = clsQuatation.GetQuotatioListModelParametersCode(BranchCode, QuotationNumber)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtTQuotatioListModelParametersCode, dtTQuotatioListModelParametersCode)

            '----------FORMULA-------------
            SetFormula(True, True, BranchCode, QuotationNumber)
            'Dim dtQuoteSpecialPrices As DataTable = clsQuatation.GeQuotationFormula(BranchCode, QuotationNumber)
            'If Not dtQuoteSpecialPrices Is Nothing AndAlso dtQuoteSpecialPrices.Rows.Count > 0 Then
            '    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormula, dtQuoteSpecialPrices.Rows(0)("PriceFormulaParamsNumbers").ToString)
            '    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormulaWithParamsNames, dtQuoteSpecialPrices.Rows(0)("PriceFormulaParamsNames").ToString)

            '  
            '    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormulaMKT, dtQuoteSpecialPrices.Rows(0)("PriceFormulaMKT").ToString)
            '    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormulaMKT_Value, dtQuoteSpecialPrices.Rows(0)("PriceFormulaMKT_Value").ToString)
            '    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormulaTFR, dtQuoteSpecialPrices.Rows(0)("PriceFormulaTFR").ToString)
            '    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceFormulaTFR_Value, dtQuoteSpecialPrices.Rows(0)("PriceFormulaTFR_Value").ToString)
            '    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BranchPriceFormulaMKT, dtQuoteSpecialPrices.Rows(0)("BranchPriceFormulaMKT").ToString)
            '    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BranchPriceFormulaMKT_Value, dtQuoteSpecialPrices.Rows(0)("BranchPriceFormulaMKT_Value").ToString)
            '    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BranchPriceFormulaTFR, dtQuoteSpecialPrices.Rows(0)("BranchPriceFormulaTFR").ToString)
            '    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BranchPriceFormulaTFR_Value, dtQuoteSpecialPrices.Rows(0)("BranchPriceFormulaTFR_Value").ToString)
            '    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._PriceCalculateFlag, dtQuoteSpecialPrices.Rows(0)("PriceCalculateFlag").ToString))

            '    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._CostFormula, dtQuoteSpecialPrices.Rows(0)("CostFormula").ToString)
            '    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._GPFormula, dtQuoteSpecialPrices.Rows(0)("GPFormula").ToString)
            '    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._CostFormulaValue, dtQuoteSpecialPrices.Rows(0)("CostFormulaValue").ToString)
            '    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DescriptionFormulaWithParamsNames, dtQuoteSpecialPrices.Rows(0)("DesctriptionFormulaNames").ToString)
            '    ''SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._CostFormulaWithParamsNames, dtQuoteSpecialPrices.Rows(0)("_CostFormulaWithParamsNames").ToString)
            '    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.SemiToolDescription, dtQuoteSpecialPrices.Rows(0)("DesctriptionFormulaNumbers").ToString)
            '    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ConstFormula, dtQuoteSpecialPrices.Rows(0)("ConstFormula").ToString)

            'End If



            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function


    Public Shared Sub SetExistPrices(BranchCode As String, QuotationNumber As String, sdtQuoteDetails As DataTable)
        Try


            Dim dtQuotePrices_E As DataTable = clsQuatation.GeQuotationPrices(BranchCode, QuotationNumber)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Consts, clsQuatation.GetQuatationConsts(BranchCode, QuotationNumber))

            Dim dtQuotePrices As New DataTable
            dtQuotePrices.Columns.Add("Price_ID")
            dtQuotePrices.Columns.Add("btnPrice")
            dtQuotePrices.Columns.Add("QTY")
            'dtQuotePrices.Columns.Add("QuantityFactor", GetType(System.Decimal))
            dtQuotePrices.Columns.Add("NetPrice", GetType(System.Decimal))
            dtQuotePrices.Columns.Add("Total", GetType(System.Decimal))
            dtQuotePrices.Columns.Add("CostPrice", GetType(System.Decimal))
            dtQuotePrices.Columns.Add("GP", GetType(System.Decimal))

            dtQuotePrices.Columns.Add("DeliveryWeeks")
            dtQuotePrices.Columns.Add("TFRPrice", GetType(System.Decimal))
            dtQuotePrices.Columns.Add("btnaddToCart")

            dtQuotePrices.Columns.Add("btnDelete")
            dtQuotePrices.Columns.Add("OrderedQuantity")
            dtQuotePrices.Columns.Add("QTYFct", GetType(System.Decimal))

            If Not dtQuotePrices_E Is Nothing Then
                For Each row As DataRow In dtQuotePrices_E.Rows
                    Dim dr As DataRow = dtQuotePrices.NewRow
                    dr.Item("QTY") = row.Item("QTY")
                    'dr.Item("QuantityFactor") = Format(row.Item("QuantityFactor"), "0.##").ToString
                    dr.Item("NetPrice") = Format(row.Item("NetPrice"), "0.##").ToString
                    dr.Item("Total") = Format(row.Item("Total"), "0.##").ToString
                    dr.Item("Costprice") = Format(row.Item("CostPrice"), "0.##").ToString
                    dr.Item("GP") = Format(row.Item("GP"), "0.##").ToString
                    dr.Item("DELIVERYWEEKS") = row.Item("DELIVERYWEEKS")
                    dr.Item("TFRPrice") = Format(row.Item("TFRPrice"), "0.##").ToString
                    dr.Item("OrderedQuantity") = row.Item("OrderedQuantity")
                    dr.Item("QTYFct") = Format(row.Item("QTYFct"), "0.##").ToString

                    dtQuotePrices.Rows.Add(dr)
                Next
            End If

            If Not sdtQuoteDetails Is Nothing Then
                Dim sXd As String = sdtQuoteDetails.Rows(0)("ExpiredDate").ToString
                If Not sXd Is Nothing AndAlso sXd <> "" AndAlso IsDate(sXd) Then
                    If CDate(sXd) > Now.Date Then
                        If Not dtQuotePrices Is Nothing AndAlso dtQuotePrices.Rows.Count > 0 AndAlso dtQuotePrices.Rows.Count < 9 Then
                            dtQuotePrices.Rows.Add()
                        End If
                    End If
                End If
            End If

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Price, dtQuotePrices)
            dtQuotePrices = Nothing
        Catch ex As Exception
            Throw
        End Try
    End Sub




    Public Shared Sub SetSessionDetails_Temporarily(bTemporarily As String)
        Try

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TemporarilyQuotation, bTemporarily.ToString.ToUpper)


        Catch ex As Exception

            Throw
        End Try

    End Sub











    Public Shared Function SetSessionDetails_SEMI_Duplicate(BranchCode As String, QuotationNumber As String) As Boolean
        Try

            'SessionManager.ClearSessionForEditMode()

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchCurrency, 0)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempdtParamsChangable, CType(Nothing, DataTable))

            ' SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DocumentsFIllDone, "NO")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyReportBuild, "NO")
            '  SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyReportBuildFilesNames, "")

            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempQuotationListCustomer, "")

            Clear_Sessions_ForBeginSendMessages()
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempAllReadyShowedtoBuildDrawingBSONAutomaticly, "NO")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempAllReadyTriedtoBuildDrawingAutomaticly, "NO")
            ' SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempRefreshPriceForm, "NO")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagModificationCatalogDeferent, "NO")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempDontGetPrice, "FALSE")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TimeStamp, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationStatus, clsQuatation.e_QuotationStatus.New_Qut)

            Dim dtQuoteDetails As DataTable = clsQuatation.GetQuatationDetails(BranchCode, QuotationNumber)
            If dtQuoteDetails Is Nothing Then
                Return False
            ElseIf dtQuoteDetails.Rows.Count < 1 Then
                Return False
            End If

            Dim dtModelDetails As DataTable = clsModel.GetModelDetails(Integer.Parse(dtQuoteDetails.Rows(0).Item("ModelNum")))

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._AllRedyBulidFormPostBack, "False")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationNumber, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TemporarilyQuotationID, "")

            SetSessionDetails_Temporarily(dtQuoteDetails.Rows(0)("TemporarilyQuotation").ToString)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.BranchCode, BranchCode)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationOpenType, dtQuoteDetails.Rows(0)("OpenType").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.familyNo, dtQuoteDetails.Rows(0)("FamilyNo").ToString)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerPrice, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerCurrency, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchPrice, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchCurrency, "")


            Dim sUdate As String = ClsDate.GetDateTimeReturnStringFormat(Now, ClsDate.Enum_DateFormatTypes._ddmmyyyy)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.LastUpdateDate, sUdate)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationDate, sUdate)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationDateWithTime, "")


            Dim sd As String = dtQuoteDetails.Rows(0)("ExpiredDate").ToString
            If IsDate(sd) Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ExpiredDate, "")
            End If

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagModificationRecursiveFinish, "True")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempAllReadyShowsTemporaryMessage, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempAllReadyShowsSaveMessage, "")

            '  SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempReportTabToShow, "PDF")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ShowQutPrice, dtQuoteDetails.Rows(0)("ShowQutPrice").ToString)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.AS400quoteReturnedJSON, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.AS400Number, "0")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.AS400RowNumber, "001")

            '  SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._BSONfolder, "")
            ' SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DrawingName, "")


            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.RateTFR_USD, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.RateMKT_USD, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FAMILYGP, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FAMILYGP_MaxQTY, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.SUBGP, "")

            Dim cntPrevIndex As Integer = 1

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PrevParameterSelected, cntPrevIndex)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.language, dtQuoteDetails.Rows(0)("lang").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.vers, dtQuoteDetails.Rows(0)("vers").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.itemNumber, dtQuoteDetails.Rows(0)("itemNumber").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CustomerNumber, dtQuoteDetails.Rows(0)("CustomerNumber").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CustomerName, dtQuoteDetails.Rows(0)("CustomerName").ToString)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerPrice, "") 'dtQuoteDetails.Rows(0)("NET_STNCustomerPrice").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchPrice, "") 'dtQuoteDetails.Rows(0)("TFR_STNBranchPrice").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerPrice, "") ' dtQuoteDetails.Rows(0)("NET_STNCustomerPrice").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TFR_STNBranchPrice, "") ' dtQuoteDetails.Rows(0)("TFR_STNBranchPrice").ToString)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMainMaterialCategory, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMainMaterial, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMaterial, "")
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.MaterialsID, dtQuoteDetails.Rows(0)("MaterialsID").ToString)
            Dim dtm As DataTable = clsMaterial.GetSelectedMaterial(dtQuoteDetails.Rows(0)("MaterialsID").ToString)
            If Not dtm Is Nothing AndAlso dtm.Rows.Count > 0 Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMaterialTabDescription, dtm.Rows(0).Item("TabDescription").ToString)
            Else
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMaterialTabDescription, "")
            End If

            Dim dtParametersLabels As DataTable = clsModel.GetParamsArrayByVal(dtQuoteDetails.Rows(0)("ModelNum").ToString, dtQuoteDetails.Rows(0)("OpenType").ToString, CInt(QuotationNumber), BranchCode)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParametersListView, dtParametersLabels)

            Dim itemNumber As String = dtQuoteDetails.Rows(0)("itemNumber").ToString
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.Delivery_Weeks, dtQuoteDetails.Rows(0)("Delivery_Weeks").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FolderPath, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerOrderNumber, dtQuoteDetails.Rows(0)("CustomerOrderNumber").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerItemNumber, dtQuoteDetails.Rows(0)("CustomerItemNumber").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.PricesCustomerAdditionalReq, dtQuoteDetails.Rows(0)("CustomerAdditionalReq").ToString)


            StateManager.SetValue(StateManager.Keys.s_shipmethod, dtQuoteDetails.Rows(0)("shipmethod").ToString)
            StateManager.SetValue(StateManager.Keys.s_CustomerType, dtQuoteDetails.Rows(0)("CustomerType").ToString)
            StateManager.SetValue(StateManager.Keys.s_paymentTerms, dtQuoteDetails.Rows(0)("paymentTerms").ToString)

            StateManager.SetValue(StateManager.Keys.s_salesperson, dtQuoteDetails.Rows(0)("salesperson").ToString)
            StateManager.SetValue(StateManager.Keys.s_salespersonEmail, dtQuoteDetails.Rows(0)("salespersonEmail").ToString)
            StateManager.SetValue(StateManager.Keys.s_deskUser, dtQuoteDetails.Rows(0)("deskUser").ToString)
            StateManager.SetValue(StateManager.Keys.s_deskUserEmail, dtQuoteDetails.Rows(0)("deskUserEmail").ToString)
            StateManager.SetValue(StateManager.Keys.s_technicalPerson, dtQuoteDetails.Rows(0)("technicalPerson").ToString)
            StateManager.SetValue(StateManager.Keys.s_technicalPersonEmail, dtQuoteDetails.Rows(0)("technicalPersonEmail").ToString)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.DeliveryRad_Weeks, dtQuoteDetails.Rows(0)("DeliveryRad_Weeks").ToString)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ValidTime_Weeks, dtQuoteDetails.Rows(0)("ValidTime_Weeks").ToString)

            Dim mc As String = dtQuoteDetails.Rows(0)("OpenType")

            Dim _dtParamList As DataTable = clsQuatation.GetQuatationParams(BranchCode, QuotationNumber)
            If mc = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelConfiguration, dtModelDetails)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListConfiguration, _dtParamList)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelNumConfiguration, dtQuoteDetails.Rows(0)("ModelNum").ToString)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelNumModification, 0)
                Dim cntCurrentIndex As Integer = _dtParamList.Rows.Count + 1
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CurrentParameterIndex, cntCurrentIndex)

                If Not _dtParamList Is Nothing AndAlso _dtParamList.Rows.Count > 0 Then
                    For Each erd As DataRow In _dtParamList.Rows
                        If erd.Item("Label") = "Workpiece Material" Then
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMainMaterialCategory, erd.Item("Stringvalue"))
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMainMaterial, erd.Item("Measure"))
                            Exit For
                        End If


                    Next
                End If
            ElseIf mc = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                'Dim _dtParamList As DataTable = clsQuatation.GetQuatationParams(BranchCode, QuotationNumber)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelModification, dtModelDetails)

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParamListModification, _dtParamList)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtModParamsFinalSelected, _dtParamList)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelNumModification, dtQuoteDetails.Rows(0)("ModelNum").ToString)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._ModelNumConfiguration, dtQuoteDetails.Rows(0)("ModelNum").ToString)
                Dim cntCurrentIndex As Integer = _dtParamList.Rows.Count + 1
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CurrentParameterIndex, cntCurrentIndex)
                If Not _dtParamList Is Nothing AndAlso _dtParamList.Rows.Count > 0 Then
                    For Each erd As DataRow In _dtParamList.Rows
                        If erd.Item("Label") = "Workpiece Material" Then
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMainMaterialCategory, erd.Item("Stringvalue"))
                            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TempMainMaterial, erd.Item("Measure"))
                            Exit For
                        End If


                    Next
                End If
            End If

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.dt_Price, CType(Nothing, DataTable))

            Dim FactorsQty As New DataTable
            FactorsQty = clsQuatation.GetQTYFactorsParameters(BranchCode, QuotationNumber)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FactorsQty, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtParametersFactors, CType(Nothing, DataTable))
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._TempDoConnect, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._DC, "")

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtTQuotatioListModelParametersCode, CType(Nothing, DataTable))

        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Shared Sub Clear_Sessions_OpenIquote(Er As String)
        Try
            If Er = "TRUE" Then
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ErrorInWSGAL, "TRUE")
            Else
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ErrorInWSGAL, "FALSE")
            End If
        Catch ex As Exception
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ErrorInWSGAL, "FALSE")
        End Try
    End Sub



    Private Shared Function GetKeyFor(Key As String) As String
        Try
            If Key <> "" Then
                If Not (HttpContext.Current.Request("rErepTr")) Is Nothing Then
                    Dim uniqid As String = HttpContext.Current.Request("rErepTr")
                    If uniqid <> "" Then
                        Key = Key & uniqid
                        Return Key
                    End If
                    'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationOpenType, "")
                    Return Key
                End If

            End If
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationOpenType, "")
            Return Key

        Catch ex As Exception
            'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationOpenType, "")
            Return Key
        End Try
        'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationOpenType, "")
        Return Key

    End Function




End Class





