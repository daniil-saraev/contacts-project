using System.ComponentModel.DataAnnotations;

namespace Core.Identity.Requests
{
    public struct RegisterRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
