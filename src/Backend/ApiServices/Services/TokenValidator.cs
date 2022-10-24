using System.IdentityModel.Tokens.Jwt;
using ApiServices.Interfaces;

namespace ApiServices.Services;

public class TokenValidator : ITokenValidator
{
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public TokenValidator()
    {
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public bool ValidateToken(string accessToken)
    {
        var token = _tokenHandler.ReadJwtToken(accessToken);
        return token.ValidTo > DateTime.UtcNow;
    }
}
