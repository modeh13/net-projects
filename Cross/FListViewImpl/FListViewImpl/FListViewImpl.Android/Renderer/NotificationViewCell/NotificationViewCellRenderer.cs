using Android.Content;
using AView = Android.Views.View;
using AListView = Android.Widget.ListView;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using System.ComponentModel;
using XFNotificationViewCell = FListViewImpl.Custom.ViewCell.NotificationViewCell;
using FListViewImpl.Droid.Renderer.NotificationViewCell;
using ListView.Forms.Plugin.Abstractions;
using System.Linq;
using System.Collections;

[assembly: ExportRenderer(typeof(XFNotificationViewCell), typeof(NotificationViewCellRenderer))]
namespace FListViewImpl.Droid.Renderer.NotificationViewCell
{
    public class NotificationViewCellRenderer : ViewCellRenderer
    {
        NotificationViewCellDroid cell;

        protected override AView GetCellCore(Cell item, AView convertView, ViewGroup parent, Context context)
        {
            var xfNotificationCell = (XFNotificationViewCell) item;            
            AListView listView = parent as AListView;
            FListView fListView = item.Parent as FListView;

            cell = convertView as NotificationViewCellDroid;

            if (cell == null)
            {
                cell = new NotificationViewCellDroid(context, xfNotificationCell, fListView, listView);                
            }
            else
            {
                cell.XFNotificationCell.PropertyChanged -= OnNativeCellPropertyChanged;
                cell.Position = listView.GetPositionForView(cell);
            }

            xfNotificationCell.PropertyChanged += OnNativeCellPropertyChanged;

            cell.Position = (fListView.ItemsSource as IList).IndexOf(xfNotificationCell.Person);
            cell.UpdateCell(xfNotificationCell);

            return cell;
        }

        protected void OnNativeCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var xfNotificationCell = (XFNotificationViewCell) sender;
            if (e.PropertyName == XFNotificationViewCell.PersonProperty.PropertyName)
            {
                Person person = xfNotificationCell.Person;
                cell.UpdateCell(xfNotificationCell);                
            }
        }
    }
}