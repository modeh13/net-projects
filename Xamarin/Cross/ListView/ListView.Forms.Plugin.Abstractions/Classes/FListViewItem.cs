namespace ListView.Forms.Plugin.Abstractions.Classes
{
    public class FListViewItem
    {
        public int Index { get; set; }
        public object Item { get; set; }

        public FListViewItem() { }

        public FListViewItem(int index, object item) {
            Index = index;
            Item = item;
        }
    }
}