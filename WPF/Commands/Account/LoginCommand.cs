using Desktop.Services.Authentication;
using Desktop.ViewModels.Account;
using IdentityApi;
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

            LoginRequest loginRequest = new LoginRequest
            {
                Email = _loginViewModel.Email,
                Password = _loginViewModel.Password
            };

            try
            {
                await _authenticationService.LoginAsync(loginRequest);
                _loginViewModel.Return.Execute(null);
            }
            catch (Exception)
            {
                //to do
            }          
        }
    }
}
