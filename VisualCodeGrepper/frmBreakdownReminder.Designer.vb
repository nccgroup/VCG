<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBreakdownReminder
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
        Me.chkNotAgain = New System.Windows.Forms.CheckBox()
        Me.lblReminder = New System.Windows.Forms.Label()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.chkAlwaysShow = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'chkNotAgain
        '
        Me.chkNotAgain.AutoSize = True
        Me.chkNotAgain.Location = New System.Drawing.Point(5, 89)
        Me.chkNotAgain.Name = "chkNotAgain"
        Me.chkNotAgain.Size = New System.Drawing.Size(180, 17)
        Me.chkNotAgain.TabIndex = 5
        Me.chkNotAgain.Text = "Do not show this reminder again."
        Me.chkNotAgain.UseVisualStyleBackColor = True
        '
        'lblReminder
        '
        Me.lblReminder.Location = New System.Drawing.Point(2, 9)
        Me.lblReminder.Name = "lblReminder"
        Me.lblReminder.Size = New System.Drawing.Size(367, 40)
        Me.lblReminder.TabIndex = 4
        Me.lblReminder.Text = "To see a visual breakdown of the LOC, number of comments, number of potential iss" & _
            "ues, etc. select 'Visual Code/Comment Breakdown' from the 'Scan' menu or or clic" & _
            "k the 'Always display' option below."
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(287, 111)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'chkAlwaysShow
        '
        Me.chkAlwaysShow.AutoSize = True
        Me.chkAlwaysShow.Location = New System.Drawing.Point(5, 66)
        Me.chkAlwaysShow.Name = "chkAlwaysShow"
        Me.chkAlwaysShow.Size = New System.Drawing.Size(264, 17)
        Me.chkAlwaysShow.TabIndex = 6
        Me.chkAlwaysShow.Text = "Always display Visual Breakdown after every scan."
        Me.chkAlwaysShow.UseVisualStyleBackColor = True
        '
        'frmBreakdownReminder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(367, 140)
        Me.ControlBox = False
        Me.Controls.Add(Me.chkAlwaysShow)
        Me.Controls.Add(Me.chkNotAgain)
        Me.Controls.Add(Me.lblReminder)
        Me.Controls.Add(Me.btnOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "frmBreakdownReminder"
        Me.Text = "Visual Breakdown"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkNotAgain As System.Windows.Forms.CheckBox
    Friend WithEvents lblReminder As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents chkAlwaysShow As System.Windows.Forms.CheckBox
End Class
