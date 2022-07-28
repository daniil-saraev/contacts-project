using ApiServices;
using Desktop.Commands.Contacts;
using Desktop.Commands.Navigation;
using System.Windows.Input;

namespace Desktop.ViewModels.Contacts
{
    public class ContactAddViewModel : BaseViewModel
    {
        public ContactViewModel Contact { get; }

        public ICommand AddContact { get; }
        public ICommand Return { get; }

        public ContactAddViewModel()
        {
            Contact = new ContactViewModel(new Contact());
            AddContact = new AddContactCommand(Contact);
            Return = new ReturnCommand();
        }
    }
}
