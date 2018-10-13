using System;

namespace ListView.Forms.Plugin.Abstractions.Classes
{
    public class FItemLongPressArgs : EventArgs
    {
        public new static readonly FItemLongPressArgs Empty = new FItemLongPressArgs();

        public FListViewItem Row { get; set; }

        public FItemLongPressArgs() { }

        public FItemLongPressArgs(int index, object item) {
            Row = new FListViewItem(index, item);
        }
    }
}