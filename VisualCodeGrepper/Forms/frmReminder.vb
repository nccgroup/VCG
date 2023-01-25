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

Public Class frmReminder

    Private Sub btnOK_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
        ' Write user choice to registry
        '==============================

        If chkNotAgain.Checked Then SaveSetting("VisualCodeGrepper", "Startup", "LangReminder", "0")
        Me.Hide()

    End Sub

    Private Sub lblReminder_Click(sender As System.Object, e As System.EventArgs) Handles lblReminder.Click

    End Sub
    Private Sub chkNotAgain_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkNotAgain.CheckedChanged

    End Sub
End Class