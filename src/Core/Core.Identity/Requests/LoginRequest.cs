using System.ComponentModel.DataAnnotations;

namespace Core.Identity.Requests
{
    public struct LoginRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
