using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using static Core.Identity.Constants.TokenConstants;

namespace Web.Authentication
{
    public class AuthenticationHeaderHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationHeaderHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Sets current access token as authentication header.
        /// </summary>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string? accessToken = _httpContextAccessor.HttpContext?.User.FindFirst(ACCESS_TOKEN)?.Value;
            if(accessToken != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
