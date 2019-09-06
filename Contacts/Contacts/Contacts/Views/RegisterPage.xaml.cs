
using Contacts.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Contacts.Views.LoginPage;

namespace Contacts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            this.BindingContext = new RegisterViewModel();

        }

    }
}