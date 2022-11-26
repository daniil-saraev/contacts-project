namespace Core.Identity.Exceptions;

[Serializable]
public class RegisterErrorException : Exception
{
    public RegisterErrorException() : base("Error occurred while creating account") { }

    public RegisterErrorException(string message) : base(message) { }

    protected RegisterErrorException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

}