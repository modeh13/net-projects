// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace FileExplorer
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem btnBack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView fileFolderTableView { get; set; }

        [Action ("BtnBack_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnBack_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnBack != null) {
                btnBack.Dispose ();
                btnBack = null;
            }

            if (fileFolderTableView != null) {
                fileFolderTableView.Dispose ();
                fileFolderTableView = null;
            }
        }
    }
}