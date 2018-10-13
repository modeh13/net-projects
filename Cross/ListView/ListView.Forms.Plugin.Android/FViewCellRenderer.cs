using System.ComponentModel;

using Android.Content;
using Android.Views;
using AView = Android.Views.View;
using ListView.Forms.Plugin.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ListView.Forms.Plugin.Android;
using Android.Runtime;

[assembly: ExportRenderer(typeof(FViewCell), typeof(FViewCellRenderer))]
namespace ListView.Forms.Plugin.Android
{
    [Preserve(AllMembers = true)]
    public class FViewCellRenderer : ViewCellRenderer
    {
        private AView view;

        protected override AView GetCellCore(Cell item, AView convertView, ViewGroup parent, Context context)
        {
            view = base.GetCellCore(item, convertView, parent, context);
            view.SetBackground(context.GetDrawable("listViewSelector"));

            return view;
        }

        protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnCellPropertyChanged(sender, args);            
        }
    }
}