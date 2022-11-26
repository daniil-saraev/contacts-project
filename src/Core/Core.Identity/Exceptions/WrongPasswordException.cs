namespace Core.Identity.Exceptions;

[Serializable]
public class WrongPasswordException : Exception
{
    public WrongPasswordException() : base("Wrong password") { }

    public WrongPasswordException(string message) : base(message) { }

    protected WrongPasswordException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
