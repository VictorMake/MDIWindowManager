Public Class MainFormAltTabs

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        ' * This is where we make our change for Custom TabsProviders *
        ' MDIWindowManager has an alternate TabsProvider that uses the intrinsic 
        ' Windows/.NET TabControl to display tabs. Let's use it.
        'Me.WindowManagerPanel1.CustomTabsProviderType = GetType(MDIWindowManager.StandardTabsProvider)
        'Me.WindowManagerPanel1.CustomTabsProviderType = GetType(MDIWindowManager.SystemTabsProvider)
        Me.WindowManagerPanel1.CustomTabsProviderType = GetType(Vs05StTabsProvider.Vs05StTabsProvider)

        ' Note 1: See the NewCustomTabsProviderInstance event elsewhere in this file for more information.
        ' Note 2: For convenience in using the SystemTabsProvider, the WindowManagerPanel.TabRenderMode property can be used in the designer.
        ' Note 3: See SystemTabsProvider class in the MDIWindowManager project to see how it does what it does.

    End Sub

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

    Private Sub WindowManagerPanel1_NewCustomTabsProviderInstance(ByVal sender As Object, ByVal e As MDIWindowManager.NewTabsProviderInstanceCreatedEventArgs) Handles WindowManagerPanel1.NewCustomTabsProviderInstance

        'Because we're not serializing Custom tab provider's properties at design-time,
        'you would respond to this event at runtime and set their properties here.
        'For instance, for the included "SystemTabsProvider," which uses
        'the built-in .NET TabControl, we can set a property like this:

        'If TypeOf e.TabsProvider Is MDIWindowManager.SystemTabsProvider Then
        '    CType(e.TabsProvider, MDIWindowManager.SystemTabsProvider).TabAppearance = TabAppearance.FlatButtons
        'End If

    End Sub

End Class