using System.Collections.Generic;

namespace MacPolloApp.Model.Entities
{
    public sealed class PqrType : BaseObservable
    {
        #region Properties
        private string id;
        public string Id
        {
            get { return id; }
            set {
                id = value;
                OnPropertyChange();
            }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set {
                _value = value;
                OnPropertyChange();
            }
        }
        #endregion

        #region Constructors
        public PqrType() {}

        public PqrType(string id, string _value)
        {
            Id = id;
            Value = _value;
        }
        #endregion

        #region Methods
        public static PqrType GetDefault()
        {
            return new PqrType("P", Resources.AppResource.ContactUs_Item_P);
        }

        public static List<PqrType> GetList()
        {
            return new List<PqrType>
            {
                new PqrType("P", Resources.AppResource.ContactUs_Item_P),
                new PqrType("Q", Resources.AppResource.ContactUs_Item_Q),
                new PqrType("R", Resources.AppResource.ContactUs_Item_R)
            };
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var item = obj as PqrType;

            if (item == null) return false;

            if (item.Id.Trim() == Id.Trim() && item.Value.Trim() == Value.Trim()) return true;

            return base.Equals(obj);
        }
        #endregion
    }
}