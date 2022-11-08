using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.App.Services.Data.Persistence.RemoteRepositoryProvider
{
    public interface IRemoteRepositoryProvider
    {
        Task<UnitOfWorkState<Contact>?> TryLoadFromRemoteRepositoryAsync();

        Task TryPushChangesToRemoteRepositoryAsync(UnitOfWorkState<Contact> unitOfWorkState);
    }
}
