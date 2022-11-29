using Core.Contacts.Models;
using Desktop.Contacts.Services;

namespace Desktop.Contacts.Models;

/// <summary>
/// A model to help <see cref="ContactsUnitOfWork"/> track local changes.
/// </summary>
internal class ContactUnit
{
    private ContactData _contact;

    public string Id => _contact.Id;

    public ContactData Contact
    {
        get
        {
            return _contact;
        }
        set
        {
            _contact = value;
        }
    }

    public State State { get; set; }

    public ContactUnit(ContactData contact, State state)
    {
        _contact = contact;
        State = state;
    }
}

internal enum State 
{
    Synced,
    New,
    Changed
}