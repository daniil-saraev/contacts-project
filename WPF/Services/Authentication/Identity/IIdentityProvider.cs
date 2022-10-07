using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Services.Authentication.Identity
{
    public interface IIdentityProvider
    {
        Task<IEnumerable<Claim>> LoginAsync(string login, string password);

        Task<IEnumerable<Claim>> RegisterAsync(string username, string email, string password);

        Task<IEnumerable<Claim>> RefreshAsync();

        Task<IEnumerable<Claim>?> RestoreUserAsync();

        Task LogoutAsync();
    }
}
