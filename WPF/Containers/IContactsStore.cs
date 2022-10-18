using Desktop.Services.Data.Persistence;
using System.Threading.Tasks;

namespace Desktop.Services.Containers
{
    public interface IContactsStore
    {
        public void RemoveContact(Contact contact);

        public void UpdateContact(Contact initialContact, Contact updatedContact);

        public void AddContact(Contact contact);

        public Task LoadContactsAsync();

        public Task SaveContactsAsync();
    }
}