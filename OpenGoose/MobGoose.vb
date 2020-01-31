Public Class MobGoose
    Inherits DesktopMob

    Public Overrides ReadOnly Property Mass As Double
        Get
            Return 10.0
        End Get
    End Property

    Public Overrides ReadOnly Property Diameter As Double
        Get
            Return 80.0
        End Get
    End Property
End Class
