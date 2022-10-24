using Core.Exceptions.Identity;
using Desktop.Services.Authentication;
using Desktop.Services.ExceptionHandler;
using Desktop.ViewModels.Account;
using System;
using System.Windows.Input;

namespace Desktop.Commands.Account
{
    public class LoginCommand : BaseCommand
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly IAuthenticationService _authenticationService;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ICommand? _returnCommand;

        public LoginCommand(LoginViewModel loginViewModel, IAuthenticationService authenticationService, IExceptionHandler exceptionHandler, ICommand? returnCommand)
        {
            _loginViewModel = loginViewModel;
            _authenticationService = authenticationService;
            _exceptionHandler = exceptionHandler;
            _returnCommand = returnCommand;
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
            catch(Exception ex)
            {
                _exceptionHandler.HandleException(ex);
            }
        }
    }
}
