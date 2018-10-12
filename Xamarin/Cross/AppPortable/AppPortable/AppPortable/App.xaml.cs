using AppPortable.Layouts;
using AppPortable.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AppPortable
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new AppPortable.MainPage();
            //MainPage = new MyContentPage(); Own Implementation
            //MainPage = new DemoList();
            //MainPage = new NavigationPage(new PersonPage()); // Permite navegación entre Páginas
            MainPage = new NavigationPage(new LayoutsPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}