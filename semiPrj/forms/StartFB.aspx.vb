Imports SemiApp_bl
Public Class StartFB
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            txtTimeOutType.Text = Request("Login")

        Catch ex As Exception
            txtTimeOutType.Text = Request("STARTFB_N")
        End Try



    End Sub

End Class