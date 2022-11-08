using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Entities.Validation;

namespace Web.Areas.Contacts.ViewModels;

public class ContactViewModel
{
    public string? Id { get; set; }

    public string? UserId { get; set; }

    [Required]
    [Display(Name = "Firstname")]
    public string FirstName { get; set; }

    [Display(Name = "Middlename")]
    public string? MiddleName { get; set; }

    [Display(Name = "Lastname")]
    public string? LastName { get; set; }

    [Required]
    [PhoneNumberValid]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? Description { get; set; }

    public ContactViewModel(Contact contact)
    {
        Id = contact.Id;
        UserId = contact.UserId;
        FirstName = contact.FirstName;
        MiddleName = contact.MiddleName;
        LastName = contact.LastName;
        PhoneNumber = contact.PhoneNumber;
        Address = contact.Address;
        Description = contact.Description;
    }

    public ContactViewModel() { }
}