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
            return _contactsUnitOfWork.CreateRelevantEntitiesList();
        }

        new public async Task SaveContactsAsync()
        {
            try
            {
                await SyncChangesWithRemoteRepositoryIfAnyAsync();
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
            try
            {
                await LoadUnitOfWorkStateFromDiskAsync();
                await SyncChangesWithRemoteRepositoryIfAnyAsync();
                await LoadUnitOfWorkStateFromRemoteRepositoryAsync();
            }
            catch (Exception)
            {
                return;
            }       
        }

        private async Task LoadUnitOfWorkStateFromDiskAsync()
        {
            try
            {
                UnitOfWorkState<Contact>? unitOfWorkState = await _diskProvider.TryLoadFromDiskAsync();
                if (unitOfWorkState != null)
                    _contactsUnitOfWork.UnitOfWorkState = unitOfWorkState;
            }
            catch (Exception)
            {
                return;
            }           
        }

        private async Task SyncChangesWithRemoteRepositoryIfAnyAsync()
        {
            try
            {
                if (!_contactsUnitOfWork.IsSynced)
                {
                    await _remoteRepositoryProvider.TryPushChangesToRemoteRepositoryAsync(_contactsUnitOfWork.UnitOfWorkState);
                    _contactsUnitOfWork.SyncUnitOfWorkState();
                }
            }
            catch (Exception)
            {
                throw;
            }                     
        }

        private async Task LoadUnitOfWorkStateFromRemoteRepositoryAsync()
        {
            try
            {
                UnitOfWorkState<Contact>? unitOfWorkState = await _remoteRepositoryProvider.TryLoadFromRemoteRepositoryAsync();
                if (unitOfWorkState != null)
                    _contactsUnitOfWork.UnitOfWorkState = unitOfWorkState;
            }
            catch (Exception)
            {
                return;
            }       
        }
    }
}
