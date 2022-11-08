using System;

namespace ContactBook.Exceptions
{
    public class SyncingWithRemoteRepositoryException : Exception
    {
        public SyncingWithRemoteRepositoryException() : base("Error occurred while syncing data") { }

        public SyncingWithRemoteRepositoryException(string message) : base(message) { }
    }
}