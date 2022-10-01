using System.Windows;

namespace Desktop.Services.ExceptionHandlers
{
    public class DefaultExceptionHandler : IExceptionHandler
    {
        public void HandleException(ExceptionContext exceptionContext)
        {
            MessageBox.Show(exceptionContext.Exception.Message, exceptionContext.Exception.Source, MessageBoxButton.OK, MessageBoxImage.Error);
            MessageBox.Show(exceptionContext.Exception.StackTrace);
        }
    }
}
