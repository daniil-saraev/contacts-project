using Core.Contacts.Models;
using Desktop.Common.Exceptions;

namespace Desktop.Contacts.Services
{
    internal interface ISyncService
    {
        /// <summary>
        /// Retrieves all user's contacts from remote repository.
        /// </summary>
        /// <exception cref="SyncingWithRemoteRepositoryException"></exception>
        Task<IEnumerable<ContactData>> Pull();

        /// <summary>
        /// Tries to push local changes to remote repository.
        /// </summary>
        /// <exception cref="SyncingWithRemoteRepositoryException"></exception>
        Task Push(UnitOfWorkState state);
    }
}
