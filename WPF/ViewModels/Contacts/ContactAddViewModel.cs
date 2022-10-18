using Desktop.Commands.Navigation;
using System.Windows.Input;
using Desktop.Commands.Contacts;
using Desktop.Services.Containers;

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

        public ContactAddViewModel(IContactsStore contactsStore)
        {
            _newContactViewModel = new ContactViewModel(new Contact());
            Return = new ReturnCommand();
            AddContact = new AddContactCommand(_newContactViewModel, contactsStore, Return);
            _newContactViewModel.PropertyChanged += NewContactViewModel_PropertyChanged;
        }

        private void NewContactViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_newContactViewModel.GetContact() == null)
                _newContactViewModel.SetContact(new Contact());
        }
    }
}
