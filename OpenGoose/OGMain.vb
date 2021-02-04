Public Class OGMain
    Public Shared Sub Main()
        Dim mob As DesktopMob = Nothing

        While mob Is Nothing
            Try
                FixWebBrowserControl()
                mob = New MobGoose
                mob.Show()
                mob.Tick()
                Application.DoEvents()
            Catch ex As Exception
                Select Case MessageBox.Show("Error initializing: " & ex.ToString, "OpenGoose", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error)
                    Case DialogResult.Abort
                        End

                    Case DialogResult.Retry
                        Continue While

                    Case DialogResult.Ignore
                        Exit While
                End Select
            End Try
        End While

        If Not My.Settings.InitialMessageShown Then
            MessageBox.Show("Thank you for downloading me, the goose." + vbNewLine + vbNewLine +
                            "You can make me go away any time by right clicking the goose icon in your system tray and selecting Exit." + vbNewLine + vbNewLine +
                            "The memes I find are from Gfycat, iFunny, Reddit, and YouTube. They are completely random, so please don't hold it against me if you don't like them.",
                            "Goose Alert", MessageBoxButtons.OK, MessageBoxIcon.Information)
            My.Settings.InitialMessageShown = True
            My.Settings.Save()
        End If

        Do
            Try
                mob.Tick()
                Application.DoEvents()
            Catch ex As Exception
                Select Case MessageBox.Show("Error in main loop: " & ex.ToString, "OpenGoose", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error)
                    Case DialogResult.Abort
                        End

                    Case DialogResult.Retry
                        Continue Do ' loop again without waiting

                    Case DialogResult.Ignore
                        ' do nothing (wait, then loop again)
                End Select
            End Try
            Threading.Thread.Sleep(10)
        Loop
    End Sub
End Class
