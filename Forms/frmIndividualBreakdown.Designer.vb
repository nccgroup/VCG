<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmIndividualBreakdown
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
        Me.chtResults = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.CopyCommentsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlMethods = New System.Windows.Forms.Panel()
        Me.lblBreakdown = New System.Windows.Forms.TextBox()
        Me.lblMethods = New System.Windows.Forms.TextBox()
        Me.CopyUnsafeMethodsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblResults = New System.Windows.Forms.Label()
        Me.mnuMenuStrip = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenWithNotepadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlResults = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.TextBox()
        CType(Me.chtResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlMethods.SuspendLayout()
        Me.mnuMenuStrip.SuspendLayout()
        Me.pnlResults.SuspendLayout()
        Me.SuspendLayout()
        '
        'chtResults
        '
        Me.chtResults.BackColor = System.Drawing.Color.Transparent
        Me.chtResults.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chtResults.BackImageTransparentColor = System.Drawing.Color.Transparent
        Me.chtResults.BorderlineColor = System.Drawing.Color.Black
        Me.chtResults.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
        ChartArea1.Name = "ChartArea1"
        Me.chtResults.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.chtResults.Legends.Add(Legend1)
        Me.chtResults.Location = New System.Drawing.Point(0, 26)
        Me.chtResults.Name = "chtResults"
        Me.chtResults.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Berry
        Series1.ChartArea = "ChartArea1"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie
        Series1.IsValueShownAsLabel = True
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Me.chtResults.Series.Add(Series1)
        Me.chtResults.Size = New System.Drawing.Size(880, 299)
        Me.chtResults.TabIndex = 6
        Me.chtResults.Text = "Chart1"
        '
        'CopyCommentsToolStripMenuItem
        '
        Me.CopyCommentsToolStripMenuItem.Name = "CopyCommentsToolStripMenuItem"
        Me.CopyCommentsToolStripMenuItem.Size = New System.Drawing.Size(236, 22)
        Me.CopyCommentsToolStripMenuItem.Text = "Copy Comments to Clipboard"
        '
        'pnlMethods
        '
        Me.pnlMethods.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.pnlMethods.Controls.Add(Me.lblBreakdown)
        Me.pnlMethods.Controls.Add(Me.lblMethods)
        Me.pnlMethods.Location = New System.Drawing.Point(648, 138)
        Me.pnlMethods.Name = "pnlMethods"
        Me.pnlMethods.Size = New System.Drawing.Size(232, 170)
        Me.pnlMethods.TabIndex = 5
        Me.pnlMethods.Visible = False
        '
        'lblBreakdown
        '
        Me.lblBreakdown.Location = New System.Drawing.Point(65, 23)
        Me.lblBreakdown.Multiline = True
        Me.lblBreakdown.Name = "lblBreakdown"
        Me.lblBreakdown.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.lblBreakdown.Size = New System.Drawing.Size(226, 164)
        Me.lblBreakdown.TabIndex = 9
        '
        'lblMethods
        '
        Me.lblMethods.Location = New System.Drawing.Point(3, 3)
        Me.lblMethods.Multiline = True
        Me.lblMethods.Name = "lblMethods"
        Me.lblMethods.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.lblMethods.Size = New System.Drawing.Size(226, 164)
        Me.lblMethods.TabIndex = 2
        '
        'CopyUnsafeMethodsToolStripMenuItem
        '
        Me.CopyUnsafeMethodsToolStripMenuItem.Name = "CopyUnsafeMethodsToolStripMenuItem"
        Me.CopyUnsafeMethodsToolStripMenuItem.Size = New System.Drawing.Size(236, 22)
        Me.CopyUnsafeMethodsToolStripMenuItem.Text = "Copy Code Issues to Clipboard"
        '
        'ExportToolStripMenuItem
        '
        Me.ExportToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyUnsafeMethodsToolStripMenuItem, Me.CopyCommentsToolStripMenuItem})
        Me.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem"
        Me.ExportToolStripMenuItem.Size = New System.Drawing.Size(52, 20)
        Me.ExportToolStripMenuItem.Text = "Export"
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
        'mnuMenuStrip
        '
        Me.mnuMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ExportToolStripMenuItem})
        Me.mnuMenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.mnuMenuStrip.Name = "mnuMenuStrip"
        Me.mnuMenuStrip.Size = New System.Drawing.Size(939, 24)
        Me.mnuMenuStrip.TabIndex = 8
        Me.mnuMenuStrip.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenWithNotepadToolStripMenuItem, Me.ToolStripMenuItem1, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'OpenWithNotepadToolStripMenuItem
        '
        Me.OpenWithNotepadToolStripMenuItem.Name = "OpenWithNotepadToolStripMenuItem"
        Me.OpenWithNotepadToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.OpenWithNotepadToolStripMenuItem.Text = "Open With Notepad..."
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(186, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'pnlResults
        '
        Me.pnlResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlResults.Controls.Add(Me.lblResults)
        Me.pnlResults.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlResults.Location = New System.Drawing.Point(0, 343)
        Me.pnlResults.Name = "pnlResults"
        Me.pnlResults.Size = New System.Drawing.Size(939, 127)
        Me.pnlResults.TabIndex = 7
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(3, 3)
        Me.Label2.Multiline = True
        Me.Label2.Name = "Label2"
        Me.Label2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.Label2.Size = New System.Drawing.Size(226, 164)
        Me.Label2.TabIndex = 2
        '
        'frmIndividualBreakdown
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(939, 470)
        Me.Controls.Add(Me.chtResults)
        Me.Controls.Add(Me.pnlMethods)
        Me.Controls.Add(Me.mnuMenuStrip)
        Me.Controls.Add(Me.pnlResults)
        Me.Name = "frmIndividualBreakdown"
        Me.Text = "Individual Breakdown"
        CType(Me.chtResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlMethods.ResumeLayout(False)
        Me.pnlMethods.PerformLayout()
        Me.mnuMenuStrip.ResumeLayout(False)
        Me.mnuMenuStrip.PerformLayout()
        Me.pnlResults.ResumeLayout(False)
        Me.pnlResults.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chtResults As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents CopyCommentsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlMethods As System.Windows.Forms.Panel
    Friend WithEvents lblMethods As System.Windows.Forms.TextBox
    Friend WithEvents CopyUnsafeMethodsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblResults As System.Windows.Forms.Label
    Friend WithEvents mnuMenuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenWithNotepadToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlResults As System.Windows.Forms.Panel
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label2 As System.Windows.Forms.TextBox
    Friend WithEvents lblBreakdown As System.Windows.Forms.TextBox
End Class
