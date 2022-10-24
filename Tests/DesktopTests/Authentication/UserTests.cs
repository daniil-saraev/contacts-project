using Desktop.Services.Authentication;
using System.Security.Claims;

namespace DesktopTests.Authentication
{
    public class UserTests
    {
        private readonly string _id;
        private readonly string _email;
        private readonly string _name;
        private readonly List<Claim> _claims;

        public UserTests()
        {
            _id = Guid.NewGuid().ToString();
            _email = "email@mail.ru";
            _name = "Ivan";
            _claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, _id),
                new Claim(ClaimTypes.Email, _email),
                new Claim(ClaimTypes.Name, _name)
            };
        }

        [Fact]
        public void AuthenticateUserTest()
        {
            // Arrange
            bool authenticationStateChangedRaised = false;
            User.AuthenticationStateChanged += () =>
            {
                authenticationStateChangedRaised = true;
            };

            // Act
            User.Authenticate(_claims);

            // Assert
            Assert.True(authenticationStateChangedRaised);
            Assert.True(User.IsAuthenticated);
            Assert.Equal(User.Id, _id);
            Assert.Equal(User.Email, _email);
            Assert.Equal(User.UserName, _name);
        }

        [Fact]
        public void LogoutUserTest()
        {
            // Arrange
            User.Authenticate(_claims);
            bool authenticationStateChangedRaised = false;
            User.AuthenticationStateChanged += () =>
            {
                authenticationStateChangedRaised = true;
            };

            // Act
            User.Logout();

            // Assert
            Assert.True(authenticationStateChangedRaised);
            Assert.False(User.IsAuthenticated);
        }
    }
}
