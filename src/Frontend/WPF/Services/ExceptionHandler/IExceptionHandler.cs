using System;

namespace Desktop.Services.ExceptionHandler
{
    public interface IExceptionHandler
    {
        void HandleException(Exception exception);
    }
}