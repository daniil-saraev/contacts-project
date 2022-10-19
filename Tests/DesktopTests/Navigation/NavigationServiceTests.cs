using Desktop.Containers;
using Desktop.Services.Containers;
using Desktop.Services.Factories;
using Desktop.Services.Navigation;
using Desktop.ViewModels.Contacts;

namespace DesktopTests.Navigation
{
    public class NavigationServiceTests
    {
        private readonly NavigationService _navigationService;
        private readonly Mock<IContactsStore> _contactsStore;
        private readonly SelectedContact _selectedContact;
        private readonly Mock<IViewModelsFactory> _viewModelsFactory;

        public NavigationServiceTests()
        {
            _contactsStore = new Mock<IContactsStore>();
            _selectedContact = new SelectedContact();           
            _viewModelsFactory = new Mock<IViewModelsFactory>();
            _navigationService = new NavigationService(_viewModelsFactory.Object);           
        }

        [Fact]
        public void SetCurrentViewModelTest()
        {
            // Arrange
            var editViewModel = new ContactEditViewModel(_selectedContact, _contactsStore.Object, _navigationService);
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
            var editViewModel = new ContactEditViewModel(_selectedContact, _contactsStore.Object, _navigationService);
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
            var editViewModel = new ContactEditViewModel(_selectedContact, _contactsStore.Object, _navigationService);
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
