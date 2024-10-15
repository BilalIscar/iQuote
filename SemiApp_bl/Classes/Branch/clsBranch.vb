Imports System.Configuration
Imports IscarDal

Public Class clsBranch
    Enum Enum_BranchesCode
        ISCAR_CANADA_JC = 1
        ISCAR_METALS_JU = 2
        INGERSOLL_USA_NU = 3
        ISCAR_MEXICO_JM = 4
        ISCAR_ARGENTINA_JA = 5
        ISCAR_BRASIL_JB = 6
        ISCAR_GMBH_IG = 7
        TUNGALOY_GMBH_LG = 8
        INGERSOLL_GMBH_NG = 9
        TAEGUTEC_EUROPE_13 = 10
        TAEGUTEC_ITALIA_TI = 11
        TUNGALOY_ITALIA_S_R_L_LI = 12
        ISCAR_ITALIA_IT = 13
        WERTEC_ITALY_14 = 14
        U_O_P_ITALY_FO = 15
        ISCAR_FRANCE_IF = 16
        OUTILTEC_FU = 17
        TAEGUTEC_FRANCE_11 = 18
        ISCAR_BELGIUM_IB = 19
        ISCAR_NEDERLAND_IH = 20
        ISCAR_TOOL_UK_IU = 21
        TAEGUTEC_UK_11 = 22
        HARDMETAL_IRELN_17 = 23
        ISCAR_SVERIGE_ID = 24
        ISCAR_FINLAND_IN = 25
        SVEA_MASKINER_11 = 26
        KJ_ISCAR_DK_11 = 27
        ISCAR_IBERICA_IP = 28
        ISCAR_PORTUGAL_IR = 29
        METALDUR_FW = 30
        ISCAR_SWISS_IZ = 31
        ISCAR_OSTR_IA = 32
        ISCAR_CROATIA_HC = 33
        ISCAR_SLOVENIJA_HL = 34
        ISCAR_TURKEY_IK = 35
        TUNGALOY_TURKEY_LY = 36
        TAEGUTEC_TURKEY_TT = 37
        SHABUM_ROMANIA_11 = 38
        ISCAR_ROMANIA_HM = 39
        ISCAR_HUNGARY_HH = 40
        ISCAR_POLAND_HP = 41
        TAEGUTEC_POLSKA_SP_Z_O_O_TC = 42
        ISCAR_CZECH_HZ = 43
        ISCAR_BULGARIA_HB = 44
        TWING_BELARUS_HO = 45
        ISCAR_SLOVAKIA_HV = 46
        ISCAR_S_A_KS = 47
        ISCAR_AUSTRALIA_KA = 48
        ISCAR_KOREA_IK_KK = 49
        TAEGUTEC_KOREA_TK = 50
        TAEGUTEC_CHINA_TN = 51
        ISCAR_CHINA_ZC = 52
        ISCAR_SHANGHAI_KC = 53
        ISCAR_SHANGHAI_XC = 54
        IMC_DALIAN_CHINA_FM = 55
        TUNGALOY_JAPAN_LJ = 56
        ISCAR_JAPAN_KJ = 57
        UNITAC_JAPAN_FN = 58
        MESCO_INC_11 = 59
        ISCAR_THAILAND_KT = 60
        TAEGUTEC_THAI_TE = 61
        ISCAR_TAIWAN_KW = 62
        LARSEN_TOUBRO_A2 = 63
        TAEGUTEC_INDIA_TA = 64
        ISCAR_ELECTROSTAL_FL = 65
        ISCAR_CIS_MOSCOW_RD = 66
        ISCAR_RUSS_LLC_HR = 67
        ISCAR_URAL_HU = 68
        ISCAR_UKRAINE_HK = 69
        ISCAR_T_A_ZC = 70
        ISCAR_HAIFA = 71
        ISCAR_T_A_AC = 72
        ISCAR_LTD_IS = 73
        Zurim_Tools_GZ = 74
    End Enum


    Public Shared Function GetSiteConnectionString() As String
        Try

            Return "MainConnectionString"

        Catch ex As Exception
            Return "MainConnectionString"
        End Try
    End Function


    Public Shared Function GetCountryName(BranchCode As String) As String

        Try
            Select Case BranchCode.ToUpper
                Case "WI" : Return "Germany"
                Case "JC" : Return "CANADA"
                Case "JU" : Return "USA"
                Case "JM" : Return "MEXICO"
                Case "JA" : Return "ARGENTINA"
                Case "JB" : Return "Brazil"
                Case "IG" : Return "Germany"
                Case "IT" : Return "Italy"
                Case "IF" : Return "FRANCE"
                Case "IB" : Return "BELGIUM"
                Case "IH" : Return "The Netherlands"
                Case "IU" : Return "UK"
                Case "ID" : Return "Sweden"
                Case "IN" : Return "Findland"
                Case "IP" : Return "Spain"
                Case "IR" : Return "PORTUGAL"
                Case "IZ" : Return "Switzerland"
                Case "IA" : Return "Australia"
                Case "HC" : Return "CROATIA"
                Case "HL" : Return "SLOVENIJA"
                Case "IK" : Return "TURKEY"
                Case "HM" : Return "Romania"
                Case "HH" : Return "HUNGARY"
                Case "HP" : Return "POLAND"
                Case "HZ" : Return "Czech Rep."
                Case "HB" : Return "BULGARIA"
                Case "HO" : Return "Belarus"
                Case "HV" : Return "SLOVAKIA"
                Case "KS" : Return "South Africa"
                Case "KA" : Return "AUSTRALIA"
                Case "X9" : Return "Korea"
                Case "KK" : Return "Korea"
                Case "ZC" : Return "CHINA"
                Case "KC" : Return "China"
                Case "XC" : Return "China"
                Case "LJ" : Return "OY JAPAN"
                Case "KJ" : Return "JAPAN"
                Case "KT" : Return "THAILAND"
                Case "KW" : Return "TAIWAN"
                Case "A2" : Return " Toubro"
                Case "KN" : Return "India"
                Case "FL" : Return "ELECTROSTAL"
                Case "RD" : Return "CIs MOSCOW"
                Case "HR" : Return "RUSS-LLC"
                Case "HU" : Return "URAL"
                Case "HK" : Return "UKRAINE"
                Case "IS" : Return "ISRAEL"
                Case "XZ" : Return "ISRAEL"
                Case "FD" : Return "USA"
                    'Case "ZZ" : Return "ISRAEL"
                Case Else
                    Return "Global"
            End Select
        Catch ex As Exception

        End Try

    End Function





    Public Shared Function GetBranchDetails(ByVal BranchCode As String) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@branchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            dt = oSql.ExecuteSPReturnDT("USP_GetBranchDetails", oParams)
            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetBranches() As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            dt = oSql.ExecuteSPReturnDT("USP_GetBranches", oParams)
            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetBranchNumberCode(ByRef BranchNumberCode As String) As String
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim sStr As String

            If IsNumeric(BranchNumberCode) Then
                sStr = oSql.ExecuteScalarUDF("Select BranchCode from glb_TBranchDetails where BranchNumber = " & CInt(BranchNumberCode))
            Else
                Dim sSql As String = "Select BranchNumber from glb_TBranchDetails where BranchCode = '" & BranchNumberCode & "'"
                sStr = CStr(oSql.ExecuteScalarUDF(sSql))
            End If
            Return sStr
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetWorkCenterID(ByRef BranchNumberCode As String) As String
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim sStr As String

            Dim sSql As String = "Select WorkCenterID from glb_TBranchDetails where BranchCode = '" & BranchNumberCode & "'"
            sStr = CStr(oSql.ExecuteScalarUDF(sSql))

            Return sStr
        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function ReturnActiveBranchCodeState() As String
        Try
            Dim bc As String = StateManager.GetValue(StateManager.Keys.s_BranchCode, True)
            If bc = "" Then
                bc = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False).ToString.Trim
            End If
            Return bc
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function ReturnActiveBranchLanguageId() As String
        Try
            Dim li As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.qut_LanguageId, False).ToString.Trim
            Return li
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function ReturnActiveBranchCodeSESSTION() As String
        Try
            Dim bc As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False).ToString.Trim
            If bc = "" Then
                bc = StateManager.GetValue(StateManager.Keys.s_BranchCode, True)
            End If
            Return bc
        Catch ex As Exception
            Throw
        End Try
    End Function


    Public Shared Function ReturnActiveBranchCodeForDocuments() As String
        Try
            Dim bc As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False).ToString.Trim
            If bc = "" Then
                bc = StateManager.GetValue(StateManager.Keys.s_BranchCode, True)

            End If
            Return bc

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function ReturnActiveStorageFolderForDouumentsForZZ() As String
        Try

            Return "ZZ" & ConfigurationManager.AppSettings("DOCMNG_ExtentionFolder")

        Catch ex As Exception
            ' GeneralException.WriteEventErrors("ReturnActiveStorageFolderForDouuments-" & ex.Message, GeneralException.e_LogTitle.GENERAL.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "ReturnActiveStorageFolderForDouumentsForZZ", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

        End Try
    End Function

    Public Shared Function ReturnActiveStorageFolderForDouuments() As String
        Try

            Return clsBranch.ReturnActiveBranchCodeForDocuments & ConfigurationManager.AppSettings("DOCMNG_ExtentionFolder")

        Catch ex As Exception
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "ReturnActiveStorageFolderForDouuments", ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)
        End Try


    End Function



    Public Shared Function GetBranchLanguages(BranchCode As String, GlobalBranchCode As String, CatalogueLanguageCode As String) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode))
            oParams.Add(New SqlParam("@GlobalBranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, CryptoManagerTDES.Decode(GlobalBranchCode)))
            oParams.Add(New SqlParam("@CatalogueLanguageCode", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, CatalogueLanguageCode))

            dt = oSql.ExecuteSPReturnDT("USP_GetBranchLanguages", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
