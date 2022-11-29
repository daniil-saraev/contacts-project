using Core.Contacts.Exceptions;
using Core.Contacts.Models;
using Core.Contacts.Requests;
using Desktop.Contacts.Models;

namespace Desktop.Contacts.Services;

/// <summary>
/// Service to track changes of user's contacts.
/// </summary>
internal class ContactsUnitOfWork
{
    private UnitOfWorkState _state;

    /// <summary>
    /// Contains synced, new and changed contacts.
    /// </summary>
    public IEnumerable<ContactData> ExistingContacts => _state.ExistingUnits.Select(unit => unit.Contact);

    public UnitOfWorkState UnitOfWorkState
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;
        }
    }

    public ContactsUnitOfWork()
    {
        _state = new UnitOfWorkState();
    }

    /// <summary>
    /// Syncs incoming contacts with the existing.
    /// </summary>
    /// <param name="contacts">Contacts from remote repository.</param>
    public void AddContacts(IEnumerable<ContactData> contacts)
    {
        foreach(var contact in contacts)
        {
            // If the contact is already stored locally, wether synced or changed, don't override it.
            if(_state.ExistingUnits.Any(unit => unit.Id == contact.Id))
                continue;
            // If the contact was deleted locally, keep it as deleted.
            if(_state.PendingDeleteRequests.Any(request => request.Id == contact.Id))
                continue;

            // If the contact does not exist locally, add it as synced.
            _state.ExistingUnits.Add(new ContactUnit(contact, State.Synced));
        }
    }

    public ContactData AddContact(AddContactRequest request)
    {
        ContactData contact = new ContactData
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            Description = request.Description
        };
        ContactUnit unit = new ContactUnit(contact, State.New);
        _state.ExistingUnits.Add(unit);
        return unit.Contact;
    }

    public ContactData UpdateContact(UpdateContactRequest request)
    {
        var unit = _state.ExistingUnits.Find(unit => unit.Id == request.Id);
        if(unit != null)
        {
            unit.Contact = new ContactData
            {
                Id = request.Id,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Description = request.Description
            };
            if (unit.State == State.Synced)
                unit.State = State.Changed;
            return unit.Contact;
        }
        else
            throw new ContactNotFoundException();
    }

    public void DeleteContact(DeleteContactRequest request)
    {
        var unit = _state.ExistingUnits.Find(unit => unit.Id == request.Id);
        if(unit != null)
        {
            // If the contact was just created locally, there is no need to remove it from remote repository.
            if(unit.State == State.Synced || unit.State == State.Changed)
            {
                _state.PendingDeleteRequests.Add(request);
            }
            _state.ExistingUnits.Remove(unit);           
        }
        else
            throw new ContactNotFoundException();
    }
}