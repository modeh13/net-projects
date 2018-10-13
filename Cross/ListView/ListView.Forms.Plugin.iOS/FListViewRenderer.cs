using CoreGraphics;
using Foundation;
using ListView.Forms.Plugin.Abstractions;
using ListView.Forms.Plugin.iOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FListView), typeof(FListViewRenderer))]
namespace ListView.Forms.Plugin.iOS
{
    [Preserve(AllMembers = true)]
    public class FListViewRenderer : ListViewRenderer
    {
        UITableView uiTableView;
        FListView fListView;
        UILongPressGestureRecognizer longPressGesture;
        static UIApplicationDelegate mainDelegate;

        public async static void Init(UIApplicationDelegate applicationDelegate)
        {
            var temp = DateTime.Now;
            //mainDelegate = applicationDelegate.;
            //mainDelegate.Window.ont
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe
            }

            if (e.NewElement != null)
            {   
                fListView = e.NewElement as FListView;
                uiTableView = Control;
                
                //Control.AllowsSelection = false;
                uiTableView.AllowsMultipleSelection = true;
                uiTableView.Delegate = new FListViewDelegate(fListView);

                longPressGesture = new UILongPressGestureRecognizer(ItemLongPress);

                var obj = this;
                var d = ViewController;
                string s = string.Empty;                
            }
        }
        

        protected void ItemLongPress(UILongPressGestureRecognizer gestureRecognizer)
        {
            if (gestureRecognizer.State == UIGestureRecognizerState.Began)
            {
                if (fListView.GetItemsSelectedCount() == 0)
                {
                    CGPoint point = gestureRecognizer.LocationInView(uiTableView);
                    NSIndexPath indexPath = uiTableView.IndexPathForRowAtPoint(point);

                    if (indexPath != null)
                    {
                        //fListView.MarkItemAsSelected(indexPath.Row, true);
                        uiTableView.Source.RowSelected(uiTableView, indexPath);
                        //uiTableView.SelectRow(indexPath, true, UITableViewScrollPosition.None);

                        ViewController.Title = "Probando";                        
                        ViewController.NavigationItem.Title = "Probando";
                        UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes
                        {
                            ForegroundColor = UIColor.Red
                        };
                    }
                }                
            }
        }
    }

    public class FListViewDelegate : UITableViewDelegate
    {
        FListView FListView;

        public FListViewDelegate() { }

        public FListViewDelegate(FListView fListView)
        {
            FListView = fListView;
        }

        public override NSIndexPath GetIndexPathForPreferredFocusedView(UITableView tableView)
        {
            return base.GetIndexPathForPreferredFocusedView(tableView);
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //base.RowSelected(tableView, indexPath);
            FListView.MarkItemAsSelected(indexPath.Row, true);
        }

        public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
        {
            //base.RowDeselected(tableView, indexPath);
            FListView.MarkItemAsSelected(indexPath.Row, false);
        }
    }
}