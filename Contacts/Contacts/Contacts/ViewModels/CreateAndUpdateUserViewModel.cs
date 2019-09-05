using Contacts.Models;
using Contacts.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Contacts.ViewModels
{
    public class CreateAndUpdateUserViewModel : INotifyPropertyChanged, IViewModel
    {
        ObservableCollection<Contact> Contacts = new ObservableCollection<Contact>();

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SaveCommand { get; set; }

        public Contact ContactSelected { get; set; }
        public string Result { get; set; }

        public CreateAndUpdateUserViewModel(Models.Contact contactSelected)
        {
            ContactSelected = contactSelected;

            MessagingCenter.Subscribe<IViewModel, ObservableCollection<Contact>>(this, "ChangeContacts", (sender, contacts) =>
            {
                Contacts = contacts;
                MessagingCenter.Unsubscribe<IViewModel, ObservableCollection<Contact>>(this, "ChangeContacts");
            });

            SaveCommand = new Command(() =>
            {
                if (IsValidUser())
                {
                    RegisterContact();
                    this.Result = "Contact Registered";
                    App.Current.MainPage.Navigation.PopAsync();
                }
            });

        }

        private void RegisterContact()
        {
            MessagingCenter.Send<IViewModel, Contact>(this, "SaveContact", this.ContactSelected);
        }

        private bool IsValidUser()
        {
            return true;
        }
    }
}
