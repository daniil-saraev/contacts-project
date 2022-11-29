using Core.Identity.Exceptions;
using Desktop.Authentication.Services;
using Desktop.Common.Commands;
using Desktop.Common.Commands.Async;
using Desktop.Common.Services;
using Desktop.Main.Account.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Desktop.Main.Account.Commands
{
    public class RegisterCommand : AsyncBaseCommand
    {
        private IAuthenticationService _authenticationService => ServiceProvider.GetRequiredService<IAuthenticationService>();
        private IExceptionHandler _exceptionHandler => ServiceProvider.GetRequiredService<IExceptionHandler>();
        private readonly RegisterViewModel _registerViewModel;
        private readonly ICommand? _returnCommand;

        public RegisterCommand(RegisterViewModel registerViewModel, ICommand? returnCommand)
        {
            _registerViewModel = registerViewModel;
            _returnCommand = returnCommand;
            _registerViewModel.ErrorsChanged += ErrorsChanged;
        }

        private void ErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && !_registerViewModel.HasErrors;
        }

        public override async Task ExecuteAsync()
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
