using ListView.Forms.Plugin.Abstractions.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FListViewImpl
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            Title = "Main Page";            
            BindingContext = new PersonViewModel();
		}

        protected void lvwPerson_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lvwPersons.SelectedItem = null;
            Debug.WriteLine("Item Tapped !");
            Person person = e.Item as Person;
        }

        protected void lvwPerson_ItemLongPress(object sender, FItemLongPressArgs e)
        {
            //handle long press event here
            Debug.WriteLine("Long Press !");
            Console.Write("Long Press !");
            Person person = e.Row.Item as Person;            
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");
        }
    }

    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string ImageUrl { get; set; }
    }
    public class PersonViewModel
    {
        private ObservableCollection<Person> personList;

        public ObservableCollection<Person> PersonList
        {
            get { return personList; }
            set { personList = value; }
        }

        public PersonViewModel()
        {
            PersonList = GetPersonList();
        }

        public ObservableCollection<Person> GetPersonList()
        {
            List<Person> list = new List<Person>();
            Person person;
            string[] names = {"Fabio", "Rosa", "Fabián", "Germán", "Luisa", "Juan Pablo", "Luis" };
            Random random = new Random();

            for (int i = 0; i <= 20; i++)
            {
                person = new Person();
                person.Id = Guid.NewGuid();
                person.Name = names[random.Next(0, names.Length - 1)];
                person.Age = random.Next(18, 40);
                //person.ImageUrl = "http://icons.iconarchive.com/icons/custom-icon-design/pretty-office-2/72/man-icon.png";
                person.ImageUrl = "https://www.udemy.com/";                
                list.Add(person);
            }

            return new ObservableCollection<Person>(list);
        }
    }
}