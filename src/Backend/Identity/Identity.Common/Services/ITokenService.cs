using System.Security.Claims;

namespace Identity.Common.Services
{
    public interface ITokenService
    {
        string GenerateToken(string secret, int expirationMinutes, IEnumerable<Claim>? claims);

        bool ValidateToken(string token, string secret);
    }
}
