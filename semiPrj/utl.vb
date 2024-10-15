Imports SemiApp_bl

Public Class utl


    Public Shared Function ReturnParamLanguage(Requestiqlang As String, Optional checkFromLogin As Boolean = False) As String

        Return CryptoManagerTDES.Encode(StateManager.GetValue(StateManager.Keys.s_BranchCode, True))

    End Function

    Public Shared Function ReturnReportLanguage(Requestiqlang As String, Optional checkFromLogin As Boolean = False) As String

        Dim rRequestLang As String = ""

        Try
            If checkFromLogin = True Then
                If Not StateManager.GetValue(StateManager.Keys.s_BranchCode, True) Is Nothing AndAlso StateManager.GetValue(StateManager.Keys.s_BranchCode, True) <> "" Then
                    Return CryptoManagerTDES.Encode(StateManager.GetValue(StateManager.Keys.s_BranchCode, True))
                End If
            End If

            If Not Requestiqlang Is Nothing AndAlso Requestiqlang.ToString <> "" Then
                Return Requestiqlang
            End If

            Try
                If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.selectedReportLanguage, False) Is Nothing Then
                    If SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.selectedReportLanguage, False).ToString <> "" Then
                        Return SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.selectedReportLanguage, False).ToString
                    End If
                End If
            Catch ex As Exception

            End Try
            If Not StateManager.GetValue(StateManager.Keys.s_BranchCode, True) Is Nothing AndAlso StateManager.GetValue(StateManager.Keys.s_BranchCode, True) <> "" Then
                Return CryptoManagerTDES.Encode(StateManager.GetValue(StateManager.Keys.s_BranchCode, True))
            Else
                Return CryptoManagerTDES.Decode("ZZ")
            End If

        Catch ex As Exception
            Return CryptoManagerTDES.Encode(StateManager.GetValue(StateManager.Keys.s_BranchCode, True))
        End Try

    End Function
End Class
