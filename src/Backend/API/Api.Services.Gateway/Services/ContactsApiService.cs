using Api.Services.Gateway.Contacts;
using Core.Entities;
using Core.Interfaces;

namespace Api.Services.Gateway.Services
{
    public class ContactsApiService : IContactsRepository
    {
        private readonly ContactsApi _contactsDbApi;
        private readonly HttpClient _httpClient;

        public ContactsApiService(string baseUrl, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _contactsDbApi = new ContactsApi(baseUrl, _httpClient);
        }

        public async Task AddAsync(Contact entity, CancellationToken cancellationToken = default)
        {
            await _contactsDbApi.AddAsync(Map(entity), cancellationToken);
        }

        public async Task DeleteAsync(Contact entity, CancellationToken cancellationToken = default)
        {
            await _contactsDbApi.DeleteAsync(Map(entity), cancellationToken);
        }

        public async Task<IEnumerable<Contact>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return (await _contactsDbApi.GetAllAsync(cancellationToken)).Select(contactDto => Map(contactDto));
        }

        public async Task<Contact?> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            return Map(await _contactsDbApi.GetAsync(id, cancellationToken));
        }

        public async Task UpdateAsync(Contact entity, CancellationToken cancellationToken = default)
        {
            await _contactsDbApi.UpdateAsync(Map(entity), cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<Contact> entities, CancellationToken cancellationToken = default)
        {
            await _contactsDbApi.AddRangeAsync(entities.Select(contact => Map(contact)), cancellationToken);
        }

        public async Task UpdateRangeAsync(IEnumerable<Contact> entities, CancellationToken cancellationToken = default)
        {
            await _contactsDbApi.UpdateRangeAsync(entities.Select(contact => Map(contact)), cancellationToken);
        }

        public async Task DeleteRangeAsync(IEnumerable<Contact> entities, CancellationToken cancellationToken = default)
        {
            await _contactsDbApi.DeleteRangeAsync(entities.Select(contact => Map(contact)), cancellationToken);
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
