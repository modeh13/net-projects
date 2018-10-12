using SQLite;

namespace AccesoSQLiteServicios.Models
{
    public class Employee
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Charge { get; set; }
        public string Email { get; set; }
    }
}