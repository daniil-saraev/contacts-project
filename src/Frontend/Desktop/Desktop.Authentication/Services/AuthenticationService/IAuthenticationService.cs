namespace Desktop.Authentication.Services;

public interface IAuthenticationService
{
    Task Login(string login, string password);

    Task Register(string username, string email, string password);

    Task Logout();

    Task Refresh(string refreshToken, string userId);
}