using Core.Contacts.Models;
using MediatR;

namespace Contacts.Common.Queries;

public struct GetAllQuery : IRequest<IEnumerable<ContactData>>
{
    public string UserId { get; set; }
}