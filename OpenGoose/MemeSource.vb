Public MustInherit Class MemeSource
    Public MustOverride Sub Init()
    Public MustOverride Function GetMeme() As Meme
    Public MustOverride Function CanGetMeme() As Boolean
End Class
