using Core.Identity.Models;
using Desktop.Authentication.Exceptions;

namespace Desktop.Authentication.Models
{
    public class User
    {
        private UserData? _data;

        public bool IsAuthenticated => _data != null;

        public event Action? UserLoggedIn;
        public event Action? UserLoggedOut;

        public UserData Data
        {
            get
            {
                if (_data.HasValue)
                    return _data.Value;
                else
                    throw new UserNotAuthenticatedException();
            }
        }

        /// <summary>
        /// Clears user data and raises <see cref="UserLoggedOut"/>.
        /// </summary>
        public void Logout()
        {
            _data = null;
            UserLoggedOut?.Invoke();
        }

        /// <summary>
        /// Sets user data and raises <see cref="UserLoggedIn"/>.
        /// </summary>
        public void Authenticate(Token accessToken, Token refreshToken, string id, string email, string username)
        {
            _data = new UserData
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Id = id,
                Email = email,
                Name = username
            };
            UserLoggedIn?.Invoke();
        }

        public struct UserData
        {
            public Token AccessToken { get; set; }

            public Token RefreshToken { get; set; }

            public string Id { get; set; }

            public string Email { get; set; }

            public string Name { get; set; }
        }
    }
}
