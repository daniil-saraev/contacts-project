using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Desktop.Contacts.Services
{
    /// <summary>
    /// Service to manage user's contacts' data locally.
    /// </summary>
    internal interface ILocalContactsStorage
    {
        /// <summary>
        /// Tries to save contacts' data to disk.
        /// </summary>
        /// <exception cref="WritingDataException"></exception>
        public Task Save(UnitOfWorkState unitOfWorkState);

        /// <summary>
        /// Tries to load contacts' data from disk.
        /// </summary>
        /// <returns><see cref="UnitOfWorkState"/> if found, otherwise null.</returns>
        /// <exception cref="ReadingDataException"></exception>
        public Task<UnitOfWorkState?> Load();
    }
}
