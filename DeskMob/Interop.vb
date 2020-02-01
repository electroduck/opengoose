Imports System.ComponentModel
Imports System.Runtime.InteropServices

Module Interop
    <StructLayout(LayoutKind.Sequential)>
    Private Structure Win32Point
        Public X As Integer
        Public Y As Integer
    End Structure

    Private Declare Ansi Function GetCursorPos Lib "user32.dll" (ByRef lpPoint As Win32Point) As Boolean

    Public Function GetMousePosition() As Point
        Dim ptPos As Win32Point

        If Not GetCursorPos(ptPos) Then
            Throw New Win32Exception(Err.LastDllError)
        End If

        Return New Point(ptPos.X, ptPos.Y)
    End Function
End Module
