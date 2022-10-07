using OpenApi;
using Core.Constants;
using ApiServices.Interfaces;
using ApiServices.Services;

namespace Web.Extentions
{
    public static class ApiServicesConfiguration
	{
		public static IServiceCollection AddApiServices(this IServiceCollection services)
		{
			IdentityApiService identityApiService = new IdentityApiService(BaseUrls.IDENTITY_API_URL);
			services.AddSingleton<IIdentityApi>(identityApiService);
			services.AddSingleton<IApiService>(identityApiService);

			ContactsDatabaseApiService contactsDbApiService = new ContactsDatabaseApiService(BaseUrls.CONTACTS_DATABASE_API_URL);
			services.AddSingleton<IRepository<Contact>>(contactsDbApiService);
			services.AddSingleton<IApiService>(contactsDbApiService);

			return services;
		}
	}
}
