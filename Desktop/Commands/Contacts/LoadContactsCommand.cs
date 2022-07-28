using Desktop.ViewModels.Contacts;
using System;
using Desktop.Queries;
using System.Collections.Generic;
using Desktop.Queries.Contacts;

namespace Desktop.Commands.Contacts
{
    public class LoadContactsCommand : BaseCommand
    {
        private IEnumerable<ContactViewModel> _contacts;
        private IQuery<IEnumerable<ContactViewModel>> _getContactsQuery;
        public LoadContactsCommand(ref IEnumerable<ContactViewModel> contacts, Func<object?, bool>? canExecuteCustom = null) : base(canExecuteCustom)
        {
            _contacts = contacts;
            _getContactsQuery = new GetAllContactsQuery();
        }

        public override async void Execute(object? parameter)
        {
            var result = await _getContactsQuery.Execute();
            if(result != null)
                _contacts = result;
        }
    }
}
