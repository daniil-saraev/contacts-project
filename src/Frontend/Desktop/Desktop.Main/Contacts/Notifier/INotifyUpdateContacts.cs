using System;

namespace Desktop.Main.Contacts.Notifier
{
    public interface INotifyUpdateContacts
    {
        void Notify();

        event Action? ContactsChanged;
    }
}
