namespace Core.Contacts.Exceptions;

public class ContactNotFoundException : Exception
{
    public ContactNotFoundException() : base("Contact not found") { }

    public ContactNotFoundException(string message) : base(message) { }
}