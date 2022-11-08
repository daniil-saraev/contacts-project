using ContactBook.Queries;
using Core.Entities;
using Core.Interfaces;

namespace Web.ContactBook.Queries;

internal class GetContactsQuery : IGetContactsQuery
{
    private readonly IContactsRepository _repository;

    public GetContactsQuery(IContactsRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Contact>> Execute(string userId, Func<Contact, bool>? predicate = null, CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(userId, predicate, cancellationToken);
    }
}