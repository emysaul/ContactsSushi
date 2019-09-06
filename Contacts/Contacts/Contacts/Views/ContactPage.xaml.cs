using Contacts.Models;
using Contacts.Utils;
using Contacts.ViewModels;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Contacts.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactPage : ContentPage, IViewModel
    {
        MonkeyManager monkeyManager = new MonkeyManager();

        ContactViewModel contactViewModel;
        public ContactPage(Models.User user)
        {
            InitializeComponent();
            contactViewModel = new ContactViewModel(user);
            this.BindingContext = contactViewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var monkeyContacts = monkeyManager.GetMonkey<Contact>("contacts");
            if (monkeyContacts != null)
                MessagingCenter.Send<IViewModel, ObservableCollection<Contact>>(this, "ChangeContacts", monkeyContacts);
        }
    }
}