using Core.Contacts.Interfaces;
using Core.Contacts.Models;
using Core.Contacts.Requests;
using Desktop.Common.Exceptions;
using Desktop.Contacts.Models;

namespace Desktop.Contacts.Services;

internal class AuthenticatedContactBookService : NotAuthenticatedContactBookService
{
    private readonly IContactBookService _contactBookApi;

    public AuthenticatedContactBookService(IContactBookService contactBookService, 
                                            ILocalContactsStorage localContactsStorage, 
                                            ContactsUnitOfWork unitOfWork) : base(localContactsStorage, unitOfWork)
    {
        _contactBookApi = contactBookService;
    }

    public override async Task<IEnumerable<ContactData>> GetAllContacts()
    {
        if(!_unitOfWork.IsSynced)
        {
            try
            {
                _unitOfWork.UnitOfWorkState = await Sync(_unitOfWork.UnitOfWorkState);                
            }
            catch (Exception)
            {
                throw new SyncingWithRemoteRepositoryException();
            }
            finally
            {
                await _localStorage.Save(_unitOfWork.UnitOfWorkState);
            }
        }            
        return await base.GetAllContacts();
    }

    public override async Task LoadContacts()
    {
        var unitOfWorkState = await _localStorage.Load();
        if(unitOfWorkState != null)
        {
            try 
            {
                _unitOfWork.UnitOfWorkState = await Sync(unitOfWorkState);
            }
            catch(Exception)
            {
                _unitOfWork.UnitOfWorkState = unitOfWorkState;
                throw new SyncingWithRemoteRepositoryException();
            }
        }
    }

    public override async Task SaveContacts()
    {  
        try 
        {
            _unitOfWork.UnitOfWorkState = await Sync(_unitOfWork.UnitOfWorkState);
        }
        catch(Exception)
        {
            throw new SyncingWithRemoteRepositoryException();
        }
        finally
        {
            await base.SaveContacts();
        }
    }

    private async Task<UnitOfWorkState> Sync(UnitOfWorkState state)
    {
        foreach(var unit in state.ExistingUnits)
        {
            if(unit.State == State.New)
            {
                await SendAddRequest(unit);
            }
            if(unit.State == State.Changed)
            {
                await SendUpdateRequest(unit);
            }
        }

        while(state.PendingDeleteRequests.Count > 0)
        {
            await _contactBookApi.DeleteContact(state.PendingDeleteRequests.Peek());
            state.PendingDeleteRequests.Dequeue();
        }

        var contacts = await _contactBookApi.GetAllContacts();
        return new UnitOfWorkState
        {
            ExistingUnits = contacts.Select(contact => new ContactUnit(contact, State.Synced)).ToList()
        };
    }

    private async Task SendAddRequest(ContactUnit unit)
    {
        await _contactBookApi.AddContact(new AddContactRequest
        {
            FirstName = unit.Contact.FirstName,
            MiddleName = unit.Contact.MiddleName,
            LastName = unit.Contact.LastName,
            PhoneNumber = unit.Contact.PhoneNumber,
            Address = unit.Contact.Address,
            Description = unit.Contact.Description
        });
        unit.State = State.Synced;
    }

    private async Task SendUpdateRequest(ContactUnit unit)
    {
        await _contactBookApi.UpdateContact(new UpdateContactRequest
        {
            Id = unit.Contact.Id,
            FirstName = unit.Contact.FirstName,
            MiddleName = unit.Contact.MiddleName,
            LastName = unit.Contact.LastName,
            PhoneNumber = unit.Contact.PhoneNumber,
            Address = unit.Contact.Address,
            Description = unit.Contact.Description
        });
        unit.State = State.Synced;
    }
}