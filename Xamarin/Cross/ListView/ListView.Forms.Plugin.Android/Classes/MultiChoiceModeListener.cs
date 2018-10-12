using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using ListView.Forms.Plugin.Abstractions;
using Xamarin.Forms.Platform.Android;
using NListView = Android.Widget.ListView;

namespace ListView.Forms.Plugin.Android.Classes
{
    public class MultiChoiceModeListener : Java.Lang.Object, AbsListView.IMultiChoiceModeListener
    {
        #region Members
        private Context Context;
        private FListView FListView;
        private NListView NativeListView;
        #endregion

        #region Constructors
        public MultiChoiceModeListener()
        {

        }

        public MultiChoiceModeListener(Context context, NListView nativeListView, FListView fListView)
        {
            Context = context;
            FListView = fListView;
            NativeListView = nativeListView;
        }
        #endregion

        #region Methods Impl
        public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
        {
            mode.Menu.Clear();
            Xamarin.Forms.IMenuItemController action = FListView.EditActions[item.ItemId];
            action.Activate();
            mode?.Finish();
            return true;
        }

        public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        {
            // Inflate the menu for the CAB           
            for (int i = 0; i < FListView.EditActions.Count; i++)
            {
                Xamarin.Forms.MenuItem menuItem = FListView.EditActions[i];
                IMenuItem item = menu.Add(global::Android.Views.Menu.None, i, global::Android.Views.Menu.None, menuItem.Text);

                if (menuItem.Icon != null)
                {
                    Drawable iconDrawable = Context.GetDrawable(menuItem.Icon);
                    if (iconDrawable != null)
                        item.SetIcon(iconDrawable);
                }
            }

            return true;
        }

        public void OnDestroyActionMode(ActionMode mode)
        {
            // Here you can make any necessary updates to the activity when
            // the CAB is removed. By default, selected items are deselected/unchecked.            
            FListView.ClearSelection();
        }

        public void OnItemCheckedStateChanged(ActionMode mode, int position, long id, bool @checked)
        {
            // Here you can do something when items are selected/de-selected,
            // such as update the title in the CAB            
            FListView.MarkItemAsSelected(position - 1, @checked);

            if (FListView.GetItemsSelectedCount() > 0)
            {
                mode.Title = $"{FListView.GetItemsSelectedCount()} seleccionados";
            }
            else
            {
                mode.Finish();
            }
        }

        public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        {
            // Here you can perform updates to the CAB due to
            // an invalidate() request
            return false;
        } 
        #endregion
    }
}