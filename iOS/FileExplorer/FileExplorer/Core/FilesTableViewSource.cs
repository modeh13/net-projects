using System;
using System.Collections.Generic;
using UIKit;
using System.Linq;
using Foundation;
using System.IO;

namespace FileExplorer.Core
{
    public class FilesTableViewSource : UITableViewSource
    {
        private List<FileFolder> Source;
        protected ViewController Owner;

        public FilesTableViewSource(IEnumerable<FileFolder> list, ViewController owner)
        {
            Source = list.ToList();
            Owner = owner;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("cell_id", indexPath) as FileFolderCell;
            FileFolder fileFolder = Source.ElementAt(indexPath.Row);
            cell.UpdateCell(fileFolder);

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Source.Count();
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //base.RowSelected(tableView, indexPath);

            //UIAlertController okAlertController = UIAlertController.Create("Row Selected", Source.ElementAt(indexPath.Row), UIAlertControllerStyle.Alert);
            //okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            //owner.PresentViewController(okAlertController, true, null);

            FileFolder fileFolder = Source.ElementAt(indexPath.Row);

            if (fileFolder.Type == Type.Folder) {
                Owner.Folders.Push(fileFolder.FullName);
                Source = Util.GetFileFolderList(fileFolder.FullName).ToList();
                tableView.ReloadData();
                Owner.ValidateBackButton();
            }

            tableView.DeselectRow(indexPath, true);
        }

        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableViewCellEditingStyle.Delete; // this example doesn't support Insert
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:
                    // remove the item from the underlying data source
                    //FileFolder fileFolder = Source.ToList().ElementAt(indexPath.Row);

                    //if (fileFolder.Type == Type.File) {
                    //    File.Delete(fileFolder.FullName);
                    //}
                    //if (fileFolder.Type == Type.Folder) {
                    //    Directory.Delete(fileFolder.FullName, true);

                    //}
                    Source.RemoveAt(indexPath.Row);
                    //tableView.BeginUpdates();
                    tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
                    //tableView.EndUpdates();
                    break;
                case UITableViewCellEditingStyle.None:
                    Console.WriteLine("CommitEditingStyle:None called");
                    break;
            }
        }
        public override bool CanEditRow(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            return true;
        }
        public override string TitleForDeleteConfirmation(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            return "Delete";
        }
    }
}