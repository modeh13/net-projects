using AssistControl.Model.Entities;
using System.Collections.ObjectModel;

namespace AssistControl.Model.Entities
{
    public class StudentDirectory : ObservableBase
    {
        private ObservableCollection<Student> mStudents;

        public ObservableCollection<Student> Students
        {
            get
            {
                return mStudents ?? new ObservableCollection<Student>();
            }
            set { mStudents = value;
                OnPropertyChange();
            }
        }
    }
}