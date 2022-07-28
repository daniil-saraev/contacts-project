using ApiServices;
using Core.Constants;
using IdentityModel.Client;

namespace Web.Services
{
    public static class ContactsDbApiFactory
    {
        private static readonly HttpClient _httpClient;
        private static bool _isInitialized;

        static ContactsDbApiFactory()
        {
            _httpClient = new HttpClient();
            _isInitialized = false;
        }

        public static void InitializeApi(TokenResponse tokenResponse)
        {
            _httpClient.SetBearerToken(tokenResponse.AccessToken);
            _isInitialized = true;
        }

        public static ContactsDbApiService CreateApiClient()
        {
            if (_isInitialized)
                return new ContactsDbApiService(BaseUrls.ContactsDatabaseAPIUrl, _httpClient);
            else
                throw new Exception("Api not initialized!");
        }
    }
}
