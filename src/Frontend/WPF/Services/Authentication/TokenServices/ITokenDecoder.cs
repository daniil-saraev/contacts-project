using System.Collections.Generic;
using System.Security.Claims;

namespace Desktop.Services.Authentication.TokenServices
{
    public interface ITokenDecoder
    {
        IEnumerable<Claim> DecodeToken(string accessToken);
    }
}
