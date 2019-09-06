using Contacts.Models;
using Contacts.ViewModels;
using MonkeyCache.FileStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Contacts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateAndUpdateUser : ContentPage
    {
        public Contact SelectedContact { get; set; } 
        public CreateAndUpdateUser(Models.Contact selectedContact)
        {
            InitializeComponent();
            this.BindingContext = new CreateAndUpdateUserViewModel(selectedContact);
        }
    }
}