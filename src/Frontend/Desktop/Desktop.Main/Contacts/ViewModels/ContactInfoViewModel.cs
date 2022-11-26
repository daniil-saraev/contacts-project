using Desktop.Main.Common.Commands;
using Desktop.Common.ViewModels;
using Desktop.Main.Contacts.Models;
using System.Windows.Input;

namespace Desktop.Main.Contacts.ViewModels
{
    public class ContactInfoViewModel : BaseViewModel
    {
        private readonly SelectedContact _selectedContact;

        public ContactViewModel Contact => _selectedContact.ContactViewModel;

        public ICommand Return { get; } = new NavigateTo<HomeViewModel>();
        public ICommand NavigateToEditView { get; } = new NavigateTo<ContactEditViewModel>();

        public ContactInfoViewModel(SelectedContact selectedContact)
        {
            _selectedContact = selectedContact;
        }
    }
}
