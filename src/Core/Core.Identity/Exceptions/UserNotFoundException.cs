namespace Core.Identity.Exceptions;

[Serializable]
public class UserNotFoundException : Exception
{
    public UserNotFoundException() : base("User not found") { }

    public UserNotFoundException(string message) : base(message) { }

    protected UserNotFoundException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
