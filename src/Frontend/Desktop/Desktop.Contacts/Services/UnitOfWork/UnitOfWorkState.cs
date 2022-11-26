using System.Collections.Generic;
using Core.Contacts.Requests;
using Desktop.Contacts.Models;

namespace Desktop.Contacts.Services
{
    internal class UnitOfWorkState
    {
        public Queue<DeleteContactRequest> PendingDeleteRequests { get; set; } = new Queue<DeleteContactRequest>();
        public List<ContactUnit> ExistingUnits { get; set; } = new List<ContactUnit>();
    }
}
