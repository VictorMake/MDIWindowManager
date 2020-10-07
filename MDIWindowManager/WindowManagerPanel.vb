Imports System.ComponentModel
Imports System.Collections.Generic
Imports MathematicalLibrary

''' <summary>
''' Provides Tabbed MDI management for MDI Parent windows.
''' </summary>
''' <remarks></remarks>
<ToolboxItemAttribute(True),
Description("Provides Tabbed MDI management for MDI Parent windows."),
ToolboxBitmap(GetType(WindowManagerPanel))>
Public Class WindowManagerPanel
    Inherits UserControl

#Region "Events"

    Private Declare Function PostMessage Lib "user32" Alias "PostMessageA" (ByVal hwnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As String) As Integer

    Public Event WindowTiling As EventHandler(Of WrappedWindowCancelEventArgs)
    Public Event WindowUnTiling As EventHandler(Of WrappedWindowCancelEventArgs)
    Public Event WindowHTiling As EventHandler(Of WrappedWindowCancelEventArgs)
    Public Event WindowPoppingOut As EventHandler(Of WrappedWindowCancelEventArgs)
    Public Event WindowPoppingIn As EventHandler(Of WrappedWindowCancelEventArgs)

    Public Event BeforeWrappedWindowAdded As EventHandler(Of WrappedWindowCancelEventArgs)
    Public Event WrappedWindowAdded As EventHandler(Of WrappedWindowEventArgs)
    Public Event BeforeWrappedWindowRemoved As EventHandler(Of WrappedWindowCancelEventArgs)
    Public Event WrappedWindowRemoved As EventHandler(Of WrappedWindowEventArgs)
    Public Event WrappedWindowsCleared As EventHandler
    Public Event WindowClosing As EventHandler(Of WrappedWindowClosingEventArgs)
    Public Event WindowClosed As EventHandler(Of WrappedWindowClosedEventArgs)
    Public Event WindowActivated As EventHandler(Of WrappedWindowEventArgs)
    Public Event WindowDeactivate As EventHandler(Of WrappedWindowEventArgs)
    Public Event WindowEnter As EventHandler(Of WrappedWindowEventArgs)
    Public Event WindowLeave As EventHandler(Of WrappedWindowEventArgs)
    Public Event WindowTextChanged As EventHandler(Of WrappedWindowEventArgs)
    Public Event WindowVisibleChanged As EventHandler(Of WrappedWindowEventArgs)

    Public Event NewCustomTabsProviderInstance As EventHandler(Of NewTabsProviderInstanceCreatedEventArgs)
    Public Event TabPaint As EventHandler(Of StandardTabsProvider.TabPaintEventArgs)

    Friend Event TempPanelDismissed As EventHandler
    Friend Event RequestChainRedoLayout As EventHandler(Of RedoLayoutEventArgs)
    Friend Event RequestChainGetTop As EventHandler(Of GetTopEventArgs)
    Friend Event RequestChainGetTopPanel As EventHandler(Of GetTopPanelEventArgs)
    Friend Event RequestMinModeChange As EventHandler(Of RequestMinModeChangeEventArgs)
    Friend Event IsActiveChanged As EventHandler

#End Region

#Region "Private Variables"

    Private m_parentForm As Form
    Private m_isLoaded As Boolean = False
    Private m_isUnloaded As Boolean = False
    Private m_titleText As String = String.Empty
    Private m_titleBackColor As Color = SystemColors.ControlDark
    Private m_titleForeColor As Color = SystemColors.ControlLightLight
    Private m_style As TabStyle = TabStyle.ClassicTabs
    Private m_showIcons As Boolean = True
    Private m_orientation As WindowManagerOrientation = WindowManagerOrientation.Top
    Private m_windowLayoutButtonsVisible As Boolean = True
    Private m_windowCloseButtonVisible As Boolean = True
    Private m_allowUserVerticalRepositioning As Boolean = False
    Private m_autoHide As Boolean = False
    Private m_inAutoHideMode As Boolean = False
    Private m_sizeValueBeforeAutoHide As Integer
    Private m_minMode As Boolean = False
    Private m_topValueBeforeMinMode As Integer
    Private m_tabRenderMode As TabsProvider = TabsProvider.Standard
    Private m_buttonRenderMode As ButtonRenderMode = ButtonRenderMode.Standard
    Private m_customTabsProviderType As Type
    Private m_enableTabPaintEvent As Boolean = False
    Private m_disableCloseAction As Boolean
    Private m_disableTileAction As Boolean
    Private m_disableHTileAction As Boolean
    Private m_disablePopoutAction As Boolean

    Private WithEvents m_windowTabStrips As New WindowTabStripCollection
    Private m_suppressTabStripRemoveAbort As Boolean = False

    Private WithEvents m_poppedOutWindowsManager As PoppedOutWindowsManager = g_poppedOutWindowsManager

    Private WithEvents m_mdiClientControl As MdiClient
    Private m_originalMDIClientRect As Rectangle

    Private WithEvents m_blankAreaLeft As DummyForm
    Private WithEvents m_blankAreaRight As DummyForm
    Private WithEvents m_minModeBar As WindowManagerPanelMinModeBar

    Private WithEvents m_auxiliaryWindow As Form

    Private m_isActive As Boolean = False
    Private m_selectedTabStrip As WindowTabStrip

    Private m_autoDetectMdiChildWindows As Boolean = True

    'Private m_tmpOriginalBackColorCache1 As Color
    'Private m_tmpOriginalForeColorCache1 As Color
    'Private m_tmpOriginalBackColorCache2 As Color
    'Private m_tmpOriginalForeColorCache2 As Color

    Private WithEvents m_nextWindowManagerPanel As WindowManagerPanel
    Friend WithEvents FocusIndicatorPanel As Panel
    Private m_isTemporaryWindowManagerPanel As Boolean

    Private m_historyBack As New List(Of WrappedWindow)
    'Private m_historyForward As New List(Of WrappedWindow)

#If DEBUG Then
    Private m_showFocusIndicator As Boolean = False ' for debugging purposes... might be exposed as a feature someday
#Else
    Private m_showFocusIndicator As Boolean = False
#End If

