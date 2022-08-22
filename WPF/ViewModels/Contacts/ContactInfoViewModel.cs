using Desktop.Commands.Navigation;
using Desktop.ViewModels;
using System.Windows.Input;

namespace Desktop.ViewModels.Contacts
{
    public class ContactInfoViewModel : BaseViewModel
    {
        public ContactViewModel? Contact { get; }
        public ICommand Return { get; }

        public ContactInfoViewModel(ContactViewModel? contact)
        {
            Contact = contact;
            Return = new ReturnCommand();
        }
    }
}
