using Desktop.Services.Data.Persistence;
using Desktop.Services.Data.Persistence.DiskProvider;
using Desktop.Services.Data.Persistence.RemoteRepositoryProvider;
using Desktop.Services.Data.UnitOfWork;

namespace Desktop.Services.Factories
{
    public class PersistenceProvidersFactory
    {
        private readonly IDiskProvider _diskProvider;
        private readonly IRemoteRepositoryProvider _remoteRepositoryProvider;
        private readonly UnitOfWork<Contact> _unitOfWork;

        public PersistenceProvidersFactory(IDiskProvider diskProvider, IRemoteRepositoryProvider remoteRepositoryProvider, UnitOfWork<Contact> unitOfWork)
        {
            _diskProvider = diskProvider;
            _remoteRepositoryProvider = remoteRepositoryProvider;
            _unitOfWork = unitOfWork;
        }

        public IPersistenceProvider GetNotAuthenticatedPersistenceProvider()
        {
            return new NotAuthenticatedPersistenceProvider(_unitOfWork, _diskProvider);
        }

        public IPersistenceProvider GetAuthenticatedPersistenceProvider()
        {
            return new AuthenticatedPersistenceProvider(_unitOfWork, _diskProvider, _remoteRepositoryProvider);
        }
    }
}
