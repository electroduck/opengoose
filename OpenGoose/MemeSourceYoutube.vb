Imports Electroduck.OpenGoose

<MemeSource(GetType(MemeVideo))>
Public Class MemeSourceYoutube
    Inherits MemeSource(Of MemeVideo)

    ' <a [^>]*href="\/watch\?v=([^&"]+).*">
    Private Const REGEX_VIDEOID As String = "<a [^>]*href=""\/watch\?v=([^&""]+).*"">"

    Private mURLs As New Queue(Of String)

    Public Overrides Sub Init()
        Dim strPage As String
        Dim strVideoIDCur As String
        Dim strVideoIDLast As String = ""
        Dim regex As New Text.RegularExpressions.Regex(REGEX_VIDEOID)
        Dim matches As Text.RegularExpressions.MatchCollection

        Using www As New Net.WebClient
            strPage = www.DownloadString("https://www.youtube.com/playlist?list=PLv3TTBr1W_9tppikBxAE_G6qjWdBljBHJ")
        End Using

        mURLs.Clear()
        matches = regex.Matches(strPage)
        For Each match As Text.RegularExpressions.Match In matches
            strVideoIDCur = match.Groups.Item(1).Value
            If strVideoIDCur <> strVideoIDLast Then
                mURLs.Enqueue("https://www.youtube.com/embed/" & strVideoIDCur)
                strVideoIDLast = strVideoIDCur
            End If
        Next
    End Sub

    Public Overrides Function GetMeme() As MemeVideo
        If Not CanGetMeme() Then
            Throw New InvalidOperationException("Cannot get Youtube instant-regret meme at this time")
        End If

        Return New MemeVideo(mURLs.Dequeue)
    End Function

    Public Overrides Function CanGetMeme() As Boolean
        Return mURLs.Count > 0
    End Function
End Class
