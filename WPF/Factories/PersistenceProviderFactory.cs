using ApiServices.Interfaces;
using Desktop.Services.Authentication.UserServices;
using Desktop.Services.DataServices.FileServices;
using Desktop.Services.DataServices.Persistence;
using Desktop.Services.DataServices.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Factories
{
    public class PersistenceProviderFactory
    {
        private readonly IRepository<Contact> _contactRepository;
        private readonly IFileService<UnitOfWork<Contact>> _fileService;
        private readonly UnitOfWork<Contact> _unitOfWork;

        public PersistenceProviderFactory(IRepository<Contact> contactRepository, IFileService<UnitOfWork<Contact>> fileService, UnitOfWork<Contact> unitOfWork)
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
