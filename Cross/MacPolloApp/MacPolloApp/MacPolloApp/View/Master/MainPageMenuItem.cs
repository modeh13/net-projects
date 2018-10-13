using System;

namespace MacPolloApp.View
{
    public class MainPageMenuItem
    {
        #region "Properties"
        public int Id { get; set; }

        public string Title { get; set; }

        public string IconSource { get; set; }

        public Type TargetType { get; set; }
        #endregion

        #region "Constructors"
        public MainPageMenuItem()
        {
            TargetType = typeof(HomePage);
        }
        #endregion
    }
}