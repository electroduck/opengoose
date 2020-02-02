Imports Electroduck.OpenGoose

<MemeSource(GetType(MemeImage))>
Public Class MemeSourceReddit
    Inherits MemeSource(Of MemeImage)

    '<img [^>]*alt="Post image" [^>]*src="[^"]*redd\.it\/([a-zA-Z0-9]+\.[a-z]{0,4})[^"]*"[^>]*>
    Private Const REGEX_IMAGE As String = "<img [^>]*alt=""Post image"" [^>]*src=""[^""]*redd\.it\/([a-zA-Z0-9]+\.[a-z]{0,4})[^""]*""[^>]*>"

    Private mURLs As New Queue(Of String)

    Public Overrides Sub Init()
        Dim strPage As String
        Dim strImageName As String
        Dim regex As New Text.RegularExpressions.Regex(REGEX_IMAGE)
        Dim matches As Text.RegularExpressions.MatchCollection

        Using www As New Net.WebClient
            strPage = www.DownloadString("https://www.reddit.com/r/dankmemes/hot/")
        End Using

        mURLs.Clear()
        matches = regex.Matches(strPage)
        For Each match As Text.RegularExpressions.Match In matches
            strImageName = match.Groups.Item(1).Value
            mURLs.Enqueue("https://i.redd.it/" & strImageName)
        Next
    End Sub

    Public Overrides Function GetMeme() As MemeImage
        If Not CanGetMeme() Then
            Throw New InvalidOperationException("Cannot get Reddit meme at this time")
        End If

        Return New MemeImage(mURLs.Dequeue)
    End Function

    Public Overrides Function CanGetMeme() As Boolean
        Return mURLs.Count > 0
    End Function
End Class
