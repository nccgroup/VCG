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

Public Class frmBreakdown

    Private Sub dgvResults_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvResults.CellDoubleClick
        ' When user clicks a cell show the individual results for that file
        ' using the Individual Breakdown form
        '==================================================================

        intComments = 0
        intCodeIssues = 0

        frmIndividualBreakdown.Close()
        frmIndividualBreakdown.Text = Me.dgvResults.Rows(e.RowIndex).Cells(0).Value

        With frmIndividualBreakdown.chtResults

            .Series(0).Points.AddY(Me.dgvResults.Rows(e.RowIndex).Cells(3).Value)
            .Series(0).Points.AddY(Me.dgvResults.Rows(e.RowIndex).Cells(4).Value)
            .Series(0).Points.AddY(Me.dgvResults.Rows(e.RowIndex).Cells(5).Value)
            If Me.dgvResults.Rows(e.RowIndex).Cells(6).Value <> Nothing Then
                .Series(0).Points.AddY(Me.dgvResults.Rows(e.RowIndex).Cells(6).Value)
                .Series(0).Points(3).LegendText = "Potentially Unfinished Code (" & Me.dgvResults.Rows(e.RowIndex).Cells(6).Value & " counts)"
                Me.pnlResults.Visible = True
                intComments = Me.dgvResults.Rows(e.RowIndex).Cells(6).Value
            End If
            If Me.dgvResults.Rows(e.RowIndex).Cells(7).Value <> Nothing Then
                .Series(0).Points.AddY(Me.dgvResults.Rows(e.RowIndex).Cells(7).Value)
                If intComments > 0 Then
                    .Series(0).Points(4).LegendText = "Potentially Bad Code (" & Me.dgvResults.Rows(e.RowIndex).Cells(7).Value & " counts)"
                Else
                    .Series(0).Points(3).LegendText = "Potentially Bad Code (" & Me.dgvResults.Rows(e.RowIndex).Cells(7).Value & " counts)"
                End If
                Me.pnlResults.Visible = True
                intCodeIssues = Me.dgvResults.Rows(e.RowIndex).Cells(7).Value
            End If
            .Series(0).Points(0).LegendText = "Overall Code (including comment-appended code) (" & Me.dgvResults.Rows(e.RowIndex).Cells(3).Value & " lines)"
            .Series(0).Points(1).LegendText = "Overall Comments (" & Me.dgvResults.Rows(e.RowIndex).Cells(4).Value & " comments)"
            .Series(0).Points(2).LegendText = "Overall Whitespace (" & Me.dgvResults.Rows(e.RowIndex).Cells(5).Value & " lines)"
            .Series("Series1")("BarLabelStyle") = "Right"
            .ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
            .Series("Series1")("DrawingStyle") = "Cylinder"

        End With

        strCurrentFileName = Me.dgvResults.Rows(e.RowIndex).Cells(8).Value
        frmMain.CountFixMeComments(Me.dgvResults.Rows(e.RowIndex).Cells(8).Value)

        With frmIndividualBreakdown
            .lblResults.Text = "File: " & Me.dgvResults.Rows(e.RowIndex).Cells(0).Value & vbNewLine & "Total Line Count: " & (Me.dgvResults.Rows(e.RowIndex).Cells(1).Value) _
            & vbNewLine & vbTab & "Number of Lines of Code (including comment-appended lines):" & (Me.dgvResults.Rows(e.RowIndex).Cells(3).Value) & vbNewLine & vbTab _
            & "Number of Comments: " & (Me.dgvResults.Rows(e.RowIndex).Cells(4).Value) & vbNewLine & vbTab & "Lines of Whitespace: " & (Me.dgvResults.Rows(e.RowIndex).Cells(5).Value) _
            & vbNewLine & vbNewLine & "Full Path: " & (Me.dgvResults.Rows(e.RowIndex).Cells(8).Value)

            .Show()
        End With

    End Sub

    Private Sub OpenInNotepadToolStripMenuItem_Click(ByVal Sender As System.Object, ByVal GridArgs As System.EventArgs) Handles OpenInNotepadToolStripMenuItem.Click

        If strCurrentFileName <> "" Then
            LaunchNPP(strCurrentFileName)
        End If

    End Sub

    Private Sub dgvResults_CellContentClick_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

        If Not e.RowIndex = -1 Then
            strCurrentFileName = Me.dgvResults.Rows(e.RowIndex).Cells(8).Value '== Bodge for ordering columns ==
        End If

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub ExportToClipboardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportToClipboardToolStripMenuItem.Click
        If frmMain.rtbResults.Text <> "" Then Clipboard.SetText(frmMain.rtbResults.Text)
    End Sub

    Private Sub ExitToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnApplyFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApplyFilter.Click
        ' Filter filename column, based on content of filter textbox
        '===========================================================
        Dim strText As String


        '== Hide any rows where the filename does not contain the filter text ==
        For Each itmItem As DataGridViewRow In dgvResults.Rows
            If itmItem.Cells(0).Value <> Nothing Then
                strText = itmItem.Cells(0).Value
                If (Not strText.Contains(txtFilter.Text)) And (Not txtFilter.Text.Trim = "") Then
                    itmItem.Visible = False
                Else
                    itmItem.Visible = True
                End If
            End If
        Next

    End Sub

End Class


