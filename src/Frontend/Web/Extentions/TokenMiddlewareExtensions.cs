using Web.Configuration;
using Web.Middleware;
using OpenApi;
using ApiServices.Interfaces;

namespace Web.Extentions
{
    public static class TokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseToken(this IApplicationBuilder builder, IEnumerable<IApiService> apiServices)
        {
            return builder.UseMiddleware<TokenMiddleware>(apiServices);
        }

        public static IApplicationBuilder UseRefreshToken(this IApplicationBuilder builder, IIdentityApi identityApi, AuthConfiguration configuration)
        {
            return builder.UseMiddleware<RefreshTokenMiddleware>(identityApi, configuration);
        }
    }
}
