' VisualCodeGrepper - Code security scanner
' Copyright (C) 2012-2021 Nick Dunn and John Murray
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

Module modRCheck

    ' Specific checks for R code
    '===========================

    Public Sub CheckRCode(ByVal CodeLine As String, ByVal FileName As String)
        ' Carry out any specific checks for the language in question
        '===========================================================

        TrackRegistryUse(CodeLine, FileName)            ' Check for registry values passed into variable names
        CheckExcel(CodeLine, FileName)                  ' Does the code interact with Excel?
        CheckRDatasets(CodeLine, FileName)              ' Check loading/saving of datasets to disc
        'CheckExternalDatasets(CodeLine, FileName)
        CheckWebInteraction(CodeLine, FileName)         ' Identify use of unencrypted connections or import of data from web pages
        CheckDatabase(CodeLine, FileName)               ' Identify misuse of databases (passwords in code, etc.)
        CheckXMLJSON(CodeLine, FileName)                ' Give warning about sensitive data written to filesystem
        CheckSerialization(CodeLine, FileName)          ' Identify any object serialization
        CheckFileAccess(CodeLine, FileName)             ' Is data imported from external files?
        CheckClipboardAccess(CodeLine, FileName)        ' Does the code read from or paste to the clipboard?
        CheckFileOutput(CodeLine, FileName)             ' Is data being written to files?
        CheckFileRace(CodeLine, FileName)               ' Check for race conditions and TOCTOU vulns
        CheckSystemInteraction(CodeLine, FileName)      ' Check command line execution
        CheckUserInteraction(CodeLine, FileName)        ' Check command line execution
        CheckRandomisation(CodeLine, FileName)          ' Locate any use of randomisation functions that are not cryptographically secure
        CheckUnsafeTempFiles(CodeLine, FileName)        ' Check for static/obvious filenames for temp files

    End Sub

    Private Sub TrackRegistryUse(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether assignements are made to variables.
        ' These will be checked further to determine whether they are
        ' sanitised or subsequently used as filenames, filepaths, etc.
        '==============================================================
        Dim arrFragments As String()
        Dim strLeft As String = ""


        '== Track assignments from argv, system variables, ini files or registry ==
        If (Regex.IsMatch(CodeLine, "\w+\s*\<\-\s*\breadRegistry\b")) Then
            ' Extract the variable name
            arrFragments = CodeLine.Split("<-")
            strLeft = GetLastItem(arrFragments.First)

            '== Store any discovered variables
            If strLeft <> "" Then
                If Not ctCodeTracker.UserVariables.Contains(strLeft) Then
                    ctCodeTracker.UserVariables.Add(strLeft)
                End If
            End If

            frmMain.ListCodeIssue("Registry Value Stored in Variable", "The code reads a registry value into a variable. If this input is subsequently used by the code without sanitisation, then it may result in unintended behaviour and may present opportunities to an attacker who can modify the registry value.", FileName, CodeIssue.MEDIUM, CodeLine)
        End If

    End Sub

    Private Sub CheckExcel(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether or not code interacts with MS Excel
        '======================================================
        Dim arrFragments As String()
        Dim strLeft As String = ""


        '== Check if data imported into a variable ==
        If (Regex.IsMatch(CodeLine, "\w+\s*\<\-\s*\b(read_excel|read_excelx|read_xlsx)\b\s*\(")) Then

            frmMain.ListCodeIssue("Excel Data Stored in Vector/Variable", "The code reads the content of an Excel file into a variable. If this input is subsequently used by the code without sanitisation, then it may result in unintended behaviour and may present opportunities to an attacker who can modify the file.", FileName, CodeIssue.MEDIUM, CodeLine)

            '== Add variable to list of user-controlled variables ==
            arrFragments = CodeLine.Split("<-")
            strLeft = GetLastItem(arrFragments.First)

            '== Store any discovered variables
            If strLeft <> "" Then
                If Not ctCodeTracker.UserVariables.Contains(strLeft) Then
                    ctCodeTracker.UserVariables.Add(strLeft)
                End If
            End If

        ElseIf Regex.IsMatch(CodeLine, "\b(read_excel|read_excelx|read_xlsx)\b\s*\(") Then
            frmMain.ListCodeIssue("Use of Excel File", "The code reads the content of an Excel file. If this input is subsequently used by the code without sanitisation, then it may result in unintended behaviour and may present opportunities to an attacker who can modify the file.", FileName, CodeIssue.LOW, CodeLine)
        End If

    End Sub

    Private Sub CheckRDatasets(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether code loads or saves an R Dataset from disk
        '=============================================================

        '== Check for use of R datasets ==
        If (Regex.IsMatch(CodeLine, "\bdata\b\s*\(")) Then
            ' Check for data loaded from an imported package
            frmMain.ListCodeIssue("Data Imported from Package", "The code imports an R dataset from a package. This input is in the control of the package provider/modifier. It should be verified prior to use in sensitive or critical situations.", FileName, CodeIssue.INFO, CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "\bload\b\s*\(") Then
            ' Check for data loaded from an R data file
            frmMain.ListCodeIssue("Data Imported from R Dataset", "The code imports an R dataset from the local system. Any data objects within the file may have been modified by other R code with access to the file and the operation should only be used on a trusted system. Note that load() overwrites existing objects with the same names without giving any warnings.", FileName, CodeIssue.LOW, CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "\bsave\b\s*\(") Then
            ' Check for data saved to an R data file
            frmMain.ListCodeIssue("Data Saved to R Dataset", "The code saves data objects to an RData file. The file will remain on disc where it may be deliberately or inadvertently modified by another R script. Any sensitive data in the file may be exposed to a third party.", FileName, CodeIssue.LOW, CodeLine)
        End If

    End Sub

    Private Sub CheckExternalDatasets(ByVal CodeLine As String, ByVal FileName As String)
        ' Check whether code loads a dataset exported from SAS, STATA, etc.
        '==================================================================

        ' This is currently done using the conf file - it's uncertain whether there's any need for regexes


    End Sub

    Private Sub CheckWebInteraction(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether code interacts with external web pages
        '==========================================================

        '== Identify any web interaction ==
        If (Regex.IsMatch(CodeLine, "\w+\s*\<\-\s*\b(paste|paste0)\b\s*\(\""\b(http|ftp)\b\:\/\/") Or Regex.IsMatch(CodeLine, "\w+\s*\<\-\s*\""\b(http|ftp)\b\:\/\/")) Then
            ' Check for unencrypted HTTP or FTP
            frmMain.ListCodeIssue("Unencrypted Connection", "The code connects to a resource using an unencrypted protocol. Any network traffic (including credentials) may be sniffed by a suitably placed attacker.", FileName, CodeIssue.MEDIUM, CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "\w+\s*\<\-\s*\bhtmltab\b\s*\(") Then
            ' Check for data loaded from an R data file
            frmMain.ListCodeIssue("Data Imported from HTML Table", "The code imports data from a table on a web page. Note that data from a public source is reliant on the curation of the provider and their safeguards against contamination.", FileName, CodeIssue.LOW, CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "\w+\s*\<\-\s*\bread_html\b\s*\(") Then
            ' Check for data saved to an R data file
            frmMain.ListCodeIssue("HTML Scraped from Web Page", "The code scrapes data from a web page. Note that the safety of any data, HTML or JavaScript imported from the page is reliant on the provider and should not be used unless it can be trusted.", FileName, CodeIssue.LOW, CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "\bread\b\.\b(table|csv|csv2|delim|delim2|fwf)\b\s*\(\""\b(http|https|ftp)") Then
            ' Read from file on web or FTP server
            frmMain.ListCodeIssue("Data Imported Over Network", "The code reads a file from a web location. Note that data from a public source is reliant on the curation of the provider and their safeguards against contamination. If an unencrypted protocol is used, any network traffic (including credentials) may be sniffed by a suitably placed attacker.", FileName, CodeIssue.MEDIUM, CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "\bwrite\b\.\b(table|csv|csv2|delim|delim2|fwf)\b\s*\(\s*\w+\s*\,\s*\""\b(http|https|ftp)") Then
            ' Write to file on web or FTP server
            frmMain.ListCodeIssue("Data Exported Over Network", "The code writes a file to a remote location. Any sensitive data may be exposed to other users with access to the location. If an unencrypted protocol is used, any network traffic (including credentials) may be sniffed by a suitably placed attacker.", FileName, CodeIssue.MEDIUM, CodeLine)
        End If

    End Sub

    Private Sub CheckDatabase(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether or not code loads or saves an R Dataset from disk
        '====================================================================

        '== Check for DB connection strings ==
        If (Regex.IsMatch(CodeLine, "\w+\s*\<\-\s*\bodbcConnect\b\s*\(w+\s*\,\s*\buid\b\s*\=\s*\w+\s*\,\s*\bpwd\b\\s*\=\""\w+\""")) Then
            ' Check for data loaded from an imported package
            frmMain.ListCodeIssue("Database Password Disclosed", "The code connects to a database and discloses the password within the source code. Any developer who is able to read the source code will be able to access the database with the disclosed credentials.", FileName, CodeIssue.HIGH, CodeLine)
        ElseIf (Regex.IsMatch(CodeLine, "\w+\s*\<\-\s*\bdbConnect\b\s*\(w+\s*\,\s*\buser\b\s*\=\s*\w+\s*\,\s*\bpassword\b\\s*\=\""\w+\""")) Then
            ' Check for data loaded from an imported package
            frmMain.ListCodeIssue("Database Password Disclosed", "The code connects to a database and discloses the password within the source code. Any developer who is able to read the source code will be able to access the database with the disclosed credentials.", FileName, CodeIssue.HIGH, CodeLine)
        End If

    End Sub

    Private Sub CheckXMLJSON(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether or not code loads or saves XML or JSON data to/from disk
        '===========================================================================

        If (Regex.IsMatch(CodeLine, "\bfromJSON\b\s*\(")) Then
            ' Check for data loaded from a JSON file
            frmMain.ListCodeIssue("JSON Data Imported from File", "The code imports JSON data from a file. The data within the file may have been modified by a user or an application with access to the file and the operation should only be used on a trusted system.", FileName, CodeIssue.INFO, CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "\b(xmlToDataFrame|xmlTreeParse|xmlRoot)\b\s*\(") Then
            ' Check for data loaded from an R data file
            frmMain.ListCodeIssue("XML Data Imported from File", "The code imports XML data from a file. The data within the file may have been modified by a user or an application with access to the file and the operation should only be used on a trusted system. Note that .", FileName, CodeIssue.INFO, CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "\bwrite\b\.\bxml\b\s*\(") Then
            ' Check for data saved to an R data file
            frmMain.ListCodeIssue("Data Saved to an XML File", "The code saves data to an XML file. The file will remain on disc where it may be deliberately or inadvertently modified by a user or an application with access to the file. Any sensitive data in the file may be exposed to a third party.", FileName, CodeIssue.LOW, CodeLine)
        End If

    End Sub

    Private Sub CheckSerialization(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether or not code loads or saves an serialized objects
        '===================================================================

        '== Check for use of serialize functions ==
        If (Regex.IsMatch(CodeLine, "\breadRDS\b\s*\(")) Then
            ' Check for objects deserialized from disc
            frmMain.ListCodeIssue("Object Deserialization", "The code imports objects to be deserialized. This can allow potentially hostile objects to be instantiated directly from data held in the filesystem.", FileName, CodeIssue.STANDARD, CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "\bsaveRDS\b\s*\(") Then
            ' Check for objects serialized to disc
            frmMain.ListCodeIssue("Object Serialized to Disc", "The code serializes objects to the file system.  This can allow potentially sensitive data to be saved to the filesystem. Any sensitive data in the file may be exposed to a third party.", FileName, CodeIssue.STANDARD, CodeLine)
        End If

    End Sub

    Private Sub CheckFileAccess(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether input is imported from a file
        ' Check for any lack of validation of imported data or unsafe data use
        '=====================================================================
        Dim arrFragments As String()
        Dim strLeft As String = ""


        '== Check for file read ==
        If Regex.IsMatch(CodeLine, "\bread\b\.\b(table|csv|csv2|delim|delim2|fwf)\b\s*\(") Then

            '== Check if data imported into a variable ==
            If (Regex.IsMatch(CodeLine, "\w+\s*\<\-\s*\bread\b\.")) Then

                frmMain.ListCodeIssue("File Input Stored in Vector/Variable", "The code reads file input into a variable. If this input is subsequently used by the code without sanitisation, then it may result in unintended behaviour and may present opportunities to an attacker who can modify the file.", FileName, CodeIssue.MEDIUM, CodeLine)

                '== Add variable to list of user-controlled variables ==
                arrFragments = CodeLine.Split("<-")
                strLeft = GetLastItem(arrFragments.First)

                '== Store any discovered variables
                If strLeft <> "" Then
                    If Not ctCodeTracker.UserVariables.Contains(strLeft) Then
                        ctCodeTracker.UserVariables.Add(strLeft)
                    End If
                End If

            Else
                frmMain.ListCodeIssue("External File Input", "The code reads file input. If this input is subsequently used by the code without sanitisation, then it may result in unintended behaviour and may present opportunities to an attacker who can modify the file.", FileName, CodeIssue.LOW, CodeLine)
            End If

        ElseIf Regex.IsMatch(CodeLine, "\bcat\b\s*\(") And Regex.IsMatch(CodeLine, "\,\s*\bfile\b\s*\=\s*\""\|\w+") Then
            frmMain.ListCodeIssue("Use of System Command Line", "The code pipes R data to another application, via the command line. This may create an increased attack surface and cause subsidiary effects outside the application. The use of data produced by the R script may exacerbate te effects", FileName, CodeIssue.HIGH, CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "\bcat\b\s*\(") And Regex.IsMatch(CodeLine, "\,\s*\bfile\b\s*\=") Then
            frmMain.ListCodeIssue("Data Saved to File", "The code saves data to an external file. The file will remain on disc where it may be deliberately or inadvertently modified by a user or an application with access to the file. Any sensitive data in the file may be exposed to a third party.", FileName, CodeIssue.LOW, CodeLine)
        End If

    End Sub

    Private Sub CheckClipboardAccess(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether input is imported from a file
        ' Check for any lack of validation of imported data or unsafe data use
        '=====================================================================
        Dim arrFragments As String()
        Dim strLeft As String = ""


        '== Check for file read ==
        If (Regex.IsMatch(CodeLine, "\bfile\b\s*\=\s*\""\bclipboard\b\""") Or (Regex.IsMatch(CodeLine, "\bpipe\b\s*\(\""\bpbpaste\b\""\)"))) Then

            '== Check if data imported into a variable ==
            If (Regex.IsMatch(CodeLine, "\w+\s*\<\-\s*\bread\b\.\b(table|DIF)\b\s*\(\bfile\b\s*\=\s*\""\bclipboard\b\""") Or (Regex.IsMatch(CodeLine, "\w+\s*\<\-\s*\bread\b\.\b(table|DIF)\b\s*\(\bpipe\b\s*\(\""\bpbpaste\b\""\)"))) Then

                frmMain.ListCodeIssue("Clipboard Content Imported into Vector/Variable", "The code reads the content of the clipboard into a variable. This input is reliant on safe bahaviour by the user, and could be affected by the behaviour of other applications running on the system, and may result in unintended behaviour.", FileName, CodeIssue.MEDIUM, CodeLine)

                '== Add variable to list of user-controlled variables ==
                arrFragments = CodeLine.Split("<-")
                strLeft = GetLastItem(arrFragments.First)

                '== Store any discovered variables
                If strLeft <> "" Then
                    If Not ctCodeTracker.UserVariables.Contains(strLeft) Then
                        ctCodeTracker.UserVariables.Add(strLeft)
                    End If
                End If

            Else
                frmMain.ListCodeIssue("Use of Clipboard Content", "The code reads content from the clipboard. This input is reliant on safe bahaviour by the user, and could be affected by the behaviour of other applications running on the system, and may result in unintended behaviour.", FileName, CodeIssue.MEDIUM, CodeLine)
            End If

        End If

    End Sub

    Private Sub CheckFileOutput(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether or not the module contains code for a servlet
        ' Check for any problems specific to servlets if necessary
        '================================================================
        Dim arrFragments As String()
        Dim strRight As String = ""
        Dim blnIsUserCont As Boolean = False


        '== Check for file read ==
        If Regex.IsMatch(CodeLine, "\bwrite\b\.\b(table|csv|csv2|delim|delim2|fwf)\b\s*\(") Then

            If Regex.IsMatch(CodeLine, "\b(table|csv|csv2|delim|delim2|fwf)\b\s*\(\s*\w+\s*\,\s*(\""(C\:\\\\temp|C\:\\\\tmp|C\:\/temp|C\:\/tmp|\/tmp))") And Not ctCodeTracker.IsLstat Then
                ' Check if writing to generic/hardcoded temp file
                frmMain.ListCodeIssue("Unsafe Temporary Directory Use", "The application appears to write to a file in the 'temp' folder. Since this folder is accessible by all users and all applications, any files stored there cannot be guaranteed to be safe from modiification or from disclosing information.", FileName, CodeIssue.MEDIUM, CodeLine)
            Else
                ' Check if file path contains user-controlled variable
                arrFragments = Regex.Split(CodeLine, "\b(table|csv|csv2|delim|delim2|fwf)\b\s*\(\s*\w+\s*\,")

                strRight = arrFragments.Last.Trim()
                '== Check for command line use with user-controlled variables ==
                If strRight <> "" Then
                    For Each strItem In ctCodeTracker.UserVariables
                        If strRight.Contains(strItem) Then
                            blnIsUserCont = True
                            frmMain.ListCodeIssue("Use of User-Controlled Path", "The code writes data to a file path or file name that appears to be a user-controlled variable. This may create an opportunity to write to an arbitrary path.", FileName, CodeIssue.MEDIUM, CodeLine)
                            Exit For
                        End If
                    Next
                End If
            End If

            If Not ctCodeTracker.IsLstat Then
                ' Check if a write takes place without verifying whether the file already exists
                frmMain.ListCodeIssue("Unsafe File Write", "The application appears to write to a file without verifying that it already exists. This may result in an accidental overwrite of data.", FileName, CodeIssue.MEDIUM, CodeLine)
            Else
                ctCodeTracker.IsLstat = False
            End If
        End If

    End Sub

    Private Sub CheckFileRace(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether or not the module contains code for a servlet
        ' Check for any problems specific to servlets if necessary
        '================================================================
        Dim intSeverity As Integer = 0  ' For TOCTOU vulns, severity will be modified according to length of time between check and usage.

        '== Check for file read ==
        If Regex.IsMatch(CodeLine, "\file\b\.\b(exists)\b\s*\(") And Not Regex.IsMatch(CodeLine, "\b(read|write)\b\.\b(table|csv)\b\s*\(") Then

            ' Check has taken place - begin monitoring for use of the file/dir
            ctCodeTracker.IsLstat = True
        ElseIf ctCodeTracker.IsLstat Then

            ' Increase line count while monitoring
            If CodeLine.Trim <> "" And CodeLine.Trim <> "{" And CodeLine.Trim <> "}" Then
                ctCodeTracker.TocTouLineCount += 1
            End If

            If ctCodeTracker.TocTouLineCount < 2 And Regex.IsMatch(CodeLine, "\b(read|write)\b\.\b(table|csv)\b\s*\(") Then
                ' Usage takes place almost immediately so no problem
                ctCodeTracker.IsLstat = False
            ElseIf ctCodeTracker.TocTouLineCount > 1 And Regex.IsMatch(CodeLine, "\b(read|write)\b\.\b(table|csv)\b\s*\(") Then
                ' Usage takes place sometime later. Set severity accordingly and notify user
                ctCodeTracker.IsLstat = False
                If ctCodeTracker.TocTouLineCount > 5 Then intSeverity = 2
                frmMain.ListCodeIssue("Potential TOCTOU (Time Of Check, Time Of Use) Vulnerability", "The file.exists() check occurs " & ctCodeTracker.TocTouLineCount & " lines before the file is accessed. The longer the time between the check and any read/write, the greater the likelihood that the check will no longer be valid.", FileName)
            End If
        End If

    End Sub

    Private Sub CheckSystemInteraction(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether or not the OS command line is used
        ' Check for any use of user-controlled variables on the command line
        '===================================================================
        Dim arrFragments As String()
        Dim strLeft As String = ""
        Dim strRight As String = ""
        Dim blnIsUserCont As Boolean = False


        '== Check for interaction witht the underlying OS ==
        If Regex.IsMatch(CodeLine, "\b(command|command2|shell)\b\s*\(") Then

            '== Check for command line use ==
            arrFragments = Regex.Split(CodeLine, "\b(command|command2|shell)\b\s*\(")
            strRight = arrFragments.Last.Trim()
            '== Check for command line use with user-controlled variables ==
            If strRight <> "" Then
                For Each strItem In ctCodeTracker.UserVariables
                    If strRight.Contains(strItem) Then
                        blnIsUserCont = True
                        frmMain.ListCodeIssue("Use of System Shell/Command", "The code runs a command on the underlying operating system, and also appears to use a user-controlled variable in conjunction with the command. This may create an opportunity to run arbitrary commands in the context of the application.", FileName, CodeIssue.HIGH, CodeLine)
                        Exit For
                    End If
                Next
            End If
            ' == No user variables ==
            If blnIsUserCont = False Then frmMain.ListCodeIssue("Use of System Shell/Command", "The code runs a command on the underlying operating system. This may create an increased attack surface and subsidiary effects outside the application.", FileName, CodeIssue.MEDIUM, CodeLine)

        ElseIf Regex.IsMatch(CodeLine, "\bSys\b\.\bgetenv\b\s*\(") Then

            '== Check for environment variable assignment to another variable ==
            If (Regex.IsMatch(CodeLine, "\w+\s*\<\-\s*\w+")) Then
                ' Extract the variable name
                arrFragments = CodeLine.Split("<-")
                strLeft = GetLastItem(arrFragments.First)

                '== Store any discovered variables
                If strLeft <> "" Then
                    If Not ctCodeTracker.UserVariables.Contains(strLeft) Then
                        ctCodeTracker.UserVariables.Add(strLeft)
                    End If
                End If
                frmMain.ListCodeIssue("Use of Environment Variable", "The code assigns an environment variable to one of the code's internal variables. As the original value is accessible to external sources, then it may be modified by an attacker and result in unintended behaviour when used.", FileName, CodeIssue.MEDIUM, CodeLine)
            Else
                frmMain.ListCodeIssue("Use of Environment Variable", "The code makes use of an environment variable. As this variable is accessible to external sources, then it may be modified by an attacker and result in unintended behaviour when used.", FileName, CodeIssue.MEDIUM, CodeLine)
            End If
        End If

    End Sub

    Private Sub CheckUserInteraction(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether or not user input from the command line is used
        ' Check for any sanitisation of user-controlled variables
        '==================================================================
        Dim arrFragments As String()
        Dim strLeft As String = ""


        '== Check for command line use ==
        If Regex.IsMatch(CodeLine, "\w+\s*\=\s*\breadline\b\s*\(") Then
            '== Assinging user input to a variable ==
            frmMain.ListCodeIssue("Direct Input From User", "The code requests direct input from the user, via the command line, and assigns it to a variable. If this input is subsequently used by the code without sanitisation, then it may result in unintended behaviour.", FileName, CodeIssue.HIGH, CodeLine)

            '== Add variable to list of user-controlled variables ==
            arrFragments = CodeLine.Split("=")
            strLeft = GetLastItem(arrFragments.First)

        ElseIf Regex.IsMatch(CodeLine, "\w+\s*\<\-\s*\breadline\b\s*\(") Then
            '== Assinging user input to a variable ==
            frmMain.ListCodeIssue("Direct Input From User", "The code requests direct input from the user, via the command line, and assigns it to a variable. If this input is subsequently used by the code without sanitisation, then it may result in unintended behaviour.", FileName, CodeIssue.HIGH, CodeLine)

            '== Add variable to list of user-controlled variables ==
            arrFragments = CodeLine.Split("<-")
            strLeft = GetLastItem(arrFragments.First)

        ElseIf Regex.IsMatch(CodeLine, "\breadline\b\s*\(") Then
            '== Generic warning for user input ==
            frmMain.ListCodeIssue("Direct Input From User", "The code requests direct input from the user, via the command line. If this input is subsequently used by the code without sanitisation, then it may result in unintended behaviour.", FileName, CodeIssue.MEDIUM, CodeLine)
        End If

        '== Store any discovered variables
        If strLeft <> "" Then
            If Not ctCodeTracker.UserVariables.Contains(strLeft) Then
                ctCodeTracker.UserVariables.Add(strLeft)
            End If
        End If

    End Sub
    Private Sub CheckRandomisation(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether or not the module contains code for a servlet
        ' Check for any problems specific to servlets if necessary
        '================================================================

        '== Check for numeric seed ==
        If Regex.IsMatch(CodeLine, "\bset\b\.\bseed\b\s*\([0-9]+\)") Then
            ctCodeTracker.HasSeed = True
            frmMain.ListCodeIssue("Repeatable Pseudo-Random Values", "The code appears to set a numeric seed value. The resulting values, while appearing random to a casual observer, are repeatable and will be the same for any machine, each time the code is run.", FileName, CodeIssue.MEDIUM, CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "\brunif\b\s*\(") Then
            ctCodeTracker.HasSeed = True
            frmMain.ListCodeIssue("Repeatable Pseudo-Random Values", "The code uses the runif() (random uniform) function. The resulting values, while appearing random to a casual observer, are repeatable and will be the same each time the code is run from any machine using the same seed value.", FileName, CodeIssue.MEDIUM, CodeLine)

        End If

    End Sub

    Private Sub CheckUnsafeTempFiles(ByVal CodeLine As String, ByVal FileName As String)
        ' Identify any creation of temp files with static names
        '======================================================

        ' Check for use/assignment of temp directory prior to file read/write 
        If Regex.IsMatch(CodeLine, "\bsetwd\b\s*\(\""(C\:\\\\temp|C\:\\\\tmp|C\:\/temp|C\:\/tmp|\/tmp)") Then
            frmMain.ListCodeIssue("Unsafe Temporary Directory Use", "The application appears to set its working directory to the 'temp' folder. Since this folder is accessible by all users and all applications, any files stored there cannot be guaranteed to be safe from modiification or from disclosing information.", FileName, CodeIssue.MEDIUM, CodeLine)
        ElseIf Regex.IsMatch(CodeLine, "\bfile\b\.\bpath\b\s*\(\""(C\:\\\\temp|C\:\\\\tmp|C\:\/temp|C\:\/tmp|\/tmp)") Or Regex.IsMatch(CodeLine, "\bfile\b\.\bpath\b\s*\(\""(C|c)\:\""\s*\,\s*\""temp|tmp\""") Then
            frmMain.ListCodeIssue("Unsafe Temporary Directory Use", "The application appears to set a file path to the 'temp' folder. Since this folder is accessible by all users and all applications, any files stored there cannot be guaranteed to be safe from modiification or from disclosing information. Additonally, if a generic or predictable hardcoded file name and path is used, there is a chance that it may clash with a name/path used by another application.", FileName, CodeIssue.MEDIUM, CodeLine)
        End If

    End Sub

End Module
