Imports Electroduck.OpenGoose

Public Class MemeSourceIFunny
    Inherits MemeSource

    Private mURLs As New Queue(Of String)

    Public Overrides Sub Init()
        Dim strPage As String
        Dim strURL As String
        Dim regex As New Text.RegularExpressions.Regex("<img[^>]* class=""media__image[^""]*""[^>]* data-src=""([^""]+)""[^>]*>")
        Dim matches As Text.RegularExpressions.MatchCollection

        Using www As New Net.WebClient
            strPage = www.DownloadString("https://ifunny.co")
        End Using

        mURLs.Clear()
        matches = regex.Matches(strPage)
        For Each match As Text.RegularExpressions.Match In matches
            strURL = match.Groups.Item(1).Value
            If strURL.EndsWith("jpg") Or strURL.EndsWith("png") Or strURL.EndsWith("gif") Then
                mURLs.Enqueue(strURL)
            End If
        Next
    End Sub

    Public Overrides Function GetMeme() As Meme
        If Not CanGetMeme() Then
            Throw New InvalidOperationException("Cannot get iFunny meme at this time")
        End If

        Return New MemeImage(mURLs.Dequeue)
    End Function

    Public Overrides Function CanGetMeme() As Boolean
        Return mURLs.Count > 0
    End Function
End Class
