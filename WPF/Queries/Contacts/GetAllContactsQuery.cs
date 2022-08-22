using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Core.Interfaces;
using DatabaseApi;
using Desktop.Factories;
using Desktop.Services;
using Desktop.ViewModels.Contacts;
using Desktop.Services.ExceptionHandler;

namespace Desktop.Queries.Contacts
{
    public class GetAllContactsQuery : IQuery<IEnumerable<ContactViewModel>>
    {
        private readonly IRepository<Contact> _contactsDb;

        public GetAllContactsQuery()
        {
            _contactsDb = RepositoryService.GetRepository();
        }

        public async Task<IEnumerable<ContactViewModel>?> Execute(object? parameter = null)
        {
            IEnumerable<Contact>? contacts = await _contactsDb.GetAllAsync();
            if (contacts != null)
                return contacts.Select(c => ContactMVMFactory.GetContactViewModel(c));
            return null;         
        }
    }
}