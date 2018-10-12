using System;

namespace NotificationList.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string SenderName { get; set; }        
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public bool IsSelected { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;
            if (((Notification)obj).Id == Id) return true;

            return base.Equals(obj);
        }
    }
}