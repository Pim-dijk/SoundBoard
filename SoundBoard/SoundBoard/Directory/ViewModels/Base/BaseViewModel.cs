using PropertyChanged;
using System;
using System.ComponentModel;

namespace SoundBoard
{
    [AddINotifyPropertyChangedInterface]

    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }
}
