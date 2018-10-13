using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppPortable.Layouts
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LayoutsPage : ContentPage
	{
		public LayoutsPage ()
		{
			InitializeComponent ();

            btnStackLayout.Clicked += BtnStackLayout_Clicked;
            btnAbsoluteLayout.Clicked += (sender, args) =>
            {
                Navigation.PushAsync(new AbsoluteLayoutPage("AbsoluteLayout Page"));
            };

            btnRelativeLayout.Clicked += (sender, args) =>
            {
                Navigation.PushAsync(new RelativeLayoutPage());
            };

            btnGrid.Clicked += (sender, args) =>
            {
                Navigation.PushAsync(new GridLayout());
            };

            btnScrollView.Clicked += BtnScrollView_Clicked;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    // DependencyService: Pode acceder a las APIs de las plataformas
                    break;
                    //...

            }
        }

        private void BtnScrollView_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ScrollViewPage());
        }

        private void BtnStackLayout_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new StackLayoutPage());
        }
    }
}