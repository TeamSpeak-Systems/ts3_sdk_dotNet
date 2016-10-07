using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Teamspeak.Sdk.Client.Example
{
    class LogAdapter: BaseAdapter<string>
    {
        private readonly Context Context;
        private readonly LayoutInflater LayoutInflater;
        private readonly List<string> Entries;

        public LogAdapter(Context context)
        {
            Context = context;
            LayoutInflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
            Entries = new List<string>();
        }

        public int Append(string line)
        {
            Entries.Add(line);
            NotifyDataSetChanged();
            return Entries.Count - 1;
        }

        public override string this[int position] { get { return Entries[position]; } }

        public override int Count { get { return Entries.Count; } }

        public override long GetItemId(int position) { return position; }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            TextView text1 = view.FindViewById<TextView>(Android.Resource.Id.Text1);
            text1.Text = Entries[position];
            return view;
        }
    }
}