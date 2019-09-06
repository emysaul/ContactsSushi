using Contacts.Helpers;
using Contacts.Models;
using Contacts.Utils;
using Contacts.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Contacts.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged, ISubcriptor
    {
        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        MonkeyManager monkeyManager = new MonkeyManager();

        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
        public User User { get; set; } = new User();
        public string Password { get; set; }
        public string Result { get; set; }

        public LoginViewModel()
        {
            AddSubscriptionBase();
            CreateCommands();
        }

        private void CreateCommands()
        {
            LoginCommand = new Command(async () =>
            {
                var monkeyUsers = monkeyManager.GetMonkey<User>("users");

                if (monkeyUsers != null)
                {
                    Users = monkeyUsers;
                }

                var userFinded = Users.FirstOrDefault(e => e.UserName.ToUpper() == User.UserName.ToUpper() && e.Password == User.Password);
                if (userFinded != null)
                {
                    await App.Current.MainPage.Navigation.PushAsync(new ContactPage(userFinded));
                }
                else
                    Result = "Usuario o contraseña no valido";
            });

            RegisterCommand = new Command(async () =>
            {
                await App.Current.MainPage.Navigation.PushAsync(new RegisterPage());
            });
        }

        private void AddSubscriptionBase()
        {
            MessagingCenter.Subscribe<ISubcriptor, User>(this, "SaveUser", ((sender, user) =>
            {
                Users.Add(user);
                MessagingCenter.Send<ISubcriptor, ObservableCollection<User>>(this, "ChangeUsers", Users);
            }));
        }

        ~LoginViewModel()
        {
            MessagingCenter.Unsubscribe<App, string>(this, "SaveUser");
        }
    }
}
