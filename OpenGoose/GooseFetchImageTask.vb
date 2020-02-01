<TaskWeight(10)>
Public Class GooseFetchImageTask
    Inherits GooseFetchWindowTask

    Public Sub New(m As DesktopMob)
        MyBase.New(m)
    End Sub

    Protected Overrides Sub OnBegin()
        Window = New ImageForm
        DirectCast(Window, ImageForm).ImageBox.Image = CreatePlaceholderMeme()
        MyBase.OnBegin()
    End Sub

    Private Function CreatePlaceholderMeme() As Bitmap
        Dim bmMeme As New Bitmap(320, 320)
        Dim brushBg As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.DiagonalBrick, Color.Black, Color.Purple)
        Dim fontText As New Font("Impact", 24.0F, FontStyle.Bold)

        Using gfx As Graphics = Graphics.FromImage(bmMeme)
            gfx.FillRectangle(brushBg, 0, 0, bmMeme.Width, bmMeme.Height)
            gfx.DrawString("SAMPLE TEXT", fontText, Brushes.White, 0.0F, 0.0F)
        End Using

        Return bmMeme
    End Function
End Class
