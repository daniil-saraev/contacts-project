using System;

namespace Desktop.Common.Services
{
    public interface IExceptionHandler
    {
        void HandleException(Exception exception);
    }
}