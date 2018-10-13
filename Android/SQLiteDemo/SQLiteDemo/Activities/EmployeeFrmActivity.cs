using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using SQLiteDemo.Models;
using Newtonsoft.Json;
using Android.Views;

namespace SQLiteDemo.Activities
{
    [Activity(Label = "Crear Empleado")]
    public class EmployeeFrmActivity : Activity
    {
        Button btnSaveEmployee;
        Button btnDeleteEmployee;
        EditText edtName;
        EditText edtEmail;
        EditText edtCharge;
        TextView tvwTitle;
        Employee employee;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.EmployeeFrm);

            // Get Controls
            tvwTitle = FindViewById<TextView>(Resource.Id.tvwTitle);
            btnSaveEmployee = FindViewById<Button>(Resource.Id.btnSaveEmployee);
            btnDeleteEmployee = FindViewById<Button>(Resource.Id.btnDeleteEmployee);
            edtName = FindViewById<EditText>(Resource.Id.edtName);
            edtEmail = FindViewById<EditText>(Resource.Id.edtEmail);
            edtCharge = FindViewById<EditText>(Resource.Id.edtCharge);

            // Set Events
            btnSaveEmployee.Click += BtnSaveEmployee_Click;
            btnDeleteEmployee.Click += BtnDeleteEmployee_Click;

            employee = JsonConvert.DeserializeObject<Employee>(Intent.GetStringExtra("employee"));

            tvwTitle.Text = (employee.Id > 0) ? "Editar Empleado" : "Crear Empleado" ;
            btnDeleteEmployee.Visibility = (employee.Id > 0) ? ViewStates.Visible : ViewStates.Gone;

            edtName.Text = employee.Name;
            edtCharge.Text = employee.Charge;
            edtEmail.Text = employee.Email;
        }

        private void BtnSaveEmployee_Click(object sender, EventArgs e)
        {
            try
            {
                int rowsAffected = 0;
                employee.Name = edtName.Text;
                employee.Email = edtEmail.Text;
                employee.Charge = edtCharge.Text;

                DataAccess<Employee> data = DataAccess<Employee>.GetInstance();
                rowsAffected = (employee.Id == 0) ? data.Insert(employee) : data.Update(employee);

                if (rowsAffected > 0) {
                    Toast.MakeText(this, "Guardado correctamente !", ToastLength.Short).Show();
                    Finish();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }

        private void BtnDeleteEmployee_Click(object sender, EventArgs e)
        {
            try
            {
                DataAccess<Employee> data = DataAccess<Employee>.GetInstance();
                int rowsAffected = data.Delete(employee.Id);

                if (rowsAffected > 0)
                {
                    Toast.MakeText(this, "Eliminado correctamente !", ToastLength.Short).Show();
                    Finish();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }
    }
}