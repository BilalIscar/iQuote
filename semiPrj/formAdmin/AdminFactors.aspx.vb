
Imports SemiApp_bl
Public Class AdminFactors

    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim unicid As String = Request("rErepTr")
        Response.Redirect("AdminFactorsm.aspx?rErepTr=" & unicid, True)


    End Sub


End Class