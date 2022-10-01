using DatabaseApi;
using Desktop.Services.DataServices.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desktop.Services.DataServices.Persistence
{
    public interface IPersistenceProvider
    {
        Task AddContactAsync(Contact contact);

        Task UpdateContactAsync(Contact initialContact, Contact updatedContact);

        Task RemoveContactAsync(Contact contact);

        Task<IEnumerable<Contact>> LoadContactsAsync();

        Task SaveContactsAsync();
    }
}
