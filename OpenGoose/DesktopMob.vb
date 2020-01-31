Public Class DesktopMob
    Private mPhysObj As New Physics.PhysicsObject
    Private mLastTickTime As Date = Date.Now
    Private mMovementTarget As Physics.Vector2D = Physics.Vector2D.Zero
    Private mMoving As Boolean = False

    Public Property Mass As Double
        Get
            Return mPhysObj.mMass
        End Get
        Set(fValue As Double)
            mPhysObj.mMass = fValue
        End Set
    End Property

    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub MoveTowards(pt As Point)
        mMovementTarget = Physics.Vector2D.FromPoint(pt)
        mMovementTarget.X -= Width / 2
        mMovementTarget.Y -= Height / 2
        mMoving = True
    End Sub

    Public Sub StopMoving()
        mMoving = False
        mMovementTarget = Physics.Vector2D.Zero
    End Sub

    Public Sub Tick()
        Dim dtThisTickTime As Date = Date.Now

        mPhysObj.ClearForces()
        If mMoving Then
            Dim vecForce As Physics.Vector2D = (mMovementTarget - mPhysObj.mPosition) / 1000.0
            vecForce.Magnitude = vecForce.Magnitude / 2.0 + Math.Pow(vecForce.Magnitude, 2.0) / 2.0
            mPhysObj.ApplyForce(vecForce)
        End If

        mPhysObj.Simulate((dtThisTickTime - mLastTickTime).TotalSeconds)
        mLastTickTime = dtThisTickTime

        DesktopLocation = mPhysObj.mPosition.AsPoint

        Invalidate()
    End Sub

    Private Sub DesktopMob_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        mPhysObj.mPosition.X = DesktopLocation.X + Width / 2
        mPhysObj.mPosition.Y = DesktopLocation.Y + Height / 2
    End Sub

    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
        'MyBase.OnPaintBackground(e)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        e.Graphics.Clear(Color.Fuchsia)
        e.Graphics.FillEllipse(Brushes.BlanchedAlmond, 0, 0, Width, Height)
        e.Graphics.DrawLine(Pens.Blue, Width \ 2, Height \ 2,
                            CInt(Width \ 2 + mPhysObj.mVelocity.X / mPhysObj.mVelocity.Magnitude * 50.0),
                            CInt(Height \ 2 + mPhysObj.mVelocity.Y / mPhysObj.mVelocity.Magnitude * 50.0))
    End Sub

End Class