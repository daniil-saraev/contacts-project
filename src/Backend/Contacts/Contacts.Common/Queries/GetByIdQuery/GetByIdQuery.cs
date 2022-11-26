using Core.Contacts.Models;
using MediatR;

namespace Contacts.Common.Queries;

public class GetByIdQuery : IRequest<ContactData>
{
    public string Id { get ; set; }

    public string UserId { get; set; }
}