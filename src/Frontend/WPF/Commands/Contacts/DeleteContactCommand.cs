using System;
using System.Windows.Input;
using Desktop.Containers;
using Desktop.Services.Containers;

namespace Desktop.Commands.Contacts
{
    public class DeleteContactCommand : BaseCommand
    {
        private readonly SelectedContact _selectedContact;
        private readonly IContactsStore _contactsStore;
        private readonly ICommand? _returnCommand;

        public DeleteContactCommand(SelectedContact selectedContact, IContactsStore contactsStore, 
                                    ICommand? returnCommand,Func<object?, bool>? canExecuteCustom = null) : base(canExecuteCustom)
        {
            _contactsStore = contactsStore;
            _selectedContact = selectedContact;
            _returnCommand = returnCommand;
            _selectedContact.ContactChanged += CurrentContactStore_SelectedContactChanged;
        }

        private void CurrentContactStore_SelectedContactChanged()
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _selectedContact.Contact != null;
        }

        public override async void Execute(object? parameter)
        {
            try
            {
                _contactsStore.RemoveContact(_selectedContact.Contact);
                await _contactsStore.SaveContactsAsync();
            }
            catch
            {
                
            }
            finally
            {
                _returnCommand?.Execute(null);
            }           
        }
    }
}
