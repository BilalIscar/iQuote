Public Class AuthenticationException
    Inherits GeneralException

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal Message As String)
        MyBase.New(Message)
    End Sub

    Public Sub New(ByVal Message As String, ByVal innerException As Exception)
        MyBase.New(Message, innerException)
    End Sub

    Public Sub New(ByVal MessageEnum As GeneralException.ErrorMessages)
        MyBase.New(MessageEnum)
    End Sub
End Class
