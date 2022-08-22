using DatabaseApi;
using Desktop.Commands.Contacts;
using Desktop.Commands.Navigation;
using Desktop.Data;
using Desktop.Queries;
using Desktop.Queries.Contacts;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Desktop.ViewModels.Contacts
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly ContactsStore _store;
        private ContactViewModel? _selectedContact;

        public ObservableCollection<ContactViewModel> Contacts => _store.Contacts;

        public ContactViewModel? SelectedContact 
        {
            get
            {
                return _selectedContact;
            }
            set
            {
                _selectedContact = value;
                OnPropertyChanged();
            }
        }

        private ICommand LoadContacts { get; }
        public ICommand DeleteContact { get; }
        public ICommand NavigateToInfoView { get; }
        public ICommand NavigateToEditView { get; }
        public ICommand NavigateToAddView { get; }

        public HomeViewModel()
        {
            _store = new ContactsStore();
            LoadContacts = new LoadContactsCommand(_store);
            DeleteContact = new DeleteContactCommand(SelectedContact);
            NavigateToInfoView = new NavigateCommand(new ContactInfoViewModel(SelectedContact), (o) => SelectedContact != null);
            NavigateToEditView = new NavigateCommand(new ContactEditViewModel(SelectedContact), (o) => SelectedContact != null);
            NavigateToAddView = new NavigateCommand(new ContactAddViewModel());

            LoadContacts.Execute(null);
            Contacts.Add(new ContactViewModel(new Contact
            {
                FirstName = "Ivan",
                LastName = "Levko",
                MiddleName = "Ivanovich",
                PhoneNumber = "+79998887766"
            }));
        }
    }
}
