using MacPolloApp.Model.Entities;
using MacPolloApp.Util.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MacPolloApp.ViewModel
{
    public sealed class ContactUsViewModel : BaseObservable
    {
        #region Properties
        private readonly short maxLengthLine = 132;
        private readonly short maxNumberLines = 10;
        private int maxLength = 0;

        public ObservableCollection<PqrType> PqrTypeItems { get; set; }

        private Pqr pqr;
        public Pqr Pqr
        {
            get { return pqr; }
            set { pqr = value; }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChange();
                OnPropertyChange(nameof(RemainingCharacters));
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set {
                isBusy = value;
                OnPropertyChange();
            }
        }

        private int remainingCharacters;
        public int RemainingCharacters
        {
            get { return maxLength - Message.Length; }
            set {
                remainingCharacters = value;
                OnPropertyChange();
            }
        }

        #endregion

        #region Commands
        public ICommand SendCommand { get; private set; } 
        #endregion

        #region Constructors
        public ContactUsViewModel()
        {
            Message = string.Empty;
            maxLength = (maxLengthLine * maxNumberLines);
            RemainingCharacters = maxLength;
            PqrTypeItems = new ObservableCollection<PqrType>(PqrType.GetList());

            SendCommand = new Command(async () => await SavePqr(), () => !IsBusy);
            Pqr = new Pqr();
        }

        private async Task SavePqr()
        {
            if (Message.Length > 50)
            {
                IsBusy = true;
                pqr.Lines = StringHelper.GetLines(maxLengthLine, Message).Take(maxNumberLines).ToList();

                //Pending -> Get User data
                await Task.Delay(3000);

                IsBusy = false;
            }
            else {
                //Notification required message
            }
        }
        #endregion
    }
}