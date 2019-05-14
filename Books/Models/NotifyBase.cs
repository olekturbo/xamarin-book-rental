using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Books.Models
{
    public class NotifyBase : INotifyPropertyChanged
    {
        protected void NotifyPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

            public event PropertyChangedEventHandler PropertyChanged;

    }
}
