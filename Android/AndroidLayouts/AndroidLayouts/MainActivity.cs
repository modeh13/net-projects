using Android.App;
using Android.OS;
using Android.Content;
using Android.Widget;
using AndroidLayouts.Activities;

namespace AndroidLayouts
{
    [Activity(Label = "AndroidLayouts", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get View's
            try
            {
                Button btnLinearLayout = FindViewById<Button>(Resource.Id.btnLinearLayout);
                Button btnRelativeLayout = FindViewById<Button>(Resource.Id.btnRelativeLayout);
                Button btnTableLayout = FindViewById<Button>(Resource.Id.btnTableLayout);
                Button btnGridLayout = FindViewById<Button>(Resource.Id.btnGridLayout);

                btnLinearLayout.Click += BtnLinearLayout_Click;
                btnRelativeLayout.Click += (sender, e) =>
                {
                    LoadLayout<RelativeLayoutActivity>();
                };

                btnTableLayout.Click += BtnTableLayout_Click;

                btnGridLayout.Click += delegate
                {
                    LoadLayout<GridLayoutActivity>();
                };
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        private void BtnTableLayout_Click(object sender, System.EventArgs e)
        {
            LoadLayout<TableLayoutActivity>();            
        }

        private void BtnLinearLayout_Click(object sender, System.EventArgs e)
        {
            LoadLayout<LinearLayoutActivity>();
        }

        protected void LoadLayout<T>() where T : Activity
        {
            Intent objIntent = new Intent(this, typeof(T));
            StartActivity(objIntent);
        }
    }
}