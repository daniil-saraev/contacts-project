using ApiServices.Interfaces;
using Desktop.Services.Data.FileServices;
using Desktop.Services.Data.Persistence.DataProviders;
using Desktop.Services.Data.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desktop.Services.Data.Persistence
{
    public class AuthenticatedPersistenceProvider : IPersistenceProvider
    {
        private readonly UnitOfWork<Contact> _contactsUnitOfWork;
        private readonly DiskProvider _diskProvider;
        private readonly RemoteRepositoryProvider _remoteRepositoryProvider;

        public AuthenticatedPersistenceProvider(UnitOfWork<Contact> unitOfWork, IFileService<UnitOfWorkState<Contact>> fileService, IRepository<Contact> contactRepository)
        {
            _contactsUnitOfWork = unitOfWork;
            _diskProvider = new DiskProvider(fileService);
            _remoteRepositoryProvider = new RemoteRepositoryProvider(contactRepository);
        }

        public Task AddContact(Contact contact)
        {
            _contactsUnitOfWork.RegisterNewEntity(contact);
            return _remoteRepositoryProvider.TrySyncWithRemoteRepository(_contactsUnitOfWork.UnitOfWorkState);
        }

        public Task UpdateContact(Contact initialContact, Contact updatedContact)
        {
            _contactsUnitOfWork.RegisterUpdatedEntity(initialContact, updatedContact);
            return _remoteRepositoryProvider.TrySyncWithRemoteRepository(_contactsUnitOfWork.UnitOfWorkState);
        }

        public Task RemoveContact(Contact contact)
        {
            _contactsUnitOfWork.RegisterRemovedEntity(contact);
            return _remoteRepositoryProvider.TrySyncWithRemoteRepository(_contactsUnitOfWork.UnitOfWorkState);
        }

        public async Task<IEnumerable<Contact>> LoadContactsAsync()
        {
            await LoadUnitOfWorkStateAsync();
            return _contactsUnitOfWork.CreateRelevantEntitiesList();
        }

        public async Task SaveContactsAsync()
        {
            await Task.WhenAll(
            _diskProvider.TrySaveToDisk(_contactsUnitOfWork.UnitOfWorkState),
            _remoteRepositoryProvider.TrySyncWithRemoteRepository(_contactsUnitOfWork.UnitOfWorkState));  
            SyncLocalUnitOfWork();
        }

        private async Task LoadUnitOfWorkStateAsync()
        {
            await LoadUnitOfWorkStateFromDiskAsync();
            await PushLocalChangesToRemoteRepositoryAsync();
            await LoadUnitOfWorkStateFromRemoteRepositoryAsync();
        }

        private async Task LoadUnitOfWorkStateFromDiskAsync()
        {
            UnitOfWorkState<Contact>? unitOfWorkState = await _diskProvider.TryLoadFromDisk();
            if (unitOfWorkState != null)
                _contactsUnitOfWork.UnitOfWorkState = unitOfWorkState;
        }

        private async Task PushLocalChangesToRemoteRepositoryAsync()
        {
            if (!_contactsUnitOfWork.IsSynced)
                await _remoteRepositoryProvider.TrySyncWithRemoteRepository(_contactsUnitOfWork.UnitOfWorkState);
        }

        private async Task LoadUnitOfWorkStateFromRemoteRepositoryAsync()
        {
            UnitOfWorkState<Contact>? unitOfWorkState = await _remoteRepositoryProvider.TryLoadFromRemoteRepository();
            if (unitOfWorkState != null)
                _contactsUnitOfWork.UnitOfWorkState = unitOfWorkState;
        }

        private void SyncLocalUnitOfWork()
        {
            foreach (var contact in _contactsUnitOfWork.DirtyEntities)
                _contactsUnitOfWork.RegisterSyncedEntity(contact);
            foreach (var contact in _contactsUnitOfWork.NewEntities)
                _contactsUnitOfWork.RegisterSyncedEntity(contact);
            foreach (var contact in _contactsUnitOfWork.RemovedEntities)
                _contactsUnitOfWork.RegisterSyncedEntity(contact);
        }
    }
}
