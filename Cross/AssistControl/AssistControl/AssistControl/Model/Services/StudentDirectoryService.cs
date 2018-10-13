using AssistControl.Model.Entities;
using AssistControl.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistControl.Model.Services
{
    public class StudentDirectoryService
    {
        public static StudentDirectory LoadStudentDirectory()
        {
            Student student;
            StudentDirectory studentDirectory = new StudentDirectory();
            DatabaseManager dbManager = new DatabaseManager();
            ObservableCollection<Student> students = new ObservableCollection<Student>(dbManager.GetList<Student>());
            Random random = new Random(DateTime.Now.Millisecond);

            if (students.Any())
            {
                studentDirectory.Students = students;
                return studentDirectory;
            }

            students = new ObservableCollection<Student>();

            string[] names = { "Andrés", "Alejandro", "Fabián", "Sebastián", "Carlos", "César", "Héctor", "Germán" };
            string[] lastNames = { "Rosales", "Cadavid", "Díaz", "Delgado", "Carvajal", "Ramírez", "Arias", "Pardo" };

            for (byte i = 0; i < 20; i++)
            {
                student = new Student()
                {
                    Name = names[random.Next(0, names.Length - 1)],
                    LastName = lastNames[random.Next(0, lastNames.Length - 1)],
                    Group = random.Next(456, 458).ToString(),
                    StudentNumber = random.Next(12345678, 345678900).ToString(),
                    Key = random.Next(12345678, 345678900).ToString(),
                    Average = random.Next(100, 1000) / 10
                };
                dbManager.Save<Student>(student);
                students.Add(student);
            }

            studentDirectory.Students = students;
            return studentDirectory;
        }
    }
}