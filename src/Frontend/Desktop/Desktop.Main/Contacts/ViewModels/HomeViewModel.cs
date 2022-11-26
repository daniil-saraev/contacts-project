using NuGet.Packaging;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Desktop.Common.ViewModels;
using Desktop.Main.Common.Commands;
using Desktop.Main.Contacts.Models;
using Desktop.Main.Contacts.Commands;
using Desktop.Main.Contacts.Notifier;
using Nito.AsyncEx;
using System.Threading.Tasks;
using System.Collections.Generic;
using Desktop.Common.Commands.Async;
using CommunityToolkit.Mvvm.Input;

namespace Desktop.Main.Contacts.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly ObservableCollection<ContactViewModel> _contacts;
        private readonly SelectedContact _selectedContact;
        private readonly INotifyContactsChanged _notifier;

        public INotifyTaskCompletion? LoadContactsTask { get; private set; }

        public ContactViewModel? SelectedContactViewModel
        {
            get
            {
                if (_selectedContact.SelectedContactIsNull)
                    return null;
                else
                    return _selectedContact.ContactViewModel;
            }
            set
            {
                if(value != null)
                    _selectedContact.ContactViewModel = value;
            }
        }

        public ReadOnlyObservableCollection<ContactViewModel> Contacts { get; }

        private IAsyncCommand<IEnumerable<ContactViewModel>?> GetContacts { get; }
        public ICommand DeleteContact { get; }
        public ICommand NavigateToInfoView { get; } = new NavigateTo<ContactInfoViewModel>();
        public ICommand NavigateToEditView { get; } = new NavigateTo<ContactEditViewModel>();
        public ICommand NavigateToAddView { get; } = new NavigateTo<ContactAddViewModel>();

        public HomeViewModel(SelectedContact selectedContact, INotifyContactsChanged notifier)
        {
            _selectedContact = selectedContact;
            _notifier = notifier;
            _notifier.ContactsChanged += UpdateContacts;
            _contacts = new ObservableCollection<ContactViewModel>();
            Contacts = new ReadOnlyObservableCollection<ContactViewModel>(_contacts);
            DeleteContact = new DeleteContactCommand(selectedContact, null);
            GetContacts = new GetContactsCommand();
            LoadContactsTask = NotifyTaskCompletion.Create(LoadContacts());   
        }
        
        private async void UpdateContacts()
        {
            await LoadContacts();
        }

        private async Task LoadContacts()
        {
            _contacts.Clear();
            _contacts.AddRange(await GetContacts.ExecuteAsync());
        }
    }
}
