<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.mnuMain = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewTargetToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewTargetFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveResultsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem9 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem10 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem11 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem14 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem15 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem8 = New System.Windows.Forms.ToolStripSeparator()
        Me.FindToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripSeparator()
        Me.SelectAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupRichTextResultsByIssueToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupRichTextResultsByFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowIndividualRichTextResultsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem16 = New System.Windows.Forms.ToolStripSeparator()
        Me.StatusBarToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ScanToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StartScanningToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripSeparator()
        Me.VisualCodeBreakdownToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VisualSecurityBreakdownToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VisualBadFuncBreakdownToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VisualCommentBreakdownToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem7 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExportFixMeCommentsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem12 = New System.Windows.Forms.ToolStripSeparator()
        Me.SortRichTextResultsOnSeverityToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SortRichTextResultsOnFileNameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem13 = New System.Windows.Forms.ToolStripSeparator()
        Me.FilterResultsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.DeleteItemToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BannedFunctionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.CCToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.JavaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PLSQLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CSToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VBToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PHPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.COBOLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator()
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutVCGToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.fbFolderBrowser = New System.Windows.Forms.FolderBrowserDialog()
        Me.ssStatusStrip = New System.Windows.Forms.StatusStrip()
        Me.sslLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.spMain = New System.Windows.Forms.SplitContainer()
        Me.cboTargetDir = New System.Windows.Forms.ComboBox()
        Me.tcMain = New System.Windows.Forms.TabControl()
        Me.tabTargetFiles = New System.Windows.Forms.TabPage()
        Me.lbTargets = New System.Windows.Forms.ListBox()
        Me.tabResults = New System.Windows.Forms.TabPage()
        Me.rtbResults = New System.Windows.Forms.RichTextBox()
        Me.tabResultsTable = New System.Windows.Forms.TabPage()
        Me.lvResults = New System.Windows.Forms.ListView()
        Me.chSeverityRanking = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chSeverity = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chTitle = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chDescription = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chFileName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chLine = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.sfdSaveFileDialog = New System.Windows.Forms.SaveFileDialog()
        Me.ofdOpenFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.cdColorDialog = New System.Windows.Forms.ColorDialog()
        Me.mnuMain.SuspendLayout()
        Me.ssStatusStrip.SuspendLayout()
        CType(Me.spMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.spMain.Panel1.SuspendLayout()
        Me.spMain.Panel2.SuspendLayout()
        Me.spMain.SuspendLayout()
        Me.tcMain.SuspendLayout()
        Me.tabTargetFiles.SuspendLayout()
        Me.tabResults.SuspendLayout()
        Me.tabResultsTable.SuspendLayout()
        Me.SuspendLayout()
        '
        'mnuMain
        '
        Me.mnuMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.ViewToolStripMenuItem, Me.ScanToolStripMenuItem, Me.ToolsToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.mnuMain.Location = New System.Drawing.Point(0, 0)
        Me.mnuMain.Name = "mnuMain"
        Me.mnuMain.Size = New System.Drawing.Size(819, 24)
        Me.mnuMain.TabIndex = 0
        Me.mnuMain.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewTargetToolStripMenuItem, Me.NewTargetFileToolStripMenuItem, Me.SaveResultsToolStripMenuItem, Me.ToolStripMenuItem5, Me.ToolStripMenuItem9, Me.ToolStripMenuItem1, Me.ToolStripMenuItem10, Me.ToolStripMenuItem11, Me.ToolStripSeparator1, Me.ToolStripMenuItem14, Me.ToolStripMenuItem15, Me.ToolStripSeparator2, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'NewTargetToolStripMenuItem
        '
        Me.NewTargetToolStripMenuItem.Name = "NewTargetToolStripMenuItem"
        Me.NewTargetToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.NewTargetToolStripMenuItem.Size = New System.Drawing.Size(238, 22)
        Me.NewTargetToolStripMenuItem.Text = "New Target Directory..."
        '
        'NewTargetFileToolStripMenuItem
        '
        Me.NewTargetFileToolStripMenuItem.Name = "NewTargetFileToolStripMenuItem"
        Me.NewTargetFileToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.T), System.Windows.Forms.Keys)
        Me.NewTargetFileToolStripMenuItem.Size = New System.Drawing.Size(238, 22)
        Me.NewTargetFileToolStripMenuItem.Text = "New Target File..."
        '
        'SaveResultsToolStripMenuItem
        '
        Me.SaveResultsToolStripMenuItem.Name = "SaveResultsToolStripMenuItem"
        Me.SaveResultsToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.SaveResultsToolStripMenuItem.Size = New System.Drawing.Size(238, 22)
        Me.SaveResultsToolStripMenuItem.Text = "Save Results as Text..."
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(235, 6)
        '
        'ToolStripMenuItem9
        '
        Me.ToolStripMenuItem9.Name = "ToolStripMenuItem9"
        Me.ToolStripMenuItem9.Size = New System.Drawing.Size(238, 22)
        Me.ToolStripMenuItem9.Text = "Clear"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(235, 6)
        '
        'ToolStripMenuItem10
        '
        Me.ToolStripMenuItem10.Name = "ToolStripMenuItem10"
        Me.ToolStripMenuItem10.Size = New System.Drawing.Size(238, 22)
        Me.ToolStripMenuItem10.Text = "Export Results as XML..."
        '
        'ToolStripMenuItem11
        '
        Me.ToolStripMenuItem11.Name = "ToolStripMenuItem11"
        Me.ToolStripMenuItem11.Size = New System.Drawing.Size(238, 22)
        Me.ToolStripMenuItem11.Text = "Import Results from XML File..."
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(235, 6)
        '
        'ToolStripMenuItem14
        '
        Me.ToolStripMenuItem14.Name = "ToolStripMenuItem14"
        Me.ToolStripMenuItem14.Size = New System.Drawing.Size(238, 22)
        Me.ToolStripMenuItem14.Text = "Export Results to CSV File.."
        '
        'ToolStripMenuItem15
        '
        Me.ToolStripMenuItem15.Name = "ToolStripMenuItem15"
        Me.ToolStripMenuItem15.Size = New System.Drawing.Size(238, 22)
        Me.ToolStripMenuItem15.Text = "Import Results from CSV File..."
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(235, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F4), System.Windows.Forms.Keys)
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(238, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CutToolStripMenuItem, Me.CopyToolStripMenuItem, Me.PasteToolStripMenuItem, Me.ToolStripMenuItem8, Me.FindToolStripMenuItem, Me.ToolStripMenuItem6, Me.SelectAllToolStripMenuItem})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
        Me.EditToolStripMenuItem.Text = "Edit"
        '
        'CutToolStripMenuItem
        '
        Me.CutToolStripMenuItem.Name = "CutToolStripMenuItem"
        Me.CutToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.CutToolStripMenuItem.Text = "Cut"
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.CopyToolStripMenuItem.Text = "Copy"
        '
        'PasteToolStripMenuItem
        '
        Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
        Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.PasteToolStripMenuItem.Text = "Paste"
        '
        'ToolStripMenuItem8
        '
        Me.ToolStripMenuItem8.Name = "ToolStripMenuItem8"
        Me.ToolStripMenuItem8.Size = New System.Drawing.Size(119, 6)
        '
        'FindToolStripMenuItem
        '
        Me.FindToolStripMenuItem.Name = "FindToolStripMenuItem"
        Me.FindToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.FindToolStripMenuItem.Text = "Find"
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        Me.ToolStripMenuItem6.Size = New System.Drawing.Size(119, 6)
        '
        'SelectAllToolStripMenuItem
        '
        Me.SelectAllToolStripMenuItem.Name = "SelectAllToolStripMenuItem"
        Me.SelectAllToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.SelectAllToolStripMenuItem.Text = "Select All"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GroupRichTextResultsByIssueToolStripMenuItem, Me.GroupRichTextResultsByFileToolStripMenuItem, Me.ShowIndividualRichTextResultsToolStripMenuItem, Me.ToolStripMenuItem16, Me.StatusBarToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.ViewToolStripMenuItem.Text = "View"
        '
        'GroupRichTextResultsByIssueToolStripMenuItem
        '
        Me.GroupRichTextResultsByIssueToolStripMenuItem.CheckOnClick = True
        Me.GroupRichTextResultsByIssueToolStripMenuItem.Name = "GroupRichTextResultsByIssueToolStripMenuItem"
        Me.GroupRichTextResultsByIssueToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.GroupRichTextResultsByIssueToolStripMenuItem.Text = "Group Rich Text Results by Issue"
        '
        'GroupRichTextResultsByFileToolStripMenuItem
        '
        Me.GroupRichTextResultsByFileToolStripMenuItem.CheckOnClick = True
        Me.GroupRichTextResultsByFileToolStripMenuItem.Name = "GroupRichTextResultsByFileToolStripMenuItem"
        Me.GroupRichTextResultsByFileToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.GroupRichTextResultsByFileToolStripMenuItem.Text = "Group Rich Text Results by File"
        '
        'ShowIndividualRichTextResultsToolStripMenuItem
        '
        Me.ShowIndividualRichTextResultsToolStripMenuItem.Checked = True
        Me.ShowIndividualRichTextResultsToolStripMenuItem.CheckOnClick = True
        Me.ShowIndividualRichTextResultsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ShowIndividualRichTextResultsToolStripMenuItem.Name = "ShowIndividualRichTextResultsToolStripMenuItem"
        Me.ShowIndividualRichTextResultsToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.ShowIndividualRichTextResultsToolStripMenuItem.Text = "Show Individual Rich Text Results"
        '
        'ToolStripMenuItem16
        '
        Me.ToolStripMenuItem16.Name = "ToolStripMenuItem16"
        Me.ToolStripMenuItem16.Size = New System.Drawing.Size(246, 6)
        '
        'StatusBarToolStripMenuItem
        '
        Me.StatusBarToolStripMenuItem.Checked = True
        Me.StatusBarToolStripMenuItem.CheckOnClick = True
        Me.StatusBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.StatusBarToolStripMenuItem.Name = "StatusBarToolStripMenuItem"
        Me.StatusBarToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.StatusBarToolStripMenuItem.Text = "Status Bar"
        '
        'ScanToolStripMenuItem
        '
        Me.ScanToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StartScanningToolStripMenuItem, Me.ToolStripMenuItem4, Me.VisualCodeBreakdownToolStripMenuItem, Me.VisualSecurityBreakdownToolStripMenuItem, Me.VisualBadFuncBreakdownToolStripMenuItem, Me.VisualCommentBreakdownToolStripMenuItem, Me.ToolStripMenuItem7, Me.ExportFixMeCommentsToolStripMenuItem, Me.ToolStripMenuItem12, Me.SortRichTextResultsOnSeverityToolStripMenuItem, Me.SortRichTextResultsOnFileNameToolStripMenuItem, Me.ToolStripMenuItem13, Me.FilterResultsToolStripMenuItem, Me.ToolStripSeparator3, Me.DeleteItemToolStripMenuItem})
        Me.ScanToolStripMenuItem.Name = "ScanToolStripMenuItem"
        Me.ScanToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.ScanToolStripMenuItem.Text = "Scan"
        '
        'StartScanningToolStripMenuItem
        '
        Me.StartScanningToolStripMenuItem.Enabled = False
        Me.StartScanningToolStripMenuItem.Name = "StartScanningToolStripMenuItem"
        Me.StartScanningToolStripMenuItem.Size = New System.Drawing.Size(366, 22)
        Me.StartScanningToolStripMenuItem.Text = "Full Scan"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(363, 6)
        '
        'VisualCodeBreakdownToolStripMenuItem
        '
        Me.VisualCodeBreakdownToolStripMenuItem.Enabled = False
        Me.VisualCodeBreakdownToolStripMenuItem.Name = "VisualCodeBreakdownToolStripMenuItem"
        Me.VisualCodeBreakdownToolStripMenuItem.Size = New System.Drawing.Size(366, 22)
        Me.VisualCodeBreakdownToolStripMenuItem.Text = "Visual Code/Comment Breakdown"
        '
        'VisualSecurityBreakdownToolStripMenuItem
        '
        Me.VisualSecurityBreakdownToolStripMenuItem.Enabled = False
        Me.VisualSecurityBreakdownToolStripMenuItem.Name = "VisualSecurityBreakdownToolStripMenuItem"
        Me.VisualSecurityBreakdownToolStripMenuItem.Size = New System.Drawing.Size(366, 22)
        Me.VisualSecurityBreakdownToolStripMenuItem.Text = "Scan Code Only (Ignore Comments)"
        '
        'VisualBadFuncBreakdownToolStripMenuItem
        '
        Me.VisualBadFuncBreakdownToolStripMenuItem.Enabled = False
        Me.VisualBadFuncBreakdownToolStripMenuItem.Name = "VisualBadFuncBreakdownToolStripMenuItem"
        Me.VisualBadFuncBreakdownToolStripMenuItem.Size = New System.Drawing.Size(366, 22)
        Me.VisualBadFuncBreakdownToolStripMenuItem.Text = "Scan For Bad Functions Only (As Defined in Config File)"
        '
        'VisualCommentBreakdownToolStripMenuItem
        '
        Me.VisualCommentBreakdownToolStripMenuItem.Enabled = False
        Me.VisualCommentBreakdownToolStripMenuItem.Name = "VisualCommentBreakdownToolStripMenuItem"
        Me.VisualCommentBreakdownToolStripMenuItem.Size = New System.Drawing.Size(366, 22)
        Me.VisualCommentBreakdownToolStripMenuItem.Text = "Scan Comments Only"
        '
        'ToolStripMenuItem7
        '
        Me.ToolStripMenuItem7.Name = "ToolStripMenuItem7"
        Me.ToolStripMenuItem7.Size = New System.Drawing.Size(363, 6)
        '
        'ExportFixMeCommentsToolStripMenuItem
        '
        Me.ExportFixMeCommentsToolStripMenuItem.Enabled = False
        Me.ExportFixMeCommentsToolStripMenuItem.Name = "ExportFixMeCommentsToolStripMenuItem"
        Me.ExportFixMeCommentsToolStripMenuItem.Size = New System.Drawing.Size(366, 22)
        Me.ExportFixMeCommentsToolStripMenuItem.Text = "Show All 'FixMe' Comments"
        '
        'ToolStripMenuItem12
        '
        Me.ToolStripMenuItem12.Name = "ToolStripMenuItem12"
        Me.ToolStripMenuItem12.Size = New System.Drawing.Size(363, 6)
        '
        'SortRichTextResultsOnSeverityToolStripMenuItem
        '
        Me.SortRichTextResultsOnSeverityToolStripMenuItem.Name = "SortRichTextResultsOnSeverityToolStripMenuItem"
        Me.SortRichTextResultsOnSeverityToolStripMenuItem.Size = New System.Drawing.Size(366, 22)
        Me.SortRichTextResultsOnSeverityToolStripMenuItem.Text = "Sort Rich Text Results on Severity"
        '
        'SortRichTextResultsOnFileNameToolStripMenuItem
        '
        Me.SortRichTextResultsOnFileNameToolStripMenuItem.Name = "SortRichTextResultsOnFileNameToolStripMenuItem"
        Me.SortRichTextResultsOnFileNameToolStripMenuItem.Size = New System.Drawing.Size(366, 22)
        Me.SortRichTextResultsOnFileNameToolStripMenuItem.Text = "Sort Rich Text Results on FileName"
        '
        'ToolStripMenuItem13
        '
        Me.ToolStripMenuItem13.Name = "ToolStripMenuItem13"
        Me.ToolStripMenuItem13.Size = New System.Drawing.Size(363, 6)
        '
        'FilterResultsToolStripMenuItem
        '
        Me.FilterResultsToolStripMenuItem.Name = "FilterResultsToolStripMenuItem"
        Me.FilterResultsToolStripMenuItem.Size = New System.Drawing.Size(366, 22)
        Me.FilterResultsToolStripMenuItem.Text = "Filter Results..."
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(363, 6)
        '
        'DeleteItemToolStripMenuItem
        '
        Me.DeleteItemToolStripMenuItem.Name = "DeleteItemToolStripMenuItem"
        Me.DeleteItemToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.DeleteItemToolStripMenuItem.Size = New System.Drawing.Size(366, 22)
        Me.DeleteItemToolStripMenuItem.Text = "Delete Selected Item(s)"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BannedFunctionsToolStripMenuItem, Me.ToolStripMenuItem2, Me.CCToolStripMenuItem, Me.JavaToolStripMenuItem, Me.PLSQLToolStripMenuItem, Me.CSToolStripMenuItem, Me.VBToolStripMenuItem, Me.PHPToolStripMenuItem, Me.COBOLToolStripMenuItem, Me.ToolStripMenuItem3, Me.OptionsToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.ToolsToolStripMenuItem.Text = "Settings"
        '
        'BannedFunctionsToolStripMenuItem
        '
        Me.BannedFunctionsToolStripMenuItem.Name = "BannedFunctionsToolStripMenuItem"
        Me.BannedFunctionsToolStripMenuItem.Size = New System.Drawing.Size(227, 22)
        Me.BannedFunctionsToolStripMenuItem.Text = "Banned/Insecure Functions..."
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(224, 6)
        '
        'CCToolStripMenuItem
        '
        Me.CCToolStripMenuItem.Checked = True
        Me.CCToolStripMenuItem.CheckOnClick = True
        Me.CCToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CCToolStripMenuItem.Name = "CCToolStripMenuItem"
        Me.CCToolStripMenuItem.Size = New System.Drawing.Size(227, 22)
        Me.CCToolStripMenuItem.Text = "C/C++"
        '
        'JavaToolStripMenuItem
        '
        Me.JavaToolStripMenuItem.CheckOnClick = True
        Me.JavaToolStripMenuItem.Name = "JavaToolStripMenuItem"
        Me.JavaToolStripMenuItem.Size = New System.Drawing.Size(227, 22)
        Me.JavaToolStripMenuItem.Text = "Java"
        '
        'PLSQLToolStripMenuItem
        '
        Me.PLSQLToolStripMenuItem.CheckOnClick = True
        Me.PLSQLToolStripMenuItem.Name = "PLSQLToolStripMenuItem"
        Me.PLSQLToolStripMenuItem.Size = New System.Drawing.Size(227, 22)
        Me.PLSQLToolStripMenuItem.Text = "PL/SQL"
        '
        'CSToolStripMenuItem
        '
        Me.CSToolStripMenuItem.CheckOnClick = True
        Me.CSToolStripMenuItem.Name = "CSToolStripMenuItem"
        Me.CSToolStripMenuItem.Size = New System.Drawing.Size(227, 22)
        Me.CSToolStripMenuItem.Text = "C#"
        '
        'VBToolStripMenuItem
        '
        Me.VBToolStripMenuItem.CheckOnClick = True
        Me.VBToolStripMenuItem.Name = "VBToolStripMenuItem"
        Me.VBToolStripMenuItem.Size = New System.Drawing.Size(227, 22)
        Me.VBToolStripMenuItem.Text = "VB"
        '
        'PHPToolStripMenuItem
        '
        Me.PHPToolStripMenuItem.CheckOnClick = True
        Me.PHPToolStripMenuItem.Name = "PHPToolStripMenuItem"
        Me.PHPToolStripMenuItem.Size = New System.Drawing.Size(227, 22)
        Me.PHPToolStripMenuItem.Text = "PHP"
        '
        'COBOLToolStripMenuItem
        '
        Me.COBOLToolStripMenuItem.CheckOnClick = True
        Me.COBOLToolStripMenuItem.Name = "COBOLToolStripMenuItem"
        Me.COBOLToolStripMenuItem.Size = New System.Drawing.Size(227, 22)
        Me.COBOLToolStripMenuItem.Text = "COBOL"
        Me.COBOLToolStripMenuItem.Visible = False
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(224, 6)
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(227, 22)
        Me.OptionsToolStripMenuItem.Text = "Options..."
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutVCGToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'AboutVCGToolStripMenuItem
        '
        Me.AboutVCGToolStripMenuItem.Name = "AboutVCGToolStripMenuItem"
        Me.AboutVCGToolStripMenuItem.Size = New System.Drawing.Size(133, 22)
        Me.AboutVCGToolStripMenuItem.Text = "About VCG"
        '
        'ssStatusStrip
        '
        Me.ssStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.sslLabel})
        Me.ssStatusStrip.Location = New System.Drawing.Point(0, 452)
        Me.ssStatusStrip.Name = "ssStatusStrip"
        Me.ssStatusStrip.Size = New System.Drawing.Size(819, 22)
        Me.ssStatusStrip.TabIndex = 1
        '
        'sslLabel
        '
        Me.sslLabel.Name = "sslLabel"
        Me.sslLabel.Size = New System.Drawing.Size(102, 17)
        Me.sslLabel.Text = "Language: C/C++"
        '
        'spMain
        '
        Me.spMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.spMain.IsSplitterFixed = True
        Me.spMain.Location = New System.Drawing.Point(0, 24)
        Me.spMain.Name = "spMain"
        Me.spMain.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'spMain.Panel1
        '
        Me.spMain.Panel1.Controls.Add(Me.cboTargetDir)
        '
        'spMain.Panel2
        '
        Me.spMain.Panel2.Controls.Add(Me.tcMain)
        Me.spMain.Size = New System.Drawing.Size(819, 428)
        Me.spMain.SplitterDistance = 25
        Me.spMain.TabIndex = 2
        '
        'cboTargetDir
        '
        Me.cboTargetDir.AllowDrop = True
        Me.cboTargetDir.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cboTargetDir.FormattingEnabled = True
        Me.cboTargetDir.Location = New System.Drawing.Point(0, 0)
        Me.cboTargetDir.Name = "cboTargetDir"
        Me.cboTargetDir.Size = New System.Drawing.Size(819, 21)
        Me.cboTargetDir.TabIndex = 0
        '
        'tcMain
        '
        Me.tcMain.Controls.Add(Me.tabTargetFiles)
        Me.tcMain.Controls.Add(Me.tabResults)
        Me.tcMain.Controls.Add(Me.tabResultsTable)
        Me.tcMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcMain.Location = New System.Drawing.Point(0, 0)
        Me.tcMain.Name = "tcMain"
        Me.tcMain.SelectedIndex = 0
        Me.tcMain.Size = New System.Drawing.Size(819, 399)
        Me.tcMain.TabIndex = 1
        '
        'tabTargetFiles
        '
        Me.tabTargetFiles.Controls.Add(Me.lbTargets)
        Me.tabTargetFiles.Location = New System.Drawing.Point(4, 22)
        Me.tabTargetFiles.Name = "tabTargetFiles"
        Me.tabTargetFiles.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTargetFiles.Size = New System.Drawing.Size(811, 373)
        Me.tabTargetFiles.TabIndex = 0
        Me.tabTargetFiles.Text = "Target Files"
        Me.tabTargetFiles.UseVisualStyleBackColor = True
        '
        'lbTargets
        '
        Me.lbTargets.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbTargets.FormattingEnabled = True
        Me.lbTargets.Location = New System.Drawing.Point(3, 3)
        Me.lbTargets.Name = "lbTargets"
        Me.lbTargets.Size = New System.Drawing.Size(805, 367)
        Me.lbTargets.TabIndex = 1
        '
        'tabResults
        '
        Me.tabResults.Controls.Add(Me.rtbResults)
        Me.tabResults.Location = New System.Drawing.Point(4, 22)
        Me.tabResults.Name = "tabResults"
        Me.tabResults.Padding = New System.Windows.Forms.Padding(3)
        Me.tabResults.Size = New System.Drawing.Size(811, 373)
        Me.tabResults.TabIndex = 1
        Me.tabResults.Text = "Results"
        Me.tabResults.UseVisualStyleBackColor = True
        '
        'rtbResults
        '
        Me.rtbResults.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbResults.Location = New System.Drawing.Point(3, 3)
        Me.rtbResults.Name = "rtbResults"
        Me.rtbResults.Size = New System.Drawing.Size(805, 367)
        Me.rtbResults.TabIndex = 0
        Me.rtbResults.Text = ""
        '
        'tabResultsTable
        '
        Me.tabResultsTable.Controls.Add(Me.lvResults)
        Me.tabResultsTable.Location = New System.Drawing.Point(4, 22)
        Me.tabResultsTable.Name = "tabResultsTable"
        Me.tabResultsTable.Padding = New System.Windows.Forms.Padding(3)
        Me.tabResultsTable.Size = New System.Drawing.Size(811, 373)
        Me.tabResultsTable.TabIndex = 2
        Me.tabResultsTable.Text = "Summary Table"
        Me.tabResultsTable.UseVisualStyleBackColor = True
        '
        'lvResults
        '
        Me.lvResults.AllowColumnReorder = True
        Me.lvResults.CheckBoxes = True
        Me.lvResults.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chSeverityRanking, Me.chSeverity, Me.chTitle, Me.chDescription, Me.chFileName, Me.chLine})
        Me.lvResults.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvResults.FullRowSelect = True
        Me.lvResults.HideSelection = False
        Me.lvResults.Location = New System.Drawing.Point(3, 3)
        Me.lvResults.Name = "lvResults"
        Me.lvResults.Size = New System.Drawing.Size(805, 367)
        Me.lvResults.TabIndex = 0
        Me.lvResults.UseCompatibleStateImageBehavior = False
        Me.lvResults.View = System.Windows.Forms.View.Details
        '
        'chSeverityRanking
        '
        Me.chSeverityRanking.Text = "Priority"
        Me.chSeverityRanking.Width = 43
        '
        'chSeverity
        '
        Me.chSeverity.Text = "Severity"
        Me.chSeverity.Width = 75
        '
        'chTitle
        '
        Me.chTitle.Text = "Title"
        Me.chTitle.Width = 229
        '
        'chDescription
        '
        Me.chDescription.Text = "Description"
        Me.chDescription.Width = 399
        '
        'chFileName
        '
        Me.chFileName.Text = "FileName"
        Me.chFileName.Width = 378
        '
        'chLine
        '
        Me.chLine.Text = "Line"
        '
        'ofdOpenFileDialog
        '
        Me.ofdOpenFileDialog.FileName = "XmlResults"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(819, 474)
        Me.Controls.Add(Me.spMain)
        Me.Controls.Add(Me.ssStatusStrip)
        Me.Controls.Add(Me.mnuMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.mnuMain
        Me.Name = "frmMain"
        Me.Text = "VCG"
        Me.mnuMain.ResumeLayout(False)
        Me.mnuMain.PerformLayout()
        Me.ssStatusStrip.ResumeLayout(False)
        Me.ssStatusStrip.PerformLayout()
        Me.spMain.Panel1.ResumeLayout(False)
        Me.spMain.Panel2.ResumeLayout(False)
        CType(Me.spMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.spMain.ResumeLayout(False)
        Me.tcMain.ResumeLayout(False)
        Me.tabTargetFiles.ResumeLayout(False)
        Me.tabResults.ResumeLayout(False)
        Me.tabResultsTable.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents mnuMain As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewTargetToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveResultsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BannedFunctionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CCToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents JavaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PLSQLToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutVCGToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents fbFolderBrowser As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents ssStatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents spMain As System.Windows.Forms.SplitContainer
    Friend WithEvents ScanToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StartScanningToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents VisualCodeBreakdownToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents VisualSecurityBreakdownToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cboTargetDir As System.Windows.Forms.ComboBox
    Friend WithEvents tcMain As System.Windows.Forms.TabControl
    Friend WithEvents tabTargetFiles As System.Windows.Forms.TabPage
    Friend WithEvents lbTargets As System.Windows.Forms.ListBox
    Friend WithEvents tabResults As System.Windows.Forms.TabPage
    Friend WithEvents FindToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SelectAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rtbResults As System.Windows.Forms.RichTextBox
    Friend WithEvents ToolStripMenuItem7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExportFixMeCommentsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents sslLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents sfdSaveFileDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ToolStripMenuItem8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem9 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabResultsTable As System.Windows.Forms.TabPage
    Friend WithEvents lvResults As System.Windows.Forms.ListView
    Friend WithEvents chSeverity As System.Windows.Forms.ColumnHeader
    Friend WithEvents chTitle As System.Windows.Forms.ColumnHeader
    Friend WithEvents chDescription As System.Windows.Forms.ColumnHeader
    Friend WithEvents chFileName As System.Windows.Forms.ColumnHeader
    Friend WithEvents chSeverityRanking As System.Windows.Forms.ColumnHeader
    Friend WithEvents ofdOpenFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ToolStripMenuItem10 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem11 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents chLine As System.Windows.Forms.ColumnHeader
    Friend WithEvents ToolStripMenuItem12 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SortRichTextResultsOnSeverityToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SortRichTextResultsOnFileNameToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CSToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem13 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents FilterResultsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents VBToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PHPToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents COBOLToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents VisualCommentBreakdownToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewTargetFileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cdColorDialog As System.Windows.Forms.ColorDialog
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupRichTextResultsByIssueToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupRichTextResultsByFileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem14 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem15 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ShowIndividualRichTextResultsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DeleteItemToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem16 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents StatusBarToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents VisualBadFuncBreakdownToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
