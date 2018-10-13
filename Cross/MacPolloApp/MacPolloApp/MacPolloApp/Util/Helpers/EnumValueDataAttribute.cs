using System;

namespace MacPolloApp.Util.Helpers
{
    public class EnumValueDataAttribute : Attribute
    {
        #region Properties
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string keyValue;
        public string KeyValue
        {
            get { return keyValue; }
            set { keyValue = value; }
        } 
        #endregion
    }
}