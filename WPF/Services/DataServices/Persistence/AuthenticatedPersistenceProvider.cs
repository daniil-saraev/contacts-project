using ApiServices.Interfaces;
using DatabaseApi;
using Desktop.Services.Authentication.UserServices;
using Desktop.Services.DataServices.FileServices;
using Desktop.Services.DataServices.UnitOfWork;
using Desktop.Services.ExceptionHandlers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Core.Models;

namespace Desktop.Services.DataServices.Persistence
{
    public class AuthenticatedPersistenceProvider : PersistenceProvider, IPersistenceProvider
    {
        private UnitOfWork<Contact> _contactsUnitOfWork;

        public AuthenticatedPersistenceProvider(UnitOfWork<Contact> unitOfWork, IFileService<UnitOfWork<Contact>> fileService, IRepository<Contact> contactRepository) : base(contactRepository, fileService)
        {
            _contactsUnitOfWork = unitOfWork;
        }

        public async Task AddContactAsync(Contact contact)
        {
            _contactsUnitOfWork.RegisterNewEntity(contact);
            await TrySyncWithRemoteRepositoryAsync(_contactsUnitOfWork);
        }

        public async Task UpdateContactAsync(Contact initialContact, Contact updatedContact)
        {
            _contactsUnitOfWork.RegisterUpdatedEntity(initialContact, updatedContact);
            await TrySyncWithRemoteRepositoryAsync(_contactsUnitOfWork);
        }

        public async Task RemoveContactAsync(Contact contact)
        {
            _contactsUnitOfWork.RegisterRemovedEntity(contact);
            await TrySyncWithRemoteRepositoryAsync(_contactsUnitOfWork);
        }

        public async Task<IEnumerable<Contact>> LoadContactsAsync()
        {
            await LoadUnitOfWork();
            return _contactsUnitOfWork.CreateRelevantEntitiesList();
        }

        public async Task SaveContactsAsync()
        {
            await Task.WhenAll(
            TrySaveToDiskAsync(_contactsUnitOfWork),
            TrySyncWithRemoteRepositoryAsync(_contactsUnitOfWork));
        }

        private async Task LoadUnitOfWork()
        {
            await LoadUnitOfWorkFromDisk();
            await PushLocalChangesToRemoteRepository();
            await LoadUnitOfWorkFromRemoteRepository();
        }

        private async Task LoadUnitOfWorkFromDisk()
        {
            UnitOfWork<Contact>? contactsFromDisk = await TryLoadFromDiskAsync();
            if (contactsFromDisk != null)
                _contactsUnitOfWork = contactsFromDisk;
        }

        private async Task LoadUnitOfWorkFromRemoteRepository()
        {
            UnitOfWork<Contact>? contactsFromRemoteRepository = await TryLoadFromRemoteRepositoryAsync();
            if (contactsFromRemoteRepository != null)
                _contactsUnitOfWork = contactsFromRemoteRepository;
        }

        private async Task PushLocalChangesToRemoteRepository()
        {
            if (!_contactsUnitOfWork.IsSynced)
                await TrySyncWithRemoteRepositoryAsync(_contactsUnitOfWork);
        }
    }
}
