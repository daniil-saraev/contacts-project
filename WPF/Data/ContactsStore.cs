using Desktop.Queries;
using Desktop.Queries.Contacts;
using Desktop.ViewModels.Contacts;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Desktop.Data
{
    public class ContactsStore
    {
        public ObservableCollection<ContactViewModel> Contacts { get; }

        public ContactsStore()
        {
            Contacts = new ObservableCollection<ContactViewModel>();
        }
    }
}
