namespace Core.Exceptions.Identity;

[Serializable]
public class DuplicateEmailsException : System.Exception
{
    public DuplicateEmailsException() : base("This email has already been registered") 
    { 

    }


}