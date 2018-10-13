using ListView.Forms.Plugin.Abstractions.Classes;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;
using XFListView = Xamarin.Forms.ListView;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System;

namespace ListView.Forms.Plugin.Abstractions
{
    public class FListView : XFListView
    {      
        #region Members
        //public event EventHandler<FItemLongPressArgs> ItemLongPress;
        public event EventHandler OnUpdateActionModeDroid;

        private static IEnumerable<object> Items;
        #endregion

        #region Properties
        private List<FListViewItem> itemsSelected;
        public List<FListViewItem> ItemsSelected
        {
            get { return itemsSelected; }
            set { itemsSelected = value; }
        }

        private ObservableCollection<MenuItem> editActions;
        public IList<MenuItem> EditActions
        {
            get
            {
                if (editActions == null)
                {
                    editActions = new ObservableCollection<MenuItem>();
                    editActions.CollectionChanged += OnContextActionsChanged;
                }

                return editActions;
            }
        }

        public bool HasEditActions
        {
            get { return editActions != null && editActions.Count > 0 && IsEnabled; }
        }
        #endregion

        #region BindablesProperties        
        public new static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable),
                                                                                                  typeof(ItemsView<Cell>), null, propertyChanged: OnItemsSourceChanged);

        public new IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty MultiSelectProperty = BindableProperty.Create(nameof(MultiSelect), typeof(bool), typeof(FListView), false);

        public bool MultiSelect
        {
            get { return (bool)GetValue(MultiSelectProperty); }
            set { SetValue(MultiSelectProperty, value); }
        }
        #endregion

        #region Constructors
        public FListView()
        {
            ItemsSelected = new List<FListViewItem>();
        }
        #endregion

        #region Events
        public void RaiseItemLongPressEvent(int index)
        {
            if (IsEnabled)
            {
                object item = GetItemByIndex(index);
                //ItemLongPress?.Invoke(this, new FItemLongPressArgs(index, item));
            }
        }

        public void UpdateActionModeDroid()
        {
            OnUpdateActionModeDroid?.Invoke(this, EventArgs.Empty);
        }

        public void RaiseItemTappedEvent(int index)
        {
            if (IsEnabled)
            {
                object item = GetItemByIndex(index);
                //ItemTapped?.Invoke(this, new ItemTappedEventArgs(null, item));
            }
        }

        static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            Items = (IEnumerable<object>)newValue;
            ((XFListView)bindable).ItemsSource = Items;
        }
        #endregion

        #region Methods
        void OnContextActionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            for (var i = 0; i < editActions.Count; i++)
                SetInheritedBindingContext(editActions[i], BindingContext);

            OnPropertyChanged(nameof(HasEditActions));
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (HasEditActions)
            {
                for (var i = 0; i < editActions.Count; i++)
                    SetInheritedBindingContext(editActions[i], BindingContext);
            }
        }

        private object GetItemByIndex(int index)
        {
            if (Items.Any())
            {
                return Items.ElementAt(index);
            }
            return null;
        }

        public void MarkItemAsSelected(int index, bool @checked)
        {
            if (@checked)
            {
                object item = GetItemByIndex(index);

                if (item != null)
                {
                    ItemsSelected.Add(new FListViewItem(index, item));
                }
            }
            else {
                if (ItemsSelected.Any(x => x.Index == index))
                {
                    ItemsSelected.Remove(ItemsSelected.First(x => x.Index == index));
                }
            }            
        }

        public void ToggleItemSelection(int index)
        {            
            MarkItemAsSelected(index, !IsSelectedItem(index));
        }

        public int GetItemsSelectedCount()
        {
            return ItemsSelected.Count();
        }

        public bool IsSelectedItem(int position)
        {
            bool isSelected = false;

            if (ItemsSelected.Any())
            {
                isSelected = ItemsSelected.Any(x => x.Index == position);
            }

            return isSelected;
        }

        public void ClearSelection()
        {
            ItemsSelected.Clear();
        }
        #endregion
    }
}