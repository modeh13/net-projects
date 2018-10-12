using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;

namespace FileExplorer.Core
{
    public class FileFolderDelegate : UITableViewDelegate
    {
        public FileFolderDelegate()
        {
        }

        public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
        {
            var action = UITableViewRowAction.Create(UITableViewRowActionStyle.Default, "Eliminar",
                (arg1, arg2) =>
                {
                    //tableView.Source.
                    //dataSource.Objects.RemoveAt(0);
                    //tableView.ReloadData();

                    //tableView.BeginUpdates();
                    // remove our 'ADD NEW' row from the underlying data
                    //tableItems.RemoveAt((int)tableView.NumberOfRowsInSection(0) - 1); // zero based :)
                                                                                      // remove the row from the table display
                    //tableView.DeleteRows(new NSIndexPath[] { NSIndexPath.FromRowSection(tableView.NumberOfRowsInSection(0) - 1, 0) }, UITableViewRowAnimation.Fade);
                    //tableView.EndUpdates(); // applies the changes

                });

            //return base.EditActionsForRow(tableView, indexPath);
            return new UITableViewRowAction[] { action };
        }
    }
}