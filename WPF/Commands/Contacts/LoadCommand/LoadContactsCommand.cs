using Desktop.Services.Data;
using Desktop.Services.ExceptionHandlers;
using System;
using System.Threading.Tasks;

namespace Desktop.Commands.Contacts.LoadCommand
{
    public class LoadContactsCommand : ILoadCommand
    {
        private readonly ContactsStore _contactsStore;
        private readonly IExceptionHandler _exceptionHandler;

        public LoadContactsCommand(ContactsStore contactsStore, IExceptionHandler exceptionHandler)
        {
            _contactsStore = contactsStore;
            _exceptionHandler = exceptionHandler;
        }

        public async Task Execute()
        {
            try
            {
                await _contactsStore.LoadContactsAsync();
            }
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(ex);
            }
        }
    }
}
