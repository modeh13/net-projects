using AssistControl.Storage;

namespace AssistControl.Model.Entities
{
    public class Student : ObservableBase, IKeyObject
    {
        #region "Properties"
        public string Key { get; set; }

        private string mName;

        public string Name
        {
            get { return mName; }
            set { mName = value;
                OnPropertyChange();
            }
        }

        private string mLastName;

        public string LastName
        {
            get { return mLastName; }
            set { mLastName = value;
                OnPropertyChange();
            }
        }

        private string mGroup;

        public string Group
        {
            get { return mGroup; }
            set { mGroup = value;
                OnPropertyChange();
            }
        }

        private string mStudentNumber;

        public string StudentNumber
        {
            get { return mStudentNumber; }
            set { mStudentNumber = value;
                OnPropertyChange();
            }
        }

        private double mAverage;

        public double Average
        {
            get { return mAverage; }
            set { mAverage = value;
                OnPropertyChange();
            }
        }
        #endregion

        #region "Contructors"
        public Student()
        {

        }
        #endregion
    }
}