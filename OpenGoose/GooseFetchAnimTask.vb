<TaskWeight(10)>
Public Class GooseFetchAnimTask
    Inherits GooseFetchWindowTask

    Public Sub New(m As DesktopMob)
        MyBase.New(m)
    End Sub

    Protected Overrides Sub OnBegin()
        Window = New ImageForm
        DirectCast(Window, ImageForm).ImageBox.ImageLocation = GetMemeFilePath()
        MyBase.OnBegin()
    End Sub

    Private Function GetMemeFilePath() As String
        Try
            Return DirectCast(Mob, MobGoose).MemeMgr.GetMeme(Of MemeAnimation).FilePath
        Catch ex As Exception
            Debug.WriteLine("Error getting animation: " & ex.ToString)
            Return ""
        End Try
    End Function
End Class
