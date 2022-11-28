using Core.Contacts.Models;
using Desktop.Main.Contacts.ViewModels;

namespace Desktop.Tests.Main.ViewModels
{
    public class ContactViewModelValidationTests
    {
        private readonly ContactViewModel _contactViewModel;
        private bool _errorsChangedRaised;

        public ContactViewModelValidationTests()
        {
            _contactViewModel = new ContactViewModel(new ContactData());
            _contactViewModel.ErrorsChanged += (sender, args) =>
            {
                _errorsChangedRaised = true;
            };
        }

        [Fact]
        public void SetNullFirstNameTest()
        {
            // Arrange
            _errorsChangedRaised = false;

            // Act
            _contactViewModel.FirstName = null;

            // Assert
            Assert.True(_errorsChangedRaised);
            Assert.True(_contactViewModel.HasErrors);
        }

        [Fact]
        public void SetEmptyFirstNameTest()
        {
            // Arrange
            _errorsChangedRaised = false;

            // Act
            _contactViewModel.FirstName = string.Empty;

            //Assert
            Assert.True(_errorsChangedRaised);
            Assert.True(_contactViewModel.HasErrors);
        }

        [Fact]
        public void SetNullPhoneNumberTest()
        {
            // Arrange
            _errorsChangedRaised = false;

            // Act
            _contactViewModel.PhoneNumber = null;

            // Assert
            Assert.True(_errorsChangedRaised);
            Assert.True(_contactViewModel.HasErrors);
        }

        [Fact]
        public void SetEmptyPhoneNumberTest()
        {
            // Arrange
            _errorsChangedRaised = false;

            // Act
            _contactViewModel.PhoneNumber = string.Empty;

            // Assert
            Assert.True(_errorsChangedRaised);
            Assert.True(_contactViewModel.HasErrors);
        }

        [Fact]
        public void SetInvalidPhoneNumberTest()
        {
            // Arrange
            _errorsChangedRaised = false;

            // Act
            _contactViewModel.PhoneNumber = "phone number";

            // Assert
            Assert.True(_errorsChangedRaised);
            Assert.True(_contactViewModel.HasErrors);
        }

        [Fact]
        public void ValidateModelTest()
        {
            // Arrange
            _errorsChangedRaised = false;

            // Act
            _contactViewModel.ValidateModel();

            // Assert
            Assert.True(_errorsChangedRaised);
            Assert.True(_contactViewModel.HasErrors);
            Assert.NotEmpty(_contactViewModel.GetErrors(nameof(_contactViewModel.FirstName)));
            Assert.NotEmpty(_contactViewModel.GetErrors(nameof(_contactViewModel.PhoneNumber)));
        }

        [Fact]
        public void AddModelErrorTest()
        {
            // Arrange
            _errorsChangedRaised = false;

            // Act
            _contactViewModel.AddModelError(nameof(_contactViewModel.LastName), "Error");

            // Assert
            Assert.True(_errorsChangedRaised);
            Assert.True(_contactViewModel.HasErrors);
            Assert.NotEmpty(_contactViewModel.GetErrors(nameof(_contactViewModel.LastName)));
        }
    }
}
