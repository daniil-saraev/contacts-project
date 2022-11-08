using DesktopApp.Services.Data.UnitOfWork;

namespace Desktop.Tests.Data.UnitOfWork
{
    public class UnitOfWorkTests
    {
        [Fact]
        public void RegisterSyncedEntityIfEntityWasNewTest()
        {
            // Arrange
            var unitOfWork = new UnitOfWork<Contact>();
            var contact = new Contact();
            unitOfWork.UnitOfWorkState = new UnitOfWorkState<Contact>()
            {
                NewEntities = new List<Contact> { contact }
            };

            // Act
            unitOfWork.RegisterSyncedEntity(contact);

            // Assert
            Assert.Contains<Contact>(contact, unitOfWork.SyncedEntities);
            Assert.DoesNotContain<Contact>(contact, unitOfWork.NewEntities);
            Assert.DoesNotContain<Contact>(contact, unitOfWork.RemovedEntities);
        }

        [Fact]
        public void RegisterSyncedEntityIfEntityWasRemovedBeforeTest()
        {
            // Arrange
            var unitOfWork = new UnitOfWork<Contact>();
            var contact = new Contact();
            unitOfWork.UnitOfWorkState = new UnitOfWorkState<Contact>()
            {
                RemovedEntities = new List<Contact> { contact }
            };

            // Act
            unitOfWork.RegisterSyncedEntity(contact);

            // Assert
            Assert.DoesNotContain<Contact>(contact, unitOfWork.SyncedEntities);
            Assert.DoesNotContain<Contact>(contact, unitOfWork.NewEntities);
            Assert.DoesNotContain<Contact>(contact, unitOfWork.RemovedEntities);
        }


        [Fact]
        public void RegisterNewEntityIfEntityIsUniqueTest()
        {
            // Arrange
            var unitOfWork = new UnitOfWork<Contact>();
            var contact = new Contact();

            // Act
            unitOfWork.RegisterNewEntity(contact);

            // Assert
            Assert.Contains<Contact>(contact, unitOfWork.NewEntities);
            Assert.DoesNotContain<Contact>(contact, unitOfWork.SyncedEntities);
            Assert.DoesNotContain<Contact>(contact, unitOfWork.RemovedEntities);
        }

        [Fact]
        public void RegisterNewEntityIfEntityWasAddedBeforeTest()
        {
            // Arrange
            var unitOfWork = new UnitOfWork<Contact>();
            var contact = new Contact();
            unitOfWork.UnitOfWorkState = new UnitOfWorkState<Contact>()
            {
                NewEntities = new List<Contact> { contact }
            };

            // Act
            unitOfWork.RegisterNewEntity(contact);

            // Assert
            Assert.Single(unitOfWork.NewEntities, contact);
        }

        [Fact]
        public void RegisterNewEntityIfEntityWasSyncedBeforeTest()
        {
            // Arrange
            var unitOfWork = new UnitOfWork<Contact>();
            var contact = new Contact();
            unitOfWork.UnitOfWorkState = new UnitOfWorkState<Contact>()
            {
                SyncedEntities = new List<Contact> { contact }
            };

            // Act
            unitOfWork.RegisterNewEntity(contact);

            // Assert
            Assert.Single(unitOfWork.SyncedEntities, contact);
        }

        [Fact]
        public void RegisterRemovedEntityIfEntityWasSyncedBeforeTest()
        {
            // Arrange
            var unitOfWork = new UnitOfWork<Contact>();
            var contact = new Contact();
            unitOfWork.UnitOfWorkState = new UnitOfWorkState<Contact>()
            {
                SyncedEntities = new List<Contact> { contact }
            };

            // Act
            unitOfWork.RegisterRemovedEntity(contact);

            // Assert
            Assert.Contains<Contact>(contact, unitOfWork.RemovedEntities);
            Assert.DoesNotContain<Contact>(contact, unitOfWork.SyncedEntities);
        }

        [Fact]
        public void RegisterRemovedEntityIfEntityWasAddedOrChangedBeforeTest()
        {
            // Arrange
            var unitOfWork = new UnitOfWork<Contact>();
            var contact = new Contact();
            unitOfWork.UnitOfWorkState = new UnitOfWorkState<Contact>()
            {
                NewEntities = new List<Contact> { contact }
            };

            // Act
            unitOfWork.RegisterRemovedEntity(contact);

            // Assert
            Assert.DoesNotContain<Contact>(contact, unitOfWork.NewEntities);
            Assert.DoesNotContain<Contact>(contact, unitOfWork.RemovedEntities);
        }

        [Fact]
        public void RegisterUpdatedEntityIfEntityWasSyncedBeforeTest()
        {
            // Arrange
            var unitOfWork = new UnitOfWork<Contact>();
            var initialContact = new Contact();
            unitOfWork.UnitOfWorkState = new UnitOfWorkState<Contact>()
            {
                SyncedEntities = new List<Contact> { initialContact }
            };
            var updatedContact = new Contact(initialContact.Id,
                initialContact.UserId,
                initialContact.FirstName,
                initialContact.MiddleName,
                initialContact.LastName,
                initialContact.PhoneNumber,
                initialContact.Address,
                initialContact.Description)
            {
                Description = "updated contact"
            };

            // Act
            unitOfWork.RegisterUpdatedEntity(initialContact, updatedContact);

            // Assert
            Assert.DoesNotContain<Contact>(initialContact, unitOfWork.SyncedEntities);
            Assert.DoesNotContain<Contact>(updatedContact, unitOfWork.SyncedEntities);
            Assert.Contains<Contact>(initialContact, unitOfWork.RemovedEntities);
            Assert.Contains<Contact>(updatedContact, unitOfWork.NewEntities);
        }

        [Fact]
        public void RegisterUpdatedEntityIfEntityWasAddedOrChangedBeforeTest()
        {
            // Arrange
            var unitOfWork = new UnitOfWork<Contact>();
            var initialContact = new Contact();
            unitOfWork.UnitOfWorkState = new UnitOfWorkState<Contact>()
            {
                NewEntities = new List<Contact> { initialContact }
            };
            var updatedContact = new Contact(initialContact.Id,
                initialContact.UserId,
                initialContact.FirstName,
                initialContact.MiddleName,
                initialContact.LastName,
                initialContact.PhoneNumber,
                initialContact.Address,
                initialContact.Description)
            {
                Description = "updated contact"
            };


            // Act
            unitOfWork.RegisterUpdatedEntity(initialContact, updatedContact);

            // Assert
            Assert.DoesNotContain<Contact>(initialContact, unitOfWork.NewEntities);
            Assert.Contains<Contact>(updatedContact, unitOfWork.NewEntities);
            Assert.DoesNotContain<Contact>(initialContact, unitOfWork.RemovedEntities);
        }
    }
}
