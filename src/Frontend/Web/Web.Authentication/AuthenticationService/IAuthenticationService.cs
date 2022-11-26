using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Web.Authentication
{
    public interface IAuthenticationService
    {
        Task<ClaimsPrincipal> LoginAsync(string email, string password);

        Task<ClaimsPrincipal> RegisterAsync(string username, string email, string password);

        Task<ClaimsPrincipal?> RefreshAsync(string accessToken, string email);
    }
}
