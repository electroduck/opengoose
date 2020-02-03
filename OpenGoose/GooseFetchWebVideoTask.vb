<TaskWeight(1000)>
Public Class GooseFetchWebVideoTask
    Inherits GooseFetchWindowTask

    Public Sub New(m As DesktopMob)
        MyBase.New(m)
    End Sub

    Protected Overrides Sub OnBegin()
        Window = New WebpageForm
        DirectCast(Window, WebpageForm).BrowserControl.Navigate(GetMemeEmbedLink())
        MyBase.OnBegin()
    End Sub

    Private Function GetMemeEmbedLink() As String
        Try
            Return DirectCast(Mob, MobGoose).MemeMgr.GetMeme(Of MemeVideo).URL
        Catch ex As Exception
            Debug.WriteLine("Error getting web video: " & ex.ToString)
            Return "about:blank"
        End Try
    End Function

End Class
