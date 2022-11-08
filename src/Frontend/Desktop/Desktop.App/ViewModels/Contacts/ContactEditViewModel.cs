using Desktop.Commands.Contacts;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Core.Entities;

namespace Desktop.App.ViewModels.Contacts
{
    public class ContactEditViewModel : BaseViewModel
    {
        private readonly SelectedContact _selectedContact;
        private readonly ContactViewModel _editedContactViewModel;

        public ContactViewModel Contact
        {
            get { return _editedContactViewModel; }
        }

        public ICommand UpdateContact { get; }
        public ICommand Return { get; }

        public ContactEditViewModel(SelectedContact selectedContact, IContactsStore contactsStore, INavigationService navigationService, IExceptionHandler exceptionHandler)
        {
            _selectedContact = selectedContact;
            _selectedContact.ContactChanged += SelectedContact_ContactChanged;
            _editedContactViewModel = new ContactViewModel(CreateContactCopy(_selectedContact));
            Return = new RelayCommand(() => navigationService.Return(), () => navigationService.CanReturn);
            UpdateContact = new UpdateContactCommand(selectedContact, contactsStore, _editedContactViewModel, exceptionHandler, Return);
        }

        private void SelectedContact_ContactChanged()
        {
            _editedContactViewModel.SetContact(CreateContactCopy(_selectedContact));
            OnPropertyChanged();
        }

        private Contact CreateContactCopy(SelectedContact selectedContactStore)
        {
            var contact = selectedContactStore.Contact;
            if (contact == null)
                return new Contact();

            var selectedContactCopy = new Contact
                (contact.UserId,
                contact.Id,
                contact.FirstName,
                contact.MiddleName,
                contact.LastName,
                contact.PhoneNumber,
                contact.Address,
                contact.Description);

            return selectedContactCopy;
        }
    }
}
