using MediatR;

namespace Contacts.Common.Commands;

public class DeleteCommand : IRequest
{
    public string Id { get; set; }

    public string UserId { get; set; }
}