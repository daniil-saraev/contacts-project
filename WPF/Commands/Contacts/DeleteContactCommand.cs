using System;
using System.Windows.Input;
using Desktop.Containers;
using Desktop.Services.Data;

namespace Desktop.Commands.Contacts
{
    public class DeleteContactCommand : BaseCommand
    {
        private readonly SelectedContact _currentContactStore;
        private readonly ContactsStore _contactsStore;
        private readonly ICommand? _returnCommand;

        public DeleteContactCommand(SelectedContact selectedContact, ContactsStore contactsStore, 
                                    ICommand? returnCommand,Func<object?, bool>? canExecuteCustom = null) : base(canExecuteCustom)
        {
            _contactsStore = contactsStore;
            _currentContactStore = selectedContact;
            _returnCommand = returnCommand;
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
            await _contactsStore.RemoveContact(_currentContactStore.Contact);
            _returnCommand?.Execute(null);
        }
    }
}
