using Core.Models;
using ApiServices.Services;
using Core.Interfaces;
using Web.HttpClientHandlers;
using ApiServices.Interfaces;

namespace Web.Extentions
{
    public static class ApiServicesConfiguration
	{
		public static IServiceCollection AddApiServices(this IServiceCollection services)
		{
			services.AddSingleton<IIdentityApi, IdentityApiService>();

			services.AddSingleton<ITokenValidator, TokenValidator>();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        	services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

			services.AddHttpClient<IRepository<Contact>, ContactsDatabaseApiService>()
                	.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

			return services;
		}
	}
}
