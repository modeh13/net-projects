using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using NotificationList.Activities;

namespace NotificationList
{
    [Activity(Label = "Demo Notificaciones", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button btnShowNotifications;
        Toolbar toolbar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "Demo Notificaciones";

            btnShowNotifications = FindViewById<Button>(Resource.Id.btnShowNotifications);
            btnShowNotifications.Click += BtnShowNotifications_Click;
        }

        private void BtnShowNotifications_Click(object sender, System.EventArgs e)
        {
            Intent objIntent = new Intent(this, typeof(NotificationActivity));
            //objIntent.SetFlags(ActivityFlags.ReorderToFront);            
            StartActivityForResult(objIntent, 0);
        }
    }
}