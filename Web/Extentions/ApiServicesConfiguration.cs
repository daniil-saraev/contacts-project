using ApiServices;
using Core.Constants;
using Core.Interfaces;

namespace Web.Extentions
{
	public static class ApiServicesConfiguration
	{
		public static IServiceCollection AddApiServices(this IServiceCollection services)
		{
			services.AddSingleton(typeof(IRepository<Contact>), new ContactsDbApiService(BaseUrls.ContactsWebApiUrl, new HttpClient()));
			return services;
		}
	}
}
