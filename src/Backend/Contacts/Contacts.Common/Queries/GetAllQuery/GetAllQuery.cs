using Core.Contacts.Models;
using MediatR;

namespace Contacts.Common.Queries;

public class GetAllQuery : IRequest<IEnumerable<ContactData>>
{
    public string UserId { get; set; }
}