using Core.Contacts.Models;
using Core.Contacts.Requests;
using Core.Common.Exceptions;

namespace Core.Contacts.Interfaces;

public interface IContactBookService
{
    /// <summary>
    /// Retrieves a collection of user's contacts.
    /// </summary>
    /// <returns>
    /// IEnumerable of <see cref="ContactData"/>.
    /// </returns>
    /// <exception cref="ApiException"></exception>
    public Task<IEnumerable<ContactData>> GetAllContacts();

    /// <summary>
    /// Retrieves a user's contact by id.
    /// </summary>
    /// <returns>
    /// <see cref="ContactData"/> if found.
    /// </returns>
    /// <exception cref="ApiException"></exception>
    public Task<ContactData> GetContactById(string id);

    /// <summary>
    /// Sends a request trying to delete a user's contact.
    /// </summary>
    /// <exception cref="ApiException"></exception>
    public Task DeleteContact(DeleteContactRequest request);

    /// <summary>
    /// Sends a request trying to update a user's contact.
    /// </summary>
    /// <returns><see cref="ContactData"/> of the updated contact.</returns>
    /// <exception cref="ApiException"></exception>
    public Task<ContactData> UpdateContact(UpdateContactRequest request);

    /// <summary>
    /// Sends a request trying to create a contact.
    /// </summary>
    /// <returns><see cref="ContactData"/> of the created contact.</returns>
    /// <exception cref="ApiException"></exception>
    public Task<ContactData> AddContact(AddContactRequest request);
}
