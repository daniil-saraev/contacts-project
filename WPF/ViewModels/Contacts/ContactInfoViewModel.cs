using Desktop.Commands.Navigation;
using Desktop.Stores;
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

        public ContactInfoViewModel(SelectedContact selectedContact)
        {
            _selectedContact = selectedContact;
            _selectedContact.ContactChanged += CurrentContactStore_CurrentContactChanged;
            _contactViewModel = new ContactViewModel(_selectedContact.Contact);
            Return = new ReturnCommand();
            NavigateToEditView = new NavigateCommand(
                new ContactEditViewModel(_selectedContact), 
                (o) => _selectedContact.Contact != null);
        }

        private void CurrentContactStore_CurrentContactChanged()
        {
            _contactViewModel.SetContact(_selectedContact.Contact);
        }
    }
}
