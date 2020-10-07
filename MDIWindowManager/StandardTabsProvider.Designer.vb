<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StandardTabsProvider
    Inherits TabsProviderBase

    'Control overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            HideToolTip(Me)
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Control Designer
    Private components As System.ComponentModel.IContainer

    ' NOTE: The following procedure is required by the Component Designer
    ' It can be modified using the Component Designer.  Do not modify it
    ' using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.DrawAreaPanel = New MDIWindowManager.DrawPanel
        Me.ScrollLeftButton = New MDIWindowManager.WindowManagerButton
        Me.ScrollRightButton = New MDIWindowManager.WindowManagerButton
        Me.FocusKludgeButton = New System.Windows.Forms.Button
        Me.ScrollTimer = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'DrawAreaPanel
        '
        Me.DrawAreaPanel.AllowDrop = True
        Me.DrawAreaPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DrawAreaPanel.Location = New System.Drawing.Point(0, 0)
        Me.DrawAreaPanel.Name = "DrawAreaPanel"
        Me.DrawAreaPanel.Size = New System.Drawing.Size(372, 20)
        Me.DrawAreaPanel.TabIndex = 4
        '
        'ScrollLeftButton
        '
        Me.ScrollLeftButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ScrollLeftButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ScrollLeftButton.Font = New System.Drawing.Font("Marlett", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.ScrollLeftButton.Location = New System.Drawing.Point(372, 0)
        Me.ScrollLeftButton.Name = "ScrollLeftButton"
        Me.ScrollLeftButton.RenderMode = MDIWindowManager.WindowManagerButton.WindowManagerButtonRenderMode.RollOverClassic
        Me.ScrollLeftButton.Size = New System.Drawing.Size(20, 20)
        Me.ScrollLeftButton.TabIndex = 5
        Me.ScrollLeftButton.Text = "3"
        Me.ScrollLeftButton.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ScrollRightButton
        '
        Me.ScrollRightButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ScrollRightButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ScrollRightButton.Font = New System.Drawing.Font("Marlett", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.ScrollRightButton.Location = New System.Drawing.Point(392, 0)
        Me.ScrollRightButton.Name = "ScrollRightButton"
        Me.ScrollRightButton.RenderMode = MDIWindowManager.WindowManagerButton.WindowManagerButtonRenderMode.RollOverClassic
        Me.ScrollRightButton.Size = New System.Drawing.Size(20, 20)
        Me.ScrollRightButton.TabIndex = 6
        Me.ScrollRightButton.Text = "4"
        Me.ScrollRightButton.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'FocusKludgeButton
        '
        Me.FocusKludgeButton.Location = New System.Drawing.Point(-11, -11)
        Me.FocusKludgeButton.Name = "FocusKludgeButton"
        Me.FocusKludgeButton.Size = New System.Drawing.Size(10, 10)
        Me.FocusKludgeButton.TabIndex = 7
        '
        'ScrollTimer
        '
        Me.ScrollTimer.Interval = 400
        '
        'StandardTabsProvider
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.Controls.Add(Me.FocusKludgeButton)
        Me.Controls.Add(Me.DrawAreaPanel)
        Me.Controls.Add(Me.ScrollLeftButton)
        Me.Controls.Add(Me.ScrollRightButton)
        Me.Name = "StandardTabsProvider"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DrawAreaPanel As MDIWindowManager.DrawPanel
    Friend WithEvents ScrollLeftButton As WindowManagerButton
    Friend WithEvents ScrollRightButton As WindowManagerButton
    Friend WithEvents FocusKludgeButton As System.Windows.Forms.Button
    Friend WithEvents ScrollTimer As System.Windows.Forms.Timer

End Class

