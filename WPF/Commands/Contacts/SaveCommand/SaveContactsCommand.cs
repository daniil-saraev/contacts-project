using Desktop.Services.Containers;
using System;
using System.Threading.Tasks;

namespace Desktop.Commands.Contacts.SaveCommand
{
    public class SaveContactsCommand : ISaveCommand
    {
        private readonly IContactsStore _contactsStore;

        public SaveContactsCommand(IContactsStore contactsStore)
        {
            _contactsStore = contactsStore;
        }

        public async Task Execute()
        {
            try
            {
                await _contactsStore.SaveContactsAsync();
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
