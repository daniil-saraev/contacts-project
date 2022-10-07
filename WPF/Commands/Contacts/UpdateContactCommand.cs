using System;
using System.Windows.Input;
using Desktop.ViewModels.Contacts;
using Desktop.Services.Data;
using Desktop.Containers;

namespace Desktop.Commands.Contacts
{
    public class UpdateContactCommand : BaseCommand
    {
        private readonly ContactsStore _contactsStore;
        private readonly SelectedContact _currentContactStore;       
        private readonly ContactViewModel _editedContact;
        private readonly ICommand? _returnCommand;

        public UpdateContactCommand(SelectedContact selectedContact, ContactsStore contactsStore, ContactViewModel editedContactViewModel, 
                                    ICommand? returnCommand, Func<object?, bool>? canExecuteCustom = null) : base(canExecuteCustom)
        {
            _contactsStore = contactsStore;
            _currentContactStore = selectedContact;
            _editedContact = editedContactViewModel;
            _returnCommand = returnCommand;
            _editedContact.ErrorsChanged += EditedContact_ErrorsChanged;
        }

        private void EditedContact_ErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _editedContact.GetContact() != null && !_editedContact.HasErrors;
        }

        public override async void Execute(object? parameter)
        {
            _editedContact.ValidateModel();
            if (_editedContact.HasErrors)
                return;
            await _contactsStore.UpdateContact(_currentContactStore.Contact, _editedContact.GetContact());
            _currentContactStore.Contact = _editedContact.GetContact();
            _returnCommand?.Execute(null);
        }
    }
}
