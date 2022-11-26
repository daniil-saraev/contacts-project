using System.ComponentModel.DataAnnotations;

namespace Core.Contacts.Requests;

public struct DeleteContactRequest
{
    [Required]
    public string Id { get; set; }
}