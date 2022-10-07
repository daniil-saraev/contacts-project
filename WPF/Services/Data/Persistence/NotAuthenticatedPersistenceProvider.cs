using ApiServices.Interfaces;
using Desktop.Services.Data.FileServices;
using Desktop.Services.Data.Persistence.DataProviders;
using Desktop.Services.Data.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desktop.Services.Data.Persistence
{
    public class NotAuthenticatedPersistenceProvider : IPersistenceProvider
    {
        private readonly UnitOfWork<Contact> _contactsUnitOfWork;
        private readonly DiskProvider _diskProvider;

        public NotAuthenticatedPersistenceProvider(UnitOfWork<Contact> unitOfWork, IFileService<UnitOfWorkState<Contact>> fileService, IRepository<Contact> contactRepository)
        {
            _contactsUnitOfWork = unitOfWork;
            _diskProvider = new DiskProvider(fileService);
        }

        public Task AddContact(Contact contact)
        {
            _contactsUnitOfWork.RegisterNewEntity(contact);
            return Task.CompletedTask;
        }

        public Task UpdateContact(Contact initialContact, Contact updatedContact)
        {
            _contactsUnitOfWork.RegisterUpdatedEntity(initialContact, updatedContact);
            return Task.CompletedTask;
        }

        public Task RemoveContact(Contact contact) 
        {
            _contactsUnitOfWork.RegisterRemovedEntity(contact);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Contact>> LoadContactsAsync()
        {
            await LoadUnitOfWorkState();
            return _contactsUnitOfWork.CreateRelevantEntitiesList();
        }

        public Task SaveContactsAsync()
        {
            return _diskProvider.TrySaveToDisk(_contactsUnitOfWork.UnitOfWorkState);
        }

        private async Task LoadUnitOfWorkState()
        {
            UnitOfWorkState<Contact>? unitOfWorkState = await _diskProvider.TryLoadFromDisk();
            if (unitOfWorkState != null)
                _contactsUnitOfWork.UnitOfWorkState = unitOfWorkState;
        }
    }
}
