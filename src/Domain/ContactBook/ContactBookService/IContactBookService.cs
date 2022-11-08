using Core.Entities;

namespace ContactBook;

public interface IContactBookService
{
    public Task<IEnumerable<Contact>> GetContacts(string userId, Func<Contact, bool>? predicate = null);

    public Task DeleteContact(Contact contact);

    public Task UpdateContact(Contact updatedContact);

    public Task AddContact(Contact contact);
}
