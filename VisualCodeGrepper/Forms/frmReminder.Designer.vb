<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReminder
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
        Me.lblReminder = New System.Windows.Forms.Label()
        Me.chkNotAgain = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(260, 72)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'lblReminder
        '
        Me.lblReminder.Location = New System.Drawing.Point(5, 9)
        Me.lblReminder.Name = "lblReminder"
        Me.lblReminder.Size = New System.Drawing.Size(330, 40)
        Me.lblReminder.TabIndex = 1
        Me.lblReminder.Text = "Remember to select the appropriate language from the 'Settings' menu before choos" & _
    "ing a direcory."
        '
        'chkNotAgain
        '
        Me.chkNotAgain.AutoSize = True
        Me.chkNotAgain.Location = New System.Drawing.Point(8, 53)
        Me.chkNotAgain.Name = "chkNotAgain"
        Me.chkNotAgain.Size = New System.Drawing.Size(180, 17)
        Me.chkNotAgain.TabIndex = 2
        Me.chkNotAgain.Text = "Do not show this reminder again."
        Me.chkNotAgain.UseVisualStyleBackColor = True
        '
        'frmReminder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(345, 101)
        Me.ControlBox = False
        Me.Controls.Add(Me.chkNotAgain)
        Me.Controls.Add(Me.lblReminder)
        Me.Controls.Add(Me.btnOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "frmReminder"
        Me.Text = "Select Language"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents lblReminder As System.Windows.Forms.Label
    Friend WithEvents chkNotAgain As System.Windows.Forms.CheckBox
End Class
