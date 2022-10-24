using Core.Interfaces;
using Desktop.Exceptions;
using Desktop.Services.Data.UnitOfWork;
using System;
using System.Threading.Tasks;

namespace Desktop.Services.Data.Persistence.RemoteRepositoryProvider
{
    public class RemoteRepositoryProvider : IRemoteRepositoryProvider
    {
        private readonly IRepository<Contact> _contactRepository;

        public RemoteRepositoryProvider(IRepository<Contact> contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public Task<UnitOfWorkState<Contact>?> TryLoadFromRemoteRepositoryAsync()
        {
            return Task.Run(async () =>
            {
                UnitOfWorkState<Contact>? unitOfWorkState = null;
                try
                {
                    var contacts = await _contactRepository.GetAllAsync();
                    if (contacts != null)
                    {
                        unitOfWorkState = new UnitOfWorkState<Contact>();
                        unitOfWorkState.SyncedEntities = contacts;
                    }
                    return unitOfWorkState;
                }
                catch (Exception)
                {
                    throw new SyncingWithRemoteRepositoryException();
                }
            });
        }

        public Task TryPushChangesToRemoteRepositoryAsync(UnitOfWorkState<Contact> unitOfWorkState)
        {
            return Task.Run(async () =>
            {
                try
                {
                    await _contactRepository.DeleteRangeAsync(unitOfWorkState.RemovedEntities);
                    await _contactRepository.AddRangeAsync(unitOfWorkState.NewEntities);
                }
                catch (Exception)
                {
                    throw new SyncingWithRemoteRepositoryException();
                }
            });
        }
    }
}
