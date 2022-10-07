using OpenApi;
using Desktop.Services.Authentication.UserServices;
using Desktop.Services.Data.Persistence;
using Desktop.Services.Data.UnitOfWork;
using Desktop.Services.Factories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Desktop.Containers;

namespace Desktop.Services.Data
{
    /// <summary>
    /// Temporary storage for all contacts the user is working with. Is a singleton.
    /// </summary>
    public class ContactsStore : IContactsStore
    {
        private readonly List<Contact> _contacts;
        private readonly PersistenceProviderFactory _persistenceProviderFactory;
        private IPersistenceProvider _persistenceProvider;

        public event Action? CollectionChanged;

        public IEnumerable<Contact> Contacts => _contacts;

        public ContactsStore(PersistenceProviderFactory persistenceProviderFactory)
        {
            _contacts = new List<Contact>();
            _persistenceProviderFactory = persistenceProviderFactory;
            _persistenceProvider = _persistenceProviderFactory.GetPersistenceProvider();
            User.AuthenticationStateChanged += User_AuthenticationStateChanged;
        }

        public Task RemoveContact(Contact contact)
        {
            _contacts.Remove(contact);
            CollectionChanged?.Invoke();
            return _persistenceProvider.RemoveContact(contact);
        }

        public Task UpdateContact(Contact previousContact, Contact updatedContact)
        {
            _contacts[_contacts.IndexOf(previousContact)] = updatedContact;
            CollectionChanged?.Invoke();
            return _persistenceProvider.UpdateContact(previousContact, updatedContact);
        }

        public Task AddContact(Contact contact)
        {
            _contacts.Add(contact);
            CollectionChanged?.Invoke();
            return _persistenceProvider.AddContact(contact);
        }

        public async Task LoadContactsAsync()
        {
            _contacts.Clear();
            _contacts.AddRange(await _persistenceProvider.LoadContactsAsync());
            CollectionChanged?.Invoke();
        }

        public Task SaveContacts()
        {
            return _persistenceProvider.SaveContactsAsync();
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
