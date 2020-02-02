Public Class MemeManager
    Private mSources As New Dictionary(Of Type, List(Of IMemeSource))

    Public Sub New()
        Dim arrSources As Type() = Reflection.Assembly.GetAssembly(GetType(MemeManager)).GetTypes
        Dim typeOutput As Type

        For Each typeSource In arrSources
            typeOutput = MemeSourceAttribute.GetOutputType(typeSource)
            If typeOutput IsNot Nothing Then
                Try
                    If Not mSources.ContainsKey(typeOutput) Then
                        mSources.Add(typeOutput, New List(Of IMemeSource))
                    End If
                    mSources(typeOutput).Add(Activator.CreateInstance(typeSource))
                    Debug.WriteLine("MemeManager registered " & typeSource.FullName & " as source of " & typeOutput.FullName)
                Catch ex As Exception
                    Debug.WriteLine("WARNING: Adding meme source " & typeOutput.FullName &
                                    " to sources list caused an exception: " & ex.ToString)
                End Try
            End If
        Next
    End Sub

    Public Function GetMeme(Of T As Meme)(Optional rand As Random = Nothing) As T
        Dim typeMeme As Type = GetType(T)
        Dim lstSources As New List(Of MemeSource(Of T))
        Dim meme As T = Nothing
        Dim nSrcIndex As Integer
        Dim src As MemeSource(Of T)

        If Not mSources.ContainsKey(typeMeme) Then
            Throw New InvalidOperationException("Cannot find any source of " & typeMeme.FullName)
        End If

        For Each ms As IMemeSource In mSources(typeMeme)
            Try
                lstSources.Add(ms)
            Catch ex As InvalidCastException
                Debug.WriteLine("WARNING: Meme source attribute for class " & ms.GetType.FullName & " specifies output as " &
                                typeMeme.FullName & " but class cannot be cast to MemeSource(Of " & typeMeme.FullName & ")")
            End Try
        Next

        If rand Is Nothing Then
            rand = New Random
        End If

        While lstSources.Count > 0
            nSrcIndex = rand.Next(lstSources.Count)
            src = lstSources(nSrcIndex)

            If Not src.CanGetMeme Then
                Try
                    src.Init()
                Catch ex As Exception
                    Debug.WriteLine("Error reinitializing meme source " & src.GetType.FullName & ": " & ex.ToString)
                    lstSources.RemoveAt(nSrcIndex)
                    Continue While
                End Try

                If Not src.CanGetMeme Then
                    lstSources.RemoveAt(nSrcIndex)
                    Continue While
                End If
            End If

            Try
                meme = src.GetMeme
                Return meme
            Catch ex As Exception
                Debug.WriteLine("Error getting meme from " & src.GetType.FullName & ": " & ex.ToString)
                lstSources.RemoveAt(nSrcIndex)
            End Try
        End While

        Throw New Exception("No source of " & typeMeme.FullName & " was able to produce a meme")
    End Function
End Class
