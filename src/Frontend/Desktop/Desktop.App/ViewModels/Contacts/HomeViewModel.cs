using CommunityToolkit.Mvvm.Input;
using Desktop.Commands.Contacts;
using Desktop.Services.Factories;
using NuGet.Packaging;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DesktopApp.Commands.Contacts;

namespace Desktop.App.ViewModels.Contacts
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

        public HomeViewModel(IContactsPresenter contactsPresenter, IContactsStore contactsStore, SelectedContact selectedContact,
                            INavigationService navigationService, IExceptionHandler exceptionHandler)
        {
            _contactsPresenter = contactsPresenter;
            _selectedContact = selectedContact;
            DeleteContact = new DeleteContactCommand(selectedContact, contactsStore, exceptionHandler, null);
            NavigateToInfoView = new RelayCommand(() => navigationService.NavigateTo<ContactInfoViewModel>());
            NavigateToEditView = new RelayCommand(() => navigationService.NavigateTo<ContactEditViewModel>());
            NavigateToAddView = new RelayCommand(() => navigationService.NavigateTo<ContactAddViewModel>());
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
