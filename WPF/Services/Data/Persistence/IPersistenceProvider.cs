using OpenApi;
using Desktop.Services.Data.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desktop.Services.Data.Persistence
{
    public interface IPersistenceProvider
    {
        Task AddContact(Contact contact);

        Task UpdateContact(Contact initialContact, Contact updatedContact);

        Task RemoveContact(Contact contact);

        Task<IEnumerable<Contact>> LoadContactsAsync();

        Task SaveContactsAsync();
    }
}
