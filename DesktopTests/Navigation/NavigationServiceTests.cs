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
        private readonly IContactsStore _contactsStore;
        private readonly SelectedContact _selectedContact;
        private readonly IViewModelsFactory _viewModelsFactory;

        public NavigationServiceTests()
        {
            _navigationService = NavigationService.GetInstance();
            _contactsStore = new Mock<IContactsStore>().Object;
            _selectedContact = new SelectedContact();           
            _viewModelsFactory = new Mock<IViewModelsFactory>().Object;
        }

        [Fact]
        public void SetCurrentViewModelTest()
        {
            // Arrange
            var initialViewModel = new ContactInfoViewModel(_selectedContact, _contactsStore, _viewModelsFactory);
            _navigationService.CurrentViewModel = initialViewModel;
            var newViewModel = new ContactEditViewModel(_selectedContact, _contactsStore);
            bool currentViewModelChangedWasInvoked = false;
            _navigationService.CurrentViewModelChanged += () => currentViewModelChangedWasInvoked = true;

            // Act
            _navigationService.CurrentViewModel = newViewModel;

            // Assert
            Assert.Equal(newViewModel, _navigationService.CurrentViewModel);
            Assert.True(currentViewModelChangedWasInvoked);
        }

        [Fact]
        public void SetCurrentViewModelIfNullTest()
        {
            // Arrange
            var initialViewModel = new ContactInfoViewModel(_selectedContact, _contactsStore, _viewModelsFactory);
            _navigationService.CurrentViewModel = initialViewModel;
            bool currentViewModelChangedWasInvoked = false;
            _navigationService.CurrentViewModelChanged += () => currentViewModelChangedWasInvoked = true;

            // Act
            _navigationService.CurrentViewModel = null;

            // Assert
            Assert.Equal(initialViewModel, _navigationService.CurrentViewModel);
            Assert.False(currentViewModelChangedWasInvoked);
        }

        [Fact]
        public void ReturnTest()
        {
            // Arrange
            var initialViewModel = new ContactInfoViewModel(_selectedContact, _contactsStore, _viewModelsFactory);
            _navigationService.CurrentViewModel = initialViewModel;
            _navigationService.CurrentViewModel = new ContactEditViewModel(_selectedContact, _contactsStore);
            bool currentViewModelChangedWasInvoked = false;
            _navigationService.CurrentViewModelChanged += () => currentViewModelChangedWasInvoked = true;

            // Act
            _navigationService.Return();

            // Assert
            Assert.Equal(initialViewModel, _navigationService.CurrentViewModel);
            Assert.True(currentViewModelChangedWasInvoked);
        }

        [Fact]
        public void ReturnIfViewHistoryIsEmpty()
        {
            // Arrange
            var initialViewModel = new ContactInfoViewModel(_selectedContact, _contactsStore, _viewModelsFactory);
            _navigationService.CurrentViewModel = initialViewModel;
            bool currentViewModelChangedWasInvoked = false;
            _navigationService.CurrentViewModelChanged += () => currentViewModelChangedWasInvoked = true;

            // Act
            _navigationService.Return();

            // Assert
            Assert.Equal(initialViewModel, _navigationService.CurrentViewModel);
            Assert.False(currentViewModelChangedWasInvoked);
        }
    }
}
