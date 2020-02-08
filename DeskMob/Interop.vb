Imports System.ComponentModel
Imports System.Runtime.InteropServices

Module Interop
    <StructLayout(LayoutKind.Sequential)>
    Private Structure Win32Point
        Public X As Integer
        Public Y As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Private Structure Win32Rect
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure

    Private Declare Ansi Function GetCursorPos Lib "user32.dll" (ByRef lpPoint As Win32Point) As Boolean

    Public Function GetMousePosition() As Point
        Dim ptPos As Win32Point

        If Not GetCursorPos(ptPos) Then
            Throw New Win32Exception(Err.LastDllError)
        End If

        Return New Point(ptPos.X, ptPos.Y)
    End Function

    Private Function ColorToCOLORREF(clr As Color) As UInteger
        Return CUInt(clr.R) Or (CUInt(clr.G) << 8) Or (CUInt(clr.B) << 16)
    End Function

    Private Declare Ansi Function SetLayeredWindowAttributes Lib "user32.dll" _
        (hWnd As IntPtr, colorrefKey As UInteger, byAlpha As Byte, dwFlaiggs As UInteger) As Boolean

    Public Sub SetColorKey(wnd As IWin32Window, clrKey As Color)
        If Not SetLayeredWindowAttributes(wnd.Handle, ColorToCOLORREF(clrKey), 255, 1) Then
            Throw New Win32Exception(Err.LastDllError)
        End If
    End Sub

    Private Declare Auto Function SetWindowLong Lib "user32.dll" (hWnd As IntPtr, nIndex As Integer, dwNewLong As UInteger) As UInteger

    Private Declare Auto Function GetWindowLong Lib "user32.dll" (hwnd As IntPtr, nIndex As Integer) As UInteger

    Public Sub ChangeWindowStyle(wnd As IWin32Window, dwAdd As UInteger, dwRemove As UInteger)
        Dim dwStyle As UInteger

        dwStyle = GetWindowLong(wnd.Handle, -16)
        dwStyle = dwStyle And Not dwRemove
        dwStyle = dwStyle Or dwAdd
        SetWindowLong(wnd.Handle, -16, dwStyle)
    End Sub

    Public Sub ChangeWindowExStyle(wnd As IWin32Window, dwAdd As UInteger, dwRemove As UInteger)
        Dim dwExStyle As UInteger

        dwExStyle = GetWindowLong(wnd.Handle, -20)
        dwExStyle = dwExStyle And Not dwRemove
        dwExStyle = dwExStyle Or dwAdd
        SetWindowLong(wnd.Handle, -20, dwExStyle)
    End Sub

    Private Declare Ansi Function GetWindowRect Lib "user32.dll" (hWnd As IntPtr, ByRef rrectOut As Win32Rect) As Boolean

    Public Function GetWindowSize(wnd As IWin32Window) As Size
        Dim rect As Win32Rect

        If Not GetWindowRect(wnd.Handle, rect) Then
            Throw New Win32Exception(Err.LastDllError)
        End If

        Return New Size(rect.Right - rect.Left, rect.Bottom - rect.Top)
    End Function

    Public Function GetWindowLocation(wnd As IWin32Window) As Point
        Dim rect As Win32Rect

        If Not GetWindowRect(wnd.Handle, rect) Then
            Throw New Win32Exception(Err.LastDllError)
        End If

        Return New Point(rect.Left, rect.Top)
    End Function

    Public Function GetWindowRectangle(wnd As IWin32Window) As Rectangle
        Dim rectWin32 As Win32Rect

        If Not GetWindowRect(wnd.Handle, rectWin32) Then
            Throw New Win32Exception(Err.LastDllError)
        End If

        Return New Rectangle(rectWin32.Left, rectWin32.Top, rectWin32.Right - rectWin32.Left, rectWin32.Bottom - rectWin32.Top)
    End Function

    Private Declare Ansi Function GetClientRect Lib "user32.dll" (hWnd As IntPtr, ByRef rrectOut As Win32Rect) As Boolean

    Public Function GetClientAreaRectangle(wnd As IWin32Window) As Rectangle
        Dim rectWin32 As Win32Rect

        If Not GetClientRect(wnd.Handle, rectWin32) Then
            Throw New Win32Exception(Err.LastDllError)
        End If

        Return New Rectangle(rectWin32.Left, rectWin32.Top, rectWin32.Right - rectWin32.Left, rectWin32.Bottom - rectWin32.Top)
    End Function

    Private Declare Ansi Function PrintWindow Lib "user32.dll" (hWndSrc As IntPtr, hDCDest As IntPtr, uFlaiggs As UInteger) As Boolean

    Public Function DrawWindow(wndDraw As IWin32Window) As Bitmap
        Dim sizeWindow As Size
        Dim bmBuffer As Bitmap

        sizeWindow = GetWindowSize(wndDraw)
        bmBuffer = New Bitmap(sizeWindow.Width, sizeWindow.Height)

        Using gfx As Graphics = Graphics.FromImage(bmBuffer)
            Using dc As New DeviceContext(gfx)
                If Not PrintWindow(wndDraw.Handle, dc.Handle, 0) Then
                    Throw New Win32Exception(Err.LastDllError)
                End If
            End Using
        End Using

        Return bmBuffer
    End Function

    Private Declare Ansi Function PaintDesktop Lib "user32.dll" (hDCDest As IntPtr) As Boolean

    Public Sub DrawDesktopBackground(gfx As Graphics, Optional wnd As IWin32Window = Nothing)
        'Using dc As New DeviceContext(gfx)
        'If Not PaintDesktop(dc.Handle) Then
        'Throw New Win32Exception(Err.LastDllError)
        'End If
        'End Using


        Using bmDesktop As Bitmap = Desktop.MyDesktop.ProgramManager.DrawToBuffer()
            If wnd Is Nothing Then
                gfx.DrawImage(bmDesktop, 0, 0)
            Else
                gfx.DrawImage(bmDesktop, GetClientAreaRectangle(wnd), GetWindowRectangle(wnd), GraphicsUnit.Pixel)
            End If
        End Using
    End Sub

    Private Declare Unicode Function FindWindowW Lib "user32.dll" (strClassName As String, strWindowName As String) As IntPtr

    Public Function GetWindowByClass(strClass As String) As ForeignWindow
        Dim hWnd As IntPtr

        hWnd = FindWindowW(strClass, Nothing)
        If hWnd = IntPtr.Zero Then
            Throw New KeyNotFoundException("Unable to find any window of class """ & strClass & """")
        End If

        Return New ForeignWindow(hWnd)
    End Function

End Module
