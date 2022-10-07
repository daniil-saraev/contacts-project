using Desktop.Services.Authentication;
using Desktop.ViewModels.Account;
using OpenApi;
using System;

namespace Desktop.Commands.Account
{
    public class RegisterCommand : BaseCommand
    {
        private readonly RegisterViewModel _registerViewModel;
        private readonly AuthenticationService _authenticationService;

        public RegisterCommand(RegisterViewModel registerViewModel, AuthenticationService authenticationService)
        {
            _registerViewModel = registerViewModel;
            _authenticationService = authenticationService;
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
                _registerViewModel.Return.Execute(null);
            }
            catch (Exception ex)
            {
                _registerViewModel.AddModelError(nameof(_registerViewModel.Username), ex.Message);
                return;
            }          
        }
    }
}
