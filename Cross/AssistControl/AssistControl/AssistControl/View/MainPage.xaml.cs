using AssistControl.Model.Entities;
using AssistControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AssistControl.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new StudentDirectoryVM();
        }

        private void lvStudents_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Student student = e.SelectedItem as Student;

            if (student != null)
            {
                Navigation.PushAsync(new StudentDetailPage(student));
                lvStudents.SelectedItem = null;
            }
        }
    }
}