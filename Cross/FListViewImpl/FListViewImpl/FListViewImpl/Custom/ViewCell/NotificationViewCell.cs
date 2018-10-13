using Xamarin.Forms;

namespace FListViewImpl.Custom.ViewCell
{
    public class NotificationViewCell : Xamarin.Forms.ViewCell
    {
        public static readonly BindableProperty PersonProperty = BindableProperty.Create(nameof(Person), typeof(Person), typeof(NotificationViewCell), default(Person));

        public Person Person
        {
            get { return (Person)GetValue(PersonProperty); }
            set { SetValue(PersonProperty, value); }
        }
    }
}