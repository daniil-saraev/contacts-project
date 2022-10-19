using Desktop.Services.Authentication.UserServices;
using Desktop.Services.Data.Persistence;
using Desktop.Services.Factories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Desktop.Containers;

namespace Desktop.Services.Containers
{
    /// <summary>
    /// Temporary storage for all contacts the user is working with.
    /// </summary>
    public class ContactsContainer : IContactsPresenter, IContactsStore
    {
        private readonly List<Contact> _contacts;
        private readonly PersistenceProvidersFactory _persistenceProviderFactory;
        private IPersistenceProvider _persistenceProvider;

        public IEnumerable<Contact> Contacts => _contacts;

        public event Action? CollectionChanged;

        public ContactsContainer(PersistenceProvidersFactory persistenceProviderFactory)
        {
            _contacts = new List<Contact>();
            _persistenceProviderFactory = persistenceProviderFactory;
            _persistenceProvider = User.IsAuthenticated ? _persistenceProviderFactory.GetAuthenticatedPersistenceProvider()
                                                        : _persistenceProviderFactory.GetNotAuthenticatedPersistenceProvider();
            User.AuthenticationStateChanged += User_AuthenticationStateChanged;
        }

        public void RemoveContact(Contact contact)
        {
            if (!_contacts.Contains(contact))
                return;
            _contacts.Remove(contact);
            CollectionChanged?.Invoke();
            _persistenceProvider.RemoveContact(contact);
        }

        public void UpdateContact(Contact initialContact, Contact updatedContact)
        {
            if (!_contacts.Contains(initialContact))
                return;
            _contacts[_contacts.IndexOf(initialContact)] = updatedContact;
            CollectionChanged?.Invoke();
            _persistenceProvider.UpdateContact(initialContact, updatedContact);
        }

        public void AddContact(Contact contact)
        {
            if (_contacts.Contains(contact))
                return;
            _contacts.Add(contact);
            CollectionChanged?.Invoke();
            _persistenceProvider.AddContact(contact);
        }

        public async Task LoadContactsAsync()
        {
            _contacts.Clear();
            _contacts.AddRange(await _persistenceProvider.LoadContactsAsync());
            CollectionChanged?.Invoke();
        }

        public Task SaveContactsAsync()
        {
            return _persistenceProvider.SaveContactsAsync();
        }

        private async void User_AuthenticationStateChanged()
        {
            if (User.IsAuthenticated)
            {
                _persistenceProvider = _persistenceProviderFactory.GetAuthenticatedPersistenceProvider();
                await SyncContactsAsync();
            }
            else
                _persistenceProvider = _persistenceProviderFactory.GetNotAuthenticatedPersistenceProvider();
        }

        private async Task SyncContactsAsync()
        {
            foreach (Contact contact in _contacts)
                contact.SetUserId(User.Id);
            await _persistenceProvider.SaveContactsAsync();
            await _persistenceProvider.LoadContactsAsync();
            CollectionChanged?.Invoke();
        }
    }
}
