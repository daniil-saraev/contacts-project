using Core.Contacts.Requests;
using Core.Contacts.Interfaces;
using Desktop.Common.Commands;
using Desktop.Common.Commands.Async;
using Desktop.Common.Services;
using Desktop.Contacts.Persistence;
using Desktop.Main.Contacts.Models;
using Desktop.Main.Contacts.Notifier;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Input;

namespace Desktop.Main.Contacts.Commands
{
    public class DeleteContactCommand : BaseCommand 
    {
        private IContactBookService _contactBook => ServiceProvider.GetRequiredService<IContactBookService>();
        private IPersistenceProvider _persistence => ServiceProvider.GetRequiredService<IPersistenceProvider>();
        private IExceptionHandler _exceptionHandler => ServiceProvider.GetRequiredService<IExceptionHandler>();
        private INotifyContactsChanged _notifyContactsChanged => ServiceProvider.GetRequiredService<INotifyContactsChanged>();

        private readonly SelectedContact _selectedContact;
        private readonly ICommand? _returnCommand;

        public DeleteContactCommand(SelectedContact selectedContact, ICommand? returnCommand)                         
        {
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
            return base.CanExecute(parameter) && !_selectedContact.SelectedContactIsNull;
        }

        public override async void Execute(object? parameter = null)
        {
            try
            {
                await _contactBook.DeleteContact(new DeleteContactRequest
                {
                    Id = _selectedContact.ContactViewModel.Id
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
