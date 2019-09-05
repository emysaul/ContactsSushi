using Contacts.Helpers;
using Contacts.Models;
using Contacts.Utils;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Contacts.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged, IViewModel
    {
        ObservableCollection<User> Users = new ObservableCollection<User>();
        public event PropertyChangedEventHandler PropertyChanged;

        public User User { get; set; } = new User();


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

        public string ConfirmatedPassword { get; set; }

        public string Result { get; set; } = string.Empty;

        public ICommand RegisterCommand { get; set; }
        public ICommand TakePhotoCommand { get; set; }

        public CrossMediaHelper crossMediaHelper = new CrossMediaHelper();
        public RegisterViewModel()
        {
            MessagingCenter.Subscribe<IViewModel, ObservableCollection<User>>(this, "ChangeUsers", (sender, users) =>
            {
                Users = users;
            });

            RegisterCommand = new Command(async () =>
            {
                if (IsValidUser())
                {

                    RegisterUser();
                    this.Result = "User Registered";
                    await App.Current.MainPage.Navigation.PopAsync();
                }
            });

            TakePhotoCommand = new Command(async () =>
            {
                Image = await crossMediaHelper.TakePhoto();
            });
        }


        private void RegisterUser()
        {
            this.User.Image = Image;
            MessagingCenter.Send<IViewModel, User>(this, "SaveUser", this.User);
        }

        private bool IsValidUser()
        {
            if (string.IsNullOrEmpty(User.UserName))
            {
                this.Result = "Not Registered: Name not valid.";
                return false;
            }

            if (!RegexValidator.ValidEmail(User.Email))
            {
                this.Result = "Not Registered Email not valid.";
                return false;
            }

            if (string.IsNullOrEmpty(ConfirmatedPassword) || ConfirmatedPassword != User.Password)
            {
                this.Result = "Not Registered: Password are not the same or not valid.";
                return false;
            }

            if (Users.Any(e => e.UserName == User.UserName))
            {
                this.Result = "Not Registered: Usuario existente";
                return false;
            }

            return true;
        }


    }
}
