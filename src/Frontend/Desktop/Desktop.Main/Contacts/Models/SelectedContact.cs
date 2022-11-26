using Desktop.Main.Contacts.ViewModels;
using System;

namespace Desktop.Main.Contacts.Models
{
    /// <summary>
    /// Temporary storage for a single contact the user is currently working with.
    /// </summary>
    public class SelectedContact
    {
        private ContactViewModel? _contactViewModel;
        public event Action? ContactChanged;
        public bool SelectedContactIsNull => _contactViewModel == null;

        public ContactViewModel ContactViewModel
        {
            get
            {
                if(_contactViewModel != null)
                    return _contactViewModel;
                else
                    throw new NullReferenceException();
            }   
            set
            {
                _contactViewModel = value;
                ContactChanged?.Invoke();
            }
        }
    }
}
