' VisualCodeGrepper - Code security scanner
' Copyright (C) 2012-2013 Nick Dunn and John Murray
'
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with this program.  If not, see <http://www.gnu.org/licenses/>.

Public Class frmIndividualBreakdown

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub OpenWithNotepadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenWithNotepadToolStripMenuItem.Click
        LaunchNPP(strCurrentFileName)
    End Sub

    Private Sub CopyUnsafeMethodsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyUnsafeMethodsToolStripMenuItem.Click
        ' Loop throught comments, format them and add to clipboard
        '=========================================================
        Dim strCode As String = ""


        If intCodeIssues > 0 Then
            For Each srItem As ScanResult In rtResultsTracker.ScanResults
                If srItem.FileName = strCurrentFileName And srItem.Severity <> CodeIssue.INFO Then
                    strCode &= "File: " & srItem.FileName & vbNewLine
                    strCode &= "Line: " & srItem.LineNumber & vbNewLine
                    strCode &= "Issue: " & srItem.Title & vbNewLine & srItem.Description & vbNewLine & srItem.CodeLine & vbNewLine & vbNewLine
                End If
            Next
            Clipboard.SetText(strCode)
        End If

    End Sub

    Private Sub CopyCommentsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyCommentsToolStripMenuItem.Click
        ' Loop throught comments, format them and add to clipboard
        '=========================================================
        Dim strComments As String = ""


        If intComments > 0 Then
            For Each srItem As ScanResult In rtResultsTracker.FixMeList
                If srItem.FileName = strCurrentFileName Then
                    strComments &= "File: " & srItem.FileName & vbNewLine
                    strComments &= "Line: " & srItem.LineNumber & vbNewLine & "Contains: '" & srItem.CodeLine & "'" & vbNewLine & vbNewLine
                End If
            Next
            Clipboard.SetText(strComments)
        End If

    End Sub

End Class