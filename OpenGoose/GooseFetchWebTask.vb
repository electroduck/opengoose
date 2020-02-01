<TaskWeight(10)>
Public Class GooseFetchWebTask
    Inherits GooseFetchWindowTask

    Public Sub New(m As DesktopMob)
        MyBase.New(m)
    End Sub

    Protected Overrides Sub OnBegin()
        Window = New WebpageForm
        DirectCast(Window, WebpageForm).BrowserControl.Navigate("https://www.amazon.com/s?k=cracked+corn")
        MyBase.OnBegin()
    End Sub

End Class
