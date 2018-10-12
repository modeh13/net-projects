<Serializable()>
<DataContract()>
Public Class Employee

    Private mId As Integer

    <DataMember()>
    Public Property Id() As Integer
        Get
            Return mId
        End Get
        Set(ByVal value As Integer)
            mId = value
        End Set
    End Property

    Private mName As String

    <DataMember()>
    Public Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal value As String)
            mName = value
        End Set
    End Property

    Private mCharge As String

    <DataMember()>
    Public Property Charge() As String
        Get
            Return mCharge
        End Get
        Set(ByVal value As String)
            mCharge = value
        End Set
    End Property

    Private mEmail As String

    <DataMember()>
    Public Property Email() As String
        Get
            Return mEmail
        End Get
        Set(ByVal value As String)
            mEmail = value
        End Set
    End Property

    Public Sub New()
    End Sub
End Class