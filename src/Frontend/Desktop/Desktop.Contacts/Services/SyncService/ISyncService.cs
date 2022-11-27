using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Contacts.Services.SyncService
{
    internal interface ISyncService
    {
        Task<UnitOfWorkState> Sync(UnitOfWorkState state);
    }
}
