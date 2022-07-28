using ApiServices;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Services
{
    public static class RepositoryService
    {
        private static IRepository<Contact>? _currentRespoitory;

        public static void Initialize(IRepository<Contact> repository)
        {
            _currentRespoitory = repository;
        }

        public static IRepository<Contact> GetRepository()
        {
            if( _currentRespoitory != null )
                return _currentRespoitory;
            throw new Exception("Repository not initialized!");
        }
    }
}
