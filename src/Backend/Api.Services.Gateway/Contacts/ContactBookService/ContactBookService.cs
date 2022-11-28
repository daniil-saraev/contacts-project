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

        public async Task<ContactData> AddContact(AddContactRequest request)
        {
            try
            {
                return await _contactsApi.AddAsync(request);
            }
            catch (Exception ex)
            {
                throw new ServerErrorException(ex);
            }
        }

        public async Task DeleteContact(DeleteContactRequest request)
        {
            try
            {
                await _contactsApi.DeleteAsync(request);
            }
            catch (Exception ex)
            {
                throw new ServerErrorException(ex);
            }
        }

        public async Task<IEnumerable<ContactData>> GetAllContacts()
        {
            try
            {
                return await _contactsApi.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ServerErrorException(ex);
            }
        }

        public async Task<ContactData> GetContactById(string id)
        {
            try
            {
                return await _contactsApi.GetAsync(id);
            }
            catch (Exception ex)
            {
                throw new ServerErrorException(ex);
            }
        }

        public async Task<ContactData> UpdateContact(UpdateContactRequest request)
        {
            try
            {
                return await _contactsApi.UpdateAsync(request);
            }
            catch (Exception ex)
            {
                throw new ServerErrorException(ex);
            }
        }
    }
}
