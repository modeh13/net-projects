using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppPortable
{
    public class MyContentPage : ContentPage
    {
        public MyContentPage()
        {
            var lbl = new Label
            {
                Text = "Escribe tu nombre"
            };

            var txtNombre = new Entry
            {
                Placeholder = "Escribe tu nombre"
            };

            var btn = new Button
            {
                Text = "Click me !"
            };

            //btn.Clicked += Btn_Clicked;
            btn.Clicked += (sender, e) =>
            {
                DisplayAlert("Mensaje", txtNombre.Text, "Cerrar");
            };

            Content = new StackLayout
            {
                Padding = 30,
                Spacing = 10,
                Children = {
                    lbl,
                    txtNombre,
                    btn
                }
            };
        }
    }
}