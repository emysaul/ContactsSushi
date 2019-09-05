using Contacts.Helpers;
using Contacts.Models;
using Contacts.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Contacts.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged, IViewModel
    {
        ObservableCollection<User> Users = new ObservableCollection<User>();

        public User User { get; set; } = new User();

        public string ConfirmatedPassword { get; set; }

        public string Result { get; set; } = string.Empty;

        public ICommand RegisterCommand { get; set; }

        public RegisterViewModel()
        {
            MessagingCenter.Subscribe<IViewModel, ObservableCollection<User>>(this, "ChangeUsers", (sender, users) =>
            {
                Users = users;
            });

            RegisterCommand = new Command(() =>
            {
                if (IsValidUser())
                {
                    RegisterUser();
                    this.Result = "User Registered";
                    App.Current.MainPage.Navigation.PopAsync();
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RegisterUser()
        {
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
