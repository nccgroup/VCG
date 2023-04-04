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


Option Explicit On
Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions
Imports System.Xml

Public Class frmMain

    '===============================
    '== Columns for summary table ==
    '-------------------------------
    Const PRIORITY_COL = 0
    Const SEVERITY_COL = 1
    Const TITLE_COL = 2
    Const DESC_COL = 3
    Const FILE_COL = 4
    Const LINE_COL = 5
    '===============================

    '===============================
    '== Local Constants
    '-------------------------------
    Const EXIT_CODE = -1

    '=========================================
    '== Configurable options for XML export ==
    '-----------------------------------------
    Public SaveCheckState As Boolean = True
    Public SaveFiltered As Boolean = False
    '=========================================


    '========================================================
    '== Application details stored from previous execution ==
    '--------------------------------------------------------
    Dim arrPreviousDirs As String()
    '========================================================


    '== Current position in Rich Text Box (for assingning fonts) ==
    Dim lngPosition As Long = 0

    '== Search text ==
    Dim strPrevSearch As String = ""

    '== Output files ==
    Dim swOutputFile As StreamWriter
    Dim swCsvOutputFile As StreamWriter
    Dim xwsXMLOutputSettings As New XmlWriterSettings()
    Dim xwOutputWriter As XmlWriter

    '== Sort order for listview (ascending or descending) ==
    Dim blnIsAscendingSeverity As Boolean = False
    Dim blnIsAscendingTitle As Boolean = False
    Dim blnIsAscendingDescription As Boolean = False
    Dim blnIsAscendingFile As Boolean = False
    Dim blnIsFiltered As Boolean = False     ' Will be set to true if we're not showing all results
    Public intFilterMin As Integer = CodeIssue.POSSIBLY_SAFE
    Public intFilterMax As Integer = CodeIssue.CRITICAL


    Private Sub NewTargetToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles NewTargetToolStripMenuItem.Click
        'Load files to be scanned
        '========================
        Dim strTargetFolder As String


        ' Get target directory
        fbFolderBrowser.ShowDialog()

        If Not Windows.Forms.DialogResult.Cancel Then
            strTargetFolder = Me.fbFolderBrowser.SelectedPath.ToString
            LoadFiles(strTargetFolder)
        End If

    End Sub

    Private Sub NewTargetFileToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles NewTargetFileToolStripMenuItem.Click
        'Load single file to be scanned
        '==============================
        Dim strTargetFile As String


        ' Get target file
        ofdOpenFileDialog.ShowDialog()

        If Not Windows.Forms.DialogResult.Cancel Then
            strTargetFile = ofdOpenFileDialog.FileName
            LoadFiles(strTargetFile)
        End If

    End Sub

    Private Function CheckFileType(ByVal TargetFile As String) As Boolean
        ' Check file type is consistent with required language
        ' This includes extensions, and exact filenames
        '=====================================================

        TargetFile = TargetFile.ToLower()

        Dim ext = Path.GetExtension(TargetFile)
        Dim fileName = Path.GetFileName(TargetFile)

        If asAppSettings.FileSuffixes.Contains(ext) Then
            Return True
        ElseIf asAppSettings.FileSuffixes.Contains(fileName) Then
            Return True
        Else
            Return False
        End If


    End Function

    Private Sub CCToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CCToolStripMenuItem.Click
        ' Set code type to be tested and uncheck other menu items
        '========================================================
        CCToolStripMenuItem.Checked = True
        SelectLanguage(AppSettings.C)

    End Sub

    Private Sub JavaToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles JavaToolStripMenuItem.Click
        ' Set code type to be tested and uncheck other menu items
        '========================================================

        JavaToolStripMenuItem.Checked = True
        SelectLanguage(AppSettings.JAVA)

    End Sub

    Private Sub PLSQLToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PLSQLToolStripMenuItem.Click
        ' Set code type to be tested and uncheck other menu items
        '========================================================

        PLSQLToolStripMenuItem.Checked = True
        SelectLanguage(AppSettings.SQL)

    End Sub

    Private Sub CSToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CSToolStripMenuItem.Click
        ' Set code type to be tested and uncheck other menu items
        '========================================================

        CSToolStripMenuItem.Checked = True
        SelectLanguage(AppSettings.CSHARP)

    End Sub

    Private Sub VBToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles VBToolStripMenuItem.Click
        ' Set code type to be tested and uncheck other menu items
        '========================================================

        VBToolStripMenuItem.Checked = True
        SelectLanguage(AppSettings.VB)

    End Sub

    Private Sub PHPToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PHPToolStripMenuItem.Click
        ' Set code type to be tested and uncheck other menu items
        '========================================================

        PHPToolStripMenuItem.Checked = True
        SelectLanguage(AppSettings.PHP)

    End Sub

    Private Sub COBOLToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles COBOLToolStripMenuItem.Click
        ' Set code type to be tested and uncheck other menu items
        '========================================================

        COBOLToolStripMenuItem.Checked = True
        SelectLanguage(AppSettings.COBOL)

    End Sub

    Public Sub DisplayError(exception As Exception, Optional Caption As String = "Error", Optional MsgBoxStyle As Integer = MsgBoxStyle.Information)
        ' Display error message to user, depending on GUI/Console mode
        '=============================================================

        If asAppSettings.IsConsole Then
            LogError($"{exception}")
        Else
            DisplayError(exception.Message, Caption, MsgBoxStyle)
        End If

    End Sub

    Public Sub DisplayError(message As String, Optional Caption As String = "Error", Optional MsgBoxStyle As Integer = MsgBoxStyle.Information)
        ' Display error message to user, depending on GUI/Console mode
        '=============================================================

        If asAppSettings.IsConsole Then
            LogError(message)
        Else
            MsgBox(message, MsgBoxStyle, Caption)
        End If

    End Sub

    Public Sub ScanFiles(CommentScan As Boolean, CodeScan As Boolean)
        ' Iterate through the files in the directory and compile the results
        '===================================================================

        Dim arrShortName() As String
        Dim strLine As String
        Dim strTrimmedComment = ""

        Dim blnIsCommented As Boolean = False
        Dim blnIsColoured As Boolean = False


        If (CommentScan = False And CodeScan = False) Then Exit Sub

        ' Initialise variables before proceeding
        rtResultsTracker.Reset()
        If Not asAppSettings.IsConsole Then
            frmBreakdown.Close()
            frmLoading.Close()
            ShowLoading("Running scans", "Initialising....", rtResultsTracker.FileList.Count)
        Else
            LogInfo("Running scans...")
        End If


        '== Open output files as required ==
        If asAppSettings.IsOutputFile Then swOutputFile = New StreamWriter(asAppSettings.OutputFile)

        ' Ensure progress is reported correctly.
        frmLoading.pbProgressBar.Value = 0

        '== Iterate through files in selected directory and perform selected scans ==
        If rtResultsTracker.FileList.Count <> Nothing Then

            If asAppSettings.AltSingleLineComment.StartsWith("\") Then
                strTrimmedComment = asAppSettings.AltSingleLineComment.TrimStart("\")
            End If

            modMain.ctCodeTracker.ResetCDictionaries()

            For Each strItem As String In rtResultsTracker.FileList

                If asAppSettings.AbortCurrentOperation Then
                    asAppSettings.AbortCurrentOperation = False
                    MessageBox.Show(Me, "Operation aborted. The results shown may be incomplete.", "Scan aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit For
                End If

                IncrementLoadingBar(strItem, rtResultsTracker.FileList.Count)

                arrShortName = strItem.Split("\")
                modMain.ctCodeTracker.Reset()

                ' Check for php.ini file and handle this separately
                If (asAppSettings.TestType = AppSettings.PHP And arrShortName.Last.ToLower() = "php.ini") And asAppSettings.IsConfigOnly = False Then

                    ctCodeTracker.HasDisableFunctions = False

                    For Each strLine In File.ReadAllLines(strItem)
                        CheckPhpIni(strLine, strItem)
                    Next

                    If ctCodeTracker.HasDisableFunctions = True Then
                        ListCodeIssue("Failure to use 'disable_functions'", "The ini file fails to use the 'disable_functions' feature, greatly increasing the segverity of a successful compromise.", strItem)
                    End If


                Else

                    rtResultsTracker.LineCount = 0

                    ' Otherwise split the line into code and comments and scan each part
                    For Each strLine In File.ReadAllLines(strItem)

                        rtResultsTracker.OverallLineCount += 1
                        rtResultsTracker.LineCount += 1

                        '== If this is a COBOL program then we need to trim the specified number of characters from start of line ==
                        '== These represent line numbers, name of copybook, etc. ==
                        If asAppSettings.TestType = AppSettings.COBOL And asAppSettings.COBOLStartColumn > 1 Then
                            If asAppSettings.COBOLStartColumn > strLine.Length Then
                                strLine = ""
                            Else
                                strLine = strLine.Substring(asAppSettings.COBOLStartColumn - 1, strLine.Length - asAppSettings.COBOLStartColumn + 1)
                            End If
                        End If

                        If strLine.Trim() <> "" Then            ' Check that it's not a blank line
                            Dim strScanResult As String
                            If blnIsCommented = False Then      ' Check whether we're already inside a comment block

                                ' Multiple conditions for single line comments.
                                ' A lot of the difficulties caused by VB, COBOL and PHP which have two types of single line comment 
                                ' and VB/COBOL which have no multiple line comment.

                                If asAppSettings.TestType = AppSettings.COBOL And (strLine.Substring(0, 1) = asAppSettings.SingleLineComment) Then
                                    ' COBOL's system for whole-line comments is simpler than other languages
                                    strScanResult = ScanLine(CommentScan, CodeScan, strLine, strItem, asAppSettings.SingleLineComment, blnIsColoured)
                                ElseIf asAppSettings.TestType = AppSettings.COBOL And (strLine.Substring(0, 1) = strTrimmedComment And asAppSettings.IsZOS) Then
                                    ' COBOL's system for whole-line comments is simpler than other languages. IBM allows single line comments with '/'
                                    strScanResult = ScanLine(CommentScan, CodeScan, strLine, strItem, strTrimmedComment, blnIsColoured)
                                ElseIf asAppSettings.TestType = AppSettings.COBOL Then
                                    ' If line of COBOL has no comment in column 1, then it's all code
                                    rtResultsTracker.OverallCodeCount += 1
                                    rtResultsTracker.CodeCount += 1
                                    CheckCode(strLine, strItem)
                                ElseIf ((strLine.Contains(asAppSettings.SingleLineComment) And asAppSettings.SingleLineComment = "//" And Not strLine.ToLower.Contains("http:" + asAppSettings.SingleLineComment))) _
                                                                Or (asAppSettings.TestType = AppSettings.SQL And strLine.Contains(asAppSettings.SingleLineComment)) Or (asAppSettings.TestType = AppSettings.VB And strLine.Contains(asAppSettings.SingleLineComment) And Not (strLine.Contains("""") And (InStr(strLine, """") < InStr(strLine, "'")))) Then
                                    strScanResult = ScanLine(CommentScan, CodeScan, strLine, strItem, asAppSettings.SingleLineComment, blnIsColoured)
                                ElseIf (asAppSettings.AltSingleLineComment.Trim() <> "" And Regex.IsMatch(strLine, "\b" & asAppSettings.AltSingleLineComment & "\b")) Then
                                    strScanResult = ScanLine(CommentScan, CodeScan, strLine, strItem, strTrimmedComment, blnIsColoured)
                                ElseIf ((Not asAppSettings.TestType = AppSettings.VB And Not asAppSettings.TestType = AppSettings.COBOL) And (strLine.Contains(asAppSettings.BlockStartComment) And strLine.Contains(asAppSettings.BlockEndComment)) And (InStr(strLine, asAppSettings.BlockStartComment) < InStr(strLine, asAppSettings.BlockEndComment)) And Not (strLine.Contains("/*/"))) Then
                                    strScanResult = ScanLine(CommentScan, CodeScan, strLine, strItem, "both", blnIsColoured)
                                ElseIf (Not asAppSettings.TestType = AppSettings.VB And Not asAppSettings.TestType = AppSettings.COBOL) And (strLine.Contains(asAppSettings.BlockStartComment) And Not (strLine.Contains("/*/")) And Not (strLine.Contains("print") And (InStr(strLine, asAppSettings.BlockStartComment) > InStr(strLine, "print")))) Then
                                    blnIsCommented = True
                                    strScanResult = ScanLine(CommentScan, CodeScan, strLine, strItem, asAppSettings.BlockStartComment, blnIsColoured)
                                Else
                                    rtResultsTracker.OverallCodeCount += 1
                                    rtResultsTracker.CodeCount += 1

                                    ' Scan code for dangerous functions
                                    CheckCode(strLine, strItem)
                                End If

                            ElseIf (Not asAppSettings.TestType = AppSettings.VB And Not asAppSettings.TestType = AppSettings.COBOL) And strLine.Contains(asAppSettings.BlockEndComment) Then    ' End of a comment block
                                blnIsCommented = False
                                strScanResult = ScanLine(CommentScan, CodeScan, strLine, strItem, asAppSettings.BlockEndComment, blnIsColoured)
                            Else
                                rtResultsTracker.CommentCount += 1
                                rtResultsTracker.OverallCommentCount += 1
                            End If

                        Else
                            '== If we have whitespace then add it to the whitespace count ==
                            rtResultsTracker.OverallWhitespaceCount += 1
                            rtResultsTracker.WhitespaceCount += 1
                        End If
                    Next

                    '== List any file-level code issues (mis-matched deletes, mallocs, etc.) ==
                    If (asAppSettings.TestType = AppSettings.C Or asAppSettings.TestType = AppSettings.JAVA Or asAppSettings.TestType = AppSettings.COBOL) Then CheckFileLevelIssues(strItem)
                End If

                '== Update graphs and tables ==
                If asAppSettings.IsConsole = False Then
                    UpdateGraphs(strItem, arrShortName, blnIsColoured)
                    blnIsColoured = False
                End If

                rtResultsTracker.FileCount += 1

                '== Avoid the GUI locking or hanging during processing ==
                Application.DoEvents()
            Next
        End If

        ' Blank line so incrementing lines get a \n
        LogInfo("")

        '== Close output files if required ==
        If asAppSettings.IsOutputFile Then swOutputFile.Close()

        '== Export CSV results if required ==
        If asAppSettings.IsCsvOutputFile Then ExportResultsCSV(intFilterMin, intFilterMax, asAppSettings.CsvOutputFile)

        '== Export XML results if required ==
        If asAppSettings.IsXmlOutputFile Then ExportResultsXML(intFilterMin, intFilterMax, asAppSettings.XmlOutputFile)

        If asAppSettings.IsConsole Then
            LogInfo("Finished scanning...")
            LogInfo("Closing VCG.")
            Me.Close()
        End If

    End Sub

    Public Function ScanLine(CommentScan As Boolean, CodeScan As Boolean, CodeLine As String, FileName As String, Comment As String, ByRef IsColoured As Boolean) As Boolean
        ' Split the line into comments and code before carrying out the relevant checks
        ' N.B. - InStr has been used as Split requires a single character
        '==============================================================================

        Dim strCode As String = ""
        Dim strComment As String = ""

        Dim blnRetVal As Boolean = False

        Dim arrSubStrings() As String = {}
        Dim arrTemp() As String = {}

        Dim intPos As Integer


        '== Split line into comments and code ==
        If Comment = "both" Then
            intPos = InStr(CodeLine, asAppSettings.BlockStartComment)
            arrSubStrings = {CodeLine.Substring(0, intPos - 1), CodeLine.Substring(intPos + 1)}
            intPos = InStr(arrSubStrings(1), asAppSettings.BlockEndComment)
            arrTemp = {arrSubStrings(1).Substring(0, intPos - 1), arrSubStrings(1).Substring(intPos + 1)}
        Else
            intPos = InStr(CodeLine, Comment)
            If CodeLine.Length > intPos Then
                arrSubStrings = {CodeLine.Substring(0, intPos - 1), CodeLine.Substring(intPos + Comment.Length - 1)}
            Else
                arrSubStrings = {CodeLine.Substring(0, intPos - 1), ""}
            End If
        End If

        '== The position of code and comments in the array depends on type of comment ==
        If Comment = asAppSettings.SingleLineComment Or Comment = asAppSettings.BlockStartComment Then
            strCode = arrSubStrings(0).Trim()
            strComment = arrSubStrings(1).Trim()
        ElseIf Comment = asAppSettings.BlockEndComment Then
            strCode = arrSubStrings(1).Trim()
            strComment = arrSubStrings(0).Trim()
        ElseIf Comment = "both" Then
            strCode = arrSubStrings(0).Trim() + arrTemp(1).Trim()
            strComment = arrSubStrings(1).Trim()
        End If

        '== Check comment content ==
        If CommentScan And strComment.Length > 0 Then blnRetVal = CheckComment(strComment, FileName, IsColoured)

        '== Scan code for dangerous functions ==
        If CodeScan And strCode.Length > 0 Then CheckCode(strCode, FileName)

        Return blnRetVal

    End Function

    Public Function CheckComment(CodeLine As String, FileName As String, ByRef IsColoured As Boolean) As Boolean
        ' Scan comment content for anything requiring attention and return results
        '=========================================================================
        ' First two params used for the scan, and to create return val
        ' Final param passed by reference, solely to be altered for the purposes of calling func
        '-------------------------------------------------------------------------

        Dim blnRetVal As Boolean = False

        Dim fntTitleFont As New Font("Century Gothic", 9, FontStyle.Bold, GraphicsUnit.Point)
        Dim fntTextFont As New Font("Century Gothic", 9, FontStyle.Regular, GraphicsUnit.Point)

        Dim strTitle As String
        Dim strDescription As String


        rtResultsTracker.OverallCommentCount += 1
        rtResultsTracker.CommentCount += 1

        '== Look through comments for anything indicating unfixed or shoddy code ==
        For Each strComment As String In asAppSettings.BadComments
            If CodeLine.ToLower.Contains(strComment) Then
                blnRetVal = True

                strTitle = "Comment Indicates Potentially Unfinished Code - " & vbNewLine
                strDescription = " Line: " & rtResultsTracker.LineCount & " - " & FileName & vbNewLine
                WriteResult(strTitle, strDescription, CodeLine, CodeIssue.INFO)

                rtResultsTracker.FixMeCount += 1
                rtResultsTracker.OverallFixMeCount += 1
                IsColoured = True

                ' Update collection and listbox
                AddToResultCollection("Comment Indicates Potentially Unfinished Code", "The comment includes some wording which indicates that the developer regards it as unfinished or does not trust it to work correctly.", FileName, CodeIssue.INFO, rtResultsTracker.LineCount, CodeLine)

                Exit For
            End If
        Next

        '== Check for any passwords included in comments ==
        If Regex.IsMatch(CodeLine, "password\s*=") Then
            ' Update collection and listbox
            AddToResultCollection("Comment Appears to Contain Password", "The comment appears to include a password. If the password is valid then it could allow access to unauthorised users.", FileName, CodeIssue.HIGH, rtResultsTracker.LineCount, CodeLine)
            blnRetVal = True
        End If

        Return blnRetVal

    End Function

    Public Sub ListCodeIssue(FunctionName As String, Description As String, FileName As String, Optional Severity As Integer = CodeIssue.STANDARD, Optional CodeLine As String = "", Optional LineNumber As Integer = 0)
        ' Notify user of any code issues found for the bad function list from files or the language-specific tests
        '=========================================================================================================
        Dim strTitle As String
        Dim strDescription As String


        If asAppSettings.OutputLevel < Severity Then Exit Sub

        If LineNumber = 0 Then LineNumber = rtResultsTracker.LineCount

        ' Set issue title and description
        strTitle = "Potentially Unsafe Code - " & FunctionName & vbNewLine
        strDescription = "Line: " & LineNumber & " - " & FileName & vbNewLine & Description & vbNewLine

        WriteResult(strTitle, strDescription, CodeLine, Severity)

        ' Update data/count
        rtResultsTracker.BadFuncCount += 1
        rtResultsTracker.OverallBadFuncCount += 1

        ' Update collection and listbox
        AddToResultCollection(FunctionName, Description, FileName, Severity, LineNumber, CodeLine)

    End Sub

    Public Sub ListMemoryIssue(IssueDictionary As Dictionary(Of String, String))
        ' Notify user of any issues found in the dictionary of variable names and associated memory mis-management
        '=========================================================================================================
        Dim strTitle As String
        Dim strDescription As String = ""

        Dim arrDescriptions As String()
        Dim arrFragments As String()

        For Each kyKey In IssueDictionary.Keys
            strTitle = "Potential Memory Mis-management. Variable Name: " & kyKey & vbNewLine
            arrDescriptions = IssueDictionary(kyKey).Split("|")

            For Each strItem In arrDescriptions
                strDescription &= strItem & vbNewLine
            Next
            strDescription &= vbNewLine

            WriteResult(strTitle, strDescription, "")

            ' Update data/count
            rtResultsTracker.BadFuncCount += 1
            rtResultsTracker.OverallBadFuncCount += 1

            arrFragments = Regex.Split(arrDescriptions.Last, "FileName: ")

            ' Update collection and listbox
            AddToResultCollection("Potential Memory Mis-management. Variable Name: " & kyKey, arrDescriptions.First, arrFragments.Last.Trim)

        Next

    End Sub

    Public Sub WriteResult(Title As String, Description As String, CodeLine As String, Optional Severity As Integer = CodeIssue.STANDARD)
        ' Write results out to main screen in appropriate format
        '=======================================================

        Dim fntTitleFont As New Font("Century Gothic", 9, FontStyle.Bold, GraphicsUnit.Point)
        Dim fntTextFont As New Font("Century Gothic", 9, FontStyle.Regular, GraphicsUnit.Point)
        Dim fntCodeFont As New Font("Consolas", 9, FontStyle.Regular, GraphicsUnit.Point)

        If asAppSettings.OutputLevel < Severity Then Exit Sub


        If asAppSettings.IsConsole = False Then


            ' Set font style and colour for title
            rtbResults.Select(lngPosition, Len(Title))
            rtbResults.SelectionFont = fntTitleFont

            If Title.Trim <> "" Then
                Select Case Severity
                    Case CodeIssue.CRITICAL
                        rtbResults.SelectionColor = Color.Purple
                        Title = "CRITICAL: " & Title
                    Case CodeIssue.HIGH
                        rtbResults.SelectionColor = Color.Red
                        Title = "HIGH: " & Title
                    Case CodeIssue.MEDIUM
                        rtbResults.SelectionColor = Color.Orange
                        Title = "MEDIUM: " & Title
                    Case CodeIssue.LOW
                        rtbResults.SelectionColor = Color.CornflowerBlue
                        Title = "LOW: " & Title
                    Case CodeIssue.INFO
                        rtbResults.SelectionColor = Color.Blue
                        Title = "SUSPICIOUS COMMENT: " & Title
                    Case CodeIssue.POSSIBLY_SAFE
                        rtbResults.SelectionColor = Color.Green
                        Title = "POTENTIAL ISSUE: " & Title
                    Case Else
                        rtbResults.SelectionColor = Color.Goldenrod
                        Title = "STANDARD: " & Title
                End Select

                rtbResults.AppendText(Title)
                lngPosition += Len(Title)
            End If

            ' Set font style and colour for description
            rtbResults.Select(lngPosition, Len(Description))
            rtbResults.SelectionFont = fntTextFont
            rtbResults.SelectionColor = Color.Black

            rtbResults.AppendText(Description)
            lngPosition += Len(Description)

            ' Set font style and colour for code
            If CodeLine.Trim <> "" Then
                CodeLine &= vbNewLine & vbNewLine

                rtbResults.Select(lngPosition, Len(CodeLine))
                rtbResults.SelectionFont = fntCodeFont
                rtbResults.SelectionColor = Color.Black

                rtbResults.AppendText(CodeLine)
                lngPosition += Len(CodeLine)
            Else
                rtbResults.AppendText(vbNewLine)
                lngPosition += Len(vbNewLine)
            End If
        End If

        '== Write details to output files if required ==
        If asAppSettings.IsOutputFile Then
            swOutputFile.Write(Title)
            swOutputFile.Write(Description)
        End If

    End Sub

    Private Sub AddToResultCollection(Title As String, Description As String, FileName As String, Optional Severity As Integer = CodeIssue.STANDARD, Optional LineNumber As Integer = 0, Optional CodeLine As String = "", Optional IsChecked As Boolean = False, Optional CheckColour As String = "LawnGreen")
        ' Build results collection and add into results listbox
        '======================================================

        Dim srScanResult As New ScanResult
        Dim lviItem As New ListViewItem
        Dim colOriginalColour As Color = asAppSettings.ListItemColour
        Dim fgFileGroup As New FileGroup
        Dim igIssueGroup As New IssueGroup
        Dim ccConverter As New ColorConverter

        Dim arrInts As String()
        Dim intR, intG, intB As Integer     ' For colour represented as RGB components

        If asAppSettings.OutputLevel < Severity Then Exit Sub


        '== Add to results collection ==
        srScanResult.ItemKey = rtResultsTracker.CurrentIndex
        srScanResult.Title = Title
        srScanResult.Description = Description
        srScanResult.FileName = FileName
        srScanResult.SetSeverity(Severity)
        srScanResult.LineNumber = LineNumber
        srScanResult.CodeLine = CodeLine
        srScanResult.IsChecked = IsChecked

        If CheckColour.Contains(",") Then
            arrInts = CheckColour.Split(",")
            intR = CInt(arrInts(0))
            intG = CInt(arrInts(1))
            intB = CInt(arrInts(2))
            srScanResult.CheckColour = Color.FromArgb(intR, intG, intB)
        Else
            srScanResult.CheckColour = ccConverter.ConvertFromString(CheckColour)
        End If


        rtResultsTracker.ScanResults.Add(srScanResult)

        '== If this is a 'fix me' comment then add it to the comments collection ==
        If Severity = CodeIssue.INFO Then
            rtResultsTracker.FixMeList.Add(srScanResult)
        End If


        If Not asAppSettings.IsConsole Then
            '== Add to listview ==
            lviItem.Name = rtResultsTracker.CurrentIndex
            lviItem.Text = srScanResult.Severity
            lviItem.SubItems.Add(srScanResult.SeverityDesc)
            lviItem.SubItems.Add(Title)
            lviItem.SubItems.Add(Description)
            lviItem.SubItems.Add(FileName)
            lviItem.SubItems.Add(LineNumber)

            lvResults.Items.Add(lviItem)
            If srScanResult.IsChecked = True Then SetCheckState(lviItem.Name, True, srScanResult.CheckColour)
        End If

        '== Add to results groups ==
        If rtResultsTracker.FileGroups.ContainsKey(FileName) Then
            ' Add the issue to the array in dictionary entry for this file
            rtResultsTracker.FileGroups.Item(FileName).AddDetail(rtResultsTracker.CurrentIndex, Severity, Title, Description, LineNumber, CodeLine)
        Else
            ' Create a new file group and add the first issue
            fgFileGroup.FileName = FileName
            fgFileGroup.AddDetail(rtResultsTracker.CurrentIndex, Severity, Title, Description, LineNumber, CodeLine)
            rtResultsTracker.FileGroups.Add(FileName, fgFileGroup)
        End If

        If rtResultsTracker.IssueGroups.ContainsKey(Title) Then
            ' Add the issue to the array in dictionary entry for this general issue title
            rtResultsTracker.IssueGroups.Item(Title).AddDetail(rtResultsTracker.CurrentIndex, FileName, LineNumber, CodeLine)
        Else
            ' Create a new issue title group and add the first issue
            igIssueGroup.Title = Title
            igIssueGroup.SetSeverity(Severity)
            igIssueGroup.AddDetail(rtResultsTracker.CurrentIndex, FileName, LineNumber, CodeLine)
            rtResultsTracker.IssueGroups.Add(Title, igIssueGroup)
        End If

        rtResultsTracker.CurrentIndex += 1

        asAppSettings.ListItemColour = colOriginalColour

    End Sub

    Public Sub SortRTBResults(Optional SortCriteria As Integer = PRIORITY_COL)
        ' Sort results and display in rich text box
        '==========================================
        Dim scSevComp As New SeverityComparer
        Dim fcFileComp As New FileComparer
        Dim strDescription As String = ""
        Dim blnCurrentOrder As Boolean


        rtbResults.Text = ""

        Select Case SortCriteria
            Case PRIORITY_COL
                blnCurrentOrder = blnIsAscendingSeverity
                blnIsAscendingSeverity = True
                rtResultsTracker.ScanResults.Sort(scSevComp)
                blnIsAscendingSeverity = blnCurrentOrder
            Case FILE_COL
                blnCurrentOrder = blnIsAscendingFile
                blnIsAscendingFile = True
                rtResultsTracker.ScanResults.Sort(fcFileComp)
                blnIsAscendingFile = blnCurrentOrder
            Case Else
                ' Do Nothing
        End Select

        For Each srResultItem In rtResultsTracker.ScanResults
            If (srResultItem.Severity <= intFilterMin And srResultItem.Severity >= intFilterMax) Then
                strDescription = srResultItem.Description
                WriteResult(srResultItem.Title & vbNewLine, strDescription & vbNewLine & "Line: " & srResultItem.LineNumber & " - Filename: " & srResultItem.FileName & vbNewLine, srResultItem.CodeLine.Trim, srResultItem.Severity)
            End If

            '== Avoid the GUI locking or hanging during processing ==
            'Application.DoEvents()
        Next

    End Sub

    Private Sub GroupRTBByIssue()
        ' Re-write RTB results, grouped by issue
        '=======================================
        Dim strDescription As String = ""
        Dim strTitle As String = ""
        Dim blnIsFirst As Boolean = True


        rtbResults.Text = ""

        ' Loop through the issues
        For Each idIssueDic As KeyValuePair(Of String, IssueGroup) In rtResultsTracker.IssueGroups

            strTitle = idIssueDic.Key
            strDescription = ""

            ' Check it is within range of severity filter
            If (idIssueDic.Value.Severity <= intFilterMin And idIssueDic.Value.Severity >= intFilterMax) Then
                For Each arrDetails As KeyValuePair(Of Integer, String()) In idIssueDic.Value.GetDetails
                    strDescription &= vbNewLine & "Line: " & arrDetails.Value(IssueGroup.LINE) & " - Filename: " & arrDetails.Value(IssueGroup.FILE)
                    If arrDetails.Value(IssueGroup.CODE).Trim <> "" Then strDescription &= vbNewLine & arrDetails.Value(IssueGroup.CODE).Trim
                Next
            End If

            WriteResult(strTitle, strDescription & vbNewLine, "", idIssueDic.Value.Severity)

            '== Avoid the GUI locking or hanging during processing ==
            'Application.DoEvents()
        Next

    End Sub

    Private Sub GroupRTBByFile()
        ' Re-write RTB results, grouped by file
        '======================================
        Dim strDescription As String = ""
        Dim intSeverity As Integer = CodeIssue.POSSIBLY_SAFE
        Dim strTitle As String = ""
        Dim blnIsFirst As Boolean = True


        rtbResults.Text = ""

        ' Loop through the issues
        For Each fdFileDic As KeyValuePair(Of String, FileGroup) In rtResultsTracker.FileGroups

            strTitle = fdFileDic.Key
            strDescription = ""
            intSeverity = CodeIssue.POSSIBLY_SAFE

            For Each arrDetails As KeyValuePair(Of Integer, String()) In fdFileDic.Value.GetDetails

                If intSeverity > fdFileDic.Value.GetSeverity(arrDetails.Key) Then intSeverity = fdFileDic.Value.GetSeverity(arrDetails.Key)

                If (intSeverity <= intFilterMin And intSeverity >= intFilterMax) Then
                    strDescription &= vbNewLine & "Line: " & arrDetails.Value(FileGroup.LINE) & " - " & arrDetails.Value(FileGroup.SEVERITY) & ": " & arrDetails.Value(FileGroup.ISSUE) & vbNewLine & arrDetails.Value(FileGroup.DESC)
                    If arrDetails.Value(IssueGroup.CODE).Trim <> "" Then strDescription &= arrDetails.Value(IssueGroup.CODE).Trim
                End If
            Next

            ' only write to screen if the issue is within filter bounds
            If (intSeverity <= intFilterMin And intSeverity >= intFilterMax) Then WriteResult(strTitle, strDescription & vbNewLine, "", intSeverity)
        Next

        '== Avoid the GUI locking or hanging during processing ==
        'Application.DoEvents()

    End Sub

    Public Sub UpdateGraphs(FileName As String, ShortName() As String, IsColoured As Boolean)
        ' Populate the display of file data and store for later re-use
        '=============================================================
        '== Store for later use ==
        Dim fdFileData As New FileData With {
            .ShortName = ShortName(ShortName.Count - 1),
            .FileName = FileName,
            .LineCount = rtResultsTracker.LineCount,
            .CodeCount = rtResultsTracker.CodeCount,
            .CommentCount = rtResultsTracker.CommentCount,
            .WhitespaceCount = rtResultsTracker.WhitespaceCount,
            .FixMeCount = rtResultsTracker.FixMeCount,
            .BadFuncCount = rtResultsTracker.BadFuncCount
        }
        rtResultsTracker.FileDataList.Add(fdFileData)

        '== Add to chart ==
        If Not asAppSettings.IsConsole Then UpdateFileView(fdFileData, rtResultsTracker.FileCount, IsColoured)

        '== Clear variables ==
        rtResultsTracker.ResetFileCountVars()

    End Sub

    Public Sub UpdateFileView(FileDetails As FileData, Index As Integer, IsColoured As Boolean)

        '== Add results into graphs and tables ==
        With frmBreakdown.dgvResults
            .Rows.Add(1)
            If IsColoured = True Then
                .DefaultCellStyle.BackColor = Color.Red
            Else
                .DefaultCellStyle.BackColor = Color.White
            End If
            .Item(0, Index).Value = FileDetails.ShortName
            .Item(1, Index).Value = FileDetails.LineCount
            .Item(3, Index).Value = FileDetails.CodeCount
            .Item(4, Index).Value = FileDetails.CommentCount
            .Item(5, Index).Value = FileDetails.WhitespaceCount
            .Item(6, Index).Value = FileDetails.FixMeCount
            .Item(7, Index).Value = FileDetails.BadFuncCount
            .Item(8, Index).Value = FileDetails.FileName
        End With

    End Sub

    Public Shared Sub CountFixMeComments(FileName As String)
        ' If fixme array is not empty, and filename for item is same as in codebreakdown results will populate panel with relevantLogInfo 
        '==============================================================================================================================

        If Not rtResultsTracker.FixMeList.Count = Nothing Then

            For Each srItem As ScanResult In rtResultsTracker.FixMeList
                If srItem.FileName = FileName Then
                    frmIndividualBreakdown.pnlResults.Visible = True
                    frmIndividualBreakdown.lblBreakdown.Text += "Line: " & srItem.LineNumber & vbNewLine & "Contains: '" & srItem.CodeLine & "'" & vbNewLine & vbNewLine
                End If
            Next
        End If

    End Sub

    Shared Sub ExportComments()
        ' Display all comments which indicate poor/unfinished code as Rich Text
        '======================================================================

        Dim strTitle As String
        Dim strDetails As String


        If rtResultsTracker.OverallLineCount = 0 Then Exit Sub

        frmCodeDetails.Close()
        frmCodeDetails.rtbCodeDetails.Clear()

        With frmMain
            .ShowLoading("Formatting Results", "Formatting .... ", rtResultsTracker.FixMeList.Count)

            For Each srItem As ScanResult In rtResultsTracker.FixMeList
                strTitle = vbNewLine & "File: " & srItem.FileName & vbNewLine
                strDetails = "Line: " & srItem.LineNumber & vbNewLine & "Contains: '" & srItem.CodeLine & "'" & vbNewLine
                .SetRtbText(strTitle)
                .SetRtbCode(strDetails)
                .IncrementLoadingBar(strTitle)
            Next
        End With


        frmCodeDetails.rtbCodeDetails.Select(0, 0)
        frmLoading.Close()
        frmCodeDetails.Show()

    End Sub

    Public Sub SetRtbCode(CodeString As String)
        'Set format for the details screen and and display the offending code
        '====================================================================

        Dim fntFont As New Font("Courier New", 9, FontStyle.Regular, GraphicsUnit.Point)


        ' == NB. it is 71 chars that fill up a typical (vertical horizontal) whitepsace on a word doc ==
        With frmCodeDetails
            .rtbCodeDetails.Select(Len(frmCodeDetails.rtbCodeDetails.Text), 1)
            .rtbCodeDetails.SelectionFont = fntFont
            .rtbCodeDetails.SelectionColor = Color.Black
            .rtbCodeDetails.SelectionBackColor = Color.LightGray
            .rtbCodeDetails.AppendText(CodeString)
        End With

    End Sub

    Public Sub SetRtbText(TitleString As String)
        'Set format for the details screen and give details of location
        '==============================================================

        Dim fntTextFont As New Font("Century Gothic", 10, FontStyle.Regular, GraphicsUnit.Point)

        With frmCodeDetails
            .rtbCodeDetails.Select(Len(frmCodeDetails.rtbCodeDetails.Text), 1)
            .rtbCodeDetails.SelectionFont = fntTextFont
            .rtbCodeDetails.SelectionColor = Color.Black
            .rtbCodeDetails.AppendText(TitleString)
        End With

    End Sub

    Public Sub ShowLoading(Title As String, StatusMessage As String, Optional NumberOfItems As Integer = 0, Optional MaxItems As Integer = -1)

        LogVerbose(StatusMessage)

        If asAppSettings.IsConsole Then Exit Sub

        Me.ssStatusStrip.Text = Title

        Try
            With frmLoading
                ' Prevent exceptions generated by Show()ing visible windows
                If (frmLoading.Visible = False) Then
                    .Show(Me)
                End If

                If MaxItems = -1 Then
                    .pbProgressBar.Maximum = NumberOfItems
                Else
                    .pbProgressBar.Maximum = MaxItems
                End If

                .Text = Title
                .lblProgress.Text = StatusMessage
                .pbProgressBar.Value = NumberOfItems
                .Refresh()
            End With
        Catch excep As ArgumentOutOfRangeException
            ' The maximum and number of items are possibly inconsistent
            Throw
        End Try
    End Sub

    Public Sub IncrementLoadingBar(LabelText As String, Optional Maximum As Integer = -1)

        ' Progress bar control holds the state of the operation. Always update that first.

        If Maximum = -1 Then
            frmLoading.pbProgressBar.Maximum = frmLoading.pbProgressBar.Value
        Else
            frmLoading.pbProgressBar.Maximum = Maximum
        End If

        With frmLoading
            .pbProgressBar.Increment(1)
            .lblProgress.Text = LabelText
            ' .Refresh() 'TODO: make the loading bar represent what is being checked for, ie. the title would be "unsafe methods", "Pointer problems", etc. and go to 100 then re-loop
        End With

        Dim newValue = frmLoading.pbProgressBar.Value

        ' For console, if Max =-1, then we don't know the status.
        If Maximum = -1 Then
            LogVerbose($"[ {newValue} / ? ] complete: {LabelText}")
        Else
            LogVerbose($"[ {newValue} / {Maximum} ] complete: {LabelText}")
        End If



    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub BannedFunctionsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles BannedFunctionsToolStripMenuItem.Click
        ' Show relevant bad functions file for language
        '==============================================

        Select Case asAppSettings.TestType
            Case AppSettings.C
                LaunchNPP(asAppSettings.CConfFile)
            Case AppSettings.JAVA
                LaunchNPP(asAppSettings.JavaConfFile)
            Case AppSettings.SQL
                LaunchNPP(asAppSettings.PLSQLConfFile)
            Case AppSettings.CSHARP
                LaunchNPP(asAppSettings.CSharpConfFile)
            Case AppSettings.PHP
                LaunchNPP(asAppSettings.PHPConfFile)
        End Select

    End Sub

    Private Sub StartScanningToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles StartScanningToolStripMenuItem.Click
        ' Ensure that config-only scanning is switched off before proceeding with full scan
        '==================================================================================

        asAppSettings.IsConfigOnly = False

        FullScan()

    End Sub

    Private Sub FullScan()
        ' Run the code breakdown from the main form
        '==========================================
        Dim intIndex As Integer


        '== GUI or console? ==
        If asAppSettings.IsConsole Then
            frmLoading.Hide()
            ScanFiles(True, True)
            Me.Hide()
        Else
            ' Disable some controls, so they don't interfere with the scan
            cboTargetDir.Enabled = False
            cboLanguage.Enabled = False
            '== If no data available then scan files mode ==
            If rtResultsTracker.OverallLineCount = 0 Then
                frmBreakdown.dgvResults.SuspendLayout()
                frmBreakdown.dgvResults.Rows.Clear()
                ScanFiles(True, True)
                frmBreakdown.dgvResults.ResumeLayout()
            Else
                For intIndex = 0 To rtResultsTracker.FileDataList.Count - 1
                    UpdateFileView(rtResultsTracker.FileDataList.Item(intIndex), intIndex, False)
                Next
            End If

            ShowResults()
            cboTargetDir.Enabled = True
            cboLanguage.Enabled = True
        End If

    End Sub

    Private Sub ShowPercentages()
        ' Fill in percentages on the datagram thing 
        '==========================================
        On Error Resume Next

        Dim intIndex As Integer

        If frmBreakdown.dgvResults.Rows.Count < 2 Then Exit Sub

        ' ToDo: This throws an exception during integer conversion. If possible find a fix and remove the "On Error Resume Next"
        For intIndex = 0 To frmBreakdown.dgvResults.Rows.Count - 2
            frmBreakdown.dgvResults.Item(2, intIndex).Value = Math.Round((Math.Abs((Int(frmBreakdown.dgvResults.Item(1, intIndex).Value) / rtResultsTracker.OverallLineCount) * 100)), 3)
        Next intIndex

    End Sub

    Private Sub ShowPieChart()
        ' Display pie chart with code breakdown
        '======================================

        '== Create chart ==
        With frmBreakdown.chtResults
            .Series(0).Points.AddY(rtResultsTracker.OverallCodeCount)
            .Series(0).Points.AddY(rtResultsTracker.OverallWhitespaceCount)
            .Series(0).Points.AddY(rtResultsTracker.OverallCommentCount)
            .Series(0).Points.AddY(rtResultsTracker.OverallFixMeCount)
            .Series(0).Points.AddY(rtResultsTracker.OverallBadFuncCount)

            .Series(0).Points(0).LegendText = "Overall code (" & rtResultsTracker.OverallCodeCount & " lines)"
            .Series(0).Points(1).LegendText = "Overall whitespace (" & rtResultsTracker.OverallWhitespaceCount & " lines)"
            .Series(0).Points(2).LegendText = "Overall comments (" & rtResultsTracker.OverallCommentCount & " comments)"
            .Series(0).Points(3).LegendText = "Potentially broken/unfinished flags (" & rtResultsTracker.OverallFixMeCount & " Counts)"
            .Series(0).Points(4).LegendText = "Potentially dangerous code (" & rtResultsTracker.OverallBadFuncCount & " Counts)"

            .Series("Series1")("BarLabelStyle") = "Right"
            .ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
            .Series("Series1")("DrawingStyle") = "Cylinder"
        End With

        '== Show percentage breakdowns ==
        frmBreakdown.lblResults.Text = rtResultsTracker.OverallLineCount & " Lines: " & vbNewLine & rtResultsTracker.OverallCommentCount &
            " Comments (~" & Math.Round(Math.Abs((rtResultsTracker.OverallCommentCount / rtResultsTracker.OverallLineCount) * 100), 1) & "%)" & vbNewLine & rtResultsTracker.OverallWhitespaceCount &
            " Lines of Whitespace (~" & Math.Round(Math.Abs((rtResultsTracker.OverallWhitespaceCount / rtResultsTracker.OverallLineCount) * 100), 1) & "%)" & vbNewLine & rtResultsTracker.OverallLineCount - (rtResultsTracker.OverallCommentCount + rtResultsTracker.OverallWhitespaceCount) &
            " Lines of Code (including comment-appended code) (~" & (100 - ((Math.Round(Math.Abs((rtResultsTracker.OverallCommentCount / rtResultsTracker.OverallLineCount) * 100), 1) + Math.Round(Math.Abs((rtResultsTracker.OverallWhitespaceCount / rtResultsTracker.OverallLineCount) * 100), 1)))) & "%)"

    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub VisualSecurityBreakdownToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles VisualSecurityBreakdownToolStripMenuItem.Click
        ' Show breakdown of good code/bad code
        '=====================================
        Dim intIndex As Integer

        asAppSettings.IsConfigOnly = False

        '== If no data available then scan files in 'code only' mode ==
        If rtResultsTracker.OverallLineCount = 0 Then
            ScanFiles(False, True)
        Else
            For intIndex = 0 To rtResultsTracker.FileDataList.Count - 1
                UpdateFileView(rtResultsTracker.FileDataList.Item(intIndex), intIndex, False)
            Next
        End If

        ShowResults()

    End Sub

    Private Sub ShowResults(Optional ShowReminder As Boolean = True)
        ' Display the results in the GUI according to user preferences
        '=============================================================


        '== Exit if in cmd line only mode ==
        If asAppSettings.IsConsole = True Then Exit Sub

        '== Show results ==
        tcMain.SelectTab(1)
        tabResults.Focus()
        tabResults.Show()
        frmLoading.Close()

        '== Show options for visual breakdown ==
        If ShowReminder And asAppSettings.DisplayBreakdownOption = True Then
            frmBreakdownReminder.ShowDialog()
        End If

        '== Show Visual Breakdown if required ==
        If ((Not ShowReminder) Or (asAppSettings.VisualBreakdownEnabled)) Then
            ShowPercentages()
            ShowPieChart()
            frmBreakdown.Show()
        End If

    End Sub

    Private Sub VisualCommentBreakdownToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles VisualCommentBreakdownToolStripMenuItem.Click
        ' Show breakdown of code/comments and ToDo/FixMe comments
        '========================================================
        Dim intIndex As Integer

        asAppSettings.IsConfigOnly = False

        '== If no data available then scan files in 'code only' mode ==
        If rtResultsTracker.OverallLineCount = 0 Then
            ScanFiles(True, False)
        Else
            For intIndex = 0 To rtResultsTracker.FileDataList.Count - 1
                UpdateFileView(rtResultsTracker.FileDataList.Item(intIndex), intIndex, False)
            Next
        End If

        ShowResults()

    End Sub

    Private Sub AboutVCGToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AboutVCGToolStripMenuItem.Click
        Dim frmAbout As New AboutBox
        frmAbout.ShowDialog(Me)
    End Sub

    Private Sub VisualCodeBreakdownToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles VisualCodeBreakdownToolStripMenuItem.Click
        ' Show breakdown of code/comments/whitespace
        '===========================================
        Dim intIndex As Integer

        '== If no data available then scan files in 'comment only' mode ==
        If rtResultsTracker.OverallLineCount = 0 Then
            asAppSettings.IsConfigOnly = False
            ScanFiles(True, False)
        Else
            For intIndex = 0 To rtResultsTracker.FileDataList.Count - 1
                UpdateFileView(rtResultsTracker.FileDataList.Item(intIndex), intIndex, False)
            Next
        End If

        ShowResults(False)

    End Sub

    Private Sub ExportFixMeCommentsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExportFixMeCommentsToolStripMenuItem.Click
        ExportComments()
    End Sub

    Private Sub OptionsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles OptionsToolStripMenuItem.Click
        ' Show the options dialog
        '========================
        Dim frmOpt As New frmOptions

        frmOpt.ShowDialog(Me)

    End Sub

    Private Sub FilterResultsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FilterResultsToolStripMenuItem.Click
        ' Filter results according to user requirements
        '==============================================

        Dim frmResultFilter As New frmFilter
        frmResultFilter.ShowDialog(Me)

    End Sub

    Private Sub DeleteItemToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles DeleteItemToolStripMenuItem.Click
        DeleteScanResult()
    End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' Save settings to registry
        '==========================
        Dim intIndex As Integer


        ' Is the main Window visible or not?
        If asAppSettings.IsConsole = False Then
            ' Save window size and location to Registry
            SaveSetting("VisualCodeGrepper", "FormLocation", "Top", Me.Top)
            SaveSetting("VisualCodeGrepper", "FormLocation", "Left", Me.Left)
            SaveSetting("VisualCodeGrepper", "FormSize", "Height", Me.Height)
            SaveSetting("VisualCodeGrepper", "FormSize", "Width", Me.Width)

            ' Guard check, in case they use the --help flag
            If (cboTargetDir.Items.Count > 0) Then
                ' Save previous filenames to Registry 
                For intIndex = 0 To 4
                    SaveSetting("VisualCodeGrepper", "Startup", "Target" & intIndex, cboTargetDir.Items().Item(intIndex))
                Next intIndex
            End If
        Else
            modNativeMethods.DetachConsole()
        End If

        ' Save Language and test settings to registry
        SaveSetting("VisualCodeGrepper", "Startup", "Language", asAppSettings.StartType)
        SaveSetting("VisualCodeGrepper", "Startup", "CConfFile", asAppSettings.CConfFile)
        SaveSetting("VisualCodeGrepper", "Startup", "JavaConfFile", asAppSettings.JavaConfFile)
        SaveSetting("VisualCodeGrepper", "Startup", "PLSQLConfFile", asAppSettings.PLSQLConfFile)
        SaveSetting("VisualCodeGrepper", "Startup", "CSConfFile", asAppSettings.CSharpConfFile)
        SaveSetting("VisualCodeGrepper", "Startup", "VBConfFile", asAppSettings.VBConfFile)
        SaveSetting("VisualCodeGrepper", "Startup", "PHPConfFile", asAppSettings.PHPConfFile)
        SaveSetting("VisualCodeGrepper", "Startup", "COBOLConfFile", asAppSettings.COBOLConfFile)

    End Sub

    Private Sub frmMain_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        ' Implement keyboard shortcuts
        '=============================

        If e.KeyCode = Keys.F And e.Modifiers = Keys.Control Then   ' Find
            tcMain.SelectedTab = tabResults
            tabResults.Focus()
            FindText()
        ElseIf e.KeyCode = Keys.F And e.Modifiers = Keys.Control And e.Modifiers = Keys.Shift Then   ' Find next
            tcMain.SelectedTab = tabResults
            tabResults.Focus()
            If strPrevSearch <> "" Then rtbResults.Find(strPrevSearch)
        ElseIf (e.KeyCode = Keys.F5 Or (e.KeyCode = Keys.R And e.Modifiers = Keys.Control)) And rtResultsTracker.TargetDirectory <> "" Then   ' Scan
            FullScan()
        End If

    End Sub

    Private Sub frmMain_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        ' Get settings from registry and apply to app
        '============================================
        AttachToConsoleIfCliRequested()
        AttachDebuggerIfConsoleRequestsIt()

        '== Get bad comments from config file ==
        LoadBadComments()

        ' Get Language and test settings from registry
        asAppSettings.TestType = GetSetting("VisualCodeGrepper", "Startup", "Language", "0")
        asAppSettings.CConfFile = GetSetting("VisualCodeGrepper", "Startup", "CConfFile", Application.StartupPath & "\cppfunctions.conf")
        asAppSettings.JavaConfFile = GetSetting("VisualCodeGrepper", "Startup", "JavaConfFile", Application.StartupPath & "\javafunctions.conf")
        asAppSettings.PLSQLConfFile = GetSetting("VisualCodeGrepper", "Startup", "PLSQLConfFile", Application.StartupPath & "\plsqlfunctions.conf")
        asAppSettings.CSharpConfFile = GetSetting("VisualCodeGrepper", "Startup", "CSConfFile", Application.StartupPath & "\csfunctions.conf")
        asAppSettings.VBConfFile = GetSetting("VisualCodeGrepper", "Startup", "VBConfFile", Application.StartupPath & "\vbfunctions.conf")
        asAppSettings.PHPConfFile = GetSetting("VisualCodeGrepper", "Startup", "PHPConfFile", Application.StartupPath & "\phpfunctions.conf")
        asAppSettings.COBOLConfFile = GetSetting("VisualCodeGrepper", "Startup", "COBOLConfFile", Application.StartupPath & "\cobolfunctions.conf")


        ' Parse and process any command line args
        Dim intIndex = ParseArgs()

        ' Early exit
        If intIndex = EXIT_CODE Then
            Me.Close()
            Return
        End If

        If asAppSettings.IsConsole = False Then
            ' If asAppSettings.IsConsole Then AttachConsole(-1)

            ' Implement context menu for text boxes, etc.
            AddContextMenu()
            PopulateLanguageOptions()

            ' Get previous window size and location from Registry
            Me.Top = GetSetting("VisualCodeGrepper", "FormLocation", "Top", "50")
            Me.Left = GetSetting("VisualCodeGrepper", "FormLocation", "Left", "100")
            Me.Height = GetSetting("VisualCodeGrepper", "FormSize", "Height", "515")
            Me.Width = GetSetting("VisualCodeGrepper", "FormSize", "Width", "835")

            ' Reset any bad/corrupted registry entries
            If (Me.Top < 0) Or (Me.Top > Screen.PrimaryScreen.Bounds.Height - 50) Then Me.Top = 50
            If (Me.Left < 0) Or (Me.Left > Screen.PrimaryScreen.Bounds.Width - 50) Then Me.Left = 100

            ' Set current language
            If asAppSettings.TestType = AppSettings.COBOL Then asAppSettings.IncludeCobol = True
            SelectLanguage(asAppSettings.TestType)

            ' Get previous filenames from registry and load into combo box
            For intIndex = 0 To 4
                cboTargetDir.Items.Insert(intIndex, GetSetting("VisualCodeGrepper", "Startup", "Target" & intIndex, ""))
            Next intIndex

            If asAppSettings.IsXmlInputFile Then ImportResultsXML(asAppSettings.XmlInputFile)

            asAppSettings.DisplayBreakdownOption = (GetSetting("VisualCodeGrepper", "DisplayOptions", "BreakdownReminder", "1") = "1")  ' default to true
            asAppSettings.VisualBreakdownEnabled = (GetSetting("VisualCodeGrepper", "DisplayOptions", "ShowBreakdown", "0") <> "0")     ' default to false
        Else
            Me.Visible = False

            ' Console apps just load files and scan. Thats it
            Me.LoadFiles(rtResultsTracker.TargetDirectory)
            FullScan()
        End If



    End Sub

    Private Sub AttachDebuggerIfConsoleRequestsIt()
        If Environment.GetCommandLineArgs.Contains("--debug") Then
            If (Debugger.IsAttached = False) Then
#If DEBUG Then
                Debugger.Launch()
#End If
            End If
        End If

    End Sub

    ''' <summary>
    ''' If running as a console app, keep the application attached to the console.
    ''' </summary>
    ''' <remarks>
    ''' Windows Forms detach the console once started. To provide CLI output we need to grab the console quickly and hold a reference to it.
    ''' This way, the application can use Console.Out/Error commands successfully. 
    ''' 
    ''' Otherwise, the Console will receive .Out events once the prompt has been returned, causing user confusion.
    ''' </remarks>
    Private Sub AttachToConsoleIfCliRequested()
        Dim args = Environment.GetCommandLineArgs

        If (args.Contains("--console") Or args.Contains("-c")) Then
            asAppSettings.IsConsole = True
            AttachProcessToConsole()
        End If
    End Sub

    Private Sub frmMain_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        'If we are in console mode then begin the scan, otherwise show the form
        '======================================================================

        If asAppSettings.IsConsole = True Then
            Me.Visible = False
            FullScan()
        End If

    End Sub

    Private Sub cboTargetDir_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs)
        ' Allow user to drag file/directory into combobox
        '================================================
        Dim arrFiles() As String

        If e.Data.GetDataPresent(DataFormats.FileDrop) Then

            ' Assign dragged files to array of filenames
            arrFiles = e.Data.GetData(DataFormats.FileDrop)

            ' Assign first name in list to combo box
            cboTargetDir.Text = arrFiles(0)
        End If

    End Sub

    Private Sub cboTargetDir_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs)
        ' Enable dragging of item(s) into combobox
        '=========================================

        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.All
        End If

    End Sub

    Private Sub cboTargetDir_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cboTargetDir.KeyDown
        ' If user presses the 'Enter' key then attemp to scan the directory they've specified
        '====================================================================================

        If e.KeyCode = Keys.Enter Then
            If (cboLanguage.Text = "") Then
                MessageBox.Show(Me, "Please select a language to scan", "Validation Error", MessageBoxButtons.OK)
                Return
            Else
                LoadFiles(cboTargetDir.Text)
            End If
        End If
    End Sub

    Private Sub lbTargets_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lbTargets.SelectedIndexChanged
        SetDeleteMenu()
    End Sub

    Private Sub tcMain_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles tcMain.SelectedIndexChanged
        SetDeleteMenu()
    End Sub

    Public Sub LoadFiles(TargetFolder As String, Optional ClearPrevious As Boolean = True)

        'If TargetFolder.Count = 0 Then Exit Sub

        TargetFolder = TargetFolder.Trim
        'If TargetFolder = "" Or TargetFolder = rtResultsTracker.TargetDirectory Then Exit Sub

        If asAppSettings.IsConsole = False Then
            If Directory.Exists(TargetFolder) = False Then
                MessageBox.Show(Me, "Target directory does not exist.", "Collecting files", MessageBoxButtons.OK)
                Return
            End If

            If lbTargets.Items.Count > 0 And asAppSettings.IsAllFileTypes = False Then

                tcMain.SelectTab(0)

                Dim result = MessageBox.Show(Me, "Target files are already filtered. Are you sure you want to rescan?", "Scan Target Folder", MessageBoxButtons.OKCancel)
                If result <> DialogResult.OK Then
                    Return
                Else
                    ClearPrevious = True
                End If
            End If

            If ClearPrevious Then ClearResults()

            ShowLoading("Collecting Files", "Loading files from target directory...", 0)
        Else
            LogInfo("Loading files from target directory...")
        End If

        Try
            Dim allFiles = Directory.GetFiles(TargetFolder, "*.*", IO.SearchOption.AllDirectories)
            Dim lstResults = New List(Of String)

            ' Iterate through files from target directory and obtain all relevant filenames
            If asAppSettings.IsAllFileTypes = False Then
                Dim count = 0
                For Each file In allFiles

                    ' If they force an exit from the loading screen, abort the operation.
                    ' Do this first, as otherwise the normal closure of the window will be marked as an Abort
                    If asAppSettings.AbortCurrentOperation = True Then
                        asAppSettings.AbortCurrentOperation = False
                        MessageBox.Show(Me, "Target scan has been aborted. File list may be incomplete", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If

                    count += 1
                    If CheckFileType(file) Then
                        lstResults.Add(file)
                        LogVerbose("Found file: " & file)
                    End If

                    If count Mod 1000 = 0 And lstResults.Count > 0 Then
                        ShowLoading("Collecting Files", $"In Progress: After {count} checks, only {lstResults.Count} match the criteria of {allFiles.Count} files", count, allFiles.Count)
                    End If
                Next
            End If

            rtResultsTracker.FileList.AddRange(lstResults)

            If asAppSettings.IsConsole = False Then
                frmLoading.Close()
            Else
                LogBlank()
                LogInfo("Loaded " & rtResultsTracker.FileList.Count & " files from target directory.")
                LogBlank()
            End If

            If rtResultsTracker.FileList.Count = 0 Then
                DisplayError($"No {cboLanguage.Text} file types could be found within the '{cboTargetDir.Text}' directory", "Collecting files", MsgBoxStyle.Information)
                Return
            End If

            'MsgBox(rtResultsTracker.FileList.Count & " Files loaded", MsgBoxStyle.Information, "Success")
            If asAppSettings.IsConsole = False Then
                sslLabel.Text += "   [" & rtResultsTracker.FileList.Count & " Files]"

                ' Enable scan menus
                SetScanMenus(True)

                lbTargets.SuspendLayout()
                lbTargets.Items.AddRange(rtResultsTracker.FileList.ToArray())
                lbTargets.ResumeLayout()

                rtResultsTracker.TargetDirectory = TargetFolder
                cboTargetDir.Text = TargetFolder

                ''== Add to list of previous targets ==
                'If Not cboTargetDir.Items.Contains(cboTargetDir.Text) Then
                '    ' Move first 4 items along to next space in the list
                '    For intIndex = 3 To 0 Step -1
                '        cboTargetDir.Items().Item(intIndex + 1) = cboTargetDir.Items().Item(intIndex)
                '    Next intIndex
                '    cboTargetDir.Items().Item(0) = cboTargetDir.Text
                'End If
            End If

        Catch exError As Exception
            DisplayError(exError, "Error", MsgBoxStyle.Critical)
        End Try

    End Sub

    Private Sub SaveResultsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SaveResultsToolStripMenuItem.Click
        ' Show dialog box for output file and save results from RichTextBox
        '==================================================================
        Dim strResultsFile As String = ""
        Dim strResults As String = ""

        Dim intIndex As Integer = 0

        Dim swResultFile As StreamWriter


        '== Get filename from user ==
        sfdSaveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"


        If sfdSaveFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            strResultsFile = sfdSaveFileDialog.FileName
        End If
        If strResultsFile.Trim = "" Then Exit Sub


        Try
            '== Open file ==
            swResultFile = New StreamWriter(strResultsFile)

            ShowLoading("Exporting Results", "Exporting results to file...", rtbResults.Lines.Count)

            '== Write results ==
            For Each strLine In rtbResults.Lines()
                swResultFile.WriteLine(strLine)
                intIndex += 1
                IncrementLoadingBar("Line: " & intIndex)
            Next

            frmLoading.Close()

            '== Close file ==
            swResultFile.Close()

        Catch exError As Exception
            DisplayError(exError.Message, "Error", MsgBoxStyle.Critical)
        End Try

    End Sub

    Private Sub AddContextMenu()
        ' Provide pop-up-menu for the relevant controls
        '==============================================

        If asAppSettings.IsConsole = True Then Exit Sub

        Dim cmFullContextMenu As New ContextMenu        ' The filenames combobox allows cut/copy/paste
        Dim cmResultsContextMenu As New ContextMenu     ' The results are just for copying, not modification
        Dim cmResultsListContextMenu As New ContextMenu ' The results table allows a file to be opened in its associated app or Notepad++

        ' ComboBox
        Dim menuItem1Cut As New MenuItem("Cut")
        Dim menuItem2Copy As New MenuItem("Copy")
        Dim menuItem3Paste As New MenuItem("Paste")
        Dim menuItem4Divider As New MenuItem("-")
        Dim menuItem5SelectAll As New MenuItem("Select All")

        ' RichTextBox
        Dim menuItem6Copy As New MenuItem("Copy")
        Dim menuItem9Divider As New MenuItem("-")
        Dim menuItem10SelectAll As New MenuItem("Select All")
        Dim menuItem7Divider As New MenuItem("-")
        Dim menuItem8Find As New MenuItem("Find")
        Dim menuItem13Divider As New MenuItem("-")
        Dim menuItem11Sort As New MenuItem("Sort on Severity")
        Dim menuItem12Sort As New MenuItem("Sort on FileName")
        Dim menuItem18Divider As New MenuItem("-")
        Dim menuItem19FilterResults As New MenuItem("Filter Results...")
        Dim menuItem20ExportFiltered As New MenuItem("Export Filtered XML Results...")

        ' ListBox
        Dim menuItem14OpenFile As New MenuItem("Open Code in Associated Editor")
        Dim menuItem15OpenAtLine As New MenuItem("Open Code at This Line in Notepad++")
        Dim menuItem16Divider As New MenuItem("-")
        Dim menuItem17Order As New MenuItem("Order on Multiple Columns...")
        Dim menuItem21Divider As New MenuItem("-")
        Dim menuItem22FilterResults As New MenuItem("Filter Results...")
        Dim menuItem23ExportFiltered As New MenuItem("Export Filtered XML Results...")
        Dim menuItem24Divider As New MenuItem("-")
        Dim menuItem25SelectColour As New MenuItem("Select Colour When Checked...")
        Dim menuItem28ChangeSeverity As New MenuItem("Change Severity...")
        Dim menuItem26Divider As New MenuItem("-")
        Dim menuItem27DeleteItem As New MenuItem("Delete Selected Item(s)")

        '== Full context menu for combo box ==
        AddHandler menuItem1Cut.Click, AddressOf CutToolStripMenuItem_Click
        AddHandler menuItem2Copy.Click, AddressOf CopyToolStripMenuItem_Click
        AddHandler menuItem3Paste.Click, AddressOf PasteToolStripMenuItem_Click
        AddHandler menuItem5SelectAll.Click, AddressOf SelectAllToolStripMenuItem_Click

        cmFullContextMenu.MenuItems.Add(menuItem1Cut)
        cmFullContextMenu.MenuItems.Add(menuItem2Copy)
        cmFullContextMenu.MenuItems.Add(menuItem3Paste)
        cmFullContextMenu.MenuItems.Add(menuItem4Divider)
        cmFullContextMenu.MenuItems.Add(menuItem5SelectAll)


        '== Specialised menu for results ==
        AddHandler menuItem6Copy.Click, AddressOf CopyToolStripMenuItem_Click
        AddHandler menuItem10SelectAll.Click, AddressOf SelectAllToolStripMenuItem_Click
        AddHandler menuItem8Find.Click, AddressOf FindToolStripMenuItem_Click
        AddHandler menuItem11Sort.Click, AddressOf SortRichTextResultsOnSeverityToolStripMenuItem_Click
        AddHandler menuItem12Sort.Click, AddressOf SortRichTextResultsOnFileNameToolStripMenuItem_Click
        AddHandler menuItem19FilterResults.Click, AddressOf FilterResultsToolStripMenuItem_Click
        AddHandler menuItem20ExportFiltered.Click, AddressOf ExportFilteredResultsXML

        cmResultsContextMenu.MenuItems.Add(menuItem6Copy)
        cmResultsContextMenu.MenuItems.Add(menuItem9Divider)
        cmResultsContextMenu.MenuItems.Add(menuItem10SelectAll)
        cmResultsContextMenu.MenuItems.Add(menuItem7Divider)
        cmResultsContextMenu.MenuItems.Add(menuItem8Find)
        cmResultsContextMenu.MenuItems.Add(menuItem13Divider)
        cmResultsContextMenu.MenuItems.Add(menuItem11Sort)
        cmResultsContextMenu.MenuItems.Add(menuItem12Sort)
        cmResultsContextMenu.MenuItems.Add(menuItem18Divider)
        cmResultsContextMenu.MenuItems.Add(menuItem19FilterResults)
        cmResultsContextMenu.MenuItems.Add(menuItem20ExportFiltered)


        '== File menu for results table ==
        AddHandler menuItem14OpenFile.Click, AddressOf OpenFileInEditor
        AddHandler menuItem15OpenAtLine.Click, AddressOf OpenAtCodeBlock
        AddHandler menuItem17Order.Click, AddressOf OrderOnMultColumns
        AddHandler menuItem22FilterResults.Click, AddressOf FilterResultsToolStripMenuItem_Click
        AddHandler menuItem23ExportFiltered.Click, AddressOf ExportFilteredResultsXML
        AddHandler menuItem25SelectColour.Click, AddressOf SelectCheckColour
        AddHandler menuItem28ChangeSeverity.Click, AddressOf SetSeverity
        AddHandler menuItem27DeleteItem.Click, AddressOf DeleteScanResult

        cmResultsListContextMenu.MenuItems.Add(menuItem14OpenFile)
        cmResultsListContextMenu.MenuItems.Add(menuItem15OpenAtLine)
        cmResultsListContextMenu.MenuItems.Add(menuItem16Divider)
        cmResultsListContextMenu.MenuItems.Add(menuItem17Order)
        cmResultsListContextMenu.MenuItems.Add(menuItem21Divider)
        cmResultsListContextMenu.MenuItems.Add(menuItem22FilterResults)
        cmResultsListContextMenu.MenuItems.Add(menuItem23ExportFiltered)
        cmResultsListContextMenu.MenuItems.Add(menuItem24Divider)
        cmResultsListContextMenu.MenuItems.Add(menuItem25SelectColour)
        cmResultsListContextMenu.MenuItems.Add(menuItem28ChangeSeverity)
        cmResultsListContextMenu.MenuItems.Add(menuItem26Divider)
        cmResultsListContextMenu.MenuItems.Add(menuItem27DeleteItem)

        '== Assign menus to controls ==
        cboTargetDir.ContextMenu = cmFullContextMenu
        rtbResults.ContextMenu = cmResultsContextMenu
        lvResults.ContextMenu = cmResultsListContextMenu

    End Sub

    Private Sub SelectCheckColour()
        ' Allow user to modify the colour for checked listbox items
        '==========================================================

        If cdColorDialog.ShowDialog() <> Windows.Forms.DialogResult.Cancel Then
            asAppSettings.ListItemColour = cdColorDialog.Color
        End If

    End Sub

    Private Sub DeleteScanResult()
        ' Delete a ScanResult from the results collection
        '================================================
        Dim intIndex As Integer = 0
        Dim intCount As Integer = 0

        Dim strKey As String = ""
        Dim strTitle As String = ""
        Dim strFileName As String = ""

        Dim arrDeleteItems As New ArrayList


        ' Get number of items to be deleted, exit if nothing to delete
        If lvResults.SelectedItems.Count < 1 Then Exit Sub

        intCount = lvResults.SelectedItems.Count

        ' Iterate through results and find selected items
        For Each objResult In rtResultsTracker.ScanResults

            strKey = objResult.ItemKey.ToString
            strTitle = objResult.Title
            strFileName = objResult.FileName

            If lvResults.Items.ContainsKey(strKey) Then
                If lvResults.Items(strKey).Selected = True Then
                    ' Get index of selected object for deletion
                    ' Result objects must be deleted afterwards as 
                    ' we would get an error if we deleted them during the iteration
                    arrDeleteItems.Add(rtResultsTracker.ScanResults.IndexOf(objResult))

                    ' Remove from groups
                    ' If groups are empty they should be deleted
                    If rtResultsTracker.FileGroups.ContainsKey(strFileName) Then
                        rtResultsTracker.FileGroups(strFileName).DeleteDetail(strKey)
                        If rtResultsTracker.FileGroups(strFileName).GetItemCount = 0 Then rtResultsTracker.FileGroups.Remove(strFileName)
                    End If

                    If rtResultsTracker.IssueGroups.ContainsKey(strTitle) Then
                        rtResultsTracker.IssueGroups(strTitle).DeleteDetail(strKey)
                        If rtResultsTracker.IssueGroups(strTitle).GetItemCount = 0 Then rtResultsTracker.IssueGroups.Remove(strTitle)
                    End If


                    ' Remove from listview
                    lvResults.Items(strKey).Remove()

                    ' Exit sub if all objects are deleted
                    intIndex += 1
                    If intIndex >= intCount Then Exit For
                End If
            End If
        Next


        ' Delete selected objects
        ' We must do this backwards (highest index first) as the 
        ' indexes will change for all items later in the list 
        ' following the deletion
        If arrDeleteItems.Count < 1 Then Exit Sub

        arrDeleteItems.Sort()
        For intIndex = arrDeleteItems.Count - 1 To 0 Step -1
            rtResultsTracker.ScanResults.RemoveAt(arrDeleteItems.Item(intIndex))
        Next intIndex


        ' Update Rich Text results
        SetRTBView(asAppSettings.RTBGrouping)

    End Sub

    Private Sub CutToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CutToolStripMenuItem.Click
        ' Handle each control as appropriate
        ' Has to be done in a slightly awkward way as ActiveControl returns the container but we 
        ' need the control which has focus (and we don't always want same action for controls)
        '=======================================================================================

        If cboTargetDir.Focused Then
            If cboTargetDir.SelectedText <> "" Then
                Clipboard.SetText(cboTargetDir.SelectedText)
                cboTargetDir.SelectedText = ""
            End If
        End If

    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        ' Handle each control as appropriate
        ' Has to be done in a slightly awkward way as ActiveControl returns the container but we 
        ' need the control which has focus (and we don't always want same action for controls)
        '=======================================================================================

        If rtbResults.Focused Then
            If rtbResults.SelectedText <> "" Then Clipboard.SetText(rtbResults.SelectedText)
        ElseIf cboTargetDir.Focused Then
            If cboTargetDir.SelectedText <> "" Then Clipboard.SetText(cboTargetDir.SelectedText)
        End If

    End Sub

    Private Sub PasteToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PasteToolStripMenuItem.Click
        ' Handle each control as appropriate
        ' Has to be done in a slightly awkward way as ActiveControl returns the container but we 
        ' need the control which has focus (and we don't always want same action for controls)
        '=======================================================================================

        If cboTargetDir.Focused Then
            If cboTargetDir.SelectedText <> "" Then Clipboard.SetText(cboTargetDir.SelectedText)
        End If

    End Sub

    Private Sub FindToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FindToolStripMenuItem.Click
        ' Handle each control as appropriate
        ' Has to be done in a slightly awkward way as ActiveControl returns the container but we 
        ' need the control which has focus (and we don't always want same action for controls)
        '=======================================================================================

        If rtbResults.Focused Then FindText()

    End Sub

    Private Sub SelectAllToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click
        ' Handle each control as appropriate
        ' Has to be done in a slightly awkward way as ActiveControl returns the container but we 
        ' need the control which has focus (and we don't always want same action for controls)
        '=======================================================================================

        If rtbResults.Focused Then
            rtbResults.SelectAll()
        ElseIf cboTargetDir.Focused Then
            cboTargetDir.SelectAll()
        End If

    End Sub

    Public Sub FindText()
        ' Locate the selected text in the Results box
        '============================================
        Dim strSearch As String

        strSearch = InputBox("Enter Text to Search For:", "Find", strPrevSearch)
        rtbResults.Find(strSearch)
        strPrevSearch = strSearch

    End Sub

    Private Sub ToolStripMenuItem9_Click(sender As System.Object, e As System.EventArgs) Handles ClearResultsMenuItem.Click
        ' Clear all results and target directory
        '=======================================
        ClearResults()
    End Sub

    Public Sub ClearResults()
        ' Clear all results and target directory
        '=======================================

        ' Clear variables
        rtResultsTracker.TargetDirectory = ""
        rtResultsTracker.Reset()
        rtResultsTracker.ResetFileListVars()

        'Clear UI
        lbTargets.Items.Clear()
        lvResults.Items.Clear()
        rtbResults.Clear()

        ' Disable menus
        SetScanMenus(False)

        ' == Don't wipe out their existing options
        ' ====
        ' cboTargetDir.Text = ""
        ' SelectLanguage(asAppSettings.TestType)

    End Sub

    Public Sub SetScanMenus(NewSetting As Boolean)
        ' Enable or disable scanning menus when target is selected/deselected
        '====================================================================

        If asAppSettings.IsConsole = True Then Exit Sub

        StartScanningToolStripMenuItem.Enabled = NewSetting
        VisualCodeBreakdownToolStripMenuItem.Enabled = NewSetting
        VisualSecurityBreakdownToolStripMenuItem.Enabled = NewSetting
        VisualBadFuncBreakdownToolStripMenuItem.Enabled = NewSetting
        VisualCommentBreakdownToolStripMenuItem.Enabled = NewSetting
        ExportFixMeCommentsToolStripMenuItem.Enabled = NewSetting

        If tcMain.SelectedTab IsNot tabResultsTable Then
            DeleteItemToolStripMenuItem.Enabled = False
        Else
            DeleteItemToolStripMenuItem.Enabled = True
        End If

    End Sub

    Public Sub SetDeleteMenu()
        ' Enable delete menu if a scan result is selected in the table
        '=============================================================

        If (tcMain.SelectedTab Is tabResultsTable Or tabResultsTable.Focused = True) And lvResults.SelectedItems.Count > 0 Then
            DeleteItemToolStripMenuItem.Enabled = True
        Else
            DeleteItemToolStripMenuItem.Enabled = False
        End If

    End Sub

    Private Sub OpenAtCodeBlock()
        ' Open the file listed in the summary table at the given line number
        '===================================================================
        Dim strFileName As String
        Dim intLineNum As Integer = 0
        Dim intResult As Integer = vbOK

        If lvResults.SelectedItems.Count = 0 Then Exit Sub

        ' Get the filename and line number from the table
        strFileName = lvResults.SelectedItems.Item(0).SubItems(FILE_COL).Text
        intLineNum = lvResults.SelectedItems.Item(0).SubItems(LINE_COL).Text

        ' If we have a file, try to open it in its associated application
        If strFileName <> "" Then
            If intLineNum > 0 Then
                LaunchNPP(strFileName, intLineNum)
            Else
                intResult = MsgBox("This type of issue does not have an associated code block and line number. Open file at line 1?", vbOKCancel, "No Line Number Available")
                If intResult = vbOK Then LaunchNPP(strFileName)
            End If
        End If

    End Sub

    Private Sub lvResults_Click(sender As Object, e As System.EventArgs) Handles lvResults.Click
        ' If an item is selected then enable the 'delete' menu item
        '==========================================================

        If lvResults.SelectedItems.Count > 0 Then
            DeleteItemToolStripMenuItem.Enabled = True
        Else
            DeleteItemToolStripMenuItem.Enabled = False
        End If

    End Sub

    Private Sub lvResults_DoubleClick(sender As Object, e As System.EventArgs) Handles lvResults.DoubleClick
        ' If an item is selected then enable the 'delete' menu item
        ' Open any file that has been double-clicked in the appropriate editor
        '=====================================================================

        If lvResults.SelectedItems.Count > 0 Then
            DeleteItemToolStripMenuItem.Enabled = True
        Else
            DeleteItemToolStripMenuItem.Enabled = False
        End If

        OpenFileInEditor()

    End Sub

    Private Sub OpenFileInEditor()
        ' Open the associated file listed in the summary table
        '=====================================================
        Dim strFileName As String
        Dim psiStart As New ProcessStartInfo()


        If lvResults.SelectedItems.Count = 0 Then Exit Sub

        ' Get the filename from the table
        strFileName = lvResults.SelectedItems.Item(0).SubItems(FILE_COL).Text

        ' If we have a file, try to open it in its associated application
        If strFileName <> "" Then
            Try
                With psiStart
                    .FileName = strFileName
                    .UseShellExecute = True
                End With

                Process.Start(psiStart)
            Catch
                DisplayError("Error reading file: " & strFileName, vbExclamation, "File Error")
            End Try
        End If

    End Sub

    Private Sub lvResults_ColumnClick(sender As Object, e As System.Windows.Forms.ColumnClickEventArgs) Handles lvResults.ColumnClick
        ' Sort the items according to which column was clicked
        '=====================================================
        Dim lviItem As ListViewItem
        Dim colOriginalColour As Color

        lvResults.Items.Clear()
        SortResults(e.Column)

        For Each itmItem In rtResultsTracker.ScanResults
            If Not blnIsFiltered Or (blnIsFiltered And itmItem.Severity >= intFilterMax And itmItem.Severity <= intFilterMin) Then
                lviItem = New ListViewItem

                colOriginalColour = asAppSettings.ListItemColour

                '== Add to listview ==
                lviItem.Name = itmItem.ItemKey
                lviItem.Text = itmItem.Severity
                lviItem.SubItems.Add(itmItem.SeverityDesc)
                lviItem.SubItems.Add(itmItem.Title)
                lviItem.SubItems.Add(itmItem.Description)
                lviItem.SubItems.Add(itmItem.FileName)
                lviItem.SubItems.Add(itmItem.LineNumber)

                If itmItem.IsChecked Then
                    asAppSettings.ListItemColour = itmItem.CheckColour
                    lviItem.Checked = itmItem.IsChecked
                End If

                lvResults.Items.Add(lviItem)

            End If
        Next

    End Sub

    Private Sub ToolStripMenuItem10_Click(sender As System.Object, e As System.EventArgs) Handles ExportResultsXMLMenuItem.Click
        ExportResultsXML()
    End Sub

    Private Sub ToolStripMenuItem11_Click(sender As System.Object, e As System.EventArgs) Handles ImportXmlResultsMenuItem.Click
        ImportResultsXML()
    End Sub

    Private Sub ToolStripMenuItem14_Click(sender As System.Object, e As System.EventArgs) Handles ExportCsvResultsMenuItem.Click
        ExportResultsCSV()
    End Sub

    Private Sub ToolStripMenuItem15_Click(sender As System.Object, e As System.EventArgs) Handles ImportCsvResultsMenuItem.Click
        ImportResultsCSV()
    End Sub

    Private Sub SortRichTextResultsOnSeverityToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SortRichTextResultsOnSeverityToolStripMenuItem.Click
        SortRTBResults()
    End Sub

    Private Sub SortRichTextResultsOnFileNameToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SortRichTextResultsOnFileNameToolStripMenuItem.Click
        SortRTBResults(FILE_COL)
    End Sub

    Private Sub StatusBarToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles StatusBarToolStripMenuItem.Click
        ' Show/hide status bar
        '=====================

        ssStatusStrip.Visible = StatusBarToolStripMenuItem.Checked

    End Sub

    Private Sub GroupRichTextResultsByIssueToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles GroupRichTextResultsByIssueToolStripMenuItem.Click
        ' Set grouping style for RTB results and uncheck other menu items
        '================================================================

        If GroupRichTextResultsByIssueToolStripMenuItem.Checked Then SetRTBView(AppSettings.ISSUEGROUP)

    End Sub

    Private Sub GroupRichTextResultsByFileToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles GroupRichTextResultsByFileToolStripMenuItem.Click
        ' Set grouping style for RTB results and uncheck other menu items
        '================================================================

        If GroupRichTextResultsByFileToolStripMenuItem.Checked Then SetRTBView(AppSettings.FILEGROUP)

    End Sub

    Private Sub ShowIndividualRichTextResultsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ShowIndividualRichTextResultsToolStripMenuItem.Click
        ' Show individual results
        '========================

        If ShowIndividualRichTextResultsToolStripMenuItem.Checked Then SetRTBView(AppSettings.INDIVIDUAL)

    End Sub

    Private Sub SetRTBView(ViewOption As Integer)
        ' Set the RTB view option for grouping results
        '=============================================

        asAppSettings.RTBGrouping = ViewOption

        Select Case ViewOption
            Case AppSettings.ISSUEGROUP
                ' Show all occurences for each particular issue 
                GroupRichTextResultsByFileToolStripMenuItem.Checked = False
                ShowIndividualRichTextResultsToolStripMenuItem.Checked = False
                GroupRTBByIssue()
            Case AppSettings.FILEGROUP
                ' Show the different issues for each file 
                GroupRichTextResultsByIssueToolStripMenuItem.Checked = False
                ShowIndividualRichTextResultsToolStripMenuItem.Checked = False
                GroupRTBByFile()
            Case AppSettings.INDIVIDUAL
                ' Default view - each issue shown separately
                ' We are using an arbitrary value as PRIORITY_COL or FILE_COL will result in sorting taking place
                GroupRichTextResultsByFileToolStripMenuItem.Checked = False
                GroupRichTextResultsByIssueToolStripMenuItem.Checked = False
                SortRTBResults(-1)
        End Select

    End Sub

    Private Sub ExportFilteredResultsXML(sender As System.Object, e As System.EventArgs)
        ExportResultsXML(intFilterMin, intFilterMax)
    End Sub

    Public Sub ExportResultsXML(Optional FilterMinimum As Integer = CodeIssue.POSSIBLY_SAFE, Optional FilterMaximum As Integer = CodeIssue.CRITICAL, Optional ExportFile As String = "")
        ' Export all errors to XML file
        '==============================
        Dim xwsSettings As New XmlWriterSettings()
        Dim xwWriter As XmlWriter
        Dim ccConverter As New ColorConverter

        Dim strResultsFile As String = ""
        Dim strLanguage As String = ""


        '== Exit if no results ==
        If rtResultsTracker.ScanResults.Count = 0 Then
            If asAppSettings.IsConsole Then LogInfo("No results to write. Exiting application.")
            Exit Sub
        End If


        If SaveFiltered = True Then
            FilterMinimum = intFilterMin
            FilterMaximum = intFilterMax
        End If

        '== Get language for results in text form in order to put meaningful comment at start of file ==
        Select Case asAppSettings.TestType
            Case AppSettings.C
                strLanguage = "C"
            Case AppSettings.JAVA
                strLanguage = "Java"
            Case AppSettings.SQL
                strLanguage = "PL/SQL"
            Case AppSettings.CSHARP
                strLanguage = "C#"
            Case AppSettings.PHP
                strLanguage = "PHP"
        End Select

        If ExportFile = "" Then
            '== Get filename from user ==
            With sfdSaveFileDialog
                .Filter = "XML files (*.xml)|*.xml|Text files (*.txt)|*.txt|All files (*.*)|*.*"

                If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                    strResultsFile = .FileName
                End If

                If strResultsFile.Trim = "" Then
                    Exit Sub
                Else
                    ExportFile = strResultsFile
                End If

            End With
        End If


        '== Create file and write output ==
        Try
            xwWriter = XmlWriter.Create(ExportFile, xwsSettings)


            ' Use indention for the xml output
            xwsSettings.Indent = True

            With xwWriter

                ' Begin document with Xml declaration
                .WriteStartDocument()

                ' Write a comment.
                .WriteComment("XML Export of VCG Results for directory: " & rtResultsTracker.TargetDirectory & ". Scanned for " & strLanguage & " security issues.")

                ' Write the root element.
                .WriteStartElement("CodeIssueCollection")

                '== Display progress to screen as appropriate ==
                ShowLoading("Exporting Results", "Exporting results to XML...", rtResultsTracker.ScanResults.Count)
                LogVerbose("Exporting results to XML. Number of records: " & CStr(rtResultsTracker.ScanResults.Count))

                ' Loop through issues and write data for each one
                For Each itmItem In rtResultsTracker.ScanResults
                    If (itmItem.Severity <= FilterMinimum And itmItem.Severity >= FilterMaximum) Then


                        .WriteStartElement("CodeIssue")

                        .WriteStartElement("Priority")
                        .WriteString(itmItem.Severity)
                        .WriteEndElement()

                        .WriteStartElement("Severity")
                        .WriteString(itmItem.SeverityDesc)
                        .WriteEndElement()

                        .WriteStartElement("Title")
                        .WriteString(itmItem.Title)
                        .WriteEndElement()

                        .WriteStartElement("Description")
                        .WriteString(itmItem.Description)
                        .WriteEndElement()

                        .WriteStartElement("FileName")
                        .WriteString(itmItem.FileName)
                        .WriteEndElement()

                        .WriteStartElement("Line")
                        .WriteString(itmItem.LineNumber)
                        .WriteEndElement()

                        .WriteStartElement("CodeLine")
                        .WriteString(itmItem.CodeLine)
                        .WriteEndElement()

                        If SaveCheckState Then
                            .WriteStartElement("Checked")
                            .WriteString(itmItem.IsChecked)
                            .WriteEndElement()
                            .WriteStartElement("CheckColour")
                            .WriteString(ccConverter.ConvertToString(itmItem.CheckColour))
                            .WriteEndElement()
                        End If

                        ' End of this issue
                        .WriteEndElement()

                    End If

                    If asAppSettings.IsConsole = False Then
                        IncrementLoadingBar("Formatting XML...")
                    End If

                Next

                frmLoading.Close()

                ' Close XmlTextWriter
                .WriteEndElement()
                .WriteEndDocument()
                .Close()

            End With

            xwWriter.Close()

            If asAppSettings.IsConsole = False Then frmLoading.Close()
            DisplayError("XML output successfully exported.", "Export Finished")

        Catch exError As Exception
            If asAppSettings.IsConsole = False Then frmLoading.Close()
            DisplayError(exError.Message)
        End Try

    End Sub

    Public Sub ImportResultsXML(Optional FileName As String = "")
        ' Import previous scan results from XML file
        '===========================================
        Dim strResultsFile As String = ""
        Dim strDescription As String = ""

        Dim xrReader As XmlTextReader
        Dim xntNodeType As XmlNodeType
        Dim srResultItem As New ScanResult
        Dim ccConverter As New ColorConverter

        Dim arrInts As String()
        Dim strColour As String
        Dim intR, intG, intB As Integer     ' For colour represented as RGB components


        '== Get user permission if app has a set of current results in place ==
        If rtResultsTracker.ScanResults.Count > 0 And asAppSettings.IsConsole = False Then
            If MsgBox("Overwrite current results with file contents?", MsgBoxStyle.YesNo, "Overwrite Results?") = MsgBoxResult.No Then Exit Sub
        End If

        If FileName = "" Then
            '== Get filename from user ==
            With ofdOpenFileDialog
                .Filter = "XML files (*.xml)|*.xml|Text files (*.txt)|*.txt|All files (*.*)|*.*"

                If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                    strResultsFile = .FileName
                    If Not (IO.File.Exists(strResultsFile)) Then Exit Sub
                End If

                If strResultsFile.Trim = "" Then Exit Sub
            End With
        Else
            strResultsFile = FileName
        End If

        '== Remove any previous results ==
        ClearResults()

        '== Load results from file ==
        Try

            xrReader = New XmlTextReader(strResultsFile)

            Dim fiFileLogInfo As New FileInfo(strResultsFile)

            '== Display progress to screen as appropriate 
            ShowLoading("Importing Results", "Importing results from XML file...", fiFileLogInfo.Length)
            LogVerbose("Importing results from XML file. File size: {0}", fiFileLogInfo.Length)

            '== Loop through each XML element
            While (xrReader.Read())

                xntNodeType = xrReader.NodeType

                'if node type was element
                If (xntNodeType = XmlNodeType.Element) Then

                    With xrReader
                        '== Populate data from each tag ==
                        If (.Name = "Priority") Then srResultItem.SetSeverity(CInt(.ReadInnerXml.ToString()))
                        If (.Name = "Title") Then srResultItem.Title = .ReadInnerXml.ToString()
                        If (.Name = "Description") Then srResultItem.Description = .ReadInnerXml.ToString()
                        If (.Name = "FileName") Then srResultItem.FileName = .ReadInnerXml.ToString()
                        If (.Name = "Line") Then srResultItem.LineNumber = .ReadInnerXml.ToString()
                        If (.Name = "CodeLine") Then srResultItem.CodeLine = .ReadInnerXml.ToString()
                        If (.Name = "Checked") Then srResultItem.IsChecked = Convert.ToBoolean(.ReadInnerXml.ToString())

                        If (.Name = "CheckColour") Then
                            strColour = .ReadInnerXml.ToString()
                            If strColour.Contains(",") Then
                                arrInts = strColour.Split(",")
                                intR = CInt(arrInts(0))
                                intG = CInt(arrInts(1))
                                intB = CInt(arrInts(2))
                                srResultItem.CheckColour = Color.FromArgb(intR, intG, intB)
                            Else
                                srResultItem.CheckColour = Color.FromName(strColour)
                            End If
                        End If

                        If .NodeType = XmlNodeType.EndElement And .Name = "CodeIssue" Then
                            '== Place result in collection and write output to screen ==
                            AddToResultCollection(srResultItem.Title, srResultItem.Description, srResultItem.FileName, srResultItem.Severity, srResultItem.LineNumber, srResultItem.CodeLine, srResultItem.IsChecked, ccConverter.ConvertToString(srResultItem.CheckColour))
                            strDescription = srResultItem.Description
                            WriteResult(srResultItem.Title & vbNewLine, strDescription & vbNewLine & "Line: " & srResultItem.LineNumber & " - Filename: " & srResultItem.FileName & vbNewLine, srResultItem.CodeLine, srResultItem.Severity)
                        End If
                    End With

                End If

                If asAppSettings.IsConsole = False Then IncrementLoadingBar("Reading XML element: " & srResultItem.Description)

            End While

            xrReader.Close()
            frmLoading.Close()

        Catch exError As Exception
            If asAppSettings.IsConsole = False Then frmLoading.Close()
            DisplayError(exError.Message)
        End Try

    End Sub

    Public Sub ExportMetaDataXML(Optional ExportFile As String = "")
        ' Export details of LoC, comments, whitespace, etc. to XML file
        '==============================================================
        Dim xwsSettings As New XmlWriterSettings()
        Dim xwWriter As XmlWriter

        Dim strResultsFile As String = ""
        Dim strLanguage As String = ""


        '== Exit if no data ==
        If rtResultsTracker.FileCount = 0 Then
            If asAppSettings.IsConsole Then LogInfo("No results to write. Exiting application.")
            Exit Sub
        End If

        '== Get language for results in text form in order to put meaningful comment at start of file ==
        Select Case asAppSettings.TestType
            Case AppSettings.C
                strLanguage = "C"
            Case AppSettings.JAVA
                strLanguage = "Java"
            Case AppSettings.SQL
                strLanguage = "PL/SQL"
            Case AppSettings.CSHARP
                strLanguage = "C#"
            Case AppSettings.PHP
                strLanguage = "PHP"
            Case AppSettings.COBOL
                strLanguage = "COBOL"
        End Select

        If ExportFile = "" Then
            '== Get filename from user ==
            With sfdSaveFileDialog
                .Filter = "XML files (*.xml)|*.xml|Text files (*.txt)|*.txt|All files (*.*)|*.*"

                If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                    strResultsFile = .FileName
                End If

                If strResultsFile.Trim = "" Then
                    Exit Sub
                Else
                    ExportFile = strResultsFile
                End If

            End With
        End If


        '== Create file and write output ==
        Try
            xwWriter = XmlWriter.Create(ExportFile, xwsSettings)

            ' Use indention for the xml output
            xwsSettings.Indent = True

            With xwWriter
                ' Begin document with Xml declaration
                .WriteStartDocument()

                ' Write a comment.
                .WriteComment("XML Export of Code Metadata for directory: " & rtResultsTracker.TargetDirectory & ". Scanned for " & strLanguage & " security issues.")

                ' Write the root element.
                .WriteStartElement("VCGCodeMetadata")

                '== Display progress to screen as appropriate ==
                ShowLoading("Exporting Results", "Exporting results to XML...", rtResultsTracker.FileCount)
                LogVerbose("Exporting results to XML. Number of files: " & CStr(rtResultsTracker.FileCount))

                ' Write metadata to file
                .WriteStartElement("TotalLines")
                .WriteString(rtResultsTracker.OverallLineCount)
                .WriteEndElement()

                .WriteStartElement("LinesOfCode")
                .WriteString(rtResultsTracker.OverallCodeCount)
                .WriteEndElement()

                .WriteStartElement("Whitespace")
                .WriteString(rtResultsTracker.OverallWhitespaceCount)
                .WriteEndElement()

                .WriteStartElement("BannedAPIs")
                .WriteString(rtResultsTracker.OverallBadFuncCount)
                .WriteEndElement()

                .WriteStartElement("Comments")
                .WriteString(rtResultsTracker.OverallCommentCount)
                .WriteEndElement()

                .WriteStartElement("FixMeComments")
                .WriteString(rtResultsTracker.OverallFixMeCount)
                .WriteEndElement()

                If asAppSettings.IsConsole = False Then
                    IncrementLoadingBar("Writing XML...")
                End If

                frmLoading.Close()

                ' Close XmlTextWriter
                .WriteEndElement()
                .WriteEndDocument()
                .Close()

            End With

            xwWriter.Close()

            If asAppSettings.IsConsole = False Then frmLoading.Close()
            DisplayError("XML output successfully exported.", "Export Finished")

        Catch exError As Exception
            If asAppSettings.IsConsole = False Then frmLoading.Close()
            DisplayError(exError.Message)
        End Try

    End Sub

    Public Sub ExportResultsCSV(Optional FilterMinimum As Integer = CodeIssue.POSSIBLY_SAFE, Optional FilterMaximum As Integer = CodeIssue.CRITICAL, Optional ExportFile As String = "")
        ' Export all errors to XML file
        '==============================
        Dim swResultFile As StreamWriter
        Dim ccConverter As New ColorConverter

        Dim strResultsFile As String = ""
        Dim strLanguage As String = ""
        Dim strDescription As String = ""
        Dim strCodeLine As String = ""



        '== Exit if no results ==
        If rtResultsTracker.ScanResults.Count = 0 Then
            If asAppSettings.IsConsole Then LogInfo("No results to write. Exiting application.")
            Exit Sub
        End If


        If SaveFiltered = True Then
            FilterMinimum = intFilterMin
            FilterMaximum = intFilterMax
        End If


        If ExportFile = "" Then
            '== Get filename from user ==
            With sfdSaveFileDialog
                .Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"

                If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                    strResultsFile = .FileName
                End If

                If strResultsFile.Trim = "" Then Exit Sub
            End With
        Else
            strResultsFile = ExportFile
        End If


        Try
            '== Open file ==
            swResultFile = New StreamWriter(strResultsFile)


            With swResultFile

                '== Display progress to screen as appropriate ==
                ShowLoading("Exporting Results", "Exporting results to CSV file...", rtResultsTracker.ScanResults.Count)
                LogVerbose("Exporting results to CSV. Number of records: " & CStr(rtResultsTracker.ScanResults.Count))

                .WriteLine("Severity,Description,Title,FileName,LineNumber,Line")

                ' Loop through issues and write data for each one
                For Each itmItem In rtResultsTracker.ScanResults
                    If (itmItem.Severity <= FilterMinimum And itmItem.Severity >= FilterMaximum) Then

                        ' Sanitise free-form text to prevent quotes from breaking things
                        strDescription = itmItem.Description.Replace("""", """""")
                        strCodeLine = itmItem.CodeLine.Replace("""", """""")
                        strDescription = itmItem.Description.Replace(vbNewLine, "")


                        If SaveCheckState Then
                            .WriteLine(itmItem.Severity & "," & itmItem.SeverityDesc & ",""" & itmItem.Title & """,""" & strDescription & """," & itmItem.FileName & "," & itmItem.LineNumber & ",""" & strCodeLine & """," & itmItem.IsChecked & ",""" & ccConverter.ConvertToString(itmItem.CheckColour) & """")
                        Else
                            .WriteLine(itmItem.Severity & "," & itmItem.SeverityDesc & ",""" & itmItem.Title & """,""" & strDescription & """," & itmItem.FileName & "," & itmItem.LineNumber & ",""" & strCodeLine & """")
                        End If

                    End If

                    If asAppSettings.IsConsole = False Then
                        IncrementLoadingBar("Writing CSV File...")
                    End If

                Next
            End With

            swResultFile.Close()

            If asAppSettings.IsConsole = False Then frmLoading.Close()
            DisplayError("CSV output successfully exported.", "Success", vbOKOnly)

        Catch exError As Exception
            If asAppSettings.IsConsole = False Then frmLoading.Close()
            DisplayError(exError.Message)
        End Try

    End Sub

    Public Sub ImportResultsCSV(Optional FileName As String = "")
        ' Import previous scan results from XML file
        '===========================================
        Dim strResultsFile As String = ""
        Dim strDescription As String = ""

        Dim arrItems As String()
        Dim arrInts As String()

        Dim srResultItem As New ScanResult
        Dim ccConverter As New ColorConverter

        Dim intR, intG, intB As Integer     ' For colour represented as RGB components


        '== Get user permission if app has a set of current results in place ==
        If rtResultsTracker.ScanResults.Count > 0 And asAppSettings.IsConsole = False Then
            If MsgBox("Overwrite current results with file contents?", MsgBoxStyle.YesNo, "Overwrite Results?") = MsgBoxResult.No Then Exit Sub
        End If

        If FileName = "" Then
            '== Get filename from user ==
            With ofdOpenFileDialog
                .Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"

                If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                    strResultsFile = .FileName
                    If Not (IO.File.Exists(strResultsFile)) Then Exit Sub
                End If

                If strResultsFile.Trim = "" Then Exit Sub
            End With
        Else
            strResultsFile = FileName
        End If

        '== Remove any previous results ==
        ClearResults()

        '== Load results from file ==
        Try
            Dim fiFileLogInfo As New FileInfo(strResultsFile)
            Using tfpParser As New Microsoft.VisualBasic.FileIO.TextFieldParser(strResultsFile)

                '== Display progress to screen as appropriate ==
                ShowLoading("Importing Results", "Importing results from CSV file...", fiFileLogInfo.Length)
                LogVerbose("Importing results from CSV file. File size: " & fiFileLogInfo.Length)

                tfpParser.TextFieldType = FileIO.FieldType.Delimited
                tfpParser.SetDelimiters(",")


                Dim firstLineSkipped = False
                While Not tfpParser.EndOfData
                    Try
                        arrItems = tfpParser.ReadFields()

                        ' First line contains headers, so skip and continue
                        If firstLineSkipped = False Then
                            firstLineSkipped = True
                            Continue While
                        End If

                        If arrItems.Length = 9 Or arrItems.Length = 7 Then

                            '== Populate data from each tag ==
                            srResultItem.SetSeverity(CInt(arrItems(0)))
                            srResultItem.Title = arrItems(2)
                            srResultItem.Description = arrItems(3)
                            srResultItem.FileName = arrItems(4)
                            srResultItem.LineNumber = arrItems(5)
                            srResultItem.CodeLine = arrItems(6)

                            If arrItems.Length = 9 Then
                                srResultItem.IsChecked = Convert.ToBoolean(arrItems(7))
                                If arrItems(8).Contains(",") Then
                                    arrInts = arrItems(8).Split(",")
                                    intR = CInt(arrInts(0))
                                    intG = CInt(arrInts(1))
                                    intB = CInt(arrInts(2))
                                    srResultItem.CheckColour = Color.FromArgb(intR, intG, intB)
                                Else
                                    srResultItem.CheckColour = Color.FromName(arrItems(8))
                                End If
                            End If

                            '== Place result in collection and write output to screen ==
                            AddToResultCollection(srResultItem.Title, srResultItem.Description, srResultItem.FileName, srResultItem.Severity, srResultItem.LineNumber, srResultItem.CodeLine, srResultItem.IsChecked, ccConverter.ConvertToString(srResultItem.CheckColour))
                            strDescription = srResultItem.Description
                            WriteResult(srResultItem.Title & vbNewLine, strDescription & vbNewLine & "Line: " & srResultItem.LineNumber & " - Filename: " & srResultItem.FileName & vbNewLine, srResultItem.CodeLine, srResultItem.Severity)
                        End If

                        If asAppSettings.IsConsole = False Then IncrementLoadingBar("Reading CSV element: " & srResultItem.Description)

                    Catch exError As Microsoft.VisualBasic.FileIO.MalformedLineException
                        LogError("Line " & exError.Message & " is not valid and will be skipped.")
                    End Try
                End While
            End Using


            '== Notify user of success ==
            If asAppSettings.IsConsole = False Then
                frmLoading.Close()
                Me.tcMain.SelectTab(2)
            End If

        Catch exError As Exception
            If asAppSettings.IsConsole = False Then
                frmLoading.Close()
            Else
                LogError($"Message: {exError.Message}")
                LogError($"StackTrace: {exError.StackTrace}")
                DisplayError(exError.Message)
            End If
        End Try

    End Sub

    Public Sub FilterResults(Optional FilterMinimum As Integer = CodeIssue.POSSIBLY_SAFE, Optional FilterMaximum As Integer = CodeIssue.CRITICAL)
        ' Clear the listview and display only the results in the required range
        '======================================================================
        Dim strDescription As String = ""
        Dim lviItem As ListViewItem
        Dim colOriginalColour As Color = asAppSettings.ListItemColour


        '== If there are no changes then exit the sub ==
        If (intFilterMin = FilterMinimum And intFilterMax = FilterMaximum) Then Exit Sub

        '== Store new settings ==
        intFilterMin = FilterMinimum
        intFilterMax = FilterMaximum

        If intFilterMax = CodeIssue.CRITICAL And intFilterMin = CodeIssue.POSSIBLY_SAFE Then
            blnIsFiltered = False
        Else
            blnIsFiltered = True
        End If


        '== Remove any previous results ==
        rtbResults.Text = ""
        lvResults.Items.Clear()

        '== Write out the new results ==
        For Each srResultItem In rtResultsTracker.ScanResults
            If srResultItem.Severity >= FilterMaximum And srResultItem.Severity <= FilterMinimum Then

                asAppSettings.ListItemColour = colOriginalColour

                '== Add to rich text ==
                strDescription = srResultItem.Description
                WriteResult(srResultItem.Title & vbNewLine, strDescription & vbNewLine & "Line: " & srResultItem.LineNumber & " - Filename: " & srResultItem.FileName & vbNewLine, srResultItem.CodeLine.Trim, srResultItem.Severity)

                '== Add to listview ==
                lviItem = New ListViewItem With {
                    .Name = srResultItem.ItemKey,
                    .Text = srResultItem.Severity
                }
                lviItem.SubItems.Add(srResultItem.SeverityDesc)
                lviItem.SubItems.Add(srResultItem.Title)
                lviItem.SubItems.Add(srResultItem.Description)
                lviItem.SubItems.Add(srResultItem.FileName)
                lviItem.SubItems.Add(srResultItem.LineNumber)


                If srResultItem.IsChecked = True Then
                    asAppSettings.ListItemColour = Color.FromName(srResultItem.CheckColour)
                    lviItem.Checked = srResultItem.IsChecked
                End If

                lvResults.Items.Add(lviItem)
            End If
        Next

    End Sub

    Private Sub lvResults_ItemCheck(sender As Object, e As System.Windows.Forms.ItemCheckEventArgs) Handles lvResults.ItemCheck
        ' Allow user to mark items
        '=========================

        '== Set item state for saves/sorts/etc. ==
        SetCheckState(Convert.ToInt32(lvResults.Items(e.Index).Name), e.NewValue, asAppSettings.ListItemColour)

    End Sub

    Private Sub SetCheckState(Index As Integer, Value As Boolean, SelectedColour As Color)
        ' Locate the item in results collection and assign appropriate check status
        '==========================================================================
        Dim intIndex As Integer


        For Each itmItem In rtResultsTracker.ScanResults
            If itmItem.ItemKey = Index Then
                itmItem.IsChecked = Value

                ' If the listview items have been sorted then they should be identified with their name, not index
                intIndex = lvResults.Items.IndexOfKey(Index)

                ' Apply the appropriate colour to the listview item
                If Value Then
                    lvResults.Items(intIndex).BackColor = SelectedColour
                    itmItem.CheckColour = SelectedColour
                Else
                    lvResults.Items(intIndex).BackColor = lvResults.BackColor
                End If

                Exit For
            End If
        Next

    End Sub

    Private Sub SetSeverity()
        ' Loop through selected items and set new severity value
        '=======================================================
        Dim arlIndexes As New ArrayList
        Dim frmNewSev As New frmNewSeverity


        '== Display dialog to get new value ==
        frmNewSev.ShowDialog(Me)
        If intNewSeverity = -1 Then Exit Sub

        '== Modify values for result set and listview ==
        For Each itmListItem In lvResults.SelectedItems
            arlIndexes.Add(itmListItem.Name)
        Next

        SetNewSeverity(arlIndexes, intNewSeverity, arlIndexes.Count)

    End Sub

    Private Sub SetNewSeverity(Indexes As ArrayList, Value As Integer, ItemCount As Integer)
        ' Locate the item in results collection and assign appropriate check status
        '==========================================================================
        Dim intIndex As Integer
        Dim intItemCount As Integer = 0


        For Each itmItem As ScanResult In rtResultsTracker.ScanResults
            If Indexes.Contains(CStr(itmItem.ItemKey)) Then

                ' Set new severity
                itmItem.SetSeverity(Value)

                ' If the listview items have been sorted then they should be identified with their name, not index
                intIndex = lvResults.Items.IndexOfKey(CStr(itmItem.ItemKey))

                ' Apply the new value to the listview item
                lvResults.Items(intIndex).Text = Value.ToString
                lvResults.Items(intIndex).SubItems(1).Text = itmItem.SeverityDesc

                ' Increase the count and exit loop if we've modified all results
                intItemCount += 1
                If intItemCount = ItemCount Then Exit For
            End If
        Next

    End Sub

    Private Sub OrderOnMultColumns()
        ' Show dialog and then order on selected columns
        '===============================================
        Dim lviItem As ListViewItem
        Dim colOriginalColour As Color = asAppSettings.ListItemColour


        frmSort.ShowDialog(Me)
        If dicColumns.Count = 3 Then
            lvResults.Items.Clear()
            SortResultsOnMultColumns()

            For Each itmItem In rtResultsTracker.ScanResults

                If Not blnIsFiltered Or (blnIsFiltered And itmItem.Severity >= intFilterMax And itmItem.Severity <= intFilterMin) Then
                    lviItem = New ListViewItem

                    asAppSettings.ListItemColour = colOriginalColour

                    '== Add to listview ==
                    lviItem.Name = itmItem.ItemKey
                    lviItem.Text = itmItem.Severity
                    lviItem.SubItems.Add(itmItem.SeverityDesc)
                    lviItem.SubItems.Add(itmItem.Title)
                    lviItem.SubItems.Add(itmItem.Description)
                    lviItem.SubItems.Add(itmItem.FileName)
                    lviItem.SubItems.Add(itmItem.LineNumber)

                    If itmItem.IsChecked = True Then
                        asAppSettings.ListItemColour = itmItem.CheckColour
                        lviItem.Checked = itmItem.IsChecked
                    End If

                    lvResults.Items.Add(lviItem)
                End If
            Next
        End If

    End Sub

    '================================================================================================
    '== All code/classes below used for sorting the summary table according to the selected column ==
    '------------------------------------------------------------------------------------------------

    Private Sub SortResultsOnMultColumns()
        ' Sort the collection on multiple columns 
        '========================================
        Dim mcsMultiCol As New MultiComparer With {
            .PrimaryField = dicColumns.Item("Primary"),
            .SecondaryField = dicColumns.Item("Secondary"),
            .TertiaryField = dicColumns.Item("Tertiary")
        }

        rtResultsTracker.ScanResults.Sort(mcsMultiCol)

    End Sub

    Private Sub SortResults(SortItem As Integer)
        ' Sort the collection - use specified item for comparison basis
        '==============================================================

        Dim scSevComp As New SeverityComparer
        Dim tcTitleComp As New TitleComparer
        Dim dcDescComp As New DescComparer
        Dim fcFileComp As New FileComparer


        Select Case SortItem
            Case PRIORITY_COL
                ' Severity
                blnIsAscendingSeverity = Not blnIsAscendingSeverity
                rtResultsTracker.ScanResults.Sort(scSevComp)
            Case SEVERITY_COL
                ' Severity
                blnIsAscendingSeverity = Not blnIsAscendingSeverity
                rtResultsTracker.ScanResults.Sort(scSevComp)
            Case TITLE_COL
                ' Issue name/title
                blnIsAscendingTitle = Not blnIsAscendingTitle
                rtResultsTracker.ScanResults.Sort(tcTitleComp)
            Case DESC_COL
                ' Issue description
                blnIsAscendingDescription = Not blnIsAscendingDescription
                rtResultsTracker.ScanResults.Sort(dcDescComp)
            Case FILE_COL
                ' Filename
                blnIsAscendingFile = Not blnIsAscendingFile
                rtResultsTracker.ScanResults.Sort(fcFileComp)
        End Select

    End Sub

    Private Class SeverityComparer
        Implements IComparer

        Private Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
            'Compare results by severity
            '===========================

            Dim scLeftResult, scRightResult As New ScanResult
            scLeftResult = CType(x, ScanResult)
            scRightResult = CType(y, ScanResult)

            If frmMain.blnIsAscendingSeverity Then
                Return New CaseInsensitiveComparer().Compare(x.Severity, y.Severity)
            Else
                Return New CaseInsensitiveComparer().Compare(y.Severity, x.Severity)
            End If

            '== Avoid the GUI locking or hanging during processing ==
            Application.DoEvents()

        End Function
    End Class

    Private Class TitleComparer
        Implements IComparer

        Private Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
            'Compare results by severity
            '===========================

            Dim scLeftResult, scRightResult As New ScanResult
            scLeftResult = CType(x, ScanResult)
            scRightResult = CType(y, ScanResult)

            If frmMain.blnIsAscendingTitle Then
                Return New CaseInsensitiveComparer().Compare(x.Title, y.Title)
            Else
                Return New CaseInsensitiveComparer().Compare(y.Title, x.Title)
            End If

            '== Avoid the GUI locking or hanging during processing ==
            Application.DoEvents()

        End Function
    End Class

    Private Class DescComparer
        Implements IComparer

        Private Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
            'Compare results by severity
            '===========================

            Dim scLeftResult, scRightResult As New ScanResult
            scLeftResult = CType(x, ScanResult)
            scRightResult = CType(y, ScanResult)

            If frmMain.blnIsAscendingDescription Then
                Return New CaseInsensitiveComparer().Compare(x.Description, y.Description)
            Else
                Return New CaseInsensitiveComparer().Compare(y.Description, x.Description)
            End If

            '== Avoid the GUI locking or hanging during processing ==
            Application.DoEvents()

        End Function
    End Class

    Private Class FileComparer
        Implements IComparer

        Private Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
            'Compare results by severity
            '===========================

            Dim scLeftResult, scRightResult As New ScanResult
            scLeftResult = CType(x, ScanResult)
            scRightResult = CType(y, ScanResult)

            If frmMain.blnIsAscendingFile Then
                Return New CaseInsensitiveComparer().Compare(x.FileName, y.FileName)
            Else
                Return New CaseInsensitiveComparer().Compare(y.FileName, x.FileName)
            End If

            '== Avoid the GUI locking or hanging during processing ==
            Application.DoEvents()

        End Function
    End Class

    Private Class MultiComparer
        Implements IComparer

        Public PrimaryField As Integer
        Public SecondaryField As Integer
        Public TertiaryField As Integer

        Private Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
            'Compare results by multiple fields
            '==================================
            Dim intRetVal As Integer

            Dim scLeftResult, scRightResult As New ScanResult

            scLeftResult = CType(x, ScanResult)
            scRightResult = CType(y, ScanResult)

            intRetVal = CompareFields(x, y, PrimaryField)
            If intRetVal = 0 Then
                intRetVal = CompareFields(x, y, SecondaryField)
                If intRetVal = 0 Then
                    intRetVal = CompareFields(x, y, TertiaryField)
                End If
            End If

            '== Avoid the GUI locking or hanging during processing ==
            Application.DoEvents()

            Return intRetVal

        End Function

        Private Function CompareFields(x As Object, y As Object, FieldID As Integer) As Integer

            Dim intRetVal As Integer

            Select Case FieldID
                Case SEVERITY_COL
                    ' Severity
                    intRetVal = New CaseInsensitiveComparer().Compare(x.Severity, y.Severity)
                Case TITLE_COL
                    ' Issue name/title
                    intRetVal = New CaseInsensitiveComparer().Compare(x.Title, y.Title)
                Case DESC_COL
                    ' Issue description
                    intRetVal = New CaseInsensitiveComparer().Compare(x.Description, y.Description)
                Case FILE_COL
                    ' Filename
                    intRetVal = New CaseInsensitiveComparer().Compare(x.FileName, y.FileName)
            End Select

            '== Avoid the GUI locking or hanging during processing ==
            Application.DoEvents()

            Return intRetVal

        End Function

    End Class

    Private Sub VisualBadFuncBreakdownToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles VisualBadFuncBreakdownToolStripMenuItem.Click
        ' Scan the code only for items listed in the associated config file
        '==================================================================
        Dim intIndex As Integer


        asAppSettings.IsConfigOnly = True

        '== If no data available then scan files in 'code only' mode ==
        If rtResultsTracker.OverallLineCount = 0 Then
            ScanFiles(False, True)
        Else
            For intIndex = 0 To rtResultsTracker.FileDataList.Count - 1
                UpdateFileView(rtResultsTracker.FileDataList.Item(intIndex), intIndex, False)
            Next
        End If

        ShowResults()

    End Sub

    Private Sub ExportMetaDataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportMetaDataToolStripMenuItem.Click
        ExportMetaDataXML()
    End Sub

    Private Sub ToolsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SettingsToolStripMenuItem.Click

    End Sub

    Private Sub spMain_Panel2_Paint(sender As Object, e As PaintEventArgs) Handles spMain.Panel2.Paint

    End Sub

    Private Sub lvResults_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvResults.SelectedIndexChanged

    End Sub

    Private Sub cboLanguage_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboLanguage.SelectedIndexChanged
        ' #### TODO: SET SELECTED LANGUAGE IN TOOLSTRIP BASED ON THEIR SELECTION

        SelectLanguage(cboLanguage.SelectedItem.Value)

    End Sub

    Private Sub PopulateLanguageOptions()
        Dim list = New List(Of KeyValuePair(Of String, Integer)) From {
            New KeyValuePair(Of String, Integer)("C/C++", AppSettings.C),
            New KeyValuePair(Of String, Integer)("Java", AppSettings.JAVA),
            New KeyValuePair(Of String, Integer)("PL/SQL", AppSettings.SQL),
            New KeyValuePair(Of String, Integer)("C#", AppSettings.CSHARP),
            New KeyValuePair(Of String, Integer)("VB", AppSettings.VB),
            New KeyValuePair(Of String, Integer)("PHP", AppSettings.PHP),
            New KeyValuePair(Of String, Integer)("COBOL", AppSettings.COBOL)
        }

        cboLanguage.DataSource = list
        cboLanguage.ValueMember = "Value"
        cboLanguage.DisplayMember = "Key"

        cboLanguage.SelectedItem = asAppSettings.StartType

    End Sub

    Private Sub cboTargetDir_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cboTargetDir.SelectionChangeCommitted

    End Sub

    Private Sub frmMain_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        frmLoading.Close()
    End Sub

    Private Sub cboLanguage_KeyDown(sender As Object, e As KeyEventArgs) Handles cboLanguage.KeyDown
        If e.KeyCode = Keys.Enter Then
            cboLanguage_SelectedIndexChanged(sender, e)
            LoadFiles(cboTargetDir.Text, True)
        End If
    End Sub
End Class


