using Core.Entities;
using ContactBook.Commands;
using Core.Interfaces;

namespace Web.ContactBook.Commands;

internal class DeleteContactCommand : IDeleteContactCommand
{
    private readonly IContactsRepository _repository;

    public DeleteContactCommand(IContactsRepository repository)
    {
        _repository = repository;
    }

    public async Task Execute(Contact contact, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAsync(contact, cancellationToken);
    }
}