using Desktop.Services.Data;
using Desktop.Services.ExceptionHandlers;
using System;
using System.Threading.Tasks;

namespace Desktop.Commands.Contacts.SaveCommand
{
    public class SaveContactsCommand : ISaveCommand
    {
        private readonly ContactsStore _contactsStore;
        private readonly IExceptionHandler _exceptionHandler;

        public SaveContactsCommand(ContactsStore contactsStore, IExceptionHandler exceptionHandler)
        {
            _contactsStore = contactsStore;
            _exceptionHandler = exceptionHandler;
        }

        public async Task Execute()
        {
            try
            {
                await _contactsStore.SaveContactsAsync();
            }
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(ex);
            }
        }
    }
}
