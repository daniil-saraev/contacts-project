using DatabaseApi;
using Core.Constants;
using ApiServices.Interfaces;
using ApiServices.Services;

namespace Web.Extentions
{
    public static class ApiServicesConfiguration
	{
		public static IServiceCollection AddApiServices(this IServiceCollection services)
		{
			IdentityService identityApiService = new IdentityService(BaseUrls.IdentityApiUrl);
			services.AddSingleton<IIdentityService>(identityApiService);
			services.AddSingleton<IApiService>(identityApiService);

			ContactsDatabaseService contactsDbApiService = new ContactsDatabaseService(BaseUrls.ContactsDatabaseApiUrl);
			services.AddSingleton<IRepository<Contact>>(contactsDbApiService);
			services.AddSingleton<IApiService>(contactsDbApiService);

			return services;
		}
	}
}
