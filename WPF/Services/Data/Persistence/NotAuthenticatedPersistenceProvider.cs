using ApiServices.Interfaces;
using Desktop.Exceptions;
using Desktop.Services.Data.FileServices;
using Desktop.Services.Data.Persistence.DiskProvider;
using Desktop.Services.Data.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desktop.Services.Data.Persistence
{
    public class NotAuthenticatedPersistenceProvider : IPersistenceProvider
    {
        private readonly UnitOfWork<Contact> _contactsUnitOfWork;
        private readonly IDiskProvider _diskProvider;

        public NotAuthenticatedPersistenceProvider(UnitOfWork<Contact> unitOfWork, IDiskProvider diskProvider)
        {
            _contactsUnitOfWork = unitOfWork;
            _diskProvider = diskProvider;
        }

        public void AddContact(Contact contact)
        {
            _contactsUnitOfWork.RegisterNewEntity(contact);
        }

        public void UpdateContact(Contact initialContact, Contact updatedContact)
        {
            _contactsUnitOfWork.RegisterUpdatedEntity(initialContact, updatedContact);
        }

        public void RemoveContact(Contact contact) 
        {
            _contactsUnitOfWork.RegisterRemovedEntity(contact);
        }

        public async Task<IEnumerable<Contact>> LoadContactsAsync()
        {
            await LoadUnitOfWorkState();
            return _contactsUnitOfWork.CreateRelevantEntitiesList();
        }

        public Task SaveContactsAsync()
        {
            return _diskProvider.TrySaveToDiskAsync(_contactsUnitOfWork.UnitOfWorkState);
        }

        private async Task LoadUnitOfWorkState()
        {
            try
            {
                UnitOfWorkState<Contact>? unitOfWorkState = await _diskProvider.TryLoadFromDiskAsync();
                if (unitOfWorkState != null)
                    _contactsUnitOfWork.UnitOfWorkState = unitOfWorkState;
            }
            catch (ReadingDataException)
            {
                return;
            }           
        }
    }
}
