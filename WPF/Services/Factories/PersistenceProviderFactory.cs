using ApiServices.Interfaces;
using Desktop.Services.Authentication.UserServices;
using Desktop.Services.Data.FileServices;
using Desktop.Services.Data.Persistence;
using Desktop.Services.Data.UnitOfWork;

namespace Desktop.Services.Factories
{
    public class PersistenceProviderFactory
    {
        private readonly IRepository<Contact> _contactRepository;
        private readonly IFileService<UnitOfWorkState<Contact>> _fileService;
        private readonly UnitOfWork<Contact> _unitOfWork;

        public PersistenceProviderFactory(IRepository<Contact> contactRepository, IFileService<UnitOfWorkState<Contact>> fileService, UnitOfWork<Contact> unitOfWork)
        {
            _contactRepository = contactRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
        }

        public IPersistenceProvider GetPersistenceProvider()
        {
            if (User.IsAuthenticated)
                return new AuthenticatedPersistenceProvider(_unitOfWork, _fileService, _contactRepository);
            else
                return new NotAuthenticatedPersistenceProvider(_unitOfWork, _fileService, _contactRepository);
        }
    }
}
