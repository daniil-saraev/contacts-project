using DatabaseApi;
using Desktop.Commands.Contacts;
using Desktop.Commands.Navigation;
using Desktop.Services.Authentication.UserServices;
using Desktop.Stores;
using System.Windows.Input;

namespace Desktop.ViewModels.Contacts
{
    public class ContactAddViewModel : BaseViewModel
    {
        private readonly SelectedContact _selectedContact;
        private readonly ContactViewModel _newContactViewModel;

        public ContactViewModel Contact
        {
            get { return _newContactViewModel; }
        }

        public ICommand AddContact { get; }

        public ICommand Return { get; }

        public ContactAddViewModel(SelectedContact currentContact)
        {
            _selectedContact = currentContact;
            _selectedContact.Contact = User.IsAuthenticated ? new Contact(User.Id) : new Contact();
            _newContactViewModel = new ContactViewModel(_selectedContact.Contact);
            Return = new ReturnCommand();
            AddContact = new AddContactCommand(_newContactViewModel, Return);
        }
    }
}
