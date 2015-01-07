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

Public Class FileGroup
    ' Hold details from code scan to facilitate ordered lists etc.
    ' Unlike ScanResult this will group filenames and line numbers
    ' together under a single issue with the same title.
    '=============================================================


    '==================================================
    '== Constants to array position of details issue ==
    '--------------------------------------------------
    Public Const SEVERITY = 0
    Public Const ISSUE = 1
    Public Const DESC = 2
    Public Const LINE = 3
    Public Const CODE = 4
    '==================================================

    Public FileName As String = ""     ' This will be unique for each issue and will serve as an identifier
    Private strDescription As String = ""

    Private arrDetails As New Dictionary(Of Integer, String())
    Private arrSeverities As New Dictionary(Of Integer, Integer)


    Private Function SetSeverity(ByVal Level As Integer) As String
        ' Set severity level and its associated description
        '==================================================
        Dim strSeverityDescription As String = ""

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
        End Select

        Return strSeverityDescription

    End Function

    Public Sub AddDetail(ByVal Key As Integer, ByVal Severity As Integer, ByVal IssueTitle As String, ByVal Description As String, Optional ByVal LineNumber As Integer = 1, Optional ByVal CodeLine As String = "")
        ' Add the issue, severity and line number for each individual occurence of an issue.
        ' This will be held in a dictionary of all issue details, where the key is the
        ' the same as the key of the individual ScanResult to assist when deleting items.
        '================================================================================
        Dim strSeverityDescription As String = ""


        If Not arrDetails.ContainsKey(Key) Then
            strSeverityDescription = SetSeverity(Severity)
            arrSeverities.Add(Key, Severity)
            arrDetails.Add(Key, {strSeverityDescription, IssueTitle, Description, LineNumber.ToString, CodeLine})
        End If

    End Sub

    Public Sub DeleteDetail(ByVal Key As Integer)
        ' Remove the individual issue from the collection
        '================================================

        If arrDetails.ContainsKey(Key) Then
            arrDetails.Remove(Key)
        End If

    End Sub

    Public Function GetDetail(Key As Integer) As String()
        'Return the individual details for a given issue number
        '======================================================
        Dim strRetVal As String()


        If arrDetails.ContainsKey(Key) Then
            strRetVal = arrDetails.Item(Key)
        Else
            strRetVal = {}
        End If

        Return strRetVal

    End Function

    Public Function GetDetails() As Dictionary(Of Integer, String())
        'Return the the complete set of details for each occurence in this issue
        '=======================================================================

        Return arrDetails

    End Function

    Public Function GetItemCount() As Integer
        Return arrDetails.Count
    End Function

    Public Function GetSeverity(Key As Integer) As Integer
        Return arrSeverities.Item(Key)
    End Function

End Class
