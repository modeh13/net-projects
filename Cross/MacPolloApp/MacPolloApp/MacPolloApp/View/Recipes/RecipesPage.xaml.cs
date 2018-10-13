using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MacPolloApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipesPage : ContentPage
    {
        #region Constructors
        public RecipesPage()
        {
            InitializeComponent();
        } 
        #endregion

        #region Events
        protected override void OnAppearing()
        {
            base.OnAppearing();
            wvwRecipes.IsVisible = false;
            actLoading.IsVisible = true;            
        }

        private void wvwRecipes_Navigated(object sender, WebNavigatedEventArgs e)
        {
            wvwRecipes.IsVisible = true;
            actLoading.IsVisible = false;
        }

        private void wvwRecipes_Navigating(object sender, WebNavigatingEventArgs e)
        {
            wvwRecipes.IsVisible = false;
            actLoading.IsVisible = true;
        } 
        #endregion
    }
}