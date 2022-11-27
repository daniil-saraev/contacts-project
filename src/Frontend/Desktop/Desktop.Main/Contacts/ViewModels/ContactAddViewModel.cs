using System.Windows.Input;
using Desktop.Main.Common.Commands;
using Desktop.Common.ViewModels;
using Desktop.Main.Contacts.Commands;
using Desktop.Main.Contacts.Models;
using Desktop.Common.Commands.Async;
using CommunityToolkit.Mvvm.Input;

namespace Desktop.Main.Contacts.ViewModels
{
    public class ContactAddViewModel : BaseViewModel
    {
        private readonly SelectedContact _selectedContact;

        public ContactViewModel Contact => _selectedContact.ContactViewModel;

        private IAsyncCommand _addContact;
        public IRelayCommand AddContact { get; }
        public ICommand Return { get; } = new NavigateTo<HomeViewModel>();

        public ContactAddViewModel(SelectedContact selectedContact)
        {
            _selectedContact = selectedContact;
            _selectedContact.ContactViewModel = new ContactViewModel();
            _addContact = new AddContactCommand(_selectedContact, Return);
            LoadingTask = new AsyncRelayCommand(_addContact.ExecuteAsync);
            AddContact = new RelayCommand(async () =>
            {
                await LoadingTask.ExecuteAsync(null);
            });
        }
    }
}
