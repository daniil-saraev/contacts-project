using Core.Contacts.Models;

namespace Desktop.Contacts.Services
{
    internal interface ISyncService
    {
        Task<IEnumerable<ContactData>> Pull();

        Task Push(UnitOfWorkState state);
    }
}
