Public Class Desktop
    Implements IWin32Window

    Public Shared ReadOnly Property MyDesktop As Desktop = New Desktop

    Public ReadOnly Property Handle As IntPtr Implements IWin32Window.Handle
        Get
            Return IntPtr.Zero
        End Get
    End Property

    Public ReadOnly Property ProgramManager As ForeignWindow
        Get
            Return GetWindowByClass("Progman")
        End Get
    End Property

End Class
