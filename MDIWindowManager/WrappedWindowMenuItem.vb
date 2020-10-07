''' <summary>
''' Custom MenuItem provided to help display wrapped windows in menus
''' </summary>
''' <remarks></remarks>
Public Class WrappedWindowMenuItem
    Inherits MenuItem

    Private WithEvents m_wrappedWindow As WrappedWindow
    Private m_textPrefix As String = String.Empty

    Public Property WrappedWindow() As WrappedWindow

        Get
            Return m_wrappedWindow
        End Get

        Set(ByVal value As WrappedWindow)
            m_wrappedWindow = value

            SyncWithWindowText()
        End Set

    End Property

    Public Property TextPrefix() As String

        Get
            Return m_textPrefix
        End Get

        Set(ByVal value As String)
            m_textPrefix = value
        End Set

    End Property

    Private Sub m_wrappedWindow_WindowTextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles m_wrappedWindow.WindowTextChanged

        SyncWithWindowText()

    End Sub

    Private Sub SyncWithWindowText()

        If m_wrappedWindow IsNot Nothing Then
            If m_wrappedWindow.Window IsNot Nothing Then
                If m_textPrefix <> String.Empty Then
                    MyBase.Text = $"{m_textPrefix} {m_wrappedWindow.Window.Text}"
                Else
                    MyBase.Text = m_wrappedWindow.Window.Text
                End If
            Else
                MyBase.Text = String.Empty
            End If
        Else
            MyBase.Text = String.Empty
        End If

    End Sub

End Class