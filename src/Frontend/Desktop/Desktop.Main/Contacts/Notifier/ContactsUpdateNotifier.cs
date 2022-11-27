using System;

namespace Desktop.Main.Contacts.Notifier
{
    internal class ContactsUpdateNotifier : INotifyUpdateContacts
    {
        public event Action? ContactsChanged;

        public void Notify()
        {
            ContactsChanged?.Invoke();
        }
    }
}
