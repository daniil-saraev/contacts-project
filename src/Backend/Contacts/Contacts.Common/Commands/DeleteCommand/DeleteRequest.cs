using MediatR;

namespace Contacts.Common.Commands;

public struct DeleteRequest : IRequest
{
    public string Id { get; set; }

    public string UserId { get; set; }
}