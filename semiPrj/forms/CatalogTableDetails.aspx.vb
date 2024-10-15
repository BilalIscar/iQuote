Imports SemiApp_bl
Public Class CatalogTableDetails
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim dt As DataTable = SessionManager.getQuotationValues(SessionManager.ActiveQuotation._dtParamsFromCatalog, "")
            gvRulles.DataSource = dt
            gvRulles.DataBind()

            If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber, False) Is Nothing Then
                lblItemNoCat.Text = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.itemNumber, False).ToString

            End If

            If Not SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.familyNo, False) Is Nothing Then
                lblItemNoCat.Text &= " : " & SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.familyNo, False).ToString

            End If
        Catch ex As Exception

        End Try
    End Sub

End Class