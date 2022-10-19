namespace IdentityAPI.Middleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.Cookies.TryGetValue("access_token", out string? token);
            if (!string.IsNullOrEmpty(token))
            {
                context.Request.Headers.Add("Authorization", "Bearer " + token);
            }

            await _next.Invoke(context);
        }
    }
}
