using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseApi;
using Desktop.ViewModels.Contacts;
using ApiServices.Interfaces;

namespace Desktop.Commands.Queries.Contacts
{
    public class GetAllContactsQuery : IQuery<IEnumerable<ContactViewModel>>
    {
        private readonly IRepository<Contact> _contactsRepository;

        public GetAllContactsQuery()
        {
            //_contactsRepository = RepositoryService.GetRepositories();
        }

        public async Task<IEnumerable<ContactViewModel>?> Execute(object? parameter = null)
        {
            IEnumerable<Contact>? contacts = await _contactsRepository.GetAllAsync();
            if (contacts != null)
                return contacts.Select(c => new ContactViewModel(c));
            return null;
        }
    }
}