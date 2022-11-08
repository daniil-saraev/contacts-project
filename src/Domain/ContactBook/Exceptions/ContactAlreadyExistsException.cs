using System;

namespace ContactBook.Exceptions;

public class ContactAlreadyExistsException : Exception
{
    public ContactAlreadyExistsException() : base("Contact already exists") { }

    public ContactAlreadyExistsException(string message) : base(message) { }
}