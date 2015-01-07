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

Public Class frmSort

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        ' Store sort order on main form and exit
        '=======================================

        If ((cboPrimary.SelectedIndex = cboSecondary.SelectedIndex) Or (cboPrimary.SelectedIndex = cboTertiary.SelectedIndex) Or (cboSecondary.SelectedIndex = cboTertiary.SelectedIndex)) Then
            MsgBox("Please select at least two different columns.", MsgBoxStyle.OkOnly, "Incorrect Column Selection")
            Exit Sub
        End If

        dicColumns.Clear()

        ' Need a primary and secondary sort for any of it to work...
        If cboPrimary.SelectedIndex > -1 And cboSecondary.SelectedIndex > -1 Then
            dicColumns.Add("Primary", cboPrimary.SelectedIndex + 1)
            dicColumns.Add("Secondary", cboSecondary.SelectedIndex + 1)
            If cboTertiary.SelectedIndex <> -1 Then
                dicColumns.Add("Tertiary", cboTertiary.SelectedIndex + 1)
            Else
                dicColumns.Add("Tertiary", -1)
            End If
        End If

        Me.Close()

    End Sub

End Class