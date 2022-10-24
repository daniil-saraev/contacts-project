using Web.Middleware;
using Core.Interfaces;
using Web.Configuration;
using ApiServices.Interfaces;

namespace Web.Extentions
{
    public static class TokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseToken(this IApplicationBuilder builder, IEnumerable<IApiService> apiServices)
        {
            return builder.UseMiddleware<TokenMiddleware>(apiServices);
        }
    }
}
