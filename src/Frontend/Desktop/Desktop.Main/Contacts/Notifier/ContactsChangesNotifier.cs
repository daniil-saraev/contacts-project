using System;

namespace Desktop.Main.Contacts.Notifier
{
    internal class ContactsChangesNotifier : INotifyContactsChanged
    {
        public event Action? ContactsChanged;

        public void Notify()
        {
            ContactsChanged?.Invoke();
        }
    }
}
