<TaskWeight(10)>
Public Class GooseWanderTask
    Inherits MobTask

    Private mDest As Point

    Public Sub New(mob As DesktopMob)
        MyBase.New(mob)
    End Sub

    Protected Overrides Sub OnBegin()
        Dim rand As New Random
        mDest = New Point(rand.Next(Screen.PrimaryScreen.WorkingArea.Width),
                          rand.Next(Screen.PrimaryScreen.WorkingArea.Height))
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

End Class
