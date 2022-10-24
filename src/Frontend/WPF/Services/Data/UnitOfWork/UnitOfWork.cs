using NuGet.Packaging;
using System.Collections.Generic;
using System.Linq;

namespace Desktop.Services.Data.UnitOfWork
{
    public class UnitOfWork<T>
    {
        private readonly List<T> _syncedEntities;
        private readonly List<T> _newEntities;
        private readonly List<T> _removedEntities;

        public IEnumerable<T> SyncedEntities => _syncedEntities;
        public IEnumerable<T> NewEntities => _newEntities;
        public IEnumerable<T> RemovedEntities => _removedEntities;

        public bool IsSynced => _newEntities.Count == 0  && _removedEntities.Count == 0;

        public UnitOfWorkState<T> UnitOfWorkState 
        { 
            get 
            {
                return new UnitOfWorkState<T>()
                {
                    SyncedEntities = _syncedEntities,
                    NewEntities = _newEntities,
                    RemovedEntities = _removedEntities
                };
            }
            set
            {
                _syncedEntities.Clear();
                _syncedEntities.AddRange(value.SyncedEntities);
                _newEntities.Clear();
                _newEntities.AddRange(value.NewEntities);
                _removedEntities.Clear();
                _removedEntities.AddRange(value.RemovedEntities);
            }
        }

        public UnitOfWork()
        {
            _syncedEntities = new List<T>();
            _newEntities = new List<T>();
            _removedEntities = new List<T>();
        }

        public IEnumerable<T> CreateListOfSyncedAndNewEntities()
        {
            var relevantContacts = new List<T>();
            relevantContacts.AddRange(_syncedEntities);
            relevantContacts.AddRange(_newEntities);
            return relevantContacts;
        }

        public void RegisterSyncedEntity(T entity)
        {
            if (_syncedEntities.Contains(entity))
                return;

            if (_newEntities.Contains(entity))
            {
                _syncedEntities.Add(entity);
                _newEntities.Remove(entity);              
            }

            if (_removedEntities.Contains(entity))
                _removedEntities.Remove(entity);
        }

        public void RegisterNewEntity(T entity)
        {
            if (!_newEntities.Contains(entity) && !_syncedEntities.Contains(entity))
            {
                _newEntities.Add(entity);
            }
            if(_removedEntities.Contains(entity))
            {
                _removedEntities.Remove(entity);
            }
        }

        public void RegisterRemovedEntity(T entity)
        {
            if (_syncedEntities.Contains(entity))
            {
                _syncedEntities.Remove(entity);
                _removedEntities.Add(entity);
            }
            if (_newEntities.Contains(entity))
            {
                _newEntities.Remove(entity);
            }
        }

        public void RegisterUpdatedEntity(T initialEntity, T updatedEntity)
        {
            if (_syncedEntities.Contains(initialEntity))
            {
                _syncedEntities.Remove(initialEntity);
                _removedEntities.Add(initialEntity);
                _newEntities.Add(updatedEntity);
            }
            if(_newEntities.Contains(initialEntity))
            {
                _newEntities.Remove(initialEntity);
                _newEntities.Add(updatedEntity);
            }
        }

        public void SyncUnitOfWorkState()
        {
            foreach (var entity in _newEntities)
                _syncedEntities.Add(entity);
            _newEntities.Clear();
            _removedEntities.Clear();
        }
    }
}
