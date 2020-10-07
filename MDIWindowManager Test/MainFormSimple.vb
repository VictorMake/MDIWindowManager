Public Class MainFormSimple

    Private Sub MainFormSimple_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'load a bunch of mdi children
        For count As Integer = 1 To 10
            Dim frm As New ChildForm1

            frm.Text = "Window " + CStr(count)
            frm.TextBox1.Text = "I am Form " + CStr(count)

            frm.MdiParent = Me

            'If AutoDetectMdiChildren property were False this would be
            'the only line of code that is different than regular old mdi.
            'WindowManagerPanel1.AddWindow(frm)

            frm.Show()
        Next count

        'set the focus on the first mdi child
        Me.MdiChildren(0).BringToFront()
        'Equivalent method: 
        'WindowManagerPanel1.SetActiveWindow(0)
        'it is recommended (though not necessary) to use the WindowManager methods

    End Sub

    Private Sub FileExitMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FileExitMenuItem.Click

        Me.Close()

    End Sub

    Private Sub SampleShowAdvMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SampleShowAdvMenuItem.Click

        Dim frm As New MainFormAdvanced

        frm.Show()

    End Sub

    Private Sub SampleShowAltTabsMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SampleShowAltTabsMenuItem.Click

        Dim frm As New MainFormAltTabs

        frm.Show()

    End Sub

    Private Sub SampleShowCustomPaintMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SampleShowCustomPaintMenuItem.Click

        Dim frm As New MainFormCustomPaint

        frm.Show()

    End Sub

End Class