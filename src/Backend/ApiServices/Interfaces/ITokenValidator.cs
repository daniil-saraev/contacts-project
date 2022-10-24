namespace ApiServices.Interfaces
{
    public interface ITokenValidator
    {
        bool ValidateToken(string token);
    }
}

