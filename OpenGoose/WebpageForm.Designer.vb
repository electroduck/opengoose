<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WebpageForm
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
        Me.BrowserControl = New System.Windows.Forms.WebBrowser()
        Me.SuspendLayout()
        '
        'BrowserControl
        '
        Me.BrowserControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BrowserControl.Location = New System.Drawing.Point(0, 0)
        Me.BrowserControl.MinimumSize = New System.Drawing.Size(20, 20)
        Me.BrowserControl.Name = "BrowserControl"
        Me.BrowserControl.ScriptErrorsSuppressed = True
        Me.BrowserControl.Size = New System.Drawing.Size(584, 361)
        Me.BrowserControl.TabIndex = 0
        '
        'WebpageForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 361)
        Me.Controls.Add(Me.BrowserControl)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "WebpageForm"
        Me.Text = "Goose Browser"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents BrowserControl As WebBrowser
End Class
