Public Class ChildFormIntro

    Private Sub ChildFormIntro_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        RichTextBox1.LoadFile(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MDIWindowManager_Test.Intro.rtf"), RichTextBoxStreamType.RichText)

    End Sub

    Private Sub RichTextBox1_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkClickedEventArgs) Handles RichTextBox1.LinkClicked

        Process.Start(e.LinkText)

    End Sub

End Class