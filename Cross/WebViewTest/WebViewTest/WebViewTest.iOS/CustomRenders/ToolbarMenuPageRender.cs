using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using WebViewTest;
using WebViewTest.iOS.CustomRenders;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ToolbarMenuPage), typeof(ToolbarMenuPageRender))]
namespace WebViewTest.iOS.CustomRenders
{
    public class ToolbarMenuPageRender : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }           
        }
    }
}