using AutoMapper;
using Core.Contacts.Models;
using Core.Common.Entities;
using Core.Contacts.Interfaces;
using MediatR;

namespace Contacts.Common.Queries;

internal class GetAllQueryHandler : IRequestHandler<GetAllQuery, IEnumerable<ContactData>>
{
    private readonly IContactsRepository _repository;
    private readonly IMapper _mapper;

    public GetAllQueryHandler(IContactsRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ContactData>> Handle(GetAllQuery query, CancellationToken cancellationToken = default)
    {
        IEnumerable<Contact> contacts = await _repository.GetAllAsync(contact => contact.UserId == query.UserId, cancellationToken);
        return contacts.Select(contact => _mapper.Map<ContactData>(contact));
    }
}