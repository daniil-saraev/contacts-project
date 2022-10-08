﻿using System;
using Desktop.ViewModels.Contacts;
using System.Windows.Input;
using Desktop.Services.Data;

namespace Desktop.Commands.Contacts
{
    public class AddContactCommand : BaseCommand
    {
        private readonly ContactsStore _contactStore;
        private readonly ContactViewModel _newContactViewModel;
        private readonly ICommand? _returnCommand;

        public AddContactCommand(ContactViewModel newContactViewModel, ContactsStore contactsStore,
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

        public override async void Execute(object? parameter)
        {
            _newContactViewModel.ValidateModel();
            if (_newContactViewModel.HasErrors)
                return;
            await _contactStore.AddContact(_newContactViewModel.GetContact());
            _returnCommand?.Execute(null);
        }
    }
}
