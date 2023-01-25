Public Class frmLoading
    Private Sub frmLoading_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        asAppSettings.AbortCurrentOperation = False
    End Sub

    Private Sub frmLoading_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        asAppSettings.AbortCurrentOperation = True
    End Sub
End Class