using FileExplorer.Core;
using System;
using System.Collections.Generic;
using System.IO;
using UIKit;

namespace FileExplorer
{
    public partial class ViewController : UIViewController
    {
        public string CurrentPath = string.Empty;
        public string ParentFolder = string.Empty;
        public Stack<string> Folders;
        protected IEnumerable<FileFolder> fileFolderList;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
            Folders = new Stack<string>();
            Folders.Push(Util.GetFolderPath());            
            ValidateBackButton();

            //var d = new DirectoryInfo("/Users/compilacion/Library/Developer/CoreSimulator/Devices/FA2D5747-6D4D-4EF8-A8DE-3F8E4CDDCDAB/data/Containers/Data/Application/");
            //var s = d.GetDirectories();

            //string folderAssests = Path.Combine(Util.GetFolderPath(), "11.0");
            //Directory.CreateDirectory(folderAssests);
            //File.WriteAllText(Path.Combine(folderAssests, "test.txt"), "Probando");

            fileFolderList = Util.GetFileFolderList(Folders.Peek());            
            ///fileFolderTableView.Delegate = new FileFolderDelegate();
            fileFolderTableView.RowHeight = UITableView.AutomaticDimension;
            fileFolderTableView.EstimatedRowHeight = 80F;
            fileFolderTableView.Source = new FilesTableViewSource(fileFolderList, this);
            //fileFolderTableView.ReloadData();
        }

        public void ValidateBackButton()
        {
            btnBack.Enabled = !Folders.Peek().Equals(Util.GetFolderPath());
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void BtnBack_Activated(UIBarButtonItem sender)
        {
            if (Folders.Count > 1)
            {
                Folders.Pop();
                fileFolderList = Util.GetFileFolderList(Folders.Peek());
                fileFolderTableView.Source = new FilesTableViewSource(fileFolderList, this);
                fileFolderTableView.ReloadData();
            }

            ValidateBackButton();
        }
    }    
}