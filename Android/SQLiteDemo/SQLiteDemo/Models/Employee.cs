using SQLite;
using System;

namespace SQLiteDemo.Models
{
    [Serializable]
    public class Employee
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Charge { get; set; }        

        public Employee()
        {
            Id = 0;
            Name = string.Empty;
            Email = string.Empty;
            Charge = string.Empty;           
        }
    }
}