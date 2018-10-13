using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DemoListView.Models;

namespace DemoListView.Adapters
{
    public class EmployeeAdapter : BaseAdapter<Employee>
    {
        //Attributes
        private Employee[] data;

        //Constructors

        public EmployeeAdapter(Employee[] data)
        {
            this.data = data;
        }

        //Methods
        public override Employee this[int position] => data[position];

        public override int Count => data?.Count() ?? 0;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View view = inflater.Inflate(Resource.Layout.EmployeeItem, parent, false);

            TextView lblName = view.FindViewById<TextView>(Resource.Id.lblName);
            TextView lblEmail = view.FindViewById<TextView>(Resource.Id.lblEmail);
            Employee employee = data[position];

            lblName.Text = employee.Name;
            lblEmail.Text = employee.Email;

            return view;
        }
    }
}