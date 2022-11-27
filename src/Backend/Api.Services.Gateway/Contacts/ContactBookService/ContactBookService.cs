using Core.Common.Constants;
using Core.Common.Exceptions;
using Core.Contacts.Interfaces;
using Core.Contacts.Models;
using Core.Contacts.Requests;

namespace Api.Services.Gateway.Contacts
{
    public class ContactBookService : IContactBookService
    {
        private readonly ContactsApi _contactsApi;
        private readonly HttpClient _httpClient;

        public ContactBookService( HttpClient httpClient)
        {
            _httpClient = httpClient;
            _contactsApi = new ContactsApi(BaseUrls.CONTACTS_DATABASE_API_URL, _httpClient);
        }

        public virtual async Task AddContact(AddContactRequest request)
        {
            try
            {
                await _contactsApi.AddAsync(request);
            }
            catch (Exception ex)
            {
                throw new ConnectionErrorException(ex);
            }
        }

        public virtual async Task DeleteContact(DeleteContactRequest request)
        {
            try
            {
                await _contactsApi.DeleteAsync(request);
            }
            catch (Exception ex)
            {
                throw new ConnectionErrorException(ex);
            }
        }

        public virtual async Task<IEnumerable<ContactData>> GetAllContacts()
        {
            try
            {
                return await _contactsApi.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ConnectionErrorException(ex);
            }
        }

        public virtual async Task<ContactData> GetContactById(string id)
        {
            try
            {
                return await _contactsApi.GetAsync(id);
            }
            catch (Exception ex)
            {
                throw new ConnectionErrorException(ex);
            }
        }

        public virtual async Task UpdateContact(UpdateContactRequest request)
        {
            try
            {
                await _contactsApi.UpdateAsync(request);
            }
            catch (Exception ex)
            {
                throw new ConnectionErrorException(ex);
            }
        }
    }
}
