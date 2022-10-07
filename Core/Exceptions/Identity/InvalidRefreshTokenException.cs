namespace Core.Exceptions.Identity
{
    public class InvalidRefreshTokenException : Exception
    {
        public InvalidRefreshTokenException() : base("Login expired. Try loggin in again") { }
    }
}
