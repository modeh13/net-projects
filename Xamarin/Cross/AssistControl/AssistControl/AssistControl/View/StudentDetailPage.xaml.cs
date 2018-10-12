using AssistControl.Model.Entities;
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
    public partial class StudentDetailPage : ContentPage
    {
        public StudentDetailPage(Student student)
        {
            InitializeComponent();
            BindingContext = student;
        }
    }
}