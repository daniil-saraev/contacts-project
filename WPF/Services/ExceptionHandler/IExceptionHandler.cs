using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Services.ExceptionHandler
{
    public interface IExceptionHandler
    {
        void HandleException (ExceptionContext exceptionContext);
    }
}
