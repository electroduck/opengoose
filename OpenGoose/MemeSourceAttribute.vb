Public Class MemeSourceAttribute
    Inherits Attribute

    Public mMemeType As Type

    Public Sub New(typeOfMeme As Type)
        mMemeType = typeOfMeme
    End Sub

    Public Shared Function GetOutputType(typeMemeSource As Type) As Type
        Dim attribs() As Object = typeMemeSource.GetCustomAttributes(True)
        For Each attrib In attribs
            If attrib.GetType Is GetType(MemeSourceAttribute) Then
                Return DirectCast(attrib, MemeSourceAttribute).mMemeType
            End If
        Next
        Return Nothing
    End Function
End Class
