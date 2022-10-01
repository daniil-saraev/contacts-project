using System;

namespace Desktop.Services.ExceptionHandlers
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
