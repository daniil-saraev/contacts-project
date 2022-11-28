using System.ComponentModel.DataAnnotations;
using Desktop.Common.ViewModels;
using Core.Contacts.Models;
using System;
using Core.Common.Entities;

namespace Desktop.Main.Contacts.ViewModels
{
    public class ContactViewModel : BaseViewModel
    {
        private ContactData _contact;

        public ContactViewModel(ContactData contact)
        {
            _contact = contact;
        }

        public ContactViewModel()
        {
            _contact = new ContactData();
        }

        public ContactData Contact
        {
            get
            {
                return _contact;
            }
            set
            {
                _contact = value;
                ValidateModel();
            }
        }

        #region Properties

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
