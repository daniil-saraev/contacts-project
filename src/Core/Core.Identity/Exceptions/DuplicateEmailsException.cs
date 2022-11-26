namespace Core.Identity.Exceptions;

[Serializable]
public class DuplicateEmailsException : Exception
{
    public DuplicateEmailsException() : base("This email has already been registered") { }

    public DuplicateEmailsException(string message) : base(message) { }

    protected DuplicateEmailsException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

}