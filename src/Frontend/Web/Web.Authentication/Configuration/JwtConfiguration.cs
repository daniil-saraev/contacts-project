namespace Web.Authentication.Configuration
{
    internal class JwtConfiguration
    {
        public string AccessTokenSecret { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
    }
}
