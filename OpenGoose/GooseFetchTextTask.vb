<TaskWeight(10)>
Public Class GooseFetchTextTask
    Inherits GooseFetchWindowTask

    Public Sub New(m As DesktopMob)
        MyBase.New(m)
    End Sub

    Protected Overrides Sub OnBegin()
        Window = New Form
        Window.Controls.Add(New Label With {
                            .Text = "Test",
                            .Dock = DockStyle.Fill
                            })
        MyBase.OnBegin()
    End Sub
End Class
