using Android.App;
using Android.OS;

namespace AndroidLayouts.Activities
{
    [Activity(Label = "RelativeLayoutActivity")]
    public class RelativeLayoutActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.RelativeLayout);
        }
    }
}