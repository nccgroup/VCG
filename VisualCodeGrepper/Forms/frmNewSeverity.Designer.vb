<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNewSeverity
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
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.cboNewLevel = New System.Windows.Forms.ComboBox()
        Me.lblNewLevel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(188, 39)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(107, 38)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'cboNewLevel
        '
        Me.cboNewLevel.FormattingEnabled = True
        Me.cboNewLevel.Items.AddRange(New Object() {"Potentially Unsafe", "Suspicious Comment", "Low", "Standard", "Medium", "High", "Critical"})
        Me.cboNewLevel.Location = New System.Drawing.Point(108, 9)
        Me.cboNewLevel.Name = "cboNewLevel"
        Me.cboNewLevel.Size = New System.Drawing.Size(154, 21)
        Me.cboNewLevel.TabIndex = 8
        '
        'lblNewLevel
        '
        Me.lblNewLevel.AutoSize = True
        Me.lblNewLevel.Location = New System.Drawing.Point(3, 12)
        Me.lblNewLevel.Name = "lblNewLevel"
        Me.lblNewLevel.Size = New System.Drawing.Size(102, 13)
        Me.lblNewLevel.TabIndex = 9
        Me.lblNewLevel.Text = "New Severity Level:"
        '
        'frmNewSeverity
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(272, 69)
        Me.Controls.Add(Me.lblNewLevel)
        Me.Controls.Add(Me.cboNewLevel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmNewSeverity"
        Me.Text = "Set Severity"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents cboNewLevel As System.Windows.Forms.ComboBox
    Friend WithEvents lblNewLevel As System.Windows.Forms.Label
End Class
