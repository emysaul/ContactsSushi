using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Contacts.Helpers
{
    public static class ExtentionEventHelper
    {
        public static void OnPropertyChanged(this PropertyChangedEventHandler PropertyChanged, object sender, [CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}
