using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace RestAppDiplomaed
{
    public partial class App : Application
    {
        private MainPage mainPage;

        public App()
        {
            InitializeComponent();
            mainPage = new RestAppDiplomaed.MainPage();

            MainPage = mainPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            mainPage.Load();
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
