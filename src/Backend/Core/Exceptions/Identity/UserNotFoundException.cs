namespace Core.Exceptions.Identity
{
    [Serializable]
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User not found") { }
    }
}
