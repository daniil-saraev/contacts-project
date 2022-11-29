using Desktop.Common.Exceptions;

namespace Desktop.Contacts.Services
{
    public interface IPersistenceProvider
    {
        /// <summary>
        /// If user is not authenticated, loads data from local storage. Otherwise,
        /// also tries to sync data with remote repository.
        /// </summary>
        /// <exception cref="ReadingDataException"/>
        /// <exception cref="SyncingWithRemoteRepositoryException"/>
        Task LoadContacts();

        /// <summary>
        /// If user is not authenticated, saves data locally. Otherwise,
        /// alse tries to save data to remote repository.
        /// </summary>
        /// <exception cref="WritingDataException"/>
        /// <exception cref="SyncingWithRemoteRepositoryException"/>
        Task SaveContacts();
    }
}
