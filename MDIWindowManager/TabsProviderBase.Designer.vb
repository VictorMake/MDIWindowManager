<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TabsProviderBase
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.WindowsMenu = New System.Windows.Forms.ContextMenu
        Me.WindowsNoWindowsMenuItem = New System.Windows.Forms.MenuItem
        Me.SuspendLayout()
        '
        'WindowsMenu
        '
        Me.WindowsMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.WindowsNoWindowsMenuItem})
        '
        'WindowsNoWindowsMenuItem
        '
        Me.WindowsNoWindowsMenuItem.Enabled = False
        Me.WindowsNoWindowsMenuItem.Index = 0
        Me.WindowsNoWindowsMenuItem.Text = "(No Windows)"
        '
        'WindowTabStripTabsBase
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Name = "WindowTabStripTabsBase"
        Me.Size = New System.Drawing.Size(412, 20)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents WindowsMenu As System.Windows.Forms.ContextMenu
    Friend WithEvents WindowsNoWindowsMenuItem As System.Windows.Forms.MenuItem

End Class
