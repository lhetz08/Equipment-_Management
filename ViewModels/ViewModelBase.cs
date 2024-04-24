using Caliburn.Micro;
using System.ComponentModel;

namespace Equipment__Management.ViewModels
{
    public class ViewModelBase : Screen, INotifyPropertyChanged, IClose
    {
        
        public event PropertyChangedEventHandler? PropertyChanged;

        public ViewModelBase()
        {            
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
