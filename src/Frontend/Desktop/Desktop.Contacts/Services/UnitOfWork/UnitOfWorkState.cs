using System.Collections.Generic;
using Core.Contacts.Requests;
using Desktop.Contacts.Models;

namespace Desktop.Contacts.Services
{
    internal class UnitOfWorkState
    {
        public List<DeleteContactRequest> PendingDeleteRequests { get; set; } = new List<DeleteContactRequest>();
        public List<ContactUnit> ExistingUnits { get; set; } = new List<ContactUnit>();

        public bool IsSynced => PendingDeleteRequests.Count == 0 && ExistingUnits.All(unit => unit.State == State.Synced);
    }
}
