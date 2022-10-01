using ApiServices.Interfaces;
using Desktop.Services.DataServices.FileServices;
using Desktop.Services.DataServices.UnitOfWork;
using System;
using System.Threading.Tasks;

namespace Desktop.Services.DataServices.Persistence
{
    public abstract class PersistenceProvider
    {
        private readonly IRepository<Contact> _contactRepository;
        private readonly IFileService<UnitOfWork<Contact>> _fileService;

        public PersistenceProvider(IRepository<Contact> repository, IFileService<UnitOfWork<Contact>> fileService)
        {
            _contactRepository = repository;
            _fileService = fileService;
        }

        protected async Task<UnitOfWork<Contact>?> TryLoadFromDiskAsync()
        {
            UnitOfWork<Contact>? unitOfWork = null;
            return await Task.Run(() =>
            {
                try
                {
                    return unitOfWork = _fileService.Read();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        protected async Task TrySaveToDiskAsync(UnitOfWork<Contact> unitOfWork)
        {
            await Task.Run(() =>
            {
                try
                {
                    _fileService.Write(unitOfWork);
                }
                catch (Exception)
                {
                    return;
                }
            });
        }

        protected async Task<UnitOfWork<Contact>?> TryLoadFromRemoteRepositoryAsync()
        {
            UnitOfWork<Contact>? unitOfWork = null;
            try
            {
                var contacts = await _contactRepository.GetAllAsync();
                if (contacts != null)
                    unitOfWork = new UnitOfWork<Contact>(contacts);
                return unitOfWork;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected async Task TrySyncWithRemoteRepositoryAsync(UnitOfWork<Contact> unitOfWork)
        {
            try
            {
                await UpdateRemoteRepositoryAsync(unitOfWork);
                UpdateLocalRepository(unitOfWork);
            }
            catch (Exception)
            {
                return;
            }
        }

        private async Task UpdateRemoteRepositoryAsync(UnitOfWork<Contact> unitOfWork)
        {
            await _contactRepository.UpdateRangeAsync(unitOfWork.DirtyEntities);
            await _contactRepository.AddRangeAsync(unitOfWork.NewEntities);
            await _contactRepository.DeleteRangeAsync(unitOfWork.RemovedEntities);
        }

        private void UpdateLocalRepository(UnitOfWork<Contact> unitOfWork)
        {
            foreach (var contact in unitOfWork.DirtyEntities)
                unitOfWork.RegisterSyncedEntity(contact);
            foreach (var contact in unitOfWork.NewEntities)
                unitOfWork.RegisterSyncedEntity(contact);
            foreach (var contact in unitOfWork.RemovedEntities)
                unitOfWork.RegisterSyncedEntity(contact);
        }
    }
}
