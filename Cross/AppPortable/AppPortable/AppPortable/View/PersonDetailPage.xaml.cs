using AppPortable.Model;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppPortable.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonDetailPage : ContentPage
    {
        public PersonDetailPage(Person context)
        {
            InitializeComponent();
            BindingContext = context;
        }
    }
}