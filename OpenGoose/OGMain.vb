Public Class OGMain
    Public Shared Sub Main()
        Dim frmMob As DesktopMob = New MobGoose
        frmMob.Show()

        Do
            frmMob.MoveTowards(GetMousePosition)
            frmMob.Tick()
            Application.DoEvents()
            Threading.Thread.Sleep(10)
        Loop
    End Sub
End Class
