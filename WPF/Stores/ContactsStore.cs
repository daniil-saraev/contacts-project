using DatabaseApi;
using Desktop.Factories;
using Desktop.Services.Authentication.UserServices;
using Desktop.Services.DataServices.Persistence;
using Desktop.Services.DataServices.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desktop.Stores
{
    /// <summary>
    /// Temporary storage for all contacts the user is working with. Is a singleton.
    /// </summary>
    public class ContactsStore : IContactsStore
    {
        private static ContactsStore _contactsStoreInstance;      
        private readonly List<Contact> _contacts;       
        private readonly PersistenceProviderFactory _persistenceProviderFactory;
        private IPersistenceProvider _persistenceProvider;

        public event Action? CollectionChanged;

        public static void InitializeStore(PersistenceProviderFactory persistenceProviderFactory)
        {
            _contactsStoreInstance = new ContactsStore(persistenceProviderFactory);
        }

        public static ContactsStore GetInstance()
        {
            if (_contactsStoreInstance == null)
                throw new Exception("ContactsStore not initialized!");
            return _contactsStoreInstance; 
        }

        public IEnumerable<Contact> Contacts => _contacts;

        public async Task RemoveContactAsync(Contact contact)
        {
            _contacts.Remove(contact);
            await _persistenceProvider.RemoveContactAsync(contact);
            CollectionChanged?.Invoke();
        }

        public async Task UpdateContactAsync(Contact previousContact, Contact updatedContact)
        {
            _contacts[_contacts.IndexOf(previousContact)] = updatedContact;
            await _persistenceProvider.UpdateContactAsync(previousContact, updatedContact);
            CollectionChanged?.Invoke();
        }

        public async Task AddContactAsync(Contact contact)
        {
            _contacts.Add(contact);
            await _persistenceProvider.AddContactAsync(contact);
            CollectionChanged?.Invoke();
        }

        public async Task LoadContactsAsync()
        {
            _contacts.Clear();
            _contacts.AddRange(await _persistenceProvider.LoadContactsAsync());
            CollectionChanged?.Invoke();
        }

        public async Task SaveContactsAsync()
        {
            await _persistenceProvider.SaveContactsAsync();
        }

        private ContactsStore(PersistenceProviderFactory persistenceProviderFactory)
        {
            _contacts = new List<Contact>();
            _persistenceProviderFactory = persistenceProviderFactory;
            _persistenceProvider = _persistenceProviderFactory.GetPersistenceProvider();
            User.AuthenticationStateChanged += User_AuthenticationStateChanged; ;
        }

        private async void User_AuthenticationStateChanged()
        {
            _persistenceProvider = _persistenceProviderFactory.GetPersistenceProvider();
            await SyncIfUserSignedInAsync();
        }

        private async Task SyncIfUserSignedInAsync()
        {
            if (User.IsAuthenticated)
            {
                foreach (Contact contact in _contacts)
                    contact.SetUserId(User.Id);
                await _persistenceProvider.SaveContactsAsync();
                await _persistenceProvider.LoadContactsAsync();
                CollectionChanged?.Invoke();
            }
        }
    }
}
