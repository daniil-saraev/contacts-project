using Core.Entities;
using System;
using System.Collections.Generic;

namespace Desktop.App.Interactors
{
    public interface IContactsPresenter
    {
        IEnumerable<Contact> Contacts { get; }

        event Action? CollectionChanged;
    }
}
