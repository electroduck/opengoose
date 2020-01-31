Public NotInheritable Class DesktopMobForm
    Private mBackBuffer As New Bitmap(1, 1)

    Public Event PaintBackBuffer(g As Graphics)

    Private Sub UpdateBackBuffer()
        If Size <> mBackBuffer.Size Then
            mBackBuffer = New Bitmap(Width, Height)
        End If
        Using gfx As Graphics = Graphics.FromImage(mBackBuffer)
            gfx.Clear(Color.Fuchsia)
            RaiseEvent PaintBackBuffer(gfx)
        End Using
    End Sub

    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
        'MyBase.OnPaintBackground(e)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        e.Graphics.DrawImage(mBackBuffer, 0, 0, Width, Height)
    End Sub

    Protected Overrides Sub OnInvalidated(e As InvalidateEventArgs)
        UpdateBackBuffer()
        MyBase.OnInvalidated(e)
    End Sub

    Protected Overrides Sub OnResizeEnd(e As EventArgs)
        UpdateBackBuffer()
        MyBase.OnResizeEnd(e)
    End Sub
End Class