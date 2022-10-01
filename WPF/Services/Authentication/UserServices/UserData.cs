using IdentityApi;
using Microsoft.AspNetCore.Identity;

namespace Desktop.Services.Authentication.UserServices
{
    public class UserData
    {
        public TokenResponse CurrentTokenResponse { get; set; }

        public UserData(TokenResponse currentTokenResponse)
        {
            CurrentTokenResponse = currentTokenResponse;
        }
    }
}
