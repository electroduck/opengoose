Namespace Physics
    Public Class PhysicsObject
        Public mPosition As Vector2D
        Public mVelocity As Vector2D
        Public mMass As Double
        Public mFriction As Double

        Private mForcesSum As Vector2D

        Public Property Acceleration As Vector2D
            Get
                Return mForcesSum / mMass
            End Get
            Set(vecValue As Vector2D)
                mForcesSum = vecValue * mMass
            End Set
        End Property

        Public Sub New()
            Me.New(1.0)
        End Sub

        Public Sub New(fMass As Double)
            Me.New(fMass, 0.3)
        End Sub

        Public Sub New(fMass As Double, fFriction As Double)
            mPosition = Vector2D.Zero
            mVelocity = Vector2D.Zero
            mForcesSum = Vector2D.Zero
            mMass = fMass
            mFriction = fFriction
        End Sub

        Public Sub ApplyForce(vecForce As Vector2D)
            mForcesSum.Add(vecForce)
        End Sub

        Public Sub RemoveForce(vecForce As Vector2D)
            mForcesSum.Subtract(vecForce)
        End Sub

        Public Sub ClearForces()
            mForcesSum.X = 0
            mForcesSum.Y = 0
        End Sub

        Public Sub Simulate(fSeconds As Double)
            mVelocity.Add(Acceleration / fSeconds)
            mVelocity.Magnitude = mVelocity.Magnitude * mFriction
            mPosition.Add(mVelocity / fSeconds)
        End Sub
    End Class
End Namespace