using Core.Models.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        [Required]
        [PhoneNumberValid]
        public string PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? Description { get; set; }
    }
}
