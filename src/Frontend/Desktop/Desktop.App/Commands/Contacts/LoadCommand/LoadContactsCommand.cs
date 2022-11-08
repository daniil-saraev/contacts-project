using System;
using System.Threading.Tasks;

namespace Desktop.App.Commands.Contacts.LoadCommand
{
    public class LoadContactsCommand : ILoadContactsCommand
    {
        private readonly IContactsStore _contactsStore;
        private readonly IExceptionHandler _exceptionHandler;

        public LoadContactsCommand(IContactsStore contactsStore, IExceptionHandler exceptionHandler)
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
