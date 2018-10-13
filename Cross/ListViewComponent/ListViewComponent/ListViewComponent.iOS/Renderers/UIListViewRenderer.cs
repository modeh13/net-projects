using CoreGraphics;
using Foundation;
using ListViewComponent.Control;
using ListViewComponent.iOS.Renderers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(UIListView), typeof(UIListViewRenderer))]
namespace ListViewComponent.iOS.Renderers
{
    public class UIListViewRenderer : ListViewRenderer
    {
        UITableView uiTableView;
        UIListView uiListView;
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe
            }

            if (e.NewElement != null)
            {
                //Control.Source = new NativeiOSListViewSource(e.NewElement as UIListView);
                uiListView = e.NewElement as UIListView;
                uiTableView = Control;

                //Control.Delegate = new ListViewDelegate();
                //Control.AllowsSelection = false;
                uiTableView.AllowsMultipleSelection = true;
                
                //uiTableView.Source.
                //Control.Source = new NativeiOSListViewSource(e.NewElement as UIListView);


                var longPressGesture = new UILongPressGestureRecognizer(LongPressMethod);
                AddGestureRecognizer(longPressGesture);
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            //if (e.PropertyName == UIListView.ItemsProperty.PropertyName)
            //{
            //    Control.Source = new NativeiOSListViewSource(Element as UIListView);
            //}
        }

        protected void LongPressMethod(UILongPressGestureRecognizer gestureRecognizer)
        {
            Console.Write("LongPress");

            if (gestureRecognizer.State == UIGestureRecognizerState.Began)
            {
                CGPoint p = gestureRecognizer.LocationInView(uiTableView);
                NSIndexPath indexPath = uiTableView.IndexPathForRowAtPoint(p);

                if (indexPath != null)
                {
                    uiListView.RaiseItemLongPressEvent(indexPath.Row);
                    uiTableView.SelectRow(indexPath, true, UITableViewScrollPosition.None);
                }
            }
        }
    }

    public class ListViewDelegate : UITableViewDelegate
    {
        public override NSIndexPath GetIndexPathForPreferredFocusedView(UITableView tableView)
        {
            return base.GetIndexPathForPreferredFocusedView(tableView);
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //base.RowSelected(tableView, indexPath);
        }

        public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
        {
            //base.RowDeselected(tableView, indexPath);
        }
    }

    public class NativeiOSListViewCell : UITableViewCell
    {
        UILabel headingLabel, subheadingLabel;
        UIImageView imageView;

        public NativeiOSListViewCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
        {
            SelectionStyle = UITableViewCellSelectionStyle.Gray;

            ContentView.BackgroundColor = UIColor.FromRGB(218, 255, 127);

            imageView = new UIImageView();

            headingLabel = new UILabel()
            {
                Font = UIFont.FromName("Cochin-BoldItalic", 22f),
                TextColor = UIColor.FromRGB(127, 51, 0),
                BackgroundColor = UIColor.Clear
            };

            subheadingLabel = new UILabel()
            {
                Font = UIFont.FromName("AmericanTypewriter", 12f),
                TextColor = UIColor.FromRGB(38, 127, 0),
                TextAlignment = UITextAlignment.Center,
                BackgroundColor = UIColor.Clear
            };

            ContentView.Add(headingLabel);
            ContentView.Add(subheadingLabel);
            ContentView.Add(imageView);
        }

        public void UpdateCell(string caption, string subtitle, UIImage image)
        {
            headingLabel.Text = caption;
            subheadingLabel.Text = subtitle;
            imageView.Image = image;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            headingLabel.Frame = new CoreGraphics.CGRect(5, 4, ContentView.Bounds.Width - 63, 25);
            subheadingLabel.Frame = new CoreGraphics.CGRect(100, 18, 100, 20);
            imageView.Frame = new CoreGraphics.CGRect(ContentView.Bounds.Width - 63, 5, 33, 33);
        }
    }

    public class NativeiOSListViewSource : UITableViewSource
    {
        // declare vars
        IEnumerable tableItems;
        UIListView listView;
        readonly NSString cellIdentifier = new NSString("TableCell");

        public IEnumerable Items
        {
            //get{ }
            set
            {
                tableItems = value;
            }
        }

        public NativeiOSListViewSource(UIListView view)
        {
            tableItems = view.ItemsSource;
            listView = view;
        }

        /// <summary>
        /// Called by the TableView to determine how many cells to create for that particular section.
        /// </summary>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return listView.ItemsCount();
        }

        #region user interaction methods

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //listView.NotifyItemSelected(tableItems[indexPath.Row]);
            Console.WriteLine("Row " + indexPath.Row.ToString() + " selected");
            tableView.DeselectRow(indexPath, true);
        }

        public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
        {
            Console.WriteLine("Row " + indexPath.Row.ToString() + " deselected");
        }

        #endregion

        /// <summary>
        /// Called by the TableView to get the actual UITableViewCell to render for the particular section and row
        /// </summary>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // request a recycled cell to save memory
            NativeiOSListViewCell cell = tableView.DequeueReusableCell(cellIdentifier) as NativeiOSListViewCell;

            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                //cell = new NativeiOSListViewCell(cellIdentifier);
                //cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
            }

            //if (String.IsNullOrWhiteSpace(tableItems[indexPath.Row].ImageFilename))
            //{
            //    cell.UpdateCell(tableItems[indexPath.Row].Name
            //        , tableItems[indexPath.Row].Category
            //        , null);
            //}
            //else
            //{
            //    cell.UpdateCell(tableItems[indexPath.Row].Name
            //        , tableItems[indexPath.Row].Category
            //        , UIImage.FromFile("Images/" + tableItems[indexPath.Row].ImageFilename + ".jpg"));
            //}

            return cell;
        }
    }
}