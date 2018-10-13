using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebViewTest.Controls;
using WebViewTest.iOS.CustomRenders;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Threading.Tasks;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(WebViewRender))]
namespace WebViewTest.iOS.CustomRenders
{
    public class WebViewRender : WebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (NativeView != null && e.NewElement != null)
                InitializeCommands((CustomWebView)e.NewElement);

            var webView = e.NewElement as CustomWebView;
            if (webView != null)                
                webView.EvaluateJavascript = (js) =>
                {
                    return Task.FromResult(this.EvaluateJavascript(js));
                };
        }

        private void InitializeCommands(CustomWebView element)
        {
            element.Refresh = () =>
            {
                ((UIWebView)NativeView).Reload();
            };

            //element.GoBackCommand = new Command(() =>
            //{
            //    var control = ((UIWebView)NativeView);
            //    if (control.CanGoBack)
            //    {
            //        element.IsBackNavigating = true;
            //        control.GoBack();
            //    }
            //});

            element.CanGoBackFunction = () =>
            {
                return ((UIWebView)NativeView).CanGoBack;
            };

            var ctl = ((UIWebView)NativeView);

            ctl.ScalesPageToFit = true;
        }
    }
}