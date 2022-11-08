namespace Api.Services.Gateway.Interfaces
{
    public interface ITokenValidator
    {
        bool ValidateToken(string token);
    }
}

