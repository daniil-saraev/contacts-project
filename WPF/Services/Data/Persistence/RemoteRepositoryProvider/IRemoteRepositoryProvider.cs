using Desktop.Services.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Services.Data.Persistence.RemoteRepositoryProvider
{
    public interface IRemoteRepositoryProvider
    {
        Task<UnitOfWorkState<Contact>?> TryLoadFromRemoteRepositoryAsync();

        Task TryPushChangesToRemoteRepositoryAsync(UnitOfWorkState<Contact> unitOfWorkState);
    }
}
