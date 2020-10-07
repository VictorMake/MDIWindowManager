Public Class MainFormAdvanced
    Inherits System.Windows.Forms.Form

    Private Enum SampleViewMode
        Simple
        AdvancedBottom
        AdvancedRight
        AdvancedLeft
    End Enum

    Private m_useWindowManager As Boolean = True
    Friend WithEvents ViewTabStyleMenuItemSep1 As System.Windows.Forms.MenuItem
    Friend WithEvents ViewTabStylesMoreInfoMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ViewShowCloseButtonMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ViewShowIconsMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ViewButtonStylesMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ViewButtonStylesStandardMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ViewButtonStylesSystemMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ViewButtonStylesProMenuItem As System.Windows.Forms.MenuItem
    Private m_viewMode As SampleViewMode

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents StatusBar1 As System.Windows.Forms.StatusBar
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents ToolBar1 As System.Windows.Forms.ToolBar
    Friend WithEvents ToolBoxPanel As System.Windows.Forms.Panel
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents ToolBoxSplitter As System.Windows.Forms.Splitter
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents WindowManagerPanel1 As MDIWindowManager.WindowManagerPanel
    Friend WithEvents FileMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents WindowMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents FileNewMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents FileOpenMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents FileMenuItemSep1 As System.Windows.Forms.MenuItem
    Friend WithEvents FileExitMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents WindowHTileMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents WindowTileMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents WindowPopOutMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents WindowMenuItemSep1 As System.Windows.Forms.MenuItem
    Friend WithEvents WindowCloseAllMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents WindowMenuItemSep2 As System.Windows.Forms.MenuItem
    Friend WithEvents ViewMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ViewSimpleMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ViewAdvRightMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ViewAdvBottomMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ViewAdvLeftMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents ToolBoxLabel As System.Windows.Forms.Label
    Friend WithEvents ViewMenuSep1 As System.Windows.Forms.MenuItem
    Friend WithEvents ShowToolboxMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ToolBoxInfoLabel1 As System.Windows.Forms.Label
    Friend WithEvents ToolBoxInfoLabel2 As System.Windows.Forms.Label
    Friend WithEvents FileNewTButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents FileOpenTButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents WindowMoreWindowsMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ClassicMdiWindowMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ClassicMdiWindowCascadeMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ClassicMdiWindowTileHorizMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ClassicMdiWindowTileVertMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ViewMenuItemSep2 As System.Windows.Forms.MenuItem
    Friend WithEvents SwitchToClassicMdiMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ViewShowLayoutButtonsMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ViewMenuSep4 As System.Windows.Forms.MenuItem
    Friend WithEvents ViewMenuItemSep3 As System.Windows.Forms.MenuItem
    Friend WithEvents ViewTabStylesMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ViewTabStylesClassicMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ViewTabStylesModernMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ViewTabStylesFlatHiliteMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ViewTabStylesAngledHiliteMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ViewAppearanceMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ViewShowTitleMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents HelpMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents HelpTopicsMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents HelpAboutMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents HelpMenuSep1 As System.Windows.Forms.MenuItem

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainFormAdvanced))
        Me.StatusBar1 = New System.Windows.Forms.StatusBar
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.FileMenuItem = New System.Windows.Forms.MenuItem
        Me.FileNewMenuItem = New System.Windows.Forms.MenuItem
        Me.FileOpenMenuItem = New System.Windows.Forms.MenuItem
        Me.FileMenuItemSep1 = New System.Windows.Forms.MenuItem
        Me.FileExitMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewSimpleMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewAdvRightMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewAdvBottomMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewAdvLeftMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewMenuSep1 = New System.Windows.Forms.MenuItem
        Me.ViewAppearanceMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewTabStylesMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewTabStylesClassicMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewTabStylesModernMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewTabStylesFlatHiliteMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewTabStylesAngledHiliteMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewTabStyleMenuItemSep1 = New System.Windows.Forms.MenuItem
        Me.ViewTabStylesMoreInfoMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewButtonStylesMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewButtonStylesStandardMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewButtonStylesSystemMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewButtonStylesProMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewMenuItemSep3 = New System.Windows.Forms.MenuItem
        Me.ViewShowTitleMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewShowIconsMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewShowLayoutButtonsMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewShowCloseButtonMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewMenuSep4 = New System.Windows.Forms.MenuItem
        Me.ShowToolboxMenuItem = New System.Windows.Forms.MenuItem
        Me.ViewMenuItemSep2 = New System.Windows.Forms.MenuItem
        Me.SwitchToClassicMdiMenuItem = New System.Windows.Forms.MenuItem
        Me.WindowMenuItem = New System.Windows.Forms.MenuItem
        Me.WindowHTileMenuItem = New System.Windows.Forms.MenuItem
        Me.WindowTileMenuItem = New System.Windows.Forms.MenuItem
        Me.WindowPopOutMenuItem = New System.Windows.Forms.MenuItem
        Me.WindowMenuItemSep1 = New System.Windows.Forms.MenuItem
        Me.WindowCloseAllMenuItem = New System.Windows.Forms.MenuItem
        Me.WindowMenuItemSep2 = New System.Windows.Forms.MenuItem
        Me.WindowMoreWindowsMenuItem = New System.Windows.Forms.MenuItem
        Me.ClassicMdiWindowMenuItem = New System.Windows.Forms.MenuItem
        Me.ClassicMdiWindowCascadeMenuItem = New System.Windows.Forms.MenuItem
        Me.ClassicMdiWindowTileHorizMenuItem = New System.Windows.Forms.MenuItem
        Me.ClassicMdiWindowTileVertMenuItem = New System.Windows.Forms.MenuItem
        Me.HelpMenuItem = New System.Windows.Forms.MenuItem
        Me.HelpTopicsMenuItem = New System.Windows.Forms.MenuItem
        Me.HelpMenuSep1 = New System.Windows.Forms.MenuItem
        Me.HelpAboutMenuItem = New System.Windows.Forms.MenuItem
        Me.ToolBar1 = New System.Windows.Forms.ToolBar
        Me.FileNewTButton = New System.Windows.Forms.ToolBarButton
        Me.FileOpenTButton = New System.Windows.Forms.ToolBarButton
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ToolBoxPanel = New System.Windows.Forms.Panel
        Me.ToolBoxInfoLabel2 = New System.Windows.Forms.Label
        Me.ToolBoxInfoLabel1 = New System.Windows.Forms.Label
        Me.ToolBoxLabel = New System.Windows.Forms.Label
        Me.Button4 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.ToolBoxSplitter = New System.Windows.Forms.Splitter
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.WindowManagerPanel1 = New MDIWindowManager.WindowManagerPanel
        Me.ToolBoxPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusBar1
        '
        Me.StatusBar1.Location = New System.Drawing.Point(0, 457)
        Me.StatusBar1.Name = "StatusBar1"
        Me.StatusBar1.Size = New System.Drawing.Size(697, 16)
        Me.StatusBar1.TabIndex = 4
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.FileMenuItem, Me.ViewMenuItem, Me.WindowMenuItem, Me.ClassicMdiWindowMenuItem, Me.HelpMenuItem})
        '
        'FileMenuItem
        '
        Me.FileMenuItem.Index = 0
        Me.FileMenuItem.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.FileNewMenuItem, Me.FileOpenMenuItem, Me.FileMenuItemSep1, Me.FileExitMenuItem})
        Me.FileMenuItem.Text = "&File"
        '
        'FileNewMenuItem
        '
        Me.FileNewMenuItem.Index = 0
        Me.FileNewMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlN
        Me.FileNewMenuItem.Text = "&New"
        '
        'FileOpenMenuItem
        '
        Me.FileOpenMenuItem.Index = 1
        Me.FileOpenMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlO
        Me.FileOpenMenuItem.Text = "&Open..."
        '
        'FileMenuItemSep1
        '
        Me.FileMenuItemSep1.Index = 2
        Me.FileMenuItemSep1.Text = "-"
        '
        'FileExitMenuItem
        '
        Me.FileExitMenuItem.Index = 3
        Me.FileExitMenuItem.Text = "E&xit"
        '
        'ViewMenuItem
        '
        Me.ViewMenuItem.Index = 1
        Me.ViewMenuItem.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.ViewSimpleMenuItem, Me.ViewAdvRightMenuItem, Me.ViewAdvBottomMenuItem, Me.ViewAdvLeftMenuItem, Me.ViewMenuSep1, Me.ViewAppearanceMenuItem, Me.ViewMenuSep4, Me.ShowToolboxMenuItem, Me.ViewMenuItemSep2, Me.SwitchToClassicMdiMenuItem})
        Me.ViewMenuItem.Text = "&View"
        '
        'ViewSimpleMenuItem
        '
        Me.ViewSimpleMenuItem.Index = 0
        Me.ViewSimpleMenuItem.RadioCheck = True
        Me.ViewSimpleMenuItem.Text = "Simple"
        '
        'ViewAdvRightMenuItem
        '
        Me.ViewAdvRightMenuItem.Index = 1
        Me.ViewAdvRightMenuItem.RadioCheck = True
        Me.ViewAdvRightMenuItem.Text = "Advanced View (Right)"
        '
        'ViewAdvBottomMenuItem
        '
        Me.ViewAdvBottomMenuItem.Index = 2
        Me.ViewAdvBottomMenuItem.RadioCheck = True
        Me.ViewAdvBottomMenuItem.Text = "Advanced View (Bottom)"
        '
        'ViewAdvLeftMenuItem
        '
        Me.ViewAdvLeftMenuItem.Index = 3
        Me.ViewAdvLeftMenuItem.RadioCheck = True
        Me.ViewAdvLeftMenuItem.Text = "Advanced View (Left)"
        '
        'ViewMenuSep1
        '
        Me.ViewMenuSep1.Index = 4
        Me.ViewMenuSep1.Text = "-"
        '
        'ViewAppearanceMenuItem
        '
        Me.ViewAppearanceMenuItem.Index = 5
        Me.ViewAppearanceMenuItem.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.ViewTabStylesMenuItem, Me.ViewButtonStylesMenuItem, Me.ViewMenuItemSep3, Me.ViewShowTitleMenuItem, Me.ViewShowIconsMenuItem, Me.ViewShowLayoutButtonsMenuItem, Me.ViewShowCloseButtonMenuItem})
        Me.ViewAppearanceMenuItem.Text = "Appearance"
        '
        'ViewTabStylesMenuItem
        '
        Me.ViewTabStylesMenuItem.Index = 0
        Me.ViewTabStylesMenuItem.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.ViewTabStylesClassicMenuItem, Me.ViewTabStylesModernMenuItem, Me.ViewTabStylesFlatHiliteMenuItem, Me.ViewTabStylesAngledHiliteMenuItem, Me.ViewTabStyleMenuItemSep1, Me.ViewTabStylesMoreInfoMenuItem})
        Me.ViewTabStylesMenuItem.Text = "Tab Style"
        '
        'ViewTabStylesClassicMenuItem
        '
        Me.ViewTabStylesClassicMenuItem.Index = 0
        Me.ViewTabStylesClassicMenuItem.RadioCheck = True
        Me.ViewTabStylesClassicMenuItem.Text = "Classic"
        '
        'ViewTabStylesModernMenuItem
        '
        Me.ViewTabStylesModernMenuItem.Index = 1
        Me.ViewTabStylesModernMenuItem.RadioCheck = True
        Me.ViewTabStylesModernMenuItem.Text = "Modern"
        '
        'ViewTabStylesFlatHiliteMenuItem
        '
        Me.ViewTabStylesFlatHiliteMenuItem.Index = 2
        Me.ViewTabStylesFlatHiliteMenuItem.RadioCheck = True
        Me.ViewTabStylesFlatHiliteMenuItem.Text = "FlatHilite"
        '
        'ViewTabStylesAngledHiliteMenuItem
        '
        Me.ViewTabStylesAngledHiliteMenuItem.Index = 3
        Me.ViewTabStylesAngledHiliteMenuItem.RadioCheck = True
        Me.ViewTabStylesAngledHiliteMenuItem.Text = "AngledHilite"
        '
        'ViewTabStyleMenuItemSep1
        '
        Me.ViewTabStyleMenuItemSep1.Index = 4
        Me.ViewTabStyleMenuItemSep1.Text = "-"
        '
        'ViewTabStylesMoreInfoMenuItem
        '
        Me.ViewTabStylesMoreInfoMenuItem.Index = 5
        Me.ViewTabStylesMoreInfoMenuItem.Text = "More info..."
        '
        'ViewButtonStylesMenuItem
        '
        Me.ViewButtonStylesMenuItem.Index = 1
        Me.ViewButtonStylesMenuItem.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.ViewButtonStylesStandardMenuItem, Me.ViewButtonStylesSystemMenuItem, Me.ViewButtonStylesProMenuItem})
        Me.ViewButtonStylesMenuItem.Text = "Button Style"
        '
        'ViewButtonStylesStandardMenuItem
        '
        Me.ViewButtonStylesStandardMenuItem.Index = 0
        Me.ViewButtonStylesStandardMenuItem.RadioCheck = True
        Me.ViewButtonStylesStandardMenuItem.Text = "Standard"
        '
        'ViewButtonStylesSystemMenuItem
        '
        Me.ViewButtonStylesSystemMenuItem.Index = 1
        Me.ViewButtonStylesSystemMenuItem.RadioCheck = True
        Me.ViewButtonStylesSystemMenuItem.Text = "System"
        '
        'ViewButtonStylesProMenuItem
        '
        Me.ViewButtonStylesProMenuItem.Index = 2
        Me.ViewButtonStylesProMenuItem.RadioCheck = True
        Me.ViewButtonStylesProMenuItem.Text = "Professional"
        '
        'ViewMenuItemSep3
        '
        Me.ViewMenuItemSep3.Index = 2
        Me.ViewMenuItemSep3.Text = "-"
        '
        'ViewShowTitleMenuItem
        '
        Me.ViewShowTitleMenuItem.Index = 3
        Me.ViewShowTitleMenuItem.Text = "Show Title"
        '
        'ViewShowIconsMenuItem
        '
        Me.ViewShowIconsMenuItem.Index = 4
        Me.ViewShowIconsMenuItem.Text = "Show Icons"
        '
        'ViewShowLayoutButtonsMenuItem
        '
        Me.ViewShowLayoutButtonsMenuItem.Index = 5
        Me.ViewShowLayoutButtonsMenuItem.Text = "Show Window Layout Buttons"
        '
        'ViewShowCloseButtonMenuItem
        '
        Me.ViewShowCloseButtonMenuItem.Index = 6
        Me.ViewShowCloseButtonMenuItem.Text = "Show Close Button"
        '
        'ViewMenuSep4
        '
        Me.ViewMenuSep4.Index = 6
        Me.ViewMenuSep4.Text = "-"
        '
        'ShowToolboxMenuItem
        '
        Me.ShowToolboxMenuItem.Index = 7
        Me.ShowToolboxMenuItem.Text = "Show Toolbox"
        '
        'ViewMenuItemSep2
        '
        Me.ViewMenuItemSep2.Index = 8
        Me.ViewMenuItemSep2.Text = "-"
        '
        'SwitchToClassicMdiMenuItem
        '
        Me.SwitchToClassicMdiMenuItem.Index = 9
        Me.SwitchToClassicMdiMenuItem.Text = "Switch to Classic MDI"
        '
        'WindowMenuItem
        '
        Me.WindowMenuItem.Index = 2
        Me.WindowMenuItem.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.WindowHTileMenuItem, Me.WindowTileMenuItem, Me.WindowPopOutMenuItem, Me.WindowMenuItemSep1, Me.WindowCloseAllMenuItem, Me.WindowMenuItemSep2, Me.WindowMoreWindowsMenuItem})
        Me.WindowMenuItem.Text = "&Window"
        '
        'WindowHTileMenuItem
        '
        Me.WindowHTileMenuItem.Index = 0
        Me.WindowHTileMenuItem.Text = "New &Horizontal Tab Group"
        '
        'WindowTileMenuItem
        '
        Me.WindowTileMenuItem.Index = 1
        Me.WindowTileMenuItem.Text = "&Tile or Untile Current Window"
        '
        'WindowPopOutMenuItem
        '
        Me.WindowPopOutMenuItem.Index = 2
        Me.WindowPopOutMenuItem.Text = "&Pop Out Current Window"
        '
        'WindowMenuItemSep1
        '
        Me.WindowMenuItemSep1.Index = 3
        Me.WindowMenuItemSep1.Text = "-"
        '
        'WindowCloseAllMenuItem
        '
        Me.WindowCloseAllMenuItem.Index = 4
        Me.WindowCloseAllMenuItem.Text = "Close A&ll"
        '
        'WindowMenuItemSep2
        '
        Me.WindowMenuItemSep2.Index = 5
        Me.WindowMenuItemSep2.Text = "-"
        '
        'WindowMoreWindowsMenuItem
        '
        Me.WindowMoreWindowsMenuItem.Index = 6
        Me.WindowMoreWindowsMenuItem.Text = "&Windows..."
        '
        'ClassicMdiWindowMenuItem
        '
        Me.ClassicMdiWindowMenuItem.Index = 3
        Me.ClassicMdiWindowMenuItem.MdiList = True
        Me.ClassicMdiWindowMenuItem.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.ClassicMdiWindowCascadeMenuItem, Me.ClassicMdiWindowTileHorizMenuItem, Me.ClassicMdiWindowTileVertMenuItem})
        Me.ClassicMdiWindowMenuItem.Text = "&Window"
        '
        'ClassicMdiWindowCascadeMenuItem
        '
        Me.ClassicMdiWindowCascadeMenuItem.Index = 0
        Me.ClassicMdiWindowCascadeMenuItem.Text = "Cascade"
        '
        'ClassicMdiWindowTileHorizMenuItem
        '
        Me.ClassicMdiWindowTileHorizMenuItem.Index = 1
        Me.ClassicMdiWindowTileHorizMenuItem.Text = "Tile Horizontally"
        '
        'ClassicMdiWindowTileVertMenuItem
        '
        Me.ClassicMdiWindowTileVertMenuItem.Index = 2
        Me.ClassicMdiWindowTileVertMenuItem.Text = "Tile Vertically"
        '
        'HelpMenuItem
        '
        Me.HelpMenuItem.Index = 4
        Me.HelpMenuItem.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.HelpTopicsMenuItem, Me.HelpMenuSep1, Me.HelpAboutMenuItem})
        Me.HelpMenuItem.Text = "Help"
        '
        'HelpTopicsMenuItem
        '
        Me.HelpTopicsMenuItem.Index = 0
        Me.HelpTopicsMenuItem.Text = "Online Documentation"
        '
        'HelpMenuSep1
        '
        Me.HelpMenuSep1.Index = 1
        Me.HelpMenuSep1.Text = "-"
        '
        'HelpAboutMenuItem
        '
        Me.HelpAboutMenuItem.Index = 2
        Me.HelpAboutMenuItem.Text = "About MDIWindowManager Sample"
        '
        'ToolBar1
        '
        Me.ToolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
        Me.ToolBar1.AutoSize = False
        Me.ToolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.FileNewTButton, Me.FileOpenTButton})
        Me.ToolBar1.DropDownArrows = True
        Me.ToolBar1.ImageList = Me.ImageList1
        Me.ToolBar1.Location = New System.Drawing.Point(0, 0)
        Me.ToolBar1.Name = "ToolBar1"
        Me.ToolBar1.ShowToolTips = True
        Me.ToolBar1.Size = New System.Drawing.Size(697, 28)
        Me.ToolBar1.TabIndex = 8
        Me.ToolBar1.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right
        '
        'FileNewTButton
        '
        Me.FileNewTButton.ImageIndex = 0
        Me.FileNewTButton.Name = "FileNewTButton"
        Me.FileNewTButton.ToolTipText = "New"
        '
        'FileOpenTButton
        '
        Me.FileOpenTButton.ImageIndex = 1
        Me.FileOpenTButton.Name = "FileOpenTButton"
        Me.FileOpenTButton.ToolTipText = "Open"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Silver
        Me.ImageList1.Images.SetKeyName(0, "")
        Me.ImageList1.Images.SetKeyName(1, "")
        '
        'ToolBoxPanel
        '
        Me.ToolBoxPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.ToolBoxPanel.Controls.Add(Me.ToolBoxInfoLabel2)
        Me.ToolBoxPanel.Controls.Add(Me.ToolBoxInfoLabel1)
        Me.ToolBoxPanel.Controls.Add(Me.ToolBoxLabel)
        Me.ToolBoxPanel.Controls.Add(Me.Button4)
        Me.ToolBoxPanel.Controls.Add(Me.Button3)
        Me.ToolBoxPanel.Controls.Add(Me.Button2)
        Me.ToolBoxPanel.Controls.Add(Me.Button1)
        Me.ToolBoxPanel.Dock = System.Windows.Forms.DockStyle.Left
        Me.ToolBoxPanel.Location = New System.Drawing.Point(0, 28)
        Me.ToolBoxPanel.Name = "ToolBoxPanel"
        Me.ToolBoxPanel.Size = New System.Drawing.Size(100, 429)
        Me.ToolBoxPanel.TabIndex = 10
        '
        'ToolBoxInfoLabel2
        '
        Me.ToolBoxInfoLabel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ToolBoxInfoLabel2.Location = New System.Drawing.Point(5, 291)
        Me.ToolBoxInfoLabel2.Name = "ToolBoxInfoLabel2"
        Me.ToolBoxInfoLabel2.Size = New System.Drawing.Size(87, 126)
        Me.ToolBoxInfoLabel2.TabIndex = 6
        Me.ToolBoxInfoLabel2.Text = "For instance, a toolbox like this could be used to switch the ""Axilliary Window"" " & _
            "property of MDIWindowManager."
        '
        'ToolBoxInfoLabel1
        '
        Me.ToolBoxInfoLabel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ToolBoxInfoLabel1.Location = New System.Drawing.Point(5, 190)
        Me.ToolBoxInfoLabel1.Name = "ToolBoxInfoLabel1"
        Me.ToolBoxInfoLabel1.Size = New System.Drawing.Size(87, 97)
        Me.ToolBoxInfoLabel1.TabIndex = 5
        Me.ToolBoxInfoLabel1.Text = "This toolbox is not a feature of MDIWindowManager. It is simply a panel docked to" & _
            " the side of the MDI parent."
        '
        'ToolBoxLabel
        '
        Me.ToolBoxLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ToolBoxLabel.BackColor = System.Drawing.SystemColors.ControlDark
        Me.ToolBoxLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.ToolBoxLabel.Location = New System.Drawing.Point(2, 2)
        Me.ToolBoxLabel.Name = "ToolBoxLabel"
        Me.ToolBoxLabel.Size = New System.Drawing.Size(92, 14)
        Me.ToolBoxLabel.TabIndex = 4
        Me.ToolBoxLabel.Text = "Toolbox"
        '
        'Button4
        '
        Me.Button4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button4.Image = Global.MDIWindowManager_Test.My.Resources.Resources.Calender_2
        Me.Button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button4.Location = New System.Drawing.Point(8, 142)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(104, 40)
        Me.Button4.TabIndex = 3
        Me.Button4.Text = "Tasks"
        Me.Button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button3.Image = Global.MDIWindowManager_Test.My.Resources.Resources.Calender_1
        Me.Button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button3.Location = New System.Drawing.Point(8, 102)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(104, 40)
        Me.Button3.TabIndex = 2
        Me.Button3.Text = "Calendar"
        Me.Button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.Image = Global.MDIWindowManager_Test.My.Resources.Resources.Books_2
        Me.Button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button2.Location = New System.Drawing.Point(8, 62)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(104, 40)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "Contacts"
        Me.Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Image = Global.MDIWindowManager_Test.My.Resources.Resources.LETTER
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(8, 22)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(104, 40)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Inbox"
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ToolBoxSplitter
        '
        Me.ToolBoxSplitter.Location = New System.Drawing.Point(100, 28)
        Me.ToolBoxSplitter.Name = "ToolBoxSplitter"
        Me.ToolBoxSplitter.Size = New System.Drawing.Size(4, 429)
        Me.ToolBoxSplitter.TabIndex = 18
        Me.ToolBoxSplitter.TabStop = False
        Me.ToolTip1.SetToolTip(Me.ToolBoxSplitter, "Test Horizontal Reposition Adjustment")
        '
        'ToolTip1
        '
        Me.ToolTip1.AutomaticDelay = 0
        Me.ToolTip1.AutoPopDelay = 10000
        Me.ToolTip1.InitialDelay = 0
        Me.ToolTip1.ReshowDelay = 0
        '
        'WindowManagerPanel1
        '
        Me.WindowManagerPanel1.AllowUserVerticalRepositioning = False
        Me.WindowManagerPanel1.AutoDetectMdiChildWindows = True
        Me.WindowManagerPanel1.AutoHide = False
        Me.WindowManagerPanel1.ButtonRenderMode = MDIWindowManager.ButtonRenderMode.Professional
        Me.WindowManagerPanel1.DisableCloseAction = False
        Me.WindowManagerPanel1.DisableHTileAction = False
        Me.WindowManagerPanel1.DisablePopoutAction = False
        Me.WindowManagerPanel1.DisableTileAction = False
        Me.WindowManagerPanel1.EnableTabPaintEvent = False
        Me.WindowManagerPanel1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.WindowManagerPanel1.Location = New System.Drawing.Point(106, 30)
        Me.WindowManagerPanel1.MinMode = False
        Me.WindowManagerPanel1.Name = "WindowManagerPanel1"
        Me.WindowManagerPanel1.Orientation = MDIWindowManager.WindowManagerOrientation.Top
        Me.WindowManagerPanel1.ShowCloseButton = True
        Me.WindowManagerPanel1.ShowIcons = True
        Me.WindowManagerPanel1.ShowLayoutButtons = False
        Me.WindowManagerPanel1.ShowTitle = True
        Me.WindowManagerPanel1.Size = New System.Drawing.Size(589, 42)
        Me.WindowManagerPanel1.Style = MDIWindowManager.TabStyle.AngledHiliteTabs
        Me.WindowManagerPanel1.TabIndex = 32
        Me.WindowManagerPanel1.TabRenderMode = MDIWindowManager.TabsProvider.Standard
        Me.WindowManagerPanel1.Text = "Open Items"
        Me.WindowManagerPanel1.TitleBackColor = System.Drawing.SystemColors.ControlDark
        Me.WindowManagerPanel1.TitleForeColor = System.Drawing.SystemColors.ControlLightLight
        '
        'MainFormAdvanced
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(697, 473)
        Me.Controls.Add(Me.WindowManagerPanel1)
        Me.Controls.Add(Me.ToolBoxSplitter)
        Me.Controls.Add(Me.ToolBoxPanel)
        Me.Controls.Add(Me.StatusBar1)
        Me.Controls.Add(Me.ToolBar1)
        Me.IsMdiContainer = True
        Me.KeyPreview = True
        Me.Menu = Me.MainMenu1
        Me.Name = "MainFormAdvanced"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MDIWindowManager Sample (Advanced)"
        Me.ToolBoxPanel.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub MainForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.ToolBoxPanel.Visible = False

        InitializeSampleView()

    End Sub

    Private Sub InitializeSampleView()

        If m_useWindowManager Then
            InitializeUsingWindowManager()
        Else
            InitializeUsingClassicMDI()
        End If

    End Sub

    Private Sub InitializeUsingWindowManager()

        'enable WindowManagerPanel
        Me.WindowManagerPanel1.AutoDetectMdiChildWindows = True
        Me.WindowManagerPanel1.Visible = True

        'show our Window Menu (see WindowMenuItem_Popup for more details)
        Me.WindowMenuItem.Visible = True
        'hide the Classic MDI Window Menu if it's visible.
        Me.ClassicMdiWindowMenuItem.Visible = False

        'set up the look and feel of our sample
        SetupWindowManagerProperties(SampleViewMode.AdvancedBottom)

        LoadStartPage()
        LoadSampleWindows()

        'set focus on the first window
        'you can also simply use Form.Show/Form.BringToFront, but this lessens flicker
        Me.WindowManagerPanel1.SetActiveWindow(0)

    End Sub

    Private Sub InitializeUsingClassicMDI()

        'disable WindowManagerPanel
        Me.WindowManagerPanel1.AutoDetectMdiChildWindows = False
        Me.WindowManagerPanel1.Visible = False

        'hide our special Window Menu
        Me.WindowMenuItem.Visible = False
        'show the Classic MDI Window Menu
        Me.ClassicMdiWindowMenuItem.Visible = True

        'set up the look and feel of our sample
        SetupWindowManagerProperties(SampleViewMode.Simple)

        LoadStartPage()
        LoadSampleWindows()
        LoadAuxWindow()

        Me.LayoutMdi(MdiLayout.Cascade)

        'set focus on the first window
        Me.MdiChildren(0).BringToFront()

    End Sub

    Private Sub CloseAllChildren()

        If m_useWindowManager Then
            Me.WindowManagerPanel1.CloseAllWindows()
        Else
            For Each frm As Form In Me.MdiChildren
                frm.Close()
            Next frm
        End If

    End Sub

    Private Sub AddChildWindow(ByVal frm As Form)

        frm.MdiParent = Me
        frm.Show()

    End Sub

    Private Sub LoadStartPage()

        Dim frm As New ChildFormIntro

        AddChildWindow(frm)

    End Sub

    Private Sub LoadSampleWindows()

        For count As Integer = 1 To 10
            Dim frm As New ChildForm1

            frm.Text = "Window " + CStr(count)
            frm.TextBox1.Text = "I am Form " + CStr(count)

            AddChildWindow(frm)
        Next count

    End Sub

    Private Sub LoadAuxWindow()

        Dim frm As New ChildAuxForm

        AddChildWindow(frm)

    End Sub

    Private Sub DoFileNew()

        AddChildWindow(New ChildForm2)

    End Sub

    Private Sub DoFileOpen()

        Dim dlg As New OpenFileDialog

        dlg.Filter = "Text Files|*.txt|All Files|*.*"

        If dlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Dim frm As New ChildForm2

            AddChildWindow(frm)

            Dim sr As New System.IO.StreamReader(dlg.FileName, System.Text.Encoding.Default)

            frm.TextBox1.Text = sr.ReadToEnd()

            sr.Close()
        End If

    End Sub

    Private Sub FileNewMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FileNewMenuItem.Click

        DoFileNew()

    End Sub

    Private Sub FileOpenMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FileOpenMenuItem.Click

        DoFileOpen()

    End Sub

    Private Sub ToolBar1_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ToolBar1.ButtonClick

        If e.Button Is FileNewTButton Then
            DoFileNew()
        ElseIf e.Button Is FileOpenTButton Then
            DoFileOpen()
        End If

    End Sub

    Private Sub FileExitMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FileExitMenuItem.Click

        Me.Close()

    End Sub

    Private Sub WindowMenuItem_Popup(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WindowMenuItem.Popup

        'We can use the features of WindowManager to load up
        'a list of all the windows.

        'remove all the old items from the menu
        For index As Integer = Me.WindowMenuItem.MenuItems.Count - 1 To 0 Step -1
            Dim mnu As MenuItem = Me.WindowMenuItem.MenuItems.Item(index)

            If TypeOf mnu Is MDIWindowManager.WrappedWindowMenuItem Then
                Me.WindowMenuItem.MenuItems.Remove(mnu)
            End If
        Next index

        'get the first 9 window items and add them to the menu
        Dim menuItems As MenuItem() = Me.WindowManagerPanel1.GetAllWindowsMenu(9, True)

        If Not menuItems Is Nothing AndAlso menuItems.Length > 0 Then
            Me.WindowMenuItem.MenuItems.AddRange(menuItems)
        End If

        'ensure the "more windows" menu item is positioned at the bottom of the menu
        Me.WindowMoreWindowsMenuItem.Index = Me.WindowMenuItem.MenuItems.Count - 1

    End Sub

    Private Sub WindowHTileMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WindowHTileMenuItem.Click

        Me.WindowManagerPanel1.HTileWrappedWindow(Me.WindowManagerPanel1.GetActiveWindow())

    End Sub

    Private Sub WindowTileMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WindowTileMenuItem.Click

        Me.WindowManagerPanel1.TileOrUntileWrappedWindow(Me.WindowManagerPanel1.GetActiveWindow())

    End Sub

    Private Sub WindowPopOutMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WindowPopOutMenuItem.Click

        Me.WindowManagerPanel1.PopOutWrappedWindow(Me.WindowManagerPanel1.GetActiveWindow())

    End Sub

    Private Sub WindowCloseAllMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WindowCloseAllMenuItem.Click

        CloseAllChildren()

    End Sub

    Private Sub WindowMoreWindowsMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WindowMoreWindowsMenuItem.Click

        Me.WindowManagerPanel1.ShowAllWindowsDialog()

    End Sub

    Private Sub ClassicMdiWindowCascadeMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClassicMdiWindowCascadeMenuItem.Click

        LayoutMdi(MdiLayout.Cascade)

    End Sub

    Private Sub ClassicMdiWindowTileHorizMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClassicMdiWindowTileHorizMenuItem.Click

        LayoutMdi(MdiLayout.TileHorizontal)

    End Sub

    Private Sub ClassicMdiWindowTileVertMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClassicMdiWindowTileVertMenuItem.Click

        LayoutMdi(MdiLayout.TileVertical)

    End Sub

    Private Sub ViewSimpleMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewSimpleMenuItem.Click

        SetupWindowManagerProperties(SampleViewMode.Simple)

    End Sub

    Private Sub ViewAdvBottomMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewAdvBottomMenuItem.Click

        SetupWindowManagerProperties(SampleViewMode.AdvancedBottom)

    End Sub

    Private Sub ViewAdvRightMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewAdvRightMenuItem.Click

        SetupWindowManagerProperties(SampleViewMode.AdvancedRight)

    End Sub

    Private Sub ViewAdvLeftMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewAdvLeftMenuItem.Click

        SetupWindowManagerProperties(SampleViewMode.AdvancedLeft)

    End Sub

    Private Sub ViewShowTitleMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewShowTitleMenuItem.Click

        Me.WindowManagerPanel1.ShowTitle = Not Me.WindowManagerPanel1.ShowTitle

        If Me.WindowManagerPanel1.ShowTitle Then
            Me.WindowManagerPanel1.Height = 42
        Else
            Me.WindowManagerPanel1.Height = 26
        End If

    End Sub

    Private Sub ViewShowLayoutButtons_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewShowLayoutButtonsMenuItem.Click

        Me.WindowManagerPanel1.ShowLayoutButtons = Not Me.WindowManagerPanel1.ShowLayoutButtons

    End Sub

    Private Sub ViewShowCloseButtonMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewShowCloseButtonMenuItem.Click

        Me.WindowManagerPanel1.ShowCloseButton = Not Me.WindowManagerPanel1.ShowCloseButton

    End Sub

    Private Sub ViewShowIconsMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewShowIconsMenuItem.Click

        Me.WindowManagerPanel1.ShowIcons = Not Me.WindowManagerPanel1.ShowIcons

    End Sub

    Private Sub ViewTabStylesClassicMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewTabStylesClassicMenuItem.Click

        Me.WindowManagerPanel1.Style = MDIWindowManager.TabStyle.ClassicTabs

    End Sub

    Private Sub ViewTabStylesModernMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewTabStylesModernMenuItem.Click

        Me.WindowManagerPanel1.Style = MDIWindowManager.TabStyle.ModernTabs

    End Sub

    Private Sub ViewTabStylesFlatHiliteMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewTabStylesFlatHiliteMenuItem.Click

        Me.WindowManagerPanel1.Style = MDIWindowManager.TabStyle.FlatHiliteTabs

    End Sub

    Private Sub ViewTabStylesAngledHiliteMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewTabStylesAngledHiliteMenuItem.Click

        Me.WindowManagerPanel1.Style = MDIWindowManager.TabStyle.AngledHiliteTabs

    End Sub

    Private Sub ViewButtonStylesStandardMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewButtonStylesStandardMenuItem.Click

        Me.WindowManagerPanel1.ButtonRenderMode = MDIWindowManager.ButtonRenderMode.Standard

    End Sub

    Private Sub ViewButtonStylesSystemMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewButtonStylesSystemMenuItem.Click

        Me.WindowManagerPanel1.ButtonRenderMode = MDIWindowManager.ButtonRenderMode.System

    End Sub

    Private Sub ViewButtonStylesProMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewButtonStylesProMenuItem.Click

        Me.WindowManagerPanel1.ButtonRenderMode = MDIWindowManager.ButtonRenderMode.Professional

    End Sub

    Private Sub ShowToolboxMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowToolboxMenuItem.Click

        Me.ToolBoxPanel.Visible = Not Me.ToolBoxPanel.Visible
        Me.ToolBoxSplitter.Visible = Me.ToolBoxPanel.Visible

    End Sub

    Private Sub SwitchToClassicMdiMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SwitchToClassicMdiMenuItem.Click

        CloseAllChildren()
        m_useWindowManager = Not m_useWindowManager
        InitializeSampleView()

    End Sub

    Private Sub ViewMenuItem_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewMenuItem.Popup

        Me.ViewSimpleMenuItem.Enabled = m_useWindowManager
        Me.ViewAdvRightMenuItem.Enabled = m_useWindowManager
        Me.ViewAdvBottomMenuItem.Enabled = m_useWindowManager
        Me.ViewAdvLeftMenuItem.Enabled = m_useWindowManager
        Me.ViewAppearanceMenuItem.Enabled = m_useWindowManager

        Me.ViewSimpleMenuItem.Checked = False
        Me.ViewAdvRightMenuItem.Checked = False
        Me.ViewAdvBottomMenuItem.Checked = False
        Me.ViewAdvLeftMenuItem.Checked = False

        Select Case m_viewMode
            Case SampleViewMode.Simple
                Me.ViewSimpleMenuItem.Checked = True
            Case SampleViewMode.AdvancedRight
                Me.ViewAdvRightMenuItem.Checked = True
            Case SampleViewMode.AdvancedBottom
                Me.ViewAdvBottomMenuItem.Checked = True
            Case SampleViewMode.AdvancedLeft
                Me.ViewAdvLeftMenuItem.Checked = True
        End Select

        Me.ViewShowTitleMenuItem.Checked = Me.WindowManagerPanel1.ShowTitle
        Me.ViewShowIconsMenuItem.Checked = Me.WindowManagerPanel1.ShowIcons
        Me.ViewShowLayoutButtonsMenuItem.Checked = Me.WindowManagerPanel1.ShowLayoutButtons
        Me.ViewShowCloseButtonMenuItem.Checked = Me.WindowManagerPanel1.ShowCloseButton
        Me.ShowToolboxMenuItem.Checked = Me.ToolBoxPanel.Visible

        Me.ViewTabStylesClassicMenuItem.Checked = False
        Me.ViewTabStylesModernMenuItem.Checked = False
        Me.ViewTabStylesFlatHiliteMenuItem.Checked = False
        Me.ViewTabStylesAngledHiliteMenuItem.Checked = False

        Select Case Me.WindowManagerPanel1.Style
            Case MDIWindowManager.TabStyle.ClassicTabs
                Me.ViewTabStylesClassicMenuItem.Checked = True
            Case MDIWindowManager.TabStyle.ModernTabs
                Me.ViewTabStylesModernMenuItem.Checked = True
            Case MDIWindowManager.TabStyle.FlatHiliteTabs
                Me.ViewTabStylesFlatHiliteMenuItem.Checked = True
            Case MDIWindowManager.TabStyle.AngledHiliteTabs
                Me.ViewTabStylesAngledHiliteMenuItem.Checked = True
        End Select

        Me.ViewButtonStylesStandardMenuItem.Checked = False
        Me.ViewButtonStylesSystemMenuItem.Checked = False
        Me.ViewButtonStylesProMenuItem.Checked = False

        Select Case Me.WindowManagerPanel1.ButtonRenderMode
            Case MDIWindowManager.ButtonRenderMode.Standard
                Me.ViewButtonStylesStandardMenuItem.Checked = True
            Case MDIWindowManager.ButtonRenderMode.System
                Me.ViewButtonStylesSystemMenuItem.Checked = True
            Case MDIWindowManager.ButtonRenderMode.Professional
                Me.ViewButtonStylesProMenuItem.Checked = True
        End Select

        If m_useWindowManager Then
            SwitchToClassicMdiMenuItem.Text = "Switch to Classic MDI"
        Else
            SwitchToClassicMdiMenuItem.Text = "Switch to MDIWindowManager"
        End If

    End Sub

    Private Sub SetupWindowManagerProperties(ByVal viewMode As SampleViewMode)

        Select Case viewMode
            Case SampleViewMode.Simple
                Me.ToolBoxPanel.Width = 100

                'get rid of the docked aux window... see other Cases below for more info
                If Not Me.WindowManagerPanel1.AuxiliaryWindow Is Nothing Then
                    Me.WindowManagerPanel1.AuxiliaryWindow.Close()
                    Me.WindowManagerPanel1.AuxiliaryWindow = Nothing
                End If

                With Me.WindowManagerPanel1
                    .Orientation = MDIWindowManager.WindowManagerOrientation.Top

                    'the following properties are normally set at design-time, but since
                    'we're showing different samples let's do the work here
                    .AllowUserVerticalRepositioning = False
                    .Top = .GetMDIClientAreaBounds.Top
                    .ShowTitle = False
                    .Height = 26
                    .AutoHide = True
                End With
            Case SampleViewMode.AdvancedBottom, SampleViewMode.AdvancedRight, SampleViewMode.AdvancedLeft
                Me.ToolBoxPanel.Width = 100

                With Me.WindowManagerPanel1
                    'these properties are normally set at design-time, but since
                    'we're showing different samples let's do the work here
                    .ShowTitle = True
                    .Height = 42
                    .AutoHide = False

                    Select Case viewMode
                        Case SampleViewMode.AdvancedBottom
                            .Orientation = MDIWindowManager.WindowManagerOrientation.Bottom
                            .AllowUserVerticalRepositioning = True
                            .Top = 200
                        Case SampleViewMode.AdvancedRight
                            .Orientation = MDIWindowManager.WindowManagerOrientation.Right
                            .AllowUserVerticalRepositioning = False
                            .Top = .GetMDIClientAreaBounds.Top
                            .Width = 450
                        Case SampleViewMode.AdvancedLeft
                            .Orientation = MDIWindowManager.WindowManagerOrientation.Left
                            .AllowUserVerticalRepositioning = False
                            .Top = .GetMDIClientAreaBounds.Top
                            .Width = 450
                    End Select

                    'set up an "aux" window that will be managed by WindowManagerPanel, but is
                    'not shown in the Tabs... instead it is docked as a sort of appendage in order
                    'to achieve the look and feel of 2 and 3-pane apps like Outlook.
                    If Me.WindowManagerPanel1.AuxiliaryWindow Is Nothing Then
                        Dim frm As New ChildAuxForm

                        frm.FormBorderStyle = Windows.Forms.FormBorderStyle.None

                        Me.WindowManagerPanel1.AuxiliaryWindow = frm

                        frm.Show()
                    End If

                    If viewMode = SampleViewMode.AdvancedBottom Then
                        .AutoHide = True
                    Else
                        .AutoHide = False
                    End If
                End With
        End Select

        m_viewMode = viewMode

    End Sub

    Private Sub HelpTopicsMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpTopicsMenuItem.Click

        Process.Start("http://www.cflashsoft.com/progs/mdiwinman/")

    End Sub

    Private Sub HelpAboutMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HelpAboutMenuItem.Click

        MsgBox("Control: " & System.Reflection.Assembly.GetAssembly(Me.WindowManagerPanel1.GetType()).GetName().FullName _
            & vbCrLf & "Test App: " & System.Reflection.Assembly.GetExecutingAssembly().FullName)

    End Sub

    Private Sub ViewTabStylesMoreInfoMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewTabStylesMoreInfoMenuItem.Click

        MsgBox("These are the default tab styles provided by WindowManagerPanel. You can customize or completely handle how tabs are drawn via the <<TabPaint>> event (see Custom Paint Sample)." & vbCrLf & vbCrLf _
            & "Additionally, you can also create totally custom 'TabProviders.' MDIWindowManager currently contains an additional TabProvider called SystemTabsProvider, which uses the standard .NET TabControl to present the tabs (see Alternate Tabs Sample). " _
            & "For convenience, you can accomplish the same shown by the Alternate Tabs Sample by using TabRenderMode property in the designer." & vbCrLf & vbCrLf _
            , MsgBoxStyle.Information)

    End Sub

End Class