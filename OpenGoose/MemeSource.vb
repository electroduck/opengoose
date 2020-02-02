Imports Electroduck.OpenGoose

Public Interface IMemeSource
    Sub Init()
    Function GetMeme() As Meme
    Function CanGetMeme() As Boolean
End Interface

Public MustInherit Class MemeSource(Of T As Meme)
    Implements IMemeSource

    Public MustOverride Sub Init() Implements IMemeSource.Init
    Public MustOverride Function GetMeme() As T
    Public MustOverride Function CanGetMeme() As Boolean Implements IMemeSource.CanGetMeme

    Private Function GetMemeAsBaseType() As Meme Implements IMemeSource.GetMeme
        Return DirectCast(GetMeme(), Meme)
    End Function
End Class
