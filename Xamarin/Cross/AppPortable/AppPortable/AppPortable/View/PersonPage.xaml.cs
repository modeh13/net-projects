using AppPortable.Model;
using AppPortable.ViewModel;
using AppPortable.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppPortable.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonPage : ContentPage
    {
        PersonViewModel context = new PersonViewModel();

        public PersonPage()
        {
            InitializeComponent();
            BindingContext = context;
            lvwPersons.ItemSelected += LvwPersons_ItemSelected;
            btnAbsolute.Clicked += BtnAbsolute_Clicked;
        }

        private void BtnAbsolute_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AbsoluteLayoutPage(txtMessage.Text));
        }

        private void LvwPersons_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null) {
                Person person = (Person) e.SelectedItem;

                // Set data to Context
                //context.Id = person.Id;
                //context.FirstName = person.FirstName;
                //context.LastName = person.LastName;
                //context.Age = person.Age;

                Navigation.PushAsync(new PersonDetailPage(person));
                lvwPersons.SelectedItem = null;
            }
        }
    }
}