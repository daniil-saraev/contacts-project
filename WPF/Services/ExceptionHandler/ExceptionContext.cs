using System;

namespace Desktop.Services.ExceptionHandler
{
    public class ExceptionContext
    {
        public Exception Exception { get; }

        public ExceptionContext(Exception exception)
        {
            Exception = exception;
        }
    }
}
