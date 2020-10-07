Public Class ChildAuxForm

    Private Sub ChildAuxForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        For count As Integer = 1 To 20
            ListView1.Items.Add("Message item " & count.ToString(), 0)
        Next count

    End Sub

    Private Sub ListView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick

        If Not ListView1.SelectedItems.Count = 0 Then
            Dim frm As New ChildForm2

            frm.Icon = System.Drawing.Icon.FromHandle(CType(Me.ImageList1.Images.Item(0), Bitmap).GetHicon)
            frm.Text = ListView1.SelectedItems.Item(0).Text
            frm.TextBox1.Text = "This is " + ListView1.SelectedItems.Item(0).Text

            frm.MdiParent = Me.MdiParent
            frm.Show()
        End If

    End Sub

End Class