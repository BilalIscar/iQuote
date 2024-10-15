Public Class Item

    Private _Expression As String
    Private _PrevItem As Item = Nothing
    Private _NextItem As Item = Nothing
    Private _SubList As Item = Nothing
    Private _SubParent As Item = Nothing
    Private _ParameterIndex As Integer
    Private _IllegalCalculation As Boolean = False

    Public Property Expression() As String
        Get
            Return _Expression
        End Get
        Set(ByVal value As String)
            _Expression = value
        End Set
    End Property

    Public Property NextItem() As Item
        Get
            Return _NextItem
        End Get
        Set(ByVal value As Item)
            _NextItem = value
        End Set
    End Property

    Public Property PrevItem() As Item
        Get
            Return _PrevItem
        End Get
        Set(ByVal value As Item)
            _PrevItem = value
        End Set
    End Property

    Public Property SubList() As Item
        Get
            Return _SubList
        End Get
        Set(ByVal value As Item)
            _SubList = value
        End Set
    End Property

    Public Property SubParent() As Item
        Get
            Return _SubParent
        End Get
        Set(ByVal value As Item)
            _SubParent = value
        End Set
    End Property

    Public Property ParameterIndex() As Integer
        Get
            Return _ParameterIndex
        End Get
        Set(ByVal value As Integer)
            _ParameterIndex = value
        End Set
    End Property

    Public Property IllegalCalculation() As Boolean
        Get
            Return _IllegalCalculation
        End Get
        Set(ByVal value As Boolean)
            _IllegalCalculation = value
        End Set
    End Property

    Public Sub New(ByVal pExpression As String, ByVal pPrevItem As Item)
        _Expression = pExpression
        _PrevItem = pPrevItem
        If Not pPrevItem Is Nothing Then
            pPrevItem.NextItem = Me
        End If
    End Sub

End Class
