using ApiServices.Interfaces;
using OpenApi;
using IdentityModel.Client;

namespace ApiServices.Services
{
    public class ContactsDatabaseApiService : IRepository<Contact>, IApiService
    {
        private readonly DatabaseApi _contactsDbApi;
        private readonly HttpClient _httpClient;

        public ContactsDatabaseApiService(string baseUrl)
        {
            _httpClient = new HttpClient();
            _contactsDbApi = new DatabaseApi(baseUrl, _httpClient);
        }

        public void InitializeToken(string token)
        {
            _httpClient.SetBearerToken(token);
        }

        public async Task AddAsync(Contact entity)
        {
            await _contactsDbApi.AddAsync(entity);
        }

        public async Task DeleteAsync(Contact entity)
        {
            await _contactsDbApi.DeleteAsync(entity);
        }

        public async Task<IEnumerable<Contact>?> GetAllAsync()
        {
            return await _contactsDbApi.GetAllAsync();
        }

        public async Task<Contact?> GetAsync(string id)
        {
            return await _contactsDbApi.GetAsync(id);
        }

        public async Task UpdateAsync(Contact entity)
        {
            await _contactsDbApi.UpdateAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<Contact> entities)
        {
            await _contactsDbApi.AddRangeAsync(entities);
        }

        public async Task UpdateRangeAsync(IEnumerable<Contact> entities)
        {
            await _contactsDbApi.UpdateRangeAsync(entities);
        }

        public async Task DeleteRangeAsync(IEnumerable<Contact> entities)
        {
            await _contactsDbApi.DeleteRangeAsync(entities);
        }
    }
}
