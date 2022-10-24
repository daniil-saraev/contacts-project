namespace Core.Exceptions.Identity;

[Serializable]
public class RegisterErrorException : System.Exception
{
    public RegisterErrorException() : base("Error occurred while creating account") { }
}