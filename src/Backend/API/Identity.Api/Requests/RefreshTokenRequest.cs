using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Requests
{
    [Serializable]
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }

        public string UserId { get; set; }
    }
}
