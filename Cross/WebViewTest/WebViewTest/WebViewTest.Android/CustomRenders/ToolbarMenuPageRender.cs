using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WebViewTest;
using WebViewTest.Droid.CustomRenders;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ToolbarMenuPage), typeof(ToolbarMenuPageRender))]
namespace WebViewTest.Droid.CustomRenders
{    
    public class ToolbarMenuPageRender : PageRenderer, TextureView.ISurfaceTextureListener
    {
        public ToolbarMenuPageRender(Context context) : base(context)
        {
        }

        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            throw new NotImplementedException();
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            throw new NotImplementedException();
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
            throw new NotImplementedException();
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            throw new NotImplementedException();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            //try
            //{
            //    SetupUserInterface();
            //    SetupEventHandlers();
            //    AddView(view);
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine(@"           ERROR: ", ex.Message);
            //}
        }
    }
}