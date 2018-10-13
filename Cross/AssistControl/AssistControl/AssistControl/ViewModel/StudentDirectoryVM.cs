using AssistControl.Model.Entities;
using AssistControl.Model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AssistControl.ViewModel
{
    public class StudentDirectoryVM : ObservableBase
    {
        private StudentDirectory mStudentDirectory;

        public StudentDirectory StudentDirectory
        {
            get { return mStudentDirectory; }
            set { mStudentDirectory = value; }
        }

        private bool mIsBusy;

        public bool IsBusy
        {
            get { return mIsBusy; }
            set { mIsBusy = value;
                OnPropertyChange();
            }
        }

        #region "Commands"
        public Command LoadDirectoryCommand { get; set; }
        #endregion

        public StudentDirectoryVM()
        {
            IsBusy = false;
            StudentDirectory = new StudentDirectory();          
            LoadDirectoryCommand = new Command(LoadDirectory, () => !IsBusy);
        }

        #region "Methods"
        protected async void LoadDirectory()
        {
            IsBusy = true;

            await Task.Delay(3000);
            StudentDirectory.Students = StudentDirectoryService.LoadStudentDirectory().Students;

            IsBusy = false;
        }
        #endregion
    }
}