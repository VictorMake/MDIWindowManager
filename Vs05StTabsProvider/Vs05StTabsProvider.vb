Imports System.ComponentModel

''' <summary>
''' Alternative TabsProvider that uses mimics the VS2005 tabs.
''' </summary>
''' <remarks>This class inherits from TabsProviderBase</remarks>
<ToolboxItemAttribute(False)> _
Public Class Vs05StTabsProvider

    'used to allow the user a margin of error before initiating a drag on a tab
    Private m_mouseDownStartPoint As Point
    'used to prevent a cascade of events when a tab is selected
    Private m_ignoreTabControlIndexChangedEvent As Boolean = False

    'A window has been added to the collection. Update the TabControl.
    Protected Overrides Sub ProcessWindowItemAdded(ByVal wrappedWindow As MDIWindowManager.WrappedWindow)

        Dim tabPage As New FarsiLibrary.Win.FATabStripItem

        tabPage.Title = wrappedWindow.Window.Text
        tabPage.Tag = wrappedWindow

        Me.FaTabStrip1.Items.Add(tabPage)

    End Sub

    'A window has been removed from the collection. Update the TabControl.
    Protected Overrides Sub ProcessWindowItemRemoved(ByVal wrappedWindow As MDIWindowManager.WrappedWindow)

        Dim deadTabPage As FarsiLibrary.Win.FATabStripItem = FindTabPage(wrappedWindow)

        If Not deadTabPage Is Nothing Then
            Me.FaTabStrip1.Items.Remove(deadTabPage)
        End If

    End Sub

    'All the windows have been removed. Clean up.
    Protected Overrides Sub ProcessWindowItemsCleared()

        Me.FaTabStrip1.Items.Clear()

    End Sub

    'The title text of a window has changed. Update it's corresponding tab in the TabControl.
    Protected Overrides Sub ProcessWindowItemTextChanged(ByVal wrappedWindow As MDIWindowManager.WrappedWindow)

        Dim tabPage As FarsiLibrary.Win.FATabStripItem = FindTabPage(wrappedWindow)

        If Not tabPage Is Nothing Then
            tabPage.Title = wrappedWindow.Window.Text
        End If

    End Sub

    'We're being told to emphasize the selected window because There may be different tabstrips on 
    'the parent MDI window due to tiling and the user should be able to tell what is the ONE 
    'truly "active" window with focus in the container.
    Protected Overrides Sub ProcessEmphasizeSelectedTabChanged()

        ApplySelectedTabEmphasis()

    End Sub

    'The selected window has changed. Update the TabControl.
    Protected Overrides Sub OnSelectedTabItemChanged(ByVal e As MDIWindowManager.SelectedWrappedWindowChangedEventArgs)

        MyBase.OnSelectedTabItemChanged(e)

        Dim tabPage As FarsiLibrary.Win.FATabStripItem = FindTabPage(Me.SelectedWrappedWindowItem)

        m_ignoreTabControlIndexChangedEvent = True
        Me.FaTabStrip1.SelectedItem = tabPage
        m_ignoreTabControlIndexChangedEvent = False

        ApplySelectedTabEmphasis()

    End Sub

    'The selected tab has been reselected... take this change to update the TabControl.
    Protected Overrides Sub OnSelectedTabItemReselected(ByVal e As System.EventArgs)

        MyBase.OnSelectedTabItemReselected(e)

        ApplySelectedTabEmphasis()

    End Sub

    Private Function FindTabPage(ByVal wrappedWindow As MDIWindowManager.WrappedWindow) As FarsiLibrary.Win.FATabStripItem

        Dim result As FarsiLibrary.Win.FATabStripItem

        For Each tabPage As FarsiLibrary.Win.FATabStripItem In Me.FaTabStrip1.Items
            If tabPage.Tag Is wrappedWindow Then
                result = tabPage
                Exit For
            End If
        Next tabPage

        Return result

    End Function

    Private Sub ApplySelectedTabEmphasis()

        Me.FaTabStrip1.EmphasizeSelectedItem = Me.EmphasizeSelectedTab

    End Sub

    Private Sub SetSelectedTabItemByPoint(ByVal pos As Point)

        Dim tabPage As FarsiLibrary.Win.FATabStripItem = Me.FaTabStrip1.GetTabItemByPoint(pos)

        If Not tabPage Is Nothing Then
            Me.FaTabStrip1.SelectedItem = tabPage
        End If

    End Sub

    Private Sub FaTabStrip1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FaTabStrip1.Click

        'the click event of the TabControl is the only way to determine if the user clicks on an already selected tab
        If Not Me.FaTabStrip1.SelectedItem Is Nothing AndAlso Me.FaTabStrip1.SelectedItem.Tag Is Me.SelectedWrappedWindowItem Then
            Me.SelectedWrappedWindowItem = Me.SelectedWrappedWindowItem 'this will trigger a reselect (see OnSelectedTabItemReselected) 
        End If

    End Sub

    Private Sub FaTabStrip1_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles FaTabStrip1.DragDrop

        'determine if we're interested in the data being dragged
        If Not e.Data.GetDataPresent(GetType(MDIWindowManager.WrappedWindowDragDropData)) Then
            e.Effect = DragDropEffects.None
        Else
            Dim data As MDIWindowManager.WrappedWindowDragDropData = CType(e.Data.GetData(GetType(MDIWindowManager.WrappedWindowDragDropData)), MDIWindowManager.WrappedWindowDragDropData)

            If data.WindowTabStrip Is Me.Parent Then
                'essentially do nothing... we handled tab rearranging in real-time during the dragover itself
                e.Effect = DragDropEffects.Move
            Else
                'a new tab was dropped... offload the work to the TabsProviderBase
                OnTabItemDropped(New MDIWindowManager.DroppedWrappedWindowEventArgs(data))
            End If
        End If

    End Sub

    Private Sub FaTabStrip1_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles FaTabStrip1.DragOver

        'determine if we're interested in the data being dragged
        If Not e.Data.GetDataPresent(GetType(MDIWindowManager.WrappedWindowDragDropData)) Then
            e.Effect = DragDropEffects.None
        Else
            Dim data As MDIWindowManager.WrappedWindowDragDropData = CType(e.Data.GetData(GetType(MDIWindowManager.WrappedWindowDragDropData)), MDIWindowManager.WrappedWindowDragDropData)

            If data.WindowTabStrip Is Me.Parent Then
                'user is reordering the tabs
                Dim tabPage As FarsiLibrary.Win.FATabStripItem = Me.FaTabStrip1.GetTabItemByPoint(Me.PointToClient(New Point(e.X, e.Y)))

                If Not tabPage Is Nothing AndAlso Not tabPage Is Me.FaTabStrip1.SelectedItem Then
                    Dim index As Integer = Me.FaTabStrip1.Items.IndexOf(tabPage)
                    Dim tabPageX As FarsiLibrary.Win.FATabStripItem = Me.FaTabStrip1.SelectedItem

                    m_ignoreTabControlIndexChangedEvent = True
                    Me.FaTabStrip1.Items.Remove(tabPageX)
                    Me.FaTabStrip1.Items.Insert(index, tabPageX)
                    Me.FaTabStrip1.SelectedItem = tabPageX
                    m_ignoreTabControlIndexChangedEvent = False
                    Me.FaTabStrip1.Refresh()
                End If

                e.Effect = DragDropEffects.Move
            Else
                'this tab comes from a different tab strip
                e.Effect = DragDropEffects.Move
            End If
        End If

    End Sub

    Private Sub FaTabStrip1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles FaTabStrip1.MouseDown

        'this will allow us to give the user a little margin of movement before initiating a drag
        m_mouseDownStartPoint = e.Location

        'right-click- select the tab at the mouse location and request that the WindowManagerPanel menu be shown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            SetSelectedTabItemByPoint(e.Location)
            Me.FaTabStrip1.Refresh()
            If Not Me.SelectedWrappedWindowItem Is Nothing Then
                OnShowWindowCmdMenuRequested(EventArgs.Empty)
            End If
        End If

    End Sub

    Private Sub FaTabStrip1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles FaTabStrip1.MouseMove

        If e.Button = Windows.Forms.MouseButtons.Left AndAlso Not Me.SelectedWrappedWindowItem Is Nothing Then
            If Math.Abs(e.Location.X - m_mouseDownStartPoint.X) >= 3 OrElse Math.Abs(e.Location.Y - m_mouseDownStartPoint.Y) >= 3 Then
                OnBeginDragTabItem(New MDIWindowManager.WrappedWindowItemEventArgs(Me.SelectedWrappedWindowItem))
            End If
        End If

    End Sub

    Private Sub FaTabStrip1_TabStripItemSelectionChanged(ByVal e As FarsiLibrary.Win.TabStripItemChangedEventArgs) Handles FaTabStrip1.TabStripItemSelectionChanged

        'The user has selected a new tab. If the tab was changed by the parent tab strip (see 
        'OnSelectedTabItemChanged) then we do nothing since this may trigger a cascade (such as 
        'if the parent tabstrip rejects the change and tries to switch back to a different tab.
        If Not m_ignoreTabControlIndexChangedEvent Then
            If Not Me.FaTabStrip1.SelectedItem Is Nothing Then
                Me.SelectedWrappedWindowItem = CType(Me.FaTabStrip1.SelectedItem.Tag, MDIWindowManager.WrappedWindow)
            Else
                Me.SelectedWrappedWindowItem = Nothing
            End If
        End If

    End Sub

End Class
