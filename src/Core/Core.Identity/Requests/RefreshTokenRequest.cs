using System.ComponentModel.DataAnnotations;

namespace Core.Identity.Requests
{
    public struct RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
