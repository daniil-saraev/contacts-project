using Desktop.Services.Containers;
using System;
using System.Threading.Tasks;

namespace Desktop.Commands.Contacts.LoadCommand
{
    public class LoadContactsCommand : ILoadCommand
    {
        private readonly IContactsStore _contactsStore;

        public LoadContactsCommand(IContactsStore contactsStore)
        {
            _contactsStore = contactsStore;
        }

        public async Task Execute()
        {
            try
            {
                await _contactsStore.LoadContactsAsync();
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
