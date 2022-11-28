using AutoMapper;
using Core.Common.Entities;
using Core.Common.Interfaces;
using Core.Contacts.Models;
using MediatR;

namespace Contacts.Common.Commands;

internal class CreateCommandHandler : IRequestHandler<CreateRequest, ContactData>
{
    private readonly IContactsRepository _repository;
    private readonly IMapper _mapper;

    public CreateCommandHandler(IContactsRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ContactData> Handle(CreateRequest request, CancellationToken cancellationToken = default)
    {
        var contact = new Contact(
            request.UserId,
            request.FirstName,
            request.MiddleName,
            request.LastName,
            request.PhoneNumber,
            request.Address,
            request.Description
        );

        await _repository.AddAsync(contact, cancellationToken);
        return _mapper.Map<ContactData>(contact);
    }
}