using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ListViewComponent.Util
{
    public class BaseObservable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChange([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}