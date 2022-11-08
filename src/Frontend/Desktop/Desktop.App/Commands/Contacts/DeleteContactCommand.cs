using System;
using System.Windows.Input;

namespace Desktop.App.Commands.Contacts
{
    public class DeleteContactCommand : BaseCommand
    {
        private readonly SelectedContact _selectedContact;
        private readonly IContactsStore _contactsStore;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ICommand? _returnCommand;

        public DeleteContactCommand(SelectedContact selectedContact, IContactsStore contactsStore, IExceptionHandler exceptionHandler,
                                    ICommand? returnCommand, Func<object?, bool>? canExecuteCustom = null) : base(canExecuteCustom)
        {
            _contactsStore = contactsStore;
            _selectedContact = selectedContact;
            _exceptionHandler = exceptionHandler;
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
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(ex);
            }
            finally
            {
                _returnCommand?.Execute(null);
            }
        }
    }
}
