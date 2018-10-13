using System.Collections.Generic;

namespace MacPolloApp.Model.Entities
{
    public sealed class Pqr : BaseObservable
    {
        #region Properties
        private PqrType type;
        public PqrType Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChange();
            }
        }

        private string personName;
        public string PersonName
        {
            get { return personName; }
            set
            {
                personName = value;
                OnPropertyChange();
            }
        }

        private string personPhone;
        public string PersonPhone
        {
            get { return personPhone; }
            set
            {
                personPhone = value;
                OnPropertyChange();
            }
        }

        private string personEmail;
        public string PersonEmail
        {
            get { return personEmail; }
            set {
                personEmail = value;
                OnPropertyChange();
            }
        }

        private List<string> lines;
        public List<string> Lines
        {
            get { return lines; }
            set {
                lines = value;
                OnPropertyChange();
            }
        }
        #endregion

        #region Constructors
        public Pqr()
        {            
            PersonName = string.Empty;
            PersonPhone = string.Empty;
            PersonEmail = string.Empty;
            Type = PqrType.GetDefault();
            Lines = new List<string>();
        }
        #endregion
    }
}