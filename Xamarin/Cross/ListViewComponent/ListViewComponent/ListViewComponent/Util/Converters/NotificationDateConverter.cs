using System;
using System.Globalization;
using Xamarin.Forms;

namespace ListViewComponent.Util.Converters
{
    public class NotificationDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string convertValue = string.Empty;

            if (value == null)
                return convertValue;

            var date = (DateTime)value;

            if (date.Year < DateTime.Now.Year) {
                return convertValue = date.ToString("yyyy/MM/dd");
            }

            if (date.Year == DateTime.Now.Year) {
                if (date.Month == DateTime.Now.Month && date.Day == DateTime.Now.Day)
                {
                    return convertValue = date.ToString("hh:mm tt");
                }
                else {
                    return convertValue = date.ToString("dd MMM");
                }
            }

            return convertValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
