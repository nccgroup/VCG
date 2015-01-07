<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOptions
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOptions))
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.ofdOpenFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.sfdSaveFileDialog = New System.Windows.Forms.SaveFileDialog()
        Me.tabOptions = New System.Windows.Forms.TabControl()
        Me.tpGeneral = New System.Windows.Forms.TabPage()
        Me.gbReporting = New System.Windows.Forms.GroupBox()
        Me.cboReporting = New System.Windows.Forms.ComboBox()
        Me.lblReporting = New System.Windows.Forms.Label()
        Me.gbOWASP = New System.Windows.Forms.GroupBox()
        Me.cbInnerClass = New System.Windows.Forms.CheckBox()
        Me.cbFinalize = New System.Windows.Forms.CheckBox()
        Me.gbOutput = New System.Windows.Forms.GroupBox()
        Me.cbOutput = New System.Windows.Forms.CheckBox()
        Me.txtOutput = New System.Windows.Forms.TextBox()
        Me.btnOutputBrowse = New System.Windows.Forms.Button()
        Me.gbFileTypes = New System.Windows.Forms.GroupBox()
        Me.lblExplain = New System.Windows.Forms.Label()
        Me.txtFileTypes = New System.Windows.Forms.TextBox()
        Me.cboFileTypes = New System.Windows.Forms.ComboBox()
        Me.gbLanguage = New System.Windows.Forms.GroupBox()
        Me.cboStartUpLanguage = New System.Windows.Forms.ComboBox()
        Me.cboCurrentLanguage = New System.Windows.Forms.ComboBox()
        Me.lblStartUpLanguage = New System.Windows.Forms.Label()
        Me.lblCurrentLanguage = New System.Windows.Forms.Label()
        Me.tpConfigFiles = New System.Windows.Forms.TabPage()
        Me.gbConfigFiles = New System.Windows.Forms.GroupBox()
        Me.btnCobolEdit = New System.Windows.Forms.Button()
        Me.btnCobolBrowse = New System.Windows.Forms.Button()
        Me.txtCobol = New System.Windows.Forms.TextBox()
        Me.lblCobol = New System.Windows.Forms.Label()
        Me.btnPHPEdit = New System.Windows.Forms.Button()
        Me.btnPHPBrowse = New System.Windows.Forms.Button()
        Me.txtPHP = New System.Windows.Forms.TextBox()
        Me.lblPHP = New System.Windows.Forms.Label()
        Me.btnVBEdit = New System.Windows.Forms.Button()
        Me.btnVBBrowse = New System.Windows.Forms.Button()
        Me.txtVB = New System.Windows.Forms.TextBox()
        Me.lblVB = New System.Windows.Forms.Label()
        Me.btnCSharpEdit = New System.Windows.Forms.Button()
        Me.btnCSharpBrowse = New System.Windows.Forms.Button()
        Me.txtCSharp = New System.Windows.Forms.TextBox()
        Me.lblCSharp = New System.Windows.Forms.Label()
        Me.btnCPPBrowse = New System.Windows.Forms.Button()
        Me.txtCPP = New System.Windows.Forms.TextBox()
        Me.btnSQLEdit = New System.Windows.Forms.Button()
        Me.btnSQLBrowse = New System.Windows.Forms.Button()
        Me.txtPLSQL = New System.Windows.Forms.TextBox()
        Me.btnJavaEdit = New System.Windows.Forms.Button()
        Me.btnJavaBrowse = New System.Windows.Forms.Button()
        Me.txtJava = New System.Windows.Forms.TextBox()
        Me.btnCPPEdit = New System.Windows.Forms.Button()
        Me.lblSQL = New System.Windows.Forms.Label()
        Me.lblJava = New System.Windows.Forms.Label()
        Me.lblCPP = New System.Windows.Forms.Label()
        Me.tpXMLExport = New System.Windows.Forms.TabPage()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.gbFilter = New System.Windows.Forms.GroupBox()
        Me.cboMaximum = New System.Windows.Forms.ComboBox()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.rbRange = New System.Windows.Forms.RadioButton()
        Me.cboMinimum = New System.Windows.Forms.ComboBox()
        Me.rbBelow = New System.Windows.Forms.RadioButton()
        Me.cboBelow = New System.Windows.Forms.ComboBox()
        Me.rbAbove = New System.Windows.Forms.RadioButton()
        Me.cboAbove = New System.Windows.Forms.ComboBox()
        Me.gbExportMode = New System.Windows.Forms.GroupBox()
        Me.cbSaveState = New System.Windows.Forms.CheckBox()
        Me.rbFiltered = New System.Windows.Forms.RadioButton()
        Me.rbAll = New System.Windows.Forms.RadioButton()
        Me.tpDisplay = New System.Windows.Forms.TabPage()
        Me.gbDisplay = New System.Windows.Forms.GroupBox()
        Me.lblColour = New System.Windows.Forms.Label()
        Me.btnColour = New System.Windows.Forms.Button()
        Me.cbReminder = New System.Windows.Forms.CheckBox()
        Me.cbShowChart = New System.Windows.Forms.CheckBox()
        Me.cdColorDialog = New System.Windows.Forms.ColorDialog()
        Me.cbShowStatusBar = New System.Windows.Forms.CheckBox()
        Me.tabOptions.SuspendLayout()
        Me.tpGeneral.SuspendLayout()
        Me.gbReporting.SuspendLayout()
        Me.gbOWASP.SuspendLayout()
        Me.gbOutput.SuspendLayout()
        Me.gbFileTypes.SuspendLayout()
        Me.gbLanguage.SuspendLayout()
        Me.tpConfigFiles.SuspendLayout()
        Me.gbConfigFiles.SuspendLayout()
        Me.tpXMLExport.SuspendLayout()
        Me.gbFilter.SuspendLayout()
        Me.gbExportMode.SuspendLayout()
        Me.tpDisplay.SuspendLayout()
        Me.gbDisplay.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(400, 349)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 0
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(481, 349)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 1
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'tabOptions
        '
        Me.tabOptions.Controls.Add(Me.tpGeneral)
        Me.tabOptions.Controls.Add(Me.tpConfigFiles)
        Me.tabOptions.Controls.Add(Me.tpXMLExport)
        Me.tabOptions.Controls.Add(Me.tpDisplay)
        Me.tabOptions.Location = New System.Drawing.Point(1, 2)
        Me.tabOptions.Name = "tabOptions"
        Me.tabOptions.SelectedIndex = 0
        Me.tabOptions.Size = New System.Drawing.Size(559, 341)
        Me.tabOptions.TabIndex = 8
        '
        'tpGeneral
        '
        Me.tpGeneral.Controls.Add(Me.gbReporting)
        Me.tpGeneral.Controls.Add(Me.gbOWASP)
        Me.tpGeneral.Controls.Add(Me.gbOutput)
        Me.tpGeneral.Controls.Add(Me.gbFileTypes)
        Me.tpGeneral.Controls.Add(Me.gbLanguage)
        Me.tpGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tpGeneral.Name = "tpGeneral"
        Me.tpGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tpGeneral.Size = New System.Drawing.Size(551, 315)
        Me.tpGeneral.TabIndex = 0
        Me.tpGeneral.Text = "General"
        Me.tpGeneral.UseVisualStyleBackColor = True
        '
        'gbReporting
        '
        Me.gbReporting.Controls.Add(Me.cboReporting)
        Me.gbReporting.Controls.Add(Me.lblReporting)
        Me.gbReporting.Location = New System.Drawing.Point(8, 139)
        Me.gbReporting.Name = "gbReporting"
        Me.gbReporting.Size = New System.Drawing.Size(536, 46)
        Me.gbReporting.TabIndex = 10
        Me.gbReporting.TabStop = False
        Me.gbReporting.Text = "Results/Reporting"
        '
        'cboReporting
        '
        Me.cboReporting.FormattingEnabled = True
        Me.cboReporting.Items.AddRange(New Object() {"Potentially Unsafe", "Suspicious Comment", "Low", "Standard", "Medium", "High", "Critical"})
        Me.cboReporting.Location = New System.Drawing.Point(245, 17)
        Me.cboReporting.Name = "cboReporting"
        Me.cboReporting.Size = New System.Drawing.Size(199, 21)
        Me.cboReporting.TabIndex = 1
        '
        'lblReporting
        '
        Me.lblReporting.AutoSize = True
        Me.lblReporting.Location = New System.Drawing.Point(12, 20)
        Me.lblReporting.Name = "lblReporting"
        Me.lblReporting.Size = New System.Drawing.Size(215, 13)
        Me.lblReporting.TabIndex = 0
        Me.lblReporting.Text = "Display/Scan for Results Equal to or Above:"
        '
        'gbOWASP
        '
        Me.gbOWASP.Controls.Add(Me.cbInnerClass)
        Me.gbOWASP.Controls.Add(Me.cbFinalize)
        Me.gbOWASP.Location = New System.Drawing.Point(8, 191)
        Me.gbOWASP.Name = "gbOWASP"
        Me.gbOWASP.Size = New System.Drawing.Size(536, 39)
        Me.gbOWASP.TabIndex = 9
        Me.gbOWASP.TabStop = False
        Me.gbOWASP.Text = "OWASP Recommendations"
        '
        'cbInnerClass
        '
        Me.cbInnerClass.AutoSize = True
        Me.cbInnerClass.Location = New System.Drawing.Point(251, 16)
        Me.cbInnerClass.Name = "cbInnerClass"
        Me.cbInnerClass.Size = New System.Drawing.Size(174, 17)
        Me.cbInnerClass.TabIndex = 1
        Me.cbInnerClass.Text = "Check for Nested Java Classes"
        Me.cbInnerClass.UseVisualStyleBackColor = True
        '
        'cbFinalize
        '
        Me.cbFinalize.AutoSize = True
        Me.cbFinalize.Location = New System.Drawing.Point(12, 16)
        Me.cbFinalize.Name = "cbFinalize"
        Me.cbFinalize.Size = New System.Drawing.Size(204, 17)
        Me.cbFinalize.TabIndex = 0
        Me.cbFinalize.Text = "Check for Finalization of Java Classes"
        Me.cbFinalize.UseVisualStyleBackColor = True
        '
        'gbOutput
        '
        Me.gbOutput.Controls.Add(Me.cbOutput)
        Me.gbOutput.Controls.Add(Me.txtOutput)
        Me.gbOutput.Controls.Add(Me.btnOutputBrowse)
        Me.gbOutput.Location = New System.Drawing.Point(8, 233)
        Me.gbOutput.Name = "gbOutput"
        Me.gbOutput.Size = New System.Drawing.Size(536, 72)
        Me.gbOutput.TabIndex = 8
        Me.gbOutput.TabStop = False
        Me.gbOutput.Text = "Output"
        '
        'cbOutput
        '
        Me.cbOutput.AutoSize = True
        Me.cbOutput.Location = New System.Drawing.Point(12, 20)
        Me.cbOutput.Name = "cbOutput"
        Me.cbOutput.Size = New System.Drawing.Size(117, 17)
        Me.cbOutput.TabIndex = 0
        Me.cbOutput.Text = "Write to Output File"
        Me.cbOutput.UseVisualStyleBackColor = True
        '
        'txtOutput
        '
        Me.txtOutput.Location = New System.Drawing.Point(9, 43)
        Me.txtOutput.Name = "txtOutput"
        Me.txtOutput.Size = New System.Drawing.Size(491, 20)
        Me.txtOutput.TabIndex = 3
        '
        'btnOutputBrowse
        '
        Me.btnOutputBrowse.Location = New System.Drawing.Point(506, 40)
        Me.btnOutputBrowse.Name = "btnOutputBrowse"
        Me.btnOutputBrowse.Size = New System.Drawing.Size(24, 23)
        Me.btnOutputBrowse.TabIndex = 4
        Me.btnOutputBrowse.Text = "..."
        Me.btnOutputBrowse.UseVisualStyleBackColor = True
        '
        'gbFileTypes
        '
        Me.gbFileTypes.Controls.Add(Me.lblExplain)
        Me.gbFileTypes.Controls.Add(Me.txtFileTypes)
        Me.gbFileTypes.Controls.Add(Me.cboFileTypes)
        Me.gbFileTypes.Location = New System.Drawing.Point(8, 68)
        Me.gbFileTypes.Name = "gbFileTypes"
        Me.gbFileTypes.Size = New System.Drawing.Size(536, 67)
        Me.gbFileTypes.TabIndex = 5
        Me.gbFileTypes.TabStop = False
        Me.gbFileTypes.Text = "File Types"
        '
        'lblExplain
        '
        Me.lblExplain.AutoSize = True
        Me.lblExplain.Location = New System.Drawing.Point(6, 44)
        Me.lblExplain.Name = "lblExplain"
        Me.lblExplain.Size = New System.Drawing.Size(478, 13)
        Me.lblExplain.TabIndex = 2
        Me.lblExplain.Text = "(Use .* to scan all file types but this will impact speed and potentially include" & _
    " non-code files in results)"
        '
        'txtFileTypes
        '
        Me.txtFileTypes.Location = New System.Drawing.Point(121, 20)
        Me.txtFileTypes.Name = "txtFileTypes"
        Me.txtFileTypes.Size = New System.Drawing.Size(409, 20)
        Me.txtFileTypes.TabIndex = 1
        '
        'cboFileTypes
        '
        Me.cboFileTypes.FormattingEnabled = True
        Me.cboFileTypes.Items.AddRange(New Object() {"C/C++", "Java", "PL/SQL", "C#", "VB", "PHP"})
        Me.cboFileTypes.Location = New System.Drawing.Point(9, 20)
        Me.cboFileTypes.Name = "cboFileTypes"
        Me.cboFileTypes.Size = New System.Drawing.Size(105, 21)
        Me.cboFileTypes.TabIndex = 0
        '
        'gbLanguage
        '
        Me.gbLanguage.Controls.Add(Me.cboStartUpLanguage)
        Me.gbLanguage.Controls.Add(Me.cboCurrentLanguage)
        Me.gbLanguage.Controls.Add(Me.lblStartUpLanguage)
        Me.gbLanguage.Controls.Add(Me.lblCurrentLanguage)
        Me.gbLanguage.Location = New System.Drawing.Point(8, 10)
        Me.gbLanguage.Name = "gbLanguage"
        Me.gbLanguage.Size = New System.Drawing.Size(536, 52)
        Me.gbLanguage.TabIndex = 4
        Me.gbLanguage.TabStop = False
        Me.gbLanguage.Text = "Language"
        '
        'cboStartUpLanguage
        '
        Me.cboStartUpLanguage.FormattingEnabled = True
        Me.cboStartUpLanguage.Items.AddRange(New Object() {"C/C++", "Java", "PL/SQL", "C#", "VB", "PHP"})
        Me.cboStartUpLanguage.Location = New System.Drawing.Point(364, 20)
        Me.cboStartUpLanguage.Name = "cboStartUpLanguage"
        Me.cboStartUpLanguage.Size = New System.Drawing.Size(100, 21)
        Me.cboStartUpLanguage.TabIndex = 3
        '
        'cboCurrentLanguage
        '
        Me.cboCurrentLanguage.FormattingEnabled = True
        Me.cboCurrentLanguage.Items.AddRange(New Object() {"C/C++", "Java", "PL/SQL", "C#", "VB", "PHP"})
        Me.cboCurrentLanguage.Location = New System.Drawing.Point(118, 20)
        Me.cboCurrentLanguage.Name = "cboCurrentLanguage"
        Me.cboCurrentLanguage.Size = New System.Drawing.Size(100, 21)
        Me.cboCurrentLanguage.TabIndex = 2
        '
        'lblStartUpLanguage
        '
        Me.lblStartUpLanguage.AutoSize = True
        Me.lblStartUpLanguage.Location = New System.Drawing.Point(258, 23)
        Me.lblStartUpLanguage.Name = "lblStartUpLanguage"
        Me.lblStartUpLanguage.Size = New System.Drawing.Size(100, 13)
        Me.lblStartUpLanguage.TabIndex = 1
        Me.lblStartUpLanguage.Text = "Start Up Language:"
        '
        'lblCurrentLanguage
        '
        Me.lblCurrentLanguage.AutoSize = True
        Me.lblCurrentLanguage.Location = New System.Drawing.Point(9, 23)
        Me.lblCurrentLanguage.Name = "lblCurrentLanguage"
        Me.lblCurrentLanguage.Size = New System.Drawing.Size(95, 13)
        Me.lblCurrentLanguage.TabIndex = 0
        Me.lblCurrentLanguage.Text = "Current Language:"
        '
        'tpConfigFiles
        '
        Me.tpConfigFiles.Controls.Add(Me.gbConfigFiles)
        Me.tpConfigFiles.Location = New System.Drawing.Point(4, 22)
        Me.tpConfigFiles.Name = "tpConfigFiles"
        Me.tpConfigFiles.Padding = New System.Windows.Forms.Padding(3)
        Me.tpConfigFiles.Size = New System.Drawing.Size(551, 315)
        Me.tpConfigFiles.TabIndex = 1
        Me.tpConfigFiles.Text = "Config Files"
        Me.tpConfigFiles.UseVisualStyleBackColor = True
        '
        'gbConfigFiles
        '
        Me.gbConfigFiles.Controls.Add(Me.btnCobolEdit)
        Me.gbConfigFiles.Controls.Add(Me.btnCobolBrowse)
        Me.gbConfigFiles.Controls.Add(Me.txtCobol)
        Me.gbConfigFiles.Controls.Add(Me.lblCobol)
        Me.gbConfigFiles.Controls.Add(Me.btnPHPEdit)
        Me.gbConfigFiles.Controls.Add(Me.btnPHPBrowse)
        Me.gbConfigFiles.Controls.Add(Me.txtPHP)
        Me.gbConfigFiles.Controls.Add(Me.lblPHP)
        Me.gbConfigFiles.Controls.Add(Me.btnVBEdit)
        Me.gbConfigFiles.Controls.Add(Me.btnVBBrowse)
        Me.gbConfigFiles.Controls.Add(Me.txtVB)
        Me.gbConfigFiles.Controls.Add(Me.lblVB)
        Me.gbConfigFiles.Controls.Add(Me.btnCSharpEdit)
        Me.gbConfigFiles.Controls.Add(Me.btnCSharpBrowse)
        Me.gbConfigFiles.Controls.Add(Me.txtCSharp)
        Me.gbConfigFiles.Controls.Add(Me.lblCSharp)
        Me.gbConfigFiles.Controls.Add(Me.btnCPPBrowse)
        Me.gbConfigFiles.Controls.Add(Me.txtCPP)
        Me.gbConfigFiles.Controls.Add(Me.btnSQLEdit)
        Me.gbConfigFiles.Controls.Add(Me.btnSQLBrowse)
        Me.gbConfigFiles.Controls.Add(Me.txtPLSQL)
        Me.gbConfigFiles.Controls.Add(Me.btnJavaEdit)
        Me.gbConfigFiles.Controls.Add(Me.btnJavaBrowse)
        Me.gbConfigFiles.Controls.Add(Me.txtJava)
        Me.gbConfigFiles.Controls.Add(Me.btnCPPEdit)
        Me.gbConfigFiles.Controls.Add(Me.lblSQL)
        Me.gbConfigFiles.Controls.Add(Me.lblJava)
        Me.gbConfigFiles.Controls.Add(Me.lblCPP)
        Me.gbConfigFiles.Location = New System.Drawing.Point(6, 6)
        Me.gbConfigFiles.Name = "gbConfigFiles"
        Me.gbConfigFiles.Size = New System.Drawing.Size(536, 233)
        Me.gbConfigFiles.TabIndex = 5
        Me.gbConfigFiles.TabStop = False
        Me.gbConfigFiles.Text = "Config Files"
        '
        'btnCobolEdit
        '
        Me.btnCobolEdit.Location = New System.Drawing.Point(482, 176)
        Me.btnCobolEdit.Name = "btnCobolEdit"
        Me.btnCobolEdit.Size = New System.Drawing.Size(48, 23)
        Me.btnCobolEdit.TabIndex = 29
        Me.btnCobolEdit.Text = "Edit"
        Me.btnCobolEdit.UseVisualStyleBackColor = True
        Me.btnCobolEdit.Visible = False
        '
        'btnCobolBrowse
        '
        Me.btnCobolBrowse.Location = New System.Drawing.Point(452, 176)
        Me.btnCobolBrowse.Name = "btnCobolBrowse"
        Me.btnCobolBrowse.Size = New System.Drawing.Size(24, 23)
        Me.btnCobolBrowse.TabIndex = 28
        Me.btnCobolBrowse.Text = "..."
        Me.btnCobolBrowse.UseVisualStyleBackColor = True
        Me.btnCobolBrowse.Visible = False
        '
        'txtCobol
        '
        Me.txtCobol.Location = New System.Drawing.Point(66, 178)
        Me.txtCobol.Name = "txtCobol"
        Me.txtCobol.Size = New System.Drawing.Size(380, 20)
        Me.txtCobol.TabIndex = 27
        Me.txtCobol.Visible = False
        '
        'lblCobol
        '
        Me.lblCobol.AutoSize = True
        Me.lblCobol.Location = New System.Drawing.Point(9, 184)
        Me.lblCobol.Name = "lblCobol"
        Me.lblCobol.Size = New System.Drawing.Size(46, 13)
        Me.lblCobol.TabIndex = 26
        Me.lblCobol.Text = "COBOL:"
        Me.lblCobol.Visible = False
        '
        'btnPHPEdit
        '
        Me.btnPHPEdit.Location = New System.Drawing.Point(482, 147)
        Me.btnPHPEdit.Name = "btnPHPEdit"
        Me.btnPHPEdit.Size = New System.Drawing.Size(48, 23)
        Me.btnPHPEdit.TabIndex = 25
        Me.btnPHPEdit.Text = "Edit"
        Me.btnPHPEdit.UseVisualStyleBackColor = True
        '
        'btnPHPBrowse
        '
        Me.btnPHPBrowse.Location = New System.Drawing.Point(452, 147)
        Me.btnPHPBrowse.Name = "btnPHPBrowse"
        Me.btnPHPBrowse.Size = New System.Drawing.Size(24, 23)
        Me.btnPHPBrowse.TabIndex = 24
        Me.btnPHPBrowse.Text = "..."
        Me.btnPHPBrowse.UseVisualStyleBackColor = True
        '
        'txtPHP
        '
        Me.txtPHP.Location = New System.Drawing.Point(66, 149)
        Me.txtPHP.Name = "txtPHP"
        Me.txtPHP.Size = New System.Drawing.Size(380, 20)
        Me.txtPHP.TabIndex = 23
        '
        'lblPHP
        '
        Me.lblPHP.AutoSize = True
        Me.lblPHP.Location = New System.Drawing.Point(9, 155)
        Me.lblPHP.Name = "lblPHP"
        Me.lblPHP.Size = New System.Drawing.Size(32, 13)
        Me.lblPHP.TabIndex = 22
        Me.lblPHP.Text = "PHP:"
        '
        'btnVBEdit
        '
        Me.btnVBEdit.Location = New System.Drawing.Point(482, 120)
        Me.btnVBEdit.Name = "btnVBEdit"
        Me.btnVBEdit.Size = New System.Drawing.Size(48, 23)
        Me.btnVBEdit.TabIndex = 21
        Me.btnVBEdit.Text = "Edit"
        Me.btnVBEdit.UseVisualStyleBackColor = True
        '
        'btnVBBrowse
        '
        Me.btnVBBrowse.Location = New System.Drawing.Point(452, 120)
        Me.btnVBBrowse.Name = "btnVBBrowse"
        Me.btnVBBrowse.Size = New System.Drawing.Size(24, 23)
        Me.btnVBBrowse.TabIndex = 20
        Me.btnVBBrowse.Text = "..."
        Me.btnVBBrowse.UseVisualStyleBackColor = True
        '
        'txtVB
        '
        Me.txtVB.Location = New System.Drawing.Point(66, 122)
        Me.txtVB.Name = "txtVB"
        Me.txtVB.Size = New System.Drawing.Size(380, 20)
        Me.txtVB.TabIndex = 19
        '
        'lblVB
        '
        Me.lblVB.AutoSize = True
        Me.lblVB.Location = New System.Drawing.Point(9, 128)
        Me.lblVB.Name = "lblVB"
        Me.lblVB.Size = New System.Drawing.Size(24, 13)
        Me.lblVB.TabIndex = 18
        Me.lblVB.Text = "VB:"
        '
        'btnCSharpEdit
        '
        Me.btnCSharpEdit.Location = New System.Drawing.Point(482, 93)
        Me.btnCSharpEdit.Name = "btnCSharpEdit"
        Me.btnCSharpEdit.Size = New System.Drawing.Size(48, 23)
        Me.btnCSharpEdit.TabIndex = 17
        Me.btnCSharpEdit.Text = "Edit"
        Me.btnCSharpEdit.UseVisualStyleBackColor = True
        '
        'btnCSharpBrowse
        '
        Me.btnCSharpBrowse.Location = New System.Drawing.Point(452, 93)
        Me.btnCSharpBrowse.Name = "btnCSharpBrowse"
        Me.btnCSharpBrowse.Size = New System.Drawing.Size(24, 23)
        Me.btnCSharpBrowse.TabIndex = 16
        Me.btnCSharpBrowse.Text = "..."
        Me.btnCSharpBrowse.UseVisualStyleBackColor = True
        '
        'txtCSharp
        '
        Me.txtCSharp.Location = New System.Drawing.Point(66, 95)
        Me.txtCSharp.Name = "txtCSharp"
        Me.txtCSharp.Size = New System.Drawing.Size(380, 20)
        Me.txtCSharp.TabIndex = 15
        '
        'lblCSharp
        '
        Me.lblCSharp.AutoSize = True
        Me.lblCSharp.Location = New System.Drawing.Point(9, 101)
        Me.lblCSharp.Name = "lblCSharp"
        Me.lblCSharp.Size = New System.Drawing.Size(24, 13)
        Me.lblCSharp.TabIndex = 14
        Me.lblCSharp.Text = "C#:"
        '
        'btnCPPBrowse
        '
        Me.btnCPPBrowse.Location = New System.Drawing.Point(452, 15)
        Me.btnCPPBrowse.Name = "btnCPPBrowse"
        Me.btnCPPBrowse.Size = New System.Drawing.Size(24, 23)
        Me.btnCPPBrowse.TabIndex = 13
        Me.btnCPPBrowse.Text = "..."
        Me.btnCPPBrowse.UseVisualStyleBackColor = True
        '
        'txtCPP
        '
        Me.txtCPP.Location = New System.Drawing.Point(66, 17)
        Me.txtCPP.Name = "txtCPP"
        Me.txtCPP.Size = New System.Drawing.Size(380, 20)
        Me.txtCPP.TabIndex = 12
        '
        'btnSQLEdit
        '
        Me.btnSQLEdit.Location = New System.Drawing.Point(482, 67)
        Me.btnSQLEdit.Name = "btnSQLEdit"
        Me.btnSQLEdit.Size = New System.Drawing.Size(48, 23)
        Me.btnSQLEdit.TabIndex = 11
        Me.btnSQLEdit.Text = "Edit"
        Me.btnSQLEdit.UseVisualStyleBackColor = True
        '
        'btnSQLBrowse
        '
        Me.btnSQLBrowse.Location = New System.Drawing.Point(452, 67)
        Me.btnSQLBrowse.Name = "btnSQLBrowse"
        Me.btnSQLBrowse.Size = New System.Drawing.Size(24, 23)
        Me.btnSQLBrowse.TabIndex = 10
        Me.btnSQLBrowse.Text = "..."
        Me.btnSQLBrowse.UseVisualStyleBackColor = True
        '
        'txtPLSQL
        '
        Me.txtPLSQL.Location = New System.Drawing.Point(66, 69)
        Me.txtPLSQL.Name = "txtPLSQL"
        Me.txtPLSQL.Size = New System.Drawing.Size(380, 20)
        Me.txtPLSQL.TabIndex = 9
        '
        'btnJavaEdit
        '
        Me.btnJavaEdit.Location = New System.Drawing.Point(482, 41)
        Me.btnJavaEdit.Name = "btnJavaEdit"
        Me.btnJavaEdit.Size = New System.Drawing.Size(48, 23)
        Me.btnJavaEdit.TabIndex = 8
        Me.btnJavaEdit.Text = "Edit"
        Me.btnJavaEdit.UseVisualStyleBackColor = True
        '
        'btnJavaBrowse
        '
        Me.btnJavaBrowse.Location = New System.Drawing.Point(452, 41)
        Me.btnJavaBrowse.Name = "btnJavaBrowse"
        Me.btnJavaBrowse.Size = New System.Drawing.Size(24, 23)
        Me.btnJavaBrowse.TabIndex = 7
        Me.btnJavaBrowse.Text = "..."
        Me.btnJavaBrowse.UseVisualStyleBackColor = True
        '
        'txtJava
        '
        Me.txtJava.Location = New System.Drawing.Point(66, 43)
        Me.txtJava.Name = "txtJava"
        Me.txtJava.Size = New System.Drawing.Size(380, 20)
        Me.txtJava.TabIndex = 6
        '
        'btnCPPEdit
        '
        Me.btnCPPEdit.Location = New System.Drawing.Point(482, 15)
        Me.btnCPPEdit.Name = "btnCPPEdit"
        Me.btnCPPEdit.Size = New System.Drawing.Size(48, 23)
        Me.btnCPPEdit.TabIndex = 5
        Me.btnCPPEdit.Text = "Edit"
        Me.btnCPPEdit.UseVisualStyleBackColor = True
        '
        'lblSQL
        '
        Me.lblSQL.AutoSize = True
        Me.lblSQL.Location = New System.Drawing.Point(9, 75)
        Me.lblSQL.Name = "lblSQL"
        Me.lblSQL.Size = New System.Drawing.Size(49, 13)
        Me.lblSQL.TabIndex = 2
        Me.lblSQL.Text = "PL/SQL:"
        '
        'lblJava
        '
        Me.lblJava.AutoSize = True
        Me.lblJava.Location = New System.Drawing.Point(9, 45)
        Me.lblJava.Name = "lblJava"
        Me.lblJava.Size = New System.Drawing.Size(33, 13)
        Me.lblJava.TabIndex = 1
        Me.lblJava.Text = "Java:"
        '
        'lblCPP
        '
        Me.lblCPP.AutoSize = True
        Me.lblCPP.Location = New System.Drawing.Point(9, 20)
        Me.lblCPP.Name = "lblCPP"
        Me.lblCPP.Size = New System.Drawing.Size(41, 13)
        Me.lblCPP.TabIndex = 0
        Me.lblCPP.Text = "C/C++:"
        '
        'tpXMLExport
        '
        Me.tpXMLExport.Controls.Add(Me.btnExport)
        Me.tpXMLExport.Controls.Add(Me.gbFilter)
        Me.tpXMLExport.Controls.Add(Me.gbExportMode)
        Me.tpXMLExport.Location = New System.Drawing.Point(4, 22)
        Me.tpXMLExport.Name = "tpXMLExport"
        Me.tpXMLExport.Padding = New System.Windows.Forms.Padding(3)
        Me.tpXMLExport.Size = New System.Drawing.Size(551, 315)
        Me.tpXMLExport.TabIndex = 2
        Me.tpXMLExport.Text = "Result Filter and XML Export"
        Me.tpXMLExport.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(451, 220)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(92, 23)
        Me.btnExport.TabIndex = 2
        Me.btnExport.Text = "Export Now..."
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'gbFilter
        '
        Me.gbFilter.Controls.Add(Me.cboMaximum)
        Me.gbFilter.Controls.Add(Me.lblTo)
        Me.gbFilter.Controls.Add(Me.rbRange)
        Me.gbFilter.Controls.Add(Me.cboMinimum)
        Me.gbFilter.Controls.Add(Me.rbBelow)
        Me.gbFilter.Controls.Add(Me.cboBelow)
        Me.gbFilter.Controls.Add(Me.rbAbove)
        Me.gbFilter.Controls.Add(Me.cboAbove)
        Me.gbFilter.Location = New System.Drawing.Point(7, 106)
        Me.gbFilter.Name = "gbFilter"
        Me.gbFilter.Size = New System.Drawing.Size(536, 108)
        Me.gbFilter.TabIndex = 1
        Me.gbFilter.TabStop = False
        Me.gbFilter.Text = "Result Filter Options"
        '
        'cboMaximum
        '
        Me.cboMaximum.Enabled = False
        Me.cboMaximum.FormattingEnabled = True
        Me.cboMaximum.Items.AddRange(New Object() {"Potentially Unsafe", "Suspicious Comment", "Low", "Standard", "Medium", "High", "Critical"})
        Me.cboMaximum.Location = New System.Drawing.Point(372, 73)
        Me.cboMaximum.Name = "cboMaximum"
        Me.cboMaximum.Size = New System.Drawing.Size(158, 21)
        Me.cboMaximum.TabIndex = 10
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(351, 76)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(19, 13)
        Me.lblTo.TabIndex = 9
        Me.lblTo.Text = "to:"
        '
        'rbRange
        '
        Me.rbRange.AutoSize = True
        Me.rbRange.Location = New System.Drawing.Point(6, 73)
        Me.rbRange.Name = "rbRange"
        Me.rbRange.Size = New System.Drawing.Size(164, 17)
        Me.rbRange.TabIndex = 8
        Me.rbRange.TabStop = True
        Me.rbRange.Text = "Display Results in the Range:"
        Me.rbRange.UseVisualStyleBackColor = True
        '
        'cboMinimum
        '
        Me.cboMinimum.Enabled = False
        Me.cboMinimum.FormattingEnabled = True
        Me.cboMinimum.Items.AddRange(New Object() {"Potentially Unsafe", "Suspicious Comment", "Low", "Standard", "Medium", "High", "Critical"})
        Me.cboMinimum.Location = New System.Drawing.Point(194, 73)
        Me.cboMinimum.Name = "cboMinimum"
        Me.cboMinimum.Size = New System.Drawing.Size(154, 21)
        Me.cboMinimum.TabIndex = 7
        '
        'rbBelow
        '
        Me.rbBelow.AutoSize = True
        Me.rbBelow.Location = New System.Drawing.Point(6, 46)
        Me.rbBelow.Name = "rbBelow"
        Me.rbBelow.Size = New System.Drawing.Size(186, 17)
        Me.rbBelow.TabIndex = 6
        Me.rbBelow.TabStop = True
        Me.rbBelow.Text = "Display Results Equal to or Below:"
        Me.rbBelow.UseVisualStyleBackColor = True
        '
        'cboBelow
        '
        Me.cboBelow.Enabled = False
        Me.cboBelow.FormattingEnabled = True
        Me.cboBelow.Items.AddRange(New Object() {"Potentially Unsafe", "Suspicious Comment", "Low", "Standard", "Medium", "High", "Critical"})
        Me.cboBelow.Location = New System.Drawing.Point(194, 46)
        Me.cboBelow.Name = "cboBelow"
        Me.cboBelow.Size = New System.Drawing.Size(154, 21)
        Me.cboBelow.TabIndex = 5
        '
        'rbAbove
        '
        Me.rbAbove.AutoSize = True
        Me.rbAbove.Checked = True
        Me.rbAbove.Location = New System.Drawing.Point(6, 19)
        Me.rbAbove.Name = "rbAbove"
        Me.rbAbove.Size = New System.Drawing.Size(188, 17)
        Me.rbAbove.TabIndex = 4
        Me.rbAbove.TabStop = True
        Me.rbAbove.Text = "Display Results Equal to or Above:"
        Me.rbAbove.UseVisualStyleBackColor = True
        '
        'cboAbove
        '
        Me.cboAbove.FormattingEnabled = True
        Me.cboAbove.Items.AddRange(New Object() {"Potentially Unsafe", "Suspicious Comment", "Low", "Standard", "Medium", "High", "Critical"})
        Me.cboAbove.Location = New System.Drawing.Point(194, 19)
        Me.cboAbove.Name = "cboAbove"
        Me.cboAbove.Size = New System.Drawing.Size(154, 21)
        Me.cboAbove.TabIndex = 3
        '
        'gbExportMode
        '
        Me.gbExportMode.Controls.Add(Me.cbSaveState)
        Me.gbExportMode.Controls.Add(Me.rbFiltered)
        Me.gbExportMode.Controls.Add(Me.rbAll)
        Me.gbExportMode.Location = New System.Drawing.Point(7, 7)
        Me.gbExportMode.Name = "gbExportMode"
        Me.gbExportMode.Size = New System.Drawing.Size(536, 92)
        Me.gbExportMode.TabIndex = 0
        Me.gbExportMode.TabStop = False
        Me.gbExportMode.Text = "Export Mode"
        '
        'cbSaveState
        '
        Me.cbSaveState.AutoSize = True
        Me.cbSaveState.Checked = True
        Me.cbSaveState.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbSaveState.Location = New System.Drawing.Point(7, 68)
        Me.cbSaveState.Name = "cbSaveState"
        Me.cbSaveState.Size = New System.Drawing.Size(439, 17)
        Me.cbSaveState.TabIndex = 2
        Me.cbSaveState.Text = "Save CheckBox State of Marked Items (Summary Table State will be Preserved in XML" & _
    ")"
        Me.cbSaveState.UseVisualStyleBackColor = True
        '
        'rbFiltered
        '
        Me.rbFiltered.AutoSize = True
        Me.rbFiltered.Location = New System.Drawing.Point(7, 44)
        Me.rbFiltered.Name = "rbFiltered"
        Me.rbFiltered.Size = New System.Drawing.Size(130, 17)
        Me.rbFiltered.TabIndex = 1
        Me.rbFiltered.Text = "Export Filtered Results"
        Me.rbFiltered.UseVisualStyleBackColor = True
        '
        'rbAll
        '
        Me.rbAll.AutoSize = True
        Me.rbAll.Checked = True
        Me.rbAll.Location = New System.Drawing.Point(7, 20)
        Me.rbAll.Name = "rbAll"
        Me.rbAll.Size = New System.Drawing.Size(107, 17)
        Me.rbAll.TabIndex = 0
        Me.rbAll.TabStop = True
        Me.rbAll.Text = "Export All Results"
        Me.rbAll.UseVisualStyleBackColor = True
        '
        'tpDisplay
        '
        Me.tpDisplay.Controls.Add(Me.gbDisplay)
        Me.tpDisplay.Location = New System.Drawing.Point(4, 22)
        Me.tpDisplay.Name = "tpDisplay"
        Me.tpDisplay.Padding = New System.Windows.Forms.Padding(3)
        Me.tpDisplay.Size = New System.Drawing.Size(551, 315)
        Me.tpDisplay.TabIndex = 3
        Me.tpDisplay.Text = "Display"
        Me.tpDisplay.UseVisualStyleBackColor = True
        '
        'gbDisplay
        '
        Me.gbDisplay.Controls.Add(Me.cbShowStatusBar)
        Me.gbDisplay.Controls.Add(Me.lblColour)
        Me.gbDisplay.Controls.Add(Me.btnColour)
        Me.gbDisplay.Controls.Add(Me.cbReminder)
        Me.gbDisplay.Controls.Add(Me.cbShowChart)
        Me.gbDisplay.Location = New System.Drawing.Point(6, 6)
        Me.gbDisplay.Name = "gbDisplay"
        Me.gbDisplay.Size = New System.Drawing.Size(540, 303)
        Me.gbDisplay.TabIndex = 0
        Me.gbDisplay.TabStop = False
        Me.gbDisplay.Text = "Display Options"
        '
        'lblColour
        '
        Me.lblColour.AutoSize = True
        Me.lblColour.Location = New System.Drawing.Point(4, 116)
        Me.lblColour.Name = "lblColour"
        Me.lblColour.Size = New System.Drawing.Size(197, 13)
        Me.lblColour.TabIndex = 3
        Me.lblColour.Text = "Set Colour of Selected Items in ListView:"
        '
        'btnColour
        '
        Me.btnColour.Location = New System.Drawing.Point(201, 111)
        Me.btnColour.Name = "btnColour"
        Me.btnColour.Size = New System.Drawing.Size(30, 22)
        Me.btnColour.TabIndex = 2
        Me.btnColour.Text = "..."
        Me.btnColour.UseVisualStyleBackColor = True
        '
        'cbReminder
        '
        Me.cbReminder.AutoSize = True
        Me.cbReminder.Location = New System.Drawing.Point(8, 29)
        Me.cbReminder.Name = "cbReminder"
        Me.cbReminder.Size = New System.Drawing.Size(281, 17)
        Me.cbReminder.TabIndex = 1
        Me.cbReminder.Text = "Show me a reminder to choose a language on start-up"
        Me.cbReminder.UseVisualStyleBackColor = True
        '
        'cbShowChart
        '
        Me.cbShowChart.AutoSize = True
        Me.cbShowChart.Location = New System.Drawing.Point(8, 59)
        Me.cbShowChart.Name = "cbShowChart"
        Me.cbShowChart.Size = New System.Drawing.Size(282, 17)
        Me.cbShowChart.TabIndex = 0
        Me.cbShowChart.Text = "Show charts and code breakdown when scan finishes"
        Me.cbShowChart.UseVisualStyleBackColor = True
        '
        'cbShowStatusBar
        '
        Me.cbShowStatusBar.AutoSize = True
        Me.cbShowStatusBar.Location = New System.Drawing.Point(8, 86)
        Me.cbShowStatusBar.Name = "cbShowStatusBar"
        Me.cbShowStatusBar.Size = New System.Drawing.Size(105, 17)
        Me.cbShowStatusBar.TabIndex = 4
        Me.cbShowStatusBar.Text = "Show Status Bar"
        Me.cbShowStatusBar.UseVisualStyleBackColor = True
        '
        'frmOptions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(559, 376)
        Me.Controls.Add(Me.tabOptions)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOptions"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Options"
        Me.tabOptions.ResumeLayout(False)
        Me.tpGeneral.ResumeLayout(False)
        Me.gbReporting.ResumeLayout(False)
        Me.gbReporting.PerformLayout()
        Me.gbOWASP.ResumeLayout(False)
        Me.gbOWASP.PerformLayout()
        Me.gbOutput.ResumeLayout(False)
        Me.gbOutput.PerformLayout()
        Me.gbFileTypes.ResumeLayout(False)
        Me.gbFileTypes.PerformLayout()
        Me.gbLanguage.ResumeLayout(False)
        Me.gbLanguage.PerformLayout()
        Me.tpConfigFiles.ResumeLayout(False)
        Me.gbConfigFiles.ResumeLayout(False)
        Me.gbConfigFiles.PerformLayout()
        Me.tpXMLExport.ResumeLayout(False)
        Me.gbFilter.ResumeLayout(False)
        Me.gbFilter.PerformLayout()
        Me.gbExportMode.ResumeLayout(False)
        Me.gbExportMode.PerformLayout()
        Me.tpDisplay.ResumeLayout(False)
        Me.gbDisplay.ResumeLayout(False)
        Me.gbDisplay.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents ofdOpenFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents sfdSaveFileDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents tabOptions As System.Windows.Forms.TabControl
    Friend WithEvents tpGeneral As System.Windows.Forms.TabPage
    Friend WithEvents gbReporting As System.Windows.Forms.GroupBox
    Friend WithEvents cboReporting As System.Windows.Forms.ComboBox
    Friend WithEvents lblReporting As System.Windows.Forms.Label
    Friend WithEvents gbOWASP As System.Windows.Forms.GroupBox
    Friend WithEvents cbInnerClass As System.Windows.Forms.CheckBox
    Friend WithEvents cbFinalize As System.Windows.Forms.CheckBox
    Friend WithEvents gbOutput As System.Windows.Forms.GroupBox
    Friend WithEvents cbOutput As System.Windows.Forms.CheckBox
    Friend WithEvents txtOutput As System.Windows.Forms.TextBox
    Friend WithEvents btnOutputBrowse As System.Windows.Forms.Button
    Friend WithEvents gbFileTypes As System.Windows.Forms.GroupBox
    Friend WithEvents lblExplain As System.Windows.Forms.Label
    Friend WithEvents txtFileTypes As System.Windows.Forms.TextBox
    Friend WithEvents cboFileTypes As System.Windows.Forms.ComboBox
    Friend WithEvents gbLanguage As System.Windows.Forms.GroupBox
    Friend WithEvents cboStartUpLanguage As System.Windows.Forms.ComboBox
    Friend WithEvents cboCurrentLanguage As System.Windows.Forms.ComboBox
    Friend WithEvents lblStartUpLanguage As System.Windows.Forms.Label
    Friend WithEvents lblCurrentLanguage As System.Windows.Forms.Label
    Friend WithEvents tpConfigFiles As System.Windows.Forms.TabPage
    Friend WithEvents gbConfigFiles As System.Windows.Forms.GroupBox
    Friend WithEvents btnCSharpEdit As System.Windows.Forms.Button
    Friend WithEvents btnCSharpBrowse As System.Windows.Forms.Button
    Friend WithEvents txtCSharp As System.Windows.Forms.TextBox
    Friend WithEvents lblCSharp As System.Windows.Forms.Label
    Friend WithEvents btnCPPBrowse As System.Windows.Forms.Button
    Friend WithEvents txtCPP As System.Windows.Forms.TextBox
    Friend WithEvents btnSQLEdit As System.Windows.Forms.Button
    Friend WithEvents btnSQLBrowse As System.Windows.Forms.Button
    Friend WithEvents txtPLSQL As System.Windows.Forms.TextBox
    Friend WithEvents btnJavaEdit As System.Windows.Forms.Button
    Friend WithEvents btnJavaBrowse As System.Windows.Forms.Button
    Friend WithEvents txtJava As System.Windows.Forms.TextBox
    Friend WithEvents btnCPPEdit As System.Windows.Forms.Button
    Friend WithEvents lblSQL As System.Windows.Forms.Label
    Friend WithEvents lblJava As System.Windows.Forms.Label
    Friend WithEvents lblCPP As System.Windows.Forms.Label
    Friend WithEvents tpXMLExport As System.Windows.Forms.TabPage
    Friend WithEvents gbExportMode As System.Windows.Forms.GroupBox
    Friend WithEvents cbSaveState As System.Windows.Forms.CheckBox
    Friend WithEvents rbFiltered As System.Windows.Forms.RadioButton
    Friend WithEvents rbAll As System.Windows.Forms.RadioButton
    Friend WithEvents gbFilter As System.Windows.Forms.GroupBox
    Friend WithEvents cboMaximum As System.Windows.Forms.ComboBox
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents rbRange As System.Windows.Forms.RadioButton
    Friend WithEvents cboMinimum As System.Windows.Forms.ComboBox
    Friend WithEvents rbBelow As System.Windows.Forms.RadioButton
    Friend WithEvents cboBelow As System.Windows.Forms.ComboBox
    Friend WithEvents rbAbove As System.Windows.Forms.RadioButton
    Friend WithEvents cboAbove As System.Windows.Forms.ComboBox
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnVBEdit As System.Windows.Forms.Button
    Friend WithEvents btnVBBrowse As System.Windows.Forms.Button
    Friend WithEvents txtVB As System.Windows.Forms.TextBox
    Friend WithEvents lblVB As System.Windows.Forms.Label
    Friend WithEvents btnPHPEdit As System.Windows.Forms.Button
    Friend WithEvents btnPHPBrowse As System.Windows.Forms.Button
    Friend WithEvents txtPHP As System.Windows.Forms.TextBox
    Friend WithEvents lblPHP As System.Windows.Forms.Label
    Friend WithEvents btnCobolEdit As System.Windows.Forms.Button
    Friend WithEvents btnCobolBrowse As System.Windows.Forms.Button
    Friend WithEvents txtCobol As System.Windows.Forms.TextBox
    Friend WithEvents lblCobol As System.Windows.Forms.Label
    Friend WithEvents tpDisplay As System.Windows.Forms.TabPage
    Friend WithEvents gbDisplay As System.Windows.Forms.GroupBox
    Friend WithEvents cbReminder As System.Windows.Forms.CheckBox
    Friend WithEvents cbShowChart As System.Windows.Forms.CheckBox
    Friend WithEvents lblColour As System.Windows.Forms.Label
    Friend WithEvents btnColour As System.Windows.Forms.Button
    Friend WithEvents cdColorDialog As System.Windows.Forms.ColorDialog
    Friend WithEvents cbShowStatusBar As System.Windows.Forms.CheckBox
End Class
