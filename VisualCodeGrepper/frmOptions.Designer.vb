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
        Me.tpJava = New System.Windows.Forms.TabPage()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.udCOBOLStart = New System.Windows.Forms.NumericUpDown()
        Me.lblCOBOLStart = New System.Windows.Forms.Label()
        Me.gbAndroid = New System.Windows.Forms.GroupBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.cbAndroid = New System.Windows.Forms.CheckBox()
        Me.gbOWASP = New System.Windows.Forms.GroupBox()
        Me.cbInnerClass = New System.Windows.Forms.CheckBox()
        Me.cbFinalize = New System.Windows.Forms.CheckBox()
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
        Me.cbShowStatusBar = New System.Windows.Forms.CheckBox()
        Me.lblColour = New System.Windows.Forms.Label()
        Me.btnColour = New System.Windows.Forms.Button()
        Me.cbReminder = New System.Windows.Forms.CheckBox()
        Me.cbShowChart = New System.Windows.Forms.CheckBox()
        Me.tpBeta = New System.Windows.Forms.TabPage()
        Me.gbBeta = New System.Windows.Forms.GroupBox()
        Me.cbSigned = New System.Windows.Forms.CheckBox()
        Me.cbCobol = New System.Windows.Forms.CheckBox()
        Me.tpGrep = New System.Windows.Forms.TabPage()
        Me.txtTempGrepTitle = New System.Windows.Forms.TextBox()
        Me.txtTempGrep = New System.Windows.Forms.TextBox()
        Me.cdColorDialog = New System.Windows.Forms.ColorDialog()
        Me.cbZOS = New System.Windows.Forms.CheckBox()
        Me.tabOptions.SuspendLayout()
        Me.tpGeneral.SuspendLayout()
        Me.gbReporting.SuspendLayout()
        Me.gbOutput.SuspendLayout()
        Me.gbFileTypes.SuspendLayout()
        Me.gbLanguage.SuspendLayout()
        Me.tpConfigFiles.SuspendLayout()
        Me.gbConfigFiles.SuspendLayout()
        Me.tpJava.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.udCOBOLStart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbAndroid.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.gbOWASP.SuspendLayout()
        Me.tpXMLExport.SuspendLayout()
        Me.gbFilter.SuspendLayout()
        Me.gbExportMode.SuspendLayout()
        Me.tpDisplay.SuspendLayout()
        Me.gbDisplay.SuspendLayout()
        Me.tpBeta.SuspendLayout()
        Me.gbBeta.SuspendLayout()
        Me.tpGrep.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(804, 596)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(6)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(150, 44)
        Me.btnCancel.TabIndex = 0
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(966, 596)
        Me.btnOK.Margin = New System.Windows.Forms.Padding(6)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(150, 44)
        Me.btnOK.TabIndex = 1
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'tabOptions
        '
        Me.tabOptions.Controls.Add(Me.tpGeneral)
        Me.tabOptions.Controls.Add(Me.tpConfigFiles)
        Me.tabOptions.Controls.Add(Me.tpJava)
        Me.tabOptions.Controls.Add(Me.tpXMLExport)
        Me.tabOptions.Controls.Add(Me.tpDisplay)
        Me.tabOptions.Controls.Add(Me.tpBeta)
        Me.tabOptions.Controls.Add(Me.tpGrep)
        Me.tabOptions.Location = New System.Drawing.Point(2, 4)
        Me.tabOptions.Margin = New System.Windows.Forms.Padding(6)
        Me.tabOptions.Name = "tabOptions"
        Me.tabOptions.SelectedIndex = 0
        Me.tabOptions.Size = New System.Drawing.Size(1120, 581)
        Me.tabOptions.TabIndex = 8
        '
        'tpGeneral
        '
        Me.tpGeneral.Controls.Add(Me.gbReporting)
        Me.tpGeneral.Controls.Add(Me.gbOutput)
        Me.tpGeneral.Controls.Add(Me.gbFileTypes)
        Me.tpGeneral.Controls.Add(Me.gbLanguage)
        Me.tpGeneral.Location = New System.Drawing.Point(8, 39)
        Me.tpGeneral.Margin = New System.Windows.Forms.Padding(6)
        Me.tpGeneral.Name = "tpGeneral"
        Me.tpGeneral.Padding = New System.Windows.Forms.Padding(6)
        Me.tpGeneral.Size = New System.Drawing.Size(1104, 534)
        Me.tpGeneral.TabIndex = 0
        Me.tpGeneral.Text = "General"
        Me.tpGeneral.UseVisualStyleBackColor = True
        '
        'gbReporting
        '
        Me.gbReporting.Controls.Add(Me.cboReporting)
        Me.gbReporting.Controls.Add(Me.lblReporting)
        Me.gbReporting.Location = New System.Drawing.Point(16, 271)
        Me.gbReporting.Margin = New System.Windows.Forms.Padding(6)
        Me.gbReporting.Name = "gbReporting"
        Me.gbReporting.Padding = New System.Windows.Forms.Padding(6)
        Me.gbReporting.Size = New System.Drawing.Size(1072, 88)
        Me.gbReporting.TabIndex = 10
        Me.gbReporting.TabStop = False
        Me.gbReporting.Text = "Results/Reporting"
        '
        'cboReporting
        '
        Me.cboReporting.FormattingEnabled = True
        Me.cboReporting.Items.AddRange(New Object() {"Potentially Unsafe", "Suspicious Comment", "Low", "Standard", "Medium", "High", "Critical"})
        Me.cboReporting.Location = New System.Drawing.Point(490, 33)
        Me.cboReporting.Margin = New System.Windows.Forms.Padding(6)
        Me.cboReporting.Name = "cboReporting"
        Me.cboReporting.Size = New System.Drawing.Size(394, 33)
        Me.cboReporting.TabIndex = 1
        '
        'lblReporting
        '
        Me.lblReporting.AutoSize = True
        Me.lblReporting.Location = New System.Drawing.Point(24, 38)
        Me.lblReporting.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblReporting.Name = "lblReporting"
        Me.lblReporting.Size = New System.Drawing.Size(430, 25)
        Me.lblReporting.TabIndex = 0
        Me.lblReporting.Text = "Display/Scan for Results Equal to or Above:"
        '
        'gbOutput
        '
        Me.gbOutput.Controls.Add(Me.cbOutput)
        Me.gbOutput.Controls.Add(Me.txtOutput)
        Me.gbOutput.Controls.Add(Me.btnOutputBrowse)
        Me.gbOutput.Location = New System.Drawing.Point(16, 365)
        Me.gbOutput.Margin = New System.Windows.Forms.Padding(6)
        Me.gbOutput.Name = "gbOutput"
        Me.gbOutput.Padding = New System.Windows.Forms.Padding(6)
        Me.gbOutput.Size = New System.Drawing.Size(1072, 138)
        Me.gbOutput.TabIndex = 8
        Me.gbOutput.TabStop = False
        Me.gbOutput.Text = "Output"
        '
        'cbOutput
        '
        Me.cbOutput.AutoSize = True
        Me.cbOutput.Location = New System.Drawing.Point(24, 35)
        Me.cbOutput.Margin = New System.Windows.Forms.Padding(6)
        Me.cbOutput.Name = "cbOutput"
        Me.cbOutput.Size = New System.Drawing.Size(229, 29)
        Me.cbOutput.TabIndex = 0
        Me.cbOutput.Text = "Write to Output File"
        Me.cbOutput.UseVisualStyleBackColor = True
        '
        'txtOutput
        '
        Me.txtOutput.Location = New System.Drawing.Point(18, 79)
        Me.txtOutput.Margin = New System.Windows.Forms.Padding(6)
        Me.txtOutput.Name = "txtOutput"
        Me.txtOutput.Size = New System.Drawing.Size(978, 31)
        Me.txtOutput.TabIndex = 3
        '
        'btnOutputBrowse
        '
        Me.btnOutputBrowse.Location = New System.Drawing.Point(1012, 77)
        Me.btnOutputBrowse.Margin = New System.Windows.Forms.Padding(6)
        Me.btnOutputBrowse.Name = "btnOutputBrowse"
        Me.btnOutputBrowse.Size = New System.Drawing.Size(48, 44)
        Me.btnOutputBrowse.TabIndex = 4
        Me.btnOutputBrowse.Text = "..."
        Me.btnOutputBrowse.UseVisualStyleBackColor = True
        '
        'gbFileTypes
        '
        Me.gbFileTypes.Controls.Add(Me.lblExplain)
        Me.gbFileTypes.Controls.Add(Me.txtFileTypes)
        Me.gbFileTypes.Controls.Add(Me.cboFileTypes)
        Me.gbFileTypes.Location = New System.Drawing.Point(16, 131)
        Me.gbFileTypes.Margin = New System.Windows.Forms.Padding(6)
        Me.gbFileTypes.Name = "gbFileTypes"
        Me.gbFileTypes.Padding = New System.Windows.Forms.Padding(6)
        Me.gbFileTypes.Size = New System.Drawing.Size(1072, 129)
        Me.gbFileTypes.TabIndex = 5
        Me.gbFileTypes.TabStop = False
        Me.gbFileTypes.Text = "File Types"
        '
        'lblExplain
        '
        Me.lblExplain.AutoSize = True
        Me.lblExplain.Location = New System.Drawing.Point(12, 85)
        Me.lblExplain.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblExplain.Name = "lblExplain"
        Me.lblExplain.Size = New System.Drawing.Size(981, 25)
        Me.lblExplain.TabIndex = 2
        Me.lblExplain.Text = "(Use .* to scan all file types but this will impact speed and potentially include" &
    " non-code files in results)"
        '
        'txtFileTypes
        '
        Me.txtFileTypes.Location = New System.Drawing.Point(242, 38)
        Me.txtFileTypes.Margin = New System.Windows.Forms.Padding(6)
        Me.txtFileTypes.Name = "txtFileTypes"
        Me.txtFileTypes.Size = New System.Drawing.Size(814, 31)
        Me.txtFileTypes.TabIndex = 1
        '
        'cboFileTypes
        '
        Me.cboFileTypes.FormattingEnabled = True
        Me.cboFileTypes.Items.AddRange(New Object() {"C/C++", "Java", "PL/SQL", "C#", "VB", "PHP", "COBOL"})
        Me.cboFileTypes.Location = New System.Drawing.Point(18, 38)
        Me.cboFileTypes.Margin = New System.Windows.Forms.Padding(6)
        Me.cboFileTypes.Name = "cboFileTypes"
        Me.cboFileTypes.Size = New System.Drawing.Size(206, 33)
        Me.cboFileTypes.TabIndex = 0
        '
        'gbLanguage
        '
        Me.gbLanguage.Controls.Add(Me.cboStartUpLanguage)
        Me.gbLanguage.Controls.Add(Me.cboCurrentLanguage)
        Me.gbLanguage.Controls.Add(Me.lblStartUpLanguage)
        Me.gbLanguage.Controls.Add(Me.lblCurrentLanguage)
        Me.gbLanguage.Location = New System.Drawing.Point(16, 19)
        Me.gbLanguage.Margin = New System.Windows.Forms.Padding(6)
        Me.gbLanguage.Name = "gbLanguage"
        Me.gbLanguage.Padding = New System.Windows.Forms.Padding(6)
        Me.gbLanguage.Size = New System.Drawing.Size(1072, 100)
        Me.gbLanguage.TabIndex = 4
        Me.gbLanguage.TabStop = False
        Me.gbLanguage.Text = "Language"
        '
        'cboStartUpLanguage
        '
        Me.cboStartUpLanguage.FormattingEnabled = True
        Me.cboStartUpLanguage.Items.AddRange(New Object() {"C/C++", "Java", "PL/SQL", "C#", "VB", "PHP", "COBOL"})
        Me.cboStartUpLanguage.Location = New System.Drawing.Point(728, 38)
        Me.cboStartUpLanguage.Margin = New System.Windows.Forms.Padding(6)
        Me.cboStartUpLanguage.Name = "cboStartUpLanguage"
        Me.cboStartUpLanguage.Size = New System.Drawing.Size(196, 33)
        Me.cboStartUpLanguage.TabIndex = 3
        '
        'cboCurrentLanguage
        '
        Me.cboCurrentLanguage.FormattingEnabled = True
        Me.cboCurrentLanguage.Items.AddRange(New Object() {"C/C++", "Java", "PL/SQL", "C#", "VB", "PHP", "COBOL"})
        Me.cboCurrentLanguage.Location = New System.Drawing.Point(236, 38)
        Me.cboCurrentLanguage.Margin = New System.Windows.Forms.Padding(6)
        Me.cboCurrentLanguage.Name = "cboCurrentLanguage"
        Me.cboCurrentLanguage.Size = New System.Drawing.Size(196, 33)
        Me.cboCurrentLanguage.TabIndex = 2
        '
        'lblStartUpLanguage
        '
        Me.lblStartUpLanguage.AutoSize = True
        Me.lblStartUpLanguage.Location = New System.Drawing.Point(516, 44)
        Me.lblStartUpLanguage.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblStartUpLanguage.Name = "lblStartUpLanguage"
        Me.lblStartUpLanguage.Size = New System.Drawing.Size(198, 25)
        Me.lblStartUpLanguage.TabIndex = 1
        Me.lblStartUpLanguage.Text = "Start Up Language:"
        '
        'lblCurrentLanguage
        '
        Me.lblCurrentLanguage.AutoSize = True
        Me.lblCurrentLanguage.Location = New System.Drawing.Point(18, 44)
        Me.lblCurrentLanguage.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblCurrentLanguage.Name = "lblCurrentLanguage"
        Me.lblCurrentLanguage.Size = New System.Drawing.Size(191, 25)
        Me.lblCurrentLanguage.TabIndex = 0
        Me.lblCurrentLanguage.Text = "Current Language:"
        '
        'tpConfigFiles
        '
        Me.tpConfigFiles.Controls.Add(Me.gbConfigFiles)
        Me.tpConfigFiles.Location = New System.Drawing.Point(8, 39)
        Me.tpConfigFiles.Margin = New System.Windows.Forms.Padding(6)
        Me.tpConfigFiles.Name = "tpConfigFiles"
        Me.tpConfigFiles.Padding = New System.Windows.Forms.Padding(6)
        Me.tpConfigFiles.Size = New System.Drawing.Size(1104, 534)
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
        Me.gbConfigFiles.Location = New System.Drawing.Point(12, 12)
        Me.gbConfigFiles.Margin = New System.Windows.Forms.Padding(6)
        Me.gbConfigFiles.Name = "gbConfigFiles"
        Me.gbConfigFiles.Padding = New System.Windows.Forms.Padding(6)
        Me.gbConfigFiles.Size = New System.Drawing.Size(1072, 415)
        Me.gbConfigFiles.TabIndex = 5
        Me.gbConfigFiles.TabStop = False
        Me.gbConfigFiles.Text = "Config Files"
        '
        'btnCobolEdit
        '
        Me.btnCobolEdit.Location = New System.Drawing.Point(964, 338)
        Me.btnCobolEdit.Margin = New System.Windows.Forms.Padding(6)
        Me.btnCobolEdit.Name = "btnCobolEdit"
        Me.btnCobolEdit.Size = New System.Drawing.Size(96, 44)
        Me.btnCobolEdit.TabIndex = 29
        Me.btnCobolEdit.Text = "Edit"
        Me.btnCobolEdit.UseVisualStyleBackColor = True
        '
        'btnCobolBrowse
        '
        Me.btnCobolBrowse.Location = New System.Drawing.Point(904, 338)
        Me.btnCobolBrowse.Margin = New System.Windows.Forms.Padding(6)
        Me.btnCobolBrowse.Name = "btnCobolBrowse"
        Me.btnCobolBrowse.Size = New System.Drawing.Size(48, 44)
        Me.btnCobolBrowse.TabIndex = 28
        Me.btnCobolBrowse.Text = "..."
        Me.btnCobolBrowse.UseVisualStyleBackColor = True
        '
        'txtCobol
        '
        Me.txtCobol.Location = New System.Drawing.Point(132, 342)
        Me.txtCobol.Margin = New System.Windows.Forms.Padding(6)
        Me.txtCobol.Name = "txtCobol"
        Me.txtCobol.Size = New System.Drawing.Size(756, 31)
        Me.txtCobol.TabIndex = 27
        '
        'lblCobol
        '
        Me.lblCobol.AutoSize = True
        Me.lblCobol.Location = New System.Drawing.Point(18, 354)
        Me.lblCobol.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblCobol.Name = "lblCobol"
        Me.lblCobol.Size = New System.Drawing.Size(91, 25)
        Me.lblCobol.TabIndex = 26
        Me.lblCobol.Text = "COBOL:"
        '
        'btnPHPEdit
        '
        Me.btnPHPEdit.Location = New System.Drawing.Point(964, 283)
        Me.btnPHPEdit.Margin = New System.Windows.Forms.Padding(6)
        Me.btnPHPEdit.Name = "btnPHPEdit"
        Me.btnPHPEdit.Size = New System.Drawing.Size(96, 44)
        Me.btnPHPEdit.TabIndex = 25
        Me.btnPHPEdit.Text = "Edit"
        Me.btnPHPEdit.UseVisualStyleBackColor = True
        '
        'btnPHPBrowse
        '
        Me.btnPHPBrowse.Location = New System.Drawing.Point(904, 283)
        Me.btnPHPBrowse.Margin = New System.Windows.Forms.Padding(6)
        Me.btnPHPBrowse.Name = "btnPHPBrowse"
        Me.btnPHPBrowse.Size = New System.Drawing.Size(48, 44)
        Me.btnPHPBrowse.TabIndex = 24
        Me.btnPHPBrowse.Text = "..."
        Me.btnPHPBrowse.UseVisualStyleBackColor = True
        '
        'txtPHP
        '
        Me.txtPHP.Location = New System.Drawing.Point(132, 287)
        Me.txtPHP.Margin = New System.Windows.Forms.Padding(6)
        Me.txtPHP.Name = "txtPHP"
        Me.txtPHP.Size = New System.Drawing.Size(756, 31)
        Me.txtPHP.TabIndex = 23
        '
        'lblPHP
        '
        Me.lblPHP.AutoSize = True
        Me.lblPHP.Location = New System.Drawing.Point(18, 298)
        Me.lblPHP.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblPHP.Name = "lblPHP"
        Me.lblPHP.Size = New System.Drawing.Size(61, 25)
        Me.lblPHP.TabIndex = 22
        Me.lblPHP.Text = "PHP:"
        '
        'btnVBEdit
        '
        Me.btnVBEdit.Location = New System.Drawing.Point(964, 231)
        Me.btnVBEdit.Margin = New System.Windows.Forms.Padding(6)
        Me.btnVBEdit.Name = "btnVBEdit"
        Me.btnVBEdit.Size = New System.Drawing.Size(96, 44)
        Me.btnVBEdit.TabIndex = 21
        Me.btnVBEdit.Text = "Edit"
        Me.btnVBEdit.UseVisualStyleBackColor = True
        '
        'btnVBBrowse
        '
        Me.btnVBBrowse.Location = New System.Drawing.Point(904, 231)
        Me.btnVBBrowse.Margin = New System.Windows.Forms.Padding(6)
        Me.btnVBBrowse.Name = "btnVBBrowse"
        Me.btnVBBrowse.Size = New System.Drawing.Size(48, 44)
        Me.btnVBBrowse.TabIndex = 20
        Me.btnVBBrowse.Text = "..."
        Me.btnVBBrowse.UseVisualStyleBackColor = True
        '
        'txtVB
        '
        Me.txtVB.Location = New System.Drawing.Point(132, 235)
        Me.txtVB.Margin = New System.Windows.Forms.Padding(6)
        Me.txtVB.Name = "txtVB"
        Me.txtVB.Size = New System.Drawing.Size(756, 31)
        Me.txtVB.TabIndex = 19
        '
        'lblVB
        '
        Me.lblVB.AutoSize = True
        Me.lblVB.Location = New System.Drawing.Point(18, 246)
        Me.lblVB.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblVB.Name = "lblVB"
        Me.lblVB.Size = New System.Drawing.Size(46, 25)
        Me.lblVB.TabIndex = 18
        Me.lblVB.Text = "VB:"
        '
        'btnCSharpEdit
        '
        Me.btnCSharpEdit.Location = New System.Drawing.Point(964, 179)
        Me.btnCSharpEdit.Margin = New System.Windows.Forms.Padding(6)
        Me.btnCSharpEdit.Name = "btnCSharpEdit"
        Me.btnCSharpEdit.Size = New System.Drawing.Size(96, 44)
        Me.btnCSharpEdit.TabIndex = 17
        Me.btnCSharpEdit.Text = "Edit"
        Me.btnCSharpEdit.UseVisualStyleBackColor = True
        '
        'btnCSharpBrowse
        '
        Me.btnCSharpBrowse.Location = New System.Drawing.Point(904, 179)
        Me.btnCSharpBrowse.Margin = New System.Windows.Forms.Padding(6)
        Me.btnCSharpBrowse.Name = "btnCSharpBrowse"
        Me.btnCSharpBrowse.Size = New System.Drawing.Size(48, 44)
        Me.btnCSharpBrowse.TabIndex = 16
        Me.btnCSharpBrowse.Text = "..."
        Me.btnCSharpBrowse.UseVisualStyleBackColor = True
        '
        'txtCSharp
        '
        Me.txtCSharp.Location = New System.Drawing.Point(132, 183)
        Me.txtCSharp.Margin = New System.Windows.Forms.Padding(6)
        Me.txtCSharp.Name = "txtCSharp"
        Me.txtCSharp.Size = New System.Drawing.Size(756, 31)
        Me.txtCSharp.TabIndex = 15
        '
        'lblCSharp
        '
        Me.lblCSharp.AutoSize = True
        Me.lblCSharp.Location = New System.Drawing.Point(18, 194)
        Me.lblCSharp.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblCSharp.Name = "lblCSharp"
        Me.lblCSharp.Size = New System.Drawing.Size(45, 25)
        Me.lblCSharp.TabIndex = 14
        Me.lblCSharp.Text = "C#:"
        '
        'btnCPPBrowse
        '
        Me.btnCPPBrowse.Location = New System.Drawing.Point(904, 29)
        Me.btnCPPBrowse.Margin = New System.Windows.Forms.Padding(6)
        Me.btnCPPBrowse.Name = "btnCPPBrowse"
        Me.btnCPPBrowse.Size = New System.Drawing.Size(48, 44)
        Me.btnCPPBrowse.TabIndex = 13
        Me.btnCPPBrowse.Text = "..."
        Me.btnCPPBrowse.UseVisualStyleBackColor = True
        '
        'txtCPP
        '
        Me.txtCPP.Location = New System.Drawing.Point(132, 33)
        Me.txtCPP.Margin = New System.Windows.Forms.Padding(6)
        Me.txtCPP.Name = "txtCPP"
        Me.txtCPP.Size = New System.Drawing.Size(756, 31)
        Me.txtCPP.TabIndex = 12
        '
        'btnSQLEdit
        '
        Me.btnSQLEdit.Location = New System.Drawing.Point(964, 129)
        Me.btnSQLEdit.Margin = New System.Windows.Forms.Padding(6)
        Me.btnSQLEdit.Name = "btnSQLEdit"
        Me.btnSQLEdit.Size = New System.Drawing.Size(96, 44)
        Me.btnSQLEdit.TabIndex = 11
        Me.btnSQLEdit.Text = "Edit"
        Me.btnSQLEdit.UseVisualStyleBackColor = True
        '
        'btnSQLBrowse
        '
        Me.btnSQLBrowse.Location = New System.Drawing.Point(904, 129)
        Me.btnSQLBrowse.Margin = New System.Windows.Forms.Padding(6)
        Me.btnSQLBrowse.Name = "btnSQLBrowse"
        Me.btnSQLBrowse.Size = New System.Drawing.Size(48, 44)
        Me.btnSQLBrowse.TabIndex = 10
        Me.btnSQLBrowse.Text = "..."
        Me.btnSQLBrowse.UseVisualStyleBackColor = True
        '
        'txtPLSQL
        '
        Me.txtPLSQL.Location = New System.Drawing.Point(132, 133)
        Me.txtPLSQL.Margin = New System.Windows.Forms.Padding(6)
        Me.txtPLSQL.Name = "txtPLSQL"
        Me.txtPLSQL.Size = New System.Drawing.Size(756, 31)
        Me.txtPLSQL.TabIndex = 9
        '
        'btnJavaEdit
        '
        Me.btnJavaEdit.Location = New System.Drawing.Point(964, 79)
        Me.btnJavaEdit.Margin = New System.Windows.Forms.Padding(6)
        Me.btnJavaEdit.Name = "btnJavaEdit"
        Me.btnJavaEdit.Size = New System.Drawing.Size(96, 44)
        Me.btnJavaEdit.TabIndex = 8
        Me.btnJavaEdit.Text = "Edit"
        Me.btnJavaEdit.UseVisualStyleBackColor = True
        '
        'btnJavaBrowse
        '
        Me.btnJavaBrowse.Location = New System.Drawing.Point(904, 79)
        Me.btnJavaBrowse.Margin = New System.Windows.Forms.Padding(6)
        Me.btnJavaBrowse.Name = "btnJavaBrowse"
        Me.btnJavaBrowse.Size = New System.Drawing.Size(48, 44)
        Me.btnJavaBrowse.TabIndex = 7
        Me.btnJavaBrowse.Text = "..."
        Me.btnJavaBrowse.UseVisualStyleBackColor = True
        '
        'txtJava
        '
        Me.txtJava.Location = New System.Drawing.Point(132, 83)
        Me.txtJava.Margin = New System.Windows.Forms.Padding(6)
        Me.txtJava.Name = "txtJava"
        Me.txtJava.Size = New System.Drawing.Size(756, 31)
        Me.txtJava.TabIndex = 6
        '
        'btnCPPEdit
        '
        Me.btnCPPEdit.Location = New System.Drawing.Point(964, 29)
        Me.btnCPPEdit.Margin = New System.Windows.Forms.Padding(6)
        Me.btnCPPEdit.Name = "btnCPPEdit"
        Me.btnCPPEdit.Size = New System.Drawing.Size(96, 44)
        Me.btnCPPEdit.TabIndex = 5
        Me.btnCPPEdit.Text = "Edit"
        Me.btnCPPEdit.UseVisualStyleBackColor = True
        '
        'lblSQL
        '
        Me.lblSQL.AutoSize = True
        Me.lblSQL.Location = New System.Drawing.Point(18, 144)
        Me.lblSQL.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblSQL.Name = "lblSQL"
        Me.lblSQL.Size = New System.Drawing.Size(92, 25)
        Me.lblSQL.TabIndex = 2
        Me.lblSQL.Text = "PL/SQL:"
        '
        'lblJava
        '
        Me.lblJava.AutoSize = True
        Me.lblJava.Location = New System.Drawing.Point(18, 87)
        Me.lblJava.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblJava.Name = "lblJava"
        Me.lblJava.Size = New System.Drawing.Size(64, 25)
        Me.lblJava.TabIndex = 1
        Me.lblJava.Text = "Java:"
        '
        'lblCPP
        '
        Me.lblCPP.AutoSize = True
        Me.lblCPP.Location = New System.Drawing.Point(18, 38)
        Me.lblCPP.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblCPP.Name = "lblCPP"
        Me.lblCPP.Size = New System.Drawing.Size(78, 25)
        Me.lblCPP.TabIndex = 0
        Me.lblCPP.Text = "C/C++:"
        '
        'tpJava
        '
        Me.tpJava.Controls.Add(Me.GroupBox2)
        Me.tpJava.Controls.Add(Me.gbAndroid)
        Me.tpJava.Controls.Add(Me.gbOWASP)
        Me.tpJava.Location = New System.Drawing.Point(8, 39)
        Me.tpJava.Margin = New System.Windows.Forms.Padding(6)
        Me.tpJava.Name = "tpJava"
        Me.tpJava.Size = New System.Drawing.Size(1104, 534)
        Me.tpJava.TabIndex = 5
        Me.tpJava.Text = "Optional Settings"
        Me.tpJava.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.cbZOS)
        Me.GroupBox2.Controls.Add(Me.udCOBOLStart)
        Me.GroupBox2.Controls.Add(Me.lblCOBOLStart)
        Me.GroupBox2.Location = New System.Drawing.Point(16, 256)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(6)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(6)
        Me.GroupBox2.Size = New System.Drawing.Size(1072, 137)
        Me.GroupBox2.TabIndex = 13
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "COBOL Settings"
        '
        'udCOBOLStart
        '
        Me.udCOBOLStart.Location = New System.Drawing.Point(696, 42)
        Me.udCOBOLStart.Name = "udCOBOLStart"
        Me.udCOBOLStart.Size = New System.Drawing.Size(120, 31)
        Me.udCOBOLStart.TabIndex = 1
        '
        'lblCOBOLStart
        '
        Me.lblCOBOLStart.AutoSize = True
        Me.lblCOBOLStart.Location = New System.Drawing.Point(17, 42)
        Me.lblCOBOLStart.Name = "lblCOBOLStart"
        Me.lblCOBOLStart.Size = New System.Drawing.Size(666, 25)
        Me.lblCOBOLStart.TabIndex = 0
        Me.lblCOBOLStart.Text = "First Code Column Position (width of initial 'line number' column + 1):"
        '
        'gbAndroid
        '
        Me.gbAndroid.Controls.Add(Me.GroupBox1)
        Me.gbAndroid.Controls.Add(Me.cbAndroid)
        Me.gbAndroid.Location = New System.Drawing.Point(16, 135)
        Me.gbAndroid.Margin = New System.Windows.Forms.Padding(6)
        Me.gbAndroid.Name = "gbAndroid"
        Me.gbAndroid.Padding = New System.Windows.Forms.Padding(6)
        Me.gbAndroid.Size = New System.Drawing.Size(1072, 96)
        Me.gbAndroid.TabIndex = 12
        Me.gbAndroid.TabStop = False
        Me.gbAndroid.Text = "Mobile Applications"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CheckBox1)
        Me.GroupBox1.Location = New System.Drawing.Point(2, 133)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(6)
        Me.GroupBox1.Size = New System.Drawing.Size(1072, 96)
        Me.GroupBox1.TabIndex = 13
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Mobile Applications"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(22, 40)
        Me.CheckBox1.Margin = New System.Windows.Forms.Padding(6)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(323, 29)
        Me.CheckBox1.TabIndex = 12
        Me.CheckBox1.Text = "Include Java Android Checks"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'cbAndroid
        '
        Me.cbAndroid.AutoSize = True
        Me.cbAndroid.Location = New System.Drawing.Point(22, 40)
        Me.cbAndroid.Margin = New System.Windows.Forms.Padding(6)
        Me.cbAndroid.Name = "cbAndroid"
        Me.cbAndroid.Size = New System.Drawing.Size(323, 29)
        Me.cbAndroid.TabIndex = 12
        Me.cbAndroid.Text = "Include Java Android Checks"
        Me.cbAndroid.UseVisualStyleBackColor = True
        '
        'gbOWASP
        '
        Me.gbOWASP.Controls.Add(Me.cbInnerClass)
        Me.gbOWASP.Controls.Add(Me.cbFinalize)
        Me.gbOWASP.Location = New System.Drawing.Point(16, 27)
        Me.gbOWASP.Margin = New System.Windows.Forms.Padding(6)
        Me.gbOWASP.Name = "gbOWASP"
        Me.gbOWASP.Padding = New System.Windows.Forms.Padding(6)
        Me.gbOWASP.Size = New System.Drawing.Size(1072, 88)
        Me.gbOWASP.TabIndex = 10
        Me.gbOWASP.TabStop = False
        Me.gbOWASP.Text = "Java OWASP Recommendations"
        '
        'cbInnerClass
        '
        Me.cbInnerClass.AutoSize = True
        Me.cbInnerClass.Location = New System.Drawing.Point(502, 35)
        Me.cbInnerClass.Margin = New System.Windows.Forms.Padding(6)
        Me.cbInnerClass.Name = "cbInnerClass"
        Me.cbInnerClass.Size = New System.Drawing.Size(345, 29)
        Me.cbInnerClass.TabIndex = 1
        Me.cbInnerClass.Text = "Check for Nested Java Classes"
        Me.cbInnerClass.UseVisualStyleBackColor = True
        '
        'cbFinalize
        '
        Me.cbFinalize.AutoSize = True
        Me.cbFinalize.Location = New System.Drawing.Point(24, 35)
        Me.cbFinalize.Margin = New System.Windows.Forms.Padding(6)
        Me.cbFinalize.Name = "cbFinalize"
        Me.cbFinalize.Size = New System.Drawing.Size(411, 29)
        Me.cbFinalize.TabIndex = 0
        Me.cbFinalize.Text = "Check for Finalization of Java Classes"
        Me.cbFinalize.UseVisualStyleBackColor = True
        '
        'tpXMLExport
        '
        Me.tpXMLExport.Controls.Add(Me.btnExport)
        Me.tpXMLExport.Controls.Add(Me.gbFilter)
        Me.tpXMLExport.Controls.Add(Me.gbExportMode)
        Me.tpXMLExport.Location = New System.Drawing.Point(8, 39)
        Me.tpXMLExport.Margin = New System.Windows.Forms.Padding(6)
        Me.tpXMLExport.Name = "tpXMLExport"
        Me.tpXMLExport.Padding = New System.Windows.Forms.Padding(6)
        Me.tpXMLExport.Size = New System.Drawing.Size(1104, 534)
        Me.tpXMLExport.TabIndex = 2
        Me.tpXMLExport.Text = "Result Filter and XML Export"
        Me.tpXMLExport.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(902, 423)
        Me.btnExport.Margin = New System.Windows.Forms.Padding(6)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(184, 44)
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
        Me.gbFilter.Location = New System.Drawing.Point(14, 204)
        Me.gbFilter.Margin = New System.Windows.Forms.Padding(6)
        Me.gbFilter.Name = "gbFilter"
        Me.gbFilter.Padding = New System.Windows.Forms.Padding(6)
        Me.gbFilter.Size = New System.Drawing.Size(1072, 208)
        Me.gbFilter.TabIndex = 1
        Me.gbFilter.TabStop = False
        Me.gbFilter.Text = "Result Filter Options"
        '
        'cboMaximum
        '
        Me.cboMaximum.Enabled = False
        Me.cboMaximum.FormattingEnabled = True
        Me.cboMaximum.Items.AddRange(New Object() {"Potentially Unsafe", "Suspicious Comment", "Low", "Standard", "Medium", "High", "Critical"})
        Me.cboMaximum.Location = New System.Drawing.Point(744, 140)
        Me.cboMaximum.Margin = New System.Windows.Forms.Padding(6)
        Me.cboMaximum.Name = "cboMaximum"
        Me.cboMaximum.Size = New System.Drawing.Size(312, 33)
        Me.cboMaximum.TabIndex = 10
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(702, 146)
        Me.lblTo.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(36, 25)
        Me.lblTo.TabIndex = 9
        Me.lblTo.Text = "to:"
        '
        'rbRange
        '
        Me.rbRange.AutoSize = True
        Me.rbRange.Location = New System.Drawing.Point(12, 140)
        Me.rbRange.Margin = New System.Windows.Forms.Padding(6)
        Me.rbRange.Name = "rbRange"
        Me.rbRange.Size = New System.Drawing.Size(326, 29)
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
        Me.cboMinimum.Location = New System.Drawing.Point(388, 140)
        Me.cboMinimum.Margin = New System.Windows.Forms.Padding(6)
        Me.cboMinimum.Name = "cboMinimum"
        Me.cboMinimum.Size = New System.Drawing.Size(304, 33)
        Me.cboMinimum.TabIndex = 7
        '
        'rbBelow
        '
        Me.rbBelow.AutoSize = True
        Me.rbBelow.Location = New System.Drawing.Point(12, 88)
        Me.rbBelow.Margin = New System.Windows.Forms.Padding(6)
        Me.rbBelow.Name = "rbBelow"
        Me.rbBelow.Size = New System.Drawing.Size(372, 29)
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
        Me.cboBelow.Location = New System.Drawing.Point(388, 88)
        Me.cboBelow.Margin = New System.Windows.Forms.Padding(6)
        Me.cboBelow.Name = "cboBelow"
        Me.cboBelow.Size = New System.Drawing.Size(304, 33)
        Me.cboBelow.TabIndex = 5
        '
        'rbAbove
        '
        Me.rbAbove.AutoSize = True
        Me.rbAbove.Checked = True
        Me.rbAbove.Location = New System.Drawing.Point(12, 37)
        Me.rbAbove.Margin = New System.Windows.Forms.Padding(6)
        Me.rbAbove.Name = "rbAbove"
        Me.rbAbove.Size = New System.Drawing.Size(375, 29)
        Me.rbAbove.TabIndex = 4
        Me.rbAbove.TabStop = True
        Me.rbAbove.Text = "Display Results Equal to or Above:"
        Me.rbAbove.UseVisualStyleBackColor = True
        '
        'cboAbove
        '
        Me.cboAbove.FormattingEnabled = True
        Me.cboAbove.Items.AddRange(New Object() {"Potentially Unsafe", "Suspicious Comment", "Low", "Standard", "Medium", "High", "Critical"})
        Me.cboAbove.Location = New System.Drawing.Point(388, 37)
        Me.cboAbove.Margin = New System.Windows.Forms.Padding(6)
        Me.cboAbove.Name = "cboAbove"
        Me.cboAbove.Size = New System.Drawing.Size(304, 33)
        Me.cboAbove.TabIndex = 3
        '
        'gbExportMode
        '
        Me.gbExportMode.Controls.Add(Me.cbSaveState)
        Me.gbExportMode.Controls.Add(Me.rbFiltered)
        Me.gbExportMode.Controls.Add(Me.rbAll)
        Me.gbExportMode.Location = New System.Drawing.Point(14, 13)
        Me.gbExportMode.Margin = New System.Windows.Forms.Padding(6)
        Me.gbExportMode.Name = "gbExportMode"
        Me.gbExportMode.Padding = New System.Windows.Forms.Padding(6)
        Me.gbExportMode.Size = New System.Drawing.Size(1072, 177)
        Me.gbExportMode.TabIndex = 0
        Me.gbExportMode.TabStop = False
        Me.gbExportMode.Text = "Export Mode"
        '
        'cbSaveState
        '
        Me.cbSaveState.AutoSize = True
        Me.cbSaveState.Checked = True
        Me.cbSaveState.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbSaveState.Location = New System.Drawing.Point(14, 131)
        Me.cbSaveState.Margin = New System.Windows.Forms.Padding(6)
        Me.cbSaveState.Name = "cbSaveState"
        Me.cbSaveState.Size = New System.Drawing.Size(881, 29)
        Me.cbSaveState.TabIndex = 2
        Me.cbSaveState.Text = "Save CheckBox State of Marked Items (Summary Table State will be Preserved in XML" &
    ")"
        Me.cbSaveState.UseVisualStyleBackColor = True
        '
        'rbFiltered
        '
        Me.rbFiltered.AutoSize = True
        Me.rbFiltered.Location = New System.Drawing.Point(14, 85)
        Me.rbFiltered.Margin = New System.Windows.Forms.Padding(6)
        Me.rbFiltered.Name = "rbFiltered"
        Me.rbFiltered.Size = New System.Drawing.Size(261, 29)
        Me.rbFiltered.TabIndex = 1
        Me.rbFiltered.Text = "Export Filtered Results"
        Me.rbFiltered.UseVisualStyleBackColor = True
        '
        'rbAll
        '
        Me.rbAll.AutoSize = True
        Me.rbAll.Checked = True
        Me.rbAll.Location = New System.Drawing.Point(14, 38)
        Me.rbAll.Margin = New System.Windows.Forms.Padding(6)
        Me.rbAll.Name = "rbAll"
        Me.rbAll.Size = New System.Drawing.Size(213, 29)
        Me.rbAll.TabIndex = 0
        Me.rbAll.TabStop = True
        Me.rbAll.Text = "Export All Results"
        Me.rbAll.UseVisualStyleBackColor = True
        '
        'tpDisplay
        '
        Me.tpDisplay.Controls.Add(Me.gbDisplay)
        Me.tpDisplay.Location = New System.Drawing.Point(8, 39)
        Me.tpDisplay.Margin = New System.Windows.Forms.Padding(6)
        Me.tpDisplay.Name = "tpDisplay"
        Me.tpDisplay.Padding = New System.Windows.Forms.Padding(6)
        Me.tpDisplay.Size = New System.Drawing.Size(1104, 534)
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
        Me.gbDisplay.Location = New System.Drawing.Point(12, 12)
        Me.gbDisplay.Margin = New System.Windows.Forms.Padding(6)
        Me.gbDisplay.Name = "gbDisplay"
        Me.gbDisplay.Padding = New System.Windows.Forms.Padding(6)
        Me.gbDisplay.Size = New System.Drawing.Size(1080, 583)
        Me.gbDisplay.TabIndex = 0
        Me.gbDisplay.TabStop = False
        Me.gbDisplay.Text = "Display Options"
        '
        'cbShowStatusBar
        '
        Me.cbShowStatusBar.AutoSize = True
        Me.cbShowStatusBar.Location = New System.Drawing.Point(16, 165)
        Me.cbShowStatusBar.Margin = New System.Windows.Forms.Padding(6)
        Me.cbShowStatusBar.Name = "cbShowStatusBar"
        Me.cbShowStatusBar.Size = New System.Drawing.Size(203, 29)
        Me.cbShowStatusBar.TabIndex = 4
        Me.cbShowStatusBar.Text = "Show Status Bar"
        Me.cbShowStatusBar.UseVisualStyleBackColor = True
        '
        'lblColour
        '
        Me.lblColour.AutoSize = True
        Me.lblColour.Location = New System.Drawing.Point(8, 223)
        Me.lblColour.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblColour.Name = "lblColour"
        Me.lblColour.Size = New System.Drawing.Size(399, 25)
        Me.lblColour.TabIndex = 3
        Me.lblColour.Text = "Set Colour of Selected Items in ListView:"
        '
        'btnColour
        '
        Me.btnColour.Location = New System.Drawing.Point(402, 213)
        Me.btnColour.Margin = New System.Windows.Forms.Padding(6)
        Me.btnColour.Name = "btnColour"
        Me.btnColour.Size = New System.Drawing.Size(60, 42)
        Me.btnColour.TabIndex = 2
        Me.btnColour.Text = "..."
        Me.btnColour.UseVisualStyleBackColor = True
        '
        'cbReminder
        '
        Me.cbReminder.AutoSize = True
        Me.cbReminder.Location = New System.Drawing.Point(16, 56)
        Me.cbReminder.Margin = New System.Windows.Forms.Padding(6)
        Me.cbReminder.Name = "cbReminder"
        Me.cbReminder.Size = New System.Drawing.Size(562, 29)
        Me.cbReminder.TabIndex = 1
        Me.cbReminder.Text = "Show me a reminder to choose a language on start-up"
        Me.cbReminder.UseVisualStyleBackColor = True
        '
        'cbShowChart
        '
        Me.cbShowChart.AutoSize = True
        Me.cbShowChart.Location = New System.Drawing.Point(16, 113)
        Me.cbShowChart.Margin = New System.Windows.Forms.Padding(6)
        Me.cbShowChart.Name = "cbShowChart"
        Me.cbShowChart.Size = New System.Drawing.Size(557, 29)
        Me.cbShowChart.TabIndex = 0
        Me.cbShowChart.Text = "Show charts and code breakdown when scan finishes"
        Me.cbShowChart.UseVisualStyleBackColor = True
        '
        'tpBeta
        '
        Me.tpBeta.Controls.Add(Me.gbBeta)
        Me.tpBeta.Location = New System.Drawing.Point(8, 39)
        Me.tpBeta.Margin = New System.Windows.Forms.Padding(6)
        Me.tpBeta.Name = "tpBeta"
        Me.tpBeta.Size = New System.Drawing.Size(1104, 534)
        Me.tpBeta.TabIndex = 4
        Me.tpBeta.Text = "Beta Functionality"
        Me.tpBeta.UseVisualStyleBackColor = True
        '
        'gbBeta
        '
        Me.gbBeta.Controls.Add(Me.cbSigned)
        Me.gbBeta.Controls.Add(Me.cbCobol)
        Me.gbBeta.Location = New System.Drawing.Point(10, 12)
        Me.gbBeta.Margin = New System.Windows.Forms.Padding(6)
        Me.gbBeta.Name = "gbBeta"
        Me.gbBeta.Padding = New System.Windows.Forms.Padding(6)
        Me.gbBeta.Size = New System.Drawing.Size(1080, 583)
        Me.gbBeta.TabIndex = 1
        Me.gbBeta.TabStop = False
        Me.gbBeta.Text = "Beta Functionality Options"
        '
        'cbSigned
        '
        Me.cbSigned.AutoSize = True
        Me.cbSigned.Location = New System.Drawing.Point(16, 56)
        Me.cbSigned.Margin = New System.Windows.Forms.Padding(6)
        Me.cbSigned.Name = "cbSigned"
        Me.cbSigned.Size = New System.Drawing.Size(474, 29)
        Me.cbSigned.TabIndex = 1
        Me.cbSigned.Text = "Include signed/unsigned comparison (C/C++)"
        Me.cbSigned.UseVisualStyleBackColor = True
        '
        'cbCobol
        '
        Me.cbCobol.AutoSize = True
        Me.cbCobol.Location = New System.Drawing.Point(16, 113)
        Me.cbCobol.Margin = New System.Windows.Forms.Padding(6)
        Me.cbCobol.Name = "cbCobol"
        Me.cbCobol.Size = New System.Drawing.Size(406, 29)
        Me.cbCobol.TabIndex = 0
        Me.cbCobol.Text = "Include COBOL scanning functionality"
        Me.cbCobol.UseVisualStyleBackColor = True
        Me.cbCobol.Visible = False
        '
        'tpGrep
        '
        Me.tpGrep.Controls.Add(Me.txtTempGrepTitle)
        Me.tpGrep.Controls.Add(Me.txtTempGrep)
        Me.tpGrep.Location = New System.Drawing.Point(8, 39)
        Me.tpGrep.Margin = New System.Windows.Forms.Padding(6)
        Me.tpGrep.Name = "tpGrep"
        Me.tpGrep.Size = New System.Drawing.Size(1104, 534)
        Me.tpGrep.TabIndex = 6
        Me.tpGrep.Text = "Temporary Grep"
        Me.tpGrep.UseVisualStyleBackColor = True
        '
        'txtTempGrepTitle
        '
        Me.txtTempGrepTitle.Location = New System.Drawing.Point(16, 15)
        Me.txtTempGrepTitle.Margin = New System.Windows.Forms.Padding(6)
        Me.txtTempGrepTitle.Multiline = True
        Me.txtTempGrepTitle.Name = "txtTempGrepTitle"
        Me.txtTempGrepTitle.ReadOnly = True
        Me.txtTempGrepTitle.Size = New System.Drawing.Size(1058, 100)
        Me.txtTempGrepTitle.TabIndex = 1
        Me.txtTempGrepTitle.Text = resources.GetString("txtTempGrepTitle.Text")
        '
        'txtTempGrep
        '
        Me.txtTempGrep.Location = New System.Drawing.Point(16, 131)
        Me.txtTempGrep.Margin = New System.Windows.Forms.Padding(6)
        Me.txtTempGrep.Multiline = True
        Me.txtTempGrep.Name = "txtTempGrep"
        Me.txtTempGrep.Size = New System.Drawing.Size(1058, 373)
        Me.txtTempGrep.TabIndex = 0
        '
        'cbZOS
        '
        Me.cbZOS.AutoSize = True
        Me.cbZOS.Checked = True
        Me.cbZOS.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbZOS.Location = New System.Drawing.Point(22, 93)
        Me.cbZOS.Margin = New System.Windows.Forms.Padding(6)
        Me.cbZOS.Name = "cbZOS"
        Me.cbZOS.Size = New System.Drawing.Size(341, 29)
        Me.cbZOS.TabIndex = 14
        Me.cbZOS.Text = "Include CICS and z/OS Checks"
        Me.cbZOS.UseVisualStyleBackColor = True
        '
        'frmOptions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1130, 656)
        Me.Controls.Add(Me.tabOptions)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(6)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOptions"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Options"
        Me.tabOptions.ResumeLayout(False)
        Me.tpGeneral.ResumeLayout(False)
        Me.gbReporting.ResumeLayout(False)
        Me.gbReporting.PerformLayout()
        Me.gbOutput.ResumeLayout(False)
        Me.gbOutput.PerformLayout()
        Me.gbFileTypes.ResumeLayout(False)
        Me.gbFileTypes.PerformLayout()
        Me.gbLanguage.ResumeLayout(False)
        Me.gbLanguage.PerformLayout()
        Me.tpConfigFiles.ResumeLayout(False)
        Me.gbConfigFiles.ResumeLayout(False)
        Me.gbConfigFiles.PerformLayout()
        Me.tpJava.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.udCOBOLStart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbAndroid.ResumeLayout(False)
        Me.gbAndroid.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.gbOWASP.ResumeLayout(False)
        Me.gbOWASP.PerformLayout()
        Me.tpXMLExport.ResumeLayout(False)
        Me.gbFilter.ResumeLayout(False)
        Me.gbFilter.PerformLayout()
        Me.gbExportMode.ResumeLayout(False)
        Me.gbExportMode.PerformLayout()
        Me.tpDisplay.ResumeLayout(False)
        Me.gbDisplay.ResumeLayout(False)
        Me.gbDisplay.PerformLayout()
        Me.tpBeta.ResumeLayout(False)
        Me.gbBeta.ResumeLayout(False)
        Me.gbBeta.PerformLayout()
        Me.tpGrep.ResumeLayout(False)
        Me.tpGrep.PerformLayout()
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
    Friend WithEvents tpBeta As System.Windows.Forms.TabPage
    Friend WithEvents gbBeta As System.Windows.Forms.GroupBox
    Friend WithEvents cbSigned As System.Windows.Forms.CheckBox
    Friend WithEvents cbCobol As System.Windows.Forms.CheckBox
    Friend WithEvents tpJava As System.Windows.Forms.TabPage
    Friend WithEvents gbOWASP As System.Windows.Forms.GroupBox
    Friend WithEvents cbInnerClass As System.Windows.Forms.CheckBox
    Friend WithEvents cbFinalize As System.Windows.Forms.CheckBox
    Friend WithEvents tpGrep As System.Windows.Forms.TabPage
    Friend WithEvents txtTempGrep As System.Windows.Forms.TextBox
    Friend WithEvents txtTempGrepTitle As System.Windows.Forms.TextBox
    Friend WithEvents gbAndroid As System.Windows.Forms.GroupBox
    Friend WithEvents cbAndroid As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents udCOBOLStart As NumericUpDown
    Friend WithEvents lblCOBOLStart As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents cbZOS As CheckBox
End Class
