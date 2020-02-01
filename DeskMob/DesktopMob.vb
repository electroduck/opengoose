Public MustInherit Class DesktopMob
    Protected mPhysObj As New Physics.PhysicsObject
    Private mLastTickTime As Date = Date.Now
    Private mMovementTarget As Physics.Vector2D = Physics.Vector2D.Zero
    Private mMoving As Boolean = False
    Private WithEvents mForm As New DesktopMobForm
    Private mTask As MobTask
    Private mMovementForceDivisor As Double = 10000.0

    Public ReadOnly Property Form As DesktopMobForm
        Get
            Return mForm
        End Get
    End Property

    Public Overridable ReadOnly Property Mass As Double
        Get
            Return 1.0
        End Get
    End Property

    Public Overridable ReadOnly Property Diameter As Double
        Get
            Return 100.0
        End Get
    End Property

    Public Overridable ReadOnly Property ClickThrough As Boolean
        Get
            Return False
        End Get
    End Property

    Protected Overridable ReadOnly Property TaskTypes As IList(Of Type)
        Get
            Return New List(Of Type)
        End Get
    End Property

    Public ReadOnly Property Position As Physics.Vector2D
        Get
            Return mPhysObj.mPosition
        End Get
    End Property

    Public ReadOnly Property Velocity As Physics.Vector2D
        Get
            Return mPhysObj.mVelocity
        End Get
    End Property

    Public Sub New()
        mPhysObj.mMass = Mass
    End Sub

    Public Sub MoveTowards(pt As Point, Optional fSlowness As Double = 10000.0)
        mMovementForceDivisor = fSlowness
        mMovementTarget = Physics.Vector2D.FromPoint(pt)
        mMovementTarget.X -= mForm.Width / 2
        mMovementTarget.Y -= mForm.Height / 2
        mMoving = True
    End Sub

    Public Sub StopMoving()
        mMoving = False
        mMovementTarget = Physics.Vector2D.Zero
    End Sub

    Protected Overridable Function GetMovementForce(vecTarget As Physics.Vector2D, vecPosition As Physics.Vector2D) As Physics.Vector2D
        Dim vecForce As Physics.Vector2D = (vecTarget - vecPosition) / mMovementForceDivisor
        vecForce.Magnitude = vecForce.Magnitude / 2.0 + Math.Pow(vecForce.Magnitude, 2.0) / 2.0
        Return vecForce
    End Function

    Protected Overridable Sub OnTick()
    End Sub

    Public Sub Tick()
        Dim dtThisTickTime As Date

        If mTask Is Nothing OrElse mTask.IsStopped Then
            StartRandomTask()
        ElseIf mTask.State = MobTask.TaskState.Running Then
            mTask.Tick()
        End If

        OnTick()

        mPhysObj.ClearForces()
        If mMoving Then
            mPhysObj.ApplyForce(GetMovementForce(mMovementTarget, mPhysObj.mPosition))
        End If

        dtThisTickTime = Date.Now
        mPhysObj.Simulate((dtThisTickTime - mLastTickTime).TotalSeconds)
        mLastTickTime = dtThisTickTime

        mForm.DesktopLocation = mPhysObj.mPosition.AsPoint

        mForm.Invalidate()
    End Sub

    Private Sub DesktopMobForm_Load(sender As Object, e As EventArgs) Handles mForm.Load
        mForm.Width = Diameter + 32
        mForm.Height = Diameter + 32
        mPhysObj.mPosition.X = mForm.DesktopLocation.X + mForm.Width \ 2
        mPhysObj.mPosition.Y = mForm.DesktopLocation.Y + mForm.Height \ 2
    End Sub

    Protected Overridable Sub OnPaint(gfx As Graphics, s As Size)
        gfx.FillEllipse(Brushes.Pink, 0, 0, s.Width, s.Height)
    End Sub

    Private Sub DesktopMobForm_PaintBackBuffer(gfx As Graphics) Handles mForm.PaintBackBuffer
        OnPaint(gfx, mForm.ClientSize)
    End Sub

    Protected Overridable Sub OnShow()
    End Sub

    Public Sub Show()
        mForm.Show()
    End Sub

    Private Sub StartRandomTask()
        If TaskTypes.Count > 0 Then
            Dim rand As New Random
            Dim nSumWeights As Integer = 0
            Dim nTgtWeightsum As Integer
            Dim nTaskType As Integer

            For Each typeTask As Type In TaskTypes
                nSumWeights += TaskWeightAttribute.GetWeight(typeTask)
            Next

            nTgtWeightsum = rand.Next(nSumWeights)
            nTaskType = 0
            While nTgtWeightsum > 0
                nTgtWeightsum -= TaskWeightAttribute.GetWeight(TaskTypes(nTaskType))
                nTaskType += 1
            End While

            If nTgtWeightsum <> 0 Then
                nTaskType -= 1
            End If

            mTask = Activator.CreateInstance(TaskTypes(nTaskType), Me)
            mTask.Begin()
        End If
    End Sub

End Class
