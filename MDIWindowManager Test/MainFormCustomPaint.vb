Public Class MainFormCustomPaint

    Private m_currentTabColor As Integer = 1

    Private Sub MainFormAltTabs_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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

    'NOTE: you must set the EnableTabPaint property at design-time or runtime to enable this event.
    Private Sub WindowManagerPanel1_TabPaint(ByVal sender As Object, ByVal e As MDIWindowManager.StandardTabsProvider.TabPaintEventArgs) Handles WindowManagerPanel1.TabPaint

        'Use e.Handled to draw the tab completely, otherwise you can change several of the options
        'such as font and color and allow the control to draw the rest.

        If e.Counter = 1 Or m_currentTabColor > 6 Then
            m_currentTabColor = 1
        End If

        If e.IsSelected Then
            'if the tab is the selected tab in the control, then draw it funny
            e.Graphics.DrawEllipse(Pens.Black, e.TabPaintOptions.TabRect)
            e.Graphics.DrawIcon(e.TabPaintOptions.Icon, e.TabPaintOptions.IconRect)
            e.Graphics.DrawString(e.TabPaintOptions.Text, e.TabPaintOptions.Font, Brushes.Black, e.TabPaintOptions.TextRect)
            e.Handled = True 'we've drawn the tab ourselves
        Else
            'tab is not selected
            e.TabPaintOptions.BackColorBrush = New SolidBrush(ColorTranslator.FromOle((Microsoft.VisualBasic.QBColor(m_currentTabColor))))
            e.TabPaintOptions.ForeColorBrush = Brushes.White
            e.TabPaintOptions.Font = New Font(e.TabPaintOptions.Font, FontStyle.Underline)
            e.Handled = False 'aside from color and font, we want the tab to draw normally otherwise
        End If

        m_currentTabColor += 1

    End Sub

End Class