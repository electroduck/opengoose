Public Class DeviceContext
    Implements IDisposable

    Private mHandle As IntPtr
    Private mGraphics As Graphics

    Public ReadOnly Property Handle As IntPtr
        Get
            Return mHandle
        End Get
    End Property

    Public Sub New(gfx As Graphics)
        mHandle = gfx.GetHdc
        mGraphics = gfx
    End Sub

#Region "IDisposable Support"
    Private mDisposed As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not mDisposed Then
            If disposing Then
                ' Dispose managed state (managed objects).
            End If

            ' Free unmanaged resources (unmanaged objects) and override Finalize() below.
            mGraphics.ReleaseHdc(mHandle)
            ' Set large fields to null.
        End If
        mDisposed = True
    End Sub

    ' Override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' Uncomment the following line if Finalize() is overridden above.
        GC.SuppressFinalize(Me)
    End Sub
#End Region


End Class
