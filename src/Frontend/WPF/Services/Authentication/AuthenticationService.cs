using Desktop.Services.Authentication;
using System.Threading.Tasks;

namespace Desktop.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly TokenProvider _tokenProvider;
        private readonly ITokenDecoder _tokenDecoder;

        public AuthenticationService(TokenProvider tokenProvider, ITokenDecoder tokenDecoder)
        {
            _tokenProvider = tokenProvider;
            _tokenDecoder = tokenDecoder;
        }

        public async Task Login(string login, string password)
        {
            var tokenResponse = await _tokenProvider.SendLoginRequest(new LoginRequest
            {
                Email = login,
                Password = password
            });

            Authenticate(tokenResponse.AccessToken);
        }

        public async Task Register(string username, string email, string password)
        {
            var tokenResponse = await _tokenProvider.SendRegisterRequest(new RegisterRequest
            {
                Email = email,
                Username = username,
                Password = password
            });

            Authenticate(tokenResponse.AccessToken);
        }

        public async Task Logout()
        {
            if (User.IsAuthenticated)
            {
                User.Logout();
                await _tokenProvider.RemoveTokenData();
            }     
        }

        public async Task RestoreSession()
        {
            var tokenResponse = await _tokenProvider.LoadTokenData();
            if (tokenResponse != null)
                Authenticate(tokenResponse.AccessToken);            
        }

        private void Authenticate(string accessToken)
        {
            var claims = _tokenDecoder.DecodeToken(accessToken);
            User.Authenticate(claims);
        }
    }
}
