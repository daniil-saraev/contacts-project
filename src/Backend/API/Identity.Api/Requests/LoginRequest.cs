using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Requests
{
    [Serializable]
    public class LoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
