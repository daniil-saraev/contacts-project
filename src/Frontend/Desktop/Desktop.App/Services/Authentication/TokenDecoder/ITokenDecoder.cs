using System.Collections.Generic;
using System.Security.Claims;

namespace Desktop.App.Services.Authentication.TokenDecoder
{
    public interface ITokenDecoder
    {
        IEnumerable<Claim> DecodeToken(string accessToken);
    }
}
