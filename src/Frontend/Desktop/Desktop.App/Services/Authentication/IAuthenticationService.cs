using System.Threading.Tasks;

namespace Desktop.App.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task Login(string login, string password);

        Task Register(string username, string email, string password);

        Task Logout();

        Task RestoreSession();
    }
}