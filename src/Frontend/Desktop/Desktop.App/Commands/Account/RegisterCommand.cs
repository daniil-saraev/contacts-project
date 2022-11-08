using Core.Exceptions.Identity;
using System;
using System.Windows.Input;

namespace Desktop.App.Commands.Account
{
    public class RegisterCommand : BaseCommand
    {
        private readonly RegisterViewModel _registerViewModel;
        private readonly IAuthenticationService _authenticationService;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ICommand? _returnCommand;

        public RegisterCommand(RegisterViewModel registerViewModel, IAuthenticationService authenticationService, IExceptionHandler exceptionHandler, ICommand? returnCommand)
        {
            _registerViewModel = registerViewModel;
            _authenticationService = authenticationService;
            _exceptionHandler = exceptionHandler;
            _returnCommand = returnCommand;
            _registerViewModel.ErrorsChanged += RegisterViewModel_ErrorsChanged;
        }

        private void RegisterViewModel_ErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && !_registerViewModel.HasErrors;
        }

        public override async void Execute(object? parameter)
        {
            _registerViewModel.ValidateModel();
            if (_registerViewModel.HasErrors)
                return;

            try
            {
                await _authenticationService.Register(_registerViewModel.Username, _registerViewModel.Email, _registerViewModel.Password);
                _returnCommand?.Execute(null);
            }
            catch (DuplicateEmailsException ex)
            {
                _registerViewModel.AddModelError(nameof(_registerViewModel.Email), ex.Message);
            }
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(ex);
            }
        }
    }
}
