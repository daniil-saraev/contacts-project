using System;
using System.Windows;
using Desktop.Common.Services;

namespace Desktop.Main.Common.Services
{
    public class MessageBoxExceptionHandler : IExceptionHandler
    {
        public void HandleException(Exception exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
        }
    }
}