using ApiServices.Interfaces;
using Desktop.Exceptions;
using Desktop.Services.Data.UnitOfWork;
using System;
using System.Threading.Tasks;

namespace Desktop.Services.Data.Persistence.DataProviders
{
    public class RemoteRepositoryProvider
    {
        private readonly IRepository<Contact> _contactRepository;

        public RemoteRepositoryProvider(IRepository<Contact> contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public Task<UnitOfWorkState<Contact>?> TryLoadFromRemoteRepository()
        {
            return Task.Run(async () =>
            {
                UnitOfWorkState<Contact>? unitOfWorkState = null;
                try
                {
                    var contacts = await _contactRepository.GetAllAsync();
                    if (contacts != null)
                        unitOfWorkState = new UnitOfWorkState<Contact>(contacts);
                    return unitOfWorkState;
                }
                catch (Exception)
                {
                    throw new SyncingWithRemoteRepositoryException();
                }
            });
        }

        public Task TrySyncWithRemoteRepository(UnitOfWorkState<Contact> unitOfWorkState)
        {
            return Task.Run(async () =>
            {
                try
                {
                    await _contactRepository.UpdateRangeAsync(unitOfWorkState.DirtyEntities);
                    await _contactRepository.AddRangeAsync(unitOfWorkState.NewEntities);
                    await _contactRepository.DeleteRangeAsync(unitOfWorkState.RemovedEntities);
                }
                catch (Exception)
                {
                    throw new SyncingWithRemoteRepositoryException();
                }
            });
        }
    }
}
