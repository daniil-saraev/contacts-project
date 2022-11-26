using System.ComponentModel.DataAnnotations;
using Core.Common.Entities;
using Core.Contacts.Models;

namespace Web.App.Areas.Contacts.ViewModels;

public class ContactViewModel
{
    public string? Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Display(Name = "Firstname")]
    public string FirstName { get; set; }

    [MaxLength(50)]
    [Display(Name = "Middlename")]
    public string? MiddleName { get; set; }

    [MaxLength(50)]
    [Display(Name = "Lastname")]
    public string? LastName { get; set; }

    [Required]
    [PhoneNumberValid]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; }

    [MaxLength(250)]
    public string? Address { get; set; }

    [MaxLength(250)]
    public string? Description { get; set; }

    public ContactViewModel(ContactData contact)
    {
        Id = contact.Id;
        FirstName = contact.FirstName;
        MiddleName = contact.MiddleName;
        LastName = contact.LastName;
        PhoneNumber = contact.PhoneNumber;
        Address = contact.Address;
        Description = contact.Description;
    }

    public ContactViewModel() : this(new ContactData())
    {
    }
}