using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Models.Requests
{
    [Serializable]
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }

        public string UserId { get; set; }
    }
}
