using Desktop.Exceptions;
using Desktop.Services.Data.Persistence.DiskProvider;
using Desktop.Services.Data.Persistence.RemoteRepositoryProvider;
using Desktop.Services.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desktop.Services.Data.Persistence
{
    public class AuthenticatedPersistenceProvider : NotAuthenticatedPersistenceProvider
    {
        private readonly UnitOfWork<Contact> _contactsUnitOfWork;
        private readonly IDiskProvider _diskProvider;
        private readonly IRemoteRepositoryProvider _remoteRepositoryProvider;

        public AuthenticatedPersistenceProvider(UnitOfWork<Contact> unitOfWork, IDiskProvider diskProvider, IRemoteRepositoryProvider remoteRepositoryProvider)
            : base(unitOfWork, diskProvider)
        {
            _contactsUnitOfWork = unitOfWork;
            _diskProvider = diskProvider;
            _remoteRepositoryProvider = remoteRepositoryProvider;
        }

        new public async Task<IEnumerable<Contact>> LoadContactsAsync()
        {
            await LoadUnitOfWorkStateAsync();
            return _contactsUnitOfWork.CreateListOfSyncedAndNewEntities();
        }

        new public async Task SaveContactsAsync()
        {
            try
            {
                await SyncChangesWithRemoteRepositoryAsync();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await _diskProvider.TrySaveToDiskAsync(_contactsUnitOfWork.UnitOfWorkState);
            }       
        }

        private async Task LoadUnitOfWorkStateAsync()
        {
            await LoadUnitOfWorkStateFromDiskAsync();
            await SyncChangesWithRemoteRepositoryAsync();
            await LoadUnitOfWorkStateFromRemoteRepositoryAsync();  
        }

        private async Task LoadUnitOfWorkStateFromDiskAsync()
        {
            if(_contactsUnitOfWork.IsSynced)
            {
                UnitOfWorkState<Contact>? unitOfWorkState = await _diskProvider.TryLoadFromDiskAsync();
                if (unitOfWorkState != null)
                    _contactsUnitOfWork.UnitOfWorkState = unitOfWorkState;
            }        
        }

        private async Task SyncChangesWithRemoteRepositoryAsync()
        {
            if (!_contactsUnitOfWork.IsSynced)
            {
                await _remoteRepositoryProvider.TryPushChangesToRemoteRepositoryAsync(_contactsUnitOfWork.UnitOfWorkState);
                _contactsUnitOfWork.SyncUnitOfWorkState();
            }                   
        }

        private async Task LoadUnitOfWorkStateFromRemoteRepositoryAsync()
        {
            UnitOfWorkState<Contact>? unitOfWorkState = await _remoteRepositoryProvider.TryLoadFromRemoteRepositoryAsync();
            if (unitOfWorkState != null)
                _contactsUnitOfWork.UnitOfWorkState = unitOfWorkState;   
        }
    }
}
