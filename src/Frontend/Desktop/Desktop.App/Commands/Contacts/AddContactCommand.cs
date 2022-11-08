using System;
using System.Windows.Input;

namespace Desktop.App.Commands.Contacts
{
    public class AddContactCommand : BaseCommand
    {
        private readonly IContactsStore _contactStore;
        private readonly ContactViewModel _newContactViewModel;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ICommand? _returnCommand;

        public AddContactCommand(ContactViewModel newContactViewModel, IContactsStore contactsStore, IExceptionHandler exceptionHandler,
                                 ICommand? returnCommand, Func<object?, bool>? canExecuteCustom = null) : base(canExecuteCustom)
        {
            _contactStore = contactsStore;
            _newContactViewModel = newContactViewModel;
            _exceptionHandler = exceptionHandler;
            _returnCommand = returnCommand;
            _newContactViewModel.ErrorsChanged += NewContactViewModel_ErrorsChanged;
        }

        private void NewContactViewModel_ErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _newContactViewModel.GetContact() != null && !_newContactViewModel.HasErrors;
        }

        public override async void Execute(object? parameter)
        {
            _newContactViewModel.ValidateModel();
            if (_newContactViewModel.HasErrors)
                return;

            if (User.IsAuthenticated)
                _newContactViewModel.GetContact().SetUserId(User.Id);

            try
            {
                _contactStore.AddContact(_newContactViewModel.GetContact());
                await _contactStore.SaveContactsAsync();
            }
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(ex);
            }
            finally
            {
                _newContactViewModel.Dispose();
                _returnCommand?.Execute(null);
            }
        }
    }
}
