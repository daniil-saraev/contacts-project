using System.Windows.Input;
using Desktop.Main.Common.Commands;
using Desktop.Common.ViewModels;
using Desktop.Main.Contacts.Commands;
using Desktop.Main.Contacts.Models;

namespace Desktop.Main.Contacts.ViewModels
{
    public class ContactEditViewModel : BaseViewModel
    {
        private readonly SelectedContact _selectedContact;

        public ContactViewModel Contact => _selectedContact.ContactViewModel;

        public ICommand UpdateContact { get; }
        public ICommand Return { get; } = new NavigateTo<HomeViewModel>();
        public ContactEditViewModel(SelectedContact selectedContact)
        {
            _selectedContact = selectedContact;   
            UpdateContact = new UpdateContactCommand(selectedContact, Return);
        }
    }
}
