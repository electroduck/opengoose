Public NotInheritable Class DesktopMobForm
    Private Enum TranspVersion
        Win2000
        WinXP
        WinVista
    End Enum

    Private mBackBuffer As New Bitmap(1, 1)
    Private mTranspVersion As TranspVersion = TranspVersion.Win2000

    Public Event PaintBackBuffer(g As Graphics)

    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            Select Case mTranspVersion
                Case TranspVersion.WinVista
                    cp.ExStyle = cp.ExStyle Or &H80000 ' newer transparency (better perf.)
                    ' Note: XP and 2000 "support" the new transparency method but it is not reliable

                Case TranspVersion.WinXP
                    cp.Style = cp.ExStyle Or &HC00080 ' no transparency

                Case TranspVersion.Win2000
                    cp.Style = cp.ExStyle Or &HC00080 ' no transparency
            End Select
            Return cp
        End Get
    End Property

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        ' Get transparency support
#If DEBUG Then
        mTranspVersion = TranspVersion.WinXP
#Else
        If Environment.OSVersion.Version.Major >= 6 Then
            mTranspVersion = TranspVersion.WinVista
        ElseIf Environment.OSVersion.Version.Major = 5 And Environment.OSVersion.Version.Minor >= 1 Then
            mTranspVersion = TranspVersion.WinXP
        End If
#End If
    End Sub

    Private Sub UpdateBackBuffer()
        If Size <> mBackBuffer.Size Then
            mBackBuffer = New Bitmap(Width, Height)
        End If
        Using gfx As Graphics = Graphics.FromImage(mBackBuffer)
            Select Case mTranspVersion
                Case TranspVersion.WinXP
                    DrawDesktopBackground(gfx, Me)
                    'gfx.Clear(SystemColors.Control)

                Case TranspVersion.Win2000
                    DrawDesktopBackground(gfx, Me)
                    'gfx.Clear(SystemColors.Control)

                Case TranspVersion.WinVista
                    gfx.Clear(Color.Fuchsia)
            End Select
            RaiseEvent PaintBackBuffer(gfx)
        End Using
    End Sub

    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
        'MyBase.OnPaintBackground(e)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        e.Graphics.DrawImage(mBackBuffer, 0, 0, Width, Height)

#If 0 Then
        If mTranspVersion = TranspVersion.WinXP Then
            Using g As Graphics = Graphics.FromHwnd(IntPtr.Zero)
                g.DrawImage(mBackBuffer, Location.X, Location.Y, Width, Height)
            End Using
        End If
#End If
    End Sub

    Protected Overrides Sub OnInvalidated(e As InvalidateEventArgs)
        UpdateBackBuffer()
        MyBase.OnInvalidated(e)
    End Sub

    Protected Overrides Sub OnResizeEnd(e As EventArgs)
        UpdateBackBuffer()
        MyBase.OnResizeEnd(e)
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        Select Case mTranspVersion
            Case TranspVersion.WinVista
                SetColorKey(Me, Color.Fuchsia)
        End Select
    End Sub
End Class