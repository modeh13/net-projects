using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace DemoAndroid
{
    [Activity(Label = "DemoAndroid", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            double dolares, pesos;
            Button btnConvertir = FindViewById<Button>(Resource.Id.btnConvertir);
            EditText txtDolares = FindViewById<EditText>(Resource.Id.txtDolares);
            EditText txtPesos = FindViewById<EditText>(Resource.Id.txtPesos);

            btnConvertir.Click += (sender, e) => {
                try
                {
                    dolares = double.Parse(txtDolares.Text);
                    pesos = dolares * 19.5;
                    txtPesos.Text = pesos.ToString();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
                }
            };
        }
    }
}