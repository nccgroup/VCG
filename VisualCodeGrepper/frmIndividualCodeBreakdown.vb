Public Class frmIndividualCodeBreakdown

    Private Sub LaunchInNotepadToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LaunchInNotepadToolStripMenuItem.Click
        frmCodeBreakdownResults.launchnpp(frmCodeBreakdownResults.filetoopen)
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        Form1.showversion()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub CopyAllUnsafeMethodsToClipboardToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CopyAllUnsafeMethodsToClipboardToolStripMenuItem.Click
        '   Me.Label2.SelectAll()
        Clipboard.SetText(Me.Label2.Text)

    End Sub
End Class