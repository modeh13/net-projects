using MacPolloApp.View;
using System.Collections.ObjectModel;

namespace MacPolloApp.ViewModel
{
    public sealed class MainPageMasterViewModel
    {
        #region Properties
        public ObservableCollection<MainPageMenuItem> MenuItems { get; set; }
        #endregion

        #region Constructors
        public MainPageMasterViewModel()
        {
            MenuItems = new ObservableCollection<MainPageMenuItem>(new[]
            {
                new MainPageMenuItem { Id = 0, Title = Resources.AppResource.Home_Title, IconSource = "menu_home.png", TargetType = typeof(HomePage) },
                new MainPageMenuItem { Id = 1, Title = Resources.AppResource.Products_Title, IconSource = "menu_products.png", TargetType = typeof(HomePage) },
                new MainPageMenuItem { Id = 2, Title = Resources.AppResource.Shops_Title, IconSource = "menu_shops.png", TargetType = typeof(HomePage) },
                new MainPageMenuItem { Id = 3, Title = Resources.AppResource.YourOrders_Title, IconSource = "menu_orders.png", TargetType = typeof(HomePage) },
                new MainPageMenuItem { Id = 4, Title = Resources.AppResource.ContactUs_Title, IconSource = "menu_contactus.png", TargetType = typeof(ContactUsPage) },
                new MainPageMenuItem { Id = 5, Title = Resources.AppResource.Recipes_Title, IconSource = "menu_recipes.png", TargetType = typeof(RecipesPage) },
                new MainPageMenuItem { Id = 6, Title = Resources.AppResource.MyAccount_Title, IconSource = "menu_myaccount.png", TargetType = typeof(HomePage) },
                new MainPageMenuItem { Id = 7, Title = Resources.AppResource.Exit_Title, IconSource = "menu_exit.png", TargetType = typeof(HomePage) },
            });
        }
        #endregion
    }
}