<AttributeUsage(AttributeTargets.Class, AllowMultiple:=False)>
Public Class TaskWeightAttribute
    Inherits Attribute

    Public mWeight As Integer

    Public Sub New(nWeight As Integer)
        mWeight = nWeight
    End Sub

    Public Shared Function GetWeight(typeTask As Type) As Integer
        Dim attribs() As Object = typeTask.GetCustomAttributes(True)
        For Each attrib In attribs
            If attrib.GetType Is GetType(TaskWeightAttribute) Then
                Return DirectCast(attrib, TaskWeightAttribute).mWeight
            End If
        Next
        Return 0
    End Function

End Class
