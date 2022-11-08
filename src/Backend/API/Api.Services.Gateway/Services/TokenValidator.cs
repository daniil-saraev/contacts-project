using System.IdentityModel.Tokens.Jwt;
using Api.Services.Gateway.Interfaces;

namespace Api.Services.Gateway.Services;

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
