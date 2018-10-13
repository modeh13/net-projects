using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppPortable.Model
{
    public class Employee
    {
        public string IdentificationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Employee() {
            
        }

        public static List<Employee> GetList()
        {
            return new List<Employee>
            {
                new Employee { IdentificationId = "19256089", FirstName = "Fabio", LastName = "Ramírez" },
                new Employee { IdentificationId = "51960561", FirstName = "Rosa", LastName = "Vela" },
                new Employee { IdentificationId = "1098665984", FirstName = "Fabián", LastName = "Ramírez" },
                new Employee { IdentificationId = "1098698008", FirstName = "Germán", LastName = "Ramírez" }
            };
        }
    }
}