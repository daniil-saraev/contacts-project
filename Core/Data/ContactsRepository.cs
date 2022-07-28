using Core.Interfaces;
using Core.Models;

namespace Core.Data
{
    public class ContactsRepository /*: IRepository<ContactModel>*/
    {
        //private readonly ContactsDbApiClient _contactsDbApiClient;
        //private const string _apiBaseUrl = "https://localhost:7256/";       
        //private HttpClient _httpClient;

        //public string? CurrentUserId { get; set; }

        //public ContactsRepository()
        //{
        //    _httpClient = new HttpClient();            
        //    _contactsDbApiClient = new ContactsDbApiClient(_apiBaseUrl, _httpClient);
        //}

        //public async Task AddAsync(ContactModel contactModel)
        //{
        //    Contact contact = ContactFactory.CreateContact(contactModel);
        //    await _contactsDbApiClient.AddAsync(contact);
        //}

        //public async Task DeleteAsync(ContactModel contactModel)
        //{
        //    Contact contact = ContactFactory.CreateContact(contactModel);
        //    await _contactsDbApiClient.DeleteAsync(contact);
        //}

        //public async Task DeleteRangeAsync(IEnumerable<ContactModel> contactModels)
        //{
        //    IEnumerable<Contact> contacts = contactModels.Select(c => ContactFactory.CreateContact(c));
        //    await _contactsDbApiClient.DeleteRangeAsync(contacts);
        //}

        //public async Task<IEnumerable<ContactModel>?> GetAllAsync()
        //{
        //    if (CurrentUserId == null) return null;
        //    IEnumerable<Contact>? contacts = await _contactsDbApiClient.GetAllAsync(CurrentUserId);
        //    IEnumerable<ContactModel>? contactModels = contacts.Select(c => ContactFactory.CreateContactModel(c));
        //    return contactModels;
        //}

        //public async Task<ContactModel?> GetAsync(int id)
        //{
        //    Contact contact = await _contactsDbApiClient.GetAsync(id);
        //    ContactModel contactModel = ContactFactory.CreateContactModel(contact);
        //    return contactModel;
        //}

        //public async Task UpdateAsync(ContactModel contactModel)
        //{
        //    Contact contact = ContactFactory.CreateContact(contactModel);
        //    await _contactsDbApiClient.UpdateAsync(contact);
        //}
    }
}
