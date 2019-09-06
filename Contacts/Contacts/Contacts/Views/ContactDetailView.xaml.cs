using Contacts.Models;
using Xamarin.Forms.Xaml;

namespace Contacts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactDetailView 
    {
        public ContactDetailView(Contact contact)
        {
            InitializeComponent();
            this.BindingContext = contact;
        }
    }
}