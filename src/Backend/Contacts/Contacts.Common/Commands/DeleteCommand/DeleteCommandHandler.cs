using Core.Common.Interfaces;
using Core.Contacts.Exceptions;
using MediatR;

namespace Contacts.Common.Commands;

internal class DeleteCommandHandler : IRequestHandler<DeleteRequest>
{
    private readonly IContactsRepository _repository;

    public DeleteCommandHandler(IContactsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteRequest request, CancellationToken cancellationToken = default)
    {
        var contact = await _repository.GetAsync(request.Id, cancellationToken);
        if(contact == null || contact.UserId != request.UserId)
            throw new ContactNotFoundException();

        await _repository.DeleteAsync(contact, cancellationToken);
        return Unit.Value;
    }
}