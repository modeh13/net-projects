using AppPortable.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppPortable
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DemoList : ContentPage
    {
        public List<Employee> ListEmploys { get; set; }

        public DemoList()
        {
            InitializeComponent();

            ListEmploys = Employee.GetList(); 

            lvwArray.ItemsSource = new string[] {
                "Fabio",
                "Rosa",
                "Fabián",
                "Germán"
            };

            lvwList.ItemsSource = ListEmploys;
            lvwList.ItemSelected += LvwList_ItemSelected;
        }

        private void LvwList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Employee employee = e.SelectedItem as Employee;
                lvwList.SelectedItem = null; // Eliminar Selección
            }
        }
    }
}