<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSort
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
        Me.lblSort = New System.Windows.Forms.Label()
        Me.cboPrimary = New System.Windows.Forms.ComboBox()
        Me.lblPrimary = New System.Windows.Forms.Label()
        Me.lblSecondary = New System.Windows.Forms.Label()
        Me.cboSecondary = New System.Windows.Forms.ComboBox()
        Me.lblTertiary = New System.Windows.Forms.Label()
        Me.cboTertiary = New System.Windows.Forms.ComboBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblSort
        '
        Me.lblSort.AutoSize = True
        Me.lblSort.Location = New System.Drawing.Point(3, 3)
        Me.lblSort.Name = "lblSort"
        Me.lblSort.Size = New System.Drawing.Size(141, 13)
        Me.lblSort.TabIndex = 0
        Me.lblSort.Text = "Choose columns to sort on..."
        '
        'cboPrimary
        '
        Me.cboPrimary.FormattingEnabled = True
        Me.cboPrimary.Items.AddRange(New Object() {"Severity", "Title", "Description", "Filename"})
        Me.cboPrimary.Location = New System.Drawing.Point(103, 29)
        Me.cboPrimary.Name = "cboPrimary"
        Me.cboPrimary.Size = New System.Drawing.Size(169, 21)
        Me.cboPrimary.TabIndex = 1
        Me.cboPrimary.Text = "Choose..."
        '
        'lblPrimary
        '
        Me.lblPrimary.AutoSize = True
        Me.lblPrimary.Location = New System.Drawing.Point(6, 32)
        Me.lblPrimary.Name = "lblPrimary"
        Me.lblPrimary.Size = New System.Drawing.Size(66, 13)
        Me.lblPrimary.TabIndex = 2
        Me.lblPrimary.Text = "Primary Sort:"
        '
        'lblSecondary
        '
        Me.lblSecondary.AutoSize = True
        Me.lblSecondary.Location = New System.Drawing.Point(6, 61)
        Me.lblSecondary.Name = "lblSecondary"
        Me.lblSecondary.Size = New System.Drawing.Size(83, 13)
        Me.lblSecondary.TabIndex = 4
        Me.lblSecondary.Text = "Secondary Sort:"
        '
        'cboSecondary
        '
        Me.cboSecondary.FormattingEnabled = True
        Me.cboSecondary.Items.AddRange(New Object() {"Severity", "Title", "Description", "Filename"})
        Me.cboSecondary.Location = New System.Drawing.Point(103, 58)
        Me.cboSecondary.Name = "cboSecondary"
        Me.cboSecondary.Size = New System.Drawing.Size(169, 21)
        Me.cboSecondary.TabIndex = 3
        Me.cboSecondary.Text = "Choose..."
        '
        'lblTertiary
        '
        Me.lblTertiary.AutoSize = True
        Me.lblTertiary.Location = New System.Drawing.Point(6, 88)
        Me.lblTertiary.Name = "lblTertiary"
        Me.lblTertiary.Size = New System.Drawing.Size(67, 13)
        Me.lblTertiary.TabIndex = 6
        Me.lblTertiary.Text = "Tertiary Sort:"
        '
        'cboTertiary
        '
        Me.cboTertiary.FormattingEnabled = True
        Me.cboTertiary.Items.AddRange(New Object() {"Severity", "Title", "Description", "Filename"})
        Me.cboTertiary.Location = New System.Drawing.Point(103, 85)
        Me.cboTertiary.Name = "cboTertiary"
        Me.cboTertiary.Size = New System.Drawing.Size(169, 21)
        Me.cboTertiary.TabIndex = 5
        Me.cboTertiary.Text = "Choose..."
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(196, 115)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 7
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(115, 115)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmSort
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(279, 147)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lblTertiary)
        Me.Controls.Add(Me.cboTertiary)
        Me.Controls.Add(Me.lblSecondary)
        Me.Controls.Add(Me.cboSecondary)
        Me.Controls.Add(Me.lblPrimary)
        Me.Controls.Add(Me.cboPrimary)
        Me.Controls.Add(Me.lblSort)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSort"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Sort"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblSort As System.Windows.Forms.Label
    Friend WithEvents cboPrimary As System.Windows.Forms.ComboBox
    Friend WithEvents lblPrimary As System.Windows.Forms.Label
    Friend WithEvents lblSecondary As System.Windows.Forms.Label
    Friend WithEvents cboSecondary As System.Windows.Forms.ComboBox
    Friend WithEvents lblTertiary As System.Windows.Forms.Label
    Friend WithEvents cboTertiary As System.Windows.Forms.ComboBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
