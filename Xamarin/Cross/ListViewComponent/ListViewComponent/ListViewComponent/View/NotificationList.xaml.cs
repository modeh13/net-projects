using ListViewComponent.Control;
using ListViewComponent.Model;
using ListViewComponent.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListViewComponent.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotificationList : ContentPage
	{
        private NotificationsViewModel viewModel;
        private const string pageTitle = "Notificaciones";

        public NotificationList ()
		{
			InitializeComponent ();
            Title = pageTitle;
            ToolbarItems.Clear();
            BindingContext = viewModel = new NotificationsViewModel();

            if (Device.RuntimePlatform == Device.iOS)
            {
                ToolbarItem item = new ToolbarItem("Seleccionar", "ic_checklist.png", DeletedNotifications, ToolbarItemOrder.Primary);
                item.Text = "Seleccionar";
                item.Order = 0;

                ((NavigationPage)Application.Current.MainPage).ToolbarItems.Clear();
                ((NavigationPage)Application.Current.MainPage).ToolbarItems.Add(item);
            }
        }

        async public void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
            UIListViewItem<Notification> notification = e.Item as UIListViewItem<Notification>;
            return;
            if (lvwNotifications.IsSelectionMode)
            {
                notification.IsSelected = !notification.IsSelected;

                if (lvwNotifications.ItemsSelectedCount() == 0)
                {
                    RestoreToolBar();
                }
                else {
                    Title = $"{lvwNotifications.ItemsSelectedCount()} seleccionados";
                }
            }
            else
            {
                //await Task.Run(new Action(() =>
                //{
                    viewModel.markNotificationAsRead(notification.Item.Id);
                    await Navigation.PushAsync(new DetailNotification(notification.Item));
                //}));
            }
        }

        protected void Handle_LongPressEvent(object sender, ItemLongClickEventArgs e)
        {
            Console.Write("Long Press !");
            if (!lvwNotifications.IsSelectionMode)
            {
                UIListViewItem<Notification> notification = lvwNotifications.GetItemByPosition(e.Position);
                notification.IsSelected = true;

                List<ToolbarItem> toolBarItems = new List<ToolbarItem>();

                if (Device.RuntimePlatform == Device.Android)
                {
                    //ToolbarItems.Add(new ToolbarItem("Delete", "ic_action_delete.png", DeletedNotifications, ToolbarItemOrder.Primary));
                    ToolbarItem item = new ToolbarItem("Delete", "ic_action_delete.png", DeletedNotifications, ToolbarItemOrder.Primary);
                    item.Text = "Delete";
                    item.Order = 0;
                    toolBarItems.Add(item);
                }

                if (Device.RuntimePlatform == Device.iOS)
                {
                    ((NavigationPage)Application.Current.MainPage).ToolbarItems.Clear();
                    ToolbarItem item = new ToolbarItem("Back", "ic_back.png", DeletedNotifications, ToolbarItemOrder.Primary);
                    item.Text = "Back";
                    item.Order = ToolbarItemOrder.Primary;
                    toolBarItems.Add(item);

                    item = new ToolbarItem("Delete", "ic_delete.png", DeletedNotifications, ToolbarItemOrder.Primary);
                    item.Text = "Delete";
                    item.Order = ToolbarItemOrder.Primary;
                    toolBarItems.Add(item);
                }

                toolBarItems.ForEach(x => ((NavigationPage)Application.Current.MainPage).ToolbarItems.Add(x));

                //ToolbarItems.Add(item);
                //((NavigationPage)Application.Current.MainPage).ToolbarItems.Add(item);

                if (Device.RuntimePlatform == Device.Android)
                {
                    //((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.DarkGray;
                }
                
                Title = $"{lvwNotifications.ItemsSelectedCount()} seleccionados";
            }
        }

        private void RestoreToolBar()
        {
            Title = pageTitle;
            ToolbarItems.Clear();
            lvwNotifications.ClearSelection();
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.Default;
            //((NavigationPage)Application.Current.MainPage).BarTextColor = Color.OrangeRed;
        }

        void DeletedNotifications()
        {

        }

        protected override bool OnBackButtonPressed()
        {
            if (lvwNotifications.IsSelectionMode)
            {
                RestoreToolBar();
                return true;
            }

            return base.OnBackButtonPressed();
        }
    }
}