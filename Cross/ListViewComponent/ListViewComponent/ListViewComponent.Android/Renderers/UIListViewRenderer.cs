using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using NListView = Android.Widget.ListView;
using ListViewComponent.Control;
using ListViewComponent.Droid.Renderers;
using Xamarin.Forms;
using FListView = Xamarin.Forms.ListView;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using FNotification = ListViewComponent.Model.Notification;
using ListViewComponent.ViewModel;
using static Android.Views.View;

[assembly: ExportRenderer(typeof(UIListView), typeof(UIListViewRenderer))]
namespace ListViewComponent.Droid.Renderers
{

    public class UIListViewRenderer : ListViewRenderer
    {
        //Properties
        private string toolbarTitle;
        private Activity mainActivity;
        private Android.Support.V7.Widget.Toolbar mainToolbar;
        private UIListView uiListView;
        private NListView nativeListView;
        private NotificationsViewModel viewModel;

        //Contructors
        public UIListViewRenderer(Context context) : base(context)
        {
            mainActivity = (Activity)Context;
            mainToolbar = mainActivity.FindViewById<Android.Support.V7.Widget.Toolbar>(Droid.Resource.Id.toolbar);
            toolbarTitle = mainToolbar.Title;            

            //mainActivity.ba
        }
        
        //Methods
        protected override void OnElementChanged(ElementChangedEventArgs<FListView> e)
        {
            base.OnElementChanged(e);
            
            if (e.NewElement != null)
            {
                // Configure the native control and subscribe to event handlers
                uiListView = Element as UIListView;
                nativeListView = Control as NListView;
                viewModel = uiListView.BindingContext as NotificationsViewModel;
                nativeListView.ItemLongClick += nativeListView_OnItemLongClick;
                nativeListView.ChoiceMode = ChoiceMode.Multiple;
                nativeListView.ItemsCanFocus = false;
                //nativeListView.ItemClick += NativeListView_ItemClick;
                //nativeListView.SetMultiChoiceModeListener(new MultiChoiceModeListener());
            }
        }

        private bool IsSelectedMode() {
            return mainToolbar.Title != toolbarTitle;
        }

        private void NativeListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (IsSelectedMode())
            {
                uiListView.MarkItemAsSelected(e.Position - 1);
                //var d = (UIListViewItem<FNotification>) nativeListView.Adapter.GetItem(e.Position);

                if (uiListView.ItemsSelectedCount() == 0)
                {
                    createActionBar();
                }
                else {
                    mainToolbar.Title = $"{uiListView.ItemsSelectedCount()} seleccionado";
                }
            }
        }

        private void nativeListView_OnItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            uiListView.RaiseItemLongPressEvent(e.Position - 1);
            return;
            if (!IsSelectedMode())
            {
                // Code to execute on item long click                
                //uiListView.RaiseItemLongPressEvent();
                uiListView.MarkItemAsSelected(e.Position - 1);

                mainToolbar = mainActivity.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
                mainToolbar.SetBackgroundColor(Android.Graphics.Color.DarkGray);
                mainToolbar.InflateMenu(Resource.Menu.NotificationMenu);
                mainToolbar.SetNavigationOnClickListener(new OnNavigationClickToolbar(mainActivity));
                mainToolbar.MenuItemClick += Toolbar_MenuItemClick;
                //mainToolbar.NavigationClick += MainToolbar_NavigationClick;
                mainToolbar.Title = $"{uiListView.ItemsSelectedCount()} seleccionado";
            }
        }

        private void MainToolbar_NavigationClick(object sender, Android.Support.V7.Widget.Toolbar.NavigationClickEventArgs e)
        {
            string s = "";
        }

        private void createActionBar()
        {
            //var colorPrimary = new TypedValue();
            //Theme.ResolveAttribute(Android.Resource.Attribute.ColorPrimary, colorPrimary, true);            
            mainToolbar = mainActivity.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            //toolbar.SetBackgroundColor(new Android.Graphics.Color(colorPrimary.Data));            
            mainToolbar.SetTitleTextColor(Android.Graphics.Color.White);
            mainToolbar.MenuItemClick -= Toolbar_MenuItemClick;
            mainToolbar.Title = toolbarTitle;
            //mainToolbar.Menu.RemoveItem(0);
            mainToolbar.Menu.Clear();
        }

        private void Toolbar_MenuItemClick(object sender, Android.Support.V7.Widget.Toolbar.MenuItemClickEventArgs e)
        {
            Toast.MakeText(mainActivity, "Bottom toolbar tapped: " + e.Item.TitleFormatted, ToastLength.Short).Show();
        }

        

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (e.KeyCode == Keycode.Back) {


                return true;
            }

            return base.OnKeyDown(keyCode, e);
        }

        
    }

    public class MultiChoiceModeListener : Java.Lang.Object, AbsListView.IMultiChoiceModeListener
    {
        public IntPtr Handle => IntPtr.Zero;

        public void Dispose()
        {
            //Dispose();
        }

        public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
        {
            throw new NotImplementedException();
        }

        public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        {
            //throw new NotImplementedException();
            return true;
        }

        public void OnDestroyActionMode(ActionMode mode)
        {
            //throw new NotImplementedException();
        }

        public void OnItemCheckedStateChanged(ActionMode mode, int position, long id, bool @checked)
        {
            //throw new NotImplementedException();
        }

        public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        {
            //throw new NotImplementedException();
            return true;
        }
    }

    public class OnNavigationClickToolbar : Java.Lang.Object, IOnClickListener
    {
        //public IntPtr Handle => throw new NotImplementedException();

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}

        private Activity mainActivity;
        public OnNavigationClickToolbar(Activity activity)
        {
            mainActivity = activity;
        }

        public void OnClick(Android.Views.View v)
        {
            mainActivity.OnBackPressed();
        }
    }
}