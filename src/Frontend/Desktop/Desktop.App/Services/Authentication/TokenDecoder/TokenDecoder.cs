using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Desktop.App.Services.Authentication.TokenDecoder
{
    public class TokenDecoder : ITokenDecoder
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public TokenDecoder()
        {
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public IEnumerable<Claim> DecodeToken(string accessToken)
        {
            var token = _tokenHandler.ReadJwtToken(accessToken);
            return token.Claims;
        }
    }
}
