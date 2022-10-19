using System.Security.Claims;

namespace IdentityAPI.Data
{
    public static class ClaimStore
    {
        public static Claim AdminClaim = new Claim(ClaimTypes.Role, "Administrator");
    }

    public static class Policies
    {
        public const string RequireAdmin = "RequireAdmin";
    }
}
