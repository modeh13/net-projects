using System.Collections.Generic;
using System.Linq;

using Android.Content;
using Android.Views;
using Android.Widget;
using SQLiteDemo.Models;

namespace SQLiteDemo.Adapter
{
    public class EmployeeAdapter : BaseAdapter<Employee>
    {
        //Attributes
        protected Context Context;
        protected List<Employee> List;
        
        //Constructor
        public EmployeeAdapter(Context context, List<Employee> list)
        {
            Context = context;
            List = list;
        }

        //Methods
        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return List.ElementAt(position).Id;            
        }        

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return List?.Count ?? 0;
            }
        }

        public override Employee this[int position] => List.ElementAt(position);

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            EmployeeAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as EmployeeAdapterViewHolder;

            if (holder == null)
            {
                holder = new EmployeeAdapterViewHolder();

                var inflater = LayoutInflater.From(Context);
                view = inflater.Inflate(Resource.Layout.EmployeeItem, parent, false);
                holder.tvwName = view.FindViewById<TextView>(Resource.Id.tvwName);
                holder.tvwCharge = view.FindViewById<TextView>(Resource.Id.tvwCharge);
                holder.tvwEmail = view.FindViewById<TextView>(Resource.Id.tvwEmail);
                holder.ivwEmployee = view.FindViewById<ImageView>(Resource.Id.ivwEmployee);

                view.Tag = holder;
            }

            Employee employee = List.ElementAt(position);
            holder.tvwName.Text = employee.Name;
            holder.tvwCharge.Text = employee.Charge;
            holder.tvwEmail.Text = employee.Email;
            holder.ivwEmployee.SetImageResource(Resource.Drawable.user);

            return view;
        }
    }

    class EmployeeAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        public TextView tvwName { get; set; }
        public TextView tvwCharge { get; set; }
        public TextView tvwEmail { get; set; }
        public ImageView ivwEmployee { get; set; }
    }
}