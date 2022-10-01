using NuGet.Packaging;
using System.Collections.Generic;

namespace Desktop.Services.DataServices.UnitOfWork
{
    public class UnitOfWork<T>
    {
        private readonly List<T> _syncedEntities;     
        private readonly List<T> _newEntities;
        private readonly List<T> _dirtyEntities;
        private readonly List<T> _removedEntities;

        public IEnumerable<T> SyncedEntities { get { return _syncedEntities; } }
        public IEnumerable<T> NewEntities { get { return _newEntities; } }
        public IEnumerable<T> DirtyEntities { get { return _dirtyEntities; } }
        public IEnumerable<T> RemovedEntities { get { return _removedEntities; } }

        public bool IsSynced => _newEntities.Count == 0 && _dirtyEntities.Count == 0 && _removedEntities.Count == 0;

        public UnitOfWork(IEnumerable<T> syncedEntities) 
        {
            _syncedEntities = new List<T>(syncedEntities);
            _newEntities = new List<T>();
            _dirtyEntities = new List<T>();
            _removedEntities = new List<T>();
        }

        public IEnumerable<T> CreateRelevantEntitiesList()
        {
            var relevantContacts = new List<T>();
            relevantContacts.AddRange(_syncedEntities);
            relevantContacts.AddRange(_newEntities);
            relevantContacts.AddRange(_dirtyEntities);
            return relevantContacts;
        }

        public void RegisterSyncedEntity(T entity)
        {
            if (_syncedEntities.Contains(entity))
                return;

            if (_dirtyEntities.Contains(entity))
            {
                _dirtyEntities.Remove(entity);
                _syncedEntities.Add(entity);
            }

            if(_newEntities.Contains(entity))
            {
                _newEntities.Remove(entity);
                _syncedEntities.Add(entity);
            }

            if (_removedEntities.Contains(entity))
                _removedEntities.Remove(entity);
        }

        public void RegisterNewEntity(T entity)
        {
            if (!_newEntities.Contains(entity))
            {
                _newEntities.Add(entity);
            }
        }

        public void RegisterRemovedEntity(T entity)
        {
            if (_syncedEntities.Contains(entity))
            {
                _syncedEntities.Remove(entity);
                _removedEntities.Add(entity);
            }
            if(_dirtyEntities.Contains(entity))
            {
                _dirtyEntities.Remove(entity);
            }
            if(_newEntities.Contains(entity))
            {
                _newEntities.Remove(entity);
            }
        }

        public void RegisterUpdatedEntity(T previousEntity, T updatedEntity)
        {
            if(_syncedEntities.Contains(previousEntity))
            {
                _syncedEntities.Remove(previousEntity);
                _dirtyEntities.Add(updatedEntity);
            }
        }
    }
}
