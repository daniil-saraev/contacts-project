using OpenApi;
using System;

namespace Desktop.Containers
{
    /// <summary>
    /// Temporary storage for a single contact the user is currently working with. Is a singleton.
    /// </summary>
    public class SelectedContact
    {
        private Contact? _contact;
        public event Action? ContactChanged;

        public SelectedContact() { }

        public Contact? Contact
        {
            get { return _contact; }
            set
            {
                _contact = value;
                ContactChanged?.Invoke();
            }
        }
    }
}
