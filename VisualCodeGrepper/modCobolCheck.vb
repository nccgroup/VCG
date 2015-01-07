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

Imports System.Text.RegularExpressions

Module modCobolCheck

    ' Specific checks for COBOL code
    '===============================

    Public Sub CheckCobolCode(ByVal CodeLine As String, ByVal FileName As String)
        ' Carry out any specific checks for the language in question
        '===========================================================

        TrackVarAssignments(CodeLine, FileName)         ' Check for matching new/delete, etc.
        CheckBuffer(CodeLine, FileName)                 ' Track buffer sizes and check for overflows
        CheckSigned(CodeLine, FileName)                 ' Check for signed/unsigned integer comparisons
        CheckInputValidation(CodeLine, FileName)        ' How is input being handled before display/storage?
        CheckFileAccess(CodeLine, FileName)          ' Are sensitive variables stored insecurely?
        CheckLogDisplay(CodeLine, FileName)             ' Is data sanitised before being written to logs?
        CheckFileRace(CodeLine, FileName)               ' Check for race conditions and TOCTOU vulns
        CheckRandomisation(CodeLine, FileName)          ' Locate any use of randomisation functions that are not cryptographically secure
        CheckUnsafeTempFiles(CodeLine, FileName)        ' Check for static/obvious filenames for temp files
        CheckDynamicCall(CodeLine, FileName)            ' Identify any user controlled variables used for dynamic function calls

        If Regex.IsMatch(CodeLine, "(LOWER|UPPER)\-CASE\s*\(\S*(Password|password|PASSWORD|pwd|PWD|passwd|PASSWD)") Then
            frmMain.ListCodeIssue("Unsafe Password Management", "The application appears to handle passwords in a case-insensitive manner. This can greatly increase the likelihood of successful brute-force and/or dictionary attacks.", FileName, CodeIssue.MEDIUM, CodeLine)
        End If

    End Sub

    Private Sub CheckIdentificationDivision(ByVal CodeLine As String, ByVal FileName As String)
        ' The Identification Division of a COBOL program should ideally contain only one PROGRAM-ID for maintainability
        '==============================================================================================================
        Dim arrFragments As String()

        ' Do we have a PROGRAM-ID?
        If ctCodeTracker.ProgramId = "" And Regex.IsMatch(CodeLine, "\bPROGRAM-ID\b\s*\w+") Then

            ' Record the ID
            arrFragments = Regex.Split(CodeLine, "\bPROGRAM-ID\b\s*")
            ctCodeTracker.ProgramId = arrFragments.Last.Trim

            ' Does the ID match the filename?
            If Not Regex.IsMatch(FileName.ToLower, "(\\|\/)" & ctCodeTracker.ProgramId & "\.{w}3") Then
                frmMain.ListCodeIssue("Filename Does Not Match PROGRAM-ID", "The filename does not Match PROGRAM-ID which can make code more difficult to read and maintain.", FileName, CodeIssue.LOW, CodeLine)
            End If
        ElseIf ctCodeTracker.ProgramId <> "" And Regex.IsMatch(CodeLine, "\bPROGRAM-ID\b") Then
            ' Report any instance of multiple IDs
            frmMain.ListCodeIssue("Multiple Use of PROGRAM-ID", "The code has more than one PROGRAM-ID which can make code more difficult to read and maintain (Original ID:" & ctCodeTracker.ProgramId & ").", FileName, CodeIssue.LOW, CodeLine)
        End If

    End Sub

    Private Sub TrackVarAssignments(ByVal CodeLine As String, ByVal FileName As String)
        ' Track the input and allocation of user-supplied variables
        '==========================================================
        Dim strVar As String = ""
        Dim strCalc As String = ""
        Dim strVarCollections As String = ""

        Dim strAssignments As String()
        Dim strFragments As String()


        '== Keep track of int/short/long variables and constants to help with detection of buffer overflows, etc. ==
        If Regex.IsMatch(CodeLine, "\w+\s+\bPIC\b") Then

            '== Check for numeric variable ==

            '== Check for fixed length buffers ==
            If Regex.IsMatch(CodeLine, "\w+\s+\bPIC\b\s*\([A,9]*\)") Then

            End If

        ElseIf Regex.IsMatch(CodeLine, "\bACCEPT\b\s+\w+") Then

            '== Track user-controlled variables ==
            strVar = GetLastItem(CodeLine).TrimEnd(".")
            If Regex.IsMatch(strVar, "^[a-zA-Z0-9_]*$") And Not ctCodeTracker.InputVars.Contains(strVar) Then ctCodeTracker.InputVars.Add(strVar)

        ElseIf Regex.IsMatch(CodeLine, "\bCOMPUTE\b\s+\w+\s*=\s*\w+") Then

            ' If a new variable is allocated then check if it is derived from any user-supplied variables
            strAssignments = Regex.Split(CodeLine, "\bCOMPUTE\b\s+\w+\s*=")
            strVarCollections = strAssignments.Last.Trim

            If strVarCollections = "" Then Exit Sub

            ' Cycle though input vars to see if they're on the right side of the = sign
            For Each strItem In ctCodeTracker.InputVars
                If Regex.IsMatch(strVarCollections, "\b" & strItem & "\b") Then
                    ' If we have an input var then extract the left of the = sign
                    strFragments = Regex.Split(CodeLine, "=")
                    strVar = GetLastItem(strFragments.First)
                    strCalc = strFragments.Last.Trim()
                    If strCalc.Contains(strItem) Then
                        If Regex.IsMatch(strVar, "^[a-zA-Z0-9_]*$") And Not ctCodeTracker.InputVars.Contains(strVar) Then ctCodeTracker.InputVars.Add(strVar)
                    End If
                    Exit For
                End If
            Next

        End If

    End Sub

    Private Sub CheckBuffer(ByVal CodeLine As String, ByVal FileName As String)
        ' Keep record of pointer/buffer assignments and add to the CodeTracker dictionary for checking
        ' Check any pointer and buffer manipulation fo overflows, out-of-bounds reads, etc.
        '=============================================================================================

        If Regex.IsMatch(CodeLine, "\bPOINTER\b") Then

        ElseIf Regex.IsMatch(CodeLine, "\bSET\b") Then

        ElseIf Regex.IsMatch(CodeLine, "\bMOVE\b\s+\w+\s+\bTO\b\s+\w+") Then

        ElseIf Regex.IsMatch(CodeLine, "\bMOVE\b\s+\w+\s*\(\s*\w+\s*\:\s*\w+\s*\)\s*\bTO\b\s+\w+") Then

        ElseIf Regex.IsMatch(CodeLine, "\bMOVE\b\s+\w+\s*\bTO\b\s+\w+\s*\(\s*\w+\s*\:\s*\w+\s*\)") Then

        ElseIf Regex.IsMatch(CodeLine, "\bMOVE\b\s+('|"")\w+('|"")\s*\bTO\b\s+\w+") Then

        End If

    End Sub

    Private Sub CheckSigned(ByVal CodeLine As String, ByVal FileName As String)
        ' Keep record of unsigned int assignments and add to CodeTracker dictionary
        ' Identify any signed/unsigned comparisons
        '==========================================================================

        '== Identify any unsigned integers ==
        If Regex.IsMatch(CodeLine, "\bUNSIGNED\b") Then
            ctCodeTracker.AddUnsigned(CodeLine)
        End If

        '== Check for signed/unsigned integer comparisons ==
        If (CodeLine.Contains("=") Or CodeLine.Contains("<") Or CodeLine.Contains(">")) Then
            If ctCodeTracker.CheckSignedComp(CodeLine) Then frmMain.ListCodeIssue("Signed/Unsigned Comparison", "The code appears to compare a signed numeric value with an unsigned numeric value. This behaviour can return unexpected results as negative numbers will be forcibly cast to large positive numbers.", FileName, CodeIssue.HIGH, CodeLine)
        End If

    End Sub

    Public Sub AddUnsigned(ByVal CodeLine As String)
        ' Take the variable name and its details and add them to the list
        '================================================================
        Dim strDescription As String = ""
        Dim strVarType As String = "unsigned"

    End Sub

    Private Sub CheckInputValidation(ByVal CodeLine As String, ByVal FileName As String)

    End Sub

    Private Sub CheckFileAccess(ByVal CodeLine As String, ByVal FileName As String)
        ' Check whether user input is being used to open files
        '=====================================================
        Dim strVar As String = ""

        If Regex.IsMatch(CodeLine, "\bOPEN\b\s+\w+") Then
            ' Cycle though input vars to see if they're on the right side of the = sign
            For Each strItem In ctCodeTracker.InputVars
                If Regex.IsMatch(CodeLine, "\bOPEN\b\s+\w*" & strItem) Then
                    frmMain.ListCodeIssue("User Controlled File/Directory Name", "The code uses a user-controlled value when opening a file/directory. Manually inspect the code to ensure safe usage.", FileName, CodeIssue.LOW, CodeLine)
                    Exit For
                End If
            Next
        End If

    End Sub

    Private Sub CheckLogDisplay(ByVal CodeLine As String, ByVal FileName As String)
        ' Check output written to logs is sanitised first and check for logged passwords
        '===============================================================================
        Dim strLogCodeLine As String = CodeLine

        strLogCodeLine = strLogCodeLine.ToLower

        If Regex.IsMatch(strLogCodeLine, "validate|encode|sanitize|sanitise") And Not strLogCodeLine.Contains("password") Then Exit Sub

        If Regex.IsMatch(CodeLine, "logerror|logger|logging|\blog\b") And CodeLine.Contains("password") Then
            If (InStr(strLogCodeLine, "log") < InStr(strLogCodeLine, "password")) Then frmMain.ListCodeIssue("Application Appears to Log User Passwords", "The application appears to write user passwords to logfiles creating a risk of credential theft.", FileName, CodeIssue.HIGH, CodeLine)
        ElseIf Regex.IsMatch(strLogCodeLine, "logerror|logger|logging|\blog\b") Then
            For Each strVar In ctCodeTracker.InputVars
                If CodeLine.Contains(strVar) Then
                    frmMain.ListCodeIssue("Unsanitized Data Written to Logs", "The application appears to write unsanitized data to its logfiles. If logs are viewed by a browser-based application this exposes risk of XSS attacks.", FileName, CodeIssue.MEDIUM, CodeLine)
                    Exit For
                End If
            Next
        End If

    End Sub

    Private Sub CheckFileRace(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for potential TOCTOU/race conditions
        '===========================================

        Dim intSeverity As Integer = 0  ' For TOCTOU vulns, severity will be modified according to length of time between check and usage.


        '== Check for TOCTOU (Time Of Check, Time Of Use) vulnerabilities==
        If (Not ctCodeTracker.IsLstat) And (Regex.IsMatch(CodeLine, "\bCALL\b\s+'CBL_CHECK_FILE_EXIST'")) Then
            ' Check has taken place - begin monitoring for use of the file/dir
            ctCodeTracker.IsLstat = True
        ElseIf ctCodeTracker.IsLstat Then
            ' Increase line count while monitoring
            If CodeLine.Trim <> "" Then
                ctCodeTracker.TocTouLineCount += 1
            End If

            If ctCodeTracker.TocTouLineCount < 2 And Regex.IsMatch(CodeLine, "\bOPEN\b") Then
                ' Usage takes place almost immediately so no problem
                ctCodeTracker.IsLstat = False
            ElseIf ctCodeTracker.TocTouLineCount > 1 And Regex.IsMatch(CodeLine, "\bOPEN\b") Then
                ' Usage takes place sometime later. Set severity accordingly and notify user
                ctCodeTracker.IsLstat = False
                If ctCodeTracker.TocTouLineCount > 5 Then intSeverity = 2
                frmMain.ListCodeIssue("Potential TOCTOU (Time Of Check, Time Of Use) Vulnerability", "The check for the file's existence occurs " & ctCodeTracker.TocTouLineCount & " lines before the file/directory is accessed. The longer the time between the check and the OPEN call, the greater the likelihood that the check will no longer be valid.", FileName)
            End If
        End If

    End Sub

    Private Sub CheckRandomisation(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for any random functions that are not cryptographically secure
        '=====================================================================

        '== Check for unsafe functions ==
        If Regex.IsMatch(CodeLine, "\bRANDOM\b") Then
            frmMain.ListCodeIssue("Use of Deterministic Pseudo-Random Values", "The code appears to use the RANDOM function. The resulting values, while appearing random to a casual observer, are predictable and may be enumerated by a skilled and determined attacker.", FileName, CodeIssue.STANDARD, CodeLine)
        End If

    End Sub

    Private Sub CheckUnsafeTempFiles(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for attempts to open temp files with obvious names
        '=========================================================

        '== Check for unsafe functions ==
        If Regex.IsMatch(CodeLine, "\bOPEN\b\s+\w+\s+\S*temp") Then
            frmMain.ListCodeIssue("Unsafe Temporary File Allocation", "The application appears to create a temporary file with a static, hard-coded name. This causes security issues in the form of a classic race condition (an attacker or other application creates a file with the same name between the application's creation and attempted usage).", FileName, CodeIssue.MEDIUM, CodeLine)
        End If

    End Sub

    Private Sub CheckDynamicCall(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for dynamic function calls
        '=================================
        Dim blnIsFound As Boolean = False


        '== Check for sanitisation of variables ==
        'If Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise") Then Exit Sub

        '== Check for function calls ==
        If Regex.IsMatch(CodeLine, ".\bCALL\b\s+('|"")\w+('|"")\s+\bUSING\b") Then

            ' If it's a static call check for user-supplied arguments
            For Each strVar In ctCodeTracker.InputVars
                If CodeLine.Contains(strVar) Then
                    frmMain.ListCodeIssue("User Controlled Variable Used as Parameter for Application Call", "The code appears to allow the use of an unvalidated user-controlled variable when executing an application call: " & strVar & ". Manually check to ensure the parameter is used safely.", FileName, CodeIssue.LOW, CodeLine)
                End If
            Next

        ElseIf Regex.IsMatch(CodeLine, ".\bCALL\b\s+\w+\s+\bUSING\b") Then

            ' If it's a dynamic function call check for user controlled function name
            For Each strVar In ctCodeTracker.InputVars
                If Regex.IsMatch(CodeLine, ".\bCALL\b\s+" & strVar & "\s+\bUSING\b") Then
                    frmMain.ListCodeIssue("User Controlled Variable Used for Dynamic Function Call", "The code appears to allow the use of an unvalidated user-controlled variable when executing a dynamic application call.", FileName, CodeIssue.HIGH, CodeLine)
                    blnIsFound = True
                    Exit For
                End If
            Next
            If blnIsFound = False Then
                frmMain.ListCodeIssue("Dynamic Function Call", "The code appears to allow the use of an unvalidated variable when executing a dynamic application call. Carry out a manual check to determine whether the variable is user-controlled.", FileName, CodeIssue.MEDIUM, CodeLine)
            End If
        End If
    End Sub

End Module
