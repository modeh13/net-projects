using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NotificationList.Models;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace NotificationList.Adapters
{
    public class NotificationAdapter : BaseAdapter<Notification>
    {
        protected Context Context;
        public List<Notification> List;

        public NotificationAdapter(Context context, List<Notification> list)
        {
            this.Context = context;
            this.List = list;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return this.List.ElementAt(position).Id;
        }

        public int GetPosition(Notification obj)
        {
            return this.List.IndexOf(obj);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            NotificationAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as NotificationAdapterViewHolder;

            if (holder == null)
            {
                holder = new NotificationAdapterViewHolder();
                var inflater = Context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                //replace with your item and your holder items
                //comment back in
                view = inflater.Inflate(Resource.Layout.NotificationItem, parent, false);
                holder.SenderName = view.FindViewById<TextView>(Resource.Id.tvwSenderName);
                holder.Subject = view.FindViewById<TextView>(Resource.Id.tvwSubject);
                holder.Message = view.FindViewById<TextView>(Resource.Id.tvwMessage);
                view.Tag = holder;
            }

            //fill in your items
            Notification notification = this[position];
            holder.SenderName.Text = notification.SenderName;
            holder.Subject.Text = notification.Subject;
            holder.Message.Text = notification.Message;

            if (notification.IsSelected) view.SetBackgroundColor(Android.Graphics.Color.LightBlue);
            else view.SetBackgroundColor(Android.Graphics.Color.White);

            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return List?.Count() ?? 0;
            }
        }

        public override Notification this[int position] => List[position];
    }

    class NotificationAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        public TextView SenderName { get; set; }
        public TextView Subject { get; set; }
        public TextView Message { get; set; }
    }
}