using System;
using Desktop.Services.ExceptionHandlers;
using Desktop.Stores;

namespace Desktop.Commands.Contacts
{
    public class DeleteContactCommand : BaseCommand
    {
        private readonly SelectedContact _currentContactStore;
        private readonly ContactsStore _contactsStore;

        public DeleteContactCommand(SelectedContact currentContactStore, Func<object?, bool>? canExecuteCustom = null) : base(canExecuteCustom)
        {
            _contactsStore = ContactsStore.GetInstance();
            _currentContactStore = currentContactStore;
            _currentContactStore.ContactChanged += CurrentContactStore_SelectedContactChanged;
        }

        private void CurrentContactStore_SelectedContactChanged()
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _currentContactStore.Contact != null;
        }

        public async override void Execute(object? parameter)
        {
            await _contactsStore.RemoveContactAsync(_currentContactStore.Contact);
        }
    }
}
