' VisualCodeGrepper - Code security scanner
' Copyright (C) 2012-2014 Nick Dunn and John Murray
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

Public Class ScanResult
    ' Hold details from code scan to facilitate ordered lists etc.
    '=============================================================


    Public Title As String = ""
    Public Description As String = ""
    Public FileName As String = ""
    Public LineNumber As Integer = 0
    Public CodeLine As String = ""
    Public ItemKey As Integer = 1

    Private strSeverityDescription As String = ""
    Private intSeverity As Integer = CodeIssue.STANDARD

    '== Has the item been marked by the user in the ListView? ==
    Public IsChecked As Boolean = False
    Public CheckColour As Color = Color.LawnGreen


    Public Sub SetSeverity(ByVal Level As Integer)
        ' Set severity level and its associated description
        '==================================================

        intSeverity = Level

        Select Case Level
            Case CodeIssue.CRITICAL
                strSeverityDescription = "Critical"
            Case CodeIssue.HIGH
                strSeverityDescription = "High"
            Case CodeIssue.MEDIUM
                strSeverityDescription = "Medium"
            Case CodeIssue.LOW
                strSeverityDescription = "Low"
            Case CodeIssue.INFO
                strSeverityDescription = "Suspicious Comment"
            Case CodeIssue.POSSIBLY_SAFE
                strSeverityDescription = "Potential Issue"
            Case Else
                strSeverityDescription = "Standard"
                intSeverity = CodeIssue.STANDARD
        End Select

    End Sub

    Public Function Severity() As Integer
        Return intSeverity
    End Function

    Public Function SeverityDesc() As String
        Return strSeverityDescription
    End Function

End Class
