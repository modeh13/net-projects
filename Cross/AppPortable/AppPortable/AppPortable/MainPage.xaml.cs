using AppPortable.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppPortable
{
    public partial class MainPage : ContentPage
    {
        public Employee NewEmployee { get; set; }

        public MainPage()
        {
            InitializeComponent();

            NewEmployee = new Employee
            {
                IdentificationId = "1098698008",
                FirstName = "Germán",
                LastName = "Ramírez"
            };

            BindingContext = NewEmployee;
        }
    }
}