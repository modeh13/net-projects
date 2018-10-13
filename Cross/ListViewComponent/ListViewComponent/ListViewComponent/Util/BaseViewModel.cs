namespace ListViewComponent.Util
{
    public abstract class BaseViewModel : BaseObservable
    {
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value;
                OnPropertyChange();
            }
        }
    }
}