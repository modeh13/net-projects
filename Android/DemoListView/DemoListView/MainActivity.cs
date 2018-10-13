using Android.App;
using Android.Widget;
using Android.OS;
using DemoListView.Services;
using DemoListView.Models;
using DemoListView.Adapters;

namespace DemoListView
{
    [Activity(Label = "DemoListView", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected EmployeeService service = new EmployeeService();

        //Views
        ListView lvwEmployess;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get Controls
            lvwEmployess = FindViewById<ListView>(Resource.Id.lvwEmployees);

            try
            {
                Employee[] employees = service.GetEmployees(20);
                //ArrayAdapter<Employee> adapter = new ArrayAdapter<Employee>(this, Android.Resource.Layout.SimpleListItem1, employees); //Generic Adapter
                EmployeeAdapter adapter = new EmployeeAdapter(employees);
                lvwEmployess.Adapter = adapter;
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short);
            }
        }
    }
}