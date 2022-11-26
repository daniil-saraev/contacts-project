using System.ComponentModel.DataAnnotations;
using Core.Common.Entities;

namespace Core.Contacts.Models;

public struct ContactData
{
    public string Id { get; set; }

    public string FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? Description { get; set; }
}