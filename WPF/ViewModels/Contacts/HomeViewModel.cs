using Desktop.Commands.Contacts;
using Desktop.Commands.Navigation;
using Desktop.Containers;
using Desktop.Services.Factories;
using NuGet.Packaging;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Desktop.ViewModels.Contacts
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IContactsStore _contactsStore;
        private readonly ObservableCollection<ContactViewModel> _contacts;
        private readonly SelectedContact _selectedContact;
        private ContactViewModel? _selectedContactViewModel;

        public ReadOnlyObservableCollection<ContactViewModel> Contacts { get; }

        public ContactViewModel? SelectedContactViewModel
        {
            get
            {
                return _selectedContactViewModel;
            } 
            set
            {
                _selectedContact.Contact = value?.GetContact();
                OnPropertyChanged();
            }
        }

        public ICommand DeleteContact { get; }
        public ICommand NavigateToInfoView { get; }
        public ICommand NavigateToEditView { get; }
        public ICommand NavigateToAddView { get; }

        public HomeViewModel(IContactsStore contactsStore, SelectedContact selectedContact, ContactCommandsFactory contactCommandsFactory)  
        {
            _contactsStore = contactsStore;
            _selectedContact = selectedContact;
            DeleteContact = contactCommandsFactory.NewDeleteContactCommand(null);
            NavigateToInfoView = new NavigateCommand(new ContactInfoViewModel(_selectedContact, contactCommandsFactory), (o) => _selectedContact.Contact != null);
            NavigateToEditView = new NavigateCommand(new ContactEditViewModel(_selectedContact, contactCommandsFactory), (o) => _selectedContact.Contact != null);
            NavigateToAddView = new NavigateCommand(new ContactAddViewModel(_selectedContact, contactCommandsFactory));

            _contacts = new ObservableCollection<ContactViewModel>(_contactsStore.Contacts.Select(c => new ContactViewModel(c)));
            Contacts = new ReadOnlyObservableCollection<ContactViewModel>(_contacts);

            _selectedContact.ContactChanged += SelectedContact_ContactChanged;
            _contactsStore.CollectionChanged += ContactsStore_CollectionChanged;
        }

        private void SelectedContact_ContactChanged()
        {
            if (_selectedContact.Contact != null)
                _selectedContactViewModel = new ContactViewModel(_selectedContact.Contact);
            else
                _selectedContactViewModel = null;
        }

        private void ContactsStore_CollectionChanged()
        {
            _contacts.Clear();
            _contacts.AddRange(_contactsStore.Contacts.Select(c => new ContactViewModel(c)));
        }
    }
}
