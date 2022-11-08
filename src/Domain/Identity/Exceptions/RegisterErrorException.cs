namespace Identity.Exceptions;

[Serializable]
public class RegisterErrorException : Exception
{
    public RegisterErrorException() : base("Error occurred while creating account") { }
}