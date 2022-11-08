using System;
using System.Windows;

namespace Desktop.App.Services.ExceptionHandler
{
    public class MessageBoxExceptionHandler : IExceptionHandler
    {
        public void HandleException(Exception exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
        }
    }
}