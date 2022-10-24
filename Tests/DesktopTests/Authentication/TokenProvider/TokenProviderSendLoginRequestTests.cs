using Core.Exceptions.Identity;
using Core.Interfaces;
using Desktop.Exceptions;
using Desktop.Services.Authentication;
using NuGet.ContentModel;

namespace DesktopTests.Authentication
{
    public class TokenProviderSendLoginRequestTests
    {
        private readonly TokenProvider _tokenProvider;
        private readonly Mock<IIdentityApi> _identityApi;
        private readonly Mock<ITokenStorage> _tokenStorage;

        public TokenProviderSendLoginRequestTests()
        {
            _tokenStorage = new Mock<ITokenStorage>();
            _identityApi = new Mock<IIdentityApi>();
            _tokenProvider = new TokenProvider(_identityApi.Object, _tokenStorage.Object);
        }

        [Fact]
        public async Task SendLoginRequestTest()
        {
            // Arrange
            var tokenResponse = new TokenResponse
            {
                IsSuccessful = true
            };
            _identityApi.Setup(api => api.LoginAsync(It.IsAny<LoginRequest>())).ReturnsAsync(tokenResponse);

            // Act
            var response = await _tokenProvider.SendLoginRequest(It.IsAny<LoginRequest>());

            // Assert
            Assert.Equal(tokenResponse, response);
            _tokenStorage.Verify(storage => storage.SaveTokenAsync(tokenResponse));
        }

        [Fact]
        public async Task SendLoginRequestThrowsUserNotFoundExceptionTest()
        {
            // Arrange
            var tokenResponse = new TokenResponse
            {
                IsSuccessful = false,
                ErrorMessage = new UserNotFoundException().Message
            };
            _identityApi.Setup(api => api.LoginAsync(It.IsAny<LoginRequest>())).ReturnsAsync(tokenResponse);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(async () => await _tokenProvider.SendLoginRequest(It.IsAny<LoginRequest>()));
        }

        [Fact]
        public async Task SendLoginRequestThrowsWrongPasswordExceptionTest()
        {
            // Arrange
            var tokenResponse = new TokenResponse
            {
                IsSuccessful = false,
                ErrorMessage = new WrongPasswordException().Message
            };
            _identityApi.Setup(api => api.LoginAsync(It.IsAny<LoginRequest>())).ReturnsAsync(tokenResponse);

            // Act & Assert
            await Assert.ThrowsAsync<WrongPasswordException>(async () => await _tokenProvider.SendLoginRequest(It.IsAny<LoginRequest>()));
        }
    }
}
