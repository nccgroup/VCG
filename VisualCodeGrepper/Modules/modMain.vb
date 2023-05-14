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

Option Explicit On
Imports System.IO
Imports System.Text.RegularExpressions

Public Module modMain

    '== Array to be used when sorting on multiple coilumns ==
    Public dicColumns As New Dictionary(Of String, Integer)

    '== Class instance to hold app settings ==
    Public asAppSettings As New AppSettings

    '== Class instances to track details of file/code scanning operations ==
    Public ctCodeTracker As New CodeTracker
    Public rtResultsTracker As New ResultsTracker

    '== Used for sharing data between main chart and individual charts ==
    Public strCurrentFileName As String = ""
    Public intComments As Integer = 0
    Public intCodeIssues As Integer = 0

    '== Placeholder to be used when modifying severity levels ==
    Public intNewSeverity As Integer = -1

    Public Function ParseArgs() As Integer
        ' Read any command line args and start application as appropriate
        '================================================================
        Dim intIndex As Integer
        Dim arrArgs() As String = Environment.GetCommandLineArgs()
        Dim exportFlagSet = False

        '== Deal with any command line options ==
        If arrArgs.Count <> 1 Then
            intIndex = 1
            While intIndex < arrArgs.Count()

                Select Case arrArgs(intIndex)

                    Case "-t", "--target"
                        ' Set target
                        intIndex += 1
                        If intIndex < arrArgs.Count() Then
                            rtResultsTracker.TargetDirectory = arrArgs(intIndex)

                            If String.IsNullOrWhiteSpace(rtResultsTracker.TargetDirectory) Then
                                LogError("No target specified (-t)")
                                ShowHelp()
                                Return -1
                            ElseIf (Directory.Exists(rtResultsTracker.TargetDirectory) = False) Then
                                LogError("Target does not exist: {0}", rtResultsTracker.TargetDirectory)
                                ShowHelp()
                                Return -1
                            End If
                        End If
                    Case "-l", "--language"
                        ' Set language
                        intIndex += 1
                        If intIndex < arrArgs.Count() Then
                            Dim language = arrArgs(intIndex)
                            If Not SetLanguage(language) Then
                                LogError("Unrecognised language: {0}", language)
                                ShowHelp()
                                Return -1
                            End If

                            ' Set up associated extensions with language
                            SetSuffixes(asAppSettings.TestType)
                        Else
                            LogError("No language specified (-l)")
                            ShowHelp()
                            Return -1
                        End If

                    Case "-e", "--extensions"
                        ' Set file extensions
                        intIndex += 1
                        If intIndex < arrArgs.Count() Then
                            asAppSettings.FileSuffixes = arrArgs(intIndex).Split("|")
                        Else
                            LogError("No extensions provided (-e)")
                            ShowHelp()
                            Return -1
                        End If

                    Case "-i", "--import"
                        ' Import XML results
                        intIndex += 1
                        If intIndex < arrArgs.Count() Then
                            If arrArgs(intIndex).ToLower.EndsWith(".xml") Then
                                If File.Exists(arrArgs(intIndex)) = False Then
                                    LogError("Import XML file does not exist: {0}", arrArgs(intIndex))
                                    ShowHelp()
                                    Return -1
                                End If

                                asAppSettings.IsXmlInputFile = True
                                asAppSettings.XmlInputFile = arrArgs(intIndex)
                                frmMain.ImportResultsXML(asAppSettings.XmlInputFile)
                            ElseIf arrArgs(intIndex).ToLower.EndsWith(".csv") Then

                                If File.Exists(arrArgs(intIndex)) = False Then
                                    LogError("Import CSV file does not exist: {0}", arrArgs(intIndex))
                                    ShowHelp()
                                    Return -1
                                End If

                                asAppSettings.IsCsvInputFile = True
                                asAppSettings.CsvInputFile = arrArgs(intIndex)
                                frmMain.ImportResultsCSV(asAppSettings.CsvInputFile)
                            End If

                            ' If results are being imported for inspection then console-only mode must be off and we should not have a target!
                            asAppSettings.IsConsole = False
                            Dim strTarget As String = ""
                            Exit While
                        Else
                            LogError("No input filename provided (-i)")
                        End If

                    Case "-x", "--export"
                        ' Automatically export XML results
                        intIndex += 1
                        If intIndex < arrArgs.Count() Then
                            asAppSettings.XmlOutputFile = arrArgs(intIndex)
                            asAppSettings.IsXmlOutputFile = True
                            exportFlagSet = True
                        Else
                            LogError("No XML results filename provided (-x)")
                            ShowHelp()
                            Return -1
                        End If

                    Case "-f", "--csv-export"
                        ' Automatically export CSV results
                        intIndex += 1
                        If intIndex < arrArgs.Count() Then
                            asAppSettings.CsvOutputFile = arrArgs(intIndex)
                            asAppSettings.IsCsvOutputFile = True
                            exportFlagSet = True
                        Else
                            LogError("No CSV results filename provided (-f)")
                            ShowHelp()
                            Return -1
                        End If

                    Case "-r", "--results"
                        ' Automatically export flat text results
                        intIndex += 1
                        If intIndex < arrArgs.Count() Then
                            asAppSettings.OutputFile = arrArgs(intIndex)
                            asAppSettings.IsOutputFile = True
                            exportFlagSet = True
                        Else
                            LogError("No results filename provided (-r)")
                            ShowHelp()
                            Return -1
                        End If

                    Case "-c", "--console"
                        AttachProcessToConsole()
                        asAppSettings.IsConsole = True
                        frmMain.Visible = False
                        asAppSettings.DisplayBreakdownOption = False
                        asAppSettings.VisualBreakdownEnabled = False

                        ' Stops error messages being printed after the Console command
                        LogBlank()

                    Case "-v", "--verbose"
                        ' Verbose mode
                        asAppSettings.IsVerbose = True
                    Case "--help", "/?", "-h"
                        ShowHelp()
                        Return -1
                    Case "--debug"
                        ' Expected flag, dealt with in AttachDebugger.. function
                    Case Else
                        ' Help
                        LogError($"Unknown option {arrArgs(intIndex)}")
                        ShowHelp()
                        Return -1
                End Select

                intIndex += 1
            End While
        End If

        If asAppSettings.IsConsole = True Then
            If exportFlagSet = False Then
                LogError("ERROR: No output flag set (i.e. XML, CSV, Flat), so no results will be generated. Exiting...")
                Return -1
            End If
            frmMain.Hide()
        End If

        Return intIndex

    End Function



    Public Sub ShowHelp()
        ' Display help and show usage
        '============================

        Dim strHelp As String

        strHelp = "Visual Code Grepper (VCG) 2.0 (C) Nick Dunn And John Murray, 2012-2014.
            Usage:  VisualCodeGrepper [OPTIONS]       
    
            STARTUP:
            Set desired starting point for GUI. If using console mode these options will set target(s) to be scanned.

             -t, --target <Filename|DirectoryName>:      Set target file or directory. Use this option either to load target 
                                                         immediately into GUI or to provide the target for console mode.
             -l, --language <CPP|PLSQL|JAVA|CS|VB|PHP>   Set target language (Default is C/C++).
             -e, --extensions <ext1|ext2|ext3>           Set file extensions to be analysed (See ReadMe or Options screen for language-specific defaults).
             -i, --import <Filename>                     Import XML/CSV results to GUI.

            OUTPUT:
            Automagically export results to a file in the specified format. Use XML output if you wish to reload results into the GUI later on.

             -x, --export <Filename>                     Automatically export results to XML file.
             -f, --csv-export <Filename>                 Automatically export results to CSV file.
             -r, --results <Filename>                    Automatically export results to flat text file.

            CONSOLE OPTIONS:
             -c, --console                               Run application in console only (hide GUI).
             -v, --verbose                               Set console output to verbose mode.
             -h, --help                                  Show (this) help.

            EXAMPLES:

            > VisualCodeGrepper.exe -t c:\code\mycodebase -lang VB
    
                Loads the UI with the Target Directory pre-filled, and language pre-selected

            > VisualCodeGrepper --console -t c:\code\mycodebase -lang VB -r c:\code\results.txt  

                Supresses the UI, scans Target Directory for VB files, and saves output to a Flat File."


        If asAppSettings.IsConsole Then
            LogBlank(strHelp)
        Else
            MessageBox.Show(strHelp, "Help", MessageBoxButtons.OK)
        End If


    End Sub

    Private Function SetLanguage(NewLanguage As String) As Boolean
        ' Get new langauge from command line
        '===================================
        Dim blnRetVal As Boolean = True

        NewLanguage = NewLanguage.ToUpper

        Select Case NewLanguage
            Case "C", "C++", "CPP"
                asAppSettings.TestType = AppSettings.C
            Case "JAVA"
                asAppSettings.TestType = AppSettings.JAVA
            Case "PL/SQL", "PLSQL", "SQL"
                asAppSettings.TestType = AppSettings.SQL
            Case "C#", "C-SHARP", "CS", "CSHARP"
                asAppSettings.TestType = AppSettings.CSHARP
            Case "VB", "VISUALBASIC", "VISUAL-BASIC"
                asAppSettings.TestType = AppSettings.VB
            Case "PHP"
                asAppSettings.TestType = AppSettings.PHP
            Case "COBOL"
                asAppSettings.TestType = AppSettings.COBOL
            Case "R"
                asAppSettings.TestType = AppSettings.R
            Case Else
                blnRetVal = False
        End Select

        Return blnRetVal

    End Function

    Public Sub LaunchNPP(FileName As String, Optional LineNumber As Integer = 1)
        ' Launch NPP if available, launch Notepad if not
        '===============================================

        Try
            ' If we're trying to open a file on a specific line in Notepad++ then the filename *must* be quoted to avoid erratic behaviour from Windows
            System.Diagnostics.Process.Start("notepad++.exe", $"-n{LineNumber} ""{FileName}""")
        Catch ex As Exception
            System.Diagnostics.Process.Start("Notepad.exe", """" & FileName & """")
        End Try

    End Sub

    <Obsolete("Not currently integrated. Coming soon!", True)>
    Public Sub LaunchVSCode(FileName As String, Optional LineNumber As Integer = 1)
        ' Launch VS Codeif available, launch Notepad if not
        '===============================================

        Try
            ' If we're trying to open a file on a specific line in Notepad++ then the filename *must* be quoted to avoid erratic behaviour from Windows
            System.Diagnostics.Process.Start("code.exe", $"--goto {LineNumber} ""{FileName}""")
        Catch ex As Exception
            System.Diagnostics.Process.Start("Notepad.exe", $"""{FileName}\""")
        End Try

    End Sub
    Public Sub SelectLanguage(Language As Integer)
        ' Set language and characteristics 
        '=================================


        ' Set language type
        asAppSettings.TestType = Language

        '== Set the file types/suffixes for the chosen language ==
        SetSuffixes(Language)

        ' This covers most languages - the different ones will be set individually, below
        asAppSettings.SingleLineComment = "//"
        asAppSettings.AltSingleLineComment = ""

        ' Load list of unsafe functions
        Select Case Language
            Case AppSettings.C
                asAppSettings.BadFuncFile = asAppSettings.CConfFile
                LoadUnsafeFunctionList(AppSettings.C)
            Case AppSettings.JAVA
                asAppSettings.BadFuncFile = asAppSettings.JavaConfFile
                LoadUnsafeFunctionList(AppSettings.JAVA)
            Case AppSettings.SQL
                asAppSettings.BadFuncFile = asAppSettings.PLSQLConfFile
                LoadUnsafeFunctionList(AppSettings.SQL)
                asAppSettings.SingleLineComment = "--"
            Case AppSettings.CSHARP
                asAppSettings.BadFuncFile = asAppSettings.CSharpConfFile
                LoadUnsafeFunctionList(AppSettings.CSHARP)
            Case AppSettings.VB
                asAppSettings.BadFuncFile = asAppSettings.VBConfFile
                LoadUnsafeFunctionList(AppSettings.VB)
                asAppSettings.SingleLineComment = "'"
                asAppSettings.AltSingleLineComment = "REM"
            Case AppSettings.PHP
                asAppSettings.BadFuncFile = asAppSettings.PHPConfFile
                LoadUnsafeFunctionList(AppSettings.PHP)
                asAppSettings.SingleLineComment = "//"
                asAppSettings.AltSingleLineComment = "\#"   ' This will be used in a regex so it must be escaped
            Case AppSettings.COBOL
                asAppSettings.BadFuncFile = asAppSettings.COBOLConfFile
                LoadUnsafeFunctionList(AppSettings.COBOL)
                asAppSettings.SingleLineComment = "*"
                asAppSettings.AltSingleLineComment = "\/"   ' This will be used in a regex so it must be escaped
            Case AppSettings.R
                asAppSettings.BadFuncFile = asAppSettings.RConfFile
                LoadUnsafeFunctionList(AppSettings.R)
                asAppSettings.SingleLineComment = "#"
        End Select

        ' Set the GUI to display correct options for the language
        If asAppSettings.IsConsole = True Then Exit Sub

        With frmMain
            Select Case Language
                Case AppSettings.C
                    .JavaToolStripMenuItem.Checked = False
                    .PLSQLToolStripMenuItem.Checked = False
                    .CSToolStripMenuItem.Checked = False
                    .VBToolStripMenuItem.Checked = False
                    .PHPToolStripMenuItem.Checked = False
                    .COBOLToolStripMenuItem.Checked = False
                    .sslLabel.Text = "Language: C/C++   File Suffixes: " & asAppSettings.CSuffixes
                Case AppSettings.JAVA
                    .CCToolStripMenuItem.Checked = False
                    .PLSQLToolStripMenuItem.Checked = False
                    .CSToolStripMenuItem.Checked = False
                    .VBToolStripMenuItem.Checked = False
                    .PHPToolStripMenuItem.Checked = False
                    .COBOLToolStripMenuItem.Checked = False
                    .sslLabel.Text = "Language: Java   File Suffixes: " & asAppSettings.JavaSuffixes
                Case AppSettings.SQL
                    .CCToolStripMenuItem.Checked = False
                    .JavaToolStripMenuItem.Checked = False
                    .CSToolStripMenuItem.Checked = False
                    .VBToolStripMenuItem.Checked = False
                    .PHPToolStripMenuItem.Checked = False
                    .COBOLToolStripMenuItem.Checked = False
                    asAppSettings.SingleLineComment = "--"
                    .sslLabel.Text = "Language: PL/SQL   File Suffixes: " & asAppSettings.PLSQLSuffixes
                Case AppSettings.CSHARP
                    .CCToolStripMenuItem.Checked = False
                    .JavaToolStripMenuItem.Checked = False
                    .PLSQLToolStripMenuItem.Checked = False
                    .VBToolStripMenuItem.Checked = False
                    .PHPToolStripMenuItem.Checked = False
                    .COBOLToolStripMenuItem.Checked = False
                    .sslLabel.Text = "Language: C#   File Suffixes: " & asAppSettings.CSharpSuffixes
                Case AppSettings.VB
                    .CCToolStripMenuItem.Checked = False
                    .JavaToolStripMenuItem.Checked = False
                    .PLSQLToolStripMenuItem.Checked = False
                    .CSToolStripMenuItem.Checked = False
                    .PHPToolStripMenuItem.Checked = False
                    .COBOLToolStripMenuItem.Checked = False
                    .sslLabel.Text = "Language: VB   File Suffixes: " & asAppSettings.VBSuffixes
                Case AppSettings.PHP
                    .CCToolStripMenuItem.Checked = False
                    .JavaToolStripMenuItem.Checked = False
                    .PLSQLToolStripMenuItem.Checked = False
                    .CSToolStripMenuItem.Checked = False
                    .VBToolStripMenuItem.Checked = False
                    .COBOLToolStripMenuItem.Checked = False
                    .sslLabel.Text = "Language: PHP   File Suffixes: " & asAppSettings.PHPSuffixes
                Case AppSettings.COBOL
                    .CCToolStripMenuItem.Checked = False
                    .JavaToolStripMenuItem.Checked = False
                    .PLSQLToolStripMenuItem.Checked = False
                    .CSToolStripMenuItem.Checked = False
                    .VBToolStripMenuItem.Checked = False
                    .sslLabel.Text = "Language: COBOL   File Suffixes: " & asAppSettings.COBOLSuffixes
                Case AppSettings.R
                    .CCToolStripMenuItem.Checked = False
                    .JavaToolStripMenuItem.Checked = False
                    .PLSQLToolStripMenuItem.Checked = False
                    .CSToolStripMenuItem.Checked = False
                    .VBToolStripMenuItem.Checked = False
                    .COBOLToolStripMenuItem.Checked = False
                    .sslLabel.Text = "Language: R   File Suffixes: " & asAppSettings.RSuffixes
            End Select
        End With

    End Sub

    Public Sub SetSuffixes(Language As Integer)
        ' Set the filetypes to scan
        '==========================

        asAppSettings.IsAllFileTypes = False

        '== Check if wildcard/all files has been specified == 
        If asAppSettings.CSuffixes.Contains(".*") Or asAppSettings.CSuffixes.Trim = "" Then
            asAppSettings.IsAllFileTypes = True
        Else
            Select Case Language
                Case AppSettings.C
                    asAppSettings.FileSuffixes = asAppSettings.CSuffixes.Split("|")
                Case AppSettings.JAVA
                    asAppSettings.FileSuffixes = asAppSettings.JavaSuffixes.Split("|")
                Case AppSettings.SQL
                    asAppSettings.FileSuffixes = asAppSettings.PLSQLSuffixes.Split("|")
                Case AppSettings.CSHARP
                    asAppSettings.FileSuffixes = asAppSettings.CSharpSuffixes.Split("|")
                Case AppSettings.VB
                    asAppSettings.FileSuffixes = asAppSettings.VBSuffixes.Split("|")
                Case AppSettings.PHP
                    asAppSettings.FileSuffixes = asAppSettings.PHPSuffixes.Split("|")
                Case AppSettings.COBOL
                    asAppSettings.FileSuffixes = asAppSettings.COBOLSuffixes.Split("|")
                Case AppSettings.R
                    asAppSettings.FileSuffixes = asAppSettings.RSuffixes.Split("|")
            End Select

            asAppSettings.NumSuffixes = asAppSettings.FileSuffixes.Length - 1
        End If

    End Sub

    Public Sub LoadUnsafeFunctionList(CurrentLanguage As Integer)
        ' Load appropriate list of bad functions from file (dependent on selected language)
        '==================================================================================

        Dim strDescription As String = ""
        Dim strConfFile As String = ""
        Dim arrFuncList As String()


        asAppSettings.BadFunctions.Clear()


        'ToDo: check these against their safe equivalents, make sure not flagging any false positives or false negatives, might be worthwhile to do a check (later) if it is flagged as _
        'eg. sprintf its not u_vsprintf, etc.

        ' Check file exists 
        If Not File.Exists(asAppSettings.BadFuncFile) Then

            ' Restore default file in case of bad registry entries, user placing non-existent file in Options dialog, etc.
            Select Case CurrentLanguage
                Case AppSettings.C
                    asAppSettings.BadFuncFile = Path.Combine(AppSettings.ApplicationDirectory, "cppfunctions.conf")
                Case AppSettings.JAVA
                    asAppSettings.BadFuncFile = Path.Combine(AppSettings.ApplicationDirectory, "javafunctions.conf")
                Case AppSettings.SQL
                    asAppSettings.BadFuncFile = Path.Combine(AppSettings.ApplicationDirectory, "plsqlfunctions.conf")
                Case AppSettings.CSHARP
                    asAppSettings.BadFuncFile = Path.Combine(AppSettings.ApplicationDirectory, "csfunctions.conf")
                Case AppSettings.VB
                    asAppSettings.BadFuncFile = Path.Combine(AppSettings.ApplicationDirectory, "vbfunctions.conf")
                Case AppSettings.PHP
                    asAppSettings.BadFuncFile = Path.Combine(AppSettings.ApplicationDirectory, "phpfunctions.conf")
                Case AppSettings.COBOL
                    asAppSettings.BadFuncFile = Path.Combine(AppSettings.ApplicationDirectory, "cobolfunctions.conf")
                Case AppSettings.R
                    asAppSettings.BadFuncFile = Path.Combine(AppSettings.ApplicationDirectory, "rfunctions.conf")
            End Select

            If Not File.Exists(asAppSettings.BadFuncFile) Then MsgBox("No config file found for bad functions.", MsgBoxStyle.Critical, "Error")

        Else
            Try
                For Each strLine In File.ReadLines(asAppSettings.BadFuncFile)

                    ' Check for comments/whitespace
                    If (strLine.Trim() <> Nothing) And (Not strLine.Trim().StartsWith("//")) Then

                        Dim ciCodeIssue As New CodeIssue

                        ' Build up array of bad functions and any associated descriptions
                        If strLine.Contains("=>") Then
                            arrFuncList = Regex.Split(strLine, "=>")
                            ciCodeIssue.FunctionName = arrFuncList.First

                            strDescription = arrFuncList.Last.Trim

                            ' Extract severity level from description (if present)
                            If strDescription.StartsWith("[0]") Or strDescription.StartsWith("[1]") Or strDescription.StartsWith("[2]") Or strDescription.StartsWith("[3]") Then
                                ciCodeIssue.Severity = CInt(strDescription.Substring(1, 1))
                                strDescription = strDescription.Substring(3).Trim
                            End If

                            ciCodeIssue.Description = strDescription
                        Else
                            ciCodeIssue.FunctionName = strLine
                            ciCodeIssue.Description = ""
                        End If

                        If Not asAppSettings.BadFunctions.Contains(ciCodeIssue) Then asAppSettings.BadFunctions.Add(ciCodeIssue)
                    End If
                Next

            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End If

        ' Fix to stop temp content being wiped at start of scan
        If asAppSettings.TempGrepText <> "" Then
            frmOptions.LoadTempGrepContent(asAppSettings.TempGrepText)
        End If

    End Sub

    Public Sub LoadBadComments()
        ' Get list of bad comments from config file
        '==========================================

        Try
            For Each strLine In File.ReadLines(asAppSettings.BadCommentFile)

                ' Check for comments/whitespace
                If (strLine.Trim() <> Nothing) And (Not strLine.Trim().StartsWith("//")) Then
                    asAppSettings.BadComments.Add(strLine)
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

    End Sub

    Public Sub CheckCode(CodeLine As String, FileName As String)
        ' Scan line of code for anything requiring attention and return results
        '======================================================================

        Dim intIndex As Integer
        Dim strCleanName As String = ""
        Dim strTidyFuncName As String = ""

        '== Locate any unsafe functions for the language in question ==
        If asAppSettings.BadFunctions.Count > 0 Then
            For intIndex = 0 To asAppSettings.BadFunctions.Count - 1

                '== Sanitise the expression ready for insertion into regex ==
                strTidyFuncName = asAppSettings.BadFunctions(intIndex).FunctionName.Trim

                '== Important - comparison MUST be case-sensitive for everything except PL/SQL, where it MUST be case-insenstive ==
                If asAppSettings.TestType = AppSettings.SQL Then strTidyFuncName = strTidyFuncName.ToUpper

                '== Add word boundaries ONLY IF the expression does not contain whitespace or dots ==
                If ((Not Regex.IsMatch(strTidyFuncName, "\s+")) And (Not strTidyFuncName.Contains("."))) Then strCleanName = "\b" & Regex.Escape(strTidyFuncName) & "\b"

                '== Important - comparison MUST be case-sensitive for everything except PL/SQL, where it MUST be case-insenstive ==
                If asAppSettings.TestType = AppSettings.SQL Then
                    If (strCleanName <> "" And Regex.IsMatch(CodeLine.ToUpper, strCleanName)) Or (strCleanName = "" And CodeLine.ToUpper.Contains(asAppSettings.BadFunctions(intIndex).FunctionName.toupper)) Then
                        frmMain.ListCodeIssue(asAppSettings.BadFunctions(intIndex).FunctionName, asAppSettings.BadFunctions(intIndex).Description, FileName, asAppSettings.BadFunctions(intIndex).Severity, CodeLine)
                    End If
                Else
                    'If CodeLine.Contains(asAppSettings.BadFunctions(intIndex).FunctionName) Then
                    If (strCleanName <> "" And Regex.IsMatch(CodeLine, strCleanName)) Or (strCleanName = "" And CodeLine.Contains(asAppSettings.BadFunctions(intIndex).FunctionName)) Then
                        frmMain.ListCodeIssue(strTidyFuncName, asAppSettings.BadFunctions(intIndex).Description, FileName, asAppSettings.BadFunctions(intIndex).Severity, CodeLine)
                    End If
                End If
                strCleanName = ""
            Next intIndex
        End If

        '== Only carry out further code checks if required by user ==
        If asAppSettings.IsConfigOnly = False Then

            '== Carry out any language-specific tests ==
            Select Case asAppSettings.TestType
                Case AppSettings.C
                    CheckCPPCode(CodeLine, FileName)
                Case AppSettings.JAVA
                    CheckJavaCode(CodeLine, FileName)
                Case AppSettings.SQL
                    CheckPLSQLCode(CodeLine.ToUpper, FileName)
                Case AppSettings.CSHARP
                    CheckCSharpCode(CodeLine, FileName)
                Case AppSettings.VB
                    CheckVBCode(CodeLine, FileName)
                Case AppSettings.PHP
                    CheckPHPCode(CodeLine, FileName)
                Case AppSettings.COBOL
                    CheckCobolCode(CodeLine, FileName)
                Case AppSettings.R
                    CheckRCode(CodeLine, FileName)
            End Select

            '== Check for possible hard-coded passwords ==
            If CodeLine.ToLower().Contains("password ") And (InStr(CodeLine.ToLower(), "password") < InStr(CodeLine, "= """)) And Not (CodeLine.Contains("''") Or CodeLine.Contains("""""")) Then
                frmMain.ListCodeIssue("Code Appears to Contain Hard-Coded Password", "The code may contain a hard-coded password which an attacker could obtain from the source or by dis-assembling the executable. Please manually review the code:", FileName, CodeIssue.MEDIUM, CodeLine)
            End If
        End If

    End Sub
    Public Function GetVarName(CodeLine As String, Optional SplitOnEquals As Boolean = False)
        ' Extract the variable name from a line of code
        '==============================================
        Dim strVarName As String = ""
        Dim arrFragments As String()


        If CodeLine.Contains("=") Or SplitOnEquals Then
            arrFragments = CodeLine.Trim.Split("=")
            strVarName = arrFragments.First
        Else
            arrFragments = CodeLine.Trim.Split(";")
            strVarName = arrFragments.First
        End If

        strVarName = GetLastItem(strVarName)

        '== Be careful of anything which may break the regex ==
        strVarName = strVarName.TrimStart("(").Trim()
        strVarName = strVarName.TrimEnd(")").Trim()

        If asAppSettings.TestType = AppSettings.PHP Then strVarName = "\" & strVarName

        Return strVarName

    End Function

    Public Function GetLastItem(ListString As String, Optional Separator As String = " ") As String
        'Split string on specified character (default: space) and return last item
        '=========================================================================
        Dim strRetVal As String = ""
        Dim arrStrings As String()

        ListString = ListString.Trim()

        Select Case Separator
            Case " "
                ' This regex prevents a split on space from returning empty strings
                arrStrings = Regex.Split(ListString, "\s+")
            Case Else
                arrStrings = ListString.Split(Separator)
        End Select

        ' Return final item
        strRetVal = arrStrings.Last.Trim

        Return strRetVal

    End Function

    Public Function GetFirstItem(ListString As String, Optional Separator As String = " ") As String
        'Split string on specified character (default: space) and return first item
        '=========================================================================
        Dim strRetVal As String = ""
        Dim arrStrings As String()

        ListString = ListString.Trim()

        Select Case Separator
            Case " "
                ' This regex prevents a split on space from returning empty strings
                arrStrings = Regex.Split(ListString, "\s+")
            Case Else
                arrStrings = ListString.Split(Separator)
        End Select

        ' Return first item
        strRetVal = arrStrings.First.Trim

        Return strRetVal

    End Function

    Public Sub CheckFileLevelIssues(FileName As String)
        'List any file-level code issues (mis-matched deletes, mallocs, etc.)
        '====================================================================

        With frmMain
            If asAppSettings.TestType = AppSettings.C And ctCodeTracker.GetMemAssign.Count > 0 Then
                .ListMemoryIssue(ctCodeTracker.GetMemAssign)
            ElseIf asAppSettings.TestType = AppSettings.JAVA Then
                If ctCodeTracker.ImplementsClone = True Then
                    .ListCodeIssue("Class Implements Public 'clone' Method", "Cloning allows an attacker to instantiate a class without running any of the class constructors by deploying hostile code in the JVM.", FileName, CodeIssue.MEDIUM)
                End If
                If ctCodeTracker.IsSerialize = True Then
                    .ListCodeIssue("Class Implements Serialization", "Serialization can be used to save objects (and their state) when the JVM is switched off. The process flattens the object, saving it as a stream of bytes, allowing an attacker to view the inner state of an object and potentially view private attributes.", FileName, CodeIssue.MEDIUM)
                End If
                If ctCodeTracker.IsDeserialize = True Then
                    .ListCodeIssue("Class Implements Deserialization", "Deserialization allows the creation of an object from a stream of bytes, allowing the instantiation of a legitimate class without calling its constructor. This behaviour can be abused by an attacker to instantiate or replicate an object’s state.", FileName, CodeIssue.MEDIUM)
                End If
                If ctCodeTracker.HasXXEEnabled = True Then
                    .ListCodeIssue("XML Entity Expansion", "The class Uses JAXB and may allow XML entity expansion, which can render the application vulnerable to the use of XML bombs. Manually confirm that JAXB 2.0 or later is in use, which is not vulnerable, otherwise check the feasibility of disabling this feature and check for validation of incoming data.", FileName, CodeIssue.STANDARD)
                End If
                If ctCodeTracker.IsFileOpen = True Then
                    .ListCodeIssue("Failure To Release Resources In All Cases", "There appears to be no 'finally' block to release resources if an exception occurs, potentially resulting in DoS conditions from excessive resource consumption.", FileName, CodeIssue.MEDIUM, "", ctCodeTracker.FileOpenLine)
                    If ctCodeTracker.HasTry = False Then
                        .ListCodeIssue("FileStream Opened Without Exception Handling", "There appears to be no 'try' block to safely open the filestream, potentially resulting in server-side exceptions.", FileName, CodeIssue.MEDIUM, "", ctCodeTracker.FileOpenLine)
                    End If
                End If
                If ctCodeTracker.HasResourceRelease = False Then
                    .ListCodeIssue("Failure To Release Resources In All Cases", "There appears to be no release of resources in the 'finally' block, potentially resulting in DoS conditions from excessive resource consumption.", FileName, CodeIssue.MEDIUM, "", ctCodeTracker.FileOpenLine)
                End If
            ElseIf asAppSettings.TestType = AppSettings.COBOL And ctCodeTracker.ProgramId.Trim() = "" Then
                .ListCodeIssue("File Has No PROGRAM-ID", "The file does not appear to include a PROGRAM-ID. The lack of a properly formatted identification division can make code more difficult to read and maintain.", FileName, CodeIssue.LOW)
            End If
        End With

    End Sub

End Module
