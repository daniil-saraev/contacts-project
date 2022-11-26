namespace Core.Identity.Exceptions;

[Serializable]
public class InvalidRefreshTokenException : Exception
{
    public InvalidRefreshTokenException() : base("Login expired. Try loggin in again") { }

    public InvalidRefreshTokenException(string message) : base(message) { }

    protected InvalidRefreshTokenException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
