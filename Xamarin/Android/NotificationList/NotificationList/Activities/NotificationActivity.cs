using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using NotificationList.Adapters;
using static Android.Content.Res.Resources;
using LNotication = NotificationList.Models.Notification;

namespace NotificationList.Activities
{
    [Activity(Label = "NotificationActivity")]
    public class NotificationActivity : Activity
    {
        NotificationAdapter adapterLvw;
        ListView lvwNotification;
        Toolbar toolbar;
        bool inDeleteToolbar = false;
        int itemsSelected = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Title = "Notificaciones";            

            // Create your application here
            SetContentView(Resource.Layout.NotificationLayout);
            createActionBar();

            lvwNotification = FindViewById<ListView>(Resource.Id.lvwNotification);
            lvwNotification.Adapter = adapterLvw = new NotificationAdapter(this, getNotificationList());
            lvwNotification.ItemLongClick += LvwNotification_ItemLongClick;
            lvwNotification.ItemClick += LvwNotification_ItemClick;

            Toast.MakeText(this, "Notifications loaded", ToastLength.Short).Show();            
        }

        private void createActionBar()
        {            
            var colorPrimary = new TypedValue();
            Theme.ResolveAttribute(Android.Resource.Attribute.ColorPrimary, colorPrimary, true);
            
            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);            
            toolbar.SetBackgroundColor(new Android.Graphics.Color(colorPrimary.Data));
            toolbar.SetTitleTextColor(Android.Graphics.Color.White);
            toolbar.MenuItemClick -= EditToolbar_MenuItemClick;
            SetActionBar(toolbar);
            ActionBar.Title = Title;
            ActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        private void clearItemsSelected()
        {
            List<LNotication> list = adapterLvw.List.Where(x => x.IsSelected = true).ToList();
            View view;
            foreach (LNotication notification in list)
            {
                view = getViewByPosition(adapterLvw.GetPosition(notification), lvwNotification);
                view.SetBackgroundColor(Android.Graphics.Color.White);
                notification.IsSelected = false;
            }
            adapterLvw.NotifyDataSetChanged();
        }

        public View getViewByPosition(int pos, ListView listView)
        {
            int firstListItemPosition = listView.FirstVisiblePosition;
            int lastListItemPosition = firstListItemPosition + listView.ChildCount - 1;

            if (pos < firstListItemPosition || pos > lastListItemPosition)
            {
                return listView.Adapter.GetView(pos, null, listView);
            }
            else
            {
                int childIndex = pos - firstListItemPosition;
                return listView.GetChildAt(childIndex);
            }
        }

        private void LvwNotification_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {            
            if (inDeleteToolbar)
            {
                return;
            }
            else
            {
                LNotication notification = adapterLvw[e.Position];

                Log.Info("LongPress", $"{notification.Id} - {notification.IsSelected}");

                notification.IsSelected = true;
                e.View.SetBackgroundColor(Android.Graphics.Color.LightBlue);                
                itemsSelected++;

                toolbar.SetBackgroundColor(Android.Graphics.Color.DarkGray);
                toolbar.InflateMenu(Resource.Menu.NotificationMenu);
                //toolbar.MenuItemClick -= EditToolbar_MenuItemClick;
                toolbar.MenuItemClick += EditToolbar_MenuItemClick;
                toolbar.Title = $"{itemsSelected} seleccionado";
                inDeleteToolbar = true;
                adapterLvw.NotifyDataSetChanged();
            }
        }

        private void LvwNotification_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {            
            LNotication notification = adapterLvw[e.Position];
            
            if (inDeleteToolbar)
            {
                Log.Info("Click", $"{notification.Id} - {notification.IsSelected}");
                View view = getViewByPosition(e.Position, lvwNotification);

                if (notification.IsSelected)
                {
                    notification.IsSelected = false;
                    itemsSelected--;
                    e.View.SetBackgroundColor(Android.Graphics.Color.White);                    
                    //view.SetBackgroundColor(Android.Graphics.Color.White);
                }
                else {                    
                    notification.IsSelected = true;
                    itemsSelected++;
                    e.View.SetBackgroundColor(Android.Graphics.Color.LightBlue);
                    //view.SetBackgroundColor(Android.Graphics.Color.LightBlue);
                }

                if (itemsSelected == 0)
                {
                    inDeleteToolbar = false;                    
                    createActionBar();
                }
                else {
                    toolbar.Title = $"{itemsSelected} seleccionado";
                }
                adapterLvw.NotifyDataSetChanged();
            }
        }

        public override void OnBackPressed()
        {
            if (inDeleteToolbar)
            {
                createActionBar();
                itemsSelected = 0;
                inDeleteToolbar = false;
                clearItemsSelected();
            }
            else {                
                base.OnBackPressed();
            }
        }

        private void EditToolbar_MenuItemClick(object sender, Toolbar.MenuItemClickEventArgs e)
        {
            Toast.MakeText(this, "Bottom toolbar tapped: " + e.Item.TitleFormatted, ToastLength.Short).Show();
        }

        protected List<LNotication> getNotificationList()
        {
            List<LNotication> list = new List<LNotication>();
            LNotication notification;
            Random random = new Random();

            string[] senders = { "Wilmar Ortiz", "Héctor García", "Germán A. Ramírez", "Aderson Rangel", "Mario Moreno" };
            string[] subjects = { "Mint AVA Request", "Dev-Trunk Test", "PWC orgchart", "Outsourching Test", "Mayasoft Conference" };
            string[] messages = { "On Windows Phone, this is the complementary color chosen by the user. Good Windows Phone applications use this as part of their styling to provide a native look and feel.",
                                  "On iOS and Android this instance is set to a contrasting color that is visible on the default background but is not the same as the default text color.",
                                  "These colors are shown on each platform below. Notice the final color - Accent - is a blue-ish color for iOS and Android; this value is defined by Xamarin.Forms.",
                                  "This article introduces the various ways the Color class can be used in Xamarin.Forms." };

            for (int i = 0; i < 20; i++)
            {
                notification = new LNotication();
                notification.Id = random.Next(1, 999);
                notification.Subject = subjects[random.Next(0, subjects.Length - 1)];
                notification.Message = messages[random.Next(0, messages.Length - 1)];
                notification.SenderName = senders[random.Next(0, senders.Length - 1)];                
                notification.Date = new DateTime(random.Next(2016, 2018), random.Next(1, 12), random.Next(1, 28));
                notification.IsSelected = false;
                list.Add(notification);
            }

            return list;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    if (inDeleteToolbar)
                    {
                        createActionBar();
                        itemsSelected = 0;
                        inDeleteToolbar = false;
                        clearItemsSelected();
                        return true;
                    }
                    else {
                        Finish();
                        return true;
                    }                    
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }        
    }
}