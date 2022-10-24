using CommunityToolkit.Mvvm.Input;
using Desktop.Interactors;
using Desktop.Services.Factories;
using Desktop.Services.Navigation;
using System.Windows.Input;

namespace Desktop.ViewModels.Contacts
{
    public class ContactInfoViewModel : BaseViewModel
    {
        private readonly SelectedContact _selectedContact;
        private readonly ContactViewModel _contactViewModel;

        public ContactViewModel Contact { get { return _contactViewModel; } }

        public ICommand Return { get; }
        public ICommand NavigateToEditView { get; }

        public ContactInfoViewModel(SelectedContact selectedContact, IContactsStore contactsStore, INavigationService navigationService)
        {
            _selectedContact = selectedContact;
            _selectedContact.ContactChanged += CurrentContactStore_CurrentContactChanged;
            _contactViewModel = new ContactViewModel(_selectedContact.Contact);
            Return = new RelayCommand(() => navigationService.Return(), () => navigationService.CanReturn);
            NavigateToEditView = new RelayCommand(() => navigationService.NavigateTo<ContactEditViewModel>());
        }

        private void CurrentContactStore_CurrentContactChanged()
        {
            _contactViewModel.SetContact(_selectedContact.Contact);
            OnPropertyChanged();
        }
    }
}
