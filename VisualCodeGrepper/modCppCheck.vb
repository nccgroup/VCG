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

Module modCppCheck

    ' Specific checks for C++ code
    '=============================

    Public Sub CheckCPPCode(ByVal CodeLine As String, ByVal FileName As String)
        ' Carry out any specific checks for the language in question
        '
        ' else-ifs have been avoided throughout due to C-programmers' tendency to efficiently   
        ' cram multiple functions into one line in one way or another.
        '====================================================================================

        TrackVarAssignments(CodeLine, FileName)     ' Check for matching new/delete, etc.
        TrackUserVarAssignments(CodeLine, FileName) ' Track any variables which are passed in on the command line, from files, etc.
        CheckBuffer(CodeLine, FileName)             ' Track buffer sizes and check for overflows
        CheckDestructorThrow(CodeLine, FileName)    ' Identify entry to class destructor, report any exception throw within destructor
        CheckRace(CodeLine, FileName)               ' Check for race conditions and TOCTOU vulns
        CheckPrintF(CodeLine, FileName)             ' Check for printf format string vulnerabilities
        CheckUnsafeTempFiles(CodeLine, FileName)    ' Check for static/obvious filenames for temp files
        CheckReallocFailure(CodeLine, FileName)     ' Check for 'free' on failure
        CheckUnsafeSafe(CodeLine, FileName)         ' Check unsafe use of return values from 'safe' functions
        CheckCmdInjection(CodeLine, FileName)       ' Check for potential command injection

        '== Beta functionality ==
        If asAppSettings.IncludeSigned Then
            CheckSigned(CodeLine, FileName)         ' Check for signed/unsigned integer comparisons
        End If

    End Sub

    Private Sub TrackVarAssignments(ByVal CodeLine As String, ByVal FileName As String)
        ' Keep record of malloc/new and track matching free and delete
        ' Mismatches and potential errors will be added to the CodeTracker dictionary
        '============================================================================

        '== Track 'malloc', 'new', etc. ==
        If (CodeLine.Contains("malloc ") Or CodeLine.Contains("malloc(")) Then
            ctCodeTracker.AddMalloc(CodeLine, FileName)

            '== Check for a 'fixed' malloc using numeric value instead of data type ==
            If Regex.IsMatch(CodeLine, "\b(malloc|xmalloc)\b\s*\(\s*\d+\s*\)") Then
                frmMain.ListCodeIssue("malloc( ) Using Fixed Value Instead of Variable Type Size", "The code uses a fixed value for malloc instead of the variable type size which is dependent on the platform (e.g. sizeof(int) instead of '4'). This can result in too much or too little memory being assigned with unpredicatble results such as performance impact, overflows or memory corruption.", FileName, CodeIssue.MEDIUM, CodeLine)
            End If
        End If

        If (CodeLine.Contains("new ") Or CodeLine.Contains("new(")) Then ctCodeTracker.AddNew(CodeLine, FileName)

        '== Check for matching 'free', 'delete', etc. ==
        If (CodeLine.Contains("free ") Or CodeLine.Contains("free(")) Then ctCodeTracker.AddFree(CodeLine, FileName)
        If (CodeLine.Contains("delete ") Or CodeLine.Contains("delete(")) Then ctCodeTracker.AddDelete(CodeLine, FileName)

    End Sub

    Private Sub TrackUserVarAssignments(ByVal CodeLine As String, ByVal FileName As String)
        ' Keep record of user-controlled variables
        '=========================================
        Dim arrFragments As String()
        Dim strLeft As String = ""


        '== Track assignments from argv, system variables, ini files or registry ==
        If (Regex.IsMatch(CodeLine, "\w+\s*\=\s*\bargv\b\s*\[")) Or (Regex.IsMatch(CodeLine, "\w+\s*\=\s*\b(getenv|GetPrivateProfileString|GetPrivateProfileInt)\b\s*\(")) Or (Regex.IsMatch(CodeLine, "\w+\s*\=\s*Registry\:\:\w+\-\>OpenSubKey")) Then
            ' Extract the variable name
            arrFragments = CodeLine.Split("=")
            strLeft = GetLastItem(arrFragments.First)
        End If

        '== Store any discovered variables
        If strLeft <> "" Then
            If Not ctCodeTracker.UserVariables.Contains(strLeft) Then
                ctCodeTracker.UserVariables.Add(strLeft)
            End If
        End If

    End Sub

    Private Sub CheckBuffer(ByVal CodeLine As String, ByVal FileName As String)
        ' Keep record of integer assignments and char arrays
        ' Add to the CodeTracker dictionary for checking
        '===================================================
        Dim arrFragments As String()
        Dim strLeft As String = ""


        '== Keep track of int/short/long variables and constants to help with detection of buffer overflows, etc. ==
        '== Check for assignment and check it's not an array ==
        If CodeLine.Contains("=") And Not (CodeLine.Contains("==") Or CodeLine.Contains("*") Or CodeLine.Contains("[")) And _
                                           (Regex.IsMatch(CodeLine, "\b(short|int|long|uint16|uint32|size_t|UINT|INT|LONG)\b")) Then
            ctCodeTracker.AddInteger(CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "\s*\w+\s*\=") And Not CodeLine.Contains("==") Then
            arrFragments = CodeLine.Split("=")
            strLeft = GetLastItem(arrFragments.First)
            'For Each itmItem In ctCodeTracker.GetIntegers
            If ctCodeTracker.GetIntegers.ContainsKey(strLeft) Then
                ctCodeTracker.AddInteger(CodeLine)
                'Exit For
            End If
            'Next
        End If

        '== Track fixed buffers, char pointers, etc. to check for overflows (avoid recording any arrays of pointers) ==
        If Regex.IsMatch(CodeLine, "\b(char|TCHAR|BYTE)\b") And CodeLine.Contains("[") And CodeLine.Contains("]") Then ctCodeTracker.AddBuffer(CodeLine)
        If Regex.IsMatch(CodeLine, "\b(char|TCHAR|BYTE)\b") And CodeLine.Contains("*") Then ctCodeTracker.AddCharStar(CodeLine)

        '== Check strcpy for potential buffer overflows, using buffer list ==
        'If CodeLine.Contains("strcpy") Or CodeLine.Contains("strcat") Or CodeLine.Contains("strncpy") Or CodeLine.Contains("strncat") Or CodeLine.Contains("sprintf") Or CodeLine.Contains("memcpy") Or CodeLine.Contains("memmove") Then ctCodeTracker.CheckOverflow(CodeLine, FileName)
        If Regex.IsMatch(CodeLine, "\b(strcpy|strlcpy|strcat|strlcat|strncpy|strncat|sprintf|memcpy|memmove)\b") Then ctCodeTracker.CheckOverflow(CodeLine, FileName)

    End Sub

    Private Sub CheckSigned(ByVal CodeLine As String, ByVal FileName As String)
        ' Keep record of unsigned int assignments and add to CodeTracker dictionary
        ' Identify any signed/unsigned comparisons
        '==========================================================================

        '== Identify any unsigned integers ==
        If Regex.IsMatch(CodeLine, "\b(unsigned|UNSIGNED|size_t|uint16|uint32)\b") Then
            ctCodeTracker.AddUnsigned(CodeLine)
        End If

        '== Check for signed/unsigned integer comparisons ==
        If (CodeLine.Contains("==") Or CodeLine.Contains("!=") Or CodeLine.Contains("<") Or CodeLine.Contains(">")) _
            And Not (CodeLine.Contains("->") Or CodeLine.Contains(">>") Or CodeLine.Contains("<<") Or CodeLine.Contains("<>")) And Not Regex.IsMatch(CodeLine, "\<\s*\w+\s*\>") And Not Regex.IsMatch(CodeLine, "\<\s*\w+\s*\w+\s*\>") Then
            If ctCodeTracker.CheckSignedComp(CodeLine) Then frmMain.ListCodeIssue("Signed/Unsigned Comparison", "The code appears to compare a signed numeric value with an unsigned numeric value. This behaviour can return unexpected results as negative numbers will be forcibly cast to large positive numbers.", FileName, CodeIssue.HIGH, CodeLine)
        End If

    End Sub

    Private Sub CheckUnsafeSafe(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for use of dubious return values from'safe' functions 
        '============================================================

        '== Identify any returned values being assigned to variables ==
        If Regex.IsMatch(CodeLine, "w+\s*\=\s*\b(snprintf|strlcpy|strlcat|strlprintf|std_strlcpy|std_strlcat|std_strlprintf)\b") Then
            frmMain.ListCodeIssue("Potential Misuse of Safe Function", "The code appears to assign the return value of a 'safe' function to a variable. This value represents the amount of bytes that the function attempted to write, not the amount actually written. Any use of this value for pointer arithmetic similar operations may result in memory corruption", FileName, CodeIssue.HIGH, CodeLine)
        End If

    End Sub

    Private Sub CheckDestructorThrow(ByVal CodeLine As String, ByVal FileName As String)
        ' Identify entry and exit points of destructor in CodeTracker
        ' Report any exception throw within destructor
        '============================================================
        Dim blnHasCheckedBraces As Boolean = False

        '== Check for entry to/exit from destructor ==
        If ctCodeTracker.IsDestructor = False And ((CodeLine.Contains("::~") Or CodeLine.Contains(":: ~") Or CodeLine.Contains(" ~")) And Not CodeLine.Contains(";")) Then
            ctCodeTracker.DestructorBraces = 0
            If CodeLine.Contains("{") Then
                ctCodeTracker.IsDestructor = ctCodeTracker.TrackBraces(CodeLine, ctCodeTracker.DestructorBraces)
                blnHasCheckedBraces = True
            Else
                ctCodeTracker.IsDestructor = True
            End If
        End If

        '== Check for any exceptions while in destructor ==
        If ctCodeTracker.IsDestructor = True Then
            If (CodeLine.Contains("throw") And ctCodeTracker.DestructorBraces > 0) Then
                frmMain.ListCodeIssue("Exception Throw in Destructor", "Throwing an exception causes an exit from the function and should not be carried out in a class destructor as it prevents memory from being safely deallocated. If the destructor is being called due to an exception thrown elsewhere in the application this will result in unexpected termination of the application with possible loss or corruption of data.", FileName)
            End If
            If Not blnHasCheckedBraces Then ctCodeTracker.IsDestructor = ctCodeTracker.TrackBraces(CodeLine, ctCodeTracker.DestructorBraces)
        End If

    End Sub

    Private Sub CheckRace(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for potential TOCTOU/race conditions
        '===========================================

        Dim intSeverity As Integer = 0  ' For TOCTOU vulns, severity will be modified according to length of time between check and usage.


        '== Check for TOCTOU (Time Of Check, Time Of Use) vulnerabilities==
        If (Not ctCodeTracker.IsLstat) And (CodeLine.Contains(" lstat(") Or CodeLine.Contains(" lstat ") Or CodeLine.Contains(" stat(") Or CodeLine.Contains(" stat ")) And ((Not CodeLine.Contains("fopen")) And (Not CodeLine.Contains("opendir"))) Then
            ' Check has taken place - begin monitoring for use of the file/dir
            ctCodeTracker.IsLstat = True
        ElseIf ctCodeTracker.IsLstat Then
            ' Increase line count while monitoring
            ctCodeTracker.TocTouLineCount += 1
            If ctCodeTracker.TocTouLineCount < 2 And (CodeLine.Contains("fopen") Or CodeLine.Contains("opendir")) Then
                ' Usage takes place almost immediately so no problem
                ctCodeTracker.IsLstat = False
            ElseIf ctCodeTracker.TocTouLineCount > 1 And (CodeLine.Contains("fopen") Or CodeLine.Contains("opendir")) Then
                ' Usage takes place sometime later. Set severity accordingly and notify user
                ctCodeTracker.IsLstat = False
                If ctCodeTracker.TocTouLineCount > 5 Then intSeverity = 2
                frmMain.ListCodeIssue("Potential TOCTOU (Time Of Check, Time Of Use) Vulnerability", "The lstat() check occurs " & ctCodeTracker.TocTouLineCount & " lines before fopen() is called. The longer the time between the check and the fopen(), the greater the likelihood that the check will no longer be valid.", FileName)
            End If
        End If

    End Sub

    Private Sub CheckPrintF(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for printf format string vulnerabilities 
        '===============================================

        If Regex.IsMatch(CodeLine, "\bprintf\b\s*\(\s*\w+\s*\)") And Not CodeLine.Contains(",") And Not CodeLine.Contains("""") Then
            frmMain.ListCodeIssue("Possible printf( ) Format String Vulnerability", "The call to printf appears to be printing a variable directly to standard output. Check whether this variable can be controlled or altered by the user to determine whether a format string vulnerability exists.", FileName, CodeIssue.HIGH, CodeLine)
        End If

    End Sub

    Private Sub CheckUnsafeTempFiles(ByVal CodeLine As String, ByVal FileName As String)
        ' Identify any creation of temp files with static names
        '======================================================

        If Regex.IsMatch(CodeLine, "\=\s*(_open|open|fopen|opendir)\s*\(\s*\""*\S*(temp|tmp)\S*\""\s*\,\s*\S*\s*\)") Then
            frmMain.ListCodeIssue("Unsafe Temporary File Allocation", "The application appears to create a temporary file with a static, hard-coded name. This causes security issues in the form of a classic race condition (an attacker creates a file with the same name between the application's creation and attempted usage) or a symbolic link attack where an attacker creates a symbolic link at the temporary file location.", FileName, CodeIssue.MEDIUM, CodeLine)
        End If

    End Sub

    Private Sub CheckReallocFailure(ByVal CodeLine As String, ByVal FileName As String)
        ' Identify any attempts to resize buffers that do not clear the buffer on failure
        '================================================================================
        Dim arrFragments As String()
        Dim strBuffer As String = ""
        Dim strDestination As String = ""


        '== Identify occurences of realloc ==
        If Regex.IsMatch(CodeLine, "\brealloc\b\s*\(") Then
            '== Extract variable names ==
            arrFragments = Regex.Split(CodeLine, "\=\s*\brealloc\b\s*\(")
            If arrFragments.Count < 2 Then Exit Sub

            '== Make sure we have the variable name and nothing else ==
            If arrFragments.First.Contains("(") Then
                strDestination = GetLastItem(arrFragments.First, "(")
                strDestination = GetLastItem(strDestination)
            Else
                strDestination = GetLastItem(arrFragments.First)
            End If

            If strDestination <> "" Then
                strBuffer = GetFirstItem(arrFragments(1), ",")
                If strDestination = strBuffer Then
                    frmMain.ListCodeIssue("Dangerous Use of realloc( )", "The source and destination buffers are the same. A failure to resize the buffer will set the pointer to NULL, possibly causing unpredicatable behaviour.", FileName, CodeIssue.MEDIUM, CodeLine)
                End If
                strDestination = strDestination.TrimStart("*").TrimStart()
                strBuffer = strBuffer.TrimStart("*").TrimStart()

                ctCodeTracker.DestinationBuffer = strDestination
                ctCodeTracker.SourceBuffer = strBuffer
            End If

        ElseIf ctCodeTracker.DestinationBuffer <> "" Then

            If Regex.IsMatch(ctCodeTracker.DestinationBuffer, "(\(|\)|\[|\])") Or Regex.IsMatch(ctCodeTracker.SourceBuffer, "(\(|\)|\[|\])") Then
                ctCodeTracker.DestinationBuffer = ""
                ctCodeTracker.SourceBuffer = ""
                Exit Sub
            End If
            If Regex.IsMatch(CodeLine, "\bfree\b\s*\(\s*(" & ctCodeTracker.DestinationBuffer & "|" & ctCodeTracker.SourceBuffer & ")") Then
                ctCodeTracker.DestinationBuffer = ""
                ctCodeTracker.SourceBuffer = ""
            ElseIf Regex.IsMatch(CodeLine, "(break|return|exit)") Then
                frmMain.ListCodeIssue("Potential Memory Leak", "On failure, the realloc function returns a NULL pointer but leaves memory allocated. The code should be modified to free the memory if NULL is returned.", FileName, CodeIssue.MEDIUM, CodeLine)
                ctCodeTracker.DestinationBuffer = ""
                ctCodeTracker.SourceBuffer = ""
            End If

        End If

    End Sub

    Private Sub CheckCmdInjection(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for potential command injection
        '======================================
        Dim blnIsFound As Boolean = False


        '== Are commands being passed to system? ==
        If Regex.IsMatch(CodeLine, "\b(system|popen|execlp)\b\s*\(") Then

            '== Is a user-controlled variable present? ==
            For Each strVar In ctCodeTracker.UserVariables
                If CodeLine.Contains(strVar) Then
                    frmMain.ListCodeIssue("User Controlled Variable Used on System Command Line", "The application appears to allow the use of an unvalidated user-controlled variable [" + strVar + "] when executing a system command.", FileName, CodeIssue.HIGH, CodeLine)
                    blnIsFound = True
                    Exit For
                End If
            Next
            If blnIsFound = False And (Regex.IsMatch(CodeLine, "\b(system|popen|execlp)\b\s*\(\s*\bgetenv\b")) Then
                '== Is a system variable present? ==
                frmMain.ListCodeIssue("Application Variable Used on System Command Line", "The application appears to allow the use of an unvalidated system variable when executing a system command.", FileName, CodeIssue.HIGH, CodeLine)
            ElseIf blnIsFound = False And ((Not CodeLine.Contains("""")) Or (CodeLine.Contains("""") And CodeLine.Contains("+")) Or (Regex.IsMatch(CodeLine, "\b(system|popen|execlp)\b\s*\(\s*\b(strcat|strncat)\b"))) Then
                '== Is an unidentified variable present? ==
                frmMain.ListCodeIssue("Application Variable Used on System Command Line", "The application appears to allow the use of an unvalidated variable when executing a system command. Carry out a manual check to determine whether the variable is user-controlled.", FileName, CodeIssue.MEDIUM, CodeLine)
            End If
        End If

    End Sub

End Module
