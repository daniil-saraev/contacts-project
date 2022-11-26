using Core.Identity.Requests;
using Core.Identity.Responses;

namespace Core.Identity.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthenticationResponse> LoginAsync(LoginRequest request);

        Task<AuthenticationResponse> RegisterAsync(RegisterRequest request);

        Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
