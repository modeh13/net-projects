using MacPolloApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MacPolloApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {   
            InitializeComponent();
            
            //Timer to automatic slider CarouselView
            Device.StartTimer(TimeSpan.FromSeconds(4), (() =>
            {
                cvcImagesHome.Position = (cvcImagesHome.Position + 1) % ((HomeViewModel)BindingContext).Images.Count;
                return true;
            }));
        }
    }
}