using Core.Contacts.Models;
using Desktop.Common.Exceptions;
using Desktop.Contacts.Services.SyncService;

namespace Desktop.Contacts.Services;

internal class AuthenticatedContactBookService : NotAuthenticatedContactBookService
{
    private readonly ISyncService _syncService;

    public AuthenticatedContactBookService(ISyncService syncService, 
                                            ILocalContactsStorage localContactsStorage, 
                                            ContactsUnitOfWork unitOfWork) : base(localContactsStorage, unitOfWork)
    {
        _syncService = syncService;
    }

    public override async Task<IEnumerable<ContactData>> GetAllContacts()
    {
        try
        {
            _unitOfWork.UnitOfWorkState = await _syncService.Sync(_unitOfWork.UnitOfWorkState);
            await _localStorage.Save(_unitOfWork.UnitOfWorkState);
            return await base.GetAllContacts();
        }
        catch (SyncingWithRemoteRepositoryException)
        {
            return await base.GetAllContacts();
        }     
    }

    public override async Task LoadContacts()
    {
        var unitOfWorkState = await _localStorage.Load();
        if(unitOfWorkState == null)
            return;
        else
            _unitOfWork.UnitOfWorkState = unitOfWorkState;

        if (!unitOfWorkState.IsSynced)
            _unitOfWork.UnitOfWorkState = await _syncService.Sync(unitOfWorkState);
    }

    public override async Task SaveContacts()
    {  
        try 
        {
            _unitOfWork.UnitOfWorkState = await _syncService.Sync(_unitOfWork.UnitOfWorkState);
        }
        finally
        {
            await base.SaveContacts();
        }
    }
}