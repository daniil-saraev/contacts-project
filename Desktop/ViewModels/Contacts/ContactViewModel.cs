using ApiServices;
using Core.Models.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Desktop.ViewModels.Contacts
{
    public class ContactViewModel : BaseViewModel
    {
        private Contact _contact;

        public ContactViewModel(Contact contact)
        {
            _contact = contact;
        }

        [Key]
        public int Id 
        {
            get { return _contact.Id; }
            set 
            {
                _contact.Id = value;
                OnPropertyChanged();
            }
        }

        [Required]
        public string UserId
        {
            get { return _contact.UserId; }
            set
            {
                _contact.UserId = value;
                OnPropertyChanged();
            }
        }

        [Required]
        public string FirstName
        {
            get { return _contact.FirstName; }
            set
            {
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

        [Required]
        [PhoneNumberValid]
        public string PhoneNumber
        {
            get { return _contact.PhoneNumber; }
            set
            {
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
    }
}
