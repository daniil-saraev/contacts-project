using OpenApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Containers
{
    public interface IContactsStore
    {
        IEnumerable<Contact> Contacts { get; }

        event Action? CollectionChanged;
    }
}
