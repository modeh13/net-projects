using MacPolloApp.Model.Entities;

namespace MacPolloApp.Util.Helpers
{
    public sealed class PickerDefaultItem : BaseObservable
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
        public PickerDefaultItem() { }

        public PickerDefaultItem(string _id, string _value)
        {
            Id = _id;
            Value = _value;
        }
        #endregion

        #region Methods
        public static PickerDefaultItem GetDefaultItem()
        {
            return new PickerDefaultItem("", Resources.AppResource.Empty_Item);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var item = obj as PickerDefaultItem;

            if (item == null) return false;

            if (item.Id.Trim() == Id.Trim() && item.Value.Trim() == Value.Trim()) return true;

            return base.Equals(obj);
        }
        #endregion
    }
}