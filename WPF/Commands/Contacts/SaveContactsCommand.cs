using Desktop.Stores;
using System;

namespace Desktop.Commands.Contacts
{
    public class SaveContactsCommand : BaseCommand
    {
        private readonly ContactsStore _contactsStore;

        public SaveContactsCommand(Func<object?, bool>? canExecuteCustom = null) : base(canExecuteCustom)
        {
            _contactsStore = ContactsStore.GetInstance();
        }

        public override async void Execute(object? parameter)
        {
            await _contactsStore.SaveContacts();
        }
    }
}
