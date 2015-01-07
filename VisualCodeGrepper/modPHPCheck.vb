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

Module modPHPCheck

    ' Specific checks for PHP code
    '=============================

    Public Sub CheckPHPCode(ByVal CodeLine As String, ByVal FileName As String)
        ' Carry out any specific checks for the language in question
        '===========================================================

        CheckSQLInjection(CodeLine, FileName)           ' Check for SQLi
        CheckXSS(CodeLine, FileName)                    ' Check for XSS
        CheckLogDisplay(CodeLine, FileName)             ' Is data sanitised before being written to logs?
        CheckRandomisation(CodeLine, FileName)          ' Locate any use of randomisation functions that are not cryptographically secure
        CheckFileValidation(CodeLine, FileName)         ' Find any unsafe file validation (checks against data from the HTTP request *instead of* the actual file
        CheckFileInclusion(CodeLine, FileName)          ' Locate any include files with unsafe extensions
        CheckExecutable(CodeLine, FileName)             ' Check for unvalidated variables being executed via cmd line/system calls
        CheckBackTick(CodeLine, FileName)               ' Check for user-supplied variables being executed on the cmdline due to backtick usage
        CheckRegisterGlobals(CodeLine, FileName)        ' Check for usage or simulation of register_globals
        CheckParseStr(CodeLine, FileName)               ' Check for any unsafe usage of parse_str

        '== Check for passwords being handled in a case-insensitive manner ==
        If Regex.IsMatch(CodeLine, "(strtolower|strtoupper)\s*\(\s*\S*(Password|password|pwd|PWD|Pwd|Passwd|passwd)") Then
            frmMain.ListCodeIssue("Unsafe Password Management", "The application appears to handle passwords in a case-insensitive manner. This can greatly increase the likelihood of successful brute-force and/or dictionary attacks.", FileName, CodeIssue.MEDIUM, CodeLine)
        End If

    End Sub

    Private Sub CheckSQLInjection(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for any SQL injection problems 
        '=====================================
        Dim strVarName As String = ""   ' Holds the variable name for the dynamic SQL statement


        '== Only check unvalidated code ==
        If ctCodeTracker.HasValidator = True Then Exit Sub


        '== Is unsanitised dynamic SQL statement prepared beforehand? ==
        If CodeLine.Contains("=") And (CodeLine.ToLower.Contains("sql") Or CodeLine.ToLower.Contains("query") Or CodeLine.ToLower.Contains("stmt") Or CodeLine.ToLower.Contains("query")) And (CodeLine.Contains("""") And (CodeLine.Contains("$") Or CodeLine.Contains("+"))) Then
            '== Extract variable name from assignment statement ==
            strVarName = GetVarName(CodeLine)
            ctCodeTracker.HasVulnSQLString = True
            If Regex.IsMatch(strVarName, "^\$[a-zA-Z0-9_]*$") And Not ctCodeTracker.SQLStatements.Contains(strVarName) Then ctCodeTracker.SQLStatements.Add(strVarName)
        End If


        If Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise") Then
            '== Remove any variables which have been sanitised from the list of vulnerable variables ==  
            RemoveSanitisedVars(CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "(mysql_query|mssql_query|pg_query)\s*\(") And Not Regex.IsMatch(CodeLine, "mysql_real_escape_string") Then

            If ctCodeTracker.HasVulnSQLString = True Then
                '== Check for use of pre-prepared statements ==
                For Each strVar In ctCodeTracker.SQLStatements
                    If Regex.IsMatch(CodeLine, strVar) Then
                        frmMain.ListCodeIssue("Potential SQL Injection", "The application appears to allow SQL injection via a pre-prepared dynamic SQL statement.", FileName, CodeIssue.CRITICAL, CodeLine)
                        Exit For
                    End If
                Next
            ElseIf CodeLine.Contains("$") Then
                '== Dynamic SQL built into connection/update ==
                frmMain.ListCodeIssue("Potential SQL Injection", "The application appears to allow SQL injection via dynamic SQL statements.", FileName, CodeIssue.CRITICAL, CodeLine)

            End If
        End If

    End Sub

    Private Sub CheckXSS(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for any XSS problems 
        '===========================
        Dim strVarName As String = ""
        Dim blnIsFound As Boolean = False
        '== Only check unvalidated code ==
        If ctCodeTracker.HasValidator = True Then Exit Sub


        If Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise") Then
            '== Remove any variables which have been sanitised from the list of vulnerable variables ==  
            RemoveSanitisedVars(CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "\$\w+\s*\=\s*\$_(GET|POST|COOKIE|REQUEST|SERVER)") Then
            '== Extract variable name from assignment statement ==
            strVarName = GetVarName(CodeLine)
            If Regex.IsMatch(strVarName, "^\\\$[a-zA-Z0-9_]*$") And Not ctCodeTracker.InputVars.Contains(strVarName) Then ctCodeTracker.InputVars.Add(strVarName)
        ElseIf Regex.IsMatch(CodeLine, "\b(print|echo|print_r)\b") And CodeLine.Contains("$") And Not Regex.IsMatch(CodeLine, "strip_tags") Then
            CheckUserVarXSS(CodeLine, FileName)
        ElseIf Regex.IsMatch(CodeLine, "\b(print|echo|print_r)\b\s*\$_(GET|POST|COOKIE|REQUEST|SERVER)") And Not Regex.IsMatch(CodeLine, "strip_tags") Then
            frmMain.ListCodeIssue("Potential XSS", "The application appears to reflect a user-supplied variable to the screen with no apparent validation or sanitisation.", FileName, CodeIssue.HIGH, CodeLine)
        End If

        '== Check for DOM-based XSS in .php pages ==
        If FileName.ToLower.EndsWith(".php") Or FileName.ToLower.EndsWith(".html") And Not Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise|strip_tags") Then
            If Regex.IsMatch(CodeLine, "\s+var\s+\w+\s*=\s*""\s*\<\?\s*\=\s*\w+\s*\?\>""\;") Then
                '== Extract variable name from assignment statement ==
                strVarName = GetVarName(CodeLine)
                If Regex.IsMatch(strVarName, "^[a-zA-Z0-9_]*$") And Not ctCodeTracker.SQLStatements.Contains(strVarName) Then ctCodeTracker.InputVars.Add(strVarName)
            ElseIf ((CodeLine.Contains("document.write(") And CodeLine.Contains("+") And CodeLine.Contains("""")) Or Regex.IsMatch(CodeLine, ".innerHTML\s*\=\s*\w+;")) Then
                For Each strVar In ctCodeTracker.InputVars
                    If Regex.IsMatch(CodeLine, strVar) Then
                        frmMain.ListCodeIssue("Potential DOM-Based XSS", "The application appears to allow XSS via an unencoded/unsanitised input variable.", FileName, CodeIssue.HIGH, CodeLine)
                        Exit For
                    End If
                Next
            ElseIf Regex.IsMatch(CodeLine, "\)\s*\.innerHTML\s*=\s*(\'|\"")\s*\<\s*\?\s*echo\s*\$_(GET|POST|COOKIE|SERVER|REQUEST)\s*\[") Then
                frmMain.ListCodeIssue("Potential DOM-Based XSS", "The application appears to allow XSS via an unencoded/unsanitised input variable.", FileName, CodeIssue.HIGH, CodeLine)
            End If
        End If

    End Sub

    Private Sub CheckLogDisplay(ByVal CodeLine As String, ByVal FileName As String)
        ' Check output written to logs is sanitised first
        '================================================

        '== Only check unvalidated code ==
        If Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise") And Not CodeLine.ToLower.Contains("password") Then
            RemoveSanitisedVars(CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "AddLog|error_log") And CodeLine.ToLower.Contains("password") Then
            If (InStr(CodeLine.ToLower, "log") < InStr(CodeLine.ToLower, "password")) Then frmMain.ListCodeIssue("Application Appears to Log User Passwords", "The application appears to write user passwords to logfiles or the screen, creating a risk of credential theft.", FileName, CodeIssue.HIGH, CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "AddLog|error_log") And Not CodeLine.ToLower.Contains("strip_tags") Then
            For Each strVar In ctCodeTracker.InputVars
                If Regex.IsMatch(CodeLine, strVar) Then
                    frmMain.ListCodeIssue("Unsanitized Data Written to Logs", "The application appears to write unsanitized data to its logfiles. If logs are viewed by a browser-based application this exposes risk of XSS attacks.", FileName, CodeIssue.MEDIUM, CodeLine)
                    Exit For
                End If
            Next
        End If

    End Sub

    Private Sub CheckRandomisation(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for any random functions that are not cryptographically secure
        '=====================================================================

        '== Check for time or non-time-based seed ==
        If Regex.IsMatch(CodeLine, "\$\w+\s*\=\s*\bopenssl_random_pseudo_bytes\b\s*\(\s*\S+\s*\,\s*(0|false|False|FALSE)") Then
            frmMain.ListCodeIssue("Use of Deterministic Pseudo-Random Values", "The code appears to use the function with the 'secure' value deliberately set to 'false'. The resulting values, while appearing random to a casual observer, are predictable and may be enumerated by a skilled and determined attacker.", FileName, CodeIssue.MEDIUM, CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "\$\w+\s*\=\s*\b(mt_rand|smt_rand)\b\s*\(\s*\)") Or Regex.IsMatch(CodeLine, "\b(mt_rand|smt_rand)\b\s*\(\w*(T|t)ime\w*\)") Then
            frmMain.ListCodeIssue("Use of Deterministic Pseudo-Random Values", "The code appears to use the mt_rand and/or smt_rand functions without a seed to generate pseudo-random values. The resulting values, while appearing random to a casual observer, are predictable and may be enumerated by a skilled and determined attacker.", FileName, CodeIssue.MEDIUM, CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "\b(mt_rand|smt_rand)\b\s*\(\s*\S+\s*\)") Then
            frmMain.ListCodeIssue("Use of Deterministic Pseudo-Random Values", "The code appears to use the mt_rand function. The resulting values, while appearing random to a casual observer, are predictable and may be enumerated by a skilled and determined attacker, although this is partly mitigated by a seed that does not appear to be time-based.", FileName, CodeIssue.STANDARD, CodeLine)
        End If

    End Sub

    Private Sub CheckFileValidation(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for any decisions based on characteristics of the $_FILES array
        '======================================================================

        '== Identify relevant 'if' statements ==
        If Regex.IsMatch(CodeLine, "\bif\b\s*\(\s*\$_FILES\s*\[\s*\$\w+\s*\]\s*\[\s*\'") Or Regex.IsMatch(CodeLine, "\bif\b\s*\(\s*\!?\s*isset\s*\(?\s*\$_FILES\s*\[\s*\$\w+\s*\]\s*\[\s*\'") Then
            frmMain.ListCodeIssue("Unsafe Processing of $_FILES Array", "The code appears to use data within the $_FILES array in order to make to decisions. this is obtained direct from the HTTP request and may be modified by the client to cause unexpected behaviour.", FileName, CodeIssue.MEDIUM, CodeLine)
        End If

    End Sub

    Private Sub CheckFileInclusion(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for any user-defined variables being used to name include files
        '======================================================================
        Dim blnIsFound As Boolean = False

        '== Identify relevant 'include' statements ==
        If Regex.IsMatch(CodeLine, "\b(file_include|include|require|include_once|require_once)\b\s*\(\s*\$") Then
            '== Check for use of user-defined variables ==
            For Each strVar In ctCodeTracker.InputVars
                If Regex.IsMatch(CodeLine, "\b(file_include|include|require|include_once|require_once)\b\s*\(\s*" & strVar) Or Regex.IsMatch(CodeLine, "\b(file_include|include|require|include_once|require_once)\b\s*\(\s*\w+\s*\.\s*" & strVar) Then
                    frmMain.ListCodeIssue("File Inclusion Vulnerability", "The code appears to use a user-controlled variable as a parameter for an include statement which could lead to a file include vulnerability.", FileName, CodeIssue.HIGH, CodeLine)
                    blnIsFound = True
                    Exit For
                End If
            Next
            If blnIsFound = False Then
                frmMain.ListCodeIssue("Variable Used as FileName", "The application appears to use a variable name in order to define a filename used by the application. It is unclear whether this variable can be controlled by the user - carry out a manual inspection to confirm.", FileName, CodeIssue.LOW, CodeLine)
            End If
        ElseIf Regex.IsMatch(CodeLine, "\b(file_include|include|require|include_once|require_once)\b\s*\(\s*(\'|\"")\w+\.(inc|txt|dat)") Then
            '== Check for use of unsafe extensions ==
            frmMain.ListCodeIssue("File Inclusion Vulnerability", "The code appears to use an unsafe file extension for an include statement which could allow an attacker to download it directly and read the uncompiled code.", FileName, CodeIssue.HIGH, CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "\b(fwrite|file_get_contents|fopen|glob|popen|file|readfile)\b\s*\(\s*\$") Then
            '== Check for use of user-defined variables ==
            For Each strVar In ctCodeTracker.InputVars
                If Regex.IsMatch(CodeLine, "\b(fwrite|file_get_contents|fopen|glob|popen|file|readfile)\b\s*\(\s*" & strVar) Or Regex.IsMatch(CodeLine, "\b(fwrite|file_get_contents|fopen|glob|popen|file|readfile)\b\s*\(\s*\w+\s*\.\s*" & strVar) Then
                    frmMain.ListCodeIssue("File Access Vulnerability", "The code appears to user-controlled variable as a parameter when accessing the filesystem. This could lead to a system compromise.", FileName, CodeIssue.HIGH, CodeLine)
                    blnIsFound = True
                    Exit For
                End If
            Next
            If blnIsFound = False Then
                frmMain.ListCodeIssue("Variable Used as FileName", "The application appears to use a variable name in order to define a filename used by the application. It is unclear whether this variable can be controlled by the user - carry out a manual inspection to confirm.", FileName, CodeIssue.LOW, CodeLine)
            End If
        End If

    End Sub

    Private Sub CheckExecutable(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for unvalidated variables being executed via cmd line/system calls
        '=========================================================================
        Dim blnIsFound As Boolean = False


        If Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise") Then Exit Sub

        If Regex.IsMatch(CodeLine, "\b(exec|shell_exec|proc_open|eval|system|popen|passthru|pcntl_exec|assert)\b") And Not Regex.IsMatch(CodeLine, "escapeshellcmd") Then
            For Each strVar In ctCodeTracker.InputVars
                If Regex.IsMatch(CodeLine, strVar) Then
                    frmMain.ListCodeIssue("User Controlled Variable Used on System Command Line", "The application appears to allow the use of an unvalidated user-controlled variable when executing a command.", FileName, CodeIssue.HIGH, CodeLine)
                    blnIsFound = True
                    Exit For
                End If
            Next
            If blnIsFound = False And CodeLine.Contains("$") Then
                frmMain.ListCodeIssue("Application Variable Used on System Command Line", "The application appears to allow the use of an unvalidated variable when executing a command. Carry out a manual check to determine whether the variable is user-controlled.", FileName, CodeIssue.MEDIUM, CodeLine)
            End If
        End If

    End Sub

    Private Sub CheckBackTick(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for user-supplied variables being executed on the cmdline due to backtick usage
        '======================================================================================
        Dim blnIsFound As Boolean = False


        If Regex.IsMatch(CodeLine, "`\s*\S*\s*\$_(GET|POST|COOKIE|REQUEST|SERVER)") Then
            frmMain.ListCodeIssue("User Controlled Variable Used on System Command Line", "The application appears to allow the use of a HTTP request variable within backticks, allowing commandline execution.", FileName, CodeIssue.HIGH, CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "`\s*\S*\s*\$\w+") Then
            For Each strVar In ctCodeTracker.InputVars
                If Regex.IsMatch(CodeLine, strVar) Then
                    frmMain.ListCodeIssue("User Controlled Variable Used on System Command Line", "The application appears to allow the use of a user-controlled variable within backticks, allowing commandline execution.", FileName, CodeIssue.HIGH, CodeLine)
                    blnIsFound = True
                    Exit For
                End If
            Next
            If blnIsFound = False Then
                frmMain.ListCodeIssue("Application Variable Used on System Command Line", "The application appears to allow the use of a variable within backticks, allowing commandline execution. Carry out a manual check to determine whether the variable is user-controlled.", FileName, CodeIssue.MEDIUM, CodeLine)
            End If
        End If

    End Sub

    Private Sub CheckPHPEvaluation(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for unvalidated variables being executed via cmd line/system calls
        '=========================================================================
        Dim blnIsFound As Boolean = False


        If Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise") Then Exit Sub

        If Regex.IsMatch(CodeLine, "\b(preg_replace|create_function)\b") And Not Regex.IsMatch(CodeLine, "strip_tags") Then
            For Each strVar In ctCodeTracker.InputVars
                If Regex.IsMatch(CodeLine, strVar) Then
                    frmMain.ListCodeIssue("Function May Evaluate PHP Code Contained in User Controlled Variable", "The application appears to allow the use of an unvalidated user-controlled variable in conjunction with a function that will evaluate PHP code.", FileName, CodeIssue.HIGH, CodeLine)
                    blnIsFound = True
                    Exit For
                End If
            Next
            If blnIsFound = False And CodeLine.Contains("$") Then
                frmMain.ListCodeIssue("Function May Evaluate PHP Code", "The application appears to allow the use of an unvalidated variable in conjunction with a function that will evaluate PHP code. Carry out a manual check to determine whether the variable is user-controlled.", FileName, CodeIssue.MEDIUM, CodeLine)
            End If
        End If

    End Sub

    Private Sub CheckRegisterGlobals(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for any unsafe use of Global Variables
        '=============================================
        Dim arrFragments As String()

        If ctCodeTracker.IsRegisterGlobals = True Then Exit Sub

        If ctCodeTracker.IsArrayMerge = False Then

            If Regex.IsMatch(CodeLine, "\bini_set\b\s*\(\s*(\'|\"")register_globals(\'|\"")\s*\,\s*(1|true|TRUE|True|\$\w+)") Then
                ' Is it being re-enabled?
                frmMain.ListCodeIssue("Use of 'register_globals'", "The application appears to re-activate the use of the dangerous 'register_globals' facility. Anything passed via GET or POST or COOKIE is automatically assigned as a global variable in the code, with potentially serious consequences.", FileName, CodeIssue.CRITICAL, CodeLine)

            ElseIf Regex.IsMatch(CodeLine, "\$\w+\s*\=\s*\barray_merge\b\s*\(\s*\$_(GET|POST|COOKIE|REQUEST|SERVER)\s*\,\s*\$_(GET|POST|COOKIE|REQUEST|SERVER)") Then
                ' Is it being simulated?
                ctCodeTracker.IsArrayMerge = True
                ' Get name of the array of input parameters
                arrFragments = Regex.Split(CodeLine, "\=\s*\barray_merge\b\s*\(\s*\$_(GET|POST|COOKIE|REQUEST|SERVER)\s*\,")
                ctCodeTracker.GlobalArrayName = GetLastItem(arrFragments.First())
                frmMain.ListCodeIssue("Indiscriminate Merging of Input Variables", "The application appears to incorporate all incoming GET and POST data into a single array. This can facilitate GET to POST conversion and may result in unexpected behaviour or unintentionally change variables.", FileName, CodeIssue.HIGH, CodeLine)
            End If

        ElseIf ctCodeTracker.IsArrayMerge = True Then
            If Regex.IsMatch(CodeLine, "\bglobal\b") And Regex.IsMatch(CodeLine, ctCodeTracker.GlobalArrayName) Then
                ctCodeTracker.IsRegisterGlobals = True
                frmMain.ListCodeIssue("Use of 'register_globals'", "The application appears to attempt to simulate the use of the dangerous 'register_globals' facility. Anything passed via GET or POST or COOKIE is automatically assigned as a global variable in the code, with potentially serious consequences.", FileName, CodeIssue.CRITICAL, CodeLine)
            End If
        End If

    End Sub

    Private Sub CheckParseStr(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for any unsafe use of parse_str which offers similar dangers to Global Variables
        '=======================================================================================
        Dim blnIsFound = False


        '== Identify unssafe usage of parse_str, with an input var, but no destination array ==
        If Regex.IsMatch(CodeLine, "\bparse_str\b\s*\(\s*\$\w+\s*\)") Then

            For Each strVar In ctCodeTracker.InputVars
                If Regex.IsMatch(CodeLine, "\bparse_str\b\s*\(\s*" & strVar & "\s*\)") Then
                    frmMain.ListCodeIssue("Use of 'parse_str' with User Controlled Variable", "The application appears to use parse_str in an unsafe manner in combination with a user-controlled variable. Anything passed as part of the input string is automatically assigned as a global variable in the code, with potentially serious consequences.", FileName, CodeIssue.CRITICAL, CodeLine)
                    blnIsFound = True
                    Exit For
                End If
            Next
            If blnIsFound = False Then
                frmMain.ListCodeIssue("Use of 'parse_str'", "The application appears to use parse_str in an unsafe manner. Anything passed as part of the input string is automatically assigned as a global variable in the code, with potentially serious consequences. Carry out a manual check to determine whether the variable is user-controlled.", FileName, CodeIssue.MEDIUM, CodeLine)
            End If
        End If

    End Sub

    Public Sub CheckPhpIni(ByVal CodeLine As String, ByVal FileName As String)
        ' Check config file for unsafe settings
        '======================================

        ' Ignore any comments
        If CodeLine.Trim().StartsWith(";") Then
            rtResultsTracker.OverallCommentCount += 1
            rtResultsTracker.CommentCount += 1
        ElseIf CodeLine.Trim() = "" Then
            rtResultsTracker.OverallWhitespaceCount += 1
            rtResultsTracker.WhitespaceCount += 1
        Else

            If Regex.IsMatch(CodeLine, "\bregister_globals\b\s*=\s*\b(on|ON|On)\b") Then
                ' register_globals
                frmMain.ListCodeIssue("Use of 'register_globals'", "The application appears to activate the use of the dangerous 'register_globals' facility. Anything passed via GET or POST or COOKIE is automatically assigned as a global variable in the code, with potentially serious consequences.", FileName, CodeIssue.CRITICAL, CodeLine)
            ElseIf Regex.IsMatch(CodeLine, "\bsafe_mode\b\s*=\s*\b(off|OFF|Off)\b") Then
                ' safe_mode
                frmMain.ListCodeIssue("De-Activation of 'safe_mode'", "The application appears to de-activate the use of 'safe_mode', which can increase risks for any CGI-based applications.", FileName, CodeIssue.MEDIUM, CodeLine)
            ElseIf Regex.IsMatch(CodeLine, "\b(magic_quotes_gpc|magic_quotes_runtime|magic_quotes_sybase)\b\s*=\s*\b(off|OFF|Off)\b") Then
                ' magic_quotes
                frmMain.ListCodeIssue("De-Activation of 'magic_quotes'", "The application appears to de-activate the use of 'magic_quotes', greatly increasing the risk of SQL injection.", FileName, CodeIssue.HIGH, CodeLine)
            ElseIf Regex.IsMatch(CodeLine, "\bdisable_functions\b\s*=\s*\w+") Then
                ' Is disable_functions being used?
                ctCodeTracker.HasDisableFunctions = True
            ElseIf Regex.IsMatch(CodeLine, "\bmysql.default_user\b\s*=\s*\broot\b") Then
                ' Log in to MySQL as 'root'?
                frmMain.ListCodeIssue("Log in to MySQL as 'root'", "The application appears to log in to MySQL as 'root', greatly increasing the consequences of a successful SQL injection attack.", FileName, CodeIssue.HIGH, CodeLine)
            End If

            rtResultsTracker.OverallCodeCount += 1
            rtResultsTracker.CodeCount += 1

        End If

        rtResultsTracker.OverallLineCount += 1
        rtResultsTracker.LineCount += 1

    End Sub

End Module
