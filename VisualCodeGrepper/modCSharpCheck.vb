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

Module modCSharpCheck

    ' Specific checks for C# code
    '============================

    Public Sub CheckCSharpCode(ByVal CodeLine As String, ByVal FileName As String)
        ' Carry out any specific checks for the language in question
        '===========================================================

        IdentifyLabels(CodeLine, FileName)
        CheckInputValidation(CodeLine, FileName)        ' Has .NET default validation been turned off?
        CheckSQLInjection(CodeLine, FileName)           ' Check for SQLi
        CheckXSS(CodeLine, FileName)                    ' Check for XSS
        CheckSecureStorage(CodeLine, FileName)          ' Are sensitive variables stored without using SecureString?
        CheckIntOverflow(CodeLine, FileName)            ' Are int overflows being trapped?
        CheckLogDisplay(CodeLine, FileName)             ' Is data sanitised before being written to logs?
        CheckFileRace(CodeLine, FileName)               ' Check for race conditions and TOCTOU vulns
        CheckSerialization(CodeLine, FileName)          ' Identify serializable objects and check their security permissions
        CheckHTTPRedirect(CodeLine, FileName)           ' Check for safe redirects and safe use of URLs
        CheckRandomisation(CodeLine, FileName)          ' Locate any use of randomisation functions that are not cryptographically secure
        CheckSAML2Validation(CodeLine, FileName)        ' Check for correct implementation of inherited SAML2 functions
        CheckUnsafeTempFiles(CodeLine, FileName)        ' Check for static/obvious filenames for temp files
        CheckUnsafeCode(CodeLine, FileName)             ' Check for use and abuse of the "unsafe" directive
        CheckThreadIssues(CodeLine, FileName)           ' Check for good/bad thread management
        CheckExecutable(CodeLine, FileName)             ' Check for unvalidated variables being executed via cmd line/system calls
        CheckWebConfig(CodeLine, FileName)              ' Check config file to determine whether .NET debugging and default errors are enabled

        If Regex.IsMatch(CodeLine, "\S*(Password|password|pwd|passwd)\S*(\.|\-\>)(ToLower|ToUpper)\s*\(") Then
            frmMain.ListCodeIssue("Unsafe Password Management", "The application appears to handle passwords in a case-insensitive manner. This can greatly increase the likelihood of successful brute-force and/or dictionary attacks.", FileName, CodeIssue.MEDIUM, CodeLine)
        End If

    End Sub

    Public Sub IdentifyLabels(ByVal CodeLine As String, ByVal FileName As String)
        ' Locate and record any labels in asp pages. These will be checked for XSS later.
        '================================================================================
        Dim arrFragments As String()
        Dim strLabel As String = ""

        '== Detect default .net input validation
        If ctCodeTracker.HasValidator = False And (FileName.ToLower.EndsWith(".asp") Or FileName.ToLower.EndsWith(".aspx")) And CodeLine.Contains("<asp:Label ID=""") Then
            arrFragments = Regex.Split(CodeLine, "\<asp\:Label\s+ID=""")
            strLabel = GetFirstItem(arrFragments.Last, """")
            If strLabel <> "" And Not ctCodeTracker.AspLabels.Contains(strLabel) Then ctCodeTracker.AspLabels.Add(strLabel)
        End If

    End Sub

    Public Sub CheckInputValidation(ByVal CodeLine As String, ByVal FileName As String)
        ' Check any input validation of user-controlled variables (or lack of)
        '=====================================================================

        '== Detect default .net input validation
        If ctCodeTracker.HasValidator = False And FileName.ToLower.EndsWith(".config") And CodeLine.ToLower.Contains("<pages validateRequest=""true""") Then
            ctCodeTracker.HasValidator = True
        ElseIf ctCodeTracker.HasValidator = False And FileName.ToLower.EndsWith(".xml") And CodeLine.ToLower.Contains("<pages> element with validateRequest=""true""") Then
            ctCodeTracker.HasValidator = True
        ElseIf FileName.ToLower.EndsWith(".config") And CodeLine.ToLower.Contains("<pages validateRequest=""false""") Then
            '== .NET validation turned off deliberately ==
            ctCodeTracker.HasValidator = False
            frmMain.ListCodeIssue("Potential Input Validation Issues", "The application appears to deliberately de-activate the default .NET input validation functionality.", FileName, CodeIssue.HIGH, CodeLine)
        ElseIf FileName.ToLower.EndsWith(".xml") And CodeLine.ToLower.Contains("<pages> element with validateRequest=""false""") Then
            '== .NET validation turned off deliberately ==
            ctCodeTracker.HasValidator = False
            frmMain.ListCodeIssue("Potential Input Validation Issues", "The application appears to deliberately de-activate the default .NET input validation functionality.", FileName, CodeIssue.HIGH, CodeLine)
        End If

    End Sub

    Public Sub CheckSQLInjection(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for any SQL injection problems 
        '=====================================
        Dim strVarName As String = ""   ' Holds the variable name for the dynamic SQL statement


        '== Only check unvalidated code ==
        If ctCodeTracker.HasValidator = True Then Exit Sub


        '== Is unsanitised dynamic SQL statement prepared beforehand? ==
        If CodeLine.Contains("=") And (CodeLine.ToLower.Contains("sql") Or CodeLine.ToLower.Contains("query")) And (CodeLine.Contains("""") And (CodeLine.Contains("&") Or CodeLine.Contains("+"))) Then
            '== Extract variable name from assignment statement ==
            strVarName = GetVarName(CodeLine)
            ctCodeTracker.HasVulnSQLString = True
            If Regex.IsMatch(strVarName, "^[a-zA-Z0-9_]*$") And Not ctCodeTracker.SQLStatements.Contains(strVarName) Then ctCodeTracker.SQLStatements.Add(strVarName)
        End If


        If Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise") Then
            '== Remove any variables which have been sanitised from the list of vulnerable variables ==  
            RemoveSanitisedVars(CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "ExecuteQuery|ExecuteSQL|ExecuteStatement|SqlCommand\(") Then

            '== Check usage of java.sql.Statement.executeQuery, etc. ==
            If CodeLine.Contains("""") And CodeLine.Contains("&") Then
                '== Dynamic SQL built into connection/update ==
                frmMain.ListCodeIssue("Potential SQL Injection", "The application appears to allow SQL injection via dynamic SQL statements.", FileName, CodeIssue.CRITICAL, CodeLine)
            ElseIf ctCodeTracker.HasVulnSQLString = True Then
                '== Otherwise check for use of pre-prepared statements ==
                For Each strVar In ctCodeTracker.SQLStatements
                    If CodeLine.Contains(strVar) Then
                        frmMain.ListCodeIssue("Potential SQL Injection", "The application appears to allow SQL injection via a pre-prepared dynamic SQL statement.", FileName, CodeIssue.CRITICAL, CodeLine)
                        Exit For
                    End If
                Next
            End If
        End If

    End Sub

    Public Sub CheckXSS(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for any XSS problems 
        '===========================
        Dim strVarName As String = ""
        Dim arrFragments As String()
        Dim blnIsFound As Boolean = False
        '== Only check unvalidated code ==
        If ctCodeTracker.HasValidator = True Then Exit Sub


        If Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise") Then
            '== Remove any variables which have been sanitised from the list of vulnerable variables ==  
            RemoveSanitisedVars(CodeLine)
            Exit Sub
        ElseIf Regex.IsMatch(CodeLine, "\bHttpCookie\b\s+\S+\s+=\s+\S+\.Cookies\.Get\(") Then
            '== Extract variable name from assignment statement ==
            strVarName = GetVarName(CodeLine)
            If Regex.IsMatch(strVarName, "^[a-zA-Z0-9_]*$") And Not ctCodeTracker.InputVars.Contains(strVarName) Then ctCodeTracker.InputVars.Add(strVarName)
        ElseIf Regex.IsMatch(CodeLine, "\bRequest\b\.Form\(""") Then
            '== Extract variable name from assignment statement ==
            arrFragments = Regex.Split(CodeLine, "\bRequest\b\.Form\(""")
            strVarName = GetFirstItem(arrFragments.First, """")
            If Regex.IsMatch(strVarName, "^[a-zA-Z0-9_]*$") And Not ctCodeTracker.InputVars.Contains(strVarName) Then ctCodeTracker.InputVars.Add(strVarName)
        ElseIf (CodeLine.Contains("=") And (CodeLine.Contains(".Value")) Or Regex.IsMatch(CodeLine, "=\s*Request\.QueryString\[")) Then
            '== Extract variable name from assignment statement ==
            strVarName = GetVarName(CodeLine)
            If Regex.IsMatch(strVarName, "^[a-zA-Z0-9_]*$") And Not ctCodeTracker.InputVars.Contains(strVarName) Then ctCodeTracker.InputVars.Add(strVarName)
        End If

        If CodeLine.Contains("Response.Write(") And CodeLine.Contains("Request.Form(") Then
            '== Classic ASP XSS==
            frmMain.ListCodeIssue("Potential XSS", "The application appears to reflect user input to the screen with no apparent validation or sanitisation.", FileName, CodeIssue.HIGH, CodeLine)
        ElseIf CodeLine.Contains("Response.Write(") And CodeLine.Contains("""") And CodeLine.Contains("+") Then
            CheckUserVarXSS(CodeLine, FileName)
        ElseIf CodeLine.Contains("Response.Write(") And Not CodeLine.Contains("""") Then
            CheckUserVarXSS(CodeLine, FileName)
        ElseIf CodeLine.Contains(".Text =") Then
            For Each strLabel In ctCodeTracker.AspLabels
                If CodeLine.Contains(strLabel) Then
                    If CodeLine.Contains("Request.QueryString[") Or CodeLine.Contains(".Cookies.Get(") Then
                        frmMain.ListCodeIssue("Potential XSS", "The application appears to reflect a user-supplied variable to the screen with no apparent validation or sanitisation.", FileName, CodeIssue.HIGH, CodeLine)
                    Else
                        CheckUserVarXSS(CodeLine, FileName)
                    End If
                End If
            Next
        End If


        '== Check for use of raw strings in HTML output ==
        If Regex.IsMatch(CodeLine, "\bHtml\b\.Raw\(") Then
            For Each strVar In ctCodeTracker.InputVars
                If CodeLine.Contains(strVar) Then
                    frmMain.ListCodeIssue("Potential XSS", "The application uses the potentially dangerous Html.Raw construct in conjunction with a user-supplied variable.", FileName, CodeIssue.HIGH, CodeLine)
                    blnIsFound = True
                    Exit For
                End If
            Next

            If Not blnIsFound Then
                frmMain.ListCodeIssue("Potential XSS", "The application uses the potentially dangerous Html.Raw construct.", FileName, CodeIssue.MEDIUM, CodeLine)
            End If
        End If


            '== Check for DOM-based XSS in .asp pages ==
            If FileName.ToLower.EndsWith(".asp") Or FileName.ToLower.EndsWith(".aspx") Then
                If Regex.IsMatch(CodeLine, "\s+var\s+\w+\s*=\s*""\s*\<\%\s*\=\s*\w+\%\>""\;") And Not Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise") Then
                    '== Extract variable name from assignment statement ==
                    strVarName = GetVarName(CodeLine)
                    If Regex.IsMatch(strVarName, "^[a-zA-Z0-9_]*$") And Not ctCodeTracker.SQLStatements.Contains(strVarName) Then ctCodeTracker.InputVars.Add(strVarName)
                ElseIf ((CodeLine.Contains("document.write(") And CodeLine.Contains("+") And CodeLine.Contains("""")) Or Regex.IsMatch(CodeLine, ".innerHTML\s*\=\s*\w+;")) And Not Regex.IsMatch(CodeLine, "\s*\S*\s*validate|encode|sanitize|sanitise\s*\S*\s*") Then
                    For Each strVar In ctCodeTracker.InputVars
                        If CodeLine.Contains(strVar) Then
                            frmMain.ListCodeIssue("Potential DOM-Based XSS", "The application appears to allow XSS via an unencoded/unsanitised input variable.", FileName, CodeIssue.HIGH, CodeLine)
                            Exit For
                        End If
                    Next
                End If
            End If

    End Sub

    Public Sub CheckUserVarXSS(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for presence of user controlled variables in a line which writes data the screen
        '=======================================================================================
        Dim blnIsFound As Boolean = False

        For Each strVar In ctCodeTracker.InputVars
            If CodeLine.Contains(strVar) Then
                frmMain.ListCodeIssue("Potential XSS", "The application appears to reflect a user-supplied variable to the screen with no apparent validation or sanitisation.", FileName, CodeIssue.HIGH, CodeLine)
                blnIsFound = True
                Exit For
            End If
        Next

        If Not blnIsFound Then
            frmMain.ListCodeIssue("Potential XSS", "The application appears to reflect data to the screen with no apparent validation or sanitisation. It was not clear if this variable is controlled by the user.", FileName, CodeIssue.MEDIUM, CodeLine)
        End If

    End Sub

    Public Sub CheckSecureStorage(ByVal CodeLine As String, ByVal FileName As String)
        ' Check if passwords are stored with char[] or String instead of SecureString
        '============================================================================

        If Regex.IsMatch(CodeLine, "\s+(String|char\[\])\s+\S*(Password|password|key)\S*") Then
            frmMain.ListCodeIssue("Insecure Storage of Sensitive Information", "The code uses standard strings and byte arrays to store sensitive transient data such as passwords and cryptographic private keys instead of the more secure SecureString class.", FileName, CodeIssue.MEDIUM, CodeLine)
        End If

    End Sub

    Public Sub CheckIntOverflow(ByVal CodeLine As String, ByVal FileName As String)
        ' Check whether precautions are in place to deal with integer overflows
        '======================================================================

        If Regex.IsMatch(CodeLine, "\bint\b\s*\w+\s*\=\s*\bchecked\b\s+\(") Then
            ' A check is in place, exit function
            Return
        ElseIf ((Regex.IsMatch(CodeLine, "\bint\b\s*\w+\s*\=\s*\bunchecked\b\s+\(")) And (CodeLine.Contains("+") Or CodeLine.Contains("*"))) Then
            ' Checks have been switched off
            frmMain.ListCodeIssue("Integer Operation With Overflow Check Deliberately Disabled", "The code carries out integer operations with a deliberate disabling of overflow defences. Manually review the code to ensure that it is safe.", FileName, CodeIssue.STANDARD, CodeLine)
        ElseIf ((Regex.IsMatch(CodeLine, "\bint\b\s*\w+\s*\=")) And (CodeLine.Contains("+") Or CodeLine.Contains("*"))) Then
            ' Unchecked operation
            frmMain.ListCodeIssue("Integer Operation Without Overflow Check", "The code carries out integer operations without enabling overflow defences. Manually review the code to ensure that it is safe", FileName, CodeIssue.STANDARD, CodeLine)
        End If

    End Sub

    Public Sub CheckExecutable(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for unvalidated variables being executed via cmd line/system calls
        '=========================================================================
        Dim blnIsFound As Boolean = False


        If Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise") Then Exit Sub

        If CodeLine.ToLower.Contains(".ProcessStartInfo(") Then
             For Each strVar In ctCodeTracker.InputVars
                If CodeLine.Contains(strVar) Then
                    frmMain.ListCodeIssue("User Controlled Variable Used on System Command Line", "The application appears to allow the use of an unvalidated user-controlled variable when executing a command.", FileName, CodeIssue.HIGH, CodeLine)
                    blnIsFound = True
                    Exit For
                End If
            Next
            If blnIsFound = False And ((Not CodeLine.Contains("""")) Or (CodeLine.Contains("""") And CodeLine.Contains("+"))) Then
                frmMain.ListCodeIssue("Application Variable Used on System Command Line", "The application appears to allow the use of an unvalidated variable when executing a command. Carry out a manual check to determine whether the variable is user-controlled.", FileName, CodeIssue.MEDIUM, CodeLine)
            End If
        End If

    End Sub

    Public Sub CheckLogDisplay(ByVal CodeLine As String, ByVal FileName As String)
        ' Check output written to logs is sanitised first
        '================================================


        '== Only check unvalidated code ==
        If ctCodeTracker.HasValidator = True And Not CodeLine.ToLower.Contains("password") Then Exit Sub

        If Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise") And Not CodeLine.ToLower.Contains("password") Then
            RemoveSanitisedVars(CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "LogError|Logger|logger|Logging|logging|System\.Diagnostics\.Debug|System\.Diagnostics\.Trace") And CodeLine.ToLower.Contains("password") Then
            If (InStr(CodeLine.ToLower, "log") < InStr(CodeLine.ToLower, "password")) Then frmMain.ListCodeIssue("Application Appears to Log User Passwords", "The application appears to write user passwords to logfiles creating a risk of credential theft.", FileName, CodeIssue.HIGH, CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "LogError|Logger|logger|Logging|logging|System\.Diagnostics\.Debug|System\.Diagnostics\.Trace") Then
            For Each strVar In ctCodeTracker.InputVars
                If CodeLine.Contains(strVar) Then
                    frmMain.ListCodeIssue("Unsanitized Data Written to Logs", "The application appears to write unsanitized data to its logfiles. If logs are viewed by a browser-based application this exposes risk of XSS attacks.", FileName, CodeIssue.MEDIUM, CodeLine)
                    Exit For
                End If
            Next
        End If

    End Sub

    Public Sub CheckSerialization(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for insecure object serialization and deserialization
        '============================================================
        Dim strClassName As String = ""
        Dim arrFragments As String()


        If ctCodeTracker.IsSerializable = True Then
            '== File content is deserialized into onjects - flag this up for further investigation ==
            If Regex.IsMatch(CodeLine, "\.(Deserialize|ReadObject)\s*\(") Then
                frmMain.ListCodeIssue("Unsafe Object Deserialization", "The code allows objects to be deserialized. This can allow potentially hostile objects to be instantiated directly from data held in the filesystem.", FileName, CodeIssue.STANDARD, CodeLine)
            End If
        End If

        If ctCodeTracker.IsSerializable = False And CodeLine.Contains("using System.Runtime.Serialization") Then
            '== Serialization is implemented in the code module ==
            ctCodeTracker.IsSerializable = True
        ElseIf ctCodeTracker.IsSerializable = True And ctCodeTracker.IsSerializableClass = False And CodeLine.Contains("[Serializable") Then
            '== Serialization is implemented for the class ==
            ctCodeTracker.IsSerializableClass = True
        ElseIf ctCodeTracker.IsSerializable = True And ctCodeTracker.IsSerializableClass = False And (CodeLine.Contains("[assembly: SecurityPermission(") Or CodeLine.Contains("[SecurityPermissionAttribute(")) Then
            '== Serialization is safely implemented so discontinue the checks ==
            ctCodeTracker.IsSerializable = False
            ctCodeTracker.IsSerializableClass = False
        ElseIf ctCodeTracker.IsSerializableClass = True And CodeLine.Contains("public class ") Then
            '== Extract the vulnerable class name and write out results ==

            ' Now we have the class name this must be reset to false
            ctCodeTracker.IsSerializableClass = False

            ' Trim away any redundant text following the classname
            arrFragments = CodeLine.Split("{")
            arrFragments = arrFragments.First().Split(":")

            strClassName = GetLastItem(arrFragments.First())
            If Regex.IsMatch(strClassName, "^[a-zA-Z0-9_]*$") Then
                frmMain.ListCodeIssue("Unsafe Object Serialization", "The code allows the object " & strClassName & " to be serialized. This can allow potentially sensitive data to be saved to the filesystem.", FileName, CodeIssue.STANDARD, CodeLine)
            End If
        End If

    End Sub

    Public Sub CheckHTTPRedirect(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for safe use HTTP redirects
        '==================================
        Dim blnIsFound As Boolean = False


        '== Check for secure HTTP usage ==
        If CodeLine.Contains("Response.Redirect(") And CodeLine.Contains("HTTP:") Then
            frmMain.ListCodeIssue("URL request sent over HTTP:", "The URL used in the HTTP request appears to be unencrypted. Check the code manually to ensure that sensitive data is not being submitted.", FileName, CodeIssue.STANDARD, CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "Response\.Redirect\(") And Not Regex.IsMatch(CodeLine, "Response\.Redirect\(\s*\""\S+\""\s*\)") Then
            For Each strVar In ctCodeTracker.InputVars
                If Regex.IsMatch(CodeLine, "Response\.Redirect\(\s*" & strVar) Or Regex.IsMatch(CodeLine, "Response\.Redirect\(\s*(\""\S+\""|S+)\s*(\+|\&)\s*" & strVar) Then
                    frmMain.ListCodeIssue("URL Request Gets Path from Unvalidated Variable", "The URL used in the HTTP request is loaded from an unsanitised variable. This can allow an attacker to redirect the user to a site under the control of a third party.", FileName, CodeIssue.MEDIUM, CodeLine)
                    blnIsFound = True
                    Exit For
                End If
            Next
            If blnIsFound = False Then
                frmMain.ListCodeIssue("URL Request Gets Path from Variable", "The URL used in the HTTP request appears to be loaded from a variable. Check the code manually to ensure that malicious URLs cannot be submitted by an attacker.", FileName, CodeIssue.STANDARD, CodeLine)
            End If
        End If

    End Sub

    Private Sub CheckRandomisation(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for any random functions that are not cryptographically secure
        '=====================================================================

        '== Check for non-time-based seed ==
        If Regex.IsMatch(CodeLine, "\bRandomize\b\(\)") Or Regex.IsMatch(CodeLine, "\bRandomize\b\(\w*(T|t)ime\w*\)") Then
            ctCodeTracker.HasSeed = False
        ElseIf Regex.IsMatch(CodeLine, "\bRandomize\b\(\S+\)") Then
            ctCodeTracker.HasSeed = True
        End If

        '== Check for unsafe functions Next() or NextBytes() ==
        If Regex.IsMatch(CodeLine, "\bRandom\b\.Next(Bytes\(|\()") Then
            If ctCodeTracker.HasSeed Then
                frmMain.ListCodeIssue("Use of Deterministic Pseudo-Random Values", "The code appears to use the Next() and/or NextBytes() functions. The resulting values, while appearing random to a casual observer, are predictable and may be enumerated by a skilled and determined attacker, although this is partly mitigated by a seed that does not appear to be time-based.", FileName, CodeIssue.STANDARD, CodeLine)
            Else
                frmMain.ListCodeIssue("Use of Deterministic Pseudo-Random Values", "The code appears to use the Next() and/or NextBytes() functions without a seed to generate pseudo-random values. The resulting values, while appearing random to a casual observer, are predictable and may be enumerated by a skilled and determined attacker.", FileName, CodeIssue.MEDIUM, CodeLine)
            End If
        End If

    End Sub

    Private Sub CheckSAML2Validation(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for validation of SAML2 conditions
        '=========================================

        '== Locate entry into overridden SAML2 function ==
        If ctCodeTracker.IsSamlFunction = False And Regex.IsMatch(CodeLine, "\boverride\b\s+\bvoid\b\s+\bValidateConditions\b\(\bSaml2Conditions\b") Then
            If CodeLine.Contains("{") Then
                ctCodeTracker.IsSamlFunction = ctCodeTracker.TrackBraces(CodeLine, ctCodeTracker.ClassBraces)
            Else
                ctCodeTracker.IsSamlFunction = True
            End If
        ElseIf ctCodeTracker.IsSamlFunction = True Then
            '== Report issue if function is empty ==
            If (CodeLine.Trim <> "" And CodeLine.Trim <> "{" And CodeLine.Trim <> "}") Then
                If Regex.IsMatch(CodeLine, "\s*\S*\s*validate|encode|sanitize|sanitise\S*\(\S*\s*conditions") Then ctCodeTracker.IsSamlFunction = False
            Else
                ctCodeTracker.IsSamlFunction = ctCodeTracker.TrackBraces(CodeLine, ctCodeTracker.ClassBraces)
                If ctCodeTracker.IsSamlFunction = False Then
                    frmMain.ListCodeIssue("Insufficient SAML2 Condition Validation", "The code includes a token handling class that inherits from Saml2SecurityTokenHandler. It appears not to perform any validation on the Saml2Conditions object passed, violating its contract with the superclass and undermining authentication/authorisation conditions.", FileName, CodeIssue.MEDIUM)
                End If
            End If

        End If

    End Sub

    Private Sub CheckUnsafeTempFiles(ByVal CodeLine As String, ByVal FileName As String)
        ' Identify any creation of temp files with static names
        '======================================================

        If Regex.IsMatch(CodeLine, "\=\s*File\.Open\(\""\S*(temp|tmp)\S*\""\,") Then
            frmMain.ListCodeIssue("Unsafe Temporary File Allocation", "The application appears to create a temporary file with a static, hard-coded name. This causes security issues in the form of a classic race condition (an attacker creates a file with the same name between the application's creation and attempted usage) or a symbolic link attack where an attacker creates a symbolic link at the temporary file location.", FileName, CodeIssue.MEDIUM, CodeLine)
        End If

    End Sub

    Public Sub CheckFileRace(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for potential TOCTOU/race conditions
        '===========================================

        Dim intSeverity As Integer = 0  ' For TOCTOU vulns, severity will be modified according to length of time between check and usage.


        '== Check for TOCTOU (Time Of Check, Time Of Use) vulnerabilities==
        If (Not ctCodeTracker.IsLstat) And (Regex.IsMatch(CodeLine, "(File|Directory)\.Exists\(") And Not Regex.IsMatch(CodeLine, "Process\.Start\(|new\s+FileInfo\(|Directory\.GetFiles\(|\.FileName\;")) Then
            ' Check has taken place - begin monitoring for use of the file/dir
            ctCodeTracker.IsLstat = True
        ElseIf ctCodeTracker.IsLstat Then
            ' Increase line count while monitoring
            If CodeLine.Trim <> "" And CodeLine.Trim <> "{" And CodeLine.Trim <> "}" Then
                ctCodeTracker.TocTouLineCount += 1
            End If

            If ctCodeTracker.TocTouLineCount < 2 And Regex.IsMatch(CodeLine, "Process\.Start\(|new\s+FileInfo\(|Directory\.GetFiles\(|\.FileName\;") Then
                ' Usage takes place almost immediately so no problem
                ctCodeTracker.IsLstat = False
            ElseIf ctCodeTracker.TocTouLineCount > 1 And Regex.IsMatch(CodeLine, "Process\.Start\(|new\s+FileInfo\(|Directory\.GetFiles\(|\.FileName\;") Then
                ' Usage takes place sometime later. Set severity accordingly and notify user
                ctCodeTracker.IsLstat = False
                If ctCodeTracker.TocTouLineCount > 5 Then intSeverity = 2
                frmMain.ListCodeIssue("Potential TOCTOU (Time Of Check, Time Of Use) Vulnerability", "The .Exists() check occurs " & ctCodeTracker.TocTouLineCount & " lines before the file/directory is accessed. The longer the time between the check and the fopen(), the greater the likelihood that the check will no longer be valid.", FileName)
            End If
        End If

    End Sub

    Private Sub CheckUnsafeCode(ByVal CodeLine As String, ByVal FileName As String)
        ' Identify any unsafe code directives
        '====================================

        If ctCodeTracker.IsUnsafe = False And Regex.IsMatch(CodeLine, "\bunsafe\b") Then
            frmMain.ListCodeIssue("Unsafe Code Directive", "The uses the 'unsafe' directive which allows the use of C-style pointers in the code. This code has an increased risk of unexpected behaviour, including buffer overflows, memory leaks and crashes.", FileName, CodeIssue.MEDIUM, CodeLine)
            If CodeLine.Contains("{") Then
                ctCodeTracker.IsUnsafe = ctCodeTracker.TrackBraces(CodeLine, ctCodeTracker.UnsafeBraces)
            Else
                ctCodeTracker.IsUnsafe = True
            End If
        End If
        If ctCodeTracker.IsUnsafe = True Then
            '== Locate any fixed size buffers ==
            If Regex.IsMatch(CodeLine, "\bfixed\b\s+char\s+\w+\s*\[") Then
                ctCodeTracker.AddBuffer(CodeLine)
            ElseIf Regex.IsMatch(CodeLine, "\bfixed\b\s+byte\s+\w+\s*\[") Then
                ctCodeTracker.AddBuffer(CodeLine, "byte")
            End If
            ctCodeTracker.IsUnsafe = ctCodeTracker.TrackBraces(CodeLine, ctCodeTracker.UnsafeBraces)
        End If

    End Sub

    Private Sub CheckThreadIssues(ByVal CodeLine As String, ByVal FileName As String)
        ' Identify potential for race conditions and deadlocking
        '=======================================================
        Dim blnIsRace As Boolean = False
        Dim strSyncObject As String = ""



        '== Identify object locked for use in synchronized block ==
        If ctCodeTracker.IsSynchronized = False And Regex.IsMatch(CodeLine, "\block\b\s*\(\s*\w+\s*\)") Then
            strSyncObject = GetSyncObject(CodeLine)
            ctCodeTracker.LockedObject = strSyncObject
            ctCodeTracker.SyncIndex += 1
        End If



        '== Identify entry into a synchronized block ==
        '== The synchronized may be followed by method type and name for a synchronized method, or by braces for a synchronized block ==
        If ctCodeTracker.IsSynchronized = False And Regex.IsMatch(CodeLine, "\block\b\s*\S*\s*\S*\s*\(") Then
            If CodeLine.Contains("{") Then
                ctCodeTracker.IsSynchronized = ctCodeTracker.TrackBraces(CodeLine, ctCodeTracker.SyncBraces)
            Else
                ctCodeTracker.IsSynchronized = True
            End If

        ElseIf ctCodeTracker.IsSynchronized = False Then

            '== Check for any unsafe modifications to instance variables == 
            If ctCodeTracker.GlobalVars.Count > 0 Then
                For Each itmItem In ctCodeTracker.GlobalVars
                    blnIsRace = CheckRaceCond(CodeLine, FileName, itmItem)
                    If blnIsRace Then Exit For
                Next
            End If

            If blnIsRace = False And ctCodeTracker.GetSetMethods.Count > 0 Then
                For Each itmItem In ctCodeTracker.GetSetMethods
                    blnIsRace = CheckRaceCond(CodeLine, FileName, itmItem)
                    If blnIsRace Then Exit For
                Next
            End If

        ElseIf ctCodeTracker.IsSynchronized Then
            '== Track the amount of code that is inside the lock - resources may be locked unnecessarily ==
            If (CodeLine.Trim <> "{" And CodeLine.Trim <> "}") Then ctCodeTracker.SyncLineCount += 1

            '== Check whether still inside synchronized code ==
            ctCodeTracker.IsSynchronized = ctCodeTracker.TrackBraces(CodeLine, ctCodeTracker.SyncBraces)

            '== Check for large areas of locked code and potential deadlock ==
            CheckSyncIssues(CodeLine, FileName)
        End If

    End Sub

    Private Sub CheckSyncIssues(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for, and report on, any issues located inside the synchronized block or when leaving the block
        '=====================================================================================================
        Dim intSeverity As Integer = 0
        Dim intIndex As Integer = 0
        Dim strSyncObject As String = ""
        Dim strOuterSyncObject As String = ""


        '== Report potentially excessive locking when leaving the code block ==
        If ctCodeTracker.IsSynchronized = False Then

            If ctCodeTracker.SyncLineCount > 14 Then
                intSeverity = CodeIssue.MEDIUM
            ElseIf ctCodeTracker.SyncLineCount > 10 Then
                intSeverity = CodeIssue.STANDARD
            ElseIf ctCodeTracker.SyncLineCount > 6 Then
                intSeverity = CodeIssue.LOW
            End If

            If ctCodeTracker.SyncLineCount > 6 Then
                frmMain.ListCodeIssue("Thread Locks - Possible Performance Impact", "There are " & ctCodeTracker.SyncLineCount & " lines of code in the locked code block. Manually check the code to ensure any shared resources are not being locked unnecessarily.", FileName, intSeverity)
            End If

            ctCodeTracker.SyncLineCount = 0

        ElseIf ctCodeTracker.LockedObject <> "" And Regex.IsMatch(CodeLine, "\block\b\s*\(\s*\w+\s*\)") Then
            '== Build dictionary for potential deadlocks by tracking synchronized blocks inside synchronized blocks ==
            strOuterSyncObject = ctCodeTracker.LockedObject
            strSyncObject = GetSyncObject(CodeLine)

            If strSyncObject <> "" Then
                '== Check if this sync block already exists ==
                For Each itmItem In ctCodeTracker.SyncBlockObjects
                    If itmItem.BlockIndex = ctCodeTracker.SyncIndex Then
                        intIndex = itmItem.BlockIndex
                        '== Add to existing block ==
                        If Not itmItem.InnerObjects.Contains(strSyncObject) Then itmItem.InnerObjects.Add(strSyncObject)
                        Exit For
                    End If
                Next

                '== Create new sync block an add inner object name ==
                If intIndex = 0 Then AddNewSyncBlock(strOuterSyncObject, strSyncObject)

                CheckDeadlock(strOuterSyncObject, strSyncObject, FileName)

            End If
        End If

    End Sub

    Private Function GetSyncObject(ByVal CodeLine As String) As String
        ' Extract the name of a synchronized object from a line of code
        '==============================================================
        Dim strSyncObject As String = ""
        Dim strFragments As String()


        strFragments = Regex.Split(CodeLine, "\block\b\s*\(")
        strSyncObject = GetFirstItem(strFragments.Last, ")")
        If strSyncObject <> "" Then ctCodeTracker.LockedObject = strSyncObject

        Return strSyncObject

    End Function

    Private Function CheckRaceCond(ByVal CodeLine As String, ByVal FileName As String, ByVal DictionaryItem As KeyValuePair(Of String, String)) As Boolean
        ' Check if line contains any references to public variables of servlets or to getter/setter methods of servlets
        '==============================================================================================================
        Dim strServletName As String = ""
        Dim arrFragments As String()
        Dim blnRetVal As Boolean = False


        If CodeLine.Contains("." & DictionaryItem.Key) Then
            arrFragments = Regex.Split(CodeLine, "." & DictionaryItem.Key)
            strServletName = GetLastItem(arrFragments.First)
            If ctCodeTracker.ServletInstances.Count > 0 And ctCodeTracker.ServletInstances.ContainsKey(strServletName) Then
                If DictionaryItem.Value = ctCodeTracker.ServletInstances.Item(strServletName) Then
                    frmMain.ListCodeIssue("Possible Race Condition", "A global variable is being used/modified without a 'lock' block.", FileName, CodeIssue.HIGH)
                    blnRetVal = True
                End If
            End If
        End If

        Return blnRetVal

    End Function

    Public Sub RemoveSanitisedVars(ByVal CodeLine As String)
        ' Remove any variables which have been sanitised from the list of vulnerable variables
        '=====================================================================================

        If ctCodeTracker.InputVars.Count > 0 Then
            For Each strVar In ctCodeTracker.InputVars
                If Not (strVar.contains("(") Or strVar.contains(")") Or strVar.contains("[") Or strVar.contains("]") Or strVar.contains(" ") Or strVar.contains("+") Or strVar.contains("*")) Then
                    If Regex.IsMatch(CodeLine, strVar & "\s*\=\s*\S*(validate|encode|sanitize|sanitise)\S*\(" & strVar) Then
                        ctCodeTracker.InputVars.Remove(strVar)
                        Exit For
                    End If
                End If
            Next
        End If

    End Sub

    Public Sub CheckWebConfig(ByVal CodeLine As String, ByVal FileName As String)
        ' Report any security issues in config file such as debugging or .net default errors
        '===================================================================================

        If Not FileName.ToLower.EndsWith("web.config") Then Exit Sub

        If Regex.IsMatch(CodeLine, "\<\s*customErrors\s+mode\s*\=\s*\""Off\""\s*\/\>") Then
            frmMain.ListCodeIssue(".NET Default Errors Enabled", "The application is configured to display .NET default errors. This can provide an attacker with useful information and should not be used in a live application.", FileName, CodeIssue.MEDIUM)
        ElseIf Regex.IsMatch(CodeLine, "\bdebug\b\s*\=\s*\""\s*true\s*\""") Then
            frmMain.ListCodeIssue(".NET Debugging Enabled", "The application is configured to return .NET debug information. This can provide an attacker with useful information and should not be used in a live application.", FileName, CodeIssue.MEDIUM)
        End If

    End Sub

End Module
