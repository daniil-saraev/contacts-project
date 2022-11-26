using System;

namespace Desktop.Main.Contacts.Notifier
{
    public interface INotifyContactsChanged
    {
        void Notify();

        event Action? ContactsChanged;
    }
}
