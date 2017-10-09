using PropertyChanged;
using System.ComponentModel;

namespace WpfPractice
{
    [AddINotifyPropertyChangedInterface]

    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        
    }
}
