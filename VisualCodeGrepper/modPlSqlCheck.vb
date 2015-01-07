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

Module modPlSqlCheck

    ' Specific checks for PL/SQL code
    '================================

    Public Sub CheckPLSQLCode(ByVal CodeLine As String, ByVal FileName As String)
        ' Carry out any specific checks for the language in question
        '===========================================================


        CheckCrypto(CodeLine, FileName)         ' Check for use Oracle encryption packages for sensitive data
        CheckSqlInjection(CodeLine, FileName)   ' Check usage of EXECUTE IMMEDIATE and OPEN FOR
        CheckPrivs(CodeLine, FileName)          ' Check privilege assignments for packages
        CheckTransControl(CodeLine, FileName)   ' Check potential for data corruption for inappropriate use of COMMIT/ROLLBACK
        CheckErrorHandling(CodeLine, FileName)  ' Identify error handling via return values instead of raising an exception
        CheckViewFormat(CodeLine, FileName)     ' Identify any data formatting within views   

    End Sub

    Private Sub CheckCrypto(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for use Oracle encryption packages for sensitive data such as passwords
        '==============================================================================

        '== Do not perform this check for SQL*Plus files ==
        If FileName.EndsWith(".sql") Then Exit Sub

        '== Check for use of DBMS_CRYPTO package (reversible encryption) or DBMS_OBFUSCATION_TOOLKIT (hashes) for anything that appears to deal with passwords ==
        If ctCodeTracker.IsOracleEncrypt = False And (CodeLine.Contains("DBMS_CRYPTO") Or CodeLine.Contains("DBMS_OBFUSCATION_TOOLKIT")) Then
            ctCodeTracker.IsOracleEncrypt = True
        End If
        If ctCodeTracker.IsOracleEncrypt = False And CodeLine.Contains("PASSWORD") And Not CodeLine.Contains("ACCEPT") Then
            frmMain.ListCodeIssue("Code Appears to Process Passwords Without the Use of a Standard Oracle Encryption Module", "The code contains references to 'password'. The absence of any hashing or decryption functions indicates that the password may be stored as plaintext.", FileName, CodeIssue.HIGH)
        End If

    End Sub

    Private Sub CheckSqlInjection(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for use of EXECUTE IMMEDIATE or OPEN FOR in combination with user-supplied data 
        '======================================================================================
        Dim strVarName As String = ""   ' Holds the variable name for the dynamic SQL statement
        Dim arrFragments As String()


        '== Is unsanitised dynamic SQL statement prepared beforehand? ==
        If ctCodeTracker.IsInsideSQLVarDec = True Then
            If Regex.IsMatch(CodeLine, "(\'|\"")\s*(SELECT|UPDATE|DELETE|INSERT|MERGE|CREATE|SAVEPOINT|ROLLBACK|DROP)") Then
                If Not ctCodeTracker.SQLStatements.Contains(ctCodeTracker.CurrentVar) Then ctCodeTracker.SQLStatements.Add(ctCodeTracker.CurrentVar)
                ctCodeTracker.IsInsideSQLVarDec = False
            ElseIf CodeLine.Contains(";") Then
                ctCodeTracker.IsInsideSQLVarDec = False
            End If
        Else
            If Regex.IsMatch(CodeLine, "\bPROCEDURE\b\s+\w+") Then
                '== Check if we are inside a procedure so we can extract any input variables ==
                ctCodeTracker.IsInsideProcDec = True
            ElseIf ctCodeTracker.IsInsideProcDec = True Then
                '== Get any varnames that are passed in to a procedure ==
                If Regex.IsMatch(CodeLine, "\w+\s+\bIN\b") Then
                    arrFragments = Regex.Split(CodeLine, "\bIN\b")
                    strVarName = GetLastItem(arrFragments.First)
                    ctCodeTracker.InputVars.Add(strVarName)
                End If
                If CodeLine.Contains(")") Then ctCodeTracker.IsInsideProcDec = False
            ElseIf (CodeLine.Contains(":=") And Regex.IsMatch(CodeLine, "(\'|\"")\s*(SELECT|UPDATE|DELETE|INSERT|MERGE|CREATE|SAVEPOINT|ROLLBACK|DROP)")) Or _
                (Regex.IsMatch(CodeLine, "(SQL|QRY|QUERY)\w*\s*\:\=")) Then
                '== Extract variable name from assignment statement ==
                arrFragments = CodeLine.Split(":")
                strVarName = arrFragments.First.Trim
                If Not ctCodeTracker.SQLStatements.Contains(strVarName) Then ctCodeTracker.SQLStatements.Add(strVarName)
            ElseIf Regex.IsMatch(CodeLine, "\:\=\s*$") Then
                '== Declaration starts on next line ==
                ctCodeTracker.IsInsideSQLVarDec = True
                '== Extract variable name from assignment statement ==
                arrFragments = CodeLine.Split(":")
                ctCodeTracker.CurrentVar = arrFragments.First.Trim
            End If
        End If

        '== Check for misuse of sql statements ==
        If ctCodeTracker.IsInsidePlSqlExecuteStmt = False Then
            If (CodeLine.Contains("EXECUTE IMMEDIATE") Or CodeLine.Contains("OPEN FOR")) And (Regex.IsMatch(CodeLine, "(\'\"")\s*\|\|\s*\w+") Or Regex.IsMatch(CodeLine, "\w+\s*\|\|\s*(\'\"")\s*\|\|")) Then
                frmMain.ListCodeIssue("Variable concatenated with dynamic SQL statement.", "Statement is potentially vulnerable to SQL injection, depending on the origin of input variables and opportunities for an attacker to modify them before they reach the procedure.", FileName, CodeIssue.CRITICAL, CodeLine)
            ElseIf (CodeLine.Contains("EXECUTE IMMEDIATE") Or CodeLine.Contains("OPEN FOR")) And Not (CodeLine.Contains("'") Or CodeLine.Contains("""")) Then
                For Each strVar In ctCodeTracker.SQLStatements
                    If CodeLine.Contains(strVar) Then
                        frmMain.ListCodeIssue("Potential SQL Injection", "The application appears to allow SQL injection through use of an input variable within a query, depending on the origin of input variables and opportunities for an attacker to modify them before they reach the procedure.", FileName, CodeIssue.CRITICAL, CodeLine)
                        Exit For
                    End If
                Next
            ElseIf (CodeLine.Contains("EXECUTE IMMEDIATE") Or CodeLine.Contains("OPEN FOR")) And Not CodeLine.Contains(";") Then
                ctCodeTracker.IsInsidePlSqlExecuteStmt = True
            End If

        Else
            If (Regex.IsMatch(CodeLine, "(\'\"")\s*\|\|\s*\w+") Or Regex.IsMatch(CodeLine, "\w+\s*\|\|\s*(\'\"")")) Then
                frmMain.ListCodeIssue("Variable concatenated with dynamic SQL statement.", "Statement is potentially vulnerable to SQL injection, depending on the origin of input variables and opportunities for an attacker to modify them before they reach the procedure.", FileName, CodeIssue.CRITICAL, CodeLine)
            ElseIf Not (CodeLine.Contains("'") Or CodeLine.Contains("""")) Then
                For Each strVar In ctCodeTracker.SQLStatements
                    If CodeLine.Contains(strVar) Then
                        frmMain.ListCodeIssue("Potential SQL Injection", "The application appears to allow SQL injection through use of an input variable within a query, depending on the origin of input variables and opportunities for an attacker to modify them before they reach the procedure.", FileName, CodeIssue.CRITICAL, CodeLine)
                        Exit For
                    End If
                Next
            End If
            If CodeLine.Contains(";") Then ctCodeTracker.IsInsidePlSqlExecuteStmt = False
        End If

    End Sub

    Private Sub CheckPrivs(ByVal CodeLine As String, ByVal FileName As String)
        ' Check privilege assignments for packages and highlight anything too liberal
        '============================================================================


        '== Check for 'CREATE OR REPLACE PACKAGE BODY' without 'AUTHID CURRENT_USER' ==
        If ctCodeTracker.IsNewPackage = False And (CodeLine.Contains("CREATE OR REPLACE PACKAGE BODY") Or CodeLine.Contains("CREATE PACKAGE BODY")) Then ctCodeTracker.IsNewPackage = True

        '== Check the privs for any new package ==
        If ctCodeTracker.IsNewPackage = True Then
            If Regex.IsMatch(CodeLine, "\bAUTHID\b\s+\bCURRENT_USER\b") Then
                ' If package is running as current user there's no problem - set to false and carry on
                ctCodeTracker.IsNewPackage = False
            ElseIf Regex.IsMatch(CodeLine, "\bAUTHID\b\s+\bDEFINER\b") Then
                ' If package is running as definer then give a warning
                frmMain.ListCodeIssue("Package Running Under Potentially Excessive Permissions", "The use of AUTHID DEFINER allows a user to run functions from this package in the role of the definer (usually a developer or administrator).", FileName)
                ctCodeTracker.IsNewPackage = False
            End If
            If ctCodeTracker.IsNewPackage = True And Regex.IsMatch(CodeLine, "\b(AS|IS)\b") Then
                ' If we've reached this point then the package has been defined with no specified privileges and so is running as definer
                frmMain.ListCodeIssue("Package Running Under Potentially Excessive Permissions", "The failure to use AUTHID CURRENT_USER allows a user to run functions from this package in the role of the definer (usually a developer or administrator).", FileName, CodeIssue.STANDARD, "1")
                ctCodeTracker.IsNewPackage = False
            End If
        End If

    End Sub

    Private Sub CheckTransControl(ByVal CodeLine As String, ByVal FileName As String)
        ' Check potential for data corruption for inappropriate use of COMMIT/ROLLBACK
        '=============================================================================

        '== Do not perform this check for SQL*Plus files ==
        If FileName.EndsWith(".sql") Then Exit Sub

        '== Check for transactional control in non-autonomous procedures ==
        If CodeLine.Contains("PRAGMA AUTONOMOUS_TRANSACTION") Then ctCodeTracker.IsAutonomousProcedure = True

        '== If the procedure is not autonomous identify any transactional controls ==
        If ctCodeTracker.IsAutonomousProcedure = False And (CodeLine.Contains("COMMIT") Or CodeLine.Contains("ROLLBACK")) Then
            frmMain.ListCodeIssue("Stored Procedure Contains COMMIT and/or ROLLBACK Statements in Procedures/Functions, Without the Use of PRAGMA AUTONOMOUS_TRANSACTION.", "This can result in data corruption, since rolling back or committing will split a wider logical transaction into two possibly conflicting sub-transactions. Exceptions to this include auditing procedures and long-running worker procedures.", FileName, CodeIssue.LOW)
        End If

    End Sub

    Private Sub CheckErrorHandling(ByVal CodeLine As String, ByVal FileName As String)
        ' Identify error handling via return values instead of raising an exception due to 
        ' risk of data corruption and implications for maintenance and bugs
        '=================================================================================

        '== Check for error handling with output parameters and magic numbers ==
        If CodeLine.Contains("ERROR") And CodeLine.Contains("OUT") And CodeLine.Contains("NUMBER") Then
            frmMain.ListCodeIssue("Error Handling With Output Parameters.", "The code appears to use output parameter(s) which implicitly signal an error by returning a special value, rather than raising an exception. This can make code harder to maintain and more error prone and can result in unexpected behaviour and data corruption.", FileName, CodeIssue.MEDIUM, CodeLine)
        End If

    End Sub

    Private Sub CheckViewFormat(ByVal CodeLine As String, ByVal FileName As String)
        ' Identify any data formatting within views due to risk of DoS and data corruption
        '=================================================================================


        '== Check for data formatting within views ==
        If ctCodeTracker.IsView = False And CodeLine.Contains("CREATE OR REPLACE VIEW") Then ctCodeTracker.IsView = True

        If ctCodeTracker.IsView = True Then
            If CodeLine.Contains("\") And ((InStr(CodeLine, "\*") <> InStr(CodeLine, "\")) And (InStr(CodeLine, "\\") <> InStr(CodeLine, "\"))) Then
                ctCodeTracker.IsView = False
            ElseIf CodeLine.Contains("TO_CHAR") Or CodeLine.Contains("TRIM(") Or CodeLine.Contains("TO_NUMBER") Or CodeLine.Contains("UPPER(") Or CodeLine.Contains("LOWER(") Then
                frmMain.ListCodeIssue("Data Formatting Within VIEW.", "This can can result in performance issues and can facilitate DoS attacks in any situation where any attacker manages to cause repeated queries against the view. There is also a possibility of data corruption due to mismatch between views and underlying tables.", FileName, CodeIssue.STANDARD, CodeLine)
            End If
        End If

    End Sub

End Module
