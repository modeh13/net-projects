using ListViewComponent.Model;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListViewComponent.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetailNotification : ContentPage
	{
		public DetailNotification (Notification notication)
		{
			InitializeComponent();
            Title = "Detalle de Notificación";
            BindingContext = notication;
		}
	}
}