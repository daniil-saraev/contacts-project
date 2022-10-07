using Core.Exceptions.Identity;
using Desktop.Services.Authentication;
using Desktop.ViewModels.Account;
using OpenApi;
using System;

namespace Desktop.Commands.Account
{
    public class LoginCommand : BaseCommand
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly AuthenticationService _authenticationService;

        public LoginCommand(LoginViewModel loginViewModel, AuthenticationService authenticationService)
        {
            _loginViewModel = loginViewModel;
            _authenticationService = authenticationService;
            _loginViewModel.ErrorsChanged += LoginViewModel_ErrorsChanged;
        }

        private void LoginViewModel_ErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && !_loginViewModel.HasErrors;
        }

        public override async void Execute(object? parameter)
        {
            _loginViewModel.ValidateModel();
            if (_loginViewModel.HasErrors)
                return;

            try
            {
                await _authenticationService.LoginAsync(_loginViewModel.Email, _loginViewModel.Password);
                _loginViewModel.Return.Execute(null);
            }
            catch (WrongPasswordException ex)
            {
                _loginViewModel.AddModelError(nameof(_loginViewModel.Password), ex.Message);
                return;
            }          
            catch (UserNotFoundException ex)
            {
                _loginViewModel.AddModelError(nameof(_loginViewModel.Email), ex.Message);
                return;
            }
        }
    }
}
