using ApiServices.Interfaces;
using DatabaseApi;
using Desktop.Services.Authentication.UserServices;
using Desktop.Services.DataServices.FileServices;
using Desktop.Services.DataServices.UnitOfWork;
using Desktop.Services.ExceptionHandlers;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desktop.Services.DataServices.Persistence
{
    public class NotAuthenticatedPersistenceProvider : PersistenceProvider, IPersistenceProvider
    {
        private UnitOfWork<Contact> _contactsUnitOfWork;

        public NotAuthenticatedPersistenceProvider(UnitOfWork<Contact> unitOfWork, IFileService<UnitOfWork<Contact>> fileService, IRepository<Contact> contactRepository) : base(contactRepository, fileService)
        {
            _contactsUnitOfWork = unitOfWork;
        }

        public Task AddContactAsync(Contact contact)
        {
            _contactsUnitOfWork.RegisterNewEntity(contact);
            return Task.CompletedTask;
        }

        public Task UpdateContactAsync(Contact initialContact, Contact updatedContact)
        {
            _contactsUnitOfWork.RegisterUpdatedEntity(initialContact, updatedContact);
            return Task.CompletedTask;
        }

        public Task RemoveContactAsync(Contact contact)
        {
            _contactsUnitOfWork.RegisterRemovedEntity(contact);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Contact>> LoadContactsAsync()
        {
            await LoadUnitOfWork();
            return _contactsUnitOfWork.CreateRelevantEntitiesList();
        }

        public async Task SaveContactsAsync()
        {
            await TrySaveToDiskAsync(_contactsUnitOfWork);
        }

        private async Task LoadUnitOfWork()
        {
            UnitOfWork<Contact>? contactsFromDisk = await TryLoadFromDiskAsync();
            if (contactsFromDisk != null)
                _contactsUnitOfWork = contactsFromDisk;
        }
    }
}
