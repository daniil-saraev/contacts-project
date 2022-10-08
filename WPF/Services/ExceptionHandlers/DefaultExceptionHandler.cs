using System;
using System.Windows;

namespace Desktop.Services.ExceptionHandlers
{
    public class DefaultExceptionHandler : IExceptionHandler
    {
        public void HandleException(Exception exception)
        {
            MessageBox.Show(exception.Message, exception.Source, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
