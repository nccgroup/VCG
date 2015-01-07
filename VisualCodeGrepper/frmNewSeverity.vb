Public Class frmNewSeverity

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        ' Set severity marker to 'do nothing' and close form
        '===================================================

        intNewSeverity = -1
        Me.Close()

    End Sub

    Private Sub btnOK_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
        ' Set new severity marker and close form
        '=======================================

        intNewSeverity = 7 - cboNewLevel.SelectedIndex
        Me.Close()

    End Sub

    Private Sub frmNewSeverity_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        cboNewLevel.SelectedIndex = CodeIssue.STANDARD

    End Sub

End Class