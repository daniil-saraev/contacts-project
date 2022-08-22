using Desktop.Commands.Contacts;
using Desktop.Commands.Navigation;
using System.Windows.Input;

namespace Desktop.ViewModels.Contacts
{
    public class ContactEditViewModel : BaseViewModel
    {
        public ContactViewModel? Contact { get; }
        public ICommand UpdateContact { get; }
        public ICommand Return { get; }

        public ContactEditViewModel(ContactViewModel? contact)
        {
            Contact = contact;
            UpdateContact = new UpdateContactCommand(Contact);
            Return = new ReturnCommand();
        }
    }
}
