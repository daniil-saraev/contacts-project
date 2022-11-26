using Core.Contacts.Models;
using Core.Contacts.Requests;

namespace Core.Contacts.Interfaces;

public interface IContactBookService
{
    public Task<IEnumerable<ContactData>> GetAllContacts();

    public Task<ContactData> GetContactById(string id);

    public Task DeleteContact(DeleteContactRequest request);

    public Task UpdateContact(UpdateContactRequest request);

    public Task AddContact(AddContactRequest request);
}
