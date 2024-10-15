Imports System.Configuration
Imports System.Drawing
'Imports System.Drawing.Imaging
Imports System.Text
Imports IscarDal

Public Class clsUpdateData
    Public Function InsertNewQuotation(ConfigORModification As String, imgPath As String, ByVal SourceQuotaionNo As Integer, TemporarilyQuotation As Boolean, lImage As Image, rImage As Image) As Integer
        Dim oTxSql As SqlDal
        oTxSql = Nothing
        Try


            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

            Dim oParams As New SqlParams()
            Dim QuotationNo As Integer = 0

            Dim BranchCode As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode)

            Dim dt_m As DataTable
            If ConfigORModification = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                dt_m = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelModification, "")
            Else
                dt_m = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelConfiguration, "")
            End If

            Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString 'dt_m.Rows(0)("ModelNum").ToString

            Dim AS400Number As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number, False)
            Dim AS400NumberStart As String = ""
            Dim AS400RowNumber As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400RowNumber, False)
            Dim AS400RowNumberStart As String = ""

            Dim TemporarilyQuotationID As String = ""


            Dim CustomerNumber As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
            Dim CustomerName As String = StateManager.GetValue(StateManager.Keys.s_CustomerName, True)
            Try
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CustomerNumber, CustomerNumber.ToString)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.CustomerName, CustomerName.ToString)
            Catch ex As Exception

            End Try


            Dim OfferBy_ID As Integer = "5" ' StateManager.GetValue(StateManager.Keys.s_UserId)

            Dim PicturePath As String = ModelNum
            Dim bitmapBytes As Byte() = Nothing

            Dim LocalCustomer As String = SessionManager.LocalCustomer.LOCAL ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.LocalCustomer)

            '------------lcl_TQuotationList------------
            Dim iOpenType As Integer = 0
            If IsNumeric(ConfigORModification) Then
                iOpenType = CInt(ConfigORModification)
            End If

            Dim FamilyNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.familyNo)
            Dim itemNumber As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber)
            Dim vers As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.vers)
            Dim lang As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.language)

            Dim loggedEmail As String = StateManager.GetValue(StateManager.Keys.s_loggedEmail, False)


            Dim dExpDate As Date = Now
            Try
                If TemporarilyQuotation = True Then
                    dExpDate = Now.AddDays(CDbl(ConfigurationManager.AppSettings("TemporaryExpiryDateDays")))
                Else
                    Dim s As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ValidTime_Weeks, False)
                    If IsNumeric(s) Then
                        dExpDate = Now.AddDays(CInt(s) * 7)
                    End If
                End If


            Catch ex As Exception

            End Try

            'oParams.Add(New SqlParam("@QuotationStart", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, QuotationStart, 12))
            oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdOutput, 0)) 'QuotationNo
            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParams.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum))
            oParams.Add(New SqlParam("@AS400Number", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdOutput, AS400Number, 10))
            oParams.Add(New SqlParam("@AS400NumberStart", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, AS400NumberStart, 10))
            oParams.Add(New SqlParam("@AS400RowNumber", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdOutput, AS400RowNumber, 10))
            oParams.Add(New SqlParam("@AS400RowNumberStart", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, AS400RowNumberStart, 10))
            oParams.Add(New SqlParam("@CustomerNumber", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, CustomerNumber, 10))
            oParams.Add(New SqlParam("@CustomerName", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, CustomerName, 255))
            oParams.Add(New SqlParam("@OfferBy_ID", SqlParam.ParamType.ptSmallInt, SqlParam.ParamDirection.pdInput, OfferBy_ID))
            oParams.Add(New SqlParam("@BranchNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, 0))
            'oParams.Add(New SqlParam("@BranchNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, BranchNum))
            oParams.Add(New SqlParam("@WorkCenterID", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, 0))
            oParams.Add(New SqlParam("@LocalCustomer", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, LocalCustomer, 1))
            'oParams.Add(New SqlParam("@img", SqlParam.ParamType.ptImage, SqlParam.ParamDirection.pdInput, bitmapBytes, bitmapBytes.Length))
            oParams.Add(New SqlParam("@img", SqlParam.ParamType.ptImage, SqlParam.ParamDirection.pdInput, bitmapBytes))
            oParams.Add(New SqlParam("@SourceQuotaionNo", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, SourceQuotaionNo))
            oParams.Add(New SqlParam("@OpenType", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, iOpenType, 20))
            oParams.Add(New SqlParam("@FamilyNo", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, FamilyNo, 20))
            oParams.Add(New SqlParam("@itemNumber", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, itemNumber, 20))
            oParams.Add(New SqlParam("@vers", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, vers, 20))
            oParams.Add(New SqlParam("@lang", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, lang, 20))
            oParams.Add(New SqlParam("@TemporarilyQuotation", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, TemporarilyQuotation))
            oParams.Add(New SqlParam("@TemporarilyQuotationID", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdOutput, 0, 200))
            oParams.Add(New SqlParam("@loggedEmail", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, loggedEmail, 20))
            oParams.Add(New SqlParam("@QuotationDate", SqlParam.ParamType.ptDateTime, SqlParam.ParamDirection.pdOutput, Now))
            oParams.Add(New SqlParam("@ExpiredDate", SqlParam.ParamType.ptDateTime, SqlParam.ParamDirection.pdInput, dExpDate))
            oParams.Add(New SqlParam("@FolderPath", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdOutput, "", 200))

            oTxSql.ExecuteSP("USP_AddQuotation", oParams)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationStatus, clsQuatation.e_QuotationStatus.Exist_Qut_OpenedFromParameters)

            QuotationNo = oParams.GetParameter("@QuotationNum").Value
            TemporarilyQuotationID = oParams.GetParameter("@TemporarilyQuotationID").Value

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.TemporarilyQuotationID, TemporarilyQuotationID)

            If QuotationNo > 0 Then

                Try



                    Dim sSdate As String = oParams.GetParameter("@QuotationDate").Value
                    Dim sFolderPath As String = oParams.GetParameter("@FolderPath").Value

                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FolderPath, sFolderPath)

                    If IsDate(sSdate) Then
                        sSdate = ClsDate.GetDateTimeReturnStringFormat(sSdate, ClsDate.Enum_DateFormatTypes._ddmmyyyy)
                    End If
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationDate, sSdate)

                    If IsDate(dExpDate) Then
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ExpiredDate, ClsDate.GetDateTimeReturnStringFormat(dExpDate, ClsDate.Enum_DateFormatTypes._ddmmyyyy)) 'oParams.GetParameter("@ExpiredDate").Value)
                    End If

                Catch ex As Exception

                End Try
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationNumber, QuotationNo)

                Dim sStart As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False)
                If sStart.ToString = "" Then sStart = "0"
                UpdateQuotation(CInt(sStart), QuotationNo, imgPath, TemporarilyQuotation, lImage, rImage)

            End If

            UpdateConstants()

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagPricesChanged, False)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagParametersChanged, False)


            Return QuotationNo

        Catch ex As Exception
            Try
                Dim LogoSql As New SqlDal(clsBranch.GetSiteConnectionString)
                Dim LogoParams As New SqlParams()
                Dim LogQuotationNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber)
                Dim LogUser_ID As String = ""
                Dim LogUser_Name As String = StateManager.GetValue(StateManager.Keys.s_DisplayName)
                Dim Logdt_m As DataTable
                If ConfigORModification = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                    Logdt_m = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelModification, "")
                Else
                    Logdt_m = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelConfiguration, "")
                End If
                Dim LogModelNum As String = Logdt_m.Rows(0)("ModelNum").ToString
                Dim LogBranchCode As String = StateManager.GetValue(StateManager.Keys.s_BranchCode)
                LogoParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, LogQuotationNo, 50))
                LogoParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, LogBranchCode, 50))
                LogoParams.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, LogModelNum, 50))
                LogoParams.Add(New SqlParam("@User_ID", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, LogUser_ID, 50))
                LogoParams.Add(New SqlParam("@User_Name", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, LogUser_Name, 50))

                LogoSql.ExecuteSP("USP_AddQuotationLogInsert", LogoParams)
            Catch ex1 As Exception

            End Try

            Throw
        End Try
    End Function
    Public Shared Sub UpdateQuotationFactorsQty()
        Try
            Dim oTxSql As New SqlDal
            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams

            Dim dt_dtFactors As DataTable = Nothing
            dt_dtFactors = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FactorsQty, "")

            Dim QuotationNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)


            Dim BranchCode As String
            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode) = "" Then
                BranchCode = StateManager.GetValue(StateManager.Keys.s_BranchCode)
            Else
                BranchCode = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode)
            End If

            Dim ModelNum As String = clsQuatation.ACTIVE_ModelNumber

            Dim oParamsS As New SqlParams
            oParamsS.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNo))
            oParamsS.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oTxSql.ExecuteSP("USP_DeleteQTYFactorsParameters", oParamsS)
            oParamsS = Nothing

            If Not dt_dtFactors Is Nothing AndAlso dt_dtFactors.Columns.Count > 0 Then


                For Each row As DataRow In dt_dtFactors.Rows
                    Try
                        Dim QTY As Integer = row("QTY")
                        Dim QtyString As String = row("QTYString")
                        Dim OrderSeq As Integer = row("OrderSeq")
                        Dim FactorValue As Decimal = row("FactorValue")
                        Dim FactorType As String = row("FactorType").ToString
                        Dim Cond1 As String = row("Cond1").ToString
                        Dim Param1 As String = row("Param1").ToString
                        Dim Opr1 As String = row("Opr1").ToString
                        Dim Val1 As String = row("Val1").ToString
                        Dim Cond2 As String = row("Cond2").ToString
                        Dim Param2 As String = row("Param2").ToString
                        Dim Opr2 As String = row("Opr2").ToString
                        Dim Val2 As String = row("Val2").ToString
                        Dim Cond3 As String = row("Cond3").ToString
                        Dim Param3 As String = row("Param3").ToString
                        Dim Opr3 As String = row("Opr3").ToString
                        Dim Val3 As String = row("Val3").ToString
                        Dim Cond4 As String = row("Cond4").ToString
                        Dim Param4 As String = row("Param4").ToString
                        Dim Opr4 As String = row("Opr4").ToString
                        Dim Val4 As String = row("Val4").ToString
                        Dim Condition1 As String = row("Condition1").ToString
                        Dim Condition2 As String = row("Condition2").ToString
                        Dim FormulaFactorValue As String = row("FormulaFactorValue").ToString

                        If IsNumeric(QTY.ToString) AndAlso IsNumeric(OrderSeq.ToString) AndAlso IsNumeric(QuotationNo.ToString) AndAlso IsNumeric(ModelNum.ToString) Then
                            oParams = New SqlParams()
                            oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNo))
                            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
                            oParams.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum))
                            oParams.Add(New SqlParam("@Qty", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QTY))
                            oParams.Add(New SqlParam("@QtyString", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, QtyString))
                            oParams.Add(New SqlParam("@OrderSeq", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, OrderSeq))
                            oParams.Add(New SqlParam("@FactorValue", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, FactorValue))
                            oParams.Add(New SqlParam("@FactorType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, FactorType))
                            oParams.Add(New SqlParam("@Cond1", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Cond1))
                            oParams.Add(New SqlParam("@Param1", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Param1))
                            oParams.Add(New SqlParam("@Opr1", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Opr1))
                            oParams.Add(New SqlParam("@Val1", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Val1))
                            oParams.Add(New SqlParam("@Cond2", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Cond2))
                            oParams.Add(New SqlParam("@Param2", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Param2))
                            oParams.Add(New SqlParam("@Opr2", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Opr2))
                            oParams.Add(New SqlParam("@Val2", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Val2))
                            oParams.Add(New SqlParam("@Cond3", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Cond3))
                            oParams.Add(New SqlParam("@Param3", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Param3))
                            oParams.Add(New SqlParam("@Opr3", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Opr3))
                            oParams.Add(New SqlParam("@Val3", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Val3))
                            oParams.Add(New SqlParam("@Cond4", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Cond4))
                            oParams.Add(New SqlParam("@Param4", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Param4))
                            oParams.Add(New SqlParam("@Opr4", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Opr4))
                            oParams.Add(New SqlParam("@Val4", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Val4))
                            oParams.Add(New SqlParam("@Condition1", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Condition1))
                            oParams.Add(New SqlParam("@Condition2", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Condition2))
                            If IsNumeric(row("SP_SETUP").ToString) Then
                                oParams.Add(New SqlParam("@SP_SETUP", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, CInt(row("SP_SETUP"))))
                            End If
                            If IsNumeric(row("STN_SETUP").ToString) Then
                                oParams.Add(New SqlParam("@STN_SETUP", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, CInt(row("STN_SETUP"))))
                            End If
                            oParams.Add(New SqlParam("@FormulaFactorValue", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, FormulaFactorValue))


                            oTxSql.ExecuteSP("USP_AddQTYFactorsParameters", oParams)

                        End If
                    Catch ex As Exception

                    End Try
                Next

            End If

            oParams = Nothing
            oTxSql = Nothing

        Catch ex As Exception

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateQuotationFactorsQty", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        End Try

    End Sub
    Public Shared Sub UpdateQuotationFactorsQuotationFactors()
        Try
            Dim oTxSql As New SqlDal
            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams

            Dim _Tempdt_Factors As DataTable = Nothing
            _Tempdt_Factors = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParametersFactors, "") '_dtFactorsWithValues

            Dim QuotationNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)


            Dim BranchCode As String
            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode) = "" Then
                BranchCode = StateManager.GetValue(StateManager.Keys.s_BranchCode)
            Else
                BranchCode = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode)
            End If

            Dim ModelNum As String = clsQuatation.ACTIVE_ModelNumber

            Dim oParamsS As New SqlParams
            oParamsS.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNo))
            oParamsS.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oTxSql.ExecuteSP("USP_DeleteQuotation_ParametersFactors", oParamsS)
            oParamsS = Nothing

            If Not _Tempdt_Factors Is Nothing AndAlso _Tempdt_Factors.Columns.Count > 0 Then

                For Each row As DataRow In _Tempdt_Factors.Rows
                    Try
                        Dim ParamCode As String = row("ParamCode")
                        Dim MinValue As Decimal = row("MinValue")
                        Dim MaxValue As Decimal = row("MaxValue")
                        Dim OrderSeq As Integer = row("OrderSeq")
                        Dim FactorParam As String = row("FactorParam")

                        Dim FactorValue As Decimal = row("FactorValue")
                        Dim FactorType As String = row("FactorType").ToString

                        Dim Condition1 As String = row("Condition1").ToString
                        Dim Condition2 As String = row("Condition2").ToString
                        Dim Condition3 As String = row("Condition3").ToString

                        Dim Manipulation As String = row("Manipulation").ToString
                        Dim Scale As String = row("Scale").ToString
                        Dim MetCondition As String = row("MetCondition").ToString
                        Dim bMetCondition As Boolean = False
                        If MetCondition = "True" Then
                            bMetCondition = True
                        End If
                        If IsNumeric(OrderSeq.ToString) AndAlso IsNumeric(QuotationNo.ToString) AndAlso IsNumeric(ModelNum.ToString) Then
                            oParams = New SqlParams()
                            oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNo))
                            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
                            oParams.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum))
                            oParams.Add(New SqlParam("@ParamCode", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, ParamCode))
                            oParams.Add(New SqlParam("@MinValue", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, MinValue))
                            oParams.Add(New SqlParam("@MaxValue", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, MaxValue))
                            oParams.Add(New SqlParam("@OrderSeq", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, OrderSeq))

                            oParams.Add(New SqlParam("@FactorParam", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, FactorParam))
                            oParams.Add(New SqlParam("@FactorValue", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, FactorValue))
                            oParams.Add(New SqlParam("@FactorType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, FactorType))
                            oParams.Add(New SqlParam("@Condition1", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Condition1))
                            oParams.Add(New SqlParam("@Condition2", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Condition2))
                            oParams.Add(New SqlParam("@Condition3", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Condition3))
                            oParams.Add(New SqlParam("@Manipulation", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Manipulation))
                            oParams.Add(New SqlParam("@Scale", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Scale))
                            oParams.Add(New SqlParam("@MetCondition", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, bMetCondition))

                            oTxSql.ExecuteSP("USP_AddQuotation_ParametersFactors", oParams)

                        End If
                    Catch ex As Exception

                    End Try
                Next

            End If

            oParams = Nothing
            oTxSql = Nothing

        Catch ex As Exception

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateQuotationFactorsQuotationFactors", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        End Try

    End Sub
    Public Shared Sub UpdateQuotationFormula()
        Try

            Dim QuotationNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
            Dim BranchCode As String = ""

            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode) = "" Then
                BranchCode = StateManager.GetValue(StateManager.Keys.s_BranchCode)
            Else
                BranchCode = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode)
            End If

            Dim oTxSql As SqlDal
            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

            Dim oParamsS As New SqlParams
            oParamsS.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNo))
            oParamsS.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oTxSql.ExecuteSP("USP_DeleteQuotationFormula", oParamsS)
            oParamsS = Nothing

            'Dim P_FormulaNumbers As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormula, False)
            'Dim P_FormulaNames As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormulaWithParamsNames, False)

            Dim P_PriceFormulaMKT As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormula_MKT, False)
            Dim P_PriceFormulaMKT_Value As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormulaMKT_Value, False)

            Dim P_PriceFormulaTFR As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormulaTFR, False)
            Dim P_PriceFormulaTFR_Value As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceFormulaTFR_Value, False)

            Dim P_BranchPriceFormulaMKT As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BranchPriceFormulaMKT, False)
            Dim P_BranchPriceFormulaMKT_Value As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BranchPriceFormulaMKT_Value, False)

            Dim P_BranchPriceFormulaTFR As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BranchPriceFormulaTFR, False)
            Dim P_BranchPriceFormulaTFR_Value As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BranchPriceFormulaTFR_Value, False)


            Dim SemiToolDescription As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.SemiToolDescription, False)
            Dim D_Formula As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._DescriptionFormulaWithParamsNames, False)
            'Dim C_CostFormula As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._CostFormulaWithParamsNames, False)
            Dim C_CostFormula As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._CostFormula, False)
            Dim C_GPFormula As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._GPFormula, False)
            Dim C_CostFormulaValue As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._CostFormulaValue, False)
            Dim C_ConstFormula As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ConstFormula, False)

            Dim P_PriceCalculateFlag As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._PriceCalculateFlag, False)

            Dim oParamsD As New SqlParams()
            oParamsD.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNo))
            oParamsD.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))

            oParamsD.Add(New SqlParam("@DesctriptionFormulaNames", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, D_Formula, 200))
            oParamsD.Add(New SqlParam("@DesctriptionFormulaNumbers", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, SemiToolDescription, 200))
            oParamsD.Add(New SqlParam("@CostFormula", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, C_CostFormula, 200))
            oParamsD.Add(New SqlParam("@GPFormula", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, C_GPFormula, 200))
            oParamsD.Add(New SqlParam("@CostFormulaValue", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, C_CostFormulaValue, 200))
            oParamsD.Add(New SqlParam("@ConstFormula", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, C_ConstFormula, 200))

            oParamsD.Add(New SqlParam("@PriceFormulaMKT", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, P_PriceFormulaMKT, 200))
            oParamsD.Add(New SqlParam("@PriceFormulaMKT_Value", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, P_PriceFormulaMKT_Value, 200))

            oParamsD.Add(New SqlParam("@PriceFormulaTFR", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, P_PriceFormulaTFR, 200))
            oParamsD.Add(New SqlParam("@PriceFormulaTFR_Value", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, P_PriceFormulaTFR_Value, 200))

            oParamsD.Add(New SqlParam("@BranchPriceFormulaMKT", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, P_BranchPriceFormulaMKT, 200))
            oParamsD.Add(New SqlParam("@BranchPriceFormulaMKT_Value", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, P_BranchPriceFormulaMKT_Value, 200))

            oParamsD.Add(New SqlParam("@BranchPriceFormulaTFR", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, P_BranchPriceFormulaTFR, 200))
            oParamsD.Add(New SqlParam("@BranchPriceFormulaTFR_Value", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, P_BranchPriceFormulaTFR_Value, 200))

            oParamsD.Add(New SqlParam("@PriceCalculateFlag", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, P_PriceCalculateFlag, 200))

            oTxSql.ExecuteSP("USP_AddQuotationFormula_n", oParamsD)

            oParamsD = Nothing
            oTxSql = Nothing


        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateQuotationFormula", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
            Throw
        End Try
    End Sub
    Public Shared Sub UpdateQuotationSubmitted(submitted As String, SubmmittedEmail As String)
        Try

            Dim QuotationNo As String = clsQuatation.ACTIVE_QuotationNumber
            Dim BranchCode As String = clsBranch.ReturnActiveBranchCodeForDocuments

            Dim oTxSql As SqlDal
            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

            Dim oParamsD As New SqlParams()
            oParamsD.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, QuotationNo))
            oParamsD.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParamsD.Add(New SqlParam("@Submitted", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, submitted, 1))
            oParamsD.Add(New SqlParam("@SubmmittedEmail", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, SubmmittedEmail))

            oTxSql.ExecuteSP("USP_UpdateQuotationSubmitted", oParamsD)

            oParamsD = Nothing
            oTxSql = Nothing

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateQuotationSubmitted", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
            Throw
        End Try
    End Sub
    Public Shared Sub UpdateQuotationOrdered(OrderQuantyty As Integer, OrderedEmail As String, OrderedPhoneNo As String, CustomerOrderNumber As String, CustomerItemNumber As String, CustomerAdditionalReq As String)
        Try
            Dim AS400NO As String = clsQuatation.ACTIVE_QuotationNumber
            Dim BranchCode As String = clsBranch.ReturnActiveBranchCodeForDocuments

            Dim oTxSql As SqlDal
            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

            Dim oParamsD As New SqlParams()
            oParamsD.Add(New SqlParam("@AS400NO", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, AS400NO))
            oParamsD.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParamsD.Add(New SqlParam("@Quantity", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, OrderQuantyty))
            oParamsD.Add(New SqlParam("@OrderedPhoneNo", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, OrderedPhoneNo))
            oParamsD.Add(New SqlParam("@OrderedEmail", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, OrderedEmail))

            oParamsD.Add(New SqlParam("@CustomerOrderNumber", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, CustomerOrderNumber))
            oParamsD.Add(New SqlParam("@CustomerItemNumber", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, CustomerItemNumber))
            oParamsD.Add(New SqlParam("@CustomerAdditionalReq", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, CustomerAdditionalReq))


            oTxSql.ExecuteSP("USP_UpdateQuotationOrdered", oParamsD)

            oParamsD = Nothing
            oTxSql = Nothing

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateQuotationSubmitted", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        End Try
    End Sub
    Public Shared Sub UpdateQuotationSuccess(DrawingType As String, CatiaBson As Boolean, Catia2D As Boolean, Catia3DPDF As Boolean, QuotationNo As String, BranchCode As String)
        Try

            Dim oTxSql As SqlDal
            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

            Dim oParamsD As New SqlParams()
            oParamsD.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, QuotationNo))
            oParamsD.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParamsD.Add(New SqlParam("@CatiaBson", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, CatiaBson))
            oParamsD.Add(New SqlParam("@Catia2D", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, Catia2D))
            oParamsD.Add(New SqlParam("@Catia3DPDF", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, Catia3DPDF))
            oParamsD.Add(New SqlParam("@UpdateType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, DrawingType, 20))



            oTxSql.ExecuteSP("USP_UpdateQuotationDrawingSuccess", oParamsD)

            oParamsD = Nothing
            oTxSql = Nothing

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateQuotationSuccess", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        End Try
    End Sub

    Public Shared Sub UpdateQuotationBSONThread(QuotationNo As String, BranchCode As String, BSONFolder As String)
        Try

            Dim oTxSql As SqlDal
            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

            Dim oParamsD As New SqlParams()
            oParamsD.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, QuotationNo))
            oParamsD.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParamsD.Add(New SqlParam("@BSONFolder", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, BSONFolder, 200))

            oTxSql.ExecuteSP("USP_UpdateQuotationBSON", oParamsD)

            oParamsD = Nothing
            oTxSql = Nothing

        Catch ex As Exception
            'GeneralException.WriteEventErrors("UpdateQuotationBSONThread-" & ex.Message, GeneralException.e_LogTitle.UPDATEDATA.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateQuotationBSONThread", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)


            Throw
        End Try
    End Sub
    Public Shared Sub UpdateQuotationAS400Number(AS400number As String, AS400Row As String, BranchCode As String, QuotationNumber As Integer, AS400quoteReturnedJSON As String)
        Try

            Dim QuotationNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
            Dim oTxSql As SqlDal
            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

            Dim oParamsD As New SqlParams()
            oParamsD.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNumber))
            oParamsD.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParamsD.Add(New SqlParam("@AS400Number", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, AS400number))
            oParamsD.Add(New SqlParam("@AS400RowNumber", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, AS400Row))
            oParamsD.Add(New SqlParam("@AS400quoteReturnedJSON", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, AS400quoteReturnedJSON))
            oParamsD.Add(New SqlParam("@FolderPath", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdOutput, "", 200))

            oTxSql.ExecuteSP("USP_UpdateQuotationAS400Number", oParamsD)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FolderPath, oParamsD.GetParameter("@FolderPath").Value)

            oParamsD = Nothing
            oTxSql = Nothing

        Catch ex As Exception
            'GeneralException.WriteEventErrors("UpdateQuotationAS400Number-" & ex.Message, GeneralException.e_LogTitle.UPDATEDATA.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateQuotationAS400Number", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        End Try

    End Sub
    'Public Shared Sub UpdateQuotationDrawing()
    '    Try

    '        Dim QuotationNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
    '        Dim BranchCode As String = ""

    '        If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode) = "" Then
    '            BranchCode = StateManager.GetValue(StateManager.Keys.s_BranchCode)
    '        Else
    '            BranchCode = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode)
    '        End If

    '        Dim sQutNoAs400 As String = clsQuatation.ACTIVE_QuotationNumber
    '        Dim qnam As String = "DRW_" & Format(CInt(sQutNoAs400), "0000000") & ".pdf"

    '        Dim DrawingName As String = qnam 'SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._DrawingName, False)

    '        Dim oTxSql As SqlDal
    '        oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

    '        Dim oParamsD As New SqlParams()
    '        oParamsD.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNo))
    '        oParamsD.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
    '        oParamsD.Add(New SqlParam("@DrawingName", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, DrawingName, 200))

    '        oTxSql.ExecuteSP("USP_UpdateQuotationDrawing", oParamsD)

    '        oParamsD = Nothing
    '        oTxSql = Nothing

    '    Catch ex As Exception
    '        'GeneralException.WriteEventErrors("UpdateQuotationDrawing-" & ex.Message, GeneralException.e_LogTitle.UPDATEDATA.ToString)
    '        GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateQuotationDrawing", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

    '        Throw
    '    End Try
    'End Sub

    Public Shared Sub UpdateQuotationDataAfter_Connect(TemporarilyQuotation As Boolean)
        Try
            Dim BranchCode As String = ""
            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode) = "" Then
                BranchCode = StateManager.GetValue(StateManager.Keys.s_BranchCode)
            Else
                BranchCode = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode)
            End If
            If BranchCode <> "ZZ" Then
                Dim QuotationNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
                Dim ValidTime_Weeks As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ValidTime_Weeks, False)

                If Not IsNumeric(ValidTime_Weeks.ToString) Then
                    ValidTime_Weeks = 1
                End If

                Dim oTxSql As SqlDal
                oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

                Dim oParamsD As New SqlParams()
                oParamsD.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNo))
                oParamsD.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
                oParamsD.Add(New SqlParam("@TemporarilyQuotation", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, TemporarilyQuotation))
                oParamsD.Add(New SqlParam("@ValidTime_Weeks", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ValidTime_Weeks))
                oParamsD.Add(New SqlParam("@ExpiredDate", SqlParam.ParamType.ptDateTime, SqlParam.ParamDirection.pdOutput, 0))
                oParamsD.Add(New SqlParam("@FolderPath", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdOutput, "", 200))

                oTxSql.ExecuteSP("UpdateQuotationDataAfter_Connect", oParamsD)

                Dim dExpDate As String = oParamsD.GetParameter("@ExpiredDate").Value

                If IsDate(dExpDate) Then
                    'dExpDate = ClsDate.GetDateTimeReturnStringFormat(dExpDate, ClsDate.Enum_DateFormatTypes._ddmmyyyy)
                    SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ExpiredDate, ClsDate.GetDateTimeReturnStringFormat(dExpDate, ClsDate.Enum_DateFormatTypes._ddmmyyyy))
                Else

                End If
                'SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ExpiredDate, dExpDate)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FolderPath, oParamsD.GetParameter("@FolderPath").Value)

                oParamsD = Nothing
                oTxSql = Nothing
            End If


        Catch ex As Exception
            'GeneralException.WriteEventErrors("UpdateQuotationDataAfter_Connect-" & ex.Message, GeneralException.e_LogTitle.UPDATEDATA.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateQuotationDataAfter_Connect", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        End Try
    End Sub

    Public Shared Function ConnectQuotation(TemporarilyQuotationID As String) As String
        Try

            Dim QuotationNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)
            Dim BranchCode As String = (StateManager.GetValue(StateManager.Keys.s_BranchCode, True)) ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)
            Dim loggedEmail As String = StateManager.GetValue(StateManager.Keys.s_loggedEmail, True)
            Dim CustomerNumber As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
            Dim CustomerName As String = StateManager.GetValue(StateManager.Keys.s_CustomerName, True)
            If Not IsNumeric(CustomerNumber) Then CustomerNumber = 0
            Dim CustomerAddress1 As String = StateManager.GetValue(StateManager.Keys.s_CustomerAddress, True)
            If CustomerAddress1 Is Nothing Then CustomerAddress1 = ""

            Dim paymentTerms As String = StateManager.GetValue(StateManager.Keys.s_paymentTerms, True)
            If paymentTerms Is Nothing Then paymentTerms = ""

            Dim shipmethod As String = StateManager.GetValue(StateManager.Keys.s_shipmethod, True)
            If shipmethod Is Nothing Then shipmethod = ""


            Dim ssalesperson As String = StateManager.GetValue(StateManager.Keys.s_salesperson, True)
            If ssalesperson Is Nothing Then ssalesperson = ""
            Dim ssalespersonEmail As String = StateManager.GetValue(StateManager.Keys.s_salespersonEmail, True)
            If ssalespersonEmail Is Nothing Then ssalespersonEmail = ""
            Dim sdeskUser As String = StateManager.GetValue(StateManager.Keys.s_deskUser, True)
            If sdeskUser Is Nothing Then sdeskUser = ""
            Dim sdeskUserEmail As String = StateManager.GetValue(StateManager.Keys.s_deskUserEmail, True)
            If sdeskUserEmail Is Nothing Then sdeskUserEmail = ""
            Dim stechnicalPerson As String = StateManager.GetValue(StateManager.Keys.s_technicalPerson, True)
            If stechnicalPerson Is Nothing Then stechnicalPerson = ""
            Dim stechnicalPersonEmail As String = StateManager.GetValue(StateManager.Keys.s_technicalPersonEmail, True)
            If stechnicalPersonEmail Is Nothing Then stechnicalPersonEmail = ""

            Dim sCustomerType As String = StateManager.GetValue(StateManager.Keys.s_CustomerType, True)
            If sCustomerType Is Nothing Then sCustomerType = ""
            'Dim BranchNumber As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchNumber, False)

            Dim oTxSql As SqlDal
            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

            Dim oParamsD As New SqlParams()
            oParamsD.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNo))
            oParamsD.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            'oParamsD.Add(New SqlParam("@BranchNumber", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, BranchNumber, 200))
            oParamsD.Add(New SqlParam("@NEWQuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdOutput, 0))
            oParamsD.Add(New SqlParam("@loggedEmail", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, loggedEmail))
            oParamsD.Add(New SqlParam("@TemporarilyQuotationID", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, TemporarilyQuotationID))
            oParamsD.Add(New SqlParam("@CustomerNumber", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, CustomerNumber))
            oParamsD.Add(New SqlParam("@CustomerName", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, CustomerName))
            oParamsD.Add(New SqlParam("@CustomerAddress1", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, CustomerAddress1))
            oParamsD.Add(New SqlParam("@paymentTerms", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, paymentTerms))
            oParamsD.Add(New SqlParam("@shipmethod", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, shipmethod))
            oParamsD.Add(New SqlParam("@CustomerType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sCustomerType))

            oParamsD.Add(New SqlParam("@salesperson", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, ssalesperson))
            oParamsD.Add(New SqlParam("@salespersonEmail", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, ssalespersonEmail))
            oParamsD.Add(New SqlParam("@deskUser", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, sdeskUser))
            oParamsD.Add(New SqlParam("@deskUserEmail", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, sdeskUserEmail))
            oParamsD.Add(New SqlParam("@technicalPerson", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, stechnicalPerson))
            oParamsD.Add(New SqlParam("@technicalPersonEmail", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, stechnicalPersonEmail))


            oTxSql.ExecuteSP("USP_ConnectQuotation", oParamsD)
            Dim s As String = oParamsD.GetParameter("@NEWQuotationNum").Value
            oParamsD = Nothing
            oTxSql = Nothing
            Return s
        Catch ex As Exception
            'GeneralException.WriteEventErrors("ConnectQuotation-" & ex.Message, GeneralException.e_LogTitle.UPDATEDATA.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "ConnectQuotation", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Return ""
        End Try

    End Function



    Private Sub SetDeleviryValidTime(dt_FamilyProperties As DataTable)
        Try
            If dt_FamilyProperties Is Nothing Then
                dt_FamilyProperties = clsPrice.GetFamilyProperties(CInt(clsQuatation.ACTIVE_ModelNumber))
            End If

            Price.GetDelivery_ValidTime(dt_FamilyProperties)
        Catch ex As Exception
            'GeneralException.WriteEventErrors("SetDeleviryValidTime-" & ex.Message, GeneralException.e_LogTitle.UPDATEDATA.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "SetDeleviryValidTime", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        End Try
    End Sub

    Public Sub UpdateQuotation(ConfigORModification As Integer, ByVal QuotationNo As Integer, imgPath As String, TemporarilyQuotation As Boolean, lImage As Image, rImage As Image)

        Dim oTxSql As New SqlDal
        oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

        Try

            SetDeleviryValidTime(CType(Nothing, DataTable))
            Dim dt_FamilyProperties As DataTable = clsPrice.GetFamilyProperties(CInt(clsQuatation.ACTIVE_ModelNumber))
            FormulaResult.GetDescriptionFormula(dt_FamilyProperties)


            Dim oParams As New SqlParams()
            Dim dt_m As DataTable
            If ConfigORModification = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                dt_m = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelModification, "")
            Else
                dt_m = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._ModelConfiguration, "")
            End If

            Dim TemporarilyBranchCode As String = ""


            Try
                If StateManager.GetValue(StateManager.Keys.s_TemporarilyBranchCode, True).ToString.Trim <> "" Then
                    TemporarilyBranchCode = StateManager.GetValue(StateManager.Keys.s_TemporarilyBranchCode, True).ToString.Trim
                End If


            Catch ex As Exception

            End Try


            Dim ShowQuotationPrice As String = "0"
            Try
                If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ShowQutPrice, False).ToString Is Nothing Then
                    ShowQuotationPrice = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ShowQutPrice, False)
                End If
            Catch ex As Exception

            End Try

            Dim Submitted As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Submitted, False)

            Dim ModelNum As String = dt_m.Rows(0)("ModelNum").ToString
            Dim ModelName As String = dt_m.Rows(0)("ModelDes").ToString

            Dim DrawingMethod As String = dt_m.Rows(0)("DrawingMethod").ToString()
            Dim BranchCode As String
            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode) = "" Then
                BranchCode = StateManager.GetValue(StateManager.Keys.s_BranchCode)
            Else
                BranchCode = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode)
            End If
            Dim QuotationNum As Integer = QuotationNo

            Dim SemiToolDescription As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.SemiToolDescription)




            Dim Delivery_Weeks As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Delivery_Weeks)
            Dim DeliveryRad_Weeks As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.DeliveryRad_Weeks)
            Dim ValidTime_Weeks As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ValidTime_Weeks)
            Dim MaterialsID As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.MaterialsID)

            Dim CatalogNum As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber)
            Dim NET_STNCustomerPrice As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerPrice)

            Dim NET_STNCustomerCurrency As String = StateManager.GetValue(StateManager.Keys.s_STNCustomerCurrency, True)

            If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerCurrency) Is Nothing Then
                If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerCurrency) <> "" Then
                    NET_STNCustomerCurrency = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.NET_STNCustomerCurrency)
                End If
            End If
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.NET_STNCustomerCurrency, NET_STNCustomerCurrency)
            Dim TFR_STNBranchPrice As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TFR_STNBranchPrice)
            Dim TFR_STNBranchCurrency As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TFR_STNBranchCurrency)

            If NET_STNCustomerPrice = "" Then NET_STNCustomerPrice = 0
            If TFR_STNBranchPrice = "" Then TFR_STNBranchPrice = 0
            oParams.Add(New SqlParam("@NET_STNCustomerPrice", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, NET_STNCustomerPrice))
            oParams.Add(New SqlParam("@NET_STNCustomerCurrency", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, NET_STNCustomerCurrency))
            oParams.Add(New SqlParam("@TFR_STNBranchPrice", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, TFR_STNBranchPrice))
            oParams.Add(New SqlParam("@TFR_STNBranchCurrency", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, TFR_STNBranchCurrency))

            Dim BSONFolder = "" 'SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._BSONfolder, False)
            Dim DrawingName = 0 ' SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._DrawingName, False)

            oParams.Add(New SqlParam("@BSONFolder", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, BSONFolder))
            oParams.Add(New SqlParam("@DrawingName", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, DrawingName))

            Dim StandartTransferPrice As Decimal

            Dim TFRCurrency As String = ""
            Dim RateTFR_USD As Decimal = 0
            Dim RateMKT_USD As Decimal = 0

            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.RateTFR_USD) <> "" Then
                RateTFR_USD = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.RateTFR_USD)
            End If
            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.RateMKT_USD) <> "" Then
                RateMKT_USD = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.RateMKT_USD)
            End If

            Dim RateTFR_MKT As Decimal = 0
            Dim RateTFR_EUR As Decimal = 0
            Dim RateCUST_BASEDON As Decimal = 0
            Dim RateEUR_USD As Decimal = 0
            Dim RateTFR_WC As Decimal = 0
            Dim RateTFR_ModelCur As Decimal = 0
            Dim RateWC_USD As Decimal = 0
            Dim RateTFR_VENDOR As Decimal = 0

            Dim FamilyGP As Decimal = 0
            If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FAMILYGP, False) Is Nothing Then
                If IsNumeric(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FAMILYGP, False)) Then
                    FamilyGP = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FAMILYGP, False)
                End If
            End If
            Dim SUBGP As Decimal = 0
            If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.SUBGP, False) Is Nothing Then
                If IsNumeric(SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.SUBGP, False)) Then
                    SUBGP = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.SUBGP, False)
                End If
            End If

            Dim SelectedCurrency As String = ""
            Dim QuotationCurrency As String = ""
            Dim RemarksSystem As String = ""
            Dim RemarksExternal As String = ""
            Dim RemarksInternal As String = ""

            Dim RemarksInternalDirection As String = ""
            If RemarksInternalDirection = "" Then RemarksInternalDirection = "ltr"

            Dim CustomerRequest As String = ""
            Dim ReportLanguageID As Integer = 1

            Dim ContactPerson As String = ""
            Dim ContactPersonPhone As String = ""
            Dim ContactPersonMail As String = ""
            Dim DrawingInLineAvailable As Boolean
            Dim CustomerAddress1 As String = StateManager.GetValue(StateManager.Keys.s_CustomerAddress, True)

            Dim loggedEmail As String = ""
            Try
                loggedEmail = StateManager.GetValue(StateManager.Keys.s_loggedEmail, True)
            Catch ex As Exception

            End Try
            Dim QuotPicPath As String = ""

            Dim UserSpecialName As String = ""

            Dim QtyPrcFormulaForReport As String = ""
            Dim LTDPriceGP As String = 0
            Dim WC_FACTOR As String = ""
            If WC_FACTOR = "" Then WC_FACTOR = 0
            Dim TFRPriceNumber As String = ""
            Dim MKTPriceNumber As String = ""

            If Not IsNumeric(TFRPriceNumber) Then TFRPriceNumber = 0
            If Not IsNumeric(MKTPriceNumber) Then MKTPriceNumber = 0
            If TFRPriceNumber = "" Then TFRPriceNumber = "0"
            If MKTPriceNumber = "" Then MKTPriceNumber = "0"

            Dim stnGP_TC_Remark As String = ""
            If InStr(UserSpecialName, " ") > 0 Then
                UserSpecialName = Left(UserSpecialName, InStr(UserSpecialName, " ") - 1) & "." & Mid(UserSpecialName, InStr(UserSpecialName, " ") + 1, 1)
            End If

            Dim DiscountFromSTN As Integer = 0
            Dim Customer_GP1 As String = ""
            Dim Customer_GP2 As String = ""
            Dim Customer_GP3 As String = ""
            Dim valid_Time As String = ""
            Dim QuotationStatus As String = ""
            Dim UserFullName As String = ""


            oParams.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum))
            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNum))
            oParams.Add(New SqlParam("@SemiToolDescription", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, SemiToolDescription, 50))
            oParams.Add(New SqlParam("@CatalogNum", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, CatalogNum, 50))
            oParams.Add(New SqlParam("@StandartTransferPrice", SqlParam.ParamType.ptFloat, SqlParam.ParamDirection.pdInput, StandartTransferPrice))
            oParams.Add(New SqlParam("@TFRCurrency", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, TFRCurrency, 50))
            oParams.Add(New SqlParam("@RateTFR_USD", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, RateTFR_USD))
            oParams.Add(New SqlParam("@RateMKT_USD", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, RateMKT_USD))
            oParams.Add(New SqlParam("@RateTFR_MKT", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, RateTFR_MKT))
            oParams.Add(New SqlParam("@RateTFR_EUR", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, RateTFR_EUR))
            oParams.Add(New SqlParam("@RateCUST_BASEDON", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, RateCUST_BASEDON))
            oParams.Add(New SqlParam("@RateEUR_USD", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, RateEUR_USD))
            oParams.Add(New SqlParam("@RateTFR_ModelCur", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, RateTFR_ModelCur))
            oParams.Add(New SqlParam("@RateTFR_WC", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, RateTFR_WC))
            oParams.Add(New SqlParam("@RateWC_USD", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, RateWC_USD))
            oParams.Add(New SqlParam("@RateTFR_VENDOR", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, RateTFR_VENDOR))

            oParams.Add(New SqlParam("@SelectedCurCode", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, SelectedCurrency, 5))
            oParams.Add(New SqlParam("@QuotationCurrency", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, QuotationCurrency, 3))
            oParams.Add(New SqlParam("@RemarksSystem", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, RemarksSystem, 2000))
            oParams.Add(New SqlParam("@CustomerRequest", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, CustomerRequest, 30))
            oParams.Add(New SqlParam("@ContactPerson", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, ContactPerson, 50))
            oParams.Add(New SqlParam("@ContactPersonPhone", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, ContactPersonPhone, 30))
            oParams.Add(New SqlParam("@ContactPersonMail", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, ContactPersonMail, 50))
            oParams.Add(New SqlParam("@DrawingNum", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdOutput, "", 11))
            oParams.Add(New SqlParam("@QuotPicPath", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, QuotPicPath, 50))
            oParams.Add(New SqlParam("@Archive", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, False))
            oParams.Add(New SqlParam("@LTDPriceGP", SqlParam.ParamType.ptFloat, SqlParam.ParamDirection.pdInput, LTDPriceGP))
            oParams.Add(New SqlParam("@WC_FACTOR", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, WC_FACTOR))

            oParams.Add(New SqlParam("@DrawingInLineAvailable", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, DrawingInLineAvailable))
            oParams.Add(New SqlParam("@SelectedLanguage", SqlParam.ParamType.ptSmallInt, SqlParam.ParamDirection.pdInput, ReportLanguageID))
            oParams.Add(New SqlParam("@QtyPrcFormulaForReport", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, QtyPrcFormulaForReport, 100))
            oParams.Add(New SqlParam("@CustomerAddress1", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, CustomerAddress1, 100))

            oParams.Add(New SqlParam("@TFRPriceNumber", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, TFRPriceNumber))
            oParams.Add(New SqlParam("@MKTPriceNumber", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, MKTPriceNumber))
            oParams.Add(New SqlParam("@stnGP_TC_Remark", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, stnGP_TC_Remark, 100))

            oParams.Add(New SqlParam("@DiscountFromSTN", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, DiscountFromSTN))
            oParams.Add(New SqlParam("@Customer_GP1", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, Customer_GP1, 50))
            oParams.Add(New SqlParam("@Customer_GP2", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, Customer_GP2, 50))
            oParams.Add(New SqlParam("@Customer_GP3", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, Customer_GP3, 50))
            oParams.Add(New SqlParam("@valid_Time", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, valid_Time, 20))
            oParams.Add(New SqlParam("@FinalQuatation", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, True))
            oParams.Add(New SqlParam("@UserName", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, UserFullName, 50))
            oParams.Add(New SqlParam("@itemNumber", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, CatalogNum, 20))


            Dim spaymentTerms As String = StateManager.GetValue(StateManager.Keys.s_paymentTerms, True).ToString.Trim
            Dim sshipmethod As String = StateManager.GetValue(StateManager.Keys.s_shipmethod, True).ToString.Trim
            Dim sCustomerType As String = StateManager.GetValue(StateManager.Keys.s_CustomerType, True).ToString.Trim
            oParams.Add(New SqlParam("@paymentTerms", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, spaymentTerms, 100))

            Dim ssalesperson As String = StateManager.GetValue(StateManager.Keys.s_salesperson, True).ToString.Trim
            If ssalesperson Is Nothing Then ssalesperson = ""
            Dim ssalespersonEmail As String = StateManager.GetValue(StateManager.Keys.s_salespersonEmail, True).ToString.Trim
            If ssalespersonEmail Is Nothing Then ssalespersonEmail = ""
            Dim sdeskUser As String = StateManager.GetValue(StateManager.Keys.s_deskUser, True).ToString.Trim
            If sdeskUser Is Nothing Then sdeskUser = ""
            Dim sdeskUserEmail As String = StateManager.GetValue(StateManager.Keys.s_deskUserEmail, True).ToString.Trim
            If sdeskUserEmail Is Nothing Then sdeskUserEmail = ""
            Dim stechnicalPerson As String = StateManager.GetValue(StateManager.Keys.s_technicalPerson, True).ToString.Trim
            If stechnicalPerson Is Nothing Then stechnicalPerson = ""
            Dim stechnicalPersonEmail As String = StateManager.GetValue(StateManager.Keys.s_technicalPersonEmail, True).ToString.Trim
            If stechnicalPersonEmail Is Nothing Then stechnicalPersonEmail = ""
            oParams.Add(New SqlParam("@salesperson", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, ssalesperson))
            oParams.Add(New SqlParam("@salespersonEmail", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, ssalespersonEmail))
            oParams.Add(New SqlParam("@deskUser", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, sdeskUser))
            oParams.Add(New SqlParam("@deskUserEmail", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, sdeskUserEmail))
            oParams.Add(New SqlParam("@technicalPerson", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, stechnicalPerson))
            oParams.Add(New SqlParam("@technicalPersonEmail", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, stechnicalPersonEmail))

            oParams.Add(New SqlParam("@shipmethod", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sshipmethod, 100))
            oParams.Add(New SqlParam("@CustomerType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sCustomerType, 100))


            'Dim sCustomerOrderNumber As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.PricesCustomerOrderNumber)
            'Dim sCustomerItemNumber As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.PricesCustomerItemNumber)
            'Dim sCustomerAdditionalReq As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.PricesCustomerAdditionalReq)

            'oParams.Add(New SqlParam("@CustomerOrderNumber", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sCustomerOrderNumber))
            'oParams.Add(New SqlParam("@CustomerItemNumber", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, sCustomerItemNumber))
            'oParams.Add(New SqlParam("@CustomerAdditionalReq", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, sCustomerAdditionalReq))


            Try

                Dim bitmapBytesL As Byte() = Images.GetImageReturnByteImageStreem(lImage)
                If Not bitmapBytesL Is Nothing Then
                    oParams.Add(New SqlParam("@mainPicL", SqlParam.ParamType.ptImage, SqlParam.ParamDirection.pdInput, bitmapBytesL, 0))
                Else
                    oParams.Add(New SqlParam("@mainPicL", SqlParam.ParamType.ptImage, SqlParam.ParamDirection.pdInput, DBNull.Value, 0))
                End If
            Catch ex As Exception
                oParams.Add(New SqlParam("@mainPicL", SqlParam.ParamType.ptImage, SqlParam.ParamDirection.pdInput, DBNull.Value, 0))
            End Try

            Try
                Dim bitmapBytesr As Byte() = Images.GetImageReturnByteImageStreem(rImage)
                If Not bitmapBytesr Is Nothing Then
                    oParams.Add(New SqlParam("@mainPicR", SqlParam.ParamType.ptImage, SqlParam.ParamDirection.pdInput, bitmapBytesr, 0))
                Else
                    oParams.Add(New SqlParam("@mainPicR", SqlParam.ParamType.ptImage, SqlParam.ParamDirection.pdInput, DBNull.Value, 0))
                End If
            Catch ex As Exception
                oParams.Add(New SqlParam("@mainPicR", SqlParam.ParamType.ptImage, SqlParam.ParamDirection.pdInput, DBNull.Value, 0))
            End Try

            If IsNumeric(Delivery_Weeks.ToString) Then
                oParams.Add(New SqlParam("@Delivery_Weeks", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, Delivery_Weeks))
            End If
            If IsNumeric(DeliveryRad_Weeks.ToString) Then
                oParams.Add(New SqlParam("@DeliveryRad_Weeks", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, DeliveryRad_Weeks))
            End If
            If IsNumeric(ValidTime_Weeks.ToString) Then
                oParams.Add(New SqlParam("@ValidTime_Weeks", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ValidTime_Weeks))
            End If
            If IsNumeric(MaterialsID.ToString) Then
                oParams.Add(New SqlParam("@MaterialsID", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, MaterialsID))
            End If


            Dim dExpDate As Date = Now
            Try
                If TemporarilyQuotation = True Then
                    dExpDate = Now.AddDays(CDbl(ConfigurationManager.AppSettings("TemporaryExpiryDateDays")))
                Else
                    Dim s As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.ValidTime_Weeks, False)
                    If IsNumeric(s) Then
                        dExpDate = Now.AddDays(CInt(s) * 7)
                    End If
                End If


            Catch ex As Exception

            End Try


            oParams.Add(New SqlParam("@ExpiredDate", SqlParam.ParamType.ptDateTime, SqlParam.ParamDirection.pdInput, dExpDate))

            oParams.Add(New SqlParam("@TemporarilyQuotation", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, TemporarilyQuotation))
            oParams.Add(New SqlParam("@FamilyGP", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, FamilyGP))
            oParams.Add(New SqlParam("@SUBGP", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, SUBGP))
            oParams.Add(New SqlParam("@loggedEmail", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, loggedEmail, 100))

            oParams.Add(New SqlParam("@TemporarilyBranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, TemporarilyBranchCode, 2))

            oParams.Add(New SqlParam("@ShowQutPrice", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, ShowQuotationPrice, 1))

            oParams.Add(New SqlParam("@Submitted", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Submitted, 1))

            oTxSql.ExecuteSP("USP_UpdateQuotation", oParams)

            If IsDate(dExpDate) Then

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.ExpiredDate, ClsDate.GetDateTimeReturnStringFormat(dExpDate, ClsDate.Enum_DateFormatTypes._ddmmyyyy))
            End If


            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagPricesChanged, False)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagParametersChanged, False)


            Dim sUdate As String = ClsDate.GetDateTimeReturnStringFormat(Now, ClsDate.Enum_DateFormatTypes._ddmmyyyy)

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.LastUpdateDate, sUdate)


            Dim PrepareBitMap As String = "False"
            Dim dt_Parameters As DataTable

            If ConfigORModification = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                dt_Parameters = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListModification, "")
            Else
                dt_Parameters = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamListConfiguration, "")
            End If

            If Not dt_Parameters Is Nothing Then
                UpdateQuotationParams(dt_Parameters, ModelNum, QuotationNo, BranchCode, oTxSql)
            End If

            UpdateQuotationPrices()

            UpdateQuotationFactorsQty()

            UpdateQuotationFactorsQuotationFactors()

            UpdateQuotatioListModelParametersCode()


            UpdateQuotationFormula()

        Catch ex As Exception

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateQuotation", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        Finally

        End Try
    End Sub


    Public Shared Sub UpdateConstants()
        Try
            Dim Consts As DataTable
            Consts = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Consts, "")
            Dim QuotationNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)

            Dim BranchCode As String
            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode) = "" Then
                BranchCode = StateManager.GetValue(StateManager.Keys.s_BranchCode)
            Else
                BranchCode = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode)
            End If
            Dim oTxSql As New SqlDal
            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)
            'save constants to DB
            Dim sqlBlock As String
            sqlBlock = "delete from lcl_TQuotationListConstants where QuotationNum = " & QuotationNo.ToString & " and  BranchCode = '" & BranchCode & "'" & vbCrLf
            oTxSql.ExecuteSql(sqlBlock)
            oTxSql = Nothing

            sqlBlock = ""
            If Not Consts Is Nothing Then
                For Each drConsts As DataRow In Consts.Rows
                    sqlBlock &= " insert into lcl_TQuotationListConstants (QuotationNum, BranchCode, ConstName, ConstValue) values (" & QuotationNo.ToString & ",'" & BranchCode & "','" & drConsts("ConstName").ToString & "','" & drConsts("ConstValue").ToString & "') " & vbCrLf
                Next

            End If

            Dim oTxSqlA As New SqlDal
            oTxSqlA = New SqlDal(clsBranch.GetSiteConnectionString)
            If sqlBlock <> "" Then
                oTxSqlA.ExecuteSql(sqlBlock)
                oTxSqlA = Nothing
            End If
            oTxSqlA = Nothing

        Catch ex As Exception

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateConstants", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        End Try
    End Sub

    Public Shared Sub UpdateQuotationPrices()


        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagPricesChanged, False)

        Try

            Dim oTxSql As New SqlDal
            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams

            Dim dt_PricesC As DataTable = Nothing
            dt_PricesC = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.dt_Price, "")


            Dim QuotationNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)

            Dim BranchCode As String
            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode) = "" Then
                BranchCode = StateManager.GetValue(StateManager.Keys.s_BranchCode)
            Else
                BranchCode = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode)
            End If

            Dim oParamsS As New SqlParams
            oParamsS.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNo))
            oParamsS.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oTxSql.ExecuteSP("USP_DeleteQuotationListPrices", oParamsS)
            oParamsS = Nothing

            If Not dt_PricesC Is Nothing AndAlso dt_PricesC.Columns.Count > 0 Then

                Dim iR As Int16 = 0
                For Each row As DataRow In dt_PricesC.Rows

                    If Not String.IsNullOrEmpty(row("QTY").ToString) AndAlso row("QTY").ToString <> "0" Then
                        Dim QTY As Integer = row("QTY")

                        Dim NetPrice As Decimal = 0
                        If Not String.IsNullOrEmpty(row("NetPrice").ToString) Then NetPrice = row("NetPrice")
                        Dim Total As Decimal = 0
                        If Not String.IsNullOrEmpty(row("Total").ToString) Then Total = row("Total")
                        Dim DeliveryWeeks As Integer = 0
                        If Not String.IsNullOrEmpty(row("DeliveryWeeks").ToString) Then DeliveryWeeks = row("DeliveryWeeks")
                        If DeliveryWeeks = 0 Then DeliveryWeeks = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Delivery_Weeks, False)
                        Dim CostPrice As Decimal = 0
                        If Not String.IsNullOrEmpty(row("CostPrice").ToString) Then CostPrice = row("CostPrice")

                        Dim GP As Decimal = 0
                        If Not String.IsNullOrEmpty(row("GP").ToString) Then GP = row("GP")

                        Dim TFRPrice As Decimal = 0
                        If Not String.IsNullOrEmpty(row("TFRPrice").ToString) Then TFRPrice = row("TFRPrice")

                        Dim QTYFct As Decimal = 0
                        If Not String.IsNullOrEmpty(row("QTYFct").ToString) Then QTYFct = row("QTYFct")

                        Dim OrderedQuantity As Boolean = 0
                        If Not String.IsNullOrEmpty(row("OrderedQuantity").ToString) Then OrderedQuantity = row("OrderedQuantity")

                        oParams = New SqlParams()
                        oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNo))
                        oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
                        oParams.Add(New SqlParam("@QTY", SqlParam.ParamType.ptSmallInt, SqlParam.ParamDirection.pdInput, QTY))
                        oParams.Add(New SqlParam("@NetPrice", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, NetPrice))
                        oParams.Add(New SqlParam("@Total", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, Total))
                        oParams.Add(New SqlParam("@DeliveryWeeks", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, DeliveryWeeks))
                        oParams.Add(New SqlParam("@CostPrice", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, CostPrice))
                        oParams.Add(New SqlParam("@GP", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, GP))
                        oParams.Add(New SqlParam("@TFRPrice", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, TFRPrice))
                        oParams.Add(New SqlParam("@QTYFct", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, QTYFct))
                        If iR > 7 Then
                            oParams.Add(New SqlParam("@CustomQTY", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, True))
                        Else
                            oParams.Add(New SqlParam("@CustomQTY", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, False))
                        End If
                        oParams.Add(New SqlParam("@OrderedQuantity", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, OrderedQuantity))

                        oTxSql.ExecuteSP("USP_AddQuotationList_Prices", oParams)

                        End If
                        iR += 1
                Next

            End If

            oParams = Nothing
            oTxSql = Nothing

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateQuotationPrices", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        End Try

    End Sub
    Public Shared Sub UpdateQuotationPricesTransaction(LastUpdate As String, newUnoitPrice As Decimal, QTY As String, ByRef PriceRevision As Integer)

        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagPricesChanged, False)

        Try

            Dim oTxSql As New SqlDal
            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams

            Dim QuotationNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)

            Dim BranchCode As String
            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode) = "" Then
                BranchCode = StateManager.GetValue(StateManager.Keys.s_BranchCode)
            Else
                BranchCode = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode)
            End If

            oParams = New SqlParams()
            oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNo))
            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParams.Add(New SqlParam("@QTY", SqlParam.ParamType.ptSmallInt, SqlParam.ParamDirection.pdInput, QTY))
            oParams.Add(New SqlParam("@NetPrice", SqlParam.ParamType.ptDecimal, SqlParam.ParamDirection.pdInput, newUnoitPrice))
            oParams.Add(New SqlParam("@LastUpdate", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, LastUpdate))
            oParams.Add(New SqlParam("@PriceRevision", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInputOutput, PriceRevision))
            oTxSql.ExecuteSP("USP_AddQuotationList_PricesTransaction", oParams)

            PriceRevision = oParams.GetParameter("@PriceRevision").Value


            Dim sla As String = LastUpdate.ToString
            If IsNumeric(sla) AndAlso sla.ToString.Length = 8 Then
                sla = sla.ToString.Substring(6, 2) & "/" & sla.ToString.Substring(4, 2) & "/" & sla.ToString.Substring(0, 4)
            End If
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.LastUpdateDate, sla)

            oParams = Nothing
            oTxSql = Nothing

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateQuotationPrices", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
            Throw
        End Try

    End Sub
    Private Sub UpdateQuotationParams(ByVal dt_Parameters As DataTable, ByVal ModelNum As String, ByVal QuotationNo As Integer, ByVal BranchCode As String, ByVal oTxSql As SqlDal)
        Try

            Dim oParamsS As New SqlParams
            oParamsS.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNo))
            oParamsS.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oTxSql.ExecuteSP("USP_DeleteQuotationListParams", oParamsS)
            oParamsS = Nothing


            Dim PrepareBitMap As String = "False"
            PrepareBitMap = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.FlagParametersChanged).ToString

            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.FlagParametersChanged, False)


            Dim oParamsN As New SqlParams


            For Each row As DataRow In dt_Parameters.Rows
                oParamsN = New SqlParams()
                Dim TabIndex As Integer = row("TabIndex")
                Dim Label As String = row("Label").ToString
                Dim Measure As String = row("Measure").ToString
                Dim Formula As String = row("Formula").ToString
                Dim i As Integer
                Dim j As Integer
                i += 1
                j = TabIndex
                Dim VisiblePrint As Integer = 0
                If row("VisiblePrint") = "True" Then VisiblePrint = 1
                Dim Remark As String = row("Remark").ToString
                Dim LabelLeft As String = "0"
                If Not row("LabelLeft") Is DBNull.Value AndAlso row("LabelLeft").ToString <> "" Then LabelLeft = row("LabelLeft")
                Dim LabelTop As String = "0"
                If Not row("LabelTop") Is DBNull.Value AndAlso row("LabelTop").ToString <> "" Then LabelTop = row("LabelTop")
                Dim VisibleGrid As Integer = 0
                If row("VisibleGrid") = "True" Then VisibleGrid = 1

                oParamsN.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNo))
                oParamsN.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
                oParamsN.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum))
                oParamsN.Add(New SqlParam("@TabIndex", SqlParam.ParamType.ptSmallInt, SqlParam.ParamDirection.pdInput, TabIndex))
                oParamsN.Add(New SqlParam("@Label", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Label, 50))
                oParamsN.Add(New SqlParam("@Measure", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Measure, 255))
                oParamsN.Add(New SqlParam("@Formula", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Formula, 255))
                oParamsN.Add(New SqlParam("@VisiblePrint", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, VisiblePrint))
                oParamsN.Add(New SqlParam("@Remark", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Remark, 255))
                oParamsN.Add(New SqlParam("@LabelLeft", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, LabelLeft, 250))
                oParamsN.Add(New SqlParam("@LabelTop", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, LabelTop, 250))
                oParamsN.Add(New SqlParam("@VisibleGrid", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, VisibleGrid))
                oParamsN.Add(New SqlParam("@Order", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, row("Order"), 10))
                oParamsN.Add(New SqlParam("@PictSelect", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, row("PictSelect"), 100))
                oParamsN.Add(New SqlParam("@PictHelp", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, row("PictHelp"), 100))
                oParamsN.Add(New SqlParam("@PrevParam", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, row("PrevParam")))
                oParamsN.Add(New SqlParam("@LabelNum", SqlParam.ParamType.ptSmallInt, SqlParam.ParamDirection.pdInput, row("LabelNum")))
                oParamsN.Add(New SqlParam("@Priority", SqlParam.ParamType.ptSmallInt, SqlParam.ParamDirection.pdInput, row("Priority")))
                oParamsN.Add(New SqlParam("@Visible", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, row("Visible")))
                oParamsN.Add(New SqlParam("@FormatFormula", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, row("FormatFormula"), 50))
                oParamsN.Add(New SqlParam("@DrawLabel", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, row("DrawLabel"), 50))
                oParamsN.Add(New SqlParam("@SaveLabel", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, row("SaveLabel")))
                oParamsN.Add(New SqlParam("@CostPar", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, row("CostPar")))
                oParamsN.Add(New SqlParam("@HyperLink", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, row("HyperLink"), 255))
                oParamsN.Add(New SqlParam("@ParamType", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, row("ParamType"), 10))
                oParamsN.Add(New SqlParam("@FindOperator", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, row("FindOperator"), 2))
                oParamsN.Add(New SqlParam("@NewLeft", SqlParam.ParamType.ptSmallInt, SqlParam.ParamDirection.pdInput, row("NewLeft")))
                oParamsN.Add(New SqlParam("@NewTop", SqlParam.ParamType.ptSmallInt, SqlParam.ParamDirection.pdInput, row("NewTop")))
                oParamsN.Add(New SqlParam("@ByParam", SqlParam.ParamType.ptSmallInt, SqlParam.ParamDirection.pdInput, row("ByParam")))
                oParamsN.Add(New SqlParam("@ByVal", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, row("ByVal").ToString, 50))
                oParamsN.Add(New SqlParam("@RemarkParam", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, row("RemarkParam").ToString, 200))
                oParamsN.Add(New SqlParam("@RemByParam", SqlParam.ParamType.ptSmallInt, SqlParam.ParamDirection.pdInput, row("RemByParam")))
                oParamsN.Add(New SqlParam("@RemByVal", SqlParam.ParamType.ptSmallInt, SqlParam.ParamDirection.pdInput, row("RemByVal")))
                oParamsN.Add(New SqlParam("@OptionalLabelNum", SqlParam.ParamType.ptSmallInt, SqlParam.ParamDirection.pdInput, row("OptionalLabelNum")))
                oParamsN.Add(New SqlParam("@DrawingField", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, row("DrawingField").ToString, 50))
                oParamsN.Add(New SqlParam("@RunCode", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, row("RunCode").ToString, 50))
                oParamsN.Add(New SqlParam("@Active", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, row("Active"), 10))
                oParamsN.Add(New SqlParam("@MainParameters", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, row("MainParameters")))
                oParamsN.Add(New SqlParam("@GPNUM_REG", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, row("GPNUM_REG")))
                oParamsN.Add(New SqlParam("@GPNUM_ISO", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, row("GPNUM_ISO")))
                oParamsN.Add(New SqlParam("@CostName", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, row("CostName").ToString, 50))
                oParamsN.Add(New SqlParam("@VisibleDes", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, row("VisibleDes")))
                oParamsN.Add(New SqlParam("@DescFormat", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, row("MainParameters").ToString, 100))
                oParamsN.Add(New SqlParam("@StringValue", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, row("StringValue").ToString, 50))
                oParamsN.Add(New SqlParam("@DescValue", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, row("DescValue").ToString, 250))

                oParamsN.Add(New SqlParam("@DescConditions", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, row("DescConditions").ToString, 250))
                oParamsN.Add(New SqlParam("@SelectModelImage", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, row("SelectModelImage").ToString, 150))
                oParamsN.Add(New SqlParam("@PropertyImageRequirement", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, row("PropertyImageRequirement")))
                oParamsN.Add(New SqlParam("@PropertyImage", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, row("PropertyImage").ToString, 150))
                oParamsN.Add(New SqlParam("@DrawingConversion", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, row("DrawingConversion")))
                oParamsN.Add(New SqlParam("@GPNUM_ISO_Alternate", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, row("GPNUM_ISO_Alternate")))
                oParamsN.Add(New SqlParam("@VisibleTable", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, row("VisibleTable")))


                oTxSql.ExecuteSP("USP_AddQuotationListParams", oParamsN)
            Next

            oParamsN = Nothing
        Catch ex As Exception
            ' GeneralException.WriteEventErrors("UpdateQuotationParams-" & ex.Message, GeneralException.e_LogTitle.UPDATEDATA.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateQuotationParams", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        End Try
    End Sub



    Public Shared Sub DeleteQuotation(QuotationNum As Integer, BranchCode As String, FolderPath As String, AllFiles As Boolean)
        Try

            If IsNumeric(QuotationNum) = True AndAlso BranchCode <> "" Then



                Dim oTxSql As SqlDal
                oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

                Dim oParamsD As New SqlParams()
                oParamsD.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNum))
                oParamsD.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))

                oTxSql.ExecuteSP("USP_DeleteQuotation", oParamsD)

                oParamsD = Nothing
                oTxSql = Nothing



            End If
        Catch ex As Exception

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "DeleteQuotation", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        End Try
    End Sub

    Public Shared Sub RenewQuotation(QuotationNum As Integer, BranchCode As String)
        Try

            If IsNumeric(QuotationNum) = True AndAlso BranchCode <> "" Then



                Dim oTxSql As SqlDal
                oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

                Dim oParamsD As New SqlParams()
                oParamsD.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNum))
                oParamsD.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))

                oTxSql.ExecuteSP("USP_RenewQuotation", oParamsD)

                oParamsD = Nothing
                oTxSql = Nothing
            End If
        Catch ex As Exception

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "RenewQuotation", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        End Try
    End Sub
    Public Shared Sub DuplicateQuotation(QuotationNum As Integer, BranchCode As String, ByRef QutNewNo As Integer)
        QutNewNo = 0
        Try

            If IsNumeric(QuotationNum) = True AndAlso BranchCode <> "" Then



                Dim oTxSql As SqlDal
                oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

                Dim oParamsD As New SqlParams()
                oParamsD.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNum))
                oParamsD.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
                oParamsD.Add(New SqlParam("@NewQutNo", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInputOutput, 0))

                oTxSql.ExecuteSP("USP_DuplicateQuotation", oParamsD)

                QutNewNo = oParamsD.GetParameter("@NewQutNo").Value

                oParamsD = Nothing
                oTxSql = Nothing
            End If
        Catch ex As Exception

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "DuplicateQuotation", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            QutNewNo = 0
            Throw
        End Try
    End Sub

    Public Shared Sub UpdateQuotatioListModelParametersCode()
        Try
            Dim oTxSql As New SqlDal
            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

            Dim QuotationNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)

            Dim BranchCode As String
            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode) = "" Then
                BranchCode = StateManager.GetValue(StateManager.Keys.s_BranchCode)
            Else
                BranchCode = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode)
            End If

            Dim ModelNum As String = clsQuatation.ACTIVE_ModelNumber

            Dim oParamsS As New SqlParams
            oParamsS.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNo))
            oParamsS.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oTxSql.ExecuteSP("USP_DeleteQuotatioListModelParametersCode", oParamsS)
            oParamsS = Nothing



            Dim sStart As String = clsQuatation.ACTIVE_OpenType
            Dim sop As String = ""
            If sStart = clsQuatation.Enum_QuotationOpenType.CONFIGURATOR Then
                sop = "Configuration"
            ElseIf sStart = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                sop = "Modification"
            End If

            oParamsS = Nothing

            Dim oParams As New SqlParams
            oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNo))
            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParams.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum))
            oParams.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, sop))

            oTxSql.ExecuteSP("USP_AddQuotatioListModelParametersCode", oParams)
            oParams = Nothing

            oTxSql = Nothing

            Dim dtTQuotatioListModelParametersCode As New DataTable
            dtTQuotatioListModelParametersCode = clsQuatation.GetQuotatioListModelParametersCode(BranchCode, QuotationNo)
            SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtTQuotatioListModelParametersCode, dtTQuotatioListModelParametersCode)

        Catch ex As Exception

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateQuotatioListModelParametersCode", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        End Try

    End Sub

    Public Shared Sub UpdateQuotatioListTemporary(TemporarilyQuotation As Boolean)
        Try
            Dim oTxSql As New SqlDal
            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

            Dim QuotationNo As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationNumber, False)

            Dim BranchCode As String
            If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode) = "" Then
                BranchCode = StateManager.GetValue(StateManager.Keys.s_BranchCode)
            Else
                BranchCode = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode)
            End If

            Dim oParams As New SqlParams
            oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNo))
            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParams.Add(New SqlParam("@TemporarilyQuotation", SqlParam.ParamType.ptBit, SqlParam.ParamDirection.pdInput, TemporarilyQuotation))

            oTxSql.ExecuteSP("USP_UpdateQuotationTemporary", oParams)
            oParams = Nothing

            oTxSql = Nothing

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateQuotatioListTemporary", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        End Try

    End Sub


    Public Shared Function Get_HH_HD_HP(BranchCode As String, QuatationNumber As String, ByRef custno As String) As String
        Try
            Dim dtHH As DataTable
            Dim dtHD As DataTable
            Dim dtHP As DataTable
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()

            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParams.Add(New SqlParam("@QuotationNumber", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuatationNumber, 4))

            dtHH = oSql.ExecuteSPReturnDT("USP_GetQutData_HH_UploadToGAL", oParams)
            dtHD = oSql.ExecuteSPReturnDT("USP_GetQutData_HD_UploadToGAL", oParams)
            dtHP = oSql.ExecuteSPReturnDT("USP_GetQutData_HP_UploadToGAL", oParams)
            Dim HH_JSON As String = ""
            Dim HD_JSON As String = ""
            Dim HP_JSON As String = ""

            If Not dtHH Is Nothing AndAlso dtHH.Rows.Count > 0 AndAlso Not dtHD Is Nothing AndAlso dtHD.Rows.Count > 0 Then

                HH_JSON = DataTableToJSONWithStringBuilder(dtHH)
                HH_JSON = HH_JSON.Replace("[{", "{")
                HH_JSON = HH_JSON.Replace("""""}]", "")

                HD_JSON = DataTableToJSONWithStringBuilder(dtHD)
                HD_JSON = HD_JSON.Replace("""""}]", "")

                HP_JSON = DataTableToJSONWithStringBuilder(dtHP)
                If dtHP.Rows.Count > 0 Then
                    HP_JSON = HP_JSON.Replace("]", "]}]}")
                Else
                    HP_JSON = HP_JSON & "[]}]}"
                End If

                custno = dtHH.Rows(0).Item("CUSTOMER")
            End If

            Return HH_JSON & HD_JSON & HP_JSON
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateQuotatioListTemporary", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            Throw
        End Try
    End Function


    Public Shared Function UpdateQuotationGAL(ByVal QuatationNumber As Integer, AS400Number As String, BranchCode As String, StartFrom As String, ByRef sMsgN As String, ByRef NewQutNumber As String, ByRef CanUpdateGal As Boolean) As Boolean

        sMsgN = "Fnc UpdateQuotationGAL StartFrom : " & StartFrom & " ## "
        sMsgN &= " -QuatationNumber=" & QuatationNumber.ToString
        sMsgN &= " -AS400Number=" & AS400Number.ToString
        sMsgN &= " -BranchCode=" & BranchCode.ToString
        sMsgN &= " ##"
        Try
            CanUpdateGal = True
            If GAL.CheckIfCanUpdateGAL() = True Then
                Dim qutToUpdate As String = ""
                If AS400Number = "0" Or AS400Number = QuatationNumber Then
                    qutToUpdate = 0
                Else
                    qutToUpdate = AS400Number
                End If
                sMsgN &= " ##CanUpdateGAL ## qutToUpdate :" & qutToUpdate.ToString

                Dim custno As String = ""
                Dim HH_HD_HP As String = Get_HH_HD_HP(BranchCode, QuatationNumber, custno)

                sMsgN &= " ##HH_JSON : " & HH_HD_HP

                If qutToUpdate <> "0" Then
                    HH_HD_HP = HH_HD_HP.Substring(HH_HD_HP.IndexOf("[") + 1, HH_HD_HP.Length - HH_HD_HP.IndexOf("[") - 3)
                    sMsgN &= " ##SQL Data " & HH_HD_HP.ToString
                End If

                If HH_HD_HP <> "" Then

                    Dim PostDataForHH_D_HP As String = ""

                    Try
                        PostDataForHH_D_HP = GAL.GalPostData(BranchCode, HH_HD_HP, custno, qutToUpdate, False)
                    Catch ex As Exception
                        sMsgN &= " ## PostDataForHH_D_HP : Error msg : " & ex.Message
                        Return False
                    End Try

                    If PostDataForHH_D_HP Is Nothing OrElse PostDataForHH_D_HP.ToString.Trim = "" OrElse PostDataForHH_D_HP.ToString.Trim.ToUpper = "FALSE" OrElse PostDataForHH_D_HP.ToString.Trim.Length < 10 Then
                        Try
                            sMsgN &= " ## PostDataForHH_D_HP Val = " & PostDataForHH_D_HP.ToString.Trim
                        Catch ex As Exception

                        End Try

                        Return False
                    End If


                    NewQutNumber = PostDataForHH_D_HP
                    sMsgN &= " ##GAL.PostDataForHH_D_HP " & NewQutNumber.ToString

                    Dim sNum As DataTable = LocalizationManager.Convert_JsonToDataTable(NewQutNumber)
                    sMsgN &= " ##NewQutNumber " & NewQutNumber.ToString

                    If Not sNum Is Nothing AndAlso sNum.Rows.Count > 0 Then
                        NewQutNumber = sNum.Rows(0).Item("quotenumber").ToString
                    Else
                        Try
                            sMsgN &= " ## sNum Val = " & sNum.ToString.Trim
                        Catch ex As Exception
                        End Try
                        Return False
                    End If

                    If NewQutNumber <> "" AndAlso IsNumeric(NewQutNumber.Replace("""", "")) Then
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.AS400Number, NewQutNumber.Replace("""", ""))
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.AS400RowNumber, "001")
                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.AS400quoteReturnedJSON, PostDataForHH_D_HP)

                        UpdateQuotationAS400Number(NewQutNumber.Replace("""", ""), "001", BranchCode, QuatationNumber, PostDataForHH_D_HP)

                        GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.AS400.ToString, "Update quotation AS400", "Succeded - " & sMsgN, BranchCode, QuatationNumber, "", clsQuatation.ACTIVE_UseLoggedEmail.ToString)

                        Return True
                    End If

                    sMsgN &= " ## End Val = False "

                    Return False

                End If
            Else
                CanUpdateGal = False
            End If
            Return False
        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateQuotationGAL", "er message : " & ex.Message & " ## " & sMsgN, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            clsMail.SendEmailError("iQuote Update GAL-Faild Error", "", False, "UpdateQuotationGAL", BranchCode)
            Return False
        End Try
    End Function

    Private Shared Function DataTableToJSONWithStringBuilder(ByVal table As DataTable) As String
        Try
            Dim JSONString = New StringBuilder()

            If table.Rows.Count > 0 Then
                JSONString.Append("[")

                For i As Integer = 0 To table.Rows.Count - 1
                    JSONString.Append("{")

                    For j As Integer = 0 To table.Columns.Count - 1

                        If j < table.Columns.Count - 1 Then
                            If table.Columns(j).DataType.Name.ToString.ToUpper.Contains("INT") Then
                                JSONString.Append("""" & table.Columns(j).ColumnName.ToString() & """:" & table.Rows(i)(j).ToString() & ",")
                            Else
                                JSONString.Append("""" & table.Columns(j).ColumnName.ToString() & """:" & """" + table.Rows(i)(j).ToString() & """,")
                            End If

                        ElseIf j = table.Columns.Count - 1 Then
                            If table.Columns(j).DataType.Name.ToString.ToUpper.Contains("INT") Then
                                JSONString.Append("""" & table.Columns(j).ColumnName.ToString() & """:" & table.Rows(i)(j).ToString())
                            Else
                                JSONString.Append("""" & table.Columns(j).ColumnName.ToString() & """:" & """" + table.Rows(i)(j).ToString() & """")
                            End If

                        End If
                    Next

                    If i = table.Rows.Count - 1 Then
                        JSONString.Append("}")
                    Else
                        JSONString.Append("},")
                    End If
                Next

                JSONString.Append("]")
            End If

            Return JSONString.ToString()
        Catch ex As Exception
            'GeneralException.WriteEventErrors("DataTableToJSONWithStringBuilder-" & ex.Message, GeneralException.e_LogTitle.UPDATEDATA.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "DataTableToJSONWithStringBuilder", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
            Return ""
        End Try
    End Function



    Public Shared Sub UpdateQuotationUploadFilesServiceResult(BranchCode As String, QuotationNo As String, ReportBranchCode As String)
        Try

            'If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode) = "" Then
            '    BranchCode = StateManager.GetValue(StateManager.Keys.s_BranchCode)
            'Else
            '    BranchCode = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode)
            'End If

            Dim oTxSql As SqlDal
            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

            Dim oParamsD As New SqlParams()
            oParamsD.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNo))
            oParamsD.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            oParamsD.Add(New SqlParam("@ReportBranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, ReportBranchCode, 2))


            oTxSql.ExecuteSP("usp_UpdateQuotationUploadSrv", oParamsD)

            oParamsD = Nothing
            oTxSql = Nothing

        Catch ex As Exception
            'GeneralException.WriteEventErrors("UpdateQuotationUploadFilesServiceResult-" & ex.Message, GeneralException.e_LogTitle.UPDATEDATA.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "UpdateQuotationUploadFilesServiceResult", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)


            Throw
        End Try
    End Sub
End Class
