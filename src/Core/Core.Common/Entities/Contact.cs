using Core.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace Core.Common.Entities
{
    [Serializable]
    public class Contact : Entity
    {
        [Required]
        [MaxLength(450)]
        public string UserId { get; private set; }

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

        public Contact(
            string userId,
            string firstName,
            string? middleName,
            string? lastName,
            string phoneNumber,
            string? address,
            string? description) : base()
        {
            UserId = userId;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Address = address;
            Description = description;
        }
    }
}
