namespace Identity.Exceptions
{
    [Serializable]
    public class InvalidRefreshTokenException : Exception
    {
        public InvalidRefreshTokenException() : base("Login expired. Try loggin in again") { }
    }
}
