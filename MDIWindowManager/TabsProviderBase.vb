Imports System.ComponentModel
Imports System.Collections.ObjectModel

''' <summary>
''' Provides a starting point and contains a lot of the plumbing for TabsProviders.
''' </summary>
''' <remarks>By inheriting from this class instead of simply implementing ITabsProvider 
''' all a TabsProvider has to to do is drop some Tab control in the designer and 
''' respond to a couple of events.</remarks>
<ToolboxItemAttribute(False)>
Public Class TabsProviderBase
    Implements ITabsProvider

    Public Event BeginDragTabItem As EventHandler(Of WrappedWindowItemEventArgs) Implements ITabsProvider.BeginDragTabItem
    Public Event TabItemDropped As EventHandler(Of DroppedWrappedWindowEventArgs) Implements ITabsProvider.TabItemDropped
    Public Event SelectedTabItemChanged As EventHandler(Of SelectedWrappedWindowChangedEventArgs) Implements ITabsProvider.SelectedTabItemChanged
    Public Event SelectedTabItemReselected As EventHandler Implements ITabsProvider.SelectedTabItemReselected
    Public Event ShowWindowCmdMenuRequested As EventHandler Implements ITabsProvider.ShowWindowCmdMenuRequested
    Public Event TabItemDoubleClicked As EventHandler(Of WrappedWindowItemEventArgs) Implements ITabsProvider.TabItemDoubleClicked

    Friend WithEvents m_wrappedWindowItems As New WrappedWindowCollection
    Private m_selectedWrappedWindowItem As WrappedWindow
    Private m_updating As Boolean = False
    Private m_emphasizeSelectedTab As Boolean = False
    Private m_showIcons As Boolean = True
    Private m_suppressSelectionChangeEvents As Boolean = False

    Public ReadOnly Property WrappedWindowItems() As ReadOnlyCollection(Of WrappedWindow) Implements ITabsProvider.WrappedWindowItems

        Get
            Return New ReadOnlyCollection(Of WrappedWindow)(DirectCast(m_wrappedWindowItems, System.Collections.Generic.IList(Of WrappedWindow)))
        End Get

    End Property

    Friend Property WrappedWindowsCollection() As WrappedWindowCollection Implements ITabsProvider.WrappedWindowsCollection

        Get
            Return m_wrappedWindowItems
        End Get

        Set(ByVal value As WrappedWindowCollection)
            m_wrappedWindowItems = value
        End Set

    End Property

    Public Property SelectedWrappedWindowItem() As WrappedWindow Implements ITabsProvider.SelectedWrappedWindowItem

        Get
            Return m_selectedWrappedWindowItem
        End Get

        Set(ByVal value As WrappedWindow)
            SetSelectedWrappedWindowItemDirect(value, False)
        End Set

    End Property

    Friend Sub SetSelectedWrappedWindowItemDirect(ByVal value As WrappedWindow, ByVal suppressEvents As Boolean) Implements ITabsProvider.SetSelectedWrappedWindowItemDirect

        If suppressEvents Then
            m_suppressSelectionChangeEvents = True
        End If

        If value IsNot m_selectedWrappedWindowItem OrElse suppressEvents Then
            Dim previouslySelectedWrappedWindow As WrappedWindow = m_selectedWrappedWindowItem

            m_selectedWrappedWindowItem = value

            OnSelectedTabItemChanged(New SelectedWrappedWindowChangedEventArgs(m_selectedWrappedWindowItem, previouslySelectedWrappedWindow))
        Else
            If value IsNot Nothing Then
                OnSelectedTabItemReselected(EventArgs.Empty)
            End If
        End If

        If suppressEvents Then
            m_suppressSelectionChangeEvents = False
        End If

    End Sub

    Public ReadOnly Property IsUpdating() As Boolean Implements ITabsProvider.IsUpdating

        Get
            Return m_updating
        End Get

    End Property

    Protected Friend Property EmphasizeSelectedTab() As Boolean Implements ITabsProvider.EmphasizeSelectedTab

        Get
            Return m_emphasizeSelectedTab
        End Get

        Set(ByVal value As Boolean)
            If m_emphasizeSelectedTab <> value Then
                m_emphasizeSelectedTab = value
                ProcessEmphasizeSelectedTabChanged()
            End If
        End Set

    End Property

    Public Property ShowIcons() As Boolean Implements ITabsProvider.ShowIcons

        Get
            Return m_showIcons
        End Get

        Set(ByVal value As Boolean)
            If m_showIcons <> value Then
                m_showIcons = value
                ProcessShowIconsChanged()
            End If
        End Set

    End Property

    Public Overridable Sub BeginUpdate() Implements ITabsProvider.BeginUpdate

        m_updating = True

    End Sub

    Public Overridable Sub EndUpdate() Implements ITabsProvider.EndUpdate

        m_updating = False

    End Sub

    Public Overridable Sub EnsureVisible() Implements ITabsProvider.EnsureVisible

        'to be implemented by inherited classes

    End Sub

    Public Overridable Sub ShowWindowsMenu() Implements ITabsProvider.ShowWindowsMenu

        ShowWindowsMenu(Me.PointToClient(Cursor.Position))

    End Sub

    Public Overridable Sub ShowWindowsMenu(ByVal pos As Point) Implements ITabsProvider.ShowWindowsMenu

        Me.WindowsMenu.Show(Me, pos)

    End Sub

    Private Sub m_wrappedWindowItems_WindowTextChanged(ByVal sender As Object, ByVal e As WrappedWindowCollection.ItemsEventArgs) Handles m_wrappedWindowItems.WindowTextChanged

        ProcessWindowItemTextChanged(e.WrappedWindow)

    End Sub

    Private Sub m_wrappedWindowItems_WrappedWindowAdded(ByVal sender As Object, ByVal e As WrappedWindowCollection.ItemsEventArgs) Handles m_wrappedWindowItems.WrappedWindowAdded

        ProcessWindowItemAdded(e.WrappedWindow)

    End Sub

    Private Sub m_wrappedWindowItems_WrappedWindowRemoved(ByVal sender As Object, ByVal e As WrappedWindowCollection.ItemsEventArgs) Handles m_wrappedWindowItems.WrappedWindowRemoved

        ProcessWindowItemRemoved(e.WrappedWindow)

    End Sub

    Private Sub m_wrappedWindowItems_WrappedWindowsCleared(ByVal sender As Object, ByVal e As EventArgs) Handles m_wrappedWindowItems.WrappedWindowsCleared

        ProcessWindowItemsCleared()

    End Sub

    Private Sub WindowsMenu_Popup(ByVal sender As Object, ByVal e As EventArgs) Handles WindowsMenu.Popup

        UnloadWrappedWindowMenuItems()

        Dim count As Integer = 0

        For Each wrappedWindow As WrappedWindow In m_wrappedWindowItems
            Dim mnu As New WrappedWindowMenuItem() With {.RadioCheck = True, .WrappedWindow = wrappedWindow}
            AddHandler mnu.Click, AddressOf HandleWrappedWindowMenuItemClick

            If wrappedWindow Is Me.SelectedWrappedWindowItem Then
                mnu.Checked = True
            End If

            Me.WindowsMenu.MenuItems.Add(mnu)
            mnu.Visible = True

            count += 1
        Next wrappedWindow

        Me.WindowsNoWindowsMenuItem.Visible = (count = 0)

    End Sub

    Private Sub UnloadWrappedWindowMenuItems()

        For index As Integer = WindowsMenu.MenuItems.Count - 1 To 0 Step -1
            Dim mnu As MenuItem = Me.WindowsMenu.MenuItems.Item(index)

            If TypeOf mnu Is WrappedWindowMenuItem Then
                RemoveHandler mnu.Click, AddressOf HandleWrappedWindowMenuItemClick
                Me.WindowsMenu.MenuItems.Remove(mnu)
                mnu.Dispose()
            End If
        Next index

    End Sub

    Private Sub HandleWrappedWindowMenuItemClick(ByVal sender As Object, ByVal e As EventArgs)

        Dim mnu As WrappedWindowMenuItem = CType(sender, WrappedWindowMenuItem)

        If m_wrappedWindowItems.Contains(mnu.WrappedWindow) Then
            Me.SelectedWrappedWindowItem = mnu.WrappedWindow
        End If

    End Sub

    Protected Overridable Sub ProcessWindowItemAdded(ByVal wrappedWindow As WrappedWindow)

        'to be implemented by inherited classes

    End Sub

    Protected Overridable Sub ProcessWindowItemRemoved(ByVal wrappedWindow As WrappedWindow)

        'to be implemented by inherited classes

    End Sub

    Protected Overridable Sub ProcessWindowItemsCleared()

        'to be implemented by inherited classes

    End Sub

    Protected Overridable Sub ProcessWindowItemTextChanged(ByVal wrappedWindow As WrappedWindow)

        'to be implemented by inherited classes

    End Sub

    Protected Overridable Sub ProcessEmphasizeSelectedTabChanged()

        'to be implemented by inherited classes

    End Sub

    Protected Overridable Sub ProcessShowIconsChanged()

        'to be implemented by inherited classes

    End Sub

    Protected Overridable Sub OnBeginDragTabItem(ByVal e As WrappedWindowItemEventArgs)

        RaiseEvent BeginDragTabItem(Me, e)

    End Sub

    Protected Overridable Sub OnTabItemDropped(ByVal e As DroppedWrappedWindowEventArgs)

        RaiseEvent TabItemDropped(Me, e)

    End Sub

    Protected Overridable Sub OnSelectedTabItemChanged(ByVal e As SelectedWrappedWindowChangedEventArgs)

        If Not m_suppressSelectionChangeEvents Then
            RaiseEvent SelectedTabItemChanged(Me, e)
        End If

        EnsureVisible()

    End Sub

    Protected Overridable Sub OnSelectedTabItemReselected(ByVal e As EventArgs)

        If Not m_suppressSelectionChangeEvents Then
            RaiseEvent SelectedTabItemReselected(Me, e)
        End If

        EnsureVisible()

    End Sub

    Protected Overridable Sub OnShowWindowCmdMenuRequested(ByVal e As EventArgs)

        RaiseEvent ShowWindowCmdMenuRequested(Me, e)

    End Sub

    Protected Overridable Sub OnTabItemDoubleClicked(ByVal e As WrappedWindowItemEventArgs)

        RaiseEvent TabItemDoubleClicked(Me, e)

    End Sub

End Class