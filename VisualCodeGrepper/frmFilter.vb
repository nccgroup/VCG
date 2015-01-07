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

Public Class frmFilter

    Private Sub frmFilter_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Start with a valid option selected
        '===================================
        cboAbove.SelectedIndex = 0
        cboBelow.SelectedIndex = 0
        cboMinimum.SelectedIndex = 0
        cboMaximum.SelectedIndex = 6
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        ' Conceal the form and apply the required updates
        '================================================
        Dim intMinimum As Integer = CodeIssue.POSSIBLY_SAFE
        Dim intMaximum As Integer = CodeIssue.CRITICAL


        '== Set report levels ==
        If rbAbove.Checked = True Then
            intMinimum = 7 - cboAbove.SelectedIndex
        ElseIf rbBelow.Checked = True Then
            intMaximum = 7 - cboBelow.SelectedIndex
        Else
            intMinimum = 7 - cboMinimum.SelectedIndex
            intMaximum = 7 - cboMaximum.SelectedIndex
            If intMaximum > intMinimum Then
                MsgBox("Maximum value cannot be less than minmum value.", vbOK, "Invalid Values")
                Exit Sub
            End If
        End If

        '== Hide the dialog ==
        Me.Hide()

        '== Filter and export ==
        frmMain.FilterResults(intMinimum, intMaximum)
        If cbExport.Checked = True Then frmMain.ExportResultsXML(intMinimum, intMaximum)

    End Sub

    Private Sub rbAbove_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbAbove.CheckedChanged
        ' Enable/disable relevant controls
        '=================================
        cboAbove.Enabled = True
        cboBelow.Enabled = False
        cboMinimum.Enabled = False
        cboMaximum.Enabled = False
    End Sub

    Private Sub rbBelow_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbBelow.CheckedChanged
        ' Enable/disable relevant controls
        '=================================
        cboBelow.Enabled = True
        cboAbove.Enabled = False
        cboMinimum.Enabled = False
        cboMaximum.Enabled = False
    End Sub

    Private Sub rbRange_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbRange.CheckedChanged
        ' Enable/disable relevant controls
        '=================================
        cboMinimum.Enabled = True
        cboMaximum.Enabled = True
        cboBelow.Enabled = False
        cboAbove.Enabled = False
    End Sub

End Class