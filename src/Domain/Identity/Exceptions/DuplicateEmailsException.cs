namespace Identity.Exceptions;

[Serializable]
public class DuplicateEmailsException : Exception
{
    public DuplicateEmailsException() : base("This email has already been registered")
    {

    }


}