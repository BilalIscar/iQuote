
Imports System.Configuration

Public Class CatalogIscarData


    Public Shared Function GetItemParametersMobileISO(itemNumber As String, vers As String, lang As String, NewStart As Boolean) As DataView
        Dim ser As String = ""
        Dim bCatError As Boolean = False

        Try
            'Dim i As Integer
            'i = "DD"
            Dim dtd As DataTable
            'Dim dtdX As DataTable
            Dim dv As DataView
            If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamsFromCatalog, "") Is Nothing AndAlso NewStart = False Then
                dtd = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamsFromCatalog, "")

            Else

                Dim sUseCATALOG_WSLocaly As String = ConfigurationManager.AppSettings("UserCATALOG_WSLocaly").ToString.Trim
                Dim sUserCATALOGWSORSQL As String = ConfigurationManager.AppSettings("UserCATALOG_WS_OR_SQL").ToString.Trim


                If sUserCATALOGWSORSQL = "WS" Then
                    If sUseCATALOG_WSLocaly = "TRUE" Then
                        Dim ws As New wsCATIscarDataLocal.IscarData
                        ser = "ws.GetItemParametersMobileISOAllParams"

                        ws.Timeout = 180000
                        'dtd = ws.GetItemParametersMobileISO(itemNumber, vers, lang)
                        'dtdX = ws.GetItemParametersISO(itemNumber, vers, lang)
                        dtd = ws.GetItemParametersMobileISOAllParams(itemNumber, vers, lang)
                        'dtd = Nothing
                        ws = Nothing
                    Else
                        Dim ws As New wsCATIscarData.IscarData
                        ws.Timeout = 180000
                        ser = "ws.GetItemParametersMobileISO"
                        bCatError = True
                        dtd = ws.GetItemParametersMobileISO(itemNumber, vers, lang)
                        'dtdX = ws.GetItemParametersISO(itemNumber, vers, lang)
                        'dtd = ws.GetItemParametersMobileISOAllParams(itemNumber, vers, lang)
                        ws = Nothing
                    End If
                Else
                    If lang = "" Then
                        lang = "EN"
                    End If
                    dtd = eCat.GetISOAllItems(itemNumber, vers, lang)
                End If

            End If

            'If Not IsNothing(dtd) Then
            '    dtd.Columns.Add("val_Choosen")
            '    For Each rd As DataRow In dtd.Rows
            '        Try
            '            If rd("val_Choosen") Is Nothing Or rd("val_Choosen").ToString.Trim = "" Then
            '                rd("val_Choosen") = rd("val")
            '            Else
            '                rd("val_Choosen") = ""
            '            End If
            '        Catch ex As Exception

            '        End Try
            '    Next
            'End If

            dv = dtd.DefaultView
            ' AND GPPRTP_ISO ='N'
            'dv.RowFilter = "Val<>'-' and (GIPRGP_REG+Val<>'RE0.00' )"
            Dim rp As String = ConfigurationManager.AppSettings("CataloIsoTableFilter").Replace("!", "<>")
            dv.RowFilter = rp

            Return dv

        Catch ex As Exception
            'GeneralException.WriteEventErrors("Function GetItemParametersMobileISO : " & ser, GeneralException.e_LogTitle.GENERAL.ToString)
            'GeneralException.WriteEventErrors("Function GetItemParametersMobileISO : " & ex.Message, GeneralException.e_LogTitle.GENERAL.ToString)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "GetItemParametersMobileISO-" & ser, ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

            'ex.Message = ex.Message & "CATALOG ERROR"

            clsMail.SendEmailError("Catalog Service error", "Function GetItemParametersMobileISO : " & ser & " <br> " & ex.Message & "<b>Error Geting Items from Catalog!</b><br>Please start a new quotation.", bCatError, "Catalog", "")
            Throw New Exception(ex.Message & " - CATALOG ERROR")
        End Try
    End Function


End Class
