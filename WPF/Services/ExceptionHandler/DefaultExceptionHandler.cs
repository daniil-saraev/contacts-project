using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Desktop.Services.ExceptionHandler
{
    public class DefaultExceptionHandler : IExceptionHandler
    {
        public void HandleException(ExceptionContext exceptionContext)
        {
            MessageBox.Show(exceptionContext.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
