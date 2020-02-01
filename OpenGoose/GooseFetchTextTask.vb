<TaskWeight(10)>
Public Class GooseFetchTextTask
    Inherits GooseFetchWindowTask

    Public Sub New(m As DesktopMob)
        MyBase.New(m)
    End Sub

    Protected Overrides Sub OnBegin()
        Window = New TextBoxForm
        DirectCast(Window, TextBoxForm).MessageTextBox.Text = "H o N c"
        DirectCast(Window, TextBoxForm).MessageTextBox.SelectAll()
        DirectCast(Window, TextBoxForm).MessageTextBox.SelectionFont = New Font("Comic Sans MS", 14.0F, FontStyle.Bold)
        DirectCast(Window, TextBoxForm).MessageTextBox.DeselectAll()
        MyBase.OnBegin()
    End Sub
End Class
