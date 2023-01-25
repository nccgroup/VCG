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

Imports System.Text.RegularExpressions


Public Class frmOptions

    Private Sub btnCPPEdit_Click(sender As System.Object, e As System.EventArgs) Handles btnCPPEdit.Click
        LaunchNPP(asAppSettings.CConfFile)
    End Sub

    Private Sub btnJavaEdit_Click(sender As System.Object, e As System.EventArgs) Handles btnJavaEdit.Click
        LaunchNPP(asAppSettings.JavaConfFile)
    End Sub

    Private Sub btnSQLEdit_Click(sender As System.Object, e As System.EventArgs) Handles btnSQLEdit.Click
        LaunchNPP(asAppSettings.PLSQLConfFile)
    End Sub

    Private Sub btnCSharpEdit_Click(sender As System.Object, e As System.EventArgs) Handles btnCSharpEdit.Click
        LaunchNPP(asAppSettings.CSharpConfFile)
    End Sub

    Private Sub btnVBEdit_Click(sender As System.Object, e As System.EventArgs) Handles btnVBEdit.Click
        LaunchNPP(asAppSettings.VBConfFile)
    End Sub

    Private Sub btnPHPEdit_Click(sender As System.Object, e As System.EventArgs) Handles btnPHPEdit.Click
        LaunchNPP(asAppSettings.PHPConfFile)
    End Sub

    Private Sub btnCobolEdit_Click(sender As System.Object, e As System.EventArgs) Handles btnCobolEdit.Click
        LaunchNPP(asAppSettings.COBOLConfFile)
    End Sub

    Private Sub btnOK_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
        ' Apply all new settings and exit
        '================================

        ' Assign the new file suffixes to the appropriate language
        AssignFileSuffixes()

        ' Set test language 
        SelectLanguage(cboCurrentLanguage.SelectedIndex)
        asAppSettings.StartType = cboStartUpLanguage.SelectedIndex

        ' Set conf file locations
        If txtCPP.Text.Trim() <> "" Then asAppSettings.CConfFile = txtCPP.Text.Trim()
        If txtJava.Text.Trim() <> "" Then asAppSettings.JavaConfFile = txtJava.Text.Trim()
        If txtPLSQL.Text.Trim() <> "" Then asAppSettings.PLSQLConfFile = txtPLSQL.Text.Trim()
        If txtCSharp.Text.Trim() <> "" Then asAppSettings.CSharpConfFile = txtCSharp.Text.Trim()
        If txtVB.Text.Trim() <> "" Then asAppSettings.VBConfFile = txtVB.Text.Trim()
        If txtPHP.Text.Trim() <> "" Then asAppSettings.PHPConfFile = txtPHP.Text.Trim()
        If txtCobol.Text.Trim() <> "" Then asAppSettings.COBOLConfFile = txtCobol.Text.Trim()

        ' Set reporting level - the reverse order in the dropdown list necessitates "7 - selected val"
        asAppSettings.OutputLevel = 7 - cboReporting.SelectedIndex


        '======= Java settings =======
        ' Set OWASP compliance
        asAppSettings.IsFinalizeCheck = cbFinalize.Checked
        asAppSettings.IsInnerClassCheck = cbInnerClass.Checked
        'Android checks
        asAppSettings.IsAndroid = cbAndroid.Checked
        '----------------------------------------------

        '======= COBOL settings =======
        ' Set width of left hand column to account for linenumbers, copybook names, etc.
        If udCOBOLStart.Value >= 1 Then
            asAppSettings.COBOLStartColumn = udCOBOLStart.Value
        Else
            udCOBOLStart.Value = 1
        End If
        asAppSettings.IsZOS = cbZOS.Checked
        '----------------------------------------------

        ' Set output file
        If asAppSettings.IsOutputFile = True And txtOutput.Text.Trim() <> "" Then asAppSettings.OutputFile = txtOutput.Text.Trim

        ' Set XML Export options
        frmMain.SaveCheckState = cbSaveState.Checked
        frmMain.SaveFiltered = rbFiltered.Checked

        ' Include any required beta functionality
        SetBetaDetails(cbSigned.Checked, cbCobol.Checked)

        ' Load contents of temporary grep box into bad function array
        If txtTempGrep.Text.Trim = "" Then
            asAppSettings.TempGrepText = ""
            modMain.LoadUnsafeFunctionList(asAppSettings.TestType)
            Exit Sub
        Else
            asAppSettings.TempGrepText = txtTempGrep.Text
            LoadTempGrepContent(txtTempGrep.Text)
        End If


        If (rbFiltered.Checked And CheckFilters() = False) Then Exit Sub

        Me.Close()

    End Sub

    Private Sub cbOutput_CheckedChanged(sender As System.Object, e As System.EventArgs)
        asAppSettings.IsOutputFile = cbOutput.CheckState
    End Sub

    Private Sub AssignFileSuffixes()
        ' Assign the chosen suffixes to the relevant language
        '====================================================

        Select Case cboFileTypes.SelectedIndex
            Case AppSettings.C
                asAppSettings.CSuffixes = txtFileTypes.Text
            Case AppSettings.JAVA
                asAppSettings.JavaSuffixes = txtFileTypes.Text
            Case AppSettings.SQL
                asAppSettings.PLSQLSuffixes = txtFileTypes.Text
            Case AppSettings.CSHARP
                asAppSettings.CSharpSuffixes = txtFileTypes.Text
            Case AppSettings.VB
                asAppSettings.VBSuffixes = txtFileTypes.Text
            Case AppSettings.PHP
                asAppSettings.PHPSuffixes = txtFileTypes.Text
            Case AppSettings.COBOL
                asAppSettings.COBOLSuffixes = txtFileTypes.Text
        End Select

    End Sub

    Private Sub frmOptions_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' Get current settings and load values into controls
        '===================================================

        ' Current language
        cboCurrentLanguage.SelectedIndex = asAppSettings.TestType
        cboStartUpLanguage.SelectedIndex = asAppSettings.StartType

        ' File suffixes
        cboFileTypes.SelectedIndex = asAppSettings.TestType

        ' Config files
        txtCPP.Text = asAppSettings.CConfFile
        txtJava.Text = asAppSettings.JavaConfFile
        txtPLSQL.Text = asAppSettings.PLSQLConfFile
        txtCSharp.Text = asAppSettings.CSharpConfFile
        txtVB.Text = asAppSettings.VBConfFile
        txtPHP.Text = asAppSettings.PHPConfFile
        txtCobol.Text = asAppSettings.COBOLConfFile

        ' Output settings
        cboReporting.SelectedIndex = 7 - asAppSettings.OutputLevel

        ' OWASP compliance
        cbFinalize.Checked = asAppSettings.IsFinalizeCheck
        cbInnerClass.Checked = asAppSettings.IsInnerClassCheck

        ' Android checks
        cbAndroid.Checked = asAppSettings.IsAndroid

        ' COBOL settings
        udCOBOLStart.Value = asAppSettings.COBOLStartColumn
        cbZOS.Checked = asAppSettings.IsZOS

        ' Output file
        cbOutput.Checked = asAppSettings.IsOutputFile
        txtOutput.Text = asAppSettings.OutputFile

        ' XML file and filter
        cbSaveState.Checked = frmMain.SaveCheckState
        rbFiltered.Checked = frmMain.SaveFiltered
        rbAll.Checked = Not frmMain.SaveFiltered
        SetFilterDetails()

        ' Beta functionality
        cbSigned.Checked = asAppSettings.IncludeSigned
        cbCobol.Checked = asAppSettings.IncludeCobol
        SetBetaDetails(asAppSettings.IncludeSigned, asAppSettings.IncludeCobol)

        ' Temporary Grep text
        txtTempGrep.Text = asAppSettings.TempGrepText

    End Sub

    Private Sub cboFileTypes_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboFileTypes.SelectedIndexChanged
        ' Display suffixes for selected language
        '=======================================

        Select Case cboFileTypes.SelectedIndex
            Case AppSettings.C
                txtFileTypes.Text = asAppSettings.CSuffixes
            Case AppSettings.JAVA
                txtFileTypes.Text = asAppSettings.JavaSuffixes
            Case AppSettings.SQL
                txtFileTypes.Text = asAppSettings.PLSQLSuffixes
            Case AppSettings.CSHARP
                txtFileTypes.Text = asAppSettings.CSharpSuffixes
            Case AppSettings.VB
                txtFileTypes.Text = asAppSettings.VBSuffixes
            Case AppSettings.PHP
                txtFileTypes.Text = asAppSettings.PHPSuffixes
            Case AppSettings.COBOL
                txtFileTypes.Text = asAppSettings.COBOLSuffixes
        End Select

    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnCPPBrowse_Click(sender As System.Object, e As System.EventArgs) Handles btnCPPBrowse.Click
        ' Show dialog box for new C++ config file
        '========================================

        ' Get target directory
        ofdOpenFileDialog.ShowDialog()

        If Not Windows.Forms.DialogResult.Cancel And ofdOpenFileDialog.FileName <> "" Then
            txtCPP.Text = ofdOpenFileDialog.FileName
            asAppSettings.CConfFile = ofdOpenFileDialog.FileName
        End If

    End Sub

    Private Sub btnJavaBrowse_Click(sender As System.Object, e As System.EventArgs) Handles btnJavaBrowse.Click
        ' Show dialog box for new Java config file
        '========================================

        ' Get target directory
        ofdOpenFileDialog.ShowDialog()

        If Not Windows.Forms.DialogResult.Cancel And ofdOpenFileDialog.FileName <> "" Then
            txtJava.Text = ofdOpenFileDialog.FileName
            asAppSettings.JavaConfFile = ofdOpenFileDialog.FileName
        End If

    End Sub

    Private Sub btnSQLBrowse_Click(sender As System.Object, e As System.EventArgs) Handles btnSQLBrowse.Click
        ' Show dialog box for new PL/SQL config file
        '===========================================

        ' Get target directory
        ofdOpenFileDialog.ShowDialog()

        If Not Windows.Forms.DialogResult.Cancel And ofdOpenFileDialog.FileName <> "" Then
            txtPLSQL.Text = ofdOpenFileDialog.FileName
            asAppSettings.PLSQLConfFile = ofdOpenFileDialog.FileName
        End If

    End Sub

    Private Sub btnVBBrowse_Click(sender As System.Object, e As System.EventArgs) Handles btnVBBrowse.Click
        ' Show dialog box for new VB config file
        '=======================================

        ' Get target directory
        ofdOpenFileDialog.ShowDialog()

        If Not Windows.Forms.DialogResult.Cancel And ofdOpenFileDialog.FileName <> "" Then
            txtVB.Text = ofdOpenFileDialog.FileName
            asAppSettings.VBConfFile = ofdOpenFileDialog.FileName
        End If
    End Sub

    Private Sub btnPHPBrowse_Click(sender As System.Object, e As System.EventArgs) Handles btnPHPBrowse.Click
        ' Show dialog box for new PHP config file
        '=======================================

        ' Get target directory
        ofdOpenFileDialog.ShowDialog()

        If Not Windows.Forms.DialogResult.Cancel And ofdOpenFileDialog.FileName <> "" Then
            txtPHP.Text = ofdOpenFileDialog.FileName
            asAppSettings.PHPConfFile = ofdOpenFileDialog.FileName
        End If
    End Sub

    Private Sub btnCobolBrowse_Click(sender As System.Object, e As System.EventArgs) Handles btnCobolBrowse.Click
        ' Show dialog box for new COBOL config file
        '==========================================

        ' Get target directory
        ofdOpenFileDialog.ShowDialog()

        If Not Windows.Forms.DialogResult.Cancel And ofdOpenFileDialog.FileName <> "" Then
            txtCobol.Text = ofdOpenFileDialog.FileName
            asAppSettings.COBOLConfFile = ofdOpenFileDialog.FileName
        End If

    End Sub

    Private Sub btnOutputBrowse_Click(sender As System.Object, e As System.EventArgs)
        ' Show dialog box for new output file
        '====================================

        ' Get target directory
        sfdSaveFileDialog.ShowDialog()

        If Not Windows.Forms.DialogResult.Cancel Then
            txtOutput.Text = sfdSaveFileDialog.FileName
            asAppSettings.CConfFile = sfdSaveFileDialog.FileName
        End If

    End Sub

    Private Sub btnCSharpBrowse_Click(sender As System.Object, e As System.EventArgs) Handles btnCSharpBrowse.Click
        ' Show dialog box for new C++ config file
        '========================================

        ' Get target directory
        ofdOpenFileDialog.ShowDialog()

        If Not Windows.Forms.DialogResult.Cancel And ofdOpenFileDialog.FileName <> "" Then
            txtCSharp.Text = ofdOpenFileDialog.FileName
            asAppSettings.CSharpConfFile = ofdOpenFileDialog.FileName
        End If

    End Sub

    Private Sub rbAbove_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbAbove.CheckedChanged
        ' Enable/disable relevant controls
        '=================================
        cboAbove.Enabled = True
        cboBelow.Enabled = False
        cboMinimum.Enabled = False
        cboMaximum.Enabled = False
    End Sub

    Private Sub rbBelow_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbBelow.CheckedChanged
        ' Enable/disable relevant controls
        '=================================
        cboBelow.Enabled = True
        cboAbove.Enabled = False
        cboMinimum.Enabled = False
        cboMaximum.Enabled = False
    End Sub

    Private Sub rbRange_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbRange.CheckedChanged
        ' Enable/disable relevant controls
        '=================================
        cboMinimum.Enabled = True
        cboMaximum.Enabled = True
        cboBelow.Enabled = False
        cboAbove.Enabled = False
    End Sub

    Private Sub btnExport_Click(sender As System.Object, e As System.EventArgs) Handles btnExport.Click
        ' Assign new values and then export to XML file
        '==============================================
        CheckFilters()
        frmMain.ExportResultsXML()
    End Sub

    Private Function CheckFilters() As Boolean
        'Check filter values are valid
        '=============================
        Dim blnRetVal As Boolean = True
        Dim intMinimum As Integer = CodeIssue.POSSIBLY_SAFE
        Dim intMaximum As Integer = CodeIssue.CRITICAL

        If rbAbove.Checked = True Then
            intMinimum = 7 - cboAbove.SelectedIndex
        ElseIf rbBelow.Checked = True Then
            intMaximum = 7 - cboBelow.SelectedIndex
        Else
            intMinimum = 7 - cboMinimum.SelectedIndex
            intMaximum = 7 - cboMaximum.SelectedIndex
            If intMaximum > intMinimum Then
                MsgBox("Maximum value cannot be less than minmum value.", vbOK, "Invalid Values")
                blnRetVal = False
            End If
        End If

        If blnRetVal Then
            frmMain.FilterResults(intMinimum, intMaximum)
        End If

        Return blnRetVal

    End Function

    Private Sub SetFilterDetails()
        ' Start with a correct and valid option selected
        '===============================================

        cboAbove.SelectedIndex = 0
        cboBelow.SelectedIndex = 0
        cboMinimum.SelectedIndex = 0
        cboMaximum.SelectedIndex = 6

        If frmMain.SaveFiltered Then
            rbRange.Checked = True
            cboMinimum.SelectedIndex = 7 - frmMain.intFilterMin
            cboMaximum.SelectedIndex = 7 - frmMain.intFilterMax
        End If

    End Sub

    Private Sub SetBetaDetails(IncludeSigned As Boolean, IncludeCobol As Boolean)
        'Implement any beta functionality that the user requires
        '=======================================================

        ' C/C++ signed/unsigned comparison
        asAppSettings.IncludeSigned = IncludeSigned

        ' COBOL scanning
        'lblCobol.Visible = IncludeCobol
        'txtCobol.Visible = IncludeCobol
        'btnCobolBrowse.Visible = IncludeCobol
        'btnCobolEdit.Visible = IncludeCobol

        ' Enable/disable controls on main form
        'frmMain.COBOLToolStripMenuItem.Visible = IncludeCobol

        'If IncludeCobol = True Then
        'cboCurrentLanguage.Items.Add("COBOL")
        'cboStartUpLanguage.Items.Add("COBOL")
        'Else
        'cboCurrentLanguage.Items.Remove("COBOL")
        'cboStartUpLanguage.Items.Remove("COBOL")
        'End If

    End Sub

    Private Sub btnColour_Click(sender As System.Object, e As System.EventArgs) Handles btnColour.Click
        ' Allow user to modify the colour for checked listbox items
        '==========================================================

        If cdColorDialog.ShowDialog() <> Windows.Forms.DialogResult.Cancel Then
            asAppSettings.ListItemColour = cdColorDialog.Color
        End If

    End Sub

    Public Sub LoadTempGrepContent(TempGrepText As String)
        ' Take content of temp grep box and add to the list of bad functions
        '===================================================================
        Dim arrTempGrepContent As String()
        Dim strDescription As String = ""
        Dim arrFuncList As String()


        arrTempGrepContent = TempGrepText.Split(vbNewLine)

        Try
            For Each strLine In arrTempGrepContent

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

    End Sub

End Class