using DatabaseApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Stores
{
    public interface IContactsStore
    {
        IEnumerable<Contact> Contacts { get; }

        event Action? CollectionChanged;
    }
}
