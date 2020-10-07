'////////////////////////////////////////////////////////////
'//  [AUTHOR]:      C. Moya                                //
'//  [DESCRIPTION]                                         //
'//                 Strongly Typed Collection Template     //
'//  [/DESCRIPTION]                                        //
'//  [COMMENTS]                                            //
'//                 Substitue xOBJECTx placeholder with    //
'//                 real types.                            //
'//  [/COMMENTS]                                           //
'////////////////////////////////////////////////////////////
'Option Strict Off
'Option Explicit On

' typename of object:     WrappedWindow
' typename of collection: WrappedWindowCollection
' name of enumerator:     WrappedWindowCollectionEnumerator


Public Class WrappedWindowCollectionException
    Inherits Exception

    Public Sub New()

        MyBase.New()

    End Sub

    Public Sub New(ByVal message As String)

        MyBase.New(message)

    End Sub

End Class

Public Class WrappedWindowCollection
    Inherits CollectionBase

    Public Event BeforeWrappedWindowAdded As EventHandler(Of ItemsCancelEventArgs)
    Public Event WrappedWindowAdded As EventHandler(Of ItemsEventArgs)
    Public Event BeforeWrappedWindowRemoved As EventHandler(Of ItemsCancelEventArgs)
    Public Event WrappedWindowRemoved As EventHandler(Of ItemsEventArgs)
    Public Event WrappedWindowsCleared As EventHandler
    Public Event WindowClosed As EventHandler(Of ItemsClosedEventArgs)
    Public Event WindowClosing As EventHandler(Of ItemsClosingEventArgs)
    Public Event WindowActivated As EventHandler(Of ItemsEventArgs)
    Public Event WindowDeactivate As EventHandler(Of ItemsEventArgs)
    Public Event WindowEnter As EventHandler(Of ItemsEventArgs)
    Public Event WindowLeave As EventHandler(Of ItemsEventArgs)
    Public Event PopInRequested As EventHandler(Of ItemsEventArgs)
    Public Event WindowTextChanged As EventHandler(Of ItemsEventArgs)
    Public Event WindowVisibleChanged As EventHandler(Of ItemsEventArgs)

    Public Class ItemsEventArgs
        Inherits System.ComponentModel.HandledEventArgs

        Private m_wrappedWindow As WrappedWindow

        Public Sub New(ByVal wrappedWindow As WrappedWindow)

            m_wrappedWindow = wrappedWindow

        End Sub

        Public ReadOnly Property WrappedWindow() As WrappedWindow

            Get
                Return m_wrappedWindow
            End Get

        End Property

        Public Shared Shadows ReadOnly Property Empty() As ItemsEventArgs

            Get
                Return New ItemsEventArgs(Nothing)
            End Get

        End Property

    End Class

    Public Class ItemsClosedEventArgs
        Inherits ItemsEventArgs

        Private m_closeReason As CloseReason = CloseReason.None

        Public Sub New(ByVal wrappedWindow As WrappedWindow, ByVal closeReason As CloseReason)

            MyBase.New(wrappedWindow)

            m_closeReason = closeReason

        End Sub

        Public ReadOnly Property CloseReason() As CloseReason

            Get
                Return m_closeReason
            End Get

        End Property

        Public Shared Shadows ReadOnly Property Empty() As ItemsClosedEventArgs

            Get
                Return New ItemsClosedEventArgs(Nothing, Windows.Forms.CloseReason.None)
            End Get

        End Property

    End Class

    Public Class ItemsCancelEventArgs
        Inherits System.ComponentModel.CancelEventArgs

        Private m_wrappedWindow As WrappedWindow

        Public Sub New(ByVal wrappedWindow As WrappedWindow)

            m_wrappedWindow = wrappedWindow

        End Sub

        Public ReadOnly Property WrappedWindow() As WrappedWindow

            Get
                Return m_wrappedWindow
            End Get

        End Property

        Public Shared Shadows ReadOnly Property Empty() As ItemsCancelEventArgs

            Get
                Return New ItemsCancelEventArgs(Nothing)
            End Get

        End Property

    End Class

    Public Class ItemsClosingEventArgs
        Inherits ItemsCancelEventArgs

        Private m_closeReason As CloseReason = CloseReason.None

        Public Sub New(ByVal wrappedWindow As WrappedWindow, ByVal closeReason As CloseReason)

            MyBase.New(wrappedWindow)

            m_closeReason = closeReason

        End Sub

        Public ReadOnly Property CloseReason() As CloseReason

            Get
                Return m_closeReason
            End Get

        End Property

        Public Shared Shadows ReadOnly Property Empty() As ItemsClosingEventArgs

            Get
                Return New ItemsClosingEventArgs(Nothing, Windows.Forms.CloseReason.None)
            End Get

        End Property

    End Class

    Public Sub New()

        MyBase.New()

    End Sub

    Default Public Property Item(ByVal index As Integer) As WrappedWindow
        Get
            Return CType(Me.List(index), WrappedWindow)
        End Get
        Set(ByVal value As WrappedWindow)
            Me.List(index) = value
        End Set
    End Property

    Public Function Add(ByVal value As WrappedWindow) As Integer

        Return Me.List.Add(value)

    End Function

    Public Function Contains(ByVal value As WrappedWindow) As Boolean

        Return Me.List.Contains(value)

    End Function

    Public Function IndexOf(ByVal value As WrappedWindow) As Integer

        Return Me.List.IndexOf(value)

    End Function

    Public Sub Remove(ByVal value As WrappedWindow)

        Me.List.Remove(value)

    End Sub

    Public Shadows Function GetEnumerator() As WrappedWindowCollectionEnumerator

        Return New WrappedWindowCollectionEnumerator(Me)

    End Function

    Public Sub Insert(ByVal index As Integer, ByVal value As WrappedWindow)

        Me.List.Insert(index, value)

    End Sub

    Protected Overrides Sub OnClear()

        For Each wrappedWindow As WrappedWindow In Me.List
            Dim cancelEventArgs As New ItemsCancelEventArgs(wrappedWindow) With {.Cancel = False}
            RaiseEvent BeforeWrappedWindowRemoved(Me, cancelEventArgs)

            If cancelEventArgs.Cancel Then
                Throw New WrappedWindowCollectionException("Clear aborted.")
                Exit For
            End If

            RemoveHandler wrappedWindow.WindowClosed, AddressOf HandleWindowClosed
        Next wrappedWindow

        MyBase.OnClear()

    End Sub

    Protected Overrides Sub OnClearComplete()

        RaiseEvent WrappedWindowsCleared(Me, EventArgs.Empty)

        MyBase.OnClearComplete()

    End Sub

    Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As Object)

        Dim cancelEventArgs As New ItemsCancelEventArgs(CType(value, WrappedWindow))

        RaiseEvent BeforeWrappedWindowAdded(Me, cancelEventArgs)

        If cancelEventArgs.Cancel Then
            Throw New WrappedWindowCollectionException("Window add aborted.")
        End If

        MyBase.OnInsert(index, value)

    End Sub

    Protected Overrides Sub OnInsertComplete(ByVal index As Integer, ByVal value As Object)

        Dim wrappedWindow As WrappedWindow = CType(value, WrappedWindow)

        With wrappedWindow
            AddHandler .WindowActivated, AddressOf HandleWindowActivated
            AddHandler .WindowDeactivate, AddressOf HandleWindowDeactivate
            AddHandler .WindowEnter, AddressOf HandleWindowEnter
            AddHandler .WindowLeave, AddressOf HandleWindowLeave
            AddHandler .WindowClosing, AddressOf HandleWindowClosing
            AddHandler .WindowClosed, AddressOf HandleWindowClosed
            AddHandler .WindowTextChanged, AddressOf HandleWindowTextChanged
            AddHandler .WindowVisibleChanged, AddressOf HandleWindowVisibleChanged
            AddHandler .PopInRequested, AddressOf HandlePopInRequested
        End With

        RaiseEvent WrappedWindowAdded(Me, New ItemsEventArgs(wrappedWindow))

        MyBase.OnInsertComplete(index, value)

    End Sub

    Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As Object)

        Dim wrappedWindow As WrappedWindow = CType(value, WrappedWindow)
        Dim cancelEventArgs As New ItemsCancelEventArgs(CType(value, WrappedWindow))

        RaiseEvent BeforeWrappedWindowRemoved(Me, cancelEventArgs)

        If cancelEventArgs.Cancel Then
            Throw New WrappedWindowCollectionException("Window remove aborted.")
        End If

        With wrappedWindow
            RemoveHandler .WindowActivated, AddressOf HandleWindowActivated
            RemoveHandler .WindowDeactivate, AddressOf HandleWindowDeactivate
            RemoveHandler .WindowClosing, AddressOf HandleWindowClosing
            RemoveHandler .WindowClosed, AddressOf HandleWindowClosed
            RemoveHandler .WindowLeave, AddressOf HandleWindowLeave
            RemoveHandler .WindowEnter, AddressOf HandleWindowEnter
            RemoveHandler .WindowTextChanged, AddressOf HandleWindowTextChanged
            RemoveHandler .WindowVisibleChanged, AddressOf HandleWindowVisibleChanged
            RemoveHandler .PopInRequested, AddressOf HandlePopInRequested
        End With

        MyBase.OnRemove(index, value)

    End Sub

    Protected Overrides Sub OnRemoveComplete(ByVal index As Integer, ByVal value As Object)

        RaiseEvent WrappedWindowRemoved(Me, New ItemsEventArgs(CType(value, WrappedWindow)))

        'If Me.List.Count = 0 Then
        '    OnClearComplete()
        'End If

        MyBase.OnRemoveComplete(index, value)

    End Sub

    Private Sub HandleWindowClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs)

        Dim wrappedWindow As WrappedWindow = CType(sender, WrappedWindow)

        Try
            RaiseEvent WindowClosed(Me, New ItemsClosedEventArgs(wrappedWindow, e.CloseReason))
        Catch
            'do nothing
        End Try

        Try
            If Me.Contains(wrappedWindow) Then
                Me.Remove(wrappedWindow)
            End If
        Catch
            'do nothing
        End Try

        Try
            If wrappedWindow.Window IsNot Nothing Then
                wrappedWindow.Window.Dispose()
            End If
        Catch
            'do nothing
        End Try

    End Sub

    Private Sub HandleWindowClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)

        If Not e.Cancel Then
            Dim wrappedWindow As WrappedWindow = CType(sender, WrappedWindow)
            Dim eventargs As New ItemsClosingEventArgs(wrappedWindow, e.CloseReason)

            RaiseEvent WindowClosing(Me, eventargs)

            e.Cancel = eventargs.Cancel
        End If

    End Sub

    Private Sub HandlePopInRequested(ByVal sender As Object, ByVal e As System.ComponentModel.HandledEventArgs)

        Dim eventargs As New ItemsEventArgs(CType(sender, WrappedWindow))
        eventargs.Handled = e.Handled

        RaiseEvent PopInRequested(sender, eventargs)

        e.Handled = eventargs.Handled

    End Sub

    Private Sub HandleWindowActivated(ByVal sender As Object, ByVal e As EventArgs)

        RaiseEvent WindowActivated(Me, New ItemsEventArgs(CType(sender, WrappedWindow)))

    End Sub

    Private Sub HandleWindowDeactivate(ByVal sender As Object, ByVal e As EventArgs)

        RaiseEvent WindowDeactivate(Me, New ItemsEventArgs(CType(sender, WrappedWindow)))

    End Sub

    Private Sub HandleWindowEnter(ByVal sender As Object, ByVal e As EventArgs)

        RaiseEvent WindowEnter(Me, New ItemsEventArgs(CType(sender, WrappedWindow)))

    End Sub

    Private Sub HandleWindowLeave(ByVal sender As Object, ByVal e As EventArgs)

        RaiseEvent WindowLeave(Me, New ItemsEventArgs(CType(sender, WrappedWindow)))

    End Sub

    Private Sub HandleWindowActive(ByVal sender As Object, ByVal e As EventArgs)

        RaiseEvent WindowEnter(Me, New ItemsEventArgs(CType(sender, WrappedWindow)))

    End Sub

    Private Sub HandleWindowDeactive(ByVal sender As Object, ByVal e As EventArgs)

        RaiseEvent WindowLeave(Me, New ItemsEventArgs(CType(sender, WrappedWindow)))

    End Sub

    Private Sub HandleWindowTextChanged(ByVal sender As Object, ByVal e As EventArgs)

        RaiseEvent WindowTextChanged(Me, New ItemsEventArgs(CType(sender, WrappedWindow)))

    End Sub

    Private Sub HandleWindowVisibleChanged(ByVal sender As Object, ByVal e As EventArgs)

        RaiseEvent WindowVisibleChanged(Me, New ItemsEventArgs(CType(sender, WrappedWindow)))

    End Sub

    Public Class WrappedWindowCollectionEnumerator

        Implements IEnumerator

        Private m_index As Integer

        Private m_currentElement As WrappedWindow

        Private m_items As WrappedWindowCollection

        Friend Sub New(ByVal collection As WrappedWindowCollection)

            MyBase.New()
            m_index = -1
            m_items = collection

        End Sub

        Public ReadOnly Property Current() As Object Implements IEnumerator.Current
            Get
                If ((m_index = -1) OrElse (m_index >= m_items.Count)) Then
                    Throw New IndexOutOfRangeException("Enumerator not started.")
                Else
                    Return m_currentElement
                End If
            End Get
        End Property

        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext

            If (m_index < (m_items.Count - 1)) Then
                m_index += 1
                m_currentElement = m_items(m_index)
                Return True
            End If
            m_index = m_items.Count
            Return False

        End Function

        Public Sub Reset() Implements IEnumerator.Reset

            m_index = -1
            m_currentElement = Nothing

        End Sub

    End Class

End Class