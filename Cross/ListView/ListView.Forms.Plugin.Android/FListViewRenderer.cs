using System;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using NListView = Android.Widget.ListView;
using Android.Widget;
using XFListView = Xamarin.Forms.ListView;
using Xamarin.Forms.Platform.Android;
using ListView.Forms.Plugin.Abstractions;
using ListView.Forms.Plugin.Android;
using ListView.Forms.Plugin.Android.Classes;
using Android.App;
using Android.Graphics.Drawables;

[assembly: Xamarin.Forms.ExportRenderer(typeof(FListView), typeof(FListViewRenderer))]
namespace ListView.Forms.Plugin.Android
{
    [Preserve(AllMembers = true)]
    public class FListViewRenderer : ListViewRenderer
    {
        private NListView nativeListView;
        private FListView fListView;
        private ActionMode ActionMode;
        
        public FListViewRenderer(Context context) : base(context)
        {

        }

        public async static void Init()
        {
            var temp = DateTime.Now;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<XFListView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // unsubscribe

                //Only enable hardware accelleration on lollipop
                if ((int)Build.VERSION.SdkInt < 21)
                {
                    SetLayerType(LayerType.Software, null);
                }
            }

            if (e.NewElement != null)
            {
                // Configure the native control and subscribe to event handlers
                fListView = Element as FListView;
                nativeListView = Control as NListView;

                if (fListView.MultiSelect)
                {
                    //1º
                    //nativeListView.ChoiceMode = ChoiceMode.MultipleModal;
                    //nativeListView.SetMultiChoiceModeListener(new MultiChoiceModeListener(Context, nativeListView, fListView));

                    //2º
                    nativeListView.ItemClick += NativeListView_ItemClick;
                    nativeListView.ItemLongClick += NativeListView_ItemLongClick;
                    fListView.OnUpdateActionModeDroid += FListView_OnUpdateActionModeDroid;                    
                    //ActionMode = new ActionMode();
                    //((Activity)Context).StartActionMode(new ActionModeCustom(Context, fListView));

                    Toast.MakeText(Context, "Multiselect ListView activated !", ToastLength.Short).Show();
                }
            }
        }

        private void FListView_OnUpdateActionModeDroid(object sender, EventArgs e)
        {
            UpdateActionModeTitle();
        }

        private ActionModeCustom CreateActionMode()
        {
            ActionModeCustom actionModeCustom = new ActionModeCustom(Context, fListView);
            actionModeCustom.OnDestroyActionModeParent += (sender, args) =>
            {
                if (ActionMode != null)
                {
                    ActionMode = null;
                }
            };

            return actionModeCustom;
        }

        public void UpdateActionModeTitle()
        {
            if (fListView.GetItemsSelectedCount() > 0)
            {
                if (ActionMode == null)
                {
                    ActionMode = ((Activity)Context).StartActionMode(CreateActionMode());
                }
            }

            if (ActionMode != null)
            {
                if (fListView.GetItemsSelectedCount() > 0)
                {
                    ActionMode.Title = $"{fListView.GetItemsSelectedCount()} seleccionados";
                }
                else
                {
                    ActionMode.Finish();
                }
            }
        }

        private void NativeListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (fListView.GetItemsSelectedCount() > 0)
            {
                fListView.ToggleItemSelection(e.Position - 1);
                //nativeListView.SetItemChecked(e.Position, fListView.IsSelectedItem(e.Position - 1));
            }
            else {
                //Call Event Xamarin Forms
            }

            UpdateActionModeTitle();
        }

        private void NativeListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            if (fListView.GetItemsSelectedCount() > 0)
            {
                if (ActionMode == null)
                {
                    ActionMode = ((Activity)Context).StartActionMode(CreateActionMode());
                }
            }
            else
            {
                ActionMode = ((Activity)Context).StartActionMode(CreateActionMode());
                fListView.MarkItemAsSelected(e.Position - 1, true);
                //nativeListView.SetItemChecked(e.Position, true);
            }

            UpdateActionModeTitle();           
        }
    }

    public class ActionModeCustom : Java.Lang.Object, ActionMode.ICallback
    {
        FListView FListView;
        Context Context;
        public event EventHandler<EventArgs> OnDestroyActionModeParent;

        public ActionModeCustom(Context context, FListView fListView)
        {
            Context = context;
            FListView = fListView;
        }

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
            FListView.ClearSelection();
            OnDestroyActionModeParent?.Invoke(this, EventArgs.Empty);
        }

        public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        {
            return false;
        }
    }
}