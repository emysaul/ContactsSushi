using Contacts.Models;
using Contacts.Utils;
using Contacts.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Contacts.ViewModels
{
    public class ContactViewModel : INotifyPropertyChanged, IViewModel
    {
        private const string ActionEdit = "Edit";
        private const string ActionCall = "Call";

        public ICommand AddOrUpdateContactComand { get; set; }
        public ICommand ShowMoreCommand { get; set; }
        public ICommand DetailCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public User User { get; set; }


        private ObservableCollection<Contact> _contacts;

        public ObservableCollection<Contact> Contacts
        {
            get { return _contacts; }
            set
            {
                if (value != _contacts)
                {
                    _contacts = value;
                    OnPropertyChanged();
                }

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Contact SelectedContact { get; set; } = new Contact();
        public ContactViewModel(User _user)
        {
            //User = _user;
            User = new User()
            {
                UserName = "Emy"
            };

            Contacts = new ObservableCollection<Contact>();

            AddSubscriptionBase();

            DetailCommand = new Command(ShowDetaill);
            DeleteCommand = new Command<Contact>(async (_selectedContact) =>
            {
                await DeleteContact(_selectedContact);
            });

            ShowMoreCommand = new Command<Contact>(async (_selectedContact) =>
            {
                await ShowMore(_selectedContact);
            });

            AddOrUpdateContactComand = new Command(async () =>
            {
                await App.Current.MainPage.Navigation.PushAsync(new CreateAndUpdateUser(new Contact()));
            });
        }


        private void AddSubscriptionBase()
        {
            MessagingCenter.Subscribe<IViewModel, Contact>(this, "SaveContact", ((sender, contact) =>
            {
                if (contact.Id == Guid.Empty)
                {
                    contact.Id = Guid.NewGuid();
                    Contacts.Add(contact);
                }
                else
                {
                    var contactIndex = Contacts.IndexOf(contact);
                    Contacts[contactIndex] = contact;
                }

                SelectedContact = new Contact();
                MessagingCenter.Send<IViewModel, ObservableCollection<Contact>>(this, "ChangeContacts", Contacts);
            }));
        }

        private async Task ShowMore(Contact _selectedContact)
        {
            string result = await App.Current.MainPage.DisplayActionSheet("More Options", "Cancel", null, ActionCall, ActionEdit);

            if (result == ActionCall)
            {
                await Task.Run(() => { Call(_selectedContact); });
            }
            else if (result == ActionEdit)
            {
                if (_selectedContact.Id != Guid.Empty)
                {
                    await App.Current.MainPage.Navigation.PushAsync(new CreateAndUpdateUser(_selectedContact));
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Add Contact", "Select contact before edit.", "Ok");
                }
            }
        }

        private async void ShowDetaill()
        {
            await App.Current.MainPage.Navigation.PushAsync(new DetailPage(SelectedContact));
        }

        private void Call(Contact _selectedContact)
        {
            Device.OpenUri(new Uri(String.Format("tel:{0}", _selectedContact.Number)));
        }

        private async Task DeleteContact(Contact _selectedContact)
        {
            if (_selectedContact.Id != Guid.Empty)
            {
                Contacts.Remove(_selectedContact);
                SelectedContact = new Contact();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Add Contact", "Select contact before edit.", "Ok");
            }
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
