﻿Public Module Util
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

    Public Function PickRandom(Of T)(lst As IList(Of T), Optional rand As Random = Nothing) As T
        If rand Is Nothing Then
            rand = New Random
        End If

        Return lst.Item(rand.Next(lst.Count))
    End Function

    Public Delegate Function DownloadDelegate(Of T)(strURL As String) As T

    Public Function SafeDownload(Of T)(procDownload As DownloadDelegate(Of T), strURL As String,
                                       Optional nTimeoutMillis As Integer = 1000) As T
        Dim thdDownload As System.Threading.Thread
        Dim output As T

        thdDownload = New Threading.Thread(
            Sub()
                output = procDownload(strURL)
            End Sub
        )

        thdDownload.Start()
        thdDownload.Join(nTimeoutMillis)

        If thdDownload.IsAlive Then
            Throw New TimeoutException("Download of " & strURL & " did not complete within " & nTimeoutMillis & "ms")
        End If

        Return output
    End Function

    Public Sub FixWebBrowserControl()
        Dim strEXEName As String = Process.GetCurrentProcess.ProcessName & ".exe"
        Dim dwRegVersion As Integer
        Dim nMajorVersion As Integer

        Using ctlBrowser As New WebBrowser
            nMajorVersion = ctlBrowser.Version.Major
        End Using

        Select Case nMajorVersion
            Case 11
                dwRegVersion = 11000

            Case 10
                dwRegVersion = 10001

            Case 9
                dwRegVersion = 9999

            Case 8
                dwRegVersion = 8888

            Case Else
                dwRegVersion = 7000
        End Select

        My.Computer.Registry.SetValue(
            "HKEY_CURRENT_USER\SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION",
            strEXEName, dwRegVersion)
    End Sub
End Module
