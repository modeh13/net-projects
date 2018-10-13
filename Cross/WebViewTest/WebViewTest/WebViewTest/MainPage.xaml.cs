using WebViewTest.ViewModel;
using Xamarin.Forms;

namespace WebViewTest
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            BindingContext = new HtmlJsTestViewModel();
        }
	}
}