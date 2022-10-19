using System;

namespace Desktop.Exceptions
{
    public class SyncingWithRemoteRepositoryException : Exception
    {
        public SyncingWithRemoteRepositoryException() : base("Error occurred while syncing data") { }
    }
    

}