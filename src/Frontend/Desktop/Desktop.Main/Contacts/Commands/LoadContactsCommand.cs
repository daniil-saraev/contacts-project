using Desktop.Common.Commands.Async;
using Desktop.Common.Services;
using Desktop.Contacts.Persistence;
using Desktop.Main.Contacts.Notifier;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Desktop.Main.Contacts.Commands
{
    internal class LoadContactsCommand : AsyncBaseCommand 
    {
        private IPersistenceProvider _persistence => ServiceProvider.GetRequiredService<IPersistenceProvider>();
        private IExceptionHandler _exceptionHandler => ServiceProvider.GetRequiredService<IExceptionHandler>();
        private INotifyContactsChanged _notifyContactsChanged => ServiceProvider.GetRequiredService<INotifyContactsChanged>();

        public LoadContactsCommand()
        {
        }

        public override async Task ExecuteAsync()
        {
            try
            {
                await _persistence.LoadContacts();
            }
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(ex);
            }
            finally
            {
                _notifyContactsChanged.Notify();
            }
        }
    }
}
