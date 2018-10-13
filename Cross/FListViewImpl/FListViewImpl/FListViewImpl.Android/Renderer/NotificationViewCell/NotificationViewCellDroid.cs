using System;
using System.Net;
using System.Threading.Tasks;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using AListView = Android.Widget.ListView;
using ListView.Forms.Plugin.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XFNotificationViewCell = FListViewImpl.Custom.ViewCell.NotificationViewCell;

namespace FListViewImpl.Droid.Renderer.NotificationViewCell
{
    internal class NotificationViewCellDroid : LinearLayout, INativeElementView
    {
        #region Properties
        public FListView FListView { get; set;} 
        public AListView AListView { get; set; }
        public XFNotificationViewCell XFNotificationCell { get; private set; }
        public Element Element => XFNotificationCell;
        public int Position { get; set; }

        public TextView TxtName { get; set; }
        public TextView TxtAge { get; set; }
        public ImageView ImgUser { get; set; }
        public ImageView ImgBack { get; set; }
        public FrameLayout FlyImages { get; set; }
        #endregion

        #region Constructors
        public NotificationViewCellDroid(Context context, XFNotificationViewCell cell, FListView fListView, AListView listView) : base(context)
        {
            XFNotificationCell = cell;
            FListView = fListView;
            AListView = listView;

            var view = (context as Activity).LayoutInflater.Inflate(Resource.Layout.NotificationItem, null);

            //Get views to each control ItemTemplate
            TxtName = view.FindViewById<TextView>(Resource.Id.txtName);
            TxtAge = view.FindViewById<TextView>(Resource.Id.txtAge);
            ImgUser = view.FindViewById<ImageView>(Resource.Id.imgUser);
            ImgBack = view.FindViewById<ImageView>(Resource.Id.imgBack);
            FlyImages = view.FindViewById<FrameLayout>(Resource.Id.flyImages);
            FlyImages.Click += FlyImages_Click; 

            float scale = (context as Activity).Resources.DisplayMetrics.Density;
            FlyImages.SetCameraDistance(8000 * scale);

            AddView(view);
        }

        private void FlyImages_Click(object sender, EventArgs e)
        {       
            bool isSelected = FListView.IsSelectedItem(Position);
            var setRightOut = (AnimatorSet)AnimatorInflater.LoadAnimator(Context, Resource.Animator.right_out);
            var setLeftIn = (AnimatorSet)AnimatorInflater.LoadAnimator(Context, Resource.Animator.left_in);

            if (!isSelected)
            {
                setRightOut.SetTarget(ImgUser);
                setLeftIn.SetTarget(ImgBack);
                setRightOut.Start();
                setLeftIn.Start();
                //this.SetBackgroundColor(Android.Graphics.Color.AliceBlue);
                FListView.MarkItemAsSelected(Position, true);                
            }
            else
            {
                setRightOut.SetTarget(ImgBack);
                setLeftIn.SetTarget(ImgUser);
                setRightOut.Start();
                setLeftIn.Start();
                //this.SetBackgroundColor(Android.Graphics.Color.White);
                FListView.MarkItemAsSelected(Position, false);
            }

            FListView.UpdateActionModeDroid();
        }
        #endregion

        #region Methods
        public void UpdateCell(XFNotificationViewCell cell)
        {
            Person person = cell.Person;
            TxtName.Text = person.Name;
            TxtAge.Text = person.Age.ToString();

            // Dispose of the old image
            if (ImgUser.Drawable != null)
            {
                using (var image = ImgUser.Drawable as BitmapDrawable)
                {
                    if (image != null)
                    {
                        if (image.Bitmap != null)
                        {
                            image.Bitmap.Dispose();
                        }
                    }
                }
            }
            
            if (FListView.IsSelectedItem(Position))
            {
                ImgUser.Alpha = 0;
                ImgBack.Alpha = 1;
                //this.SetBackgroundColor(Android.Graphics.Color.AliceBlue);
                //FlyImages_Click(FlyImages, EventArgs.Empty);
            }
            else
            {
                ImgUser.Alpha = 1;
                ImgBack.Alpha = 0;
                //this.SetBackgroundColor(Android.Graphics.Color.White);
                //FlyImages_Click(FlyImages, EventArgs.Empty);
            }

            SetImage(person.ImageUrl);
        }

        public async void SetImage(string urlImage)
        {
            if (!string.IsNullOrWhiteSpace(urlImage))
            {
                if (Uri.IsWellFormedUriString(urlImage, UriKind.Absolute))
                {
                    var imgBitmap = await GetImageBitmapFromUrl(urlImage);

                    if (imgBitmap != null)
                    {
                        ImgUser.SetImageBitmap(imgBitmap);
                    }
                    else
                    {
                        ImgUser.SetImageResource(Resource.Drawable.ic_action_account_circle);
                    }
                }
                else
                {
                    //Display new image
                    await Context.Resources.GetBitmapAsync(urlImage).ContinueWith((t) =>
                    {
                        var bitmap = t.Result;
                        if (bitmap != null)
                        {
                            ImgUser.SetImageBitmap(bitmap);
                            bitmap.Dispose();
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
            else
            {
                // Clear the image
                ImgUser.SetImageBitmap(null);
                ImgUser.SetImageResource(Resource.Drawable.ic_action_account_circle);
            }
        }

        private async Task<Bitmap> GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;
            if (!string.IsNullOrEmpty(url))
                try
                {
                    using (var webClient = new WebClient())
                    {
                        var imageBytes = await webClient.DownloadDataTaskAsync(url);
                        if (imageBytes != null && imageBytes.Length > 0)
                        {
                            imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                        }
                    }
                }
                catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                }

            return imageBitmap;
        }
        #endregion
    }
}