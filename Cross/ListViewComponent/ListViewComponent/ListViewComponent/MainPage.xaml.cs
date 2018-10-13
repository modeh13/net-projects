using ListViewComponent.View;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ListViewComponent
{
	public partial class MainPage : ContentPage
	{        
		public MainPage()
		{
            Title = "Menú";
			InitializeComponent();
        }

        async protected void Btn_ShowNotifications_Clicked(object sender, System.EventArgs e)
        {
            //await Task.Run(new Action(() =>
            //{
            //    Navigation.PushAsync(new NotificationList());
            //}));

            await Navigation.PushAsync(new NotificationList());
        }
    }
}