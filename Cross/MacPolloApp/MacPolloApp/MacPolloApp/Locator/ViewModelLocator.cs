using MacPolloApp.ViewModel;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;


namespace MacPolloApp.Locator
{
    public sealed class ViewModelLocator
    {
        #region "Properties"
        private UnityContainer unityContainer;
        private UnityServiceLocator unityServiceLocator;
        
        // Create for each ViewModel
        public MainPageMasterViewModel MainPageMasterViewModel => unityContainer.Resolve<MainPageMasterViewModel>();
        public HomeViewModel HomeViewModel => unityContainer.Resolve<HomeViewModel>();
        public ContactUsViewModel ContactUsViewModel => unityContainer.Resolve<ContactUsViewModel>();
        #endregion

        #region "Constructors"
        public ViewModelLocator()
        {
            unityContainer = new UnityContainer();
            unityServiceLocator = new UnityServiceLocator(unityContainer);

            //Register Services
            //unityContainer.RegisterType<IEmployeeService, EmployeeService>();

            //Register ViewModels
            unityContainer.RegisterType<MainPageMasterViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<HomeViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ContactUsViewModel>(new ContainerControlledLifetimeManager());

            ServiceLocator.SetLocatorProvider(() => unityServiceLocator);
        }
        #endregion
    }
}