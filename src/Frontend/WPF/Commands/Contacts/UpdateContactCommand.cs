using System;
using System.Windows.Input;
using Desktop.ViewModels.Contacts;
using Desktop.Interactors;
using Desktop.Services.ExceptionHandler;

namespace Desktop.Commands.Contacts
{
    public class UpdateContactCommand : BaseCommand
    {
        private readonly IContactsStore _contactsStore;
        private readonly SelectedContact _selectedContact;       
        private readonly ContactViewModel _editedContact;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ICommand? _returnCommand;

        public UpdateContactCommand(SelectedContact selectedContact, IContactsStore contactsStore, ContactViewModel editedContactViewModel,
                                    IExceptionHandler exceptionHandler, ICommand? returnCommand, Func<object?, bool>? canExecuteCustom = null) 
                                    : base(canExecuteCustom)
        {
            _contactsStore = contactsStore;
            _selectedContact = selectedContact;
            _editedContact = editedContactViewModel;
            _exceptionHandler = exceptionHandler;
            _returnCommand = returnCommand;
            _editedContact.ErrorsChanged += EditedContact_ErrorsChanged;
        }

        private void EditedContact_ErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) 
                && _selectedContact.Contact != null 
                && _editedContact.GetContact() != null 
                && !_editedContact.HasErrors;
        }

        public override async void Execute(object? parameter)
        {
            _editedContact.ValidateModel();
            if (_editedContact.HasErrors)
                return;

            try
            {
                _contactsStore.UpdateContact(_selectedContact.Contact, _editedContact.GetContact());
                await _contactsStore.SaveContactsAsync();           
            }
            catch (System.Exception ex)
            {
                _exceptionHandler.HandleException(ex);
            }
            finally
            {
                _editedContact.Dispose();
                _returnCommand?.Execute(null);
            }        
        }
    }
}
