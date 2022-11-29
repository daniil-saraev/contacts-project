using System;

namespace Desktop.Main.Contacts.Notifier
{
    /// <summary>
    /// Service to notify viewmodels to update a contact list.
    /// </summary>
    public interface INotifyUpdateContacts
    {
        void Notify();

        event Action? ContactsChanged;
    }
}
