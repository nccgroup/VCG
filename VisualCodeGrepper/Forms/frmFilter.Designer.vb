<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFilter
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
        Me.gbFilter = New System.Windows.Forms.GroupBox()
        Me.cboMaximum = New System.Windows.Forms.ComboBox()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.rbRange = New System.Windows.Forms.RadioButton()
        Me.cboMinimum = New System.Windows.Forms.ComboBox()
        Me.rbBelow = New System.Windows.Forms.RadioButton()
        Me.cboBelow = New System.Windows.Forms.ComboBox()
        Me.rbAbove = New System.Windows.Forms.RadioButton()
        Me.cboAbove = New System.Windows.Forms.ComboBox()
        Me.gbExport = New System.Windows.Forms.GroupBox()
        Me.cbExport = New System.Windows.Forms.CheckBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.gbFilter.SuspendLayout()
        Me.gbExport.SuspendLayout()
        Me.SuspendLayout()
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
        Me.gbFilter.Location = New System.Drawing.Point(7, 4)
        Me.gbFilter.Name = "gbFilter"
        Me.gbFilter.Size = New System.Drawing.Size(551, 113)
        Me.gbFilter.TabIndex = 0
        Me.gbFilter.TabStop = False
        Me.gbFilter.Text = "Filter Options"
        '
        'cboMaximum
        '
        Me.cboMaximum.Enabled = False
        Me.cboMaximum.FormattingEnabled = True
        Me.cboMaximum.Items.AddRange(New Object() {"Potentially Unsafe", "Suspicious Comment", "Low", "Standard", "Medium", "High", "Critical"})
        Me.cboMaximum.Location = New System.Drawing.Point(385, 73)
        Me.cboMaximum.Name = "cboMaximum"
        Me.cboMaximum.Size = New System.Drawing.Size(157, 21)
        Me.cboMaximum.TabIndex = 10
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(360, 76)
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
        Me.cboMinimum.Location = New System.Drawing.Point(196, 73)
        Me.cboMinimum.Name = "cboMinimum"
        Me.cboMinimum.Size = New System.Drawing.Size(157, 21)
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
        Me.cboBelow.Location = New System.Drawing.Point(196, 46)
        Me.cboBelow.Name = "cboBelow"
        Me.cboBelow.Size = New System.Drawing.Size(157, 21)
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
        Me.cboAbove.Location = New System.Drawing.Point(196, 19)
        Me.cboAbove.Name = "cboAbove"
        Me.cboAbove.Size = New System.Drawing.Size(157, 21)
        Me.cboAbove.TabIndex = 3
        '
        'gbExport
        '
        Me.gbExport.Controls.Add(Me.cbExport)
        Me.gbExport.Location = New System.Drawing.Point(7, 124)
        Me.gbExport.Name = "gbExport"
        Me.gbExport.Size = New System.Drawing.Size(551, 44)
        Me.gbExport.TabIndex = 1
        Me.gbExport.TabStop = False
        Me.gbExport.Text = "Export"
        '
        'cbExport
        '
        Me.cbExport.AutoSize = True
        Me.cbExport.Location = New System.Drawing.Point(6, 19)
        Me.cbExport.Name = "cbExport"
        Me.cbExport.Size = New System.Drawing.Size(158, 17)
        Me.cbExport.TabIndex = 0
        Me.cbExport.Text = "Export Results After Filtering"
        Me.cbExport.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(483, 174)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(402, 174)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmFilter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(565, 203)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.gbExport)
        Me.Controls.Add(Me.gbFilter)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmFilter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Results Filter"
        Me.gbFilter.ResumeLayout(False)
        Me.gbFilter.PerformLayout()
        Me.gbExport.ResumeLayout(False)
        Me.gbExport.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbFilter As System.Windows.Forms.GroupBox
    Friend WithEvents cboAbove As System.Windows.Forms.ComboBox
    Friend WithEvents rbBelow As System.Windows.Forms.RadioButton
    Friend WithEvents cboBelow As System.Windows.Forms.ComboBox
    Friend WithEvents rbAbove As System.Windows.Forms.RadioButton
    Friend WithEvents rbRange As System.Windows.Forms.RadioButton
    Friend WithEvents cboMinimum As System.Windows.Forms.ComboBox
    Friend WithEvents cboMaximum As System.Windows.Forms.ComboBox
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents gbExport As System.Windows.Forms.GroupBox
    Friend WithEvents cbExport As System.Windows.Forms.CheckBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
