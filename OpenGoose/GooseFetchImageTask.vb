<TaskWeight(1000)>
Public Class GooseFetchImageTask
    Inherits GooseFetchWindowTask

    Public Sub New(m As DesktopMob)
        MyBase.New(m)
    End Sub

    Protected Overrides Sub OnBegin()
        Window = New ImageForm
        DirectCast(Window, ImageForm).ImageBox.Image = GetMeme()
        MyBase.OnBegin()
    End Sub

    Private Function GetMeme() As Bitmap
        Try
            Return DirectCast(Mob, MobGoose).MemeMgr.GetMeme(Of MemeImage).DrawingImage
        Catch ex As Exception
            Debug.WriteLine("Error getting meme: " & ex.ToString)
            Return CreatePlaceholderMeme()
        End Try
    End Function

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
