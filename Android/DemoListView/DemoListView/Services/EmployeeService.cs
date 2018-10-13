using System;
using DemoListView.Models;

namespace DemoListView.Services
{
    public class EmployeeService
    {
        private string[] positions = { "Supervisor", "Operador", "Gerente", "Director" };

        public Employee[] GetEmployees(int number)
        {
            Employee[] employees = new Employee[number];
            Employee employee;
            Random random = new Random();

            for (int i = 0; i < number; i++)
            {
                employee = new Employee
                {
                    Name = Guid.NewGuid().ToString().Substring(0, 10),
                    Position = positions[random.Next(0, 3)],                    
                };
                employee.Email = $"{employee.Name}@mycompany.com.co";
                employees[i] = employee;
            }

            return employees;
        }
    }
}