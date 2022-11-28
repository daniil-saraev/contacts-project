using Core.Contacts.Interfaces;
using Core.Contacts.Models;
using Core.Contacts.Requests;

namespace Desktop.Contacts.Services;

internal class NotAuthenticatedContactBookService : IContactBookService, IPersistenceProvider
{
    protected readonly ILocalContactsStorage _localStorage;
    protected readonly ContactsUnitOfWork _unitOfWork;

    public NotAuthenticatedContactBookService(ILocalContactsStorage localContactsStorage, ContactsUnitOfWork unitOfWork)
    {
        _localStorage = localContactsStorage;
        _unitOfWork = unitOfWork;
    }

    public Task<ContactData> AddContact(AddContactRequest request)
    {
        return Task.FromResult(_unitOfWork.AddContact(request));
    }

    public Task<ContactData> UpdateContact(UpdateContactRequest request)
    {
        return Task.FromResult(_unitOfWork.UpdateContact(request));
    }

    public Task DeleteContact(DeleteContactRequest request)
    {
        _unitOfWork.DeleteContact(request);
        return Task.CompletedTask;
    }

    public Task<ContactData> GetContactById(string id)
    {
        return Task.FromResult(_unitOfWork.ExistingContacts.First(unit => unit.Id == id));
    }

    public virtual Task<IEnumerable<ContactData>> GetAllContacts()
    {
        return Task.FromResult(_unitOfWork.ExistingContacts);
    }

    public virtual async Task LoadContacts()
    {
        var unitOfWorkState = await _localStorage.Load();
        if(unitOfWorkState != null)
        {
            _unitOfWork.UnitOfWorkState = unitOfWorkState;
        }
    }

    public virtual async Task SaveContacts()
    {
        await _localStorage.Save(_unitOfWork.UnitOfWorkState);
    }
}