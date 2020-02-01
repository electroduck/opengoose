<TaskWeight(20)>
Public Class GooseWanderTask
    Inherits MobTask

    Private mDest As Point

    Public Sub New(mob As DesktopMob)
        MyBase.New(mob)
    End Sub

    Protected Overrides Sub OnBegin()
        Dim rand As New Random
        Dim scrnTarget As Screen

        scrnTarget = Screen.AllScreens(rand.Next(Screen.AllScreens.Length))
        mDest = RandomPointInRect(rand, scrnTarget.WorkingArea)

        Mob.MoveTowards(mDest, 100000.0)
    End Sub

    Protected Overrides Sub OnTick()
        If (Physics.Vector2D.FromPoint(mDest) - Mob.Position).Magnitude < 150 Then
            Mob.StopMoving()
            SetComplete()
        End If
    End Sub

    Protected Overrides Sub OnAbort()
        Mob.StopMoving()
        MyBase.OnAbort()
    End Sub

    Private Function RandomPointInRect(rand As Random, rect As Rectangle) As Point
        Return New Point(rect.X + rand.Next(rect.Width), rect.Y + rand.Next(rect.Height))
    End Function

End Class
