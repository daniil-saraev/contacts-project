using NuGet.Packaging;
using System.Collections.Generic;

namespace Desktop.Services.Data.UnitOfWork
{
    public class UnitOfWork<T>
    {
        private UnitOfWorkState<T> _unitOfWorkState;
        private List<T> _syncedEntities => _unitOfWorkState.SyncedEntities;
        private List<T> _newEntities => _unitOfWorkState.NewEntities;
        private List<T> _dirtyEntities => _unitOfWorkState.DirtyEntities;
        private List<T> _removedEntities => _unitOfWorkState.RemovedEntities;

        public UnitOfWorkState<T> UnitOfWorkState { get => _unitOfWorkState; set => _unitOfWorkState = value; }

        public IEnumerable<T> SyncedEntities => _unitOfWorkState.SyncedEntities;
        public IEnumerable<T> NewEntities => _unitOfWorkState.NewEntities;
        public IEnumerable<T> DirtyEntities => _unitOfWorkState.DirtyEntities;
        public IEnumerable<T> RemovedEntities => _unitOfWorkState.RemovedEntities;

        public bool IsSynced => _newEntities.Count == 0 && _dirtyEntities.Count == 0 && _removedEntities.Count == 0;

        public UnitOfWork()
        {
            _unitOfWorkState = new UnitOfWorkState<T>();
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

            if (_newEntities.Contains(entity))
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
            if (_dirtyEntities.Contains(entity))
            {
                _dirtyEntities.Remove(entity);
            }
            if (_newEntities.Contains(entity))
            {
                _newEntities.Remove(entity);
            }
        }

        public void RegisterUpdatedEntity(T previousEntity, T updatedEntity)
        {
            if (_syncedEntities.Contains(previousEntity))
            {
                _syncedEntities.Remove(previousEntity);
                _dirtyEntities.Add(updatedEntity);
            }
        }
    }
}
