using AppPortable.Model;
using AppPortable.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppPortable.ViewModel
{
    public class PersonViewModel : Person
    {
        public ObservableCollection<Person> Persons { get; set; }
        PersonService service = new PersonService();
        Person person;

        public PersonViewModel()
        {
            Persons = service.GetList();

            SaveCommand = new Command(async() => await Save(), () => !IsBusy);
            UpdateCommand = new Command(async () => await Update(), () => !IsBusy);
            DeleteCommand = new Command(async () => await Delete(), () => !IsBusy);
            ClearCommand = new Command(Clear, () => !IsBusy);
            AbsoluteCommand = new Command<string>(async (string message) => await GoToAbsolutePage(message), (message) => !IsBusy);
        }

        //Commands (Se ejecutan en el Formulario)
        public Command SaveCommand { get; set; }
        public Command UpdateCommand { get; set; }
        public Command DeleteCommand { get; set; }
        public Command ClearCommand { get; set; }
        public Command<string> AbsoluteCommand { get; set; }

        public async Task Save()
        {
            IsBusy = true;
            person = new Person()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = FirstName,
                LastName = LastName,
                Age = Age
            };
            await Task.Delay(2000);
            service.Add(person);
            Clear();
            IsBusy = false;
        }

        public async Task Update()
        {
            IsBusy = true;
            person = new Person()
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                Age = Age
            };
            await Task.Delay(2000);
            service.Update(person);
            IsBusy = false;
        }

        public async Task Delete()
        {
            IsBusy = true;
            person = new Person()
            {
                Id = Id,
            };

            await Task.Delay(2000);
            service.Delete(person.Id);
            Clear();
            IsBusy = false;
        }

        public async Task GoToAbsolutePage(string message)
        {
            IsBusy = true;            
            
            await Task.Delay(2000);
            IsBusy = false;
        }

        private void Clear() {
            Id = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Age = 0;            
        }
    }
}