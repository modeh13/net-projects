namespace DemoListView.Models
{
    public class Employee
    {
        //Attributes
        private string name;
        private string position;
        private string email;

        //Properties
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Position
        {
            get { return position; }
            set { position = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        //Constructor
        public Employee()
        {                
        }

        public Employee(string name, string position, string email)
        {
            Name = name;
            Position = position;
            Email = email;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}