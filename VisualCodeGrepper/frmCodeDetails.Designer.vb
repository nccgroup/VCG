<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCodeDetails
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
        Me.rtbCodeDetails = New System.Windows.Forms.RichTextBox()
        Me.SuspendLayout()
        '
        'rtbCodeDetails
        '
        Me.rtbCodeDetails.BackColor = System.Drawing.SystemColors.Window
        Me.rtbCodeDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbCodeDetails.EnableAutoDragDrop = True
        Me.rtbCodeDetails.Font = New System.Drawing.Font("Century Gothic", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbCodeDetails.ForeColor = System.Drawing.SystemColors.MenuText
        Me.rtbCodeDetails.Location = New System.Drawing.Point(0, 0)
        Me.rtbCodeDetails.Name = "rtbCodeDetails"
        Me.rtbCodeDetails.Size = New System.Drawing.Size(1250, 527)
        Me.rtbCodeDetails.TabIndex = 2
        Me.rtbCodeDetails.Text = ""
        '
        'frmCodeDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1250, 527)
        Me.Controls.Add(Me.rtbCodeDetails)
        Me.Name = "frmCodeDetails"
        Me.Text = "frmCodeDetails"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents rtbCodeDetails As System.Windows.Forms.RichTextBox
End Class
