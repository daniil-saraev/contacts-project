using Core.Contacts.Models;

namespace Desktop.Contacts.Models;

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

public enum State 
{
    Synced,
    New,
    Changed
}