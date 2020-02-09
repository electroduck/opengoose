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

    Public Sub SetAlpha(wnd As IWin32Window, byAlpha As Byte)
        If Not SetLayeredWindowAttributes(wnd.Handle, 0, byAlpha, 2) Then
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

    Private mDesktopCache As Bitmap = Nothing
    Private mDesktopCacheMutex As New Object

    Public Sub DrawDesktopBackground(gfx As Graphics, Optional wnd As IWin32Window = Nothing)
        'Using dc As New DeviceContext(gfx)
        'If Not PaintDesktop(dc.Handle) Then
        'Throw New Win32Exception(Err.LastDllError)
        'End If
        'End Using

        SyncLock mDesktopCacheMutex
            If mDesktopCache Is Nothing Then
                mDesktopCache = Desktop.MyDesktop.ProgramManager.DrawToBuffer()
            End If
        End SyncLock

        If wnd Is Nothing Then
            gfx.DrawImage(mDesktopCache, 0, 0)
        Else
            gfx.DrawImage(mDesktopCache, GetClientAreaRectangle(wnd), GetWindowRectangle(wnd), GraphicsUnit.Pixel)
        End If
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

    Private Delegate Function EnumWindowsProc(hWnd As IntPtr, param As IntPtr) As Boolean

    Private Declare Ansi Function EnumWindows Lib "user32.dll" (procCallback As EnumWindowsProc, param As IntPtr) As Boolean

    Public Function GetAllWindows() As IList(Of ForeignWindow)
        Dim lstWindows As New List(Of ForeignWindow)

        If Not EnumWindows(
            Function(hWnd As IntPtr, param As IntPtr)
                lstWindows.Add(New ForeignWindow(hWnd))
                Return True
            End Function, IntPtr.Zero) Then
            Throw New Win32Exception(Err.LastDllError)
        End If

        Return lstWindows
    End Function

    Private Declare Ansi Function IsWindowVisible Lib "user32.dll" (hWnd As IntPtr) As Boolean

    Public Function GetVisibleWindows() As IList(Of ForeignWindow)
        Dim lstWindows As New List(Of ForeignWindow)

        If Not EnumWindows(
            Function(hWnd As IntPtr, param As IntPtr)
                If IsWindowVisible(hWnd) Then
                    lstWindows.Add(New ForeignWindow(hWnd))
                End If
                Return True
            End Function, IntPtr.Zero) Then
            Throw New Win32Exception(Err.LastDllError)
        End If

        Return lstWindows
    End Function

    Private Declare Ansi Function InvalidateRect Lib "user32.dll" (hWnd As IntPtr, ByRef rrect As Win32Rect, bErase As Boolean) As Boolean

    Public Sub RedrawRectangle(wndRedraw As IWin32Window, rect As Rectangle)
        Dim rectWin32 As Win32Rect
        rectWin32.Left = rect.Left
        rectWin32.Top = rect.Top
        rectWin32.Right = rect.Right
        rectWin32.Bottom = rect.Bottom

        If Not InvalidateRect(wndRedraw.Handle, rectWin32, False) Then
            Throw New Win32Exception(Err.LastDllError)
        End If
    End Sub

    Public Sub RedrawIntersecting(wndRedraw As IWin32Window, wndCheck As IWin32Window)
        Dim rectWindowCheck As Rectangle
        Dim rectWindowRedraw As Rectangle
        Dim rectInvalidate As Rectangle

        rectWindowCheck = GetWindowRectangle(wndCheck)
        rectWindowRedraw = GetWindowRectangle(wndRedraw)

        If Not rectWindowRedraw.IntersectsWith(rectWindowCheck) Then
            Return
        End If

        rectInvalidate = rectWindowRedraw
        rectInvalidate.Intersect(rectWindowCheck)

        RedrawRectangle(wndRedraw, rectInvalidate)
    End Sub

End Module
