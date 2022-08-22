using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace ContactsDatabaseAPI.Middleware
{
    public class UserIdMiddleware
    {
        private readonly RequestDelegate _next;

        public UserIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string? accessToken = await context.GetTokenAsync("access_token");
            if (accessToken != null)
            {
                //var handler = new JwtSecurityTokenHandler();
                //var token = handler.ReadJwtToken(accessToken);
                //context.User.Claims.Append(token.Claims.First(c => c.Type == "id"));
                context.Request.Headers.Add("Authorization", "Bearer " + accessToken);
            }
            await _next.Invoke(context);
        }
    }
}
