using ApiServices.Interfaces;
using Desktop.Services.Authentication.UserServices;
using Desktop.Services.Data.FileServices;
using Desktop.Services.Data.Persistence;
using Desktop.Services.Data.UnitOfWork;

namespace Desktop.Services.Factories
{
    public class PersistenceProvidersFactory
    {
        private readonly IRepository<Contact> _contactRepository;
        private readonly IFileService<UnitOfWorkState<Contact>> _fileService;
        private readonly UnitOfWork<Contact> _unitOfWork;

        public PersistenceProvidersFactory(IRepository<Contact> contactRepository, IFileService<UnitOfWorkState<Contact>> fileService, UnitOfWork<Contact> unitOfWork)
        {
            _contactRepository = contactRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
        }

        public IPersistenceProvider GetNotAuthenticatedPersistenceProvider()
        {
            return new NotAuthenticatedPersistenceProvider(_unitOfWork, _fileService);
        }

        public IPersistenceProvider GetAuthenticatedPersistenceProvider()
        {
            return new AuthenticatedPersistenceProvider(_unitOfWork, _fileService, _contactRepository);
        }
    }
}
