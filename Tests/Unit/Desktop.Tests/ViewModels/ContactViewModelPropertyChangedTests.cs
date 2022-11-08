using DesktopApp.ViewModels.Contacts;

namespace Desktop.Tests.ViewModels
{
    public class ContactViewModelPropertyChangedTests
    {
        private readonly ContactViewModel _contactViewModel;

        public ContactViewModelPropertyChangedTests()
        {
            _contactViewModel = new ContactViewModel(new Contact());
        }

        [Fact]
        public void SetFirstNameTest()
        {
            // Arrange
            var propertyChangedRaised = false;
            _contactViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(_contactViewModel.FirstName))
                    propertyChangedRaised = true;
            };

            // Act 
            _contactViewModel.FirstName = "Ivan";

            // Assert
            Assert.True(propertyChangedRaised);
        }

        [Fact]
        public void SetMiddleNameTest()
        {
            // Arrange
            var propertyChangedRaised = false;
            _contactViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(_contactViewModel.MiddleName))
                    propertyChangedRaised = true;
            };

            // Act 
            _contactViewModel.MiddleName = "Ivanovich";

            // Assert
            Assert.True(propertyChangedRaised);
        }

        [Fact]
        public void SetLastNameTest()
        {
            // Arrange
            var propertyChangedRaised = false;
            _contactViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(_contactViewModel.LastName))
                    propertyChangedRaised = true;
            };

            // Act 
            _contactViewModel.LastName = "Ivanov";

            // Assert
            Assert.True(propertyChangedRaised);
        }

        [Fact]
        public void SetPhoneNumberTest()
        {
            // Arrange
            var propertyChangedRaised = false;
            _contactViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(_contactViewModel.PhoneNumber))
                    propertyChangedRaised = true;
            };

            // Act 
            _contactViewModel.PhoneNumber = "+79998887766";

            // Assert
            Assert.True(propertyChangedRaised);
        }

        [Fact]
        public void SetAddressTest()
        {
            // Arrange
            var propertyChangedRaised = false;
            _contactViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(_contactViewModel.Address))
                    propertyChangedRaised = true;
            };

            // Act 
            _contactViewModel.Address = "Moscow";

            // Assert
            Assert.True(propertyChangedRaised);
        }

        [Fact]
        public void SetDescriptionTest()
        {
            // Arrange
            var propertyChangedRaised = false;
            _contactViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(_contactViewModel.Description))
                    propertyChangedRaised = true;
            };

            // Act 
            _contactViewModel.Description = "Senior developer";

            // Assert
            Assert.True(propertyChangedRaised);
        }
    }
}
