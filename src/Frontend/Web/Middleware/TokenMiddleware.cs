using ApiServices.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace Web.Middleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEnumerable<IApiService> _apiServices;

        public TokenMiddleware(RequestDelegate next, IEnumerable<IApiService> apiServices)
        {
            _next = next;
            _apiServices = apiServices;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.Cookies.TryGetValue("access_token", out string? token);
            token ??= context.Session.GetString("access_token");
            if(context.Request.Headers.Contains(new KeyValuePair<string, StringValues>("Authorization", "Bearer " + token)))
            {
                await _next.Invoke(context);
                return;
            }

            if (!string.IsNullOrEmpty(token))
            {            
                context.Request.Headers.Add("Authorization", "Bearer " + token);
                foreach (var service in _apiServices)
                    service.InitializeToken(token);
            }

            await _next.Invoke(context);
        }
    }
}
