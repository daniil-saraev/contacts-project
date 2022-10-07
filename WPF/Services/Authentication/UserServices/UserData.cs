using OpenApi;
using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

namespace Desktop.Services.Authentication.UserServices
{
    public class UserData
    {
        public string RefreshToken { get; set; }
        public IEnumerable<Claim> UserClaims { get; set; }

        public UserData(string refreshToken, IEnumerable<Claim> claims)
        {
            RefreshToken = refreshToken;
            UserClaims = claims;
        }
    }
}
