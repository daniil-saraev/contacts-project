using System.ComponentModel.DataAnnotations;
using Core.Common.Entities;

namespace Core.Contacts.Requests;

public struct AddContactRequest
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [MaxLength(50)]
    public string? MiddleName { get; set; }

    [MaxLength(50)]
    public string? LastName { get; set; }

    [Required]
    [PhoneNumberValid]
    public string PhoneNumber { get; set; }

    [MaxLength(250)]
    public string? Address { get; set; }

    [MaxLength(250)]
    public string? Description { get; set; }
}