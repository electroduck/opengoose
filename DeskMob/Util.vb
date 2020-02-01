Public Module Util
    Public Function GetRectContainingAll(rects As IEnumerable(Of Rectangle)) As Rectangle
        Dim nMinX As Integer = 0
        Dim nMinY As Integer = 0
        Dim nMaxX As Integer = 0
        Dim nMaxY As Integer = 0

        For Each rect As Rectangle In rects
            If rect.X < nMinX Then
                nMinX = rect.X
            End If
            If (rect.X + rect.Width) > nMaxX Then
                nMaxX = rect.X + rect.Width
            End If
            If rect.Y < nMinY Then
                nMinY = rect.Y
            End If
            If (rect.Y + rect.Height) > nMaxY Then
                nMaxY = rect.Y + rect.Height
            End If
        Next

        Return New Rectangle(nMinX, nMinY, nMaxX - nMinX, nMaxY - nMinY)
    End Function

    Public Function GetAllScreenAreas() As IList(Of Rectangle)
        Dim lstScreenAreas As New List(Of Rectangle)
        For Each scrn As Screen In Screen.AllScreens
            lstScreenAreas.Add(scrn.WorkingArea)
        Next
        Return lstScreenAreas
    End Function
End Module
