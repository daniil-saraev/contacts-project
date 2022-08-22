namespace IdentityAPI.Responses
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; }

        public bool IsSuccessful { get; set; }
    }
}
