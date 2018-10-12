// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AccesoSQLiteServicios
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSaveSQLite { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSendService { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnSaveSQLite != null) {
                btnSaveSQLite.Dispose ();
                btnSaveSQLite = null;
            }

            if (btnSendService != null) {
                btnSendService.Dispose ();
                btnSendService = null;
            }
        }
    }
}