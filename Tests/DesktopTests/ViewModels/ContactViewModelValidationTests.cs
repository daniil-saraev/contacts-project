using Desktop.ViewModels.Contacts;

namespace DesktopTests.ViewModels
{
    public class ContactViewModelValidationTests
    {
        private readonly ContactViewModel _contactViewModel;

        public ContactViewModelValidationTests()
        {
            _contactViewModel = new ContactViewModel(new Contact());
        }

        [Fact]
        public void SetNullFirstNameTest()
        {
            // Arrange
            bool errorsChangedRaised = false;
            _contactViewModel.ErrorsChanged += (sender, args) =>
            {
                errorsChangedRaised = true;
            };

            // Act
            _contactViewModel.FirstName = null;

            // Assert
            Assert.True(errorsChangedRaised);
            Assert.True(_contactViewModel.HasErrors);
        }

        [Fact]
        public void SetEmptyFirstNameTest()
        {
            // Arrange
            bool errorsChangedRaised = false;
            _contactViewModel.ErrorsChanged += (sender, args) =>
            {
                errorsChangedRaised = true;
            };
            
            // Act
            _contactViewModel.FirstName = string.Empty;

            //Assert
            Assert.True(errorsChangedRaised);
            Assert.True(_contactViewModel.HasErrors);
        }

        [Fact]
        public void SetNullPhoneNumberTest()
        {
            // Arrange
            bool errorsChangedRaised = false;
            _contactViewModel.ErrorsChanged += (sender, args) =>
            {
                errorsChangedRaised = true;
            };

            // Act
            _contactViewModel.PhoneNumber = null;

            // Assert
            Assert.True(errorsChangedRaised);
            Assert.True(_contactViewModel.HasErrors);
        }

        [Fact]
        public void SetEmptyPhoneNumberTest()
        {
            // Arrange
            bool errorsChangedRaised = false;
            _contactViewModel.ErrorsChanged += (sender, args) =>
            {
                errorsChangedRaised = true;
            };

            // Act
            _contactViewModel.PhoneNumber = string.Empty;

            // Assert
            Assert.True(errorsChangedRaised);
            Assert.True(_contactViewModel.HasErrors);
        }

        [Fact]
        public void SetInvalidPhoneNumberTest()
        {
            // Arrange
            bool errorsChangedRaised = false;
            _contactViewModel.ErrorsChanged += (sender, args) =>
            {
                errorsChangedRaised = true;
            };

            // Act
            _contactViewModel.PhoneNumber = "phone number";

            // Assert
            Assert.True(errorsChangedRaised);
            Assert.True(_contactViewModel.HasErrors);
        }

        [Fact]
        public void ContactViewModelDisposedTest()
        {
            // Arrange
            bool errorsChangedRaised = false;
            _contactViewModel.ErrorsChanged += (sender, args) =>
            {
                errorsChangedRaised = true;
            };

            // Act
            _contactViewModel.Dispose();

            // Assert
            Assert.True(errorsChangedRaised);
            Assert.True(_contactViewModel.HasErrors);
        }
    }
}
