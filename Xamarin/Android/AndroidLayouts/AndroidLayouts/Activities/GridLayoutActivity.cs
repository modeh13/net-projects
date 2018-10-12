using Android.App;
using Android.OS;

namespace AndroidLayouts.Activities
{
    [Activity(Label = "GridLayoutActivity")]
    public class GridLayoutActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.GridLayout);
        }
    }
}