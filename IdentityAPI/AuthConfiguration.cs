using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Core.Constants;

namespace IdentityServer
{
    public class AuthConfiguration
    {
        public string Secret { get; set; }
        public int TokenExpirationMinutes { get; set; }
    }
}
