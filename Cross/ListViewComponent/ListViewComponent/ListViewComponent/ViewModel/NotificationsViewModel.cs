using ListViewComponent.Control;
using ListViewComponent.Model;
using ListViewComponent.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace ListViewComponent.ViewModel
{
    public class NotificationsViewModel :BaseViewModel
    {
        #region Properties
        private ObservableCollection<Notification> notificationList;

        public ObservableCollection<Notification> NotificationList
        {
            get { return notificationList; }
            set { notificationList = value; }
        }
        #endregion

        #region Constructors
        public NotificationsViewModel()
        {   
            NotificationList = new ObservableCollection<Notification>(getNotificationList());
        }
        #endregion

        #region Methods
        private List<Notification> getNotificationList()
        {
            IsBusy = true;
            //List<UIListViewItem<Notification>> items = new List<UIListViewItem<Notification>>();
            List<Notification> list = new List<Notification>();
            Notification notification;
            //UIListViewItem<Notification> item;
            Random random = new Random();

            string[] senders = { "Wilmar Ortiz", "Héctor García", "Germán A. Ramírez", "Aderson Rangel", "Mario Moreno" };
            string[] subjects = { "Mint AVA Request", "Dev-Trunk Test", "PWC orgchart", "Outsourching Test", "Mayasoft Conference" };
            string[] messages = { "On Windows Phone, this is the complementary color chosen by the user. Good Windows Phone applications use this as part of their styling to provide a native look and feel.",
                                  "On iOS and Android this instance is set to a contrasting color that is visible on the default background but is not the same as the default text color.",
                                  "These colors are shown on each platform below. Notice the final color - Accent - is a blue-ish color for iOS and Android; this value is defined by Xamarin.Forms.",
                                  "This article introduces the various ways the Color class can be used in Xamarin.Forms." };

            for (int i = 0; i < 20; i++)
            {
                notification = new Notification();
                notification.Id = Guid.NewGuid();                
                notification.Subject = subjects[random.Next(0, subjects.Length - 1)];
                notification.Message = messages[random.Next(0, messages.Length - 1)];
                notification.SenderName = senders[random.Next(0, senders.Length - 1)];
                notification.UserImage = Notification.getDefaultUserImage();
                notification.Date = new DateTime(random.Next(2016, 2018), random.Next(1, 12), random.Next(1, 28));
                list.Add(notification);
            }

            IsBusy = false;
            return list.OrderByDescending(x => x.Date).ToList();
        }

        public void markNotificationAsRead(Guid id)
        {
            NotificationList.FirstOrDefault(x => x.Id == id).IsRead = true;
        } 
        #endregion
    }
}