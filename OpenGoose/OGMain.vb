Public Class OGMain
    Public Shared Sub Main()
        Dim mob As DesktopMob = Nothing

        While mob Is Nothing
            Try
                FixWebBrowserControl()
                mob = New MobGoose
                mob.Show()
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
