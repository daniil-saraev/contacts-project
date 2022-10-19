using Desktop.Services.Authentication;
using Desktop.ViewModels.Account;
using OpenApi;
using System;
using System.Windows.Input;

namespace Desktop.Commands.Account
{
    public class RegisterCommand : BaseCommand
    {
        private readonly RegisterViewModel _registerViewModel;
        private readonly AuthenticationService _authenticationService;
        private readonly ICommand? _returnCommand;

        public RegisterCommand(RegisterViewModel registerViewModel, AuthenticationService authenticationService, ICommand? returnCommand)
        {
            _registerViewModel = registerViewModel;
            _authenticationService = authenticationService;
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
                await _authenticationService.RegisterAsync(_registerViewModel.Username, _registerViewModel.Email, _registerViewModel.Password);
                _returnCommand?.Execute(null);
            }
            catch (Exception ex)
            {
                _registerViewModel.AddModelError(nameof(_registerViewModel.Username), ex.Message);
                return;
            }          
        }
    }
}
