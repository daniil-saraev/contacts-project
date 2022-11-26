using System;

namespace Desktop.Common.Exceptions
{
    public class SyncingWithRemoteRepositoryException : Exception
    {
        public SyncingWithRemoteRepositoryException() : base("Error occurred while syncing data") { }

        public SyncingWithRemoteRepositoryException(string message) : base(message) { }

        public SyncingWithRemoteRepositoryException(Exception inner) : base("Error occurred while syncing data", inner) { }
    }
}