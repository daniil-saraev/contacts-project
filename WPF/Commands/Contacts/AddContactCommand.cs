using System;
using Desktop.ViewModels.Contacts;
using System.Windows.Input;
using Desktop.Services.Authentication.UserServices;
using Desktop.Services.Containers;

namespace Desktop.Commands.Contacts
{
    public class AddContactCommand : BaseCommand
    {
        private readonly IContactsStore _contactStore;
        private readonly ContactViewModel _newContactViewModel;
        private readonly ICommand? _returnCommand;

        public AddContactCommand(ContactViewModel newContactViewModel, IContactsStore contactsStore,
                                 ICommand? returnCommand, Func<object?, bool>? canExecuteCustom = null) : base(canExecuteCustom)
        {
            _contactStore = contactsStore;
            _newContactViewModel = newContactViewModel;
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

        public override void Execute(object? parameter)
        {
            _newContactViewModel.ValidateModel();
            if (_newContactViewModel.HasErrors)
                return;
            if(User.IsAuthenticated)
                _newContactViewModel.GetContact().SetUserId(User.Id);
            _contactStore.AddContact(_newContactViewModel.GetContact());
            _newContactViewModel.Dispose();
            _returnCommand?.Execute(null);
        }
    }
}
