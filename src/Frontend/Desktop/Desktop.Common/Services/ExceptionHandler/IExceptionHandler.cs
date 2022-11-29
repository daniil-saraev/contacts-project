using System;

namespace Desktop.Common.Services
{
    /// <summary>
    /// A service to implement application's logic of interaction with a user in case of core errors.
    /// </summary>
    public interface IExceptionHandler
    {
        void HandleException(Exception exception);
    }
}