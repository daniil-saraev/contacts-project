using Core.Contacts.Interfaces;
using Core.Contacts.Requests;
using Desktop.Common.Commands.Async;
using Desktop.Common.Services;
using Desktop.Contacts.Persistence;
using Desktop.Main.Contacts.Models;
using Desktop.Main.Contacts.Notifier;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Desktop.Main.Contacts.Commands
{
    public class UpdateContactCommand : AsyncBaseCommand
    {
        private IContactBookService _contactBook => ServiceProvider.GetRequiredService<IContactBookService>();
        private IPersistenceProvider _persistence => ServiceProvider.GetRequiredService<IPersistenceProvider>();
        private IExceptionHandler _exceptionHandler => ServiceProvider.GetRequiredService<IExceptionHandler>();
        private INotifyUpdateContacts _notifyContactsChanged => ServiceProvider.GetRequiredService<INotifyUpdateContacts>();

        private readonly SelectedContact _selectedContact;
        private readonly ICommand? _returnCommand;

        public UpdateContactCommand(SelectedContact selectedContact, ICommand? returnCommand)                        
        {
            _selectedContact = selectedContact;
            _returnCommand = returnCommand;
            _selectedContact.ContactViewModel.ErrorsChanged += EditedContact_ErrorsChanged;
        }

        private void EditedContact_ErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter)
                && !_selectedContact.ContactViewModel.HasErrors;
        }

        public override async Task ExecuteAsync()
        {
            _selectedContact.ContactViewModel.ValidateModel();
            if (_selectedContact.ContactViewModel.HasErrors)
                return;

            try
            {
                await _contactBook.UpdateContact(new UpdateContactRequest
                {
                    Id = _selectedContact.ContactViewModel.Id,
                    FirstName = _selectedContact.ContactViewModel.FirstName,
                    MiddleName = _selectedContact.ContactViewModel.MiddleName,
                    LastName = _selectedContact.ContactViewModel.LastName,
                    PhoneNumber = _selectedContact.ContactViewModel.PhoneNumber,
                    Address = _selectedContact.ContactViewModel.Address,
                    Description = _selectedContact.ContactViewModel.Description
                });
                await _persistence.SaveContacts();
            }
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(ex);
            }
            finally
            {
                _notifyContactsChanged.Notify();
                _returnCommand?.Execute(null);
            }
        }
    }
}
