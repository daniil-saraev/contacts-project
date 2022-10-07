namespace Core.Exceptions.Identity
{
    public class WrongPasswordException : Exception
    {
        public WrongPasswordException() : base("Wrong password") { }
    }
}
