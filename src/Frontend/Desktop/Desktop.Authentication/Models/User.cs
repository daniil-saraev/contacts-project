using Core.Identity.Models;
using Desktop.Authentication.Exceptions;

namespace Desktop.Authentication.Models
{
    public class User
    {
        private UserData? _data;
        public bool IsAuthenticated => _data != null;
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

        public void Logout()
        {
            if (!IsAuthenticated)
                return;
            _data = null;
        }

        public void Authenticate(Token accessToken, Token refreshToken, string id, string email, string username)
        {
            if (IsAuthenticated)
                return;
            _data = new UserData
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Id = id,
                Email = email,
                Name = username
            };
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
