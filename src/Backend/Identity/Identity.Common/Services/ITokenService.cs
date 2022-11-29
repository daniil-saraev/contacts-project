using System.Security.Claims;

namespace Identity.Common.Services
{
    public interface ITokenService
    {
        /// <summary>
        /// Generates a token.
        /// </summary>
        /// <param name="expirationMinutes">How long a token is valid once it's generated.</param>
        /// <param name="claims"> Claims to include into a token.</param>
        /// <returns>
        /// A token.
        /// </returns>
        string GenerateToken(string secret, int expirationMinutes, IEnumerable<Claim>? claims);

        /// <summary>
        /// Validates a token.
        /// </summary>
        /// <returns>True if token is valid, false if token has expired or secret is invalid.</returns>
        bool ValidateToken(string token, string secret);
    }
}
