using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Requests
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
