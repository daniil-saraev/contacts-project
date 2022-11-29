using Core.Identity.Exceptions;
using Desktop.Authentication.Services;
using Desktop.Common.Commands.Async;
using Desktop.Common.Services;
using Desktop.Main.Account.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Desktop.Main.Account.Commands
{
    public class LoginCommand : AsyncBaseCommand
    {
        private IAuthenticationService _authenticationService => ServiceProvider.GetRequiredService<IAuthenticationService>();
        private IExceptionHandler _exceptionHandler => ServiceProvider.GetRequiredService<IExceptionHandler>();
        private readonly LoginViewModel _loginViewModel;
        private readonly ICommand? _returnCommand;

        public LoginCommand(LoginViewModel loginViewModel, ICommand? returnCommand)
        {
            _loginViewModel = loginViewModel;
            _returnCommand = returnCommand;
            _loginViewModel.ErrorsChanged += ErrorsChanged;
        }

        private void ErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && !_loginViewModel.HasErrors;
        }

        public override async Task ExecuteAsync()
        {
            _loginViewModel.ValidateModel();
            if (_loginViewModel.HasErrors)
                return;

            try
            {
                await _authenticationService.Login(_loginViewModel.Email, _loginViewModel.Password);
                _returnCommand?.Execute(null);
            }
            catch (WrongPasswordException ex)
            {
                _loginViewModel.AddModelError(nameof(_loginViewModel.Email), ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                _loginViewModel.AddModelError(nameof(_loginViewModel.Email), ex.Message);
            }
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(ex);
            }
        }
    }
}
