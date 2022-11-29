using AutoMapper;
using Core.Contacts.Interfaces;
using Core.Contacts.Exceptions;
using Core.Contacts.Models;
using MediatR;

namespace Contacts.Common.Queries;

internal class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, ContactData>
{
    private readonly IContactsRepository _repository;
    private readonly IMapper _mapper;

    public GetByIdQueryHandler(IContactsRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ContactData> Handle(GetByIdQuery query, CancellationToken cancellationToken = default)
    {
        var contact = await _repository.GetAsync(query.Id, cancellationToken);
        if(contact == null || contact.UserId != query.UserId)
            throw new ContactNotFoundException();
        else
            return _mapper.Map<ContactData>(contact);
    }
}
