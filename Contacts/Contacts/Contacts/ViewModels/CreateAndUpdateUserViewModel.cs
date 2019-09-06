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
    public class CreateAndUpdateUserViewModel : INotifyPropertyChanged, ISubcriptor
    {
        public ObservableCollection<Contact> Contacts { get; set; } = new ObservableCollection<Contact>();

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SaveCommand { get; set; }
        public ICommand TakePhotoCommand { get; set; }
        public ICommand ViewMoreFieldsCommand { get; set; }
        public ICommand ReturnComand { get; set; }
        public Contact ContactSelected { get; set; }

        public string Result { get; set; }
        public bool ShowImage { get; set; } = true;
        public bool ShowMoreFields { get; set; }

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

        MediaHelper crossMediaHelper = new MediaHelper();
        MonkeyManager monkeyManager = new MonkeyManager();
        public CreateAndUpdateUserViewModel(Models.Contact contactSelected)
        {
            ContactSelected = contactSelected;

            if (contactSelected != null && !string.IsNullOrEmpty(this.ContactSelected?.ImagePath))
                this.Image = ImageSource.FromFile(this.ContactSelected.ImagePath);
            else
                this.Image = "ic_camera.png";

            LoadContacts();
            CreateSubscription();
            CreateCommands();
        }

        private void CreateCommands()
        {
            SaveCommand = new Command(async () =>
            {
                if (IsValidUser())
                {
                    RegisterContact();
                    this.Result = "Contact Registered";
                    await App.Current.MainPage.Navigation.PopAsync();
                }
                else
                    await App.Current.MainPage.DisplayAlert("Contact", "Contact Not valid", "Ok");
            });

            ViewMoreFieldsCommand = new Command(() =>
            {
                ShowMoreFields = true;
            });

            ReturnComand = new Command(async () =>
            {
                await App.Current.MainPage.Navigation.PopAsync();
            });

            TakePhotoCommand = new Command(async () =>
            {
                var imagePhoto = await crossMediaHelper.TakePhoto();
                if (imagePhoto != null)
                {
                    Image = imagePhoto.Image;
                    this.ContactSelected.ImagePath = imagePhoto.Path;
                }
            });
        }

        private void CreateSubscription()
        {
            MessagingCenter.Subscribe<ISubcriptor, ObservableCollection<Contact>>(this, "ChangeContacts", (sender, contacts) =>
            {
                LoadContacts();
                MessagingCenter.Unsubscribe<ISubcriptor, ObservableCollection<Contact>>(this, "ChangeContacts");
            });
        }

        private void LoadContacts()
        {
            var monkeyContacts = monkeyManager.GetMonkey<Contact>("contacts");

            if (monkeyContacts != null)
            {
                Contacts = monkeyContacts;
            }
        }

        private void RegisterContact()
        {
            MessagingCenter.Send<ISubcriptor, Contact>(this, "SaveContact", this.ContactSelected);
        }

        private bool IsValidUser()
        {
            return true;
        }
    }
}
