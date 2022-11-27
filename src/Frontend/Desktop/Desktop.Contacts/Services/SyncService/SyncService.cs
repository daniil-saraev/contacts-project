using Core.Contacts.Interfaces;
using Core.Contacts.Requests;
using Desktop.Common.Exceptions;
using Desktop.Contacts.Models;

namespace Desktop.Contacts.Services.SyncService
{
    internal class SyncService : ISyncService
    {
        private readonly IContactBookService _contactBookApi;

        public SyncService(IContactBookService contactBookApi)
        {
            _contactBookApi = contactBookApi;
        }

        public async Task<UnitOfWorkState> Sync(UnitOfWorkState state)
        {
            try
            {
                await Push(state);
                return await Pull();           
            }
            catch (Exception)
            {
                throw new SyncingWithRemoteRepositoryException();
            }
            
        }

        private async Task<UnitOfWorkState> Pull()
        {
            return new UnitOfWorkState
            {
                ExistingUnits = (await _contactBookApi.GetAllContacts())
                                    .Select(contact => new ContactUnit(contact, State.Synced))
                                    .ToList()
            };
        }

        private async Task Push(UnitOfWorkState state)
        {
            await SendAddRequests(state.ExistingUnits.Where(unit => unit.State == State.New));
            await SendUpdateRequests(state.ExistingUnits.Where(unit => unit.State == State.Changed));
            await SendDeleteRequests(state.PendingDeleteRequests);
        }

        private async Task SendAddRequests(IEnumerable<ContactUnit> addedUnits)
        {
            foreach (var unit in addedUnits)
            {
                await _contactBookApi.AddContact(new AddContactRequest
                {
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

        private async Task SendUpdateRequests(IEnumerable<ContactUnit> changedUnits)
        {
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
                request.Remove(request);
            }
        }
    }
}
