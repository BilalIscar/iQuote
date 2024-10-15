Imports System.Net

Imports SemiApp_bl
Public Class ucBettredirect
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim streI As String = Request.Form("rErepTr")

            'lblName.Text = name
            'lblEmail.Text = email
            Dim UniqueID As String = Request.QueryString("rErepTr")



            Response.Redirect("../formAdmin/UserCustomerCnt.aspx?rErepTr=" & UniqueID.ToString, False)
        Catch ex As Exception
            ' SessionManager.SetQuotationValue(SessionManager.ActiveQuotation.QuotationOpenType, "")
        End Try



    End Sub

End Class