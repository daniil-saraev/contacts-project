using Core.Interfaces;
using System;
using Desktop.Factories;
using Desktop.Commands;
using Desktop.Services;
using Desktop.ViewModels.Contacts;
using Desktop.Services.ExceptionHandler;
using DatabaseApi;

namespace Desktop.Commands.Contacts
{
    public class AddContactCommand : BaseCommand
    {
        private readonly IRepository<Contact> _contactsDb;
        private readonly ContactViewModel? _contactViewModel;

        public AddContactCommand(ContactViewModel contactViewModel, Func<object?, bool>? canExecuteCustom = null) : base(canExecuteCustom)
        {
            _contactsDb = RepositoryService.GetRepository();
            _contactViewModel = contactViewModel;
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _contactViewModel != null;
        }

        public async override void Execute(object? parameter)
        {
            try
            {
                await _contactsDb.AddAsync(ContactMVMFactory.GetContactModel(_contactViewModel));
            }
            catch (Exception ex)
            {
                exceptionHandler?.HandleException(new ExceptionContext(ex));
            }
        }
    }
}
