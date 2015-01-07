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

Module modVBCheck

    ' Specific checks for VB code
    '============================

    Public Sub CheckVBCode(ByVal CodeLine As String, ByVal FileName As String)
        ' Carry out any specific checks for the language in question.
        ' A lot of our VB checks are generic ASP checks and use the functions
        ' in the C# module.
        '====================================================================

        CheckInputValidation(CodeLine, FileName)        '(same check will work for VB and C# - hence we use function in C# module)
        CheckSQLInjection(CodeLine, FileName)           ' Check for SQLi (same check will work for VB and C# - hence we use function in C# module)
        CheckXSS(CodeLine, FileName)                    ' Check for XSS (same check will work for VB and C# - hence we use function in C# module)
        CheckSecureStorage(CodeLine, FileName)          ' Are sensitive variables stored without using SecureString? (same check will work for VB and C# - hence we use function in C# module)
        CheckLogDisplay(CodeLine, FileName)             ' Is data sanitised before being written to logs? (same check will work for VB and C# - hence we use function in C# module)
        CheckFileRace(CodeLine, FileName)               ' Check for race conditions and TOCTOU vulns (same check will work for VB and C# - hence we use function in C# module)
        CheckHTTPRedirect(CodeLine, FileName)           ' Check for safe redirects and safe use of URLs (same check will work for VB and C# - hence we use function in C# module)
        CheckRandomisation(CodeLine, FileName)          ' Locate any use of randomisation functions that are not cryptographically secure
        CheckSAML2Validation(CodeLine, FileName)        ' Check for correct implementation of inherited SAML2 functions
        CheckUnsafeTempFiles(CodeLine, FileName)        ' Check for static/obvious filenames for temp files
        CheckCryptoKeys(CodeLine, FileName)             ' Check for hardcoded keys
        CheckExecutable(CodeLine, FileName)             ' Check for unvalidated variables being executed via cmd line/system calls (same check will work for VB and C# - hence we use function in C# module)
        CheckWebConfig(CodeLine, FileName)              ' Check config file to determine whether .NET debugging and default errors are enabled

        If Regex.IsMatch(CodeLine, "\S*(Password|password|pwd|passwd)\S*\.(ToLower|ToUpper)\s*\(") Then
            frmMain.ListCodeIssue("Unsafe Password Management", "The application appears to handle passwords in a case-insensitive manner. This can greatly increase the likelihood of successful brute-force and/or dictionary attacks.", FileName, CodeIssue.MEDIUM, CodeLine)
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
        If Regex.IsMatch(CodeLine, "\bRnd\b\s*\(") Then
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
        If ctCodeTracker.IsSamlFunction = False And Regex.IsMatch(CodeLine, "\bOverrides\b\s+\b(Sub|Function)\b\s+\bValidateConditions\b\(\bSaml2Conditions\b") Then
            ctCodeTracker.IsSamlFunction = True
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
            If Regex.IsMatch(CodeLine, "\bEnd\b\s+\b(Sub|Function)\b") Then ctCodeTracker.IsSamlFunction = True
        End If

    End Sub

    Private Sub CheckUnsafeTempFiles(ByVal CodeLine As String, ByVal FileName As String)
        ' Identify any creation of temp files with static names
        '======================================================

        If Regex.IsMatch(CodeLine, "(file\S*|File\S*|\.FileName)\s+\=\s+\""\S*(temp|tmp)\S*\""\,") Then
            frmMain.ListCodeIssue("Unsafe Temporary File Allocation", "The application appears to create a temporary file with a static, hard-coded name. This causes security issues in the form of a classic race condition (an attacker creates a file with the same name between the application's creation and attempted usage) or a symbolic linbk attack where an attacker creates a symbolic link at the temporary file location.", FileName, CodeIssue.MEDIUM, CodeLine)
        End If

    End Sub

    Private Sub CheckCryptoKeys(ByVal CodeLine As String, ByVal FileName As String)
        ' Identify any hardcoded static keys and IVs
        '===========================================

        If Regex.IsMatch(CodeLine, "\b(Private|Public|Dim)\b\s+\b(Const|ReadOnly)\b\s+\w*(crypt|Crypt|CRYPT|key|Key|KEY)\w*\s+As\s+String\s*\=\s*\""") Or _
            Regex.IsMatch(CodeLine, "\b(Private|Public|Dim)\b\s+\b(Const|ReadOnly)\b\s+\w*(iv|Iv|IV)\s+As\s+Byte\(\)\s*\=\s*New\s+Byte\s*\(\w*\)\s*\{") Then
            frmMain.ListCodeIssue("Hardcoded Crypto Key", "The code appears to use hardcoded encryption keys. These can be rendered visible with the use of debugger or hex editor, exposing encrypted data.", FileName, CodeIssue.MEDIUM, CodeLine)
        End If

    End Sub

End Module
