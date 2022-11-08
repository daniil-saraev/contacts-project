using Core.Entities;
using ContactBook.Commands;
using Core.Interfaces;

namespace Web.ContactBook.Commands;

internal class UpdateContactCommand : IUpdateContactCommand
{
    private readonly IContactsRepository _repository;

    public UpdateContactCommand(IContactsRepository repository)
    {
        _repository = repository;
    }

    public async Task Execute(Contact updatedContact, CancellationToken cancellationToken = default)
    {
        await _repository.UpdateAsync(updatedContact, cancellationToken);
    }
}