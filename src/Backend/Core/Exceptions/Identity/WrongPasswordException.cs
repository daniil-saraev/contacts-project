namespace Core.Exceptions.Identity
{
    [Serializable]
    public class WrongPasswordException : Exception
    {
        public WrongPasswordException() : base("Wrong password") { }
    }
}
