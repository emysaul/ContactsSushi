using Contacts.Helpers;
using Contacts.Models;
using Contacts.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Contacts.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged, ISubcriptor
    {
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        public event PropertyChangedEventHandler PropertyChanged;

        MediaHelper crossMediaHelper = new MediaHelper();
        MonkeyManager monkeyManager = new MonkeyManager();

        public ICommand RegisterCommand { get; set; }
        public ICommand TakePhotoCommand { get; set; }

        public User User { get; set; } = new User();

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
        public bool ShowImage { get; set; }
 
        public RegisterViewModel()
        {
            LoadUsers();
            CreateSubscriptions();
            CreateCommands();

            this.Image = "ic_camera.png";
        }

        private void CreateSubscriptions()
        {
            MessagingCenter.Subscribe<ISubcriptor, ObservableCollection<User>>(this, "ChangeUsers", (sender, users) =>
            {
                Users = users;
                monkeyManager.SaveMokey<User>(users, "users");
            });
        }

        private void CreateCommands()
        {
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
                var imagePhoto = await crossMediaHelper.TakePhoto();
                if (imagePhoto != null)
                {
                    Image = imagePhoto.Image;
                    this.User.ImagePath = imagePhoto.Path;
                }
            });
        }

        private void RegisterUser()
        {
            MessagingCenter.Send<ISubcriptor, User>(this, "SaveUser", this.User);
        }

        private void LoadUsers()
        {
            var monkeyUsers = monkeyManager.GetMonkey<User>("users");

            if (monkeyUsers != null)
            {
                Users = monkeyUsers;
            }
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
