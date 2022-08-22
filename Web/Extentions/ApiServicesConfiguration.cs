using DatabaseApi;
using Core.Constants;
using Core.Interfaces;
using IdentityApi;

namespace Web.Extentions
{
	public static class ApiServicesConfiguration
	{
		public static IServiceCollection AddApiServices(this IServiceCollection services)
		{
			IdentityApiService identityApiService = new IdentityApiService(BaseUrls.IdentityServerUrl, new HttpClient());
			services.AddSingleton(identityApiService);
			services.AddSingleton<IApiService>(identityApiService);

			ContactsDbApiService contactsDbApiService = new ContactsDbApiService(BaseUrls.ContactsDatabaseAPIUrl, new HttpClient());
			services.AddSingleton<IRepository<Contact>>(contactsDbApiService);
			services.AddSingleton<IApiService>(contactsDbApiService);

			return services;
		}
	}
}
