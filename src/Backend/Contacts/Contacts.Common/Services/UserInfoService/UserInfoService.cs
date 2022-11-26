using System.Security.Claims;
using Core.Contacts.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Contacts.Common.Services;

internal class UserInfoService : IUserInfoService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserInfoService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
                return userId;
            else
                throw new UnauthorizedException();
        }
    }
}