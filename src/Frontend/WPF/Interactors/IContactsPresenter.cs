using System;
using System.Collections.Generic;

namespace Desktop.Interactors
{
    public interface IContactsPresenter
    {
        IEnumerable<Contact> Contacts { get; }

        event Action? CollectionChanged;
    }
}
