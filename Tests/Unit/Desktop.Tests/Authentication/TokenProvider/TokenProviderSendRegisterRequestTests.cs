using ApiServicesGateway.Interfaces;
using Core.Exceptions.Identity;
using Core.Interfaces;
using Desktop.Services.Authentication.TokenProvider;

namespace Desktop.Tests.Authentication.TokenProvider
{
    public class TokenProviderSendRegisterRequestTests
    {
        private readonly TokenProvider _tokenProvider;
        private readonly Mock<IIdentityApi> _identityApi;
        private readonly Mock<ITokenStorage> _tokenStorage;

        public TokenProviderSendRegisterRequestTests()
        {
            _tokenStorage = new Mock<ITokenStorage>();
            _identityApi = new Mock<IIdentityApi>();
            _tokenProvider = new TokenProvider(_identityApi.Object, _tokenStorage.Object);
        }

        [Fact]
        public async Task SendRegisterRequestTest()
        {
            // Arrange
            var tokenResponse = new TokenResponse
            {
                IsSuccessful = true
            };
            _identityApi.Setup(api => api.RegisterAsync(It.IsAny<RegisterRequest>())).ReturnsAsync(tokenResponse);

            // Act
            var response = await _tokenProvider.SendRegisterRequest(It.IsAny<RegisterRequest>());

            // Assert
            Assert.Equal(tokenResponse, response);
            _tokenStorage.Verify(storage => storage.SaveTokenAsync(tokenResponse));
        }

        [Fact]
        public async Task SendRegisterRequestThrowsDuplicateEmailsExceptionExceptionTest()
        {
            // Arrange
            var tokenResponse = new TokenResponse
            {
                IsSuccessful = false,
                ErrorMessage = new DuplicateEmailsException().Message
            };
            _identityApi.Setup(api => api.RegisterAsync(It.IsAny<RegisterRequest>())).ReturnsAsync(tokenResponse);

            // Act & Assert
            await Assert.ThrowsAsync<DuplicateEmailsException>(async () => await _tokenProvider.SendRegisterRequest(It.IsAny<RegisterRequest>()));
        }

        [Fact]
        public async Task SendRegisterRequestThrowsRegisterErrorExceptionTest()
        {
            // Arrange
            var tokenResponse = new TokenResponse
            {
                IsSuccessful = false,
                ErrorMessage = new RegisterErrorException().Message
            };
            _identityApi.Setup(api => api.RegisterAsync(It.IsAny<RegisterRequest>())).ReturnsAsync(tokenResponse);

            // Act & Assert
            await Assert.ThrowsAsync<RegisterErrorException>(async () => await _tokenProvider.SendRegisterRequest(It.IsAny<RegisterRequest>()));
        }
    }
}
