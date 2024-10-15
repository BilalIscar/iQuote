Public Class Notification
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'closeme.Attributes.Add("OnClick", "javascript:window.close():")
        'If Not Request("Error") Is Nothing Then
        '    'lblNot.Text = Request("Error").ToString.Replace("SessionTimeOut", "Session Time Out!")
        'End If


    End Sub

    'Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
    '    Response.Redirect("~/Default.aspx")
    'End Sub
End Class