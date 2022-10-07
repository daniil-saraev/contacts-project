using System.Collections.Generic;

namespace Desktop.Services.Data.UnitOfWork
{
    public class UnitOfWorkState<T>
    {
        public List<T> SyncedEntities { get; }
        public List<T> NewEntities { get; }
        public List<T> DirtyEntities { get; }
        public List<T> RemovedEntities { get; }

        public UnitOfWorkState()
        {
            SyncedEntities = new List<T>();
            NewEntities = new List<T>();
            DirtyEntities = new List<T>();
            RemovedEntities = new List<T>();
        }

        public UnitOfWorkState(IEnumerable<T> syncedEntities)
        {
            SyncedEntities = new List<T>(syncedEntities);
            NewEntities = new List<T>();
            DirtyEntities = new List<T>();
            RemovedEntities = new List<T>();
        }
    }
}
