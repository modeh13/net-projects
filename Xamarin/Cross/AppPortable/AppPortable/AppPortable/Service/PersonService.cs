using AppPortable.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace AppPortable.Service
{
    public class PersonService
    {
        // Listado de Personas
        ObservableCollection<Person> persons;

        public PersonService() {
            persons = new ObservableCollection<Person>();
        }

        // Métodos
        public ObservableCollection<Person> GetList() {
            return persons;
        }

        public Person GetById(string id)
        {
            return persons.FirstOrDefault(x => x.Id == id);
        }

        public void Add(Person entity) {
            persons.Add(entity);
        }

        public void Update(Person entity) {
            int index = persons.IndexOf(persons.FirstOrDefault(x => x.Id == entity.Id));

            if (index > 0) {
                persons[index] = entity;
            }
        }

        public void Delete(string id) {
            persons.Remove(persons.FirstOrDefault(x => x.Id == id));
        }
    }
}