namespace IdentityAPI.Configuration
{
    public class AuthConfiguration
    {
        public string AccessTokenSecret { get; set; }
        public string RefreshTokenSecret { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationMinutes { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
}
