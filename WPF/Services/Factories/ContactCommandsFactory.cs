using Desktop.Commands.Contacts;
using Desktop.Containers;
using Desktop.Services.Data;
using Desktop.ViewModels.Contacts;
using System.Windows.Input;

namespace Desktop.Services.Factories
{
    public class ContactCommandsFactory
    {
        private readonly ContactsStore _contactsStore;
        private readonly SelectedContact _selectedContact;

        public ContactCommandsFactory(ContactsStore contactsStore, SelectedContact selectedContact)
        {
            _contactsStore = contactsStore;
            _selectedContact = selectedContact;
        }

        public ICommand NewDeleteContactCommand(ICommand? returnCommand)
        {
            return new DeleteContactCommand(_selectedContact, _contactsStore, returnCommand);
        }

        public ICommand NewAddContactCommand(ContactViewModel newContactViewModel, ICommand? returnCommand)
        {
            return new AddContactCommand(newContactViewModel, _contactsStore, returnCommand);
        }

        public ICommand NewUpdateContactCommand(ContactViewModel editedContactViewModel, ICommand? returnCommand)
        {
            return new UpdateContactCommand(_selectedContact, _contactsStore, editedContactViewModel, returnCommand);
        }
    }
}
