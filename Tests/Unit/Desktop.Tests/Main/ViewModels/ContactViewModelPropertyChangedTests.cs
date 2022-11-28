using Core.Contacts.Models;
using Desktop.Main.Contacts.ViewModels;

namespace Desktop.Tests.Main.ViewModels
{
    public class ContactViewModelPropertyChangedTests
    {
        private readonly ContactViewModel _contactViewModel;
        private bool _propertyChangedRaised;

        public ContactViewModelPropertyChangedTests()
        {
            _contactViewModel = new ContactViewModel(new ContactData());
            _contactViewModel.PropertyChanged += (sender, e) =>
            {
                _propertyChangedRaised = true;
            };
        }

        [Fact]
        public void SetFirstNameTest()
        {
            // Arrange
            _propertyChangedRaised = false;
            
            // Act 
            _contactViewModel.FirstName = "Ivan";

            // Assert
            Assert.True(_propertyChangedRaised);
        }

        [Fact]
        public void SetMiddleNameTest()
        {
            // Arrange
            _propertyChangedRaised = false;

            // Act 
            _contactViewModel.MiddleName = "Ivanovich";

            // Assert
            Assert.True(_propertyChangedRaised);
        }

        [Fact]
        public void SetLastNameTest()
        {
            // Arrange
            _propertyChangedRaised = false;

            // Act 
            _contactViewModel.LastName = "Ivanov";

            // Assert
            Assert.True(_propertyChangedRaised);
        }

        [Fact]
        public void SetPhoneNumberTest()
        {
            // Arrange
            _propertyChangedRaised = false;

            // Act 
            _contactViewModel.PhoneNumber = "+79998887766";

            // Assert
            Assert.True(_propertyChangedRaised);
        }

        [Fact]
        public void SetAddressTest()
        {
            // Arrange
            _propertyChangedRaised = false;

            // Act 
            _contactViewModel.Address = "Moscow";

            // Assert
            Assert.True(_propertyChangedRaised);
        }

        [Fact]
        public void SetDescriptionTest()
        {
            // Arrange
            _propertyChangedRaised = false;

            // Act 
            _contactViewModel.Description = "Senior developer";

            // Assert
            Assert.True(_propertyChangedRaised);
        }
    }
}
