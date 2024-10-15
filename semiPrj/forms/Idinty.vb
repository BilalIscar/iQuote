Imports SemiApp_bl

Public Class Idinty


    Public Shared Function CheckSesstionTimeOut() As Boolean
        Try
            Dim sSess As String = ""
            If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False) Is Nothing Then
                sSess = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.QuotationOpenType, False).ToString
            End If

          If sSess = Nothing Then sSess = ""
            If sSess = "" Or sSess = "0" Then

                Return True
            End If
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Shared Function CheckSesstionTimeOutQuotList() As Boolean
        Try
            Dim sSess As String = ""
            If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempDontGetPrice, False) Is Nothing Then
                sSess = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TempDontGetPrice, False).ToString
            End If

            If sSess = Nothing Then sSess = ""
            If sSess = "" Then

                Return True
            End If
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
