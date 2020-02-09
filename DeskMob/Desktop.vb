Public Class Desktop
    Implements IWin32Window

    Public Shared ReadOnly Property MyDesktop As Desktop = New Desktop

    Private Shared mBuffer As New Bitmap(1, 1)

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

    Public ReadOnly Property Windows As IList(Of ForeignWindow)
        Get
            Return GetAllWindows()
        End Get
    End Property

    Public ReadOnly Property VisibleWindows As IList(Of ForeignWindow)
        Get
            Return GetVisibleWindows()
        End Get
    End Property

    Public ReadOnly Property EntireDesktopRectangle As Rectangle
        Get
            Return GetRectContainingAll(GetAllScreenAreas())
        End Get
    End Property

    ' TODO: Fix positioning of windows.
    Public Sub DrawEverythingOn(gfx As Graphics, wndDrawingOn As IWin32Window)
        Dim fwndTarget As New ForeignWindow(wndDrawingOn.Handle)
        Dim rectTarget As Rectangle
        Dim rectClient As Rectangle
        Dim rectCur As Rectangle
        Dim rectIntersect As Rectangle
        Dim rectSource As Rectangle
        Dim rectDest As Rectangle

        rectTarget = fwndTarget.ScreenRectangle
        rectClient = fwndTarget.ClientAreaRectangle

        DrawDesktopBackground(gfx, wndDrawingOn)
        For Each fwndCur As ForeignWindow In VisibleWindows
            If fwndCur.Handle <> fwndTarget.Handle Then
                Try
                    rectCur = fwndCur.ScreenRectangle
                    If rectCur.IntersectsWith(rectTarget) Then
                        rectIntersect = rectCur
                        rectIntersect.Intersect(rectTarget)
                        rectDest.X = rectTarget.X - rectIntersect.X
                        rectDest.Y = rectTarget.Y - rectIntersect.Y
                        rectDest.Width = rectCur.Width
                        rectDest.Height = rectCur.Height
                        rectSource.X = rectCur.X - rectIntersect.X
                        rectSource.Y = rectIntersect.Y - rectCur.Y
                        rectSource.Width = rectCur.Width
                        rectSource.Height = rectCur.Height
                        If rectCur.Y < rectTarget.Y Then
                            rectDest.Y = 0
                        End If
                        Using bmBuffer As Bitmap = fwndCur.DrawToBuffer
                            gfx.DrawImage(bmBuffer, rectSource, rectDest, GraphicsUnit.Pixel)
                        End Using
                    End If
                Catch ex As Exception
                    Debug.WriteLine("Error drawing foreign window: " & ex.ToString)
                End Try
            End If
        Next
    End Sub

    Public Function DrawEverything(Optional lstExcludeWindows As IList(Of IntPtr) = Nothing) As Bitmap
        Dim rectDesktop As Rectangle
        Dim rectWindow As Rectangle
        Dim bmBuffer As Bitmap

        If lstExcludeWindows Is Nothing Then
            lstExcludeWindows = New List(Of IntPtr)
        End If

        rectDesktop = EntireDesktopRectangle
        bmBuffer = New Bitmap(rectDesktop.Width, rectDesktop.Height)

        Using gfx As Graphics = Graphics.FromImage(bmBuffer)
            DrawDesktopBackground(gfx)
            For Each fwndCur As ForeignWindow In VisibleWindows
                rectWindow = fwndCur.ScreenRectangle
                If rectDesktop.IntersectsWith(rectWindow) And Not lstExcludeWindows.Contains(fwndCur.Handle) Then
                    Try
                        Using bmWindow As Bitmap = fwndCur.DrawToBuffer
                            gfx.DrawImage(bmBuffer, rectWindow.X - rectDesktop.X, rectWindow.Y - rectDesktop.Y)
                        End Using
                    Catch ex As Exception
                        Debug.WriteLine("Error drawing foreign window: " & ex.ToString)
                    End Try
                End If
            Next
        End Using

        Return bmBuffer
    End Function

End Class
