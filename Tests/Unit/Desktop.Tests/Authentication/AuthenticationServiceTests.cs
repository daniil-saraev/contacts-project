using Core.Common.Exceptions;
using Core.Identity.Exceptions;
using Core.Identity.Interfaces;
using Core.Identity.Models;
using Core.Identity.Requests;
using Core.Identity.Responses;
using Desktop.Authentication.Models;
using Desktop.Authentication.Services;

namespace Desktop.Tests.Authentication
{
    public class AuthenticationServiceTests
    {
        private readonly AuthenticationService _authenticationService;
        private readonly User _user;
        private readonly Mock<IIdentityService> _identityService;
        private readonly Mock<IUserDataStorage> _storage;

        public AuthenticationServiceTests()
        {
            _user = new User();
            _identityService = new Mock<IIdentityService>();
            _storage = new Mock<IUserDataStorage>();
            _authenticationService = new AuthenticationService(_identityService.Object, _user, _storage.Object);
        }

        #region Login
        [Fact]
        public async Task LoginSuccessfulTest()
        {
            // Arrange
            var response = new AuthenticationResponse 
            {
                IsSuccessful = true,
                UserEmail = "email@email.com",
                UserId = "userId",
                UserName = "username"
            };
            _identityService.Setup(service => service.LoginAsync(It.IsAny<LoginRequest>()))
                                                .Returns(Task.FromResult(response));
            bool userLoggedInRaised = false;
            _user.UserLoggedIn += () => userLoggedInRaised = true;

            // Act
            await _authenticationService.Login("login", "password");

            // Assert
            Assert.True(userLoggedInRaised);
            Assert.True(_user.Data.Email == response.UserEmail 
                        && _user.Data.Id == response.UserId
                        && _user.Data.Name == response.UserName);
            Assert.True(_user.IsAuthenticated);
            _storage.Verify(storage => storage.SaveData(_user.Data));
        }

        [Fact]
        public async Task LoginIfResponseIsUnsuccessfulTest()
        {
            // Arrange
            var response = new AuthenticationResponse
            {
                IsSuccessful = false,
                ExceptionType = ExceptionType.WrongPasswordException
            };
            _identityService.Setup(service => service.LoginAsync(It.IsAny<LoginRequest>()))
                                                .Returns(Task.FromResult(response));
            bool userLoggedInRaised = false;
            _user.UserLoggedIn += () => userLoggedInRaised = true;

            // Act & Assert
            await Assert.ThrowsAsync<WrongPasswordException>(async () => 
                await _authenticationService.Login("login", "password"));
            Assert.False(userLoggedInRaised);
            Assert.False(_user.IsAuthenticated);
        }

        [Fact]
        public async Task LoginIfIdentityServiceThrowsException()
        {
            // Arrange
            _identityService.Setup(service => service.LoginAsync(It.IsAny<LoginRequest>()))
                                                .ThrowsAsync(new ServerErrorException());

            bool userLoggedInRaised = false;
            _user.UserLoggedIn += () => userLoggedInRaised = true;

            // Act & Assert
            await Assert.ThrowsAsync<ServerErrorException>(async () => 
                await _authenticationService.Login("login", "password"));
            Assert.False(userLoggedInRaised);
            Assert.False(_user.IsAuthenticated);                                     
        }
        #endregion

        #region Register
        [Fact]
        public async Task RegisterSuccessfulTest()
        {
            // Arrange
            var response = new AuthenticationResponse 
            {
                IsSuccessful = true,
                UserEmail = "email@email.com",
                UserId = "userId",
                UserName = "username"
            };
            _identityService.Setup(service => service.RegisterAsync(It.IsAny<RegisterRequest>()))
                                                .Returns(Task.FromResult(response));
            bool userLoggedInRaised = false;
            _user.UserLoggedIn += () => userLoggedInRaised = true;

            // Act
            await _authenticationService.Register("login", "email", "password");

            // Assert
            Assert.True(userLoggedInRaised);
            Assert.True(_user.Data.Email == response.UserEmail 
                        && _user.Data.Id == response.UserId
                        && _user.Data.Name == response.UserName);
            Assert.True(_user.IsAuthenticated);
            _storage.Verify(storage => storage.SaveData(_user.Data));
        }

