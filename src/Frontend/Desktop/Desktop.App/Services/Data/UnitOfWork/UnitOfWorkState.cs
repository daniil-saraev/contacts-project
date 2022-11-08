using System.Collections.Generic;

namespace Desktop.App.Services.Data.UnitOfWork
{
    public class UnitOfWorkState<T>
    {
        public IEnumerable<T> SyncedEntities { get; set; }
        public IEnumerable<T> NewEntities { get; set; }
        public IEnumerable<T> RemovedEntities { get; set; }

        public UnitOfWorkState()
        {
            SyncedEntities = new List<T>();
            NewEntities = new List<T>();
            RemovedEntities = new List<T>();
        }
    }
}
