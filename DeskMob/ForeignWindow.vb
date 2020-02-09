Public Class ForeignWindow
    Implements IWin32Window

    Private mHandle As IntPtr = IntPtr.Zero

    Public ReadOnly Property Handle As IntPtr Implements IWin32Window.Handle
        Get
            Return mHandle
        End Get
    End Property

    Public ReadOnly Property Size As Drawing.Size
        Get
            Return GetWindowSize(Me)
        End Get
    End Property

    Public ReadOnly Property Location As Point
        Get
            Return GetWindowLocation(Me)
        End Get
    End Property

    Public ReadOnly Property ScreenRectangle As Rectangle
        Get
            Return GetWindowRectangle(Me)
        End Get
    End Property

    Public ReadOnly Property ClientAreaRectangle As Rectangle
        Get
            Return GetClientAreaRectangle(Me)
        End Get
    End Property

    Public Sub New(hWnd As IntPtr)
        If hWnd = IntPtr.Zero Then
            Throw New ArgumentNullException("hWnd")
        End If

        mHandle = hWnd
    End Sub

    Public Function DrawToBuffer() As Bitmap
        Return DrawWindow(Me)
    End Function

End Class
