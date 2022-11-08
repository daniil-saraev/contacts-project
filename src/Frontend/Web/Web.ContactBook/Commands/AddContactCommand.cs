using Core.Entities;
using ContactBook.Commands;
using Core.Interfaces;

namespace Web.ContactBook.Commands;

internal class AddContactCommand : IAddContactCommand
{
    private readonly IContactsRepository _repository;

    public AddContactCommand(IContactsRepository repository)
    {
        _repository = repository;
    }

    public async Task Execute(Contact contact, CancellationToken cancellationToken = default)
    {
        await _repository.AddAsync(contact, cancellationToken);
    }
}