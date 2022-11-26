using System.Windows.Input;
using Desktop.Main.Common.Commands;
using Desktop.Common.ViewModels;
using Desktop.Main.Contacts.Commands;
using Desktop.Main.Contacts.Models;

namespace Desktop.Main.Contacts.ViewModels
{
    public class ContactAddViewModel : BaseViewModel
    {
        private readonly SelectedContact _selectedContact;

        public ContactViewModel Contact => _selectedContact.ContactViewModel;

        public ICommand AddContact { get; }

        public ICommand Return { get; } = new NavigateTo<HomeViewModel>();

        public ContactAddViewModel(SelectedContact selectedContact)
        {
            _selectedContact = selectedContact;
            _selectedContact.ContactViewModel = new ContactViewModel();
            AddContact = new AddContactCommand(_selectedContact, Return);
        }
    }
}
