Public Class MobGoose
    Inherits DesktopMob

    Public Overrides ReadOnly Property Mass As Double
        Get
            Return 10.0
        End Get
    End Property

    Public Overrides ReadOnly Property Diameter As Double
        Get
            Return 80.0
        End Get
    End Property

    Protected Overrides Sub OnPaint(gfx As Graphics, s As Size)
        Dim penBody As New Pen(Color.White, 32)
        Dim penBodyBg As New Pen(Color.Gray, 34)
        Dim nCenterX As Integer = s.Width \ 2
        Dim nCenterY As Integer = s.Height \ 2
        Dim nOffsetX As Integer = mPhysObj.mVelocity.UnitVector.X * 12.0
        Dim nOffsetY As Integer = mPhysObj.mVelocity.UnitVector.Y * 12.0
        Dim nHeadX As Integer = nCenterX + nOffsetX
        Dim nHeadY As Integer = nCenterY + nOffsetY - 24

        DrawRoundedLine(gfx, penBodyBg, nCenterX - nOffsetX, nCenterY - nOffsetY, nCenterX + nOffsetX, nCenterY + nOffsetY)
        DrawRoundedLine(gfx, penBody, nCenterX - nOffsetX, nCenterY - nOffsetY, nCenterX + nOffsetX, nCenterY + nOffsetY)
        DrawCircle(gfx, penBodyBg.Brush, nHeadX, nHeadY, 18)
        DrawCircle(gfx, penBody.Brush, nHeadX, nHeadY, 16)
    End Sub

    Private Sub DrawRoundedLine(gfx As Graphics, pn As Pen, x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer)
        gfx.DrawLine(pn, x1, y1, x2, y2)
        DrawCircle(gfx, pn.Brush, x1, y1, pn.Width)
        DrawCircle(gfx, pn.Brush, x2, y2, pn.Width)
    End Sub

    Private Sub DrawCircle(gfx As Graphics, br As Brush, x As Integer, y As Integer, d As Integer)
        gfx.FillEllipse(br, x - d \ 2, y - d \ 2, d, d)
    End Sub
End Class
