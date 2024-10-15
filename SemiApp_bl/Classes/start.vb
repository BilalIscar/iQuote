Public Class start
    Public Shared Function StartForm(statusQut As String, StartFrom As String, itemNumber As String, sLang As String, svers As String, BranchCode As String, Fam As String, FamilyPic As String, CusNo As String, RequestApplication As String, QutNumber As String, ClearSess As Boolean, ConnectFromQutList As Boolean) As Boolean
        Try

            If ClearSess = True Then
                SessionManager.Clear_ALL_Sessions()

            End If

            If QutNumber <> "" AndAlso QutNumber <> "0" Then

                Return SessionManager.SetSessionDetails_SEMI_ExistQuoatation(BranchCode, QutNumber, statusQut, ConnectFromQutList)

            Else

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.BranchCode, BranchCode)
                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationOpenType, StartFrom)

                Dim dtTimeStamp As DataTable = clsQuatation.GetQuotationDetailsByTimeStamp("XX")

                dtTimeStamp.Rows.Add()
                dtTimeStamp.Rows(0).Item("ID") = "0"
                dtTimeStamp.Rows(0).Item("OpenType") = StartFrom
                dtTimeStamp.Rows(0).Item("Language") = sLang
                dtTimeStamp.Rows(0).Item("vesion") = svers
                dtTimeStamp.Rows(0).Item("RequestDate") = Date.Now
                dtTimeStamp.Rows(0).Item("RequestApplication") = RequestApplication
                dtTimeStamp.Rows(0).Item("ItemNumber") = itemNumber

                Try
                    If StartFrom = clsQuatation.Enum_QuotationOpenType.MODIFICATION Then
                        dtTimeStamp.Rows(0).Item("ModelNum") = Fam
                        dtTimeStamp.Rows(0).Item("ModelID") = Fam
                        dtTimeStamp.Rows(0).Item("FamilyID") = FamilyPic
                    End If
                Catch ex As Exception

                    GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "StartForm - dtTimeStamp", "Error log in MONA LogOut - " & ex.Message, clsBranch.ReturnActiveBranchCodeState, clsQuatation.ACTIVE_QuotationNumber, clsQuatation.ACTIVE_GALQuotationNumber, clsQuatation.ACTIVE_UseLoggedEmail)

                End Try

                SessionManager.SetQuotationValue(SessionManager.ActiveQuotation._dtTempOnlyForCheck, dtTimeStamp)
                Try

                Catch ex As Exception

                End Try

            End If

        Catch ex As Exception
            Throw
        End Try

    End Function


End Class
