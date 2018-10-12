using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace DemoAndroidActivities
{
    [Activity(Label = "DemoAndroidActivities", MainLauncher = true)]
    public class MainActivity : Activity
    {
        // Controls
        Button btnCalcular;
        EditText txtIngresosMex;
        EditText txtEgresosMex;
        EditText txtIngresosCol;
        EditText txtEgresosCol;

        // Atributos
        double ingresosMex, egresosMex, ingresosCol, egresosCol;
        double capitalMex, capitalCol;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get Controls
            btnCalcular = FindViewById<Button>(Resource.Id.btnCalcular);
            txtIngresosMex = FindViewById<EditText>(Resource.Id.txtIngresosMex);
            txtEgresosMex = FindViewById<EditText>(Resource.Id.txtEgresosMex);
            txtIngresosCol = FindViewById<EditText>(Resource.Id.txtIngresosCol);
            txtEgresosCol = FindViewById<EditText>(Resource.Id.txtEgresosCol);

            // Set EventHandlers
            btnCalcular.Click += delegate
            {
                try
                {
                    ingresosMex = double.Parse(txtIngresosMex.Text);
                    egresosMex = double.Parse(txtEgresosMex.Text);
                    ingresosCol = double.Parse(txtIngresosCol.Text);
                    ingresosCol = double.Parse(txtIngresosCol.Text);
                    capitalMex = ingresosMex - egresosMex;
                    capitalCol = ingresosCol - egresosCol;
                    Cargar();
                }
                catch (System.Exception e)
                {
                    Toast.MakeText(this, e.Message, ToastLength.Short).Show();
                }
            };
        }

        protected void Cargar()
        {
            Intent objIntent = new Intent(this, typeof(CapitalViewActivity));
            objIntent.PutExtra("capitalMex", capitalMex);
            objIntent.PutExtra("capitalCol", capitalCol);
            StartActivity(objIntent);
        }
    }
}