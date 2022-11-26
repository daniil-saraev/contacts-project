using Core.Common.Entities;
using Core.Contacts.Models;
using MediatR;

namespace Contacts.Common.Commands;

public class CreateCommand : IRequest<ContactData>
{
    public string UserId { get; set; }

    public string FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? Description { get; set; }
}