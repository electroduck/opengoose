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

    Public Function DownloadToTempFile() As String
        Dim strTempFile As String = My.Computer.FileSystem.GetTempFileName
        Dim www As New Net.WebClient
        www.DownloadFile(URL, strTempFile)
        Return strTempFile
    End Function

    Public Function DownloadToMemory() As Byte()
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