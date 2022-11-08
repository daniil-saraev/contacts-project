using System;

namespace Desktop.App.Services.ExceptionHandler
{
    public interface IExceptionHandler
    {
        void HandleException(Exception exception);
    }
}