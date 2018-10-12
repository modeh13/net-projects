// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace EjercicioControles
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ivwVisor { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MapKit.MKMapView mvwMap { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl sgmSelector { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIWebView wvwVisorWeb { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ivwVisor != null) {
                ivwVisor.Dispose ();
                ivwVisor = null;
            }

            if (mvwMap != null) {
                mvwMap.Dispose ();
                mvwMap = null;
            }

            if (sgmSelector != null) {
                sgmSelector.Dispose ();
                sgmSelector = null;
            }

            if (wvwVisorWeb != null) {
                wvwVisorWeb.Dispose ();
                wvwVisorWeb = null;
            }
        }
    }
}