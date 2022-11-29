using Core.Contacts.Models;

namespace Desktop.Contacts.Services;

/// <summary>
/// Designed to work with authenticated user. 
/// Extends <see cref="NotAuthenticatedContactBookService"/> providing synchronization functionality.
/// </summary>
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
            _unitOfWork.AddContacts(await _syncService.Pull());
            await _localStorage.Save(_unitOfWork.UnitOfWorkState);
            return await base.GetAllContacts();
        }
        catch (Exception)
        {
            return await base.GetAllContacts();
        }     
    }

    public override async Task LoadContacts()
    {
        try
        {
            var unitOfWorkState = await _localStorage.Load();
            if (unitOfWorkState == null)
                return;
            else _unitOfWork.UnitOfWorkState = unitOfWorkState;

        }
        finally
        {
            if (!_unitOfWork.UnitOfWorkState.IsSynced)
                await _syncService.Push(_unitOfWork.UnitOfWorkState);
            _unitOfWork.AddContacts(await _syncService.Pull());
        }
    }

    public override async Task SaveContacts()
    {  
        try 
        {
            await _syncService.Push(_unitOfWork.UnitOfWorkState);
        }
        finally
        {
            await base.SaveContacts();
        }
    }
}