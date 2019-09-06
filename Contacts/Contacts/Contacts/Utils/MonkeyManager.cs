using Contacts.Models;
using MonkeyCache;
using MonkeyCache.FileStore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Contacts.Utils
{
    public class MonkeyManager
    {
        IBarrel file;
        public MonkeyManager()
        {
            file = Barrel.Current;
        }

        public void SaveMokey<T>(ObservableCollection<T> elementArray, string nameMonkey) where T : Entity
        {
            file.Add<ObservableCollection<T>>(nameMonkey, elementArray, TimeSpan.FromDays(1));
        }

        public ObservableCollection<T> GetMonkey<T>(string nameMonkey) where T : Entity
        {
            if (file.Exists(nameMonkey))
            {
                var enumerableMonkey = file.Get<ObservableCollection<T>>(nameMonkey);
                return enumerableMonkey;
            }
            return null;

        }
    }
}
