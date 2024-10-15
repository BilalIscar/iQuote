Imports System.Configuration
Imports IscarDal
'Imports EncryptionDecryption
Public Class clsRates_Currency


    Public Shared Sub Get_Rates(ByVal BranchCode As String, FromCur As String, ToCur As String, Rate_T As String)

        Try

            Dim sCustomer As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, True)
            Dim dt As DataTable = GAL.GetGalData("CURRENCY", BranchCode, ConfigurationManager.AppSettings("AS400APPpathForGetData"), sCustomer, "", 1, FromCur, ToCur, "", "", "", True)

            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                If Not dt.Rows(0).Item("exchangeRate") Is Nothing AndAlso dt.Rows(0).Item("exchangeRate").ToString <> "" Then
                    If Rate_T.ToUpper = "RATETFRUSD" Then

                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.RateTFR_USD, dt.Rows(0).Item("exchangeRate"))
                    ElseIf Rate_T.ToUpper = "RATEMKTUSD" Then

                        SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.RateMKT_USD, dt.Rows(0).Item("exchangeRate"))
                    End If
                End If
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub


End Class
