Public Class MobGoose
    Inherits DesktopMob

    Private mFeetRotation As Double = 0.0

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

    Private Shared ReadOnly mTaskTypes As New List(Of Type) From {
        GetType(GooseWanderTask)
    }

    Protected Overrides ReadOnly Property TaskTypes As IList(Of Type)
        Get
            Return mTaskTypes
        End Get
    End Property

    Protected Overrides Sub OnPaint(gfx As Graphics, s As Size)
        Dim brushGoose As New SolidBrush(Color.White)
        Dim brushGooseBg As New SolidBrush(Color.Gray)
        Dim penBody As New Pen(brushGoose, 24)
        Dim penBodyBg As New Pen(brushGooseBg, 26)
        Dim penHead As New Pen(brushGoose, 16)
        Dim penHeadBg As New Pen(brushGooseBg, 18)
        Dim nCenterX As Integer = s.Width \ 2
        Dim nCenterY As Integer = s.Height \ 2
        Dim nOffsetX As Integer = mPhysObj.mVelocity.UnitVector.X * 12.0
        Dim nOffsetY As Integer = mPhysObj.mVelocity.UnitVector.Y * 12.0
        Dim nBreastX As Integer = nCenterX + nOffsetX
        Dim nBreastY As Integer = nCenterY + nOffsetY
        Dim nButtX As Integer = nCenterX - nOffsetX
        Dim nButtY As Integer = nCenterY - nOffsetY
        Dim nFrontTipX As Integer = nCenterX + mPhysObj.mVelocity.UnitVector.X * 16.0
        Dim nFrontTipY As Integer = nCenterY + mPhysObj.mVelocity.UnitVector.Y * 16.0
        Dim nHeadX As Integer = nFrontTipX
        Dim nHeadY As Integer = nFrontTipY - 16
        Dim bFacingLeft As Boolean = mPhysObj.mVelocity.X < 0
        Dim nBeakBaseX As Integer = nHeadX + If(bFacingLeft, -7, 7)
        Dim nBeakTopY As Integer = nHeadY - 4
        Dim nBeakBottomY As Integer = nHeadY + 4
        Dim nBeakTipX As Integer = nBeakBaseX + If(bFacingLeft, -8, 8)
        Dim nBeakTipY As Integer = nHeadY
        Dim nEyeX As Integer = nBeakBaseX + If(bFacingLeft, 6, -6)
        Dim nEyeY As Integer = nBeakTipY
        Dim nFeetCenterX As Integer = nCenterX
        Dim nFeetCenterY As Integer = nCenterY + 16
        Dim nFootAX As Integer = nFeetCenterX + Math.Cos(mFeetRotation) * 8.0
        Dim nFootAY As Integer = nFeetCenterY + Math.Sin(mFeetRotation) * 4.0
        Dim nFootBX As Integer = nFeetCenterX + Math.Cos(mFeetRotation + Math.PI) * 8.0
        Dim nFootBY As Integer = nFeetCenterY + Math.Sin(mFeetRotation + Math.PI) * 4.0

        Dim ptBeak(2) As Point
        ptBeak(0) = New Point(nBeakBaseX, nBeakTopY)
        ptBeak(1) = New Point(nBeakBaseX, nBeakBottomY)
        ptBeak(2) = New Point(nBeakTipX, nBeakTipY)

        DrawCircle(gfx, Brushes.Orange, nFootAX, nFootAY, 6)
        DrawCircle(gfx, Brushes.Orange, nFootBX, nFootBY, 6)
        DrawRoundedLine(gfx, penBodyBg, nButtX, nButtY, nBreastX, nBreastY)
        DrawRoundedLine(gfx, penHeadBg, nFrontTipX, nFrontTipY, nHeadX, nHeadY)
        DrawRoundedLine(gfx, penBody, nButtX, nButtY, nBreastX, nBreastY)
        DrawRoundedLine(gfx, penHead, nFrontTipX, nFrontTipY, nHeadX, nHeadY)
        gfx.FillPolygon(Brushes.Orange, ptBeak)
        DrawCircle(gfx, Brushes.Black, nEyeX, nEyeY, 4)
    End Sub

    Private Sub DrawRoundedLine(gfx As Graphics, pn As Pen, x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer)
        gfx.DrawLine(pn, x1, y1, x2, y2)
        DrawCircle(gfx, pn.Brush, x1, y1, pn.Width)
        DrawCircle(gfx, pn.Brush, x2, y2, pn.Width)
    End Sub

    Private Sub DrawCircle(gfx As Graphics, br As Brush, x As Integer, y As Integer, d As Integer)
        gfx.FillEllipse(br, x - d \ 2, y - d \ 2, d, d)
    End Sub

    Protected Overrides Sub OnTick()
        mFeetRotation += mPhysObj.mVelocity.Magnitude * 7.0
        If mFeetRotation > (2 * Math.PI) Then
            mFeetRotation -= 2 * Math.PI
        End If
    End Sub
End Class
