using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Desktop.ViewModels.Contacts;
using Core.Interfaces;
using ApiServices;
using Desktop.Services;
using Desktop.Factories;

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
            try
            {
                IEnumerable<Contact>? contacts = await _contactsDb.GetAllAsync();
                if (contacts != null)
                    return contacts.Select(c => ContactMVMFactory.GetContactViewModel(c));
                return null;
            }
            catch (Exception ex)
            {
                //todo
            }
            return null;
        }
    }
}