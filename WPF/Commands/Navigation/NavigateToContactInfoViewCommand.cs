using Desktop.Containers;
using Desktop.Services.Factories;
using Desktop.Services.Navigation;
using Desktop.ViewModels.Contacts;
using System;

namespace Desktop.Commands.Navigation
{
    public class NavigateToContactInfoViewCommand : BaseCommand
    {
        private readonly NavigationService _navigationService;
        private readonly SelectedContact _selectedContact;
        private readonly IViewModelsFactory _viewModelsFactory;

        public NavigateToContactInfoViewCommand(SelectedContact selectedContact, IViewModelsFactory viewModelsFactory, Func<object?, bool>? canExecuteCustom = null) : base(canExecuteCustom)
        {
            _navigationService = NavigationService.GetInstance();
            _viewModelsFactory = viewModelsFactory;
            _selectedContact = selectedContact;
            _selectedContact.ContactChanged += SelectedContact_ContactChanged;
        }

        private void SelectedContact_ContactChanged()
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _selectedContact.Contact != null;
        }

        public override void Execute(object? parameter)
        {
            _navigationService.CurrentViewModel = _viewModelsFactory.GetViewModel<ContactInfoViewModel>();
        }
    }
}
