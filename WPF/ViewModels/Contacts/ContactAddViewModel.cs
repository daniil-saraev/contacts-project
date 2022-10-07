using OpenApi;
using Desktop.Commands.Contacts;
using Desktop.Commands.Navigation;
using Desktop.Services.Authentication.UserServices;
using System.Windows.Input;
using Desktop.Services.Factories;
using Desktop.Containers;

namespace Desktop.ViewModels.Contacts
{
    public class ContactAddViewModel : BaseViewModel
    {
        private readonly SelectedContact _selectedContact;
        private readonly ContactViewModel _newContactViewModel;
        private readonly ContactCommandsFactory _commandsFactory;

        public ContactViewModel Contact
        {
            get { return _newContactViewModel; }
        }

        public ICommand AddContact { get; }

        public ICommand Return { get; }

        public ContactAddViewModel(SelectedContact currentContact, ContactCommandsFactory commandsFactory)
        {
            _selectedContact = currentContact;
            _selectedContact.Contact = User.IsAuthenticated ? new Contact(User.Id) : new Contact();
            _newContactViewModel = new ContactViewModel(_selectedContact.Contact);
            _commandsFactory = commandsFactory;
            Return = new ReturnCommand();
            AddContact = _commandsFactory.NewAddContactCommand(_newContactViewModel, Return);
        }
    }
}
