using AutoMapper;
using Core.Contacts.Interfaces;
using Core.Contacts.Exceptions;
using Core.Contacts.Models;
using MediatR;

namespace Contacts.Common.Commands;

internal class UpdateCommandHandler : IRequestHandler<UpdateRequest, ContactData>
{
    private readonly IContactsRepository _repository;
    private readonly IMapper _mapper;

    public UpdateCommandHandler(IContactsRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ContactData> Handle(UpdateRequest request, CancellationToken cancellationToken = default)
    {
        var contact = await _repository.GetAsync(request.Id, cancellationToken);
        if(contact == null || contact.UserId != request.UserId)
            throw new ContactNotFoundException();

        contact.FirstName = request.FirstName;
        contact.MiddleName = request.MiddleName;
        contact.LastName = request.LastName;
        contact.PhoneNumber = request.PhoneNumber;
        contact.Address = request.Address;
        contact.Description = request.Description;

        await _repository.UpdateAsync(contact, cancellationToken);
        return _mapper.Map<ContactData>(contact);
    }
}