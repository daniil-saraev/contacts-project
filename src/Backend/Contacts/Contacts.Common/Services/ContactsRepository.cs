using Contacts.Common.Data;
using Core.Common.Entities;
using Core.Common.Base;
using Core.Contacts.Interfaces;

namespace Contacts.Common.Services
{
    internal class ContactsRepository : BaseRepository<Contact>, IContactsRepository
    {
        public ContactsRepository(ContactsDbContext dbContext) : base(dbContext)
        {

        }
    }
}
