Public Class Node
    Private _nodeValue As String
    Private _Nodes As List(Of Node)
    Private _Parent As Node

    Public Property NodeValue() As String
        Get
            Return _nodeValue
        End Get
        Set(ByVal value As String)
            _nodeValue = value
        End Set
    End Property

    Public ReadOnly Property Nodes() As List(Of Node)
        Get
            Return _Nodes
        End Get
    End Property

    Public ReadOnly Property Parent()
        Get
            Return _Parent
        End Get
    End Property

    Public Sub New(ByVal fName As String, ByVal Parent As Node)
        NodeValue = fName
        _Parent = Parent
    End Sub

    Public Sub AddNode(ByVal n As Node)
        If _Nodes Is Nothing Then
            _Nodes = New List(Of Node)
        End If
        _Nodes.Add(n)
        n._Parent = Me
    End Sub
End Class
