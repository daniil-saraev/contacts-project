using System.Windows.Input;
using Desktop.Main.Common.Commands;
using Desktop.Common.ViewModels;
using Desktop.Main.Contacts.Commands;
using Desktop.Main.Contacts.Models;
using Desktop.Common.Commands.Async;
using CommunityToolkit.Mvvm.Input;

namespace Desktop.Main.Contacts.ViewModels
{
    public class ContactEditViewModel : BaseViewModel
    {
        private readonly SelectedContact _selectedContact;

        public ContactViewModel Contact => _selectedContact.ContactViewModel;

        private IAsyncCommand _updateContact;
        public IRelayCommand UpdateContact { get; }
        public ICommand Return { get; } = new NavigateTo<HomeViewModel>();
        public ContactEditViewModel(SelectedContact selectedContact)
        {
            _selectedContact = selectedContact;   
            _updateContact = new UpdateContactCommand(selectedContact, Return);
            LoadingTask = new AsyncRelayCommand(_updateContact.ExecuteAsync);
            UpdateContact = new RelayCommand(async () =>
            {
                await LoadingTask.ExecuteAsync(null);
            });
        }
    }
}
