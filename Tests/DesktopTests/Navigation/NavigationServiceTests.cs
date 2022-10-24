using Desktop.Interactors;
using Desktop.Services.ExceptionHandler;
using Desktop.Services.Factories;
using Desktop.Services.Navigation;
using Desktop.ViewModels.Contacts;

namespace DesktopTests.Navigation
{
    public class NavigationServiceTests
    {
        private readonly SelectedContact _selectedContact;
        private readonly Mock<IContactsStore> _contactsStore;
        private readonly Mock<IViewModelsFactory> _viewModelsFactory;
        private readonly Mock<IExceptionHandler> _exceptionHandler;
        private readonly NavigationService _navigationService;

        public NavigationServiceTests()
        {
            _selectedContact = new SelectedContact();    
            _contactsStore = new Mock<IContactsStore>();
            _viewModelsFactory = new Mock<IViewModelsFactory>();
            _exceptionHandler = new Mock<IExceptionHandler>();
            _navigationService = new NavigationService(_viewModelsFactory.Object);           
        }

        [Fact]
        public void SetCurrentViewModelTest()
        {
            // Arrange
            var editViewModel = new ContactEditViewModel(_selectedContact, _contactsStore.Object, _navigationService, _exceptionHandler.Object);
            var infoViewModel = new ContactInfoViewModel(_selectedContact, _contactsStore.Object, _navigationService);
            _viewModelsFactory.Setup(factory => factory.GetViewModel<ContactEditViewModel>()).Returns(editViewModel);
            _viewModelsFactory.Setup(factory => factory.GetViewModel<ContactInfoViewModel>()).Returns(infoViewModel);
            _navigationService.NavigateTo<ContactEditViewModel>();
            bool currentViewModelChangedWasInvoked = false;
            _navigationService.CurrentViewModelChanged += () => currentViewModelChangedWasInvoked = true;

            // Act
            _navigationService.NavigateTo<ContactInfoViewModel>();

            // Assert
            Assert.Equal(infoViewModel, _navigationService.CurrentViewModel);
            Assert.True(currentViewModelChangedWasInvoked);
        }

        [Fact]
        public void ReturnTest()
        {
            // Arrange
            var editViewModel = new ContactEditViewModel(_selectedContact, _contactsStore.Object, _navigationService, _exceptionHandler.Object);
            var infoViewModel = new ContactInfoViewModel(_selectedContact, _contactsStore.Object, _navigationService);
            _viewModelsFactory.Setup(factory => factory.GetViewModel<ContactEditViewModel>()).Returns(editViewModel);
            _viewModelsFactory.Setup(factory => factory.GetViewModel<ContactInfoViewModel>()).Returns(infoViewModel);
            _navigationService.NavigateTo<ContactEditViewModel>();
            _navigationService.NavigateTo<ContactInfoViewModel>();
            bool currentViewModelChangedWasInvoked = false;
            _navigationService.CurrentViewModelChanged += () => currentViewModelChangedWasInvoked = true;

            // Act
            _navigationService.Return();

            // Assert
            Assert.Equal(editViewModel, _navigationService.CurrentViewModel);
            Assert.True(currentViewModelChangedWasInvoked);
        }

        [Fact]
        public void ReturnIfViewHistoryIsEmpty()
        {
            // Arrange
            var editViewModel = new ContactEditViewModel(_selectedContact, _contactsStore.Object, _navigationService, _exceptionHandler.Object);
            _viewModelsFactory.Setup(factory => factory.GetViewModel<ContactEditViewModel>()).Returns(editViewModel);
            _navigationService.NavigateTo<ContactEditViewModel>();
            bool currentViewModelChangedWasInvoked = false;
            _navigationService.CurrentViewModelChanged += () => currentViewModelChangedWasInvoked = true;

            // Act
            _navigationService.Return();

            // Assert
            Assert.Equal(editViewModel, _navigationService.CurrentViewModel);
            Assert.False(currentViewModelChangedWasInvoked);
        }
    }
}
