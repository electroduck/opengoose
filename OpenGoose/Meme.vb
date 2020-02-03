Public MustInherit Class Meme
    Private mURL As String
    Public ReadOnly Property URL As String
        Get
            Return mURL
        End Get
    End Property

    Public Sub New(strURL As String)
        mURL = strURL
    End Sub

    Public Overridable Function DownloadToTempFile() As String
        Dim strTempFile As String = My.Computer.FileSystem.GetTempFileName
        Dim www As New Net.WebClient
        www.DownloadFile(URL, strTempFile)
        Return strTempFile
    End Function

    Public Overridable Function DownloadToMemory() As Byte()
        Dim www As New Net.WebClient
        Return www.DownloadData(URL)
    End Function
End Class

Public Class MemeImage
    Inherits Meme

    Private mDrawingImage As Image

    Public Sub New(strURL As String)
        MyBase.New(strURL)
    End Sub

    Public ReadOnly Property DrawingImage As Image
        Get
            If mDrawingImage Is Nothing Then
                Using stmData As New IO.MemoryStream(DownloadToMemory())
                    mDrawingImage = New Bitmap(stmData)
                End Using
            End If
            Return mDrawingImage
        End Get
    End Property
End Class

Public Class MemeAnimation
    Inherits Meme

    Private mFilePath As String
    Private mReferrer As String

    Private Const USERAGENT_CHROME As String = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36"

    Public Sub New(strURL As String, Optional strReferrer As String = "https://gfycat.com/popular-gifs")
        MyBase.New(strURL)
        mReferrer = strReferrer
    End Sub

    Public Overrides Function DownloadToTempFile() As String
        Dim strTempFile As String = My.Computer.FileSystem.GetTempFileName
        Dim www As New Net.WebClient
        www.Headers.Add(Net.HttpRequestHeader.Referer, mReferrer)
        www.Headers.Add(Net.HttpRequestHeader.UserAgent, USERAGENT_CHROME)
        www.DownloadFile(URL, strTempFile)
        Return strTempFile
    End Function

    Public Overrides Function DownloadToMemory() As Byte()
        Dim www As New Net.WebClient
        www.Headers.Add(Net.HttpRequestHeader.Referer, mReferrer)
        www.Headers.Add(Net.HttpRequestHeader.UserAgent, USERAGENT_CHROME)
        Return www.DownloadData(URL)
    End Function

    Public ReadOnly Property FilePath As String
        Get
            If mFilePath Is Nothing Then
                mFilePath = DownloadToTempFile()
            End If
            Return mFilePath
        End Get
    End Property
End Class

Public Class MemeVideo
    Inherits Meme

    Public Sub New(strURL As String)
        MyBase.New(strURL)
    End Sub

    Public Overrides Function DownloadToMemory() As Byte()
        Throw New NotSupportedException("Downloading videos is not supported")
    End Function

    Public Overrides Function DownloadToTempFile() As String
        Throw New NotSupportedException("Downloading videos is not supported")
    End Function
End Class
