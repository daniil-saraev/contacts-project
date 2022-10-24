using Core.Models.Validation;
using System.ComponentModel.DataAnnotations;
using System;

namespace Desktop.ViewModels.Contacts
{
    public class ContactViewModel : BaseViewModel
    {
        private Contact? _contact;
        
        public ContactViewModel(Contact contact)
        {
            _contact = contact;
        }

        public void SetContact(Contact contact)
        {
            _contact = contact;
            ValidateModel();
        }

        public Contact? GetContact()
        {
            return _contact;
        }

        public override void Dispose()
        {
            _contact = null;
            ValidateModel();
        }

        #region Properties

        public string? Id
        {
            get { return _contact?.Id; }
        }

        public string? UserId
        {
            get { return _contact?.UserId; }
        }

        public string? FullName
        {
            get { return $"{_contact?.LastName} {_contact?.FirstName}"; }
        }

        [Required(ErrorMessage = "First name is required")]
        public string? FirstName
        {
            get { return _contact?.FirstName; }
            set
            {
                if (_contact == null)
                    return;
                ValidateProperty(value);
                _contact.FirstName = value;
                OnPropertyChanged();
            }
        }

        public string? MiddleName
        {
            get { return _contact?.MiddleName; }
            set
            {
                if (_contact == null)
                    return;
                _contact.MiddleName = value;
                OnPropertyChanged();
            }
        }

        public string? LastName
        {
            get { return _contact?.LastName; }
            set
            {
                if (_contact == null)
                    return;
                _contact.LastName = value;
                OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "Phone number is required")]
        [PhoneNumberValid(ErrorMessage = "Invalid phone number")]
        public string? PhoneNumber
        {
            get { return _contact?.PhoneNumber; }
            set
            {
                if (_contact == null)
                    return;
                ValidateProperty(value);
                _contact.PhoneNumber = value;               
                OnPropertyChanged();
            }
        }

        public string? Address
        {
            get { return _contact?.Address; }
            set
            {
                if (_contact == null)
                    return;
                _contact.Address = value;
                OnPropertyChanged();
            }
        }

        public string? Description
        {
            get { return _contact?.Description; }
            set
            {
                if (_contact == null)
                    return;
                _contact.Description = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
