Namespace Physics
    Public Structure Vector2D
        Implements IEquatable(Of Vector2D)

        Public X As Double
        Public Y As Double

        Public Property Direction As Double
            Get
                Return Math.Atan(Y / X)
            End Get
            Set(fValue As Double)
                Dim fMag As Double = Magnitude
                X = fMag * Math.Cos(fValue)
                Y = fMag * Math.Sin(fValue)
            End Set
        End Property

        Public Property Magnitude As Double
            Get
                Return Math.Sqrt(Math.Pow(X, 2.0) + Math.Pow(Y, 2.0))
            End Get
            Set(fValue As Double)
                Dim fMag As Double = Magnitude
                If fMag = 0 Then
                    X = 0
                    Y = 0
                Else
                    X = (X / fMag) * fValue
                    Y = (Y / fMag) * fValue
                End If
            End Set
        End Property

        Public ReadOnly Property AsPoint As Point
            Get
                Return New Point(X, Y)
            End Get
        End Property

        Public ReadOnly Property AsPointF As PointF
            Get
                Return New PointF(X, Y)
            End Get
        End Property

        Public ReadOnly Property UnitVector As Vector2D
            Get
                Dim vec As Vector2D
                vec.X = X
                vec.Y = Y
                vec.Magnitude = 1
                Return vec
            End Get
        End Property

        Public Shared Function Zero() As Vector2D
            Dim vecZero As Vector2D
            vecZero.X = 0
            vecZero.Y = 0
            Return vecZero
        End Function

        Public Shared Function FromPoint(pt As Point) As Vector2D
            Dim vec As Vector2D
            vec.X = pt.X
            vec.Y = pt.Y
            Return vec
        End Function

        Public Shared Function FromPoint(pt As PointF) As Vector2D
            Dim vec As Vector2D
            vec.X = pt.X
            vec.Y = pt.Y
            Return vec
        End Function

        Public Shared Function FromMousePosition() As Vector2D
            Return FromPoint(GetMousePosition)
        End Function

        Public Function EqualsVector(vecOther As Vector2D) As Boolean Implements IEquatable(Of Vector2D).Equals
            Return vecOther.X = X And vecOther.Y = Y
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            Return If(obj.GetType Is GetType(Vector2D), EqualsVector(obj), False)
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return X Xor Y
        End Function

        Public Overrides Function ToString() As String
            Return "(" & X.ToString & "," & Y.ToString & ")"
        End Function

        Public Sub Add(vecOther As Vector2D)
            X += vecOther.X
            Y += vecOther.Y
        End Sub

        Public Sub Subtract(vecOther As Vector2D)
            X -= vecOther.X
            Y -= vecOther.Y
        End Sub

        Public Shared Operator +(vecA As Vector2D, vecB As Vector2D) As Vector2D
            Dim vecSum As Vector2D
            vecSum.X = vecA.X + vecB.X
            vecSum.Y = vecA.Y + vecB.Y
            Return vecSum
        End Operator

        Public Shared Operator -(vec As Vector2D) As Vector2D
            Dim vecNeg As Vector2D
            vecNeg.X = -vec.X
            vecNeg.Y = -vec.Y
            Return vecNeg
        End Operator

        Public Shared Operator -(vecA As Vector2D, vecB As Vector2D) As Vector2D
            Return vecA + (-vecB)
        End Operator

        Public Shared Operator =(vecA As Vector2D, vecB As Vector2D) As Boolean
            Return vecA.EqualsVector(vecB)
        End Operator

        Public Shared Operator <>(vecA As Vector2D, vecB As Vector2D) As Boolean
            Return Not vecA.EqualsVector(vecB)
        End Operator

        Public Shared Operator /(vec As Vector2D, fScalar As Double) As Vector2D
            Dim vecDivided As Vector2D
            vecDivided.X = vec.X / fScalar
            vecDivided.Y = vec.Y / fScalar
            Return vecDivided
        End Operator

        Public Shared Operator *(vec As Vector2D, fScalar As Double) As Vector2D
            Dim vecMultiplied As Vector2D
            vecMultiplied.X = vec.X * fScalar
            vecMultiplied.Y = vec.Y * fScalar
            Return vecMultiplied
        End Operator
    End Structure
End Namespace