' NOTE: You can use the "Rename" command on the context menu to change the class name "Service1" in code, svc and config file together.
' NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.vb at the Solution Explorer and start debugging.
Imports WcfGeneric
Imports MySql
Imports MySql.Data.MySqlClient

Public Class GarvGenericService
    Implements IGarvGenericService

    Public Sub New()
    End Sub

    Public Function InsertEmployee(employee As Employee) As Integer Implements IGarvGenericService.InsertEmployee
        Dim connectionString As String = GetConnectionString()
        Dim mySqlConnection As New MySqlConnection(connectionString)
        Dim rowsAffected As Integer = 0

        Try
            mySqlConnection.Open()

            Dim mySqlCommand As New MySqlCommand
            With mySqlCommand
                .Connection = mySqlConnection
                .CommandText = $"INSERT INTO employees (Name, Charge, Email) VALUES ('{employee.Name}', '{employee.Charge}', '{employee.Email}')"
                .CommandType = CommandType.Text
            End With

            rowsAffected = mySqlCommand.ExecuteNonQuery()
            mySqlConnection.Close()

        Catch ex As Exception

        Finally

        End Try

        Return rowsAffected
    End Function

    Protected Function GetConnectionString() As String
        Dim server As String = "localhost"
        Dim database As String = "garv_company"
        Dim user As String = "user_admin"
        Dim password As String = "Sigma1702*"

        Return $"SERVER={server}; DATABASE={database}; UID={user}; PASSWORD={password};"
    End Function
End Class