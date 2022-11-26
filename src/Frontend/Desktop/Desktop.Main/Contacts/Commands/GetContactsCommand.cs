using Core.Contacts.Interfaces;
using Desktop.Common.Commands.Async;
using Desktop.Common.Services;
using Desktop.Main.Contacts.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desktop.Main.Contacts.Commands
{
    public class GetContactsCommand : AsyncBaseCommand<IEnumerable<ContactViewModel>?>
    {
        private IContactBookService _contactBook => ServiceProvider.GetRequiredService<IContactBookService>();
        private IExceptionHandler _exceptionHandler => ServiceProvider.GetRequiredService<IExceptionHandler>();

        public override async Task<IEnumerable<ContactViewModel>?> ExecuteAsync()
        {
            try
            {
                return (await _contactBook.GetAllContacts()).Select(contact => new ContactViewModel(contact));
            }
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(ex);
                return null;
            }           
        }
    }
}
