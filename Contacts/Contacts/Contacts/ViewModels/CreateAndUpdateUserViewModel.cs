using Contacts.Helpers;
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
        public ICommand TakePhotoCommand { get; set; }

        public Contact ContactSelected { get; set; }
        public string Result { get; set; }

        public bool ShowImage { get; set; }

        private ImageSource _image;
        public ImageSource Image
        {
            get { return _image; }
            set
            {
                if (value != _image)
                {
                    _image = value;
                    PropertyChanged.OnPropertyChanged(this);
                    if (value != null)
                        ShowImage = true;

                }
            }
        }

        public CrossMediaHelper crossMediaHelper = new CrossMediaHelper();
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


            TakePhotoCommand = new Command(async () =>
            {
                Image = await crossMediaHelper.TakePhoto();
            });

        }

        private void RegisterContact()
        {
            this.ContactSelected.Image = this.Image;

            MessagingCenter.Send<IViewModel, Contact>(this, "SaveContact", this.ContactSelected);
        }

        private bool IsValidUser()
        {
            return true;
        }
    }
}
