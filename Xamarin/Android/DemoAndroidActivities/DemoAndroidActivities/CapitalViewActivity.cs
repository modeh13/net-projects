using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace DemoAndroidActivities
{
    [Activity(Label = "CapitalViewActivity")]
    public class CapitalViewActivity : Activity
    {
        Button btnSalir;
        EditText txtCapitalMex;
        EditText txtCapitalCol;
        ImageView ivwMexico;
        ImageView ivwColombia;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CapitalView);

            // Get Controls
            btnSalir = FindViewById<Button>(Resource.Id.btnSalir);
            txtCapitalMex = FindViewById<EditText>(Resource.Id.txtCapitalMex);
            txtCapitalCol = FindViewById<EditText>(Resource.Id.txtCapitalCol);
            ivwMexico = FindViewById<ImageView>(Resource.Id.ivwMexico);
            ivwColombia = FindViewById<ImageView>(Resource.Id.ivwColombia);

            // Set EventHandlers
            btnSalir.Click += (sender, e) => {
                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            };

            // Set Value Controls
            try
            {
                txtCapitalMex.Text = Intent.GetDoubleExtra("capitalMex", default(double)).ToString();
                txtCapitalCol.Text = Intent.GetDoubleExtra("capitalCol", default(double)).ToString();

                ivwMexico.SetImageResource(Resource.Drawable.mexico);
                ivwColombia.SetImageResource(Resource.Drawable.colombia);
            }
            catch (Exception e)
            {
                Toast.MakeText(this, e.Message, ToastLength.Short).Show();
            }
        }
    }
}