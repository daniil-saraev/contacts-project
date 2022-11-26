using CommunityToolkit.Mvvm.Input;
using Desktop.Authentication.Models;
using Desktop.Authentication.Services;
using Desktop.Common.Commands.Async;
using Desktop.Common.Exceptions;
using Desktop.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Desktop.Main.Account.Commands;

internal class RestoreUserSessionCommand : AsyncBaseCommand
{
    private IUserDataStorage _userDataStorage => ServiceProvider.GetRequiredService<IUserDataStorage>();
    private User _user => ServiceProvider.GetRequiredService<User>();
    private IExceptionHandler _exceptionHandler => ServiceProvider.GetRequiredService<IExceptionHandler>();

    public override async Task ExecuteAsync()
    {
        try
        {
            var data = await _userDataStorage.LoadData();
            if (data.HasValue)
                _user.Authenticate(
                    data.Value.AccessToken,
                    data.Value.RefreshToken,
                    data.Value.Id,
                    data.Value.Email,
                    data.Value.Name);
        }
        catch (ReadingDataException ex)
        {
            _exceptionHandler.HandleException(ex);
        }
    }
}