        [Fact]
        public async Task RegisterIfResponseIsUnsuccessfulTest()
        {
            // Arrange
            var response = new AuthenticationResponse
            {
                IsSuccessful = false,
                ExceptionType = ExceptionType.DuplicateEmailsException
            };
            _identityService.Setup(service => service.RegisterAsync(It.IsAny<RegisterRequest>()))
                                                .Returns(Task.FromResult(response));
            bool userLoggedInRaised = false;
            _user.UserLoggedIn += () => userLoggedInRaised = true;

            // Act & Assert
            await Assert.ThrowsAsync<DuplicateEmailsException>(async () => 
                await _authenticationService.Register("login", "email", "password"));
            Assert.False(userLoggedInRaised);
            Assert.False(_user.IsAuthenticated);
        }

        [Fact]
        public async Task RegisterIfIdentityServiceThrowsException()
        {
            // Arrange
            _identityService.Setup(service => service.RegisterAsync(It.IsAny<RegisterRequest>()))
                                                .ThrowsAsync(new ServerErrorException());

            bool userLoggedInRaised = false;
            _user.UserLoggedIn += () => userLoggedInRaised = true;

            // Act & Assert
            await Assert.ThrowsAsync<ServerErrorException>(async () => 
                await _authenticationService.Register("login", "email", "password"));
            Assert.False(userLoggedInRaised);
            Assert.False(_user.IsAuthenticated);                                     
        }
        #endregion

        #region Refresh
        [Fact]
        public async Task RefreshSuccessfulTest()
        {
            // Arrange
            var response = new AuthenticationResponse 
            {
                IsSuccessful = true,
                UserEmail = "email@email.com",
                UserId = "userId",
                UserName = "username"
            };
            _identityService.Setup(service => service.RefreshTokenAsync(It.IsAny<RefreshTokenRequest>()))
                                                .Returns(Task.FromResult(response));
            bool userLoggedInRaised = false;
            _user.UserLoggedIn += () => userLoggedInRaised = true;

            // Act
            await _authenticationService.Refresh("token", "userId");

            // Assert
            Assert.True(userLoggedInRaised);
            Assert.True(_user.Data.Email == response.UserEmail 
                        && _user.Data.Id == response.UserId
                        && _user.Data.Name == response.UserName);
            Assert.True(_user.IsAuthenticated);
            _storage.Verify(storage => storage.SaveData(_user.Data));
        }

        [Fact]
        public async Task RefreshIfResponseIsUnsuccessfulTest()
        {
            // Arrange
            var response = new AuthenticationResponse
            {
                IsSuccessful = false,
                ExceptionType = ExceptionType.InvalidRefreshTokenException
            };
            _identityService.Setup(service => service.RefreshTokenAsync(It.IsAny<RefreshTokenRequest>()))
                                                .Returns(Task.FromResult(response));
            bool userLoggedInRaised = false;
            _user.UserLoggedIn += () => userLoggedInRaised = true;

            // Act & Assert
            await Assert.ThrowsAsync<InvalidRefreshTokenException>(async () => 
                await _authenticationService.Refresh("token", "userId"));
            Assert.False(userLoggedInRaised);
            Assert.False(_user.IsAuthenticated);
        }

        [Fact]
        public async Task RefreshIfIdentityServiceThrowsException()
        {
            // Arrange
            _identityService.Setup(service => service.RefreshTokenAsync(It.IsAny<RefreshTokenRequest>()))
                                                .ThrowsAsync(new ServerErrorException());

            bool userLoggedInRaised = false;
            _user.UserLoggedIn += () => userLoggedInRaised = true;

            // Act & Assert
            await Assert.ThrowsAsync<ServerErrorException>(async () => 
                await _authenticationService.Refresh("token", "userId"));
            Assert.False(userLoggedInRaised);
            Assert.False(_user.IsAuthenticated);                                     
        }
        #endregion

        #region Logout
        [Fact]
        public async Task LogoutTest()
        {
            // Arrange
            _user.Authenticate(new Token(), new Token(), "id", "email", "username");
            bool userLoggedOutRaised = false;
            _user.UserLoggedOut += () => userLoggedOutRaised = true;

            // Act
            await _authenticationService.Logout();

            // Assert
            Assert.True(userLoggedOutRaised);
            Assert.False(_user.IsAuthenticated);
            _storage.Verify(storage => storage.RemoveData());
        }

        public async Task LogoutIfUserIsNotAuthenticated()
        {
            // Arrange
            bool userLoggedOutRaised = false;
            _user.UserLoggedOut += () => userLoggedOutRaised = true;

            // Act
            await _authenticationService.Logout();

            // Assert
            Assert.False(userLoggedOutRaised);
            Assert.False(_user.IsAuthenticated);
            _storage.VerifyNoOtherCalls();
        }
        #endregion
    }
}
