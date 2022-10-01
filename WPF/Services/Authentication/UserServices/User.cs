using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Desktop.Services.Authentication.UserServices
{
    public class User
    {
        private static User _userInstance;

        private static IdentityUser? _identityUser;

        public static event Action? AuthenticationStateChanged;   

        public static bool IsAuthenticated => _identityUser != null;
        public static string? Id => _identityUser?.Id;
        public static string? UserName => _identityUser?.UserName;
        public static string? Email => _identityUser?.Email;

        public static User GetUser()
        {
            _userInstance ??= new User();
            return _userInstance;
        }

        public void Logout()
        {
            _identityUser = null;
            AuthenticationStateChanged?.Invoke();
        }

        public void Authenticate(IEnumerable<Claim> claims)
        {
            _identityUser = new IdentityUser
            {
                Id = claims.First(c => c.Type == "id").Value,
                Email = claims.First(c => c.Type == ClaimTypes.Email).Value,
                UserName = claims.First(c => c.Type == ClaimTypes.Name).Value
            };
            AuthenticationStateChanged?.Invoke();
        }

        private User()
        {
        }
    }
}
