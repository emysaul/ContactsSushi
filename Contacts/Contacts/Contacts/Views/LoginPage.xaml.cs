using Contacts.ViewModels;
using MonkeyCache.FileStore;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Contacts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            Barrel.ApplicationId = "MI_BARRIEL_FOLDER";
            this.BindingContext = new LoginViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Clean();
        }

        private void Clean()
        {
            txtName.Text = string.Empty;
            txtPassword.Text = string.Empty;
        }
    }
}