using Desktop.Commands.Navigation;
using Desktop.Containers;
using Desktop.Services.Factories;
using System.Windows.Input;

namespace Desktop.ViewModels.Contacts
{
    public class ContactInfoViewModel : BaseViewModel
    {
        private readonly SelectedContact _selectedContact;
        private readonly ContactViewModel _contactViewModel;
        private readonly ContactCommandsFactory _commandsFactory;

        public ContactViewModel Contact { get { return _contactViewModel; } }

        public ICommand Return { get; }
        public ICommand NavigateToEditView { get; }

        public ContactInfoViewModel(SelectedContact selectedContact, ContactCommandsFactory commandsFactory)
        {
            _selectedContact = selectedContact;
            _selectedContact.ContactChanged += CurrentContactStore_CurrentContactChanged;
            _contactViewModel = new ContactViewModel(_selectedContact.Contact);
            _commandsFactory = commandsFactory;
            Return = new ReturnCommand();
            NavigateToEditView = new NavigateCommand(
                new ContactEditViewModel(_selectedContact, _commandsFactory), 
                (o) => _selectedContact.Contact != null);
        }

        private void CurrentContactStore_CurrentContactChanged()
        {
            _contactViewModel.SetContact(_selectedContact.Contact);
        }
    }
}
