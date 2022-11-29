using System;
using Desktop.Common.Commands;
using Desktop.Common.Services;
using Desktop.Common.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Desktop.Main.Common.Commands;

public class NavigateTo<T> : BaseCommand where T : BaseViewModel
{
    private INavigationService _navigationService => ServiceProvider.GetRequiredService<INavigationService>();

    public NavigateTo(Func<bool>? canExecuteCustom = null) : base(canExecuteCustom)
    {
    }

    public override void Execute(object? parameter = null)
    {
        _navigationService.NavigateTo<T>();
    }
}