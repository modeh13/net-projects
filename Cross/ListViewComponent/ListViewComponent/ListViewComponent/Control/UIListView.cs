using ListViewComponent.Util;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ListViewComponent.Control
{
    public class ItemLongClickEventArgs : EventArgs
    {
        public int Position { get; set; }

        public ItemLongClickEventArgs() {}
        public ItemLongClickEventArgs(int position) {
            Position = position;
        }
    }

    public class UIListView : ListView
    {
        #region Handlers Definition
        public event EventHandler<ItemLongClickEventArgs> ItemLongPress;

        public void RaiseItemLongPressEvent(int position)
        {
            if (IsEnabled)
                ItemLongPress?.Invoke(this, new ItemLongClickEventArgs(position));
        }

        public event EventHandler<ItemTappedEventArgs> ItemTapped;

        public void RaiseItemTapped()
        {
            
            //ItemTapped?.Invoke()
            //if (IsEnabled)
            //Ite
                //ItemTapped(this, ItemTappedEventArgs.Empty);
        }

        public virtual int ItemsCount()
        {
            return 0;
        }

        public virtual void MarkItemAsSelected(int position)
        {
        }

        public virtual int ItemsSelectedCount()
        {
            return 0;
        }

        public UIListView()
        {            
        }
        #endregion
    }    

    public class UIListView<T> : UIListView
    {
        public bool IsSelectionMode
        {
            get {
                if (Items.Any())
                {
                    if (Items.Any(x => x.IsSelected))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        #region BindablesProperties        
        public new static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), 
                                                                                                  typeof(ItemsView<Cell>), null, propertyChanged: OnItemsSourceChanged);

        public new IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        #endregion

        private static IEnumerable<UIListViewItem<T>> Items;
      
        static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            Items = GetItemSource((IEnumerable<T>)newValue);
            ((ListView)bindable).ItemsSource = Items;            
        }

        public override void MarkItemAsSelected(int position)
        {
            base.MarkItemAsSelected(position);

            if (Items.Any())
            {
                UIListViewItem<T> item = Items.ElementAt(position);
                item.IsSelected = !item.IsSelected;
            }
        }

        public override int ItemsSelectedCount()
        {
            if (Items.Any(x => x.IsSelected))
            {
                return Items.Count(x => x.IsSelected);
            }
            else {
                return base.ItemsSelectedCount();
            }
        }

        public override int ItemsCount()
        {
            if (Items.Any())
            {
                return Items.Count();
            }

            return base.ItemsCount();
        }

        public UIListViewItem<T> GetItemByPosition(int position)
        {
            return Items.ElementAt(position);
        }

        public void ClearSelection()
        {
            if (Items?.Any(x => x.IsSelected) ?? false)
            {
                Items.Where(x => x.IsSelected).ToList().ForEach(y => y.IsSelected = false);
            }
        }

        private static IEnumerable<UIListViewItem<T>> GetItemSource(IEnumerable<T> list)
        {            
            List<UIListViewItem<T>> items = new List<UIListViewItem<T>>();

            foreach (T item in list)
            {   
                items.Add(new UIListViewItem<T>(false, item));
            }

            return items;
        }
        public UIListView()
        {
            
        }
    }

    public class UIListViewItem<TEntity> : BaseObservable
    {
        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value;
                OnPropertyChange();
            }
        }

        private TEntity item;

        public TEntity Item
        {
            get { return item; }
            set
            {
                item = value;
                OnPropertyChange();
            }
        }

        public UIListViewItem()
        {

        }

        public UIListViewItem(bool isSelected, TEntity entity)
        {
            IsSelected = isSelected;
            Item = entity;
        }
    }
}