Imports System.Configuration
Imports IscarDal
Imports Newtonsoft.Json.Linq

Public Class clsQuatation





    Enum Enum_QuotationOpenType As Integer
        NON = 0
        MODIFICATION = 1
        CONFIGURATOR = 2
        'CONFIGURATOR_CONTINUE = 3
    End Enum

    Enum e_QuotationStatus As Integer
        'NEW_QUOTATION_MODIF = 1
        'NEW_QUOTATION_CONFOG = 2
        'EXIST_QUOTATION_MODIF = 3
        'EXIST_QUOTATION_CONFOG = 4
        New_Qut = 0
        Exist_Qut_OpenedFromParameters = 1
        Exist_QutOpenedFromQuotationList = 2
    End Enum

    Public Shared Function ACTIVE_OpenType() As String
        If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False) Is Nothing Then
            Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)
        End If

        Return ""
    End Function
    Public Shared Function ACTIVE_OpenTypeName() As String

        Dim ModType As String = "Configuration"
        If clsQuatation.ACTIVE_OpenType = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
            ModType = "Modification"
        End If

        Return ModType

    End Function

    Public Shared Function ACTIVE_UseCustomerNo() As String
        Try
            Dim s As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, False)
            If s Is Nothing Then s = ""
            Return s
        Catch ex As Exception
            Return ""
        End Try

    End Function

    Public Shared Function ACTIVE_UseLoggedEmail() As String
        Try
            Dim s As String = StateManager.GetValue(StateManager.Keys.s_loggedEmail, False)
            If s Is Nothing Then s = ""
            Return s
        Catch ex As Exception
            Return ""
        End Try

    End Function


    Public Shared Function ACTIVE_ModelDT() As DataTable
        Try
            If clsQuatation.ACTIVE_OpenType.ToString = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then 'returnCurentOpenType() 
                Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelConfiguration, "")
            ElseIf clsQuatation.ACTIVE_OpenType.ToString = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then 'returnCurentOpenType() 
                Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelModification, "")
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function ACTIVE_QuotationNumber() As String
        Try

            Dim sQutNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number, False).ToString

            If Not sQutNo Is Nothing AndAlso sQutNo <> "" AndAlso IsNumeric(sQutNo) AndAlso CInt(sQutNo) > 0 Then
                Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number, False)
            Else
                Dim sSes As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False).ToString.Trim
                If Not sSes Is Nothing AndAlso sSes <> "" Then
                    Return sSes
                Else
                    Return "0"
                End If
                Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
            End If
        Catch ex As Exception
            Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
        End Try

    End Function
    Public Shared Function ACTIVE_ModelNumber() As String
        Try
            Dim sStart As String = clsQuatation.ACTIVE_OpenType.ToString 'returnCurentOpenType()

            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumConfiguration, False)
            ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelNumModification, False)
            Else
                Dim s As String = ""
                Return s
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetActiveQuotation_DTparams() As DataTable
        Dim OpenType As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType)

        Dim _dtParamList As DataTable = Nothing
        If OpenType = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
            _dtParamList = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListModification, "")

        ElseIf OpenType = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
            _dtParamList = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListConfiguration, "")
        End If
        Return _dtParamList
    End Function

    Public Shared Function GetQuotationConstants(ModelNum As Integer, ModelType As String, WorkCenterID As Integer, QTY As Integer) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum, 2))
            oParams.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, ModelType))
            oParams.Add(New SqlParam("@WorkCenterID", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, WorkCenterID, 2))
            oParams.Add(New SqlParam("@QTY", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QTY))


            dt = oSql.ExecuteSPReturnDT("USP_GetModelConstant", oParams)


            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function


    Public Shared Function GetFamily(FindString As String) As String
        Try

            'Dim ItemRefrence As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber)
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim Params As New SqlParams()
            Dim dtFam As DataTable

            'Params.Add(New SqlParam("@ItemRefrence", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, ItemRefrence, 10))
            Params.Add(New SqlParam("@FindString", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, FindString, 20))

            'dtFam = oSql.ExecuteSPReturnDT("USP_GetFamilyNo", Params)
            dtFam = oSql.ExecuteSPReturnDT("USP_GetFamilyNo_1", Params)

            If Not dtFam Is Nothing AndAlso dtFam.Rows.Count > 0 Then
                Return dtFam.Rows(0).Item(0).ToString
            End If
            Return ""
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function SetSTNTEMPLabel() As String

        Try
            Dim dt As DataTable = clsQuatation.GetActiveQuotation_DTparams
            Dim dv_Params As DataView = dt.Copy.DefaultView
            dv_Params.RowFilter = "Label Like 'Find String%'"
            Dim sStr As String = ""
            If dv_Params.Count > 0 Then
                sStr = dv_Params(0).Item("Measure").ToString
            End If


            'For i As Int16 = 0 To dv_Params.Count - 1
            '    sStr &= dv_Params(i).Item("Measure").ToString
            '    'lblSTNstr.Text = sStr
            'Next
            Return sStr
        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function getQuotationList(BranchCode As String, QuotationNum As String, itemNumber As String, Ordered As Boolean, Valid As Boolean, Expired As Boolean, Submitted As Boolean, loggedEmail As String, sAny As String, QuotationBranchCode As String, CusToNo As String) As DataTable

        Try

            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))

            If IsNumeric(QuotationNum) Then
                oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, CInt(QuotationNum)))
            End If
            If itemNumber <> "" Then
                oParams.Add(New SqlParam("@itemNumber", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, itemNumber, 20))
            End If
            'If sAny <> "" ThenThen "" >< sAny If'            


            '    oParams.Add(New SqlParam("@sAny", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sAny, 100))
            'End If
            oParams.Add(New SqlParam("@Ordered", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, Ordered))
            oParams.Add(New SqlParam("@Valid", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, Valid))
            oParams.Add(New SqlParam("@Expired", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, Expired))
            oParams.Add(New SqlParam("@Submitted", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, Submitted))
            oParams.Add(New SqlParam("@loggedEmail", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, loggedEmail, 50))
            oParams.Add(New SqlParam("@QuotationBranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, QuotationBranchCode, 2))
            If sAny.ToString.Trim <> "" Then
                oParams.Add(New SqlParam("@Any", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sAny.ToString.Trim))
            End If
            If CusToNo.ToString.Trim <> "" Then
                oParams.Add(New SqlParam("@customerNo", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, CusToNo.ToString.Trim))
            End If
            dt = oSql.ExecuteSPReturnDT("usp_GetQuotationsList", oParams)


            Return dt
        Catch ex As Exception
            Throw New NotImplementedException()
        End Try


    End Function

    Public Shared Function GetQuatationDetails(ByVal BranchCode As String, ByVal QuatationNumber As Integer) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuatationNumber, 4))

            dt = oSql.ExecuteSPReturnDT("USP_GetQuotationDetails", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetQuotationBySerialNo(ByVal SerialNo As String) As DataTable
        Try
            'ByVal BranchCode As String,
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, "ZZ", 2))
            oParams.Add(New SqlParam("@TemporarilyQuotationID", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, SerialNo, 200))

            dt = oSql.ExecuteSPReturnDT("USP_GetQuotationBySerialNo", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetQuotationListImages(ByVal BranchCode As String, ByVal QuatationNumber As Integer) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuatationNumber, 4))

            dt = oSql.ExecuteSPReturnDT("USP_GetQuotationListImages", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function





    Public Shared Function GeQuotationPrices(ByVal BranchCode As String, ByVal QuatationNumber As Integer) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuatationNumber, 4))

            dt = oSql.ExecuteSPReturnDT("USP_GetQuotationPrices", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GeQuotationPricesTransaction(ByVal BranchCode As String, ByVal QuatationNumber As Integer, PriceRevision As Integer) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuatationNumber, 4))
            oParams.Add(New SqlParam("@PriceRevision", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, PriceRevision))

            'dt = oSql.ExecuteSPReturnDT("USP_GetQuotationPricesTransaction", oParams)
            dt = oSql.ExecuteSPReturnDT("USP_GetQuotationPricesTransactionSeletect", oParams)



            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GeQuotationPricesOrdered(ByVal BranchCode As String, ByVal QuatationNumber As Integer) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuatationNumber, 4))

            dt = oSql.ExecuteSPReturnDT("USP_GetQuotationPricesOrdered", oParams)



            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GeQuotationFormula(ByVal BranchCode As String, ByVal QuatationNumber As Integer) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuatationNumber, 4))

            dt = oSql.ExecuteSPReturnDT("USP_GETQuotationFormula", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function



    Public Shared Function GetQuatationParams(ByVal BranchCode As String, ByVal QuatationNumber As Integer) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuatationNumber, 4))

            dt = oSql.ExecuteSPReturnDT("USP_GetQuotationParams", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetQuatationConsts(ByVal BranchCode As String, ByVal QuatationNumber As Integer) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuatationNumber, 4))

            dt = oSql.ExecuteSPReturnDT("USP_GetQuotationConsts", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function




    Public Shared Function GetQuotationStatusFromAS400(ByVal BranchCode As String,
    ByVal AS400Number As String, ByVal AS400RowNumber As String, ByVal As400AppCode As String,
 ByRef QuotationNum As String, ByRef ModelNumber As String, ByRef QuotationFormStart As String, DSCO As String, QZSPC As String) As DataTable
        GetQuotationStatusFromAS400 = Nothing
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()

            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParams.Add(New SqlParam("@AS400Number", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, AS400Number, 10))
            oParams.Add(New SqlParam("@AS400RowNumber", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, AS400RowNumber, 10))
            oParams.Add(New SqlParam("@As400AppCode", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, As400AppCode, 200))
            oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdOutput, "", 20))
            oParams.Add(New SqlParam("@ModelNumber", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdOutput, "", 10))
            oParams.Add(New SqlParam("@QuotationFormStart", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdOutput, "", 10))
            oParams.Add(New SqlParam("@DSCO", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, DSCO, 100))
            oParams.Add(New SqlParam("@QZSPC", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, QZSPC, 1))

            oSql.ExecuteSP("USP_GetQuotationAS400", oParams)

            QuotationNum = oParams.GetParameter("@QuotationNum").Value.ToString
            ModelNumber = oParams.GetParameter("@ModelNumber").Value.ToString
            QuotationFormStart = oParams.GetParameter("@QuotationFormStart").Value.ToString

        Catch ex As Exception
            Throw
        End Try
    End Function


    Public Shared Function ACTIVE_iQuoteQuotationNumber() As String
        Try

            Dim QutNumber As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)


            Return QutNumber.ToString
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Public Shared Function ACTIVE_GALQuotationNumber() As String
        Try

            Dim QutNumber As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number, False)


            Return QutNumber.ToString
        Catch ex As Exception
            Return ""
        End Try
    End Function




    Public Shared Function GETFamilylCuttingCondiotionValue(ByVal ModelId As String) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@ModelId", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, Integer.Parse(ModelId)))

            dt = oSql.ExecuteSPReturnDT("USP_GETFamilylCuttingCondiotionValue", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetQuotationDetailsByTimeStamp(ByVal UniqId As String) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@UniqId", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, UniqId))

            dt = oSql.ExecuteSPReturnDT("USP_GetimeStampUniqID", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetQTYFactorsParameters(ByVal BranchCode As String, ByVal QuatationNumber As Integer) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuatationNumber))
            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))

            dt = oSql.ExecuteSPReturnDT("usp_GetQuotatioListQTYFactorsParameters", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function


    Public Shared Function GetQuotationParametersFactors(ByVal BranchCode As String, ByVal QuatationNumber As Integer) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuatationNumber))
            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))

            dt = oSql.ExecuteSPReturnDT("usp_GetQuotatioList_ParametersFactors", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetQuotatioListModelParametersCode(ByVal BranchCode As String, ByVal QuatationNumber As Integer) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuatationNumber))
            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))

            dt = oSql.ExecuteSPReturnDT("usp_GetQuotatioListModelParametersCode", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function Get_CreateTimeStampUniqID(OpenType As String, Language As String, vesion As String, FamilyID As String, RequestApplication As String, ItemNumber As String) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@OpenType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, OpenType))
            oParams.Add(New SqlParam("@Language", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Language))
            oParams.Add(New SqlParam("@vesion", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, vesion))
            oParams.Add(New SqlParam("@FamilyID", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, FamilyID))
            oParams.Add(New SqlParam("@RequestApplication", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, RequestApplication))
            oParams.Add(New SqlParam("@ItemNumber", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, ItemNumber))
            dt = oSql.ExecuteSPReturnDT("USP_CreateTimeStampUniqID", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Sub GetCustomerAccountDetails(CustomerNo As String, _BranchCode As String)
        Try

            SessionManager.Clear_Sessions_ShowCustomerType()

            'Dim StringToGet As String = ""

            'StringToGet = "customer/" & CustomerNo

            Dim dt As DataTable = GAL.GetGalData("CUSTOMER", _BranchCode, ConfigurationManager.AppSettings("AS400APPpathForGetData"), CustomerNo, "", "", "", "", "", "", "", True)

            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then


                'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CustomerName, dt.Rows(0).Item("name").ToString.Trim)
                StateManager.SetValue(StateManager.Keys.s_CustomerName, dt.Rows(0).Item("name").ToString.Trim)
                'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerCurrency, dt.Rows(0).Item("invoicecurrency").ToString.Trim)
                StateManager.SetValue(StateManager.Keys.s_STNCustomerCurrency, dt.Rows(0).Item("invoicecurrency").ToString.Trim)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerCurrency, dt.Rows(0).Item("invoicecurrency").ToString.Trim)


                Dim sShM As String = ""
                Try
                    sShM = dt.Rows(0).Item("shipmethod").ToString.Trim()
                Catch ex As Exception
                    sShM = ""
                End Try

                Dim sPym As String = ""
                Try
                    sPym = dt.Rows(0).Item("paymentTerms").ToString.Trim()
                Catch ex As Exception
                    sPym = ""
                End Try

                Try
                    Dim Ssalesperson = dt.Rows(0).Item("salesperson").ToString.Trim()
                    Dim SsalespersonEmail = dt.Rows(0).Item("salespersonEmail").ToString.Trim()
                    Dim SdeskUser = dt.Rows(0).Item("deskUser").ToString.Trim()
                    Dim SdeskUserEmail = dt.Rows(0).Item("deskUserEmail").ToString.Trim()
                    Dim StechnicalPerson = dt.Rows(0).Item("technicalPerson").ToString.Trim()
                    Dim StechnicalPersonEmail = dt.Rows(0).Item("technicalPersonEmail").ToString.Trim()

                    StateManager.SetValue(StateManager.Keys.s_salesperson, Ssalesperson)
                    StateManager.SetValue(StateManager.Keys.s_salespersonEmail, SsalespersonEmail)
                    StateManager.SetValue(StateManager.Keys.s_deskUser, SdeskUser)
                    StateManager.SetValue(StateManager.Keys.s_deskUserEmail, SdeskUserEmail)
                    StateManager.SetValue(StateManager.Keys.s_technicalPerson, StechnicalPerson)
                    StateManager.SetValue(StateManager.Keys.s_technicalPersonEmail, StechnicalPersonEmail)

                Catch ex As Exception

                End Try

                Try
                    Dim spg As String = dt.Rows(0).Item("type").ToString.Trim()
                    StateManager.SetValue(StateManager.Keys.s_CustomerType, spg)

                    Dim dtspg As DataTable = customer.Get_CustomerQuotationOptions(spg)
                    If Not dtspg Is Nothing AndAlso dtspg.Rows.Count > 0 AndAlso dtspg.Rows(0).Item("ShowQuotation").ToString = True Then
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ShowQutPrice, "1")
                    Else

                        StateManager.SetValue(StateManager.Keys.s_TemporarilyBranchCode, StateManager.GetValue(StateManager.Keys.s_BranchCode, True))
                        StateManager.SetValue(StateManager.Keys.s_BranchCode, "ZZ")
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.BranchCode, "ZZ")
                        SessionManager.SetSessionDetails_Temporarily("TRUE")
                    End If
                Catch ex As Exception
                    sShM = ""
                End Try

                If sShM <> "" Then
                    Try

                        Dim dtSh As DataTable = GAL.GetGalData("IQUOTE", _BranchCode, ConfigurationManager.AppSettings("AS400APPpathForGetData"), CustomerNo, "", "", "", "", "shipmethod/" & sShM, "", "", True)
                        If Not dtSh Is Nothing AndAlso dtSh.Rows.Count > 0 Then
                            StateManager.SetValue(StateManager.Keys.s_shipmethod, dtSh.Rows(0).Item("shortdescription").ToString)
                        End If
                    Catch ex As Exception
                        'GeneralException.WriteEventErrors("Qut number: " & SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False) & " - Faild to get shipmethod", GeneralException.e_LogTitle.GENERAL.ToString)
                        GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "GetCustomerAccountDetails - Faild to get shipmethod", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

                    End Try
                End If

                If sPym <> "" Then
                    Try
                        'Dim dtPy As DataTable = EncryptionDecryption.clsGal_Data.GetGalData("IQUOTE", _BranchCode, "v2", CustomerNo, "", "", "", "", "paymentTerm/" & sPym)
                        Dim dtPy As DataTable = GAL.GetGalData("IQUOTE", _BranchCode, ConfigurationManager.AppSettings("AS400APPpathForGetData"), CustomerNo, "", "", "", "", "paymentTerm/" & sPym, "", "", True)
                        If Not dtPy Is Nothing AndAlso dtPy.Rows.Count > 0 Then
                            StateManager.SetValue(StateManager.Keys.s_paymentTerms, dtPy.Rows(0).Item("shortdescription").ToString)
                        End If
                    Catch ex As Exception
                        'GeneralException.WriteEventErrors("Qut number: " & SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False) & " - Faild to get paymentTerms", GeneralException.e_LogTitle.GENERAL.ToString)
                        GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "GetCustomerAccountDetails - Faild to get paymentTerms", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

                    End Try
                End If
            End If


            '   StringToGet = "customer/" & CustomerNo & "/address/"

            'If _BranchCode = "JU" Then
            '    CustomerNo = CustomerNo & "/address/11815"
            'End If

            Dim dtS As DataTable = GAL.GetGalData("CUSTOMERSUB", _BranchCode, ConfigurationManager.AppSettings("AS400APPpathForGetData"), CustomerNo, "", "", "", "", "", "", "", True)

            If Not dtS Is Nothing AndAlso dtS.Rows.Count > 0 Then
                'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CustomerAddress1, dtS.Rows(0).Item("address1").ToString.Trim)
                StateManager.SetValue(StateManager.Keys.s_CustomerAddress, dtS.Rows(0).Item("address1").ToString.Trim)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub



    Public Shared Function IsTemporaryCustomer() As Boolean

        Try
            Dim s_sgcn As String = ""
            s_sgcn = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, False)
            If s_sgcn Is Nothing Then s_sgcn = ""
            If s_sgcn = "" Or s_sgcn = "0" Or Not IsNumeric(s_sgcn) Then
                Return True
            End If
            Return False
        Catch ex As Exception
            Return True
        End Try
    End Function

    Public Shared Function IsTemporaryCustomerShowType() As Boolean

        Try
            Dim s_ShowPriceType As String = ""
            s_ShowPriceType = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ShowQutPrice, False).ToString.ToUpper.Trim
            If s_ShowPriceType Is Nothing Then s_ShowPriceType = ""

            If s_ShowPriceType = "" Or s_ShowPriceType = "0" Then
                Return True
            End If
            Return False
        Catch ex As Exception
            Return True
        End Try
    End Function

    Public Shared Function IsTemporaryLoggedEmail() As Boolean

        Try
            Dim s_QuotationLoggedEmail As String = ""

            s_QuotationLoggedEmail = StateManager.GetValue(StateManager.Keys.s_loggedEmail, False).ToString.ToUpper.Trim

            If s_QuotationLoggedEmail Is Nothing Then s_QuotationLoggedEmail = ""

            If s_QuotationLoggedEmail = "" Or s_QuotationLoggedEmail = "0" Or Not s_QuotationLoggedEmail.Contains("@") Then
                Return True
            End If

            Return False
        Catch ex As Exception
            Return True
        End Try
    End Function



    Public Shared Function IsTemporary_User(Check_ShowPriceType As Boolean, ToCheckIFUserLogedAndNotConnectToCustomer As Boolean) As Boolean

        Try
            Dim s_QuotationLoggedEmail As String = ""
            Dim s_BranchCode As String = ""


            s_QuotationLoggedEmail = StateManager.GetValue(StateManager.Keys.s_loggedEmail, False).ToString.ToUpper.Trim
            If s_QuotationLoggedEmail Is Nothing Then s_QuotationLoggedEmail = ""
            If s_QuotationLoggedEmail = "" Or s_QuotationLoggedEmail = "0" Or Not s_QuotationLoggedEmail.Contains("@") Then
                s_QuotationLoggedEmail = ""
            End If
            s_BranchCode = StateManager.GetValue(StateManager.Keys.s_BranchCode, False).ToString.ToUpper.Trim



            If s_BranchCode Is Nothing Then s_BranchCode = ""

            If ToCheckIFUserLogedAndNotConnectToCustomer = True Then
                If s_BranchCode = "ZZ" Or s_BranchCode = "" Then
                    Return True
                End If
            Else
                If s_QuotationLoggedEmail = "" Then
                    Return True
                End If
            End If

            If s_QuotationLoggedEmail = "" Or s_QuotationLoggedEmail = "0" Or Not s_QuotationLoggedEmail.Contains("@") Then
                Return True
            End If
            If ToCheckIFUserLogedAndNotConnectToCustomer = True Then
                If Check_ShowPriceType = True Then
                    Dim s_ShowPriceType As String = ""
                    s_ShowPriceType = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ShowQutPrice, False).ToString.ToUpper.Trim
                    If s_ShowPriceType Is Nothing Then s_ShowPriceType = ""
                    If s_ShowPriceType = "" Or s_ShowPriceType = "0" Then
                        Return True
                    End If
                End If


                If IsTemporaryCustomer() Then
                    Return True
                End If
            End If


            Return False
        Catch ex As Exception
            Return True
        End Try

    End Function

    Public Shared Function IsTemporary_Quotatiom(Optional Check_IsTemporary_User As Boolean = True) As Boolean

        Try
            Dim s_TemporaryQut As String = ""

            s_TemporaryQut = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TemporarilyQuotation, False).ToString.ToUpper.Trim

            If s_TemporaryQut Is Nothing Then s_TemporaryQut = ""

            If s_TemporaryQut = "" Or s_TemporaryQut = "TRUE" Or s_TemporaryQut = "1" Then
                Return True
            End If

            If Check_IsTemporary_User = True Then
                Return IsTemporary_User(True, True) 'False

            End If

        Catch ex As Exception
            Return True
        End Try
    End Function




    Public Shared Function Check_IfAllRedySentMailToCustomer(BranchCode As String, loggedEmail As String) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParams.Add(New SqlParam("@loggedEmail", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, loggedEmail, 50))

            dt = oSql.ExecuteSPReturnDT("USP_IsAccountLogin", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function



    Public Shared Function checkIfCanDoDrawingForActiveModel() As Boolean
        'Return True
        Try

            Dim dtm As DataTable = clsQuatation.ACTIVE_ModelDT

            If Not dtm Is Nothing AndAlso dtm.Rows.Count > 0 Then
                If CBool(dtm.Rows(0).Item("DoDrawing2Dand3D")) = False Then
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Return True
        End Try

    End Function
    Public Shared Function checkIfCanDoCalcPriceForActiveModel() As Boolean
        'Return True
        Try

            Dim dtm As DataTable = clsQuatation.ACTIVE_ModelDT

            If Not dtm Is Nothing AndAlso dtm.Rows.Count > 0 Then
                If CBool(dtm.Rows(0).Item("DoCalcPrice")) = False Then
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Return True
        End Try

    End Function




End Class

