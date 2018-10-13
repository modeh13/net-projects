using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MacPolloApp.View.Master
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        #region Constructors
        public MainPage()
        {
            Detail = new NavigationPage(new HomePage());
            InitializeComponent();
            MasterPage.lvwMenu.ItemSelected += lvwMenu_ItemSelected;
        }
        #endregion

        #region Events
        private void lvwMenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainPageMenuItem;
            if (item == null)
                return;

            var page = (Page) Activator.CreateInstance(item.TargetType);
            page.Title = string.IsNullOrEmpty(page.Title) ? item.Title : page.Title;

            Detail = new NavigationPage(page);
            IsPresented = false;
            MasterPage.lvwMenu.SelectedItem = null;
        }
        #endregion 
    }
}