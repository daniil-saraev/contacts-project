using System.Windows.Input;
using Desktop.Commands.Contacts;
using Desktop.Services.Containers;
using Desktop.Services.Navigation;
using CommunityToolkit.Mvvm.Input;

namespace Desktop.ViewModels.Contacts
{
    public class ContactAddViewModel : BaseViewModel
    {
        private readonly ContactViewModel _newContactViewModel;

        public ContactViewModel Contact
        {
            get { return _newContactViewModel; }
        }

        public ICommand AddContact { get; }

        public ICommand Return { get; }

        public ContactAddViewModel(IContactsStore contactsStore, INavigationService navigationService)
        {
            _newContactViewModel = new ContactViewModel(new Contact());
            _newContactViewModel.PropertyChanged += NewContactViewModel_PropertyChanged;
            Return = new RelayCommand(() => navigationService.Return(), () => navigationService.CanReturn);
            AddContact = new AddContactCommand(_newContactViewModel, contactsStore, Return);           
        }

        private void NewContactViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_newContactViewModel.GetContact() == null)
                _newContactViewModel.SetContact(new Contact());
        }
    }
}
