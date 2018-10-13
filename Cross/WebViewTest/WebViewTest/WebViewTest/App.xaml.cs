using Xamarin.Forms;

namespace WebViewTest
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            //MainPage = new WebViewTest.MainPage(); //WebView
            //MainPage = new ErrorPage(); //Error Page
            MainPage = new NavigationPage(new ToolbarMenuPage());
		}

		protected override void OnStart ()
		{
            // Handle when your app starts            
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}