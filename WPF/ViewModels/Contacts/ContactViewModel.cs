using DatabaseApi;
using Core.Models.Validation;
using System.ComponentModel.DataAnnotations;
using System;

namespace Desktop.ViewModels.Contacts
{
    public class ContactViewModel : BaseViewModel
    {
        private Contact _contact;
        
        public ContactViewModel(Contact contact)
        {
            _contact = contact;
        }

        public void SetContact(Contact contact)
        {
            _contact = contact;
        }

        public Contact GetContact()
        {
            return _contact;
        }

        #region Properties

        public string Id
        {
            get { return _contact.Id; }
        }

        [Required]
        public string UserId
        {
            get { return _contact.UserId; }
        }

        public string FullName
        {
            get { return $"{_contact.LastName} {_contact.FirstName}"; }
        }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName
        {
            get { return _contact.FirstName; }
            set
            {
                ValidateProperty(value);
                _contact.FirstName = value;
                OnPropertyChanged();
            }
        }

        public string? MiddleName
        {
            get { return _contact.MiddleName; }
            set
            {
                _contact.MiddleName = value;
                OnPropertyChanged();
            }
        }

        public string? LastName
        {
            get { return _contact.LastName; }
            set
            {
                _contact.LastName = value;
                OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "Phone number is required")]
        [PhoneNumberValid(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber
        {
            get { return _contact.PhoneNumber; }
            set
            {
                ValidateProperty(value);
                _contact.PhoneNumber = value;               
                OnPropertyChanged();
            }
        }

        public string? Address
        {
            get { return _contact.Address; }
            set
            {
                _contact.Address = value;
                OnPropertyChanged();
            }
        }

        public string? Description
        {
            get { return _contact.Description; }
            set
            {
                _contact.Description = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
