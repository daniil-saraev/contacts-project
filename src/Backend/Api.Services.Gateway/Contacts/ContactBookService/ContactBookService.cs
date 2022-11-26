using Core.Contacts.Interfaces;
using Core.Contacts.Models;
using Core.Contacts.Requests;

namespace Api.Services.Gateway.Contacts
{
    public class ContactBookService : IContactBookService
    {
        private readonly ContactsApi _contactsApi;
        private readonly HttpClient _httpClient;

        public ContactBookService(string baseUrl, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _contactsApi = new ContactsApi(baseUrl, _httpClient);
        }

        public virtual async Task AddContact(AddContactRequest request)
        {
            await _contactsApi.AddAsync(request);
        }

        public virtual async Task DeleteContact(DeleteContactRequest request)
        {
            await _contactsApi.DeleteAsync(request);
        }

        public virtual async Task<IEnumerable<ContactData>> GetAllContacts()
        {
            return await _contactsApi.GetAllAsync();
        }

        public virtual async Task<ContactData> GetContactById(string id)
        {
            return await _contactsApi.GetAsync(id);
        }

        public virtual async Task UpdateContact(UpdateContactRequest request)
        {
            await _contactsApi.UpdateAsync(request);
        }
    }
}
