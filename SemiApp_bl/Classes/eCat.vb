Imports System.Configuration
Imports IscarDal
Public Class eCat

    Public Shared Function Getcat_MenuApplication() As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            'oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
            'oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuatationNumber, 4))

            dt = oSql.ExecuteSPReturnDT("USP_cat_MenuApplication", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetISOAllItems(cat As String, VER As String, Lang As String) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@cat", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, cat, 50))
            oParams.Add(New SqlParam("@VER", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, VER, 50))
            oParams.Add(New SqlParam("@Lang", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Lang, 50))


            dt = oSql.ExecuteSPReturnDT("USP_GETIsoALLItems", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function Get_SubData_1(FamilyCode As String, reapettime As Int16) As DataTable
        Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
        Dim oParams As New SqlParams()
        Dim dt As DataTable

        oParams.Add(New SqlParam("@MANUM", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, FamilyCode, 50))

        dt = oSql.ExecuteSPReturnDT("USP_cat_MenuFamilyTool", oParams)

        Dim i As Integer = dt.Rows.Count
        Dim j As Integer = dt.Rows(i - 1).Item("FamilyToolsId")
        If i < reapettime Then
            i = reapettime
        End If


        Do Until i >= reapettime
            dt.Rows.Add()
            dt.Rows(dt.Rows.Count - 1)("FamilyToolsId") = j + 1
            dt.Rows(dt.Rows.Count - 1)("MANUM") = "FamilyCode"
            dt.Rows(dt.Rows.Count - 1)("FamilyToolDes") = ""
            dt.Rows(dt.Rows.Count - 1)("Active") = False
            dt.Rows(dt.Rows.Count - 1)("Visible") = True
            dt.Rows(dt.Rows.Count - 1)("PictureName") = ""
            i += 1
            j += 1
        Loop

        'Dim dt As New DataTable
        'dt.Columns.Add("Index")
        'dt.Columns.Add("Name")
        'dt.Columns.Add("PICTURE")

        'dt.Rows.Add()
        'dt.Rows(dt.Rows.Count - 1)("Index") = "1"
        'dt.Rows(dt.Rows.Count - 1)("Name") = "indexable"
        'dt.Rows(dt.Rows.Count - 1)("PICTURE") = "indexable"
        'i += 1


        'dt.Rows.Add()
        'dt.Rows(dt.Rows.Count - 1)("Index") = "2"
        'dt.Rows(dt.Rows.Count - 1)("Name") = "Solid"
        'dt.Rows(dt.Rows.Count - 1)("PICTURE") = "Solid"
        'i += 1

        'Do Until i >= reapettime
        '    dt.Rows.Add()
        '    dt.Rows(dt.Rows.Count - 1)("Index") = i + 1
        '    dt.Rows(dt.Rows.Count - 1)("Name") = ""
        '    dt.Rows(dt.Rows.Count - 1)("PICTURE") = ""
        '    i += 1
        'Loop

        Return dt
        ''Try
        ''    Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
        ''    Dim oParams As New SqlParams()
        ''    Dim dt As DataTable

        ''    'oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode, 2))
        ''    'oParams.Add(New SqlParam("@QuotationNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuatationNumber, 4))

        ''    dt = oSql.ExecuteSPReturnDT("", oParams)

        ''    Return dt
        ''Catch ex As Exception
        ''    Throw
        ''End Try

    End Function





    Public Shared Function Getcat_MOBILEISOALLITEMS(ver As String, Line As String, Local_Cloud As String) As DataTable

        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)

            Dim dt As DataTable = clsModel.GetModellist("M", ver, Line)

            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Try

                        If dr("ModelNum").ToString <> "" Then

                            Dim dt1 As DataTable = clsCatFamilyItems.GetCatFamilyItems_AS400(dr("ModelNum").ToString.Trim, ver)

                            If Not dt1 Is Nothing AndAlso dt1.Rows.Count > 0 Then

                                For Each dr1 As DataRow In dt1.Rows
                                    If Not dr1("GICAT") Is Nothing AndAlso dr1("GICAT").ToString.Trim <> "" Then
                                        Try
                                            'Dim Local_Cloud As String =
                                            Dim dtC As DataTable

                                            If Local_Cloud = "LOCAL" Then
                                                Dim ws As New wsCATIscarDataLocal.IscarData
                                                ws.Timeout = 180000
                                                dtC = ws.GetItemParametersMobileISOAllParams(dr1("GICAT").ToString.Trim, ver, "en")
                                                ws = Nothing
                                            ElseIf Local_Cloud = "CLOUD" Then
                                                Dim ws As New wsCATIscarData.IscarData
                                                ws.Timeout = 180000
                                                dtC = ws.GetItemParametersMobileISOAllParams(dr1("GICAT").ToString.Trim, ver, "en")
                                                ws = Nothing
                                            End If

                                            For Each rr As DataRow In dtC.Rows
                                                Dim oParams As New SqlParams()
                                                oParams.Add(New SqlParam("@GIPRGP_REG", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, rr.Item("GIPRGP_REG").ToString, 50))
                                                oParams.Add(New SqlParam("@GIPRGP_ISO", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, rr.Item("GIPRGP_ISO").ToString, 50))
                                                oParams.Add(New SqlParam("@GPNUM_REG", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, rr.Item("GPNUM_REG").ToString, 50))
                                                oParams.Add(New SqlParam("@GPNUM_ISO", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, rr.Item("GPNUM_ISO").ToString, 50))
                                                oParams.Add(New SqlParam("@GPDSC_REG", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, rr.Item("GPDSC_REG").ToString, 50))
                                                oParams.Add(New SqlParam("@GPDSC_ISO", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, rr.Item("GPDSC_ISO").ToString, 50))
                                                oParams.Add(New SqlParam("@GFIREM_REG", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, rr.Item("GFIREM_REG").ToString, 50))
                                                oParams.Add(New SqlParam("@GFIREM_ISO", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, rr.Item("GFIREM_ISO").ToString, 50))
                                                oParams.Add(New SqlParam("@GPPRTP_REG", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, rr.Item("GPPRTP_REG").ToString, 50))
                                                oParams.Add(New SqlParam("@GPPRTP_ISO", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, rr.Item("GPPRTP_ISO").ToString, 50))
                                                oParams.Add(New SqlParam("@ValRemark", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, rr.Item("ValRemark").ToString, 500))
                                                oParams.Add(New SqlParam("@Val", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, rr.Item("Val").ToString, 50))
                                                oParams.Add(New SqlParam("@ShowHide", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, rr.Item("ShowHide").ToString, 50))
                                                'oParams.Add(New SqlParam("@ShowHide", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, rr.Item("ShowHide").ToString, 50))
                                                'oParams.Add(New SqlParam("@ShowHide", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, rr.Item("ShowHide").ToString, 50))
                                                oParams.Add(New SqlParam("@cat", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, dr1("GICAT").ToString.Trim, 50))
                                                oParams.Add(New SqlParam("@VER", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, ver, 50))
                                                oParams.Add(New SqlParam("@Lang", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, "en", 50))

                                                oSql.ExecuteSP("USP_AddIsoItems", oParams)
                                                oParams = Nothing
                                            Next
                                        Catch ex As Exception
                                            Throw
                                        End Try
                                    End If

                                Next

                            End If

                        End If

                    Catch ex As Exception
                        Throw
                    End Try

                Next
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
