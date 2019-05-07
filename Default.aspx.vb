
Partial Class _Default
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
        End If

        If Page.IsPostBack And Len(TextBox1.Text) > 0 Then
            Response.Redirect("guide/?Search=" & HttpUtility.UrlEncode(TextBox1.Text.ToString))
            Response.End()
        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Response.Redirect("guide/?Search=" & HttpUtility.UrlEncode(TextBox1.Text.ToString))
        Response.End()
    End Sub
End Class
