Public Class OGMain
    Public Shared Sub Main()
        Dim frmMob As New DesktopMob
        frmMob.Mass = 100.0
        frmMob.Show()

        Do
            frmMob.MoveTowards(GetMousePosition)
            frmMob.Tick()
            Application.DoEvents()
            Threading.Thread.Sleep(10)
        Loop
    End Sub
End Class
