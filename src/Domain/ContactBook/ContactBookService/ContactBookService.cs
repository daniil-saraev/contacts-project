using ContactBook.Exceptions;
using Core.Entities;
using ContactBook.Commands;
using ContactBook.Queries;

namespace ContactBook;

internal class ContactBookService : IContactBookService
{
    private readonly IAddContactCommand _addContactCommand;
    private readonly IDeleteContactCommand _deleteContactCommand;
    private readonly IUpdateContactCommand _updateContactCommand;
    private readonly IGetContactsQuery _getContactsQuery;

    public ContactBookService(IAddContactCommand addCommand, IDeleteContactCommand deleteCommand, IUpdateContactCommand updateCommand, IGetContactsQuery getQuery)
    {
        _addContactCommand = addCommand;
        _deleteContactCommand = deleteCommand;
        _updateContactCommand = updateCommand;
        _getContactsQuery = getQuery;
    }

    public async Task DeleteContact(Contact contact)
    {
        try
        {
            await _deleteContactCommand.Execute(contact);
        }
        catch (Exception)
        {
            throw new ContactNotFoundException();
        }
    }

    public async Task UpdateContact(Contact updatedContact)
    {
        try
        {
            await _updateContactCommand.Execute(updatedContact);
        }
        catch (Exception)
        {
            throw new ContactNotFoundException();
        }
    }

    public async Task AddContact(Contact contact)
    {
        try
        {
            await _addContactCommand.Execute(contact);
        }
        catch (Exception)
        {
            throw new ContactAlreadyExistsException();
        }
    }

    public async Task<IEnumerable<Contact>> GetContacts(string userId, Func<Contact, bool>? predicate = null)
    {
        return await _getContactsQuery.Execute(userId, predicate);
    }
}