#End Region

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        InitializePrimaryTabStrip()

    End Sub

    'UserControl1 overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            HideToolTip(Me)
            RemoveParentHandlers()
            m_poppedOutWindowsManager = Nothing
            m_nextWindowManagerPanel = Nothing
            m_windowTabStrips = Nothing
            m_mdiClientControl = Nothing
            m_parentForm = Nothing

            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents CloseWindowButton As WindowManagerButton
    Friend WithEvents PopoutWindowButton As WindowManagerButton
    Friend WithEvents TileWindowButton As WindowManagerButton
    Friend WithEvents TabStripsContainer As Panel
    Friend WithEvents WindowManagerSplitter1 As MDIWindowManager.WindowManagerSplitter
    Friend WithEvents HTileButton As WindowManagerButton
    Friend WithEvents WindowManagerSplitter2 As MDIWindowManager.WindowManagerSplitter
    Friend WithEvents TitleLabelMenuGlyph As Label
    Friend WithEvents TitleLabel As Label
    Friend WithEvents WindowButtonsPanel As Panel
    Friend WithEvents TitlePanel As Panel
    Friend WithEvents WindowButtonsContextMenu As ContextMenu
    Friend WithEvents WindowButtonsToggleWindowButtonsMenuItem As MenuItem
    Friend WithEvents TitleContextMenu As ContextMenu
    Friend WithEvents WindowContextMenu As ContextMenu
    Friend WithEvents WindowHTileMenuItem As MenuItem
    Friend WithEvents WindowTileWindowMenuItem As MenuItem
    Friend WithEvents WindowPopoutWindowMenuItem As MenuItem
    Friend WithEvents WindowMenuSep1 As MenuItem
    Friend WithEvents WindowCloseMenuItem As MenuItem
    Friend WithEvents TitleOptionsHTileMenuItem As MenuItem
    Friend WithEvents TitleOptionsTileWindowMenuItem As MenuItem
    Friend WithEvents TitleOptionsPopoutWindowMenuItem As MenuItem
    Friend WithEvents TitleCloseWindowMenuItem As MenuItem
    Friend WithEvents TitleMenuSep2 As MenuItem
    Friend WithEvents TitleMenuSep3 As MenuItem
    Friend WithEvents TitleNoWindowsMenuItem As MenuItem
    Friend WithEvents TitleMinimizeMenuItem As MenuItem
    Friend WithEvents TitleMenuSep1 As MenuItem
    Friend WithEvents DesignerNote1Label As Label
    Friend WithEvents TitleLabelBackground As Label

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WindowManagerPanel))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.DesignerNote1Label = New System.Windows.Forms.Label
        Me.HTileButton = New MDIWindowManager.WindowManagerButton
        Me.TileWindowButton = New MDIWindowManager.WindowManagerButton
        Me.PopoutWindowButton = New MDIWindowManager.WindowManagerButton
        Me.CloseWindowButton = New MDIWindowManager.WindowManagerButton
        Me.TitleLabel = New System.Windows.Forms.Label
        Me.TabStripsContainer = New System.Windows.Forms.Panel
        Me.TitleContextMenu = New System.Windows.Forms.ContextMenu
        Me.TitleMinimizeMenuItem = New System.Windows.Forms.MenuItem
        Me.TitleMenuSep1 = New System.Windows.Forms.MenuItem
        Me.TitleOptionsHTileMenuItem = New System.Windows.Forms.MenuItem
        Me.TitleOptionsTileWindowMenuItem = New System.Windows.Forms.MenuItem
        Me.TitleOptionsPopoutWindowMenuItem = New System.Windows.Forms.MenuItem
        Me.TitleMenuSep2 = New System.Windows.Forms.MenuItem
        Me.TitleCloseWindowMenuItem = New System.Windows.Forms.MenuItem
        Me.TitleMenuSep3 = New System.Windows.Forms.MenuItem
        Me.TitleNoWindowsMenuItem = New System.Windows.Forms.MenuItem
        Me.TitleLabelMenuGlyph = New System.Windows.Forms.Label
        Me.WindowButtonsContextMenu = New System.Windows.Forms.ContextMenu
        Me.WindowButtonsToggleWindowButtonsMenuItem = New System.Windows.Forms.MenuItem
        Me.WindowContextMenu = New System.Windows.Forms.ContextMenu
        Me.WindowHTileMenuItem = New System.Windows.Forms.MenuItem
        Me.WindowTileWindowMenuItem = New System.Windows.Forms.MenuItem
        Me.WindowPopoutWindowMenuItem = New System.Windows.Forms.MenuItem
        Me.WindowMenuSep1 = New System.Windows.Forms.MenuItem
        Me.WindowCloseMenuItem = New System.Windows.Forms.MenuItem
        Me.WindowButtonsPanel = New System.Windows.Forms.Panel
        Me.TitlePanel = New System.Windows.Forms.Panel
        Me.TitleLabelBackground = New System.Windows.Forms.Label
        Me.FocusIndicatorPanel = New System.Windows.Forms.Panel
        Me.WindowManagerSplitter2 = New MDIWindowManager.WindowManagerSplitter
        Me.WindowManagerSplitter1 = New MDIWindowManager.WindowManagerSplitter
        Me.TabStripsContainer.SuspendLayout()
        Me.WindowButtonsPanel.SuspendLayout()
        Me.TitlePanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'DesignerNote1Label
        '
        Me.DesignerNote1Label.AutoSize = True
        Me.DesignerNote1Label.Location = New System.Drawing.Point(3, 1)
        Me.DesignerNote1Label.Name = "DesignerNote1Label"
        Me.DesignerNote1Label.Size = New System.Drawing.Size(336, 13)
        Me.DesignerNote1Label.TabIndex = 0
        Me.DesignerNote1Label.Text = "Заметка: Актуальная позиция может быть изменена при выполнении (смотри Orientation property)"
        Me.ToolTip1.SetToolTip(Me.DesignerNote1Label, "Заметка: Актуальная позиция может быть изменена при выполнении (смотри Orientation property)")
        Me.DesignerNote1Label.Visible = False
        '
        'HTileButton
        '
        Me.HTileButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HTileButton.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.HTileButton.Font = New System.Drawing.Font("Marlett", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.HTileButton.Image = CType(resources.GetObject("HTileButton.Image"), System.Drawing.Image)
        Me.HTileButton.Location = New System.Drawing.Point(1, 0)
        Me.HTileButton.Name = "HTileButton"
        Me.HTileButton.RenderMode = MDIWindowManager.WindowManagerButton.WindowManagerButtonRenderMode.RollOverClassic
        Me.HTileButton.Size = New System.Drawing.Size(20, 18)
        Me.HTileButton.TabIndex = 0
        Me.HTileButton.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ToolTip1.SetToolTip(Me.HTileButton, "Новая Горизонтальная Вкладка Группы")
        '
        'TileWindowButton
        '
        Me.TileWindowButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TileWindowButton.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TileWindowButton.Font = New System.Drawing.Font("Marlett", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.TileWindowButton.Image = CType(resources.GetObject("TileWindowButton.Image"), System.Drawing.Image)
        Me.TileWindowButton.Location = New System.Drawing.Point(21, 0)
        Me.TileWindowButton.Name = "TileWindowButton"
        Me.TileWindowButton.RenderMode = MDIWindowManager.WindowManagerButton.WindowManagerButtonRenderMode.RollOverClassic
        Me.TileWindowButton.Size = New System.Drawing.Size(20, 18)
        Me.TileWindowButton.TabIndex = 1
        Me.TileWindowButton.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ToolTip1.SetToolTip(Me.TileWindowButton, "Добавить или Убрать Мозаичное Окно")
        '
        'PopoutWindowButton
        '
        Me.PopoutWindowButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PopoutWindowButton.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.PopoutWindowButton.Font = New System.Drawing.Font("Marlett", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.PopoutWindowButton.Image = CType(resources.GetObject("PopoutWindowButton.Image"), System.Drawing.Image)
        Me.PopoutWindowButton.Location = New System.Drawing.Point(41, 0)
        Me.PopoutWindowButton.Name = "PopoutWindowButton"
        Me.PopoutWindowButton.RenderMode = MDIWindowManager.WindowManagerButton.WindowManagerButtonRenderMode.RollOverClassic
        Me.PopoutWindowButton.Size = New System.Drawing.Size(20, 18)
        Me.PopoutWindowButton.TabIndex = 2
        Me.PopoutWindowButton.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ToolTip1.SetToolTip(Me.PopoutWindowButton, "Извлечь Текущее Окно")
        '
        'CloseWindowButton
        '
        Me.CloseWindowButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CloseWindowButton.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.CloseWindowButton.Font = New System.Drawing.Font("Marlett", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.CloseWindowButton.Image = CType(resources.GetObject("CloseWindowButton.Image"), System.Drawing.Image)
        Me.CloseWindowButton.Location = New System.Drawing.Point(61, 0)
        Me.CloseWindowButton.Name = "CloseWindowButton"
        Me.CloseWindowButton.RenderMode = MDIWindowManager.WindowManagerButton.WindowManagerButtonRenderMode.RollOverClassic
        Me.CloseWindowButton.Size = New System.Drawing.Size(32, 18)
        Me.CloseWindowButton.TabIndex = 3
        Me.CloseWindowButton.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ToolTip1.SetToolTip(Me.CloseWindowButton, "Закрыть Текущее Окно")
        '
        'TitleLabel
        '
        Me.TitleLabel.AutoSize = True
        Me.TitleLabel.BackColor = System.Drawing.SystemColors.ControlDark
        Me.TitleLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.TitleLabel.Location = New System.Drawing.Point(0, 0)
        Me.TitleLabel.Name = "TitleLabel"
        Me.TitleLabel.Size = New System.Drawing.Size(0, 13)
        Me.TitleLabel.TabIndex = 8
        '
        'TabStripsContainer
        '
        Me.TabStripsContainer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabStripsContainer.Controls.Add(Me.DesignerNote1Label)
        Me.TabStripsContainer.Location = New System.Drawing.Point(4, 20)
        Me.TabStripsContainer.Name = "TabStripsContainer"
        Me.TabStripsContainer.Size = New System.Drawing.Size(340, 23)
        Me.TabStripsContainer.TabIndex = 9
        '
        'TitleContextMenu
        '
        Me.TitleContextMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.TitleMinimizeMenuItem, Me.TitleMenuSep1, Me.TitleOptionsHTileMenuItem, Me.TitleOptionsTileWindowMenuItem, Me.TitleOptionsPopoutWindowMenuItem, Me.TitleMenuSep2, Me.TitleCloseWindowMenuItem, Me.TitleMenuSep3, Me.TitleNoWindowsMenuItem})
        '
        'TitleMinimizeMenuItem
        '
        Me.TitleMinimizeMenuItem.Index = 0
        Me.TitleMinimizeMenuItem.Text = "Ми&нимизировать"
        '
        'TitleMenuSep1
        '
        Me.TitleMenuSep1.Index = 1
        Me.TitleMenuSep1.Text = "-"
        '
        'TitleOptionsHTileMenuItem
        '
        Me.TitleOptionsHTileMenuItem.Index = 2
        Me.TitleOptionsHTileMenuItem.Text = "Новая &Горизонтальная Вкладка Группы"
        '
        'TitleOptionsTileWindowMenuItem
        '
        Me.TitleOptionsTileWindowMenuItem.Index = 3
        Me.TitleOptionsTileWindowMenuItem.Text = "&Добавить или Убрать Мозаичное Окно"
        '
        'TitleOptionsPopoutWindowMenuItem
        '
        Me.TitleOptionsPopoutWindowMenuItem.Index = 4
        Me.TitleOptionsPopoutWindowMenuItem.Text = "&Извлечь Текущее Окно"
        '
        'TitleMenuSep2
        '
        Me.TitleMenuSep2.Index = 5
        Me.TitleMenuSep2.Text = "-"
        '
        'TitleCloseWindowMenuItem
        '
        Me.TitleCloseWindowMenuItem.Index = 6
        Me.TitleCloseWindowMenuItem.Text = "&Закрыть Текущее Окно"
        '
        'TitleMenuSep3
        '
        Me.TitleMenuSep3.Index = 7
        Me.TitleMenuSep3.Text = "-"
        '
        'TitleNoWindowsMenuItem
        '
        Me.TitleNoWindowsMenuItem.Enabled = False
        Me.TitleNoWindowsMenuItem.Index = 8
        Me.TitleNoWindowsMenuItem.Text = "(Нет Окон)"
        '
        'TitleLabelMenuGlyph
        '
        Me.TitleLabelMenuGlyph.BackColor = System.Drawing.SystemColors.ControlDark
        Me.TitleLabelMenuGlyph.Font = New System.Drawing.Font("Marlett", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.TitleLabelMenuGlyph.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.TitleLabelMenuGlyph.Location = New System.Drawing.Point(0, 0)
        Me.TitleLabelMenuGlyph.Name = "TitleLabelMenuGlyph"
        Me.TitleLabelMenuGlyph.Size = New System.Drawing.Size(16, 16)
        Me.TitleLabelMenuGlyph.TabIndex = 13
        Me.TitleLabelMenuGlyph.Text = "u"
        '
        'WindowButtonsContextMenu
        '
        Me.WindowButtonsContextMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.WindowButtonsToggleWindowButtonsMenuItem})
        '
        'WindowButtonsToggleWindowButtonsMenuItem
        '
        Me.WindowButtonsToggleWindowButtonsMenuItem.Index = 0
        Me.WindowButtonsToggleWindowButtonsMenuItem.Text = "Скрыть &Кнопки Окон"
        '
        'WindowContextMenu
        '
        Me.WindowContextMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.WindowHTileMenuItem, Me.WindowTileWindowMenuItem, Me.WindowPopoutWindowMenuItem, Me.WindowMenuSep1, Me.WindowCloseMenuItem})
        '
        'WindowHTileMenuItem
        '
        Me.WindowHTileMenuItem.Index = 0
        Me.WindowHTileMenuItem.Text = "Новая &Горизонтальная Вкладка Группы"
        '
        'WindowTileWindowMenuItem
        '
        Me.WindowTileWindowMenuItem.Index = 1
        Me.WindowTileWindowMenuItem.Text = "&Добавить или Убрать Мозаику"
        '
        'WindowPopoutWindowMenuItem
        '
        Me.WindowPopoutWindowMenuItem.Index = 2
        Me.WindowPopoutWindowMenuItem.Text = "&Извлечь"
        '
        'WindowMenuSep1
        '
        Me.WindowMenuSep1.Index = 3
        Me.WindowMenuSep1.Text = "-"
        '
        'WindowCloseMenuItem
        '
        Me.WindowCloseMenuItem.Index = 4
        Me.WindowCloseMenuItem.Text = "&Закрыть"
        '
        'WindowButtonsPanel
        '
        Me.WindowButtonsPanel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.WindowButtonsPanel.Controls.Add(Me.HTileButton)
        Me.WindowButtonsPanel.Controls.Add(Me.TileWindowButton)
        Me.WindowButtonsPanel.Controls.Add(Me.PopoutWindowButton)
        Me.WindowButtonsPanel.Controls.Add(Me.CloseWindowButton)
        Me.WindowButtonsPanel.Location = New System.Drawing.Point(346, 20)
        Me.WindowButtonsPanel.Name = "WindowButtonsPanel"
        Me.WindowButtonsPanel.Size = New System.Drawing.Size(93, 23)
        Me.WindowButtonsPanel.TabIndex = 14
        '
        'TitlePanel
        '
        Me.TitlePanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TitlePanel.BackColor = System.Drawing.SystemColors.ControlDark
        Me.TitlePanel.Controls.Add(Me.TitleLabelMenuGlyph)
        Me.TitlePanel.Controls.Add(Me.TitleLabel)
        Me.TitlePanel.Controls.Add(Me.TitleLabelBackground)
        Me.TitlePanel.Location = New System.Drawing.Point(4, 4)
        Me.TitlePanel.Name = "TitlePanel"
        Me.TitlePanel.Size = New System.Drawing.Size(436, 15)
        Me.TitlePanel.TabIndex = 15
        '
        'TitleLabelBackground
        '
        Me.TitleLabelBackground.BackColor = System.Drawing.SystemColors.ControlDark
        Me.TitleLabelBackground.Font = New System.Drawing.Font("Marlett", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.TitleLabelBackground.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.TitleLabelBackground.Location = New System.Drawing.Point(0, 0)
        Me.TitleLabelBackground.Name = "TitleLabelBackground"
        Me.TitleLabelBackground.Size = New System.Drawing.Size(16, 16)
        Me.TitleLabelBackground.TabIndex = 14
        '
        'FocusIndicatorPanel
        '
        Me.FocusIndicatorPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.FocusIndicatorPanel.Location = New System.Drawing.Point(0, 0)
        Me.FocusIndicatorPanel.Name = "FocusIndicatorPanel"
        Me.FocusIndicatorPanel.Size = New System.Drawing.Size(4, 38)
        Me.FocusIndicatorPanel.TabIndex = 16
        '
        'WindowManagerSplitter2
        '
        Me.WindowManagerSplitter2.BackColor = System.Drawing.SystemColors.Control
        Me.WindowManagerSplitter2.Cursor = System.Windows.Forms.Cursors.VSplit
        Me.WindowManagerSplitter2.Location = New System.Drawing.Point(0, 4)
        Me.WindowManagerSplitter2.Name = "WindowManagerSplitter2"
        Me.WindowManagerSplitter2.Size = New System.Drawing.Size(4, 38)
        Me.WindowManagerSplitter2.Style = MDIWindowManager.WindowManagerSplitter.SplitterStyle.Vertical
        Me.WindowManagerSplitter2.TabIndex = 12
        '
        'WindowManagerSplitter1
        '
        Me.WindowManagerSplitter1.BackColor = System.Drawing.SystemColors.Control
        Me.WindowManagerSplitter1.Cursor = System.Windows.Forms.Cursors.HSplit
        Me.WindowManagerSplitter1.Dock = System.Windows.Forms.DockStyle.Top
        Me.WindowManagerSplitter1.Location = New System.Drawing.Point(0, 0)
        Me.WindowManagerSplitter1.Name = "WindowManagerSplitter1"
        Me.WindowManagerSplitter1.Size = New System.Drawing.Size(444, 4)
        Me.WindowManagerSplitter1.Style = MDIWindowManager.WindowManagerSplitter.SplitterStyle.Horizontal
        Me.WindowManagerSplitter1.TabIndex = 10
        '
        'WindowManagerPanel
        '
        Me.Controls.Add(Me.TitlePanel)
        Me.Controls.Add(Me.WindowManagerSplitter2)
        Me.Controls.Add(Me.WindowManagerSplitter1)
        Me.Controls.Add(Me.TabStripsContainer)
        Me.Controls.Add(Me.WindowButtonsPanel)
        Me.Controls.Add(Me.FocusIndicatorPanel)
        Me.Name = "WindowManagerPanel"
        Me.Size = New System.Drawing.Size(444, 42)
        Me.TabStripsContainer.ResumeLayout(False)
        Me.TabStripsContainer.PerformLayout()
        Me.WindowButtonsPanel.ResumeLayout(False)
        Me.TitlePanel.ResumeLayout(False)
        Me.TitlePanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Properties"

    ''' <summary>
    ''' The text that will appear in the titlebar of the panel.
    ''' </summary>
    <Category("Appearance"),
    Description("The text that will appear in the titlebar of the panel."),
    Browsable(True),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Overrides Property Text() As String

        Get
            Return Me.TitleLabel.Text
        End Get

        Set(ByVal value As String)
            Me.TitleLabel.Text = value
            'undone: in the future this will allow us to track the active window in the title
            'bar and still remember the panel's title
            m_titleText = value
        End Set

    End Property

    ''' <summary>
    ''' The background color of the title.
    ''' </summary>
    <Category("Appearance"),
    Description("The background color of the title."),
    Browsable(True),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property TitleBackColor() As Color

        Get
            Return m_titleBackColor
        End Get

        Set(ByVal value As Color)
            m_titleBackColor = value

            Me.TitlePanel.BackColor = value
            Me.TitleLabel.BackColor = value
            Me.TitleLabelBackground.BackColor = value
            Me.TitleLabelMenuGlyph.BackColor = value

            If Me.NextSubPanel IsNot Nothing Then
                Me.NextSubPanel.TitleBackColor = value
            End If
        End Set

    End Property

    ''' <summary>
    ''' The foreground color of the title used to display text.
    ''' </summary>
    <Category("Appearance"),
    Description("The foreground color of the title used to display text."),
    Browsable(True),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property TitleForeColor() As Color

        Get
            Return m_titleForeColor
        End Get

        Set(ByVal value As Color)
            m_titleForeColor = value

            Me.TitlePanel.ForeColor = value
            Me.TitleLabel.ForeColor = value
            Me.TitleLabelBackground.ForeColor = value
            Me.TitleLabelMenuGlyph.ForeColor = value

            If Me.NextSubPanel IsNot Nothing Then
                Me.NextSubPanel.TitleForeColor = value
            End If
        End Set

    End Property

    ''' <summary>
    ''' Show or hide the panel's titlebar.
    ''' </summary>
    <Category("Appearance"),
    Description("Show or hide the panel's titlebar."),
    Browsable(True)>
    Public Property ShowTitle() As Boolean

        Get
            Return Me.TitlePanel.Visible
        End Get

        Set(ByVal value As Boolean)
            Me.TitlePanel.Visible = value

            If Me.NextSubPanel IsNot Nothing Then
                Me.NextSubPanel.ShowTitle = value
            End If
        End Set

    End Property

    ''' <summary>
    ''' Not supported.
    ''' </summary>
    <Category("Layout"),
    Description("Not supported."),
    Browsable(True),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Overrides Property Anchor() As AnchorStyles

        Get
            Return MyBase.Anchor
        End Get

        Set(ByVal value As AnchorStyles)
            If Not value = AnchorStyles.None Then
                Throw New Exception("Invalid property value. Anchor property is not supported or necessary.")
            Else
                MyBase.Anchor = value
            End If
        End Set

    End Property

    ''' <summary>
    ''' Not supported (See Orientation property).
    ''' </summary>
    <Category("Layout"),
    Description("Not supported (See Orientation property)."),
    Browsable(True),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Overrides Property Dock() As DockStyle

        Get
            Return MyBase.Dock
        End Get

        Set(ByVal value As DockStyle)
            If Not value = DockStyle.None Then
                Throw New Exception("Invalid property value. Please use the 'Orientation' property instead.")
            Else
                MyBase.Dock = value
            End If
        End Set

    End Property

    ''' <summary>
    ''' Placement of the panel on the MDI parent form.
    ''' </summary>
    <Category("Layout"),
    Description("Placement of the panel on the MDI parent form."),
    Browsable(True)>
    Public Property Orientation() As WindowManagerOrientation

        Get
            Return m_orientation
        End Get

        Set(ByVal value As WindowManagerOrientation)

            If Me.DesignMode AndAlso value = WindowManagerOrientation.Top AndAlso Me.DesignMode AndAlso Me.AuxiliaryWindow IsNot Nothing Then
                'I don't think we really need to enforce this
                'Throw New System.Exception("Orientiation.Top is not allowed when an AuxiliaryWindow is set.")
            Else
                Me.MinMode = False

                m_orientation = value

                'I don't think we really need to enforce this
                'If m_orientation = WindowManagerOrientation.Top Then
                '    Me.AllowUserVerticalRepositioning = False
                'End If

                If Me.DesignMode Then
                    If m_orientation = WindowManagerOrientation.Top OrElse m_orientation = WindowManagerOrientation.Left OrElse m_orientation = WindowManagerOrientation.Right Then
                        Me.AllowUserVerticalRepositioning = False
                    Else
                        Me.AllowUserVerticalRepositioning = True
                    End If
                End If

                SnapToSize()

                If Me.DesignMode Then
                    SnapToDesignSize()
                End If

                WindowManagerSplitter2.Visible = (m_orientation = WindowManagerOrientation.Left OrElse m_orientation = WindowManagerOrientation.Right)

                Select Case m_orientation
                    Case WindowManagerOrientation.Left
                        WindowManagerSplitter2.Dock = DockStyle.Right
                    Case WindowManagerOrientation.Right
                        WindowManagerSplitter2.Dock = DockStyle.Left
                End Select

                If Me.NextSubPanel IsNot Nothing Then
                    Me.NextSubPanel.Orientation = m_orientation
                    CopyAppearance(Me.NextSubPanel)
                End If
            End If

        End Set

    End Property

    ''' <summary>
    ''' Look and feel of the tabs that represent the windows.
    ''' </summary>
    <Category("Appearance"),
    Description("Look and feel of the tabs that represent the windows."),
    Browsable(True)>
    Public Property Style() As TabStyle

        Get
            Return m_style
        End Get

        Set(ByVal value As TabStyle)
            m_style = value

            Try
                LayoutTabStrips()
            Catch
                'do nothing
            End Try

            If Me.NextSubPanel IsNot Nothing Then
                Me.NextSubPanel.Style = value
            End If
        End Set

    End Property

    ''' <summary>
    ''' Show the icon of the associated window in its corresponding tab.
    ''' </summary>
    <Category("Appearance"),
    Description("Show the icon of the associated window in its corresponding tab."),
    Browsable(True)>
    Public Property ShowIcons() As Boolean

        Get
            Return m_showIcons
        End Get

        Set(ByVal value As Boolean)
            m_showIcons = value

            Try
                LayoutTabStrips()
            Catch
                'do nothing
            End Try

            If Me.NextSubPanel IsNot Nothing Then
                Me.NextSubPanel.ShowIcons = value
            End If
        End Set

    End Property

    ''' <summary>
    ''' Allow user to reposition (up/down) the panel.
    ''' </summary>
    <Category("Layout"),
    Description("Allow user to reposition (up/down) the panel."),
    Browsable(True)>
    Public Property AllowUserVerticalRepositioning() As Boolean

        Get
            Return m_allowUserVerticalRepositioning
        End Get

        Set(ByVal value As Boolean)
            If Me.DesignMode AndAlso value AndAlso Me.Orientation = WindowManagerOrientation.Top Then
                'I don't think we really need to enforce this, the panel will simply ignore the positioning if attempted
                'Throw New Exception("User repositioning is not allowed when Orientation=Top")
            Else
                m_allowUserVerticalRepositioning = value
            End If
        End Set

    End Property

    ''' <summary>
    ''' Allows the panel to disappear when no windows exist.
    ''' </summary>
    <Category("Layout"),
    Description("Allows the panel to disappear when no windows exist."),
    Browsable(True)>
    Public Property AutoHide() As Boolean

        Get
            Return m_autoHide
        End Get

        Set(ByVal value As Boolean)
            m_autoHide = value

            If m_isLoaded Then
                Try
                    If m_autoHide Then
                        AttemptAutoHide()
                    Else
                        AttemptUndoAutoHide()
                    End If
                Catch
                    'do nothing
                End Try

                If Not m_autoHide Then
                    Me.Visible = True
                    m_inAutoHideMode = False
                End If
            End If
        End Set

    End Property

    ''' <summary>
    ''' Put the panel in a minimized mode.
    ''' </summary>
    <Browsable(False)>
    Public Property MinMode() As Boolean

        Get
            Return m_minMode
        End Get

        Set(ByVal value As Boolean)
            'todo: we're going to leave the bug where MinMode is not cancelled when Orientation=Left/Right 
            'and a window in a SUBPANEL is activated. This is caused because subpanels aren't *technically* in
            'minmode when the primary panel is Oriented on the Left or Right and therefore don't react to 
            'MinMode=False because of the following conditional. Changing this might mess some things up.
            'Need to find a workaround.
            If value <> m_minMode Then
                ToggleMinMode()
            End If
        End Set

    End Property

    ''' <summary>
    ''' Gets the collection of horizontally tiled strips that currently exist in the panel.
    ''' </summary>
    <Browsable(False)>
    Public ReadOnly Property TabStrips() As WindowTabStripCollection

        Get
            Return m_windowTabStrips
        End Get

    End Property

    ''' <summary>
    ''' Gets or sets the tabstrip with the focus.
    ''' </summary>
    <Browsable(False)>
    Public Property SelectedTabStrip() As WindowTabStrip

        Get
            If m_selectedTabStrip IsNot Nothing Then
                Return m_selectedTabStrip
            Else
                If m_windowTabStrips.Count <> 0 Then
                    Return m_windowTabStrips.Item(0)
                Else
                    Return Nothing
                End If
            End If
        End Get

        Set(ByVal value As WindowTabStrip)
            If value IsNot Nothing Then
                If m_windowTabStrips.Contains(value) Then
                    If m_selectedTabStrip IsNot Nothing Then
                        m_selectedTabStrip.IsActive = False
                    End If

                    If m_selectedTabStrip IsNot value Then
                        Me.MinMode = False
                    End If

                    m_selectedTabStrip = value

                    If m_isActive Then
                        m_selectedTabStrip.IsActive = True
                    End If
                Else
                    Throw New ArgumentException("Item not in collection.")
                End If
            Else
                m_selectedTabStrip = value
            End If
        End Set

    End Property

    Friend Property NextSubPanel() As WindowManagerPanel

        Get
            Return m_nextWindowManagerPanel
        End Get

        Set(ByVal value As WindowManagerPanel)
            If value Is Me Then
                Throw New InvalidOperationException("Cannot reference self.")
            Else
                m_nextWindowManagerPanel = value
            End If
        End Set

    End Property

    Private Property IsActive() As Boolean

        Get
            Return m_isActive
        End Get

        Set(ByVal value As Boolean)
            If value <> m_isActive Then
                m_isActive = value

                OnIsActiveChanged(EventArgs.Empty)

                'undone: do something pretty here to indicate focus
                If m_showFocusIndicator Then
                    If m_isActive Then
                        Me.WindowManagerSplitter2.BackColor = SystemColors.Highlight
                        Me.FocusIndicatorPanel.BackColor = SystemColors.Highlight
                    Else
                        Me.WindowManagerSplitter2.BackColor = Me.BackColor
                        Me.FocusIndicatorPanel.BackColor = Me.BackColor
                    End If
                End If
            End If

            If Me.SelectedTabStrip IsNot Nothing AndAlso Me.SelectedTabStrip.IsActive <> m_isActive Then
                Me.SelectedTabStrip.IsActive = m_isActive
            End If
        End Set

    End Property

    ''' <summary>
    ''' Gets or sets a window that will act as side-by-side pane.
    ''' </summary>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property AuxiliaryWindow() As Form
        Get
            Return m_auxiliaryWindow
        End Get

        Set(ByVal value As Form)
            If value IsNot Nothing AndAlso Me.Orientation = WindowManagerOrientation.Top Then
                Throw New Exception("AuxiliaryWindow is not supported when Orientation=Top")
            Else
                m_auxiliaryWindow = value

                If value IsNot Nothing Then
                    If value.MdiParent IsNot GetMDIParent() Then
                        value.MdiParent = GetMDIParent()
                    End If

                    If value.FormBorderStyle <> FormBorderStyle.None Then
                        value.FormBorderStyle = FormBorderStyle.None
                    End If
                End If

                ReconcileAuxiliaryWindowSize()
            End If
        End Set

    End Property

    ''' <summary>
    ''' Allows MDIWindowManager to automatically take control of MDI child windows.
    ''' </summary>
    <Category("Behavior"),
    Description("Allows MDIWindowManager to automatically take control of MDI child windows."),
    Browsable(True)>
    Public Property AutoDetectMdiChildWindows() As Boolean

        Get
            Return m_autoDetectMdiChildWindows
        End Get

        Set(ByVal value As Boolean)
            m_autoDetectMdiChildWindows = value
        End Set

    End Property

    ''' <summary>
    ''' Show or hide the window layout options buttons.
    ''' </summary>
    <Category("Appearance"),
    Description("Show or hide the window layout options buttons."),
    Browsable(True)>
    Public Property ShowLayoutButtons() As Boolean

        Get
            Return m_windowLayoutButtonsVisible
        End Get

        Set(ByVal value As Boolean)
            m_windowLayoutButtonsVisible = value
            ArrangeWindowActionButtons()

            If Me.NextSubPanel IsNot Nothing Then
                Me.NextSubPanel.ShowLayoutButtons = value
            End If
        End Set

    End Property

    ''' <summary>
    ''' Show or hide the Close Window button.
    ''' </summary>
    <Category("Appearance"),
    Description("Show or hide the Close Window button."),
    Browsable(True)>
    Public Property ShowCloseButton() As Boolean

        Get
            Return m_windowCloseButtonVisible
        End Get

        Set(ByVal value As Boolean)
            m_windowCloseButtonVisible = value
            ArrangeWindowActionButtons()

            If Me.NextSubPanel IsNot Nothing Then
                Me.NextSubPanel.ShowCloseButton = value
            End If
        End Set

    End Property

    ''' <summary>
    ''' Allows you to select from a list of predefined TabProviders at design-time instead of using the CustomTabsProvider property.
    ''' </summary>
    <Category("Appearance"),
    Description("Allows you to select from a list of predefined TabProviders to change the look and feel of the tabs."),
    Browsable(True)>
    Public Property TabRenderMode() As TabsProvider

        Get
            Return m_tabRenderMode
        End Get

        Set(ByVal value As TabsProvider)
            If value <> m_tabRenderMode Then
                m_tabRenderMode = value
                InitializePrimaryTabStrip()
            End If
        End Set

    End Property

    ''' <summary>
    ''' Specifies the look and feel for the buttons.
    ''' </summary>
    <Category("Appearance"),
    Description("Specifies the look and feel for the buttons."),
    Browsable(True)>
    Public Property ButtonRenderMode() As ButtonRenderMode

        Get
            Return m_buttonRenderMode
        End Get

        Set(ByVal value As ButtonRenderMode)
            m_buttonRenderMode = value

            Select Case m_buttonRenderMode
                Case ButtonRenderMode.Standard
                    Me.CloseWindowButton.RenderMode = WindowManagerButton.WindowManagerButtonRenderMode.RollOverClassic
                    Me.PopoutWindowButton.RenderMode = WindowManagerButton.WindowManagerButtonRenderMode.RollOverClassic
                    Me.TileWindowButton.RenderMode = WindowManagerButton.WindowManagerButtonRenderMode.RollOverClassic
                    Me.HTileButton.RenderMode = WindowManagerButton.WindowManagerButtonRenderMode.RollOverClassic
                Case ButtonRenderMode.System
                    Me.CloseWindowButton.RenderMode = WindowManagerButton.WindowManagerButtonRenderMode.Standard
                    Me.PopoutWindowButton.RenderMode = WindowManagerButton.WindowManagerButtonRenderMode.Standard
                    Me.TileWindowButton.RenderMode = WindowManagerButton.WindowManagerButtonRenderMode.Standard
                    Me.HTileButton.RenderMode = WindowManagerButton.WindowManagerButtonRenderMode.Standard
                Case ButtonRenderMode.Professional
                    Me.CloseWindowButton.RenderMode = WindowManagerButton.WindowManagerButtonRenderMode.Fancy
                    Me.PopoutWindowButton.RenderMode = WindowManagerButton.WindowManagerButtonRenderMode.Fancy
                    Me.TileWindowButton.RenderMode = WindowManagerButton.WindowManagerButtonRenderMode.Fancy
                    Me.HTileButton.RenderMode = WindowManagerButton.WindowManagerButtonRenderMode.Fancy
            End Select

            If Me.NextSubPanel IsNot Nothing Then
                Me.NextSubPanel.ButtonRenderMode = value
            End If
        End Set

    End Property

    ''' <summary>
    ''' Allows you to specify a different provider for the look and feel of the tabs. (Set at design-time or app start only).
    ''' </summary>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property CustomTabsProviderType() As Type

        Get
            Return m_customTabsProviderType
        End Get

        Set(ByVal value As Type)
            If m_customTabsProviderType IsNot value Then
                m_customTabsProviderType = value
                InitializePrimaryTabStrip()
            End If
        End Set

    End Property

    ''' <summary>
    ''' Enables the TabPaint event to allow owner drawing of tabs.
    ''' </summary>
    <Category("Behavior"),
    Description("Enables the TabPaint event to allow owner drawing of tabs."),
    Browsable(True)>
    Public Property EnableTabPaintEvent() As Boolean

        Get
            Return m_enableTabPaintEvent
        End Get

        Set(ByVal value As Boolean)
            m_enableTabPaintEvent = value
            LayoutTabStrips()
        End Set

    End Property

    ''' <summary>
    ''' Disable the Close action button and menu items.
    ''' </summary>
    <Category("Appearance"),
    Description("Disable the Close action button and menu items."),
    Browsable(True)>
    Public Property DisableCloseAction() As Boolean

        Get
            Return m_disableCloseAction
        End Get

        Set(ByVal value As Boolean)
            m_disableCloseAction = value
            Me.CloseWindowButton.Enabled = Not value
        End Set

    End Property

    ''' <summary>
    ''' Disable the Tile action button and menu items.
    ''' </summary>
    <Category("Appearance"),
    Description("Disable the Tile action button and menu items."),
    Browsable(True)>
    Public Property DisableTileAction() As Boolean
        Get
            Return m_disableTileAction
        End Get
        Set(ByVal value As Boolean)
            m_disableTileAction = value
            Me.TileWindowButton.Enabled = Not value
        End Set
    End Property

    ''' <summary>
    ''' Disable the New Horizontral Group action button and menu items.
    ''' </summary>
    <Category("Appearance"),
    Description("Disable the New Horizontal Group action button and menu items."),
    Browsable(True)>
    Public Property DisableHTileAction() As Boolean
        Get
            Return m_disableHTileAction
        End Get
        Set(ByVal value As Boolean)
            m_disableHTileAction = value
            Me.HTileButton.Enabled = Not value
        End Set
    End Property

    ''' <summary>
    ''' Disable the Popout Window action button and menu items.
    ''' </summary>
    <Category("Appearance"),
    Description("Disable the Popout Window action button and menu items."),
    Browsable(True)>
    Public Property DisablePopoutAction() As Boolean
        Get
            Return m_disablePopoutAction
        End Get
        Set(ByVal value As Boolean)
            m_disablePopoutAction = value
            Me.PopoutWindowButton.Enabled = Not value
        End Set
    End Property

#End Region

#Region "Methods"

    Private Sub InitializePrimaryTabStrip()

        If m_windowTabStrips.Count > 1 OrElse GetAllWindows(False).Count > 0 Then
            Throw New InvalidOperationException("TabStrip Initialization not possible at this time.")
        Else
            If m_windowTabStrips.Count > 0 Then
                m_suppressTabStripRemoveAbort = True
                Try
                    Me.Controls.Remove(m_windowTabStrips.Item(0))
                    m_windowTabStrips.RemoveAt(0)
                Catch ex As Exception
                    Throw ex
                Finally
                    m_suppressTabStripRemoveAbort = False
                End Try
                'InitializePrimaryTabStrip will now be recalled by an event in m_windowTabStrips
            Else
                Dim windowTabStrip As WindowTabStrip = CreateWindowTabStrip()

                windowTabStrip.Width = Me.TabStripsContainer.ClientRectangle.Width
                m_windowTabStrips.Add(windowTabStrip)
            End If
        End If

    End Sub

    Private Function CreateWindowTabStrip() As WindowTabStrip

        Dim windowTabStrip As New WindowTabStrip

        If m_customTabsProviderType IsNot Nothing Then
            Dim newTabsProvider As ITabsProvider = CType(Activator.CreateInstance(m_customTabsProviderType), ITabsProvider)

            OnNewCustomTabsProviderInstance(New NewTabsProviderInstanceCreatedEventArgs(newTabsProvider))

            windowTabStrip.TabsProvider = newTabsProvider
        Else
            Select Case m_tabRenderMode
                Case TabsProvider.Standard
                    'do nothing
                Case TabsProvider.System
                    windowTabStrip.TabsProvider = New SystemTabsProvider
            End Select
        End If

        Return windowTabStrip

    End Function

    Private Function GetMDIParent() As Form

        'Return CType(Me.Parent, Form)
        Return m_parentForm

    End Function

    Private Function GetMDIClient() As MdiClient

        If m_mdiClientControl Is Nothing Then
            m_mdiClientControl = ScanForMdiClientControl()

            If m_mdiClientControl IsNot Nothing Then
                m_originalMDIClientRect = m_mdiClientControl.Bounds
            End If
        End If

        Return m_mdiClientControl

    End Function

    Private Function ScanForMdiClientControl() As MdiClient

        Dim parentForm As Form = GetMDIParent()

        If parentForm IsNot Nothing Then
            For Each ctl As Control In parentForm.Controls
                If TypeOf ctl Is MdiClient Then
                    Return CType(ctl, MdiClient)
                End If
            Next ctl
        End If

        Return Nothing

    End Function

    Friend Sub SetAsTemporaryPanel(ByVal nextWindowManagerPanel As WindowManagerPanel)

        Me.NextSubPanel = nextWindowManagerPanel
        m_isTemporaryWindowManagerPanel = True

    End Sub

    ''' <summary>
    ''' Determine whether this is the currently focused WindowManagerPanel.
    ''' </summary>
    Public Function IsActivePanel() As Boolean

        Return Me.IsActive

    End Function

    ''' <summary>
    ''' Determine if this is the top primary panel on an MDI form.
    ''' </summary>
    Public Function IsPrimaryPanel() As Boolean

        Dim eventargs As New GetTopPanelEventArgs

        OnRequestChainGetTopPanel(eventargs)

        Return (eventargs.TopPanel Is Me OrElse eventargs.TopPanel Is Nothing)

    End Function

    ''' <summary>
    ''' (Reserved. Not intended for normal use. The Primary Panel is always the one created at design-time).
    ''' </summary>
    Public Function GetPrimaryPanel() As WindowManagerPanel

        Dim eventargs As New GetTopPanelEventArgs

        OnRequestChainGetTopPanel(eventargs)

        If eventargs.TopPanel IsNot Nothing Then
            Return eventargs.TopPanel
        Else
            Return Me
        End If

    End Function

    ''' <summary>
    ''' Get all the windows being managed.
    ''' </summary>
    Public Function GetAllWindows(Optional ByVal includeSubPanels As Boolean = True) As WrappedWindowCollection

        Dim wrappedWindows As New WrappedWindowCollection

        For Each windowTabStrip As WindowTabStrip In m_windowTabStrips
            For Each wrappedWindow As WrappedWindow In windowTabStrip.Items
                wrappedWindows.Add(wrappedWindow)
            Next wrappedWindow
        Next windowTabStrip

        If includeSubPanels Then
            If Me.NextSubPanel IsNot Nothing Then
                Dim chainWrappedWindows As WrappedWindowCollection = Me.NextSubPanel.GetAllWindows(includeSubPanels)

                For Each wrappedWindow As WrappedWindow In chainWrappedWindows
                    wrappedWindows.Add(wrappedWindow)
                Next wrappedWindow
            End If
        End If

        Return wrappedWindows

    End Function

    Private Function GetAllPanels() As List(Of WindowManagerPanel)

        Return GetPanels(includeOnlySubPanels:=False)

    End Function

    ''' <summary>
    ''' Returns a collection containing all subpanels.
    ''' </summary>
    Public Function GetAllSubPanels() As List(Of WindowManagerPanel)

        Return GetPanels(includeOnlySubPanels:=True)

    End Function

    Private Function GetPanels(ByVal includeOnlySubPanels As Boolean) As List(Of WindowManagerPanel)

        Dim panels As New List(Of WindowManagerPanel)

        If Not includeOnlySubPanels Then
            panels.Add(Me)
        End If

        Dim windowManagerPanel As WindowManagerPanel = Me.NextSubPanel

        Do Until windowManagerPanel Is Nothing
            panels.Add(windowManagerPanel)
            windowManagerPanel = windowManagerPanel.NextSubPanel
        Loop

        Return panels

    End Function

    ''' <summary>
    ''' Get the currently active panel.
    ''' </summary>
    Public Function GetActivePanel() As WindowManagerPanel

        Dim result As WindowManagerPanel
        Dim panels As List(Of WindowManagerPanel) = GetAllPanels()

        For Each panel As WindowManagerPanel In panels
            If panel.IsActivePanel() Then
                result = panel
                Exit For
            End If
        Next panel

        If result Is Nothing AndAlso Not Me.IsTemporaryPanel Then
            result = Me
        End If

        Return result

    End Function

    ''' <summary>
    ''' Add a window to the WindowManagerPanel.
    ''' </summary>
    Public Sub AddWindow(ByVal wrappedWindow As WrappedWindow)

        If Not m_autoDetectMdiChildWindows Then
            _AddWindow(wrappedWindow)
        Else
            Throw New InvalidOperationException("AddWindow not supported when AutoDetectMdiChildWindows is enabled.")
        End If

    End Sub

    Private Sub _AddWindow(ByVal wrappedWindow As WrappedWindow)

        AddWindow(m_windowTabStrips.Item(0), wrappedWindow)

    End Sub

    ''' <summary>
    ''' Add a window to the WindowManagerPanel.
    ''' </summary>
    Public Sub AddWindow(ByVal frm As Form)

        If Not m_autoDetectMdiChildWindows Then
            _AddWindow(frm)
        Else
            Throw New InvalidOperationException("AddWindow not supported when AutoDetectMdiChildWindows is enabled.")
        End If

    End Sub

    Private Sub _AddWindow(ByVal frm As Form)

        AddWindow(m_windowTabStrips.Item(0), frm)

    End Sub

    Friend Sub AddWindow(ByVal windowTabStrip As WindowTabStrip, ByVal frm As Form)

        If GetWrapperForWindow(frm) Is Nothing Then
            AddWindow(windowTabStrip, New WrappedWindow(frm))
        End If

    End Sub

    Friend Sub AddWindow(ByVal windowTabStrip As WindowTabStrip, ByVal wrappedWindow As WrappedWindow)

        If windowTabStrip IsNot Nothing AndAlso wrappedWindow IsNot Nothing Then
            'If Not windowTabStrip.WrappedWindows.Contains(wrappedWindow) Then
            If Not GetAllWindows().Contains(wrappedWindow) Then
                wrappedWindow.Window.MdiParent = GetMDIParent()
                wrappedWindow.Window.SetBounds(0 - wrappedWindow.Window.Width, 0 - wrappedWindow.Window.Height, 0, 0, BoundsSpecified.Location)
                wrappedWindow.Window.Show()

                Try
                    windowTabStrip.Items.Add(wrappedWindow)
                Catch
                    'do nothing
                End Try

                SetActiveWindow(wrappedWindow)
            End If
        End If

    End Sub

    ''' <summary>
    ''' Remove a window from the WindowManagerPanel.
    ''' </summary>
    Public Sub RemoveWindow(ByVal wrappedWindow As WrappedWindow)

        'since we're kinda enforcing the limitation that a window can only belong to one
        'tabstrip, this is mostly unecessary... but we're keeping it for now.
        Dim windowTabStrip As WindowTabStrip = GetWrappedWindowTabStrip(wrappedWindow, False)

        Do Until windowTabStrip Is Nothing
            windowTabStrip.Items.Remove(wrappedWindow)
            windowTabStrip = GetWrappedWindowTabStrip(wrappedWindow, False)
        Loop

        If Me.NextSubPanel IsNot Nothing Then
            Me.NextSubPanel.RemoveWindow(wrappedWindow)
        End If

    End Sub

    ''' <summary>
    ''' Remove a window from the WindowManagerPanel.
    ''' </summary>
    Public Sub RemoveWindow(ByVal frm As Form)

        'since we're kinda enforcing the limitation that a window can only belong to one
        'tabstrip, this is mostly unecessary... but we're keeping it for now.
        For Each windowTabStrip As WindowTabStrip In m_windowTabStrips
            For Each wrappedWindow As WrappedWindow In windowTabStrip.Items
                If wrappedWindow.Window Is frm Then
                    windowTabStrip.Items.Remove(wrappedWindow)
                End If
            Next wrappedWindow
        Next windowTabStrip

        If Me.NextSubPanel IsNot Nothing Then
            Me.NextSubPanel.RemoveWindow(frm)
        End If

    End Sub

    ''' <summary>
    ''' Set the active window.
    ''' </summary>
    Public Sub SetActiveWindow(ByVal wrappedWindow As WrappedWindow)

        Dim windowTabStrip As WindowTabStrip = GetWrappedWindowTabStrip(wrappedWindow)

        If windowTabStrip IsNot Nothing Then
            windowTabStrip.SelectedItem = wrappedWindow
            ActivateWindow(wrappedWindow.Window)
        End If

    End Sub

    ''' <summary>
    ''' Set the active window.
    ''' </summary>
    Public Sub SetActiveWindow(ByVal frm As Form)

        Dim wrappedWindow As WrappedWindow = GetWrapperForWindow(frm)

        If wrappedWindow IsNot Nothing Then
            SetActiveWindow(wrappedWindow)
        End If

    End Sub

    ''' <summary>
    ''' Set the active window.
    ''' </summary>
    Public Sub SetActiveWindow(ByVal index As Integer)

        SetActiveWindow(GetAllWindows().Item(index))

    End Sub

    ''' <summary>
    ''' Get the currently active window.
    ''' </summary>
    Public Function GetActiveWindow() As WrappedWindow

        Dim windowManagerPanel As WindowManagerPanel = GetActivePanel()

        If windowManagerPanel IsNot Nothing Then
            Return windowManagerPanel.GetSelectedWindow()
        Else
            Return Nothing
        End If

    End Function

    'this function returns the selected window in THIS tabstrip not the window that may be active in the MDI parent.
    'hidden from end-user to lesson confusion.
    Friend Function GetSelectedWindow() As WrappedWindow

        Dim windowTabStrip As WindowTabStrip = Me.SelectedTabStrip

        If windowTabStrip IsNot Nothing Then
            Return windowTabStrip.SelectedItem
        Else
            Return Nothing
        End If

    End Function

    Private Sub UserCloseActiveWindow()

        UserCloseWindow(GetActiveWindow())

    End Sub

    Private Sub UserCloseSelectedWindow()

        UserCloseWindow(GetSelectedWindow())

    End Sub

    ''' <summary>
    ''' Helper method to close a window as if done by the user in order to get the desired Reason code in the window's Unload events.
    ''' </summary>
    Public Sub UserCloseWindow(ByVal wrappedWindow As WrappedWindow)

        If wrappedWindow IsNot Nothing Then
            UserCloseWindow(wrappedWindow.Window)
        End If

    End Sub

    ''' <summary>
    ''' Helper method to close a window as if done by the user in order to get the desired Reason code in the window's Unload events.
    ''' </summary>
    Public Sub UserCloseWindow(ByVal window As IWin32Window)

        If window IsNot Nothing Then
            'using PostMessage allows the window to receive the proper CloseReason in its Close events
            PostMessage(window.Handle, &H112, &HF060&, vbNullString) 'WM_SYSCOMMAND, SC_CLOSE
        End If

    End Sub

    ''' <summary>
    ''' Determine if this panel will be automatically be unloaded when all its windows are closed 
    ''' </summary>
    Public Function IsTemporaryPanel() As Boolean

        Return m_isTemporaryWindowManagerPanel

    End Function

    ''' <summary>
    ''' Find the wrapper object for a given window.
    ''' </summary>
    Public Function GetWrapperForWindow(ByVal frm As Form, Optional ByVal searchSubPanels As Boolean = True) As WrappedWindow

        For Each windowTabStrip As WindowTabStrip In m_windowTabStrips
            For Each wrappedWindow As WrappedWindow In windowTabStrip.Items
                If wrappedWindow.Window Is frm Then
                    Return wrappedWindow
                End If
            Next wrappedWindow
        Next windowTabStrip

        If searchSubPanels AndAlso Me.NextSubPanel IsNot Nothing Then
            Return Me.NextSubPanel.GetWrapperForWindow(frm, searchSubPanels)
        Else
            Return Nothing
        End If

    End Function

    ''' <summary>
    ''' Search this panel and all subpanels to identify the panel that contains the specified wrapped window.
    ''' </summary>
    Public Function GetWrappedWindowPanel(ByVal wrappedWindow As WrappedWindow) As WindowManagerPanel

        If wrappedWindow IsNot Nothing Then
            If GetAllWindows(False).Contains(wrappedWindow) Then
                Return Me
            Else
                If Me.NextSubPanel IsNot Nothing Then
                    Return Me.NextSubPanel.GetWrappedWindowPanel(wrappedWindow)
                Else
                    Return Nothing
                End If
            End If
        End If

    End Function

    ''' <summary>
    ''' Search this panel and all subpanels to identify the tabstrip that contains the specified wrapped window.
    ''' </summary>
    Public Function GetWrappedWindowTabStrip(ByVal wrappedWindow As WrappedWindow, Optional ByVal searchSubPanels As Boolean = True) As WindowTabStrip

        If wrappedWindow IsNot Nothing Then
            If m_windowTabStrips IsNot Nothing Then
                For Each windowTabStrip As WindowTabStrip In m_windowTabStrips
                    If windowTabStrip.Items.Contains(wrappedWindow) Then
                        Return windowTabStrip
                    End If
                Next windowTabStrip
            End If

            If searchSubPanels AndAlso Me.NextSubPanel IsNot Nothing Then
                Return Me.NextSubPanel.GetWrappedWindowTabStrip(wrappedWindow, searchSubPanels)
            Else
                Return Nothing
            End If
        End If

    End Function

    ''' <summary>
    ''' Determine if a wrapped window is alone in a tabstrip.
    ''' </summary>
    Public Function IsWrappedWindowTiled(ByVal wrappedWindow As WrappedWindow) As Boolean

        Dim windowTabStrip As WindowTabStrip = GetWrappedWindowTabStrip(wrappedWindow)

        If windowTabStrip IsNot Nothing Then
            Return (windowTabStrip.Items.Count = 1)
        Else
            Return False
        End If

    End Function

    ''' <summary>
    ''' Determine if a wrapped window is on a temporarily chained subpanel.
    ''' </summary>
    Public Function IsWrappedWindowHTiled(ByVal wrappedWindow As WrappedWindow) As Boolean

        If wrappedWindow IsNot Nothing Then
            Dim windowManagerPanel As WindowManagerPanel = Me.GetWrappedWindowPanel(wrappedWindow)

            Return (windowManagerPanel IsNot Nothing AndAlso windowManagerPanel.IsTemporaryPanel)
        End If

    End Function

    ''' <summary>
    ''' Determine if a wrapped window is removed from the MDI parent and overlapping on the desktop.
    ''' </summary>
    Public Function IsWrappedWindowPoppedOut(ByVal wrappedWindow As WrappedWindow) As Boolean

        If wrappedWindow IsNot Nothing Then
            Return g_poppedOutWindowsManager.Contains(wrappedWindow)
        End If

    End Function

    ''' <summary>
    ''' Toggle tiling or untiling of a wrapped window.
    ''' </summary>
    Public Sub TileOrUntileWrappedWindow(ByVal wrappedWindow As WrappedWindow)

        If wrappedWindow IsNot Nothing Then
            If GetAllWindows(False).Contains(wrappedWindow) Then
                If IsWrappedWindowTiled(wrappedWindow) Then
                    UntileWrappedWindow(wrappedWindow)
                Else
                    TileWrappedWindow(wrappedWindow)
                End If
            Else
                If Me.NextSubPanel IsNot Nothing Then
                    Me.NextSubPanel.TileOrUntileWrappedWindow(wrappedWindow)
                End If
            End If
        End If

    End Sub

    ''' <summary>
    ''' Create a new tabstrip and put a wrapped window in it.
    ''' </summary>
    Public Sub TileWrappedWindow(ByVal wrappedWindow As WrappedWindow)

        If wrappedWindow IsNot Nothing Then
            Dim windowManagerPanel As WindowManagerPanel = GetWrappedWindowPanel(wrappedWindow)

            If windowManagerPanel IsNot Nothing Then
                If windowManagerPanel Is Me Then
                    Dim windowTabStrip As WindowTabStrip = GetWrappedWindowTabStrip(wrappedWindow, False)

                    If windowTabStrip IsNot Nothing Then
                        Dim eventargs As New WrappedWindowCancelEventArgs(wrappedWindow, windowTabStrip, Me)

                        OnWindowTiling(eventargs)

                        If Not eventargs.Cancel Then
                            Try
                                windowManagerPanel.Focus()
                            Catch
                                'do nothing
                            End Try

                            If windowTabStrip IsNot Nothing Then
                                windowTabStrip.Items.Remove(wrappedWindow)
                            End If

                            Dim newWindowTabStrip As WindowTabStrip = CreateWindowTabStrip()

                            m_windowTabStrips.Add(newWindowTabStrip)
                            Me.SelectedTabStrip = newWindowTabStrip
                            newWindowTabStrip.Items.Add(wrappedWindow)
                            newWindowTabStrip.SelectedItem = wrappedWindow
                            ActivateWindow(newWindowTabStrip.SelectedItem.Window)

                            LayoutTabStrips()
                        End If
                    End If
                Else
                    windowManagerPanel.TileWrappedWindow(wrappedWindow)
                End If
            End If
        End If

    End Sub

    ''' <summary>
    ''' Return the wrapped window to main tabstrip.
    ''' </summary>
    Public Sub UntileWrappedWindow(ByVal wrappedWindow As WrappedWindow)

        If wrappedWindow IsNot Nothing Then
            Dim windowManagerPanel As WindowManagerPanel = GetWrappedWindowPanel(wrappedWindow)

            If windowManagerPanel IsNot Nothing Then
                If windowManagerPanel Is Me Then
                    Dim windowTabStrip As WindowTabStrip = GetWrappedWindowTabStrip(wrappedWindow, False)

                    If windowTabStrip IsNot Nothing Then
                        Dim eventargs As New WrappedWindowCancelEventArgs(wrappedWindow, windowTabStrip, Me)

                        OnWindowUnTiling(eventargs)

                        If Not eventargs.Cancel Then
                            Try
                                windowManagerPanel.Focus()
                            Catch
                                'do nothing
                            End Try

                            Dim primaryWindowTabStrip As WindowTabStrip = m_windowTabStrips.Item(0)

                            If windowTabStrip IsNot primaryWindowTabStrip Then
                                If windowTabStrip IsNot Nothing Then
                                    windowTabStrip.Items.Remove(wrappedWindow)

                                    If windowTabStrip.Items.Count = 0 Then
                                        If m_windowTabStrips.Contains(windowTabStrip) Then
                                            m_windowTabStrips.Remove(windowTabStrip)
                                        End If
                                    End If
                                End If

                                primaryWindowTabStrip.Items.Add(wrappedWindow)
                                primaryWindowTabStrip.SelectedItem = wrappedWindow
                                ActivateWindow(primaryWindowTabStrip.SelectedItem.Window)

                                LayoutTabStrips()
                            End If
                        End If
                    End If
                Else
                    windowManagerPanel.UntileWrappedWindow(wrappedWindow)
                End If
            End If
        End If

    End Sub

    ''' <summary>
    ''' Add a new subpanel and put a wrapped window in it.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub HTileWrappedWindow(ByVal wrappedWindow As WrappedWindow)

        If wrappedWindow IsNot Nothing Then
            If GetAllWindows(False).Contains(wrappedWindow) Then
                Dim windowTabStrip As WindowTabStrip = GetWrappedWindowTabStrip(wrappedWindow, False)

                If Me.NextSubPanel IsNot Nothing AndAlso Me.NextSubPanel.IsTemporaryPanel Then
                    Dim eventargs As New WrappedWindowCancelEventArgs(wrappedWindow, windowTabStrip, Me)

                    OnWindowHTiling(eventargs)

                    If Not eventargs.Cancel Then
                        Dim nextWindowManagerPanel As WindowManagerPanel = Me.NextSubPanel

                        Try
                            nextWindowManagerPanel.Focus()
                        Catch
                            'do nothing
                        End Try

                        windowTabStrip.Items.Remove(wrappedWindow)
                        nextWindowManagerPanel.TabStrips.Item(0).Items.Add(wrappedWindow)
                        nextWindowManagerPanel.SetActiveWindow(wrappedWindow)
                        ActivateWindow(wrappedWindow.Window)
                    End If
                Else
                    Dim eventargs As New WrappedWindowCancelEventArgs(wrappedWindow, windowTabStrip, Me)

                    OnWindowHTiling(eventargs)

                    If Not eventargs.Cancel Then
                        If GetAllWindows(False).Count > 1 Then
                            Dim newWindowManagerPanel As New WindowManagerPanel

                            'newWindowManagerPanel.Style = Me.Style

                            If Me.Orientation = WindowManagerOrientation.Top Then
                                newWindowManagerPanel.Orientation = WindowManagerOrientation.Bottom
                            Else
                                newWindowManagerPanel.Orientation = Me.Orientation
                            End If

                            'newWindowManagerPanel.ShowTitle = Me.ShowTitle
                            'newWindowManagerPanel.ShowLayoutButtons = Me.ShowLayoutButtons
                            CopyAppearance(newWindowManagerPanel)

                            newWindowManagerPanel.AllowUserVerticalRepositioning = True

                            Dim mdiParentForm As Form = GetMDIParent()
                            Dim mdiClientControl As MdiClient = GetMDIClient()
                            Dim correctionNeeded As Boolean = False
                            Dim bottomBarrier As Integer = Me.Top + Me.Height + 50 + newWindowManagerPanel.Height + 50

                            If NextSubPanel Is Nothing Then
                                If mdiClientControl.PointToClient(mdiParentForm.PointToScreen(New Point(Me.Left, bottomBarrier))).Y > mdiClientControl.ClientRectangle.Bottom Then
                                    bottomBarrier -= mdiClientControl.PointToClient(mdiParentForm.PointToScreen(New Point(Me.Left, bottomBarrier))).Y - mdiClientControl.ClientRectangle.Bottom
                                    correctionNeeded = True
                                End If
                            Else
                                If bottomBarrier > Me.NextSubPanel.Top Then
                                    bottomBarrier -= bottomBarrier - Me.NextSubPanel.Top
                                    correctionNeeded = True
                                End If
                            End If

                            Dim okToContinue As Boolean = False

                            If correctionNeeded Then
                                Dim eventargs2 As RedoLayoutEventArgs = New RedoLayoutEventArgs(Me, mdiClientControl, True)

                                bottomBarrier = bottomBarrier - 50 - newWindowManagerPanel.Height - 50 - Me.Height
                                eventargs2.BottomBarrier = bottomBarrier
                                OnRequestChainRedoLayout(eventargs2)

                                If eventargs2.Cancel Then
                                    If Me.NextSubPanel Is Nothing Then
                                        bottomBarrier = mdiClientControl.ClientRectangle.Bottom - 50 - newWindowManagerPanel.Height - 50 - Me.Height
                                    Else
                                        bottomBarrier = NextSubPanel.Top - 50 - newWindowManagerPanel.Height - 50 - Me.Height
                                    End If

                                    eventargs2 = New RedoLayoutEventArgs(Me, mdiClientControl, True)
                                    eventargs2.BottomBarrier = bottomBarrier
                                    OnRequestChainRedoLayout(eventargs2)
                                End If

                                okToContinue = Not eventargs2.Cancel
                            Else
                                If Me.NextSubPanel Is Nothing Then
                                    bottomBarrier = CInt((mdiClientControl.ClientRectangle.Bottom - (Me.Top)) / 2)
                                Else
                                    bottomBarrier = CInt((Me.NextSubPanel.Top - (Me.Top)) / 2)
                                End If

                                okToContinue = True
                            End If

                            If Not okToContinue Then
                                MsgBox("Нет доступного пространства для открытия новой горизонтальной вкладки группы.", MsgBoxStyle.Exclamation, System.Reflection.Assembly.GetEntryAssembly.GetName().Name)
                            Else
                                Dim eventargs2 As New RedoLayoutEventArgs(Me, mdiClientControl, False) With {.BottomBarrier = bottomBarrier}
                                OnRequestChainRedoLayout(eventargs2)

                                windowTabStrip.Items.Remove(wrappedWindow)

                                Me.Parent.Controls.Add(newWindowManagerPanel)
                                'Me.Parent.Refresh()

                                Dim oldNextWindowManagerPanel As WindowManagerPanel = Me.NextSubPanel

                                Me.NextSubPanel = Nothing

                                If correctionNeeded Then
                                    Me.Top = bottomBarrier
                                End If

                                newWindowManagerPanel.CustomTabsProviderType = Me.CustomTabsProviderType

                                OnNewCustomTabsProviderInstance(New NewTabsProviderInstanceCreatedEventArgs(newWindowManagerPanel.TabStrips.Item(0).TabsProvider))

                                newWindowManagerPanel.SetAsTemporaryPanel(oldNextWindowManagerPanel)

                                Me.NextSubPanel = newWindowManagerPanel

                                newWindowManagerPanel.Visible = True

                                If correctionNeeded Then
                                    newWindowManagerPanel.SetBounds(Me.Location.X, Me.Top + Me.Height + 50, Me.Size.Width, Me.Size.Height)
                                Else
                                    newWindowManagerPanel.SetBounds(Me.Location.X, bottomBarrier, Me.Size.Width, Me.Size.Height)
                                End If

                                Try
                                    newWindowManagerPanel.Focus()
                                Catch
                                    'do nothing
                                End Try

                                newWindowManagerPanel.TabStrips.Item(0).Items.Add(wrappedWindow)
                                newWindowManagerPanel.SetActiveWindow(wrappedWindow)
                                ActivateWindow(wrappedWindow.Window)
                            End If
                        End If
                    End If
                End If
            Else
                If Me.NextSubPanel IsNot Nothing Then
                    Me.NextSubPanel.HTileWrappedWindow(wrappedWindow)
                End If
            End If
        End If

    End Sub

    ''' <summary>
    ''' Remove the wrapped window from the panel and place it on the desktop as an overlapping window.
    ''' </summary>
    Public Sub PopOutWrappedWindow(ByVal wrappedWindow As WrappedWindow)

        Dim windowTabStrip As WindowTabStrip = GetWrappedWindowTabStrip(wrappedWindow)

        If windowTabStrip IsNot Nothing Then
            Dim eventargs As New WrappedWindowCancelEventArgs(wrappedWindow, windowTabStrip, Me)

            OnWindowPoppingOut(eventargs)

            If Not eventargs.Cancel Then
                'get and save these references because the WindowManagerPanel may clear them
                'if the form being popped out is that last remaining form in the panel
                Dim parentForm As Form = Me.GetMDIParent()
                Dim poppedOutWindowsManager As PoppedOutWindowsManager = m_poppedOutWindowsManager

                Try
                    windowTabStrip.Items.Remove(wrappedWindow)
                Catch ex As WindowTabStripCollectionException
                    'do nothing
                End Try

                wrappedWindow.AdjustFormProperties(False)
                wrappedWindow.Window.MdiParent = Nothing
                wrappedWindow.InitCustomSystemMenu()

                poppedOutWindowsManager.AddWindow(wrappedWindow, parentForm)

                ActivateWindow(wrappedWindow.Window)
            End If
        End If

    End Sub

    ''' <summary>
    ''' Return an overlapping wrapped window to WindowManagerPanel.
    ''' </summary>
    Public Sub PopInWrappedWindow(ByVal wrappedWindow As WrappedWindow)

        If wrappedWindow IsNot Nothing Then
            Dim eventargs As New WrappedWindowCancelEventArgs(wrappedWindow, Nothing, Me)

            OnWindowPoppingIn(eventargs)

            If Not eventargs.Cancel Then
                wrappedWindow.Window.WindowState = FormWindowState.Normal

                Try
                    m_poppedOutWindowsManager.RemoveWindow(wrappedWindow)
                Catch
                    'do nothing
                End Try

                Dim windowTabStrip As WindowTabStrip = m_windowTabStrips.Item(0)
                Dim mdiParentForm As Form = GetMDIParent()

                wrappedWindow.Window.Visible = False
                wrappedWindow.Window.MdiParent = mdiParentForm
                wrappedWindow.RemoveCustomSystemMenu()
                wrappedWindow.AdjustFormProperties(True)

                windowTabStrip.Items.Add(wrappedWindow)
                windowTabStrip.SelectedItem = wrappedWindow
                ActivateWindow(windowTabStrip.SelectedItem.Window)
            End If
        End If

    End Sub

    ''' <summary>
    ''' Close all windows being managed.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CloseAllWindows(Optional ByVal includeSubPanels As Boolean = True)

        Dim wrappedWindows As WrappedWindowCollection = GetAllWindows(includeSubPanels)

        For count As Integer = wrappedWindows.Count - 1 To 0 Step -1
            Try
                ShowWindowWithFocus(wrappedWindows.Item(count).Window)
                wrappedWindows.Item(count).Window.Close()
            Catch
                'do nothing
            End Try
        Next count

    End Sub

    ''' <summary>
    ''' Helper function to easily get the adjusted MDIClient area bounds of the parent form.
    ''' </summary>
    Public Function GetMDIClientAreaBounds() As Rectangle

        Dim mdiParentForm As Form = Me.GetMDIParent
        Dim mdiClientControl As MdiClient = GetMDIClient()

        If mdiClientControl IsNot Nothing Then
            Return mdiParentForm.RectangleToClient(mdiClientControl.RectangleToScreen(mdiClientControl.ClientRectangle))
        Else
            Return New Rectangle(0, 0, 0, 0)
        End If

    End Function

    ''' <summary>
    ''' Minimize the WindowManagerPanel.
    ''' </summary>
    Public Sub ToggleMinMode()

        If Not Me.IsTemporaryPanel Then
            Select Case Me.Orientation
                Case WindowManagerOrientation.Bottom
                    ToggleMinModeForBottom()
                Case WindowManagerOrientation.Left, WindowManagerOrientation.Right
                    ToggleMinModeForSides()
            End Select
        Else
            ToggleMinModeForBottom()
        End If


    End Sub

    ''' <summary>
    ''' Collapse the WindowManagerPanel to its smallest vertical size.
    ''' </summary>
    Public Sub SnapToMinimumSize(Optional ByVal includeSubPanels As Boolean = False)

        If includeSubPanels AndAlso Me.NextSubPanel IsNot Nothing Then
            Me.NextSubPanel.SnapToMinimumSize(includeSubPanels)
        End If

        If m_allowUserVerticalRepositioning Then
            If Me.NextSubPanel IsNot Nothing Then
                Me.Top = Me.NextSubPanel.Top - Me.Height
            Else
                Dim mdiClientControl As MdiClient = GetMDIClient()

                Me.Top = mdiClientControl.Bottom - Me.Height
            End If

            LayoutWindows()
        End If

    End Sub

    Private Sub ShowTitleMenu(Optional ByVal atMousePosition As Boolean = False)

        If atMousePosition = False Then
            Me.TitleContextMenu.Show(Me.TitleLabel, New Point(0, Me.TitleLabel.ClientRectangle.Bottom))
        Else
            Me.TitleContextMenu.Show(Me.TitleLabel, Me.TitlePanel.PointToClient(System.Windows.Forms.Cursor.Position))
        End If

    End Sub

    Private Sub ToggleWindowButtons()

        Me.WindowButtonsPanel.Visible = Not Me.WindowButtonsPanel.Visible

        If Not Me.WindowButtonsPanel.Visible Then
            ShowBalloonTip("Вы можете выровнять или закрыть окна по правой клавише на этой вкладке.", m_windowTabStrips.Item(0))
        End If

    End Sub

    Private Sub UnloadWrappedWindowMenuItems()

        For index As Integer = Me.TitleContextMenu.MenuItems.Count - 1 To 0 Step -1
            Dim mnu As MenuItem = Me.TitleContextMenu.MenuItems.Item(index)

            If TypeOf mnu Is WrappedWindowMenuItem Then
                RemoveHandler mnu.Click, AddressOf HandleWrappedWindowMenuItemClick
                Me.TitleContextMenu.MenuItems.Remove(mnu)
                mnu.Dispose()
            End If
        Next index

    End Sub

    Private Sub CopyAppearance(ByVal toWindowManagerPanel As WindowManagerPanel)

        If toWindowManagerPanel IsNot Nothing Then
            With toWindowManagerPanel
                .EnableTabPaintEvent = Me.EnableTabPaintEvent
                .DisableCloseAction = Me.DisableCloseAction
                .DisableTileAction = Me.DisableTileAction
                .DisableHTileAction = Me.DisableHTileAction
                .DisablePopoutAction = Me.DisablePopoutAction
                .ButtonRenderMode = Me.ButtonRenderMode
                .BackColor = Me.BackColor
                .ForeColor = Me.ForeColor
                .TitleBackColor = Me.TitleBackColor
                .TitleForeColor = Me.TitleForeColor
                .Style = Me.Style
                .ShowTitle = Me.ShowTitle
                .ShowIcons = Me.ShowIcons
                .ShowLayoutButtons = Me.ShowLayoutButtons
                .ShowCloseButton = Me.ShowCloseButton
                .Height = Me.Height
            End With
        End If

    End Sub

    ''' <summary>
    ''' Display a dialog that displays all the windows in all panels.
    ''' </summary>
    Public Function ShowAllWindowsDialog() As DialogResult

        Return ShowAllWindowsDialog(Nothing)

    End Function

    ''' <summary>
    ''' Display a dialog that displays all the windows in all panels.
    ''' </summary>
    Public Function ShowAllWindowsDialog(ByVal owner As IWin32Window) As DialogResult

        Dim frm As New WindowsForm

        frm.CloseWindowsButton.Visible = Not m_disableCloseAction

        frm.LoadWindowsList(GetAllWindows())

        Dim result As DialogResult = frm.ShowDialog(owner)

        'all this code used to be in WindowsForm itself... but bugs in the way MDI focuses child windows
        'let to putting all the work here after the dialog is dismissed.
        If result = DialogResult.OK Then
            Select Case frm.UserActionResult
                Case WindowsForm.UserAction.ActivateWindow
                    If frm.WindowList.SelectedItems.Count > 0 Then
                        ActivateWindow(CType(frm.WindowList.SelectedItems.Item(0).Tag, WrappedWindow).Window)
                    End If
                Case WindowsForm.UserAction.CloseWindows
                    If frm.WindowList.SelectedItems.Count > 0 Then
                        For Each item As ListViewItem In frm.WindowList.SelectedItems
                            Dim wrappedWindow As WrappedWindow = CType(item.Tag, WrappedWindow)

                            With wrappedWindow
                                ShowWindowWithFocus(.Window)
                                .Window.Refresh()
                                .Window.Close()
                            End With
                        Next item
                    End If
            End Select
        End If

        frm.Dispose()

        Return result

    End Function

    ''' <summary>
    ''' Retrieve an array of menu items representing all open windows.
    ''' </summary>
    Public Function GetAllWindowsMenu(Optional ByVal includeTopNineAccelerators As Boolean = False) As MenuItem()

        Return GetAllWindowsMenu(0, includeTopNineAccelerators)

    End Function

    ''' <summary>
    ''' Retrieve an array of menu items representing all open windows.
    ''' </summary>
    Public Function GetAllWindowsMenu(ByVal limit As Integer, Optional ByVal includeTopNineAccelerators As Boolean = False) As MenuItem()

        Dim menuItems As WrappedWindowMenuItem()
        Dim wrappedWindows As WrappedWindowCollection = GetAllWindows()

        If wrappedWindows.Count > 0 Then
            Dim activePanel As WindowManagerPanel = GetActivePanel()
            Dim activeWrappedWindow As WrappedWindow

            If activePanel IsNot Nothing Then
                activeWrappedWindow = activePanel.GetActiveWindow()
            End If

            'ReDim_menuItems(wrappedWindows.Count - 1)
            Re.Dim(menuItems, wrappedWindows.Count - 1)
            Dim i As Integer = 0

            For Each wrappedWindow As WrappedWindow In wrappedWindows
                Dim mnu As New WrappedWindowMenuItem

                If activeWrappedWindow IsNot Nothing AndAlso activeWrappedWindow Is wrappedWindow Then
                    mnu.RadioCheck = True
                    mnu.Checked = True
                End If

                If includeTopNineAccelerators AndAlso i < 9 Then
                    mnu.TextPrefix = "&" & (i + 1).ToString("")
                End If

                mnu.WrappedWindow = wrappedWindow
                AddHandler mnu.Click, AddressOf HandleWindowMenuItemClick
                menuItems(i) = mnu

                i += 1

                If limit > 0 AndAlso i = limit Then
                    Exit For
                End If
            Next wrappedWindow

            'ReDim_Preserve menuItems(i - 1)
            Re.DimPreserve(menuItems, i - 1)
        End If

        Return menuItems

    End Function

    Private Sub CleanUp()

        m_isUnloaded = True

        Dim wrappedWindows As WrappedWindowCollection = GetAllWindows()

        For Each wrappedWindow As WrappedWindow In wrappedWindows
            wrappedWindow.Dispose()
        Next wrappedWindow

        UnloadLeftBlankArea()
        UnloadRightBlankArea()

    End Sub

    Private Sub AddParentHandlers()

        If m_parentForm IsNot Nothing Then
            AddHandler m_parentForm.MdiChildActivate, AddressOf HandleParentMdiChildActivate
            AddHandler m_parentForm.FormClosed, AddressOf HandleParentFormClosed
        End If

    End Sub

    Private Sub RemoveParentHandlers()

        If m_parentForm IsNot Nothing Then
            RemoveHandler m_parentForm.MdiChildActivate, AddressOf HandleParentMdiChildActivate
            RemoveHandler m_parentForm.FormClosed, AddressOf HandleParentFormClosed
        End If

    End Sub

    Private Sub AddWindowToHistory(ByVal wrappedWindow As WrappedWindow)

        If m_historyBack.Count = 0 OrElse Not (m_historyBack.Count > 0 AndAlso m_historyBack.Item(m_historyBack.Count - 1) Is wrappedWindow) Then
            m_historyBack.Add(wrappedWindow)

            If m_historyBack.Count > 20 Then
                m_historyBack.RemoveAt(0)
            End If
        End If

    End Sub

    Private Sub RemoveWindowFromHistory(ByVal wrappedWindow As WrappedWindow)

        For index As Integer = m_historyBack.Count - 1 To 0 Step -1
            If m_historyBack.Item(index) Is wrappedWindow Then
                m_historyBack.RemoveAt(index)
            End If
        Next index

    End Sub

    Friend Function GetWindowHistoryBack() As System.Collections.ObjectModel.ReadOnlyCollection(Of WrappedWindow)

        Return New ObjectModel.ReadOnlyCollection(Of WrappedWindow)(m_historyBack)

    End Function

    Friend Function GetTabStripPreviousWrappedWindow(ByVal windowTabStrip As WindowTabStrip) As WrappedWindow

        Dim result As WrappedWindow
        Dim tabStripWrappedWindows As WrappedWindowCollection = windowTabStrip.Items
        Dim historyBack As System.Collections.ObjectModel.ReadOnlyCollection(Of WrappedWindow) = GetPrimaryPanel().GetWindowHistoryBack()

        For index As Integer = historyBack.Count - 1 To 0 Step -1
            Dim wrappedWindow As WrappedWindow = historyBack.Item(index)

            If tabStripWrappedWindows.Contains(wrappedWindow) Then
                result = wrappedWindow
                Exit For
            End If
        Next index

        'If result Is Nothing AndAlso historyBack.Count > 0 Then
        'result = m_historyBack.Item(historyBack.Count - 1)
        'End If

        Return result

    End Function

    Private Sub TrackActiveMdiChild()

        If m_parentForm IsNot Nothing Then
            Dim activeWindow As Form = m_parentForm.ActiveMdiChild()

            If activeWindow IsNot Nothing Then
                Dim panels As List(Of WindowManagerPanel) = GetPrimaryPanel().GetAllPanels()
                Dim foundPanel As WindowManagerPanel
                Dim foundTabStrip As WindowTabStrip
                Dim found As Boolean

                For Each panel As WindowManagerPanel In panels
                    For Each tabstrip As WindowTabStrip In panel.TabStrips
                        For Each wrappedWindow As WrappedWindow In tabstrip.Items
                            If wrappedWindow.Window Is activeWindow Then
                                foundPanel = panel
                                foundTabStrip = tabstrip
                                found = True
                                Exit For
                            End If
                        Next wrappedWindow
                    Next tabstrip
                Next panel

                If found Then
                    If foundPanel.SelectedTabStrip IsNot foundTabStrip Then
                        foundPanel.SelectedTabStrip = foundTabStrip
                    End If

                    For Each panel As WindowManagerPanel In panels
                        panel.IsActive = (panel Is foundPanel)
                    Next panel
                End If
            End If
        End If

    End Sub

    Private Sub ActivateWindow(ByVal frm As Form)

        ShowWindowWithFocus(frm)
        TrackActiveMdiChild()

    End Sub

#End Region

#Region "Internal Window Layout Routines"

    Private Sub LayoutTabStrips(Optional ByVal dontFitTabStrips As Boolean = False)

        If m_windowTabStrips.Count > 0 Then
            If Me.WindowButtonsPanel.Visible Then
                Me.TabStripsContainer.Width = Me.WindowButtonsPanel.Left - Me.TabStripsContainer.Left - 5
            Else
                Me.TabStripsContainer.Width = Me.WindowButtonsPanel.Right - Me.TabStripsContainer.Left
            End If

            If Not dontFitTabStrips Then
                SetBestTabStripWidths(m_windowTabStrips, Me.TabStripsContainer.ClientRectangle.Width - 1)
            End If

            Dim currentX As Integer = 0

            For Each windowTabStrip As WindowTabStrip In m_windowTabStrips
                With windowTabStrip
                    .Style = m_style
                    .EnableTabPaintEvent = m_enableTabPaintEvent
                    .ShowIcons = m_showIcons
                    .SetBounds(currentX, 0, .Width, Me.TabStripsContainer.ClientRectangle.Height - 5)
                    .Visible = True
                    .Refresh()

                    currentX += .Width
                End With
            Next windowTabStrip
        End If

    End Sub

    Private Sub LayoutWindows()

        If m_isUnloaded Then Exit Sub

        Dim mdiParentForm As Form = GetMDIParent()
        Dim mdiClientControl As MdiClient = GetMDIClient()

        If mdiClientControl IsNot Nothing Then
            Dim myAdjustedMDIBounds As Rectangle = mdiClientControl.RectangleToClient(mdiParentForm.RectangleToScreen(Me.Bounds))
            Dim tabStripAdjustedMDIBounds As Rectangle
            Dim windowTabStrip As WindowTabStrip
            Dim bottomBarrier As Integer

            If m_nextWindowManagerPanel Is Nothing Then
                bottomBarrier = mdiClientControl.ClientRectangle.Bottom
            Else
                bottomBarrier = mdiClientControl.RectangleToClient(mdiParentForm.RectangleToScreen(m_nextWindowManagerPanel.Bounds)).Top
            End If

            If mdiClientControl IsNot Nothing Then
                If m_windowTabStrips.Count > 1 Then
                    For Each windowTabStrip In m_windowTabStrips
                        For Each wrappedWindow As WrappedWindow In windowTabStrip.Items
                            tabStripAdjustedMDIBounds = mdiClientControl.RectangleToClient(Me.TabStripsContainer.RectangleToScreen(windowTabStrip.Bounds))

                            wrappedWindow.Window.SetBounds(tabStripAdjustedMDIBounds.Left, myAdjustedMDIBounds.Bottom, tabStripAdjustedMDIBounds.Width - 3, bottomBarrier - myAdjustedMDIBounds.Bottom)

                            If Not wrappedWindow.Window.Visible Then
                                wrappedWindow.Window.Visible = True
                            End If
                        Next wrappedWindow

                        mdiParentForm.Controls.Add(windowTabStrip.ResizeSplitter)

                        Dim splitterBounds As New Rectangle(tabStripAdjustedMDIBounds.Left + tabStripAdjustedMDIBounds.Width - 3, myAdjustedMDIBounds.Bottom, 3, bottomBarrier - myAdjustedMDIBounds.Bottom)

                        splitterBounds = mdiParentForm.RectangleToClient(mdiClientControl.RectangleToScreen(splitterBounds))
                        windowTabStrip.ResizeSplitter.Bounds = splitterBounds
                        windowTabStrip.ResizeSplitter.BackColor = Me.BackColor
                        windowTabStrip.ResizeSplitter.Visible = True
                    Next windowTabStrip

                    LayoutSideAreas(True, mdiParentForm, mdiClientControl, myAdjustedMDIBounds, bottomBarrier)
                Else
                    If m_windowTabStrips.Count > 0 Then
                        If mdiParentForm.Controls.Contains(m_windowTabStrips.Item(0).ResizeSplitter) Then
                            m_windowTabStrips.Item(0).ResizeSplitter.Visible = False
                            mdiParentForm.Controls.Remove(m_windowTabStrips.Item(0).ResizeSplitter)
                        End If

                        For Each wrappedWindow As WrappedWindow In m_windowTabStrips.Item(0).Items
                            If Me.Orientation = WindowManagerOrientation.Right Then
                                wrappedWindow.Window.SetBounds(myAdjustedMDIBounds.Left + 5, myAdjustedMDIBounds.Bottom, myAdjustedMDIBounds.Width - 5, bottomBarrier - myAdjustedMDIBounds.Bottom)
                            ElseIf Me.Orientation = WindowManagerOrientation.Left Then
                                wrappedWindow.Window.SetBounds(myAdjustedMDIBounds.Left, myAdjustedMDIBounds.Bottom, myAdjustedMDIBounds.Width - 5, bottomBarrier - myAdjustedMDIBounds.Bottom)
                            Else
                                wrappedWindow.Window.SetBounds(myAdjustedMDIBounds.Left, myAdjustedMDIBounds.Bottom, myAdjustedMDIBounds.Width, bottomBarrier - myAdjustedMDIBounds.Bottom)
                            End If

                            'If Not wrappedWindow.Window.Visible Then
                            '    wrappedWindow.Window.Visible = True
                            'End If
                        Next wrappedWindow

                        LayoutSideAreas(False, mdiParentForm, mdiClientControl, myAdjustedMDIBounds, bottomBarrier)
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub LayoutSideAreas(ByVal multipleTabStrips As Boolean, ByVal mdiParentForm As Form, ByVal mdiClientControl As MdiClient, ByVal myAdjustedMDIBounds As Rectangle, ByVal bottomBarrier As Integer)

        If multipleTabStrips OrElse (Me.Orientation = WindowManagerOrientation.Right OrElse Me.Orientation = WindowManagerOrientation.Left) Then
            Dim windowTabStrip As WindowTabStrip
            Dim tabStripAdjustedMDIBounds As Rectangle

            If multipleTabStrips OrElse (Me.Orientation = WindowManagerOrientation.Bottom OrElse Me.Orientation = WindowManagerOrientation.Top) OrElse Me.Orientation = WindowManagerOrientation.Right Then
                If m_blankAreaLeft Is Nothing Then
                    m_blankAreaLeft = New DummyForm

                    With m_blankAreaLeft
                        .MdiParent = mdiParentForm
                        .BackColor = Me.BackColor
                        .Visible = True
                    End With
                End If

                If Me.Orientation = WindowManagerOrientation.Right Then
                    m_blankAreaLeft.WindowManagerSplitter1.Dock = DockStyle.Left
                    m_blankAreaLeft.Enabled = True
                Else
                    m_blankAreaLeft.Enabled = False
                End If

                windowTabStrip = m_windowTabStrips.Item(0)
                tabStripAdjustedMDIBounds = mdiClientControl.RectangleToClient(Me.TabStripsContainer.RectangleToScreen(windowTabStrip.Bounds))

                If multipleTabStrips OrElse (Me.Orientation = WindowManagerOrientation.Bottom OrElse Me.Orientation = WindowManagerOrientation.Top) Then
                    m_blankAreaLeft.SetBounds(myAdjustedMDIBounds.Left, myAdjustedMDIBounds.Bottom, tabStripAdjustedMDIBounds.Left - myAdjustedMDIBounds.Left, bottomBarrier - myAdjustedMDIBounds.Bottom)
                ElseIf Me.Orientation = WindowManagerOrientation.Right Then
                    m_blankAreaLeft.SetBounds(myAdjustedMDIBounds.Left, myAdjustedMDIBounds.Bottom, 5, bottomBarrier - myAdjustedMDIBounds.Bottom)
                End If
            Else
                UnloadLeftBlankArea()
            End If

            '---------------------------

            If multipleTabStrips OrElse (Me.Orientation = WindowManagerOrientation.Bottom OrElse Me.Orientation = WindowManagerOrientation.Top) OrElse Me.Orientation = WindowManagerOrientation.Left Then
                If m_blankAreaRight Is Nothing Then
                    m_blankAreaRight = New DummyForm
                    With m_blankAreaRight
                        .MdiParent = mdiParentForm
                        .BackColor = Me.BackColor
                        .Visible = True
                        .Enabled = False
                    End With
                End If

                If Me.Orientation = WindowManagerOrientation.Left Then
                    m_blankAreaRight.WindowManagerSplitter1.Dock = DockStyle.Right
                    m_blankAreaRight.Enabled = True
                Else
                    m_blankAreaRight.Enabled = False
                End If

                windowTabStrip = m_windowTabStrips.Item(m_windowTabStrips.Count - 1)
                tabStripAdjustedMDIBounds = mdiClientControl.RectangleToClient(Me.TabStripsContainer.RectangleToScreen(windowTabStrip.Bounds))

                If multipleTabStrips OrElse (Me.Orientation = WindowManagerOrientation.Bottom OrElse Me.Orientation = WindowManagerOrientation.Top) Then
                    m_blankAreaRight.SetBounds(tabStripAdjustedMDIBounds.Right, myAdjustedMDIBounds.Bottom, myAdjustedMDIBounds.Right - tabStripAdjustedMDIBounds.Right, bottomBarrier - myAdjustedMDIBounds.Bottom)
                ElseIf Me.Orientation = WindowManagerOrientation.Left Then
                    m_blankAreaRight.SetBounds(myAdjustedMDIBounds.Right - 5, myAdjustedMDIBounds.Bottom, 5, bottomBarrier - myAdjustedMDIBounds.Bottom)
                End If
            Else
                UnloadRightBlankArea()
            End If
        Else
            UnloadLeftBlankArea()
            UnloadRightBlankArea()
        End If

    End Sub

    Private Sub UnloadLeftBlankArea()

        If m_blankAreaLeft IsNot Nothing Then
            m_blankAreaLeft.Dispose()
            m_blankAreaLeft = Nothing
        End If

    End Sub

    Private Sub UnloadRightBlankArea()

        If m_blankAreaRight IsNot Nothing Then
            m_blankAreaRight.Dispose()
            m_blankAreaRight = Nothing
        End If

    End Sub

    Private Sub AttemptAutoHide(Optional ByVal force As Boolean = False)

        If Me.Site Is Nothing OrElse Not Me.Site.DesignMode Then
            If m_autoHide AndAlso Not m_inAutoHideMode AndAlso m_nextWindowManagerPanel Is Nothing Then
                If GetAllWindows(False).Count = 0 OrElse force Then
                    Select Case m_orientation
                        Case WindowManagerOrientation.Bottom, WindowManagerOrientation.Top
                            m_sizeValueBeforeAutoHide = Me.Height
                            Me.Visible = False 'Me.Height = 0
                            m_inAutoHideMode = True
                        Case WindowManagerOrientation.Left, WindowManagerOrientation.Right
                            m_sizeValueBeforeAutoHide = Me.Width
                            Me.Visible = False 'Me.Width = 0
                            m_inAutoHideMode = True
                    End Select

                    ReconcileAuxiliaryWindowSize()
                End If
            End If
        End If

    End Sub

    Private Sub AttemptUndoAutoHide(Optional ByVal force As Boolean = False)

        If m_autoHide AndAlso m_inAutoHideMode Then
            If GetAllWindows(False).Count > 0 OrElse force Then
                Select Case m_orientation
                    Case WindowManagerOrientation.Bottom, WindowManagerOrientation.Top
                        Me.Visible = True 'Me.Height = m_iSizeValueBeforeShrink
                        m_inAutoHideMode = False
                    Case WindowManagerOrientation.Left, WindowManagerOrientation.Right
                        Me.Visible = True 'Me.Width = m_iSizeValueBeforeShrink
                        m_inAutoHideMode = False
                End Select

                ReconcileAuxiliaryWindowSize()
            End If
        End If

    End Sub

    Private Sub SetBestTabStripWidths(ByVal windowTabStrips As WindowTabStripCollection, ByVal canvasWidth As Integer)

        Dim totalWidth As Integer
        Dim correctForZeroWidths As Boolean = True

        For Each windowTabStrip As WindowTabStrip In windowTabStrips
            If windowTabStrip.Width = 0 Then
                correctForZeroWidths = True
            End If
        Next windowTabStrip

        If correctForZeroWidths Then
            For Each windowTabStrip As WindowTabStrip In windowTabStrips
                windowTabStrip.Width = CInt(Me.TabStripsContainer.ClientRectangle.Width / m_windowTabStrips.Count)
            Next windowTabStrip
        End If

        For Each windowTabStrip As WindowTabStrip In windowTabStrips
            totalWidth += windowTabStrip.Width
        Next windowTabStrip

        If totalWidth > 0 Then
            For Each windowTabStrip As WindowTabStrip In windowTabStrips
                windowTabStrip.Width = CInt(canvasWidth * (windowTabStrip.Width / totalWidth))
            Next windowTabStrip
        End If

    End Sub

    Private Sub SnapToSize()

        Try
            Dim parentForm As Form = GetMDIParent()
            Dim mdiClientControl As MdiClient = GetMDIClient()

            If mdiClientControl IsNot Nothing Then
                Dim mdiClientAdjustedBounds As Rectangle = parentForm.RectangleToClient(mdiClientControl.RectangleToScreen(mdiClientControl.ClientRectangle))

                Select Case m_orientation
                    Case WindowManagerOrientation.Bottom
                        Me.SetBounds(mdiClientAdjustedBounds.Left, 0, mdiClientAdjustedBounds.Width, 0, BoundsSpecified.X Or BoundsSpecified.Width)
                    Case WindowManagerOrientation.Top
                        Me.SetBounds(mdiClientAdjustedBounds.Left, mdiClientAdjustedBounds.Top, mdiClientAdjustedBounds.Width, 0, BoundsSpecified.X Or BoundsSpecified.Width Or BoundsSpecified.Y)
                    Case WindowManagerOrientation.Right
                        Me.Left = mdiClientAdjustedBounds.Right - Me.Width
                    Case WindowManagerOrientation.Left
                        Me.Left = mdiClientAdjustedBounds.Left
                End Select

                LayoutWindows()
            End If
        Catch
            'do nothing
        End Try

    End Sub

    Private Sub AdjustForClientTop()

        Try
            Dim mdiClientControl As MdiClient = GetMDIClient()

            If mdiClientControl IsNot Nothing Then
                If Not m_minMode Then
                    If mdiClientControl.Location.Y <> m_originalMDIClientRect.Location.Y Then
                        Dim topDelta As Integer = mdiClientControl.Location.Y - m_originalMDIClientRect.Location.Y

                        Me.Top += topDelta
                    End If
                Else
                    If m_orientation = WindowManagerOrientation.Bottom AndAlso mdiClientControl.Height <> m_originalMDIClientRect.Height Then
                        Dim topDelta As Integer = mdiClientControl.Height - m_originalMDIClientRect.Height

                        Me.Top += topDelta
                    End If
                End If
            End If
        Catch
            'do nothing
        End Try

    End Sub

    Private Sub AdjustForClientLeft()

        Try
            Dim mdiClientControl As MdiClient = GetMDIClient()

            If mdiClientControl IsNot Nothing Then
                If mdiClientControl.Location.X <> m_originalMDIClientRect.Location.X Then
                    Dim leftDelta As Integer = mdiClientControl.Location.X - m_originalMDIClientRect.Location.X

                    Me.Width -= leftDelta
                End If
            End If
        Catch
            'do nothing
        End Try

    End Sub

    Private Sub AdjustForClientWidth()

        Dim mdiClientControl As MdiClient = GetMDIClient()

        If mdiClientControl IsNot Nothing Then
            Select Case m_orientation
                Case WindowManagerOrientation.Right, WindowManagerOrientation.Left
                    If m_originalMDIClientRect.Width <> mdiClientControl.Width Then
                        Dim delta As Integer = mdiClientControl.Width - m_originalMDIClientRect.Width

                        If Me.Width + delta < 40 Then
                            If Not m_minMode Then
                                Me.Width = 40
                            Else
                                m_topValueBeforeMinMode = 40
                            End If
                        Else
                            If Not m_minMode Then
                                Me.Width += delta
                            Else
                                m_topValueBeforeMinMode += delta
                            End If
                        End If
                    End If
            End Select
        End If

    End Sub

    Private Sub AdjustForClientHeight()

        Dim mdiClientControl As MdiClient = GetMDIClient()

        If Me.Top + Me.Height > mdiClientControl.Height Then
            Dim bottomBarrier As Integer = mdiClientControl.Height - Me.Height

            If Not bottomBarrier < mdiClientControl.Top Then
                Dim eventargs As RedoLayoutEventArgs = New RedoLayoutEventArgs(Me, mdiClientControl, True) With {.BottomBarrier = bottomBarrier}

                OnRequestChainRedoLayout(eventargs)

                If Not eventargs.Cancel Then
                    Dim eventargs2 As RedoLayoutEventArgs = New RedoLayoutEventArgs(Me, mdiClientControl, False) With {.BottomBarrier = bottomBarrier}

                    OnRequestChainRedoLayout(eventargs2)

                    Me.Top = bottomBarrier
                End If
            End If
        End If

    End Sub

    Private Sub AdjustForNonVisibleTitle()

        Dim delta As Integer
        Dim originalTop As Integer = Me.WindowButtonsPanel.Top

        If Me.TitlePanel.Visible Then
            Me.WindowButtonsPanel.Top = Me.TitlePanel.Bottom + 1
            Me.TabStripsContainer.Top = Me.WindowButtonsPanel.Top

            delta = Me.WindowButtonsPanel.Top - originalTop
        Else
            Me.WindowButtonsPanel.Top = Me.TitlePanel.Top
            Me.TabStripsContainer.Top = Me.TitlePanel.Top

            delta = Me.WindowButtonsPanel.Top - originalTop
        End If

        Me.TabStripsContainer.Height -= delta

        'we're not going to resize the panel itself...
        'let the developer deal with it
        'Me.Height += iDelta

    End Sub

    Friend Sub ToggleMinModeForBottom()

        If Not m_minMode Then
            If Me.NextSubPanel IsNot Nothing Then
                Me.NextSubPanel.MinMode = True
            End If

            m_topValueBeforeMinMode = Me.Top
            SnapToMinimumSize(includeSubPanels:=False)

            If Me.Orientation <> WindowManagerOrientation.Left AndAlso Me.Orientation <> WindowManagerOrientation.Right Then
                m_minMode = True
            End If
        Else
            RaiseEvent RequestMinModeChange(Me, New RequestMinModeChangeEventArgs(False))

            m_minMode = False

            Me.Top = m_topValueBeforeMinMode
            LayoutWindows()

            If Me.NextSubPanel IsNot Nothing Then
                Me.NextSubPanel.MinMode = False
            End If
        End If

    End Sub

    Friend Sub ToggleMinModeForSides()

        If Me.Orientation = WindowManagerOrientation.Left OrElse Me.Orientation = WindowManagerOrientation.Right Then
            Dim eventargs As New GetTopPanelEventArgs

            OnRequestChainGetTopPanel(eventargs)

            If eventargs.TopPanel IsNot Nothing Then
                eventargs.TopPanel.ToggleMinModeForSides()
            Else
                Dim mdiParentForm As Form = GetMDIParent()
                Dim mdiClientControl As MdiClient = GetMDIClient()

                If Not m_minMode Then
                    m_minMode = True

                    Dim ctl As New WindowManagerPanelMinModeBar

                    ctl.MinModeButton.RotatedText = Me.Text
                    mdiParentForm.Controls.Add(ctl)
                    ctl.BringToFront()

                    Select Case Me.Orientation
                        Case WindowManagerOrientation.Right
                            ctl.Location = New Point(mdiClientControl.Right - ctl.Size.Width, Me.Location.Y)
                            ctl.Anchor = AnchorStyles.Right Or AnchorStyles.Top Or AnchorStyles.Bottom
                        Case WindowManagerOrientation.Left
                            ctl.Location = New Point(mdiClientControl.Left, Me.Location.Y)
                            ctl.Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Bottom
                    End Select

                    ctl.Height = mdiClientControl.Bottom - Me.Location.Y

                    m_minModeBar = ctl

                    m_topValueBeforeMinMode = Me.Width
                    Me.Width = ctl.Width - 5

                    ctl.BringToFront()
                Else
                    m_minMode = False

                    If m_minModeBar IsNot Nothing Then
                        If mdiParentForm.Controls.Contains(m_minModeBar) Then
                            mdiParentForm.Controls.Remove(m_minModeBar)
                        End If

                        m_minModeBar.Dispose()
                        m_minModeBar = Nothing
                    End If

                    Me.Width = m_topValueBeforeMinMode
                End If
            End If
        End If

    End Sub

    Private Sub SnapToDesignSize()

        If Me.Orientation = WindowManagerOrientation.Left OrElse Me.Orientation = WindowManagerOrientation.Right Then
            Dim mdiClientControl As MdiClient = GetMDIClient()

            If mdiClientControl IsNot Nothing Then
                Dim mdiClientAdjustedBounds As Rectangle = ParentForm.RectangleToClient(mdiClientControl.RectangleToScreen(mdiClientControl.ClientRectangle))

                Me.Top = mdiClientAdjustedBounds.Top
                Me.Width = CInt(mdiClientAdjustedBounds.Width / 4)
            End If
        ElseIf Me.Orientation = WindowManagerOrientation.Top Then
            Dim mdiClientControl As MdiClient = GetMDIClient()

            If mdiClientControl IsNot Nothing Then
                Dim mdiClientAdjustedBounds As Rectangle = ParentForm.RectangleToClient(mdiClientControl.RectangleToScreen(mdiClientControl.ClientRectangle))

                'Dim tempVal As Boolean = m_allowUserVerticalRepositioning

                'm_allowUserVerticalRepositioning = True
                Me.Top = mdiClientAdjustedBounds.Top
                'm_allowUserVerticalRepositioning = tempVal
            End If
        End If

    End Sub

    Private Sub ReconcileAuxiliaryWindowSize()

        Dim mdiParentForm As Form = GetMDIParent()
        Dim mdiClientControl As MdiClient = GetMDIClient()

        If mdiClientControl IsNot Nothing Then
            If m_auxiliaryWindow IsNot Nothing Then
                Dim myAdjustedMDIBounds As Rectangle = mdiClientControl.RectangleToClient(mdiParentForm.RectangleToScreen(Me.Bounds))

                Select Case m_orientation
                    Case WindowManagerOrientation.Bottom
                        If Not m_inAutoHideMode Then
                            m_auxiliaryWindow.SetBounds(0, 0, mdiClientControl.ClientSize.Width, myAdjustedMDIBounds.Top)
                        Else
                            m_auxiliaryWindow.SetBounds(0, 0, mdiClientControl.ClientSize.Width, mdiClientControl.ClientSize.Height)
                        End If
                    Case WindowManagerOrientation.Right
                        m_auxiliaryWindow.SetBounds(0, 0, myAdjustedMDIBounds.Left, mdiClientControl.ClientSize.Height)
                    Case WindowManagerOrientation.Left
                        m_auxiliaryWindow.SetBounds(myAdjustedMDIBounds.Width, 0, mdiClientControl.ClientSize.Width - myAdjustedMDIBounds.Width, mdiClientControl.ClientSize.Height)
                    Case WindowManagerOrientation.Top
                        'not doing anything special because we're not expecting an Aux Window when orientation=top
                        'm_auxiliaryWindow.SetBounds(myAdjustedMDIBounds.Width, 0, mdiClientControl.ClientSize.Width - myAdjustedMDIBounds.Width, mdiClientControl.ClientSize.Height)
                End Select
            End If
        End If

    End Sub

    Private Sub ArrangeWindowActionButtons()

        Dim buttons As Control() = {Me.CloseWindowButton, Me.PopoutWindowButton, Me.TileWindowButton, Me.HTileButton}
        Dim pos As Integer = Me.WindowButtonsPanel.ClientSize.Width
        Dim buttonsWidth As Integer = 0

        Me.WindowButtonsPanel.Visible = True 'must do this because for some dumb reason the buttons' visible property isn't accurately reflected by the framework otherwise

        Me.CloseWindowButton.Visible = m_windowCloseButtonVisible
        Me.PopoutWindowButton.Visible = m_windowLayoutButtonsVisible
        Me.TileWindowButton.Visible = m_windowLayoutButtonsVisible
        Me.HTileButton.Visible = m_windowLayoutButtonsVisible

        For Each button As Control In buttons
            If button.Visible Then
                button.Left = pos - button.Width
                buttonsWidth += button.Width
                pos -= button.Width
            End If
        Next button

        Me.WindowButtonsPanel.Width = buttonsWidth
        Me.WindowButtonsPanel.Left = Me.TitlePanel.Right - Me.WindowButtonsPanel.Width - 1

        Me.WindowButtonsPanel.Visible = Not (buttonsWidth = 0)

    End Sub

#End Region

#Region "Protected"

    Protected Overridable Sub OnWindowTiling(ByVal e As WrappedWindowCancelEventArgs)

        RaiseEvent WindowTiling(Me, e)

    End Sub

    Protected Overridable Sub OnWindowUnTiling(ByVal e As WrappedWindowCancelEventArgs)

        RaiseEvent WindowUnTiling(Me, e)

    End Sub

    Protected Overridable Sub OnWindowHTiling(ByVal e As WrappedWindowCancelEventArgs)

        RaiseEvent WindowHTiling(Me, e)

    End Sub

    Protected Overridable Sub OnWindowPoppingOut(ByVal e As WrappedWindowCancelEventArgs)

        RaiseEvent WindowPoppingOut(Me, e)

    End Sub

    Protected Overridable Sub OnWindowPoppingIn(ByVal e As WrappedWindowCancelEventArgs)

        RaiseEvent WindowPoppingIn(Me, e)

    End Sub

    Protected Overridable Sub OnBeforeWrappedWindowAdded(ByVal e As WrappedWindowCancelEventArgs)

        RaiseEvent BeforeWrappedWindowAdded(Me, e)

    End Sub

    Protected Overridable Sub OnWrappedWindowAdded(ByVal e As WrappedWindowEventArgs)

        RaiseEvent WrappedWindowAdded(Me, e)

    End Sub

    Protected Overridable Sub OnBeforeWrappedWindowRemoved(ByVal e As WrappedWindowCancelEventArgs)

        RaiseEvent BeforeWrappedWindowRemoved(Me, e)

    End Sub

    Protected Overridable Sub OnWrappedWindowRemoved(ByVal e As WrappedWindowEventArgs)

        RaiseEvent WrappedWindowRemoved(Me, e)

    End Sub

    Protected Overridable Sub OnWrappedWindowsCleared(ByVal e As EventArgs)

        RaiseEvent WrappedWindowsCleared(Me, e)

    End Sub

    Protected Overridable Sub OnWindowClosing(ByVal e As WrappedWindowClosingEventArgs)

        RaiseEvent WindowClosing(Me, e)

    End Sub

    Protected Overridable Sub OnWindowClosed(ByVal e As WrappedWindowClosedEventArgs)

        If Not Not m_isTemporaryWindowManagerPanel Then
            RemoveWindowFromHistory(e.WrappedWindow)
        End If

        RaiseEvent WindowClosed(Me, e)

    End Sub

    Protected Overridable Sub OnWindowActivated(ByVal e As WrappedWindowEventArgs)

        If Not m_isTemporaryWindowManagerPanel Then
            AddWindowToHistory(e.WrappedWindow)
        End If

        RaiseEvent WindowActivated(Me, e)

    End Sub

    Protected Overridable Sub OnWindowDeactivate(ByVal e As WrappedWindowEventArgs)

        RaiseEvent WindowDeactivate(Me, e)

    End Sub

    Protected Overridable Sub OnWindowEnter(ByVal e As WrappedWindowEventArgs)

        RaiseEvent WindowEnter(Me, e)

    End Sub

    Protected Overridable Sub OnWindowLeave(ByVal e As WrappedWindowEventArgs)

        RaiseEvent WindowLeave(Me, e)

    End Sub

    Protected Overridable Sub OnWindowTextChanged(ByVal e As WrappedWindowEventArgs)

        RaiseEvent WindowTextChanged(Me, e)

    End Sub

    Protected Overridable Sub OnWindowVisibleChanged(ByVal e As WrappedWindowEventArgs)

        RaiseEvent WindowVisibleChanged(Me, e)

    End Sub

    Protected Overridable Sub OnTempPanelDismissed(ByVal e As EventArgs)

        RaiseEvent TempPanelDismissed(Me, e)

    End Sub

    Protected Overridable Sub OnRequestChainRedoLayout(ByVal e As RedoLayoutEventArgs)

        RaiseEvent RequestChainRedoLayout(Me, e)

    End Sub

    Protected Overridable Sub OnRequestChainGetTop(ByVal e As GetTopEventArgs)

        RaiseEvent RequestChainGetTop(Me, e)

    End Sub

    Protected Overridable Sub OnRequestChainGetTopPanel(ByVal e As GetTopPanelEventArgs)

        RaiseEvent RequestChainGetTopPanel(Me, e)

    End Sub

    Protected Overridable Sub OnIsActiveChanged(ByVal e As EventArgs)

        RaiseEvent IsActiveChanged(Me, e)

    End Sub

    Protected Overridable Sub OnNewCustomTabsProviderInstance(ByVal e As NewTabsProviderInstanceCreatedEventArgs)

        RaiseEvent NewCustomTabsProviderInstance(Me, e)

    End Sub

#End Region

#Region "Event Handlers"

    Private Sub WindowManagerPanel_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

        m_isLoaded = True

        AdjustForNonVisibleTitle()

        If m_autoHide Then
            AttemptAutoHide()
        Else
            SnapToSize()
        End If

        DesignerNote1Label.Visible = Me.DesignMode

    End Sub

    Private Sub WindowManagerPanel_BackColorChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Me.BackColorChanged

        Me.WindowManagerSplitter1.BackColor = Me.BackColor
        Me.WindowManagerSplitter2.BackColor = Me.BackColor

    End Sub

    Private Sub WindowManagerPanel_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Resize

        'I don't think we really need to enforce this
        'If Me.Width < MINIMUM_PANEL_WIDTH Then
        'Me.Width = MINIMUM_PANEL_WIDTH
        'End If

        Dim mdiParentForm As Form = GetMDIParent()

        If mdiParentForm IsNot Nothing Then
            If mdiParentForm.WindowState <> FormWindowState.Minimized Then
                SnapToSize()
                ReconcileAuxiliaryWindowSize()
            End If
        End If

    End Sub

    Private Sub WindowManagerPanel_LocationChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.LocationChanged

        SnapToSize()
        ReconcileAuxiliaryWindowSize()

    End Sub

    Private Sub WindowManagerPanel_SizeChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.SizeChanged

        Try
            If m_nextWindowManagerPanel IsNot Nothing Then
                m_nextWindowManagerPanel.Size = Me.Size
            End If
        Catch
            'do nothing
        End Try

    End Sub

    Private Sub WindowManagerPanel_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Enter

        'todo: set focus to selected window here perhaps?
        'Me.IsActive = True
        TrackActiveMdiChild()

    End Sub

    Private Sub WindowManagerPanel_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Leave

        'Me.IsActive = False
        TrackActiveMdiChild()

    End Sub

    Private Sub WindowManagerPanel_VisibleChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.VisibleChanged

        Try
            If m_blankAreaLeft IsNot Nothing Then
                m_blankAreaLeft.Visible = Me.Visible
            End If

            If m_blankAreaRight IsNot Nothing Then
                m_blankAreaRight.Visible = Me.Visible
            End If

            If Me.Visible Then
                SnapToSize()
            End If
        Catch
            'do nothing
        End Try

    End Sub

    Private Sub WindowManagerPanel_ParentChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.ParentChanged

        UnloadLeftBlankArea()
        UnloadRightBlankArea()

        RemoveParentHandlers()

        If Me.Parent IsNot Nothing Then
            If TypeOf Me.Parent Is Form Then
                m_parentForm = DirectCast(Me.Parent, Form)
                AddParentHandlers()
            Else
                Throw New NotSupportedException("Parent must be a Windows Form class.")
            End If
        End If

    End Sub

    Private Sub HandleWindowTabStripItemsFirstAdded(ByVal sender As Object, ByVal e As EventArgs)

        AttemptUndoAutoHide()

    End Sub

    Private Sub HandleWindowTabStripItemsCleared(ByVal sender As Object, ByVal e As EventArgs)

        m_windowTabStrips.Remove(CType(sender, WindowTabStrip))

        AttemptAutoHide()

    End Sub

    Private Sub HandleWindowTabStripEnter(ByVal sender As Object, ByVal e As EventArgs)

        'Me.IsActive = True
        Me.SelectedTabStrip = CType(sender, WindowTabStrip)
        TrackActiveMdiChild()

    End Sub

    Private Sub HandleWindowTabStripLeave(ByVal sender As Object, ByVal e As EventArgs)

        'Me.IsActive = False
        'Me.ActiveWindowTabStrip = Nothing 'let's not do this. I think we'd like to remember the last one for now
        TrackActiveMdiChild()

    End Sub

    Private Sub HandleWindowTabStripNewSplitSize(ByVal sender As Object, ByVal e As WindowTabStrip.WindowTabStripNewSplitSizeEventArgs)

        Dim windowTabStrip As WindowTabStrip = CType(sender, WindowTabStrip)
        Dim windowTabStripIndex As Integer = m_windowTabStrips.IndexOf(windowTabStrip)

        If windowTabStripIndex <> m_windowTabStrips.Count - 1 Then
            Dim windowTabStrip2 As WindowTabStrip = m_windowTabStrips.Item(windowTabStripIndex + 1)

            If windowTabStrip.Width + e.NewSizeDelta > windowTabStrip.ResizeSplitter.Width AndAlso windowTabStrip2.Width - e.NewSizeDelta > windowTabStrip2.ResizeSplitter.Width Then
                windowTabStrip.Width += e.NewSizeDelta
                windowTabStrip2.Width -= e.NewSizeDelta
            End If

            LayoutTabStrips(dontFitTabStrips:=True)
            LayoutWindows()
        End If

    End Sub

    Private Sub HandleWindowTabStripSelectedItemChanged(ByVal sender As Object, ByVal e As SelectedWrappedWindowChangedEventArgs)

        If e.SelectedWrappedWindow IsNot Nothing AndAlso e.SelectedWrappedWindow.Window IsNot Nothing AndAlso Not e.SelectedWrappedWindow.Window.IsDisposed Then
            Dim mdiParentForm As Form = GetMDIParent()

            If e.SelectedWrappedWindow.Window.MdiParent IsNot mdiParentForm Then
                If e.SelectedWrappedWindow.Window.MdiParent IsNot mdiParentForm Then
                    e.SelectedWrappedWindow.Window.MdiParent = mdiParentForm
                End If
            End If

            'e.SelectedWrappedWindow.Window.SetBounds(0 - e.SelectedWrappedWindow.Window.Width, 0 - e.SelectedWrappedWindow.Window.Height, 0, 0, BoundsSpecified.Location)
            CType(sender, WindowTabStrip).BeginUpdate()
            ActivateWindow(e.SelectedWrappedWindow.Window)
            LayoutWindows()
            CType(sender, WindowTabStrip).EndUpdate()
        End If

        Me.MinMode = False

    End Sub

    Private Sub HandleWindowTabStripSelectedItemReselected(ByVal sender As Object, ByVal e As EventArgs)

        Dim wrappedWindow As WrappedWindow = CType(sender, WindowTabStrip).SelectedItem

        If wrappedWindow IsNot Nothing AndAlso wrappedWindow.Window IsNot Nothing AndAlso Not wrappedWindow.Window.IsDisposed Then
            CType(sender, WindowTabStrip).BeginUpdate()
            ActivateWindow(wrappedWindow.Window)
            LayoutWindows()
            CType(sender, WindowTabStrip).EndUpdate()
        End If

        Me.MinMode = False

    End Sub

    Private Sub HandleWindowTabStripItemTabDoubleClicked(ByVal sender As Object, ByVal e As WrappedWindowItemEventArgs)

        PopOutWrappedWindow(e.WrappedWindow)

    End Sub

    Private Sub HandleWindowTabStripRequestPreviousWindowSelect(ByVal sender As Object, ByVal e As WrappedWindowItemEventArgs)

        Dim windowTabStrip As WindowTabStrip = CType(sender, WindowTabStrip)

        If windowTabStrip.Items.Count > 0 Then
            Dim wrappedWindow As WrappedWindow = Me.GetTabStripPreviousWrappedWindow(windowTabStrip)

            If wrappedWindow IsNot Nothing Then
                windowTabStrip.SelectedItem = wrappedWindow
            Else
                windowTabStrip.SelectedItem = windowTabStrip.Items.Item(0)
                'todo: IN ADDITION we may want to set focus on next window in back history
            End If
        Else
            'todo: we may want to set focus on next window in back history
        End If

    End Sub

    Private Sub HandleWindowTabStripTabPaint(ByVal sender As Object, ByVal e As StandardTabsProvider.TabPaintEventArgs)

        RaiseEvent TabPaint(Me, e)

    End Sub

    Private Sub MDIClient_RepositionAndResize(ByVal sender As Object, ByVal e As EventArgs) Handles m_mdiClientControl.Resize, m_mdiClientControl.LocationChanged

        Dim mdiParentForm As Form = GetMDIParent()

        If mdiParentForm IsNot Nothing Then
            If mdiParentForm.WindowState <> FormWindowState.Minimized Then
                'adjustments must occur in this order
                AdjustForClientWidth()
                SnapToSize()
                AdjustForClientTop()
                AdjustForClientLeft()
                AdjustForClientHeight()
                ReconcileAuxiliaryWindowSize()

                Dim mdiClient As MdiClient = GetMDIClient()

                If mdiClient IsNot Nothing Then
                    m_originalMDIClientRect = mdiClient.Bounds
                End If
            End If
        End If

    End Sub

    Private Sub HandleWindowTabStripBeginDragItem(ByVal sender As Object, ByVal e As WrappedWindowItemEventArgs)

        Dim windowTabStrip As WindowTabStrip = CType(sender, WindowTabStrip)
        Dim data As New WrappedWindowDragDropData(Me, windowTabStrip, e.WrappedWindow)

        windowTabStrip.DoDragDrop(data, DragDropEffects.Move)

    End Sub

    Private Sub HandleWindowTabStripItemDropped(ByVal sender As Object, ByVal e As DroppedWrappedWindowEventArgs)

        Dim windowTabStrip As WindowTabStrip = CType(sender, WindowTabStrip)
        Dim wrappedWindow As WrappedWindow = e.Data.WrappedWindow
        Dim sourceWindowTabStrip As WindowTabStrip = e.Data.WindowTabStrip

        sourceWindowTabStrip.Items.Remove(wrappedWindow)
        windowTabStrip.Items.Add(wrappedWindow)

        SetActiveWindow(wrappedWindow)

    End Sub

    Private Sub HandleWindowTabStripShowWindowMenuRequested(ByVal sender As Object, ByVal e As EventArgs)

        If Not (m_disableTileAction AndAlso m_disableHTileAction AndAlso m_disablePopoutAction AndAlso m_disableCloseAction) Then
            WindowContextMenu.Show(Me, Me.PointToClient(System.Windows.Forms.Cursor.Position))
        End If

    End Sub

    Private Sub WindowTabStrips_TabStripAdded(ByVal sender As Object, ByVal e As WindowTabStripCollection.ItemsEventArgs) Handles m_windowTabStrips.TabStripAdded

        Me.TabStripsContainer.Controls.Add(e.WindowTabStrip)

        With e.WindowTabStrip
            .Width = CInt(Me.TabStripsContainer.ClientRectangle.Width / m_windowTabStrips.Count)

            AddHandler .Enter, AddressOf HandleWindowTabStripEnter
            AddHandler .Leave, AddressOf HandleWindowTabStripLeave
            AddHandler .SelectedItemChanged, AddressOf HandleWindowTabStripSelectedItemChanged
            AddHandler .SelectedItemReselected, AddressOf HandleWindowTabStripSelectedItemReselected
            AddHandler .ItemsFirstAdded, AddressOf HandleWindowTabStripItemsFirstAdded
            AddHandler .ItemsCleared, AddressOf HandleWindowTabStripItemsCleared
            AddHandler .NewSplitSize, AddressOf HandleWindowTabStripNewSplitSize
            AddHandler .BeginDragItem, AddressOf HandleWindowTabStripBeginDragItem
            AddHandler .ItemDropped, AddressOf HandleWindowTabStripItemDropped
            AddHandler .ShowWindowMenuRequested, AddressOf HandleWindowTabStripShowWindowMenuRequested
            AddHandler .ItemTabDoubleClicked, AddressOf HandleWindowTabStripItemTabDoubleClicked
            AddHandler .RequestPreviousWindowSelect, AddressOf HandleWindowTabStripRequestPreviousWindowSelect
            AddHandler .TabPaint, AddressOf HandleWindowTabStripTabPaint

            AddHandler .Items.BeforeWrappedWindowAdded, AddressOf HandleBeforeWrappedWindowAdded
            AddHandler .Items.BeforeWrappedWindowRemoved, AddressOf HandleBeforeWrappedWindowRemoved
            AddHandler .Items.WrappedWindowAdded, AddressOf HandleWrappedWindowAdded
            AddHandler .Items.WrappedWindowRemoved, AddressOf HandleWrappedWindowRemoved
            AddHandler .Items.WrappedWindowsCleared, AddressOf HandleWrappedWindowsCleared
            AddHandler .Items.WindowActivated, AddressOf HandleWindowActivated
            AddHandler .Items.WindowClosed, AddressOf HandleWindowClosed
            AddHandler .Items.WindowClosing, AddressOf HandleWindowClosing
            AddHandler .Items.WindowDeactivate, AddressOf HandleWindowDeactivate
            AddHandler .Items.WindowEnter, AddressOf HandleWindowEnter
            AddHandler .Items.WindowLeave, AddressOf HandleWindowLeave
            AddHandler .Items.WindowTextChanged, AddressOf HandleWindowTextChanged
            AddHandler .Items.WindowVisibleChanged, AddressOf HandleWindowVisibleChanged
        End With

        LayoutTabStrips()

    End Sub

    Private Sub WindowTabStrips_BeforeTabStripRemoved(ByVal sender As Object, ByVal e As WindowTabStripCollection.ItemsCancelEventArgs) Handles m_windowTabStrips.BeforeTabStripRemoved

        If m_windowTabStrips.Count = 1 AndAlso Not m_isTemporaryWindowManagerPanel Then
            e.Cancel = True
        End If

    End Sub

    Private Sub WindowTabStrips_TabStripRemoved(ByVal sender As Object, ByVal e As WindowTabStripCollection.ItemsEventArgs) Handles m_windowTabStrips.TabStripRemoved, m_windowTabStrips.TabStripClearWarn

        With e.WindowTabStrip
            RemoveHandler .Enter, AddressOf HandleWindowTabStripEnter
            RemoveHandler .Leave, AddressOf HandleWindowTabStripLeave
            RemoveHandler .SelectedItemChanged, AddressOf HandleWindowTabStripSelectedItemChanged
            RemoveHandler .SelectedItemReselected, AddressOf HandleWindowTabStripSelectedItemReselected
            RemoveHandler .ItemsFirstAdded, AddressOf HandleWindowTabStripItemsFirstAdded
            RemoveHandler .ItemsCleared, AddressOf HandleWindowTabStripItemsCleared
            RemoveHandler .NewSplitSize, AddressOf HandleWindowTabStripNewSplitSize
            RemoveHandler .BeginDragItem, AddressOf HandleWindowTabStripBeginDragItem
            RemoveHandler .ItemDropped, AddressOf HandleWindowTabStripItemDropped
            RemoveHandler .ShowWindowMenuRequested, AddressOf HandleWindowTabStripShowWindowMenuRequested
            RemoveHandler .ItemTabDoubleClicked, AddressOf HandleWindowTabStripItemTabDoubleClicked
            RemoveHandler .RequestPreviousWindowSelect, AddressOf HandleWindowTabStripRequestPreviousWindowSelect
            RemoveHandler .TabPaint, AddressOf HandleWindowTabStripTabPaint

            RemoveHandler .Items.BeforeWrappedWindowAdded, AddressOf HandleBeforeWrappedWindowAdded
            RemoveHandler .Items.BeforeWrappedWindowRemoved, AddressOf HandleBeforeWrappedWindowRemoved
            RemoveHandler .Items.WrappedWindowAdded, AddressOf HandleWrappedWindowAdded
            RemoveHandler .Items.WrappedWindowRemoved, AddressOf HandleWrappedWindowRemoved
            RemoveHandler .Items.WrappedWindowsCleared, AddressOf HandleWrappedWindowsCleared
            RemoveHandler .Items.WindowActivated, AddressOf HandleWindowActivated
            RemoveHandler .Items.WindowClosed, AddressOf HandleWindowClosed
            RemoveHandler .Items.WindowClosing, AddressOf HandleWindowClosing
            RemoveHandler .Items.WindowDeactivate, AddressOf HandleWindowDeactivate
            RemoveHandler .Items.WindowEnter, AddressOf HandleWindowEnter
            RemoveHandler .Items.WindowLeave, AddressOf HandleWindowLeave
            RemoveHandler .Items.WindowTextChanged, AddressOf HandleWindowTextChanged
            RemoveHandler .Items.WindowVisibleChanged, AddressOf HandleWindowVisibleChanged

            Me.TabStripsContainer.Controls.Remove(e.WindowTabStrip)

            Dim mdiParentForm As Form = GetMDIParent()

            If mdiParentForm IsNot Nothing Then
                If mdiParentForm.Controls.Contains(.ResizeSplitter) Then
                    .ResizeSplitter.Visible = False
                    mdiParentForm.Controls.Remove(.ResizeSplitter)
                End If
            End If
        End With

        If Me.SelectedTabStrip Is e.WindowTabStrip Then
            If Not Me.TabStrips.Count = 0 Then
                Me.SelectedTabStrip = Me.TabStrips.Item(0)
            Else
                Me.SelectedTabStrip = Nothing
            End If
        End If

        LayoutTabStrips()
        LayoutWindows()

    End Sub

    Private Sub WindowTabStrips_TabStripsCleared(ByVal sender As Object, ByVal e As EventArgs) Handles m_windowTabStrips.TabStripsCleared

        Me.TabStripsContainer.Controls.Clear()
        LayoutTabStrips()
        LayoutWindows()

        If m_isTemporaryWindowManagerPanel Then
            OnTempPanelDismissed(EventArgs.Empty)
            Me.Parent.Controls.Remove(Me)
            Me.Dispose()
        Else
            InitializePrimaryTabStrip()
        End If

    End Sub

    Private Sub HandleBeforeWrappedWindowAdded(ByVal sender As Object, ByVal e As WrappedWindowCollection.ItemsCancelEventArgs)

        If Not e.Cancel Then
            Dim wrappedWindow As WrappedWindow = GetWrapperForWindow(e.WrappedWindow.Window)

            If Not Me.IsTemporaryPanel AndAlso wrappedWindow IsNot Nothing Then
                'todo: why is this here?
                'this is a check to make sure to make sure the window isn't already being handled
                'maybe we should throw an exception instead?
                e.Cancel = True
            Else
                Dim eventargs As New WrappedWindowCancelEventArgs(e.WrappedWindow, GetWrappedWindowTabStrip(e.WrappedWindow, False), Me)
                OnBeforeWrappedWindowAdded(eventargs)
                e.Cancel = eventargs.Cancel
            End If
        End If

    End Sub

    Private Sub HandleWrappedWindowAdded(ByVal sender As Object, ByVal e As WrappedWindowCollection.ItemsEventArgs)

        OnWrappedWindowAdded(New WrappedWindowEventArgs(e.WrappedWindow, GetWrappedWindowTabStrip(e.WrappedWindow), Me))

    End Sub

    Private Sub HandleBeforeWrappedWindowRemoved(ByVal sender As Object, ByVal e As WrappedWindowCollection.ItemsCancelEventArgs)

        If Not e.Cancel Then
            Dim eventargs As New WrappedWindowCancelEventArgs(e.WrappedWindow, GetWrappedWindowTabStrip(e.WrappedWindow), Me)

            OnBeforeWrappedWindowRemoved(eventargs)

            e.Cancel = eventargs.Cancel
        End If

    End Sub

    Private Sub HandleWrappedWindowRemoved(ByVal sender As Object, ByVal e As WrappedWindowCollection.ItemsEventArgs)

        OnWrappedWindowRemoved(New WrappedWindowEventArgs(e.WrappedWindow, GetWrappedWindowTabStrip(e.WrappedWindow), Me))

    End Sub

    Private Sub HandleWrappedWindowsCleared(ByVal sender As Object, ByVal e As EventArgs)

        OnWrappedWindowsCleared(EventArgs.Empty)

    End Sub

    Private Sub HandleWindowClosing(ByVal sender As Object, ByVal e As WrappedWindowCollection.ItemsClosingEventArgs)

        If Not e.Cancel Then
            Dim eventargs As New WrappedWindowClosingEventArgs(e.WrappedWindow, GetWrappedWindowTabStrip(e.WrappedWindow), Me, e.CloseReason)

            OnWindowClosing(eventargs)

            e.Cancel = eventargs.Cancel
        End If

    End Sub

    Private Sub HandleWindowClosed(ByVal sender As Object, ByVal e As WrappedWindowCollection.ItemsClosedEventArgs)

        OnWindowClosed(New WrappedWindowClosedEventArgs(e.WrappedWindow, GetWrappedWindowTabStrip(e.WrappedWindow), Me, e.CloseReason))

    End Sub

    Private Sub HandleWindowActivated(ByVal sender As Object, ByVal e As WrappedWindowCollection.ItemsEventArgs)

        OnWindowActivated(New WrappedWindowEventArgs(e.WrappedWindow, GetWrappedWindowTabStrip(e.WrappedWindow), Me))

    End Sub

    Private Sub HandleWindowDeactivate(ByVal sender As Object, ByVal e As WrappedWindowCollection.ItemsEventArgs)

        OnWindowDeactivate(New WrappedWindowEventArgs(e.WrappedWindow, GetWrappedWindowTabStrip(e.WrappedWindow), Me))

    End Sub

    Private Sub HandleWindowEnter(ByVal sender As Object, ByVal e As WrappedWindowCollection.ItemsEventArgs)

        OnWindowEnter(New WrappedWindowEventArgs(e.WrappedWindow, GetWrappedWindowTabStrip(e.WrappedWindow), Me))

    End Sub

    Private Sub HandleWindowLeave(ByVal sender As Object, ByVal e As WrappedWindowCollection.ItemsEventArgs)

        OnWindowLeave(New WrappedWindowEventArgs(e.WrappedWindow, GetWrappedWindowTabStrip(e.WrappedWindow), Me))

    End Sub

    Private Sub HandleWindowTextChanged(ByVal sender As Object, ByVal e As WrappedWindowCollection.ItemsEventArgs)

        OnWindowTextChanged(New WrappedWindowEventArgs(e.WrappedWindow, GetWrappedWindowTabStrip(e.WrappedWindow), Me))

    End Sub

    Private Sub HandleWindowVisibleChanged(ByVal sender As Object, ByVal e As WrappedWindowCollection.ItemsEventArgs)

        OnWindowVisibleChanged(New WrappedWindowEventArgs(e.WrappedWindow, GetWrappedWindowTabStrip(e.WrappedWindow), Me))

    End Sub

    Private Sub HandleWindowMenuItemClick(ByVal sender As Object, ByVal e As EventArgs)

        Dim mnu As WrappedWindowMenuItem = CType(sender, WrappedWindowMenuItem)

        If mnu.WrappedWindow IsNot Nothing Then
            If mnu.WrappedWindow.Window IsNot Nothing Then
                ActivateWindow(mnu.WrappedWindow.Window)
            End If
        End If

    End Sub

    Private Sub HandleParentMdiChildActivate(ByVal sender As Object, ByVal e As EventArgs)

        'todo: We might also achieve the same functionality and reduce flicker if we 
        'use events of the hidden magical MDIClient control (such as moving the window
        'out of the visible area as soon as its added and before its shown). However,
        'this seems kludgy to me and subject to future incompatabilities. Will investigate further.

        If Not m_isTemporaryWindowManagerPanel Then
            If m_autoDetectMdiChildWindows Then
                Try
                    Dim mdiChildForm As Form = m_parentForm.ActiveMdiChild

                    If mdiChildForm IsNot Nothing AndAlso Not mdiChildForm.IsDisposed AndAlso mdiChildForm IsNot Me.AuxiliaryWindow AndAlso Not TypeOf mdiChildForm Is DummyForm Then
                        Dim wrappedWindow As WrappedWindow = GetWrapperForWindow(mdiChildForm)

                        If wrappedWindow Is Nothing Then
                            wrappedWindow = New WrappedWindow(mdiChildForm)
                            mdiChildForm.SetBounds(0 - mdiChildForm.Width, 0 - mdiChildForm.Height, 0, 0, BoundsSpecified.Location)
                            _AddWindow(wrappedWindow)
                            '_AddWindow(mdiChildForm)
                            SetActiveWindow(mdiChildForm)
                        Else
                            SetActiveWindow(wrappedWindow)
                        End If
                    End If
                Catch
                    'do nothing
                End Try
            End If

            TrackActiveMdiChild()
        End If

    End Sub

    Private Sub HandleParentFormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs)

        CleanUp()

    End Sub

    Private Sub WindowManagerSplitter1_NewSize(ByVal sender As Object, ByVal e As NewSizeEventArgs) Handles WindowManagerSplitter1.NewSize

        If m_allowUserVerticalRepositioning Then
            Dim mdiParentForm As Form = GetMDIParent()
            Dim mdiClientControl As MdiClient = GetMDIClient()
            Dim adjustedBounds As Rectangle = mdiParentForm.RectangleToClient(e.Rectangle)

            With adjustedBounds
                If .Y < mdiClientControl.Top Then
                    .Y = mdiClientControl.Top
                End If

                Dim bottomBarrier As Integer

                If m_nextWindowManagerPanel Is Nothing Then
                    bottomBarrier = mdiClientControl.Bottom
                Else
                    bottomBarrier = m_nextWindowManagerPanel.Top
                End If

                If .Y + Me.Height > bottomBarrier Then
                    .Y = bottomBarrier - Me.Height
                End If

                If Me.Top <> .Y Then
                    m_minMode = False

                    Me.Top = .Y

                    LayoutWindows()
                End If
            End With
        End If

    End Sub

    Private Sub TabStripsContainer_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles TabStripsContainer.Resize

        Dim mdiParentForm As Form = GetMDIParent()

        If mdiParentForm IsNot Nothing Then
            If mdiParentForm.WindowState <> FormWindowState.Minimized Then
                LayoutTabStrips()
                LayoutWindows()
            End If
        End If

    End Sub

    Private Sub CloseWindowButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CloseWindowButton.Click

        UserCloseSelectedWindow()

    End Sub

    Private Sub TileWindowButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileWindowButton.Click

        TileOrUntileWrappedWindow(GetSelectedWindow())

    End Sub

    Private Sub PopoutWindowButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles PopoutWindowButton.Click

        PopOutWrappedWindow(GetSelectedWindow())

    End Sub

    Private Sub m_nextWindowManagerPanel_LocationChanged(ByVal sender As Object, ByVal e As EventArgs) Handles m_nextWindowManagerPanel.LocationChanged

        If m_nextWindowManagerPanel.Top < Me.Bottom Then
            m_nextWindowManagerPanel.Top = Me.Bottom
        End If

        LayoutWindows()

    End Sub

    Private Sub m_nextWindowManagerPanel_TempPanelDismissed(ByVal sender As Object, ByVal e As EventArgs) Handles m_nextWindowManagerPanel.TempPanelDismissed

        Me.NextSubPanel = m_nextWindowManagerPanel.NextSubPanel
        LayoutTabStrips()
        LayoutWindows()

        If Not m_isTemporaryWindowManagerPanel Then
            AttemptAutoHide()
        End If

        If Me.GetActiveWindow() IsNot Nothing Then
            ActivateWindow(Me.GetActiveWindow().Window)
        End If

    End Sub

    Private Sub m_nextWindowManagerPanel_RequestChainGetTop(ByVal sender As Object, ByVal e As GetTopEventArgs) Handles m_nextWindowManagerPanel.RequestChainGetTop

        e.AbsoluteTop = Me.Top

        OnRequestChainGetTop(e)

    End Sub

    Private Sub m_nextWindowManagerPanel_SizeChanged(ByVal sender As Object, ByVal e As EventArgs) Handles m_nextWindowManagerPanel.SizeChanged

        Me.Size = m_nextWindowManagerPanel.Size

    End Sub

    Private Sub m_nextWindowManagerPanel_WindowActivated(ByVal sender As Object, ByVal e As WrappedWindowEventArgs) Handles m_nextWindowManagerPanel.WindowActivated

        OnWindowActivated(e)

    End Sub

    Private Sub m_nextWindowManagerPanel_WindowClosed(ByVal sender As Object, ByVal e As WrappedWindowClosedEventArgs) Handles m_nextWindowManagerPanel.WindowClosed

        OnWindowClosed(e)

    End Sub

    Private Sub m_nextWindowManagerPanel_WindowClosing(ByVal sender As Object, ByVal e As WrappedWindowClosingEventArgs) Handles m_nextWindowManagerPanel.WindowClosing

        OnWindowClosing(e)

    End Sub

    Private Sub m_nextWindowManagerPanel_WindowDeactivate(ByVal sender As Object, ByVal e As WrappedWindowEventArgs) Handles m_nextWindowManagerPanel.WindowDeactivate

        OnWindowDeactivate(e)

    End Sub

    Private Sub m_nextWindowManagerPanel_WindowEnter(ByVal sender As Object, ByVal e As WrappedWindowEventArgs) Handles m_nextWindowManagerPanel.WindowEnter

        OnWindowEnter(e)

    End Sub

    Private Sub m_nextWindowManagerPanel_WindowHTiling(ByVal sender As Object, ByVal e As WrappedWindowCancelEventArgs) Handles m_nextWindowManagerPanel.WindowHTiling

        OnWindowHTiling(e)

    End Sub

    Private Sub m_nextWindowManagerPanel_WindowLeave(ByVal sender As Object, ByVal e As WrappedWindowEventArgs) Handles m_nextWindowManagerPanel.WindowLeave

        OnWindowLeave(e)

    End Sub

    Private Sub m_nextWindowManagerPanel_WindowPoppingIn(ByVal sender As Object, ByVal e As WrappedWindowCancelEventArgs) Handles m_nextWindowManagerPanel.WindowPoppingIn

        OnWindowPoppingIn(e)

    End Sub

    Private Sub m_nextWindowManagerPanel_WindowPoppingOut(ByVal sender As Object, ByVal e As WrappedWindowCancelEventArgs) Handles m_nextWindowManagerPanel.WindowPoppingOut

        OnWindowPoppingOut(e)

    End Sub

    Private Sub m_nextWindowManagerPanel_WindowTextChanged(ByVal sender As Object, ByVal e As WrappedWindowEventArgs) Handles m_nextWindowManagerPanel.WindowTextChanged

        OnWindowTextChanged(e)

    End Sub

    Private Sub m_nextWindowManagerPanel_WindowTiling(ByVal sender As Object, ByVal e As WrappedWindowCancelEventArgs) Handles m_nextWindowManagerPanel.WindowTiling

        OnWindowTiling(e)

    End Sub

    Private Sub m_nextWindowManagerPanel_WindowUnTiling(ByVal sender As Object, ByVal e As WrappedWindowCancelEventArgs) Handles m_nextWindowManagerPanel.WindowUnTiling

        OnWindowUnTiling(e)

    End Sub

    Private Sub m_nextWindowManagerPanel_WindowVisibleChanged(ByVal sender As Object, ByVal e As WrappedWindowEventArgs) Handles m_nextWindowManagerPanel.WindowVisibleChanged

        OnWindowVisibleChanged(e)

    End Sub

    Private Sub m_nextWindowManagerPanel_WrappedWindowAdded(ByVal sender As Object, ByVal e As WrappedWindowEventArgs) Handles m_nextWindowManagerPanel.WrappedWindowAdded

        OnWrappedWindowAdded(e)

    End Sub

    Private Sub m_nextWindowManagerPanel_WrappedWindowRemoved(ByVal sender As Object, ByVal e As WrappedWindowEventArgs) Handles m_nextWindowManagerPanel.WrappedWindowRemoved

        OnWrappedWindowRemoved(e)

    End Sub

    Private Sub m_nextWindowManagerPanel_WrappedWindowsCleared(ByVal sender As Object, ByVal e As EventArgs) Handles m_nextWindowManagerPanel.WrappedWindowsCleared

        OnWrappedWindowsCleared(e)

    End Sub

    Private Sub m_nextWindowManagerPanel_BeforeWrappedWindowRemoved(ByVal sender As Object, ByVal e As WrappedWindowCancelEventArgs) Handles m_nextWindowManagerPanel.BeforeWrappedWindowRemoved

        OnBeforeWrappedWindowRemoved(e)

    End Sub

    Private Sub m_nextWindowManagerPanel_BeforeWrappedWindowAdded(ByVal sender As Object, ByVal e As WrappedWindowCancelEventArgs) Handles m_nextWindowManagerPanel.BeforeWrappedWindowAdded

        OnBeforeWrappedWindowAdded(e)

    End Sub

    Private Sub m_nextWindowManagerPanel_NewTabsProviderInstanceCreated(ByVal sender As Object, ByVal e As NewTabsProviderInstanceCreatedEventArgs) Handles m_nextWindowManagerPanel.NewCustomTabsProviderInstance

        OnNewCustomTabsProviderInstance(e)

    End Sub

    Private Sub m_nextWindowManagerPanel_TabPaint(ByVal sender As Object, ByVal e As StandardTabsProvider.TabPaintEventArgs) Handles m_nextWindowManagerPanel.TabPaint

        'simply bubble up the event
        RaiseEvent TabPaint(Me, e)

    End Sub

    Private Sub HTileButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles HTileButton.Click

        HTileWrappedWindow(GetSelectedWindow())

    End Sub

    Private Sub m_nextWindowManagerPanel_RequestChainRedoLayout(ByVal sender As Object, ByVal e As RedoLayoutEventArgs) Handles m_nextWindowManagerPanel.RequestChainRedoLayout

        Dim newTop As Integer = Me.Top

        If Me.Top + Me.Height > e.BottomBarrier Then
            newTop = e.BottomBarrier - Me.Height
        End If

        If e.MDIClient.PointToClient(Me.Parent.PointToScreen(New Point(Me.Left, newTop))).Y < -2 Then
            e.Cancel = True
        Else
            e.BottomBarrier = newTop
            OnRequestChainRedoLayout(e)

            If Not e.Test Then
                Me.Top = newTop
            End If
        End If

    End Sub

    Private Sub m_nextWindowManagerPanel_RequestMinModeChange(ByVal sender As Object, ByVal e As RequestMinModeChangeEventArgs) Handles m_nextWindowManagerPanel.RequestMinModeChange

        Me.MinMode = e.MinModeValue

    End Sub

    Private Sub OrientationSplitters_ResizePending(ByVal sender As Object, ByVal e As BeforeSizeEventArgs) Handles m_blankAreaLeft.ResizePending, m_blankAreaRight.ResizePending, WindowManagerSplitter2.BeforeSize

        Dim eventargs As New GetTopEventArgs() With {.AbsoluteTop = Me.Top}
        OnRequestChainGetTop(eventargs)

        Dim point As Point = GetMDIParent.PointToScreen(New Point(0, eventargs.AbsoluteTop))
        point.X = e.Rectangle.X

        Dim mdiClientControl As MdiClient = GetMDIClient()

        Dim rect As New Rectangle(point, New Size(e.Rectangle.Width, mdiClientControl.Parent.PointToScreen(New Point(0, mdiClientControl.Bottom)).Y - point.Y))

        e.Rectangle = rect

    End Sub

    Private Sub OrientationSplitters_ResizeRequested(ByVal sender As Object, ByVal e As NewSizeEventArgs) Handles m_blankAreaLeft.ResizeRequested, m_blankAreaRight.ResizeRequested, WindowManagerSplitter2.NewSize

        Dim mdiClientControl As MdiClient = GetMDIClient()

        If mdiClientControl IsNot Nothing Then
            Dim mdiParentForm As Form = GetMDIParent()
            Dim mdiClientAdjustedBounds As Rectangle = mdiParentForm.RectangleToClient(mdiClientControl.RectangleToScreen(mdiClientControl.ClientRectangle))
            Dim adjustedBounds As Rectangle = mdiParentForm.RectangleToClient(e.Rectangle)

            Select Case Me.Orientation
                Case WindowManagerOrientation.Left
                    Dim newLeft As Integer

                    If adjustedBounds.Left <= mdiClientAdjustedBounds.Right Then
                        newLeft = adjustedBounds.Left
                    Else
                        newLeft = mdiClientAdjustedBounds.Right
                    End If

                    If newLeft - Me.Left >= MINIMUM_PANEL_WIDTH Then
                        Me.Width = newLeft - Me.Left
                    Else
                        Me.Width = MINIMUM_PANEL_WIDTH
                    End If
                Case WindowManagerOrientation.Right
                    Dim newRight As Integer

                    If adjustedBounds.Right >= mdiClientAdjustedBounds.Left Then
                        newRight = adjustedBounds.Left
                    Else
                        newRight = mdiClientAdjustedBounds.Left
                    End If

                    If Me.Right - newRight >= MINIMUM_PANEL_WIDTH Then
                        Me.Width = Me.Right - newRight
                    Else
                        Me.Width = MINIMUM_PANEL_WIDTH
                    End If
            End Select
        End If

    End Sub

    Private Sub WindowButtons_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles PopoutWindowButton.MouseDown, CloseWindowButton.MouseDown, TileWindowButton.MouseDown, HTileButton.MouseDown

        If e.Button = Windows.Forms.MouseButtons.Right Then
            Dim ctl As Control = CType(sender, Control)

            WindowButtonsContextMenu.Show(ctl, ctl.PointToClient(System.Windows.Forms.Cursor.Position))
        End If

    End Sub

    Private Sub Title_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles TitleLabel.MouseEnter, TitleLabelBackground.MouseEnter, TitleLabelMenuGlyph.MouseEnter

        'm_tmpOriginalBackColorCache1 = TitleLabel.BackColor
        'm_tmpOriginalForeColorCache1 = TitleLabel.ForeColor

        'm_tmpOriginalBackColorCache2 = TitleLabelMenuGlyph.BackColor
        'm_tmpOriginalForeColorCache2 = TitleLabelMenuGlyph.ForeColor

        Me.TitleLabel.BackColor = SystemColors.Highlight
        Me.TitleLabel.ForeColor = SystemColors.HighlightText

        Me.TitleLabelBackground.BackColor = SystemColors.Highlight
        Me.TitleLabelBackground.ForeColor = SystemColors.HighlightText

        Me.TitleLabelMenuGlyph.BackColor = SystemColors.Highlight
        Me.TitleLabelMenuGlyph.ForeColor = SystemColors.HighlightText

    End Sub

    Private Sub Title_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles TitleLabel.MouseLeave, TitleLabelBackground.MouseLeave, TitleLabelMenuGlyph.MouseLeave

        Me.TitleLabel.BackColor = m_titleBackColor
        Me.TitleLabel.ForeColor = m_titleForeColor

        Me.TitleLabelBackground.BackColor = m_titleBackColor
        Me.TitleLabelBackground.ForeColor = m_titleForeColor

        Me.TitleLabelMenuGlyph.BackColor = m_titleBackColor
        Me.TitleLabelMenuGlyph.ForeColor = m_titleForeColor

    End Sub

    Private Sub Title_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles TitleLabel.MouseDown, TitleLabelBackground.MouseDown, TitleLabelMenuGlyph.MouseDown

        If e.Button = Windows.Forms.MouseButtons.Left Then
            ShowTitleMenu()
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            ShowTitleMenu(atMousePosition:=True)
        End If

    End Sub

    Private Sub TitleLabel_SizeChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TitleLabel.SizeChanged

        Me.TitleLabelBackground.Width = Me.TitleLabel.Width
        Me.TitleLabelMenuGlyph.Left = Me.TitleLabel.Right

    End Sub

    Private Sub TitleLabelMenuGlyph_LocationChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TitleLabelMenuGlyph.LocationChanged

        If Me.TitleLabelMenuGlyph.Right > Me.TitlePanel.ClientRectangle.Right Then
            Me.TitleLabelMenuGlyph.Left = Me.TitlePanel.ClientRectangle.Right - Me.TitleLabelMenuGlyph.Width
        End If

    End Sub

    Private Sub TitlePanel_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles TitlePanel.MouseDown

        If e.Button = Windows.Forms.MouseButtons.Right Then
            ShowTitleMenu(atMousePosition:=True)
        End If

    End Sub

    Private Sub TitlePanel_SizeChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TitlePanel.SizeChanged

        If Me.TitleLabel.Right + Me.TitleLabelMenuGlyph.Width > Me.TitlePanel.ClientRectangle.Right Then
            Me.TitleLabelMenuGlyph.Left = Me.TitlePanel.ClientRectangle.Right - Me.TitleLabelMenuGlyph.Width
        Else
            Me.TitleLabelMenuGlyph.Left = Me.TitleLabel.Right
        End If

    End Sub

    Private Sub WindowButtonsPanel_VisibleChanged(ByVal sender As Object, ByVal e As EventArgs) Handles WindowButtonsPanel.VisibleChanged

        If Me.WindowButtonsPanel.Visible Then
            Me.TabStripsContainer.Width = Me.WindowButtonsPanel.Left - Me.TabStripsContainer.Left - 5
        Else
            Me.TabStripsContainer.Width = Me.WindowButtonsPanel.Right - Me.TabStripsContainer.Left
        End If

    End Sub

    Private Sub WindowButtonsPanel_SizeChanged(ByVal sender As Object, ByVal e As EventArgs) Handles WindowButtonsPanel.SizeChanged, WindowButtonsPanel.LocationChanged

        If Me.WindowButtonsPanel.Visible AndAlso Me.WindowButtonsPanel.Width > 0 Then
            Me.TabStripsContainer.Width = Me.WindowButtonsPanel.Left - Me.TabStripsContainer.Left - 5
        End If

    End Sub

    Private Sub WindowButtonsToggleWindowButtonsMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles WindowButtonsToggleWindowButtonsMenuItem.Click

        Me.ShowLayoutButtons = Not Me.ShowLayoutButtons

    End Sub

    Private Sub WindowButtonsContextMenu_Popup(ByVal sender As Object, ByVal e As EventArgs) Handles WindowButtonsContextMenu.Popup

        Me.WindowButtonsToggleWindowButtonsMenuItem.Checked = Not Me.ShowLayoutButtons

    End Sub

    Private Sub WindowContextMenu_Popup(ByVal sender As Object, ByVal e As EventArgs) Handles WindowContextMenu.Popup

        Me.WindowTileWindowMenuItem.Enabled = Not m_disableTileAction
        Me.WindowHTileMenuItem.Enabled = Not m_disableHTileAction
        Me.WindowPopoutWindowMenuItem.Enabled = Not m_disablePopoutAction

        If m_disableTileAction AndAlso m_disableHTileAction AndAlso m_disablePopoutAction Then
            Me.WindowTileWindowMenuItem.Visible = False
            Me.WindowHTileMenuItem.Visible = False
            Me.WindowPopoutWindowMenuItem.Visible = False
            Me.WindowMenuSep1.Visible = False
        Else
            Me.WindowTileWindowMenuItem.Visible = True
            Me.WindowHTileMenuItem.Visible = True
            Me.WindowPopoutWindowMenuItem.Visible = True
            Me.WindowMenuSep1.Visible = True
        End If

        Me.WindowCloseMenuItem.Enabled = Not m_disableCloseAction
        Me.WindowCloseMenuItem.Visible = Not m_disableCloseAction

        If Me.WindowMenuSep1.Visible Then
            Me.WindowMenuSep1.Visible = Not m_disableCloseAction
        End If

    End Sub

    Private Sub TitleContextMenu_Popup(ByVal sender As Object, ByVal e As EventArgs) Handles TitleContextMenu.Popup

        Me.TitleOptionsTileWindowMenuItem.Enabled = Not m_disableTileAction
        Me.TitleOptionsHTileMenuItem.Enabled = Not m_disableHTileAction
        Me.TitleOptionsPopoutWindowMenuItem.Enabled = Not m_disablePopoutAction

        If m_disableTileAction AndAlso m_disableHTileAction AndAlso m_disablePopoutAction Then
            Me.TitleOptionsTileWindowMenuItem.Visible = False
            Me.TitleOptionsHTileMenuItem.Visible = False
            Me.TitleOptionsPopoutWindowMenuItem.Visible = False
            Me.TitleMenuSep2.Visible = False
        Else
            Me.TitleOptionsTileWindowMenuItem.Visible = True
            Me.TitleOptionsHTileMenuItem.Visible = True
            Me.TitleOptionsPopoutWindowMenuItem.Visible = True
            Me.TitleMenuSep2.Visible = True
        End If

        Me.TitleCloseWindowMenuItem.Enabled = Not m_disableCloseAction
        Me.TitleCloseWindowMenuItem.Visible = Not m_disableCloseAction
        Me.TitleMenuSep3.Visible = Not m_disableCloseAction

        Me.TitleMinimizeMenuItem.Checked = Me.MinMode
        'Me.TitleOptionsToggleWindowButtonsMenuItem.Checked = Not Me.ShowLayoutButtons

        UnloadWrappedWindowMenuItems()

        Dim count As Integer = 0

        For Each windowTabStrip As WindowTabStrip In m_windowTabStrips
            For Each wrappedWindow As WrappedWindow In windowTabStrip.Items
                Dim mnu As New WrappedWindowMenuItem() With {.RadioCheck = True, .WrappedWindow = wrappedWindow}
                AddHandler mnu.Click, AddressOf HandleWrappedWindowMenuItemClick

                If windowTabStrip Is Me.SelectedTabStrip Then
                    If wrappedWindow Is windowTabStrip.SelectedItem Then
                        mnu.Checked = True
                    End If
                End If

                Me.TitleContextMenu.MenuItems.Add(mnu)
                mnu.Visible = True

                count += 1
            Next wrappedWindow
        Next windowTabStrip

        Me.TitleNoWindowsMenuItem.Visible = (count = 0)

    End Sub

    Private Sub HandleWrappedWindowMenuItemClick(ByVal sender As Object, ByVal e As EventArgs)

        Dim mnu As WrappedWindowMenuItem = CType(sender, WrappedWindowMenuItem)

        Dim windowTabStrip As WindowTabStrip = GetWrappedWindowTabStrip(mnu.WrappedWindow, False)

        If windowTabStrip IsNot Nothing Then
            Me.SelectedTabStrip = windowTabStrip
            windowTabStrip.SelectedItem = mnu.WrappedWindow
            ActivateWindow(windowTabStrip.SelectedItem.Window)
        End If

    End Sub

    Private Sub TitlePanel_VisibleChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TitlePanel.VisibleChanged

        AdjustForNonVisibleTitle()

    End Sub

    Private Sub TitleOptionsToggleWindowButtonsMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)

        Me.ShowLayoutButtons = Not Me.ShowLayoutButtons

    End Sub

    Private Sub TitleOptionsHTileMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TitleOptionsHTileMenuItem.Click

        HTileWrappedWindow(GetSelectedWindow())

    End Sub

    Private Sub TitleOptionsTileWindowMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TitleOptionsTileWindowMenuItem.Click

        TileOrUntileWrappedWindow(GetSelectedWindow())

    End Sub

    Private Sub TitleOptionsPopoutWindowMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TitleOptionsPopoutWindowMenuItem.Click

        PopOutWrappedWindow(GetSelectedWindow())

    End Sub

    Private Sub TitleCloseWindowMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TitleCloseWindowMenuItem.Click

        UserCloseSelectedWindow()

    End Sub

    Private Sub WindowHTileMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles WindowHTileMenuItem.Click

        HTileWrappedWindow(GetSelectedWindow())

    End Sub

    Private Sub WindowTileWindowMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles WindowTileWindowMenuItem.Click

        TileOrUntileWrappedWindow(GetSelectedWindow())

    End Sub

    Private Sub WindowPopoutWindowMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles WindowPopoutWindowMenuItem.Click

        PopOutWrappedWindow(GetSelectedWindow())

    End Sub

    Private Sub WindowCloseMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles WindowCloseMenuItem.Click

        UserCloseSelectedWindow()

    End Sub

    Private Sub WindowManagerSplitter1_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) Handles WindowManagerSplitter1.DoubleClick

        ToggleMinModeForBottom()

    End Sub

    Private Sub WindowManagerSplitter2_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) Handles WindowManagerSplitter2.DoubleClick, m_blankAreaLeft.DoubleClick, m_blankAreaRight.DoubleClick

        'this is confusing to the user. let them use the Minimize menu option instead.
        'ToggleMinModeForSides()

    End Sub

    Private Sub m_nextWindowManagerPanel_RequestChainGetTopPanel(ByVal sender As Object, ByVal e As GetTopPanelEventArgs) Handles m_nextWindowManagerPanel.RequestChainGetTopPanel

        OnRequestChainGetTopPanel(e)

        If e.TopPanel Is Nothing Then
            e.TopPanel = Me
        End If

    End Sub

    Private Sub m_minModeBar_MinModeButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles m_minModeBar.MinModeButtonClick

        ToggleMinModeForSides()

    End Sub

    Private Sub m_auxiliaryWindow_LocationChanged(ByVal sender As Object, ByVal e As EventArgs) Handles m_auxiliaryWindow.LocationChanged

        ReconcileAuxiliaryWindowSize()

    End Sub

    Private Sub m_auxiliaryWindow_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles m_auxiliaryWindow.Resize

        ReconcileAuxiliaryWindowSize()

    End Sub

    Private Sub TitleMinimizeMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TitleMinimizeMenuItem.Click

        ToggleMinMode()

    End Sub

    Private Sub m_poppedOutWindowsManager_PopInRequested(ByVal sender As Object, ByVal e As WrappedWindowCollection.ItemsEventArgs) Handles m_poppedOutWindowsManager.PopInRequested

        If Not e.Handled Then
            If Not Me.IsTemporaryPanel Then
                Try
                    If m_poppedOutWindowsManager.GetWindowParent(e.WrappedWindow) Is GetMDIParent() Then
                        PopInWrappedWindow(e.WrappedWindow)
                        e.Handled = True
                    End If
                Catch
                    'do nothing
                End Try
            End If
        End If

    End Sub

#End Region

#Region "Nested Classes"

    Protected Friend Class TempPanelDismissedEventArgs
        Inherits EventArgs

        Private m_nextWindowTabStrip As WindowTabStrip

        Public Sub New(ByVal nextWindowTabStrip As WindowTabStrip)

            m_nextWindowTabStrip = nextWindowTabStrip

        End Sub

        Public ReadOnly Property WindowTabStrip() As WindowTabStrip

            Get
                Return m_nextWindowTabStrip
            End Get

        End Property

    End Class

    Protected Friend Class RedoLayoutEventArgs
        Inherits CancelEventArgs

        Private m_originalWindowManagerPanel As WindowManagerPanel
        Private m_bottomBarrier As Integer
        Private m_test As Boolean = False
        Private m_mdiClientControl As MdiClient

        Public Sub New(ByVal originalWindowManagerPanel As WindowManagerPanel, ByVal mdiClientControl As MdiClient, ByVal test As Boolean)

            m_originalWindowManagerPanel = originalWindowManagerPanel
            m_mdiClientControl = mdiClientControl
            m_test = test

        End Sub

        Public ReadOnly Property OriginalWindowManagerPanel() As WindowManagerPanel

            Get
                Return m_originalWindowManagerPanel
            End Get

        End Property

        Public ReadOnly Property Test() As Boolean

            Get
                Return m_test
            End Get

        End Property

        Public Property BottomBarrier() As Integer

            Get
                Return m_bottomBarrier
            End Get

            Set(ByVal value As Integer)
                m_bottomBarrier = value
            End Set

        End Property

        Public ReadOnly Property MDIClient() As MdiClient

            Get
                Return m_mdiClientControl
            End Get

        End Property

    End Class

    Protected Friend Class GetTopEventArgs
        Inherits EventArgs

        Private m_absoluteTop As Integer

        Public Property AbsoluteTop() As Integer

            Get
                Return m_absoluteTop
            End Get

            Set(ByVal value As Integer)
                m_absoluteTop = value
            End Set

        End Property

    End Class

    Protected Friend Class GetTopPanelEventArgs
        Inherits EventArgs

        Private m_topPanel As WindowManagerPanel

        Public Property TopPanel() As WindowManagerPanel

            Get
                Return m_topPanel
            End Get

            Set(ByVal value As WindowManagerPanel)
                m_topPanel = value
            End Set

        End Property

    End Class

    Protected Friend Class RequestMinModeChangeEventArgs
        Inherits EventArgs

        Private m_minMode As Boolean

        Public Sub New()

        End Sub

        Public Sub New(ByVal minModeValue As Boolean)

            m_minMode = minModeValue

        End Sub

        Public Property MinModeValue() As Boolean

            Get
                Return m_minMode
            End Get

            Set(ByVal value As Boolean)
                m_minMode = value
            End Set

        End Property

    End Class

#End Region

End Class