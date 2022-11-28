using Core.Contacts.Interfaces;
using Core.Contacts.Models;
using Core.Contacts.Requests;
using Desktop.Common.Exceptions;
using Desktop.Contacts.Models;

namespace Desktop.Contacts.Services
{
    internal class SyncService : ISyncService
    {
        private readonly IContactBookService _contactBookApi;

        public SyncService(IContactBookService contactBookApi)
        {
            _contactBookApi = contactBookApi;
        }

        public async Task<IEnumerable<ContactData>> Pull()
        {
            try
            {
                return await _contactBookApi.GetAllContacts();
            }
            catch (Exception)
            {
                throw new SyncingWithRemoteRepositoryException();
            }     
        }

        public async Task Push(UnitOfWorkState state)
        {
            try
            {
                await SendAddRequests(state.ExistingUnits.Where(unit => unit.State == State.New));
                await SendUpdateRequests(state.ExistingUnits.Where(unit => unit.State == State.Changed));
                await SendDeleteRequests(state.PendingDeleteRequests);
            }
            catch (Exception)
            {
                throw new SyncingWithRemoteRepositoryException();
            }         
        }

        private async Task SendAddRequests(IEnumerable<ContactUnit> addedUnits)
        {
            if(addedUnits.Any())
                foreach (var unit in addedUnits)
                {
                    var contact = await _contactBookApi.AddContact(new AddContactRequest
                    {
                        FirstName = unit.Contact.FirstName,
                        MiddleName = unit.Contact.MiddleName,
                        LastName = unit.Contact.LastName,
                        PhoneNumber = unit.Contact.PhoneNumber,
                        Address = unit.Contact.Address,
                        Description = unit.Contact.Description
                    });
                    unit.Contact = contact;
                    unit.State = State.Synced;
                }
        }

        private async Task SendUpdateRequests(IEnumerable<ContactUnit> changedUnits)
        {   
            if (changedUnits.Any())
                foreach (var unit in changedUnits)
                {
                    await _contactBookApi.UpdateContact(new UpdateContactRequest
                    {
                        Id = unit.Contact.Id,
                        FirstName = unit.Contact.FirstName,
                        MiddleName = unit.Contact.MiddleName,
                        LastName = unit.Contact.LastName,
                        PhoneNumber = unit.Contact.PhoneNumber,
                        Address = unit.Contact.Address,
                        Description = unit.Contact.Description
                    });
                    unit.State = State.Synced;
                }
        }

        private async Task SendDeleteRequests(List<DeleteContactRequest> requests)
        {
            while (requests.Count > 0)
            {
                var request = requests.First();
                await _contactBookApi.DeleteContact(request);
                requests.Remove(request);
            }
        }
    }
}
