using ListViewComponent.Util;
using System;
using Xamarin.Forms;

namespace ListViewComponent.Model
{
    public class Notification : BaseObservable
    {
        #region Properties
        private Guid id;

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        private string subject;

        public string Subject
        {
            get { return subject; }
            set
            {
                subject = value;
                OnPropertyChange();
            }
        }

        private string message;

        public string Message
        {
            get { return message; }
            set { message = value;
                OnPropertyChange();
            }
        }

        private string senderName;

        public string SenderName
        {
            get { return senderName; }
            set { senderName = value;
                OnPropertyChange();
            }
        }

        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value;
                OnPropertyChange();
            }
        }

        private ImageSource userImage;

        public ImageSource UserImage
        {
            get { return userImage; }
            set { userImage = value;
                OnPropertyChange();
            }
        }

        private bool isRead;

        public bool IsRead
        {
            get { return isRead; }
            set { isRead = value;
                OnPropertyChange();
            }
        }

        #endregion

        #region Methods
        public static ImageSource getDefaultUserImage()
        {
            return UriImageSource.FromUri(new Uri("http://icons.iconarchive.com/icons/custom-icon-design/pretty-office-2/72/man-icon.png"));
        } 
        #endregion
    }
}