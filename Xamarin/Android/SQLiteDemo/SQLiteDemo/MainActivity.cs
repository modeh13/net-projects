using Android.App;
using Android.Widget;
using Android.OS;
using SQLiteDemo.Models;
using SQLiteDemo.Adapter;
using Android.Content;
using SQLiteDemo.Activities;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SQLiteDemo
{
    [Activity(Label = "SQLiteDemo", MainLauncher = true)]
    public class MainActivity : Activity
    {
        TextView tvwTitle;
        Button btnCreateEmployee;
        ListView lvwEmployees;

        DataAccess<Employee> data;
        List<Employee> employeesList;
        EmployeeAdapter adapter;

        #region "Events Activity"
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //Get Controls
            tvwTitle = FindViewById<TextView>(Resource.Id.tvwTitle);
            btnCreateEmployee = FindViewById<Button>(Resource.Id.btnCreateEmployee);
            lvwEmployees = FindViewById<ListView>(Resource.Id.lvwEmployees);

            //Set Controls Events
            lvwEmployees.ItemClick += LvwEmployees_ItemClick;
            lvwEmployees.ItemSelected += LvwEmployees_ItemSelected;
            btnCreateEmployee.Click += BtnCreateEmployee_Click;
        }        

        protected override void OnStart()
        {
            base.OnStart();

            //Refresh ListView Employees
            data = DataAccess<Employee>.GetInstance();
            employeesList = data.GetList();
            adapter = new EmployeeAdapter(this, employeesList);
            lvwEmployees.Adapter = adapter;
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        #endregion

        #region "Events Controls"
        private void LvwEmployees_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {

        }

        private void LvwEmployees_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            LoadEmployeeForm(adapter[e.Position]);            
        }

        private void BtnCreateEmployee_Click(object sender, System.EventArgs e)
        {
            LoadEmployeeForm(new Employee());
        }
        #endregion

        #region "Methods"
        private void LoadEmployeeForm(Employee employee)
        {
            Intent objIntent = new Intent(this, typeof(EmployeeFrmActivity));
            objIntent.SetFlags(ActivityFlags.ReorderToFront);
            objIntent.PutExtra("employee", JsonConvert.SerializeObject(employee));
            StartActivityForResult(objIntent, 0);
        }
        #endregion
    }
}