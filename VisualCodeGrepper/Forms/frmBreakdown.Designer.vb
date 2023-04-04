<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBreakdown
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
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.OpenInNotepadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.dgvResults = New System.Windows.Forms.DataGridView()
        Me.clmName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TotalLines = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PercentageTotal = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LinesofCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CommentLines = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Whitespace = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmFixme = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CodeIssues = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FullPath = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportToClipboardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuBreakdown = New System.Windows.Forms.MenuStrip()
        Me.txtFilter = New System.Windows.Forms.TextBox()
        Me.lblDoubleClick = New System.Windows.Forms.Label()
        Me.lblResults = New System.Windows.Forms.Label()
        Me.pnlResults = New System.Windows.Forms.Panel()
        Me.btnApplyFilter = New System.Windows.Forms.Button()
        Me.chtResults = New System.Windows.Forms.DataVisualization.Charting.Chart()
        CType(Me.dgvResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnuBreakdown.SuspendLayout()
        Me.pnlResults.SuspendLayout()
        CType(Me.chtResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'OpenInNotepadToolStripMenuItem
        '
        Me.OpenInNotepadToolStripMenuItem.Name = "OpenInNotepadToolStripMenuItem"
        Me.OpenInNotepadToolStripMenuItem.Size = New System.Drawing.Size(233, 22)
        Me.OpenInNotepadToolStripMenuItem.Text = "Open Selected File in Notepad"
        '
        'dgvResults
        '
        Me.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResults.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.clmName, Me.TotalLines, Me.PercentageTotal, Me.LinesofCode, Me.CommentLines, Me.Whitespace, Me.clmFixme, Me.CodeIssues, Me.FullPath})
        Me.dgvResults.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvResults.Location = New System.Drawing.Point(0, 476)
        Me.dgvResults.Name = "dgvResults"
        Me.dgvResults.Size = New System.Drawing.Size(1225, 445)
        Me.dgvResults.TabIndex = 7
        '
        'clmName
        '
        Me.clmName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.clmName.HeaderText = "Name"
        Me.clmName.Name = "clmName"
        Me.clmName.ReadOnly = True
        Me.clmName.Width = 60
        '
        'TotalLines
        '
        Me.TotalLines.HeaderText = "Total Lines"
        Me.TotalLines.Name = "TotalLines"
        '
        'PercentageTotal
        '
        Me.PercentageTotal.HeaderText = "Percentage of Total"
        Me.PercentageTotal.Name = "PercentageTotal"
        Me.PercentageTotal.ReadOnly = True
        '
        'LinesofCode
        '
        Me.LinesofCode.HeaderText = "Lines Of Code"
        Me.LinesofCode.Name = "LinesofCode"
        '
        'CommentLines
        '
        Me.CommentLines.HeaderText = "Commented Lines"
        Me.CommentLines.Name = "CommentLines"
        Me.CommentLines.ReadOnly = True
        '
        'Whitespace
        '
        Me.Whitespace.HeaderText = "Whitespace"
        Me.Whitespace.Name = "Whitespace"
        '
        'clmFixme
        '
        Me.clmFixme.HeaderText = "Potentially Unsafe Flags"
        Me.clmFixme.Name = "clmFixme"
        Me.clmFixme.ToolTipText = "occurances of todo, fixme, etc."
        '
        'CodeIssues
        '
        Me.CodeIssues.HeaderText = "Potentially Unsafe Code"
        Me.CodeIssues.Name = "CodeIssues"
        '
        'FullPath
        '
        Me.FullPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.FullPath.HeaderText = "Full Path"
        Me.FullPath.Name = "FullPath"
        Me.FullPath.ReadOnly = True
        Me.FullPath.Width = 68
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenInNotepadToolStripMenuItem, Me.ToolStripMenuItem1, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(230, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(233, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'ExportToolStripMenuItem
        '
        Me.ExportToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExportToClipboardToolStripMenuItem})
        Me.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem"
        Me.ExportToolStripMenuItem.Size = New System.Drawing.Size(52, 20)
        Me.ExportToolStripMenuItem.Text = "Export"
        '
        'ExportToClipboardToolStripMenuItem
        '
        Me.ExportToClipboardToolStripMenuItem.Name = "ExportToClipboardToolStripMenuItem"
        Me.ExportToClipboardToolStripMenuItem.Size = New System.Drawing.Size(228, 22)
        Me.ExportToClipboardToolStripMenuItem.Text = "Copy All Results to Clipboard"
        '
        'mnuBreakdown
        '
        Me.mnuBreakdown.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ExportToolStripMenuItem})
        Me.mnuBreakdown.Location = New System.Drawing.Point(0, 0)
        Me.mnuBreakdown.Name = "mnuBreakdown"
        Me.mnuBreakdown.Size = New System.Drawing.Size(1225, 24)
        Me.mnuBreakdown.TabIndex = 11
        Me.mnuBreakdown.Text = "MenuStrip1"
        '
        'txtFilter
        '
        Me.txtFilter.Location = New System.Drawing.Point(607, 401)
        Me.txtFilter.Name = "txtFilter"
        Me.txtFilter.Size = New System.Drawing.Size(304, 20)
        Me.txtFilter.TabIndex = 12
        Me.txtFilter.Text = "Filter...."
        '
        'lblDoubleClick
        '
        Me.lblDoubleClick.AutoSize = True
        Me.lblDoubleClick.Location = New System.Drawing.Point(602, 372)
        Me.lblDoubleClick.Name = "lblDoubleClick"
        Me.lblDoubleClick.Size = New System.Drawing.Size(334, 13)
        Me.lblDoubleClick.TabIndex = 10
        Me.lblDoubleClick.Text = "Double click on an item below to view an individual code breakdown."
        '
        'lblResults
        '
        Me.lblResults.AutoSize = True
        Me.lblResults.Location = New System.Drawing.Point(14, 13)
        Me.lblResults.Name = "lblResults"
        Me.lblResults.Size = New System.Drawing.Size(39, 13)
        Me.lblResults.TabIndex = 0
        Me.lblResults.Text = "Label1"
        '
        'pnlResults
        '
        Me.pnlResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlResults.Controls.Add(Me.lblResults)
        Me.pnlResults.Location = New System.Drawing.Point(12, 372)
        Me.pnlResults.Name = "pnlResults"
        Me.pnlResults.Size = New System.Drawing.Size(584, 80)
        Me.pnlResults.TabIndex = 9
        '
        'btnApplyFilter
        '
        Me.btnApplyFilter.Location = New System.Drawing.Point(915, 400)
        Me.btnApplyFilter.Name = "btnApplyFilter"
        Me.btnApplyFilter.Size = New System.Drawing.Size(82, 20)
        Me.btnApplyFilter.TabIndex = 13
        Me.btnApplyFilter.Text = "Apply Filter"
        Me.btnApplyFilter.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnApplyFilter.UseVisualStyleBackColor = True
        '
        'chtResults
        '
        Me.chtResults.BackColor = System.Drawing.Color.Transparent
        Me.chtResults.BackImageTransparentColor = System.Drawing.Color.White
        Me.chtResults.BorderlineColor = System.Drawing.Color.Black
        Me.chtResults.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
        ChartArea1.Name = "ChartArea1"
        Me.chtResults.ChartAreas.Add(ChartArea1)
        Me.chtResults.Dock = System.Windows.Forms.DockStyle.Top
        Legend1.Name = "Legend1"
        Me.chtResults.Legends.Add(Legend1)
        Me.chtResults.Location = New System.Drawing.Point(0, 24)
        Me.chtResults.Name = "chtResults"
        Me.chtResults.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Berry
        Series1.ChartArea = "ChartArea1"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie
        Series1.IsValueShownAsLabel = True
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Me.chtResults.Series.Add(Series1)
        Me.chtResults.Size = New System.Drawing.Size(1225, 345)
        Me.chtResults.TabIndex = 14
        Me.chtResults.Text = "Chart1"
        '
        'frmBreakdown
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1225, 921)
        Me.Controls.Add(Me.chtResults)
        Me.Controls.Add(Me.dgvResults)
        Me.Controls.Add(Me.mnuBreakdown)
        Me.Controls.Add(Me.txtFilter)
        Me.Controls.Add(Me.lblDoubleClick)
        Me.Controls.Add(Me.pnlResults)
        Me.Controls.Add(Me.btnApplyFilter)
        Me.Name = "frmBreakdown"
        Me.Text = "Code Breakdown"
        CType(Me.dgvResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnuBreakdown.ResumeLayout(False)
        Me.mnuBreakdown.PerformLayout()
        Me.pnlResults.ResumeLayout(False)
        Me.pnlResults.PerformLayout()
        CType(Me.chtResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OpenInNotepadToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dgvResults As System.Windows.Forms.DataGridView
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportToClipboardToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuBreakdown As System.Windows.Forms.MenuStrip
    Friend WithEvents txtFilter As System.Windows.Forms.TextBox
    Friend WithEvents lblDoubleClick As System.Windows.Forms.Label
    Friend WithEvents lblResults As System.Windows.Forms.Label
    Friend WithEvents pnlResults As System.Windows.Forms.Panel
    Friend WithEvents btnApplyFilter As System.Windows.Forms.Button
    Friend WithEvents chtResults As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents clmName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TotalLines As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PercentageTotal As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LinesofCode As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CommentLines As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Whitespace As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmFixme As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CodeIssues As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FullPath As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
