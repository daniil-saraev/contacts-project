using Core.Contacts.Exceptions;
using Core.Contacts.Models;
using Core.Contacts.Requests;
using Desktop.Contacts.Models;

namespace Desktop.Contacts.Services;

internal class ContactsUnitOfWork
{
    private UnitOfWorkState _state;
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

    public void AddContact(AddContactRequest request)
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
    }

    public void UpdateContact(UpdateContactRequest request)
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
        }
        else
            throw new ContactNotFoundException();
    }

    public void DeleteContact(DeleteContactRequest request)
    {
        var unit = _state.ExistingUnits.Find(unit => unit.Id == request.Id);
        if(unit != null)
        {
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