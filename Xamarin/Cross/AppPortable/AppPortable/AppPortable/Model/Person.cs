using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AppPortable.Model
{
    public class Person : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string id;

        public string Id
        {
            get { return id; }
            set { id = value;
                OnPropertyChanged();
            }
        }

        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FullName));
            }
        }
        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FullName));
            }
        }

        private int age;

        public int Age
        {
            get { return age; }
            set { age = value;
                OnPropertyChanged();
            }
        }

        private string fullName;
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
            set { fullName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Message));
            }
        }

        public string Message { get { return $"Your name is {FullName}"; } }


        private bool isBusy = false; // Manage Proccess with this Model

        public bool IsBusy
        {
            get { return isBusy = false; }
            set { isBusy = value;
                OnPropertyChanged();
            }
        }

        //Métodos
        public void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}