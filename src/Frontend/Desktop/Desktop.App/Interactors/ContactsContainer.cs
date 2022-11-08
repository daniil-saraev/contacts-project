using Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desktop.App.Interactors
{
    /// <summary>
    /// Temporary storage for all contacts the user is working with.
    /// </summary>
    public class ContactsInteractor : IContactsPresenter, IContactsStore
    {
        private readonly List<Contact> _contacts;
        private readonly PersistenceProvidersFactory _persistenceProviderFactory;
        private IPersistenceProvider _persistenceProvider;

        public IEnumerable<Contact> Contacts => _contacts;

        public event Action? CollectionChanged;

        public ContactsInteractor(PersistenceProvidersFactory persistenceProviderFactory)
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
            var loadedContacts = await _persistenceProvider.LoadContactsAsync();
            _contacts.Clear();
            _contacts.AddRange(loadedContacts);
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
            await _persistenceProvider.LoadContactsAsync();
            CollectionChanged?.Invoke();
        }
    }
}
