using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.App.Services.Data.Persistence.DiskProvider
{
    public interface IDiskProvider
    {
        Task<UnitOfWorkState<Contact>?> TryLoadFromDiskAsync();

        Task TrySaveToDiskAsync(UnitOfWorkState<Contact> unitOfWorkState);
    }
}
