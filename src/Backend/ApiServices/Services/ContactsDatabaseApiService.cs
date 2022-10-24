using IdentityModel.Client;
using Core.Interfaces;
using ApiServices.Database;
using Core.Constants;
using Core.Models;

namespace ApiServices.Services
{
    public class ContactsDatabaseApiService : IRepository<Core.Models.Contact>, IApiService
    {
        private readonly DatabaseApi _contactsDbApi;
        private readonly HttpClient _httpClient;

        public ContactsDatabaseApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _contactsDbApi = new DatabaseApi(BaseUrls.CONTACTS_DATABASE_API_URL, _httpClient);
        }

        public void SetBearerToken(string token)
        {
            _httpClient.SetBearerToken(token);
        }

        public async Task AddAsync(Contact entity)
        {
            await _contactsDbApi.AddAsync(Map(entity));
        }

        public async Task DeleteAsync(Contact entity)
        {
            await _contactsDbApi.DeleteAsync(Map(entity));
        }

        public async Task<IEnumerable<Contact>?> GetAllAsync()
        {
            return (await _contactsDbApi.GetAllAsync()).Select(contactDto => Map(contactDto));
        }

        public async Task<Contact?> GetAsync(string id)
        {
            return Map((await _contactsDbApi.GetAsync(id)));
        }

        public async Task UpdateAsync(Contact entity)
        {
            await _contactsDbApi.UpdateAsync(Map(entity));
        }

        public async Task AddRangeAsync(IEnumerable<Contact> entities)
        {
            await _contactsDbApi.AddRangeAsync(entities.Select(contact => Map(contact)));
        }

        public async Task UpdateRangeAsync(IEnumerable<Contact> entities)
        {
            await _contactsDbApi.UpdateRangeAsync(entities.Select(contact => Map(contact)));
        }

        public async Task DeleteRangeAsync(IEnumerable<Contact> entities)
        {
            await _contactsDbApi.DeleteRangeAsync(entities.Select(contact => Map(contact)));
        }

        private static ContactDto Map(Contact entity)
        {
            var contactDto = new ContactDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                FirstName = entity.FirstName,
                MiddleName = entity.MiddleName,
                LastName = entity.LastName,
                PhoneNumber = entity.PhoneNumber,
                Description = entity.Description,
                Address = entity.Address
            };
            return contactDto;
        }

        private static Contact Map(ContactDto dto)
        {
            var contact = new Contact(
                dto.Id,
                dto.UserId,
                dto.FirstName,
                dto.MiddleName,
                dto.LastName,
                dto.PhoneNumber,
                dto.Address,
                dto.Description
            );
            return contact;
        }
    }
}
