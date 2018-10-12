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
	public partial class ScrollViewPage : ContentPage
	{
		public ScrollViewPage ()
		{
			InitializeComponent();

            lvwDatos.ItemsSource = new string[] {
                "Fabio",
                "Rosa",
                "Fabián",
                "Germán",
                "Luisa",
                "Juan Pablo",
                "Luis Fernando"
            };

            lvwApellidos.ItemsSource = new string[]{
                "Ramírez",
                "Montejo",
                "Vela",
                "Suárez",
                "Carrillo",
            };

            btnScroll.Clicked += (sender, args) =>
            {
                svwContent.ScrollToAsync(txtNombre, ScrollToPosition.Center, true);
                txtNombre.Focus();
            };
        }
    }
}