using CommunityToolkit.Mvvm.Input;
using Desktop.Commands.Contacts;
using Desktop.Commands.Navigation;
using Desktop.Containers;
using Desktop.Services.Containers;
using Desktop.Services.Factories;
using Desktop.Services.Navigation;
using NuGet.Packaging;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Desktop.ViewModels.Contacts
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IContactsPresenter _contactsPresenter;
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

        public HomeViewModel(IContactsPresenter contactsPresenter, IContactsStore contactsStore, SelectedContact selectedContact, INavigationService navigationService)  
        {
            _contactsPresenter = contactsPresenter;
            _selectedContact = selectedContact;
            DeleteContact = new DeleteContactCommand(selectedContact, contactsStore, null);
            NavigateToInfoView = new NavigateToContactInfoViewCommand(selectedContact, viewModelsFactory);
            NavigateToEditView = new NavigateToContactEditViewCommand(selectedContact, viewModelsFactory);
            NavigateToAddView = new NavigateToContactAddViewCommand(viewModelsFactory);
            _contacts = new ObservableCollection<ContactViewModel>(_contactsPresenter.Contacts.Select(c => new ContactViewModel(c)));
            Contacts = new ReadOnlyObservableCollection<ContactViewModel>(_contacts);

            _selectedContact.ContactChanged += SelectedContact_ContactChanged;
            _contactsPresenter.CollectionChanged += ContactsStore_CollectionChanged;
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
            _contacts.AddRange(_contactsPresenter.Contacts.Select(c => new ContactViewModel(c)));
        }
    }
}
