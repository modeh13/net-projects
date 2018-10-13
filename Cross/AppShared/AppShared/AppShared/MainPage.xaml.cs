using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppShared
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

#if __ANDROID__
            lblMessage.Text = "Welcome to Xamarin from ANDRODI";
#elif __IOS__
            lblMessage.Text = "Welcome to Xamarin from iOS";
        
#elif __WINDOWS_UWP__
            lblMessage.Text = "Welcome to Xamarin from UWP";
#endif
        }
    }
}