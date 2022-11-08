using Core.Entities;
using Desktop.Services.Data.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desktop.App.Services.Data.Persistence
{
    public interface IPersistenceProvider
    {
        void AddContact(Contact contact);

        void UpdateContact(Contact initialContact, Contact updatedContact);

        void RemoveContact(Contact contact);

        Task<IEnumerable<Contact>> LoadContactsAsync();

        Task SaveContactsAsync();
    }
}
