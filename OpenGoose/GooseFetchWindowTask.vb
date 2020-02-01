<TaskWeight(10)>
Public MustInherit Class GooseFetchWindowTask
    Inherits MobTask

    Private Enum SourceSide
        Top
        Bottom
        Left
        Right
    End Enum

    Private mDestA As Physics.Vector2D
    Private mDestB As Physics.Vector2D
    Private mDraggingForm As Boolean = False
    Private mSrcSide As SourceSide

    Private mForm As Form
    Protected Property Window As Form
        Get
            Return mForm
        End Get
        Set(frmValue As Form)
            If State = TaskState.Running Then
                Throw New InvalidOperationException("Cannot change fetch task form while task is running")
            Else
                mForm = frmValue
            End If
        End Set
    End Property

    Public Sub New(mobFetcher As DesktopMob)
        MyBase.New(mobFetcher)
    End Sub

    Protected Overrides Sub OnBegin()
        Dim rand As New Random
        Dim rectAllScreens As Rectangle
        Dim nStartX As Integer
        Dim nStartY As Integer
        Dim nGooseX As Integer
        Dim nGooseY As Integer
        Dim nTargetX As Integer
        Dim nTargetY As Integer

        If mForm Is Nothing Then
            Throw New InvalidOperationException("Window fetch task must set form before calling MyBase.OnBegin")
        End If

        rectAllScreens = GetRectContainingAll(GetAllScreenAreas)

        Select Case rand.Next(4)
            Case 0 ' From above
                mSrcSide = SourceSide.Top
                nStartX = rand.Next(rectAllScreens.X, rectAllScreens.X + rectAllScreens.Width - mForm.Width)
                nStartY = rectAllScreens.Y - mForm.Height - 16
                nGooseX = nStartX + mForm.Width \ 2
                nGooseY = nStartY - Mob.Diameter \ 2
                nTargetX = nGooseX
                nTargetY = rand.Next(nGooseY + mForm.Height * 2, nGooseY + mForm.Height * 4)

            Case 1 ' From below
                mSrcSide = SourceSide.Bottom
                nStartX = rand.Next(rectAllScreens.X, rectAllScreens.X + rectAllScreens.Width - mForm.Width)
                nStartY = rectAllScreens.Y + rectAllScreens.Height + mForm.Height + 16
                nGooseX = nStartX + mForm.Width \ 2
                nGooseY = nStartY + Mob.Diameter \ 2
                nTargetX = nGooseX
                nTargetY = rand.Next(nGooseY - mForm.Height * 3, nGooseY - mForm.Height * 2)

            Case 2 ' From left
                mSrcSide = SourceSide.Left
                nStartX = rectAllScreens.X - mForm.Width - 16
                nStartY = rand.Next(rectAllScreens.Y, rectAllScreens.Y + rectAllScreens.Height - mForm.Height)
                nGooseX = nStartX - Mob.Diameter \ 2
                nGooseY = nStartY + mForm.Height \ 2
                nTargetX = rand.Next(nGooseX + mForm.Width * 2, nGooseX + mForm.Width * 4)
                nTargetY = nGooseY

            Case 3 ' From right
                mSrcSide = SourceSide.Right
                nStartX = rectAllScreens.X + rectAllScreens.Width + mForm.Width + 16
                nStartY = rand.Next(rectAllScreens.Y, rectAllScreens.Y + rectAllScreens.Height - mForm.Height)
                nGooseX = nStartX + Mob.Diameter \ 2
                nGooseY = nStartY + mForm.Height \ 2
                nTargetX = rand.Next(nGooseX - mForm.Width * 3, nGooseX - mForm.Width * 2)
                nTargetY = nGooseY

            Case Else
                Throw New Exception("Invalid entry direction from random generator")
        End Select

        mDraggingForm = False
        mForm.StartPosition = FormStartPosition.Manual
        mForm.DesktopLocation = New Point(rectAllScreens.X - mForm.Width - 16, rectAllScreens.Y - mForm.Height - 16)
        mDestA = Physics.Vector2D.FromPoint(New Point(nGooseX, nGooseY))
        mDestB = Physics.Vector2D.FromPoint(New Point(nTargetX, nTargetY))

        AddHandler mForm.FormClosing, AddressOf mForm_Closing
        AddHandler mForm.FormClosed, AddressOf mForm_Closed

        Mob.MoveTowards(mDestA.AsPoint, 10000.0)
    End Sub

    Protected Overrides Sub OnTick()
        If Not mDraggingForm Then
            If (mDestA - Mob.Position).Magnitude < 150 Then
                Mob.StopMoving()
                Mob.Position = mDestA
                mDraggingForm = True
                mForm.Show()
                Mob.MoveTowards(mDestB.AsPoint, 100000.0)
            End If
        Else
            If (mDestB - Mob.Position).Magnitude < 150 Then
                Mob.StopMoving()

                RemoveHandler mForm.FormClosing, AddressOf mForm_Closing
                RemoveHandler mForm.FormClosed, AddressOf mForm_Closed

                SetComplete()
            End If

            Select Case mSrcSide
                Case SourceSide.Top
                    mForm.DesktopLocation = New Point(Mob.Position.X - mForm.Width \ 2, Mob.Position.Y + Mob.Diameter)

                Case SourceSide.Bottom
                    mForm.DesktopLocation = New Point(Mob.Position.X - mForm.Width \ 2, Mob.Position.Y - mForm.Height + Mob.Diameter \ 2)

                Case SourceSide.Left
                    mForm.DesktopLocation = New Point(Mob.Position.X + Mob.Diameter, Mob.Position.Y - mForm.Height \ 2)

                Case SourceSide.Right
                    mForm.DesktopLocation = New Point(Mob.Position.X - mForm.Width + Mob.Diameter \ 2, Mob.Position.Y - mForm.Height \ 2)
            End Select

            mForm.Select()
        End If
    End Sub

    Protected Overrides Sub OnAbort()
        mForm.Hide()
        MyBase.OnAbort()
    End Sub

    Private Sub mForm_Closing(sender As Object, e As FormClosingEventArgs)
        e.Cancel = True
    End Sub

    Private Sub mForm_Closed(sender As Object, e As FormClosedEventArgs)
        Abort()
    End Sub
End Class
