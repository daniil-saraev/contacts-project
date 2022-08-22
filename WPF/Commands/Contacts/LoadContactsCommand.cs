using System;
using Desktop.Services.ExceptionHandler;
using Desktop.Data;
using Desktop.Queries;
using System.Collections.Generic;
using Desktop.ViewModels.Contacts;
using Desktop.Queries.Contacts;
using NuGet.Packaging;

namespace Desktop.Commands.Contacts
{
    public class LoadContactsCommand : BaseCommand
    {
        private ContactsStore _store;
        private IQuery<IEnumerable<ContactViewModel>> _loadContacts;

        public LoadContactsCommand(ContactsStore contacts, Func<object?, bool>? canExecuteCustom = null) : base(canExecuteCustom)
        {
            _store = contacts;
            _loadContacts = new GetAllContactsQuery();
        }

        public override async void Execute(object? parameter)
        {
            try
            {
                var contacts = await _loadContacts.Execute();
                _store.Contacts.AddRange(contacts);
            }
            catch (Exception ex)
            {
                exceptionHandler?.HandleException(new ExceptionContext(ex));
            }
        }
    }
}
