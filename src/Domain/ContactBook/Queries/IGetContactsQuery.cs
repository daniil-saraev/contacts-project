using Core.Entities;

namespace ContactBook.Queries;

public interface IGetContactsQuery
{
    Task<IEnumerable<Contact>> Execute(string userId, Func<Contact, bool>? predicate = null, CancellationToken cancellationToken = default);
}