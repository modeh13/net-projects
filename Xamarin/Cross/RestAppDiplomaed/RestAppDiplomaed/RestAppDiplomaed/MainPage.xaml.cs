using RestAppDiplomaed.Models;
using RestAppDiplomaed.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace RestAppDiplomaed
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Country> Countries { get; set; }
        private RestClient client;
        public Command RefreshCommand { get; set; }        

        public MainPage()
        {
            Countries = new ObservableCollection<Country>();
            client = new RestClient();
            RefreshCommand = new Command(() => Load());

            InitializeComponent();
        }

        public async void Load()
        {
            var result = await client.GetCountries();
            Countries.Clear();

            foreach (var item in result)
            {
                Countries.Add(item);
            }
            IsBusy = false;
        }        
    }
}