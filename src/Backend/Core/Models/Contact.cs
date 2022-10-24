﻿using Core.Models.Validation;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    [Serializable]
    public class Contact
    {
        [Key]
        public string Id { get; private set; }

        [Required]
        public string UserId { get; private set; }

        [Required]
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        [Required]
        [PhoneNumberValid]
        public string PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? Description { get; set; }

        public Contact()
        {
            UserId = Guid.NewGuid().ToString();
            Id = Guid.NewGuid().ToString();
        }

        public Contact(string id, 
            string userId, 
            string firstName, 
            string? middleName, 
            string? lastName, 
            string phoneNumber, 
            string? address, 
            string? description)
        {
            Id = id;
            UserId = userId;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Address = address;
            Description = description;
        }

        public void SetUserId(string userId)
        {
            UserId = userId;
        }
    }
}
