using ApiServices.Identity;
using System.Threading.Tasks;

namespace Desktop.Services.Authentication
{
    public interface ITokenStorage
    {
        Task<TokenResponse?> GetTokenAsync();

        Task SaveTokenAsync(TokenResponse userData);

        Task RemoveTokenAsync();
    }
}
