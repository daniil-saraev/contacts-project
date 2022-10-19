namespace IdentityAPI.Responses
{
    public class TokenResponse
    {
        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }

        public string? ErrorMessage { get; set; }

        public bool IsSuccessful { get; set; }
    }
}
