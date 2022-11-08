using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Requests
{
    [Serializable]
    public class RegisterRequest
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
