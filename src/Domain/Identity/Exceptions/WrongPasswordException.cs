namespace Identity.Exceptions
{
    [Serializable]
    public class WrongPasswordException : Exception
    {
        public WrongPasswordException() : base("Wrong password") { }
    }
}
