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

Public Class ResultsTracker

    '==============================================================================
    ' This collection holds the results of the scan to allow them to be sorted etc.
    '------------------------------------------------------------------------------
    Public ScanResults As New ArrayList
    Public IssueGroups As New Dictionary(Of String, IssueGroup)     ' Individual issue title with collection of associated filenames/linenumbers 
    Public FileGroups As New Dictionary(Of String, FileGroup)       ' Individual filename with collection of associated issues
    '==============================================================================

    '============================================================
    '== These variables hold data about the code being scanned ==
    '------------------------------------------------------------
    Public FileCount As Integer
    Public TargetDirectory As String

    ' This acts as the key for each issue being added
    ' It allows us to link the issue data to the listview item
    Public CurrentIndex As Integer = 0

    ' Total counts for the directory
    Public OverallCommentCount As Long
    Public OverallCodeCount As Long
    Public OverallWhitespaceCount As Long
    Public OverallLineCount As Long
    Public OverallFixMeCount As Long
    Public OverallBadFuncCount As Long

    ' Individual counts for each file
    Public CommentCount As Long
    Public CodeCount As Long
    Public WhitespaceCount As Long
    Public LineCount As Long
    Public FixMeCount As Long
    Public BadFuncCount As Long
    '============================================================

    '=====================================================================
    '== Arrays to hold the list of files in the directory being scanned ==
    '== and any fixme/todo comments found in the files                  ==
    '---------------------------------------------------------------------
    Public FileList As New ArrayList
    Public FileDataList As New ArrayList
    Public FixMeList As New ArrayList
    '=====================================================================


    Public Sub Reset()
        'Set all variables to zero ready for a new scan
        '==============================================

        OverallCommentCount = 0
        OverallCodeCount = 0
        OverallWhitespaceCount = 0
        OverallLineCount = 0
        OverallFixMeCount = 0
        OverallBadFuncCount = 0

        CommentCount = 0
        CodeCount = 0
        WhitespaceCount = 0
        LineCount = 0
        FixMeCount = 0
        BadFuncCount = 0

        CurrentIndex = 0

        FixMeList.Clear()
        ScanResults.Clear()
        IssueGroups.Clear()
        FileGroups.Clear()

    End Sub

    Public Sub ResetFileCountVars()
        'Set all variables from local file count to zero ready for to scan a new file
        '============================================================================

        CommentCount = 0
        CodeCount = 0
        WhitespaceCount = 0
        LineCount = 0
        FixMeCount = 0
        BadFuncCount = 0

    End Sub

    Public Sub ResetFileListVars()
        'Set all variables from local file count to zero ready for to scan a new file
        '============================================================================

        FileCount = 0
        FileList.Clear()
        FileDataList.Clear()

    End Sub

End Class
