using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebViewTest
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ToolbarMenuPage : ContentPage
	{
		public ToolbarMenuPage ()
		{
			InitializeComponent ();


            if (Device.RuntimePlatform == Device.Android)
            {
                ToolbarItems.Add(new ToolbarItem()
                {
                    Text = "Item 1",
                    Priority = 1,
                    Order = ToolbarItemOrder.Secondary
                });

                ToolbarItems.Add(new ToolbarItem()
                {
                    Text = "Item 2",
                    Priority = 2,
                    Order = ToolbarItemOrder.Secondary
                });
            }
        }
	}
}