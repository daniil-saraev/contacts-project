using OpenApi;
using Desktop.Commands.Contacts;
using Desktop.Commands.Navigation;
using System.Windows.Input;
using Desktop.Services.Factories;
using Desktop.Containers;

namespace Desktop.ViewModels.Contacts
{
    public class ContactEditViewModel : BaseViewModel
    {
        private readonly SelectedContact _selectedContact;
        private readonly ContactViewModel _editedContactViewModel;
        private readonly ContactCommandsFactory _commandsFactory;

        public ContactViewModel Contact
        {
            get { return _editedContactViewModel; }
        }

        public ICommand UpdateContact { get; }
        public ICommand Return { get; }

        public ContactEditViewModel(SelectedContact selectedContact, ContactCommandsFactory commandsFactory)
        {
            _selectedContact = selectedContact;
            _selectedContact.ContactChanged += CurrentContactStore_CurrentContactChanged;
            _editedContactViewModel = new ContactViewModel(CreateContactCopy(_selectedContact));
            _commandsFactory = commandsFactory;
            Return = new ReturnCommand();
            UpdateContact = _commandsFactory.NewUpdateContactCommand(_editedContactViewModel, Return);
        }

        private void CurrentContactStore_CurrentContactChanged()
        {
            _editedContactViewModel.SetContact(CreateContactCopy(_selectedContact));
        }

        private Contact CreateContactCopy(SelectedContact selectedContactStore)
        {
            Contact? contact = selectedContactStore.Contact;
            if (contact == null)
                return new Contact();

            Contact selectedContactCopy = new Contact(contact.UserId, contact.Id)
            {
                FirstName = contact.FirstName,
                MiddleName = contact.MiddleName,
                LastName = contact.LastName,
                PhoneNumber = contact.PhoneNumber,
                Address = contact.Address,
                Description = contact.Description
            };
            return selectedContactCopy;
        }
    }
}
