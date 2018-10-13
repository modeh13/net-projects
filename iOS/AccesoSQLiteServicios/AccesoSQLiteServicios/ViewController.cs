using System;
using System.IO;
using UIKit;
using SQLite;
using AccesoSQLiteServicios.Models;
using System.Linq;
using System.Collections.Generic;

namespace AccesoSQLiteServicios
{
    public partial class ViewController : UIViewController
    {
        SQLiteConnection Connection;
        string[] names = { "Germán A. Ramírez Vela", "Héctor García", "César Cadavid", "Alejandro Carvajal", "Andrés Rosales", "Andrés Arias", "Carlos Delgado" };
        string[] charges = { "Desarrollador I", "Desarrollador II", "Desarrollador III", "Desarrollador IV" };
        string[] emails = { "german.ramirez@gmail.com", "hector.garcia@gmail.com", "cesar.cadavid@gmail.com", "alejandro.carvajal@gmail.com", "andres.rosales@gmail.com", "andres.arias@gmail.com", "carlos.delgado@gmail.com" };

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            string pathDB = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            pathDB = Path.Combine(pathDB, "Base.db3");

            Connection = new SQLiteConnection(pathDB);
            Connection.CreateTable<Employee>();

            btnSaveSQLite.TouchUpInside += BtnSaveSQLite_TouchUpInside;
            btnSendService.TouchUpInside += BtnSendService_TouchUpInside;
        }

        private void BtnSendService_TouchUpInside(object sender, EventArgs e)
        {
            int savedCount = 0;
            int rowsAffected = 0;
            WcfGeneric.GarvGenericService wcfGeneric = new WcfGeneric.GarvGenericService();
            UIActionSheet visor = new UIActionSheet();

            try
            {                
                List<Employee> employees = Connection.Table<Employee>().ToList();

                foreach (Employee employee in employees)
                {
                    visor.Add(employee.Id.ToString());
                    visor.Add(employee.Name);
                    visor.Add(employee.Charge);
                    visor.Add(employee.Email);

                    wcfGeneric.InsertEmployee(new WcfGeneric.Employee
                    {
                        Id = 0,
                        Name = employee.Name,
                        Charge = employee.Charge,
                        Email = employee.Email
                    }, out rowsAffected, out bool result);

                    //wcfGeneric.InsertEmployeeAsync(new WcfGeneric.Employee
                    //{
                    //    Id = 0,
                    //    Name = employee.Name,
                    //    Charge = employee.Charge,
                    //    Email = employee.Email
                    //});

                    if (rowsAffected > 0) { savedCount++; }
                }
                visor.Title = "";
                visor.Style = UIActionSheetStyle.BlackOpaque;
                visor.ShowInView(this.View);

                MessageBox("Info", $"Se enviaron {savedCount} elementos al Servicio.");
            }
            catch (Exception ex)
            {
                MessageBox("Error", ex.Message);
            }
        }

        private void BtnSaveSQLite_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                int rowsAffected = 0;
                Random random = new Random();
                Employee employee = new Employee
                {
                    Name = names[random.Next(0, names.Length - 1)],
                    Charge = charges[random.Next(0, charges.Length - 1)],
                    Email = emails[random.Next(0, emails.Length - 1)],
                };

                rowsAffected = Connection.Insert(employee);

                if (rowsAffected > 0) { MessageBox("Info", "Guardado correctamente !"); }
            }
            catch (Exception ex)
            {
                MessageBox("Error", ex.Message);
            }
        }

        public void MessageBox(string title, string message)
        {
            UIAlertView alert = new UIAlertView
            {
                Title = title,
                Message = message
            };

            alert.AddButton("OK");
            alert.Show();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}