using ApiServicesGateway.Interfaces;
using Core.Exceptions.Identity;
using Desktop.Services.Authentication.TokenProvider;

namespace Desktop.Tests.Authentication.TokenProvider
{
    public class TokenProviderSendRefreshRequestTests
    {
        private readonly TokenProvider _tokenProvider;
        private readonly Mock<IIdentityApi> _identityApi;
        private readonly Mock<ITokenStorage> _tokenStorage;

        public TokenProviderSendRefreshRequestTests()
        {
            _tokenStorage = new Mock<ITokenStorage>();
            _identityApi = new Mock<IIdentityApi>();
            _tokenProvider = new TokenProvider(_identityApi.Object, _tokenStorage.Object);
        }

        [Fact]
        public async Task SendRefreshRequestTest()
        {
            // Arrange
            var tokenResponse = new TokenResponse
            {
                IsSuccessful = true
            };
            _identityApi.Setup(api => api.RefreshTokenAsync(It.IsAny<RefreshTokenRequest>())).ReturnsAsync(tokenResponse);

            // Act
            var response = await _tokenProvider.SendRefreshRequest(It.IsAny<RefreshTokenRequest>());

            // Assert
            Assert.Equal(tokenResponse, response);
            _tokenStorage.Verify(storage => storage.SaveTokenAsync(tokenResponse));
        }

        [Fact]
        public async Task SendRefreshRequestThrowsInvalidRefreshTokenExceptionTest()
        {
            // Arrange
            var tokenResponse = new TokenResponse
            {
                IsSuccessful = false,
                ErrorMessage = new InvalidRefreshTokenException().Message
            };
            _identityApi.Setup(api => api.RefreshTokenAsync(It.IsAny<RefreshTokenRequest>())).ReturnsAsync(tokenResponse);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidRefreshTokenException>(async () => await _tokenProvider.SendRefreshRequest(It.IsAny<RefreshTokenRequest>()));
        }
    }
}
